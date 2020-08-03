using NUnit.Framework;
using SearchFight.Console.DI;

namespace SearchFight.Tests
{
    public class BootstrapperTests
    {
        [Test]
        public void Verify()
        {
            Bootstrapper.Start();
        }
    }
}