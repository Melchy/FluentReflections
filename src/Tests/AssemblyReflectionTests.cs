using System.Linq;
using FluentAssertions;
using FluentReflections;
using Xunit;

namespace Tests
{
    public class AssemblyReflectionTests
    {
        [Fact]
        public void GetAllImplementing()
        {
            var a = typeof(BaseNonGeneric)
                .Assembly.Reflection().GetAllTypesImplementing<BaseNonGeneric>()
                .Select(x=>x.Type);
            a.Should().Contain(typeof(ClassToAnalyze));
        }
        
        [Fact]
        public void GetAllTypesWithAttribute()
        {
            var a = typeof(TestAttribute)
                .Assembly.Reflection().GetAllTypesWithAttribute<TestAttribute>()
                .Select(x=>x.Type);
            a.Should().Contain(typeof(ClassToAnalyze));
        }
    }
}