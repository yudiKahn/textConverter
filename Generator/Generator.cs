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

        public static IEnumerable<NodeAbstraction> AbstractSyntaxTree<T>() where T  : new()
        {
            Type t = typeof(T);
            NodeAbstraction parent = new(t.Name, "");
            parent.ValueType = NodeValueType.obj;
            parent.PId = "0";

            List<NodeAbstraction> res = new() { parent};
            List<(Type, NodeAbstraction) > types = new() { (t,parent) };

            for(int i=0; i<types.Count; i++)
            {
                foreach (var prop in types[i].Item1.GetProperties())
                {
                    if (prop.PropertyType.IsPrimitive || prop.PropertyType == typeof(string))
                    {
                        res.Add(new NodeAbstraction(prop.Name, GetRndValueFromType(prop.PropertyType))
                        {
                             PId = types[i].Item2.Id, ValueType = GetValueTypeFromType(prop.PropertyType)
                        });
                    } 
                }
            }
            return res;
        }
        private static NodeValueType GetValueTypeFromType(Type type)
        {
            if (type == typeof(bool)) return NodeValueType.bol;
            else if (type == typeof(string)) return NodeValueType.str;
            else if (type == typeof(int)) return NodeValueType.num;
            else if (type.IsArray) return NodeValueType.arr;
            else return NodeValueType.obj;
        }
        private static object GetRndValueFromType(Type type)
        {
            var rnd = new Random();
            if(type == typeof(bool))
                return new bool[] { true, false }[rnd.Next(0, 2)].ToString().ToLower();
            else if(type == typeof(int))
                return rnd.Next(0, int.MaxValue);
            else if(type == typeof(string))
            {
                int lngthOfTxt = rnd.Next(2, 10);
                string res = "";
                for (int i = 0; i < lngthOfTxt; i++)
                    res += (char)rnd.Next('a', 'z');
                return res;
            }
            else if(type == typeof(char))
                return (char)rnd.Next(char.MinValue, char.MaxValue);
            return "null";
        }
    }
}