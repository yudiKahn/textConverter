using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextConvertorNuget.Service.Extensions
{
    public static class NodeAbstractionExtensions
    {
        public static int GetNumOfChildren(this NodeAbstraction _this, IEnumerable<NodeAbstraction> nodes)
        {
            int res = 0;
            foreach (var node in nodes)
            {
                if (node.PId == _this.Id) res++;
            }
            return res;
        }

        public static IEnumerable<NodeAbstraction> NodesSort(this IEnumerable<NodeAbstraction> _this)
        {
            var res = _this.ToList();
            Predicate<int> IsParentInTop = (index) =>
            {
                bool flag = false;
                for (int i = 0; i < res.Count; i++)
                {
                    if (res[i].Id == res[index].PId && i < index - 1)
                    {
                        flag = true;
                        break;
                    }
                }
                return flag;
            };
            for (int i = 1; i < res.Count; i++)
            {
                for (int j = i; res[j].PId != res[j - 1].PId && IsParentInTop(j); j--)
                {
                    var tmp = res[j];
                    res[j] = res[j - 1];
                    res[j - 1] = tmp;
                }
            }
            return res;
        }
    }
}
