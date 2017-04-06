using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeduShop.Model.Models
{
    [Table("Functions")]
    public class Function
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }

        [Required]
        [MaxLength(50)]
        public string Name { set; get; }

        [Required]
        [MaxLength(256)]
        public string URL { set; get; }

        public int DisplayOrder { set; get; }

        [Required]
        public int ParentId { set; get; }

        [ForeignKey("ParentId")]
        public virtual Function Parent { set; get; }

        [MaxLength(10)]
        public string Target { set; get; }

        public bool Status { set; get; }
    }
}