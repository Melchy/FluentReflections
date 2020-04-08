using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyModel;

namespace FluentReflections
{
    public static class SolutionReflection
    {
        public static IEnumerable<TypeReflection> GetAllTypesImplementing<T>()
        {
            return GetAllTypesImplementing(typeof(T));
        }
        
        public static IEnumerable<TypeReflection> GetAllTypesImplementing(Type baseType)
        {
            var assembliesReferencingType = GetAllAssembliesReferencingType(baseType);
            return assembliesReferencingType.SelectMany(x => x.GetAllTypesImplementing(baseType));
        }

        public static IEnumerable<TypeReflection> GetAllTypesImplementing<T>(string assembliesPrefix)
        {
            return GetAllTypesImplementing(typeof(T), assembliesPrefix);
        }
        
        public static IEnumerable<TypeReflection> GetAllTypesImplementing(Type baseType, string assembliesPrefix)
        {
            var assebliesReferencingType = GetAllAssembliesReferencingType(baseType, assembliesPrefix);
            return assebliesReferencingType.SelectMany(x => x.GetAllTypesImplementing(baseType));
        }

        public static IEnumerable<TypeReflection> GetAllTypesWithAttribute<T>()
            where T : Attribute
        {
            return GetAllTypesWithAttribute(typeof(T));
        }
        
        public static IEnumerable<TypeReflection> GetAllTypesWithAttribute(Type attribute)
        {
            var assebliesReferencingType = GetAllAssembliesReferencingType(attribute);
            return assebliesReferencingType.SelectMany(x => x.GetAllTypesWithAttribute(attribute));
        }
        
        public static IEnumerable<TypeReflection> GetAllTypesWithAttribute<T>(string assembliesPrefix)
            where T : Attribute
        {
            return GetAllTypesWithAttribute(typeof(T), assembliesPrefix);
        }
        
        public static IEnumerable<TypeReflection> GetAllTypesWithAttribute(Type attribute, string assembliesPrefix)
        {
            var assebliesReferencingType = GetAllAssembliesReferencingType(attribute, assembliesPrefix);
            return assebliesReferencingType.SelectMany(x => x.GetAllTypesWithAttribute(attribute));
        }

        public static IEnumerable<AssemblyReflection> GetAllAssembliesReferencingType<T>()
        {
            return GetAllAssembliesReferencingType(typeof(T));
        }
        
        public static IEnumerable<AssemblyReflection> GetAllAssembliesReferencingType(Type type)
        {
            var runtimeAssemblyNames = DependencyContext.Default.RuntimeLibraries;
            var assembliesReferencingType = GetAllAssembliesThatReferenceType(type, runtimeAssemblyNames);
            return assembliesReferencingType.Select(x => x.Reflection());
        }

        public static IEnumerable<AssemblyReflection> GetAllAssembliesReferencingType<T>(string assembliesPrefix)
        {
            return GetAllAssembliesReferencingType(typeof(T), assembliesPrefix);
        }
        
        public static IEnumerable<AssemblyReflection> GetAllAssembliesReferencingType(Type type, string assembliesPrefix)
        {
            var runtimeAssemblyNames = DependencyContext.Default.RuntimeLibraries;
            var assembliesFilteredByName = runtimeAssemblyNames.Where(x =>
                x.Name.StartsWith(assembliesPrefix, StringComparison.InvariantCulture));

            var assembliesReferencingType = GetAllAssembliesThatReferenceType(type, assembliesFilteredByName);
            return assembliesReferencingType.Select(x => x.Reflection());
        }

        private static IEnumerable<Assembly> GetAllAssembliesThatReferenceType(Type type,
            IEnumerable<RuntimeLibrary> runtimeLibrariesToSearch)
        {
            var searchedAssemblyName = type.Assembly.GetName().Name;
            var asseblies = runtimeLibrariesToSearch.Where(
                x => x.Dependencies.Any(y => y.Name == searchedAssemblyName
                )).ToList();

            var assemblyOfSearchedType = Assembly.Load(type.Assembly.FullName);
            return  asseblies.Select(x => Assembly.Load(new AssemblyName(x.Name)))
                .Concat(new []{assemblyOfSearchedType});
        }
    }
}