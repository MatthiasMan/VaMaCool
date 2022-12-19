using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VaMaCool
{
    class CoolPropertyVisitor : CoolBaseVisitor<object>
    {
        private ClassMap Current { get; set; }


        public override object VisitClassDefine([NotNull] CoolParser.ClassDefineContext context)
        {
            var curr = Manager.Classes.FirstOrDefault(c => c.Name == context.TYPE(0).GetText());

            if (curr == null)
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

        public override object VisitProperty([NotNull] CoolParser.PropertyContext context)
        {
            Formal x = (Formal)Visit(context.formal());

            x.Expression = context.expression();

            Current.Properties.Add(x);

            return base.VisitProperty(context);
        }

        public override object VisitFormal([NotNull] CoolParser.FormalContext context)
        {
            //string x = (string)Visit(context.ID());
            string x = context.ID().GetText();
            string y = context.TYPE().GetText();
            return new Formal() { Id=x, Type=y, Expression=null};
        }

        public override object VisitId([NotNull] CoolParser.IdContext context)
        {
            return context.ID().GetText();
        }
    }
}
