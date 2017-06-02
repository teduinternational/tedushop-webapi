namespace TeduShop.Web.Models
{
    public class OrderDetailViewModel
    {
        public int OrderID { set; get; }

        public int ProductID { set; get; }

        public int Quantity { set; get; }

        public int Price { set; get; }
        public int ColorId { get; set; }
        public int SizeId { get; set; }

        public ProductViewModel Product { get; set; }
        public ColorViewModel Color { get; set; }
        public SizeViewModel Size { get; set; }
    }
}