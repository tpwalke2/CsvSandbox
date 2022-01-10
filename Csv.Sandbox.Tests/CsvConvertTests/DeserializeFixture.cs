using System;
using System.Linq;
using System.Reflection;
using Csv.Extensions;
using NUnit.Framework;

namespace Csv.Tests.CsvConvertTests
{
    [TestFixture]
    public class DeserializeFixture
    {
        [Test]
        public void DeserializeEmptyString()
        {
            var result = CsvConvert.Deserialize<SimpleExample>("");
            Assert.That(result, Is.SameAs(default(SimpleExample)));
        }
        
        [Test]
        public void DeserializeNoEntriesOnlyHeader()
        {
            const string input = @"Count,Flag,Description";
            var result = CsvConvert.Deserialize<SimpleExample>(input);
            Assert.That(result, Is.SameAs(default(SimpleExample)));
        }
        
        [Test]
        public void DeserializeAllProperties()
        {
            const string input = @"Count,Flag,Description
5,true,""This is the description""";
            var result = CsvConvert.Deserialize<SimpleExample>(input);
            
            Assert.That(result.Count, Is.EqualTo(5));
            Assert.That(result.Flag, Is.True);
            Assert.That(result.Description, Is.EqualTo("This is the description"));
        }
        
        [Test]
        public void Deserialize_DifferentSeparator()
        {
            const string input = @"Count!Flag!Description
5!true!""This is the description""";
            var result = CsvConvert.Deserialize<SimpleExample>(input, new CsvConvertSettings
            {
                Separator = '!'
            });
            
            Assert.That(result.Count, Is.EqualTo(5));
            Assert.That(result.Flag, Is.True);
            Assert.That(result.Description, Is.EqualTo("This is the description"));
        }
        
        [Test]
        public void Deserialize_ShouldHonorPropertyAttribute()
        {
            const string input = @"count,flag,desc
5,true,""This is the description""";
            var result = CsvConvert.Deserialize<PropertyAttributeExample>(input);
            
            Assert.That(result.Count, Is.EqualTo(5));
            Assert.That(result.Flag, Is.True);
            Assert.That(result.Description, Is.EqualTo("This is the description"));
        }
        
        [Test]
        public void Deserialize_ShouldHandleEscapedPropertyNamesAttribute()
        {
            const string input = @"""The Count"",""A Flag"",""A Description""
5,True,""This is the description""";
            var result = CsvConvert.Deserialize<EscapedPropertyAttributeExample>(input);
            
            Assert.That(result.Count, Is.EqualTo(5));
            Assert.That(result.Flag, Is.True);
            Assert.That(result.Description, Is.EqualTo("This is the description"));
        }
        
        [Test]
        public void DeserializeRecord()
        {
            const string input = @"Count,Flag,Description
5,true,""This is the description""";
            var result = CsvConvert.Deserialize<RecordExample>(input);
            
            Assert.That(result.Count, Is.EqualTo(5));
            Assert.That(result.Flag, Is.True);
            Assert.That(result.Description, Is.EqualTo("This is the description"));
        }
        
        [Test]
        public void Deserialize_ShouldHonorIgnoreAttribute()
        {
            const string input = @"Count,Flag,Description
5,true,""This is the description""";
            var result = CsvConvert.Deserialize<IgnoreAttributeExample>(input);
            
            Assert.That(result.Count, Is.EqualTo(0));
            Assert.That(result.Flag, Is.True);
            Assert.That(result.Description, Is.EqualTo("This is the description"));
        }
        
        [Test]
        public void DeserializeStruct()
        {
            const string input = @"Count,Flag,Description
5,true,""This is the description""";
            var result = CsvConvert.Deserialize<StructExample>(input);
            
            Assert.That(result.Count, Is.EqualTo(5));
            Assert.That(result.Flag, Is.True);
            Assert.That(result.Description, Is.EqualTo("This is the description"));
        }
        
        [Test]
        public void DeserializeMultipleInstances()
        {
            const string input = @"Count,Flag,Description
5,true,""This is the description""
10,false,""This is another description""";
            var result = CsvConvert.DeserializeList<SimpleExample>(input).ToList();
            
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Count, Is.EqualTo(5));
            Assert.That(result[0].Flag, Is.True);
            Assert.That(result[0].Description, Is.EqualTo("This is the description"));
            Assert.That(result[1].Count, Is.EqualTo(10));
            Assert.That(result[1].Flag, Is.False);
            Assert.That(result[1].Description, Is.EqualTo("This is another description"));
        }

        [Test]
        public void DeserializeShouldHandleMissingProperties()
        {
            const string input = @"Count
5";
            var result = CsvConvert.Deserialize<SimpleExample>(input);
            
            Assert.That(result.Count, Is.EqualTo(5));
            Assert.That(result.Flag, Is.False);
            Assert.That(result.Description, Is.EqualTo(default(string)));
        }

        [Test]
        public void DeserializeShouldNotCareAboutInputOrder()
        {
            const string input = @"Description,Count,Flag
""This is the description"",5,true";
            var result = CsvConvert.Deserialize<SimpleExample>(input);
            
            Assert.That(result.Count, Is.EqualTo(5));
            Assert.That(result.Flag, Is.True);
            Assert.That(result.Description, Is.EqualTo("This is the description"));
        }

        [Test]
        public void DeserializeShouldThrowErrorSomethingWithInvalidFormat()
        {
            const string input = @"Count
""This is the description""";
            Assert.Throws<FormatException>(() => CsvConvert.Deserialize<SimpleExample>(input));
        }
        
        [Test]
        public void DeserializeShouldThrowUseErrorCallbackIfProvided()
        {
            const string input = @"Count
""This is the description""";

            var onErrorWasCalled = false;

            CsvConvert.Deserialize<SimpleExample>(input, new CsvConvertSettings
            {
                OnError = errorMessage => onErrorWasCalled = true
            });
            
            Assert.That(onErrorWasCalled, Is.True);
        }

        [Test]
        public void DeserializeShouldIgnoreHeadersThatDoNotCorrespondWithAccessors()
        {
            const string input = @"Description,ID
""This is the description"",5";
            var result = CsvConvert.Deserialize<SimpleExample>(input);
            
            Assert.That(result.Count, Is.EqualTo(0));
            Assert.That(result.Flag, Is.False);
            Assert.That(result.Description, Is.EqualTo("This is the description"));
        }

        [Test]
        public void DeserializeShouldIgnorePrivateAccessors()
        {
            const string input = @"Count,Description
5,""This is the description""";
            var result = CsvConvert.Deserialize<AccessModifierExample>(input);

            var privateAccessors = typeof(AccessModifierExample)
                                   .GetAccessors(BindingFlags.Instance | BindingFlags.NonPublic)
                                   .ToDictionary(gs => gs.Name, gs => gs);
            
            Assert.That(privateAccessors["Count"].Value[result], Is.EqualTo(0));
            Assert.That(result.Description, Is.EqualTo("This is the description"));
        }

        [Test]
        public void DeserializeShouldIgnoreProtectedAccessors()
        {
            const string input = @"Flag,Description
true,""This is the description""";
            var result = CsvConvert.Deserialize<AccessModifierExample>(input);

            var privateAccessors = typeof(AccessModifierExample)
                                   .GetAccessors(BindingFlags.Instance | BindingFlags.NonPublic)
                                   .ToDictionary(gs => gs.Name, gs => gs);
            
            Assert.That(privateAccessors["Flag"].Value[result], Is.False);
            Assert.That(result.Description, Is.EqualTo("This is the description"));
        }
    }
}
