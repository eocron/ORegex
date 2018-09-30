using System.Collections.Generic;

namespace Eocron
{
    public interface IOMatchCollection<TValue> : IReadOnlyCollection<IOMatch<TValue>>
    {
    }
}