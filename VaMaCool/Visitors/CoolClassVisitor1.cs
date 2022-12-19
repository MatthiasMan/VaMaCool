using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Text;

namespace VaMaCool
{
    class CoolClassVisitor1 : CoolBaseVisitor<bool>
    {
        public override bool VisitClassDefine([NotNull] CoolParser.ClassDefineContext context)
        {
            Manager.Classes.Add(new ClassMap() { Name = context.TYPE(0).GetText() });
            return true;
        }
    }
}
