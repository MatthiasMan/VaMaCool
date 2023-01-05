using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VaMaCool
{
    class CoolMapVisitor1 : CoolBaseVisitor<object>
    {
        public override object VisitClassDefine([NotNull] CoolParser.ClassDefineContext context)
        {
            Manager.Classes.Add(new ClassMap() { Name = context.TYPE(0).GetText() });
            return base.VisitClassDefine(context);
        }

       /* public override object VisitMethod([NotNull] CoolParser.MethodContext context)
        {
            Manager.Classes.Last().Methods.Add(context.ID().GetText());
            return base.VisitMethod(context);
        }*/

        public override object VisitProperty([NotNull] CoolParser.PropertyContext context)
        {
            string x = (string)Visit(context.formal());

           // Manager.Classes.Last().Properties.Add((x, null, 0));
            return base.VisitProperty(context);
        }

        public override object VisitFormal([NotNull] CoolParser.FormalContext context)
        {
            string x = (string)Visit(context.ID());
            return x;
            //return base.VisitFormal(context);
        }

        public override object VisitId([NotNull] CoolParser.IdContext context)
        {
            return context.ID().GetText();

            //return base.VisitId(context);
        }

    }
}
