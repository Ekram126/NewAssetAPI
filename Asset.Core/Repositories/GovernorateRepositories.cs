using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.GovernorateVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Asset.Core.Repositories
{
    public class GovernorateRepositories : IGovernorateRepository
    {
        private ApplicationDbContext _context;
        string msg;

        public GovernorateRepositories(ApplicationDbContext context)
        {
            _context = context;
        }


        public EditGovernorateVM GetById(int id)
        {
            return _context.Governorates.Where(a => a.Id == id).Select(item => new EditGovernorateVM
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                NameAr = item.NameAr,
                Population = item.Population,
                Latitude = item.Latitude,
                Longtitude = item.Longtitude,
                Area = item.Area,
                Logo = item.Logo
            }).First();
        }




        public IEnumerable<IndexGovernorateVM.GetData> GetAll()
        {
            return _context.Governorates.ToList().Select(item => new IndexGovernorateVM.GetData
            {
                Id = item.Id,
                Name = item.Name,
                NameAr = item.NameAr,
                Code = item.Code,
                Population = item.Population.ToString(),
                Area = item.Area.ToString(),
                Latitude = item.Latitude,
                Longtitude = item.Longtitude,
                Logo = item.Logo
            });
        }

        public int Add(CreateGovernorateVM GovernorateVM)
        {
            Governorate GovernorateObj = new Governorate();
            try
            {
                if (GovernorateVM != null)
                {

                    GovernorateObj.Code = GovernorateVM.Code;
                    GovernorateObj.Name = GovernorateVM.Name;
                    GovernorateObj.NameAr = GovernorateVM.NameAr;
                    GovernorateObj.Population = GovernorateVM.Population;
                    GovernorateObj.Area = GovernorateVM.Area;
                    GovernorateObj.Latitude = GovernorateVM.Latitude;
                    GovernorateObj.Longtitude = GovernorateVM.Longtitude;
                    _context.Governorates.Add(GovernorateObj);
                    _context.SaveChanges();


                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return GovernorateObj.Id;
        }

        public int Delete(int id)
        {
            var GovernorateObj = _context.Governorates.Find(id);
            try
            {
                if (GovernorateObj != null)
                {
                    _context.Governorates.Remove(GovernorateObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return 0;
        }

        public int Update(EditGovernorateVM GovernorateVM)
        {
            try
            {

                var GovernorateObj = _context.Governorates.Find(GovernorateVM.Id);
                GovernorateObj.Code = GovernorateVM.Code;
                GovernorateObj.Name = GovernorateVM.Name;
                GovernorateObj.NameAr = GovernorateVM.NameAr;
                GovernorateObj.Population = GovernorateVM.Population;
                GovernorateObj.Area = GovernorateVM.Area;
                GovernorateObj.Latitude = GovernorateVM.Latitude;
                GovernorateObj.Longtitude = GovernorateVM.Longtitude;
                _context.Entry(GovernorateObj).State = EntityState.Modified;
                _context.SaveChanges();
                return GovernorateObj.Id;



            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return 0;
        }

        public IEnumerable<Governorate> GetAllGovernorates()
        {
            return _context.Governorates.ToList();
        }

        public EditGovernorateVM GetGovernorateByName(string govName)
        {
            return _context.Governorates.Where(a => (a.Name == govName || a.NameAr == govName)).Select(item => new EditGovernorateVM
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                NameAr = item.NameAr,
                Population = item.Population,
                Area = item.Area,
                Latitude = item.Latitude,
                Longtitude = item.Longtitude,
                Logo = item.Logo
            }).First();



        }

        public IEnumerable<GovernorateWithHospitalsVM> GetGovernorateWithHospitals()
        {
            return _context.Governorates.ToList().Select(item => new GovernorateWithHospitalsVM
            {
                Id = item.Id,
                Name = item.Name,
                NameAr = item.NameAr,
                Latitude = item.Latitude,
                Longtitude = item.Longtitude,
                Population = item.Population,
                Area = item.Area,
                Logo = item.Logo,
                HospitalsCount = _context.Hospitals.Where(h => h.GovernorateId == item.Id).ToList().Count()
            }).OrderByDescending(a=>a.HospitalsCount).ToList();
        }
    }
}