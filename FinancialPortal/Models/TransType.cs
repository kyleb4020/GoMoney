using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinancialPortal.Models
{
    public class TransType
    {
        public int Id { get; set; }
        [Display(Name = "Type")]
        public string Name { get; set; }
    }

    //This will be for things like:
    //~Cash
    //~Credit Card
    //~Check
    //~Debit Card
}