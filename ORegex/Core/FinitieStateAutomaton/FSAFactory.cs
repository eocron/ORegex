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
            Evaluate(start, end, result, root, false);
            result.AddFinal(end);
            result.AddStart(start);

            result = _preprocessor.Preprocess(result);

            return result;
        }

        public void Evaluate(int start, int end, FSA<TValue> fsa, AstNodeBase node, bool isLazy)
        {
            if (node is AstAtomNode<TValue>)
            {
                EvaluateAtom(start,end, fsa,(AstAtomNode<TValue>)node, isLazy);
            }
            else if(node is AstConcatNode)
            {
                EvaluateConcat(start, end, fsa, (AstConcatNode)node, isLazy);
            }
            else if(node is AstOrNode)
            {
                EvaluateOr(start, end, fsa, (AstOrNode)node, isLazy);
            }
            else if(node is AstRepeatNode)
            {
                EvaluateRepeat(start, end, fsa, (AstRepeatNode)node, isLazy);
            }
            else if(node is AstRootNode)
            {
                EvaluateRoot(start, end, fsa, (AstRootNode)node, isLazy);
            }
            else
            {
                throw new NotImplementedException(node.GetType().Name);
            }
        }

        private void EvaluateRoot(int start, int end, FSA<TValue> fsa, AstRootNode astRootNode, bool isLazy)
        {
            Evaluate(start, end, fsa, astRootNode.Regex, isLazy);
        }

        private void EvaluateRepeat(int start, int end, FSA<TValue> fsa, AstRepeatNode astRepeatNode, bool isLazy)
        {
            var prev = start;
            for (int i = 0; i < astRepeatNode.MinCount; i++)
            {
                var next = CreateNewState(fsa);
                Evaluate(prev, next, fsa, astRepeatNode.Argument, isLazy);
                prev = next;
            }

            fsa.AddEpsilonTransition(prev, end);

            if (astRepeatNode.MaxCount == int.MaxValue)
            {
                RepeatZeroOrInfinite(prev, end, fsa, astRepeatNode.Argument, isLazy);
            }
            else
            {
                int count = astRepeatNode.MaxCount - astRepeatNode.MinCount;
                for (int i = 0; i < count; i++)
                {
                    var next = CreateNewState(fsa);
                    Evaluate(prev, next, fsa, astRepeatNode.Argument, isLazy);
                    fsa.AddEpsilonTransition(next, end);
                    prev = next;
                }
            }
        }

        private void RepeatZeroOrInfinite(int start, int end, FSA<TValue> fsa, AstNodeBase node, bool isLazy)
        {
            var tmp = CreateNewState(fsa);
            Evaluate(tmp, tmp, fsa, node, isLazy);
            fsa.AddEpsilonTransition(start, tmp);
            fsa.AddEpsilonTransition(tmp, end);
            fsa.AddEpsilonTransition(start, end);
        }

        private void EvaluateOr(int start, int end, FSA<TValue> fsa, AstOrNode node, bool isLazy)
        {
            foreach (var child in node.GetChildren())
            {
                EvaluateCondition(start, end, fsa, child, isLazy);
            }
        }

        private void EvaluateConcat(int start, int end, FSA<TValue> fsa, AstConcatNode node, bool isLazy)
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
                        var edge = new ComplexPredicateEdge<TValue>(captureFsa);
                        edge.IsLazy = isLazy;
                        EvaluateCondition(start, end, fsa, edge);
                        return;
                    }
                }
            }

            var prev = start;
            foreach (var child in node.GetChildren())
            {
                var next = CreateNewState(fsa);
                Evaluate(prev, next, fsa, child, isLazy);
                prev = next;
            }
            fsa.AddEpsilonTransition(prev, end);
        }

        private void EvaluateAtom(int start, int end, FSA<TValue> fsa, AstAtomNode<TValue> node, bool isLazy)
        {
            node.Condition.IsLazy = isLazy;
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

        private void EvaluateCondition(int start, int end, FSA<TValue> fsa, AstNodeBase node, bool isLazy)
        {
            var tmp1 = CreateNewState(fsa);
            var tmp2 = CreateNewState(fsa);

            fsa.AddEpsilonTransition(start, tmp1);
            Evaluate(tmp1, tmp2, fsa, node, isLazy);
            fsa.AddEpsilonTransition(tmp2, end);
        }

        public int CreateNewState(FSA<TValue> fsa)
        {
            return fsa.NewState();
        }
    }
}
