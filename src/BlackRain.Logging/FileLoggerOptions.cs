using System;
using Microsoft.Extensions.Logging;

namespace BlackRain.Logging
{
    public class FileLoggerOptions
    {
        /// <summary>
        ///     Specifies the minimum log level requires for messages to appear in the log.
        /// </summary>
        public LogLevel MinimumLogLevel { get; set; }

        /// <summary>
        ///     Specifies the format used to represent the timestamp of a log message. This should be a valid date and time format
        ///     string. See https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings for
        ///     more information.
        /// </summary>
        public string TimeStampFormat { get; set; }

        /// <summary>
        ///     Specifies the relative directory the log files will be stored in.
        /// </summary>
        public string LogDirectory { get; set; }

        /// <summary>
        ///     Specifies the extension to be used for log files.
        /// </summary>
        public string LogExtension { get; set; }

        /// <summary>
        ///     Specifies the name prefix used across all individual log files.
        /// </summary>
        public string LogName { get; set; }

        /// <summary>
        ///     Specifies a factory method to be used for retrieving the <see cref="DateTimeOffset" /> representation of the
        ///     current time.
        /// </summary>
        public Func<DateTimeOffset> TimeStampFactory { get; set; }
    }
}
