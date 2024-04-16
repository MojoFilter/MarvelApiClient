using System.ComponentModel;
using System.Globalization;

namespace MarvelApiClient;

internal class StringIdTypeConverter<T>(Func<string, T> factory) : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) =>
        sourceType == typeof(string);

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value) =>
        factory((string)value);

}
