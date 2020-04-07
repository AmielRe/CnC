using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Attributes
{
    public static class ReflectionExtensions
    {
        public static T GetAttribute<T>(this Type classType) where T : class
        {
            return ReflectionExtensions.GetAttribute<T>(classType, true);
        }
        public static T GetAttribute<T>(this Type classType, bool includeInheritedAttributes) where T : class
        {
            if (classType == null)
                return null;

            object attr = classType.GetCustomAttributes(includeInheritedAttributes).Where(a => a.GetType() == typeof(T)).FirstOrDefault();
            return attr as T;
        }
    }
}
