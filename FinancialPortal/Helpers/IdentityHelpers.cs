using FinancialPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace FinancialPortal.Helpers
{
    public static class IdentityHelpers
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        //I made this so I could add DisplayName as a method onto an IIdentity.
        //This is useful for me because it displays their DisplayName in the _LoginPartial
        public static bool Household(this IIdentity user)
        {
            var thisUser = db.Users.FirstOrDefault(u => u.UserName == user.Name);
            if (thisUser.Household != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}