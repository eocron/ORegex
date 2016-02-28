using Eocron.Core.Ast;

namespace Eocron
{
    public sealed class OMatch<TValue> : OCapture<TValue>
    {
        public readonly OCaptureTable<TValue> OCaptures;

        internal OMatch(TValue[] collection, OCaptureTable<TValue> table, Range range): base(collection, range)
        {
            OCaptures = table;
        }
    }
}
