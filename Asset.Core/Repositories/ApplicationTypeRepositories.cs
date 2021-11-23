using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.ApplicationTypeVM;
using Asset.ViewModels.RoleCategoryVM;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Asset.Core.Repositories
{
    public class ApplicationTypeRepositories : IApplicationTypeRepository
    {
        private ApplicationDbContext _context;

        public ApplicationTypeRepositories(ApplicationDbContext context)
        {
            _context = context;
        }


        //public EditApplicationTypeVM GetById(int id)
        //{
        //  return _context.ApplicationTypes.Where(a=>a.Id ==  id).Select(item => new EditApplicationTypeVM
        //  {
        //        Id = item.Id,
        //        Name = item.Name,
        //        NameAr = item.NameAr
        //    }).FirstOrDefault();
        //}




        public IEnumerable<IndexApplicationTypeVM.GetData> GetAll()
        {
            var lstApplicationTypes = _context.ApplicationTypes.ToList().Select(item => new IndexApplicationTypeVM.GetData
            {
                Id = item.Id,
                Code= item.Code,
                Name = item.Name,
                NameAr = item.NameAr
            });

            return lstApplicationTypes;
        }

        //public int Add(CreateApplicationTypeVM ApplicationTypeVM)
        //{
        //    ApplicationType ApplicationTypeObj = new ApplicationType();
        //    try
        //    {
        //        if (ApplicationTypeVM != null)
        //        {
        //            ApplicationTypeObj.Name = ApplicationTypeVM.Name;
        //            ApplicationTypeObj.NameAr = ApplicationTypeVM.NameAr;
        //            _context.ApplicationTypes.Add(ApplicationTypeObj);
        //            _context.SaveChanges();

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string str  = ex.Message;
        //    }
        //    return ApplicationTypeObj.Id;
        //}

        //public int Delete(int id)
        //{
        //    var ApplicationTypeObj = _context.ApplicationTypes.Find(id);
        //    try
        //    {
        //        if (ApplicationTypeObj != null)
        //        {
        //            _context.ApplicationTypes.Remove(ApplicationTypeObj);
        //            return _context.SaveChanges();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string str = ex.Message;
        //    }
        //    return 0;
        //}

        //public int Update(EditApplicationTypeVM ApplicationTypeVM)
        //{
        //    try
        //    {
        //        var ApplicationTypeObj = _context.ApplicationTypes.Find(ApplicationTypeVM.Id);
        //        ApplicationTypeObj.Id = ApplicationTypeVM.Id;
        //        ApplicationTypeObj.Name = ApplicationTypeVM.Name;
        //        ApplicationTypeObj.NameAr = ApplicationTypeVM.NameAr;
        //        _context.Entry(ApplicationTypeObj).State = EntityState.Modified;
        //        _context.SaveChanges();
        //        return ApplicationTypeObj.Id;
        //    }
        //    catch (Exception ex)
        //    {
        //      string  msg = ex.Message;
        //    }
        //    return 0;
        //}
    }
}