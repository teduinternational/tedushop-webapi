using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeduShop.Web.Models
{
    public class ProductQuantityViewModel
    {

        public int ProductId { get; set; }

        public int SizeId { get; set; }


        public int ColorId { get; set; }

        public int Quantity { get; set; }

        public  ProductViewModel Product { get; set; }

        public virtual SizeViewModel Size { get; set; }

        public virtual ColorViewModel Color { get; set; }
    }
}