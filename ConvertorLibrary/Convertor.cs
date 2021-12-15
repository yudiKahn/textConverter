using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertorLibrary
{
    public class Convertor<T> : IConvertor where T : IConvertor, new()
    {
        public string Convert(string original)
        {
            T _convert = new T();
            return _convert.Convert(original);
        }
    }
}
