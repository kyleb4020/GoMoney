using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinancialPortal.Models
{
    public class EmailModel
    {
        [Required, Display(Name = "Name")]
        public string ToName { get; set; }
        [Required, Display(Name = "Email"), EmailAddress]
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        [AllowHtml]
        public string Body { get; set; }
    }
}