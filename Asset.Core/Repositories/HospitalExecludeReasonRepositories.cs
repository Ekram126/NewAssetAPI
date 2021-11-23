using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.HospitalExecludeReasonVM;
using Asset.ViewModels.RoleCategoryVM;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Asset.Core.Repositories
{
    public class HospitalExecludeReasonRepositories : IHospitalExecludeReasonRepository
    {
        private ApplicationDbContext _context;

        public HospitalExecludeReasonRepositories(ApplicationDbContext context)
        {
            _context = context;
        }


        public EditHospitalExecludeReasonVM GetById(int id)
        {
          return _context.HospitalExecludeReasons.Where(a=>a.Id ==  id).Select(item => new EditHospitalExecludeReasonVM
          {
                Id = item.Id,
                Name = item.Name,
                NameAr = item.NameAr
            }).FirstOrDefault();

        }




        public IEnumerable<IndexHospitalExecludeReasonVM.GetData> GetAll()
        {
            var lstHospitalExecludeReasons = _context.HospitalExecludeReasons.ToList().Select(item => new IndexHospitalExecludeReasonVM.GetData
            {
                Id = item.Id,
                Name = item.Name,
                NameAr = item.NameAr
            });

            return lstHospitalExecludeReasons;
        }

        public int Add(CreateHospitalExecludeReasonVM HospitalExecludeReasonVM)
        {
            HospitalExecludeReason HospitalExecludeReasonObj = new HospitalExecludeReason();
            try
            {
                if (HospitalExecludeReasonVM != null)
                {
                    HospitalExecludeReasonObj.Name = HospitalExecludeReasonVM.Name;
                    HospitalExecludeReasonObj.NameAr = HospitalExecludeReasonVM.NameAr;
                    _context.HospitalExecludeReasons.Add(HospitalExecludeReasonObj);
                    _context.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                string str  = ex.Message;
            }
            return HospitalExecludeReasonObj.Id;
        }

        public int Delete(int id)
        {
            var HospitalExecludeReasonObj = _context.HospitalExecludeReasons.Find(id);
            try
            {
                if (HospitalExecludeReasonObj != null)
                {
                    _context.HospitalExecludeReasons.Remove(HospitalExecludeReasonObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }

            return 0;
        }

        public int Update(EditHospitalExecludeReasonVM HospitalExecludeReasonVM)
        {
            try
            {
                var HospitalExecludeReasonObj = _context.HospitalExecludeReasons.Find(HospitalExecludeReasonVM.Id);
                HospitalExecludeReasonObj.Id = HospitalExecludeReasonVM.Id;
                HospitalExecludeReasonObj.Name = HospitalExecludeReasonVM.Name;
                HospitalExecludeReasonObj.NameAr = HospitalExecludeReasonVM.NameAr;
                _context.Entry(HospitalExecludeReasonObj).State = EntityState.Modified;
                _context.SaveChanges();
                return HospitalExecludeReasonObj.Id;
            }
            catch (Exception ex)
            {
              string  msg = ex.Message;
            }

            return 0;
        }
    }
}