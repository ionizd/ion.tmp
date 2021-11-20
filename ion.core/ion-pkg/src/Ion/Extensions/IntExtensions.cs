using System;

namespace Ion.Extensions;

public static class IntExtensions
{
    public static TimeSpan Seconds(this uint @int)
    {
        return TimeSpan.FromSeconds(@int);
    }

    public static TimeSpan MilliSeconds(this uint @int)
    {
        return TimeSpan.FromMilliseconds(@int);
    }
}