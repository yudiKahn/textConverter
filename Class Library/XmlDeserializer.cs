using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ClassLibrary.Service.Extensions;

namespace ClassLibrary
{
    public class XmlDeserializer : IDeserializer
    {
        public NodesTree[] Deserialize(string original)
        {
            original = Trim(original);
            List<NodesTree> resList = new List<NodesTree>();
            NodesRecursion(original, (arr) =>
            {
                foreach (var kv in arr)
                    resList.Add(kv);
            });
            return resList.ToArray().Sort();
        }

        public int FindTagCloserIndex(int index, string xml)
        {
            List<char> tags = new List<char>();
            for (int i = index+1; i < xml.Length; i++)
            {
                if (xml[i].IsOpener(Service.Deserializers.Xml) && xml[i + 1] != '/')
                    tags.Add(xml[i]);
                else if(xml[i].IsOpener(Service.Deserializers.Xml) && xml[i+1] == '/')
                {
                    if(tags.Count > 0)
                        tags.RemoveAt(tags.Count - 1);
                    else
                    {
                        for (int i2 = i; i2 < xml.Length; i2++)
                        {
                            if (xml[i2].IsCloser(Service.Deserializers.Xml))
                                return i2;
                        }
                    }

                }
            }
            return -1;
        }

        public NodesTree[] GetTopNodes(string xml)
        {
            List<NodesTree> res = new List<NodesTree>();
            for (int i = 0; i < xml.Length; i++)
            {
                if (xml[i].IsOpener(Service.Deserializers.Xml) && xml[i+1] != '/') // we are at key
                {
                    int indexOfEndValue = FindTagCloserIndex(i, xml);
                    int startKey = i, endKey = 0;
                    for (int k = i; true; k++)
                    {
                        if (xml[k] == '>')
                        {
                            endKey = k;
                            break;
                        }
                    }

                    string key = xml.Substring(startKey, endKey - startKey+1);
                    key = new Regex("(<|>)").Replace(key, string.Empty);
                    string val = xml.Substring(endKey+1, indexOfEndValue - endKey - key.Length);
                    val = val[0] != '<' ? new Regex("(</|(?<=</).*)").Replace(val,string.Empty) : val;
                    res.Add(new NodesTree(key, val));
                    i = indexOfEndValue + 1;
                }
            }
            return res.ToArray();
        }

        public void NodesRecursion(string original, Action<NodesTree[]> forEach, int X = 0, string PId = "0")
        {
            NodesTree[] tmp = GetTopNodes(original);
            foreach (var kv in tmp)
            {
                kv.X = X;
                kv.PId = PId;
            }
            forEach(tmp);
            foreach (var pair in tmp)
            {
                if (((string)pair.Value)[0].IsOpener(Service.Deserializers.Xml))
                {
                    NodesRecursion((string)pair.Value, forEach, X + 1, pair.Id);
                }
            }
        }

        public string Trim(string original)
        {
            return new Regex("(\n|\t|\r)").Replace(original, string.Empty);
        }
    }
}
