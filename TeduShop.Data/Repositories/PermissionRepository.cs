using TeduShop.Data.Infrastructure;
using TeduShop.Model.Models;

namespace TeduShop.Data.Repositories
{
    public interface IPermissionRepository : IRepository<Permission>
    {
    }

    public class PermissionRepository : RepositoryBase<Permission>, IPermissionRepository
    {
        public PermissionRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}