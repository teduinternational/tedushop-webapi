using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeduShop.Data.Infrastructure;
using TeduShop.Data.Repositories;
using TeduShop.Model.Models;

namespace TeduShop.Service
{
    public interface IFunctionService
    {
        Function Create(Function function);

        IEnumerable<Function> GetAll();
        void Save();
    }

    public class FunctionService : IFunctionService
    {
        private IFunctionRepository _functionRepository;
        private IUnitOfWork _unitOfWork;

        public FunctionService(IFunctionRepository functionRepository, IUnitOfWork unitOfWork)
        {
            _functionRepository = functionRepository;
            _unitOfWork = unitOfWork;
        }

        public Function Create(Function function)
        {
            return _functionRepository.Add(function);
        }

        public IEnumerable<Function> GetAll()
        {
            return _functionRepository.GetMulti(x => x.Status).OrderBy(x => x.DisplayOrder);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
