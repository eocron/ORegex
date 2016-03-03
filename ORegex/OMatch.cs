using Eocron.Core.Ast;

namespace Eocron
{
    public sealed class OMatch<TValue> : OCapture<TValue>
    {
        public readonly OCaptureTable<TValue> Captures;

        internal OMatch(TValue[] collection, OCaptureTable<TValue> table, Range range): base(collection, range)
        {
            Captures = table;
        }
    }
}
