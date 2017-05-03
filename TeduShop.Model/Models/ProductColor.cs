using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeduShop.Model.Models
{
    [Table("ProductColors")]
    public class ProductColor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int ProductId { get; set; }

        public int ColorId { get; set; }

        public int Quantity { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        [ForeignKey("ColorId")]
        public virtual Color Color { get; set; }
    }
}
