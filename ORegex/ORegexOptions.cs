using System;

namespace Eocron
{
    [Flags]
    public enum ORegexOptions
    {
        /// <summary>
        /// Nothing.
        /// </summary>
        None = 1<<0,

        ///// <summary>
        ///// Reverse oregex pattern so AB? in fact becomes B?A
        ///// </summary>
        //ReversePattern = 1<<1,

        /// <summary>
        /// Reverse sequence run so it is from N to 1 instead of 1 to N.
        /// </summary>
        ReverseSequence = 1<<2,
    }
}
