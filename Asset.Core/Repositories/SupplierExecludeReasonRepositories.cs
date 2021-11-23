using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.SupplierExecludeReasonVM;
using Asset.ViewModels.RoleCategoryVM;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Asset.Core.Repositories
{
    public class SupplierExecludeReasonRepositories : ISupplierExecludeReasonRepository
    {
        private ApplicationDbContext _context;

        public SupplierExecludeReasonRepositories(ApplicationDbContext context)
        {
            _context = context;
        }


        public EditSupplierExcludeReasonVM GetById(int id)
        {
          return _context.SupplierExecludeReasons.Where(a=>a.Id ==  id).Select(item => new EditSupplierExcludeReasonVM
          {
                Id = item.Id,
                Name = item.Name,
                NameAr = item.NameAr
            }).FirstOrDefault();

        }




        public IEnumerable<IndexSupplierExcludeReasonVM.GetData> GetAll()
        {
            var lstSupplierExecludeReasons = _context.SupplierExecludeReasons.ToList().Select(item => new IndexSupplierExcludeReasonVM.GetData
            {
                Id = item.Id,
                Name = item.Name,
                NameAr = item.NameAr
            });

            return lstSupplierExecludeReasons;
        }

        public int Add(CreateSupplierExcludeReasonVM SupplierExecludeReasonVM)
        {
            SupplierExecludeReason SupplierExecludeReasonObj = new SupplierExecludeReason();
            try
            {
                if (SupplierExecludeReasonVM != null)
                {
                    SupplierExecludeReasonObj.Name = SupplierExecludeReasonVM.Name;
                    SupplierExecludeReasonObj.NameAr = SupplierExecludeReasonVM.NameAr;
                    _context.SupplierExecludeReasons.Add(SupplierExecludeReasonObj);
                    _context.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                string str  = ex.Message;
            }
            return SupplierExecludeReasonObj.Id;
        }

        public int Delete(int id)
        {
            var SupplierExecludeReasonObj = _context.SupplierExecludeReasons.Find(id);
            try
            {
                if (SupplierExecludeReasonObj != null)
                {
                    _context.SupplierExecludeReasons.Remove(SupplierExecludeReasonObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }

            return 0;
        }

        public int Update(EditSupplierExcludeReasonVM SupplierExecludeReasonVM)
        {
            try
            {
                var SupplierExecludeReasonObj = _context.SupplierExecludeReasons.Find(SupplierExecludeReasonVM.Id);
                SupplierExecludeReasonObj.Id = SupplierExecludeReasonVM.Id;
                SupplierExecludeReasonObj.Name = SupplierExecludeReasonVM.Name;
                SupplierExecludeReasonObj.NameAr = SupplierExecludeReasonVM.NameAr;
                _context.Entry(SupplierExecludeReasonObj).State = EntityState.Modified;
                _context.SaveChanges();
                return SupplierExecludeReasonObj.Id;
            }
            catch (Exception ex)
            {
              string  msg = ex.Message;
            }

            return 0;
        }
    }
}