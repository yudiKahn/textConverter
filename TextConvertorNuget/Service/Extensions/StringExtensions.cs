using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextConvertorNuget.Service.Extensions
{
    public static class StringExtensions
    {
        public static string CleanQuotationMarks(this string _this)
        {
            string str = _this;
            if (str[0] == '\"') str = str.Substring(1);
            if (str[str.Length - 1] == '\"') str = str.Substring(0, str.Length - 1);
            return str;
        }

        public static bool IsOpener(this string _this, Format format)
        {
            try {
                if (format == Format.JSON && (_this[0] == '{' || _this[0] == '[')) return true;
                if (format == Format.XML && (_this[0] == '<' || _this[1] != '/')) return true;
                return false;
            } catch { return false; }
        }
        public static bool IsCloser(this string _this, Format format)
        {
            try
            {
                if (format == Format.JSON && (_this == "}" || _this == "]")) return true;
                if (format == Format.XML && (_this == ">")) return true;
                return false;
            }
            catch { return false; }
        }

        public static bool IsBool(this string _this) => _this.ToLower()=="true" || _this.ToLower()=="false";
    }
}
