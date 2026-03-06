using WsElecciones.CrossCutting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;

namespace WsElecciones.Api.Middleware
{
    public static class LogginConfiguration
    {
        public static void AddLogging(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog();
            builder.Services.AddLogging(x => x.AddSerilog());
            builder.Logging.AddConsole();
        }
        public static Serilog.Core.Logger AddLoggerConfiguration(this WebApplication app)
        {
            var filepath = AppContext.BaseDirectory;
            return new LoggerConfiguration()
                .MinimumLevel.Is(LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Async(x =>
                {
                    if (Constants.IsInDevelopment)
                    {
                        //x.Console();
                    }

                })
                .WriteTo.Conditional(x => x.Level is LogEventLevel.Information or LogEventLevel.Warning, conf =>
                {
                    if (!Constants.IsInDevelopment)
                    {
                        conf.Async(x =>
                        {
                            x.File($"{filepath}\\ApiCtaCte.log",
                                rollingInterval: RollingInterval.Day,
                                rollOnFileSizeLimit: true,
                                retainedFileCountLimit: 2,
                                fileSizeLimitBytes: 100 * 1024 * 1024);
                        });
                    }
                })
                .WriteTo.Conditional(x => x.Level is LogEventLevel.Fatal or LogEventLevel.Error, conf =>
                {
                    if (!Constants.IsInDevelopment)
                    {
                        conf.Async(x =>
                        {
                            x.MSSqlServer(app.Configuration.GetSection("ConnectionStrings:DefaultConnection").Value,
                                sinkOptions: new MSSqlServerSinkOptions
                                {
                                    TableName = "Logs",
                                    AutoCreateSqlTable = true
                                },
                                columnOptions: new ColumnOptions
                                {
                                    AdditionalColumns = new Collection<SqlColumn>
                                    {
                                    new() { ColumnName = "AppName", DataType= System.Data.SqlDbType.NVarChar, DataLength = 100 },
                                    new() { ColumnName = "Username", DataType= System.Data.SqlDbType.NVarChar, DataLength = 20 },
                                    new() { ColumnName = "TraceIdentifier", DataType= System.Data.SqlDbType.NVarChar, DataLength = 100 },
                                    new() { ColumnName = "QueryString", DataType= System.Data.SqlDbType.NVarChar, DataLength = -1 },
                                    new() { ColumnName = "RequestBody", DataType= System.Data.SqlDbType.NVarChar, DataLength = -1 },
                                    new() { ColumnName = "RouteParams", DataType= System.Data.SqlDbType.NVarChar, DataLength = -1 },
                                    }
                                });
                        });
                    }
                })
                .CreateLogger();
        }
    }
}
