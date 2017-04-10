using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinancialPortal.Models;
using Microsoft.AspNet.Identity;
using FinancialPortal.Helpers;
using FinancialPortal.ViewModels;
using DevTrends.MvcDonutCaching;

namespace FinancialPortal.Controllers
{
    [Authorize]
    public class BudgetsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private BudgetHelpers bh = new BudgetHelpers();

        // GET: Budgets
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if(user.Household == null)
            {
                return RedirectToAction("Index", "Households");
            }
            //Check if current month has been created
            var thisMonth = DateTime.Now.Month;
            var thisYear = DateTime.Now.Year;
            if (!user.Household.Months.Any(m=>m.MonthNum == thisMonth && m.Year == thisYear))
            {
                var month = bh.NewMonth(thisMonth, thisYear, user.Household);
                if(user.Household.Budgets != null)
                {
                    foreach(var budget in user.Household.Budgets)
                    {
                        Budget newBudget = bh.DuplicateBudget(budget, month);
                        db.Budgets.Add(newBudget);
                        month.Budgets.Add(newBudget);
                    }
                }
                db.Months.Add(month);
                db.SaveChanges();
            }
            //Update Spent totals
            foreach(var budge in user.Household.Budgets)
            {
                budge.Spent = bh.SpentBudget(budge);
                db.Entry(budge).State = EntityState.Modified;
                db.SaveChanges();
            }
            //
            var budgets = user.Household.Budgets.ToList();
            var months = user.Household.Months.OrderBy(m=>m.Year).OrderBy(m => m.MonthNum).ToList();
            var VM = new BudgetsIndexVM();
            VM.Budgets = budgets;
            VM.Months = months;
            var categories = user.Household.Categories.ToList();
            ViewBag.CategoryId = new MultiSelectList(categories, "Id", "Name");
            ViewBag.EditCategoryId = new MultiSelectList(categories, "Id", "Name");

            return View(VM);
        }

        //GET: Budgets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user.Household == null)
            {                
                return RedirectToAction("Index", "Households");
            }
            Budget budget = db.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            var transactions = budget.Categories.SelectMany(c => c.Transactions.Where(t => !t.Void && t.Bank.Include && t.Created.Month == budget.Month.MonthNum));
            BudgetDetailsVM VM = new BudgetDetailsVM();
            VM.Budget = budget;
            VM.Transactions = transactions.ToList();
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            ViewBag.TypeId = new SelectList(db.TransTypes, "Id", "Name");
            ViewBag.EditCategoryId = new SelectList(db.Categories, "Id", "Name");
            ViewBag.EditTypeId = new SelectList(db.TransTypes, "Id", "Name");
            return View(VM);
        }

        // GET: Budgets/Create
        //public ActionResult Create()
        //{
        //    ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name");
        //    return View();
        //}

        // POST: Budgets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Value,Expense")] Budget budget, List<int> CategoryId)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                budget.Created = DateTimeOffset.UtcNow;
                budget.Household = user.Household;
                budget.Month = user.Household.Months.FirstOrDefault(m => m.MonthNum == DateTime.Now.Month && m.Year == DateTime.Now.Year);
                foreach(var cat in CategoryId)
                {
                    budget.Categories.Add(db.Categories.Find(cat));
                }
                budget.Spent = bh.SpentBudget(budget);

                db.Budgets.Add(budget);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", budget.HouseholdId);
            return View(budget);
        }

        // GET: Budgets/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Budget budget = db.Budgets.Find(id);
        //    if (budget == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", budget.HouseholdId);
        //    return View(budget);
        //}

        // POST: Budgets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Value,Expense,Created,Updated,HouseholdId")] Budget budget, List<int>EditCategoryId)
        {
            if (ModelState.IsValid)
            {
                Budget EBudget = db.Budgets.Find(budget.Id);
                EBudget.Name = budget.Name;
                EBudget.Description = budget.Description;
                EBudget.Value = budget.Value;
                EBudget.Expense = budget.Expense;
                EBudget.Categories.Clear();
                foreach(var id in EditCategoryId)
                {
                    EBudget.Categories.Add(db.Categories.Find(id));
                }
                EBudget.Updated = DateTimeOffset.UtcNow;

                db.Entry(EBudget).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", budget.HouseholdId);
            return View(budget);
        }

        // GET: Budgets/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Budget budget = db.Budgets.Find(id);
        //    if (budget == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(budget);
        //}

        // POST: Budgets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Budget budget = db.Budgets.Find(id);
            db.Budgets.Remove(budget);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
