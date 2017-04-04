using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPortal.Models
{
    public class Bank
    {
        public Bank()
        {
            this.Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double InitialBalance { get; set; }
        public double Balance { get; set; }
        public double? ReconciledBalance { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        public int? HouseholdId { get; set; }
        public int? BankTypeId { get; set; }
        public bool Include { get; set; }

        public virtual Household Household { get; set; }
        public virtual BankType BankType { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}