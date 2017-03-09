using System;
using System.Reflection;

namespace TechnoRex.Utils.Reflection
{
    public static class ObjectReflection
    {
        public static object FollowPropertyPath(object value, string path)
        {
            Type currentType = value.GetType();

            foreach (string propertyName in path.Split('.'))
            {

                try
                {
                    PropertyInfo property = currentType.GetProperty(propertyName);
                    value = property.GetValue(value, null);
                    currentType = property.PropertyType;
                }
                catch (Exception)
                {
                    value = "^^";
                }
            }
            return value;
        }
    }
}