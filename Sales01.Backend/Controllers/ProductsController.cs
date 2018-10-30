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
    using Sales01.Backend.Helpers;

    [Authorize(Roles = RolesHelper.PowerUser)]
    public class ProductsController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: Products
        public async Task<ActionResult> Index()
        {
            return View(await this.db.Products
                .Include(p => p.User)
                .Include(p => p.Category)
                .OrderBy(p => p.BarCode).ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await this.db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            var user = db
               .Users
               .Where(u => u.UserName == User.Identity.Name)
               .FirstOrDefault();

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var view = new ProductView {
                UserId = user.UserId,
            };

            ViewBag.CategoryId = new SelectList(ComboHelper.GetCategories(), "CategoryId", "Description");
            return View(view);
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductView view)
        {
            if (ModelState.IsValid)
            {
                var product = this.ToView(view);

                using (var transaction = this.db.Database.BeginTransaction())
                {

                    this.db.Products.Add(product);
                    try
                    {
                        await this.db.SaveChangesAsync();

                        view.ProductId = product.ProductId;

                        var pic = string.Empty;
                        var folder = "~/Content/Products";

                        if (view.LifeLogo != null)
                        {
                            pic = FilesHelper.UploadPhoto(view.LifeLogo, folder, string.Format("{0}", product.BarCode), string.Format("{0}", product.ProductId));

                            product.ImageMimeType = view.LifeLogo.ContentType;
                            int length = view.LifeLogo.ContentLength;
                            byte[] buffer = new byte[length];
                            view.LifeLogo.InputStream.Read(buffer, 0, length);
                            product.ImagenProduct = buffer;
                        }

                        if (!string.IsNullOrEmpty(pic))
                        {
                            product.ImagePath = pic;
                            this.db.Entry(product).State = EntityState.Modified;
                            await this.db.SaveChangesAsync();
                        }


                        transaction.Commit();
                        return RedirectToAction("Index");
                    }
                    catch (System.Exception ex)
                    {
                        transaction.Rollback();
                        if (ex.InnerException != null &&
                        ex.InnerException.InnerException != null &&
                        ex.InnerException.InnerException.Message.Contains("_Index"))
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

        private Product ToView(ProductView view)
        {
            return new Product
            {
                Description = view.Description,
                BarCode = view.BarCode,
                Price = view.Price,
                Remarks = view.Remarks,
                IsAvailable = view.IsAvailable,
                PublishOn = view.PublishOn,
                UserId = view.UserId,
                CategoryId = view.CategoryId,
            };
        }

        // GET: Products/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await this.db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            var ViewProduct = this.ToProduct(product);
            ViewBag.CategoryId = new SelectList(ComboHelper.GetCategories(), "CategoryId", "Description",product.CategoryId);
            return View(ViewProduct);
        }

        private ProductView ToProduct(Product product)
        {
            return new ProductView
            {
                BarCode = product.BarCode,
                Description = product.Description,
                ImagePath = product.ImagePath,
                ImageMimeType = product.ImageMimeType,
                ImagenProduct = product.ImagenProduct,
                Price = product.Price,
                Remarks = product.Remarks,
                ProductId = product.ProductId,
                IsAvailable = product.IsAvailable,
                PublishOn = product.PublishOn,
                UserId = product.UserId,
                CategoryId = product.CategoryId,
            };
        }
        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProductView view)
        {
            if (ModelState.IsValid)
            {
                var product = this.ToProductView(view);

                var pic = view.ImagePath;
                var folder = "~/Content/Products";


                if (view.LifeLogo != null)
                {
                    pic = FilesHelper.UploadPhoto(view.LifeLogo, folder, string.Format("{0}", product.BarCode), string.Format("{0}", product.ProductId));

                    view.ImageMimeType = view.LifeLogo.ContentType;
                    int length = view.LifeLogo.ContentLength;
                    byte[] buffer = new byte[length];
                    view.LifeLogo.InputStream.Read(buffer, 0, length);
                    view.ImagenProduct = buffer;
                    product.ImagenProduct = buffer;
                }
                using (var transaction = this.db.Database.BeginTransaction())
                {
                    this.db.Entry(product).State = EntityState.Modified;
                    try
                    {
                        await this.db.SaveChangesAsync();
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        if (ex.InnerException != null &&
                        ex.InnerException.InnerException != null &&
                        ex.InnerException.InnerException.Message.Contains("_Index"))
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
            ViewBag.CategoryId = new SelectList(ComboHelper.GetCategories(), "CategoryId", "Description", view.CategoryId);
            return View(view);
        }

        private Product ToProductView(ProductView view)
        {
            return new Product
            {
                BarCode = view.BarCode,
                Description = view.Description,
                ImagePath = view.ImagePath,
                ImageMimeType = view.ImageMimeType,
                ImagenProduct = view.ImagenProduct,
                Price = view.Price,
                Remarks = view.Remarks,
                ProductId = view.ProductId,
                IsAvailable = view.IsAvailable,
                PublishOn = view.PublishOn,
                UserId = view.UserId,
                CategoryId = view.CategoryId,
            };
        }

        // GET: Products/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await this.db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            using (var transaction = this.db.Database.BeginTransaction())
            {
                this.db.Products.Remove(product);
                try
                {
                    await this.db.SaveChangesAsync();
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

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Product product = await this.db.Products.FindAsync(id);
            this.db.Products.Remove(product);
            await this.db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
