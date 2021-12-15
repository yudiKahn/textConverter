using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ClassLibrary.Service;
using ClassLibrary.Service.Extensions;

namespace ClassLibrary
{
    public class XmlDeserializer : IDeserializer
    {
        public IEnumerable<NodesTree> Deserialize(string original)
        {
            original = Trim(original);
            List<NodesTree> res = new();
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
                } else if (XmlHelper.IsCloseTagOpener(xml[i] + xml.ElementAtOrDefault(i+1).ToString()))
                {
                    if (tags.Count == 0) 
                        return i;
                    if (tags.Count > 0) tags.RemoveAt(tags.Count-1);
                }
            }
            return res;
        }

        public IEnumerable<NodesTree> GetTopNodes(string xml)
        {
            List<NodesTree> res = new();
            for(int i=0; i < xml.Length; i++)
            {
                char x = xml[i];
                if (XmlHelper.IsTagCloser(xml[i]))
                {
                    int endKeyI = i, startKeyI = i - 1;
                    for (; !XmlHelper.IsOpenTagOpener(xml[startKeyI-1] + xml[startKeyI].ToString()); startKeyI--);
                    int startValI = i + 1, endValI = FindTagCloserIndex(i, xml);

                    string key = xml.Substring(startKeyI, endKeyI - startKeyI);
                    string val = xml.Substring(startValI, endValI - startValI);

                    res.Add(new NodesTree(key, val));

                    for (; !XmlHelper.IsTagCloser(xml[endValI]); endValI++) ;
                    i = endValI;
                }
            }
            return res;
        }

        public void NodesRecursion(string original, Action<IEnumerable<NodesTree>, int, string> forEach, int X = 0, string PId = "0")
        {
            var tmp = GetTopNodes(original);
            forEach(tmp, X, PId);
            foreach (var kv in tmp)
            {
                string val = (string)kv.Value;  
                if(XmlHelper.IsOpenTagOpener(val[0] + val[1].ToString()))
                {
                    NodesRecursion(val, forEach, X+1, kv.Id);
                }
            }
        }

        public string Trim(string original) => new Regex("(\n|\t|\r)").Replace(original, "");
    }
}
