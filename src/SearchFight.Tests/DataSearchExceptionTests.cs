using System;
using FluentAssertions;
using NUnit.Framework;
using SearchFight.Services.Exceptions;

namespace SearchFight.Tests
{
    public class DataSearchExceptionTests
    {
        [Test]
        public void Constructor1Test()
        {
            var result = new DataSearcherException();
            result.Should().NotBeNull();
            result.Message.Should().Be($"Exception of type '{typeof(DataSearcherException)}' was thrown.");
            result.InnerException.Should().BeNull();
        }

        [Test]
        public void Constructor2Test()
        {
            var expectedMessage = "test message";
            var result = new DataSearcherException(expectedMessage);
            result.Should().NotBeNull();
            result.Message.Should().Be(expectedMessage);
            result.InnerException.Should().BeNull();
        }

        [Test]
        public void Constructor3Test()
        {
            var expectedMessage = "test message";
            var expectedInnerException = new Exception("Hello");
            var result = new DataSearcherException(expectedMessage, expectedInnerException);
            result.Should().NotBeNull();
            result.Message.Should().Be(expectedMessage);
            result.InnerException.Should().Be(expectedInnerException);
        }
    }
}