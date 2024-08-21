using log4net.Appender;
using log4net.Layout;
using log4net.Repository;
using log4net;
using System.IO;
using System;

namespace AtmWebAppTesting.Helpers
{
    public class LoggingHelper
    {
        public static void ConfigureLogging()
        {
            string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logging");
            Directory.CreateDirectory(logDirectory);
            string filePath = Path.Combine(logDirectory, $"logFile_{DateTime.Now:yyyyMMdd_HHmmss}.log");

            // Define a simple layout for the logs
            PatternLayout layout = new PatternLayout
            {
                ConversionPattern = "%date [%thread] %-5level %logger - %message%newline"
            };
            layout.ActivateOptions();

            // Set up console appender
            ConsoleAppender consoleAppender = new ConsoleAppender
            {
                Layout = layout
            };
            consoleAppender.ActivateOptions();

            // Set up file appender
            RollingFileAppender fileAppender = new RollingFileAppender
            {
                AppendToFile = true,
                File = filePath,
                Layout = layout,
                MaxSizeRollBackups = 5,
                MaximumFileSize = "1MB",
                RollingStyle = RollingFileAppender.RollingMode.Size,
                StaticLogFileName = true
            };
            fileAppender.ActivateOptions();

            // Create the repository and configure it
            ILoggerRepository repository = LogManager.CreateRepository("NETCoreRepository");
            log4net.Config.BasicConfigurator.Configure(repository, consoleAppender, fileAppender);
        }
    }
}
