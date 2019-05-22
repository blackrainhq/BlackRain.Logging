using System;

namespace BlackRain.Logging.Abstractions
{
    /// <summary>
    ///     Interface all types representing a log writer should implement.
    /// </summary>
    public interface IQueuedLogWriter : IDisposable
    {
        void Enqueue(LogMessage message);
    }
}
