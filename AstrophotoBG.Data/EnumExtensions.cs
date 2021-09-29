using AstrophotoBG.Data.Enumerations;
using System.ComponentModel;

namespace AstrophotoBG.Data
{
    public static class EnumExtensions
    {
        public static string ToDescriptionString(this CategoryList value)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])value
               .GetType()
               .GetField(value.ToString())
               .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
}
