using AutoMapper;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TeduShop.Model.Models;
using TeduShop.Service;
using TeduShop.Web.Infrastructure.Core;
using TeduShop.Web.Infrastructure.Extensions;
using TeduShop.Web.Models;

namespace TeduShop.Web.Controllers
{
    [RoutePrefix("api/productQuantity")]
    public class ProductQuantityController : ApiControllerBase
    {
        private IProductQuantityService _productQuantityService;

        public ProductQuantityController(IProductQuantityService productQuantityService,
            IErrorService errorService) : base(errorService)
        {
            _productQuantityService = productQuantityService;
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, int productId, int? sizeId, int? colorId)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var model = _productQuantityService.GetAll(productId, sizeId, colorId);

                IEnumerable<ProductQuantityViewModel> modelVm = Mapper.Map<IEnumerable<ProductQuantity>, IEnumerable<ProductQuantityViewModel>>(model);

                response = request.CreateResponse(HttpStatusCode.OK, modelVm);

                return response;
            });
        }

        [HttpPost]
        [Route("add")]
        public HttpResponseMessage Create(HttpRequestMessage request, ProductQuantityViewModel productQuantityVm)
        {
            if (ModelState.IsValid)
            {
                var newQuantity = new ProductQuantity();
                try
                {
                    if (_productQuantityService.CheckExist(productQuantityVm.ProductId, productQuantityVm.SizeId, productQuantityVm.ColorId))
                    {
                        return request.CreateErrorResponse(HttpStatusCode.BadRequest, "Màu sắc kích thước cho sản phẩm này đã tồn tại");
                    }
                    else
                    {
                        newQuantity.UpdateProductQuantity(productQuantityVm);

                        _productQuantityService.Add(newQuantity);
                        _productQuantityService.Save();
                        return request.CreateResponse(HttpStatusCode.OK, productQuantityVm);
                    }
                }
                catch (Exception dex)
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
        public HttpResponseMessage Delete(HttpRequestMessage request, int productId, int colorId, int sizeId)
        {
            _productQuantityService.Delete(productId,colorId,sizeId);
            _productQuantityService.Save();

            return request.CreateResponse(HttpStatusCode.OK, "Xóa thành công");
        }

        [Route("getcolors")]
        [HttpGet]
        public HttpResponseMessage GetColors(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var model = _productQuantityService.GetListColor();

                IEnumerable<ColorViewModel> modelVm = Mapper.Map<IEnumerable<Color>, IEnumerable<ColorViewModel>>(model);

                response = request.CreateResponse(HttpStatusCode.OK, modelVm);

                return response;
            });
        }

        [Route("getsizes")]
        [HttpGet]
        public HttpResponseMessage GetSizes(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var model = _productQuantityService.GetListSize();

                List<SizeViewModel> modelVm = Mapper.Map<List<Size>, List<SizeViewModel>>(model);

                response = request.CreateResponse(HttpStatusCode.OK, modelVm);

                return response;
            });
        }
    }
}