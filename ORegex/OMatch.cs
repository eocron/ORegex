using Eocron.Core;
using Eocron.Core.Ast;

namespace Eocron
{
    public sealed class OMatch<TValue> : OCapture<TValue>
    {
        /// <summary>
        /// All captured groups.
        /// </summary>
        public readonly OCaptureTable<TValue> Captures;

        internal OMatch(SequenceHandler<TValue> handler, OCaptureTable<TValue> table, Range range): base(handler, range)
        {
            Captures = table;
        }
    }
}
