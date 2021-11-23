using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.HospitalHoldReasonVM;
using Asset.ViewModels.RoleCategoryVM;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Asset.Core.Repositories
{
    public class HospitalHoldReasonRepositories : IHospitalHoldReasonRepository
    {
        private ApplicationDbContext _context;

        public HospitalHoldReasonRepositories(ApplicationDbContext context)
        {
            _context = context;
        }


        public EditHospitalHoldReasonVM GetById(int id)
        {
          return _context.HospitalHoldReasons.Where(a=>a.Id ==  id).Select(item => new EditHospitalHoldReasonVM
          {
                Id = item.Id,
                Name = item.Name,
                NameAr = item.NameAr
            }).FirstOrDefault();

        }




        public IEnumerable<IndexHospitalHoldReasonVM.GetData> GetAll()
        {
            var lstHospitalHoldReasons = _context.HospitalHoldReasons.ToList().Select(item => new IndexHospitalHoldReasonVM.GetData
            {
                Id = item.Id,
                Name = item.Name,
                NameAr = item.NameAr
            });

            return lstHospitalHoldReasons;
        }

        public int Add(CreateHospitalHoldReasonVM HospitalHoldReasonVM)
        {
            HospitalHoldReason HospitalHoldReasonObj = new HospitalHoldReason();
            try
            {
                if (HospitalHoldReasonVM != null)
                {
                    HospitalHoldReasonObj.Name = HospitalHoldReasonVM.Name;
                    HospitalHoldReasonObj.NameAr = HospitalHoldReasonVM.NameAr;
                    _context.HospitalHoldReasons.Add(HospitalHoldReasonObj);
                    _context.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                string str  = ex.Message;
            }
            return HospitalHoldReasonObj.Id;
        }

        public int Delete(int id)
        {
            var HospitalHoldReasonObj = _context.HospitalHoldReasons.Find(id);
            try
            {
                if (HospitalHoldReasonObj != null)
                {
                    _context.HospitalHoldReasons.Remove(HospitalHoldReasonObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }

            return 0;
        }

        public int Update(EditHospitalHoldReasonVM HospitalHoldReasonVM)
        {
            try
            {
                var HospitalHoldReasonObj = _context.HospitalHoldReasons.Find(HospitalHoldReasonVM.Id);
                HospitalHoldReasonObj.Id = HospitalHoldReasonVM.Id;
                HospitalHoldReasonObj.Name = HospitalHoldReasonVM.Name;
                HospitalHoldReasonObj.NameAr = HospitalHoldReasonVM.NameAr;
                _context.Entry(HospitalHoldReasonObj).State = EntityState.Modified;
                _context.SaveChanges();
                return HospitalHoldReasonObj.Id;
            }
            catch (Exception ex)
            {
              string  msg = ex.Message;
            }

            return 0;
        }
    }
}