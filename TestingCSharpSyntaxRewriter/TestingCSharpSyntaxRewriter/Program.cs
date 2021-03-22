using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingCSharpSyntaxRewriter
{
    class Program
    {
        static void Main(string[] args)
        {
            var tree = CSharpSyntaxTree.ParseText(@"
            class Program
            {
                static void Main()
                {
                    if(true)
                    {
                        Console.WriteLine(""It was true!"");
                    }
                    if(false)
                    {
                        Console.WriteLine(""OMG, how'd we get here?!"");
                    }
                }
            }");

            var root = tree.GetRoot();
            //var rewriter = new MyRewriter();
            //var newRoot = rewriter.Visit(root);
            //Console.WriteLine(newRoot);

            var ifStatements = root.DescendantNodes().OfType<IfStatementSyntax>();
            foreach (var ifStatement in ifStatements)
            {
                var body = ifStatement.Statement;
                var block = SyntaxFactory.Block(body);
                var newIfStatement = ifStatement.WithStatement(block);
                root = root.ReplaceNode(ifStatement, newIfStatement);
            }
            Console.WriteLine(root);

            Console.ReadLine();
        }
    }

    public class MyRewriter : CSharpSyntaxRewriter
    {
        public override SyntaxNode VisitIfStatement(IfStatementSyntax node)
        {
            var body = node.Statement;
            var block = SyntaxFactory.Block(body);
            var newIfStatement = node.WithStatement(block);
            return newIfStatement;

        }
    }
}
