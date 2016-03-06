using System.Collections;
using System.Collections.Generic;

namespace Eocron
{
    public sealed class OCaptureTable<TValue> : IEnumerable<KeyValuePair<string, List<OCapture<TValue>>>>
    {
        private readonly Dictionary<string, List<OCapture<TValue>>> _nameToCaptureIndex;
        private readonly List<OCapture<TValue>>[] _idToCaptureIndex;
        public OCaptureTable(string[] captureNames)
        {
            _nameToCaptureIndex = new Dictionary<string, List<OCapture<TValue>>>(captureNames.Length);
            _idToCaptureIndex = new List<OCapture<TValue>>[captureNames.Length];
            for(int i = 0; i < captureNames.Length;i++)
            {
                var name = captureNames[i];
                List<OCapture<TValue>> tmp;
                if (!_nameToCaptureIndex.TryGetValue(name, out tmp))
                {
                    tmp = new List<OCapture<TValue>>(1);
                    _nameToCaptureIndex[name] = tmp;
                }
                _idToCaptureIndex[i] = tmp;
            }
        }

        public int Count
        {
            get { return _nameToCaptureIndex.Count; }
        }

        public IEnumerable<OCapture<TValue>> this[string name]
        {
            get { return _nameToCaptureIndex[name]; }
        }

        public IEnumerable<OCapture<TValue>> this[int id]
        {
            get { return _idToCaptureIndex[id]; }
        }

        internal void Add(int captureId, OCapture<TValue> capture)
        {
            _idToCaptureIndex[captureId].Add(capture);
        }

        public IEnumerator<KeyValuePair<string, List<OCapture<TValue>>>> GetEnumerator()
        {
            return _nameToCaptureIndex.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
