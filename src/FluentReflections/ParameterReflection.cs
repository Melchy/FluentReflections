using System;
using System.Linq;
using System.Reflection;

namespace FluentReflections
{
    public class ParameterReflection
    {
        public ParameterInfo ParameterInfo { get; }
        
        public ParameterReflection(ParameterInfo parameterInfo)
        {
            ParameterInfo = parameterInfo;
        }

        public string GetName()
        {
            return ParameterInfo.Name;
        }

        public int GetPosition()
        {
            return ParameterInfo.Position;
        }

        public new TypeReflection GetType()
        {
            return ParameterInfo.ParameterType.Reflection();
        }
        
        public bool HasAttribute<TAttribute>()
        {
            return HasAttribute(typeof(TAttribute));
        }
        
        public bool HasAttribute(Type attributeType)
        {
            if(!attributeType.Reflection().IsAttribute())
                throw new InvalidOperationException($"Type {attributeType.Name} is not attribute. Attributes must implement {nameof(Attribute)}.");
            return ParameterInfo.GetCustomAttributes(attributeType, true).Any();
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

            var attribute = ParameterInfo.GetCustomAttributes(attributeType, true).FirstOrDefault();
            if(attribute == null)
                throw new InvalidOperationException($"Method {ParameterInfo.Name} does not define attribute ${attributeType.Name}");
            
            return (Attribute)attribute;
        }
    }
}