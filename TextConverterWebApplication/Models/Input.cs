using TextConverterLibrary;

namespace TextConverterWebApplication.Models
{
    public class Input
    {
        public string Text { get; set; }
        public Format InputFormat { get; set; }
        public Format OutputFormat { get; set; }
        public bool IsAuto { get; set; }
    }
}
