using System.Collections.Generic;

namespace Eocron
{
    public interface IOCaptureTable<TValue> : IEnumerable<KeyValuePair<string, IList<IOCapture<TValue>>>>
    {
        IEnumerable<IOCapture<TValue>> this[string name] { get; }
        IEnumerable<IOCapture<TValue>> this[int id] { get; }
        int Count { get; }
    }
}