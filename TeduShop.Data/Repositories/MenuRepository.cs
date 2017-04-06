using TeduShop.Data.Infrastructure;
using TeduShop.Model.Models;

namespace TeduShop.Data.Repositories
{
    public interface IMenuRepository : IRepository<Function>
    {
    }

    public class MenuRepository : RepositoryBase<Function>, IMenuRepository
    {
        public MenuRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}