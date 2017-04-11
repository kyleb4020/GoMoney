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
            var budgets = db.Households.Find(user.Household.Id).Budgets.Where(b => b.Month.MonthNum == DateTime.Now.Month && b.Expense).ToList();
            var transactions = db.Households.Find(user.Household.Id).Banks.SelectMany(b => b.Transactions).Where(t=>t.Created > DateTime.Today.AddDays(-7)).OrderByDescending(t=>t.Created).ToList();
            //string JSONbudgets = JsonConvert.SerializeObject(budgets);
            //ViewBag.JSONbudgets = JSONbudgets;
            DashboardVM VM = new DashboardVM();
            VM.Budgets = budgets;
            VM.Transactions = transactions;
            return View(VM);
        }

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}

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