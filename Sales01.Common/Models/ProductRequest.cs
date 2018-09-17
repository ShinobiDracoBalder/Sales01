﻿namespace Sales01.Common.Models
{
    using System;

    public class ProductRequest
    {
        
        public int ProductId { get; set; }

        
        public string Description { get; set; }

        
        public string Remarks { get; set; }

        
        public string ImagePath { get; set; }

        public string ImageMimeType { get; set; }

        public byte[] ImagenProduct { get; set; }

        public Decimal Price { get; set; }

        public bool IsAvailable { get; set; }

        public DateTime PublishOn { get; set; }

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

        public string BarCode { get; set; }

        public override string ToString()
        {
            return this.Description;
        }
    }
}
