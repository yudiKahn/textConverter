using System.Reflection;
using TextConverterLibrary;
using TextConverterLibrary.Parallel;
using TextConverterLibrary.Service;

namespace Generator
{
    public class Generate
    {
        private static Random Random;
        static Generate()
        {
            Random = new Random();
        }
        public static string Text(IEnumerable<NodeAbstraction> structure, Format format)
        {
            ISerializer serializer = format == Format.XML ? new XmlSerializer() : new JsonSerializer();
            return serializer.Serialize(structure);
        }

        public static IEnumerable<NodeAbstraction> AbstractSyntaxTree<T>(int n = 1) where T : new()
        {
            List<NodeAbstraction> res = new();
            var topId = string.Empty;

            for (int time = 0; time < n; time++)
            {
                Type t = typeof(T);
                if (time == 0)
                {
                    var top = new NodeAbstraction(t.Name + "s", "")
                    {
                        PId = "0",
                        ValueType = NodeValueType.obj,
                        X = 0
                    };
                    topId = top.Id;
                    res.Add(top);
                }
                NodeAbstraction parent = new(t.Name+(time + 1), "");
                parent.ValueType = NodeValueType.obj;
                parent.PId = topId;
                parent.X = 1;

                res.Add(parent);
                List<(Type, NodeAbstraction)> types = new() { (t, parent) };

                for (int i = 0; i < types.Count; i++)
                {
                    var currType = GetValueTypeFromType(types[i].Item1);
                    if (currType == NodeValueType.obj)
                    {
                        foreach (var prop in types[i].Item1.GetProperties())
                        {
                            var ast = new NodeAbstraction(prop.Name, GetRndValueFromType(prop.PropertyType))
                            {
                                PId = types[i].Item2.Id,
                                ValueType = GetValueTypeFromType(prop.PropertyType),
                                X = 2
                            };
                            res.Add(ast);   

                            if (!prop.PropertyType.IsPrimitive && prop.PropertyType != typeof(string))
                                types.Add((prop.PropertyType, ast));
                        }
                    }
                    else if (currType == NodeValueType.arr)
                    {
                        var arrOfType = types[i].Item1.GetElementType();
                        var l = Random.Next(1, 16);
                        var pid = types[i].Item2.Id;
                        for (int j = 0; j < l; j++)
                        {
                            var ast = new NodeAbstraction("key", GetRndValueFromType(arrOfType))
                            {
                                PId = pid,
                                ValueType = GetValueTypeFromType(arrOfType),
                                X = 2
                            };
                            res.Add(ast);
                        }
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
            else if (type == typeof(char)) return NodeValueType.str;
            else if (type.IsArray) return NodeValueType.arr;
            else return NodeValueType.obj;
        }
        private static object GetRndValueFromType(Type type)
        {
            if(type == typeof(bool))
                return new bool[] { true, false }[Random.Next(0, 2)].ToString().ToLower();
            else if(type == typeof(int))
                return Random.Next(0, 20);
            else if(type == typeof(string))
            {
                int lngthOfTxt = Random.Next(2, 10);
                string res = "";
                for (int i = 0; i < lngthOfTxt; i++)
                    res += (char)Random.Next('a', 'z');
                return res;
            }
            else if(type == typeof(char))
                return (char)Random.Next('a', 'z');
            return "null";
        }
    }
}