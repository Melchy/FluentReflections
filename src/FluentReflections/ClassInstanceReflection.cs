using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Linq;

namespace FluentReflections
{
    public class ClassInstanceReflection : TypeReflection
    {
        private object Instance { get; }

        public T GetValue<T>()
        {
            if (!IsOrImplements<T>())
            {
                throw new InvalidOperationException($"Type {Type.Name} can not be casted to type {typeof(T)}");
            }
            return (T) Instance;
        }

        public object GetValue()
        {
            return Instance;
        }

        [SuppressMessage("", "CA1000")]
        public static ClassInstanceReflection CreateInstanceTypeReflection(object instance)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            return new ClassInstanceReflection(instance, instance.GetType());
        }

        public ClassInstanceReflection(object instance, Type type) : base(type)
        {
            Instance = instance;
        }

        public new PropertyOrFieldInstanceReflection GetPropertyOrField(string name)
        {
            var propertyOrField = base.GetPropertyOrField(name);
            if (propertyOrField.IsProperty())
            {
                return new PropertyOrFieldInstanceReflection(propertyOrField.Property, propertyOrField.Property.GetValue(Instance, null), Instance);
            }
            else
            {
                return new PropertyOrFieldInstanceReflection(propertyOrField.Field, propertyOrField.Field.GetValue(Instance), Instance);
            }
        }
        
        public void SetPropertyOrField(string name, object value)
        {
            var propertyOrField = base.GetPropertyOrField(name);
            if (propertyOrField.IsProperty())
            {
                propertyOrField.Property.SetValue(Instance, value);
            }
            else
            {
                propertyOrField.Field.SetValue(Instance, value);
            }
        }

        public new IEnumerable<PropertyOrFieldInstanceReflection> GetPropertiesAndFields()
        {
            var propertiesAndFields = base.GetPropertiesAndFields().Select(x=>x.Name);
            return propertiesAndFields.Select(GetPropertyOrField);
        }

        public new IEnumerable<MethodInstanceReflection> GetMethods()
        {
            var methods = base.GetMethods();
            return methods.Select(x => new MethodInstanceReflection(x.MethodInfo, Instance));
        }
        
        public new MethodInstanceReflection GetMethod(string name)
        {
            var method = base.GetMethod(name);
            return new MethodInstanceReflection(method.MethodInfo, Instance);
        }

        public IDictionary<string, object> GetPropertiesAndFieldsAsDictionary()
        {
            var propertiesAndFields = GetPropertiesAndFields();
            return propertiesAndFields.ToDictionary(x => x.Name, x => x.GetValue());
        }
        
        public dynamic GetPropertiesAndFieldsAsDynamic()
        {
            var propertiesAndFields = GetPropertiesAndFields();
            IDictionary<string, object> result = new ExpandoObject();
            foreach (var propertyAndField in propertiesAndFields)
            {
                result[propertyAndField.Name] = propertyAndField.GetValue();
            }

            return result;
        }
    }
}