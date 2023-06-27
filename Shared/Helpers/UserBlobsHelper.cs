using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorApp.Shared.Helpers
{
    public static class UserBlobsHelper
    {
        public static string GetBlobRelativePath(string userId, string fileName)
        {
            return $"{userId}/{fileName}";
        }
    }
}
