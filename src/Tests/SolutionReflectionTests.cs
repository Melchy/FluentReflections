using System.Linq;
using FluentAssertions;
using FluentReflections;
using Xunit;

namespace Tests
{
    public class SolutionReflectionTests
    {
        [Fact]
        public void GetAllTypesInSolution()
        {
            var solutionReflectionTests = SolutionReflection.GetAllTypesImplementing<BaseNonGeneric>()
                .FirstOrDefault(x => x.Type == typeof(ClassToAnalyze));
            solutionReflectionTests.Should().NotBeNull();
        }
        
        [Fact]
        public void GetAllTypesInSolutionAssembliesFilteredByPrefix()
        {
            var solutionReflectionTests = SolutionReflection.GetAllTypesImplementing<BaseNonGeneric>("Te")
                .FirstOrDefault(x => x.Type == typeof(ClassToAnalyze));
            solutionReflectionTests.Should().NotBeNull();
        }
        
        [Fact]
        public void GetAllTypesInSolutionWithAttribute()
        {
            var solutionReflectionTests = SolutionReflection.GetAllTypesWithAttribute<TestAttribute>()
                .FirstOrDefault(x => x.Type == typeof(ClassToAnalyze));
            solutionReflectionTests.Should().NotBeNull();
        }
        
        [Fact]
        public void GetAllTypesInSolutionWithAttributeFilteredByPrefix()
        {
            var solutionReflectionTests = SolutionReflection.GetAllTypesWithAttribute<TestAttribute>("Te")
                .FirstOrDefault(x => x.Type == typeof(ClassToAnalyze));
            solutionReflectionTests.Should().NotBeNull();
        }
        
        [Fact]
        public void GetAllAssembliesReferencingType()
        {
            var solutionReflectionTests = SolutionReflection.GetAllAssembliesReferencingType<BaseNonGeneric>();
            solutionReflectionTests.Should().ContainSingle();
            solutionReflectionTests.First().Assembly.GetName().Name.Should().Be(typeof(BaseNonGeneric).Assembly.GetName().Name);
        }
        
        [Fact]
        public void GetAllAssembliesReferencingTypeFilteredByPrefix()
        {
            var solutionReflectionTests = SolutionReflection.GetAllAssembliesReferencingType<BaseNonGeneric>("Te");
            solutionReflectionTests.Should().ContainSingle();
            solutionReflectionTests.First().Assembly.GetName().Name.Should().Be(typeof(BaseNonGeneric).Assembly.GetName().Name);
        }
    }
}