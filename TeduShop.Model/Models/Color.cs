using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeduShop.Model.Models
{
    [Table("Colors")]
    public class Color
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(250)]
        public string Name
        {
            get; set;
        }

        [StringLength(250)]
        public string Code { get; set; }
    }
}