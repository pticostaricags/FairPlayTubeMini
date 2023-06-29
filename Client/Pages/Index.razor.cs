using BlazorApp.Client.Components;
using BlazorApp.Client.Services;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Routes = BlazorApp.Shared.Constants.Routes;

namespace BlazorApp.Client.Pages
{
    public partial class Index
    {
        [CascadingParameter]
        private Task<AuthenticationState>? AuthenticationStateTask { get; set; }
        [Inject]
        private IToastService? ToastService { get; set; }
        [Inject]
        private INavigationService? NavigationService { get; set; }
        private bool IsBusy { get; set; }
        private MenuGrid.MenuGridItem[] MenuGridItems = new MenuGrid.MenuGridItem[0];

        protected override async Task OnInitializedAsync()
        {
            try
            {
                this.IsBusy = true;
                var state = await this.AuthenticationStateTask!;
                if (state?.User?.Identity?.IsAuthenticated == true)
                {
                    if (state.User.IsInRole(BlazorApp.Shared.Constants.Roles.admin))
                    {
                        this.MenuGridItems =
                        this.MenuGridItems.Union(new MenuGrid.MenuGridItem[]
                        {
                            new MenuGrid.MenuGridItem()
                            {
                                CssClass="bi bi-people-fill",
                                OnClick=new EventCallback(this,()=>this.NavigationService!
                                .NavigateToListUsers()),
                                ShowTitleBelowIcon=true,
                                Title="List Users"
                            }
                        })
                        .ToArray();
                    }
                    if (state.User.IsInRole(BlazorApp.Shared.Constants.Roles.creator))
                    {
                        this.MenuGridItems =
                        this.MenuGridItems.Union(new MenuGrid.MenuGridItem[]
                        {
                            new MenuGrid.MenuGridItem()
                            {
                                CssClass="bi bi-cloud-upload-fill",
                                OnClick=new EventCallback(this,()=>this.NavigationService!
                                .NavigateToUploadVideo()),
                                ShowTitleBelowIcon=true,
                                Title="Upload Video"
                            }
                        })
                        .ToArray();
                    }
                }
                this.MenuGridItems =
                        this.MenuGridItems.Union(new MenuGrid.MenuGridItem[]
                        {
                            new MenuGrid.MenuGridItem()
                            {
                                Title="Videos List",
                                CssClass="bi bi-play-btn-fill",
                                ShowTitleBelowIcon=true,
                                OnClick=new EventCallback(this,()=>this.NavigationService!
                                .NavigateToVideosList()),
                            }
                        })
                        .ToArray();
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
