using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticModelAndSymbols
{
    class Program
    {
        static void Main(string[] args)
        {
            var tree = CSharpSyntaxTree.ParseText(@"
            public partial class MyPartialClass
            {
                void MyMethod()
                {
                    System.Console.WriteLine(""Hello World"");
                }
            }

            public partial class MyPartialClass
            { 
            }");

            var mscorLib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
            var compilation = CSharpCompilation.Create("MyCompilation",
                syntaxTrees: new[] { tree }, references: new[] { mscorLib });

            var root = tree.GetRoot();

            var semanticModel = compilation.GetSemanticModel(tree);
            var methodSyntax = root.DescendantNodes().OfType<MethodDeclarationSyntax>().Single();

            var methodSymbol = semanticModel.GetDeclaredSymbol(methodSyntax);

            //var parentAssembly = methodSymbol.ContainingAssembly;

            //var firstClass = root.DescendantNodes().OfType<ClassDeclarationSyntax>().First();
            //var secondClass = root.DescendantNodes().OfType<ClassDeclarationSyntax>().Last();

            //var firstSymbol = semanticModel.GetDeclaredSymbol(firstClass);
            //var secondSymbol = semanticModel.GetDeclaredSymbol(secondClass);

            //var areEqual = (firstSymbol == secondSymbol);
            //Console.WriteLine(areEqual);
            //Console.ReadLine();

            var invokedMethod = root.DescendantNodes().OfType<InvocationExpressionSyntax>().Single();
            var symbolInfo = semanticModel.GetSymbolInfo(invokedMethod);
            var invokedSymbol = symbolInfo.Symbol as IMethodSymbol;

            var containingAssembly = invokedSymbol.ContainingAssembly;
        }
    }


}
