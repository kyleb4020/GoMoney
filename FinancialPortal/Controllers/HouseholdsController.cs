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
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Configuration;
using FinancialPortal.ViewModels;
using FinancialPortal.Helpers;
using DevTrends.MvcDonutCaching;

namespace FinancialPortal.Controllers
{
    [Authorize]
    public class HouseholdsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private InvitationHelper ih = new InvitationHelper();
        private HouseholdHelper hh = new HouseholdHelper();

        // GET: Households
        public ActionResult Index(string invite, string error)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var email = new EmailModel();
            var IndexVM = new HouseholdIndexVM();
            IndexVM.Email = email;
            if(user.Household != null)
            {
                foreach(var invitation in user.Household.Invitations)
                {
                    if(invitation.Expiration.Value.DateTime < DateTime.Now)
                    {
                        invitation.Expired = true;
                    }
                }
                //Update Household Balance Every Time Household Index is loaded.
                user.Household.Balance = hh.HouseholdBalance(user.Household);
                
                db.Entry(user.Household).State = EntityState.Modified;
                db.SaveChanges();
            }
            IndexVM.Household = user.Household;

            ViewBag.Sent = invite;
            ViewBag.HouseholdsError = error;
            return View(IndexVM);
        }

        //Banks Partial
        [DonutOutputCache(Duration = 0)]
        public PartialViewResult _Banks()
        {            
            var user = db.Users.Find(User.Identity.GetUserId());
            var household = user.Household;
            return PartialView(household);
        }

        //Categories Partial
        //[DonutOutputCache(Duration = 0)]
        public PartialViewResult _Categories()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var household = user.Household;
            return PartialView(household);
        }

        // GET: Households/Join
        public ActionResult Join()
        {
            return View();
        }

        //POST: Households/Join
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Join(string Email, string Password)
        {
            var invite = db.Invitations.FirstOrDefault(i => i.ToEmail == Email && i.Code == Password);
            var household = db.Households.FirstOrDefault(h=>h.Invitations.Any(i=>i.Id == invite.Id));
            var user = db.Users.Find(User.Identity.GetUserId());

            if (household != null)
            {
                invite.Accepted = true;
                household.Members.Add(user);
                user.Household = household;
                db.Entry(household).State = EntityState.Modified;
                db.Entry(invite).State = EntityState.Modified;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Households");
            }
            ViewBag.HouseholdsError = "There was an error joining that household. Please try again or create a household of your own.";
            return RedirectToAction("Index", "Households",new { error = ViewBag.HouseholdsError });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Leave(int HouseholdId)
        {
            var household = db.Households.Find(HouseholdId);
            var user = db.Users.Find(User.Identity.GetUserId());

            household.Members.Remove(user);
            user.Household = null;
            db.Entry(household).State = EntityState.Modified;
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Households");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Invite(EmailModel model, int HouseholdId)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(User.Identity.GetUserId());

                var invitation = ih.CreateInvitation(HouseholdId, model.ToEmail, user.Id);

                try
                {
                    var body = "<p>You have received an invitation to join Go Money from: <bold>{0}</bold>({1})</p><br><p>{2}</p>";
                    var from = "Go Money <do_not_reply@GoMoney.com>";
                    if (model.Body == null)
                    {
                        model.Body = "Please join my household!<br>The name of my household is: " + user.Household.Name + "<br>The password is: " + invitation.Code + "<br><br>If you are not already a user of Go Money, you will need to first register for an account. <br>Afterward registering, use the navigation bar to find the Household tab. Click the Join an existing household button. Simply put in <b>the email this invitation was sent to</b> and <b>the password</b> and you will be added to the household.<br><br><a href='http://kbartholomew-budgeter.azurewebsites.net' target='_blank'><button style='color: #fff; background-color: #337ab7; border-color: #2e6da4;display: inline-block; padding: 6px 12px; margin-bottom: 0; font-size: 14px; font-weight: normal; line-height: 1.42857143; text-align: center; white-space: nowrap; vertical-align: middle; -ms-touch-action: manipulation; touch-action: manipulation; cursor: pointer; -webkit-user-select: none; -moz-user-select: none; -ms-user-select: none; user-select: none; background-image: none; border: 1px solid transparent; border-radius: 4px;'>Click here</button></a> to go to Go Money!";
                    }
                    else
                    {
                        model.Body = model.Body + "<br>The name of my household is: " + user.Household.Name + "<br>The password is: " + invitation.Code + "<br><br>If you are not already a user of Go Money, you will need to first register for an account. <br>Afterward registering, use the navigation bar to find the Household tab. Click the Join an existing household button. Simply put in <b>the email this invitation was sent to</b> and <b>the password</b> and you will be added to the household.<br><br><a href='http://kbartholomew-budgeter.azurewebsites.net' target='_blank'><button style='color: #fff; background-color: #337ab7; border-color: #2e6da4;display: inline-block; padding: 6px 12px; margin-bottom: 0; font-size: 14px; font-weight: normal; line-height: 1.42857143; text-align: center; white-space: nowrap; vertical-align: middle; -ms-touch-action: manipulation; touch-action: manipulation; cursor: pointer; -webkit-user-select: none; -moz-user-select: none; -ms-user-select: none; user-select: none; background-image: none; border: 1px solid transparent; border-radius: 4px;'>Click here</button></a> to go to Go Money!";
                    }
                    var email = new MailMessage(from,
                         model.ToEmail)
                    {
                        Subject = "Invitation to Join Go Money",
                        Body = string.Format(body, user.FirstName + " " + user.LastName, user.Email,
                                      model.Body),
                        IsBodyHtml = true
                    };
                    var svc = new PersonalEmail();
                    await svc.SendAsync(email);
                    var household = db.Households.Find(HouseholdId);
                    household.Invitations.Add(invitation);
                    db.Entry(household).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.Sent = "Your invitation has been sent!";
                    return RedirectToAction("Index", new { invite = ViewBag.Sent });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }
            }
            ViewBag.Sent = "Your invitation DID NOT send correctly!";
            return RedirectToAction("Index",new { invite = ViewBag.Sent });
        }

        // GET: Households/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Household household = db.Households.Find(id);
        //    if (household == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(household);
        //}

        // GET: Households/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Households/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Balance,Created,Updated")] Household household)
        {
            if (ModelState.IsValid)
            {
                household.Created = DateTimeOffset.Now;
                household.Balance = 0;
                household.Members.Add(db.Users.Find(User.Identity.GetUserId()));
                household.Categories = db.Categories.Where(c => c.Default).ToList();
                db.Households.Add(household);
                db.Users.Find(User.Identity.GetUserId()).Household = household;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(household);
        }

        // GET: Households/Edit/5
        public ActionResult Edit(int? id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user.Household == null)
            {
                RedirectToAction("Index", "Households");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Balance,Created,Updated")] Household household)
        {
            if (ModelState.IsValid)
            {
                db.Entry(household).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(household);
        }

        // GET: Households/Delete/5
        public ActionResult Delete(int? id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user.Household == null)
            {
                RedirectToAction("Index", "Households");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Household household = db.Households.Find(id);
            db.Households.Remove(household);
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
