using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Tree;
using ORegex.Core.Ast;
using ORegex.Core.Ast.GroupQuantifiers;

namespace ORegex.Core.Parse
{
    public sealed class ORegexAstFactory<TValue>
    {
        public static AstNodeBase Create(IParseTree node, ORegexAstFactoryArgs<TValue> args)
        {
            if(IsRoot(node, args))
            {
                return CreateRoot(node, args);
            }
            else if(IsConcat(node, args))
            {
                return CreateConcat(node, args);
            }
            else if (IsAtom(node, args))
            {
                return CreateAtom(node, args);
            }
            else if (IsGroup(node, args))
            {
                return CreateGroup(node, args);
            }
            else if (IsUnOper(node, args))
            {
                return CreateUnOper(node, args);
            }
            else if (IsBinOper(node, args))
            {
                return CreateBinOper(node, args);
            }
            else
            {
                return Create(node.GetChild(0), args);//fictive node, skip him
            }
        }

        private static bool IsRoot(IParseTree node, ORegexAstFactoryArgs<TValue> args)
        {
            return args.IsName(node, "expr");
        }

        private static bool IsConcat(IParseTree node, ORegexAstFactoryArgs<TValue> args)
        {
            return node.ChildCount > 1 && args.IsName(node, "concat");
        }

        private static bool IsAtom(IParseTree node, ORegexAstFactoryArgs<TValue> args)
        {
            return args.IsName(node, "natom") || args.IsName(node, "atom");
        }

        private static bool IsOrAtom(IParseTree node, ORegexAstFactoryArgs<TValue> args)
        {
            return node.ChildCount >= 3 && args.IsName(node, "atom");
        }

        private static bool IsGroup(IParseTree node, ORegexAstFactoryArgs<TValue> args)
        {
            return node.ChildCount == 3 && args.IsName(node, "group");
        }

        private static bool IsBinOper(IParseTree node, ORegexAstFactoryArgs<TValue> args)
        {
            return node.ChildCount >= 3 && args.IsName(node, "binOper");
        }

        private static bool IsUnOper(IParseTree node, ORegexAstFactoryArgs<TValue> args)
        {
            return node.ChildCount == 2 && args.IsName(node, "unOper");
        }

        private static AstNodeBase CreateRoot(IParseTree node, ORegexAstFactoryArgs<TValue> args)
        {
            List<AstNodeBase> children = new List<AstNodeBase>();
            bool matchBegin = false;
            bool matchEnd = false;

            if (node.GetChild(0).GetText() == "^")
            {
                matchBegin = true;
            }

            if (node.GetChild(node.ChildCount - 1).GetText() == "$")
            {
                matchEnd = true;
            }

            for (int i = 0; i < node.ChildCount; i++)
            {
                if (matchBegin && i == 0 || matchEnd && i == node.ChildCount - 1)
                {
                    continue;
                }

                var child = node.GetChild(i);

                children.Add(Create(child, args));
            }
            return new AstRootNode(children, matchBegin, matchEnd);
        }

        private static AstNodeBase CreateAtom(IParseTree node, ORegexAstFactoryArgs<TValue> args)
        {
            if (node.ChildCount == 1)
            {
                var name = node.GetChild(0).GetText();
                if (name == ".")
                {
                    return new AstAtomNode<TValue>(name, args.GetPredicate(name));
                }
                else
                {
                    return CreateNAtom(node.GetChild(0), args);
                }
            }
            else
            {
                var children = new List<AstAtomNode<TValue>>();
                for (int i = 1; i < node.ChildCount - 1; i++)
                {
                    var child = CreateNAtom(node.GetChild(i), args);
                    children.Add(child);
                }

                var isNegate = node.GetChild(0).GetText().Length == 2;
                if (isNegate)
                {
                    Func<TValue, bool> p;
                    string n;
                    args.GetInvertedPredicate(children.Select(x => x.Name), out p, out n);
                    return new AstAtomNode<TValue>(n, p);
                }
                else
                {
                    return new AstOrNode(children);
                }
            }
        }

        private static AstAtomNode<TValue> CreateNAtom(IParseTree node, ORegexAstFactoryArgs<TValue> args)
        {
            var name = node.GetChild(0).ToString();
            var atomName = name.Substring(1, name.Length - 2);
            var atomPredicate = args.GetPredicate(atomName);
            return new AstAtomNode<TValue>(atomName, atomPredicate);
        }

        private static AstGroupNode CreateGroup(IParseTree node, ORegexAstFactoryArgs<TValue> args)
        {
            var predicate = node.GetChild(0).GetText();
            List<AstNodeBase> children = new List<AstNodeBase>();
            for (int i = 1; i < node.ChildCount - 1; i++)
            {
                var child = Create(node.GetChild(i), args);
                children.Add(child);
            }

            QuantifierBase quantifier = null;
            if(predicate.StartsWith("(?<") && predicate.Length > 4)
            {
                var name = predicate.Substring(3, predicate.Length - 4);
                quantifier = new CaptureQuantifier(predicate, name);
            }
            else if(predicate.StartsWith("(?"))
            {
                quantifier = new LookAheadQuantifier(predicate);
            }
            return new AstGroupNode(children, quantifier);
        }

        private static AstRepeatNode CreateUnOper(IParseTree node, ORegexAstFactoryArgs<TValue> args)
        {
            var arg = Create(node.GetChild(0), args);
            var oper = node.GetChild(1).ToString();
            int min = 0, max = 0;
            bool isGreedy = false;
            switch(oper)
            {
                case "*": min = 0; max = int.MaxValue; break;
                case "*?": min = 0; max = int.MaxValue; isGreedy = true; break;
                case "+": min = 1; max = int.MaxValue; break;
                case "+?": min = 1; max = int.MaxValue; isGreedy = true; break;
                case "?": min = 0; max = 1; break;
                default:
                    throw new NotImplementedException("Unsuported operator.");
            }

            return new AstRepeatNode(arg, min, max, isGreedy);
        }

        private static AstOrNode CreateBinOper(IParseTree node, ORegexAstFactoryArgs<TValue> args)
        {
            List<AstNodeBase> children = new List<AstNodeBase>();
            for (int i = 0; i < node.ChildCount; i++)
            {
                var child = node.GetChild(i);
                if (child.GetText() != "|")
                {
                    children.Add(Create(child, args));
                }
            }
            return new AstOrNode(children);
        }

        private static AstConcatNode CreateConcat(IParseTree node, ORegexAstFactoryArgs<TValue> args)
        {
            List<AstNodeBase> children = new List<AstNodeBase>();
            for (int i = 0; i < node.ChildCount; i++)
            {
                var child = node.GetChild(i);

                children.Add(Create(child, args));
            }
            return new AstConcatNode(children); 
        }
    }
}
