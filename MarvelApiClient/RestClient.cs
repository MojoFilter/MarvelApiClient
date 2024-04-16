using RestSharp;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace MarvelApiClient;

internal sealed class MarvelRestClient(Uri apiUri, ApiKey apiKey, PrivateKey privateKey) : IMarvelApiClient
{
    public async Task<IEnumerable<Comic>> GetReleasesAsync(ReleaseDateDescriptor timeFrame, CancellationToken cancellationToken = default)
    {
        var resource = $"v1/public/comics?format=comic&noVariants=true&dateDescriptor={timeFrame}";
        var wrapper = await GetAsync<ComicWrapper>(resource, cancellationToken).ConfigureAwait(false);
        return wrapper.Data.Results.Select(Map).ToList();
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

    private Comic Map(ComicInfo info)
    {
        var title = this.StripTitleDates(info.Title);
        return new Comic(
            info.Id,
            title,
            info.IssueNumber,
            info.Description,
            new($"{info.Thumbnail.Path}.{info.Thumbnail.Extension}"));
    }

    private string StripTitleDates(string title) {
        var match = Regex.Match(title, @"(?<title>.+)(?<date> (\([0-9]{4}.*))(?<number> #.+)");
        return match.Success ? match.Groups["title"].Value : title;
    }

    private readonly ApiKey _apiKey = apiKey;
    private readonly PrivateKey _privateKey = privateKey;
    private readonly Uri _uri = apiUri;

    record ComicInfo(int Id, string Title, double IssueNumber, string Description, ImageLocation Thumbnail);
    record ComicContainer(ComicInfo[] Results);
    record ComicWrapper(ComicContainer Data);
}
