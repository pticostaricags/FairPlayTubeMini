using Microsoft.AspNetCore.Components;

namespace BlazorApp.Client.Components
{
    public partial class MenuGrid
    {
        [EditorRequired]
        [Parameter]
        public MenuGridItem[]? MenuItems { get; set; }

        public class MenuGridItem
        {
            public string? Title { get; set; } = string.Empty;
            public string? CssClass { get; set; } = string.Empty;
            public EventCallback OnClick { get; set; }
            public bool ShowTitleBelowIcon { get; set; } = false;
            public string? ImageSrc { get; set; }
            public string? ImageCss { get; set; }
            public bool ShowImage { get; set; }
        }
    }
}
