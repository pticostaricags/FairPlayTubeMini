using BlazorApp.Client.CustomProviders;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorApp.Client.Pages.User
{
    public partial class Profile
    {
        private AuthData? _userInfo;

        [Inject]
        private IUserProfileService? UserProfileService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            this._userInfo = await this.UserProfileService!.GetUserInfoAsync();
        }
    }
}
