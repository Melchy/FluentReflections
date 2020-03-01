using FluentAssertions;
using FluentReflections;
using Xunit;

namespace Tests
{
    public class ClassInstanceReflectionTests
    {
        [Fact]
        public void SetPropertyOrField()
        {
            var methodReflection = new ClassToAnalyze(1,1, 1).Reflection();
            methodReflection.SetPropertyOrField("_privateField", "test");
            var newValue = methodReflection.GetPropertyOrField("_privateField").GetValue<string>();
            newValue.Should().Be("test");
        }
    }
}