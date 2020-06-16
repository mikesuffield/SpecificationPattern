using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SpecificationPattern.Shared.Extensions
{
    public static class EnumExtensions
    {
        public static string DisplayName(this Enum inputEnum)
        {
            var enumType = inputEnum.GetType();
            var memberInfo = enumType.GetMember(inputEnum.ToString());

            if (memberInfo != null && memberInfo.Length > 0)
            {
                var displayAttributes = memberInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);
                if ((displayAttributes != null && displayAttributes.Count() > 0))
                {
                    return ((DisplayAttribute)displayAttributes.ElementAt(0)).Name;
                }
            }

            return inputEnum.ToString();
        }
    }
}
