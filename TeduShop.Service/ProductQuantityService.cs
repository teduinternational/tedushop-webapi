using System;
using System.Collections.Generic;
using System.Linq;
using TeduShop.Data.Infrastructure;
using TeduShop.Data.Repositories;
using TeduShop.Model.Models;

namespace TeduShop.Service
{
    public interface IProductQuantityService
    {
        void Add(ProductQuantity productQuantity);

        void Delete(int id);

        List<ProductQuantity> GetAll(int productId, int? sizeId, int? colorId);
        bool CheckExist(int productId, int sizeId, int colorId);

        void Save();
    }

    public class ProductQuantityService : IProductQuantityService
    {
        private IProductQuantityRepository _productQuantityRepository;
        private IUnitOfWork _unitOfWork;

        public ProductQuantityService(IProductQuantityRepository productQuantityRepository, IUnitOfWork unitOfWork)
        {
            this._productQuantityRepository = productQuantityRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Add(ProductQuantity productImage)
        {
            _productQuantityRepository.Add(productImage);
        }

        public bool CheckExist(int productId, int sizeId, int colorId)
        {
            return _productQuantityRepository.CheckContains(x => x.ProductId == productId && x.ColorId == colorId && x.SizeId == sizeId);
        }

        public void Delete(int id)
        {
            _productQuantityRepository.Delete(id);
        }

        public List<ProductQuantity> GetAll(int productId, int? sizeId, int? colorId)
        {
            var query = _productQuantityRepository.GetMulti(x => x.ProductId == productId);
            if (sizeId.HasValue)
                query = query.Where(x => x.SizeId == sizeId.Value);
            if (colorId.HasValue)
                query = query.Where(x => x.ColorId == colorId.Value);
            return query.ToList();

        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}