using System;

namespace TeduShop.Web.Models
{
    [Serializable]
    public class ProductViewModel
    {
        public int ID { set; get; }

        public string Name { set; get; }

        public string Alias { set; get; }

        public int CategoryID { set; get; }

        public string ThumbnailImage { set; get; }

        public decimal Price { set; get; }

        public decimal? PromotionPrice { set; get; }

        public bool IncludedVAT { get; set; }


        public int? Warranty { set; get; }

        public string Description { set; get; }

        public string Content { set; get; }

        public bool? HomeFlag { set; get; }

        public bool? HotFlag { set; get; }

        public int? ViewCount { set; get; }

        public DateTime? CreatedDate { set; get; }

        public string CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }

        public string UpdatedBy { set; get; }

        public string MetaKeyword { set; get; }

        public string MetaDescription { set; get; }

        public bool Status { set; get; }

        public string Tags { set; get; }

        public bool Checked { set; get; }


        public decimal OriginalPrice { set; get; }
        public virtual ProductCategoryViewModel ProductCategory { set; get; }
    }
}