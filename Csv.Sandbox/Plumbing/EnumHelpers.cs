using System;
using System.Collections.Generic;
using System.Linq;
using Csv.Extensions;

namespace Csv.Plumbing;

public static class EnumHelpers
{
    public static IDictionary<string, object> GetDescriptionMap(Type enumType) =>
        Enum.GetValues(enumType)
            .Cast<object>()
            .SelectMany(GetDescriptions)
            .ToTypedDictionary();

    private static IEnumerable<(TResult Value, string Description)> GetDescriptions<TResult>(TResult value) => value
        .GetDescriptions()
        .Select(description => (value, description));

    private static IDictionary<string, TResult> ToTypedDictionary<TResult>(
        this IEnumerable<(TResult Value, string Description)> input) => input.ToDictionary(
        x => x.Description,
        x => x.Value,
        StringComparer.OrdinalIgnoreCase);
}
