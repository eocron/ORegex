using ORegex.Core.Ast;

namespace ORegex
{
    public sealed class ObjectMatch<TValue> : ObjectCapture<TValue>
    {
        public readonly CaptureTable<TValue> Captures;

        internal ObjectMatch(TValue[] collection, CaptureTable<TValue> table, Range range): base(collection, range)
        {
            Captures = table;
        }
    }
}
