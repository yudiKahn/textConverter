using System.Text;
using TextConvertorNuget;
using PerformanceCompare;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System;

namespace Tester
{
    public static class Program
    {
        public static void Main()
        {
            string JsonStr = File.ReadAllText(path: @"D:\Projects\Text Convertor\ConsoleTester\jsonFile.json", encoding: Encoding.UTF8);
            string XmlStr = File.ReadAllText(path:  @"D:\Projects\Text Convertor\ConsoleTester\xmlFile.xml", encoding: Encoding.UTF8);
            
            var converter = Factory.GetOfType<IConverter>();
            var res = converter[(Format.XML, Format.JSON)](XmlStr);
            Console.WriteLine(res);
        }
    }
}