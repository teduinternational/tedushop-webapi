using System.Web.Http;
using TeduShop.Service;
using TeduShop.Web.Infrastructure.Core;

namespace TeduShop.Web.Controllers
{
    [RoutePrefix("api/home")]
    [Authorize]
    public class HomeController : ApiControllerBase
    {
        private IErrorService _errorService;

        public HomeController(IErrorService errorService) : base(errorService)
        {
            this._errorService = errorService;
        }
    }
}