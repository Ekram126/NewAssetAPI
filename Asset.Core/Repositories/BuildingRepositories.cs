using Asset.Domain.Repositories;
using Asset.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Asset.Core.Repositories
{
    public class BuildingRepositories : IBuildingRepository
    {
        private ApplicationDbContext _context;
        string msg;

        public BuildingRepositories(ApplicationDbContext context)
        {
            _context = context;
        }


        public Building GetById(int id)
        {
            return _context.Buildings.Find(id);
        }




        public IEnumerable<Building> GetAll()
        {
            return _context.Buildings.ToList();
        }

        public int Add(Building buildingVM)
        {
            Building buildingObj = new Building();
            try
            {
                if (buildingVM != null)
                {
              
                    buildingObj.Code = buildingVM.Code;
                    buildingObj.Name = buildingVM.Name;
                    buildingObj.NameAr = buildingVM.NameAr;
                    buildingObj.HospitalId = buildingVM.HospitalId;
                    buildingObj.Brief = buildingVM.Brief;
                    buildingObj.BriefAr = buildingVM.BriefAr;
                    _context.Buildings.Add(buildingObj);
                    _context.SaveChanges();
                 

                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return buildingObj.Id;
        }

        public int Delete(int id)
        {
            var BuildingObj = _context.Buildings.Find(id);
            try
            {
                if (BuildingObj != null)
                {

                    var lstFloors = _context.Floors.Where(a => a.BuildingId == id).ToList();
                    if (lstFloors.Count > 0)
                    {
                        foreach (var item in lstFloors)
                        {
                            var lstRooms = _context.Rooms.Where(a => a.FloorId == item.Id).ToList();
                            if (lstRooms.Count > 0)
                            {
                                foreach (var itm in lstRooms)
                                {
                                    _context.Rooms.Remove(itm);
                                    _context.SaveChanges();
                                }
                            }
                            _context.Floors.Remove(item);
                            _context.SaveChanges();
                        }
                    }



                    _context.Buildings.Remove(BuildingObj);
                    return _context.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return 0;
        }

        public int Update(Building buildingVM)
        {
            try
            {
  
                var buildingObj = _context.Buildings.Find(buildingVM.Id);
                buildingObj.Code = buildingVM.Code;
                buildingObj.Name = buildingVM.Name;
                buildingObj.NameAr = buildingVM.NameAr;
                buildingObj.HospitalId = buildingVM.HospitalId;
                buildingObj.Brief = buildingVM.Brief;
                buildingObj.BriefAr = buildingVM.BriefAr;
                _context.Entry(buildingObj).State = EntityState.Modified;
                _context.SaveChanges();
                return buildingObj.Id;



            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return 0;
        }

        public IEnumerable<Building> GetAllBuildings()
        {
           return _context.Buildings.ToList();
        }

        public IEnumerable<Building> GetAllBuildingsByHospitalId(int hospitalId)
        {
            return _context.Buildings.Where(a=>a.HospitalId ==  hospitalId).ToList();
        }
    }
}