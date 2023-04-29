using System.ComponentModel;

namespace Weather.Web.Extensions
{
    public static class Extensions
    {
        public static Nullable<T> ToNullable<T>(this string s) where T : struct
        {
            Nullable<T> result = new Nullable<T>();
            try
            {
                if (!string.IsNullOrEmpty(s) && s.Trim().Length > 0)
                {
                    TypeConverter conv = TypeDescriptor.GetConverter(typeof(T));
                    result = (T)conv.ConvertFrom(s);
                }
            }
            catch { }
            return result;            
        }

        public static T? ToNullableEnu<T>(this string s) where T : struct
        {
            if (!string.IsNullOrEmpty(s))
            {
                var converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(T));
                if (converter.IsValid(s)) return (T)converter.ConvertFromString(s);
                if (typeof(T).IsEnum) { T t; if (Enum.TryParse<T>(s, out t)) return t; }
            }

            return null;
        }
    }
}
