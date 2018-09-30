using Eocron.Core;
using Eocron.Core.Ast;

namespace Eocron
{
    internal sealed class OMatch<TValue> : OCapture<TValue>, IOMatch<TValue>
    {
        /// <summary>
        /// All captured groups.
        /// </summary>
        public IOCaptureTable<TValue> Captures { get; }

        internal OMatch(SequenceHandler<TValue> handler, OCaptureTable<TValue> table, Range range): base(handler, range)
        {
            Captures = table;
        }
    }
}
