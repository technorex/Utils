using System;
using System.Linq;
using System.Runtime.Serialization;

namespace TechnoRex.Utils.Reflection
{
    public static class EnumReflection
    {
        public static string GetDescriptionFromEnumValue<T>(Enum value)
        {
            EnumMemberAttribute attribute = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(T), false)
                .SingleOrDefault() as EnumMemberAttribute;
            return attribute == null ? value.ToString() : attribute.Value;
        }
    }
}