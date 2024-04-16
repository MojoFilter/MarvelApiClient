using System.ComponentModel;

namespace MarvelApiClient;

[TypeConverter(typeof(PrivateKeyTypeConverter))]
public record PrivateKey(string Value);

internal sealed class PrivateKeyTypeConverter() :
    StringIdTypeConverter<PrivateKey>(s => new(s))
{ }