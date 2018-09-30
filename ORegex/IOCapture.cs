using System.Collections.Generic;

namespace Eocron
{
    public interface IOCapture<out TValue> : IEnumerable<TValue>
    {
        IEnumerable<TValue> Values { get; }
        int Index { get; }
        int Length { get; }
    }
}