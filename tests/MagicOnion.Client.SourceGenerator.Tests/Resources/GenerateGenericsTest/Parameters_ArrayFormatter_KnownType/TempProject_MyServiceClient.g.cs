﻿// <auto-generated />
#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 219
#pragma warning disable 168

// NOTE: Disable warnings for nullable reference types.
// `#nullable disable` causes compile error on old C# compilers (-7.3)
#pragma warning disable 8603 // Possible null reference return.
#pragma warning disable 8618 // Non-nullable variable must contain a non-null value when exiting constructor. Consider declaring it as nullable.
#pragma warning disable 8625 // Cannot convert null literal to non-nullable reference type.

namespace TempProject
{
    using global::System;
    using global::Grpc.Core;
    using global::MagicOnion;
    using global::MagicOnion.Client;
    using global::MessagePack;
    
    [global::MagicOnion.Ignore]
    public class MyServiceClient : global::MagicOnion.Client.MagicOnionClientBase<global::TempProject.IMyService>, global::TempProject.IMyService
    {
        class ClientCore
        {
            public global::MagicOnion.Client.Internal.RawMethodInvoker<global::System.String[], global::MessagePack.Nil> GetStringValuesAsync;
            public global::MagicOnion.Client.Internal.RawMethodInvoker<global::System.Int32[], global::MessagePack.Nil> GetIntValuesAsync;
            public global::MagicOnion.Client.Internal.RawMethodInvoker<global::System.Int32[], global::MessagePack.Nil> GetInt32ValuesAsync;
            public global::MagicOnion.Client.Internal.RawMethodInvoker<global::System.Single[], global::MessagePack.Nil> GetSingleValuesAsync;
            public global::MagicOnion.Client.Internal.RawMethodInvoker<global::System.Boolean[], global::MessagePack.Nil> GetBooleanValuesAsync;
            public ClientCore(global::MagicOnion.Serialization.IMagicOnionSerializerProvider serializerProvider)
            {
                this.GetStringValuesAsync = global::MagicOnion.Client.Internal.RawMethodInvoker.Create_RefType_ValueType<global::System.String[], global::MessagePack.Nil>(global::Grpc.Core.MethodType.Unary, "IMyService", "GetStringValuesAsync", serializerProvider);
                this.GetIntValuesAsync = global::MagicOnion.Client.Internal.RawMethodInvoker.Create_RefType_ValueType<global::System.Int32[], global::MessagePack.Nil>(global::Grpc.Core.MethodType.Unary, "IMyService", "GetIntValuesAsync", serializerProvider);
                this.GetInt32ValuesAsync = global::MagicOnion.Client.Internal.RawMethodInvoker.Create_RefType_ValueType<global::System.Int32[], global::MessagePack.Nil>(global::Grpc.Core.MethodType.Unary, "IMyService", "GetInt32ValuesAsync", serializerProvider);
                this.GetSingleValuesAsync = global::MagicOnion.Client.Internal.RawMethodInvoker.Create_RefType_ValueType<global::System.Single[], global::MessagePack.Nil>(global::Grpc.Core.MethodType.Unary, "IMyService", "GetSingleValuesAsync", serializerProvider);
                this.GetBooleanValuesAsync = global::MagicOnion.Client.Internal.RawMethodInvoker.Create_RefType_ValueType<global::System.Boolean[], global::MessagePack.Nil>(global::Grpc.Core.MethodType.Unary, "IMyService", "GetBooleanValuesAsync", serializerProvider);
            }
        }
        
        readonly ClientCore core;
        
        public MyServiceClient(global::MagicOnion.Client.MagicOnionClientOptions options, global::MagicOnion.Serialization.IMagicOnionSerializerProvider serializerProvider) : base(options)
        {
            this.core = new ClientCore(serializerProvider);
        }
        
        private MyServiceClient(MagicOnionClientOptions options, ClientCore core) : base(options)
        {
            this.core = core;
        }
        
        protected override global::MagicOnion.Client.MagicOnionClientBase<IMyService> Clone(global::MagicOnion.Client.MagicOnionClientOptions options)
            => new MyServiceClient(options, core);
        
        public global::MagicOnion.UnaryResult<global::MessagePack.Nil> GetStringValuesAsync(global::System.String[] arg0)
            => this.core.GetStringValuesAsync.InvokeUnary(this, "IMyService/GetStringValuesAsync", arg0);
        public global::MagicOnion.UnaryResult<global::MessagePack.Nil> GetIntValuesAsync(global::System.Int32[] arg0)
            => this.core.GetIntValuesAsync.InvokeUnary(this, "IMyService/GetIntValuesAsync", arg0);
        public global::MagicOnion.UnaryResult<global::MessagePack.Nil> GetInt32ValuesAsync(global::System.Int32[] arg0)
            => this.core.GetInt32ValuesAsync.InvokeUnary(this, "IMyService/GetInt32ValuesAsync", arg0);
        public global::MagicOnion.UnaryResult<global::MessagePack.Nil> GetSingleValuesAsync(global::System.Single[] arg0)
            => this.core.GetSingleValuesAsync.InvokeUnary(this, "IMyService/GetSingleValuesAsync", arg0);
        public global::MagicOnion.UnaryResult<global::MessagePack.Nil> GetBooleanValuesAsync(global::System.Boolean[] arg0)
            => this.core.GetBooleanValuesAsync.InvokeUnary(this, "IMyService/GetBooleanValuesAsync", arg0);
    }
}


