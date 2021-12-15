using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextConvertorNuget.Interfaces;
using TextConvertorNuget.Service.Extensions;

namespace TextConvertorNuget.Recursion
{
    public class XmlSerializer : IXmlSerializer
    {
        public string Serialize(IEnumerable<NodeAbstraction> nodes)
        {
            string res = "";
            List<NodeAbstraction> opens = new();
            int i = 0;
            int lastX = 0;
            foreach (var node in nodes)
            {
                int numsOfClosingTags = lastX - node.X;
                for (int j = 0; j < numsOfClosingTags; j++)
                {
                    res += $"</{opens[opens.Count - 1].Key}>";
                    opens.RemoveAt(opens.Count - 1);
                }
                res += $"<{node.Key}>";
                if (IsPrimitiveValue(node) || node.GetNumOfChildren(nodes) == 0)
                {
                    res += $"{((string)node.Value)}</{node.Key}>";
                }
                else
                    opens.Add(node);


                lastX = node.X;
                i++;
            }
            for (int i2 = opens.Count - 1; i2 >= 0; i2--)
            {
                res += $"</{opens[i2].Key}>";
            }

            return res;
        }
        public bool IsPrimitiveValue(NodeAbstraction node) => node.ValueType != NodeValueType.obj && node.ValueType != NodeValueType.arr;
    }
}
