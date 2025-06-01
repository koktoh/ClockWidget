using System.Linq;
using System.Reflection;
using System.Windows.Media;

namespace ClockWidget.Common
{
    public static class DeepCopyHelper
    {
        public static T DeepCopy<T>(T src)
        where T : new()
        {
            if (src is null) return default;

            var copy = new T();

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(x => x.CanRead && x.CanWrite);

            foreach (var property in properties)
            {
                var value = property.GetValue(src);

                if (value is null)
                {
                    property.SetValue(copy, null);
                }
                else if (property.PropertyType.IsValueType || property.PropertyType == typeof(string))
                {
                    property.SetValue(copy, value);
                }
                else if (property.PropertyType == typeof(FontFamily))
                {
                    var srcFont = (FontFamily)value;
                    var copiedFont = new FontFamily(srcFont.Source);
                    property.SetValue(copy, copiedFont);
                }
                else
                {
                    var method = typeof(DeepCopyHelper)
                    .GetMethod(nameof(DeepCopy))
                    .MakeGenericMethod(property.PropertyType);

                    var copiedValue = method.Invoke(null, new[] { value });
                    property.SetValue(copy, copiedValue);
                }
            }

            return copy;
        }
    }
}
