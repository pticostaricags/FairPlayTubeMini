using Azure.Storage.Blobs;
using BlazorApp.Shared;
using BlazorApp.Shared.Helpers;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using System.IO;
using System.Net.Http.Json;

namespace BlazorApp.Client.Pages.User
{
    public partial class UploadVideo
    {
        [Inject]
        private HttpClient? HttpClient { get; set; }
        [Inject]
        private IToastService? ToastService { get; set; }
        [CascadingParameter]
        private Task<AuthenticationState>? AuthenticationStateTask { get; set; }
        private bool IsBusy { get; set; }
        private IndexVideoModel indexVideoModel = new();
        private byte[]? selectedFileBytes;

        private IndexVideoResponseModel? indexVideoResponseModel { get; set; }

        private async void OnFileSelectedAsync(InputFileChangeEventArgs inputFileChangeEventArgs)
        {
            try
            {
                this.IsBusy = true;
                StateHasChanged();
                this.indexVideoModel.VideoSourceUrl = null;
                this.selectedFileBytes = null;
                var maxbytesAllowed = 300 * 1024 * 1024;
                var stream = inputFileChangeEventArgs.File.OpenReadStream(maxbytesAllowed);
                MemoryStream memoryStream = new();
                await stream.CopyToAsync(memoryStream);
                this.selectedFileBytes = memoryStream.ToArray();
                var state = await this.AuthenticationStateTask!;
                var userName = state.User.Identity!.Name;
                var sasUrlModel = await this.HttpClient!
                    .GetFromJsonAsync<GenerateVideoSasUrlResponseModel>
                    ($"/api/GenerateVideoSasUrl?" +
                    $"videoFileName={this.indexVideoModel.VideoFileName}" +
                    $"&userId={userName}");
                this.indexVideoModel.VideoSourceUrl = sasUrlModel!.VideoSasUrl;
            }
            catch (Exception ex)
            {
                this.ToastService!.ShowError(ex.Message);
            }
            finally
            {
                this.IsBusy = false;
                StateHasChanged();
            }
        }
        private async Task OnValidSubmitAsync()
        {
            try
            {
                this.IsBusy = true;
                StateHasChanged();
                var state = await this.AuthenticationStateTask!;
                var userName = state.User.Identity!.Name;
                var fileRelativePath = UserBlobsHelper.GetBlobRelativePath(userName, this.indexVideoModel.VideoFileName);
                BlobClient blobClient = new BlobClient(blobUri: new Uri(this.indexVideoModel.VideoSourceUrl));
                var uploadFileResponse = await blobClient.UploadAsync(new BinaryData(this.selectedFileBytes!));
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
                StateHasChanged();
            }
        }
    }
}