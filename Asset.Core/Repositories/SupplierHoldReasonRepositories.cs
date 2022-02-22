using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.SupplierHoldReasonVM;
using Asset.ViewModels.RoleCategoryVM;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Asset.Core.Repositories
{
    public class SupplierHoldReasonRepositories : ISupplierHoldReasonRepository
    {
        private ApplicationDbContext _context;

        public SupplierHoldReasonRepositories(ApplicationDbContext context)
        {
            _context = context;
        }


        public EditSupplierHoldReasonVM GetById(int id)
        {
          return _context.SupplierHoldReasons.Where(a=>a.Id ==  id).Select(item => new EditSupplierHoldReasonVM
          {
                Id = item.Id,
                Name = item.Name,
                NameAr = item.NameAr
            }).FirstOrDefault();

        }




        public IEnumerable<IndexSupplierHoldReasonVM.GetData> GetAll()
        {
            var lstSupplierHoldReasons = _context.SupplierHoldReasons.ToList().Select(item => new IndexSupplierHoldReasonVM.GetData
            {
                Id = item.Id,
                Name = item.Name,
                NameAr = item.NameAr
            });

            return lstSupplierHoldReasons;
        }

        public int Add(CreateSupplierHoldReasonVM SupplierHoldReasonVM)
        {
            SupplierHoldReason SupplierHoldReasonObj = new SupplierHoldReason();
            try
            {
                if (SupplierHoldReasonVM != null)
                {
                    SupplierHoldReasonObj.Name = SupplierHoldReasonVM.Name;
                    SupplierHoldReasonObj.NameAr = SupplierHoldReasonVM.NameAr;
                    _context.SupplierHoldReasons.Add(SupplierHoldReasonObj);
                    _context.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                string str  = ex.Message;
            }
            return SupplierHoldReasonObj.Id;
        }

        public int Delete(int id)
        {
            var SupplierHoldReasonObj = _context.SupplierHoldReasons.Find(id);
            try
            {
                if (SupplierHoldReasonObj != null)
                {
                    _context.SupplierHoldReasons.Remove(SupplierHoldReasonObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }

            return 0;
        }

        public int Update(EditSupplierHoldReasonVM SupplierHoldReasonVM)
        {
            try
            {
                var SupplierHoldReasonObj = _context.SupplierHoldReasons.Find(SupplierHoldReasonVM.Id);
                SupplierHoldReasonObj.Id = SupplierHoldReasonVM.Id;
                SupplierHoldReasonObj.Name = SupplierHoldReasonVM.Name;
                SupplierHoldReasonObj.NameAr = SupplierHoldReasonVM.NameAr;
                _context.Entry(SupplierHoldReasonObj).State = EntityState.Modified;
                _context.SaveChanges();
                return SupplierHoldReasonObj.Id;
            }
            catch (Exception ex)
            {
              string  msg = ex.Message;
            }

            return 0;
        }
    }
}