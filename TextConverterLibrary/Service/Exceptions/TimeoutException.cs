using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextConverterLibrary.Service.Exceptions
{
    internal class TimeoutException : Exception
    {
        public TimeoutException() { }
        public TimeoutException(string msg) : base(msg) { }
        public TimeoutException(string msg, Exception inner) : base(msg, inner) { }
    }
}
