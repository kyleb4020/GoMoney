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
using FinancialPortal.ViewModels;

namespace FinancialPortal.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Categories
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user.Household == null)
            {
                return RedirectToAction("Index", "Households");
            }
            var categories = db.Households.Find(user.HouseholdId).Categories.ToList();
            CategoriesIndexVM VM = new CategoriesIndexVM();
            VM.Categories = categories;
            VM.HouseholdId = Convert.ToInt32(user.HouseholdId);
            var budgetList = db.Budgets.Where(b => b.HouseholdId == user.HouseholdId).ToList();
            budgetList.Add(new Budget { Id = 0, Name = "", Description = "" });
            budgetList = budgetList.OrderBy(b=>b.Id).ToList();
            ViewBag.BudgetId = new SelectList(budgetList, "Id", "Name",0);
            ViewBag.EditBudgetId = new SelectList(budgetList, "Id", "Name");
            return View(VM);
        }

        // GET: Categories/Details/5
        //public ActionResult Details(int? id)
        //{   
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Category category = db.Categories.Find(id);
        //    if (category == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(category);
        //}

        // GET: Categories/Create
        //public ActionResult Create()
        //{            
        //    ViewBag.BudgetId = new SelectList(db.Budgets, "Id", "Name");
        //    return View();
        //}

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description")] Category category, int? BudgetId)
        {
            if (ModelState.IsValid)
            {
                category.Created = DateTimeOffset.UtcNow;
                var household = db.Users.Find(User.Identity.GetUserId()).Household;
                category.Households.Add(household);
                household.Categories.Add(category);
                if (BudgetId != 0 && BudgetId != null)
                {
                    var budget = db.Budgets.Find(BudgetId);
                    category.Budgets.Add(budget);
                    budget.Categories.Add(category);
                    db.Entry(budget).State = EntityState.Modified;
                }

                db.Entry(household).State = EntityState.Modified;

                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index","Categories");
            }
            var user = db.Users.Find(User.Identity.GetUserId());
            var categories = db.Households.Find(user.HouseholdId).Categories.ToList();
            CategoriesIndexVM VM = new CategoriesIndexVM();
            VM.Categories = categories;
            VM.HouseholdId = Convert.ToInt32(user.HouseholdId);
            var budgetList = db.Budgets.Where(b => b.HouseholdId == user.HouseholdId).ToList();
            budgetList.Add(new Budget { Id = 0, Name = "", Description = "" });
            budgetList.Sort();
            ViewBag.BudgetId = new SelectList(budgetList, "Id", "Name", 0);
            ViewBag.EditBudgetId = new SelectList(budgetList, "Id", "Name");
            ViewBag.ErrorMessage = "There was an error, please try to create the new category again.";
            return View("Index","Categories",VM);
        }

        // GET: Categories/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Category category = db.Categories.Find(id);
        //    if (category == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.BudgetId = new SelectList(db.Budgets, "Id", "Name");
        //    return View(category);
        //}

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Default")] Category category, int EditBudgetId)
        {
            if (ModelState.IsValid)
            {
                var Cat = db.Categories.Find(category.Id);
                var user = db.Users.Find(User.Identity.GetUserId());
                var oldBudget = Cat.Budgets.FirstOrDefault(b => b.HouseholdId == user.HouseholdId);
                if (Cat.Default)
                {
                    Cat.Households.Remove(user.Household);
                    Category NewCategory = new Category
                    {
                        Name = category.Name,
                        Description = category.Description,
                        Created = DateTimeOffset.UtcNow,
                        Default = false
                    };
                    NewCategory.Households.Add(user.Household);
                    if(EditBudgetId != 0)
                    {
                        NewCategory.Budgets.Add(db.Budgets.Find(EditBudgetId));
                    }
                    db.Categories.Add(NewCategory);
                }
                else
                { 
                    Cat.Name = category.Name;
                    Cat.Description = category.Description;
                    if (oldBudget != null)
                    {
                        if (oldBudget.Id != EditBudgetId && EditBudgetId != 0)
                        {
                            Cat.Budgets.Remove(oldBudget);
                            Cat.Budgets.Add(db.Budgets.Find(EditBudgetId));
                        }
                    }
                    else
                    {
                        if(EditBudgetId != 0)
                        { 
                            Cat.Budgets.Add(db.Budgets.Find(EditBudgetId));
                        }
                    }
                }
                db.Entry(Cat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BudgetId = new SelectList(db.Budgets, "Id", "Name");
            return View(category);
        }

        // GET: Categories/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Category category = db.Categories.Find(id);
        //    if (category == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(category);
        //}

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? budgetId, bool Default)
        {
            Category category = db.Categories.Find(id);
            if (Default)
            {
                var household = db.Users.Find(User.Identity.GetUserId()).Household;
                household.Categories.Remove(category);
                category.Households.Remove(household);
                if(budgetId != null)
                {
                    household.Budgets.FirstOrDefault(b => b.Id == budgetId).Categories.Remove(category);
                    category.Budgets.Remove(db.Budgets.Find(budgetId));
                }
                db.Entry(household).State = EntityState.Modified;
                db.Entry(category).State = EntityState.Modified;
            }
            else
            {
                db.Categories.Remove(category);
            }
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
