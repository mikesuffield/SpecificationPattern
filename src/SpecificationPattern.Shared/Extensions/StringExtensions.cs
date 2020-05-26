using System;

namespace SpecificationPattern.Shared.Extensions
{
    public static class StringExtensions
    {
        public static T ToEnum<T>(this string input) where T : Enum
        {
            try
            {
                return (T)Enum.Parse(typeof(T), input);
            }
            catch
            {
                throw new Exception($"Unable to convert string '{input}' to {typeof(T).Name} Enum");
            }
        }
    }
}
