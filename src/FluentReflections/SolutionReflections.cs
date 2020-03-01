using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyModel;

namespace FluentReflections
{
    public static class SolutionReflections
    {
        public static IEnumerable<TypeReflection> GetAllTypesImplementing<T>()
        {
            var assebliesReferencingType = GetAllAssembliesReferencingType<T>();
            return assebliesReferencingType.SelectMany(x => x.GetAllTypesImplementing<T>());
        }

        public static IEnumerable<TypeReflection> GetAllTypesImplementing<T>(string assembliesPrefix)
        {
            var assebliesReferencingType = GetAllAssembliesReferencingType<T>(assembliesPrefix);
            return assebliesReferencingType.SelectMany(x => x.GetAllTypesImplementing<T>());
        }

        public static IEnumerable<TypeReflection> GetAllTypesWithAttribute<T>(string assembliesPrefix)
            where T : Attribute
        {
            var assebliesReferencingType = GetAllAssembliesReferencingType<T>(assembliesPrefix);
            return assebliesReferencingType.SelectMany(x => x.GetAllTypesWithAttribute<T>());
        }

        private static IEnumerable<AssemblyReflections> GetAllAssembliesReferencingType<T>()
        {
            var runtimeAssemblyNames = DependencyContext.Default.RuntimeLibraries;
            var assembliesReferencingType = GetAssembliesReferencingAssemblyWithType<T>(runtimeAssemblyNames);
            return assembliesReferencingType.Select(x => x.Reflection());
        }

        private static IEnumerable<AssemblyReflections> GetAllAssembliesReferencingType<T>(string assembliesPrefix)
        {
            var runtimeAssemblyNames = DependencyContext.Default.RuntimeLibraries;
            var assembliesFilteredByName = runtimeAssemblyNames.Where(x =>
                x.Name.StartsWith(assembliesPrefix, StringComparison.InvariantCulture));

            var assembliesReferencingType = GetAssembliesReferencingAssemblyWithType<T>(assembliesFilteredByName);
            return assembliesReferencingType.Select(x => x.Reflection());
        }

        private static IEnumerable<Assembly> GetAssembliesReferencingAssemblyWithType<T>(
            IEnumerable<RuntimeLibrary> runtimeLibrariesToSearch)
        {
            var searchedAssemblyName = typeof(T).Assembly.GetName().Name;
            var asseblies = runtimeLibrariesToSearch.Where(
                x => x.Dependencies.Any(y => y.Name == searchedAssemblyName
                ));
            return asseblies.Select(x => Assembly.Load(new AssemblyName(x.Name)));
        }
    }
}