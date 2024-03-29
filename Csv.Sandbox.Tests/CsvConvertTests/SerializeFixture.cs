using System.Collections.Generic;
using NUnit.Framework;

namespace Csv.Tests.CsvConvertTests;

[TestFixture]
public class SerializeFixture
{
    [Test]
    public void SerializeNull()
    {
        var result = CsvConvert.Serialize((SimpleExample)null);
        Assert.That(result, Is.EqualTo(""));
    }

    [Test]
    public void SerializeAnonymousObject_NamedFields()
    {
        // this would look nicer if .NET didn't erase names during the compile
        const string expected = """
                                Item1,Item2
                                5,True
                                """;
        var input  = (Count: 5, Flag: true);
        var result = CsvConvert.Serialize(input);
        Assert.That(result, Is.EqualTo(expected));
    }
        
    [Test]
    public void SerializeAnonymousObject_UnnamedFields()
    {
        const string expected = """
                                Item1,Item2
                                5,True
                                """;
        var input  = (5, true);
        var result = CsvConvert.Serialize(input);
        Assert.That(result, Is.EqualTo(expected));
    }
        
    [Test]
    public void SerializeSingleObject()
    {
        const string expected = """
                                Count,Flag,Description
                                5,True,"This is the description"
                                """;

        var input = new SimpleExample
        {
            Count       = 5,
            Flag        = true,
            Description = "This is the description"
        };
            
        var result = CsvConvert.Serialize(input);
        Assert.That(result, Is.EqualTo(expected));
    }
    
    [Test]
    public void Serialize_ShouldEscapeDoubleQuote()
    {
        const string expected = """
                                Count,Flag,Description
                                5,True,"This is a quote""
                                """;

        var input = new SimpleExample
        {
            Count       = 5,
            Flag        = true,
            Description = "This is a quote\""
        };
        
        var result = CsvConvert.Serialize(input);
        Assert.That(result, Is.EqualTo(expected));
    }
        
    [Test]
    public void Serialize_ShouldNotEmitHeader()
    {
        const string expected = """
                                5,True,"This is the description"
                                """;

        var input = new SimpleExample
        {
            Count       = 5,
            Flag        = true,
            Description = "This is the description"
        };
            
        var result = CsvConvert.Serialize(input, new CsvConvertSettings {EmitHeader = false});
        Assert.That(result, Is.EqualTo(expected));
    }
        
    [Test]
    public void Serialize_EscapeStringsWithSeparator()
    {
        const string expected = """
                                Count!Flag!Description
                                5!True!"Hi!"
                                """;

        var input = new SimpleExample
        {
            Count       = 5,
            Flag        = true,
            Description = "Hi!"
        };
            
        var result = CsvConvert.Serialize(input, new CsvConvertSettings
        {
            Separator = '!'
        });
        Assert.That(result, Is.EqualTo(expected));
    }
        
    [Test]
    public void Serialize_DifferentSeparator()
    {
        const string expected = """
                                Count!Flag!Description
                                5!True!"This is the description"
                                """;

        var input = new SimpleExample
        {
            Count       = 5,
            Flag        = true,
            Description = "This is the description"
        };
            
        var result = CsvConvert.Serialize(input, new CsvConvertSettings
        {
            Separator = '!'
        });
        Assert.That(result, Is.EqualTo(expected));
    }
        
    [Test]
    public void Serialize_ShouldHonorPropertyAttribute()
    {
        const string expected = """
                                count,flag,desc
                                5,True,"This is the description"
                                """;

        var input = new PropertyAttributeExample
        {
            Count       = 5,
            Flag        = true,
            Description = "This is the description"
        };
            
        var result = CsvConvert.Serialize(input);
        Assert.That(result, Is.EqualTo(expected));
    }
        
    [Test]
    public void Serialize_ShouldEscapePropertyNames()
    {
        const string expected = """
                                "The Count","A Flag","A Description"
                                5,True,"This is the description"
                                """;

        var input = new EscapedPropertyAttributeExample
        {
            Count       = 5,
            Flag        = true,
            Description = "This is the description"
        };
            
        var result = CsvConvert.Serialize(input);
        Assert.That(result, Is.EqualTo(expected));
    }
        
    [Test]
    public void SerializeRecord()
    {
        const string expected = """
                                Count,Flag,Description
                                5,True,"This is the description"
                                """;

        var input = new RecordExample
        {
            Count       = 5,
            Flag        = true,
            Description = "This is the description"
        };
            
        var result = CsvConvert.Serialize(input);
        Assert.That(result, Is.EqualTo(expected));
    }
        
    [Test]
    public void SerializeStruct()
    {
        const string expected = """
                                Count,Flag,Description
                                5,True,"This is the description"
                                """;

        var input = new StructExample
        {
            Count       = 5,
            Flag        = true,
            Description = "This is the description"
        };
            
        var result = CsvConvert.Serialize(input);
        Assert.That(result, Is.EqualTo(expected));
    }
        
    [Test]
    public void Serialize_ShouldHonorIgnoreAttribute()
    {
        const string expected = """
                                Flag,Description
                                True,"This is the description"
                                """;

        var input = new IgnoreAttributeExample
        {
            Count       = 5,
            Flag        = true,
            Description = "This is the description"
        };
            
        var result = CsvConvert.Serialize(input);
        Assert.That(result, Is.EqualTo(expected));
    }
        
    [Test]
    public void Serialize_OnlyPublicModifiers()
    {
        const string expected = """
                                Description
                                "This is the description"
                                """;

        var input = new AccessModifierExample(5, true, "This is the description");
            
        var result = CsvConvert.Serialize(input);
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void SerializeMultipleObjects()
    {
        const string expected = """
                                Count,Flag,Description
                                5,True,"This is the description"
                                10,False,"This is another description"
                                """;

        var input = new List<SimpleExample>
        {
            new()
            {
                Count       = 5,
                Flag        = true,
                Description = "This is the description"
            },
            new()
            {
                Count       = 10,
                Flag        = false,
                Description = "This is another description"
            }
        };
            
        var result = CsvConvert.SerializeList(input);
        Assert.That(result, Is.EqualTo(expected));
    }
        
    [Test]
    public void SerializeMultipleObjects_NullInput()
    {
        var result = CsvConvert.SerializeList((IEnumerable<SimpleExample>)null);
        Assert.That(result, Is.EqualTo(""));
    }
    
    [Test]
    public void SerializeShouldHonorEnums()
    {
        var input = new EnumExample
        {
            Status = ExampleStatuses.Done
        };
        
        const string expected = """
                                Status
                                Done
                                """;
        
        var result = CsvConvert.Serialize(input);
        
        Assert.That(result, Is.EqualTo(expected));
    }
    
    [Test]
    public void SerializeShouldHonorEnumDescriptions()
    {
        var input = new EnumExample
        {
            Status = ExampleStatuses.InProgress
        };
        
        const string expected = """
                                Status
                                "In Progress"
                                """;
        
        var result = CsvConvert.Serialize(input);
        
        Assert.That(result, Is.EqualTo(expected));
    }
}