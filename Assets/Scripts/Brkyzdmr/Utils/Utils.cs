using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Brkyzdmr.Utils
{
    public static class Utils
    {
        public static IEnumerable<MethodInfo> GetMethodsWithAttribute(Type classType, Type attributeType)
        {
            return classType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(methodInfo => methodInfo.GetCustomAttributes(attributeType, true).Length > 0);
        }

        public static T GetAttributeOfMethod<T>(MethodInfo methodInfo) where T : Attribute
        {
            return methodInfo.GetCustomAttributes(typeof(T), true).Cast<T>().FirstOrDefault();
        }
    }
}