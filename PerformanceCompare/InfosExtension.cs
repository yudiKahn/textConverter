using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceCompare
{
    public static class InfosExtension
    {
        public static void Print(this IEnumerable<Info> _this, Action<string> action)
        {
            foreach(var info in _this)
            {
                action(info.ToString());
            }
        }
    }
}
