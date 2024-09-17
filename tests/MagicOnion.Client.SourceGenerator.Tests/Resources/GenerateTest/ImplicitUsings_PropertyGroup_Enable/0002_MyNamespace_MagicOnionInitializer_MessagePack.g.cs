﻿// <auto-generated />
#pragma warning disable CS0618 // 'member' is obsolete: 'text'
#pragma warning disable CS0612 // 'member' is obsolete
#pragma warning disable CS8019 // Unnecessary using directive.
#pragma warning disable CS1522 // Empty switch block

namespace MyNamespace
{
    using global::System;
    using global::MessagePack;

    partial class MagicOnionInitializer
    {
        /// <summary>
        /// Gets the generated MessagePack formatter resolver.
        /// </summary>
        public static global::MessagePack.IFormatterResolver Resolver => MessagePackGeneratedResolver.Instance;
        class MessagePackGeneratedResolver : global::MessagePack.IFormatterResolver
        {
            public static readonly global::MessagePack.IFormatterResolver Instance = new MessagePackGeneratedResolver();

            MessagePackGeneratedResolver() {}

            public global::MessagePack.Formatters.IMessagePackFormatter<T> GetFormatter<T>()
                => FormatterCache<T>.formatter;

            static class FormatterCache<T>
            {
                public static readonly global::MessagePack.Formatters.IMessagePackFormatter<T> formatter;

                static FormatterCache()
                {
                    var f = MessagePackGeneratedGetFormatterHelper.GetFormatter(typeof(T));
                    if (f != null)
                    {
                        formatter = (global::MessagePack.Formatters.IMessagePackFormatter<T>)f;
                    }
                }
            }
        }
        static class MessagePackGeneratedGetFormatterHelper
        {
            static readonly global::System.Collections.Generic.Dictionary<global::System.Type, int> lookup;

            static MessagePackGeneratedGetFormatterHelper()
            {
                lookup = new global::System.Collections.Generic.Dictionary<global::System.Type, int>(7)
                {
                    {typeof(global::MagicOnion.DynamicArgumentTuple<global::System.Int32, global::System.Collections.Generic.IReadOnlyList<global::System.Int32>, global::System.IO.FileMode, global::System.Linq.ILookup<global::System.String, global::System.String>, global::System.Net.Http.ClientCertificateOption, global::System.Threading.ApartmentState, global::System.Threading.Tasks.TaskCreationOptions>), 0},
                    {typeof(global::System.Collections.Generic.IReadOnlyList<global::System.Int32>), 1},
                    {typeof(global::System.Linq.ILookup<global::System.String, global::System.String>), 2},
                    {typeof(global::System.IO.FileMode), 3},
                    {typeof(global::System.Net.Http.ClientCertificateOption), 4},
                    {typeof(global::System.Threading.ApartmentState), 5},
                    {typeof(global::System.Threading.Tasks.TaskCreationOptions), 6},
                };
            }
            internal static object GetFormatter(global::System.Type t)
            {
                int key;
                if (!lookup.TryGetValue(t, out key))
                {
                    return null;
                }
            
                switch (key)
                {
                    case 0: return new global::MagicOnion.Serialization.MessagePack.DynamicArgumentTupleFormatter<global::System.Int32, global::System.Collections.Generic.IReadOnlyList<global::System.Int32>, global::System.IO.FileMode, global::System.Linq.ILookup<global::System.String, global::System.String>, global::System.Net.Http.ClientCertificateOption, global::System.Threading.ApartmentState, global::System.Threading.Tasks.TaskCreationOptions>(default(global::System.Int32), default(global::System.Collections.Generic.IReadOnlyList<global::System.Int32>), default(global::System.IO.FileMode), default(global::System.Linq.ILookup<global::System.String, global::System.String>), default(global::System.Net.Http.ClientCertificateOption), default(global::System.Threading.ApartmentState), default(global::System.Threading.Tasks.TaskCreationOptions));
                    case 1: return new global::MessagePack.Formatters.InterfaceReadOnlyListFormatter<global::System.Int32>();
                    case 2: return new global::MessagePack.Formatters.InterfaceLookupFormatter<global::System.String, global::System.String>();
                    case 3: return new MessagePackEnumFormatters.FileModeFormatter();
                    case 4: return new MessagePackEnumFormatters.ClientCertificateOptionFormatter();
                    case 5: return new MessagePackEnumFormatters.ApartmentStateFormatter();
                    case 6: return new MessagePackEnumFormatters.TaskCreationOptionsFormatter();
                    default: return null;
                }
            }
        }
        /// <summary>Type hints for Ahead-of-Time compilation.</summary>
        [Preserve]
        static class TypeHints
        {
            [Preserve]
            internal static void Register()
            {
                _ = MessagePackGeneratedResolver.Instance.GetFormatter<global::MagicOnion.DynamicArgumentTuple<global::System.Int32, global::System.Collections.Generic.IReadOnlyList<global::System.Int32>, global::System.IO.FileMode, global::System.Linq.ILookup<global::System.String, global::System.String>, global::System.Net.Http.ClientCertificateOption, global::System.Threading.ApartmentState, global::System.Threading.Tasks.TaskCreationOptions>>();
                _ = MessagePackGeneratedResolver.Instance.GetFormatter<global::MessagePack.Nil>();
                _ = MessagePackGeneratedResolver.Instance.GetFormatter<global::System.Collections.Generic.IReadOnlyList<global::System.Int32>>();
                _ = MessagePackGeneratedResolver.Instance.GetFormatter<global::System.Int32>();
                _ = MessagePackGeneratedResolver.Instance.GetFormatter<global::System.IO.FileMode>();
                _ = MessagePackGeneratedResolver.Instance.GetFormatter<global::System.Linq.ILookup<global::System.String, global::System.String>>();
                _ = MessagePackGeneratedResolver.Instance.GetFormatter<global::System.Net.Http.ClientCertificateOption>();
                _ = MessagePackGeneratedResolver.Instance.GetFormatter<global::System.String>();
                _ = MessagePackGeneratedResolver.Instance.GetFormatter<global::System.Threading.ApartmentState>();
                _ = MessagePackGeneratedResolver.Instance.GetFormatter<global::System.Threading.Tasks.TaskCreationOptions>();
            }
        }
    }
}