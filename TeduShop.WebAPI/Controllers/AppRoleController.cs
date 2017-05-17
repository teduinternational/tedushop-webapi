using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TeduShop.Common.Exceptions;
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
        private IPermissionService _permissionService;
        public AppRoleController(IErrorService errorService, IPermissionService permissionService) : base(errorService)
        {
            _permissionService = permissionService;
        }

        [Route("getlistpaging")]
        [HttpGet]
        public HttpResponseMessage GetListPaging(HttpRequestMessage request, int page, int pageSize, string filter = null)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                int totalRow = 0;
                var query = AppRoleManager.Roles;
                if (!string.IsNullOrEmpty(filter))
                    query = query.Where(x => x.Description.Contains(filter));
                totalRow = query.Count();

                var model = query.OrderBy(x => x.Name).Skip((page - 1) * pageSize).Take(pageSize);

                IEnumerable<ApplicationRoleViewModel> modelVm = Mapper.Map<IEnumerable<AppRole>, IEnumerable<ApplicationRoleViewModel>>(model);

                PaginationSet<ApplicationRoleViewModel> pagedSet = new PaginationSet<ApplicationRoleViewModel>()
                {
                    PageIndex = page,
                    TotalRows = totalRow,
                    PageSize = pageSize,
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
                var model = AppRoleManager.Roles.ToList();
                IEnumerable<ApplicationRoleViewModel> modelVm = Mapper.Map<IEnumerable<AppRole>, IEnumerable<ApplicationRoleViewModel>>(model);

                response = request.CreateResponse(HttpStatusCode.OK, modelVm);

                return response;
            });
        }
        [Route("getAllPermission")]
        [HttpGet]
        public HttpResponseMessage GetAllPermission(HttpRequestMessage request, string functionId)
        {
            return CreateHttpResponse(request, () =>
            {
                ICollection<PermissionViewModel> permissions = new List<PermissionViewModel>();
                HttpResponseMessage response = null;
                var roles = AppRoleManager.Roles.ToList();
                var listPermission = _permissionService.GetByFunctionId(functionId);
                if (listPermission.Count == 0)
                {
                    foreach (var item in roles)
                    {
                        permissions.Add(new PermissionViewModel()
                        {
                            RoleId = item.Id,
                            CanCreate = false,
                            CanDelete = false,
                            CanRead = false,
                            CanUpdate = false,
                            AppRole = new ApplicationRoleViewModel()
                            {
                                Id = item.Id,
                                Description = item.Description,
                                Name = item.Name
                            }
                        });
                    }
                }
                else
                {
                    foreach (var item in roles)
                    {
                        if (!permissions.Any(x => x.RoleId == item.Id))
                        {
                            permissions.Add(new PermissionViewModel()
                            {
                                RoleId = item.Id,
                                CanCreate = false,
                                CanDelete = false,
                                CanRead = false,
                                CanUpdate = false,
                                AppRole = new ApplicationRoleViewModel()
                                {
                                    Id = item.Id,
                                    Description = item.Description,
                                    Name = item.Name
                                }
                            });
                        }
                    }
                }
                response = request.CreateResponse(HttpStatusCode.OK, permissions);

                return response;
            });
        }

        [HttpPost]
        [Route("savePermission")]
        public HttpResponseMessage SavePermission(HttpRequestMessage request, List<PermissionViewModel> permissions, string functionId)
        {
            if (ModelState.IsValid)
            {

                var permission = new Permission();
                _permissionService.DeleteAll(functionId);
                foreach (var item in permissions)
                {
                    permission.UpdatePermission(item);
                    permission.FunctionId = functionId;
                    _permissionService.Add(permission);
                }
                try
                {
                    _permissionService.SaveChange();
                    return request.CreateResponse(HttpStatusCode.OK, "Lưu quyền thành cống");
                }
                catch (Exception ex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
                }
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [Route("detail/{id}")]
        [HttpGet]
        public HttpResponseMessage Details(HttpRequestMessage request, string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(id) + " không có giá trị.");
            }
            AppRole appRole = AppRoleManager.FindById(id);
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
                    AppRoleManager.Create(newAppRole);
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
                var appRole = AppRoleManager.FindById(applicationRoleViewModel.Id);
                try
                {
                    appRole.UpdateApplicationRole(applicationRoleViewModel, "update");
                    AppRoleManager.Update(appRole);
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
            var appRole = AppRoleManager.FindById(id);

            AppRoleManager.Delete(appRole);
            return request.CreateResponse(HttpStatusCode.OK, id);
        }
    }
}