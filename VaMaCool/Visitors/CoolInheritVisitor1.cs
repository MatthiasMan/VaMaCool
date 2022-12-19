using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VaMaCool
{
    class CoolInheritVisitor1 : CoolBaseVisitor<bool>
    {
        public override bool VisitClassDefine([NotNull] CoolParser.ClassDefineContext context)
        {
            if (context.TYPE(1) != null && context.TYPE(1).GetText() != "")
            {
                var classs =Manager.Classes.FirstOrDefault(c => c.Name == context.TYPE(0).GetText());
                var superClasss = Manager.Classes.FirstOrDefault(c => c.Name == context.TYPE(1).GetText());

                if (classs == null || superClasss == null)
                    throw new Exception();

                classs.InheritsFrom = superClasss;         
            
            }

            return true;
        }
    }
}
