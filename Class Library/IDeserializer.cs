using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public interface IDeserializer
    {
        string Trim(string original);
        int FindTagCloserIndex(int index, string original);
        NodesTree[] GetTopNodes(string original);
        void NodesRecursion(string original, Action<NodesTree[]> forEach, int X = 0, string PId = "0");
        NodesTree[] Deserialize(string original);
    }
}
