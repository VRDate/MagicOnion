using Grpc.Core;
using MagicOnion.Internal;
using NSubstitute;

namespace MagicOnion.Client.Tests;

public class UnaryTest
{
    [Fact]
    public void Create()
    {
        // Arrange
        var callInvokerMock = Substitute.For<CallInvoker>();

        // Act
        var client = MagicOnionClient.Create<IUnaryTestService>(callInvokerMock);

        // Assert
        client.Should().NotBeNull();
    }

    [Fact]
    public async Task Clone_WithOptions()
    {
        // Arrange
        var actualCallOptions = default(CallOptions);
        var callInvokerMock = Substitute.For<CallInvoker>();
        callInvokerMock.AsyncUnaryCall(default(Method<Box<Nil>, Box<Nil>>)!, default, default, default!)
            .ReturnsForAnyArgs(x =>
            {
                // method, host, callOptions, request
                var callOptions = x.ArgAt<CallOptions>(2);
                actualCallOptions = callOptions;
                return new AsyncUnaryCall<Box<Nil>>(Task.FromResult(Box.Create(Nil.Default)), Task.FromResult(Metadata.Empty), () => Status.DefaultSuccess, () => Metadata.Empty, () => { });
            });
        var client = MagicOnionClient.Create<IUnaryTestService>(callInvokerMock);

        // Act
        client = client.WithOptions(new CallOptions(new Metadata() { { "foo", "bar" } }));
        await client.ParameterlessNonGenericReturnType();

        // Assert
        client.Should().NotBeNull();
        callInvokerMock.Received();
        actualCallOptions.Headers.Should().Contain(x => x.Key == "foo" && x.Value == "bar");
    }

    [Fact]
    public async Task Clone_WithHost()
    {
        // Arrange
        var callInvokerMock = Substitute.For<CallInvoker>();
        callInvokerMock.AsyncUnaryCall(Arg.Any<Method<Box<Nil>, Box<Nil>>>(), "www.example.com", Arg.Any<CallOptions>(), Arg.Any<Box<Nil>>())
            .Returns(new AsyncUnaryCall<Box<Nil>>(Task.FromResult(Box.Create(Nil.Default)), Task.FromResult(Metadata.Empty), () => Status.DefaultSuccess, () => Metadata.Empty, () => { }));
        var client = MagicOnionClient.Create<IUnaryTestService>(callInvokerMock);

        // Act
        client = client.WithHost("www.example.com");
        await client.ParameterlessNonGenericReturnType();

        // Assert
        client.Should().NotBeNull();
        callInvokerMock.Received();
    }

    [Fact]
    public async Task Clone_WithCancellationToken()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        var actualCancellationToken = default(CancellationToken);
        var callInvokerMock = Substitute.For<CallInvoker>();
        callInvokerMock.AsyncUnaryCall(default(Method<Box<Nil>, Box<Nil>>)!, default, default, default!)
            .ReturnsForAnyArgs(x =>
            {
                // method, host, callOptions, request
                var callOptions = x.ArgAt<CallOptions>(2);
                actualCancellationToken = callOptions.CancellationToken;
                return new AsyncUnaryCall<Box<Nil>>(Task.FromResult(Box.Create(Nil.Default)), Task.FromResult(Metadata.Empty), () => Status.DefaultSuccess, () => Metadata.Empty, () => { });
            });
        var client = MagicOnionClient.Create<IUnaryTestService>(callInvokerMock);

        // Act
        client = client.WithCancellationToken(cts.Token);
        await client.ParameterlessNonGenericReturnType();

