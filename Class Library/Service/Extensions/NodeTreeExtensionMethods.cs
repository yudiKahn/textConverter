using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Service.Extensions
{
    public static class NodeTreeExtensionMethods
    {

        public static IEnumerable<NodesTree> NodesSort(this IEnumerable<NodesTree> _this)
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
                for (int j = i; res[j].PId != res[j-1].PId && IsParentInTop(j); j--)
                {
                    var tmp = res[j];
                    res[j] = res[j - 1];
                    res[j - 1] = tmp;
                }
            }
            return res;
        }


        public static void Print(this IEnumerable<NodesTree> _this, Action<string> action)
        {
            Func<int, string> func = (n) =>
            {
                string res = "";
                for (int i = 0; i < n; res += " ", i++) ;
                return res;
            };
            foreach (var node in _this)
                action(func(node.X)+node.ToString());
        }

        public static int GetNumOfChildren(this NodesTree _this, IEnumerable<NodesTree> nodes)
        {
            int res = 0;
            foreach (var node in nodes)
            {
                if (node.PId == _this.Id) res++;
            }
            return res;
        }

        public static IEnumerable<NodesTree> GetChildren(this NodesTree _this, IEnumerable<NodesTree> fromNodes)
        {
            List<NodesTree> children = new();
            foreach (var n in fromNodes)
            {
                if(n.PId == _this.Id)
                    children.Add(n);
            }
            return children;
        }
    }
}
