using System;
using ORegex.Core.Ast;
using ORegex.Core.Ast.GroupQuantifiers;

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
            Evaluate(start, end, result, root, 0);
            result.AddFinal(end);
            result.AddStart(start);

            result = _preprocessor.Preprocess(result);

            return result;
        }

        public void Evaluate(int start, int end, FSA<TValue> fsa, AstNodeBase node, int classGUID)
        {
            if (node is AstAtomNode<TValue>)
            {
                EvaluateAtom(start,end, fsa,(AstAtomNode<TValue>)node, classGUID);
            }
            else if(node is AstConcatNode)
            {
                EvaluateConcat(start, end, fsa, (AstConcatNode)node, classGUID);
            }
            else if(node is AstOrNode)
            {
                EvaluateOr(start, end, fsa, (AstOrNode)node, classGUID);
            }
            else if(node is AstRepeatNode)
            {
                EvaluateRepeat(start, end, fsa, (AstRepeatNode)node, classGUID);
            }
            else if(node is AstRootNode)
            {
                EvaluateRoot(start, end, fsa, (AstRootNode)node, classGUID);
            }
            else
            {
                throw new NotImplementedException(node.GetType().Name);
            }
        }

        private void EvaluateRoot(int start, int end, FSA<TValue> fsa, AstRootNode astRootNode, int classGUID)
        {
            Evaluate(start, end, fsa, astRootNode.Regex, classGUID);
        }

        private void EvaluateRepeat(int start, int end, FSA<TValue> fsa, AstRepeatNode astRepeatNode, int classGUID)
        {
            var prev = start;
            for (int i = 0; i < astRepeatNode.MinCount; i++)
            {
                var next = CreateNewState(fsa);
                Evaluate(prev, next, fsa, astRepeatNode.Argument, classGUID);
                prev = next;
            }

            fsa.AddEpsilonTransition(prev, end);

            if (astRepeatNode.MaxCount == int.MaxValue)
            {
                RepeatZeroOrInfinite(prev, end, fsa, astRepeatNode.Argument, classGUID);
            }
            else
            {
                int count = astRepeatNode.MaxCount - astRepeatNode.MinCount;
                for (int i = 0; i < count; i++)
                {
                    var next = CreateNewState(fsa);
                    Evaluate(prev, next, fsa, astRepeatNode.Argument, classGUID);
                    fsa.AddEpsilonTransition(next, end);
                    prev = next;
                }
            }
        }

        private void RepeatZeroOrInfinite(int start, int end, FSA<TValue> fsa, AstNodeBase node, int classGUID)
        {
            var tmp = CreateNewState(fsa);
            Evaluate(tmp, tmp, fsa, node, classGUID);
            fsa.AddEpsilonTransition(start, tmp);
            fsa.AddEpsilonTransition(tmp, end);
            fsa.AddEpsilonTransition(start, end);
        }

        private void EvaluateOr(int start, int end, FSA<TValue> fsa, AstOrNode node, int classGUID)
        {
            foreach (var child in node.GetChildren())
            {
                EvaluateCondition(start, end, fsa, child, classGUID);
            }
        }

        private void EvaluateConcat(int start, int end, FSA<TValue> fsa, AstConcatNode node, int classGUID)
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

            if (groupName != null && groupName != fsa.Name)
            {
                classGUID = GenerateNewGuid(classGUID);
            }

            var prev = start;
            foreach (var child in node.GetChildren())
            {
                var next = CreateNewState(fsa);
                Evaluate(prev, next, fsa, child, classGUID);
                prev = next;
            }
            fsa.AddEpsilonTransition(prev, end);
        }

        /// <summary>
        /// TODO: Replace this shit with context.
        /// </summary>
        /// <param name="oldGuid"></param>
        /// <returns></returns>
        private int GenerateNewGuid(int oldGuid)
        {
            return new Random(oldGuid).Next();
        }

        private void EvaluateAtom(int start, int end, FSA<TValue> fsa, AstAtomNode<TValue> node, int classGUID)
        {
            EvaluateCondition(start, end, fsa, node.Condition, classGUID);
        }

        private void EvaluateCondition(int start, int end, FSA<TValue> fsa, Func<TValue, bool> condition, int classGUID)
        {
            var tmp1 = CreateNewState(fsa);
            var tmp2 = CreateNewState(fsa);

            fsa.AddEpsilonTransition(start, tmp1);
            fsa.AddTransition(tmp1, condition, tmp2, classGUID);
            fsa.AddEpsilonTransition(tmp2, end);
        }

        private void EvaluateCondition(int start, int end, FSA<TValue> fsa, AstNodeBase node, int classGUID)
        {
            var tmp1 = CreateNewState(fsa);
            var tmp2 = CreateNewState(fsa);

            fsa.AddEpsilonTransition(start, tmp1);
            Evaluate(tmp1, tmp2, fsa, node, classGUID);
            fsa.AddEpsilonTransition(tmp2, end);
        }

        public int CreateNewState(FSA<TValue> fsa)
        {
            return fsa.NewState();
        }
    }
}
