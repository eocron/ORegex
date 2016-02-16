using System.Collections.Generic;

namespace ORegex.Core.FinitieStateAutomaton
{
    public sealed class CFSAContext<TValue>
    {
        public readonly Dictionary<string, List<ObjectCapture<TValue>>> _nameToCaptures;

        public readonly List<ObjectCapture<TValue>>[] _captures;

        public readonly TValue[] Sequence;

        public CFSAContext(CFSA<TValue> cfsa, TValue[] collection)
        {
            var captureGroupNames = cfsa.CaptureGroupNames;
            Sequence = collection;
            _nameToCaptures = new Dictionary<string, List<ObjectCapture<TValue>>>(captureGroupNames.Length * 2);
            _captures = new List<ObjectCapture<TValue>>[captureGroupNames.Length];

            for (int i = 0; i < captureGroupNames.Length; i++)
            {
                List<ObjectCapture<TValue>> tmp;
                if (!_nameToCaptures.TryGetValue(captureGroupNames[i], out tmp))
                {
                    tmp = new List<ObjectCapture<TValue>>();
                }

                _captures[i] = tmp;
                _nameToCaptures[i.ToString()] = tmp;
                _nameToCaptures[captureGroupNames[i]] = tmp;
            }
        }

        public void AddCapture(string name, ObjectCapture<TValue> capture)
        {
            _nameToCaptures[name].Add(capture);
        }

        public void AddCapture(int index, ObjectCapture<TValue> capture)
        {
            _captures[index].Add(capture);
        }
        
    }
}
