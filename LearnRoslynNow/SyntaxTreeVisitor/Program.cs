using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxTreeVisitor
{
    class Program
    {
        static void Main(string[] args)
        {
            var walker = new MyWalker();
            var tree = CSharpSyntaxTree.ParseText(@"
            class C
            {
                void M()
                {
                }
            }");

            var root = tree.GetRoot();
            walker.Visit(root);
            var result = walker.sb.ToString();

            Console.WriteLine(result);
            Console.ReadLine();
        }
    }

    public class MyWalker : CSharpSyntaxWalker
    {
        public MyWalker()
            : base(SyntaxWalkerDepth.Token)
        {

        }

        public StringBuilder sb = new StringBuilder();

        //public override void VisitToken(SyntaxToken token)
        //{
        //    //sb.Append(token.ToFullString());
        //    sb.Append(token.ToString());
        //}

        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            sb.Append(node.ToString());
            base.VisitMethodDeclaration(node);
        }
    }
}
