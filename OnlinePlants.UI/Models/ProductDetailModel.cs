using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlinePlants.UI.Models
{
    public class ProductDetailModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public decimal ShippingCharge { get; set; }
        public bool FreeShipping { get; set; }
        public string Sku { get; set; }
        public string ProductType { get; set; }
        public string Weight { get; set; }
        public string Size { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public string Packaging { get; set; }
        public string MfgIn { get; set; }
        public string MfgBy { get; set; }
        public string ProductPicture { get; set; }
    }
}