using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextConverterLibrary.Service.Exceptions
{
    internal class SyntaxException : Exception
    {
        public SyntaxException() { }
        public SyntaxException(string msg) : base(msg) { }
        public SyntaxException(string msg, Exception inner) : base(msg, inner) { }
    }
}
