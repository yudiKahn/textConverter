using System.Text.RegularExpressions;
using TextConverterLibrary.Interfaces;
using TextConverterLibrary.Service;
using TextConverterLibrary.Service.Extensions;
using TextConverterLibrary.Service.Exceptions;
using TaskParallel = System.Threading.Tasks.Parallel;

namespace TextConverterLibrary.Parallel
{
    public class XmlDeserializer : IXmlDeserializer
    {
        public IEnumerable<NodeAbstraction> Deserialize(string original)
        {
            original = Trim(original);
            List<NodeAbstraction> resList = new();
            List<string[]> stringsToWorkOn = new() { new string[] { "0", "0", original } };

            byte iOfValue = 2, iOfX = 0, iOfPid = 1;
            bool isFirstLoop = true;
            for (int i = 0; i < stringsToWorkOn.Count; i++)
            {
                var kvs = GetTopNodes(stringsToWorkOn[i][iOfValue]);
                if (isFirstLoop)
                {
                    if (kvs.Count() > 1) throw new SyntaxException("More then one root element");
                    isFirstLoop = false;
                }
                TaskParallel.ForEach(kvs, kv =>
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

        public static int FindTagCloserIndex(int index, string xml)
        {
            List<char> tags = new();
            for(int i = index; i < xml.Length;i++)
            {
                if (XmlHelper.IsOpenTagOpener(xml[i] + xml[i+1].ToString()))
                {
                    tags.Add(xml[i]);
                }
                else if (XmlHelper.IsCloseTagOpener(xml[i] + xml.ElementAtOrDefault(i + 1).ToString()))
                {
                    if (tags.Count == 0)
                        return i;
                    if (tags.Count > 0) tags.RemoveAt(tags.Count - 1);
                }
            };
            throw new SyntaxException("No closer tag found");
        }

        public IEnumerable<NodeAbstraction> GetTopNodes(string xml)
        {
            for (int i = 0; i < xml.Length; i++)
            {
                if (XmlHelper.IsOpenTagOpener(xml[i..(i + 2)]))
                {
                    int endKeyI = i+2, startKeyI = i+1;
                    for (; !XmlHelper.IsTagCloser(xml[endKeyI]); endKeyI++)
                        if (!XmlHelper.IsValidTagCharacter(xml[endKeyI])) throw new SyntaxException();
                    
                    int startValI = endKeyI+1, endValI = FindTagCloserIndex(endKeyI+1, xml);

                    string key = xml[startKeyI..endKeyI];
                    string val = xml[startValI..endValI];

                    yield return new NodeAbstraction(key, val);

                    for (int j = endValI; true; j++)
                    {
                        if (XmlHelper.IsTagCloser(xml[j]))
                        {
                            string nameOfClosingTag = xml[(endValI + 2)..j];
                            if (nameOfClosingTag != key) throw new SyntaxException($"The closing tag doesn't match the opening tag ({key} and {nameOfClosingTag})");
                            i = j;
                            break;
                        }
                    }
                }
            }
        }
        public static string Trim(string input) => Regex.Replace(input, @"(>\s+|\s+<)", m => m.Value.Contains('<') ? "<" : ">");
    }
}