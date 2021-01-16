using System;
using System.Collections.Generic;
using System.Linq;
using Csv.Extensions;
using Csv.Parser;
using Csv.Plumbing.Reflection;

namespace Csv.Converter
{
    internal static class Deserializer
    {
        public static T Deserialize<T>(string input, CsvConvertSettings settings = null) where T : new()
        {
            return DeserializeList<T>(input, settings)
                .FirstOrDefault();
        }

        public static IEnumerable<T> DeserializeList<T>(string input, CsvConvertSettings settings = null)
            where T : new()
        {
            settings ??= new CsvConvertSettings();

            var parsedRows = Parser.Parser.Parse(input, new Settings
            {
                Separator = settings.Separator
            });
            if (parsedRows.Count <= 1) return new List<T>();

            var headers = parsedRows[0];
            var accessors = typeof(T)
                .GetAccessors()
                .ToDictionary(
                    gs => gs.Name,
                    gs => gs);

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
            return Enumerable.Range(0, headers.Count)
                             .Where(columnIndex => accessors.ContainsKey(headers[columnIndex]))
                             .Select(columnIndex => new ConvertContext(columnIndex, accessors[headers[columnIndex]]))
                             .Aggregate(
                                 new T(),
                                 (current, next) => ApplyFieldValue(
                                     headers[next.ColumnIndex],
                                     currentRow,
                                     rowIndex,
                                     onError,
                                     current,
                                     next));
        }

        private class ConvertContext
        {
            public ConvertContext(int columnIndex, ValueAccessor accessor)
            {
                ColumnIndex = columnIndex;
                Accessor    = accessor;
            }

            public int ColumnIndex { get; }
            public ValueAccessor Accessor { get; }
        }

        private static T ApplyFieldValue<T>(
            string header,
            IList<string> currentRow,
            int rowIndex,
            Action<string> onError,
            T currentResult,
            ConvertContext context)
        {
            try
            {
                context.Accessor.Value[currentResult] = context.Accessor.Type.Convert(currentRow[context.ColumnIndex]);
            }
            catch (ArgumentException)
            {
                var errorMessage = $"Invalid format in record '{rowIndex}' and field {header}";
                if (onError == null) throw new FormatException(errorMessage);
                onError(errorMessage);
            }

            return currentResult;
        }
    }
}
