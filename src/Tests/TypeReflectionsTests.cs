using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentReflections;
using Xunit;

namespace Tests
{
    public class TypeReflectionsTests
    {
        [Fact]
        public void InvokeStaticGenericMethod()
        {
            var returnValue = typeof(ClassToAnalyze)
                .Reflection()
                .InvokeStaticGenericMethod("GenericStaticMethod", new Type[]{typeof(object)});
            returnValue.Should().BeNull();
        }
        
        [Fact]
        public void InvokeStaticMethod()
        {
            var returnValue = typeof(ClassToAnalyze).Reflection().InvokeStaticMethod("PrivateStaticMethod");
            returnValue.Should().BeNull();
        }
        
        [Fact]
        public void ImplementsType()
        {
            var implements = typeof(ClassToAnalyze).Reflection().Implements(typeof(object));
            implements.Should().Be(true);
        }
        
        [Fact]
        public void ImplementsTypeReturnsFalseForItSelf()
        {
            var implements = typeof(ClassToAnalyze).Reflection().Implements(typeof(ClassToAnalyze));
            implements.Should().Be(false);
        }
        
        [Fact]
        public void IsOrImplements()
        {
            var implements = typeof(ClassToAnalyze).Reflection().IsOrImplements(typeof(ClassToAnalyze));
            implements.Should().Be(true);
        }
        
        [Fact]
        public void ImplementsGeneric()
        {
            var implements = typeof(ClassToAnalyze).Reflection().Implements<object>();
            implements.Should().Be(true);
        }
        
        [Fact]
        public void IsOrImplementsGeneric()
        {
            var implements = typeof(ClassToAnalyze).Reflection().IsOrImplements<ClassToAnalyze>();
            implements.Should().Be(true);
        }
        
        [Fact]
        public void ImplementsWorksWithOpenGenericType()
        {
            var implements = typeof(ClassToAnalyze).Reflection().Implements(typeof(BaseClass<>));
            implements.Should().Be(true);
        }
        
        [Fact]
        public void ImplementsReturnsFalseIfTypeDoesNotImplementGivenType()
        {
            var implements = typeof(ClassToAnalyze).Reflection().Implements(typeof(int));
            implements.Should().Be(false);
        }
        
        [Fact]
        public void ImplementsReturnsFalseIfTypeDoesNotImplementGivenOpenGenericType()
        {
            var implements = typeof(ClassToAnalyze).Reflection().Implements(typeof(Dictionary<,>));
            implements.Should().Be(false);
        }
        
        [Fact]
        public void IsOrImplementsWorksWithOpenGenericType()
        {
            var implements = typeof(ClassToAnalyze).Reflection().IsOrImplements(typeof(BaseClass<>));
            implements.Should().Be(true);
        }
        
        [Fact]
        public void InstanceCanBeCreatedUsingPrivateConstructor()
        {
            var instance = typeof(ClassToAnalyze).Reflection().CreateInstance();
            instance.Should().NotBe(null);
        }
        
        [Fact]
        public void InstanceCanBeCreatedWithArguments()
        {
            var instance = typeof(ClassToAnalyze).Reflection().CreateInstance(1,2);
            instance.Should().NotBe(null);
        }
        
        [Fact]
        public void InstanceCreatedWithIncorrectArgumentsCausesError()
        {
            var classToAnalyzeReflection = typeof(ClassToAnalyze).Reflection();
            classToAnalyzeReflection.Invoking(x => x.CreateInstance(1))
                .Should().Throw<InvalidOperationException>();
        }
        
        [Fact]
        public void InstanceCreatedWithInvalidArgumentTypeCausesError()
        {
            var classToAnalyzeReflection = typeof(ClassToAnalyze).Reflection();
            classToAnalyzeReflection.Invoking(x => x.CreateInstance(new object()))
                .Should().Throw<InvalidOperationException>();
        }
        
        [Fact]
        public void CrateInstanceWithGeneric()
        {
            var instance = typeof(ClassToAnalyze).Reflection().CreateInstance<ClassToAnalyze>();
            instance.Should().NotBe(null);
        }
        
