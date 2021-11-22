using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Service.Extensions
{
    public static class StringExtensionsMethods
    {
        public static bool IsBool(this string _this)
        {
            return _this.ToLower() == "true" || _this.ToLower() == "false";
        }

        public static string TrySubstring(this string _this, int start)
        {
            string res = "";
            try
            {
                res = _this.Substring(start, _this.Length);
                return res;
            }
            catch
            {
                return res;
            }
        }
        public static string TrySubstring(this string _this, int start, int length)
        {
            string res = "";
            try
            {
                res = _this.Substring(start, length);
                return res;
            }
            catch
            {
                return res;
            }
        }

        public static string CleanQuotationMarks(this string str)
        {
            if (str[0] == '\"') str = str.Substring(1);
            if (str[str.Length - 1] == '\"') str = str.Substring(0, str.Length - 1);
            return str;
        }

        public static bool GraterThen(this string _this, string other) =>  Convert.ToInt32(_this) > Convert.ToInt32(other);

        public static bool SmallerThen(this string _this, string other) => Convert.ToInt32(_this) < Convert.ToInt32(other);

    }
}
