using Eocron.Core.Ast;

namespace Eocron
{
    public sealed class OMatch<TValue> : OCapture<TValue>
    {
        /// <summary>
        /// All captured groups.
        /// </summary>
        public readonly OCaptureTable<TValue> Captures;

        internal OMatch(TValue[] collection, OCaptureTable<TValue> table, Range range): base(collection, range)
        {
            Captures = table;
        }
    }
}
