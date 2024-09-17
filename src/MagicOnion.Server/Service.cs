using Grpc.Core;
using MagicOnion.Server.Diagnostics;
using MessagePack;

namespace MagicOnion.Server;

public abstract class ServiceBase<TServiceInterface> : IService<TServiceInterface>
    where TServiceInterface : IServiceMarker
{
    // NOTE: Properties `Context` and `Metrics` are set by an internal setter during instance activation of the service.
    //       For details, please refer to `ServiceProviderHelper.CreateService`.
    public ServiceContext Context { get; internal set; }
    internal MagicOnionMetrics Metrics { get; set; }

    public ServiceBase()
    {
        this.Context = default!;
        this.Metrics = default!;
    }

    // Helpers

    protected TResponse ReturnStatusCode<TResponse>(int statusCode, string detail)
    {
        Context.CallContext.Status = new Status((StatusCode)statusCode, detail ?? "");
#pragma warning disable CS8603 // Possible null reference return.
        return default;
#pragma warning restore CS8603 // Possible null reference return.
    }

    protected UnaryResult<TResponse> ReturnStatus<TResponse>(StatusCode statusCode, string detail)
    {
        Context.CallContext.Status = new Status(statusCode, detail ?? "");
        return default(UnaryResult<TResponse>); // dummy
    }

    [Ignore]
    public ClientStreamingContext<TRequest, TResponse> GetClientStreamingContext<TRequest, TResponse>()
        => new ClientStreamingContext<TRequest, TResponse>((StreamingServiceContext<TRequest, Nil /* Dummy */>)Context);

    [Ignore]
    public ServerStreamingContext<TResponse> GetServerStreamingContext<TResponse>()
        => new ServerStreamingContext<TResponse>((StreamingServiceContext<Nil /* Dummy */, TResponse>)Context);

    [Ignore]
    public DuplexStreamingContext<TRequest, TResponse> GetDuplexStreamingContext<TRequest, TResponse>()
        => new DuplexStreamingContext<TRequest, TResponse>((StreamingServiceContext<TRequest, TResponse>)Context);

    // Interface methods for Client

    TServiceInterface IService<TServiceInterface>.WithOptions(CallOptions option)
    {
        throw new NotSupportedException("Invoke from client proxy only");
    }

    TServiceInterface IService<TServiceInterface>.WithHeaders(Metadata headers)
    {
        throw new NotSupportedException("Invoke from client proxy only");
    }

    TServiceInterface IService<TServiceInterface>.WithDeadline(DateTime deadline)
    {
        throw new NotSupportedException("Invoke from client proxy only");
    }

    TServiceInterface IService<TServiceInterface>.WithCancellationToken(CancellationToken cancellationToken)
    {
        throw new NotSupportedException("Invoke from client proxy only");
    }

    TServiceInterface IService<TServiceInterface>.WithHost(string host)
    {
        throw new NotSupportedException("Invoke from client proxy only");
    }
}
