using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextConverterLibrary.Service
{
    public static class UID
    {
        private static int _id;
        public static void StartWith(int startNumber) => _id = _id == 0 ? startNumber : _id;

        public static string Get()
        {
            return _id++.ToString();
        }
    }
}
