using System;

namespace Csv.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class CsvIgnoreAttribute: Attribute
    { }
}
