using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorApp.Shared.DataModels
{
    public class CreateVideoInfo
    {
        public long OwnerApplicationUserId { get; set; }
        public string AccountId { get; set; }
        public string VideoId { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
