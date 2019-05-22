using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BlackRain.Logging.Abstractions;
using BlackRain.Logging.Internal;

namespace BlackRain.Logging
{
    /// <summary>
    ///     Represents a log queued writer that writes messages in the log queue to file.
    /// </summary>
    public class FileQueuedLogWriter : IQueuedLogWriter
    {
        private readonly TimeSpan _baseIntervalTime;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly ConcurrentQueue<LogMessage> _loggingQueue;
        private readonly FileLoggerOptions _options;

        private Task _processingTask;

        public FileQueuedLogWriter(FileLoggerOptions options)
        {
            Requires.NotNull(options, nameof(options));

            _options = options;
            _cancellationTokenSource = new CancellationTokenSource();
            _loggingQueue = new ConcurrentQueue<LogMessage>();
            _baseIntervalTime = TimeSpan.FromSeconds(5);

            _processingTask = Task.Run(ProcessQueueAsync);
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        public void Enqueue(LogMessage message)
        {
            Requires.NotNull(message, nameof(message));

            _loggingQueue.Enqueue(message);
        }

        private async Task ProcessQueueAsync()
        {
            EnsureDirectoryCreated();

            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                // Nothing to do, we can just idle the full base idle time.
                if (_loggingQueue.IsEmpty)
                    await Task.Delay(_baseIntervalTime);

                var roundTimer = Stopwatch.StartNew();

                var messages = new List<LogMessage>();
                while (_loggingQueue.TryDequeue(out var message))
                    messages.Add(message);

                // We want to create a new log every day. This should work, as long as we're not logging
                // messages from last month all of a sudden.
                foreach (var logs in messages.GroupBy(message => message.TimeStamp.Day))
                {
                    var log = logs.FirstOrDefault();

                    if (log == null)
                        continue;

                    var fileName = $"{_options.LogName} {log.TimeStamp:yyyy-M-d}{_options.LogExtension}";
                    var fileLocation = Path.Combine(_options.LogDirectory, fileName);

                    using (var sw = File.AppendText(fileLocation))
                    {
                        foreach (var entry in logs) await sw.WriteAsync(entry.Message);
                    }
                }

                roundTimer.Stop();

                var sleepTime = CalculateRemainingSleepTime(roundTimer.Elapsed);

                await Task.Delay(sleepTime);
            }
        }

        private TimeSpan CalculateRemainingSleepTime(TimeSpan roundTime)
        {
            // Last batch took longer than the base idle time - we're probably processing a burst of messages.
            // Best keep at it.
            if (roundTime >= _baseIntervalTime)
                return TimeSpan.Zero;

            return _baseIntervalTime - roundTime;
        }

        private void EnsureDirectoryCreated()
        {
            var dir = _options.LogDirectory ?? "Logs";

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }
    }
}
