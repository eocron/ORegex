using System.Collections;
using System.Collections.Generic;

namespace Eocron
{
    public sealed class OCaptureTable<TValue> : IOCaptureTable<TValue>
    {
        private readonly Dictionary<string, IList<IOCapture<TValue>>> _nameToCaptureIndex;
        private readonly IList<IOCapture<TValue>>[] _idToCaptureIndex;
        public OCaptureTable(string[] captureNames)
        {
            _nameToCaptureIndex = new Dictionary<string, IList<IOCapture<TValue>>>(captureNames.Length);
            _idToCaptureIndex = new IList<IOCapture<TValue>>[captureNames.Length];
            for(int i = 0; i < captureNames.Length;i++)
            {
                var name = captureNames[i];
                IList<IOCapture<TValue>> tmp;
                if (!_nameToCaptureIndex.TryGetValue(name, out tmp))
                {
                    tmp = new List<IOCapture<TValue>>(1);
                    _nameToCaptureIndex[name] = tmp;
                }
                _idToCaptureIndex[i] = tmp;
            }
        }

        public int Count => _nameToCaptureIndex.Count;

        public IEnumerable<IOCapture<TValue>> this[string name] => _nameToCaptureIndex[name];

        public IEnumerable<IOCapture<TValue>> this[int id] => _idToCaptureIndex[id];

        internal void Add(int captureId, OCapture<TValue> capture)
        {
            _idToCaptureIndex[captureId].Add(capture);
        }

        public IEnumerator<KeyValuePair<string, IList<IOCapture<TValue>>>> GetEnumerator()
        {
            return _nameToCaptureIndex.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
