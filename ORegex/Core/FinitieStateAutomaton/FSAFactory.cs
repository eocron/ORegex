using System;
using ORegex.Core.Ast;
using ORegex.Core.Ast.GroupQuantifiers;
using ORegex.Core.FinitieStateAutomaton.Predicates;

namespace ORegex.Core.FinitieStateAutomaton
{
    public sealed class FSAFactory<TValue>
    {
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
                EvaluateAtom(start,end, fsa,(AstAtomNode<TValue>)node);
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
            Evaluate(start, end, fsa, astRootNode.Regex);
        }

        private void EvaluateRepeat(int start, int end, FSA<TValue> fsa, AstRepeatNode astRepeatNode)
        {
            var prev = start;
            for (int i = 0; i < astRepeatNode.MinCount; i++)
            {
                var next = CreateNewState(fsa);
                Evaluate(prev, next, fsa, astRepeatNode.Argument);
                prev = next;
            }

            fsa.AddEpsilonTransition(prev, end);

            if (astRepeatNode.MaxCount == int.MaxValue)
            {
                RepeatZeroOrInfinite(prev, end, fsa, astRepeatNode.Argument);
            }
            else
            {
                int count = astRepeatNode.MaxCount - astRepeatNode.MinCount;
                for (int i = 0; i < count; i++)
                {
                    var next = CreateNewState(fsa);
                    Evaluate(prev, next, fsa, astRepeatNode.Argument);
                    fsa.AddEpsilonTransition(next, end);
                    prev = next;
                }
            }
        }

        private void RepeatZeroOrInfinite(int start, int end, FSA<TValue> fsa, AstNodeBase node)
        {
            var tmp = CreateNewState(fsa);
            Evaluate(tmp, tmp, fsa, node);
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
                        var captureFsa = Create(group, groupName);
                        EvaluateCondition(start, end, fsa, new ComplexPredicateEdge<TValue>(captureFsa));
                        return;
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
