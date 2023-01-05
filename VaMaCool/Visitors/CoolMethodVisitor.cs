using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VaMaCool
{
    class CoolMethodVisitor : CoolBaseVisitor<object>
    {
        private ClassMap Current { get; set; }


        public override object VisitClassDefine([NotNull] CoolParser.ClassDefineContext context)
        {
            var curr = Manager.Classes.FirstOrDefault(c => c.Name == context.TYPE(0).GetText());

            if(curr == null)
            {
                throw new Exception();
            }

            Current = curr;

            foreach (var feat in context.feature())
            {
                Visit(feat);
            }

            return true;
        }

        public override object VisitMethod([NotNull] CoolParser.MethodContext context)
        {

            Method x = new Method();

            x.Id = context.ID().GetText();

            x.Expression = context.expression();
            x.Parameter.AddRange(context.formal());
            
            Current.Methods.Add(x);

            return base.VisitMethod(context);
        }

        public override object VisitExpression([NotNull] CoolParser.ExpressionContext context)
        {
            return base.VisitExpression(context);
        }

        public override object VisitId([NotNull] CoolParser.IdContext context)
        {
            return context.ID().GetText();
        }
    }
}
