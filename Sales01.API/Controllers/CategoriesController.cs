namespace Sales01.API.Controllers
{
    using Sales01.Domain.Models;
    using System.Linq;
    using System.Web.Http;

    
    [RoutePrefix("api/Categories")]
    public class CategoriesController : ApiController
    {
        private DataContext db = new DataContext();

        [HttpGet]
       // [Authorize]
        public IQueryable<Category> GetCategories()
        {
            return db.Categories.OrderBy(c => c.Description);
        }        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
