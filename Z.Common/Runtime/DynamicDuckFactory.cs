using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;

namespace Z.Common.Runtime
{
#warning DynamicDuckFactory missing support for out/in/ref parameters, operator overloading methods and generics

    public interface IA
    {
        int MyProperty { get; set; }
    }

    public class Dummy
    {
        private IA _dummy;

        public int MyProperty
        {
            get { return _dummy.MyProperty; }
            set { _dummy.MyProperty = value; }
        }

        public Dummy(IA dummy)
        {
            _dummy = dummy;
        }
    }

    /// <summary>
    /// Generates types dyanmically that are proxy wrappers to another type, lowering dependencies
    /// </summary>
    public static class DynamicDuckFactory
    {
        private static readonly AssemblyName _asmName = new AssemblyName("DynamicDucksAssembly");
        private static readonly AssemblyBuilder _asmBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(_asmName, AssemblyBuilderAccess.RunAndSave);
        private static readonly ModuleBuilder _moduleBuilder = _asmBuilder.DefineDynamicModule(_asmName.Name, _asmName.Name + ".dll");

        private static BindingFlags _defaultBindingFlags = BindingFlags.Public | BindingFlags.Instance;

        private static Exception WrappedNotImplementingInheritsFrom(string reason)
        {
            return new Exception(string.Format("Wrapped type does not implement InheritsFrom type : {0}", reason));
        }

        public static TInheritsFrom CreateDynamicDuck<TWrapped, TInheritsFrom>(TWrapped wrappedObject) where TInheritsFrom : class
        {
            //check if wrapped object implements the interface
            Type tWrapped = typeof(TWrapped);
            Type tInheritsFrom = typeof(TInheritsFrom);
            
            string newTypeName = string.Format("DynamicDuck_{0}_{1}", tWrapped.Name, tInheritsFrom.Name);
            Type existing = _asmBuilder.GetType(newTypeName);
            if (existing != null)
                return Activator.CreateInstance(existing, wrappedObject) as TInheritsFrom;

            var methodsToImplement = tInheritsFrom.GetMethods(_defaultBindingFlags);
            var propsToImpelemnt = tInheritsFrom.GetProperties(_defaultBindingFlags);

            VerifyInheritedMethodsExist(tWrapped, methodsToImplement);
            VerifyInheritedPropertiesExist(tWrapped, propsToImpelemnt);

            var inheritedInterfaces = new Type[] { tInheritsFrom };
            var typeAttributes = TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.AutoClass | TypeAttributes.AnsiClass | TypeAttributes.BeforeFieldInit;
            var methodAttributes = MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual | MethodAttributes.Final;
            var propAttributes = methodAttributes | MethodAttributes.SpecialName;

            TypeBuilder newTypeBuilder = _moduleBuilder.DefineType(newTypeName, typeAttributes, typeof(object), inheritedInterfaces);

            // private TWrapped _wrapped;
            FieldBuilder wrappedBuilder = newTypeBuilder.DefineField("_wrapped", typeof(TWrapped), FieldAttributes.Private);

            // public DynamicDuck(TWrapped wrapped) { _wrapped = wrapped; }
            GenCtor(newTypeBuilder, tWrapped, wrappedBuilder);

            Dictionary<string, PropertyBuilder> props = new Dictionary<string, PropertyBuilder>();
            foreach (var method in methodsToImplement)
            {
                //copy the method signature, implement as a call to _wrapped
                Type[] paramTypes = method.GetParameters().Select(x => x.ParameterType).ToArray();
                if (paramTypes.Length == 0)
                    paramTypes = Type.EmptyTypes;

                if (method.IsSpecialName)
                {
                    MethodBuilder methodBuilder = newTypeBuilder.DefineMethod(method.Name, propAttributes, method.CallingConvention, method.ReturnType, paramTypes);

                    if (method.Name.StartsWith("get_"))
                        GenGetter(newTypeBuilder, tWrapped, wrappedBuilder, props, method, methodBuilder);
                    else if (method.Name.StartsWith("set_"))
                        GenSetter(newTypeBuilder, tWrapped, wrappedBuilder, props, method, methodBuilder);
                }
                else
                {
                    MethodBuilder methodBuilder = newTypeBuilder.DefineMethod(method.Name, methodAttributes, method.CallingConvention, method.ReturnType, paramTypes);
                    GenMethod(tWrapped, wrappedBuilder, method, methodBuilder);
                }
            }

            Type newType = newTypeBuilder.CreateType();
            TInheritsFrom retVal = Activator.CreateInstance(newType, wrappedObject) as TInheritsFrom;
            return retVal;
        }

