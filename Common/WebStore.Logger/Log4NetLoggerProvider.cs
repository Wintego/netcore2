using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml;
using log4net;
using log4net.Core;
using Microsoft.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace WebStore.Logger
{
    public class Log4NetLoggerProvider : ILoggerProvider
    {
        private readonly string _configurationFile;

        private readonly ConcurrentDictionary<string, Log4NetLogger> _Loggers =
            new ConcurrentDictionary<string, Log4NetLogger>();
        public Log4NetLoggerProvider(string ConfigurationFile)
        {
            _configurationFile = ConfigurationFile;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return _Loggers.GetOrAdd(categoryName, category =>
            {
                var xml = new XmlDocument();
                var file_name = _configurationFile;
                xml.Load(file_name);
                return new Log4NetLogger(category, xml["log4net"]);
            });
        }

        public void Dispose()
        {
            _Loggers.Clear();
        }
    }

    public class Log4NetLogger : ILogger
    {
        private readonly ILog _Log;
        public Log4NetLogger(string CategoryName, XmlElement Configuration)
        {
            var logger_repository = LoggerManager.CreateRepository(
                Assembly.GetEntryAssembly(),
                typeof(log4net.Repository.Hierarchy.Hierarchy)
            );
            _Log = LogManager.GetLogger(logger_repository.Name, CategoryName);
            log4net.Config.XmlConfigurator.Configure(logger_repository, Configuration);
        }
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if(!IsEnabled(logLevel)) return;
            if(formatter is null) throw  new ArgumentNullException(nameof(formatter));

            var msg = formatter(state, exception);
            if(string.IsNullOrEmpty(msg) && exception is null) return;

            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    _Log.Debug(msg);
                    break;
                case LogLevel.Information:
                    _Log.Info(msg);
                    break;
                case LogLevel.Warning:
                    _Log.Warn(msg);
                    break;
                case LogLevel.Error:
                    _Log.Error(msg ?? exception.ToString());
                    break;
                case LogLevel.Critical:
                    _Log.Fatal(msg ?? exception.ToString());

                    break;
                case LogLevel.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
            }
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    return _Log.IsDebugEnabled;
                case LogLevel.Information:
                    return _Log.IsInfoEnabled;
                case LogLevel.Warning:
                    return _Log.IsWarnEnabled;
                case LogLevel.Error:
                    return _Log.IsErrorEnabled;
                case LogLevel.Critical:
                    return _Log.IsFatalEnabled;
                case LogLevel.None:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
            }
        }

        public IDisposable BeginScope<TState>(TState state) => null;
    }
}
