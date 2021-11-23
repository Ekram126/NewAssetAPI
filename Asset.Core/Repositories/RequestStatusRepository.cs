using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.RequestStatusVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class RequestStatusRepository : IRequestStatusRepository
    {
        private ApplicationDbContext _context;

        public RequestStatusRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Add(RequestStatus createRequestVM)
        {
            RequestStatus reqStatusObj = new RequestStatus();
            try
            {
                if (createRequestVM != null)
                {

                    reqStatusObj.Color = createRequestVM.Color;
                    reqStatusObj.Name = createRequestVM.Name;
                    reqStatusObj.NameAr = createRequestVM.NameAr;
                    _context.RequestStatus.Add(reqStatusObj);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return reqStatusObj.Id;
        }

        public int Delete(int id)
        {
            var reqStatusObj = _context.RequestStatus.Find(id);
            try
            {
                if (reqStatusObj != null)
                {
                    _context.RequestStatus.Remove(reqStatusObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }

        public IEnumerable<IndexRequestStatusVM> GetAll()
        {
            return _context.RequestStatus.Select(sts => new IndexRequestStatusVM
            {
                Id = sts.Id,
                Name = sts.Name,
                NameAr = sts.NameAr,
                Color = sts.Color
            }).ToList();
        }

        public IndexRequestStatusVM GetById(int id)
        {
            return _context.RequestStatus.Where(a => a.Id == id).Select(item => new IndexRequestStatusVM
            {
                Id = item.Id,
                Color = item.Color,
                Name = item.Name,
                NameAr = item.NameAr
            }).First();
        }

        public int Update(IndexRequestStatusVM editRequestVM)
        {
            try
            {
                var reqStatusObj = _context.RequestStatus.Find(editRequestVM.Id);
                reqStatusObj.Color = editRequestVM.Color;
                reqStatusObj.Name = editRequestVM.Name;
                reqStatusObj.NameAr = editRequestVM.NameAr;
                _context.Entry(reqStatusObj).State = EntityState.Modified;
                _context.SaveChanges();
                return reqStatusObj.Id;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }
    
    }
}
