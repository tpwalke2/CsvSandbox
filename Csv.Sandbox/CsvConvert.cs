using System.Collections.Generic;
using Csv.Converter;

namespace Csv;

public static class CsvConvert
{
    public static string Serialize<T>(T input, CsvConvertSettings settings = null)
    {
        return Serializer.Serialize(input, settings);
    }

    public static string SerializeList<T>(IEnumerable<T> input, CsvConvertSettings settings = null)
    {
        return Serializer.SerializeList(input, settings);
    }
        
    public static T Deserialize<T>(string input, CsvConvertSettings settings = null) where T : new()
    {
        return Deserializer.Deserialize<T>(input, settings);
    }

    public static IEnumerable<T> DeserializeList<T>(string input, CsvConvertSettings settings = null)
        where T : new()
    {
        return Deserializer.DeserializeList<T>(input, settings);
    }
}