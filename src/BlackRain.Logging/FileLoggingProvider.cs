using System;
using BlackRain.Logging.Abstractions;
using BlackRain.Logging.Internal;
using Microsoft.Extensions.Logging;

namespace BlackRain.Logging
{
    /// <summary>
    ///     Represents a logging provider that provides file loggers through a
    ///     <see cref="T:Microsoft.Extensions.Logging.LoggerFactory" />.
    /// </summary>
    public class FileLoggerProvider : ILoggerProvider
    {
        private readonly IQueuedLogWriter _logWriter;
        private readonly FileLoggerOptions _options;

        /// <summary>
        ///     Creates a new instance of the <see cref="FileLoggerProvider" />.
        /// </summary>
        /// <param name="options">A delegate specifying the options to be used for file logging.</param>
        public FileLoggerProvider(Action<FileLoggerOptions> options)
        {
            Requires.NotNull(options, nameof(options));

            var opts = new FileLoggerOptions
            {
                MinimumLogLevel = LogLevel.Information,
                TimeStampFormat = "yyyy-MM-dd HH:mm:ss",
                LogDirectory = "Logs",
                LogExtension = ".log",
                LogName = "Log",
                TimeStampFactory = () => DateTimeOffset.UtcNow
            };

            options(opts);

            _options = opts;
            _logWriter = new FileQueuedLogWriter(opts);
        }

        /// <inheritdoc />
        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(_logWriter, categoryName, _options);
        }

        public void Dispose()
        {
            _logWriter?.Dispose();
        }
    }
}
