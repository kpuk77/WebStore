using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace WebStore.Logger
{
    public static class Log4NetFactory
    {
        private static string CheckPath(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException("Не указан путь к файлу конфигурации");

            if (Path.IsPathRooted(filePath))
                return filePath;

            var assembly = Assembly.GetEntryAssembly();
            var dir = Path.GetDirectoryName(assembly!.Location);

            return Path.Combine(dir!, filePath);
        }

        public static ILoggerFactory AddLog4Net(this ILoggerFactory factory, string configuration = "log4net.config")
        {
            factory.AddProvider(new Log4NetProvider(CheckPath(configuration)));

            return factory;
        }
    }
}