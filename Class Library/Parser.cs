using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class Parser<TFrom, TTo> where TFrom : IDeserializer, new() where TTo : ISerializer, new()
    {
        //public static string RootElement = "__root";
        private TFrom Deserializer = new TFrom();
        private TTo Serializer = new TTo();

        public string Parse(string original)
        {
            
            var nodesTree = Deserializer.Deserialize(original);
            return Serializer.Serialize(nodesTree.ToArray());
        }
    }
}
