using Asset.Domain.Repositories;
using Asset.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Asset.Core.Repositories
{
    public class RoomRepositories : IRoomRepository
    {
        private ApplicationDbContext _context;

        public RoomRepositories(ApplicationDbContext context)
        {
            _context = context;
        }


        public Room GetById(int id)
        {
            return _context.Rooms.Find(id);
        }




        public IEnumerable<Room> GetAll()
        {
            return _context.Rooms.ToList();
        }

        public int Add(Room roomVM)
        {
            Room roomObj = new Room();
            try
            {
                if (roomVM != null)
                {
                    roomObj.Code = roomVM.Code;
                    roomObj.Name = roomVM.Name;
                    roomObj.NameAr = roomVM.NameAr;
                    roomObj.FloorId = roomVM.FloorId;
                    roomObj.HospitalId = roomVM.HospitalId;
                    _context.Rooms.Add(roomObj);
                    _context.SaveChanges();


                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return roomObj.Id;
        }

        public int Delete(int id)
        {
            var roomObj = _context.Rooms.Find(id);
            try
            {
                if (roomObj != null)
                {
                    _context.Rooms.Remove(roomObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return 0;
        }

        public int Update(Room roomVM)
        {
            try
            {

                var roomObj = _context.Rooms.Find(roomVM.Id);
                roomObj.Code = roomVM.Code;
                roomObj.Name = roomVM.Name;
                roomObj.NameAr = roomVM.NameAr;
                roomObj.FloorId = roomVM.FloorId; 
                roomObj.HospitalId = roomVM.HospitalId;
                _context.Entry(roomObj).State = EntityState.Modified;
                _context.SaveChanges();
                return roomObj.Id;



            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return 0;
        }

        public IEnumerable<Room> GetAllFloors()
        {
            return _context.Rooms.ToList();
        }

        public IEnumerable<Room> GetAllRoomsByFloorId(int floorId)
        {
            return _context.Rooms.Where(a => a.FloorId == floorId).ToList();
        }
    }
}