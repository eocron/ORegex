//using System.Diagnostics;
//using System.Text.RegularExpressions;

//namespace ORegex
//{
//    [DebuggerDisplay("index={Index}, length={Length};")]
//    public sealed class ObjectMatch<TValue> : ObjectGroup<TValue>
//    {
//        public readonly ObjectGroupCollection<TValue> Groups;

//        internal ObjectMatch(Regex pattern, Match match, TValue[] collection, int codeLength)
//            : base(match, collection)
//        {

//            Groups = new ObjectGroupCollection<TValue>(pattern, match, this, collection, codeLength);
//        }
//    }
//}
