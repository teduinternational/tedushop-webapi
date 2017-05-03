using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeduShop.Model.Models
{
    [Table("ProductSizes")]
   public class ProductSize
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int ProductId { get; set; }

        public int SizeId { get; set; }

        public int Quantity { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        [ForeignKey("SizeId")]
        public virtual Size Size { get; set; }
    }
}
