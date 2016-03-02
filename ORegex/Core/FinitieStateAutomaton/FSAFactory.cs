using System;
using Eocron.Core.Ast;
using Eocron.Core.Ast.GroupQuantifiers;
using Eocron.Core.FinitieStateAutomaton.Predicates;

namespace Eocron.Core.FinitieStateAutomaton
{
    public sealed class FSAFactory<TValue>
    {
        public const string RepeatFsaName = "#repeat";
        public const string OrFsaName = "#or";

        private readonly FSAPreprocessor<TValue> _preprocessor = new FSAPreprocessor<TValue>(); 
        public FSA<TValue> Create(AstNodeBase root, string name)
        {
            var result = new FSA<TValue>(name);
            var start = result.NewState();
            var end = result.NewState();
            Evaluate(start, end, result, root);
            result.AddFinal(end);
            result.AddStart(start);

            result = _preprocessor.Preprocess(result);

            return result;
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
                EvaluateCondition(prev, next, fsa, toRepeat);
                prev = next;
            }

            fsa.AddEpsilonTransition(prev, end);

            if (astRepeatNode.MaxCount == int.MaxValue)
            {
                RepeatZeroOrInfinite(prev, end, fsa, toRepeat);
            }
            else
            {
                int count = astRepeatNode.MaxCount - astRepeatNode.MinCount;
                for (int i = 0; i < count; i++)
                {
                    var next = CreateNewState(fsa);
                    EvaluateCondition(prev, next, fsa, toRepeat);
                    fsa.AddEpsilonTransition(next, end);
                    prev = next;
                }
            }
        }

        private void RepeatZeroOrInfinite(int start, int end, FSA<TValue> fsa, AstNodeBase predicate)
        {
            var tmp = CreateNewState(fsa);
            Evaluate(tmp, tmp, fsa, predicate);
            fsa.AddEpsilonTransition(start, tmp);
            fsa.AddEpsilonTransition(tmp, end);
            fsa.AddEpsilonTransition(start, end);
        }

        private void EvaluateOr(int start, int end, FSA<TValue> fsa, AstOrNode node)
        {
            foreach (var child in node.GetChildren())
            {
                EvaluateCondition(start, end, fsa, child);
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
            foreach (var child in node.GetChildren())
            {
                var next = CreateNewState(fsa);
                Evaluate(prev, next, fsa, child);
                prev = next;
            }
            fsa.AddEpsilonTransition(prev, end);
        }

        private void EvaluateAtom(int start, int end, FSA<TValue> fsa, AstAtomNode<TValue> node)
        {
            EvaluateCondition(start, end, fsa, node.Condition);
        }

        private void EvaluateCondition(int start, int end, FSA<TValue> fsa, PredicateEdgeBase<TValue> condition)
        {
            var tmp1 = CreateNewState(fsa);
            var tmp2 = CreateNewState(fsa);

            fsa.AddEpsilonTransition(start, tmp1);
            fsa.AddTransition(tmp1, condition, tmp2);
            fsa.AddEpsilonTransition(tmp2, end);
        }

        private void EvaluateCondition(int start, int end, FSA<TValue> fsa, AstNodeBase node)
        {
            var tmp1 = CreateNewState(fsa);
            var tmp2 = CreateNewState(fsa);

            fsa.AddEpsilonTransition(start, tmp1);
            Evaluate(tmp1, tmp2, fsa, node);
            fsa.AddEpsilonTransition(tmp2, end);
        }

        public int CreateNewState(FSA<TValue> fsa)
        {
            return fsa.NewState();
        }
    }
}
