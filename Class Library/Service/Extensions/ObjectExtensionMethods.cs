using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Service.Extensions
{
    public static class ObjectExtensionMethods
    {
        /// <summary>This method check if the object correspends an opener tag.</summary>

        /// <param name="deserializer"> Parameter description for s goes here</param>
        public static bool IsOpener(this object _this, Deserializers deserializer)
        {
            try
            {
                switch (deserializer)
                {
                    case Deserializers.Json:
                        return (char)_this == '[' || (char)_this == '{';
                    case Deserializers.Xml:
                        return ((string)_this)[0] == '<' && ((string)_this)[1] != '/';
                    default:
                        return false;
                }
            }
            catch
            {
                throw new Exception("");
            }
        }
        public static bool IsCloser(this object _this, Deserializers deserializer)
        {
            try
            {
                switch (deserializer)
                {
                    case Deserializers.Json:
                        return (char)_this == ']' || (char)_this == '}';
                    case Deserializers.Xml:
                        return (char)_this == '>';
                    default:
                        return false;
                }
            }
            catch
            {
                throw new Exception("");
            }
        }
    }
}
