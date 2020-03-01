using System;
using System.Linq;
using FluentAssertions;
using FluentReflections;
using Xunit;

namespace Tests
{
    public class ParameterReflectionTests
    {
        [Fact]
        public void GetAttribute()
        {
            var methodReflection = typeof(ClassToAnalyze).Reflection().GetMethod("PrivateMethod");
            var attribute = methodReflection.GetParameters().First().GetAttribute<TestAttribute>();
            attribute.Should().NotBeNull();
        }
        
        [Fact]
        public void HasAttribute()
        {
            var methodReflection = typeof(ClassToAnalyze).Reflection().GetMethod("PrivateMethod");
            var attribute = methodReflection.GetParameters().First().HasAttribute<TestAttribute>();
            attribute.Should().BeTrue();
        }
        
        [Fact]
        public void GetAttributeThatIsNotUsedThrows()
        {
            var methodReflection = typeof(ClassToAnalyze).Reflection().GetMethod("PrivateMethod");
            var attributeReflection = methodReflection.GetParameters().First();
            attributeReflection.Invoking(x => x.GetAttribute<NotUsededAttribute>())
                .Should()
                .Throw<InvalidOperationException>();
        }
    }
}