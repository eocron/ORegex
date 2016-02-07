using System;
using ORegex.Core.Ast;

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
                var next = new State<TValue>();
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
                    var next = new State<TValue>();
                    Evaluate(prev, next, astRepeatNode.Argument);
                    next.AddEpsilonTransition(end);
                    prev = next;
                }
            }
        }

        private void RepeatZeroOrInfinite(State<TValue> start, State<TValue> end, AstNodeBase node)
        {
            var tmp = new State<TValue>();
            Evaluate(tmp,tmp, node);
            start.AddEpsilonTransition(tmp);
            tmp.AddEpsilonTransition(end);
            start.AddEpsilonTransition(end);
        }

        private void EvaluateOr(State<TValue> start, State<TValue> end, AstOrNode node)
        {
            foreach (var child in node.GetChildren())
            {
                Evaluate(start,end, child);
            }
        }

        private void EvaluateConcat(State<TValue> start, State<TValue> end, AstConcatNode node)
        {
            var prev = start;
            foreach (var child in node.GetChildren())
            {
                var next = new State<TValue>();
                Evaluate(prev, next, child);
                prev = next;
            }
            prev.AddEpsilonTransition(end);

            if (node is AstGroupNode)
            {
                //throw new NotImplementedException("Group quantifier logic.");
            }
        }

        private void EvaluateAtom(State<TValue> start, State<TValue> end, AstAtomNode<TValue> node)
        {
            start.AddTransition(node.Condition, end);
        }
    }
}
