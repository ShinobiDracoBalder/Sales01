namespace Sales01.Backend.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web;
    using Sales01.Domain.Models;

    [NotMapped]
    public class ProductView : Product
    {

        [Display(Name = "ImageFile")]
        public HttpPostedFileBase LifeLogo { get; set; }
    }
}