﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
namespace Eocron.Core.Ast
{
    public abstract class AstNodeBase: IEnumerable<AstNodeBase>
    {
        public abstract IEnumerable<AstNodeBase> GetChildren();

        public readonly Range Range;

        protected AstNodeBase(Range range)
        {
            Range = range;
        }

        public static void Print(AstNodeBase node, int depth = 0)
        {
            if (depth == 0)
            {
                Console.WriteLine();
            }
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

            foreach(var child in node.GetChildren())
            {
                Print(child, depth + 1);
            }
        }


        public IEnumerator<AstNodeBase> GetEnumerator()
        {
            return GetChildren().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
