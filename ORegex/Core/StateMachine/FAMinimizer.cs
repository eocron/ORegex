using System.Linq;

namespace ORegex.Core.StateMachine
{
    public sealed class FAMinimizer<TValue>
    {
        public static FA<TValue> Minimize(FA<TValue> dfa)
        {
            var reversedNDFSM = Reverse(dfa);
            var reversedDFSM = FASubsetConverter<TValue>.NfaToDfa(reversedNDFSM);
            var NDFSM = Reverse(reversedDFSM);
            var DFA = FASubsetConverter<TValue>.NfaToDfa(NDFSM);
            return DFA;
        }
        private static FA<TValue> Reverse(FA<TValue> dfa)
        {
            return
                new FA<TValue>(dfa.Name, dfa.Transitions.Select(x => new FATrans<TValue>(x.EndState, x.Condition, x.StartState)),
                    dfa.F, dfa.Q0);
        }
    }
}
