using BlazorApp.Shared.Constants;
using BlazorApp.Shared.DataModels;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BlazorApp.Client.Pages.Admin
{
    [Route(BlazorApp.Shared.Constants.Routes.ListUsers)]
    [Authorize(Roles = BlazorApp.Shared.Constants.Roles.admin)]
    public partial class ListUsers
    {
        [Inject]
        private IToastService? ToastService { get; set; }
        [Inject]
        private NavigationManager? NavigationManager { get; set; }
        private bool IsBusy { get; set; }
        private ApplicationUserList? Users { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                this.IsBusy = true;
                string baseAddress=$"{NavigationManager!.BaseUri}";
                HttpClient httpClient = new HttpClient()
                {
                    BaseAddress = new Uri(baseAddress)
                };
                this.Users = await httpClient.GetFromJsonAsync<ApplicationUserList>("data-api/rest/UsersList");
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