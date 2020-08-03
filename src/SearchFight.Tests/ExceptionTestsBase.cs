using System;
using FluentAssertions;
using NUnit.Framework;

namespace SearchFight.Tests
{
    public abstract class ExceptionTestsBase<TException>
        where TException: Exception
    {
        protected abstract TException CreateException();
        protected abstract TException CreateException(string exceptionMessage);
        protected abstract TException CreateException(string exceptionMessage, Exception innerException);

        [Test]
        public void Constructor1Test()
        {
            var result = CreateException();
            result.Should().NotBeNull();
            result.Message.Should().Be($"Exception of type '{typeof(TException)}' was thrown.");
            result.InnerException.Should().BeNull();
        }

        [Test]
        public void Constructor2Test()
        {
            var expectedMessage = "test message";
            var result = CreateException(expectedMessage);
            result.Should().NotBeNull();
            result.Message.Should().Be(expectedMessage);
            result.InnerException.Should().BeNull();
        }

        [Test]
        public void Constructor3Test()
        {
            var expectedMessage = "test message";
            var expectedInnerException = new Exception("Hello");
            var result = CreateException(expectedMessage, expectedInnerException);
            result.Should().NotBeNull();
            result.Message.Should().Be(expectedMessage);
            result.InnerException.Should().Be(expectedInnerException);
        }

        [Test]
        public void Constructor4Test()
        {
            var expectedMessage = "test message";
            var expectedInnerException = new Exception("Hello");
            var result = CreateException(expectedMessage, expectedInnerException);

            result.Should().BeBinarySerializable();
        }
    }
}