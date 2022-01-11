using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TextConverterLibrary.Interfaces;
using TextConverterLibrary.Service.Extensions;
using TextConverterLibrary.Service;

namespace TextConverterLibrary.Recursion
{
    public class XmlDeserializer : IXmlDeserializer
    {
        public IEnumerable<NodeAbstraction> Deserialize(string original)
        {
            original = Trim(original);
            List<NodeAbstraction> res = new();
            NodesRecursion(original, (arr, X, Pid) =>
            {
                foreach (var kv in arr)
                {
                    kv.X = X;
                    kv.PId = Pid;
                    kv.ValueType = XmlHelper.GetValueType((string)kv.Value);
                    res.Add(kv);
                }
            });
            return res.NodesSort();
        }

        public int FindTagCloserIndex(int index, string xml)
        {
            int res = -1;
            List<char> tags = new();
            for (int i = index; i < xml.Length; i++)
            {
                char c = xml[i];
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
            }
            return res;
        }

        public IEnumerable<NodeAbstraction> GetTopNodes(string xml)
        {
            List<NodeAbstraction> res = new();
            for (int i = 0; i < xml.Length; i++)
            {
                char x = xml[i];
                if (XmlHelper.IsTagCloser(xml[i]))
                {
                    int endKeyI = i, startKeyI = i - 1;
                    for (; !XmlHelper.IsOpenTagOpener(xml[startKeyI - 1] + xml[startKeyI].ToString()); startKeyI--) ;
                    int startValI = i + 1, endValI = FindTagCloserIndex(i, xml);

                    string key = xml.Substring(startKeyI, endKeyI - startKeyI);
                    string val = xml.Substring(startValI, endValI - startValI);

                    res.Add(new NodeAbstraction(key, val));

                    for (; !XmlHelper.IsTagCloser(xml[endValI]); endValI++) ;
                    i = endValI;
                }
            }
            return res;
        }

        public void NodesRecursion(string original, Action<IEnumerable<NodeAbstraction>, int, string> forEach, int X = 0, string PId = "0")
        {
            var tmp = GetTopNodes(original);
            forEach(tmp, X, PId);
            foreach (var kv in tmp)
            {
                string val = kv.Value.ToString();
                if (val.Length > 1 && XmlHelper.IsOpenTagOpener(val[0] + val[1].ToString()))
                {
                    NodesRecursion(val, forEach, X + 1, kv.Id);
                }
            }
        }

        public string Trim(string original) => new Regex("(\n|\t|\r)").Replace(original, "");
    }
}
