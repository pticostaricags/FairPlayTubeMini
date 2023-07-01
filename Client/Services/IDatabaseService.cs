using BlazorApp.Shared.DataModels;

namespace BlazorApp.Client.Services
{
    public interface IDatabaseService
    {
        Task<ApplicationUser?> AddUserAsync(CreateApplicationUser createApplicationUser);
        Task<long> GetApplicationUserIdAsync(string? userName);
        Task<ApplicationUser?> GetUsersByProviderUserIdAsync(string providerUserId);
    }
}