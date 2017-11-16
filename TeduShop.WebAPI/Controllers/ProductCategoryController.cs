using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using TeduShop.Model.Models;
using TeduShop.Service;
using TeduShop.Web.Infrastructure.Core;
using TeduShop.Web.Infrastructure.Extensions;
using TeduShop.Web.Models;

namespace TeduShop.Web.Controllers
{
    [RoutePrefix("api/productCategory")]
    [Authorize]
    public class ProductCategoryController : ApiControllerBase
    {
        #region Initialize

        private IProductCategoryService _productCategoryService;

        public ProductCategoryController(IErrorService errorService, IProductCategoryService productCategoryService)
            : base(errorService)
        {
            this._productCategoryService = productCategoryService;
        }

        #endregion Initialize

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string filter)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _productCategoryService.GetAll();

                var responseData = Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }
        [Route("getallhierachy")]
        [HttpGet]
        public HttpResponseMessage GetAllHierachy(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var response = request.CreateResponse(HttpStatusCode.OK, GetCategoryViewModel());
                return response;
            });
        }
        [Route("detail/{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _productCategoryService.GetById(id);

                var responseData = Mapper.Map<ProductCategory, ProductCategoryViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);

                return response;
            });
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _productCategoryService.GetAll(keyword);

                totalRow = model.Count();
                var query = model.OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(query);

                var paginationSet = new PaginationSet<ProductCategoryViewModel>()
                {
                    Items = responseData,
                    PageIndex = page,
                    TotalRows = totalRow,
                    PageSize = pageSize
                };
                var response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;
            });
        }

        [Route("add")]
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage Create(HttpRequestMessage request, ProductCategoryViewModel productCategoryVm)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var newProductCategory = new ProductCategory();
                    newProductCategory.UpdateProductCategory(productCategoryVm);
                    newProductCategory.CreatedDate = DateTime.Now;
                    _productCategoryService.Add(newProductCategory);
                    _productCategoryService.Save();

                    var responseData = Mapper.Map<ProductCategory, ProductCategoryViewModel>(newProductCategory);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("update")]
        [HttpPut]
        public HttpResponseMessage Update(HttpRequestMessage request, ProductCategoryViewModel productCategoryVm)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    if (productCategoryVm.ID == productCategoryVm.ParentID)
                    {
                        response = request.CreateResponse(HttpStatusCode.BadRequest, "Danh mục này không thể làm con chính nó.k");
                    }
                    else
                    {
                        var dbProductCategory = _productCategoryService.GetById(productCategoryVm.ID);

                        dbProductCategory.UpdateProductCategory(productCategoryVm);
                        dbProductCategory.UpdatedDate = DateTime.Now;

                        _productCategoryService.Update(dbProductCategory);
                        try
                        {
                            _productCategoryService.Save();
                        }
                        catch (DbEntityValidationException e)
                        {
                            foreach (var eve in e.EntityValidationErrors)
                            {
                                Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                                foreach (var ve in eve.ValidationErrors)
                                {
                                    Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                        ve.PropertyName, ve.ErrorMessage);
                                }
                            }
                            throw;
                        }
                        var responseData = Mapper.Map<ProductCategory, ProductCategoryViewModel>(dbProductCategory);
                        response = request.CreateResponse(HttpStatusCode.Created, responseData);
                    }

                }

                return response;
            });
        }

        [Route("delete")]
        [HttpDelete]
        [AllowAnonymous]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var oldProductCategory = _productCategoryService.Delete(id);
                    _productCategoryService.Save();

                    var responseData = Mapper.Map<ProductCategory, ProductCategoryViewModel>(oldProductCategory);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("deletemulti")]
        [HttpDelete]
        [AllowAnonymous]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedProductCategories)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var listProductCategory = new JavaScriptSerializer().Deserialize<List<int>>(checkedProductCategories);
                    foreach (var item in listProductCategory)
                    {
                        _productCategoryService.Delete(item);
                    }

                    _productCategoryService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, listProductCategory.Count);
                }

                return response;
            });
        }

        private List<ProductCategoryViewModel> GetCategoryViewModel(long? selectedParent = null)
        {
            List<ProductCategoryViewModel> items = new List<ProductCategoryViewModel>();

            //get all of them from DB
            var allCategorys = _productCategoryService.GetAll();
            //get parent categories
            IEnumerable<ProductCategory> parentCategorys = allCategorys.Where(c => c.ParentID == null);

            foreach (var cat in parentCategorys)
            {
                //add the parent category to the item list
                items.Add(new ProductCategoryViewModel
                {
                    ID = cat.ID,
                    Name = cat.Name,
                    DisplayOrder = cat.DisplayOrder,
                    Status = cat.Status,
                    CreatedDate = cat.CreatedDate
                });
                //now get all its children (separate Category in case you need recursion)
                GetSubTree(allCategorys.ToList(), cat, items);
            }
            return items;
        }
        private void GetSubTree(IList<ProductCategory> allCats, ProductCategory parent, IList<ProductCategoryViewModel> items)
        {
            var subCats = allCats.Where(c => c.ParentID == parent.ID);
            foreach (var cat in subCats)
            {
                //add this category
                items.Add(new ProductCategoryViewModel
                {
                    ID = cat.ID,
                    Name = parent.Name + " >> " + cat.Name,
                    DisplayOrder = cat.DisplayOrder,
                    Status = cat.Status,
                    CreatedDate = cat.CreatedDate
                });
                //recursive call in case your have a hierarchy more than 1 level deep
                GetSubTree(allCats, cat, items);
            }
        }
    }
}