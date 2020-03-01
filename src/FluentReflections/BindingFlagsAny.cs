using System.Reflection;

namespace FluentReflections
{
    public static class BindingFlagsAny
    {
        public static BindingFlags Get()
        {
            return BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
        }
    }
}