using System;

namespace Eocron.Core.Objects
{
    [Flags]
    public enum ComparisonOptions
    {
        None = 0,
        CheckArraysOrder = 1,
        CheckCollectionsOrder = 2
    }
}