﻿using FinancialPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPortal.ViewModels
{
    public class HouseholdIndexVM
    {
        public Household Household { get; set; }
        public EmailModel Email { get; set; }
    }

    public class BankDetailsVM
    {
        public Bank Bank { get; set; }
        public Transaction Transaction { get; set; }
    }

    public class TransactionIndexVM
    {
        public ICollection<Transaction> AllTrans { get; set; }
        public Transaction NewTrans { get; set; }
    }

    public class CategoriesIndexVM
    {
        public ICollection<Category> Categories { get; set; }
        public int HouseholdId { get; set; }
        public Category NewCategory { get; set; }
    }

    public class BudgetsIndexVM
    {
        public ICollection<Budget> Budgets { get; set; }
        public ICollection<Month> Months { get; set; }
        public Budget NewBudget { get; set; }
    }

    public class BudgetDetailsVM
    {
        public Budget Budget { get; set; }
        public ICollection<Transaction>Transactions { get; set; }
    }

    public class DashboardVM
    {
        //public ICollection<Budget> Budgets { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
        public Month Month { get; set; }
        //public ICollection<Bank> Banks { get; set; }
        //public string DashBudgets { get; set; }
        //public string DashSpent { get; set; }
        //public string DashAccounts { get; set; }
    }

    public class DashBudgetsVM
    {
        public string Name { get; set; }
        public double Amount { get; set; }
        public double Spent { get; set; }
    }

    public class DashAccountsVM
    {
        public string Name { get; set; }
        public double Amount { get; set; }
        public double? Reconciled { get; set; }
    }

    public class DashSpentVM
    {
        public string label { get; set; }
        public double data { get; set; }
    }
}