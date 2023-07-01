using BlazorApp.Client.Services;
using BlazorApp.Shared.DataModels;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace BlazorApp.Client.Pages
{
    public partial class SuccessLogin
    {
        [CascadingParameter]
        private Task<AuthenticationState>? AuthenticationStateTask { get; set; }
        [Inject]
        private NavigationManager? NavigationManager { get; set; }
        [Inject]
        private IToastService? ToastService { get; set; }
        [Inject]
        private IDatabaseService? DatabaseService { get; set; }
        private bool IsBusy { get; set; }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                this.IsBusy = true;
                var state = await this.AuthenticationStateTask!;
                //verify if user exists in table
                var id = state.User!.Claims.Single(p => p.Type == BlazorApp.Shared.Constants.Claims.NameIdentifier)
                    .Value!;
                string baseAddress = $"{NavigationManager!.BaseUri}";
                HttpClient httpClient = new HttpClient()
                {
                    BaseAddress = new Uri(baseAddress)
                };
                var userEntity = await this.DatabaseService!.GetUsersByProviderUserIdAsync(id);
                if (userEntity is null)
                {
                    //If user does not exist, add it since it's first time login
                    userEntity = await DatabaseService!.AddUserAsync(new CreateApplicationUser()
                    {
                        Username = state.User!.Identity!.Name,
                        ProviderUserId = id
                    });
                }
                this.NavigationManager.NavigateTo("/");
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