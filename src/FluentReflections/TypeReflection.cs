using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace FluentReflections
{
    public class TypeReflection
    {
        public Type Type { get; }

        public TypeReflection(Type type)
        {
            Type = type;
        }

        public virtual bool HasAttribute<TAttribute>()
            where TAttribute : Attribute
        {
            return HasAttribute(typeof(TAttribute));
        }

        public virtual bool HasAttribute(Type attributeType)
        {
            if (!attributeType.Reflection().IsAttribute())
                throw new InvalidOperationException(
                    $"Type {attributeType.Name} is not attribute. Attribute must implement {nameof(Attribute)} class.");
            return Attribute.GetCustomAttribute(Type, attributeType) != null;
        }

        public TAttribute GetAttribute<TAttribute>() where TAttribute : Attribute
        {
            var queueNameAttribute = GetAttribute(typeof(TAttribute));
            return (TAttribute) queueNameAttribute;
        }

        public Attribute GetAttribute(Type attribute)
        {
            if (!attribute.Reflection().IsAttribute())
                throw new InvalidOperationException(
                    $"Type {attribute.Name} is not attribute. Attribute must implement {nameof(Attribute)} class.");
            var queueNameAttribute = Attribute.GetCustomAttribute(Type, attribute);
            if (queueNameAttribute == null)
            {
                throw new InvalidOperationException($"{Type} does not define ${attribute.Name}.");
            }

            return queueNameAttribute;
        }

        public bool Implements<TType>()
        {
            return Implements(typeof(TType));
        }

        public bool IsOrImplements<TType>()
        {
            return IsOrImplements(typeof(TType));
        }

        public bool Implements(Type type)
        {
            return IsOrImplements(type) && Type != type;
        }

        public bool IsOrImplements(Type type)
        {
            if (type.IsGenericTypeDefinition) //open generic types can not be checked using IsAssignableFrom
            {
                return ImplementsOpenGenericType(type);
            }

            return type.IsAssignableFrom(Type);
        }

        public bool HasEmptyConstructor()
        {
            var constructor = Type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
            return constructor != null;
        }
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns>Returns newly created instance or null for nullable value type.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public object? CreateInstance(params object[] args)
        {
            try
            {
                var instance = Activator.CreateInstance(Type, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, args, null);
                return instance;
            }
            catch (MissingMethodException e)
            {
                throw new InvalidOperationException($"Type {Type.Name} does not have constructor with {args.Length} arguments or some arguments have incorrect types.");
            }
        }
        
        public object CreateInstance<TResult>(params object[] args)
        {
            var instance = CreateInstance(args);
            if (instance is TResult result)
            {
                return result;
            }
            else
            {
                throw new InvalidOperationException($"Type {Type.Name} could not be casted to ${nameof(TResult)}");
            }
        }
        

        public bool IsSimpleType()
        {
            return Type.IsPrimitive ||
                   new Type[]
                   {
                       typeof(String),
                       typeof(Decimal),
                       typeof(DateTime),
                       typeof(DateTimeOffset),
                       typeof(TimeSpan),
                       typeof(Guid),
                       typeof(object)
                   }.Contains(Type);
        }

        public IEnumerable<MethodReflection> GetMethods()
        {
            return Type.GetMethods(BindingFlagsAny.Get())
                .Where(x=>!(x.Name.StartsWith("set") || x.Name.StartsWith("get")))
                .Select(x => x.Reflection());
        }
        
        public IEnumerable<MethodReflection> GetMethodsWithoutParentMethods()
        {
            return Type.GetMethods(BindingFlagsAny.Get() | BindingFlags.DeclaredOnly)
                .Where(x=>!(x.Name.StartsWith("set") || x.Name.StartsWith("get")))
                .Select(x => x.Reflection());
        }
        
        public bool HasMethod(string propertyName)
        {
            return Type.GetMethod(propertyName, BindingFlagsAny.Get()) != null;
        }
        
        public MethodReflection GetMethod(string methodName)
        {
            var method = Type.GetMethod(methodName, BindingFlagsAny.Get());
            if(method == null)
                throw new InvalidOperationException($"Type {Type.Name} does not implement method {methodName}");
            
            return method.Reflection();
        }
        
        public IEnumerable<PropertyOrFieldReflection> GetPropertiesAndFields()
        {
            var properties = Type.GetProperties(BindingFlagsAny.Get())
                .Where(x=>x.GetIndexParameters().Length == 0) //filter out indexers
                .Select(x => x.Reflection());
            var fields = Type.GetFields(BindingFlagsAny.Get())
                .Select(x => x.Reflection())
                .Where(x=>!x.HasAttribute<CompilerGeneratedAttribute>());
            return properties.Concat(fields);
        }
        
        public bool HasPropertyOrField(string propertyOrFieldName)
        {
            var field = Type.GetField(propertyOrFieldName, BindingFlagsAny.Get());
            if (field != null) return true;
            var property = Type.GetProperty(propertyOrFieldName, BindingFlagsAny.Get());
            if (property != null) return true;
            return false;
        }
        
        public PropertyOrFieldReflection GetPropertyOrField(string propertyOrFieldName)
        {
            var field = Type.GetField(propertyOrFieldName, BindingFlagsAny.Get());
            if (field != null) return field.Reflection();
            var property = Type.GetProperty(propertyOrFieldName, BindingFlagsAny.Get());
            if(property == null)
                throw new InvalidOperationException($"Type {Type.Name} does not implement property or field {property}");
            return property.Reflection();
        }

        private bool ImplementsOpenGenericType(Type generic)
        {
            var toCheck = Type;
            while (toCheck != null && toCheck != typeof(object)) {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur) {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }
        
        public bool IsAttribute()
        {
            return Implements<Attribute>();
        }
        
        public bool IsVoid()
        {
            return Type == typeof(void);
        }

        public object? InvokeStaticMethod(string name, params object[] arguments)
        {
            return GetMethod(name).MethodInfo.Invoke(null, arguments);
        }
        
        public object? InvokeStaticGenericMethod(string name, Type[] genericTypes, params object[] arguments)
        {
            var genericMethod = GetMethod(name).MethodInfo.MakeGenericMethod(genericTypes);
            return genericMethod.Invoke(null, arguments);
        }
    }
}