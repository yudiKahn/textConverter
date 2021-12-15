using System.Text;
using TextConvertorNuget;
using PerformanceCompare;
using Newtonsoft.Json;
using System.Xml;
using System.Xml.Linq;
using ClassLibrary.Service.Extensions;
using ClassLibrary.Service;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System;
using Autofac;

namespace Tester
{
    public static class Program
    {
        public static void Main()
        {
            var converter = Factory.GetOfType<IConverter>();
            var res = converter[(Format.XML, Format.JSON)](TmpService.XmlStr);
            Console.WriteLine(res);
        }
    }


    public class TmpService
    {
        public static string JsonStr = File.ReadAllText(path: @"D:\Projects\Text Convertor\Tester\jsonFile.json", encoding: Encoding.UTF8);
        public static string XmlStr = File.ReadAllText(path: @"D:\Projects\Text Convertor\Tester\xmlFile.xml", encoding: Encoding.UTF8);

        //https://random-data-api.com/api/stripe/random_stripe?size=5
        public static void NodesTree()
        {
            /*var parser = new Parser<JsonDeserializer, XmlSerializer>();
            var res = parser.Parse(_jsonStr);
            Console.WriteLine(res);*/
        }

        public static void Newton()
        {
            var doc = JsonConvert.DeserializeXmlNode(JsonStr).ToString();
            Console.WriteLine(doc);
        }
    }
}