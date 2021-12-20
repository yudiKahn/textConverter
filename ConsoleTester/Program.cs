using System.Text;
using TextConvertorNuget;
using PerformanceCompare;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System;
using System.Xml;
using Newtonsoft.Json;
using System.Xml.Linq;

namespace Tester
{
    public static class Program
    {
        public static void Main()
        {

            string JsonStr = File.ReadAllText(path: @"D:\Projects\Text Convertor\ConsoleTester\jsonFile.json", encoding: Encoding.UTF8);
            string XmlStr = File.ReadAllText(path: @"D:\Projects\Text Convertor\ConsoleTester\xmlFile.xml", encoding: Encoding.UTF8);
            Performance.Compare(() =>
            {
                Factory.method = Converter.PARALLEL;
                var converter = Factory.GetOfType<IConverter>();
                var res = converter[(Format.JSON, Format.XML)](JsonStr);
                Console.WriteLine(res);
            }, () =>
            {
                Factory.method = Converter.RECURSION;
                var converter = Factory.GetOfType<IConverter>();
                var res = converter[(Format.JSON, Format.XML)](JsonStr);
                //Console.WriteLine(res);
            }, () =>
            {
                XNode node = JsonConvert.DeserializeXNode(JsonStr);
                //Console.WriteLine(node.ToString());
            }).Print(Console.WriteLine);
        }
    }
}