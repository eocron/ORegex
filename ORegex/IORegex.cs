using System;
using System.Collections.Generic;

namespace Eocron
{
    public interface IORegex<TValue>
    {
        ORegexOptions Options { get; }

        string Pattern { get; }

        /// <summary>
        /// Tries to match pattern starting from startIndex position.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        IOMatchCollection<TValue> Matches(IList<TValue> values, int startIndex = -1);

        /// <summary>
        /// Tries to match pattern starting from startIndex position.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        IOMatch<TValue> Match(IList<TValue> values, int startIndex = -1);

        /// <summary>
        /// Tries to match pattern starting from startIndex position.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        bool IsMatch(IList<TValue> values, int startIndex = -1);

        /// <summary>
        /// Replaces mathes in sequence by given replacProvider.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="replaceProvider">Provides subSequence replacement.</param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        List<TValue> Replace(IList<TValue> values, Func<IOMatch<TValue>, IEnumerable<TValue>> replaceProvider, int startIndex = -1);
    }
}