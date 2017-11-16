using System;
using System.Collections.Generic;
using TeduShop.Data.Infrastructure;
using TeduShop.Model.Models;
using System.Linq;
using System.Data.Entity;

namespace TeduShop.Data.Repositories
{
    public interface IAnnouncementRepository : IRepository<Announcement>
    {
        IQueryable<Announcement> GetAllUnread(string userId);
    }

    public class AnnouncementRepository : RepositoryBase<Announcement>, IAnnouncementRepository
    {
        public AnnouncementRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IQueryable<Announcement> GetAllUnread(string userId)
        {
            var query = (from x in DbContext.Announcements
                         join y in DbContext.AnnouncementUsers
                         on x.ID equals y.AnnouncementId
                         into xy
                         from y in xy.DefaultIfEmpty()
                         where (y.HasRead == null || y.HasRead==false)
                         && (y.UserId == null || y.UserId == userId)
                         select x).Include(x => x.AppUser);
            return query;
        }
    }
}