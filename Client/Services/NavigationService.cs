using Microsoft.AspNetCore.Components;
using Routes = BlazorApp.Shared.Constants.Routes;

namespace BlazorApp.Client.Services
{
    public class NavigationService : INavigationService
    {
        private NavigationManager _navigationManager;

        public NavigationService(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }
        private void NavigateTo(string url, bool forceLoad)
        {
            this._navigationManager.NavigateTo(url, forceLoad);
        }
        public void NavigateToHome()
        {
            this.NavigateTo("/", false);
        }

        public void NavigateToVideosList()
        {
            this.NavigateTo(Routes.VideosList, false);
        }

        public void NavigateToUploadVideo()
        {
            this.NavigateTo(Routes.UploadVideo, false);
        }

        public void NavigateToListUsers()
        {
            this.NavigateTo(Routes.ListUsers, false);
        }

    }
}
