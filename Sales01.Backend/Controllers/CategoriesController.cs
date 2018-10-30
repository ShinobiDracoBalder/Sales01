namespace Sales01.Backend.Controllers
{
    using System;
    using System.Data.Entity;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using Backend.Models;
    using Domain.Models;
    using Sales01.Backend.Helpers;

    public class CategoriesController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: Categories
        public async Task<ActionResult> Index()
        {
            return View(await db.Categories.ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CategoryView view)
        {
            if (ModelState.IsValid)
            {
                var category = this.ToCategories(view);
                using (var transaction = db.Database.BeginTransaction())
                {
                    db.Categories.Add(category);
                    try
                    {
                        await db.SaveChangesAsync();

                        var pic = string.Empty;
                        var folder = "~/Content/Categories";

                        if (view.ImageFile != null)
                        {
                            pic = FilesHelper.UploadPhoto(view.ImageFile, folder, string.Format("{0}", category.Description), string.Format("{0}", category.CategoryId));

                            category.ImageMimeType = view.ImageFile.ContentType;
                            int length = view.ImageFile.ContentLength;
                            byte[] buffer = new byte[length];
                            view.ImageFile.InputStream.Read(buffer, 0, length);
                            category.ImagenCategory = buffer;
                        }

                        if (!string.IsNullOrEmpty(pic))
                        {
                            category.ImagePath = pic;
                            db.Entry(category).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                        }

                        transaction.Commit();

                        return RedirectToAction("Index");

                    }
                    catch (Exception ex)
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

            return View(view);
        }

        private Category ToCategories(CategoryView view)
        {
            return new Category {
                Description = view.Description,
                ImagePath = view.ImagePath,
                ImageMimeType = view.ImageMimeType,
                ImagenCategory = view.ImagenCategory,
            };
        }

        // GET: Categories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            var view = ToViewCategories(category);
            return View(view);
        }

        private CategoryView ToViewCategories(Category category)
        {
            return new CategoryView {
                Description= category.Description,
                CategoryId = category.CategoryId,
                ImageMimeType = category.ImageMimeType,
                ImagenCategory = category.ImagenCategory,
                ImagePath = category.ImagePath,
            };
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CategoryView view)
        {
            var category = ToCategorys(view);

            if (ModelState.IsValid)
            {
                using (var transaction = db.Database.BeginTransaction())
                {

                    try
                    {
                        var pic = string.Empty;
                        var folder = "~/Content/Categories";

                        if (view.ImageFile != null)
                        {
                            pic = FilesHelper.UploadPhoto(view.ImageFile, folder, string.Format("{0}", category.Description), string.Format("{0}", category.CategoryId));

                            category.ImageMimeType = view.ImageFile.ContentType;
                            int length = view.ImageFile.ContentLength;
                            byte[] buffer = new byte[length];
                            view.ImageFile.InputStream.Read(buffer, 0, length);
                            category.ImagenCategory = buffer;
                        }

                        db.Entry(category).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        transaction.Commit();
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
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
            return View(category);
        }

        private Category ToCategorys(CategoryView view)
        {
            return new Category {
                Description = view.Description,
                CategoryId = view.CategoryId,
            };
        }

        // GET: Categories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            try
            {
                db.Categories.Remove(category);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
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
            }
            return RedirectToAction("Index");
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Category category = await db.Categories.FindAsync(id);
            db.Categories.Remove(category);
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
