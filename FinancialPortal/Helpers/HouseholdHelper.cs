using FinancialPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPortal.Helpers
{
    public class HouseholdHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public double HouseholdBalance(Household household)
        {
            var balance = household.Banks.Where(b => b.Include == true).
                    Where(bt => bt.BankType.Name == "Savings Account" ||
                    bt.BankType.Name == "Checking Account" ||
                    bt.BankType.Name == "Retirement Account").Sum(b => b.Balance) -
                    household.Banks.Where(b => b.Include == true).
                    Where(bt => bt.BankType.Name == "Credit Card" ||
                    bt.BankType.Name == "Loan").Sum(b => b.Balance);
            return balance;
        }
    }
}