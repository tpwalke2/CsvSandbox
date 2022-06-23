using System.Collections.Generic;

namespace Csv.Extensions;

public static class DictionaryExtensions
{
    public static TValue TryGetOrFallbackTo<TKey, TValue>(
        this IDictionary<TKey, TValue> input,
        TKey key,
        TValue fallbackValue = default)
    {
        if (key == null) return fallbackValue;

        return input.TryGetValue(key, out var result) ? result : fallbackValue;
    }
}
