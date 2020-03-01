using FluentAssertions;
using FluentReflections;
using Xunit;

namespace Tests
{
    public class PropertyOrFieldInstanceReflectionTests
    {
        [Fact]
        public void GetPropertyValue()
        {
            var propertyOrFieldReflection = new ClassToAnalyze(1,1, 1).Reflection().GetPropertyOrField("PrivateProperty");
            var value = propertyOrFieldReflection.GetValue<string>();
            value.Should().Be("PrivateProperty");
        }
        
        [Fact]
        public void SetPropertyValue()
        {
            var propertyOrFieldReflection = new ClassToAnalyze(1,1, 1).Reflection().GetPropertyOrField("PrivateProperty");
            propertyOrFieldReflection.SetValue("test");
            var value = propertyOrFieldReflection.GetValue<string>();
            value.Should().Be("test");
        }
        
        [Fact]
        public void GetFieldValue()
        {
            var propertyOrFieldReflection = new ClassToAnalyze(1,1, 1).Reflection().GetPropertyOrField("_privateField");
            var value = propertyOrFieldReflection.GetValue<string>();
            value.Should().Be("_privateField");
        }
        
        [Fact]
        public void SetFieldValue()
        {
            var propertyOrFieldReflection = new ClassToAnalyze(1,1, 1).Reflection().GetPropertyOrField("_privateField");
            propertyOrFieldReflection.SetValue("test");
            var value = propertyOrFieldReflection.GetValue<string>();
            value.Should().Be("test");
        }
        
        [Fact]
        public void GetEnclosingType()
        {
            var propertyOrFieldReflection = new ClassToAnalyze(1,1, 1).Reflection().GetPropertyOrField("_privateField");
            var result = propertyOrFieldReflection.GetEnclosingType<ClassToAnalyze>();
            result.Should().NotBeNull();
        }
    }
}