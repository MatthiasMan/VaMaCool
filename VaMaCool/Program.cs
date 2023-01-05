using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VaMaCool.Visitors;

namespace VaMaCool
{
    class Program
    {
        public static string _input1 = "class P {f() : Int { 1 };};";
        public static string _input2 = "x <- 5 + 3;";
        public static string _input3 = "x <- 2 + 3 * 10 / 5;";


        static void Main(string[] args)
        {
            LexerTest();

            ParserTest();
        }

        static void LexerTest()
        {
            //string input = "class Cons inherits List { xcar: Int;   xcdr: List; isNil() : Bool { false };   init(hd: Int, tl: List) : Cons { { xcar < -hd;   xcdr < -tl;    self;    }}};";

            AntlrInputStream inputStream = new AntlrInputStream(_input3);
            CoolLexer lex = new CoolLexer(inputStream);

            List<IToken> itl = (List<IToken>)lex.GetAllTokens();
            foreach (var item in itl)
            {
                Console.Write(item.ToString() + " -> ");
                string tokenname = lex.Vocabulary.GetDisplayName(item.Type);

                // string tokenname = lex.Vocabulary.GetLiteralName(item.Type);
                // if (tokenname == null) tokenname = lex.Vocabulary.GetSymbolicName(item.Type);
                Console.WriteLine(tokenname);
            }
        }

        static void ParserTest()
        {

            var s = File.ReadAllText(Directory.GetCurrentDirectory() + "/y.txt");







            //string input = "john says: hello @michael this will not work\n";

            AntlrInputStream inputStream = new AntlrInputStream(/*_input3*/ s);
            CoolLexer lex = new CoolLexer(inputStream);

            CommonTokenStream commonTokenStream = new CommonTokenStream(lex);
            
            CoolParser parser = new CoolParser(commonTokenStream);

            //CoolParser.ClassDefineContext ctx1 = parser.classDefine();
            //Console.WriteLine(ctx1.ToStringTree());
            //CoolParser.ExpressionContext ctx2 = parser.expression();
            //Console.WriteLine(ctx2.ToStringTree());
            //CoolParser.MethodContext ctx3 = parser.method();
            //Console.WriteLine(ctx3.ToStringTree());

            CoolParser.ProgramContext ctx2 = parser.program();
            
            ////////////////////// Initialize Class Skelets ///////////////////////////////////////////////
            CoolClassVisitor1 vs1 = new CoolClassVisitor1();
            var vs1R = vs1.Visit(ctx2);

            CoolInheritVisitor1 vs2 = new CoolInheritVisitor1();
            var vs2R = vs2.Visit(ctx2);

            CoolMethodVisitor vs3 = new CoolMethodVisitor();
            var vs3R = vs3.Visit(ctx2);

            CoolPropertyVisitor vs4 = new CoolPropertyVisitor();
            var vs4R = vs4.Visit(ctx2);
            /////////////////////////////////////////////////////////////////////////////////////////

            //foreach (var item in parser.method())
            // {

            // }


            //CoolParser.MethodContext ctx3 =
            //parser.InContext

            var mainClass = Manager.Classes.FirstOrDefault(c=>c.Name == "Main");


            //Manager.ObjectScope = new Scope1() { Name = mainClass.Name, IdValues };
            //Manager.CurrentScope = Manager.ObjectScope;




            //CoolVisitor2  v = new CoolVisitor2();
            CoolVisitor3 v = new CoolVisitor3();
            var res = v.Visit(ctx2);
        }
    }
}