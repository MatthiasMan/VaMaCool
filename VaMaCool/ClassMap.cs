using System;
using System.Collections.Generic;
using System.Text;

namespace VaMaCool
{
    public class ClassMap
    {
        public ClassMap InheritsFrom { get; set; }

        public string Name { get; set; }

        public List<Method> Methods { get; set; } = new List<Method>();

        public List<Property> Properties { get; set; } = new List<Property>();
    }

    public class Property
    {
        public string Id { get; set; }

        public string Type { get; set; }

        public CoolParser.ExpressionContext Expression { get; set; }

        public object Value { get; set; }
    }

    public class Method
    {
        public string Id { get; set; }

        public string Type { get; set; }

        public List<CoolParser.FormalContext> Parameter { get; set; } = new List<CoolParser.FormalContext>();

        public CoolParser.ExpressionContext Expression { get; set; }
    }
}
