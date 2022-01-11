using System.Reflection;
using TextConverterLibrary;
using TextConverterLibrary.Parallel;

namespace Generator
{
    public class Generate
    {
        public static string Text(IEnumerable<NodeAbstraction> structure, Format format)
        {
            ISerializer serializer = format == Format.XML ? new XmlSerializer() : new JsonSerializer();
            return serializer.Serialize(structure);
        }

        public static IEnumerable<NodeAbstraction> StructureFromType<T>() where T  : new()
        {
            Type t = typeof(T);
            NodeAbstraction parent = new(t.Name, "");
            parent.ValueType = NodeValueType.obj;
            parent.PId = "0";

            List<NodeAbstraction> res = new() { parent};
            foreach (var prop in t.GetProperties())
            {
                NodeAbstraction tmp = new(prop.Name,"");
                tmp.ValueType = getValueTypeFromString(prop.PropertyType.Name);
                tmp.Value = getRndValueFromType(tmp.ValueType);
                tmp.PId = parent.Id;
                res.Add(tmp);
            }
            return res;
        }

        private static NodeValueType getValueTypeFromString(string str)
        {
            switch (str.ToLower())
            {
                case "boolean": return NodeValueType.bol;
                case "int32":return NodeValueType.num;
                case "string": default: return NodeValueType.str;
            }
        }
        private static object getRndValueFromType(NodeValueType type)
        {
            var rnd = new Random();
            switch (type)
            {
                case NodeValueType.bol: return new bool[]{true,false }[rnd.Next(0,2)].ToString().ToLower();
                case NodeValueType.str:
                    int lngthOfTxt = rnd.Next(2, 10);
                    string res = "";
                    for (int i = 0; i < lngthOfTxt; i++)
                        res += (char)rnd.Next('a','z');
                    return res;
                case NodeValueType.num: return rnd.Next(0, 10);
                default: return "default";
            }
        }
    }
}