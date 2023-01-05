using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VaMaCool.Visitors
{
    public  class CoolVisitor3 : CoolBaseVisitor<VisitorValue>
    {
        private string _lastName = string.Empty;
        public override VisitorValue VisitProgram([NotNull] CoolParser.ProgramContext context)
        {

            Console.WriteLine("--VisitProgram--");


            if (Manager.ObjectScope == null)
            {
                Manager.CurrentScope = new Scope1()
                {
                    Name = "Main",
                    Children = new List<Scope1>(),
                    Parent = null,
                    EnumArt = EnumArt.Class,
                    IdValues = new List<Property>()
                };

                var mainCl = Manager.Classes.FirstOrDefault(c => c.Name == "Main");

                foreach (var prop in mainCl.Properties)
                {
                    Manager.CurrentScope.IdValues.Add(prop);
                }

                Manager.ObjectScope = Manager.CurrentScope;

               /* Method mainMethod = mainCl.Methods.FirstOrDefault(m => m.Id == "main");

                Scope1 mainScope = new Scope1()
                {
                    Name = "main",
                    Children = new List<Scope1>(),
                    EnumArt = EnumArt.Method,
                    Parent = Manager.ObjectScope,
                    IdValues = new List<Property>()
                };

                Manager.ObjectScope.Children.Add(mainScope);

                Manager.CurrentScope = mainScope;

                //Visit(mainMethod.Expression);*/

                foreach (var classs in context.classDefine())
                {
                    foreach (var feat in classs.feature())
                    {
                        if(feat.method() != null && feat.method().ID().GetText() == "main")
                        {
                            Visit(feat.method());
                        }
                        
                    } 
                } 
            }


            return base.VisitProgram(context);
        }


        public override VisitorValue VisitClassDefine([NotNull] CoolParser.ClassDefineContext context)
        {
            Console.WriteLine("--VisitClassDefine--");

           

            return base.VisitClassDefine(context);
        }

        public override VisitorValue VisitMethod([NotNull] CoolParser.MethodContext context)
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

            var methotyp = context.TYPE().GetText();
            var res = Visit(context.expression());

            if (methotyp != res.Type)
                throw new Exception("Method returned wrong type");

            Manager.CurrentScope = Manager.CurrentScope.Parent;
            Manager.CurrentScope.Children.Remove(newBlock);
            return res;
        }

        public override VisitorValue VisitInt([NotNull] CoolParser.IntContext context)
        {
            Console.WriteLine("--VisitInt--");
            if (!int.TryParse(context.INT().GetText(), out int value))
                throw new Exception("could not parse to int");
            return new VisitorValue() { Object = value, Type = "Int" };
        }

        public override VisitorValue VisitString([NotNull] CoolParser.StringContext context)
        {
            Console.WriteLine("--VisitString--");
            string value = context.STRING().GetText();
            return new VisitorValue() { Object = value, Type = "String" };
        }

        public override VisitorValue VisitBoolean([NotNull] CoolParser.BooleanContext context)
        {
            Console.WriteLine("--VisitBoolean--");
            if(context.value.Text == "TRUE")
                return new VisitorValue() { Object = true, Type = "Boolean"};
            else if (context.value.Text == "FALSE")
                return new VisitorValue() { Object = false, Type = "Boolean" };
            else
                throw new Exception("value was not TRUE or FALSE");
        }

        public override VisitorValue VisitBoolNot([NotNull] CoolParser.BoolNotContext context)
        {
            Console.WriteLine("--VisitBooleanNot--");
            var res = Visit(context.expression());

            if(res.Type != "Boolean")
                throw new Exception("expression didnt return a boolean");

            if ((Boolean)res.Object == true)
                return new VisitorValue() { Object = false, Type = "Boolean" };
            else if ((Boolean)res.Object == false)
                return new VisitorValue() { Object = true, Type = "Boolean" };
            else
                throw new Exception("Boolean is not true or false");
        }

        public override VisitorValue VisitComparisson([NotNull] CoolParser.ComparissonContext context)
        {
            Console.WriteLine("--VisitComparisson--");
            VisitorValue leftVisit = Visit(context.expression(0));
            VisitorValue rightVisit = Visit(context.expression(1));

            if (leftVisit.Type != rightVisit.Type)
                throw new Exception("left and right Types are not the same");

            object left = leftVisit.Object;
            object right = rightVisit.Object;

            VisitorValue result = new VisitorValue() { Type = "Boolean" };

            switch (context.op.Text)
            {
                case "<=":
                    if (leftVisit.Type == "Int")
                        result.Object = (int)left <= (int)right;
                    else
                        throw new Exception("can only compare Integer with this comparer");
                    break;
                case "<":
                    if (leftVisit.Type == "Int")
                        result.Object = (int)left < (int)right;
                    else
                        throw new Exception("can only compare Integer with this comparer");
                    break;
                case "=":
                    if (leftVisit.Type == "Int")
                        result.Object = (int)left == (int)right;
                    else if (leftVisit.Type == "String")
                        result.Object = (string)left == (string)right;
                    else if (leftVisit.Type == "Boolean")
                        result.Object = (bool)left == (bool)right;
                    else
                        throw new Exception("can only compare Integer with this comparer");
                    break;
                default:
                    throw new Exception("Comparison operator not found");
            }

            return result;
        }

        public override VisitorValue VisitArithmetic([NotNull] CoolParser.ArithmeticContext context)
        {
            Console.WriteLine("--VisitArithmetic--");
            int value = 0;

            VisitorValue leftVisit = Visit(context.expression(0));
            VisitorValue rightVisit = Visit(context.expression(1));

            if (leftVisit.Type != "Int" ||  rightVisit.Type != "Int")
                throw new Exception("left and right Types are not Int");

            int left = (int)leftVisit.Object;
            int right = (int)rightVisit.Object;


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

            return new VisitorValue() { Object = value, Type = "Int"};
        }

        public override VisitorValue VisitId([NotNull] CoolParser.IdContext context)
        {
            Console.WriteLine("--VisitID--");

            var val = Manager.CurrentScope.Find(context.GetText());
            object result;

            if (val == (null, null))
                throw new KeyNotFoundException();
            else
            {
                if (val.Item1.Expression == null)
                    return new VisitorValue { Type = val.Item1.Type, Object = null }; // just x: Int;
                else
                    return Visit(val.Item1.Expression);
            }
        }

        public override VisitorValue VisitParentheses([NotNull] CoolParser.ParenthesesContext context)
        {
            Console.WriteLine("--VisitParentheses--");
            return Visit(context.expression());
        }

        public override VisitorValue VisitIf([NotNull] CoolParser.IfContext context)
        {
            Console.WriteLine("--VisitIf--");
            var fir = Visit(context.expression(0));

            if (fir.Type != "Boolean")
                throw new Exception("Statement is no boolean");

            if ((bool)fir.Object)
                Visit(context.expression(1));
            else
                Visit(context.expression(2));

            return new VisitorValue { Object = null, Type = null};
        }

        public override VisitorValue VisitIsvoid([NotNull] CoolParser.IsvoidContext context)
        {
            Console.WriteLine("--VisitVoid--");
            var fir = Visit(context.expression());

            if (fir.Type == "Void")
                return new VisitorValue { Object = true, Type = "Boolean" };
            else
                return new VisitorValue { Object = false, Type = "Boolean" };
        }

        public override VisitorValue VisitWhile([NotNull] CoolParser.WhileContext context)
        {
            Console.WriteLine("--VisitWhile--");
            var fir = Visit(context.expression(0));

            if (fir.Type != "Boolean")
                throw new Exception("Statement is no boolean");

            while ((bool)fir.Object)
                Visit(context.expression(1));

            return new VisitorValue { Object = null, Type = null };
        }

        public override VisitorValue VisitCase([NotNull] CoolParser.CaseContext context)
        {
            throw new NotImplementedException();
            var fir = Visit(context.expression(0));

            for (int i = 0; i < context.formal().Length; i++)
            {
                var curF = context.formal()[i];


                if (true)
                {
                    Visit(context.expression(i + 1));
                    break;
                }
            }

            return base.VisitCase(context);
        }

        public override VisitorValue VisitLetIn([NotNull] CoolParser.LetInContext context)
        {
            Console.WriteLine("--VisitLetIn--");

            Scope1 newBlock = new Scope1()
            {
                Name = "LetIn",
                Children = new List<Scope1>(),
                Parent = Manager.CurrentScope
            };

            Manager.CurrentScope.Children.Add(newBlock);
            Manager.CurrentScope = newBlock;

            foreach (var prop in context.property())
            {
                Property newP = new Property()
                {
                    Expression = prop.expression(),
                    Id = prop.formal().ID().GetText(),
                    Type = prop.formal().TYPE().GetText()
                };

                newBlock.IdValues.Add(newP);
            }

            var result = Visit(context.expression());

            Manager.CurrentScope = Manager.CurrentScope.Parent;
            Manager.CurrentScope.Children.Remove(newBlock);

            return result;
        }

        public override VisitorValue VisitAssignment([NotNull] CoolParser.AssignmentContext context)
        {
            Console.WriteLine("--VisitAssignment--");

            Property prop = Manager.CurrentScope.Find(context.ID().GetText()).Item1;

            _lastName = prop.Id;

            if (prop.Expression != null)
                prop.Value = Visit(prop.Expression);

            _lastName = String.Empty;

            // eg for new B()=> we need to set scope-------- TODO ints get visited, shouldnt be  
            prop.Value = Visit(context.expression());


            return new VisitorValue { Type = "Void"};
        }

        public override VisitorValue VisitNew([NotNull] CoolParser.NewContext context)
        {
            Console.WriteLine("--VisitNew--");

            Scope1 newBlock = new Scope1()
            {
                Name = _lastName, //context.TYPE().GetText(),
                Children = new List<Scope1>(),
                Parent = Manager.CurrentScope,
                EnumArt = EnumArt.Class,

            };

            var mainCl = Manager.Classes.FirstOrDefault(c => c.Name == context.TYPE().GetText());

            foreach (var prop in mainCl.Properties)
            {
                newBlock.IdValues.Add(prop);
            }

            Manager.CurrentScope.Children.Add(newBlock);

            return new VisitorValue { Type = context.TYPE().GetText() };
        }

        public override VisitorValue VisitBlock([NotNull] CoolParser.BlockContext context)
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

            return new VisitorValue { Type = "Void"};
        }

        public override VisitorValue VisitDispatchExplicit([NotNull] CoolParser.DispatchExplicitContext context)
        {

            var result = Visit(context.expression(0));

            //result.Type

            var variabl = Manager.CurrentScope.Find(context.expression(0).GetText());

            if (variabl == (null, null))   // e.g. (new Car)
                Manager.CurrentScope.Find("");

            if (variabl == (null, null))
                throw new Exception();

            string methoName = context.ID().GetText();

            //Manager.CurrentScope.Children.FirstOrDefault(c => c.Name ==);

            Manager.CurrentScope = variabl.Item2;

            var x = Manager.CurrentScope.Find(methoName);

            string oldScopeName = Manager.CurrentScope.Name;

            Manager.CurrentScope = x.Item2;

            var result = Visit(x.Item1.Expression);

            Manager.CurrentScope = Manager.ObjectScope.FindToBot(oldScopeName);

            //string x = (string) Visit(context.expression(0));




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

        }
    }
}
