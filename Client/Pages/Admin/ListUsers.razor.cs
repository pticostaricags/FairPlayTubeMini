using BlazorApp.Shared.DataModels;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BlazorApp.Client.Pages.Admin
{
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
                this.Users = await httpClient.GetFromJsonAsync<ApplicationUserList>("data-api/rest/ApplicationUser");
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