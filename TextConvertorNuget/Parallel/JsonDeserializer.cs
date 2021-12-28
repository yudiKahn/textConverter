using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TextConvertorNuget.Interfaces;
using TextConvertorNuget.Service.Exceptions;
using TextConvertorNuget.Service.Extensions;

namespace TextConvertorNuget.Parallel
{
    public class JsonDeserializer : IJsonDeserializer
    {
        public IEnumerable<NodeAbstraction> Deserialize(string original)
        {
            original = "\"__root\":" + Trim(original);
            List<NodeAbstraction> resList = new();
            List<string[]> stringsToWorkOn = new() { new string[] { "0","0", original } };

            byte iOfValue = 2, iOfX = 0, iOfPid = 1;

            bool isFirstLoop = true;
            for(int i = 0; i < stringsToWorkOn.Count; i++)
            {
                var kvs = GetTopNodes(stringsToWorkOn[i][iOfValue]);
                if (isFirstLoop)
                {
                    if (kvs.Count() > 1) throw new SyntaxException("More then one root element");
                    isFirstLoop = false;
                }
                System.Threading.Tasks.Parallel.ForEach(kvs, kv =>
                {
                    kv.X = Convert.ToInt32(stringsToWorkOn[i][iOfX]);
                    kv.PId = stringsToWorkOn[i][iOfPid];
                    if (((string)kv.Value)[0] == '{' || ((string)kv.Value)[0] == '[')
                    {
                        stringsToWorkOn.Add(new string[] { 
                            (Convert.ToInt32(stringsToWorkOn[i][iOfX])+1).ToString(),
                            kv.Id,
                            (string)kv.Value 
                        });
                        if (((string)kv.Value)[0] == '{') kv.ValueType = NodeValueType.obj;
                        else if (((string)kv.Value)[0] == '[') kv.ValueType = NodeValueType.arr;
                    }
                    else if (((string)kv.Value)[0] == '"')
                    {
                        kv.ValueType = NodeValueType.str;
                        kv.Value = ((string)kv.Value).CleanQuotationMarks();
                    }
                    else if (char.IsDigit(((string)kv.Value)[0])) kv.ValueType = NodeValueType.num;
                    else if (((string)kv.Value).IsBool()) kv.ValueType = NodeValueType.bol;
                    resList.Add(kv);
                });

                stringsToWorkOn.RemoveAt(0);
                i--;
            };

            return resList.NodesSort();
        }

        public static int FindTagCloserIndex(int index, string json)
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
                    if (char.IsDigit(json[i]) && !char.IsDigit(json[i + 1]) && json[i + 1] != '.') return i;
                }
            }
            else if (json.Substring(index, 4).IsBool() || json.Substring(index, 5).IsBool())
            {
                for (int i = index; i < json.Length; i++)
                {
                    if (json[i].ToString().ToLower() == "e") return i;
                }
            }
            else
            {
                List<char> tags = new();

                if (!json[index].ToString().IsOpener(Format.JSON)) throw new Exception();
                tags.Add(json[index]);
                for (int i = index + 1; i < json.Length; i++)
                {
                    if (json[i].ToString().IsOpener(Format.JSON)) tags.Add(json[i]);
                    else if (json[i].ToString().IsCloser(Format.JSON))
                    {
                        if (IsTagMatch(tags[tags.Count - 1], json[i]))
                            tags.RemoveAt(tags.Count - 1);
                        if (tags.Count == 0) return i;
                    }
                }
            }
            throw new SyntaxException("No closer tag found");
        }

        public IEnumerable<NodeAbstraction> GetTopNodes(string json)
        {
            List<NodeAbstraction> res = new();
            for (int i = 0; i < json.Length; i++)
            {
                if (json[i] == ':') // there is a key and value
                {
                    int indexOfEndValue = FindTagCloserIndex((i + 1), json);
                    int startKey = i - 2, endKey = i;
                    for (; json[startKey] != '"'; startKey--) ;

                    string key = json.Substring(startKey, endKey - startKey).CleanQuotationMarks();
                    string val = json.Substring(i + 1, indexOfEndValue - i);
                    res.Add(new NodeAbstraction(key, val));
                    i = indexOfEndValue + 1;
                }
                else if (json[0] == '[' && i > 0)// there is no key (in array)
                {
                    int startVal = i, endVal;
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
                            if (json[v].ToString().IsOpener(Format.JSON)) tags.Add(json[v]);
                            else if (json[v].ToString().IsCloser(Format.JSON) && IsTagMatch(tags[tags.Count - 1], json[v]))
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
                    res.Add(new NodeAbstraction(key, val));
                    i = endVal + (isPrimitive ? 1 : 0);
                }
            }
            return res;
        }
 
        public static string Trim(string input) =>
            Regex.Replace(input, @"\s(?=(?:""[^""]*""|[^""])*$)", string.Empty);
        public static bool IsTagMatch(char open, char close) => (open == '[' && close == ']') || (open == '{' && close == '}');
    }
}