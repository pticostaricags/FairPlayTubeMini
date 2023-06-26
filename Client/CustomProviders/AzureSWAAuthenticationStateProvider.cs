using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;

namespace BlazorApp.Client.CustomProviders
{
    /*
     * Check
     * https://github.com/anthonychu/blazor-auth-static-web-apps/blob/main/src/StaticWebAppsAuthenticationExtensions/StaticWebAppsAuthenticationStateProvider.cs
     * https://anthonychu.ca/post/blazor-auth-azure-static-web-apps/
     * */
    public class AzureSWAAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IUserProfileService _userProfileService;

        public AzureSWAAuthenticationStateProvider(IUserProfileService userProfileService)
        {
            this._userProfileService = userProfileService;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                AuthData? authData = await this._userProfileService.GetUserInfoAsync();
                var identity = new ClaimsIdentity(authData!.ClientPrincipal!.IdentityProvider);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, authData.ClientPrincipal.UserId!));
                identity.AddClaim(new Claim(ClaimTypes.Name, authData.ClientPrincipal.UserDetails!));
                identity.AddClaims(authData.ClientPrincipal.UserRoles!
                    .Select(role => new Claim(ClaimTypes.Role, role)));
                return new AuthenticationState(new ClaimsPrincipal(identity));
            }
            catch (Exception)
            {
                return new AuthenticationState(new ClaimsPrincipal());
            }
        }
    }

    public class AuthData
    {
        public Clientprincipal? ClientPrincipal { get; set; }
    }

    public class Clientprincipal
    {
        public string? UserId { get; set; }
        public string[]? UserRoles { get; set; }
        public object[]? Claims { get; set; }
        public string? IdentityProvider { get; set; }
        public string? UserDetails { get; set; }
    }

    public interface IUserProfileService
    {
        Task<AuthData?> GetUserInfoAsync();
    }

    public class AzureSWAUserProfileService: IUserProfileService
    {
        private readonly string _baseAddress;
        private readonly HttpClient _httpClient;

        public AzureSWAUserProfileService(string baseAddress)
        {
            this._baseAddress = baseAddress;
            this._httpClient = new HttpClient() { BaseAddress = new Uri(this._baseAddress) };
        }
        public async Task<AuthData?> GetUserInfoAsync()
        {
            string requestUrl = "/.auth/me";
            var authData = await this._httpClient.GetFromJsonAsync<AuthData>(requestUrl);
            return authData;
        }
    }
}