        [Fact]
        public void CrateInstanceGenericThrowsIfGenericTypeDoesNotImplementUsedType()
        {
            var classToAnalyseReflection = typeof(ClassToAnalyze).Reflection();
            classToAnalyseReflection.Invoking(x=>
                    x.CreateInstance<int>())
                .Should().Throw<InvalidOperationException>();
        }
        
        [Fact]
        public void CrateInstanceWithGenericAndArguments()
        {
            var instance = typeof(ClassToAnalyze).Reflection().CreateInstance<ClassToAnalyze>(1,1);
            instance.Should().NotBe(null);
        }
        
        [Fact]
        public void HasAttributeReturnsTrueIfClassImplementsGivenAttribute()
        {
            var hasAttribute = typeof(ClassToAnalyze).Reflection().HasAttribute(typeof(TestAttribute));
            hasAttribute.Should().BeTrue();
        }
        
        [Fact]
        public void HasAttributeGeneric()
        {
            var hasAttribute = typeof(ClassToAnalyze).Reflection().HasAttribute<TestAttribute>();
            hasAttribute.Should().BeTrue();
        }
        
        [Fact]
        public void HasAttributeCheckForBaseClassAttributes()
        {
            var hasAttribute = typeof(ClassToAnalyze).Reflection().HasAttribute<BaseTestAttribute>();
            hasAttribute.Should().BeTrue();
        }
        
        [Fact]
        public void HasAttributeReturnsFalseIfClassDoesNotImplementsGivenAttribute()
        {
            var hasAttribute = typeof(ClassToAnalyze).Reflection().HasAttribute(typeof(NotUsededAttribute));
            hasAttribute.Should().BeFalse();
        }
        
        [Fact]
        public void HasAttributeThrowsIfNonAttributeClassIsPassedAsAttribute()
        {
            var classToAnalyseReflection = typeof(ClassToAnalyze).Reflection();
            classToAnalyseReflection.Invoking(x=>
                x.HasAttribute(typeof(int)))
                .Should().Throw<InvalidOperationException>();
        }
        
        [Fact]
        public void GetAttributeReturnsAttributeIfClassImplementsGivenAttribute()
        {
            var attribute = typeof(ClassToAnalyze).Reflection().GetAttribute(typeof(TestAttribute));
            attribute.Should().NotBeNull();
        }
        
        [Fact]
        public void GetAttributeGeneric()
        {
            var attribute = typeof(ClassToAnalyze).Reflection().GetAttribute<TestAttribute>();
            attribute.Should().BeOfType<TestAttribute>();
        }
        
        [Fact]
        public void GetAttributeThrowsIfClassDoesNotImplementGivenAttribute()
        {
            var classToAnalyseReflection = typeof(ClassToAnalyze).Reflection();
            classToAnalyseReflection
                .Invoking(x => x.GetAttribute(typeof(NotUsededAttribute)))
                .Should()
                .Throw<InvalidOperationException>();
        }
        
        [Fact]
        public void GetAttributeThrowsIfNonAttributeClassIsPassed()
        {
            var classToAnalyseReflection = typeof(ClassToAnalyze).Reflection();
            classToAnalyseReflection
                .Invoking(x => x.GetAttribute(typeof(int)))
                .Should()
                .Throw<InvalidOperationException>();
        }
        
        [Fact]
        public void HasEmptyConstructorReturnsTrueIfEmptyConstructorIsPresent()
        {
            var hasEmptyConstructor = typeof(ClassToAnalyze).Reflection().HasEmptyConstructor();
            hasEmptyConstructor.Should().BeTrue();
        }
        
        [Fact]
        public void HasEmptyConstructorReturnsFalseIfEmptyConstructorIsNotPresent()
        {
            var hasEmptyConstructor = typeof(ClassWithoutEmptyConstructor).Reflection().HasEmptyConstructor();
            hasEmptyConstructor.Should().BeFalse();
        }
        
        [Fact]
        public void GetMethodsWithoutParentMethods()
        {
            var methods = typeof(ClassToAnalyze).Reflection().GetMethodsWithoutParentMethods();
            methods.Count().Should().Be(8);
        }
        
        
        [Fact]
        public void GetMethods()
        {
            var methods = typeof(ClassToAnalyze).Reflection().GetMethods();
            methods.Count().Should().Be(14);
        }
        
