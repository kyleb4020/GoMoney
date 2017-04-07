using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinancialPortal.Models
{
    public class Category
    {
        public Category()
        {
            this.Transactions = new HashSet<Transaction>();
            this.Households = new HashSet<Household>();
            this.Budgets = new HashSet<Budget>();
        }

        public int Id { get; set; }
        [Display(Name = "Category")]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Default { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }

        public virtual ICollection<Household> Households { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<Budget> Budgets { get; set; }
    }
}