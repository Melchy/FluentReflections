using System;
using System.Reflection;

namespace FluentReflections
{
    public class MethodInstanceReflection : MethodReflection
    {
        private object _enclosingType;

        public MethodInstanceReflection(MethodInfo methodInfo, object enclosingType) : base(methodInfo)
        {
            if (!enclosingType.Reflection().HasMethod(methodInfo.Name))
            {
                throw new InvalidOperationException($"Enclosing type must contain given method. Type {enclosingType.GetType()} does not contain method {methodInfo.Name}");
            }
            _enclosingType = enclosingType;
        }

        public object? Invoke(params object?[] arguments)
        {
            if (IsGeneric())
            {
                throw new InvalidOperationException("Can not call invoke on generic method. Call InvokeGeneric instead.");
            }
            return MethodInfo.Invoke(_enclosingType, arguments);
        }
        
        public object? InvokeGeneric(Type[] genericTypes, params object?[] arguments)
        {
            if (!IsGeneric())
            {
                throw new InvalidOperationException("Can not call invokeGeneric on non generic method. Call Invoke instead.");
            }
            var genericMethod = MethodInfo.MakeGenericMethod(genericTypes);
            return genericMethod.Invoke(_enclosingType, arguments);
        }

        public object GetEnclosingType()
        {
            return _enclosingType;
        }
        
        public object GetEnclosingType<TType>()
        {
            if (!_enclosingType.Reflection().IsOrImplements<TType>())
            {
                throw new InvalidOperationException($"Enclosing type {_enclosingType.GetType().Name} can not be casted to type {typeof(TType)}");
            }
            return (TType) _enclosingType;
        }
    }
}