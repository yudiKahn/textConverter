using System.Text;
using System.Xml;
using Newtonsoft.Json;
using System.Xml.Linq;
using PerformanceCompare;
using TextConverterLibrary;
using Generator;
using TextConverterLibrary.Parallel;

namespace Tester
{
    public static class Program
    {
        public static void Main()
        {
            string xmlStr = @"<person><name>dani</name><id>4</id></person>";
            string jsonStr = @"{""person"":{""name"":""dani"",""age"":1}}";
            Factory.Method = Converter.PARALLEL;
            string jsStr = new Converter().Convert(from:Format.XML, to:Format.JSON, xmlStr);
            Console.WriteLine(jsStr);
            /*Format to = Format.XML, from = Format.JSON;
            string str = Generate.Text(Generate.StructureFromType<Person>(), from);
            var converter = new Converter();
            var xml = converter.Convert(from: from, to: to, str);
            Console.WriteLine(xml);*/
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
                XNode node = JsonConvert.DeserializeXNode("");
                Console.WriteLine(node.ToString());
                return "newton json to xml";
            }, () =>
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml("");
                string json = JsonConvert.SerializeXmlNode(doc);
                Console.WriteLine(json);
                return "newton xml to json";
            }).Print(Console.WriteLine);
        }
    }
    public class Person
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsCool { get; set; }
    }
}