﻿using System;

namespace ORegex.Core.FinitieStateAutomaton
{
    public sealed class PredicateConst<TValue>
    {
        public static readonly Func<TValue, bool> Epsilon = x => { throw new NotImplementedException("epsilon condition.");};

        public static readonly Func<TValue, bool> AlwaysTrue = x => true;
    }
}