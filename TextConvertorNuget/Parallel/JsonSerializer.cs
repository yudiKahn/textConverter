using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextConvertorNuget.Interfaces;

namespace TextConvertorNuget.Parallel
{
    public class JsonSerializer : IJsonSerializer
    {
        public string Serialize(IEnumerable<NodeAbstraction> nodes)
        {
            throw new NotImplementedException();
        }
    }
}
