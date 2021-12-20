using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TextConvertorNuget.Interfaces;
using TextConvertorNuget.Service.Extensions;
using TextConvertorNuget.Service;

namespace TextConvertorNuget.Parallel
{
    public class XmlDeserializer : IXmlDeserializer
    {
        public IEnumerable<NodeAbstraction> Deserialize(string original)
        {
            original = Trim(original);
            List<NodeAbstraction> resList = new();
            List<string[]> stringsToWorkOn = new() { new string[] { "0", "0", original } };

            int iOfValue = 2, iOfX = 0, iOfPid = 1;

            for (int i = 0; i < stringsToWorkOn.Count; i++)
            {
                var kvs = GetTopNodes(stringsToWorkOn[i][iOfValue]);
                //if in first iteration check if there is more then one root element
                System.Threading.Tasks.Parallel.ForEach(kvs, kv =>
                {
                    kv.X = Convert.ToInt32(stringsToWorkOn[i][iOfX]);
                    kv.PId = stringsToWorkOn[i][iOfPid];
                    kv.ValueType = XmlHelper.GetValueType((string)kv.Value);
                    if (kv.ValueType == NodeValueType.obj || kv.ValueType == NodeValueType.arr)
                    {
                        stringsToWorkOn.Add(new string[] {
                            (Convert.ToInt32(stringsToWorkOn[i][iOfX])+1).ToString(),
                            kv.Id,
                            (string)kv.Value
                        });
                    }
                    resList.Add(kv);
                }); 
                stringsToWorkOn.RemoveAt(0);
                i--;
            };

            return resList.NodesSort();
        }

        public int FindTagCloserIndex(int index, string xml)
        {
            int res = -1;
            List<char> tags = new();
            for(int i = index; i < xml.Length;i++)//System.Threading.Tasks.Parallel.For(index, xml.Length, (i, state) =>
            {
                if (XmlHelper.IsOpenTagOpener(xml[i] + xml.ElementAtOrDefault(i + 1).ToString()))
                {
                    tags.Add(xml[i]);
                }
                else if (XmlHelper.IsCloseTagOpener(xml[i] + xml.ElementAtOrDefault(i + 1).ToString()))
                {
                    if (tags.Count == 0)
                        return i;
                    if (tags.Count > 0) tags.RemoveAt(tags.Count - 1);
                }
            };//);
            return res;
        }

        public IEnumerable<NodeAbstraction> GetTopNodes(string xml)
        {
            for (int i = 0; i < xml.Length; i++)
            {
                if (XmlHelper.IsTagCloser(xml[i]))
                {
                    int endKeyI = i, startKeyI = i - 1;
                    for (; !XmlHelper.IsOpenTagOpener(xml[startKeyI - 1] + xml[startKeyI].ToString()); startKeyI--) ;
                    int startValI = i + 1, endValI = FindTagCloserIndex(i, xml);

                    string key = xml.Substring(startKeyI, endKeyI - startKeyI);
                    string val = xml.Substring(startValI, endValI - startValI);

                    yield return new NodeAbstraction(key, val);

                    for (; !XmlHelper.IsTagCloser(xml[endValI]); endValI++) ;
                    i = endValI;
                }
            }
        }

        public string Trim(string input) => Regex.Replace(input, @"(>\s+|\s+<)", m => m.Value.Contains('<') ? "<" : ">");
    }
}
