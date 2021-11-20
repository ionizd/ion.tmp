using System;
using System.Linq;

namespace Ion.Extensions;

public static class StringExtensions
{
    public static bool IsNullOrEmpty(this string @str)
    {
        return string.IsNullOrEmpty(@str);
    }

    public static bool IsNotNullOrEmpty(this string @str)
    {
        return !string.IsNullOrEmpty(@str);
    }

    public static bool DoesNotContain(this string @str, params string[] values)
    {
        if (values == null || values.Length == 0) throw new InvalidOperationException(nameof(values));
        return !values.Any(@str.Contains);
    }
}