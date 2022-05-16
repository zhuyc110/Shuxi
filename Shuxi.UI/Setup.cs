using Microsoft.Extensions.Logging;
using MvvmCross.Platforms.Wpf.Core;
using Serilog;
using Serilog.Extensions.Logging;
using System.IO;

namespace Shuxi.UI
{
    internal class Setup : MvxWpfSetup<Core.App>
    {
        protected override ILoggerProvider CreateLogProvider()
        {
            return new SerilogLoggerProvider();
        }

        protected override ILoggerFactory CreateLogFactory()
        {
            var logPath = $"{Directory.GetCurrentDirectory()}\\Logs\\Log.txt";

            // serilog configuration
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Trace()
                .WriteTo
                    .Async(_ => _
                        .File(
                            logPath,
                            rollingInterval: RollingInterval.Day,
                            outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] [{Level:u4}] {Message} <{SourceContext}>{NewLine}{Exception}",
                            rollOnFileSizeLimit: true,
                            fileSizeLimitBytes: 2097152
                        ))
                .CreateLogger();

            return new SerilogLoggerFactory();
        }
    }
}
