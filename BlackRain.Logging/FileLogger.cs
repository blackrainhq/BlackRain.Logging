using System;
using System.Text;
using BlackRain.Logging.Abstractions;
using BlackRain.Logging.Internal;
using Microsoft.Extensions.Logging;

namespace BlackRain.Logging
{
    public class FileLogger : ILogger
    {
        private readonly string _categoryName;
        private readonly IQueuedLogWriter _logWriter;
        private readonly FileLoggerOptions _options;

        public FileLogger(IQueuedLogWriter logWriter, string categoryName, FileLoggerOptions options)
        {
            Requires.NotNull(logWriter, nameof(logWriter));
            Requires.NotEmpty(categoryName, nameof(categoryName));
            Requires.NotNull(options, nameof(options));

            _logWriter = logWriter;
            _categoryName = categoryName;
            _options = options;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            var sb = new StringBuilder();

            var timeStamp = _options.TimeStampFactory?.Invoke() ?? DateTimeOffset.UtcNow;
            var format = _options.TimeStampFormat ?? "yyyy-MM-dd HH:mm:ss";

            // May be tempting to replace this with a single interpolated string containing all the formatting and actual values,
            // but if we do that we completely defeat the purpose of the StringBuilder - intermediate strings are created under the hood
            // which are then "filled" with the results of our interpolation expressions, resulting in more allocations than necessary.
            sb.Append(timeStamp.ToString(format));
            sb.Append(" [");
            sb.Append(logLevel.ToString());
            sb.Append("] ");
            sb.Append(_categoryName);
            sb.Append(": ");
            sb.AppendLine(formatter(state, exception));

            if (exception != null)
                sb.AppendLine(exception.ToString());

            _logWriter.Enqueue(new LogMessage
            {
                Message = sb.ToString(),
                TimeStamp = timeStamp
            });
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= _options.MinimumLogLevel;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            // I have no clue how to implement this properly, given the fact that typeof(TState) can be anything.
            // This means that we would either just have to call .ToString() on state and just pray that it prints something
            // remotely useful to the log (e.g. for a ImmutableList<T>, it won't). We can implement this once we have 
            // an implementation that makes sense, and actually implements scoping in a meaningful way.
            return null;
        }
    }
}
