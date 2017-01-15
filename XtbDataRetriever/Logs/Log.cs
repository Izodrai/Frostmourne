using NLog;
using NLog.Targets;
using NLog.Config;

namespace XtbDataRetriever.Logs
{
    class Log
    {
        /*
        log.Trace("trace log message");
        log.Debug("debug log message");
        log.Info("info log message");
        log.Warn("warn log message");
        log.Error("error log message");
        log.Fatal("fatal log message");
        */

        public static Logger InitLog()
        {
            // Step 1. Create configuration object 
            var config = new LoggingConfiguration();

            // Step 2. Create targets and add them to the configuration 
            var consoleTarget = new ColoredConsoleTarget();
            config.AddTarget("console", consoleTarget);

            // Step 3. Set target properties 
            consoleTarget.Layout = @"${date:format=yyyy\-MM\-dd HH\:mm\:ss} ${level} ->> ${message}";

            // Step 4. Define rules
            var rule1 = new LoggingRule("*", LogLevel.Debug, consoleTarget);
            config.LoggingRules.Add(rule1);

            // Step 5. Activate the configuration
            LogManager.Configuration = config;

            // usage
            return LogManager.GetLogger("");
        }
    }
}
