using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentReflections
{
    public class AssemblyReflections
    {
        private readonly Assembly _assembly;

        public AssemblyReflections(Assembly assembly)
        {
            _assembly = assembly;
        }

        public IEnumerable<TypeReflection> GetAllTypesImplementing<T>()
        {
            return GetAllTypesImplementing(typeof(T));
        }

        public IEnumerable<TypeReflection> GetAllTypesImplementing(Type typeToSearch)
        {
            return _assembly.GetTypes().Select(x => ReflectionFactory.Reflection((Type) x))
                .Where(x => x.Implements(typeToSearch));
        }

        public IEnumerable<TypeReflection> GetAllTypesWithAttribute<T>() where T : Attribute
        {
            return _assembly.GetTypes().Select(x => x.Reflection()).Where(x => x.HasAttribute<T>());
        }
    }
}