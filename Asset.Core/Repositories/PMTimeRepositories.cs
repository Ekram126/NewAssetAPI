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
    public class PMTimeRepositories : IPMTimeRepository
    {

        private ApplicationDbContext _context;


        public PMTimeRepositories(ApplicationDbContext context)
        {
            _context = context;
        }

        //public int Add(CreateOriginVM model)
        //{
        //    Origin originObj = new Origin();
        //    try
        //    {
        //        if (model != null)
        //        {
        //            originObj.Name = model.Name;
        //            originObj.NameAr = model.NameAr;
        //            originObj.Code = model.Code;
        //            _context.Origins.Add(originObj);
        //            _context.SaveChanges();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string str = ex.Message;
        //    }
        //    return originObj.Id;
        //}

        //public int Delete(int id)
        //{
        //    var originObj = _context.Origins.Find(id);
        //    try
        //    {
        //        if (originObj != null)
        //        {
        //            _context.Origins.Remove(originObj);
        //            return _context.SaveChanges();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string msg = ex.Message;
        //    }
        //    return 0;
        //}

        public IEnumerable<PMTime> GetAll()
        {
            return _context.PMTimes.ToList();
        }

        //public IEnumerable<Origin> GetAllOrigins()
        //{
        //    return _context.Origins.ToList();
        //}

        //public EditOriginVM GetById(int id)
        //{
        //    return _context.Origins.Where(a => a.Id == id).Select(item => new EditOriginVM
        //    {
        //        Id = item.Id,
        //        Code = item.Code,
        //        Name = item.Name,
        //        NameAr = item.NameAr
        //    }).First();
        //}

        //public IEnumerable<IndexOriginVM.GetData> GetOriginByName(string originName)
        //{
        //    return _context.Origins.Where(a => a.Name == originName || a.NameAr == originName).ToList().Select(item => new IndexOriginVM.GetData
        //    {
        //        Id = item.Id,
        //        Code = item.Code,
        //        Name = item.Name,
        //        NameAr = item.NameAr
        //    });
        //}

        //public int Update(EditOriginVM model)
        //{
        //    try
        //    {
        //        var originObj = _context.Origins.Find(model.Id);
        //        originObj.Id = model.Id;
        //        originObj.Name = model.Name;
        //        originObj.NameAr = model.NameAr;
        //        originObj.Code = model.Code;
        //        _context.Entry(originObj).State = EntityState.Modified;
        //        _context.SaveChanges();
        //        return originObj.Id;
        //    }
        //    catch (Exception ex)
        //    {
        //        string msg = ex.Message;
        //    }
        //    return 0;
        //}
    }
}
