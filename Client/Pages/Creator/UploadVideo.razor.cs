using Azure.Storage.Blobs;
using BlazorApp.Shared;
using BlazorApp.Shared.Helpers;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using System.IO;
using System.Net.Http.Json;

namespace BlazorApp.Client.Pages.Creator
{
    [Route(BlazorApp.Shared.Constants.Routes.UploadVideo)]
    [Authorize(Roles = BlazorApp.Shared.Constants.Roles.creator)]
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
        public int VideoNameMaxLength { get; set; } = 50;
        public int VideoNameRemainingCharacterCount => VideoNameMaxLength - this.indexVideoModel?.VideoFileName?.Length ?? 0;
        private bool IsSubmitting { get; set; } = false;
        private bool ShowSubmitButton { get; set; } = true;
        private int UploadProgress { get; set; } = 0;
        private async void OnFileSelectedAsync(InputFileChangeEventArgs inputFileChangeEventArgs)
        {
            try
            {
                IsBusy = true;
                StateHasChanged();
                indexVideoModel.VideoSourceUrl = null;
                selectedFileBytes = null;
                var maxbytesAllowed = 300 * 1024 * 1024;
                var stream = inputFileChangeEventArgs.File.OpenReadStream(maxbytesAllowed);
                MemoryStream memoryStream = new();
                await stream.CopyToAsync(memoryStream);
                selectedFileBytes = memoryStream.ToArray();
                var state = await AuthenticationStateTask!;
                var userName = state.User.Identity!.Name;
                var sasUrlModel = await HttpClient!
                    .GetFromJsonAsync<GenerateVideoSasUrlResponseModel>
                    ($"/api/GenerateVideoSasUrl?" +
                    $"videoFileName={indexVideoModel.VideoFileName}" +
                    $"&userId={userName}");
                indexVideoModel.VideoSourceUrl = sasUrlModel!.VideoSasUrl;
                this.ShowSubmitButton = true;
            }
            catch (Exception ex)
            {
                ToastService!.ShowError(ex.Message);
            }
            finally
            {
                IsBusy = false;
                StateHasChanged();
            }
        }
        private async Task OnValidSubmitAsync()
        {
            try
            {
                IsBusy = true;
                this.IsSubmitting = true;
                StateHasChanged();
                var state = await AuthenticationStateTask!;
                var userName = state.User.Identity!.Name;
                var fileRelativePath = UserBlobsHelper.GetBlobRelativePath(userName, indexVideoModel.VideoFileName);
                BlobClient blobClient = new BlobClient(blobUri: new Uri(indexVideoModel.VideoSourceUrl));
                var uploadFileResponse = await blobClient.UploadAsync(new BinaryData(selectedFileBytes!),
                    options: new Azure.Storage.Blobs.Models.BlobUploadOptions()
                    {
                        ProgressHandler = new Progress<long>(async (long bytesTransferred) =>
                        {
                            await InvokeAsync(() =>
                            {
                                UploadProgress = selectedFileBytes!.Length == 0 ? 0 : (int)((bytesTransferred / selectedFileBytes!.Length) * 100);
                            });
                        })
                    });
                var response = await HttpClient!
                    .PostAsJsonAsync("/api/IndexVideo", indexVideoModel);
                if (response.IsSuccessStatusCode)
                {
                    indexVideoResponseModel =
                        await response.Content.ReadFromJsonAsync<IndexVideoResponseModel>();
                }
            }
            catch (Exception ex)
            {
                ToastService!.ShowError(ex.Message);
            }
            finally
            {
                IsBusy = false;
                StateHasChanged();
            }
        }
    }
}