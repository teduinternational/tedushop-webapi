using System;
using System.Collections.Generic;
using System.Linq;
using TeduShop.Data.Infrastructure;
using TeduShop.Data.Repositories;
using TeduShop.Model.Models;

namespace TeduShop.Service
{
    public interface IAnnouncementService
    {
        Announcement Create(Announcement announcement);

        List<Announcement> GetListByUserId(string userId, int pageIndex, int pageSize, out int totalRow);

        List<Announcement> GetListByUserId(string userId, int top);

        void Delete(int notificationId);

        void MarkAsRead(string userId, int notificationId);

        Announcement GetDetail(int id);

        List<Announcement> GetListAll(int pageIndex, int pageSize, out int totalRow);

        List<Announcement> ListAllUnread(string userId, int pageIndex, int pageSize, out int totalRow);

        void Save();

    }

    public class AnnouncementService : IAnnouncementService
    {
        private IAnnouncementRepository _announcementRepository;
        private IAnnouncementUserRepository _announcementUserRepository;

        private IUnitOfWork _unitOfWork;

        public AnnouncementService(IAnnouncementRepository announcementRepository,
            IAnnouncementUserRepository announcementUserRepository,
            IUnitOfWork unitOfWork)
        {
            _announcementRepository = announcementRepository;
            _announcementUserRepository = announcementUserRepository;
            _unitOfWork = unitOfWork;
        }

        public Announcement Create(Announcement announcement)
        {
            return _announcementRepository.Add(announcement);
        }

        public void Delete(int notificationId)
        {
            _announcementRepository.Delete(notificationId);
        }

        public List<Announcement> GetListAll(int pageIndex, int pageSize, out int totalRow)
        {
            var query = _announcementRepository.GetAll(new string[] { "AppUser" });
            totalRow = query.Count();
            return query.OrderByDescending(x => x.CreatedDate)
                .Skip(pageSize * (pageIndex - 1))
                .Take(pageSize).ToList();
        }

        public List<Announcement> GetListByUserId(string userId, int pageIndex, int pageSize, out int totalRow)
        {
            var query = _announcementRepository.GetMulti(x => x.UserId == userId);
            totalRow = query.Count();
            return query.OrderByDescending(x => x.CreatedDate)
                .Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
        }

        public List<Announcement> GetListByUserId(string userId, int top)
        {
            return _announcementRepository.GetMulti(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedDate)
                .Take(top).ToList();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public Announcement GetDetail(int id)
        {
            return _announcementRepository.GetSingleByCondition(x => x.ID == id, new string[] { "AppUser" });
        }

        public List<Announcement> ListAllUnread(string userId, int pageIndex, int pageSize, out int totalRow)
        {
            var query = _announcementRepository.GetAllUnread(userId);
            totalRow = query.Count();
            return query.OrderByDescending(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public void MarkAsRead(string userId, int notificationId)
        {
            var announ = _announcementUserRepository.GetSingleByCondition(x => x.AnnouncementId == notificationId && x.UserId == userId);
            if (announ == null)
            {
                _announcementUserRepository.Add(new AnnouncementUser()
                {
                    AnnouncementId = notificationId,
                    UserId = userId,
                    HasRead = true
                });
            }
            else
            {
                announ.HasRead = true;
            }
        }
    }
}