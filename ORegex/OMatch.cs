using Eocron.Core.Ast;

namespace Eocron
{
    public sealed class OMatch<TValue> : OCapture<TValue>
    {
        public readonly CaptureTable<TValue> Captures;

        internal OMatch(TValue[] collection, CaptureTable<TValue> table, Range range): base(collection, range)
        {
            Captures = table;
        }
    }
}
