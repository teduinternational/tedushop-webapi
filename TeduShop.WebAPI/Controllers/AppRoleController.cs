using AutoMapper;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TeduShop.Common.Exceptions;
using TeduShop.Identity;
using TeduShop.Model.Models;
using TeduShop.Service;
using TeduShop.Web.App_Start;
using TeduShop.Web.Infrastructure.Core;
using TeduShop.Web.Infrastructure.Extensions;
using TeduShop.Web.Models;

namespace TeduShop.Web.Controllers
{
    [RoutePrefix("api/appRole")]
    [Authorize]
    public class AppRoleController : ApiControllerBase
    {
        private ApplicationRoleManager _roleManager;

        public AppRoleController(IErrorService errorService, ApplicationRoleManager roleManager) : base(errorService)
        {
            _roleManager = roleManager;
        }

        [Route("getlistpaging")]
        [HttpGet]
        public HttpResponseMessage GetListPaging(HttpRequestMessage request, int page, int pageSize, string filter = null)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                int totalRow = 0;
                var query = _roleManager.Roles;
                if (!string.IsNullOrEmpty(filter))
                    query = query.Where(x => x.Description.Contains(filter));
                totalRow = query.Count();

                var model = query.OrderBy(x => x.Name).Skip((page - 1) * pageSize).Take(pageSize);

                IEnumerable<ApplicationRoleViewModel> modelVm = Mapper.Map<IEnumerable<AppRole>, IEnumerable<ApplicationRoleViewModel>>(model);

                PaginationSet<ApplicationRoleViewModel> pagedSet = new PaginationSet<ApplicationRoleViewModel>()
                {
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize),
                    Items = modelVm
                };

                response = request.CreateResponse(HttpStatusCode.OK, pagedSet);

                return response;
            });
        }

        [Route("getlistall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var model = _roleManager.Roles.ToList();
                IEnumerable<ApplicationRoleViewModel> modelVm = Mapper.Map<IEnumerable<AppRole>, IEnumerable<ApplicationRoleViewModel>>(model);

                response = request.CreateResponse(HttpStatusCode.OK, modelVm);

                return response;
            });
        }

        [Route("detail/{id}")]
        [HttpGet]
        public HttpResponseMessage Details(HttpRequestMessage request, string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(id) + " không có giá trị.");
            }
            AppRole appRole = _roleManager.FindById(id);
            if (appRole == null)
            {
                return request.CreateErrorResponse(HttpStatusCode.NoContent, "No group");
            }
            return request.CreateResponse(HttpStatusCode.OK, appRole);
        }

        [HttpPost]
        [Route("add")]
        public HttpResponseMessage Create(HttpRequestMessage request, ApplicationRoleViewModel applicationRoleViewModel)
        {
            if (ModelState.IsValid)
            {
                var newAppRole = new AppRole();
                newAppRole.UpdateApplicationRole(applicationRoleViewModel);
                try
                {
                    _roleManager.Create(newAppRole);
                    return request.CreateResponse(HttpStatusCode.OK, applicationRoleViewModel);
                }
                catch (NameDuplicatedException dex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, dex.Message);
                }
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [HttpPut]
        [Route("update")]
        public HttpResponseMessage Update(HttpRequestMessage request, ApplicationRoleViewModel applicationRoleViewModel)
        {
            if (ModelState.IsValid)
            {
                var appRole = _roleManager.FindById(applicationRoleViewModel.Id);
                try
                {
                    appRole.UpdateApplicationRole(applicationRoleViewModel, "update");
                    _roleManager.Update(appRole);
                    return request.CreateResponse(HttpStatusCode.OK, appRole);
                }
                catch (NameDuplicatedException dex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, dex.Message);
                }
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [HttpDelete]
        [Route("delete")]
        public HttpResponseMessage Delete(HttpRequestMessage request, string id)
        {
            var appRole = _roleManager.FindById(id);

            _roleManager.Delete(appRole);
            return request.CreateResponse(HttpStatusCode.OK, id);
        }
    }
}