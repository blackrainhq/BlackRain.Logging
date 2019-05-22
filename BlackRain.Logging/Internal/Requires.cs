using System;

namespace BlackRain.Logging.Internal
{
    internal static class Requires
    {
        /// <summary>
        ///     Requires the specified value to be non-null, and throws an <see cref="ArgumentNullException" /> if it fails.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="parameterName"></param>
        public static void NotNull<T>(T value, string parameterName) where T : class
        {
            if (value == null)
                throw new ArgumentNullException(parameterName);
        }

        /// <summary>
        ///     Requires the specified value to be non-null, nor empty, and throws a <see cref="ArgumentNullException" /> if it
        ///     fails.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="parameterName"></param>
        public static void NotEmpty(string value, string parameterName)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(parameterName);
        }
    }
}