        private static void GenMethod(Type tWrapped, FieldBuilder wrappedBuilder, MethodInfo method, MethodBuilder methodBuilder)
        {
            int i = 1;
            ParameterInfo[] parameters = method.GetParameters();
            foreach (ParameterInfo pInfo in parameters)
            {
                ParameterBuilder paramBuilder = null;
                object[] customAttribs = pInfo.GetCustomAttributes(true);
                if (customAttribs.Length > 0)
                {
                    foreach (var attrib in customAttribs)
                    {
                        if (attrib.GetType() == typeof(ParamArrayAttribute))
                        {
                            paramBuilder = methodBuilder.DefineParameter(i++, ParameterAttributes.In, pInfo.Name);
                            ConstructorInfo ctorInfo = typeof(ParamArrayAttribute).GetConstructor(Type.EmptyTypes);
                            CustomAttributeBuilder attrBldr = new CustomAttributeBuilder(ctorInfo, new object[0]);
                            paramBuilder.SetCustomAttribute(attrBldr);
                        }
                    }
                }
                else
                {
                    paramBuilder = methodBuilder.DefineParameter(i++, pInfo.Attributes, pInfo.Name);
                }
            }

            ILGenerator methodILGen = methodBuilder.GetILGenerator();
            methodILGen.Emit(OpCodes.Ldarg_0);
            methodILGen.Emit(OpCodes.Ldfld, wrappedBuilder);
            for (short j = 1; j < parameters.Length + 1; j++)
            {
                methodILGen.Emit(OpCodes.Ldarg, j);
            }

            Type[] parameterTypes = parameters.Select(x => x.ParameterType).ToArray();
            var wrappedMethod = tWrapped.GetMethod(method.Name, BindingFlags.Public | BindingFlags.Instance, null, parameterTypes, null);
            methodILGen.EmitCall(OpCodes.Callvirt, wrappedMethod, parameters.Select(x => x.ParameterType).ToArray());
            methodILGen.Emit(OpCodes.Ret);
        }

        private static void GenSetter(TypeBuilder newTypeBuilder, Type tWrapped, FieldBuilder wrappedBuilder, Dictionary<string, PropertyBuilder> definedprops, MethodInfo methodToGen, MethodBuilder methodBuilder)
        {
            var methodIL = methodBuilder.GetILGenerator();
            string propName = methodToGen.Name.Substring(4);
            var propType = methodToGen.GetParameters()[0].ParameterType;
            var propTypes = new Type[] { propType };
            var wrappedMethod = tWrapped.GetMethod(methodToGen.Name, BindingFlags.Public | BindingFlags.Instance);
            methodIL.Emit(OpCodes.Ldarg_0);
            methodIL.Emit(OpCodes.Ldfld, wrappedBuilder);
            methodIL.Emit(OpCodes.Ldarg_1);
            methodIL.EmitCall(OpCodes.Callvirt, wrappedMethod, propTypes);
            methodIL.Emit(OpCodes.Ret);

            PropertyBuilder propBuilder = null;
            if (!definedprops.TryGetValue(propName, out propBuilder))
                definedprops[propName] = propBuilder = newTypeBuilder.DefineProperty(propName, PropertyAttributes.None, propType, Type.EmptyTypes);

            propBuilder.SetSetMethod(methodBuilder);
        }

