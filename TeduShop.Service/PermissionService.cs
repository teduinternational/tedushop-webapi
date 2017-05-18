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
    public interface IPermissionService
    {
        ICollection<Permission> GetByFunctionId(string functionId);
        ICollection<Permission> GetByUserId(string userId);
        void Add(Permission permission);
        void DeleteAll(string functionId);
        void SaveChange();
    }

    public class PermissionService : IPermissionService
    {
        private IPermissionRepository _permissionRepository;
        private IUnitOfWork _unitOfWork;

        public PermissionService(IPermissionRepository permissionRepository, IUnitOfWork unitOfWork)
        {
            this._permissionRepository = permissionRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Add(Permission permission)
        {
            _permissionRepository.Add(permission);
        }

        public void DeleteAll(string functionId)
        {
            _permissionRepository.DeleteMulti(x => x.FunctionId == functionId);
        }

        public ICollection<Permission> GetByFunctionId(string functionId)
        {
            return _permissionRepository
                .GetMulti(x => x.FunctionId == functionId, new string[] { "AppRole", "AppRole" }).ToList();
        }

        public ICollection<Permission> GetByUserId(string userId)
        {
            return _permissionRepository.GetByUserId(userId);
        }

        public void SaveChange()
        {
            _unitOfWork.Commit();
        }
    }
}
