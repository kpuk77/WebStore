using System;
using System.Reflection;
using System.Xml;

using log4net;

using Microsoft.Extensions.Logging;

namespace WebStore.Logger
{
    public class Log4NetLogger : ILogger
    {
        private readonly ILog _Log;

        public Log4NetLogger(string category, XmlElement configuration)
        {
            var loggerRepository = LogManager.CreateRepository(Assembly.GetEntryAssembly(),
                typeof(log4net.Repository.Hierarchy.Hierarchy));

            _Log = LogManager.GetLogger(loggerRepository.Name, category);

            log4net.Config.XmlConfigurator.Configure(loggerRepository, configuration);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter is null)
                throw new ArgumentNullException(nameof(formatter));

            if(!IsEnabled(logLevel))
                return;

            var logMessage = formatter(state, exception);

            if (string.IsNullOrEmpty(logMessage) && exception is null)
                return;

            switch (logLevel)
            {
                default: throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);

                case LogLevel.None: break;

                case LogLevel.Trace:
                case LogLevel.Debug:
                    _Log.Debug(logMessage);
                    break;

                case LogLevel.Information: 
                    _Log.Info(logMessage);
                    break;

                case LogLevel.Warning:
                    _Log.Warn(logMessage);
                    break;

                case LogLevel.Error:
                    _Log.Error(logMessage);
                    break;

                case LogLevel.Critical:
                    _Log.Fatal(logMessage);
                    break;
            }
        }

        public bool IsEnabled(LogLevel logLevel) => logLevel switch
        {
            LogLevel.None => false,
            LogLevel.Trace => _Log.IsDebugEnabled,
            LogLevel.Debug => _Log.IsDebugEnabled,
            LogLevel.Information => _Log.IsInfoEnabled,
            LogLevel.Warning => _Log.IsWarnEnabled,
            LogLevel.Error => _Log.IsErrorEnabled,
            LogLevel.Critical => _Log.IsFatalEnabled,
            _ => throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null)
        };

        public IDisposable BeginScope<TState>(TState state) => null;
    }
}
