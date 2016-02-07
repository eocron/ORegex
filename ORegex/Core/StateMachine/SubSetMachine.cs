using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORegex.Core.StateMachine
{
    class SubSetMachine<TValue>
    {
        public void Build(State<TValue> start)
        {
            var closure = Closure(new[] {start});

        }
        IEnumerable<State<TValue>> Closure(IEnumerable<State<TValue>> inputStates)
        {
            var output = new HashSet<State<TValue>>();
            output.UnionWith(inputStates);

            // Keeps states we are going to add later
            while (true)
            {
                var statesToAdd = new HashSet<State<TValue>>();
                foreach (var state in output)
                {
                    foreach (var edge in output.SelectMany(x=> x.Transitions))
                    {
                        if (edge.Item1 == null)
                        {
                            if (!output.Intersect(statesToAdd).Any())
                            {
                                statesToAdd.Add(edge.Item2);
                            }
                        }
                    }
                }
                if (!statesToAdd.Any())
                    break; // Exit loop if there are no states to add
                output.UnionWith(statesToAdd); // Add all states to output
            }
            return output;
        }

        IEnumerable<State<TValue>> Goto(List<State<TValue>> inputState, Func<TValue, bool> inputCharacter)
        {
            var output = new HashSet<State<TValue>>();
            foreach (var state in inputState)
            {
                foreach (var edge in state.Transitions)
                {
                    if (edge.Item1 == inputCharacter)
                    {
                        output.Add(edge.Item2);
                    }
                }
            }
            return Closure(output);
        }
    }
}
