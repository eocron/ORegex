using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORegex.Core.Nsm
{
    public class FsmGraphLibrary<T>
    {
        public FsmGraph<T> CreateAtom(Func<T, bool> predicate)
        {
            var result = new FsmGraph<T>();
            result.SetStartState(0);
            result.SetFinalState(1);
            result.AddEdge(0, predicate, 1);
            return result;
        }

        public FsmGraph<T> CreateRepeat(FsmGraph<T> graph, int min, int max)
        {
            var result = new FsmGraph<T>();
            result.SetStartState(0);
            int stateId = 0;
            for (int i = 0; i < min; i++, stateId++ )
            {
                result.AddEdge(i, )
            }
            return result;
        }
    }
}
