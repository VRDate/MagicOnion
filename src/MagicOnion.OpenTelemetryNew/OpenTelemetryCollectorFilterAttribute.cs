using System;
using System.Threading.Tasks;
using MagicOnion.Server;
using OpenTelemetry.Trace;
using OpenTelemetry.Trace.Configuration;

namespace MagicOnion.OpenTelemetry
{
    /// <summary>
    /// Global filter. Handle Unary and most outside logging.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class OpenTelemetryCollectorFilterAttribute : Attribute, IMagicOnionFilterFactory<MagicOnionFilterAttribute>
    {
        public int Order { get; set; }

        public MagicOnionFilterAttribute CreateInstance(IServiceLocator serviceLocator)
        {
            return new OpenTelemetryCollectorFilter(serviceLocator.GetService<TracerFactory>(), serviceLocator.GetService<MagicOnionOpenTelemetryOptions>());
        }
    }

    internal class OpenTelemetryCollectorFilter : MagicOnionFilterAttribute
    {
        readonly TracerFactory tracerFactcory;
        readonly string serviceName;

        public OpenTelemetryCollectorFilter(TracerFactory tracerFactory, MagicOnionOpenTelemetryOptions telemetryOption)
        {
            this.tracerFactcory = tracerFactory;
            this.serviceName = telemetryOption.ServiceName;
        }

        public override async ValueTask Invoke(ServiceContext context, Func<ServiceContext, ValueTask> next)
        {
            // https://github.com/open-telemetry/opentelemetry-specification/blob/master/specification/trace/semantic_conventions/rpc.md#grpc

            // span name must be `$package.$service/$method` but MagicOnion has no $package.
            var tracer = tracerFactcory.GetTracer(context.CallContext.Method);

            // incoming kind: SERVER
            using (tracer.StartActiveSpan($"{context.CallContext.Method}", SpanKind.Server, out var span))
            {
                try
                {
                    span.SetAttribute("rpc.service", serviceName);
                    span.SetAttribute("net.peer.ip", context.CallContext.Peer);
                    span.SetAttribute("net.host.name", context.CallContext.Host);
                    span.SetAttribute("message.type", "RECIEVED");
                    span.SetAttribute("message.id", context.ContextId.ToString());
                    span.SetAttribute("message.uncompressed_size", context.GetRawRequest()?.LongLength ?? 0);

                    span.SetAttribute("magiconion.method.type", context.MethodType.ToString());
                    span.SetAttribute("magiconion.service.type", context.ServiceType.Name);
                    span.SetAttribute("magiconion.auth.enabled", !string.IsNullOrEmpty(context.CallContext.AuthContext.PeerIdentityPropertyName));
                    span.SetAttribute("magiconion.auth.peer.authenticated", context.CallContext.AuthContext.IsPeerAuthenticated);

                    await next(context);

                    span.SetAttribute("grpc.status_code", (long)context.CallContext.Status.StatusCode);
                    span.Status = OpenTelemetrygRpcStatusHelper.ConvertStatus(context.CallContext.Status.StatusCode).WithDescription(context.CallContext.Status.Detail);
                }
                catch (Exception ex)
                {
                    span.SetAttribute("exception", ex.ToString());
                    span.SetAttribute("grpc.status_code", (long)context.CallContext.Status.StatusCode);
                    span.Status = OpenTelemetrygRpcStatusHelper.ConvertStatus(context.CallContext.Status.StatusCode).WithDescription(context.CallContext.Status.Detail);
                }
            }
        }
    }
}