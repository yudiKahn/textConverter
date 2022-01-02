using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextConverterLibrary
{
    public class NodeAbstraction
    {
        public int X { get; set; }
        public string Id { get; }
        public string PId { get; set; }
        public string Key { get; set; }
        public object Value { get; set; }
        public NodeValueType ValueType { get; set; }
        public NodeAbstraction(string key, object val)
        {
            Key = key;
            Value = val;
            X = 0;

            Service.UID.StartWith(1);
            Id = Service.UID.Get();
        }
        public override string ToString() => $"({Id},p:{PId}) {Key}: " +
            (ValueType == NodeValueType.arr || ValueType == NodeValueType.obj ? $"({ValueType})" : $"{Value}");
    }
}
