using System;
using System.Reflection;

namespace FluentReflections
{
    public static class ReflectionFactory
    {
        public static TypeReflection Reflection(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            return new TypeReflection(type);
        }

        public static PropertyOrFieldReflection Reflection(this PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException(nameof(propertyInfo));
            return new PropertyOrFieldReflection(propertyInfo);
        }
        
        public static PropertyOrFieldReflection Reflection(this FieldInfo fieldInfo)
        {
            if (fieldInfo == null)
                throw new ArgumentNullException(nameof(fieldInfo));
            return new PropertyOrFieldReflection(fieldInfo);
        }

        public static ClassInstanceReflection Reflection<T>(this T instance)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));
            return ClassInstanceReflection.CreateInstanceTypeReflection(instance);
        }

        public static AssemblyReflections Reflection(this Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));
            return new AssemblyReflections(assembly);
        }

        public static MethodReflection Reflection(this MethodInfo methodInfo)
        {
            if (methodInfo == null)
                throw new ArgumentNullException(nameof(methodInfo));
            return new MethodReflection(methodInfo);
        }

        public static ParameterReflection Reflection(this ParameterInfo parameterInfo)
        {
            if (parameterInfo == null)
                throw new ArgumentNullException(nameof(parameterInfo));
            return new ParameterReflection(parameterInfo);
        }
    }
}