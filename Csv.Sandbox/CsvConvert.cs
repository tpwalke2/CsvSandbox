using System;
using System.Collections.Generic;
using System.Linq;
using Csv.Extensions;
using Csv.Plumbing.Reflection;

namespace Csv
{
    public static class CsvConvert
    {
        public static T DeserializeObject<T>(string input, CsvConvertSettings settings = null) where T : new()
        {
            return DeserializeObjects<T>(input, settings)
                .FirstOrDefault();
        }

        public static IEnumerable<T> DeserializeObjects<T>(string input, CsvConvertSettings settings = null) where T : new()
        {
            settings = settings ?? new CsvConvertSettings();
            
            var parsedRows = Parser.Parser.Parse(input);
            if (parsedRows.Count <= 1) return new List<T>();

            var headers = parsedRows[0];
            var accessors = typeof(T).GetAccessors();

            return Enumerable.Range(1, parsedRows.Count - 1)
                             .Select(rowIndex => new {rowIndex, row = parsedRows[rowIndex]})
                             .Select(rowTuple => DeserializeObject<T>(
                                         headers,
                                         accessors,
                                         rowTuple.row,
                                         rowTuple.rowIndex,
                                         settings.OnError));
        }

        private static T DeserializeObject<T>(
            IList<string> headers,
            IDictionary<string, ValueAccessor> accessors,
            IList<string> currentRow,
            int rowIndex, 
            Action<string> onError = null) where T : new()
        {
            var currentResult = new T();

            for (var j = 0; j < headers.Count; j++)
            {
                if (!accessors.ContainsKey(headers[j])) continue;
                var accessor = accessors[headers[j]];

                try
                {
                    accessor.Value[currentResult] = accessor.Type.Convert(currentRow[j]);
                } catch (ArgumentException)
                {
                    var errorMessage = $"Invalid format in record '{rowIndex}' and field {headers[j]}";
                    if (onError == null) throw new FormatException(errorMessage);
                    onError(errorMessage);
                }
            }

            return currentResult;
        }
    }
}
