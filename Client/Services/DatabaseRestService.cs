using BlazorApp.Shared.DataModels;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;

namespace BlazorApp.Client.Services
{
    public class DatabaseRestService : IDatabaseService
    {
        private readonly NavigationManager _navigationManager;
        private readonly HttpClient _httpClient;
        public DatabaseRestService(NavigationManager navigationManager)
        {
            this._navigationManager = navigationManager;
            string baseAddress = $"{this._navigationManager!.BaseUri}";
            this._httpClient = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress)
            };
        }

        public async Task<ApplicationUser?> GetUsersByProviderUserIdAsync(string providerUserId)
        {
            var userEntity = await this._httpClient!
                    .GetFromJsonAsync<ApplicationUserList>($"data-api/rest/UsersList?$filter={nameof(ApplicationUser.ProviderUserId)} eq '{providerUserId}'");
            return userEntity?.value?.FirstOrDefault();
        }

        public async Task<ApplicationUser?> AddUserAsync(CreateApplicationUser createApplicationUser)
        {
            var response = await this._httpClient!.PostAsJsonAsync<CreateApplicationUser>("data-api/rest/AddUser",
                        createApplicationUser,
                        options: new System.Text.Json.JsonSerializerOptions()
                        {
                            PropertyNamingPolicy = null
                        });
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ApplicationUserList>();
            return result!.value.FirstOrDefault();
        }

        public async Task<long> GetApplicationUserIdAsync(string? userName)
        {
            var userEntity = await this._httpClient!
                .GetFromJsonAsync<ApplicationUserList>($"data-api/rest/UsersList?$filter={nameof(ApplicationUser.Username)} eq '{userName}'");
            if (userEntity != null && userEntity.value.Length == 1)
                return userEntity!.value[0].ApplicationUserId;
            else
                throw new Exception(userName + " not found.");
        }
    }
}
