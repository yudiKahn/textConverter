using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary.Service.Extensions;

namespace ClassLibrary
{
    public class JsonSerializer : ISerializer
    {
        public string Serialize(NodesTree[] nodes)
        {
            string res = "";
            List<NodesTree> opens = new List<NodesTree>();
            for (int i = 0; i < nodes.Length; i++)
            {
                res += $"{nodes[i].Key}:";
                if (IsPrimitiveValue(nodes[i]) || nodes[i].GetNumOfChildren(nodes) == 0)
                {
                    res += $"{((string)nodes[i].Value)}</{nodes[i].Key}>";
                }
                else
                    opens.Add(nodes[i]);


                if (i < nodes.Length - 1 && nodes[i].PId != nodes[i + 1].PId && nodes[i + 1].PId != nodes[i].Id)
                {
                    res += $"</{opens[opens.Count - 1].Key}>";
                    opens.RemoveAt(opens.Count - 1);
                }

            }
            for (int i = opens.Count - 1; i >= 0; i--)
            {
                res += $"</{opens[i].Key}>";
            }

            return res;
        }
        public bool IsPrimitiveValue(NodesTree node) => node.ValueType != NodeValueType.obj && node.ValueType != NodeValueType.arr;

    }
}
