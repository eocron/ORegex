using System;

namespace Eocron
{
    [Flags]
    public enum EocronOptions
    {
        None = 0,
        /// <summary>
        /// When set ignores redundant predicates and not throwing exceptions if it exists.
        /// Useful property to debug pattern before finalization of concept.
        /// </summary>
        IgnoreRedundantPredicates = 1
    }
}
