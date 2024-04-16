namespace MarvelApiClient;

public record Comic(int Id, string Title, double IssueNumber, string Description, Uri ThumbnailUri)
{
    //public Uri ThumbnailUri => new($"{Thumbnail.Path}.{Thumbnail.Extension}");
}
