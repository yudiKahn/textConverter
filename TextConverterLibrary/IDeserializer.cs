using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextConverterLibrary
{
    public interface IDeserializer
    {
        IEnumerable<NodeAbstraction> Deserialize(string original);
    }
}
