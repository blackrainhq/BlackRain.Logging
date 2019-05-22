using System;
using BlackRain.Logging.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BlackRain.Logging
{
    /// <summary>
    ///     Contains extension methods to enable file logging support through <see cref="ILoggingBuilder" />.
    /// </summary>
    public static class LoggingBuilderExtensions
    {
        /// <summary>
        ///     Registers the file logging provider with its default options.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ILoggingBuilder AddFileLogging(this ILoggingBuilder builder)
        {
            builder.Services.AddSingleton<ILoggerProvider, FileLoggerProvider>();

            return builder;
        }

        /// <summary>
        ///     Registers the file logging provider with the specified options.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static ILoggingBuilder AddFileLogging(this ILoggingBuilder builder, Action<FileLoggerOptions> options)
        {
            Requires.NotNull(options, nameof(options));

            // Because we're currently not injection IOptions<FileLoggerOptions> into the logger provider,
            // we can't rely on Services.Configure(options) here. Instead, we'll construct a logger provider
            // by passing it the options lambda directly, and then register that instance as a singleton.
            var loggerProvider = new FileLoggerProvider(options);

            builder.Services.AddSingleton<ILoggerProvider>(loggerProvider);

            return builder;
        }

        /// <summary>
        ///     Registers the file logging provider with the specified logger factory.
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static ILoggerFactory AddFileLogging(this ILoggerFactory factory)
        {
            return AddFileLogging(factory, options => { });
        }

        /// <summary>
        ///     Registers the file logging provider with the specified logger factory using the provided options.
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static ILoggerFactory AddFileLogging(this ILoggerFactory factory, Action<FileLoggerOptions> options)
        {
            Requires.NotNull(options, nameof(options));

            factory.AddProvider(new FileLoggerProvider(options));

            return factory;
        }
    }
}
