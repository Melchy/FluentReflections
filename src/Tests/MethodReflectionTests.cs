using System;
using FluentAssertions;
using FluentReflections;
using Xunit;

namespace Tests
{
    public class MethodReflectionTests
    {
        [Fact]
        public void GetReturnTypeOnVoidReturnsVoidType()
        {
            var methodReflection = typeof(ClassToAnalyze).Reflection().GetMethod("PrivateMethod");
            var returnType = methodReflection.GetReturnType();
            returnType.IsVoid().Should().BeTrue();
        }
        
        [Fact]
        public void HasAttribute()
        {
            var methodReflection = typeof(ClassToAnalyze).Reflection().GetMethod("PrivateMethod");
            var hasAttribute = methodReflection.HasAttribute<TestAttribute>();
            hasAttribute.Should().BeTrue();
        }
        
        [Fact]
        public void GetAttribute()
        {
            var methodReflection = typeof(ClassToAnalyze).Reflection().GetMethod("PrivateMethod");
            var attribute = methodReflection.GetAttribute<TestAttribute>();
            attribute.Should().NotBeNull();
        }
        
        [Fact]
        public void GetAttributeWithAttributeThatIsNotDefinedThrowsError()
        {
            var methodReflection = typeof(ClassToAnalyze).Reflection().GetMethod("PrivateMethod");
            Action sutCall = () => methodReflection.GetAttribute<NotUsededAttribute>();
            sutCall.Should().Throw<InvalidOperationException>();
        }
    }
}