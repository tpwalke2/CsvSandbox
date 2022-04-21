using System;

namespace Csv.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class CsvPropertyAttribute: Attribute
{
    public CsvPropertyAttribute() {}

    public CsvPropertyAttribute(string propertyName)
    {
        PropertyName = propertyName;
    }
        
    public string PropertyName { get; set; }
}