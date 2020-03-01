using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentReflections
{
    public class MethodReflection
    {
        public MethodInfo MethodInfo { get; }
        

        public MethodReflection(MethodInfo methodInfo)
        {
            MethodInfo = methodInfo;
        }

        public IEnumerable<ParameterReflection> GetParameters()
        {
            return MethodInfo.GetParameters().Select(x => x.Reflection());
        }
        
        public TypeReflection GetReturnType()
        {
            return MethodInfo.ReturnType.Reflection();
        }

        public bool HasAttribute<TAttribute>() where  TAttribute : Attribute
        {
            return HasAttribute(typeof(TAttribute));
        }
        
        public bool HasAttribute(Type attributeType)
        {
            if(!attributeType.Reflection().IsAttribute())
                throw new InvalidOperationException($"Type {attributeType.Name} is not attribute. Attributes must implement {nameof(Attribute)}.");
            return MethodInfo.GetCustomAttributes(attributeType, true).Any();
        }

        public TAttribute GetAttribute<TAttribute>() where TAttribute : Attribute
        {
            var attribute = GetAttribute(typeof(TAttribute));
            return (TAttribute) attribute;
        }
        
        public Attribute GetAttribute(Type attributeType)
        {
            if(!attributeType.Reflection().IsAttribute())
                throw new InvalidOperationException($"Type {attributeType.Name} is not attribute. Attributes must implement {nameof(Attribute)}.");

            var attribute = MethodInfo.GetCustomAttributes(attributeType, true).FirstOrDefault();
            if(attribute == null)
                throw new InvalidOperationException($"Method {MethodInfo.Name} does not define attribute ${attributeType.Name}");
            
            return (Attribute)attribute;
        }

        public bool IsPublic()
        {
            return MethodInfo.IsPublic;
        }
        
        public bool IsPrivate()
        {
            return MethodInfo.IsPrivate;
        }
        
        public bool IsProtected()
        {
            return MethodInfo.IsFamily;
        }
        
        public bool IsInternal()
        {
            return MethodInfo.IsAssembly;
        }
        
        public bool IsStatic()
        {
            return MethodInfo.IsStatic;
        }

        public string GetName()
        {
            return MethodInfo.Name;
        }
        
        public bool IsGeneric()
        {
            return MethodInfo.IsGenericMethod;
        }
    }
}