using BlazorApp.Shared;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BlazorApp.Client.Pages.Creator
{
    public partial class EditVideoInsights
    {
        [Parameter]
        public string? VideoId { get; set; }
        [Inject]
        private HttpClient? HttpClient { get; set; }
        [Inject]
        private IToastService? ToastService { get; set; }
        private bool IsLoading { get; set; }

        private SearchVideosResponseModel? SearchVideosResponse { get; set; }
        private GenerateVideoEditAccessTokenResponseModel? GenerateVideoEditAccessTokenResponse { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                this.IsLoading = true;
                this.GenerateVideoEditAccessTokenResponse = await this.HttpClient!.GetFromJsonAsync<GenerateVideoEditAccessTokenResponseModel>($"api/GenerateVideoEditAccessToken");
                var searchResponse = await this.HttpClient!
                    .GetFromJsonAsync<SearchVideosResponseModel>($"api/SearchVideos?pageNumber=1" +
                    $"&videoId={VideoId}");
                if (searchResponse!.results.Length > 0)
                {
                    searchResponse!.results[0].editAccessToken = this.GenerateVideoEditAccessTokenResponse!.AccessToken;
                    this.SearchVideosResponse = searchResponse;
                }
            }
            catch (Exception ex)
            {
                this.ToastService?.ShowError(ex.Message);
            }
            finally
            {
                this.IsLoading = false;
            }
        }
    }
}