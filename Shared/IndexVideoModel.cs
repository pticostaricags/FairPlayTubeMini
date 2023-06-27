using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Shared
{
    public class IndexVideoModel
    {
        [Required]
        public string VideoFileName { get; set; }
        [Required]
        public string VideoSourceUrl { get; set; }
    }
}