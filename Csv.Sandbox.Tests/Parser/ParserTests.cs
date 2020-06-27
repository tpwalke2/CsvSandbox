using System;
using Csv.Parser;
using NUnit.Framework;

namespace Csv.Tests.Parser
{
    [TestFixture]
    public class ParserTests
    {
        [Test]
        public void Parse_OneLine_ShouldGenerateOneRow()
        {
            const string input = "Row1";
            var result = Csv.Parser.Parser.Parse(input);
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public void Parse_TwoLines_ShouldGenerateTwoRows()
        {
            const string input  = "Row1\nRow2";
            var result = Csv.Parser.Parser.Parse(input);
            Assert.That(result.Count, Is.EqualTo(2));
        }
        
        [Test]
        public void Parse_BlankLines_ShouldNotGenerateRows()
        {
            const string input  = "Row1\n\nRow2";
            var result = Csv.Parser.Parser.Parse(input);
            Assert.That(result.Count, Is.EqualTo(2));
        }
        
        [Test]
        public void Parse_ShouldCreateOneField()
        {
            const string input  = "Row1Field1";
            var result = Csv.Parser.Parser.Parse(input);
            Assert.That(result[0].Count, Is.EqualTo(1));
            Assert.That(result[0][0], Is.EqualTo("Row1Field1"));
        }

        [Test]
        public void Parse_ShouldCreateTwoFields()
        {
            const string input  = "Row1Field1,Row1Field2";
            var result = Csv.Parser.Parser.Parse(input);
            Assert.That(result[0].Count, Is.EqualTo(2));
            Assert.That(result[0][0], Is.EqualTo("Row1Field1"));
            Assert.That(result[0][1], Is.EqualTo("Row1Field2"));
        }

        [Test]
        public void Parse_ShouldHandleQuotedSeparators()
        {
            const string input  = "1997,Ford,E350,\"Super, luxurious truck\"";
            var result = Csv.Parser.Parser.Parse(input);
            Assert.That(result[0][0], Is.EqualTo("1997"));
            Assert.That(result[0][1], Is.EqualTo("Ford"));
            Assert.That(result[0][2], Is.EqualTo("E350"));
            Assert.That(result[0][3], Is.EqualTo("Super, luxurious truck"));
        }
        
        [Test]
        public void Parse_ShouldHandleEscapedQuotes()
        {
            const string input  = "1997,Ford,E350,\"Super, \"\"luxurious\"\" truck\"";
            var result = Csv.Parser.Parser.Parse(input);
            Assert.That(result[0][0], Is.EqualTo("1997"));
            Assert.That(result[0][1], Is.EqualTo("Ford"));
            Assert.That(result[0][2], Is.EqualTo("E350"));
            Assert.That(result[0][3], Is.EqualTo("Super, \"luxurious\" truck"));
        }
        
        [Test]
        public void Parse_ShouldAllowNewLinesInQuotedField()
        {
            const string input  = @"""Go get one now
they are going fast""";
            var result = Csv.Parser.Parser.Parse(input);
            Assert.That(result[0][0], Is.EqualTo(@"Go get one now
they are going fast"));
        }
        
        [Test]
        public void Parse_ShouldHandleQuotedFields()
        {
            const string input  = "\"1997\",\"Ford\",\"E350\"";
            var result = Csv.Parser.Parser.Parse(input);
            Assert.That(result[0][0], Is.EqualTo("1997"));
            Assert.That(result[0][1], Is.EqualTo("Ford"));
            Assert.That(result[0][2], Is.EqualTo("E350"));
        }
        
        [Test]
        public void Parse_ShouldHandleUnescapedQuotesInFields()
        {
            const string input  = "New York City,40°42′46\"N,74°00′21\"W";
            var          result = Csv.Parser.Parser.Parse(input);
            Assert.That(result[0][0], Is.EqualTo("New York City"));
            Assert.That(result[0][1], Is.EqualTo("40°42′46\"N"));
            Assert.That(result[0][2], Is.EqualTo("74°00′21\"W"));
        }
        
        [Test]
        public void Parse_ShouldIncludeWhitespaceInFields()
        {
            const string input  = " 1997 , Ford , E350 ";
            var          result = Csv.Parser.Parser.Parse(input);
            Assert.That(result[0][0], Is.EqualTo(" 1997 "));
            Assert.That(result[0][1], Is.EqualTo(" Ford "));
            Assert.That(result[0][2], Is.EqualTo(" E350 "));
        }

        [Test]
        public void Parse_ShouldAllowDifferentSeparator()
        {
            const string input  = "1997!Ford!E350!\"Super! luxurious truck\"";
            var result = Csv.Parser.Parser.Parse(input, new Settings()
            {
                Separator = '!'
            });
            Assert.That(result[0][0], Is.EqualTo("1997"));
            Assert.That(result[0][1], Is.EqualTo("Ford"));
            Assert.That(result[0][2], Is.EqualTo("E350"));
            Assert.That(result[0][3], Is.EqualTo("Super! luxurious truck"));
        }
        
        [Test]
        public void Parse_ShouldAllowOptionToTrimWhitespace()
        {
            const string input  = " 1997 , Ford , E350 ";
            var result = Csv.Parser.Parser.Parse(input, new Settings()
            {
                TrimWhitespace = true
            });
            Assert.That(result[0][0], Is.EqualTo("1997"));
            Assert.That(result[0][1], Is.EqualTo("Ford"));
            Assert.That(result[0][2], Is.EqualTo("E350"));
        }

        [Test]
        public void Parse_ShouldThrowErrorIfQuotedFieldContainsUnescapedQuote()
        {
            const string input = "\"Super! \"luxurious truck\"";
            Assert.Throws<FormatException>(() => Csv.Parser.Parser.Parse(input));
        }
        
        [Test]
        public void Parse_ShouldUseErrorCallbackIfAvailableWhenQuotedFieldContainsUnescapedQuote()
        {
            const string input = "\"Super! \"luxurious truck\"";
            var actualLineNumber = 0;
            var actualColumn = 0;
            Csv.Parser.Parser.Parse(input, new Settings()
            {
                OnError = (lineNumber, column) =>
                {
                    actualLineNumber = lineNumber;
                    actualColumn = column;
                }
            });
            
            Assert.That(actualLineNumber, Is.EqualTo(1));
            Assert.That(actualColumn, Is.EqualTo(9));
        }
        
        [Test]
        public void Parse_ShouldCountSeparatorsForErrors()
        {
            const string input = "1,\"Bad \" Field\"";
            var actualLineNumber = 0;
            var actualColumn = 0;
            Csv.Parser.Parser.Parse(input, new Settings()
            {
                OnError = (lineNumber, column) =>
                {
                    actualLineNumber = lineNumber;
                    actualColumn     = column;
                }
            });
            
            Assert.That(actualLineNumber, Is.EqualTo(1));
            Assert.That(actualColumn, Is.EqualTo(8));
        }
        
        [Test]
        public void Parse_ShouldGenerateCorrectLineNumberOnError()
        {
            const string input = "Row1\n\"Bad \" Field\"";
            var actualLineNumber = 0;
            Csv.Parser.Parser.Parse(input, new Settings()
            {
                OnError = (lineNumber, column) =>
                {
                    actualLineNumber = lineNumber;
                }
            });
            
            Assert.That(actualLineNumber, Is.EqualTo(2));
        }
    }
}
