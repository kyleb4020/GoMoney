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
using Microsoft.AspNet.Identity;
using FinancialPortal.ViewModels;

namespace FinancialPortal.Controllers
{
    [Authorize]
    public class TransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private BankHelpers bh = new BankHelpers();
        private HouseholdHelper hh = new HouseholdHelper();

        // GET: Transactions
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user.Household == null)
            {
                return RedirectToAction("Index", "Households");
            }
            var transactions = db.Households.Find(user.HouseholdId).Banks.SelectMany(b=>b.Transactions.Where(t=>t.Void == false));
            ViewBag.OtherBankId = new SelectList(db.Banks, "Id", "Name");
            ViewBag.BankId = new SelectList(db.Banks, "Id", "Name");
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            ViewBag.TypeId = new SelectList(db.TransTypes, "Id", "Name");
            ViewBag.EditCategoryId = new SelectList(db.Categories, "Id", "Name");
            ViewBag.EditTypeId = new SelectList(db.TransTypes, "Id", "Name");
            TransactionIndexVM VM = new TransactionIndexVM();
            VM.AllTrans = transactions.ToList();
            return View(VM);
        }

        // GET: Transactions/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Transaction transaction = db.Transactions.Find(id);
        //    if (transaction == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(transaction);
        //}

        // GET: Transactions/Create
        //public ActionResult Create()
        //{
        //    ViewBag.BankId = new SelectList(db.Banks, "Id", "Name");
        //    ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
        //    ViewBag.TypeId = new SelectList(db.TransTypes, "Id", "Name");
        //    return View();
        //}

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Value,Expense,BankId")] Transaction transaction, int CategoryId, int TypeId, int? OtherBankId, bool Connected)
        {
            if (ModelState.IsValid)
            {
                //Finalize Transaction Info
                transaction.Category = db.Categories.Find(CategoryId);
                transaction.Type = db.TransTypes.Find(TypeId);
                transaction.Created = DateTimeOffset.UtcNow;
                transaction.Submitter = User.Identity.GetUserId();
                db.Transactions.Add(transaction);
                //Create Connected Transaction
                if (Connected)
                {
                    var connectedTrans = new Transaction();
                    connectedTrans.Name = transaction.Name;
                    connectedTrans.Description = transaction.Description;
                    connectedTrans.Value = transaction.Value;
                    connectedTrans.Expense = !transaction.Expense;
                    connectedTrans.BankId = Convert.ToInt32(OtherBankId);
                    connectedTrans.Category = db.Categories.Find(CategoryId);
                    connectedTrans.Type = db.TransTypes.Find(TypeId);
                    connectedTrans.Created = DateTimeOffset.UtcNow;
                    connectedTrans.Submitter = User.Identity.GetUserId();
                    db.Transactions.Add(connectedTrans);

                    //Calculate Connected Bank Totals
                    var bank2 = db.Banks.Find(OtherBankId);
                    bank2.Balance = bh.NewBankTotal(bank2, connectedTrans);
                    
                    db.Entry(bank2).State = EntityState.Modified;
                }
                db.SaveChanges();

                //Calculate New Bank Totals
                var bank1 = db.Banks.Find(transaction.BankId);
                bank1.Balance = bh.NewBankTotal(bank1, transaction);
                
                db.Entry(bank1).State = EntityState.Modified;

                //Update Household Total
                var household = bank1.Household;
                household.Balance = hh.HouseholdBalance(household);
                
                db.Entry(household).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Details","Banks",new { id = transaction.BankId});
            }

            ViewBag.OtherBankId = new SelectList(db.Banks, "Id", "Name", OtherBankId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", transaction.CategoryId);
            ViewBag.TypeId = new SelectList(db.TransTypes, "Id", "Name", transaction.TypeId);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Transaction transaction = db.Transactions.Find(id);
        //    if (transaction == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.BankId = new SelectList(db.Banks, "Id", "Name", transaction.BankId);
        //    ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", transaction.CategoryId);
        //    ViewBag.TypeId = new SelectList(db.TransTypes, "Id", "Name", transaction.TypeId);
        //    return View(transaction);
        //}

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Value,Created,Updated,Expense,Reconciled,ReconciledValue,Submitter,Editor,BankId,CategoryId,TypeId")] Transaction transaction, int EditCategoryId, int EditTypeId)
        {
            if (ModelState.IsValid)
            {
                //Update Ticket
                var Trans = db.Transactions.Find(transaction.Id);
                Trans.Name = transaction.Name;
                Trans.Description = transaction.Description;
                Trans.Value = transaction.Value;
                Trans.Updated = DateTimeOffset.UtcNow;
                Trans.Expense = transaction.Expense;
                Trans.Reconciled = transaction.Reconciled;
                Trans.ReconciledValue = transaction.ReconciledValue;
                Trans.Editor = User.Identity.GetUserId();
                Trans.Category = db.Categories.Find(EditCategoryId);
                Trans.Type = db.TransTypes.Find(EditTypeId);
                db.Entry(Trans).State = EntityState.Modified;
                db.SaveChanges();

                //Calculate New Bank Totals
                var bank1 = db.Banks.Find(Trans.BankId);
                bank1.Balance = bh.TotalAllTrans(bank1);

                db.Entry(bank1).State = EntityState.Modified;

                //Update Household Total
                var household = bank1.Household;
                household.Balance = hh.HouseholdBalance(household);

                db.Entry(household).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Details", "Banks", new { id = bank1.Id });
            }

            return RedirectToAction("Details", "Banks", new { id = transaction.BankId });
        }

        // GET: Transactions/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Transaction transaction = db.Transactions.Find(id);
        //    if (transaction == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(transaction);
        //}

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? bankId)
        {
            Transaction transaction = db.Transactions.Find(id);
            transaction.Void = true;
            db.SaveChanges();
            if(bankId != null)
            {
                return RedirectToAction("Details", "Banks", new { id = bankId });
            }
            else
            {
                return RedirectToAction("Index", "Transactions");
            }
        }

        //GET Transactions/Voided
        public ActionResult Voided()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user.Household == null)
            {
                return RedirectToAction("Index", "Households");
            }
            var transactions = db.Households.Find(user.HouseholdId).Banks.SelectMany(b => b.Transactions.Where(t => t.Void));

            return View(transactions);
        }

        //POST: Transactions/Voided/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Voided(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            transaction.Void = false;
            db.SaveChanges();
            return RedirectToAction("Voided", "Transactions");
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
