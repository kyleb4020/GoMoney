using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinancialPortal.Models;
using FinancialPortal.Helpers;
using FinancialPortal.ViewModels;
using Microsoft.AspNet.Identity;

namespace FinancialPortal.Controllers
{
    [Authorize]
    public class BanksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private HouseholdHelper hh = new HouseholdHelper();
        private BankHelpers bh = new BankHelpers();

        // GET: Banks
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user.Household == null)
            {
                return RedirectToAction("Index", "Households");
            }
            var banks = db.Banks.Include(b => b.Household);
            return View(banks.ToList());
        }

        // GET: Banks/Details/5
        public ActionResult Details(int? id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user.Household == null)
            {
                return RedirectToAction("Index", "Households");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bank bank = db.Banks.Find(id);
            if (bank == null)
            {
                return HttpNotFound();
            }
            var VM = new BankDetailsVM();
            VM.Bank = bank;
            ViewBag.OtherBankId = new SelectList(db.Banks, "Id", "Name");
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            ViewBag.TypeId = new SelectList(db.TransTypes, "Id", "Name");
            ViewBag.EditCategoryId = new SelectList(db.Categories, "Id", "Name");
            ViewBag.EditTypeId = new SelectList(db.TransTypes, "Id", "Name");
            return View(VM);
        }

        // GET: Banks/Create
        public ActionResult Create()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user.Household == null)
            {
                return RedirectToAction("Index", "Households");
            }
            var bankTypes = db.BankTypes.ToList();
            ViewBag.BankTypeId = new SelectList(bankTypes, "Id", "Name");
            return View();
        }

        // POST: Banks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Balance,BankTypeId,Include")] Bank bank)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                bank.InitialBalance = bank.Balance;
                bank.Created = DateTimeOffset.UtcNow;
                bank.Household = user.Household;
                bank.BankType = db.BankTypes.Find(bank.BankTypeId);
                Bank newBank = bank;
                db.Banks.Add(newBank);
                db.SaveChanges();

                //Add Initial Balance Transaction
                var inTrans = new Transaction();
                inTrans.Bank = newBank;
                inTrans.Category = db.Categories.FirstOrDefault(c => c.Name == "Initial Balance");
                inTrans.Created = DateTimeOffset.UtcNow;
                inTrans.Description = "This is the starting balance";
                if(inTrans.Bank.BankType.Name == "Credit Card" || inTrans.Bank.BankType.Name == "Loan")
                {
                    inTrans.Expense = true;
                }
                else if(inTrans.Bank.BankType.Name == "Savings Account" || 
                    inTrans.Bank.BankType.Name == "Checking Account" || 
                    inTrans.Bank.BankType.Name == "Retirement Account")
                {
                    inTrans.Expense = false;
                }
                inTrans.Name = "Initial Balance";
                inTrans.Submitter = user.Id;
                inTrans.Type = db.TransTypes.FirstOrDefault(tt => tt.Name == "Other");
                inTrans.Value = bank.InitialBalance;
                inTrans.Void = false;
                db.Transactions.Add(inTrans);
                db.Banks.Find(newBank.Id).Transactions.Add(inTrans);
                db.SaveChanges();

                //Recalculate Bank Balance
                newBank.Balance = bh.TotalAllTrans(bank);
                db.Entry(newBank).State = EntityState.Modified;

                //Update Household Balance
                user.Household.Balance = hh.HouseholdBalance(user.Household);
                db.Entry(user.Household).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bank);
        }

        // GET: Banks/Edit/5
        public ActionResult Edit(int? id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user.Household == null)
            {
                return RedirectToAction("Index", "Households");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bank bank = db.Banks.Find(id);
            if (bank == null)
            {
                return HttpNotFound();
            }
            var bankTypes = db.BankTypes.ToList();
            ViewBag.BankTypeId = new SelectList(bankTypes, "Id", "Name", bank.BankTypeId);
            return View(bank);
        }

        // POST: Banks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,InitialBalance,BankTypeId,Include")] Bank bank)
        {
            if (ModelState.IsValid)
            {                
                var thisBank = db.Banks.Find(bank.Id);
                var household = thisBank.Household;
                thisBank.Name = bank.Name;
                thisBank.Description = bank.Description;
                thisBank.InitialBalance = bank.InitialBalance;
                thisBank.BankTypeId = bank.BankTypeId;
                thisBank.Include = bank.Include;
                thisBank.Updated = DateTimeOffset.UtcNow;

                //Update Initial Transaction
                var iniTrans = thisBank.Transactions.FirstOrDefault(t => t.Category.Name == "Initial Balance");
                iniTrans.Value = thisBank.InitialBalance;
                iniTrans.Updated = DateTimeOffset.UtcNow;
                iniTrans.Editor = User.Identity.GetUserId();
                db.Entry(iniTrans).State = EntityState.Modified;

                //Recalculate Bank Balance
                thisBank.Balance = bh.TotalAllTrans(thisBank);
                db.Entry(thisBank).State = EntityState.Modified;

                //Update Household Balance
                household.Balance = hh.HouseholdBalance(household);
                db.Entry(household).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", bank.HouseholdId);
            return View(bank);
        }

        // GET: Banks/Delete/5
        public ActionResult Delete(int? id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user.Household == null)
            {
                return RedirectToAction("Index", "Households");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bank bank = db.Banks.Find(id);
            if (bank == null)
            {
                return HttpNotFound();
            }
            return View(bank);
        }

        // POST: Banks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bank bank = db.Banks.Find(id);
            Household household = bank.Household;
            db.Banks.Remove(bank);

            //Update Household balance
            household.Balance = hh.HouseholdBalance(household);
            db.Entry(household).State = EntityState.Modified;

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
