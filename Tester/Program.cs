using System.Text;
using ClassLibrary;
using PerformanceCompare;
using Newtonsoft.Json;
using System.Xml;
using System.Xml.Linq;

namespace Tester
{
    public static class Program
    {
        public static void Main()
        { 
            var jsonStr = File.ReadAllText(path: @"D:\Projects\Text Convertor\Tester\jsonFile.json", encoding: Encoding.UTF8);
            var xmlStr = File.ReadAllText(path: @"D:\Projects\Text Convertor\Tester\xmlFile.xml", encoding: Encoding.UTF8);

            Performance.Compare(() =>
            {
                var parser = new Parser<JsonDeserializer, XmlSerializer>();
                var myxml = parser.Parse(jsonStr);
                Console.WriteLine(myxml);
            }, () =>
            {
                string newtonxml = JsonConvert.DeserializeXNode(jsonStr, "__root").ToString();
                Console.WriteLine(newtonxml);
            }, () => { }).Print(Console.WriteLine);


            /*
            var xml = new JsonDeserializer().Deserialize(jsonStr);//parser.Parse(jsonStr); 
            foreach (var i in xml)
            {
                var val = ToShow(i.ValueType) ? i.Value : "";
                Console.WriteLine($"{Space(i.X)}({i.Id},p:{i.PId}){i.Key}: ({i.ValueType}) {val}");
            }
            */
            //Console.WriteLine(xml);

        }
        public static string Space(int n)
        {
            string res = "";
            for (int i = 0; i < n; i++)
                res += " ";
            return res;
        }
        public static bool ToShow(NodeValueType nvt) => nvt == NodeValueType.str || nvt == NodeValueType.num || nvt == NodeValueType.bol;
    }
}