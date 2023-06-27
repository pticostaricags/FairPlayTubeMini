using BlazorApp.Shared;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BlazorApp.Client.Pages.User
{
    public partial class UploadVideo
    {
        [Inject]
        private HttpClient? HttpClient { get; set; }
        [Inject]
        private IToastService? ToastService { get; set; }
        private bool IsBusy { get; set; }
        private IndexVideoModel indexVideoModel = new();
        private IndexVideoResponseModel? indexVideoResponseModel { get; set; }

        private async Task OnValidSubmitAsync()
        {
            try
            {
                this.IsBusy = true;
                var response = await this.HttpClient!
                    .PostAsJsonAsync<IndexVideoModel>("/api/IndexVideo", this.indexVideoModel);
                if (response.IsSuccessStatusCode)
                {
                    this.indexVideoResponseModel =
                        await response.Content.ReadFromJsonAsync<IndexVideoResponseModel>();
                }
            }
            catch (Exception ex)
            {
                this.ToastService!.ShowError(ex.Message);
            }
            finally
            {
                this.IsBusy = false;
            }
        }
    }
}