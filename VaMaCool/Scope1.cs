using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VaMaCool
{
    /*class ScopeVal
    {
        public string Name { get; set; }

        public object Value { get; set; }

        public EnumArt x { get; set; }

    }*/

    public enum EnumArt
    {
        Class,
        Method,
        Object
    }

    public class Scope1
    {
        // Scope-Art Class / Method 

        public EnumArt EnumArt { get; set; }

        public string Name { get; set; }

        public List<Scope1> Children { get; set; } = new List<Scope1>();

        public Scope1 Parent { get; set; } = null;

        public List<Property> IdValues { get; set; } = new List<Property>();
        //public Dictionary<string, object> IdValues { get; set; } = new Dictionary<string, object>();

        public void Add(Property id)
        {
            IdValues.Add(id);
        }

        public (Property,Scope1) Find(string Id)
        {
            if(IdValues.Any(IdV=>IdV.Id == Id))
            {
                return (IdValues.FirstOrDefault(i => i.Id == Id), this);
            }
            else
            {  //                   wenn ich eine klasse bin will ich nicht in den naechst aeußeren scope schauen
                if (Parent != null && this.EnumArt != EnumArt.Class)
                {
                    return Parent.Find(Id);
                }
                else
                    return (null, null);
            }
        }

        public Scope1 FindToBot(string name)
        {
            Scope1 res = null;

            if (this.Name == name)
                return this;
            else
            {
                this.Children.ForEach(c =>
                {
                    if (res != null)
                        return;

                    res = c.FindToBot(name);
                } );
            }

            return res;
        }
    }
}
