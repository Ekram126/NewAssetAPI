using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.CityVM;
using Asset.ViewModels.RoleCategoryVM;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Asset.Core.Repositories
{
    public class CityRepositories : ICityRepository
    {
        private ApplicationDbContext _context;
       

        public CityRepositories(ApplicationDbContext context)
        {
            _context = context;
        }


        public City GetById(int id)
        {
            var CityObj = _context.Cities.Find(id);
            return CityObj;
        }




        public IEnumerable<IndexCityVM.GetData> GetAll()
        {
            var lstCitys = _context.Cities.ToList().Select(item => new IndexCityVM.GetData
            {
                Id = item.Id,
                Name = item.Name,
                NameAr = item.NameAr,
                Code = item.Code
            });

            return lstCitys;
        }

        public int Add(CreateCityVM cityVM)
        {
            City cityObj = new City();
            try
            {
                if (cityVM != null)
                {
                    cityObj.Code = cityVM.Code;
                    cityObj.Name = cityVM.Name;
                    cityObj.NameAr = cityVM.NameAr;
                    cityObj.GovernorateId = cityVM.GovernorateId;
                    _context.Cities.Add(cityObj);
                    _context.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                string str  = ex.Message;
            }
            return cityObj.Id;
        }

        public int Delete(int id)
        {
            var CityObj = _context.Cities.Find(id);
            try
            {
                if (CityObj != null)
                {
                    _context.Cities.Remove(CityObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }

            return 0;
        }

        public int Update(EditCityVM cityVM)
        {
            try
            {
                var cityObj = _context.Cities.Find(cityVM.Id);
                cityObj.Id = cityVM.Id;
                cityObj.Code = cityVM.Code;
                cityObj.Name = cityVM.Name;
                cityObj.NameAr = cityVM.NameAr;
                cityObj.GovernorateId = cityVM.GovernorateId;
                _context.Entry(cityObj).State = EntityState.Modified;
                _context.SaveChanges();
                return cityObj.Id;
            }
            catch (Exception ex)
            {
              string  msg = ex.Message;
            }

            return 0;
        }

        public IEnumerable<IndexCityVM.GetData> GetCitiesByGovernorateId(int govId)
        {
            var lstCities = _context.Cities.ToList().Where(a => a.GovernorateId == govId).Select(item => new IndexCityVM.GetData
            {
                Id = item.Id,
                Name = item.Name,
                NameAr = item.NameAr,
                Code = item.Code
            });

            return lstCities;
        }

        public IEnumerable<IndexCityVM.GetData> GetCitiesByGovernorateName(string govName)
        {
         return   (from gov in _context.Governorates
             join city in _context.Cities on gov.Id equals city.GovernorateId
             where (gov.Name == govName || gov.NameAr == govName)
            select city).ToList().Select(item => new IndexCityVM.GetData
            {
                Id = item.Id,
                Name = item.Name,
                NameAr = item.NameAr,
                Code = item.Code
            });

             
        }


        public IEnumerable<City> GetAllCitys()
        {
            return _context.Cities.ToList();
        }

        public IEnumerable<City> GetAllCities()
        {
            return _context.Cities.ToList();
        }
        public int GetGovIdByGovernorateName(string govName)
        {
            if(govName != "null")
            {
              return _context.Governorates.Where(g => g.Name == govName).FirstOrDefault().Id;
            }
            return 0;
        }
        public int GetCityIdByName(string name)
        {
            if (name != "null")
            {
                int id= _context.Cities.Where(h => h.Name == name || h.NameAr==name).FirstOrDefault().Id;
                return id;
            }
            return 0;
        }
    }
}