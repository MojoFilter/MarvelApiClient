namespace MarvelApiClient;

public interface IMarvelApiClientBuilder
{
    IMarvelApiClient Build(ApiKey apiKey, PrivateKey privateKey);
}

public class MarvelApiClientBuilder : IMarvelApiClientBuilder
{
    public IMarvelApiClient Build(ApiKey apiKey, PrivateKey privateKey) => new MarvelRestClient(_apiUri, apiKey, privateKey);

    private Uri _apiUri = new("https://gateway.marvel.com");

}
