using System;
using System.Collections.Generic;
using System.Text;
using Grpc.AspNetCore.Server.Model;
using Grpc.Core;
using Microsoft.AspNetCore.Routing;

namespace MagicOnion.Server.Glue
{
    internal class MagicOnionGlueServiceBinder<TService> : ServiceBinderBase
        where TService : class
    {
        private readonly ServiceMethodProviderContext<TService> _context;

        public MagicOnionGlueServiceBinder(ServiceMethodProviderContext<TService> context)
        {
            _context = context;
        }

        private IList<object> GetMetadataFromHandler(Delegate handler)
        {
            var methodHandler = ((MethodHandler)handler.Target!);
            var serviceType = methodHandler.ServiceType;

            // NOTE: We need to collect Attributes for Endpoint metadata. ([Authorize], [AllowAnonymous] ...)
            // https://github.com/grpc/grpc-dotnet/blob/7ef184f3c4cd62fbc3cde55e4bb3e16b58258ca1/src/Grpc.AspNetCore.Server/Model/Internal/ProviderServiceBinder.cs#L89-L98
            var metadata = new List<object>();
            metadata.AddRange(serviceType.GetCustomAttributes(inherit: true));
            metadata.AddRange(methodHandler.MethodInfo.GetCustomAttributes(inherit: true));

            metadata.Add(new HttpMethodMetadata(new[] { "POST" }, acceptCorsPreflight: true));
            return metadata;
        }

        public override void AddMethod<TRequest, TResponse>(Method<TRequest, TResponse> method, UnaryServerMethod<TRequest, TResponse> handler)
        {
            _context.AddUnaryMethod(method, GetMetadataFromHandler(handler), (_, request, context) => handler(request, context));
        }

        public override void AddMethod<TRequest, TResponse>(Method<TRequest, TResponse> method, ClientStreamingServerMethod<TRequest, TResponse> handler)
        {
            _context.AddClientStreamingMethod(method, GetMetadataFromHandler(handler), (_, request, context) => handler(request, context));
        }

        public override void AddMethod<TRequest, TResponse>(Method<TRequest, TResponse> method, ServerStreamingServerMethod<TRequest, TResponse> handler)
        {
            _context.AddServerStreamingMethod(method, GetMetadataFromHandler(handler), (_, request, stream, context) => handler(request, stream, context));
        }

        public override void AddMethod<TRequest, TResponse>(Method<TRequest, TResponse> method, DuplexStreamingServerMethod<TRequest, TResponse> handler)
        {
            _context.AddDuplexStreamingMethod(method, GetMetadataFromHandler(handler), (_, request, stream, context) => handler(request, stream, context));
        }
    }
}
