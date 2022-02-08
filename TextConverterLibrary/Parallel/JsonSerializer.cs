using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextConverterLibrary.Interfaces;

namespace TextConverterLibrary.Parallel
{
    public class JsonSerializer : IJsonSerializer
    {
        public string Serialize(IEnumerable<NodeAbstraction> nodes)
        {
            string res = "{";
            List<NodeAbstraction> opens = new();
            bool IsInArray = false;
            int i = 0;
            int lastX = 0;
            foreach (var node in nodes)
            {
                int numsOfClosingTags = lastX - node.X;
                for (int j = 0; j < numsOfClosingTags; j++)
                {
                    if (res[res.Length - 1] == ',')
                        res = res.Substring(0, res.Length - 1);
                    res += opens[opens.Count - 1].ValueType == NodeValueType.arr ? "]," : "},";
                    opens.RemoveAt(opens.Count - 1);
                }

                if (!IsInArray)
                {
                    res += $"\"{node.Key}\":";
                }

                if (node.ValueType == NodeValueType.str) res += $"\"{node.Value}\",";
                else if (node.ValueType == NodeValueType.num || node.ValueType == NodeValueType.bol) res += $"{node.Value},";
                else if (node.ValueType == NodeValueType.obj)
                {
                    res += '{';
                    opens.Add(node);
                }
                else if (node.ValueType == NodeValueType.arr)
                {
                    IsInArray = true;
                    res += '[';
                    opens.Add(node);
                }

                lastX = node.X;
                i++;
            }
            //?
            opens.Reverse();

            foreach (var node in opens)
            {
                if (res[res.Length - 1] == ',')
                    res = res.Substring(0, res.Length - 1);
                res += node.ValueType == NodeValueType.arr ? "]" : "}";
            }
            if (res[res.Length - 1] == ',')
                res = res.Substring(0, res.Length - 1);
            return res+'}';
        }
    }

}
