using Grpc.Core;
using MagicOnion.Server.Diagnostics;
using Microsoft.Extensions.Logging;

namespace MagicOnion.Server;

public class DuplexStreamingContext<TRequest, TResponse> : IAsyncStreamReader<TRequest>, IServerStreamWriter<TResponse>, IDisposable
{
    readonly StreamingServiceContext<TRequest, TResponse> context;
    readonly IAsyncStreamReader<TRequest> innerReader;
    readonly IAsyncStreamWriter<TResponse> innerWriter;
    readonly ILogger logger;

    internal DuplexStreamingContext(StreamingServiceContext<TRequest, TResponse> context)
    {
        this.context = context;
        this.innerReader = context.RequestStream!;
        this.innerWriter = context.ResponseStream!;
        this.logger = context.Logger;
    }

    public ServiceContext ServiceContext => context;
    
    /// <summary>
    /// IServerStreamWriter Methods.
    /// </summary>
    public WriteOptions? WriteOptions
    {
        get => innerWriter.WriteOptions;
        set => innerWriter.WriteOptions = value;
    }

    /// <summary>IAsyncStreamReader Methods.</summary>
    public TRequest Current { get; private set; } = default!; /* lateinit */

    /// <summary>IAsyncStreamReader Methods.</summary>
    public async Task<bool> MoveNext(CancellationToken cancellationToken = default(CancellationToken))
    {
        if (await innerReader.MoveNext(cancellationToken))
        {
            MagicOnionServerLog.ReadFromStream(logger, context, typeof(TRequest), false);
            this.Current = innerReader.Current;
            return true;
        }
        else
        {
            MagicOnionServerLog.ReadFromStream(logger, context, typeof(TRequest), true);
            return false;
        }
    }

    /// <summary>IAsyncStreamReader Methods.</summary>
    public void Dispose()
    {
        (innerReader as IDisposable)?.Dispose();
    }

    /// <summary>
    /// IServerStreamWriter Methods.
    /// </summary>
    public Task WriteAsync(TResponse message)
    {
        MagicOnionServerLog.WriteToStream(logger, context, typeof(TResponse));
        return innerWriter.WriteAsync(message);
    }

    public DuplexStreamingResult<TRequest, TResponse> Result()
    {
        return default(DuplexStreamingResult<TRequest, TResponse>); // dummy
    }

    public DuplexStreamingResult<TRequest, TResponse> ReturnStatus(StatusCode statusCode, string detail)
    {
        context.CallContext.Status = new Status(statusCode, detail);

        return default(DuplexStreamingResult<TRequest, TResponse>); // dummy
    }
}
