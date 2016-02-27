using System;

namespace Eocron
{
    /// <summary>
    /// Responsible for object mapping to string for Regex engine to process it.
    /// </summary>
    internal sealed class RegexObjectMapper
    {
#if DEBUG
        private const char LeftUnicodeBorder = 'a';
        private const char RightUnicodeBorder = 'z';
#else
        private const char LeftUnicodeBorder = '\u00AF';
        private const char RightUnicodeBorder = '\uFFFA';
#endif
        private const int MaxCharStateCount = RightUnicodeBorder - LeftUnicodeBorder + 1;

        public readonly int CodeLength;
        private readonly char[,] _mapTable;
        internal RegexObjectMapper(int maxState)
        {
            int count = maxState+1;
            CodeLength = (int)Math.Ceiling(Math.Log(count, MaxCharStateCount));
            _mapTable = new char[count, CodeLength];

            for (int i = 0; i < count; i++)
            {
                int n = i;
                for (int j = 0; j < CodeLength; j++)
                {
                    char c = (char)(n % MaxCharStateCount + LeftUnicodeBorder);
                    n = n / MaxCharStateCount;
                    _mapTable[i, j] = c;
                }
            }
        }

        public string GetStateString(int[] states)
        {
            char[] result = new char[CodeLength * states.Length];
            for(int i =0;i<states.Length;i++)
            {
                var state = states[i];
                if (state == -1)
                {
                    for (int j = 0; j < CodeLength; j++)
                    {
                        result[CodeLength * i + j] = char.MaxValue;
                    }
                }
                else
                {
                    for (int j = 0; j < CodeLength; j++)
                    {
                        result[CodeLength * i + j] = _mapTable[state, j];
                    }
                }
            }
            return new string(result);
        }

        public string GetStateString(int state)
        {
            char[] result = new char[CodeLength];
            for (int j = 0; j < CodeLength; j++)
            {
                result[j] = _mapTable[state, j];
            }
            return new string(result);
        }

        public bool IsExist(int state)
        {
            return state < _mapTable.GetLength(0) && state >= 0;
        }

        public int ConvertIndex(int index)
        {
            return index * CodeLength;
        }
    }
}
