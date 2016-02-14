//using System.Collections;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text.RegularExpressions;

//namespace ORegex
//{
//    [DebuggerDisplay("Count = {Count}")]
//    public sealed class ObjectGroupCollection<TValue> : IEnumerable
//    {
//        private readonly ObjectGroup<TValue>[] _groups;
//        private readonly Dictionary<string, ObjectGroup<TValue>> _nameToGroup;

//        internal ObjectGroupCollection(Regex pattern, Match match, ObjectGroup<TValue> parent, TValue[] collection, int codeLength)
//        {
//            List<ObjectGroup<TValue>> tmp = new List<ObjectGroup<TValue>>();
//            tmp.Add(parent);
//            tmp.AddRange(match.Groups.Cast<Group>().Skip(1).Select(x => new ObjectGroup<TValue>(x, collection, codeLength)));
//            _groups = tmp.ToArray();

//            var names = pattern.GetGroupNames();
//            var nums = pattern.GetGroupNumbers();
//            _nameToGroup = new Dictionary<string, ObjectGroup<TValue>>();
//            for(int i = 0; i < names.Length;i++)
//            {
//                _nameToGroup[names[i]] = _groups[nums[i]];
//            }
//        }

//        public int Count
//        {
//            get
//            {
//                return _groups.Length;
//            }
//        }

//        public ObjectGroup<TValue> this[int i]
//        {
//            get
//            {
//                return _groups[i];
//            }
//        }

//        public ObjectGroup<TValue> this[string name]
//        {
//            get
//            {
//                return _nameToGroup[name];
//            }
//        }

//        public IEnumerator GetEnumerator()
//        {
//            return _groups.GetEnumerator();
//        }
//    }
//}
