using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinancialPortal.Models
{
    public class Budget
    {
        public Budget()
        {
            this.Categories = new HashSet<Category>();
        }

        public int Id { get; set; }
        [Display(Name = "Budget")]
        public string Name { get; set; }
        public string Description { get; set; }
        public double Value { get; set; }
        public double Spent { get; set; }
        public bool Expense { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        public int? HouseholdId { get; set; }
        public int MonthId { get; set; }

        public virtual Month Month { get; set; }
        public virtual Household Household { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}