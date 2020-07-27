using Grpc.Core;
using MagicOnion.Hosting;
using MagicOnion.OpenTelemetry;
using MagicOnion.Server;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Exporter.Prometheus;
using OpenTelemetry.Metrics.Configuration;
using OpenTelemetry.Trace.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ChatApp.Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //GrpcEnvironment.SetLogger(new Grpc.Core.Logging.ConsoleLogger());

            await MagicOnionHost.CreateDefaultBuilder()
                .UseMagicOnion()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMagicOnionOpenTelemetry((options, meterOptions) =>
                    {
                        // open-telemetry with Prometheus exporter
                        meterOptions.MetricExporter = new PrometheusExporter(new PrometheusExporterOptions() { Url = options.MetricsExporterEndpoint });
                    },
                    (options, tracerBuilder) =>
                    {
                        // open-telemetry with Zipkin exporter
                        tracerBuilder.UseZipkin(o =>
                        {
                            o.ServiceName = options.ServiceName;
                            o.Endpoint = new Uri(options.TracerExporterEndpoint);
                        });
                    });
                    services.AddHostedService<PrometheusExporterMetricsService>();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    var meterFactory = services.BuildServiceProvider().GetService<MeterFactory>();
                    services.Configure<MagicOnionHostingOptions>(options =>
                    {
                        options.Service.GlobalFilters.Add(new OpenTelemetryCollectorFilterAttribute());
                        options.Service.GlobalStreamingHubFilters.Add(new OpenTelemetryHubCollectorFilterAttribute());
                        options.Service.MagicOnionLogger = new OpenTelemetryCollectorLogger(meterFactory);
                    });
                })
                .RunConsoleAsync();
        }
    }
}
