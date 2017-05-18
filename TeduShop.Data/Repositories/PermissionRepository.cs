using System;
using System.Collections.Generic;
using TeduShop.Data.Infrastructure;
using TeduShop.Model.Models;
using System.Linq;

namespace TeduShop.Data.Repositories
{
    public interface IPermissionRepository : IRepository<Permission>
    {
        List<Permission> GetByUserId(string userId);
    }

    public class PermissionRepository : RepositoryBase<Permission>, IPermissionRepository
    {
        public PermissionRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public List<Permission> GetByUserId(string userId)
        {
            var query = from f in DbContext.Functions
                        join p in DbContext.Permissions on f.ID equals p.FunctionId
                        join r in DbContext.AppRoles on p.RoleId equals r.Id
                        join ur in DbContext.UserRoles on r.Id equals ur.RoleId
                        join u in DbContext.Users on ur.UserId equals u.Id
                        where u.Id == userId
                        select p;
            return query.ToList();
        }
    }
}