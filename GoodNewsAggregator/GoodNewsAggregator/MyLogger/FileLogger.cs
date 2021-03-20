using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace GoodNewsAggregator.MyLogger
{
    public class FileLogger : ILogger
    {
        private string path { get; set; }
        private object _lock = new object();

        public FileLogger(string _path)
        {
            path = _path;
        }
        
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (_lock != null)
            {
                File.AppendAllText(path, formatter(state, exception)+Environment.NewLine);
            }
        }
    }
}
