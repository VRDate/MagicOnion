﻿// <auto-generated />
#pragma warning disable CS0618 // 'member' is obsolete: 'text'
#pragma warning disable CS0612 // 'member' is obsolete
#pragma warning disable CS0414 // The private field 'field' is assigned but its value is never used
#pragma warning disable CS8019 // Unnecessary using directive.
#pragma warning disable CS1522 // Empty switch block
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously.

namespace TempProject
{
    partial class MagicOnionInitializer
    {
        static partial class MagicOnionGeneratedClient
        {
            [global::MagicOnion.Ignore]
            public class TempProject_MyHubClient : global::MagicOnion.Client.StreamingHubClientBase<global::TempProject.IMyHub, global::TempProject.IMyHubReceiver>, global::TempProject.IMyHub
            {
                public TempProject_MyHubClient(global::TempProject.IMyHubReceiver receiver, global::Grpc.Core.CallInvoker callInvoker, global::MagicOnion.Client.StreamingHubClientOptions options)
                    : base("IMyHub", receiver, callInvoker, options)
                {
                }

                public global::System.Threading.Tasks.Task<global::System.String[]> GetStringValuesAsync()
                    => this.WriteMessageWithResponseTaskAsync<global::MessagePack.Nil, global::System.String[]>(1774317884, global::MessagePack.Nil.Default);
                public global::System.Threading.Tasks.Task<global::System.Int32[]> GetIntValuesAsync()
                    => this.WriteMessageWithResponseTaskAsync<global::MessagePack.Nil, global::System.Int32[]>(-400881550, global::MessagePack.Nil.Default);
                public global::System.Threading.Tasks.Task<global::System.Int32[]> GetInt32ValuesAsync()
                    => this.WriteMessageWithResponseTaskAsync<global::MessagePack.Nil, global::System.Int32[]>(309063297, global::MessagePack.Nil.Default);
                public global::System.Threading.Tasks.Task<global::System.Single[]> GetSingleValuesAsync()
                    => this.WriteMessageWithResponseTaskAsync<global::MessagePack.Nil, global::System.Single[]>(702446639, global::MessagePack.Nil.Default);
                public global::System.Threading.Tasks.Task<global::System.Boolean[]> GetBooleanValuesAsync()
                    => this.WriteMessageWithResponseTaskAsync<global::MessagePack.Nil, global::System.Boolean[]>(2082077357, global::MessagePack.Nil.Default);

                public global::TempProject.IMyHub FireAndForget()
                    => new FireAndForgetClient(this);

                [global::MagicOnion.Ignore]
                class FireAndForgetClient : global::TempProject.IMyHub
                {
                    readonly TempProject_MyHubClient parent;

                    public FireAndForgetClient(TempProject_MyHubClient parent)
                        => this.parent = parent;

                    public global::TempProject.IMyHub FireAndForget() => this;
                    public global::System.Threading.Tasks.Task DisposeAsync() => throw new global::System.NotSupportedException();
                    public global::System.Threading.Tasks.Task WaitForDisconnect() => throw new global::System.NotSupportedException();

                    public global::System.Threading.Tasks.Task<global::System.String[]> GetStringValuesAsync()
                        => parent.WriteMessageFireAndForgetTaskAsync<global::MessagePack.Nil, global::System.String[]>(1774317884, global::MessagePack.Nil.Default);
                    public global::System.Threading.Tasks.Task<global::System.Int32[]> GetIntValuesAsync()
                        => parent.WriteMessageFireAndForgetTaskAsync<global::MessagePack.Nil, global::System.Int32[]>(-400881550, global::MessagePack.Nil.Default);
                    public global::System.Threading.Tasks.Task<global::System.Int32[]> GetInt32ValuesAsync()
                        => parent.WriteMessageFireAndForgetTaskAsync<global::MessagePack.Nil, global::System.Int32[]>(309063297, global::MessagePack.Nil.Default);
                    public global::System.Threading.Tasks.Task<global::System.Single[]> GetSingleValuesAsync()
                        => parent.WriteMessageFireAndForgetTaskAsync<global::MessagePack.Nil, global::System.Single[]>(702446639, global::MessagePack.Nil.Default);
                    public global::System.Threading.Tasks.Task<global::System.Boolean[]> GetBooleanValuesAsync()
                        => parent.WriteMessageFireAndForgetTaskAsync<global::MessagePack.Nil, global::System.Boolean[]>(2082077357, global::MessagePack.Nil.Default);

                }

                protected override void OnBroadcastEvent(global::System.Int32 methodId, global::System.ReadOnlyMemory<global::System.Byte> data)
                {
                    switch (methodId)
                    {
                    }
                }

                protected override void OnResponseEvent(global::System.Int32 methodId, global::System.Object taskSource, global::System.ReadOnlyMemory<global::System.Byte> data)
                {
                    switch (methodId)
                    {
                        case 1774317884: // Task<String[]> GetStringValuesAsync()
                            base.SetResultForResponse<global::System.String[]>(taskSource, data);
                            break;
                        case -400881550: // Task<Int32[]> GetIntValuesAsync()
                            base.SetResultForResponse<global::System.Int32[]>(taskSource, data);
                            break;
                        case 309063297: // Task<Int32[]> GetInt32ValuesAsync()
                            base.SetResultForResponse<global::System.Int32[]>(taskSource, data);
                            break;
                        case 702446639: // Task<Single[]> GetSingleValuesAsync()
                            base.SetResultForResponse<global::System.Single[]>(taskSource, data);
                            break;
                        case 2082077357: // Task<Boolean[]> GetBooleanValuesAsync()
                            base.SetResultForResponse<global::System.Boolean[]>(taskSource, data);
                            break;
                    }
                }

                protected override void OnClientResultEvent(global::System.Int32 methodId, global::System.Guid messageId, global::System.ReadOnlyMemory<global::System.Byte> data)
                {
                }
            }
        }
    }
}