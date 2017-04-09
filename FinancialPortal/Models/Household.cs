using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPortal.Models
{
    public class Household
    {
        public Household()
        {
            this.Banks = new HashSet<Bank>();
            this.Budgets = new HashSet<Budget>();
            this.Members = new HashSet<ApplicationUser>();
            this.Invitations = new HashSet<Invitation>();
            this.Categories = new HashSet<Category>();
            this.Months = new HashSet<Month>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public double Balance { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }

        public virtual ICollection<Month> Months { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Invitation> Invitations { get; set; }
        public virtual ICollection<ApplicationUser> Members { get; set; }
        public virtual ICollection<Bank> Banks { get; set; }
        public virtual ICollection<Budget> Budgets { get; set; }
    }
}