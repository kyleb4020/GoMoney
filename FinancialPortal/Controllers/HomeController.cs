using FinancialPortal.Models;
using FinancialPortal.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinancialPortal.Controllers
{
    [RequireHttps]
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if(user.Household == null)
            {
                return RedirectToAction("Index", "Households");
            }
            var transactions = db.Households.Find(user.Household.Id).Banks.SelectMany(b => b.Transactions).Where(t=>t.Created > DateTime.Today.AddDays(-7)).OrderByDescending(t=>t.Created).ToList();
            
            DashboardVM VM = new DashboardVM();
            VM.Transactions = transactions;
            VM.Month = db.Households.Find(user.Household.Id).Months.FirstOrDefault(m=>m.MonthNum == DateTime.Now.Month && m.Year == DateTime.Now.Year);
            var monthList = db.Households.Find(user.Household.Id).Months.ToList();
            ViewBag.BudgetMonths = new SelectList(monthList, "Id", "Name", VM.Month.Id);
            ViewBag.SpendingMonths = new SelectList(monthList, "Id", "Name", VM.Month.Id);
            return View(VM);
        }

        public ActionResult GetBudgetsData(int month)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var Month = db.Months.Find(month);
            var data = db.Households.Find(user.Household.Id).Budgets.Where(b => b.Month.MonthNum == Month.MonthNum && b.Expense).ToList().Select(b => new
            {
                Name = b.Name,
                Amount = b.Value,
                Spent = b.Spent
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAccountsData()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var data = db.Households.Find(user.Household.Id).Banks.ToList().Select(b=>new
            {
                Name = b.Name,
                Amount = b.Balance,
                Reconciled = b.ReconciledBalance
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSpendingData(int month)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var Month = db.Months.Find(month);
            var data = db.Households.Find(user.Household.Id).Budgets.Where(b => b.Month.MonthNum == Month.MonthNum && b.Expense).ToList().Select(b=>new
            {
                label = b.Name,
                data = b.Spent
            });
            
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        
        public FileContentResult UserPhotos(string userId)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (userId == null)
                {
                    userId = User.Identity.GetUserId();
                }

                if(userId == null)
                {
                    string fileName = HttpContext.Server.MapPath(@"/img/user.jpg");

                    byte[] imageData = null;
                    FileInfo fileInfo = new FileInfo(fileName);
                    long imageFileLength = fileInfo.Length;
                    FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    imageData = br.ReadBytes((int)imageFileLength);

                    return File(imageData, "image/png");
                }

                var bdUsers = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
                var userImage = bdUsers.Users.Find(userId);

                if (userImage.ProfilePic == null)
                {
                    string fileName = HttpContext.Server.MapPath(@"/img/user.jpg");

                    byte[] imageData = null;
                    FileInfo fileInfo = new FileInfo(fileName);
                    long imageFileLength = fileInfo.Length;
                    FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    imageData = br.ReadBytes((int)imageFileLength);

                    return File(imageData, "image/png");
                }
                return new FileContentResult(userImage.ProfilePic, "image/jpeg");
            }
            else
            {
                string fileName = HttpContext.Server.MapPath(@"/img/user.jpg");

                byte[] imageData = null;
                FileInfo fileInfo = new FileInfo(fileName);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData = br.ReadBytes((int)imageFileLength);

                return File(imageData, "image/png");
            }
        }
    }
}