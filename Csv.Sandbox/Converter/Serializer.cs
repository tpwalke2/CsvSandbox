using System;
using System.Collections.Generic;
using System.Linq;
using Csv.Extensions;

namespace Csv.Converter
{
    internal static class Serializer
    {
        public static string Serialize<T>(T input, CsvConvertSettings settings = null)
        {
            return input == null
                ? ""
                : SerializeList(new[] {input}, settings);
        }

        public static string SerializeList<T>(IEnumerable<T> input, CsvConvertSettings settings = null)
        {
            if (input == null) return "";
            
            settings ??= new CsvConvertSettings();

            var accessors = typeof(T)
                            .GetAccessors()
                            .ToList();
            
            var lines = new List<string>
            {
                string.Join(settings.Separator, accessors.Select(x => x.Name))
            };

            lines.AddRange(input
                               .Select(item => string.Join(
                                           settings.Separator,
                                           accessors
                                               .Select(x => SanitizeOutput(
                                                           x.Value[item].ToString(),
                                                           settings.Separator)))));

            return lines.Count == 1 
                ? ""
                : string.Join(Environment.NewLine, lines);
        }

        private static string SanitizeOutput(string input, char separator)
        {
            return input.IndexOfAny(new[] {' ', separator}) == -1
                ? input
                : $"\"{input}\"";
        }    
    }
}
