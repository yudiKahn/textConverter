using System.Text;
using System.Xml;
using Newtonsoft.Json;
using System.Xml.Linq;
using PerformanceCompare;
using TextConverterLibrary;

namespace Tester
{
    public static class Program
    {
        static string JsonStr = File.ReadAllText(path: @"D:\Projects\Text Convertor\ConsoleTester\jsonFile.json", encoding: Encoding.UTF8);
        static string XmlStr = File.ReadAllText(path: @"D:\Projects\Text Convertor\ConsoleTester\xmlFile.xml", encoding: Encoding.UTF8);
        public static void Main()
        {
            var converter = new Converter();
            var json = converter.Convert(Format.XML, Format.JSON, XmlStr);
            Console.WriteLine(json);
        }

        public static void Compare()
        {
            Performance.Compare(() =>
            {
                return "Parallel json to xml";
            }, () =>
            {
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