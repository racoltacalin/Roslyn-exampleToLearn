using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxWalker
{
    class Program
    {
        public static void Main()
        {
            const string programText =
                @"using System;
                using System.Collections.Generic;
                using System.Linq;
                using System.Text;
                using Microsoft.CodeAnalysis;
                using Microsoft.CodeAnalysis.CSharp;

                namespace TopLevel
                {
                    using Microsoft;
                    using System.ComponentModel;

                    namespace Child1
                    {
                        using Microsoft.Win32;
                        using System.Runtime.InteropServices;

                        class Foo { }
                    }

                    namespace Child2
                    {
                        using System.CodeDom;
                        using Microsoft.CSharp;

                        class Bar { }
                    }
                }";

            SyntaxTree tree = CSharpSyntaxTree.ParseText(programText);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

            var collector = new UsingCollector();
            collector.Visit(root);
            foreach (var directive in collector.Usings)
            {
                Console.WriteLine(directive.Name);
            }
        }
    }
}
