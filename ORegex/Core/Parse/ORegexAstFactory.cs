using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using ORegex.Core.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORegex.Core.Parse
{
    public sealed class ORegexAstFactory<TValue>
    {
        public static AstNodeBase Create(IParseTree node, RegexGrammarParser parser)
        {
            if(IsRoot(node, parser))
            {
                return CreateRoot(node, parser);
            }
            else if(IsConcat(node, parser))
            {
                return CreateConcat(node, parser);
            }
            else if (IsAtom(node, parser))
            {
                return CreateAtom(node, parser);
            }
            else if (IsGroup(node, parser))
            {
                return CreateGroup(node, parser);
            }
            else if (IsUnOper(node, parser))
            {
                return CreateUnOper(node, parser);
            }
            else if (IsBinOper(node, parser))
            {
                return CreateBinOper(node, parser);
            }
            else
            {
                return Create(node.GetChild(0), parser);//fictive node, skip him
            }
        }

        private static bool IsRoot(IParseTree node, RegexGrammarParser parser)
        {
            return node.ChildCount == 1 && IsName(node, parser, "expr");
        }

        private static bool IsConcat(IParseTree node, RegexGrammarParser parser)
        {
            return node.ChildCount > 1 && IsName(node, parser, "concat");
        }

        private static bool IsAtom(IParseTree node, RegexGrammarParser parser)
        {
            return (node.ChildCount == 1 || node.ChildCount >=3) && IsName(node, parser, "atom");
        }

        private static bool IsNAtom(IParseTree node, RegexGrammarParser parser)
        {
            return node.ChildCount == 1 && IsName(node, parser, "natom");
        }

        private static bool IsGroup(IParseTree node, RegexGrammarParser parser)
        {
            return node.ChildCount >= 3 && IsName(node, parser, "group");
        }

        private static bool IsBinOper(IParseTree node, RegexGrammarParser parser)
        {
            return node.ChildCount >= 3 && IsName(node, parser, "binOper");
        }

        private static bool IsUnOper(IParseTree node, RegexGrammarParser parser)
        {
            return node.ChildCount == 2 && IsName(node, parser, "unOper");
        }

        private static bool IsName(IParseTree node, RegexGrammarParser parser, string name)
        {
            return name == GetName(node, parser);
        }

        private static string GetName(IParseTree node, RegexGrammarParser parser)
        {
            var rule = (RuleContext)node;
            var name = parser.RuleNames[rule.RuleIndex];
            return name;
        }

        private static AstNodeBase CreateRoot(IParseTree node, RegexGrammarParser parser)
        {
            List<AstNodeBase> children = new List<AstNodeBase>();
            for (int i = 0; i < node.ChildCount; i++)
            {
                var child = node.GetChild(i);

                children.Add(Create(child, parser));
            }
            return new AstRootNode(children);
        }

        private static AstNodeBase CreateAtom(IParseTree node, RegexGrammarParser parser)
        {
            if (node.ChildCount == 1)
            {
                var name = node.GetChild(0).ToString();
                if (name == ".")
                {
                    return new AstAnyAtomNode();
                }
                else
                {
                    return CreateNAtom(node.GetChild(0), parser);
                }
            }
            else
            {
                var isNegate = node.GetChild(0).GetText().Length == 2;
                List<AstNodeBase> children = new List<AstNodeBase>();
                for(int i = 1; i < node.ChildCount-1;i++)
                {
                    var child = CreateNAtom(node.GetChild(i), parser);
                    children.Add(child);
                }
                return new AstVarAtomNode(children, isNegate);
            }
        }

        private static AstAtomNode<TValue> CreateNAtom(IParseTree node, RegexGrammarParser parser)
        {
            var name = node.GetChild(0).ToString();
            return new AstAtomNode<TValue>(name.Substring(1, name.Length - 2));
        }

        private static AstGroupNode CreateGroup(IParseTree node, RegexGrammarParser parser)
        {
            var predicate = node.GetChild(0).GetText();
            List<AstNodeBase> children = new List<AstNodeBase>();
            for (int i = 1; i < node.ChildCount - 1; i++)
            {
                var child = Create(node.GetChild(i), parser);
                children.Add(child);
            }

            if(predicate.StartsWith("(?<"))
            {
                var name = predicate.Substring(3, predicate.Length - 4);
                return new AstGroupNode(name, children);
            }
            return new AstGroupNode(children);
        }

        private static AstRepeatNode CreateUnOper(IParseTree node, RegexGrammarParser parser)
        {
            var arg = Create(node.GetChild(0), parser);
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

        private static AstOrNode CreateBinOper(IParseTree node, RegexGrammarParser parser)
        {
            List<AstNodeBase> children = new List<AstNodeBase>();
            for (int i = 0; i < node.ChildCount; i++)
            {
                var child = node.GetChild(i);
                if (child.GetText() != "|")
                {
                    children.Add(Create(child, parser));
                }
            }
            return new AstOrNode(children);
        }

        private static AstConcatNode CreateConcat(IParseTree node, RegexGrammarParser parser)
        {
            List<AstNodeBase> children = new List<AstNodeBase>();
            for (int i = 0; i < node.ChildCount; i++)
            {
                var child = node.GetChild(i);

                children.Add(Create(child, parser));
            }
            return new AstConcatNode(children); 
        }
    }
}
