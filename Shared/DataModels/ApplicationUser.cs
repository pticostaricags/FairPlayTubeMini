using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorApp.Shared.DataModels
{

    public class ApplicationUserList
    {
        public ApplicationUser[] value { get; set; }
    }

    public class ApplicationUser
    {
        public int ApplicationUserId { get; set; }
        public string Username { get; set; }
    }


}
