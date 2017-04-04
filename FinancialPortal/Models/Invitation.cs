using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinancialPortal.Models
{
    public class Invitation
    {
        public int Id { get; set; }
        [Display(Name = "Recipient Email")]
        public string ToEmail { get; set; }
        [Display(Name = "Sender")]
        public string GenUserId { get; set; }
        public string Code { get; set; }
        [Display(Name = "Accepted")]
        public bool Accepted { get; set; }
        public bool Expired { get; set; }
        public int HouseholdId { get; set; }
        [Display(Name = "Date Sent")]
        public DateTimeOffset Created { get; set; }
        [Display(Name = "Date Expires")]
        public DateTimeOffset? Expiration { get; set; }

        public virtual Household Household { get; set; }
    }
}