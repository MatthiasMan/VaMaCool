using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using Antlr4.Runtime.Misc;
using System.Linq;
using System.Text;
using Antlr4.Runtime.Tree;

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

        public override object VisitExpression([Antlr4.Runtime.Misc.NotNull] CoolParser.ExpressionContext context)
        {
            Console.WriteLine("--VisitExpr--");
            return base.VisitExpression(context);
        }

        public override object VisitNew([NotNull] CoolParser.NewContext context)
        {
            Scope1 newBlock = new Scope1()
            {
                Name = context.TYPE().GetText(),
                Children = new List<Scope1>(),
                Parent = Manager.CurrentScope
            };

            var mainCl = Manager.Classes.FirstOrDefault(c => c.Name == context.TYPE().GetText());

            foreach (var prop in mainCl.Properties)
            {
                newBlock.IdValues.Add(prop);
            }

            Manager.CurrentScope.Children.Add(newBlock);

            return base.VisitNew(context);
        }



        public override object VisitDispatchExplicit([Antlr4.Runtime.Misc.NotNull] CoolParser.DispatchExplicitContext context)
        {
            var variabl =Manager.CurrentScope.IdValues.FirstOrDefault(i => i.Id == context.expression(0).GetText());
            if (variabl == null)
                throw new Exception();

            string methoName =context.ID().GetText();

            //Manager.CurrentScope.Children.FirstOrDefault(c => c.Name ==);



            string x = (string) Visit(context.expression(0));


            













            //object va = Manager.CurrentScope.IdValues[x];

            /*if(Manager.Classes.Select(c => c.Name).Contains(nameof(va).ToString()) ) // check if type = something from the classes or not
            {
                var t = Manager.CurrentScope.Children.Find(xx=> xx.Name == va.ToString()  );

                Manager.CurrentScope = t;

                var metho = (string)Visit(context.ID());

                //t.Foo91();

            }
            else
            {

            }*/



            return base.VisitDispatchExplicit(context);
        }

        public override object VisitClassDefine([NotNull] CoolParser.ClassDefineContext context)
        {
            Console.WriteLine("--VisitClassDefine--");

            if(Manager.ObjectScope == null)
            {
                Manager.CurrentScope = new Scope1()
                {
                    Name = "Main",
                    Children = new List<Scope1>(),
                    Parent = null,
                    IdValues = new List<Formal>()
                };

                var mainCl = Manager.Classes.FirstOrDefault(c => c.Name == "Main");

                foreach (var prop in mainCl.Properties)
                {
                    Manager.CurrentScope.IdValues.Add(prop);
                } 

                Manager.ObjectScope = Manager.CurrentScope;
            }
            //string className = context.TYPE(0).GetText();
            
            
                 
            
            
            
            
            return base.VisitClassDefine(context);
        }

        public override object VisitMethod([NotNull] CoolParser.MethodContext context)
        {
            Console.WriteLine("--VisitMethod--");
            Scope1 newBlock = new Scope1()
            {
                Name = context.ID().GetText(),
                Children = new List<Scope1>(),
                Parent = Manager.CurrentScope
            };

            Manager.CurrentScope.Children.Add(newBlock);
            Manager.CurrentScope = newBlock;

            var res = context.expression();

            Manager.CurrentScope = Manager.CurrentScope.Parent;
            Manager.CurrentScope.Children.Remove(newBlock);
            return res;
        }

        public override object VisitId([NotNull] CoolParser.IdContext context)
        {
            Console.WriteLine("--VisitID--");

            var val = Manager.CurrentScope.Find(context.GetText());
            object result;

            if(val == null)
            {
                throw new KeyNotFoundException();
            }
            else
            {
                result = Visit(val.Expression);
            }

            return result;
        }

        public override object VisitInt([NotNull] CoolParser.IntContext context)
        {
            Console.WriteLine("--VisitInt--");
            int.TryParse(context.INT().GetText(), out int value);
            return value;
        }

        public override object VisitBlock([NotNull] CoolParser.BlockContext context)
        {
            Console.WriteLine("--VisitExpression--");

            Scope1 newBlock = new Scope1()
            {
                Name = "block",
                Children = new List<Scope1>(),
                Parent = Manager.CurrentScope
            };

            Manager.CurrentScope.Children.Add(newBlock);
            Manager.CurrentScope = newBlock;

            foreach (var expr in context.expression())
            {
                Visit(expr);
            }

            Manager.CurrentScope = Manager.CurrentScope.Parent;
            Manager.CurrentScope.Children.Remove(newBlock);

            return 0;
        }

        public override object VisitWhile([NotNull] CoolParser.WhileContext context)
        {
            Console.WriteLine("--VisitWhile--");

            while ((int)Visit(context.expression(0)) == 0)
            {
                Visit(context.expression(1));
            }

            return 0;
        }

        public override object VisitParentheses([NotNull] CoolParser.ParenthesesContext context)
        {
            Console.WriteLine("--VisitParentheses--");
            return Visit(context.expression());

            return base.VisitParentheses(context);
        }

        public override object VisitArithmetic([NotNull] CoolParser.ArithmeticContext context)
        {
            Console.WriteLine("--VisitArithmetic--");
            int value = 0;

            int left = (int)Visit(context.expression(0));
            int right = (int)Visit(context.expression(1));
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

        public override object VisitAssignment([NotNull] CoolParser.AssignmentContext context)
        {
            Console.WriteLine("--VisitAssignment--");

            Manager.CurrentScope.IdValues.Add(new Formal()
            {
                Id = context.ID().GetText(),
                Expression = context.expression()
            });

            //if (typeof(context.expression().GetType()) == typeof(CoolParser.NewContext))
            //{                
            //} 

            // eg for new B()=> we need to set scope-------- TODO ints get visited, shouldnt be  
            Visit(context.expression());

            return base.VisitAssignment(context);
        }

        public override object VisitComparisson([NotNull] CoolParser.ComparissonContext context)
        {
            Console.WriteLine("--VisitComparisson--");
            int left = (int)Visit(context.expression(0));
            int right = (int)Visit(context.expression(1));

            switch (context.op.Text)
            {
                case "<=":
                    if (left <= right)
                    {
                        return 0;
                    }
                    else
                        return -1;
                    break;
                case "<":
                    if (left < right)
                    {
                        return 0;
                    }
                    else
                        return -1;
                    break;
                case "=":
                    if (left == right)
                    {
                        return 0;
                    }
                    else
                        return -1;
                    break;
                default:
                    throw new Exception("Arithmetic operator not found");
            }

            return base.VisitComparisson(context);
        }

    }
}
