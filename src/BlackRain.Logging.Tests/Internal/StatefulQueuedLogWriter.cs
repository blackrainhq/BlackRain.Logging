using BlackRain.Logging.Abstractions;

namespace BlackRain.Logging.Tests.Internal
{
    public class StatefulQueuedLogWriter : IQueuedLogWriter
    {
        public LogMessage LastMessage { get; private set; }

        public void Dispose()
        {
        }

        public void Enqueue(LogMessage message)
        {
            LastMessage = message;
        }
    }
}
