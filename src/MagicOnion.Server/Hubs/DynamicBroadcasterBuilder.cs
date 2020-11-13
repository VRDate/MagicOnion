using MagicOnion.Utils;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MagicOnion.Server.Hubs
{
#if ENABLE_SAVE_ASSEMBLY
    public
#else
    internal
#endif
        static class AssemblyHolder
    {
        public const string ModuleName = "MagicOnion.Server.Hubs.DynamicBroadcaster";

        readonly static DynamicAssembly assembly;
        internal static DynamicAssembly Assembly { get { return assembly; } }

        static AssemblyHolder()
        {
            assembly = new DynamicAssembly(ModuleName);
        }

#if ENABLE_SAVE_ASSEMBLY

        public static AssemblyBuilder Save()
        {
            return assembly.Save();
        }

#endif
    }

#if ENABLE_SAVE_ASSEMBLY
    public
#else
    internal
#endif
        static class DynamicBroadcasterBuilder<T>
    {
        public static readonly Type BroadcasterType;
        public static readonly Type BroadcasterType_ExceptOne;
        public static readonly Type BroadcasterType_ExceptMany;
        // TODO:impl Type
        public static readonly Type BroadcasterType_ToOne;
        public static readonly Type BroadcasterType_ToMany;

        static readonly MethodInfo groupWriteAllMethodInfo = typeof(IGroup).GetMethod(nameof(IGroup.WriteAllAsync))!;
        static readonly MethodInfo groupWriteExceptOneMethodInfo = typeof(IGroup).GetMethods().First(x => x.Name == nameof(IGroup.WriteExceptAsync) && !x.GetParameters()[2].ParameterType.IsArray);
        static readonly MethodInfo groupWriteExceptManyMethodInfo = typeof(IGroup).GetMethods().First(x => x.Name == nameof(IGroup.WriteExceptAsync) && x.GetParameters()[2].ParameterType.IsArray);
        static readonly MethodInfo groupWriteToOneMethodInfo = typeof(IGroup).GetMethods().First(x => x.Name == nameof(IGroup.WriteToAsync) && !x.GetParameters()[2].ParameterType.IsArray);
        static readonly MethodInfo groupWriteToManyMethodInfo = typeof(IGroup).GetMethods().First(x => x.Name == nameof(IGroup.WriteToAsync) && x.GetParameters()[2].ParameterType.IsArray);

        static readonly MethodInfo fireAndForget = typeof(DynamicBroadcasterBuilder<T>).GetMethod(nameof(FireAndForget), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)!;

        static DynamicBroadcasterBuilder()
        {
            var t = typeof(T);
            var ti = t.GetTypeInfo();
            if (!ti.IsInterface) throw new Exception("Broadcaster Proxy only allows interface. Type:" + ti.Name);

            var asm = AssemblyHolder.Assembly;
            var methodDefinitions = BroadcasterHelper.SearchDefinitions(t);
            BroadcasterHelper.VerifyMethodDefinitions(methodDefinitions);

            {
                var typeBuilder = asm.DefineType($"{AssemblyHolder.ModuleName}.{ti.FullName}Broadcaster_{Guid.NewGuid().ToString()}", TypeAttributes.Public, typeof(object), new Type[] { t })!;
                var (group, ctor) = DefineConstructor(typeBuilder);
                DefineMethods(typeBuilder, t, group, methodDefinitions, groupWriteAllMethodInfo, null);
                BroadcasterType = typeBuilder.CreateTypeInfo()!.AsType();
            }
            {
                var typeBuilder = asm.DefineType($"{AssemblyHolder.ModuleName}.{ti.FullName}BroadcasterExceptOne_{Guid.NewGuid().ToString()}", TypeAttributes.Public, typeof(object), new Type[] { t });
                var (group, except, ctor) = DefineConstructor2(typeBuilder);
                DefineMethods(typeBuilder, t, group, methodDefinitions, groupWriteExceptOneMethodInfo, except);
                BroadcasterType_ExceptOne = typeBuilder.CreateTypeInfo()!.AsType();
            }
            {
                var typeBuilder = asm.DefineType($"{AssemblyHolder.ModuleName}.{ti.FullName}BroadcasterExceptMany_{Guid.NewGuid().ToString()}", TypeAttributes.Public, typeof(object), new Type[] { t });
                var (group, except, ctor) = DefineConstructor3(typeBuilder);
                DefineMethods(typeBuilder, t, group, methodDefinitions, groupWriteExceptManyMethodInfo, except);
                BroadcasterType_ExceptMany = typeBuilder.CreateTypeInfo()!.AsType();
            }
            {
                var typeBuilder = asm.DefineType($"{AssemblyHolder.ModuleName}.{ti.FullName}BroadcasterToOne_{Guid.NewGuid().ToString()}", TypeAttributes.Public, typeof(object), new Type[] { t });
                var (group, to, ctor) = DefineConstructor2(typeBuilder);
                DefineMethods(typeBuilder, t, group, methodDefinitions, groupWriteToOneMethodInfo, to);
                BroadcasterType_ToOne = typeBuilder.CreateTypeInfo()!.AsType();
            }
            {
                var typeBuilder = asm.DefineType($"{AssemblyHolder.ModuleName}.{ti.FullName}BroadcasterToMany_{Guid.NewGuid().ToString()}", TypeAttributes.Public, typeof(object), new Type[] { t });
                var (group, to, ctor) = DefineConstructor3(typeBuilder);
                DefineMethods(typeBuilder, t, group, methodDefinitions, groupWriteToManyMethodInfo, to);
                BroadcasterType_ToMany = typeBuilder.CreateTypeInfo()!.AsType();
            }
        }

        static (FieldInfo, ConstructorInfo) DefineConstructor(TypeBuilder typeBuilder)
        {
            // .ctor(IGroup group)
            var groupField = typeBuilder.DefineField("group", typeof(IGroup), FieldAttributes.Private | FieldAttributes.InitOnly);

            var ctor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new[] { typeof(IGroup) });
            var il = ctor.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, typeof(object).GetConstructors().First());
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, groupField);
            il.Emit(OpCodes.Ret);

            return (groupField, ctor);
        }

        static (FieldInfo groupField, FieldInfo exceptField, ConstructorInfo) DefineConstructor2(TypeBuilder typeBuilder)
        {
            // .ctor(IGroup group, Guid except)
            var groupField = typeBuilder.DefineField("group", typeof(IGroup), FieldAttributes.Private | FieldAttributes.InitOnly);
            var connectionIdField = typeBuilder.DefineField("connectionId", typeof(Guid), FieldAttributes.Private | FieldAttributes.InitOnly);

            var ctor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new[] { typeof(IGroup), typeof(Guid) });
            var il = ctor.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, typeof(object).GetConstructors().First());
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, groupField);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_2);
            il.Emit(OpCodes.Stfld, connectionIdField);
            il.Emit(OpCodes.Ret);

            return (groupField, connectionIdField, ctor);
        }

        static (FieldInfo groupField, FieldInfo exceptField, ConstructorInfo) DefineConstructor3(TypeBuilder typeBuilder)
        {
            // .ctor(IGroup group, Guid[] except)
            var groupField = typeBuilder.DefineField("group", typeof(IGroup), FieldAttributes.Private | FieldAttributes.InitOnly);
            var connectionIdsField = typeBuilder.DefineField("connectionIds", typeof(Guid[]), FieldAttributes.Private | FieldAttributes.InitOnly);

            var ctor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new[] { typeof(IGroup), typeof(Guid[]) });
            var il = ctor.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, typeof(object).GetConstructors().First());
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, groupField);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_2);
            il.Emit(OpCodes.Stfld, connectionIdsField);
            il.Emit(OpCodes.Ret);

            return (groupField, connectionIdsField, ctor);
        }

        static void DefineMethods(TypeBuilder typeBuilder, Type interfaceType, FieldInfo groupField, BroadcasterHelper.MethodDefinition[] definitions, MethodInfo writeMethod, FieldInfo? exceptField)
        {
            // Proxy Methods
            for (int i = 0; i < definitions.Length; i++)
            {
                var def = definitions[i];
                var parameters = def.MethodInfo.GetParameters().Select(x => x.ParameterType).ToArray();

                var method = typeBuilder.DefineMethod(def.MethodInfo.Name, MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.Virtual,
                    def.MethodInfo.ReturnType,
                    parameters);
                var il = method.GetILGenerator();

                // like this.
                // return group.WriteAllAsync(9013131, new DynamicArgumentTuple<int, string>(senderId, message));
                // BroadcasterHelper.FireAndForget(...)

                // load group field
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, groupField);

                // arg1
                il.EmitLdc_I4(def.MethodId);

                // create request for arg2
                for (int j = 0; j < parameters.Length; j++)
                {
                    il.Emit(OpCodes.Ldarg, j + 1);
                }

                Type? callType = null;
                if (parameters.Length == 0)
                {
                    // use Nil.
                    callType = typeof(Nil);
                    il.Emit(OpCodes.Ldsfld, typeof(Nil).GetField("Default")!);
                }
                else if (parameters.Length == 1)
                {
                    // already loaded parameter.
                    callType = parameters[0];
                }
                else
                {
                    // call new DynamicArgumentTuple<T>
                    callType = BroadcasterHelper.dynamicArgumentTupleTypes[parameters.Length - 2].MakeGenericType(parameters);
                    il.Emit(OpCodes.Newobj, callType.GetConstructors().First());
                }

                if (writeMethod != groupWriteAllMethodInfo)
                {
                    if (exceptField == null) throw new InvalidOperationException("Non group-wide write method requires except field.");
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldfld, exceptField);
                }

                if (def.MethodInfo.ReturnType == typeof(void))
                {
                    il.Emit(OpCodes.Ldc_I4_1); // fireAndForget = true
                }
                else
                {
                    il.Emit(OpCodes.Ldc_I4_0); // fireAndForget = false
                }

                il.Emit(OpCodes.Callvirt, writeMethod.MakeGenericMethod(callType));

                if (def.MethodInfo.ReturnType == typeof(void))
                {
                    il.Emit(OpCodes.Call, fireAndForget);
                }

                il.Emit(OpCodes.Ret);
            }
        }

        static async void FireAndForget(Task task)
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                MagicOnionServerInternalLogger.Current.LogError(ex, "exception occured in client broadcast.");
            }
        }
    }
}