using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinancialPortal.Models;
using Microsoft.AspNet.Identity;
using System.Security.Cryptography;
using System.Text;

namespace FinancialPortal.Helpers
{

    public class InvitationHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private static Random random = new Random();

        public static string GetUniqueKey(int maxSize)
        {
            char[] chars = new char[62];
            chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[maxSize];
                crypto.GetNonZeroBytes(data);
            }
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }

        public Invitation CreateInvitation(int HouseholdId, string ToEmail, string userId)
        {
            var invite = new Invitation();
            invite.ToEmail = ToEmail;
            invite.GenUserId = userId;
            invite.Accepted = false;
            invite.Expired = false;
            invite.Created = DateTimeOffset.Now;
            invite.Expiration = DateTimeOffset.Now.Date.AddMonths(1);
            invite.HouseholdId = HouseholdId;
            invite.Code = GetUniqueKey(20);

            return (invite);
        }
    }
}