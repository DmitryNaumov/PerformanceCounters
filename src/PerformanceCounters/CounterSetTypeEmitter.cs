using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace NeedfulThings.PerformanceCounters
{
    internal static class CounterSetTypeEmitter
    {
        private static readonly AssemblyBuilder AssemblyBuilder;
        private static readonly ModuleBuilder ModuleBuilder;
        private static int Counter;

        static CounterSetTypeEmitter()
        {
            var assemblyName = new AssemblyName("PerformanceCounter.ProxyAssembly");
            var access = AssemblyBuilderAccess.Run;

            AssemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, access);
            ModuleBuilder = AssemblyBuilder.DefineDynamicModule("PerformanceCounter.ProxyAssembly.dll");
        }

        public static Type GeneratePerformanceCounterSetImplementation(Type counterSetInterface)
        {
            var typeBuilder = ModuleBuilder.DefineType(
                counterSetInterface.Name + "Implementation",
                TypeAttributes.Class,
                typeof(object),
                new[] {counterSetInterface});

            var properties = counterSetInterface.GetProperties();

            var parameterTypes = new List<Type>();
            parameterTypes.Add(typeof(string));
            parameterTypes.Add(typeof(IReadOnlyCollection<IReadOnlyPerformanceCounter>));
            parameterTypes.AddRange(properties.Select(property => property.PropertyType));

            var constructorBuilder = typeBuilder.DefineConstructor(
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                parameterTypes.ToArray());

            var ctorIl = constructorBuilder.GetILGenerator();

            ctorIl.Emit(OpCodes.Ldarg_0); // this
            ctorIl.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));

            DefineProperty(1, "CategoryName", typeof(string), typeBuilder, ctorIl);
            DefineProperty(2, "Counters", typeof(IReadOnlyCollection<IReadOnlyPerformanceCounter>), typeBuilder, ctorIl);

            for (int i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                DefineProperty((byte) (i+3), property.Name, property.PropertyType, typeBuilder, ctorIl);
            }

            ctorIl.Emit(OpCodes.Ret);

            return typeBuilder.CreateType();
        }

        private static void DefineProperty(byte index, string name, Type type, TypeBuilder typeBuilder, ILGenerator ctorIl)
        {
            var backingField = typeBuilder.DefineField("_" + name, type, FieldAttributes.Private);

            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldarg_S, index);
            ctorIl.Emit(OpCodes.Stfld, backingField);

            var propertyBuilder = typeBuilder.DefineProperty(name, PropertyAttributes.HasDefault, type, Type.EmptyTypes);

            var propertyGetterBuilder = typeBuilder.DefineMethod(
                "get_" + name,
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig | MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.NewSlot,
                type,
                Type.EmptyTypes);
            propertyGetterBuilder.SetImplementationFlags(MethodImplAttributes.Managed);

            var methodIl = propertyGetterBuilder.GetILGenerator();
            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldfld, backingField);
            methodIl.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(propertyGetterBuilder);
        }
    }
}