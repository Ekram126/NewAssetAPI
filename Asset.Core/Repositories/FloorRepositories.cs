using Asset.Domain.Repositories;
using Asset.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Asset.Core.Repositories
{
    public class FloorRepositories : IFloorRepository
    {
        private ApplicationDbContext _context;
        string msg;

        public FloorRepositories(ApplicationDbContext context)
        {
            _context = context;
        }


        public Floor GetById(int id)
        {
            return _context.Floors.Find(id);
        }




        public IEnumerable<Floor> GetAll()
        {
            return _context.Floors.ToList();
        }

        public int Add(Floor floorVM)
        {
            Floor floorObj = new Floor();
            try
            {
                if (floorVM != null)
                {
                    floorObj.Code = floorVM.Code;
                    floorObj.Name = floorVM.Name;
                    floorObj.NameAr = floorVM.NameAr;
                    floorObj.BuildingId = floorVM.BuildingId;
                    floorObj.HospitalId = floorVM.HospitalId;
                    _context.Floors.Add(floorObj);
                    _context.SaveChanges();


                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return floorObj.Id;
        }

        public int Delete(int id)
        {
            var FloorObj = _context.Floors.Find(id);
            try
            {
                if (FloorObj != null)
                {
                    var lstRooms = _context.Rooms.Where(a => a.FloorId == id).ToList();
                    if (lstRooms.Count > 0)
                    {
                        foreach (var itm in lstRooms)
                        {
                            _context.Rooms.Remove(itm);
                            _context.SaveChanges();
                        }
                    }

                    _context.Floors.Remove(FloorObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return 0;
        }

        public int Update(Floor floorVM)
        {
            try
            {

                var floorObj = _context.Floors.Find(floorVM.Id);
                floorObj.Code = floorVM.Code;
                floorObj.Name = floorVM.Name;
                floorObj.NameAr = floorVM.NameAr;
                floorObj.BuildingId = floorVM.BuildingId; 
                floorObj.HospitalId = floorVM.HospitalId;
                _context.Entry(floorObj).State = EntityState.Modified;
                _context.SaveChanges();
                return floorObj.Id;



            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return 0;
        }

        public IEnumerable<Floor> GetAllFloors()
        {
            return _context.Floors.ToList();
        }


        public IEnumerable<Floor> GetAllFloorsBybuildingId(int buildId)
        {
            return _context.Floors.Where(a => a.BuildingId == buildId).ToList();
        }
    }
}