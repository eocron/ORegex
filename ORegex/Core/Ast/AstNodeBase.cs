using System;
using System.Diagnostics;
using System.Linq;
namespace ORegex.Core.Ast
{
    public abstract class AstNodeBase
    {
        public static void Print(AstNodeBase node, int depth = 0)
        {
            var attr = node.GetType().GetCustomAttributes(true).OfType<DebuggerDisplayAttribute>().FirstOrDefault();

            Console.Write(string.Join("", Enumerable.Repeat("  ", depth)));
            if (attr != null)
            {
                Console.WriteLine(attr.Value);
            }
            else
            {
                Console.WriteLine(node);
            }
            var astNonTerminal = node as AstNonTerminalNodeBase;
            if (astNonTerminal != null)
            {
                for (int i = 0; i < astNonTerminal.Children.Length; i++)
                {
                    var child = astNonTerminal.Children[i];
                    Print(child, depth + 1);
                }
            }
        }
    }
}
