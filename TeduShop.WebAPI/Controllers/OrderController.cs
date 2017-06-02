using AutoMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TeduShop.Model.Models;
using TeduShop.Service;
using TeduShop.Web.Infrastructure.Core;
using TeduShop.Web.Infrastructure.Extensions;
using TeduShop.Web.Models;
using TeduShop.Web.Providers;

namespace TeduShop.Web.Controllers
{
    [RoutePrefix("api/Order")]
    public class OrderController : ApiControllerBase
    {
        private IOrderService _orderService;

        public OrderController(IOrderService orderService, IErrorService errorService) : base(errorService)
        {
            _orderService = orderService;
        }

        [Route("getlistpaging")]
        [HttpGet]
        [Permission(Action = "Read", Function = "ORDER")]
        public HttpResponseMessage GetListPaging(HttpRequestMessage request, string startDate, string endDate,
            string customerName, string paymentStatus, int page, int pageSize, string filter = null)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                int totalRow = 0;

                var model = _orderService.GetList(startDate, endDate, customerName, paymentStatus, page, pageSize, out totalRow);
                List<OrderViewModel> modelVm = Mapper.Map<List<Order>, List<OrderViewModel>>(model);

                PaginationSet<OrderViewModel> pagedSet = new PaginationSet<OrderViewModel>()
                {
                    PageIndex = page,
                    PageSize = pageSize,
                    TotalRows = totalRow,
                    Items = modelVm,
                };

                response = request.CreateResponse(HttpStatusCode.OK, pagedSet);

                return response;
            });
        }

        [Route("detail/{id}")]
        [HttpGet]
        [Permission(Action = "Read", Function = "ORDER")]
        //[Authorize(Roles = "ViewUser")]
        public HttpResponseMessage Details(HttpRequestMessage request, int id)
        {
            if (id == 0)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(id) + " không có giá trị.");
            }
            var order = _orderService.GetDetail(id);
            if (order == null)
            {
                return request.CreateErrorResponse(HttpStatusCode.NoContent, "Không có dữ liệu");
            }
            else
            {
                var orderVm = Mapper.Map<Order, OrderViewModel>(order);

                return request.CreateResponse(HttpStatusCode.OK, orderVm);
            }
        }

        [Route("getalldetails")]
        [HttpGet]
        [Permission(Action = "Read", Function = "ORDER")]
        //[Authorize(Roles = "ViewUser")]
        public HttpResponseMessage GetOrderDetails(HttpRequestMessage request, int id)
        {
            if (id == 0)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(id) + " không có giá trị.");
            }
            var details = _orderService.GetOrderDetails(id);
            if (details == null)
            {
                return request.CreateErrorResponse(HttpStatusCode.NoContent, "Không có dữ liệu");
            }
            else
            {
                var detailVms = Mapper.Map<List<OrderDetail>, List<OrderDetailViewModel>>(details);

                return request.CreateResponse(HttpStatusCode.OK, detailVms);
            }
        }

        [HttpPost]
        [Route("add")]
        //[Authorize(Roles = "AddUser")]
        [Permission(Action = "Create", Function = "USER")]
        public HttpResponseMessage Create(HttpRequestMessage request, OrderViewModel orderVm)
        {
            if (ModelState.IsValid)
            {
                var newOrder = new Order();
                newOrder.UpdateOrder(orderVm);
                try
                {
                    var listOrderDetails = new List<OrderDetail>();
                    foreach (var item in orderVm.OrderDetails)
                    {
                        listOrderDetails.Add(new OrderDetail()
                        {
                            ColorId = item.ColorId,
                            ProductID = item.ProductID,
                            Quantity = item.Quantity,
                            Price = item.Price,
                            SizeId = item.SizeId,
                        });
                    }
                    newOrder.OrderDetails = listOrderDetails;
                    var result = _orderService.Create(newOrder);
                    var model = Mapper.Map<Order, OrderViewModel>(result);
                    return request.CreateResponse(HttpStatusCode.OK, model);
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

        [HttpPut]
        [Route("updateStatus")]
        //[Authorize(Roles = "UpdateUser")]
        [Permission(Action = "Update", Function = "ORDER")]
        public HttpResponseMessage UpdateStatus(HttpRequestMessage request, int orderId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _orderService.UpdateStatus(orderId);
                    return request.CreateResponse(HttpStatusCode.OK, orderId);
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


    }
}