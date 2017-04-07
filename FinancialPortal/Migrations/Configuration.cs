namespace FinancialPortal.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FinancialPortal.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(FinancialPortal.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }


            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "demogomoney@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "demogomoney@mailinator.com",
                    Email = "demogomoney@mailinator.com",
                    FirstName = "Demo",
                    LastName = "User",
                    DisplayName = "Demo Admin"
                }, "Go4Money!");
                var userId = userManager.FindByEmail("demogomoney@mailinator.com").Id;
                userManager.AddToRole(userId, "Admin");
            }
            //context.SaveChanges();

            context.TransTypes.AddOrUpdate(ty => ty.Name,
                new TransType { Name = "Debit" },
                new TransType { Name = "Credit" },
                new TransType { Name = "Cash" },
                new TransType { Name = "Check" },
                new TransType { Name = "Other" });
            //context.SaveChanges();

            context.BankTypes.AddOrUpdate(bt => bt.Name,
                new BankType { Name = "Credit Card" },
                new BankType { Name = "Savings Account" },
                new BankType { Name = "Checking Account" },
                new BankType { Name = "Retirement Account" },
                new BankType { Name = "Loan" }
                );

            context.Categories.AddOrUpdate(c => c.Name,
                new Category
                {
                    Name = "Charitable Giving",
                    Description = "Purchases or donations for charity",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Gifts",
                    Description = "Purchases made to give to someone else",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Automotive/Fuel",
                    Description = "Expenses related to vehicles such as fuel and maintenance",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Healthcare/Medical",
                    Description = "All health-related expenses including doctor's visits and medicine",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Insurance",
                    Description = "All expenses related to any and all insurance",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Office Expenses",
                    Description = "Expenses for the office (i.e. Office Supplies, Office Maintenance, Printing, etc.)",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Services/Supplies",
                    Description = "Money spend on services or supplies (i.e. dry cleaning, housekeeping, etc.)",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Postage/Shipping",
                    Description = "Expense for mailing letters or sending packages",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Taxes",
                    Description = "The money you give to Uncle Sam",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Other Expenses",
                    Description = "Expenses that don't belong with normal expenses",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Check Payment",
                    Description = "Payment made via check (either to you or from you)",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Service Charges/Fees",
                    Description = "ATM Fees, Transfer Fees, and other pesky fees",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Home Improvement",
                    Description = "Money spent on improving or maintaining the home",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Electronics/General Merchandise",
                    Description = "Purchases made on new gadgets, gizmos, and other goodies",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Groceries",
                    Description = "Food purchases for things you eat at home",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Pets/Pet Care",
                    Description = "Money spent on animal care (i.e. food, grooming, etc.)",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Mortgage",
                    Description = "The often monthly payment made towards owning your home",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Rent",
                    Description = "The often monthly payment made to your landlord so you can keep living in your home",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Loans",
                    Description = "Money paid to those from whom you've borrowed money",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Utilities",
                    Description = "Payment for things such as gas, electric, water, sewage, etc.",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Cable/Satellite/Telecom",
                    Description = "Payment for your television services",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Personal/Family",
                    Description = "Money spent on personal or family items (i.e. clothing, haircut, etc)",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "ATM/Cash Withdraws",
                    Description = "Cash money taken from your accounts",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Education",
                    Description = "Expenses for education (i.e. tuition, books, etc.)",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Subscriptions/Renewals",
                    Description = "Money spent on items like Magazine or Newspaper subscriptions",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Restaurants",
                    Description = "Money spent on eating out",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Entertainment/Recreation",
                    Description = "Money spent on things done just for fun (i.e. movies, concerts, hobbies, etc.)",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Travel",
                    Description = "Money spent to see the world (i.e. airfare, hotels, etc.)",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Income",
                    Description = "Money that is paid to you",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Sales/Service Income",
                    Description = "Income from sales or services provided by you",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Salary/Regular  Income",
                    Description = "Your (hopefully) consistent paycheck",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Investment/Retirement Income",
                    Description = "Money made from investments",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Interest Income",
                    Description = "Money made from bank accounts as interest",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Other Income",
                    Description = "Income from sources other than others listed",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Rewards",
                    Description = "Sometimes rewards come in the form of cash",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Deposits",
                    Description = "Money put into a bank account",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Expenses Reimbursements",
                    Description = "Getting paid back for money previously spent",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Refunds/Adjustments",
                    Description = "Money back or extra paid related to another transaction",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Transfers",
                    Description = "Money moved from one account into another",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Securities Trades",
                    Description = "Money moving related to securies",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Savings",
                    Description = "Money put away for a rainy day or for something fun",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Credit Card Payments",
                    Description = "Money paid to credit cards",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Deferred Compensation",
                    Description = "Money that came after some time",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Retirement Contributions",
                    Description = "Money set aside for when you retire",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Initial Balance",
                    Description = "This is the starting balance of a newly added account",
                    Created = DateTimeOffset.Now,
                    Default = true
                },
                new Category
                {
                    Name = "Uncategorized",
                    Description = "If you're not sure how to categorize something, put it here",
                    Created = DateTimeOffset.Now,
                    Default = true
                }
                );
            //context.SaveChanges();

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
