using System;
using state = System.Int32;
namespace ORegex.Core.StateMachine
{
    public class Edge<TValue>
    {
        public state StartState;

        public state EndState;

        public Func<TValue, bool> Condition;
    }
}
