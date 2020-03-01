using System;
using System.Reflection;

namespace FluentReflections
{
    public class PropertyOrFieldInstanceReflection : PropertyOrFieldReflection
    {
        private object _value;
        private object _enclosingType;

        public PropertyOrFieldInstanceReflection(FieldInfo fieldInfo, object value, object enclosingType) : base(fieldInfo)
        {
            var isOrImplements = value.Reflection().IsOrImplements(fieldInfo.FieldType);
            if (isOrImplements == false)
            {
                throw new InvalidOperationException($"Type of value must be or implement type passed in {nameof(fieldInfo)}. {value.GetType()} does not implement {fieldInfo.Name}");
            }

            _value = value;
            if (!enclosingType.Reflection().HasPropertyOrField(fieldInfo.Name))
            {
                throw new InvalidOperationException($"Enclosing type must contain given field. Type {enclosingType.GetType()} does not contain field {fieldInfo.Name}");
            }
            _enclosingType = enclosingType;
        }
        
        public PropertyOrFieldInstanceReflection(PropertyInfo propertyInfo, object value, object enclosingType) : base(propertyInfo)
        {
            var isOrImplements = value.Reflection().IsOrImplements(propertyInfo.PropertyType);
            if (isOrImplements == false)
            {
                throw new InvalidOperationException($"Type of value must be or implement type passed in {nameof(propertyInfo)}. {value.GetType()} does not implement {propertyInfo.PropertyType.Name}");
            }

            _value = value;
            if (!enclosingType.Reflection().HasPropertyOrField(propertyInfo.Name))
            {
                throw new InvalidOperationException($"Enclosing type must contain given field. Type {enclosingType.GetType()} does not contain property {propertyInfo.Name}");
            }
            _enclosingType = enclosingType;
        }

        public void SetValue(object value)
        {
            if (IsProperty())
            {
                Property.SetValue(_enclosingType, value);
                _value = value;
            }
            else
            {
                Field.SetValue(_enclosingType, value);
                _value = value;
            }
        }

        public object GetValue()
        {
            return _value;
        }
        
        public TType GetValue<TType>()
        {
            if (!_value.Reflection().IsOrImplements<TType>())
            {
                throw new InvalidOperationException($"Property or field {Member.Name} can not be casted to type {typeof(TType)}");
            }
            return (TType) _value;
        }
        
        public object GetEnclosingType()
        {
            return _enclosingType;
        }
        
        public TType GetEnclosingType<TType>()
        {
            if (!_enclosingType.Reflection().IsOrImplements<TType>())
            {
                throw new InvalidOperationException($"Enclosing type {_enclosingType.GetType().Name} can not be casted to type {typeof(TType)}");
            }
            return (TType) _enclosingType;
        }
    }
}