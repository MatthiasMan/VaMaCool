using System;
using System.Collections.Generic;
using System.Text;

namespace VaMaCool
{
    public static class Manager
    {
        public static List<ClassMap> Classes { get; set; } = new List<ClassMap>();
        public static Scope1 ObjectScope { get; set; } = null;
        public static Scope1 CurrentScope { get; set; } = null;
    }
}
