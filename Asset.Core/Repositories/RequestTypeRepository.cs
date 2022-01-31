using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.RequestTypeVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class RequestTypeRepository : IRequestTypeRepository
    {
        private ApplicationDbContext _context;
        string msg;

        public RequestTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(CreateRequestTypeVM createRequestTypeVM)
        {
            try
            {
                if (createRequestTypeVM != null)
                {
                    RequestType requestType = new RequestType();
                    requestType.Name = createRequestTypeVM.Name;
                    requestType.NameAr = createRequestTypeVM.NameAr;
                    requestType.Code = createRequestTypeVM.Code;
                    _context.RequestTypes.Add(requestType);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }

        public void Delete(int id)
        {
            var requestType = _context.RequestTypes.Find(id);
            try
            {
                if (requestType != null)
                {
                    _context.RequestTypes.Remove(requestType);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }

        public IEnumerable<IndexRequestTypeVM> GetAll()
        {
            return _context.RequestTypes.Select(Type => new IndexRequestTypeVM
            {
                Id = Type.Id,
                Name = Type.Name,
                NameAr= Type.NameAr,
                Code= Type.Code
            }).ToList();
        }

        public IndexRequestTypeVM GetById(int id)
        {
            return _context.RequestTypes.Select(Type => new IndexRequestTypeVM
            {
                Id = Type.Id,
                Name = Type.Name,
                NameAr = Type.NameAr,
                Code = Type.Code
            }).Where(e => e.Id == id).FirstOrDefault();
        }

        public IEnumerable<IndexRequestTypeVM> SortRequestTypes(SortRequestTypeVM sortObj)
        {
            var lstRequestTypes = GetAll().ToList();
            if (sortObj.Code != "")
            {
                if (sortObj.SortStatus == "descending")
                    lstRequestTypes = lstRequestTypes.OrderByDescending(d => d.Code).ToList();
                else
                    lstRequestTypes = lstRequestTypes.OrderBy(d => d.Code).ToList();
            }
            if (sortObj.Name != "")
            {
                if (sortObj.SortStatus == "descending")
                    lstRequestTypes = lstRequestTypes.OrderByDescending(d => d.Name).ToList();
                else
                    lstRequestTypes = lstRequestTypes.OrderBy(d => d.Name).ToList();
            }

            else if (sortObj.NameAr != "")
            {
                if (sortObj.SortStatus == "descending")
                    lstRequestTypes = lstRequestTypes.OrderByDescending(d => d.NameAr).ToList();
                else
                    lstRequestTypes = lstRequestTypes.OrderBy(d => d.NameAr).ToList();
            }

            return lstRequestTypes;
        }

        public void Update(EditRequestTypeVM editRequestTypeVM)
        {
            try
            {
                var requestTypeObj = _context.RequestTypes.Find(editRequestTypeVM.Id);
                requestTypeObj.Id = editRequestTypeVM.Id;
                requestTypeObj.Name = editRequestTypeVM.Name;
                requestTypeObj.NameAr = editRequestTypeVM.NameAr;
                requestTypeObj.Code = editRequestTypeVM.Code;
                _context.Entry(requestTypeObj).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }
    }
}
