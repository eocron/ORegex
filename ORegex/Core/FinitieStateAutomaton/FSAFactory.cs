using System;
using System.Linq;
using Eocron.Core.Ast;
using Eocron.Core.Ast.GroupQuantifiers;
using Eocron.Core.FinitieStateAutomaton.Predicates;

namespace Eocron.Core.FinitieStateAutomaton
{
    public sealed class FSAFactory<TValue>
    {
        private readonly FSAPreprocessor<TValue> _preprocessor = new FSAPreprocessor<TValue>();

        public FSA<TValue> CreateRawFsa(AstNodeBase root, string name)
        {
            var result = new FSA<TValue>(name);
            var start = result.NewState();
            var end = result.NewState();
            Evaluate(start, end, result, root);
            result.AddFinal(end);
            result.AddStart(start);
            return result;
        }

        public FiniteAutomaton<TValue> Create(AstNodeBase root, string name)
        {
            var nfa = CreateRawFsa(root, name);
            var dfa = _preprocessor.Preprocess(nfa);
            return new FiniteAutomaton<TValue>(new CFSA<TValue>(dfa), new CFSA<TValue>(nfa));
        }

        public void Evaluate(int start, int end, FSA<TValue> fsa, AstNodeBase node)
        {
            if (node is AstAtomNode<TValue>)
            {
                EvaluateAtom(start, end, fsa, (AstAtomNode<TValue>)node);
            }
            else if(node is AstConcatNode)
            {
                EvaluateConcat(start, end, fsa, (AstConcatNode)node);
            }
            else if(node is AstOrNode)
            {
                EvaluateOr(start, end, fsa, (AstOrNode)node);
            }
            else if(node is AstRepeatNode)
            {
                EvaluateRepeat(start, end, fsa, (AstRepeatNode)node);
            }
            else if(node is AstRootNode)
            {
                EvaluateRoot(start, end, fsa, (AstRootNode)node);
            }
            else
            {
                throw new NotImplementedException(node.GetType().Name);
            }
        }

        private void EvaluateRoot(int start, int end, FSA<TValue> fsa, AstRootNode astRootNode)
        {
            fsa.ExactBegin = astRootNode.MatchBegin;
            fsa.ExactEnd = astRootNode.MatchEnd;
            Evaluate(start, end, fsa, astRootNode.Regex);
        }

        private void EvaluateRepeat(int start, int end, FSA<TValue> fsa, AstRepeatNode astRepeatNode)
        {
            var toRepeat = astRepeatNode.Argument;
            var prev = start;
            for (int i = 0; i < astRepeatNode.MinCount; i++)
            {
                var next = CreateNewState(fsa);
                Evaluate(prev, next, fsa, toRepeat);
                prev = next;
            }

            if (astRepeatNode.MaxCount == int.MaxValue)
            {
                RepeatZeroOrInfinite(prev, end, fsa, toRepeat, astRepeatNode.IsLazy);
            }
            else
            {
                int count = astRepeatNode.MaxCount - astRepeatNode.MinCount - 1;
                int next;
                for (int i = 0; i < count; i++)
                {
                    next = CreateNewState(fsa);
                    RepeatZeroOrOne(prev, next, fsa, toRepeat, astRepeatNode.IsLazy);
                    prev = next;
                }
                next = end;
                RepeatZeroOrOne(prev, next, fsa, toRepeat, astRepeatNode.IsLazy);
            }
        }
        //private void RepeatZeroOrOne(int start, int end, FSA<TValue> fsa, AstNodeBase node, bool isLasy)
        //{
        //    if (isLasy)
        //    {
        //        var lazyEdge = new SystemPredicateEdge<TValue>("#lazyEps", true);
        //        fsa.AddTransition(start, lazyEdge, end);
        //        Evaluate(start, end, fsa, node);
        //    }
        //    else
        //    {
        //        Evaluate(start, end, fsa, node);
        //        fsa.AddEpsilonTransition(start, end);
        //    }
        //}

