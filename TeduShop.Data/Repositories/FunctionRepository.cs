using System.Collections.Generic;
using System.Linq;
using TeduShop.Data.Infrastructure;
using TeduShop.Model.Models;

namespace TeduShop.Data.Repositories
{
    public interface IFunctionRepository : IRepository<Function>
    {
        List<Function> GetListFunctionWithPermission(string userId);
    }

    public class FunctionRepository : RepositoryBase<Function>, IFunctionRepository
    {
        public FunctionRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        /// <summary>
        /// Get list permission follow by permission
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Function> GetListFunctionWithPermission(string userId)
        {
            var query = (from f in DbContext.Functions
                         join p in DbContext.Permissions on f.ID equals p.FunctionId
                         join r in DbContext.AppRoles on p.RoleId equals r.Id
                         join ur in DbContext.UserRoles on r.Id equals ur.RoleId
                         join u in DbContext.Users on ur.UserId equals u.Id
                         where u.Id == userId && (p.CanRead == true)
                         select f);
            var parentIds = query.Select(x => x.ParentId).Distinct();
            query = query.Union(DbContext.Functions.Where(f => parentIds.Contains(f.ID)));

            return query.ToList();
        }
    }
}