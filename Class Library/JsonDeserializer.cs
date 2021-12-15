using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ClassLibrary.Service.Extensions;
using ClassLibrary.Service;

namespace ClassLibrary
{
    public class JsonDeserializer : IDeserializer
    {
        public IEnumerable<NodesTree> Deserialize(string original)
        {
            original = "\"__root\":"+Trim(original);
            List<NodesTree> resList = new List<NodesTree>();
            NodesRecursion(original, (arr, X, Pid) =>
            {
                foreach (var kv in arr)
                {
                    kv.X = X;
                    kv.PId = Pid;
                    if (((string)kv.Value)[0] == '{') kv.ValueType = NodeValueType.obj;
                    else if (((string)kv.Value)[0] == '[') kv.ValueType = NodeValueType.arr;
                    else if (((string)kv.Value)[0] == '"')
                    {
                        kv.ValueType = NodeValueType.str;
                        kv.Value = ((string)kv.Value).CleanQuotationMarks();
                    }
                    else if (char.IsDigit(((string)kv.Value)[0])) kv.ValueType = NodeValueType.num;
                    else if (((string)kv.Value)[0] == 't' || ((string)kv.Value)[0] == 'f') kv.ValueType = NodeValueType.bol;
                    resList.Add(kv);
                }
            });
            return resList.NodesSort();
        }

        public int FindTagCloserIndex(int index, string json)
        {
            if (json[index] == '"')
            {
                for (int i = index + 1; i < json.Length; i++)
                {
                    if (json[i] == '"') return i;
                }
            }
            else if (char.IsDigit(json[index]))
            {
                for (int i = index; i < json.Length; i++)
                {
                    if (char.IsDigit(json[i]) && !char.IsDigit(json[i + 1]) && json[i+1] != '.') return i;
                }
            }
            else if(json.TrySubstring(index, 4).IsBool() || json.TrySubstring(index, 5).IsBool())
            {
                for (int i = index; i < json.Length; i++)
                {
                    if (json[i].ToString().ToLower() == "e") return i;
                }
            }
            else
            {
                List<char> tags = new List<char>();

                if (!json[index].IsOpener(Service.Deserializers.Json)) throw new Exception();
                //else if(IsCloser(json[index]))
                tags.Add(json[index]);
                for (int i = index+1; i < json.Length; i++)
                {
                    if (json[i].IsOpener(Service.Deserializers.Json)) tags.Add(json[i]);
                    else if (json[i].IsCloser(Service.Deserializers.Json))
                    {
                        if (IsTagMatch(tags[tags.Count - 1], json[i]))
                            tags.RemoveAt(tags.Count - 1);
                        if (tags.Count == 0) return i;
                    }
                }
            }
            return -1;
        }

        public IEnumerable<NodesTree> GetTopNodes(string json)
        {
            List<NodesTree> res = new List<NodesTree>();
            for (int i = 0; i < json.Length; i++)
            {
                if (json[i] == ':') // there is a key and value
                {
                    int indexOfEndValue = FindTagCloserIndex((i + 1), json);
                    int startKey = i-2, endKey = i;
                    for (; json[startKey] != '"'; startKey--);

                    string key = json.Substring(startKey, endKey - startKey).CleanQuotationMarks();
                    string val = json.Substring(i + 1, indexOfEndValue - i);
                    res.Add(new NodesTree(key, val));
                    i = indexOfEndValue + 1;
                }
                else if (json[0] == '[' && i > 0)// there is no key (in array)
                {
                    int startVal = i, endVal = 0;
                    bool isPrimitive = json[i] == '"' || json[i] == 't' || json[i] == 'f' || char.IsDigit(json[i]);
                    List<char> tags = new() { json[i] };

                    for (int v = i + 1; true; v++)
                    {
                        if (isPrimitive && (json[v + 1] == ',' || json[v + 1] == ']'))
                        {
                            endVal = v;
                            break;
                        }
                        else if (!isPrimitive)
                        {
                            if (json[v].IsOpener(Deserializers.Json)) tags.Add(json[v]);
                            else if (json[v].IsCloser(Deserializers.Json) && IsTagMatch(tags[tags.Count - 1], json[v]))
                            {
                                tags.RemoveAt(tags.Count - 1);
                                if (tags.Count == 0)
                                {
                                    endVal = v + 1; break;
                                }

                            }
                        }
                    }

                    string key = "key";
                    string val = json.Substring(startVal, endVal - startVal);
                    res.Add(new NodesTree(key, val));
                    i = endVal + (isPrimitive ? 1 : 0);
                }
            }
            return res;
        }

        public void NodesRecursion(string original, Action<IEnumerable<NodesTree>, int, string> forEach, int X = 0, string PId = "0")
        {
            var tmp = GetTopNodes(original);
            forEach(tmp, X, PId);
            foreach (var pair in tmp)
            {
                if (((string)pair.Value)[0].IsOpener(Service.Deserializers.Json))
                {
                    NodesRecursion((string)pair.Value, forEach, X + 1, pair.Id);
                }
            }
        }

        public string Trim(string original)
        {
            var ignore = new Regex("( |[\r\n]+)");
            return ignore.Replace(original, string.Empty);
        }
        public static bool IsTagMatch(char open, char close) => (open == '[' && close == ']') || (open == '{' && close == '}');
    }
}
