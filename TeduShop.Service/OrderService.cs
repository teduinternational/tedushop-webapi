using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TeduShop.Data.Infrastructure;
using TeduShop.Data.Repositories;
using TeduShop.Model.Models;

namespace TeduShop.Service
{
    public interface IOrderService
    {
        Order Create(Order order);
        List<Order> GetList(string startDate, string endDate, string customerName, string status,
            int pageIndex, int pageSize, out int totalRow);

        Order GetDetail(int orderId);

        OrderDetail CreateDetail(OrderDetail order);

        void DeleteDetail(int productId, int orderId, int colorId, int sizeId);

        void UpdateStatus(int orderId);

        List<OrderDetail> GetOrderDetails(int orderId);

        void Save();
    }

    public class OrderService : IOrderService
    {
        private IOrderRepository _orderRepository;
        private IOrderDetailRepository _orderDetailRepository;
        private IUnitOfWork _unitOfWork;

        public OrderService(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, IUnitOfWork unitOfWork)
        {
            this._orderRepository = orderRepository;
            this._orderDetailRepository = orderDetailRepository;
            this._unitOfWork = unitOfWork;
        }

        public Order Create(Order order)
        {
            try
            {
                _orderRepository.Add(order);
                _unitOfWork.Commit();
                return order;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void UpdateStatus(int orderId)
        {
            var order = _orderRepository.GetSingleById(orderId);
            order.Status = true;
            _orderRepository.Update(order);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public List<Order> GetList(string startDate, string endDate, string customerName,
            string paymentStatus, int pageIndex, int pageSize, out int totalRow)
        {
            var query = _orderRepository.GetAll();
            if (!string.IsNullOrEmpty(startDate))
            {
                DateTime start = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.GetCultureInfo("vi-VN"));
                query = query.Where(x => x.CreatedDate >= start);

            }
            if (!string.IsNullOrEmpty(endDate))
            {
                DateTime end = DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.GetCultureInfo("vi-VN"));
                query = query.Where(x => x.CreatedDate <= end);
            }
            if (!string.IsNullOrEmpty(paymentStatus))
                query = query.Where(x => x.PaymentStatus == paymentStatus);
            totalRow = query.Count();
            return query.OrderByDescending(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public Order GetDetail(int orderId)
        {
            return _orderRepository.GetSingleByCondition(x => x.ID == orderId, new string[] { "OrderDetails" });
        }

        public List<OrderDetail> GetOrderDetails(int orderId)
        {
            return _orderDetailRepository.GetMulti(x => x.OrderID == orderId, new string[] { "Order","Color","Size", "Product" }).ToList();
        }

        public OrderDetail CreateDetail(OrderDetail order)
        {
            return _orderDetailRepository.Add(order);
        }

        public void DeleteDetail(int productId, int orderId, int colorId, int sizeId)
        {
            var detail = _orderDetailRepository.GetSingleByCondition(x => x.ProductID == productId
           && x.OrderID == orderId && x.ColorId == colorId && x.SizeId == sizeId);
            _orderDetailRepository.Delete(detail);
        }
    }
}