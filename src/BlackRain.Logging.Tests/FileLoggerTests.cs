using System;
using BlackRain.Logging.Tests.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlackRain.Logging.Tests
{
    [TestClass]
    public class FileLoggerTests
    {
        private readonly Func<FileLogger> _loggerFactory = () =>
            new FileLogger(new StatefulQueuedLogWriter(), "category", new FileLoggerOptions { MinimumLogLevel = LogLevel.Information });

        [TestMethod]
        public void FileLogger_Invalids()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                new FileLogger(null, "category", new FileLoggerOptions()));

            Assert.ThrowsException<ArgumentNullException>(() =>
                new FileLogger(new StatefulQueuedLogWriter(), null, new FileLoggerOptions()));

            Assert.ThrowsException<ArgumentNullException>(() =>
                new FileLogger(new StatefulQueuedLogWriter(), "category", null));
        }

        [TestMethod]
        public void FileLogger_BeginScope()
        {
            var logger = _loggerFactory();

            var scope = logger.BeginScope(DateTimeOffset.Now);

            Assert.IsNull(scope);
        }

        [TestMethod]
        public void FileLogger_IsEnabled()
        {
            var logger = _loggerFactory();

            var debug = logger.IsEnabled(LogLevel.Debug);
            var info = logger.IsEnabled(LogLevel.Information);
            var error = logger.IsEnabled(LogLevel.Error);

            Assert.IsFalse(debug);
            Assert.IsTrue(info);
            Assert.IsTrue(error);
        }

        [TestMethod]
        public void FileLogger_Log_Enabled()
        {
            var logWriter = new StatefulQueuedLogWriter();
            var logger = new FileLogger(logWriter, "category", new FileLoggerOptions
            {
                MinimumLogLevel = LogLevel.Information
            });

            logger.LogDebug("foo");

            Assert.IsNull(logWriter.LastMessage);

            logger.LogError("something went horribly wrong, call the police!");

            Assert.IsNotNull(logWriter.LastMessage);
        }

        [TestMethod]
        public void FileLogger_Log()
        {
            var timeStamp = DateTimeOffset.MinValue;
            var logWriter = new StatefulQueuedLogWriter();
            var logger = new FileLogger(logWriter, "category",
                new FileLoggerOptions { TimeStampFactory = () => timeStamp });

            logger.LogInformation("Test");

            var expectedMessage = $"{timeStamp:yyyy-MM-dd HH:mm:ss} [Information] category: Test{Environment.NewLine}";

            Assert.AreEqual(expectedMessage, logWriter.LastMessage.Message);
        }
    }
}
