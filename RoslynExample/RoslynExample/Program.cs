using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RoslynExample
{
    class Program
    {
        public static void Main(string[] args)
        {
            Compilation test = CreateTestCompilation();
            foreach (SyntaxTree sourceTree in test.SyntaxTrees)
            {
                // creation of the semantic model
                SemanticModel model = test.GetSemanticModel(sourceTree);
                // initialization of our rewriter class
                InitializerRewriter rewriter = new InitializerRewriter(model);
                // analysis of the tree
                SyntaxNode newSource = rewriter.Visit(sourceTree.GetRoot());
                if (!Directory.Exists(@"../new_src"))
                    Directory.CreateDirectory(@"../new_src");
                // if we changed the tree we save a new file
                if (newSource != sourceTree.GetRoot())
                {
                    File.WriteAllText(Path.Combine(@"../new_src", Path.GetFileName(sourceTree.FilePath)), newSource.ToFullString());
                }
            }
        }

        private static Compilation CreateTestCompilation()
        {
            // creation of the syntax tree for every file
            String programPath = @"Program.cs";
            String programText = File.ReadAllText(programPath);
            SyntaxTree programTree = CSharpSyntaxTree.ParseText(programText)
                                     .WithFilePath(programPath);
            String rewriterPath = @"InitializerRewriter.cs";
            String rewriterText = File.ReadAllText(rewriterPath);
            SyntaxTree rewriterTree = CSharpSyntaxTree.ParseText(rewriterText)
                                .WithFilePath(rewriterPath);
            SyntaxTree[] sourceTrees = { programTree, rewriterTree };

            // gathering the assemblies
            MetadataReference mscorlib = MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location);
            MetadataReference codeAnalysis = MetadataReference.CreateFromFile(typeof(SyntaxTree).GetTypeInfo().Assembly.Location);
            MetadataReference csharpCodeAnalysis = MetadataReference.CreateFromFile(typeof(CSharpSyntaxTree).GetTypeInfo().Assembly.Location);
            MetadataReference[] references = { mscorlib, codeAnalysis, csharpCodeAnalysis };

            // compilation
            return CSharpCompilation.Create("ConsoleApplication",
                             sourceTrees,
                             references,
                             new CSharpCompilationOptions(OutputKind.ConsoleApplication));
        }
    }
}

