using System;
using Csv.Plumbing;

namespace Csv.Extensions;

public static class StringExtensions
{
    public static object AsEnum(this string s, Type enumType, object defaultValue = default) =>
        Enum.TryParse(enumType, s, true, out var result)
            ? result
            : EnumHelpers.GetDescriptionMap(enumType)
                         .TryGetOrFallbackTo(s, defaultValue);
}
