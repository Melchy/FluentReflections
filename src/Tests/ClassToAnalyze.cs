﻿using System;

namespace Tests
{
    public class NotUsededAttribute : Attribute
    {
        
    }
    public class BaseTestAttribute : Attribute
    {
        
    }
    
    public class TestAttribute : Attribute
    {
        
    }

    public class ClassWithoutEmptyConstructor
    {
        public ClassWithoutEmptyConstructor(string foo)
        {
            
        }
    }
    

    [BaseTest]
    public class BaseClass<TType>
    {
        
    }
    
    [Test]
    public class ClassToAnalyze : BaseClass<string>
    {
        private ClassToAnalyze()
        {
            
        }

        protected ClassToAnalyze(int obj, int obj2)
        {
            
        }
        
        public ClassToAnalyze(int obj, int obj2, int obj3)
        {
            
        }
        
        [Test]
        private string PrivateProperty { get; set; } = "PrivateProperty";
        public string PublicProperty { get; set; } = "PublicProperty";
        protected string ProtectedProperty { get; set; } = "ProtectedProperty";

        private string _privateField = "_privateField";
        protected string ProtectedField = "ProtectedField";
        public string PublicField = "PublicField";

        private static string PrivateStaticProperty { get; set; } = "PrivateStaticProperty";
        public static string PublicStaticProperty { get; set; } = "PublicStaticProperty";
        protected static string ProtectedStaticProperty { get; set; } = "ProtectedStaticProperty";

        private static string _privateStaticField = "_privateStaticField";
        protected static string ProtectedStaticField = "ProtectedStaticField";
        public static string PublicStaticField = "PublicStaticField";

        public void PublicMethod()
        {
            
        }
        
        private void PrivateMethod([Test] int param)
        {
            
        }
        
        protected void ProtectedMethod()
        {
            
        }
        
        public static void PublicStaticMethod()
        {
            
        }
        
        private void GenericMethod<TType>()
        {
            
        }
        
        private static void PrivateStaticMethod()
        {
            
        }
        
        private static void GenericStaticMethod<TType>()
        {
            
        }
        
        protected static void ProtectedStaticMethod()
        {
            
        }
        
        public int this[int i]
        {
            get => 1;
            set {}
        }
        
    }
}