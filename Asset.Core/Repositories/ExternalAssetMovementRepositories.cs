using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.AssetDetailAttachmentVM;
using Asset.ViewModels.AssetDetailVM;
using Asset.ViewModels.AssetMovementVM;
using Asset.ViewModels.ExternalAssetMovementVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class ExternalAssetMovementRepositories : IExternalAssetMovementRepository
    {
        private ApplicationDbContext _context;


        public ExternalAssetMovementRepositories(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Add(ExternalAssetMovement movementObj)
        {
            ExternalAssetMovement assetMovementObj = new ExternalAssetMovement();
            try
            {
                if (movementObj != null)
                {
                    assetMovementObj.MovementDate = movementObj.MovementDate;
                    assetMovementObj.AssetDetailId = movementObj.AssetDetailId;
                    assetMovementObj.Notes = movementObj.Notes;
                    assetMovementObj.HospitalName = movementObj.HospitalName;
                    assetMovementObj.MovementDate = movementObj.MovementDate;
                    _context.ExternalAssetMovements.Add(assetMovementObj);
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

        public int CreateExternalAssetMovementAttachments(ExternalAssetMovementAttachment attachObj)
        {
            ExternalAssetMovementAttachment documentObj = new ExternalAssetMovementAttachment();
            documentObj.Title = attachObj.Title;
            documentObj.FileName = attachObj.FileName;
            documentObj.ExternalAssetMovementId = attachObj.ExternalAssetMovementId;
            _context.ExternalAssetMovementAttachments.Add(documentObj);
            _context.SaveChanges();
            return attachObj.Id;
        }

        public int Delete(int id)
        {
            var externalAssetMovementObj = _context.ExternalAssetMovements.Find(id);
            try
            {
                if (externalAssetMovementObj != null)
                {
                    _context.ExternalAssetMovements.Remove(externalAssetMovementObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }

        public EditExternalAssetMovementVM GetById(int id)
        {

            EditExternalAssetMovementVM assetMovementObj = new EditExternalAssetMovementVM();
            var lstAssetMovements = _context.ExternalAssetMovements.Include(a => a.AssetDetail)
                 .Include(a => a.AssetDetail.MasterAsset).OrderByDescending(a => a.MovementDate)
                 .Where(a => a.Id == id)
                 .Select(item => new EditExternalAssetMovementVM
                 {
                     Id = item.Id,
                     AssetDetailId = item.AssetDetailId,
                     AssetName = item.AssetDetail.MasterAsset != null ? item.AssetDetail.MasterAsset.Name : "",
                     AssetNameAr = item.AssetDetail.MasterAsset != null ? item.AssetDetail.MasterAsset.NameAr : "",
                     BarCode = item.AssetDetail.Barcode,
                     SerialNumber = item.AssetDetail.SerialNumber,
                     ModelNumber = item.AssetDetail.MasterAsset != null ? item.AssetDetail.MasterAsset.ModelNumber : "",
                     MovementDate = item.MovementDate,
                     HospitalName = item.HospitalName,
                     Notes = item.Notes,
                     ListMovementAttachments = _context.ExternalAssetMovementAttachments.Where(a => a.ExternalAssetMovementId == id).ToList()
                 }).ToList();
            if (lstAssetMovements.Count > 0)
            {
                assetMovementObj = lstAssetMovements[0];
            }
            return assetMovementObj;
        }

        public IndexExternalAssetMovementVM GetExternalAssetMovements(int pageNumber, int pageSize)
        {
            IndexExternalAssetMovementVM mainClass = new IndexExternalAssetMovementVM();

            List<IndexExternalAssetMovementVM.GetData> list = new List<IndexExternalAssetMovementVM.GetData>();

            var lstMovements = _context.ExternalAssetMovements.Include(a => a.AssetDetail)
                .Include(a => a.AssetDetail.MasterAsset).OrderByDescending(a => a.MovementDate)
                .Select(item => new IndexExternalAssetMovementVM.GetData
                {
                    Id = item.Id,
                    AssetDetailId = item.AssetDetailId,
                    AssetName = item.AssetDetail.MasterAsset != null ? item.AssetDetail.MasterAsset.Name : "",
                    AssetNameAr = item.AssetDetail.MasterAsset != null ? item.AssetDetail.MasterAsset.NameAr : "",
                    BarCode = item.AssetDetail.Barcode,
                    SerialNumber = item.AssetDetail.SerialNumber,
                    ModelNumber = item.AssetDetail.MasterAsset != null ? item.AssetDetail.MasterAsset.ModelNumber : "",
                    MovementDate = item.MovementDate,
                    HospitalName = item.HospitalName,
                    Notes = item.Notes
                }).ToList();
            var movementPerPage = lstMovements.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            mainClass.Results = movementPerPage;
            mainClass.Count = lstMovements.Count();
            return mainClass;
        }

        public IEnumerable<ExternalAssetMovementAttachment> GetExternalMovementAttachmentByExternalAssetMovementId(int externalAssetMovementId)
        {
            return _context.ExternalAssetMovementAttachments.ToList().Where(a => a.ExternalAssetMovementId == externalAssetMovementId).ToList();
        }

        public IEnumerable<ExternalAssetMovement> GetExternalMovementsByAssetDetailId(int assetId)
        {
            List<ExternalAssetMovement> list = new List<ExternalAssetMovement>();

            var lstMovements = _context.ExternalAssetMovements.ToList().Where(a => a.AssetDetailId == assetId).OrderByDescending(a => a.MovementDate).ToList();
            if (lstMovements.Count > 0)
            {
                foreach (var item in lstMovements)
                {
                    ExternalAssetMovement getDataObj = new ExternalAssetMovement();
                    getDataObj.Id = item.Id;
                    getDataObj.AssetDetailId = item.AssetDetailId;
                    getDataObj.MovementDate = item.MovementDate;
                    getDataObj.HospitalName = item.HospitalName;
                    getDataObj.Notes = item.Notes;
                    list.Add(getDataObj);
                }
            }
            return list;
        }

        public IndexExternalAssetMovementVM SearchExternalAssetMovement(SearchExternalAssetMovementVM searchObj, int pageNumber, int pageSize)
        {
            IndexExternalAssetMovementVM mainClass = new IndexExternalAssetMovementVM();
            List<IndexExternalAssetMovementVM.GetData> list = new List<IndexExternalAssetMovementVM.GetData>();

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




            var lstExternalMovement = _context.ExternalAssetMovements
                            .Include(a => a.AssetDetail)
                            .Include(a => a.AssetDetail.Hospital)
                            .Include(a => a.AssetDetail.Supplier)
                            .Include(a => a.AssetDetail.MasterAsset)
                            .Include(a => a.AssetDetail.MasterAsset.brand)
                            .ToList();

            foreach (var detail in lstExternalMovement)
            {
                IndexExternalAssetMovementVM.GetData item = new IndexExternalAssetMovementVM.GetData();
                item.Id = detail.Id;
                item.BarCode = detail.AssetDetail.Barcode;
                item.SerialNumber = detail.AssetDetail.SerialNumber;
                item.DepartmentId = detail.AssetDetail.DepartmentId;
                item.ModelNumber = detail.AssetDetail.MasterAsset.ModelNumber;
                item.OriginId = detail.AssetDetail.MasterAsset.OriginId;
                item.MasterAssetId = detail.AssetDetail.MasterAsset.Id;
                item.AssetName = detail.AssetDetail.MasterAsset.Name;
                item.AssetNameAr = detail.AssetDetail.MasterAsset.NameAr;
                item.MovementDate = detail.MovementDate;
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

                item.HospitalId = detail.AssetDetail.HospitalId;
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

        public int Update(ExternalAssetMovement movementObj)
        {
            try
            {

                var assetDetailObj = _context.ExternalAssetMovements.Find(movementObj.Id);
                assetDetailObj.Id = movementObj.Id;
                assetDetailObj.AssetDetailId = movementObj.AssetDetailId;
                assetDetailObj.MovementDate = movementObj.MovementDate;
                assetDetailObj.Notes = movementObj.Notes;
                assetDetailObj.HospitalName = movementObj.HospitalName;
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
