using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentReflections
{
    public class AssemblyReflection
    {
        public Assembly Assembly { get; }

        public AssemblyReflection(Assembly assembly)
        {
            Assembly = assembly;
        }

        public IEnumerable<TypeReflection> GetAllTypesImplementing<T>()
        {
            return GetAllTypesImplementing(typeof(T));
        }

        public IEnumerable<TypeReflection> GetAllTypesImplementing(Type typeToSearch)
        {
            return Assembly.GetTypes().Select(x => ReflectionFactory.Reflection((Type) x))
                .Where(x => x.Implements(typeToSearch));
        }

        public IEnumerable<TypeReflection> GetAllTypesWithAttribute<T>() where T : Attribute
        {
            return GetAllTypesWithAttribute(typeof(T));
        }
        
        public IEnumerable<TypeReflection> GetAllTypesWithAttribute(Type type)
        {
            return Assembly.GetTypes().Select(x => x.Reflection()).Where(x => x.HasAttribute(type));
        }
    }
}