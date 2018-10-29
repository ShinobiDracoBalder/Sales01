namespace Sales01.Domain.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "The field {0} is requiered.")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contains a maximum of {1} characters lenght.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "The field {0} is requiered.")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contains a maximum of {1} characters lenght.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The field {0} is requiered.")]
        [MaxLength(100, ErrorMessage = "The field {0} only can contains a maximum of {1} characters lenght.")]
        [Index("User_UserName_Index", IsUnique = true)]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [Display(Name = "NickName")]
        [Required(ErrorMessage = "The field {0} is requiered.")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contains a maximum of {1} characters lenght.")]
        public string NickName { get; set; }

        [Required(ErrorMessage = "Please enter the field {0} is required..")]
        [MaxLength(20, ErrorMessage = "The field {0} only can contains a maximum of {1} characters lenght.")]
        [DataType(DataType.PhoneNumber)]
        public string Telephone { get; set; }

        [Required(ErrorMessage = "Please enter the field {0} is required..")]
        [Display(Name = "Cell Phone")]
        [MaxLength(20, ErrorMessage = "The field {0} must be maximun {1} characters length")]
        [DataType(DataType.PhoneNumber)]
        public string Cellphone { get; set; }

        [Required(ErrorMessage = "Please enter the field {0} is required..")]
        [Display(Name = "Address")]
        [MaxLength(100, ErrorMessage = "The field {0} must be maximun {1} characters length")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please enter the field {0} is required..")]
        [Display(Name = "Postal Code")]
        [MaxLength(20, ErrorMessage = "The field {0} must be maximun {1} characters length")]
        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }

        [Display(Name = "Image")]
        public string ImagePath { get; set; }

        public string ImageMimeType { get; set; }

        public byte[] ImagenUser { get; set; }

        public int UserTypeId { get; set; }

        [JsonIgnore]
        public virtual UserType UserType { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        [NotMapped]
        public byte[] ImageArray { get; set; }

        [NotMapped]
        public string Password { get; set; }

        [Display(Name = "Image")]
        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(ImagePath))
                {
                    return "noimage";
                }

                if (this.UserTypeId == 1)
                {
                    return string.Format(
                        "http://russiaapi.azurewebsites.net{0}",
                        ImagePath.Substring(1));
                }

                return this.ImagePath;
            }
        }

        [Display(Name = "User")]
        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", this.FirstName, this.LastName);
            }
        }
    }
}
