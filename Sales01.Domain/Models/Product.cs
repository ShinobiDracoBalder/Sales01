﻿namespace Sales01.Domain.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [StringLength(50)]
        [Index("ProductDescriptionIndex", IsUnique = true)]
        public string Description { get; set; }

        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        [Display(Name = "Image")]
        public string ImagePath { get; set; }

        public string ImageMimeType { get; set; }

        public byte[] ImagenProduct { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Range(0, double.MaxValue, ErrorMessage = "You must select a {0}between {1} and {2}")]
        public Decimal Price { get; set; }

        [Display(Name = "Is Available")]
        public bool IsAvailable { get; set; }

        [Display(Name = "Publish On")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PublishOn { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        [NotMapped]
        public byte[] ImageArray { get; set; }

        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(this.ImagePath))
                {
                    return "noproduct";
                }

                return $"https://salesapiservices.azurewebsites.net/{this.ImagePath.Substring(1)}";
            }

        }

        [Required]
        [StringLength(50)]
        public string BarCode { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Range(0, double.MaxValue, ErrorMessage = "You must select a {0}between {1} and {2}")]
        public decimal Quantity { get; set; }

        [DataType(DataType.MultilineText)]
        public string Characteristics { get; set; }

        [DataType(DataType.MultilineText)]
        public string Ingredients { get; set; }
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }

        public override string ToString()
        {
            return this.Description;
        }
    }
}