        //private void RepeatZeroOrInfinite(int start, int end, FSA<TValue> fsa, AstNodeBase predicate, bool isLasy)
        //{

        //    var tmp = CreateNewState(fsa);
        //    if (isLasy)
        //    {
        //        var lazyEdge = new SystemPredicateEdge<TValue>("#lazyEps", true);
        //        fsa.AddTransition(tmp, lazyEdge, end);
        //        Evaluate(tmp, tmp, fsa, predicate);

        //        fsa.AddTransition(start, lazyEdge, end);
        //        fsa.AddEpsilonTransition(start, tmp);
        //    }
        //    else
        //    {
        //        Evaluate(tmp, tmp, fsa, predicate);
        //        fsa.AddEpsilonTransition(tmp, end);

        //        fsa.AddEpsilonTransition(start, tmp);
        //        fsa.AddEpsilonTransition(start, end);
        //    }
        //}
        private void RepeatZeroOrOne(int start, int end, FSA<TValue> fsa, AstNodeBase node, bool isLasy)
        {
            if (isLasy)
            {
                fsa.AddEpsilonTransition(start, end);
                Evaluate(start, end, fsa, node);
            }
            else
            {
                Evaluate(start, end, fsa, node);
                fsa.AddEpsilonTransition(start, end);
            }
        }

        private void RepeatZeroOrInfinite(int start, int end, FSA<TValue> fsa, AstNodeBase predicate, bool isLasy)
        {
            var tmp = CreateNewState(fsa);
            if (isLasy)
            {
                fsa.AddEpsilonTransition(tmp, end);
                Evaluate(tmp, tmp, fsa, predicate);

                fsa.AddEpsilonTransition(start, end);
                fsa.AddEpsilonTransition(start, tmp);
            }
            else
            {
                Evaluate(tmp, tmp, fsa, predicate);
                fsa.AddEpsilonTransition(tmp, end);

                fsa.AddEpsilonTransition(start, tmp);
                fsa.AddEpsilonTransition(start, end);
            }
        }

        private void EvaluateOr(int start, int end, FSA<TValue> fsa, AstOrNode node)
        {
            foreach (var child in node.GetChildren())
            {
                Evaluate(start, end, fsa, child);
            }
        }

        private void EvaluateConcat(int start, int end, FSA<TValue> fsa, AstConcatNode node)
        {
            if (node is AstGroupNode)
            {
                var group = (AstGroupNode) node;
                if (group.Quantifier != null && group.Quantifier is CaptureQuantifier)
                {
                    var groupName = ((CaptureQuantifier) group.Quantifier).CaptureName;
                    if (groupName != fsa.Name)
                    {
                        //throw new NotImplementedException("Capturing");
                    }
                }
            }

            var prev = start;
            int next;
            var children = node.GetChildren().ToArray();
            for(int i = 0; i < children.Length -1;i++)
            {
                next = CreateNewState(fsa);
                Evaluate(prev, next, fsa, children[i]);
                prev = next;
            }
            next = end;
            Evaluate(prev, next, fsa, children[children.Length - 1]);
        }

        private void EvaluateAtom(int start, int end, FSA<TValue> fsa, AstAtomNode<TValue> node)
        {
            EvaluateCondition(start, end, fsa, node.Condition);
        }

        private void EvaluateCondition(int start, int end, FSA<TValue> fsa, PredicateEdgeBase<TValue> condition)
        {
            fsa.AddTransition(start, condition, end);
        }

        private void EvaluateCondition(int start, int end, FSA<TValue> fsa, AstNodeBase node)
        {
            var tmp1 = CreateNewState(fsa);
            var tmp2 = CreateNewState(fsa);

            fsa.AddEpsilonTransition(start, tmp1);
            Evaluate(tmp1, tmp2, fsa, node);
            fsa.AddEpsilonTransition(tmp2, end);
        }

        private int CreateNewState(FSA<TValue> fsa)
        {
            return fsa.NewState();
        }
    }
}
