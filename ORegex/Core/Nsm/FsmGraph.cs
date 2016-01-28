using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORegex.Core.Nsm
{
    public sealed class FsmGraph<T>
    {
        public readonly List<Edge<T>> Edges = new List<Edge<T>>();

        public readonly HashSet<int> FinalStates = new HashSet<int>();

        public int StartState = 0;

        public void AddEdge(int from, Func<T, bool> predicate, int to)
        {
            var anySame = Edges.Any(x => x.From == from && x.Predicate == predicate);
            if(anySame)
            {
                throw new InvalidOperationException("Such predicate already exist for this state.");
            }

            Edges.Add(new Edge<T>() { From = from, Predicate = predicate, To = to });
        }

        public void SetStartState(int state)
        {
            StartState = state;
        }

        public void SetFinalState(int state)
        {
            FinalStates.Add(state);
        }

        public static FsmGraph<T> RedirectTo(FsmGraph<T> source, FsmGraph<T> destination)
        {
            var shiftState = source.Edges.SelectMany(x => new int[] { x.From, x.To }).Max() + 1;
            var result = new FsmGraph<T>();

            foreach (var s in destination.Edges)
            {
                result.AddEdge(s.From + shiftState, s.Predicate, s.To + shiftState);
            }

            foreach(var s in source.Edges)
            {
                result.AddEdge(s.From, s.Predicate, s.To);
            }

            result.SetStartState(source.StartState);

            foreach(var s in destination.FinalStates)
            {
                result.SetFinalState(s + shiftState);
            }

            foreach(var s in source.FinalStates)
            {
                result.AddEdge(s, null, destination.StartState+shiftState);
            }

            return result;
        }


    }
}
