using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlackRain.Logging.Tests
{
    [TestClass]
    public class FileLoggerProviderTests
    {
        [TestMethod]
        public void FileLogger_NullOptions()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new FileLoggerProvider(null));
        }

        [TestMethod]
        public void FileLogger_CreateLogger()
        {
            var loggerProvider = new FileLoggerProvider(options => { });

            var logger = loggerProvider.CreateLogger("test");

            Assert.IsNotNull(logger);
        }
    }
}
