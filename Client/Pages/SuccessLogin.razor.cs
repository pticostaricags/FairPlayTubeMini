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
        protected override async Task OnInitializedAsync()
        {
            try
            {
                var state = await this.AuthenticationStateTask!;
                //verify if user exists in table
                var id = state.User!.Claims.Single(p => p.Type == BlazorApp.Shared.Constants.Claims.NameIdentifier)
                    .Value!;
                string baseAddress = $"{NavigationManager!.BaseUri}";
                HttpClient httpClient = new HttpClient()
                {
                    BaseAddress = new Uri(baseAddress)
                };
                var userEntity = await httpClient!
                    .GetFromJsonAsync<ApplicationUserList>($"data-api/rest/UsersList?$filter={nameof(ApplicationUser.ProviderUserId)} eq '{id}'");
                if (userEntity!.value.Length == 0)
                {
                    //If user does not exist, add it since it's first time login
                    var response = await httpClient!.PostAsJsonAsync<CreateApplicationUser>("data-api/rest/AddUser",
                        new CreateApplicationUser()
                        {
                            Username = state.User!.Identity!.Name,
                            ProviderUserId = id
                        },
                        options:new System.Text.Json.JsonSerializerOptions()
                        {
                            PropertyNamingPolicy = null
                        });
                    response.EnsureSuccessStatusCode();
                    userEntity = await response.Content.ReadFromJsonAsync<ApplicationUserList>();
                }
                this.NavigationManager.NavigateTo("/");
            }
            catch (Exception ex)
            {
                this.ToastService!.ShowError(ex.Message);
            }
            finally
            {

            }
        }
    }
}