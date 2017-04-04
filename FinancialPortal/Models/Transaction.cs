using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinancialPortal.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        [Display(Name = "Title")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Display(Name = "Amount")]
        public double Value { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        public bool Expense { get; set; }
        public bool Reconciled { get; set; }
        public bool Void { get; set; }
        public double ReconciledValue { get; set; }
        public string Submitter { get; set; }
        public string Editor { get; set; }
        public int BankId { get; set; }
        public int? CategoryId { get; set; }
        public int? TypeId { get; set; }

        public virtual Bank Bank { get; set; }
        public virtual Category Category { get; set; }
        public virtual TransType Type {get;set;}
    }
}