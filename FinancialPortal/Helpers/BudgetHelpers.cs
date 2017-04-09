using FinancialPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPortal.Helpers
{
    public class BudgetHelpers
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public string GetMonth(int month)
        {
            switch (month)
            {
                case 1:
                    return "January";
                case 2:
                    return "February";
                case 3:
                    return "March";
                case 4:
                    return "April";
                case 5:
                    return "May";
                case 6:
                    return "June";
                case 7:
                    return "July";
                case 8:
                    return "August";
                case 9:
                    return "September";
                case 10:
                    return "October";
                case 11:
                    return "November";
                case 12:
                    return "December";
                default:
                    return "";
            }
        }

        public Month NewMonth(int month,int year, Household household)
        {
            var NewMonth = new Month
            {
                MonthNum = month,
                Year = year,
                Household = household
            };
            NewMonth.Name = GetMonth(NewMonth.MonthNum);
            return NewMonth;
        }

        public Budget DuplicateBudget(Budget budget, Month month)
        {
            Budget newBudget = new Budget
            {
                Name = budget.Name,
                Description = budget.Description,
                Value = budget.Value,
                Expense = budget.Expense,
                Created = DateTimeOffset.UtcNow,
                Categories = budget.Categories,
                Household = budget.Household,
                Month = month
            };
            return newBudget;
        }

        public double SpentBudget(Budget budget)
        {
            var Spent = budget.Categories.Sum(c => c.Transactions.Where(t => !t.Void && t.Bank.Include && t.Created.Month == budget.Month.MonthNum).Sum(t => t.Value));
            return Spent;
        }

        
    }
}