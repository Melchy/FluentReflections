using System;
using FluentAssertions;
using FluentReflections;
using Xunit;

namespace Tests
{
    public class MethodInstanceReflectionTests
    {
        [Fact]
        public void InvokeMethod()
        {
            var methodReflection = new ClassToAnalyze(1,1, 1).Reflection().GetMethod("PrivateMethod");
            var returnValue = methodReflection.Invoke(1);
            returnValue.Should().BeNull();
        }

        [Fact]
        public void InvokeOnGenericMethodThrowsException()
        {
            var methodReflection = new ClassToAnalyze(1,1, 1).Reflection().GetMethod("GenericMethod");
            methodReflection
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<InvalidOperationException>();
        }
        
        [Fact]
        public void InvokeGeneric()
        {
            var methodReflection = new ClassToAnalyze(1,1, 1).Reflection().GetMethod("GenericMethod");
            var returnValue = methodReflection.InvokeGeneric(new Type[]{typeof(object)});
            returnValue.Should().BeNull();
        }
        
        [Fact]
        public void InvokeGenericOnNonGenericMethod()
        {
            var methodReflection = new ClassToAnalyze(1,1, 1).Reflection().GetMethod("PrivateMethod");
            methodReflection
                .Invoking(x => x.InvokeGeneric(new Type[]{typeof(object)}))
                .Should()
                .Throw<InvalidOperationException>();
        }
    }
}