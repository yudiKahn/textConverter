using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceCompare
{
    public class Info
    {
        public string Name { get; set; }
        public string Time { get; set; }
        public Info(string name, string Time)
        {
            this.Name = name; this.Time = Time;
        }
        public override string ToString() => $"{Name}\nTime: {Time}ms\n---------";
    }
}
