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
                    assetMovementObj.HospitalId = movementObj.HospitalId;

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

        public IndexAssetMovementVM GetAll(int pageNumber, int pageSize)
        {

            IndexAssetMovementVM mainClass = new IndexAssetMovementVM();
            List<IndexAssetMovementVM.GetData> list = new List<IndexAssetMovementVM.GetData>();

            list = _context.AssetMovements
                .Include(a => a.Building)
                   .Include(a => a.Room)
                      .Include(a => a.Floor)
                .Include(a => a.AssetDetail).Include(a => a.AssetDetail.MasterAsset).ToList()
                .OrderByDescending(a => a.MovementDate).Select(item => new IndexAssetMovementVM.GetData
                {
                    Id = item.Id,
                    MovementDate = item.MovementDate,
                    MoveDesc = item.MoveDesc,
                    HospitalId = item.HospitalId,
                    RoomName = item.Room != null ? item.Room.Name : "",
                    RoomNameAr = item.Room != null ? item.Room.NameAr : "",
                    FloorName = item.Floor != null ? item.Floor.Name : "",
                    FloorNameAr = item.Floor != null ? item.Floor.NameAr : "",
                    BuildingName = item.Building != null ? item.Building.Name : "",
                    BuildingNameAr = item.Building != null ? item.Building.NameAr : "",

                    AssetName = item.AssetDetail.MasterAsset != null ? item.AssetDetail.MasterAsset.Name : "",
                    AssetNameAr = item.AssetDetail.MasterAsset != null ? item.AssetDetail.MasterAsset.NameAr : "",
                    BarCode = item.AssetDetail != null ? item.AssetDetail.Barcode : "",
                    SerialNumber = item.AssetDetail != null ? item.AssetDetail.SerialNumber : "",
                    ModelNumber = item.AssetDetail.MasterAsset != null ? item.AssetDetail.MasterAsset.ModelNumber : "",
                    AssetDetailId = item.AssetDetailId
                }).ToList();

            var movementPerPage = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            mainClass.Results = movementPerPage;
            mainClass.Count = list.Count();
            return mainClass;
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
                    getDataObj.HospitalId = item.HospitalId;
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
                HospitalId = item.HospitalId,
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
                assetDetailObj.HospitalId = movementObj.HospitalId;
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

        public IndexAssetMovementVM SearchAssetMovement(SearchAssetMovementVM searchObj, int pageNumber, int pageSize)
        {
            IndexAssetMovementVM mainClass = new IndexAssetMovementVM();
            List<IndexAssetMovementVM.GetData> list = new List<IndexAssetMovementVM.GetData>();

            //ApplicationUser userObj = new ApplicationUser();
            //ApplicationRole roleObj = new ApplicationRole();
            //List<string> userRoleNames = new List<string>();
            //var lstUsers = _context.ApplicationUser.Where(a => a.Id == searchObj.UserId).ToList();
            //if (lstUsers.Count > 0)
            //{
            //    userObj = lstUsers[0];
            //    var roles = (from userRole in _context.UserRoles
            //                 join role in _context.ApplicationRole on userRole.RoleId equals role.Id
            //                 where userRole.UserId == searchObj.UserId
            //                 select role);
            //    foreach (var role in roles)
            //    {
            //        userRoleNames.Add(role.Name);
            //    }
            //}




            var lstMovement = _context.AssetMovements
                            .Include(a => a.AssetDetail)
                            .Include(a => a.AssetDetail.Hospital)
                            .Include(a => a.AssetDetail.Supplier)
                            .Include(a => a.AssetDetail.MasterAsset)
                            .Include(a => a.AssetDetail.MasterAsset.brand)
                            .Include(a => a.Building)
                            .Include(a => a.Room)
                            .Include(a => a.Floor)
                 .Include(a => a.AssetDetail).Include(a => a.AssetDetail.MasterAsset).ToList();

            foreach (var detail in lstMovement)
            {
                IndexAssetMovementVM.GetData item = new IndexAssetMovementVM.GetData();
                item.Id = detail.Id;
                item.BarCode = detail.AssetDetail.Barcode;
                item.SerialNumber = detail.AssetDetail.SerialNumber;
                item.DepartmentId = detail.AssetDetail.DepartmentId;
                item.HospitalId = detail.AssetDetail.HospitalId;
                item.ModelNumber = detail.AssetDetail.MasterAsset.ModelNumber;
                item.OriginId = detail.AssetDetail.MasterAsset.OriginId;
                item.MasterAssetId = detail.AssetDetail.MasterAsset.Id;
                item.AssetName = detail.AssetDetail.MasterAsset.Name;
                item.AssetNameAr = detail.AssetDetail.MasterAsset.NameAr;
                item.MovementDate = detail.MovementDate;
                if (detail.RoomId != null)
                {
                    var lstRooms = _context.Rooms.ToList().Where(a => a.Id == detail.RoomId).ToList();
                    if (lstRooms.Count > 0)
                    {
                        item.RoomName = lstRooms[0].Name;
                        item.RoomNameAr = lstRooms[0].NameAr;
                    }
                }
                if (detail.FloorId != null)
                {
                    var lstFloors = _context.Floors.ToList().Where(a => a.Id == detail.FloorId).ToList();
                    if (lstFloors.Count > 0)
                    {
                        item.FloorName = lstFloors[0].Name;
                        item.FloorNameAr = lstFloors[0].NameAr;
                    }
                }
                if (detail.BuildingId != null)
                {
                    var lstBuildings = _context.Buildings.ToList().Where(a => a.Id == detail.BuildingId).ToList();
                    if (lstBuildings.Count > 0)
                    {
                        item.BuildingName = lstBuildings[0].Name;
                        item.BuildingNameAr = lstBuildings[0].NameAr;
                    }
                }


                if (detail.AssetDetail.MasterAsset.brand != null)
                {
                    item.BrandId = detail.AssetDetail.MasterAsset.BrandId;
                    item.BrandName = detail.AssetDetail.MasterAsset.brand.Name;
                    item.BrandNameAr = detail.AssetDetail.MasterAsset.brand.NameAr;
                }
                if (detail.AssetDetail.Supplier != null)
                {
                    item.SupplierId = detail.AssetDetail.SupplierId;
                    item.SupplierName = detail.AssetDetail.Supplier.Name;
                    item.SupplierNameAr = detail.AssetDetail.Supplier.NameAr;
                }

                item.GovernorateId = detail.AssetDetail.Hospital.GovernorateId;
                item.CityId = detail.AssetDetail.Hospital.CityId;
                item.OrganizationId = detail.AssetDetail.Hospital.OrganizationId;
                item.SubOrganizationId = detail.AssetDetail.Hospital.SubOrganizationId;
                list.Add(item);
            }



            if (searchObj.GovernorateId > 0 && searchObj.CityId > 0 && searchObj.HospitalId > 0)
            {
                list = list.Where(a => a.HospitalId == searchObj.HospitalId).ToList();
            }
            else if (searchObj.GovernorateId == 0 && searchObj.CityId == 0 && searchObj.OrganizationId == 0 && searchObj.SubOrganizationId == 0 && searchObj.HospitalId == 0)
            {
                list = list.ToList();
            }
            else if (searchObj.GovernorateId > 0 && searchObj.CityId == 0 && searchObj.OrganizationId == 0 && searchObj.SubOrganizationId == 0 && searchObj.HospitalId == 0)
            {
                list = list.Where(a => a.GovernorateId == searchObj.GovernorateId).ToList();
            }
            else if (searchObj.GovernorateId > 0 && searchObj.CityId > 0 && searchObj.OrganizationId == 0 && searchObj.SubOrganizationId == 0 && searchObj.HospitalId == 0)
            {
                list = list.Where(a => a.CityId == searchObj.CityId).ToList();
            }
            else if (searchObj.GovernorateId == 0 && searchObj.CityId == 0 && searchObj.OrganizationId > 0 && searchObj.SubOrganizationId == 0 && searchObj.HospitalId == 0)
            {
                list = list.Where(a => a.OrganizationId == searchObj.OrganizationId).ToList();
            }
            else if (searchObj.GovernorateId == 0 && searchObj.CityId == 0 && searchObj.OrganizationId > 0 && searchObj.SubOrganizationId > 0 && searchObj.HospitalId == 0)
            {
                list = list.Where(a => a.SubOrganizationId == searchObj.SubOrganizationId).ToList();
            }
            else if (searchObj.GovernorateId > 0 && searchObj.CityId > 0 && searchObj.OrganizationId > 0 && searchObj.SubOrganizationId > 0 && searchObj.HospitalId == 0)
            {
                list = list.Where(a => a.SubOrganizationId == searchObj.SubOrganizationId).ToList();
            }
            else if (searchObj.GovernorateId > 0 && searchObj.CityId > 0 && searchObj.OrganizationId > 0 && searchObj.SubOrganizationId == 0 && searchObj.HospitalId == 0)
            {
                list = list.Where(a => a.OrganizationId == searchObj.OrganizationId).ToList();
            }

            if (list.Count > 0)
            {
                if (searchObj.ModelNumber != "")
                {
                    list = list.Where(b => b.ModelNumber == searchObj.ModelNumber).ToList();
                }
                else
                    list = list.ToList();

                if (searchObj.OriginId != 0)
                {
                    list = list.Where(a => a.OriginId == searchObj.OriginId).ToList();
                }
                else
                    list = list.ToList();

                if (searchObj.BrandId != 0)
                {
                    list = list.Where(a => a.BrandId == searchObj.BrandId).ToList();
                }
                else
                    list = list.ToList();

                if (searchObj.MasterAssetId != 0)
                {
                    list = list.Where(a => a.MasterAssetId == searchObj.MasterAssetId).ToList();
                }
                else
                    list = list.ToList();

                if (searchObj.DepartmentId != 0)
                {
                    list = list.Where(a => a.DepartmentId == searchObj.DepartmentId).ToList();
                }
                else
                    list = list.ToList();

                if (searchObj.SerialNumber != "")
                {
                    list = list.Where(b => b.SerialNumber == searchObj.SerialNumber).ToList();
                }
                else
                    list = list.ToList();

                if (searchObj.BarCode != "")
                {
                    list = list.Where(b => b.BarCode == searchObj.BarCode).ToList();
                }
                else
                    list = list.ToList();



                if (searchObj.SupplierId != 0)
                {
                    list = list.Where(a => a.SupplierId == searchObj.SupplierId).ToList();
                }
                else
                    list = list.ToList();


                if (searchObj.PeriorityId != 0)
                {
                    list = list.Where(b => b.PeriorityId == searchObj.PeriorityId).ToList();
                }

                if (searchObj.MasterAssetId != 0)
                {
                    list = list.Where(b => b.MasterAssetId == searchObj.MasterAssetId).ToList();
                }





                string setstartday, setstartmonth, setendday, setendmonth = "";
                DateTime startingFrom = new DateTime();
                DateTime endingTo = new DateTime();
                if (searchObj.Start == null)
                {

                }
                else
                {
                    searchObj.StartDate = DateTime.Parse(searchObj.Start.ToString());
                    var startyear = searchObj.StartDate.Value.Year;
                    var startmonth = searchObj.StartDate.Value.Month;
                    var startday = searchObj.StartDate.Value.Day;
                    if (startday < 10)
                        setstartday = searchObj.StartDate.Value.Day.ToString().PadLeft(2, '0');
                    else
                        setstartday = searchObj.StartDate.Value.Day.ToString();

                    if (startmonth < 10)
                        setstartmonth = searchObj.StartDate.Value.Month.ToString().PadLeft(2, '0');
                    else
                        setstartmonth = searchObj.StartDate.Value.Month.ToString();

                    var sDate = startyear + "/" + setstartmonth + "/" + setstartday;
                    startingFrom = DateTime.Parse(sDate);
                }

                if (searchObj.End == null)
                {
                }
                else
                {
                    searchObj.EndDate = DateTime.Parse(searchObj.End.ToString());
                    var endyear = searchObj.EndDate.Value.Year;
                    var endmonth = searchObj.EndDate.Value.Month;
                    var endday = searchObj.EndDate.Value.Day;
                    if (endday < 10)
                        setendday = searchObj.EndDate.Value.Day.ToString().PadLeft(2, '0');
                    else
                        setendday = searchObj.EndDate.Value.Day.ToString();
                    if (endmonth < 10)
                        setendmonth = searchObj.EndDate.Value.Month.ToString().PadLeft(2, '0');
                    else
                        setendmonth = searchObj.EndDate.Value.Month.ToString();
                    var eDate = endyear + "/" + setendmonth + "/" + setendday;
                    endingTo = DateTime.Parse(eDate);
                }
                if (searchObj.Start != "" && searchObj.End != "")
                {
                    list = list.Where(a => a.MovementDate.Value.Date >= startingFrom.Date && a.MovementDate.Value.Date <= endingTo.Date).ToList();
                }
            }
            var movementPerPage = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            mainClass.Results = movementPerPage;
            mainClass.Count = list.Count();
            return mainClass;
        }
    }
}
