using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace VaMaCool
{
    public class CoolVisitor2:CoolBaseVisitor<object>
    {
        public override object VisitIf([NotNull] CoolParser.IfContext context)
        {
           
            if ((bool)Visit(context.expression(0)))
            {

            }
            else
            {

            }
            return base.VisitIf(context);
        }

        public override object VisitExpression([NotNull] CoolParser.ExpressionContext context)
        {
            Console.WriteLine("--VisitExpr--");
            return base.VisitExpression(context);
        }
    }
}
