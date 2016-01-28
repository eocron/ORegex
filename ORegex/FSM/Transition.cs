using System;

namespace ORegex.FSM
{
    public class Transition<TValue>
    {
        public string StartState { get; private set; }
        public Func<TValue, bool> Symbol { get; private set; }
        public string EndState { get; private set; }

        public Transition(string startState, Func<TValue, bool> symbol, string endState)
        {
            StartState = startState;
            Symbol = symbol;
            EndState = endState;
        }

        public override string ToString()
        {
            return string.Format("({0}, {1}) -> {2}\n", StartState, Symbol, EndState);
        }
    }
}

