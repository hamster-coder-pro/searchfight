using System;
using System.Collections.Generic;
using System.Text;
using Castle.Core.Logging;
using FluentAssertions;
using NUnit.Framework;
using Search.Common.Extensions;

namespace SearchFight.Tests
{
    public class StringExtensionsTests
    {
        [Test]
        [TestCase("0123456789", 0, 2, "01")]
        [TestCase("0123456789", 1, 4, "123")]
        [TestCase("0123456789", 4, 20, "456789")]
        public void CopySuccess(string input, int startIndex, int endIndex, string expectedResult)
        {
            var actualResult = StringExtensions.Copy(input, startIndex, endIndex);
            actualResult.Should().Be(expectedResult);
        }

        [Test]
        [TestCase(null, 0, 2, typeof(ArgumentNullException))]
        [TestCase("0123456789", -1, 4, typeof(ArgumentOutOfRangeException))]
        [TestCase("0123456789", 4, -1, typeof(ArgumentOutOfRangeException))]
        public void CopyException(string input, int startIndex, int endIndex, Type expectedException)
        {
            Assert.Throws(expectedException, () => StringExtensions.Copy(input, startIndex, endIndex));
        }

        [Test]
        [TestCase("0123456789", 4, ".", "012.")]
        [TestCase("0123456789", 20, ".", "0123456789")]
        public void OverflowPositive(string input, int length, string overflow, string expectedResult)
        {
            var actualResult = StringExtensions.Overflow(input, length, overflow);
            actualResult.Should().Be(expectedResult);
        }

        [Test]
        [TestCase("0123456789", 3, "...")]
        [TestCase("0123456789", 4, "0...")]
        [TestCase("0123456789", 10, "0123456789")]
        public void OverflowPositive(string input, int length, string expectedResult)
        {
            var actualResult = StringExtensions.Overflow(input, length);
            actualResult.Should().Be(expectedResult);
        }

        [Test]
        [TestCase("0123456789", 2, "abc", typeof(ArgumentOutOfRangeException))]
        [TestCase("0123456789", -1, ".", typeof(ArgumentOutOfRangeException))]
        [TestCase(null, -1, ".", typeof(ArgumentNullException))]
        public void OverflowNegative(string input, int length, string overflow, Type expectedException)
        {
            Assert.Throws(expectedException, () => StringExtensions.Overflow(input, length, overflow));
        }

        [Test]
        [TestCase("0123456789", 2, typeof(ArgumentOutOfRangeException))]
        [TestCase("0123456789", -1, typeof(ArgumentOutOfRangeException))]
        [TestCase(null, -1, typeof(ArgumentNullException))]
        public void OverflowNegative(string input, int length, Type expectedException)
        {
            Assert.Throws(expectedException, () => StringExtensions.Overflow(input, length));
        }

        [Test]
        [TestCase("0123401234", "0", 0, StringComparison.InvariantCulture, 1)]
        [TestCase("0123401234", "0", 1, StringComparison.InvariantCulture, 6)]
        [TestCase("0123401234", "23", 0, StringComparison.InvariantCulture, 4)]
        [TestCase("0123401234", "23", 3, StringComparison.InvariantCulture, 9)]
        [TestCase("0123401234", "23", 8, StringComparison.InvariantCulture, -1)]
        public void AfterIndexOfPositive(string input, string value, int startIndex, StringComparison stringComparison, int expectedResult)
        {
            var actualResult = StringExtensions.AfterIndexOf(input, value, startIndex, stringComparison);
            actualResult.Should().Be(expectedResult);
        }

        [Test]
        [TestCase("0123401234", "0", 0, 1)]
        [TestCase("0123401234", "0", 1, 6)]
        [TestCase("0123401234", "23", 0, 4)]
        [TestCase("0123401234", "23", 3, 9)]
        [TestCase("0123401234", "23", 8, -1)]
        public void AfterIndexOfPositive(string input, string value, int startIndex, int expectedResult)
        {
            var actualResult = StringExtensions.AfterIndexOf(input, value, startIndex);
            actualResult.Should().Be(expectedResult);
        }

        [Test]
        [TestCase("0123401234", "0", StringComparison.InvariantCulture, 1)]
        [TestCase("0123401234", "23", StringComparison.InvariantCulture, 4)]
        [TestCase("0123401234", "5", StringComparison.InvariantCulture, -1)]
        public void AfterIndexOfPositive(string input, string value, StringComparison stringComparison, int expectedResult)
        {
            var actualResult = StringExtensions.AfterIndexOf(input, value, stringComparison);
            actualResult.Should().Be(expectedResult);
        }

        [Test]
        [TestCase("0123401234", "0", 1)]
        [TestCase("0123401234", "23", 4)]
        [TestCase("0123401234", "5", -1)]
        public void AfterIndexOfPositive(string input, string value, int expectedResult)
        {
            var actualResult = StringExtensions.AfterIndexOf(input, value);
            actualResult.Should().Be(expectedResult);
        }

        // negative

        [Test]
        [TestCase(null, "0", 0, StringComparison.InvariantCulture, typeof(ArgumentNullException))]
        [TestCase("0123401234", null, 1, StringComparison.InvariantCulture, typeof(ArgumentNullException))]
        [TestCase("0123401234", "23", -1, StringComparison.InvariantCulture, typeof(ArgumentOutOfRangeException))]
        [TestCase("0123401234", "23", 3, -1, typeof(ArgumentException))]
        public void AfterIndexOfNegative(string input, string value, int startIndex, StringComparison stringComparison, Type expectedException)
        {
            Assert.Throws(expectedException, () => StringExtensions.AfterIndexOf(input, value, startIndex, stringComparison));
        }

        [Test]
        [TestCase(null, "0", 0, typeof(ArgumentNullException))]
        [TestCase("0123401234", null, 1, typeof(ArgumentNullException))]
        [TestCase("0123401234", "23", -1, typeof(ArgumentOutOfRangeException))]
        public void AfterIndexOfNegative(string input, string value, int startIndex, Type expectedException)
        {
            Assert.Throws(expectedException, () => StringExtensions.AfterIndexOf(input, value, startIndex));
        }

        [Test]
        [TestCase(null, "0", StringComparison.InvariantCulture, typeof(ArgumentNullException))]
        [TestCase("0123401234", null, StringComparison.InvariantCulture, typeof(ArgumentNullException))]
        [TestCase("0123401234", "23", -1, typeof(ArgumentException))]
        public void AfterIndexOfNegative(string input, string value, StringComparison stringComparison, Type expectedException)
        {
            Assert.Throws(expectedException, () => StringExtensions.AfterIndexOf(input, value, stringComparison));
        }

        [Test]
        [TestCase(null, "0", typeof(ArgumentNullException))]
        [TestCase("0123401234", null, typeof(ArgumentNullException))]
        public void AfterIndexOfNegative(string input, string value, Type expectedException)
        {
            Assert.Throws(expectedException, () => StringExtensions.AfterIndexOf(input, value));
        }
    }
}
