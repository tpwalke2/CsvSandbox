using System.Collections.Generic;
using Csv.Converter;

namespace Csv
{
    public static class CsvConvert
    {
        public static string SerializeObject<T>(T input, CsvConvertSettings settings = null)
        {
            return Serializer.SerializeObject(input, settings);
        }

        public static string SerializeObjects<T>(IEnumerable<T> input, CsvConvertSettings settings = null)
        {
            return Serializer.SerializeObjects(input, settings);
        }
        
        public static T DeserializeObject<T>(string input, CsvConvertSettings settings = null) where T : new()
        {
            return Deserializer.DeserializeObject<T>(input, settings);
        }

        public static IEnumerable<T> DeserializeObjects<T>(string input, CsvConvertSettings settings = null)
            where T : new()
        {
            return Deserializer.DeserializeObjects<T>(input, settings);
        }
    }
}
