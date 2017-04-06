using Microsoft.AspNet.Identity.EntityFramework;
using TeduShop.Data;
using TeduShop.Model.Models;

namespace TeduShop.Identity
{
    public class ApplicationUserStore : UserStore<AppUser>
    {
        public ApplicationUserStore(TeduShopDbContext context)
            : base(context)
        {
        }
    }
}