using System;
using System.Collections.Generic;
using System.Linq;
using Csv.Attributes;

namespace Csv.Extensions;

public static class ObjectExtensions
{
    public static IEnumerable<string> GetDescriptions(this object value)
    {
        if (!value.GetType().IsEnum) throw new ArgumentException("Argument must be enum type", nameof(value));
        
        var fi = value.GetType().GetField(value.ToString());

        var attributes = (CsvEnumAttribute[])fi?.GetCustomAttributes(
            typeof(CsvEnumAttribute),
            false);

        return attributes?.Select(x => x.Name) ?? Array.Empty<string>();
    }
}
