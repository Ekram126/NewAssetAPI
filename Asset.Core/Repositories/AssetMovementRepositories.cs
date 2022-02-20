using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.AssetDetailAttachmentVM;
using Asset.ViewModels.AssetDetailVM;
using Asset.ViewModels.AssetMovementVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class AssetMovementRepositories : IAssetMovementRepository
    {
        private ApplicationDbContext _context;


        public AssetMovementRepositories(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Add(CreateAssetMovementVM movementObj)
        {
            AssetMovement assetMovementObj = new AssetMovement();
            try
            {
                if (movementObj != null)
                {
                    assetMovementObj.MovementDate = movementObj.MovementDate;
                    assetMovementObj.RoomId = movementObj.RoomId;
                    assetMovementObj.FloorId = movementObj.FloorId;
                    assetMovementObj.BuildingId = movementObj.BuildingId;
                    assetMovementObj.AssetDetailId = movementObj.AssetDetailId;
                    assetMovementObj.MoveDesc = movementObj.MoveDesc;
                    _context.AssetMovements.Add(assetMovementObj);
                    _context.SaveChanges();

                    var assetDetailObj = _context.AssetDetails.Find(movementObj.AssetDetailId);
                    assetDetailObj.RoomId = movementObj.RoomId;
                    assetDetailObj.FloorId = movementObj.FloorId;
                    assetDetailObj.BuildingId = movementObj.BuildingId;
                    _context.Entry(assetDetailObj).State = EntityState.Modified;
                    _context.SaveChanges();

                    return assetMovementObj.Id;
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return assetMovementObj.Id;
        }

        public int Delete(int id)
        {
            var assetMovementObj = _context.AssetMovements.Find(id);
            try
            {
                if (assetMovementObj != null)
                {
                    _context.AssetMovements.Remove(assetMovementObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }

            return 0;
        }

        public IEnumerable<AssetMovement> GetAllAssetMovements()
        {
            return _context.AssetMovements.ToList();
        }

        IEnumerable<IndexAssetMovementVM.GetData> IAssetMovementRepository.GetAll()
        {
            return _context.AssetMovements.ToList().OrderByDescending(a => a.MovementDate).Select(item => new IndexAssetMovementVM.GetData
            {
                Id = item.Id,
                MovementDate = item.MovementDate,
                RoomName = _context.Rooms.ToList().Where(a => a.Id == item.RoomId).ToList().First().Name,
                RoomNameAr = _context.Rooms.ToList().Where(a => a.Id == item.RoomId).ToList().First().NameAr,
                FloorName = _context.Floors.ToList().Where(a => a.Id == item.FloorId).ToList().First().Name,
                FloorNameAr = _context.Floors.ToList().Where(a => a.Id == item.FloorId).ToList().First().NameAr,
                BuildingName = _context.Buildings.ToList().Where(a => a.Id == item.BuildingId).ToList().First().Name,
                BuildingNameAr = _context.Buildings.ToList().Where(a => a.Id == item.BuildingId).ToList().First().NameAr,

                AssetDetailId = item.AssetDetailId
            }).ToList();
        }

        public IEnumerable<IndexAssetMovementVM.GetData> GetMovementByAssetDetailId(int assetId)
        {

            List<IndexAssetMovementVM.GetData> list = new List<IndexAssetMovementVM.GetData>();
            var lstMovements = _context.AssetMovements.ToList().Where(a => a.AssetDetailId == assetId).OrderByDescending(a => a.MovementDate).ToList();
            if (lstMovements.Count > 0)
            {
                foreach (var item in lstMovements)
                {
                    IndexAssetMovementVM.GetData getDataObj = new IndexAssetMovementVM.GetData();
                    getDataObj.Id = item.Id;
                    getDataObj.AssetDetailId = item.AssetDetailId;
                    getDataObj.MovementDate = item.MovementDate;
                    var lstRooms = _context.Rooms.ToList().Where(a => a.Id == item.RoomId).ToList();
                    if (lstRooms.Count > 0)
                    {
                        getDataObj.RoomName = lstRooms[0].Name;
                        getDataObj.RoomNameAr = lstRooms[0].NameAr;
                    }
                    var lstFloors = _context.Floors.ToList().Where(a => a.Id == item.FloorId).ToList();
                    if (lstFloors.Count > 0)
                    {
                        getDataObj.FloorName = lstFloors[0].Name;
                        getDataObj.FloorNameAr = lstFloors[0].NameAr;
                    }
                    var lstBuildings = _context.Buildings.ToList().Where(a => a.Id == item.BuildingId).ToList();
                    if (lstBuildings.Count > 0)
                    {
                        getDataObj.BuildingName = lstBuildings[0].Name;
                        getDataObj.BuildingNameAr = lstBuildings[0].NameAr;
                    }
                    list.Add(getDataObj);
                }
            }
            return list;
        }

        public AssetMovement GetById(int id)
        {
            var assetMovementObj = _context.AssetMovements.ToList().Where(a => a.Id == id).Select(item => new AssetMovement
            {
                Id = item.Id,
                MovementDate = item.MovementDate,
                BuildingId = item.Id,
                FloorId = item.FloorId,
                RoomId = item.RoomId,
                AssetDetailId = item.AssetDetailId
            }).First();
            return assetMovementObj;
        }


        public int Update(EditAssetMovementVM movementObj)
        {
            try
            {

                var assetDetailObj = _context.AssetMovements.Find(movementObj.Id);
                assetDetailObj.Id = movementObj.Id;
                assetDetailObj.MovementDate = movementObj.MovementDate;
                assetDetailObj.AssetDetailId = movementObj.AssetDetailId;
                assetDetailObj.BuildingId = movementObj.BuildingId;
                assetDetailObj.FloorId = movementObj.FloorId;
                assetDetailObj.RoomId = movementObj.RoomId;
                assetDetailObj.MoveDesc = movementObj.MoveDesc;
                _context.Entry(assetDetailObj).State = EntityState.Modified;
                _context.SaveChanges();
                return assetDetailObj.Id;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return 0;
        }
    }


}
