using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeduShop.Model.Models
{
    [Table("AppRoles")]
    public class AppRole : IdentityRole
    {
        public AppRole() : base()
        {
        }

        [StringLength(250)]
        public string Description { set; get; }
    }
}