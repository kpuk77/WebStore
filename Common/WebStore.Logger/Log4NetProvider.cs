using System.Collections.Concurrent;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace WebStore.Logger
{
    public class Log4NetProvider : ILoggerProvider
    {
        private readonly string _Configuration;
        private readonly ConcurrentDictionary<string, Log4NetLogger> _Loggers = new();

        public Log4NetProvider(string configuration) => _Configuration = configuration;

        public void Dispose() => _Loggers.Clear();

        public ILogger CreateLogger(string categoryName) =>
            _Loggers.GetOrAdd(categoryName, c =>
            {
                var xml = new XmlDocument();
                xml.Load(_Configuration);
                return new Log4NetLogger(c, xml["log4net"]);
            });
    }
}