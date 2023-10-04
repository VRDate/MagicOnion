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
            public global::MagicOnion.Client.Internal.RawMethodInvoker<global::MessagePack.Nil, global::TempProject.MyGenericObject<global::TempProject.MyGenericObject<global::TempProject.MyObject>>> A;
            public global::MagicOnion.Client.Internal.RawMethodInvoker<global::MessagePack.Nil, global::TempProject.MyGenericObject<global::TempProject.MyGenericObject<global::TempProject.MyGenericObject<global::TempProject.MyObject>>>> B;
            public global::MagicOnion.Client.Internal.RawMethodInvoker<global::MessagePack.Nil, global::TempProject.MyGenericObject<global::TempProject.MyGenericObject<global::TempProject.MyGenericObject<global::System.Int32>>>> C;
            public ClientCore(global::MagicOnion.Serialization.IMagicOnionSerializerProvider serializerProvider)
            {
                this.A = global::MagicOnion.Client.Internal.RawMethodInvoker.Create_ValueType_RefType<global::MessagePack.Nil, global::TempProject.MyGenericObject<global::TempProject.MyGenericObject<global::TempProject.MyObject>>>(global::Grpc.Core.MethodType.Unary, "IMyService", "A", serializerProvider);
                this.B = global::MagicOnion.Client.Internal.RawMethodInvoker.Create_ValueType_RefType<global::MessagePack.Nil, global::TempProject.MyGenericObject<global::TempProject.MyGenericObject<global::TempProject.MyGenericObject<global::TempProject.MyObject>>>>(global::Grpc.Core.MethodType.Unary, "IMyService", "B", serializerProvider);
                this.C = global::MagicOnion.Client.Internal.RawMethodInvoker.Create_ValueType_RefType<global::MessagePack.Nil, global::TempProject.MyGenericObject<global::TempProject.MyGenericObject<global::TempProject.MyGenericObject<global::System.Int32>>>>(global::Grpc.Core.MethodType.Unary, "IMyService", "C", serializerProvider);
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
        
        public global::MagicOnion.UnaryResult<global::TempProject.MyGenericObject<global::TempProject.MyGenericObject<global::TempProject.MyObject>>> A()
            => this.core.A.InvokeUnary(this, "IMyService/A", global::MessagePack.Nil.Default);
        public global::MagicOnion.UnaryResult<global::TempProject.MyGenericObject<global::TempProject.MyGenericObject<global::TempProject.MyGenericObject<global::TempProject.MyObject>>>> B()
            => this.core.B.InvokeUnary(this, "IMyService/B", global::MessagePack.Nil.Default);
        public global::MagicOnion.UnaryResult<global::TempProject.MyGenericObject<global::TempProject.MyGenericObject<global::TempProject.MyGenericObject<global::System.Int32>>>> C()
            => this.core.C.InvokeUnary(this, "IMyService/C", global::MessagePack.Nil.Default);
    }
}