        // Assert
        client.Should().NotBeNull();
        callInvokerMock.Received();
        actualCancellationToken.Should().Be(cts.Token);
    }
    
    [Fact]
    public async Task Clone_WithHeaders()
    {
        // Arrange
        var actualHeaders = default(Metadata);
        var callInvokerMock = Substitute.For<CallInvoker>();
        callInvokerMock.AsyncUnaryCall(default(Method<Box<Nil>, Box<Nil>>)!, default, default, default!)
            .ReturnsForAnyArgs(x =>
            {
                // method, host, callOptions, request
                var callOptions = x.ArgAt<CallOptions>(2);
                actualHeaders = callOptions.Headers;
                return new AsyncUnaryCall<Box<Nil>>(Task.FromResult(Box.Create(Nil.Default)), Task.FromResult(Metadata.Empty), () => Status.DefaultSuccess, () => Metadata.Empty, () => { });
            });
        var client = MagicOnionClient.Create<IUnaryTestService>(callInvokerMock);

        // Act
        client = client.WithHeaders(new Metadata() { { "foo", "bar" }});
        await client.ParameterlessNonGenericReturnType();

        // Assert
        client.Should().NotBeNull();
        callInvokerMock.Received();
        actualHeaders.Should().Contain(x => x.Key == "foo" && x.Value == "bar");
    }

    [Fact]
    public async Task ParameterlessRequestsNil()
    {
        // Arrange
        var serializedResponse = new ReadOnlyMemory<byte>();
        var callInvokerMock = Substitute.For<CallInvoker>();
        callInvokerMock.AsyncUnaryCall(default(Method<Box<Nil>, Box<Nil>>)!, default, default, default!)
            .ReturnsForAnyArgs(x =>
            {
                // method, host, callOptions, request
                var method = x.ArgAt<Method<Box<Nil>, Box<Nil>>>(0);

                var serializationContext = new MockSerializationContext();
                method.RequestMarshaller.ContextualSerializer(Box.Create(Nil.Default), serializationContext);
                serializedResponse = serializationContext.ToMemory();
                return new AsyncUnaryCall<Box<Nil>>(Task.FromResult(Box.Create(Nil.Default)), Task.FromResult(Metadata.Empty), () => Status.DefaultSuccess, () => Metadata.Empty, () => { });
            });

        // Act
        var client = MagicOnionClient.Create<IUnaryTestService>(callInvokerMock);
        var result = await client.ParameterlessReturnNil();

        // Assert
        result.Should().Be(Nil.Default);
        callInvokerMock.Received();
        serializedResponse.ToArray().Should().BeEquivalentTo(new[] { MessagePackCode.Nil });
    }

    [Fact]
    public async Task ParameterlessReturnNil()
    {
        // Arrange
        var callInvokerMock = Substitute.For<CallInvoker>();
        callInvokerMock.AsyncUnaryCall(Arg.Any<Method<Box<Nil>, Box<Nil>>>(), Arg.Any<string>(), Arg.Any<CallOptions>(), Arg.Is<Box<Nil>>(x => x.Value.Equals(Nil.Default)))
            .Returns(new AsyncUnaryCall<Box<Nil>>(Task.FromResult(Box.Create(Nil.Default)), Task.FromResult(Metadata.Empty), () => Status.DefaultSuccess, () => Metadata.Empty, () => { }));

        // Act
        var client = MagicOnionClient.Create<IUnaryTestService>(callInvokerMock);
        var result = await client.ParameterlessReturnNil();

        // Assert
        result.Should().Be(Nil.Default);
        callInvokerMock.Received();
    }

    [Fact]
    public async Task ParameterlessReturnValueType()
    {
        // Arrange
        var callInvokerMock = Substitute.For<CallInvoker>();
        callInvokerMock.AsyncUnaryCall(Arg.Any<Method<Box<Nil>, Box<int>>>(), Arg.Any<string>(), Arg.Any<CallOptions>(), Arg.Is<Box<Nil>>(x => x.Value.Equals(Nil.Default)))
            .Returns(new AsyncUnaryCall<Box<int>>(Task.FromResult(Box.Create(123)), Task.FromResult(Metadata.Empty), () => Status.DefaultSuccess, () => Metadata.Empty, () => { }));

        // Act
        var client = MagicOnionClient.Create<IUnaryTestService>(callInvokerMock);
        var result = await client.ParameterlessReturnValueType();

        // Assert
        result.Should().Be(123);
        callInvokerMock.Received();
    }

    [Fact]
    public async Task ParameterlessReturnRefType()
    {
        // Arrange
        var callInvokerMock = Substitute.For<CallInvoker>();
        callInvokerMock.AsyncUnaryCall(Arg.Any<Method<Box<Nil>, string>>(), Arg.Any<string>(), Arg.Any<CallOptions>(), Arg.Is<Box<Nil>>(x => x.Value.Equals(Nil.Default)))
            .Returns(new AsyncUnaryCall<string>(Task.FromResult("FooBar"), Task.FromResult(Metadata.Empty), () => Status.DefaultSuccess, () => Metadata.Empty, () => { }));

        // Act
        var client = MagicOnionClient.Create<IUnaryTestService>(callInvokerMock);
        var result = await client.ParameterlessReturnRefType();

        // Assert
        result.Should().Be("FooBar");
        callInvokerMock.Received();
    }

    [Fact]
    public async Task OneRefTypeParameterReturnValueType()
    {
        // Arrange
        var request = "RequestValue";
        var response = 123;
        var callInvokerMock = Substitute.For<CallInvoker>();
        callInvokerMock.AsyncUnaryCall(Arg.Any<Method<string, Box<int>>>(), Arg.Any<string>(), Arg.Any<CallOptions>(), request)
            .Returns(new AsyncUnaryCall<Box<int>>(Task.FromResult(Box.Create(response)), Task.FromResult(Metadata.Empty), () => Status.DefaultSuccess, () => Metadata.Empty, () => { }));

        // Act
        var client = MagicOnionClient.Create<IUnaryTestService>(callInvokerMock);
        var result = await client.OneRefTypeParameterReturnValueType(request);

        // Assert
        result.Should().Be(123);
        callInvokerMock.Received();
    }

    [Fact]
    public async Task OneValueTypeParameterReturnValueType()
    {
        // Arrange
        var request = 123;
        var response = 456;
        var callInvokerMock = Substitute.For<CallInvoker>();
        callInvokerMock.AsyncUnaryCall(Arg.Any<Method<Box<int>, Box<int>>>(), Arg.Any<string>(), Arg.Any<CallOptions>(), Box.Create(request))
            .Returns(new AsyncUnaryCall<Box<int>>(Task.FromResult(Box.Create(response)), Task.FromResult(Metadata.Empty), () => Status.DefaultSuccess, () => Metadata.Empty, () => { }));

        // Act
        var client = MagicOnionClient.Create<IUnaryTestService>(callInvokerMock);
        var result = await client.OneValueTypeParameterReturnValueType(request);

        // Assert
        result.Should().Be(456);
        callInvokerMock.Received();
    }

    [Fact]
    public async Task OneRefTypeParameterReturnRefType()
    {
        // Arrange
        var request = "RequestValue";
        var response = "Ok";
        var sentRequest = default(string);
        var callInvokerMock = Substitute.For<CallInvoker>();
        callInvokerMock.AsyncUnaryCall(Arg.Any<Method<string, string>>(), Arg.Any<string>(), Arg.Any<CallOptions>(), request)
            .Returns(new AsyncUnaryCall<string>(Task.FromResult(response), Task.FromResult(Metadata.Empty), () => Status.DefaultSuccess, () => Metadata.Empty, () => { }))
            .AndDoes(x =>
            {
                // method, host, callOptions, request
                var request = x.ArgAt<string>(3);
                sentRequest = request;
            });

        // Act
        var client = MagicOnionClient.Create<IUnaryTestService>(callInvokerMock);
        var result = await client.OneRefTypeParameterReturnRefType(request);

        // Assert
        result.Should().Be("Ok");
        callInvokerMock.Received();
        sentRequest.Should().Be("RequestValue");
    }

    [Fact]
    public async Task OneValueTypeParameterReturnRefType()
    {
        // Arrange
        var request = 123;
        var response = "OK";
        var sentRequest = default(Box<int>);
        var callInvokerMock = Substitute.For<CallInvoker>();
        callInvokerMock.AsyncUnaryCall(Arg.Any<Method<Box<int>, string>>(), Arg.Any<string>(), Arg.Any<CallOptions>(), Box.Create(request))
            .Returns(new AsyncUnaryCall<string>(Task.FromResult(response), Task.FromResult(Metadata.Empty), () => Status.DefaultSuccess, () => Metadata.Empty, () => { }))
            .AndDoes(x =>
            {
                // method, host, callOptions, request
                var request = x.ArgAt<Box<int>>(3);
                sentRequest = request;
            });

        // Act
        var client = MagicOnionClient.Create<IUnaryTestService>(callInvokerMock);
        var result = await client.OneValueTypeParameterReturnRefType(request);

        // Assert
        result.Should().Be("OK");
        callInvokerMock.Received();
        (sentRequest?.Value).Should().Be(123);
    }

    [Fact]
    public async Task TwoParametersReturnRefType()
    {
        // Arrange
        var requestArg1 = 123;
        var requestArg2 = "Foo";
        var response = "OK";
        var sentRequest = default(Box<DynamicArgumentTuple<int, string>>);
        var callInvokerMock = Substitute.For<CallInvoker>();
        callInvokerMock.AsyncUnaryCall(Arg.Any<Method<Box<DynamicArgumentTuple<int, string>>, string>>(), Arg.Any<string>(), Arg.Any<CallOptions>(), Arg.Any<Box<DynamicArgumentTuple<int, string>>>())
            .Returns(new AsyncUnaryCall<string>(Task.FromResult(response), Task.FromResult(Metadata.Empty), () => Status.DefaultSuccess, () => Metadata.Empty, () => { }))
            .AndDoes(x =>
            {
                // method, host, callOptions, request
                var request = x.ArgAt<Box<DynamicArgumentTuple<int, string>>>(3);
                sentRequest = request;
            });

        // Act
        var client = MagicOnionClient.Create<IUnaryTestService>(callInvokerMock);
        var result = await client.TwoParametersReturnRefType(requestArg1, requestArg2);

        // Assert
        result.Should().Be("OK");
        callInvokerMock.Received();
        (sentRequest?.Value.Item1).Should().Be(123);
        (sentRequest?.Value.Item2).Should().Be("Foo");
    }

    [Fact]
    public async Task TwoParametersReturnValueType()
    {
        // Arrange
        var requestArg1 = 123;
        var requestArg2 = "Foo";
        var response = 987;
        var sentRequest = default(Box<DynamicArgumentTuple<int, string>>);
        var callInvokerMock = Substitute.For<CallInvoker>();
        callInvokerMock.AsyncUnaryCall(Arg.Any<Method<Box<DynamicArgumentTuple<int, string>>, Box<int>>>(), Arg.Any<string>(), Arg.Any<CallOptions>(), Arg.Any<Box<DynamicArgumentTuple<int, string>>>())
            .Returns(new AsyncUnaryCall<Box<int>>(Task.FromResult(Box.Create(response)), Task.FromResult(Metadata.Empty), () => Status.DefaultSuccess, () => Metadata.Empty, () => { }))
            .AndDoes(x =>
            {
                // method, host, callOptions, request
                var request = x.ArgAt<Box<DynamicArgumentTuple<int, string>>>(3);
                sentRequest = request;
            });

        // Act
        var client = MagicOnionClient.Create<IUnaryTestService>(callInvokerMock);
        var result = await client.TwoParametersReturnValueType(requestArg1, requestArg2);

        // Assert
        result.Should().Be(987);
        callInvokerMock.Received();
        (sentRequest?.Value.Item1).Should().Be(123);
        (sentRequest?.Value.Item2).Should().Be("Foo");
    }

    [Fact]
    public async Task ParameterlessNonGenericReturnType()
    {
        // Arrange
        var callInvokerMock = Substitute.For<CallInvoker>();
        callInvokerMock.AsyncUnaryCall(Arg.Any<Method<Box<Nil>, Box<Nil>>>(), Arg.Any<string>(), Arg.Any<CallOptions>(), Arg.Is<Box<Nil>>(x => x.Value.Equals(Nil.Default)))
            .Returns(new AsyncUnaryCall<Box<Nil>>(Task.FromResult(Box.Create(Nil.Default)), Task.FromResult(Metadata.Empty), () => Status.DefaultSuccess, () => Metadata.Empty, () => { }));

        // Act
        var client = MagicOnionClient.Create<IUnaryTestService>(callInvokerMock);
        await client.ParameterlessNonGenericReturnType();

        // Assert
        callInvokerMock.Received();
    }

    [Fact]
    public async Task OneRefTypeParameterNonGenericReturnType()
    {
        // Arrange
        var request = "RequestValue";
        var callInvokerMock = Substitute.For<CallInvoker>();
        callInvokerMock.AsyncUnaryCall(Arg.Any<Method<string, Box<Nil>>>(), Arg.Any<string>(), Arg.Any<CallOptions>(), request)
            .Returns(new AsyncUnaryCall<Box<Nil>>(Task.FromResult(Box.Create(Nil.Default)), Task.FromResult(Metadata.Empty), () => Status.DefaultSuccess, () => Metadata.Empty, () => { }));

        // Act
        var client = MagicOnionClient.Create<IUnaryTestService>(callInvokerMock);
        await client.OneRefTypeParameterNonGenericReturnType(request);

        // Assert
        callInvokerMock.Received();
    }

    [Fact]
    public async Task OneValueTypeParameterNonGenericReturnType()
    {
        // Arrange
        var request = 123;
        var callInvokerMock = Substitute.For<CallInvoker>();
        callInvokerMock.AsyncUnaryCall(Arg.Any<Method<Box<int>, Box<Nil>>>(), Arg.Any<string>(), Arg.Any<CallOptions>(), Box.Create(request))
            .Returns(new AsyncUnaryCall<Box<Nil>>(Task.FromResult(Box.Create(Nil.Default)), Task.FromResult(Metadata.Empty), () => Status.DefaultSuccess, () => Metadata.Empty, () => { }));

        // Act
        var client = MagicOnionClient.Create<IUnaryTestService>(callInvokerMock);
        await client.OneValueTypeParameterNonGenericReturnType(request);

        // Assert
        callInvokerMock.Received();
    }

    [Fact]
    public async Task TwoParametersNonGenericReturnType()
    {
        // Arrange
        var requestArg1 = 123;
        var requestArg2 = "Foo";
        var sentRequest = default(Box<DynamicArgumentTuple<int, string>>);
        var callInvokerMock = Substitute.For<CallInvoker>();
        callInvokerMock.AsyncUnaryCall(Arg.Any<Method<Box<DynamicArgumentTuple<int, string>>, Box<Nil>>>(), Arg.Any<string>(), Arg.Any<CallOptions>(), Arg.Any<Box<DynamicArgumentTuple<int, string>>>())
            .Returns(new AsyncUnaryCall<Box<Nil>>(Task.FromResult(Box.Create(Nil.Default)), Task.FromResult(Metadata.Empty), () => Status.DefaultSuccess, () => Metadata.Empty, () => { }))
            .AndDoes(x =>
            {
                // method, host, callOptions, request
                var request = x.ArgAt<Box<DynamicArgumentTuple<int, string>>>(3);
                sentRequest = request;
            });

        // Act
        var client = MagicOnionClient.Create<IUnaryTestService>(callInvokerMock);
        await client.TwoParametersNonGenericReturnType(requestArg1, requestArg2);

        // Assert
        callInvokerMock.Received();
        (sentRequest?.Value.Item1).Should().Be(123);
        (sentRequest?.Value.Item2).Should().Be("Foo");
    }

    [Fact]
    public async Task ThrowsResponseHeaders()
    {
        // Arrange
        var callInvokerMock = Substitute.For<CallInvoker>();
        callInvokerMock.AsyncUnaryCall(Arg.Any<Method<Box<Nil>, Box<int>>>(), Arg.Any<string>(), Arg.Any<CallOptions>(), Arg.Is<Box<Nil>>(x => x.Value.Equals(Nil.Default)))
            .Returns(new AsyncUnaryCall<Box<int>>(
                Task.FromException<Box<int>>(new RpcException(new Status(StatusCode.Unknown, "Faulted"), "Faulted")),
                Task.FromException<Metadata>(new RpcException(new Status(StatusCode.Unknown, "FaultedOnResponseHeaders"), "FaultedOnResponseHeaders")),
                () => Status.DefaultSuccess,
                () => Metadata.Empty,
                () => { }));

        // Act
        var client = MagicOnionClient.Create<IUnaryTestService>(callInvokerMock);
        var result = await Assert.ThrowsAsync<RpcException>(async () => await client.ParameterlessReturnValueType().ResponseHeadersAsync);

        // Assert
        result.StatusCode.Should().Be(StatusCode.Unknown);
        result.Message.Should().Be("FaultedOnResponseHeaders");
        callInvokerMock.Received();
    }

    [Fact]
    public async Task ThrowsResponse()
    {
        // Arrange
        var callInvokerMock = Substitute.For<CallInvoker>();
        callInvokerMock.AsyncUnaryCall(Arg.Any<Method<Box<Nil>, Box<int>>>(), Arg.Any<string>(), Arg.Any<CallOptions>(), Arg.Is<Box<Nil>>(x => x.Value.Equals(Nil.Default)))
            .Returns(new AsyncUnaryCall<Box<int>>(Task.FromException<Box<int>>(new RpcException(new Status(StatusCode.Unknown, "Faulted"), "Faulted")), Task.FromResult(Metadata.Empty), () => Status.DefaultSuccess, () => Metadata.Empty, () => { }));

        // Act
        var client = MagicOnionClient.Create<IUnaryTestService>(callInvokerMock);
        var result = await Assert.ThrowsAsync<RpcException>(async () => await client.ParameterlessReturnValueType());

        // Assert
        result.StatusCode.Should().Be(StatusCode.Unknown);
        result.Message.Should().Be("Faulted");
        callInvokerMock.Received();
    }

    public interface IUnaryTestService : IService<IUnaryTestService>
    {
        UnaryResult<Nil> ParameterlessReturnNil();
        UnaryResult<int> ParameterlessReturnValueType();
        UnaryResult<string> ParameterlessReturnRefType();
        UnaryResult<int> OneRefTypeParameterReturnValueType(string arg1);
        UnaryResult<int> OneValueTypeParameterReturnValueType(int arg1);
        UnaryResult<string> OneRefTypeParameterReturnRefType(string arg1);
        UnaryResult<string> OneValueTypeParameterReturnRefType(int arg1);
        UnaryResult<int> TwoParametersReturnValueType(int arg1, string arg2);
        UnaryResult<string> TwoParametersReturnRefType(int arg1, string arg2);

        UnaryResult ParameterlessNonGenericReturnType();
        UnaryResult OneRefTypeParameterNonGenericReturnType(string arg1);
        UnaryResult OneValueTypeParameterNonGenericReturnType(int arg1);
        UnaryResult TwoParametersNonGenericReturnType(int arg1, string arg2);
    }

    [Fact]
    public void MaxParameters()
    {
        // Arrange
        var callInvokerMock = Substitute.For<CallInvoker>();

        // Act
        var client = MagicOnionClient.Create<IMaxParametersService>(callInvokerMock);

        // Assert
        client.Should().NotBeNull();
    }

    public interface IMaxParametersService : IService<IMaxParametersService>
    {
        UnaryResult<int> ManyParameterReturnValueType(string arg1, bool arg2, long arg3, uint arg4, char arg5, byte arg6, int arg7, int arg8, int arg9, int arg10, int arg11, int arg12, int arg13, int arg14, int arg15);
    }

    [Fact]
    public void TooManyParameters()
    {
        // Arrange
        var callInvokerMock = Substitute.For<CallInvoker>();

        // Act / Assert
        Assert.Throws<TypeInitializationException>(() => MagicOnionClient.Create<ITooManyParametersService>(callInvokerMock));
    }

    public interface ITooManyParametersService : IService<ITooManyParametersService>
    {
        UnaryResult<int> AMethod(string arg1, bool arg2, long arg3, uint arg4, char arg5, byte arg6, int arg7, int arg8, int arg9, int arg10, int arg11, int arg12, int arg13, int arg14, int arg15, int arg16 /* T16 */);
    }

    [Fact]
    public void ReturnTaskOfUnaryResult()
    {
        // Arrange
        var callInvokerMock = Substitute.For<CallInvoker>();

        // Act / Assert
        Assert.Throws<TypeInitializationException>(() => MagicOnionClient.Create<IReturnTaskOfUnaryResultService>(callInvokerMock));
    }

    public interface IReturnTaskOfUnaryResultService : IService<IReturnTaskOfUnaryResultService>
    {
        Task<UnaryResult<int>> AMethod();
    }
}
