using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.OriginVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class OriginRepositories : IOriginRepository
    {

        private ApplicationDbContext _context;


        public OriginRepositories(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Add(CreateOriginVM model)
        {
            Origin originObj = new Origin();
            try
            {
                if (model != null)
                {
                    originObj.Name = model.Name;
                    originObj.NameAr = model.NameAr;
                    originObj.Code = model.Code;
                    _context.Origins.Add(originObj);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return originObj.Id;
        }

        public int Delete(int id)
        {
            var originObj = _context.Origins.Find(id);
            try
            {
                if (originObj != null)
                {
                    _context.Origins.Remove(originObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }

        public IEnumerable<IndexOriginVM.GetData> GetAll()
        {
            return _context.Origins.ToList().Select(item => new IndexOriginVM.GetData
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                NameAr = item.NameAr
            });
        }

        public IEnumerable<Origin> GetAllOrigins()
        {
            return _context.Origins.ToList();
        }

        public EditOriginVM GetById(int id)
        {
            return _context.Origins.Where(a => a.Id == id).Select(item => new EditOriginVM
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                NameAr = item.NameAr
            }).First();
        }

        public IEnumerable<IndexOriginVM.GetData> GetOriginByName(string originName)
        {
            return _context.Origins.Where(a => a.Name == originName || a.NameAr == originName).ToList().Select(item => new IndexOriginVM.GetData
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                NameAr = item.NameAr
            });
        }

        public IEnumerable<IndexOriginVM.GetData> SortOrigins(SortOriginVM sortObj)
        {
            var lstOrigins = GetAll().ToList();
            if (sortObj.Code != "")
            {
                if (sortObj.SortStatus == "descending")
                    lstOrigins = lstOrigins.OrderByDescending(d => d.Code).ToList();
                else
                    lstOrigins = lstOrigins.OrderBy(d => d.Code).ToList();
            }
            else if (sortObj.Name != "")
            {
                if (sortObj.SortStatus == "descending")
                    lstOrigins = lstOrigins.OrderByDescending(d => d.Name).ToList();
                else
                    lstOrigins = lstOrigins.OrderBy(d => d.Name).ToList();
            }

            else if (sortObj.NameAr != "")
            {
                if (sortObj.SortStatus == "descending")
                    lstOrigins = lstOrigins.OrderByDescending(d => d.NameAr).ToList();
                else
                    lstOrigins = lstOrigins.OrderBy(d => d.NameAr).ToList();
            }

            return lstOrigins;
        }

        public int Update(EditOriginVM model)
        {
            try
            {
                var originObj = _context.Origins.Find(model.Id);
                originObj.Id = model.Id;
                originObj.Name = model.Name;
                originObj.NameAr = model.NameAr;
                originObj.Code = model.Code;
                _context.Entry(originObj).State = EntityState.Modified;
                _context.SaveChanges();
                return originObj.Id;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }
    }
}
