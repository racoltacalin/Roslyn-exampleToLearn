using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;

namespace LearnRoslynNow
{
    class Program
    {
        static void Main()
        {
            var tree = CSharpSyntaxTree.ParseText(@"
            class C
            {
                void Method()
                {
                    try
                    {
                    //this is a blog
                    }
                    catch(Exception)
                    {
                    }
                }
            }");

            /// Checking for errors in our "code"
            //var diagnostics = tree.GetDiagnostics().Where(n => n.Severity == Microsoft.CodeAnalysis.DiagnosticSeverity.Error).First();
            //Console.WriteLine(diagnostics);

            Console.WriteLine("---------------------------------------");

            var root = tree.GetRoot();
            Console.WriteLine(root);

            Console.WriteLine("---------------------------------------");

            var method = root.DescendantNodes().OfType<MethodDeclarationSyntax>().First();
            Console.WriteLine(method);

            Console.WriteLine("---------------------------------------");

            var tryStatement = root.DescendantNodes().OfType<TryStatementSyntax>().First();
            Console.WriteLine(tryStatement);

            Console.WriteLine("---------------------------------------");

            var block = tryStatement.Block;
            Console.WriteLine(block);

            Console.WriteLine("---------------------------------------");

            var returnType = SyntaxFactory.ParseTypeName("string");
            Console.WriteLine(returnType);

            Console.WriteLine("---------------------------------------");

            /// You need to create a newMethod to change the name of method (for example...)
            var newMethod = method.WithReturnType(returnType);
            Console.WriteLine(newMethod);

            Console.ReadLine();
        }
    }
}
