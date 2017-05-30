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

        void Delete(int productId, int colorId, int sizeId);

        List<ProductQuantity> GetAll(int productId, int? sizeId, int? colorId);

        bool CheckExist(int productId, int sizeId, int colorId);

        List<Size> GetListSize();

        List<Color> GetListColor();

        void Save();
    }

    public class ProductQuantityService : IProductQuantityService
    {
        private IProductQuantityRepository _productQuantityRepository;
        private IColorRepository _colorRepository;
        private ISizeRepository _sizeRepository;
        private IUnitOfWork _unitOfWork;

        public ProductQuantityService(IProductQuantityRepository productQuantityRepository,
            IColorRepository colorRepository, ISizeRepository sizeRepository,
            IUnitOfWork unitOfWork)
        {
            this._productQuantityRepository = productQuantityRepository;
            _sizeRepository = sizeRepository;
            _colorRepository = colorRepository;
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

        public void Delete(int productId, int colorId, int sizeId)
        {
            var productQuantity = _productQuantityRepository.GetSingleByCondition(x => x.ProductId == productId && x.ColorId == colorId && x.SizeId == sizeId);
            _productQuantityRepository.Delete(productQuantity);
        }

        public List<ProductQuantity> GetAll(int productId, int? sizeId, int? colorId)
        {
            var query = _productQuantityRepository.GetMulti(x => x.ProductId == productId, new string[] { "Color", "Size" });
            if (sizeId.HasValue)
                query = query.Where(x => x.SizeId == sizeId.Value);
            if (colorId.HasValue)
                query = query.Where(x => x.ColorId == colorId.Value);
            return query.ToList();

        }

        public List<Color> GetListColor()
        {
            return _colorRepository.GetAll().ToList();
        }

        public List<Size> GetListSize()
        {
            return _sizeRepository.GetAll().ToList();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}