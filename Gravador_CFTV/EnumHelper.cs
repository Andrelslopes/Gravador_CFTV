using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Gravador_CFTV
{
    public static class EnumHelper
    {
        public static List<object> ToList<T>()
        {
            return Enum.GetValues(typeof(T))
                .Cast<Enum>()
                .Select(v => new
                {
                    Value = v,
                    Text = GetDescription(v)
                })
                .Cast<object>()
                .ToList();
        }

        public static string GetDescription(Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            var attributes = (DescriptionAttribute[])
                fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }
}
