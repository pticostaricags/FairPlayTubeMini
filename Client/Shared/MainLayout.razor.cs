using Microsoft.AspNetCore.Components;

namespace BlazorApp.Client.Shared
{
    public partial class MainLayout
    {
        [Inject]
        private NavigationManager? NavigationManager { get; set; }
        private string? SuccessLoginUrl => $"{this.NavigationManager!.Uri}/SuccessLogin";
    }
}
