using System.ComponentModel;

namespace MarvelApiClient;

[TypeConverter(typeof(ApiKeyTypeConverter))]
public sealed record ApiKey(string Value);

internal sealed class ApiKeyTypeConverter() :
    StringIdTypeConverter<ApiKey>(s => new(s))
{ }