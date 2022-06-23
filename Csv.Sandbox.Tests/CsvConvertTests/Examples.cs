using Csv.Attributes;

namespace Csv.Tests.CsvConvertTests;

public struct StructExample
{
    public StructExample()
    {
        Count       = 0;
        Flag        = false;
        Description = "";
    }
    
    public int Count { get; set; }
    public bool Flag { get; set; }
    public string Description { get; set; }
}

public record RecordExample
{
    public int Count { get; set; }
    public bool Flag { get; set; }
    public string Description { get; set; }
}

public class IgnoreAttributeExample
{
    [CsvIgnore]
    public int Count { get; set; }
    public bool Flag { get; set; }
    public string Description { get; set; }
}

public class PropertyAttributeExample
{
    [CsvProperty("count")]
    public int Count { get; set; }
    [CsvProperty(PropertyName = "flag")]
    public bool Flag { get; set; }
    [CsvProperty(PropertyName = "desc")]
    public string Description { get; set; }
}
    
public class EscapedPropertyAttributeExample
{
    [CsvProperty("The Count")]
    public int Count { get; set; }
    [CsvProperty(PropertyName = "A Flag")]
    public bool Flag { get; set; }
    [CsvProperty(PropertyName = "A Description")]
    public string Description { get; set; }
}

public class AccessModifierExample
{
    public AccessModifierExample() {}

    public AccessModifierExample(int count, bool flag, string description)
    {
        Count       = count;
        Flag        = flag;
        Description = description;
    }
        
    private int Count { get; set; }
    protected bool Flag { get; set; }
    public string Description { get; set; }
}
    
public class SimpleExample
{
    public int Count { get; set; }
    public bool Flag { get; set; }
    public string Description { get; set; }
}

public class EnumExample
{
    public ExampleStatuses Status { get; set; }
}

public enum ExampleStatuses
{
    ToDo = 0,
    [CsvEnum("In Progress")]
    InProgress,
    Done
}