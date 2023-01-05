using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VaMaCool
{
    class CoolClassVisitor1 : CoolBaseVisitor<bool>
    {
        public override bool VisitClassDefine([NotNull] CoolParser.ClassDefineContext context)
        {
            if (Manager.Classes.Any(c => c.Name == context.TYPE(0).GetText()))
                throw new Exception("[CoolClassVisitor1][VisitClassDefine]Class already Exists");

            Manager.Classes.Add(new ClassMap() { Name = context.TYPE(0).GetText() });
            return true;
        }
    }
}
