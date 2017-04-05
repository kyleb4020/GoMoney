using FinancialPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPortal.Helpers
{
    public class BankHelpers
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public double TotalAllTrans(Bank bank)
        {
            double balance = 0;
            if (bank.BankType.Name == "Savings Account" ||
                    bank.BankType.Name == "Checking Account" ||
                    bank.BankType.Name == "Retirement Account")
            {
                balance = bank.Transactions.Where(t=>!t.Void).Where(t => !t.Expense).Sum(t => t.Value) -
                bank.Transactions.Where(t => !t.Void).Where(t => t.Expense).Sum(t => t.Value);
            }
            else if (bank.BankType.Name == "Credit Card" ||
                bank.BankType.Name == "Loan")
            {
                balance = bank.Transactions.Where(t => !t.Void).Where(t => t.Expense).Sum(t => t.Value) -
                bank.Transactions.Where(t => !t.Void).Where(t => !t.Expense).Sum(t => t.Value);
            }
                return balance;
        }

        public double NewBankTotal(Bank bank, Transaction transaction)
        {
            if (bank.BankType.Name == "Savings Account" ||
                    bank.BankType.Name == "Checking Account" ||
                    bank.BankType.Name == "Retirement Account")
            {
                if (!transaction.Expense)
                {
                    bank.Balance += transaction.Value;
                }
                else
                {
                    bank.Balance -= transaction.Value;
                }
            }
            else if (bank.BankType.Name == "Credit Card" ||
                bank.BankType.Name == "Loan")
            {
                if (transaction.Expense)
                {
                    bank.Balance += transaction.Value;
                }
                else
                {
                    bank.Balance -= transaction.Value;
                }
            }
            return bank.Balance;
        }
    }
}