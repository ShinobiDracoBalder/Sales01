using Newtonsoft.Json;
using System.Collections.Generic;

namespace Sales01.Common.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
       
        public string Description { get; set; }
      
        public string ImagePath { get; set; }
        public string ImageMimeType { get; set; }

        public byte[] ImagenCategory { get; set; }

        //[JsonIgnore]
        //public virtual ICollection<ProductRequest> Products { get; set; }
        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(this.ImagePath))
                {
                    return "noproduct";
                }
                return $"https://salesbackend.azurewebsites.net{this.ImagePath.Substring(1)}";
            }
        }
    }
}
