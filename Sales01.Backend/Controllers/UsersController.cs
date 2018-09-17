namespace Sales01.Backend.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using Sales01.Backend.Models;
    using Sales01.Domain.Models;
    using Sales01.Common.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Sales01.Backend.Helpers;

    [Authorize(Roles = RolesHelper.ADMIN)]
    public class UsersController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        public ActionResult OnOffAdmin(int id)
        {

            var user = db.Users.Find(id);

            if (user != null)
            {
                var userContext = new ApplicationDbContext();
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));

                var userASP = userManager.FindByEmail(user.UserName);

                if (userASP != null)
                {
                    if (userManager.IsInRole(userASP.Id, RolesHelper.ADMIN))
                    {
                        userManager.RemoveFromRole(userASP.Id, RolesHelper.ADMIN);
                    }
                    else
                    {
                        userManager.AddToRole(userASP.Id, RolesHelper.ADMIN);
                    }
                }
            }

            return RedirectToAction("Index");
        }
        // GET: Users
        public async Task<ActionResult> Index()
        {
            var users = db.Users.Include(u => u.UserType);
            return View(await users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.UserTypeId = new SelectList(ComboHelper.GetUserTypeId(), "UserTypeId", "Description");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserView view)
        {
            if (ModelState.IsValid)
            {
                var user = this.ToUsers(view);
                using (var transaction = db.Database.BeginTransaction())
                {
                    db.Users.Add(user);
                    try
                    {
                        await db.SaveChangesAsync();


                        UsersHelper.CreateUserASP(view.UserName, RolesHelper.PowerUser, view.Passwords);

                        var pic = string.Empty;
                        var folder = "~/Content/Users";

                        if (view.LifeLogo != null)
                        {
                            pic = FilesHelper.UploadPhoto(view.LifeLogo, folder, string.Format("{0}", user.FirstName), string.Format("{0}", user.UserId));

                            user.ImageMimeType = view.LifeLogo.ContentType;
                            int length = view.LifeLogo.ContentLength;
                            byte[] buffer = new byte[length];
                            view.LifeLogo.InputStream.Read(buffer, 0, length);
                            user.ImagenUser = buffer;
                        }

                        if (!string.IsNullOrEmpty(pic))
                        {
                            user.ImagePath = pic;
                            db.Entry(user).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                        }

                        transaction.Commit();

                        return RedirectToAction("Index");
                    }
                    catch (System.Exception ex)
                    {
                        transaction.Rollback();
                        if (ex.InnerException != null &&
                        ex.InnerException.InnerException != null &&
                        ex.InnerException.InnerException.Message.Contains("_Index") &&
                        ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                        {
                            ModelState.AddModelError(string.Empty, "There are a record with the same value");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, ex.Message);

                            string message = string.Format("<b>Message:</b> {0}<br /><br />", ex.Message);
                            message += string.Format("<b>StackTrace:</b> {0}<br /><br />", ex.StackTrace.Replace(Environment.NewLine, string.Empty));
                            message += string.Format("<b>Source:</b> {0}<br /><br />", ex.Source.Replace(Environment.NewLine, string.Empty));
                            message += string.Format("<b>TargetSite:</b> {0}", ex.TargetSite.ToString().Replace(Environment.NewLine, string.Empty));
                            ModelState.AddModelError(string.Empty, message);
                        }
                    }
                }
            }

            ViewBag.UserTypeId = new SelectList(ComboHelper.GetUserTypeId(), "UserTypeId", "Description", view.UserTypeId);
            return View(view);
        }

        private User ToUsers(UserView view)
        {
            return new User
            {
                FirstName = view.FirstName,
                LastName = view.LastName,
                Address = view.Address,
                Telephone = view.Telephone,
                Cellphone = view.Cellphone,
                PostalCode = view.PostalCode,
                UserName = view.UserName,
                NickName = view.NickName,
                UserTypeId = view.UserTypeId,
            };
        }

        // GET: Users/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserTypeId = new SelectList(ComboHelper.GetUserTypeId(), "UserTypeId", "Description", user.UserTypeId);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.UserTypeId = new SelectList(ComboHelper.GetUserTypeId(), "UserTypeId", "Description", user.UserTypeId);
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            using (var transaction = db.Database.BeginTransaction())
            {

                db.Users.Remove(user);
                try
                {
                    await db.SaveChangesAsync();

                    if (UsersHelper.DeleteUser(user.UserName, RolesHelper.PowerUser))
                    {
                        transaction.Commit();
                    }

                    transaction.Commit();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    if (ex.InnerException != null &&
                        ex.InnerException.InnerException != null &&
                        ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                    {
                        ModelState.AddModelError(string.Empty, "The record can't be delete because it has related records");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);

                        string message = string.Format("<b>Message:</b> {0}<br /><br />", ex.Message);
                        message += string.Format("<b>StackTrace:</b> {0}<br /><br />", ex.StackTrace.Replace(Environment.NewLine, string.Empty));
                        message += string.Format("<b>Source:</b> {0}<br /><br />", ex.Source.Replace(Environment.NewLine, string.Empty));
                        message += string.Format("<b>TargetSite:</b> {0}", ex.TargetSite.ToString().Replace(Environment.NewLine, string.Empty));
                        ModelState.AddModelError(string.Empty, message);

                    }
                    return RedirectToAction("Index");
                }
            }
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            User user = await db.Users.FindAsync(id);
            db.Users.Remove(user);
            await db.SaveChangesAsync();
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
