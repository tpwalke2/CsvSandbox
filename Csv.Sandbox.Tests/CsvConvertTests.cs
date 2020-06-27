using System;
using System.Linq;
using System.Reflection;
using Csv.Extensions;
using NUnit.Framework;

namespace Csv.Tests
{
    [TestFixture]
    public class CsvConvert_ShouldHandleSimpleClass
    {
        [Test]
        public void DeserializeEmptyString()
        {
            var result = CsvConvert.DeserializeObject<SimpleExample>("");
            Assert.That(result, Is.SameAs(default(SimpleExample)));
        }
        
        [Test]
        public void DeserializeNoEntriesOnlyHeader()
        {
            const string input = @"Count,Flag,Description";
            var result = CsvConvert.DeserializeObject<SimpleExample>(input);
            Assert.That(result, Is.SameAs(default(SimpleExample)));
        }
        
        [Test]
        public void DeserializeAllProperties()
        {
            const string input = @"Count,Flag,Description
5,true,""This is the description""";
            var result = CsvConvert.DeserializeObject<SimpleExample>(input);
            
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
            var result = CsvConvert.DeserializeObjects<SimpleExample>(input).ToList();
            
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
            var result = CsvConvert.DeserializeObject<SimpleExample>(input);
            
            Assert.That(result.Count, Is.EqualTo(5));
            Assert.That(result.Flag, Is.False);
            Assert.That(result.Description, Is.EqualTo(default(string)));
        }

        [Test]
        public void DeserializeShouldNotCareAboutInputOrder()
        {
            const string input = @"Description,Count,Flag
""This is the description"",5,true";
            var result = CsvConvert.DeserializeObject<SimpleExample>(input);
            
            Assert.That(result.Count, Is.EqualTo(5));
            Assert.That(result.Flag, Is.True);
            Assert.That(result.Description, Is.EqualTo("This is the description"));
        }

        [Test]
        public void DeserializeShouldThrowErrorSomethingWithInvalidFormat()
        {
            const string input = @"Count
""This is the description""";
            Assert.Throws<FormatException>(() => CsvConvert.DeserializeObject<SimpleExample>(input));
        }
        
        [Test]
        public void DeserializeShouldThrowUseErrorCallbackIfProvided()
        {
            const string input = @"Count
""This is the description""";

            var onErrorWasCalled = false;

            CsvConvert.DeserializeObject<SimpleExample>(input, new CsvConvertSettings
            {
                OnError = (errorMessage) => onErrorWasCalled = true
            });
            
            Assert.That(onErrorWasCalled, Is.True);
        }

        [Test]
        public void DeserializeShouldIgnoreHeadersThatDoNotCorrespondWithAccessors()
        {
            const string input = @"Description,ID
""This is the description"",5";
            var result = CsvConvert.DeserializeObject<SimpleExample>(input);
            
            Assert.That(result.Count, Is.EqualTo(0));
            Assert.That(result.Flag, Is.False);
            Assert.That(result.Description, Is.EqualTo("This is the description"));
        }

        [Test]
        public void DeserializeShouldIgnorePrivateAccessors()
        {
            const string input = @"Count,Description
5,""This is the description""";
            var result = CsvConvert.DeserializeObject<AccessModifierExample>(input);

            var privateAccessors = typeof(AccessModifierExample).GetAccessors(BindingFlags.Instance | BindingFlags.NonPublic);
            
            Assert.That(privateAccessors["Count"].Value[result], Is.EqualTo(0));
            Assert.That(result.Description, Is.EqualTo("This is the description"));
        }

        [Test]
        public void DeserializeShouldIgnoreProtectedAccessors()
        {
            const string input = @"Flag,Description
true,""This is the description""";
            var result = CsvConvert.DeserializeObject<AccessModifierExample>(input);

            var privateAccessors = typeof(AccessModifierExample).GetAccessors(BindingFlags.Instance | BindingFlags.NonPublic);
            
            Assert.That(privateAccessors["Flag"].Value[result], Is.False);
            Assert.That(result.Description, Is.EqualTo("This is the description"));
        }
    }

    public class AccessModifierExample
    {
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
}
