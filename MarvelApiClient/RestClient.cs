using RestSharp;
using System.Security.Cryptography;
using System.Text;

namespace MarvelApiClient;

public enum ReleaseDateDescriptor
{
    lastWeek,
    thisWeek,
    NextWeek,
    ThisMonth
}

public record ImageLocation(string Path, string Extension);

public record Comic(int Id, string Title, double IssueNumber, string Description, ImageLocation Thumbnail)
{
    public Uri ThumbnailUri => new($"{Thumbnail.Path}.{Thumbnail.Extension}");
}

public record ApiKey(string Value);
public record PrivateKey(string Value);

public interface IMarvelApiClient
{
    Task<IEnumerable<Comic>> GetReleasesAsync(ReleaseDateDescriptor timeFrame, CancellationToken cancellationToken = default);
}


internal sealed class MarvelRestClient(Uri apiUri, ApiKey apiKey, PrivateKey privateKey) : IMarvelApiClient
{
    public async Task<IEnumerable<Comic>> GetReleasesAsync(ReleaseDateDescriptor timeFrame, CancellationToken cancellationToken = default)
    {
        var resource = $"v1/public/comics?dateDescriptor={timeFrame}";
        var wrapper = await GetAsync<ComicWrapper>(resource, cancellationToken).ConfigureAwait(false);
        return wrapper.Data.Results;
    }

    private async Task<T> GetAsync<T>(string resource, CancellationToken cancellationToken)
    {
        var timeStamp = DateTime.UtcNow.Ticks.ToString();
        var hash = GetHash(timeStamp);
        var path = resource + $"&ts={timeStamp}&apikey={_apiKey.Value}&hash={hash}";
        var options = new RestClientOptions(_uri);
        var client = new RestClient(options);
        var request = new RestRequest(path);
        return await client.GetAsync<T>(request, cancellationToken) ?? throw new Exception();
    }

    private string GetHash(string timeStamp)
    {
        using (var md5 = MD5.Create())
        {
            var input = timeStamp + _privateKey.Value + _apiKey.Value;
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = md5.ComputeHash(inputBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
        

    private readonly ApiKey _apiKey = apiKey;
    private readonly PrivateKey _privateKey = privateKey;
    private readonly Uri _uri = apiUri;

    record ComicContainer(Comic[] Results);
    record ComicWrapper(ComicContainer Data);
}
