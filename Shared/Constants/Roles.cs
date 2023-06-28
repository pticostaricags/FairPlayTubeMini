using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorApp.Shared.Constants
{
    public static class Roles
    {
        public const string admin = nameof(admin);
        public const string creator = nameof(creator);
        public const string anonymous = nameof(anonymous);
        public const string authenticated = nameof(authenticated);
    }
}
