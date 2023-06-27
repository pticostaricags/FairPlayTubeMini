using BlazorApp.Shared;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BlazorApp.Client.Pages
{
    public partial class VideosList
    {
        [Inject]
        private HttpClient? HttpClient { get; set; }
        [Inject]
        private IToastService? ToastService { get; set; }
        private bool IsBusy { get; set; } = false;
        private SearchVideosResponseModel? SearchResults { get; set; }
        private int _pageNumber = 1;
        private string _searchTerm = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                this.IsBusy = true;
                this.SearchResults =
                    await HttpClient!.GetFromJsonAsync<SearchVideosResponseModel>
                    ($"api/searchVideos?searchTerm={this._searchTerm}" +
                    $"&pageNumber={this._pageNumber}");
            }
            catch (Exception ex)
            {
                ToastService?.ShowError(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OnNextPageButtonClickedAsync()
        {
            this._pageNumber++;
            await LoadDataAsync();
        }
    }
}