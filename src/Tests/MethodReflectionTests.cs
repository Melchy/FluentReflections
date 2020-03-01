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
    }
}