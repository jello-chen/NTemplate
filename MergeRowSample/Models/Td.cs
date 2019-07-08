using System.Collections.Generic;

namespace MergeRowSample
{
    public class Td
    {
        public string Text { get; set; }
        public int Rowspan { get; set; } = 1;
        public int Colspan { get; set; } = 1;
        public string Class { get; set; }
        public string Style { get; set; }
        public Dictionary<string, string> Attributes { get; set; }
    }
}
