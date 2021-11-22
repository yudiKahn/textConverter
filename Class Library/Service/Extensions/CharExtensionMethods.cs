using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Service.Extensions
{
    public static class CharExtensionMethods
    {
        public static bool IsOpener(this char _this, Deserializers deserializer)
        {
            switch (deserializer)
            {
                case Deserializers.Json:
                    return _this == '[' || _this == '{';
                case Deserializers.Xml:
                    return _this == '<';
                default:
                    return false;
            }
        }
        public static bool IsCloser(this char _this, Deserializers deserializer)
        {
            switch (deserializer)
            {
                case Deserializers.Json:
                    return _this == ']' || _this == '}';
                case Deserializers.Xml:
                    return _this == '>';
                default:
                    return false;
            }
        }
    }
}
