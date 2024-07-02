using System;
using System.Reflection;

namespace Library.Misc
{
    public class Reflection
    {
        public static bool IsAnonymousType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return type.Name.Contains("__AnonymousType");
        }

        public static T GetCustomAttribute<T>(PropertyInfo propertyInfo) where T : System.Attribute
        {
            if (propertyInfo == null)
            {
                return null;
            }

            T customAttribute = null;

            object[] custAttribs = propertyInfo.GetCustomAttributes(typeof(T), true);
            if (custAttribs != null && custAttribs.Length > 0)
            {
                customAttribute = (T)custAttribs[0];
            }

            return customAttribute;
        }
    }
}
