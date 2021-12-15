using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TextConvertorNuget.Interfaces;

namespace TextConvertorNuget.Parallel
{
    public class JsonDeserializer : IJsonDeserializer
    {
        public IEnumerable<NodeAbstraction> Deserialize(string original)
        {
            original = "\"__root\":" + Trim(original);
            List<NodeAbstraction> resList = new();
            
            return resList;
        }

        private string Trim(string text)
        {
            var rgx = new Regex("/(\"[^\"]*\")|\\s/g");
            return rgx.Replace(text, string.Empty);
        }

    }
}
