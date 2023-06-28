using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BlazorApp.Shared
{
    public class SearchVideosModel
    {
        [Required]
        public int? PageNumber { get; set; }
        [Required]
        public string SearchTerm { get; set; }
        public string VideoId { get; set; }
    }
}
