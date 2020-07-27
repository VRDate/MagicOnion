using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MagicOnion.Hosting.Logging;
using System.Reflection;

namespace MagicOnion.Hosting
{
    // ref: https://github.com/aspnet/AspNetCore/blob/4e44025a52e4b73aa17e09a8041b0e166e0c5ce0/src/DefaultBuilder/src/WebHost.cs
    /// <summary>
    /// Provides convenience methods for creating instances of <see cref="IHost"/> and <see cref="IHostBuilder"/> with pre-configured defaults.
    /// </summary>
    public static class MagicOnionHost
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HostBuilder"/> class with pre-configured defaults.
        /// </summary>
        /// <remarks>
        ///   The following defaults are applied to the returned <see cref="HostBuilder"/>:
        ///     set the <see cref="IHostingEnvironment.EnvironmentName"/> to the NETCORE_ENVIRONMENT,
        ///     load <see cref="IConfiguration"/> from 'appsettings.json' and 'appsettings.[<see cref="IHostingEnvironment.EnvironmentName"/>].json',
        ///     load <see cref="IConfiguration"/> from User Secrets when <see cref="IHostingEnvironment.EnvironmentName"/> is 'Development' using the entry assembly,
        ///     load <see cref="IConfiguration"/> from environment variables,
        ///     and configure the <see cref="SimpleConsoleLogger"/> to log to the console,
        /// </remarks>
        /// <returns>The initialized <see cref="IHostBuilder"/>.</returns>
        public static IHostBuilder CreateDefaultBuilder(bool useSimpleConsoleLogger = true) => CreateDefaultBuilder(useSimpleConsoleLogger, LogLevel.Debug);

        /// <summary>
        /// Initializes a new instance of the <see cref="HostBuilder"/> class with pre-configured defaults.
        /// </summary>
        /// <remarks>
        ///   The following defaults are applied to the returned <see cref="HostBuilder"/>:
        ///     set the <see cref="IHostingEnvironment.EnvironmentName"/> to the NETCORE_ENVIRONMENT,
        ///     load <see cref="IConfiguration"/> from 'appsettings.json' and 'appsettings.[<see cref="IHostingEnvironment.EnvironmentName"/>].json',
        ///     load <see cref="IConfiguration"/> from User Secrets when <see cref="IHostingEnvironment.EnvironmentName"/> is 'Development' using the entry assembly,
        ///     load <see cref="IConfiguration"/> from environment variables,
        ///     and configure the <see cref="SimpleConsoleLogger"/> to log to the console,
        /// </remarks>
        /// <param name="useSimpleConsoleLogger"></param>
        /// <param name="minSimpleConsoleLoggerLogLevel"></param>
        /// <returns>The initialized <see cref="IHostBuilder"/>.</returns>
        public static IHostBuilder CreateDefaultBuilder(bool useSimpleConsoleLogger, LogLevel minSimpleConsoleLoggerLogLevel) => CreateDefaultBuilder(useSimpleConsoleLogger, minSimpleConsoleLoggerLogLevel, "");

        /// <summary>
        /// Initializes a new instance of the <see cref="HostBuilder"/> class with pre-configured defaults.
        /// </summary>
        /// <remarks>
        ///   The following defaults are applied to the returned <see cref="HostBuilder"/>:
        ///     set the <see cref="IHostingEnvironment.EnvironmentName"/> to the parameter of hostEnvironmentVariable,
        ///     load <see cref="IConfiguration"/> from 'appsettings.json' and 'appsettings.[<see cref="IHostingEnvironment.EnvironmentName"/>].json',
        ///     load <see cref="IConfiguration"/> from User Secrets when <see cref="IHostingEnvironment.EnvironmentName"/> is 'Development' using the entry assembly,
        ///     load <see cref="IConfiguration"/> from environment variables,
        ///     and configure the <see cref="SimpleConsoleLogger"/> to log to the console,
        /// </remarks>
        /// <param name="useSimpleConsoleLogger"></param>
        /// <param name="minSimpleConsoleLoggerLogLevel"></param>
        /// <param name="hostEnvironmentVariable"></param>
        /// <returns>The initialized <see cref="IHostBuilder"/>.</returns>
        public static IHostBuilder CreateDefaultBuilder(bool useSimpleConsoleLogger, LogLevel minSimpleConsoleLoggerLogLevel, string hostEnvironmentVariable)
        {
            var builder = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder();

            ConfigureHostConfigurationDefault(builder, hostEnvironmentVariable);
            ConfigureLoggingDefault(builder, useSimpleConsoleLogger, minSimpleConsoleLoggerLogLevel);

            return builder;
        }

        internal static void ConfigureHostConfigurationDefault(IHostBuilder builder, string hostEnvironmentVariable)
        {
            builder.ConfigureHostConfiguration(config =>
            {
                // NOTE: This is backward compatibility for older versions.
                // It's strongly recommended to use "DOTNET_" prefix expected by GenericHost. (e.g. DOTNET_ENVIRONMENT)
                config.AddEnvironmentVariables(prefix: "NETCORE_");

                config.AddInMemoryCollection(new[] { new KeyValuePair<string, string>(HostDefaults.ApplicationKey, Assembly.GetExecutingAssembly().GetName().Name) });
            });

            if (!string.IsNullOrWhiteSpace(hostEnvironmentVariable))
            {
                builder.UseEnvironment(System.Environment.GetEnvironmentVariable(hostEnvironmentVariable) ?? "Production");
            }
        }

        internal static void ConfigureLoggingDefault(IHostBuilder builder, bool useSimpleConsoleLogger, LogLevel minSimpleConsoleLoggerLogLevel)
        {
            if (useSimpleConsoleLogger)
            {
                builder.ConfigureLogging(logging =>
                {
                    // Use SimpleConsoleLogger instead of the default ConsoleLogger.
                    var consoleLogger = logging.Services.FirstOrDefault(x => x.ImplementationType?.FullName == "Microsoft.Extensions.Logging.Console.ConsoleLoggerProvider");
                    if (consoleLogger != null)
                    {
                        logging.Services.Remove(consoleLogger);
                    }

                    logging.AddSimpleConsole();
                    logging.AddFilter((providerName, category, level) =>
                    {
                        if (providerName == typeof(SimpleConsoleLogger).FullName)
                        {
                            // omit system message
                            if (category.StartsWith("Microsoft.Extensions.Hosting.Internal"))
                            {
                                if (level <= LogLevel.Debug) return false;
                            }

                            return level >= minSimpleConsoleLoggerLogLevel;
                        }

                        return true;
                    });
                });
            }
        }
    }
}