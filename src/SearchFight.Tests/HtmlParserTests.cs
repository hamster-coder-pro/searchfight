using System;
using FluentAssertions;
using NUnit.Framework;
using SearchFight.Services.Exceptions;
using SearchFight.Services.Services;

namespace SearchFight.Tests
{
    public class HtmlParserTests
    {
        [Test]
        public void ConstructorSucceed()
        {
            // AAA
            var testee = new HtmlParser();
        }


        [Test]
        [TestCase("test test", "test", 0, 0)]
        [TestCase("test test", "test", 1, 5)]
        public void IndexOfTagSucceed(string text, string tag, int startIndex, int expectedResult)
        {
            // Arrange
            var testee = new HtmlParser();

            // Act
            var actual = testee.IndexOfTag(text, tag, startIndex);

            // Assert
            actual.Should().Be(expectedResult);
        }

        [Test]
        [TestCase(null, "test", 0, typeof(ArgumentNullException))]
        [TestCase("test test", null, 1, typeof(ArgumentNullException))]
        [TestCase("test test", "test", -1, typeof(ArgumentOutOfRangeException))]
        [TestCase("test test", "oleg", 0, typeof(DataSearcherException))]
        public void IndexOfTagException(string text, string tag, int startIndex, Type expectedException)
        {
            // Arrange
            var testee = new HtmlParser();

            // Act + Assert
            Assert.Throws(expectedException, () => testee.IndexOfTag(text, tag, startIndex));
        }

        [Test]
        [TestCase("test test", "tes", 0, 3)]
        [TestCase("test test", "tes", 1, 8)]
        public void AfterIndexOfTagSucceed(string text, string tag, int startIndex, int expectedResult)
        {
            // Arrange
            var testee = new HtmlParser();

            // Act
            var actual = testee.AfterIndexOfTag(text, tag, startIndex);

            // Assert
            actual.Should().Be(expectedResult);
        }

        [Test]
        [TestCase(null, "test", 0, typeof(ArgumentNullException))]
        [TestCase("test test", null, 1, typeof(ArgumentNullException))]
        [TestCase("test test", "test", -1, typeof(ArgumentOutOfRangeException))]
        [TestCase("test test", "oleg", 0, typeof(DataSearcherException))]
        [TestCase("test test", "test", 1, typeof(DataSearcherException))]
        public void AfterIndexOfTagException(string text, string tag, int startIndex, Type expectedException)
        {
            // Arrange
            var testee = new HtmlParser();

            // Act + Assert
            Assert.Throws(expectedException, () => testee.AfterIndexOfTag(text, tag, startIndex));
        }

        [Test]
        [TestCase("1", 1)]
        [TestCase("11", 11)]
        [TestCase("1,1", 11)]
        [TestCase("asd1adf,asdf1adfasd", 11)]
        [TestCase("a1s2d3a4d5f6a7s8f9a0fasd", 1234567890)]
        public void ExtractNumberSucceed(string text, int expectedResult)
        {
            // Arrange
            var testee = new HtmlParser();

            // Act
            var actual = testee.ExtractNumber(text);

            // Assert
            actual.Should().Be(expectedResult);
        }

        [Test]
        [TestCase("a1s2d3a4d5f6a7s8f9a0f1a2s3d4", typeof(DataSearcherException))]
        [TestCase("", typeof(DataSearcherException))]
        [TestCase(null, typeof(ArgumentNullException))]
        public void ExtractNumberException(string text, Type expectedException)
        {
            // Arrange
            var testee = new HtmlParser();

            // Act + Assert
            Assert.Throws(expectedException, () => testee.ExtractNumber(text));
        }
    }
}