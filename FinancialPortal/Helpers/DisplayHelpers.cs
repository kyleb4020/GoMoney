using FinancialPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace FinancialPortal.Helpers
{
    public static class DisplayHelpers
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static bool InHousehold(string userId)
        {
            var user = db.Users.Find(userId);
            if(user.Household != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string FirstName(string userId)
        {
            var user = db.Users.Find(userId);

            return (user.FirstName);
        }
        public static string LastName(string userId)
        {
            var user = db.Users.Find(userId);

            return (user.LastName);
        }
        public static string DisplayName(string userId)
        {
            var user = db.Users.Find(userId);

            return (user.DisplayName);
        }
        public static string MonthName(int monthNum)
        {
            switch (monthNum)
            {
                case 1:
                    return ("January");
                case 2:
                    return ("February");
                case 3:
                    return ("March");
                case 4:
                    return ("April");
                case 5:
                    return ("May");
                case 6:
                    return ("June");
                case 7:
                    return ("July");
                case 8:
                    return ("August");
                case 9:
                    return ("September");
                case 10:
                    return ("October");
                case 11:
                    return ("November");
                case 12:
                    return ("December");
                default:
                    return ("");
            }
        }
        public static DateTime TimeZone(DateTimeOffset currentDateTime, int TimeZoneOffset)
        {
            var offset = -1 * (TimeZoneOffset);
            var utcTime = currentDateTime.ToUniversalTime();
            DateTime adjTime = utcTime.AddMinutes(offset).DateTime;
            return adjTime;
        }
    }
}