namespace MarvelApiClient;

public interface IMarvelApiClient
{
    Task<IEnumerable<Comic>> GetReleasesAsync(ReleaseDateDescriptor timeFrame, CancellationToken cancellationToken = default);
}
