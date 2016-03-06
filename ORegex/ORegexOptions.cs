using System;

namespace Eocron
{
    [Flags]
    public enum ORegexOptions
    {
        None = 1<<0,
        /// <summary>
        /// Match pattern starting from end of sequence and through to begining.
        /// </summary>
        RightToLeft = 1<<1,
    }
}