        [Theory]
        [InlineData(typeof(Boolean))]
        [InlineData(typeof(Byte))]
        [InlineData(typeof(SByte))]
        [InlineData(typeof(Int16))]
        [InlineData(typeof(UInt16))]
        [InlineData(typeof(Int32))]
        [InlineData(typeof(UInt32))]
        [InlineData(typeof(Int64))]
        [InlineData(typeof(UInt64))]
        [InlineData(typeof(IntPtr))]
        [InlineData(typeof(Char))]
        [InlineData(typeof(UIntPtr))]
        [InlineData(typeof(Double))]
        [InlineData(typeof(Single))]
        [InlineData(typeof(Object))]
        [InlineData(typeof(String))]
        [InlineData(typeof(Decimal))]
        [InlineData(typeof(DateTime))]
        [InlineData(typeof(DateTimeOffset))]
        [InlineData(typeof(TimeSpan))]
        [InlineData(typeof(Guid))]
        public void IsSimpleTypeSimpleValues(Type type)
        {
            type.Reflection().IsSimpleType().Should().BeTrue();
        }
        
        [Fact]
        public void IsSimpleTypeNonSimple()
        {
            typeof(ClassToAnalyze).Reflection().IsSimpleType().Should().BeFalse();
        }
        
        [Fact]
        public void GetMethodThatExists()
        {
            var method = typeof(ClassToAnalyze).Reflection().GetMethod("PrivateMethod");
            method.Should().NotBe(null);
        }
        
        [Fact]
        public void GetMethodThatDoesNotExists()
        {
            var classToAnalyzeReflection = typeof(ClassToAnalyze).Reflection();
            classToAnalyzeReflection.Invoking(x => x.GetMethod("MethodThatDoesNotExists"))
                .Should()
                .Throw<InvalidOperationException>();
        }
        
        [Fact]
        public void HasMethodReturnsTrueIfMethodExists()
        {
            var hasMethod = typeof(ClassToAnalyze).Reflection().HasMethod("PrivateMethod");
            hasMethod.Should().BeTrue();
        }
        
        [Fact]
        public void HasMethodReturnsFalseIfMethodDoesNotExists()
        {
            var hasMethod = typeof(ClassToAnalyze).Reflection().HasMethod("MethodThatDoesNotExists");
            hasMethod.Should().BeFalse();
        }
        
        [Fact]
        public void HasPropertyReturnsTrueIfPropertyExists()
        {
            var hasProperty = typeof(ClassToAnalyze).Reflection().HasPropertyOrField("PrivateProperty");
            hasProperty.Should().BeTrue();
        }
        
        [Fact]
        public void HasPropertyReturnsFalseIfPropertyDoesNotExists()
        {
            var hasPropertyOrField = typeof(ClassToAnalyze).Reflection().HasPropertyOrField("PropertyThatDoesNotExists");
            hasPropertyOrField.Should().BeFalse();
        }
        
        [Fact]
        public void GetPropertiesAndFields()
        {
            var properties = typeof(ClassToAnalyze).Reflection().GetPropertiesAndFields();
            properties.Should().HaveCount(12);
        }
        
        [Fact]
        public void GetPropertyThatExistsReturnsProperty()
        {
            var property = typeof(ClassToAnalyze).Reflection().GetPropertyOrField("PrivateProperty");
            property.Should().NotBeNull();
        }
        
        [Fact]
        public void GetFieldReturnsField()
        {
            var property = typeof(ClassToAnalyze).Reflection().GetPropertyOrField("_privateField");
            property.Should().NotBeNull();
        }
        
        [Fact]
        public void GetPropertyThatDoesNotExistsThrows()
        {
            var classToAnalyzeReflection = typeof(ClassToAnalyze).Reflection();
            classToAnalyzeReflection.Invoking(x => x.GetPropertyOrField("PropertyThatDoesNotExists"))
                .Should()
                .Throw<InvalidOperationException>();
        }
    }
}