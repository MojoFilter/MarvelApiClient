using CommunityToolkit.Mvvm.ComponentModel;
using MarvelApiClient;

namespace MarvelApiClientSample;

public partial class MainViewModel : ObservableObject
{
    public MainViewModel(IMarvelApiClient client)
    {
        _client = client;
        this.ReleaseDateOptions = Enum.GetValues<ReleaseDateDescriptor>();
        this.SelectedReleaseDate = ReleaseDateDescriptor.thisWeek;        
    }

    public IEnumerable<ReleaseDateDescriptor> ReleaseDateOptions { get; }

    [ObservableProperty]
    private ReleaseDateDescriptor _selectedReleaseDate;

    [ObservableProperty]
    private IEnumerable<Comic> _results = Enumerable.Empty<Comic>();

    [ObservableProperty]
    private bool _isLoading;

    async partial void OnSelectedReleaseDateChanged(ReleaseDateDescriptor value)
    {
        await this.LoadResultsAsync();
    }

    private async Task LoadResultsAsync()
    {
        this.IsLoading = true;
        try
        {
            var results = await _client.GetReleasesAsync(this.SelectedReleaseDate);
            this.Results = results;
        }
        finally
        {
            this.IsLoading = false;
        }
    }

    private readonly IMarvelApiClient _client;
}
