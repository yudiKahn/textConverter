using System.Text;
using TextConvertorNuget;
using System.Xml;
using Newtonsoft.Json;
using System.Xml.Linq;
using PerformanceCompare;

namespace Tester
{
    public static class Program
    {
        static string JsonStr = File.ReadAllText(path: @"D:\Projects\Text Convertor\ConsoleTester\jsonFile.json", encoding: Encoding.UTF8);
        static string XmlStr = File.ReadAllText(path: @"D:\Projects\Text Convertor\ConsoleTester\xmlFile.xml", encoding: Encoding.UTF8);
        public static void Main()
        {
            var converter = Factory.GetOfType<IConverter>();
            var json = converter[(Format.XML, Format.JSON)](XmlStr);
            Console.WriteLine(json);
        }

        public static void Compare()
        {
            Performance.Compare(() =>
            {
                Factory.method = Converter.PARALLEL;
                var converter = Factory.GetOfType<IConverter>();
                var res = converter[(Format.JSON, Format.XML)](JsonStr);
                return "Parallel json to xml";
            }, () =>
            {
                Factory.method = Converter.PARALLEL;
                var converter = Factory.GetOfType<IConverter>();
                var res = converter[(Format.JSON, Format.XML)](JsonStr);
                //Console.WriteLine(res);
                return "Recursion json to xml";
            }, () =>
            {
                XNode node = JsonConvert.DeserializeXNode(JsonStr);
                Console.WriteLine(node.ToString());
                return "newton json to xml";
            }, () =>
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(XmlStr);
                string json = JsonConvert.SerializeXmlNode(doc);
                Console.WriteLine(json);
                return "newton xml to json";
            }).Print(Console.WriteLine);
        }
    }
}