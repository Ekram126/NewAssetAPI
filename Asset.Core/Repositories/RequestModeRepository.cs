using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.RequestModeVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class RequestModeRepository : IRequestModeRepository
    {
        private ApplicationDbContext _context;
        string msg;

        public RequestModeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(CreateRequestMode createRequestMode)
        {
            try
            {
                if (createRequestMode != null)
                {
                    RequestMode requestMode = new RequestMode();
                    requestMode.Name = createRequestMode.Name;
                    requestMode.NameAr = createRequestMode.NameAr;
                    _context.RequestMode.Add(requestMode);
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
            var requestMode = _context.RequestMode.Find(id);
            try
            {
                if (requestMode != null)
                {
                    _context.RequestMode.Remove(requestMode);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }
        public IEnumerable<IndexRequestMode> GetAll()
        {
            return _context.RequestMode.Select(mode => new IndexRequestMode
            {
                Id = mode.Id,
                Name = mode.Name,
                NameAr = mode.NameAr
            }).ToList();
        }

        public IndexRequestMode GetById(int id)
        {
            return _context.RequestMode.Select(mode => new IndexRequestMode
            {
                Id = mode.Id,
                Name = mode.Name,
                NameAr = mode.NameAr
            }).FirstOrDefault();
        }

        public void Update(int id, EditRequestMode editRequestMode)
        {
            try
            {

                RequestMode requestMode = new RequestMode();
                requestMode.Id = editRequestMode.Id;
                requestMode.Name = editRequestMode.Name;
                requestMode.NameAr = editRequestMode.NameAr;
                _context.Entry(requestMode).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }
    }
}
