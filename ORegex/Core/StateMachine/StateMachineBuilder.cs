using System;
using System.Text.RegularExpressions;
using ORegex.Core.Ast;
using ORegex.Core.Ast.GroupQuantifiers;

namespace ORegex.Core.StateMachine
{
    public sealed class StateMachineBuilder<TValue>
    {
        public void Evaluate(State<TValue> start, State<TValue> end, AstNodeBase node)
        {
            if (node is AstAtomNode<TValue>)
            {
                EvaluateAtom(start,end,(AstAtomNode<TValue>)node);
            }
            else if(node is AstConcatNode)
            {
                EvaluateConcat(start, end, (AstConcatNode) node);
            }
            else if(node is AstOrNode)
            {
                EvaluateOr(start,end,(AstOrNode)node);
            }
            else if(node is AstRepeatNode)
            {
                EvaluateRepeat(start,end, (AstRepeatNode)node);
            }
            else if(node is AstRootNode)
            {
                EvaluateRoot(start, end, (AstRootNode) node);
            }
            else
            {
                throw new NotImplementedException(node.GetType().Name);
            }
        }

        private void EvaluateRoot(State<TValue> start, State<TValue> end, AstRootNode astRootNode)
        {
            Evaluate(start,end,astRootNode.Regex);
        }

        private void EvaluateRepeat(State<TValue> start, State<TValue> end, AstRepeatNode astRepeatNode)
        {
            var prev = start;
            for (int i = 0; i < astRepeatNode.MinCount; i++)
            {
                var next = CreateNewState();
                Evaluate(prev, next, astRepeatNode.Argument);
                prev = next;
            }

            prev.AddEpsilonTransition(end);

            if (astRepeatNode.MaxCount == int.MaxValue)
            {
                RepeatZeroOrInfinite(prev, end, astRepeatNode.Argument);
            }
            else
            {
                int count = astRepeatNode.MaxCount - astRepeatNode.MinCount;
                for (int i = 0; i < count; i++)
                {
                    var next = CreateNewState();
                    Evaluate(prev, next, astRepeatNode.Argument);
                    next.AddEpsilonTransition(end);
                    prev = next;
                }
            }
        }

        private void RepeatZeroOrInfinite(State<TValue> start, State<TValue> end, AstNodeBase node)
        {
            var tmp = CreateNewState();
            Evaluate(tmp,tmp, node);
            start.AddEpsilonTransition(tmp);
            tmp.AddEpsilonTransition(end);
            start.AddEpsilonTransition(end);
        }

        private void EvaluateOr(State<TValue> start, State<TValue> end, AstOrNode node)
        {
            foreach (var child in node.GetChildren())
            {
                EvaluateCondition(start, end, child);
            }
        }

        private void EvaluateConcat(State<TValue> start, State<TValue> end, AstConcatNode node)
        {
            string groupName = null;
            if (node is AstGroupNode)
            {
                var group = (AstGroupNode) node;
                if (group.Quantifier != null && group.Quantifier is CaptureQuantifier)
                {
                    var captureQ = (CaptureQuantifier) group.Quantifier;
                    groupName = captureQ.CaptureName;
                }
            }

            if (groupName != null)
            {
                //throw new NotImplementedException();
            }
            var prev = start;
            foreach (var child in node.GetChildren())
            {
                var next = CreateNewState();
                Evaluate(prev, next, child);
                prev = next;
            }
            prev.AddEpsilonTransition(end);
        }

        private void EvaluateAtom(State<TValue> start, State<TValue> end, AstAtomNode<TValue> node)
        {
            EvaluateCondition(start, end, node.Condition);
        }

        private void EvaluateCondition(State<TValue> a, State<TValue> b, Func<TValue, bool> condition)
        {
            var tmp1 = CreateNewState();
            var tmp2 = CreateNewState();

            a.AddEpsilonTransition(tmp1);
            tmp1.AddTransition(condition, tmp2);
            tmp2.AddEpsilonTransition(b);
        }

        private void EvaluateCondition(State<TValue> a, State<TValue> b, AstNodeBase node)
        {
            var tmp1 = CreateNewState();
            var tmp2 = CreateNewState();

            a.AddEpsilonTransition(tmp1);
            Evaluate(tmp1, tmp2, node);
            tmp2.AddEpsilonTransition(b);
        }

        public State<TValue> CreateNewState()
        {
            return new State<TValue>();
        }
    }
}
