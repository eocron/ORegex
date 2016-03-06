namespace Eocron.Core
{
    internal sealed class FixedSizeStack<TValue>
    {
        private readonly TValue[] _stack;
        private int _count;

        public int Count
        {
            get { return _count; }
        }

        public TValue this[int index]
        {
            get { return _stack[_count - index - 1]; }
        }
        public FixedSizeStack(int size)
        {
            _stack = new TValue[size];
        }

        public void Push(TValue value)
        {
            _stack[_count++] = value;
        }

        public TValue Pop()
        {
            return _stack[--_count];
        }

        public TValue Peek()
        {
            return _stack[_count - 1];
        }

        public void Clear()
        {
            _count = 0;
        }
    }
}
