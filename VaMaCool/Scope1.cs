using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VaMaCool
{
    class ScopeVal
    {
        public string Name { get; set; }

        public object Value { get; set; }

        public EnumArt x { get; set; }

    }

    enum EnumArt
    {
        Class,
        Method,
        Object
    }

    public class Scope1
    {
        // Scope-Art Class / Method 

        public string Name { get; set; }

        public List<Scope1> Children { get; set; } = new List<Scope1>();

        public Scope1 Parent { get; set; } = null;

        public List<Formal> IdValues { get; set; } = new List<Formal>();
        //public Dictionary<string, object> IdValues { get; set; } = new Dictionary<string, object>();

        public void Add(Formal id)
        {
            IdValues.Add(id);
        }

        public Formal Find(string Id)
        {
            if(IdValues.Any(IdV=>IdV.Id == Id))
            {
                return IdValues.FirstOrDefault(i => i.Id == Id);
            }
            else
            {
                if (Parent != null)
                {
                    return Parent.Find(Id);
                }
                else
                    return null;
            }
        }
    }
}
