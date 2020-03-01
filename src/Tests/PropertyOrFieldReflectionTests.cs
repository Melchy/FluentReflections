using System;
using FluentAssertions;
using FluentReflections;
using Xunit;

namespace Tests
{
    public class PropertyOrFieldReflectionTests
    {
        [Fact]
        public void HasAttribute()
        {
            var propertyOrFieldReflection = typeof(ClassToAnalyze).Reflection().GetPropertyOrField("PrivateProperty");
            var hasAttribute = propertyOrFieldReflection.HasAttribute<TestAttribute>();
            hasAttribute.Should().BeTrue();
        }
        
        [Fact]
        public void HasAttributeFalse()
        {
            var propertyOrFieldReflection = typeof(ClassToAnalyze).Reflection().GetPropertyOrField("PrivateProperty");
            var hasAttribute = propertyOrFieldReflection.HasAttribute<NotUsededAttribute>();
            hasAttribute.Should().BeFalse();
        }
        
        [Fact]
        public void HasAttributeThrowsForNonAttribute()
        {
            var propertyOrFieldReflection = typeof(ClassToAnalyze).Reflection().GetPropertyOrField("PrivateProperty");
            propertyOrFieldReflection.Invoking(x=> x.HasAttribute(typeof(int)))
                .Should()
                .Throw<InvalidOperationException>();
        }
        
        [Fact]
        public void GetAttribute()
        {
            var propertyOrFieldReflection = typeof(ClassToAnalyze).Reflection().GetPropertyOrField("PrivateProperty");
            var attribute = propertyOrFieldReflection.GetAttribute<TestAttribute>();
            attribute.Should().NotBeNull();
        }
        
        [Fact]
        public void GetAttributeThrowsIfDoesntHaveAttribute()
        {
            var propertyOrFieldReflection = typeof(ClassToAnalyze).Reflection().GetPropertyOrField("PrivateProperty");
            propertyOrFieldReflection.Invoking(x=> x.GetAttribute(typeof(NotUsededAttribute)))
                .Should()
                .Throw<InvalidOperationException>();
        }
        
        [Fact]
        public void GetAttributeThrowsForNonAttribute()
        {
            var propertyOrFieldReflection = typeof(ClassToAnalyze).Reflection().GetPropertyOrField("PrivateProperty");
            propertyOrFieldReflection.Invoking(x=> x.GetAttribute(typeof(int)))
                .Should()
                .Throw<InvalidOperationException>();
        }
    }
}