        private static void GenGetter(TypeBuilder newTypeBuilder, Type tWrapped, FieldBuilder wrappedBuilder, Dictionary<string, PropertyBuilder> definedprops, MethodInfo methodToGen, MethodBuilder methodBuilder)
        {
            var methodIL = methodBuilder.GetILGenerator();
            string propName = methodToGen.Name.Substring(4); var propType = methodToGen.ReturnType;
            var propTypes = new Type[] { propType };
            var wrappedMethod = tWrapped.GetMethod(methodToGen.Name, BindingFlags.Public | BindingFlags.Instance);

            methodIL.Emit(OpCodes.Ldarg_0);
            methodIL.Emit(OpCodes.Ldfld, wrappedBuilder);
            methodIL.EmitCall(OpCodes.Callvirt, wrappedMethod, Type.EmptyTypes);
            methodIL.Emit(OpCodes.Ret);

            PropertyBuilder propBuilder = null;
            if (!definedprops.TryGetValue(propName, out propBuilder))
                definedprops[propName] = propBuilder = newTypeBuilder.DefineProperty(propName, PropertyAttributes.None, propType, Type.EmptyTypes);

            propBuilder.SetGetMethod(methodBuilder);
        }

        private static void GenCtor(TypeBuilder newTypeBuilder, Type tWrapped, FieldBuilder wrappedBuilder)
        {
            Type[] parameterTypes = { tWrapped };
            ConstructorBuilder ctorBuilder = newTypeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, parameterTypes);
            ILGenerator ctorIL = ctorBuilder.GetILGenerator();

            // For a constructor, argument zero is a reference to the new
            // instance. Push it on the stack before calling the base
            // class constructor. Specify the default constructor of the 
            // base class (System.Object) by passing an empty array of 
            // types (Type.EmptyTypes) to GetConstructor.
            ctorIL.Emit(OpCodes.Ldarg_0);
            ctorIL.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));

            // Push the instance on the stack before pushing the argument
            // that is to be assigned to the private field m_number.
            ctorIL.Emit(OpCodes.Ldarg_0);
            ctorIL.Emit(OpCodes.Ldarg_1);
            ctorIL.Emit(OpCodes.Stfld, wrappedBuilder);
            ctorIL.Emit(OpCodes.Ret);
        }

        private static void VerifyInheritedPropertiesExist(Type tWrapped, PropertyInfo[] propsToImpelemnt)
        {
            foreach (var property in propsToImpelemnt)
            {
                Type[] indexParams = property.GetIndexParameters().Select(x => x.ParameterType).ToArray();
                if (indexParams.Length == 0)
                    indexParams = Type.EmptyTypes;

                PropertyInfo implProperty = tWrapped.GetProperty(property.Name, _defaultBindingFlags, null, property.PropertyType, indexParams, null);
                if (implProperty == null)
                    throw WrappedNotImplementingInheritsFrom(string.Format("Property {0} does not exist in {1}", implProperty, tWrapped.FullName));
            }
        }

        private static void VerifyInheritedMethodsExist(Type tWrapped, MethodInfo[] methodsToImplement)
        {
            foreach (var method in methodsToImplement)
            {
                Type[] paramTypes = method.GetParameters().Select(x => x.ParameterType).ToArray();
                if (paramTypes.Length == 0)
                    paramTypes = Type.EmptyTypes;

                Type retType = method.ReturnType;
                MethodInfo implMethod = tWrapped.GetMethod(method.Name, _defaultBindingFlags, null, paramTypes, null);
                if (implMethod == null)
                    throw WrappedNotImplementingInheritsFrom(string.Format("Method {0} does not exist in {1}", method, tWrapped.FullName));

                if (implMethod.ReturnType != method.ReturnType)
                    throw WrappedNotImplementingInheritsFrom(string.Format("Method {0} differs in return value from {1}", method, implMethod));
            }
        }
    }
}
