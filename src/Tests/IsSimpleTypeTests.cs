using System;
using FluentAssertions;
using FluentReflections;
using Xunit;

namespace Tests
{
    public class IsSimpleTypeTests
    {
        struct TestStruct
        {
            public string Prop1;
            public int Prop2;
        }

        class TestClass1
        {
            public string Prop1;
            public int Prop2;
        }

        enum TestEnum { TheValue }
        
        
        [Fact]
        public void IsSimpleType()
        {
            typeof(TestEnum).Reflection().IsSimpleType().Should().BeTrue();
            typeof(string).Reflection().IsSimpleType().Should().BeTrue();
            typeof(char).Reflection().IsSimpleType().Should().BeTrue();
            typeof(Guid).Reflection().IsSimpleType().Should().BeTrue();
            
            typeof(bool).Reflection().IsSimpleType().Should().BeTrue();
            typeof(byte).Reflection().IsSimpleType().Should().BeTrue();
            typeof(short).Reflection().IsSimpleType().Should().BeTrue();
            typeof(int).Reflection().IsSimpleType().Should().BeTrue();
            typeof(long).Reflection().IsSimpleType().Should().BeTrue();
            typeof(float).Reflection().IsSimpleType().Should().BeTrue();
            typeof(double).Reflection().IsSimpleType().Should().BeTrue();
            typeof(decimal).Reflection().IsSimpleType().Should().BeTrue();

            typeof(sbyte).Reflection().IsSimpleType().Should().BeTrue();
            typeof(ushort).Reflection().IsSimpleType().Should().BeTrue();
            typeof(uint).Reflection().IsSimpleType().Should().BeTrue();
            typeof(ulong).Reflection().IsSimpleType().Should().BeTrue();
            
            typeof(DateTime).Reflection().IsSimpleType().Should().BeTrue();
            typeof(DateTimeOffset).Reflection().IsSimpleType().Should().BeTrue();
            typeof(TimeSpan).Reflection().IsSimpleType().Should().BeTrue();

            
            typeof(TestStruct).Reflection().IsSimpleType().Should().BeFalse();
            typeof(TestClass1).Reflection().IsSimpleType().Should().BeFalse();

            typeof(TestEnum?).Reflection().IsSimpleType().Should().BeTrue();
            typeof(char?).Reflection().IsSimpleType().Should().BeTrue();
            typeof(Guid?).Reflection().IsSimpleType().Should().BeTrue();

            typeof(bool?).Reflection().IsSimpleType().Should().BeTrue();
            typeof(byte?).Reflection().IsSimpleType().Should().BeTrue();
            typeof(short?).Reflection().IsSimpleType().Should().BeTrue();
            typeof(int?).Reflection().IsSimpleType().Should().BeTrue();
            typeof(long?).Reflection().IsSimpleType().Should().BeTrue();
            typeof(float?).Reflection().IsSimpleType().Should().BeTrue();
            typeof(double?).Reflection().IsSimpleType().Should().BeTrue();
            typeof(decimal?).Reflection().IsSimpleType().Should().BeTrue();

            typeof(sbyte?).Reflection().IsSimpleType().Should().BeTrue();
            typeof(ushort?).Reflection().IsSimpleType().Should().BeTrue();
            typeof(uint?).Reflection().IsSimpleType().Should().BeTrue();
            typeof(ulong?).Reflection().IsSimpleType().Should().BeTrue();
            
            typeof(DateTime?).Reflection().IsSimpleType().Should().BeTrue();
            typeof(DateTimeOffset?).Reflection().IsSimpleType().Should().BeTrue();
            typeof(TimeSpan?).Reflection().IsSimpleType().Should().BeTrue();

            typeof(TestStruct?).Reflection().IsSimpleType().Should().BeFalse();
        }
    }
}