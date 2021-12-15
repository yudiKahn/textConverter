﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextConvertorNuget.Service
{
    public static class XmlHelper
    {
        public static bool IsTagCloser(char c) => c == '>';
        public static bool IsOpenTagOpener(string str) => str[0] == '<' && str[1] != '/';
        public static bool IsCloseTagOpener(string str) => str[0] == '<' && str[1] == '/';

        public static NodeValueType GetValueType(string val)
        {
            val = val.ToLower();
            if (val == "true" || val == "false") return NodeValueType.bol;
            else if (val[0] == '<' && val[val.Length - 1] == '>') return NodeValueType.obj;
            else if (char.IsDigit(val[0])) return NodeValueType.num;
            else return NodeValueType.str;
        }

    }
}
