using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.RequestPeriorityVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class RequestPeriorityRepository : IRequestPeriorityRepository
    {
        private ApplicationDbContext _context;
        string msg;

        public RequestPeriorityRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(CreateRequestPeriority createRequestPeriority)
        {
            try
            {
                if (createRequestPeriority != null)
                {
                    RequestPeriority requestPeriority = new RequestPeriority();
                    requestPeriority.Name = createRequestPeriority.Name;
                    requestPeriority.NameAr = createRequestPeriority.NameAr;

                    requestPeriority.Color = createRequestPeriority.Color;
                    requestPeriority.Icon = createRequestPeriority.Icon;

                    _context.RequestPeriority.Add(requestPeriority);
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
            var requestPeriority = _context.RequestPeriority.Find(id);
            try
            {
                if (requestPeriority != null)
                {
                    _context.RequestPeriority.Remove(requestPeriority);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }

        public IEnumerable<IndexRequestPeriority> GetAll()
        {
            return _context.RequestPeriority.Select(per => new IndexRequestPeriority
            {
                Id = per.Id,
               Name = per.Name,
                NameAr = per.NameAr,
                Color= per.Color,
                Icon= per.Icon
                
                
            }).ToList();
        }

        public IndexRequestPeriority GetById(int id)
        {
            return _context.RequestPeriority.Select(per => new IndexRequestPeriority
            {
                Id = per.Id,
                Name = per.Name,
               NameAr = per.NameAr,
                Color = per.Color,
                Icon = per.Icon
            }).FirstOrDefault();
        }

        public IEnumerable<IndexRequestPeriority> GetByPeriorityName(string name)
        { 
            
            return _context.RequestPeriority.Where(a=>a.Name == name || a.NameAr == name).Select(per => new IndexRequestPeriority
            {
                Id = per.Id,
               Name = per.Name,
                NameAr = per.NameAr,
                Color = per.Color,
                Icon = per.Icon
            }).ToList();
            
        }

        public void Update(int id, EditRequestPeriority editRequestPeriority)
        {

            try
            {
                RequestPeriority requestPeriority = new RequestPeriority();
                requestPeriority.Id = editRequestPeriority.Id;
                requestPeriority.Name = editRequestPeriority.Name;
                requestPeriority.NameAr = editRequestPeriority.NameAr;
                requestPeriority.Color = editRequestPeriority.Color;
                requestPeriority.Icon = editRequestPeriority.Icon;
                _context.Entry(requestPeriority).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }
    }
}
