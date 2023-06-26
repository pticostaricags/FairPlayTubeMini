using Microsoft.AspNetCore.Components;

namespace BlazorApp.Client.Pages
{
    public partial class Index
    {
        [Inject]
        private NavigationManager? NavigationManager { get; set; }
        protected override async Task OnInitializedAsync()
        {
            var baseUrl = this.NavigationManager!.BaseUri;
            HttpClient httpClient = new HttpClient() { BaseAddress = new Uri(baseUrl) };
            var response = await httpClient.GetAsync("/.auth/me");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var json = await response.Content.ReadAsStringAsync();
            }
            else
            {

            }
        }
    }
}
