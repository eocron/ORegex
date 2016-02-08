using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORegex.Core.StateMachine
{
    public sealed class IdGenerator
    {
        public int Count
        {
            get { return _map.Count; }
        }
        private readonly Dictionary<object, int> _map = new Dictionary<object, int>();

        public int GetId(object obj)
        {
            int id;
            if (!_map.TryGetValue(obj, out id))
            {
                id = _map.Count;
                _map[obj] = id;
            }
            return id;
        }
    }
}
