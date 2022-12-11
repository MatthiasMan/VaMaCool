using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Text;

namespace VaMaCool
{
    public class CoolVisitor1 : CoolBaseVisitor<int>
    {
        public override int VisitClassDefine([NotNull] CoolParser.ClassDefineContext context)
        {
            Console.WriteLine("--VisitClassDefine--");
            return base.VisitClassDefine(context);
        }

        public override int VisitMethod([NotNull] CoolParser.MethodContext context)
        {
            Console.WriteLine("--VisitMethod--");
            return base.VisitMethod(context);
        }

        public override int VisitExpression([NotNull] CoolParser.ExpressionContext context)
        {
            Console.WriteLine("--VisitExpr--");
            return base.VisitExpression(context);
        }

        public override int VisitId([NotNull] CoolParser.IdContext context)
        {
            Console.WriteLine("--VisitID--");
            return base.VisitId(context);
        }

        public override int VisitInt([NotNull] CoolParser.IntContext context)
        {
            Console.WriteLine("--VisitInt--");
            int.TryParse(context.INT().GetText(), out int value);
            return value;
        }

        public override int VisitParentheses([NotNull] CoolParser.ParenthesesContext context)
        {
            Console.WriteLine("--VisitParentheses--");
            return base.VisitParentheses(context);
        }

        public override int VisitArithmetic([NotNull] CoolParser.ArithmeticContext context)
        {
            Console.WriteLine("--VisitArithmetic--");
            int value = 0;

            int left = Visit(context.expression(0));
            int right = Visit(context.expression(1));
            switch (context.op.Text)
            {
                case "+":
                    value = left + right;
                    break;
                case "-":
                    value = left - right;
                    break;
                case "*":
                    value = left * right;
                    break;
                case "/":
                    value = left / right;
                    break;
                default:
                    throw new Exception("Arithmetic operator not found");
            }
            return value;
        }

        public override int VisitAssignment([NotNull] CoolParser.AssignmentContext context)
        {
            Console.WriteLine("--VisitAssignment--");
            return base.VisitAssignment(context);
        }
    }
}
