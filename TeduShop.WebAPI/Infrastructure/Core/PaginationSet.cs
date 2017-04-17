using System.Collections.Generic;
using System.Linq;

namespace TeduShop.Web.Infrastructure.Core
{
    public class PaginationSet<T>
    {
        public int PageIndex { set; get; }

        public int TotalPages { set; get; }
        public int TotalRows { set; get; }
        public IEnumerable<T> Items { set; get; }
    }
}