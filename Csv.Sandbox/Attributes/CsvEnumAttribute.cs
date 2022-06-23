using System;

namespace Csv.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class CsvEnumAttribute : Attribute
{
    public CsvEnumAttribute(string name)
    {
        Name = name;
    }
    
    public string Name { get; }
}
