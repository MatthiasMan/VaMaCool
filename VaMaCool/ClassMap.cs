using System;
using System.Collections.Generic;
using System.Text;

namespace VaMaCool
{
    public class ClassMap
    {
        public ClassMap InheritsFrom { get; set; }

        public string Name { get; set; }

        public List<string> Methods { get; set; } = new List<string>();

        public List<Formal> Properties { get; set; } = new List<Formal>();
    }

    public class Formal
    {
        public string Id { get; set; }

        public string Type { get; set; }

        public CoolParser.ExpressionContext Expression { get; set; }
    }
}
