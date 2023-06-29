namespace BlazorApp.Client.Services
{
    public interface INavigationService
    {
        void NavigateToHome();
        void NavigateToListUsers();
        void NavigateToUploadVideo();
        void NavigateToVideosList();
    }
}