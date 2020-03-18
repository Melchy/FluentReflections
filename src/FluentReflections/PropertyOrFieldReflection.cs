using System;
using System.Linq;
using System.Reflection;

namespace FluentReflections
{
    public class PropertyOrFieldReflection
    {
        public PropertyInfo? Property { get; }
        public FieldInfo? Field { get; }
        public MemberInfo Member { get; }
        public string Name => Member.Name;

        public PropertyOrFieldReflection(PropertyInfo property)
        {
            Member = property;
            Property = property;
        }
        
        public PropertyOrFieldReflection(FieldInfo fieldInfo)
        {
            Member = fieldInfo;
            Field = fieldInfo;
        }

        public new TypeReflection GetType()
        {
            if (Field != null)
                return Field.FieldType.Reflection();
            return Property.PropertyType.Reflection();
        }
        
        public bool HasAttribute<TAttribute>() where TAttribute : Attribute
        {
            return HasAttribute(typeof(TAttribute));
        }

        public bool HasAttribute(Type attributeType)
        {
            if (!attributeType.Reflection().IsAttribute())
                throw new InvalidOperationException(
                    $"Type {attributeType.Name} is not attribute. Attribute must implement {nameof(Attribute)} class.");
            
            var attribute = Member.CustomAttributes.FirstOrDefault(x => x.AttributeType == attributeType);
            if (attribute == null)
                return false;
            return true;
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

            var attribute = Member.GetCustomAttributes(attributeType, true).FirstOrDefault();
            if(attribute == null)
                throw new InvalidOperationException($"Method {Member.Name} does not define attribute ${attributeType.Name}");
            
            return (Attribute)attribute;
        }

        public bool IsField()
        {
            return Field != null;
        }
        
        public bool IsProperty()
        {
            return Property != null;
        }
    }
}