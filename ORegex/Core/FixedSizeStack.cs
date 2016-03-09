using System;

namespace Eocron.Core
{
    internal sealed class FixedSizeStack<TValue>
    {
        private TValue[] _stack;
        private int _count;
        private readonly int _growthFactor;

        public int Count
        {
            get { return _count; }
        }

        public TValue this[int index]
        {
            get { return _stack[index]; }
        }
        public FixedSizeStack(int size, int growthFactor = 2)
        {
            _growthFactor = growthFactor;
            _stack = new TValue[size];
        }

        public void Push(TValue value)
        {
            if (_count == _stack.Length)
            {
                Array.Resize(ref _stack, _count*_growthFactor);
            }
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
