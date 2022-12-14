using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.AssetDetailAttachmentVM;
using Asset.ViewModels.AssetDetailVM;
using Asset.ViewModels.BrandVM;
using Asset.ViewModels.CityVM;
using Asset.ViewModels.GovernorateVM;
using Asset.ViewModels.HospitalVM;
using Asset.ViewModels.OrganizationVM;
using Asset.ViewModels.PMAssetTaskScheduleVM;
using Asset.ViewModels.PMAssetTaskVM;
using Asset.ViewModels.PmAssetTimeVM;
using Asset.ViewModels.SupplierVM;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using QRCoder;
using System.Drawing;


using System.Threading.Tasks;
using System.IO;
using Asset.ViewModels.RequestVM;
using Asset.ViewModels.WorkOrderVM;

namespace Asset.Core.Repositories
{
    public class AssetDetailRepositories : IAssetDetailRepository
    {
        private ApplicationDbContext _context;
        public AssetDetailRepositories(ApplicationDbContext context)
        {
            _context = context;
        }
        public int Add(CreateAssetDetailVM model)
        {
            AssetDetail assetDetailObj = new AssetDetail();
            try
            {
                if (model != null)
                {
                    assetDetailObj.Code = model.Code;
                    if (model.PurchaseDate != "")
                        assetDetailObj.PurchaseDate = DateTime.Parse(model.PurchaseDate);
                    assetDetailObj.Price = model.Price;
                    assetDetailObj.SerialNumber = model.SerialNumber;
                    assetDetailObj.Remarks = model.Remarks;
                    assetDetailObj.Barcode = model.Barcode;
                    if (model.InstallationDate != "")
                        assetDetailObj.InstallationDate = DateTime.Parse(model.InstallationDate);
                    assetDetailObj.RoomId = model.RoomId;
                    assetDetailObj.FloorId = model.FloorId;
                    assetDetailObj.BuildingId = model.BuildingId;
                    if (model.ReceivingDate != "")
                        assetDetailObj.ReceivingDate = DateTime.Parse(model.ReceivingDate);
                    if (model.OperationDate != "")
                        assetDetailObj.OperationDate = DateTime.Parse(model.OperationDate);
                    assetDetailObj.PONumber = model.PONumber;
                    assetDetailObj.DepartmentId = model.DepartmentId;
                    if (model.SupplierId > 0)
                        assetDetailObj.SupplierId = model.SupplierId;
                    assetDetailObj.HospitalId = model.HospitalId;
                    assetDetailObj.MasterAssetId = model.MasterAssetId;
                    if (model.WarrantyStart != "")
                        assetDetailObj.WarrantyStart = DateTime.Parse(model.WarrantyStart);
                    if (model.WarrantyEnd != "")
                        assetDetailObj.WarrantyEnd = DateTime.Parse(model.WarrantyEnd);

                    assetDetailObj.CreatedBy = model.CreatedBy;
                    assetDetailObj.DepreciationRate = model.DepreciationRate;
                    assetDetailObj.CostCenter = model.CostCenter;
                    assetDetailObj.WarrantyExpires = model.WarrantyExpires;
                    assetDetailObj.FixCost = model.FixCost;

                    _context.AssetDetails.Add(assetDetailObj);
                    _context.SaveChanges();

                    model.Id = assetDetailObj.Id;

                    int assetDetailId = model.Id;
                    if (model.ListOwners.Count > 0)
                    {
                        foreach (var item in model.ListOwners)
                        {
                            AssetOwner ownerObj = new AssetOwner();
                            ownerObj.AssetDetailId = assetDetailId;
                            ownerObj.EmployeeId = int.Parse(item.ToString());
                            ownerObj.HospitalId = int.Parse(model.HospitalId.ToString());
                            _context.AssetOwners.Add(ownerObj);
                            _context.SaveChanges();
                        }
                    }
                    if (model.MasterAssetId > 0 && model.InstallationDate != null)
                    {
                        var dates = new List<DateTime>();
                        var masterObj = _context.MasterAssets.Find(model.MasterAssetId);
                        var pmtimeId = masterObj.PMTimeId;
                        if (pmtimeId == 1)
                        {
                            var pmdate = DateTime.Parse(model.InstallationDate).AddYears(1);
                            if (pmdate.DayOfWeek == DayOfWeek.Friday)
                            {
                                pmdate = pmdate.AddDays(-1);
                            }
                            if (pmdate.DayOfWeek == DayOfWeek.Saturday)
                            {
                                pmdate = pmdate.AddDays(1);
                            }

                        }
                        if (pmtimeId == 2)
                        {

                            for (var dt = DateTime.Parse(model.InstallationDate).AddDays(1); dt <= DateTime.Parse(model.InstallationDate).AddYears(1); dt = dt.AddMonths(6))
                            {
                                if (dt.DayOfWeek == DayOfWeek.Friday)
                                {
                                    dt = dt.AddDays(-1);
                                }
                                if (dt.DayOfWeek == DayOfWeek.Saturday)
                                {
                                    dt = dt.AddDays(1);
                                }

                                dates.Add(dt);
                            }

                        }
                        if (pmtimeId == 3)
                        {
                            for (var dt = DateTime.Parse(model.InstallationDate).AddDays(1); dt <= DateTime.Parse(model.InstallationDate).AddYears(1); dt = dt.AddMonths(3))
                            {
                                if (dt.DayOfWeek == DayOfWeek.Friday)
                                {
                                    dt = dt.AddDays(-1);
                                }
                                if (dt.DayOfWeek == DayOfWeek.Saturday)
                                {
                                    dt = dt.AddDays(1);
                                }

                                dates.Add(dt);
                            }
                        }

                        if (dates.Count > 0)
                        {
                            foreach (var item in dates)
                            {
                                PMAssetTime assetTimeObj = new PMAssetTime();
                                assetTimeObj.PMDate = item;
                                assetTimeObj.AssetDetailId = assetDetailId;
                                assetTimeObj.HospitalId = model.HospitalId;
                                _context.PMAssetTimes.Add(assetTimeObj);
                                _context.SaveChanges();
                            }
                        }

                    }

                    if (model.BuildingId != 0 && model.FloorId != 0 && model.RoomId != 0)
                    {
                        AssetMovement movementObj = new AssetMovement();
                        movementObj.RoomId = model.RoomId;
                        movementObj.FloorId = model.FloorId;
                        movementObj.BuildingId = model.BuildingId;
                        movementObj.HospitalId = model.HospitalId;
                        _context.AssetMovements.Add(movementObj);
                        _context.SaveChanges();
                    }

                    AssetStatusTransaction transObj = new AssetStatusTransaction();
                    transObj.AssetDetailId = assetDetailId;
                    transObj.AssetStatusId = int.Parse(model.AssetStatusId.ToString());
                    transObj.StatusDate = DateTime.Today.Date;
                    transObj.HospitalId = model.HospitalId;
                    _context.AssetStatusTransactions.Add(transObj);
                    _context.SaveChanges();


                    return assetDetailId;
                }
            }
            catch (SqlException ex)
            {
                string str = ex.Message;
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return assetDetailObj.Id;
        }
        public int Delete(int id)
        {
            var assetDetailObj = _context.AssetDetails.Find(id);
            try
            {
                if (assetDetailObj != null)
                {
                    var lstOwners = _context.AssetOwners.Where(a => a.AssetDetailId == id).ToList();
                    if (lstOwners.Count > 0)
                    {
                        foreach (var ownr in lstOwners)
                        {
                            _context.AssetOwners.Remove(ownr);
                            _context.SaveChanges();
                        }

                    }
                    _context.AssetDetails.Remove(assetDetailObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }

            return 0;
        }
        public IEnumerable<IndexAssetDetailVM.GetData> GetAll()
        {
            var lstAssetDetails = _context.AssetDetails.Include(a => a.MasterAsset)
                .Include(a => a.Hospital)
                .Include(a => a.Hospital.Governorate)
                .Include(a => a.Hospital.City)
                .ToList().Select(item => new IndexAssetDetailVM.GetData
                {
                    Id = item.Id,
                    Code = item.Code,
                    MasterImg = item.MasterAsset.AssetImg != "" ? item.MasterAsset.AssetImg : "",
                    Model = item.MasterAsset.ModelNumber,
                    Price = item.Price,
                    Serial = item.SerialNumber,
                    BarCode = item.Barcode,
                    SerialNumber = item.SerialNumber,
                    PurchaseDate = item.PurchaseDate,
                    SupplierId = item.SupplierId,
                    DepartmentId = item.DepartmentId,
                    HospitalName = item.HospitalId > 0 ? item.Hospital.Name : "",
                    HospitalNameAr = item.HospitalId > 0 ? item.Hospital.NameAr : "",
                    AssetName = item.MasterAssetId > 0 ? item.MasterAsset.Name : "",
                    AssetNameAr = item.MasterAssetId > 0 ? item.MasterAsset.NameAr : "",
                    GovernorateName = item.HospitalId > 0 ? item.Hospital.Governorate.Name : "",
                    GovernorateNameAr = item.HospitalId > 0 ? item.Hospital.Governorate.NameAr : "",
                    CityName = item.HospitalId > 0 ? item.Hospital.City.Name : "",
                    CityNameAr = item.HospitalId > 0 ? item.Hospital.City.NameAr : "",
                    QrFilePath = item.QrFilePath,
                    CreatedBy = item.CreatedBy
                });

            return lstAssetDetails;
        }
        public IEnumerable<IndexAssetDetailVM.GetData> GetAssetDetailsByAssetId(int assetId)
        {
            var lstAssetDetails = _context.AssetDetails.ToList().Where(a => a.MasterAssetId == assetId).Select(item => new IndexAssetDetailVM.GetData
            {
                Id = item.Id,
                HospitalId = item.HospitalId,
                Code = item.Code,
                Price = item.Price,
                Serial = item.SerialNumber,
                BarCode = item.Barcode,
                SerialNumber = item.SerialNumber,
                PurchaseDate = item.PurchaseDate,
                CreatedBy = item.CreatedBy,
                HospitalName = _context.Hospitals.Where(a => a.Id == item.HospitalId).ToList().First().Name,
                HospitalNameAr = _context.Hospitals.Where(a => a.Id == item.HospitalId).ToList().First().NameAr,
                AssetName = _context.MasterAssets.Where(a => a.Id == item.MasterAssetId).ToList().First().Name,
                AssetNameAr = _context.MasterAssets.Where(a => a.Id == item.MasterAssetId).ToList().First().NameAr,

            });
            return lstAssetDetails;
        }
        public async Task<IEnumerable<IndexAssetDetailVM.GetData>> GetAssetDetailsByUserId(string userId)
        {
            if (userId != null)
            {
                var userObj = await _context.Users.FindAsync(userId);
                ApplicationRole roleObj = new ApplicationRole();
                Employee empObj = new Employee();
                string userRoleName = "";
                List<string> userRoleNames = new List<string>();
                var obj = _context.ApplicationUser.Where(a => a.Id == userId).ToList();
                userObj = obj[0];

                var lstRoles = _context.ApplicationRole.Where(a => a.Id == userObj.RoleId).ToList();
                if (lstRoles.Count > 0)
                {
                    roleObj = lstRoles[0];
                    userRoleName = roleObj.Name;

                    var roles = (from userRole in _context.UserRoles
                                 join role in _context.ApplicationRole on userRole.RoleId equals role.Id
                                 where userRole.UserId == userId
                                 select role);
                    foreach (var role in roles)
                    {
                        userRoleNames.Add(role.Name);
                    }
                }

                var lstEmployees = _context.Employees.Where(a => a.Email == userObj.Email).ToList();
                if (lstEmployees.Count > 0)
                {
                    empObj = lstEmployees[0];
                }

                if (userRoleNames.Contains("AssetOwner"))
                {


                    List<IndexAssetDetailVM.GetData> lstAssetDetails = _context.AssetOwners.Take(10).Include(a => a.AssetDetail)
                        .Include(a => a.AssetDetail.MasterAsset).Include(a => a.AssetDetail.Hospital)
                                      .Include(a => a.AssetDetail.Supplier).Include(a => a.AssetDetail.MasterAsset.brand)
                                    .Include(a => a.AssetDetail.Hospital.Governorate)
                                    .Include(a => a.AssetDetail.Hospital.City).Include(a => a.AssetDetail.Hospital.Organization).Include(a => a.AssetDetail.Hospital.SubOrganization)
                          .OrderBy(a => a.AssetDetail.Barcode)
                                    .Where(a => a.EmployeeId == empObj.Id && a.AssetDetail.HospitalId == userObj.HospitalId).Select(detail => new IndexAssetDetailVM.GetData
                                    {
                                        Id = detail.AssetDetail.Id,
                                        Code = detail.AssetDetail.Code,
                                        UserId = userObj.Id,
                                        CreatedBy = detail.AssetDetail.CreatedBy,
                                        Price = detail.AssetDetail.Price,
                                        BarCode = detail.AssetDetail.Barcode,
                                        Barcode = detail.AssetDetail.Barcode,
                                        MasterImg = detail.AssetDetail.MasterAsset.AssetImg,
                                        Serial = detail.AssetDetail.SerialNumber,
                                        BrandName = detail.AssetDetail.MasterAsset.brand.Name,
                                        BrandNameAr = detail.AssetDetail.MasterAsset.brand.NameAr,
                                        Model = detail.AssetDetail.MasterAsset.ModelNumber,
                                        SerialNumber = detail.AssetDetail.SerialNumber,
                                        MasterAssetId = detail.AssetDetail.MasterAssetId,
                                        PurchaseDate = detail.AssetDetail.PurchaseDate,
                                        HospitalId = detail.AssetDetail.Hospital.Id,
                                        HospitalName = detail.AssetDetail.Hospital.Name,
                                        HospitalNameAr = detail.AssetDetail.Hospital.NameAr,
                                        AssetName = detail.AssetDetail.MasterAsset.Name,
                                        AssetNameAr = detail.AssetDetail.MasterAsset.NameAr,
                                        GovernorateId = detail.AssetDetail.Hospital.GovernorateId,
                                        GovernorateName = detail.AssetDetail.Hospital.Governorate.Name,
                                        GovernorateNameAr = detail.AssetDetail.Hospital.Governorate.NameAr,
                                        CityId = detail.AssetDetail.Hospital.CityId,
                                        CityName = detail.AssetDetail.Hospital.City.Name,
                                        CityNameAr = detail.AssetDetail.Hospital.City.NameAr,
                                        OrganizationId = detail.AssetDetail.Hospital.OrganizationId,
                                        OrgName = detail.AssetDetail.Hospital.Organization.Name,
                                        OrgNameAr = detail.AssetDetail.Hospital.Organization.NameAr,
                                        SubOrganizationId = detail.AssetDetail.Hospital.SubOrganizationId,
                                        SubOrgName = detail.AssetDetail.Hospital.SubOrganization.Name,
                                        SubOrgNameAr = detail.AssetDetail.Hospital.SubOrganization.NameAr,
                                        SupplierName = detail.AssetDetail.Supplier.Name,
                                        SupplierNameAr = detail.AssetDetail.Supplier.NameAr,
                                        QrFilePath = detail.AssetDetail.QrFilePath
                                    }).ToList();

                    return lstAssetDetails;

                }
                else
                {

                    List<IndexAssetDetailVM.GetData> lstAssetDetails = await _context.AssetDetails.Take(10)
                                            .Include(a => a.Hospital).Include(a => a.MasterAsset).Include(a => a.Supplier).Include(a => a.MasterAsset.brand)
                                            .Include(a => a.Hospital.Governorate).Include(a => a.Hospital.City).Include(a => a.Hospital.Organization).Include(a => a.Hospital.SubOrganization)

                                           .Select(detail => new IndexAssetDetailVM.GetData
                                           {
                                               Id = detail.Id,
                                               Code = detail.Code,
                                               UserId = userObj.Id,
                                               Price = detail.Price,
                                               BarCode = detail.Barcode,
                                               Barcode = detail.Barcode,
                                               CreatedBy = detail.CreatedBy,
                                               MasterImg = detail.MasterAsset.AssetImg,
                                               Serial = detail.SerialNumber,
                                               BrandName = detail.MasterAsset.brand.Name,
                                               BrandNameAr = detail.MasterAsset.brand.NameAr,
                                               Model = detail.MasterAsset.ModelNumber,
                                               SerialNumber = detail.SerialNumber,
                                               MasterAssetId = detail.MasterAssetId,
                                               PurchaseDate = detail.PurchaseDate,
                                               HospitalId = detail.Hospital.Id,
                                               HospitalName = detail.Hospital.Name,
                                               HospitalNameAr = detail.Hospital.NameAr,
                                               AssetName = detail.MasterAsset.Name,
                                               AssetNameAr = detail.MasterAsset.NameAr,
                                               GovernorateId = detail.Hospital.GovernorateId,
                                               GovernorateName = detail.Hospital.Governorate.Name,
                                               GovernorateNameAr = detail.Hospital.Governorate.NameAr,
                                               CityId = detail.Hospital.CityId,
                                               CityName = detail.Hospital.City.Name,
                                               CityNameAr = detail.Hospital.City.NameAr,
                                               OrganizationId = detail.Hospital.OrganizationId,
                                               OrgName = detail.Hospital.Organization.Name,
                                               OrgNameAr = detail.Hospital.Organization.NameAr,
                                               SubOrganizationId = detail.Hospital.SubOrganizationId,
                                               SubOrgName = detail.Hospital.SubOrganization.Name,
                                               SubOrgNameAr = detail.Hospital.SubOrganization.NameAr,
                                               SupplierName = detail.Supplier.Name,
                                               SupplierNameAr = detail.Supplier.NameAr,
                                               ListAssetIds = _context.AssetOwners.Where(a => a.EmployeeId == empObj.Id).Select(a => detail.Id).ToList(),
                                               QrFilePath = detail.QrFilePath
                                           }).ToListAsync();


                    if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                    {
                        lstAssetDetails = lstAssetDetails.ToList();
                    }
                    if (userObj.GovernorateId > 0 && userObj.CityId == 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                    {
                        lstAssetDetails = lstAssetDetails.Where(a => a.GovernorateId == userObj.GovernorateId).ToList();
                    }

                    if (userObj.GovernorateId > 0 && userObj.CityId > 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                    {
                        lstAssetDetails = lstAssetDetails.Where(a => a.GovernorateId == userObj.GovernorateId && a.CityId == userObj.CityId).ToList();
                    }

                    if (userObj.GovernorateId > 0 && userObj.CityId > 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId > 0)
                    {
                        if (userRoleNames.Contains("AssetOwner"))
                        {


                            var lstAssetOwners = _context.AssetOwners.Include(a => a.AssetDetail)
                                .Include(a => a.AssetDetail.MasterAsset).Include(a => a.AssetDetail.Hospital)
                                              .Include(a => a.AssetDetail.Supplier).Include(a => a.AssetDetail.MasterAsset.brand)
                                            .Include(a => a.AssetDetail.Hospital.Governorate)
                                            .Include(a => a.AssetDetail.Hospital.City).Include(a => a.AssetDetail.Hospital.Organization).Include(a => a.AssetDetail.Hospital.SubOrganization)
                                .Where(a => a.EmployeeId == empObj.Id && a.AssetDetail.HospitalId == userObj.HospitalId).Select(detail => new IndexAssetDetailVM.GetData
                                {
                                    Id = detail.AssetDetail.Id,
                                    Code = detail.AssetDetail.Code,
                                    UserId = userObj.Id,
                                    Price = detail.AssetDetail.Price,
                                    Serial = detail.AssetDetail.SerialNumber,
                                    BarCode = detail.AssetDetail.Barcode,
                                    Barcode = detail.AssetDetail.Barcode,
                                    MasterImg = detail.AssetDetail.MasterAsset.AssetImg,
                                    BrandName = detail.AssetDetail.MasterAsset.brand.Name,
                                    BrandNameAr = detail.AssetDetail.MasterAsset.brand.NameAr,
                                    Model = detail.AssetDetail.MasterAsset.ModelNumber,
                                    SerialNumber = detail.AssetDetail.SerialNumber,
                                    MasterAssetId = detail.AssetDetail.MasterAssetId,
                                    PurchaseDate = detail.AssetDetail.PurchaseDate,
                                    HospitalId = detail.AssetDetail.Hospital.Id,
                                    HospitalName = detail.AssetDetail.Hospital.Name,
                                    HospitalNameAr = detail.AssetDetail.Hospital.NameAr,
                                    AssetName = detail.AssetDetail.MasterAsset.Name,
                                    AssetNameAr = detail.AssetDetail.MasterAsset.NameAr,
                                    GovernorateId = detail.AssetDetail.Hospital.GovernorateId,
                                    GovernorateName = detail.AssetDetail.Hospital.Governorate.Name,
                                    GovernorateNameAr = detail.AssetDetail.Hospital.Governorate.NameAr,
                                    CityId = detail.AssetDetail.Hospital.CityId,
                                    CityName = detail.AssetDetail.Hospital.City.Name,
                                    CityNameAr = detail.AssetDetail.Hospital.City.NameAr,
                                    OrganizationId = detail.AssetDetail.Hospital.OrganizationId,
                                    OrgName = detail.AssetDetail.Hospital.Organization.Name,
                                    OrgNameAr = detail.AssetDetail.Hospital.Organization.NameAr,
                                    SubOrganizationId = detail.AssetDetail.Hospital.SubOrganizationId,
                                    SubOrgName = detail.AssetDetail.Hospital.SubOrganization.Name,
                                    SubOrgNameAr = detail.AssetDetail.Hospital.SubOrganization.NameAr,
                                    SupplierName = detail.AssetDetail.Supplier.Name,
                                    SupplierNameAr = detail.AssetDetail.Supplier.NameAr,
                                    QrFilePath = detail.AssetDetail.QrFilePath
                                }).ToList();

                            lstAssetDetails = lstAssetOwners.ToList();
                        }
                        else
                        {
                            lstAssetDetails = lstAssetDetails.Where(a => a.GovernorateId == userObj.GovernorateId && a.CityId == userObj.CityId && a.HospitalId == userObj.HospitalId).ToList();
                        }
                    }
                    if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId > 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                    {
                        lstAssetDetails = lstAssetDetails.Where(a => a.OrganizationId == userObj.OrganizationId).ToList();
                    }
                    if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId > 0 && userObj.SubOrganizationId > 0 && userObj.HospitalId == 0)
                    {
                        lstAssetDetails = lstAssetDetails.Where(a => a.OrganizationId == userObj.OrganizationId && a.SubOrganizationId == userObj.SubOrganizationId).ToList();
                    }

                    if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId > 0 && userObj.SubOrganizationId > 0 && userObj.HospitalId > 0)
                    {
                        lstAssetDetails = lstAssetDetails.Where(a => a.OrganizationId == userObj.OrganizationId && a.SubOrganizationId == userObj.SubOrganizationId && a.HospitalId == userObj.HospitalId).ToList();
                    }
                    return lstAssetDetails;
                }
            }
            return null;
        }
        public async Task<IndexAssetDetailVM> GetAssetDetailsByUserId2(int pageNumber, int pageSize, string userId)
        {

            IndexAssetDetailVM mainClass = new IndexAssetDetailVM();

            List<IndexAssetDetailVM.GetData> list = new List<IndexAssetDetailVM.GetData>();

            List<IndexAssetDetailVM.GetData> lstAssetDetails = new List<IndexAssetDetailVM.GetData>();

            if (userId != null)
            {
                var userObj = await _context.Users.FindAsync(userId);
                ApplicationRole roleObj = new ApplicationRole();
                Employee empObj = new Employee();
                List<string> userRoleNames = new List<string>();
                var obj = _context.ApplicationUser.Where(a => a.Id == userId).ToList();
                userObj = obj[0];


                var roles = (from userRole in _context.UserRoles
                             join role in _context.ApplicationRole on userRole.RoleId equals role.Id
                             where userRole.UserId == userObj.Id
                             select role);
                foreach (var role in roles)
                {
                    userRoleNames.Add(role.Name);
                }

                var lstEmployees = _context.Employees.Where(a => a.Email == userObj.Email).ToList();
                if (lstEmployees.Count > 0)
                {
                    empObj = lstEmployees[0];
                }

                if (userRoleNames.Contains("AssetOwner"))
                {
                    lstAssetDetails = _context.AssetOwners.Include(a => a.AssetDetail)
                        .Include(a => a.Employee)
                        .Include(a => a.AssetDetail.MasterAsset).Include(a => a.AssetDetail.Hospital)
                                      .Include(a => a.AssetDetail.Supplier)
                                      .Include(a => a.AssetDetail.MasterAsset.brand)
                                      .Include(a => a.AssetDetail.Hospital.Governorate)
                                      .Include(a => a.AssetDetail.Hospital.City)
                                      .Include(a => a.AssetDetail.Hospital.Organization)
                                      .Include(a => a.AssetDetail.Hospital.SubOrganization)
                                          .OrderBy(a => a.AssetDetail.Barcode)
                                    // .Where(a => a.EmployeeId == empObj.Id && a.AssetDetail.HospitalId == userObj.HospitalId)
                                    .Select(detail => new IndexAssetDetailVM.GetData
                                    {
                                        Id = detail.AssetDetail.Id,
                                        Code = detail.AssetDetail.Code,
                                        UserId = userObj.Id,
                                        EmployeeId = detail.EmployeeId,
                                        CreatedBy = detail.AssetDetail.CreatedBy,
                                        Price = detail.AssetDetail.Price,
                                        BarCode = detail.AssetDetail.Barcode,
                                        Barcode = detail.AssetDetail.Barcode,
                                        MasterImg = detail.AssetDetail.MasterAsset.AssetImg,
                                        Serial = detail.AssetDetail.SerialNumber,
                                        BrandName = detail.AssetDetail.MasterAsset.brand.Name,
                                        BrandNameAr = detail.AssetDetail.MasterAsset.brand.NameAr,
                                        Model = detail.AssetDetail.MasterAsset.ModelNumber,
                                        SerialNumber = detail.AssetDetail.SerialNumber,
                                        MasterAssetId = detail.AssetDetail.MasterAssetId,
                                        PurchaseDate = detail.AssetDetail.PurchaseDate,
                                        HospitalId = detail.AssetDetail.Hospital.Id,
                                        HospitalName = detail.AssetDetail.Hospital.Name,
                                        HospitalNameAr = detail.AssetDetail.Hospital.NameAr,
                                        AssetName = detail.AssetDetail.MasterAsset.Name,
                                        AssetNameAr = detail.AssetDetail.MasterAsset.NameAr,
                                        GovernorateId = detail.AssetDetail.Hospital.GovernorateId,
                                        GovernorateName = detail.AssetDetail.Hospital.Governorate.Name,
                                        GovernorateNameAr = detail.AssetDetail.Hospital.Governorate.NameAr,
                                        CityId = detail.AssetDetail.Hospital.CityId,
                                        CityName = detail.AssetDetail.Hospital.City.Name,
                                        CityNameAr = detail.AssetDetail.Hospital.City.NameAr,
                                        OrganizationId = detail.AssetDetail.Hospital.OrganizationId,
                                        OrgName = detail.AssetDetail.Hospital.Organization.Name,
                                        OrgNameAr = detail.AssetDetail.Hospital.Organization.NameAr,
                                        SubOrganizationId = detail.AssetDetail.Hospital.SubOrganizationId,
                                        SubOrgName = detail.AssetDetail.Hospital.SubOrganization.Name,
                                        SubOrgNameAr = detail.AssetDetail.Hospital.SubOrganization.NameAr,
                                        SupplierName = detail.AssetDetail.Supplier.Name,
                                        SupplierNameAr = detail.AssetDetail.Supplier.NameAr,
                                        QrFilePath = detail.AssetDetail.QrFilePath
                                    }).ToList();
                }
                else
                {
                    lstAssetDetails = _context.AssetDetails.Include(a => a.MasterAsset).Include(a => a.Hospital).Include(a => a.Supplier).Include(a => a.MasterAsset.brand)
                                             .Include(a => a.Hospital.Governorate).Include(a => a.Hospital.City).Include(a => a.Hospital.Organization).Include(a => a.Hospital.SubOrganization)
                                              .OrderBy(a => a.Barcode)
                                             .Select(detail => new IndexAssetDetailVM.GetData
                                             {
                                                 Id = detail.Id,
                                                 Code = detail.Code,
                                                 UserId = userObj.Id,
                                                 //EmployeeId = _context.AssetOwners.Where(a => a.AssetDetailId == detail.Id).ToList().FirstOrDefault().EmployeeId,
                                                 Price = detail.Price,
                                                 CreatedBy = detail.CreatedBy,
                                                 BarCode = detail.Barcode,
                                                 Barcode = detail.Barcode,
                                                 MasterImg = detail.MasterAsset.AssetImg,
                                                 Serial = detail.SerialNumber,
                                                 BrandName = detail.MasterAsset.brand.Name,
                                                 BrandNameAr = detail.MasterAsset.brand.NameAr,
                                                 Model = detail.MasterAsset.ModelNumber,
                                                 SerialNumber = detail.SerialNumber,
                                                 MasterAssetId = detail.MasterAssetId,
                                                 PurchaseDate = detail.PurchaseDate,
                                                 HospitalId = detail.Hospital.Id,
                                                 HospitalName = detail.Hospital.Name,
                                                 HospitalNameAr = detail.Hospital.NameAr,
                                                 AssetName = detail.MasterAsset.Name,
                                                 AssetNameAr = detail.MasterAsset.NameAr,
                                                 GovernorateId = detail.Hospital.GovernorateId,
                                                 GovernorateName = detail.Hospital.Governorate.Name,
                                                 GovernorateNameAr = detail.Hospital.Governorate.NameAr,
                                                 CityId = detail.Hospital.CityId,
                                                 CityName = detail.Hospital.City.Name,
                                                 CityNameAr = detail.Hospital.City.NameAr,
                                                 OrganizationId = detail.Hospital.OrganizationId,
                                                 OrgName = detail.Hospital.Organization.Name,
                                                 OrgNameAr = detail.Hospital.Organization.NameAr,
                                                 SubOrganizationId = detail.Hospital.SubOrganizationId,
                                                 SubOrgName = detail.Hospital.SubOrganization.Name,
                                                 SubOrgNameAr = detail.Hospital.SubOrganization.NameAr,
                                                 SupplierName = detail.Supplier.Name,
                                                 SupplierNameAr = detail.Supplier.NameAr,
                                                 QrFilePath = detail.QrFilePath
                                             }).ToList();
                }


                if (userObj.HospitalId > 0)
                {
                    if (userRoleNames.Contains("AssetOwner"))
                    {
                        lstAssetDetails = lstAssetDetails.Where(a => a.EmployeeId == empObj.Id && a.HospitalId == userObj.HospitalId).ToList();
                    }
                    if (userRoleNames.Contains("EngDepManager"))
                    {
                        lstAssetDetails = lstAssetDetails.Where(a => a.HospitalId == userObj.HospitalId).ToList();
                    }
                    if (userRoleNames.Contains("TLHospitalManager"))
                    {
                        lstAssetDetails = lstAssetDetails.Where(a => a.HospitalId == userObj.HospitalId).ToList();
                    }
                }
                if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                {
                    lstAssetDetails = lstAssetDetails.ToList();
                }
                if (userObj.GovernorateId > 0 && userObj.CityId == 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                {
                    lstAssetDetails = lstAssetDetails.Where(a => a.GovernorateId == userObj.GovernorateId).ToList();
                }

                if (userObj.GovernorateId > 0 && userObj.CityId > 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                {
                    lstAssetDetails = lstAssetDetails.Where(a => a.GovernorateId == userObj.GovernorateId && a.CityId == userObj.CityId).ToList();
                }

                //if (userObj.GovernorateId > 0 && userObj.CityId > 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId > 0)
                //{
                //    lstAssetDetails = lstAssetDetails.Where(a => a.GovernorateId == userObj.GovernorateId && a.CityId == userObj.CityId && a.HospitalId == userObj.HospitalId).ToList();
                //}
                if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId > 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                {
                    lstAssetDetails = lstAssetDetails.Where(a => a.OrganizationId == userObj.OrganizationId).ToList();
                }
                if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId > 0 && userObj.SubOrganizationId > 0 && userObj.HospitalId == 0)
                {
                    lstAssetDetails = lstAssetDetails.Where(a => a.OrganizationId == userObj.OrganizationId && a.SubOrganizationId == userObj.SubOrganizationId).ToList();
                }

                //if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId > 0 && userObj.SubOrganizationId > 0 && userObj.HospitalId > 0)
                //{
                //    lstAssetDetails = lstAssetDetails.Where(a => a.OrganizationId == userObj.OrganizationId && a.SubOrganizationId == userObj.SubOrganizationId && a.HospitalId == userObj.HospitalId).ToList();
                //}

                var requestsPerPage = lstAssetDetails.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                mainClass.Results = requestsPerPage;
                mainClass.Count = lstAssetDetails.Count();
                return mainClass;
            }
            return null;
        }
        public async Task<IEnumerable<IndexAssetDetailVM.GetData>> GetAssetsByUserId(string userId)
        {
            List<IndexAssetDetailVM.GetData> list = new List<IndexAssetDetailVM.GetData>();
            if (userId != null)
            {
                var userObj = await _context.Users.FindAsync(userId);
                var lstAssetDetails = await _context.AssetDetails.Include(a => a.MasterAsset)
                            .Include(a => a.MasterAsset.brand)
                               .Include(a => a.Supplier)
                                 .Include(a => a.Department)
                           .Include(a => a.Hospital).ThenInclude(h => h.Organization)
                           .Include(a => a.Hospital).ThenInclude(h => h.Governorate)
                           .Include(a => a.Hospital).ThenInclude(h => h.City)
                           .Include(a => a.Hospital).ThenInclude(h => h.SubOrganization)
                           .OrderBy(a => a.Barcode).ToListAsync();

                foreach (var item in lstAssetDetails)
                {
                    IndexAssetDetailVM.GetData getDataobj = new IndexAssetDetailVM.GetData();

                    getDataobj.Id = item.Id;
                    getDataobj.Code = item.Code;
                    getDataobj.Price = item.Price;
                    getDataobj.CreatedBy = item.CreatedBy;
                    getDataobj.Barcode = item.Barcode;
                    getDataobj.Serial = item.SerialNumber;
                    getDataobj.SerialNumber = item.SerialNumber;
                    getDataobj.BarCode = item.Barcode;
                    getDataobj.Model = item.MasterAsset.ModelNumber;
                    getDataobj.MasterAssetId = item.MasterAssetId;
                    getDataobj.PurchaseDate = item.PurchaseDate;
                    getDataobj.HospitalId = item.HospitalId;
                    getDataobj.DepartmentId = item.DepartmentId;
                    getDataobj.HospitalName = item.Hospital.Name;
                    getDataobj.HospitalNameAr = item.Hospital.NameAr;
                    getDataobj.AssetName = item.MasterAsset.Name;
                    getDataobj.AssetNameAr = item.MasterAsset.NameAr;
                    getDataobj.GovernorateId = item.Hospital.Governorate.Id;
                    getDataobj.GovernorateName = item.Hospital.Governorate.Name;
                    getDataobj.GovernorateNameAr = item.Hospital.Governorate.NameAr;
                    getDataobj.CityId = item.Hospital.City.Id;
                    getDataobj.CityName = item.Hospital.City.Name;
                    getDataobj.CityNameAr = item.Hospital.City.NameAr;
                    getDataobj.OrganizationId = item.Hospital.Organization.Id;
                    getDataobj.OrgName = item.Hospital.Organization.Name;
                    getDataobj.OrgNameAr = item.Hospital.Organization.NameAr;
                    getDataobj.SubOrgName = item.Hospital.SubOrganization.Name;
                    getDataobj.SubOrgNameAr = item.Hospital.SubOrganization.NameAr;
                    getDataobj.SubOrganizationId = item.Hospital.SubOrganization.Id;
                    getDataobj.BrandId = item.MasterAsset.brand != null ? item.MasterAsset.brand.Id : 0;
                    getDataobj.BrandName = item.MasterAsset.brand != null ? item.MasterAsset.brand.Name : "";
                    getDataobj.BrandNameAr = item.MasterAsset.brand != null ? item.MasterAsset.brand.NameAr : "";
                    getDataobj.SupplierId = item.Supplier != null ? item.Supplier.Id : 0;
                    getDataobj.SupplierName = item.Supplier != null ? item.Supplier.Name : "";
                    getDataobj.SupplierNameAr = item.Supplier != null ? item.Supplier.NameAr : "";
                    getDataobj.DepartmentName = item.Department != null ? item.Department.Name : "";
                    getDataobj.DepartmentNameAr = item.Department != null ? item.Department.NameAr : "";

                    var lstTransactions = _context.AssetStatusTransactions.Where(a => a.AssetDetailId == item.Id).OrderByDescending(a => a.StatusDate.Value.Date).ToList();
                    if (lstTransactions.Count > 0)
                    {
                        getDataobj.AssetStatusId = lstTransactions[0].AssetStatusId;
                    }

                    list.Add(getDataobj);
                }


                if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                {
                    list = list.ToList();
                }

                if (userObj.GovernorateId > 0 && userObj.CityId == 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                {
                    list = list.Where(a => a.GovernorateId == userObj.GovernorateId).ToList();
                }

                if (userObj.GovernorateId > 0 && userObj.CityId > 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                {
                    list = list.Where(a => a.GovernorateId == userObj.GovernorateId && a.CityId == userObj.CityId).ToList();
                }

                if (userObj.GovernorateId > 0 && userObj.CityId > 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId > 0)
                {
                    list = list.Where(a => a.GovernorateId == userObj.GovernorateId && a.CityId == userObj.CityId && a.HospitalId == userObj.HospitalId).ToList();
                }
                if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId > 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                {
                    list = list.Where(a => a.OrganizationId == userObj.OrganizationId).ToList();
                }
                if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId > 0 && userObj.SubOrganizationId > 0 && userObj.HospitalId == 0)
                {
                    list = list.Where(a => a.OrganizationId == userObj.OrganizationId && a.SubOrganizationId == userObj.SubOrganizationId).ToList();
                }

                if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId > 0 && userObj.SubOrganizationId > 0 && userObj.HospitalId > 0)
                {
                    list = list.Where(a => a.OrganizationId == userObj.OrganizationId && a.SubOrganizationId == userObj.SubOrganizationId && a.HospitalId == userObj.HospitalId).ToList();
                }
                return list;
            }
            return null;

        }
        public EditAssetDetailVM GetById(int id)
        {
            var lstAssetDetails = _context.AssetDetails

                                    .Include(a => a.MasterAsset)
                                .Include(a => a.MasterAsset.brand)
                                .Include(a => a.Building)
                                .Include(a => a.Floor).Include(a => a.Room)
                                .Include(a => a.Hospital)
                                .Include(a => a.Hospital.Governorate)
                                .Include(a => a.Hospital.City)
                                .Include(a => a.Hospital.Organization)
                                .Include(a => a.Hospital.SubOrganization)
                                  .Include(a => a.Supplier)
                                  .Include(a => a.Department)
                               .ToList().Where(a => a.Id == id).ToList();//.FirstOrDefault();

            if (lstAssetDetails.Count > 0)
            {
                var assetDetailObj = lstAssetDetails[0];
                EditAssetDetailVM item = new EditAssetDetailVM();

                item.Id = assetDetailObj.Id;
                item.CreatedBy = assetDetailObj.CreatedBy;
                item.MasterAssetId = assetDetailObj.MasterAssetId;
                item.AssetName = assetDetailObj.MasterAsset.Name;
                item.AssetNameAr = assetDetailObj.MasterAsset.NameAr;
                item.Model = assetDetailObj.MasterAsset.ModelNumber;
                item.Code = assetDetailObj.Code;
                item.PurchaseDate = assetDetailObj.PurchaseDate != null ? assetDetailObj.PurchaseDate.Value.ToShortDateString() : "";
                item.Price = assetDetailObj.Price;
                item.SerialNumber = assetDetailObj.SerialNumber;
                item.Remarks = assetDetailObj.Remarks;
                item.Barcode = assetDetailObj.Barcode;
                item.BarCode = assetDetailObj.Barcode;
                item.InstallationDate = assetDetailObj.InstallationDate != null ? assetDetailObj.InstallationDate.Value.ToShortDateString() : "";
                item.OperationDate = assetDetailObj.OperationDate != null ? assetDetailObj.OperationDate.Value.ToShortDateString() : "";
                item.ReceivingDate = assetDetailObj.ReceivingDate != null ? assetDetailObj.ReceivingDate.Value.ToShortDateString() : "";
                item.PONumber = assetDetailObj.PONumber;
                item.WarrantyExpires = assetDetailObj.WarrantyExpires;

                item.AssetImg = assetDetailObj.MasterAsset.AssetImg;

                item.BuildingId = assetDetailObj.BuildingId;
                if (assetDetailObj.BuildingId != null)
                {
                    item.BuildName = assetDetailObj.Building.Name;
                    item.BuildNameAr = assetDetailObj.Building.NameAr;
                }
                item.RoomId = assetDetailObj.RoomId;
                if (assetDetailObj.RoomId != null)
                {
                    item.RoomName = assetDetailObj.Room.Name;
                    item.RoomNameAr = assetDetailObj.Room.NameAr;
                }
                item.FloorId = assetDetailObj.FloorId;
                if (assetDetailObj.FloorId != null)
                {
                    item.FloorName = assetDetailObj.Floor.Name;
                    item.FloorNameAr = assetDetailObj.Floor.NameAr;
                }



                if (assetDetailObj.MasterAsset.brand != null)
                {

                    item.BrandName = assetDetailObj.MasterAsset.brand.Name;
                    item.BrandNameAr = assetDetailObj.MasterAsset.brand.NameAr;
                }

                item.DepartmentId = assetDetailObj.Department != null ? assetDetailObj.DepartmentId : 0;
                item.DepartmentName = assetDetailObj.Department != null ? assetDetailObj.Department.Name : "";
                item.DepartmentNameAr = assetDetailObj.Department != null ? assetDetailObj.Department.NameAr : "";


                var lstAssetTransactions = _context.AssetStatusTransactions.Include(a => a.AssetStatus).Where(a => a.AssetDetailId == assetDetailObj.Id).ToList().OrderByDescending(a => a.StatusDate).ToList();

                if (lstAssetTransactions.Count > 0)
                {
                    item.AssetStatusId = lstAssetTransactions[0].AssetStatus.Id;
                    item.AssetStatus = lstAssetTransactions[0].AssetStatus.Name;
                    item.AssetStatusAr = lstAssetTransactions[0].AssetStatus.NameAr;
                }

                item.DepartmentId = assetDetailObj.DepartmentId;
                item.SupplierId = assetDetailObj.SupplierId;
                item.HospitalId = assetDetailObj.HospitalId;
                if (assetDetailObj.HospitalId != null)
                {
                    item.HospitalName = assetDetailObj.Hospital.Name;
                    item.HospitalNameAr = assetDetailObj.Hospital.NameAr;
                }
                item.MasterAssetId = assetDetailObj.MasterAssetId;
                item.WarrantyStart = assetDetailObj.WarrantyStart != null ? assetDetailObj.WarrantyStart.Value.ToShortDateString() : "";
                item.WarrantyEnd = assetDetailObj.WarrantyEnd != null ? assetDetailObj.WarrantyEnd.Value.ToShortDateString() : "";
                item.CostCenter = assetDetailObj.CostCenter;
                item.DepreciationRate = assetDetailObj.DepreciationRate;
                item.QrFilePath = assetDetailObj.QrFilePath;

                item.GovernorateId = assetDetailObj.Hospital.GovernorateId;
                item.CityId = assetDetailObj.Hospital.CityId;
                item.OrganizationId = assetDetailObj.Hospital.OrganizationId;
                item.SubOrganizationId = assetDetailObj.Hospital.SubOrganizationId;



                item.SupplierName = assetDetailObj.Supplier != null ? assetDetailObj.Supplier.Name : "";
                item.SupplierNameAr = assetDetailObj.Supplier != null ? assetDetailObj.Supplier.NameAr : "";

                item.BrandId = assetDetailObj.MasterAsset.brand != null ? assetDetailObj.MasterAsset.BrandId : 0;
                item.BrandName = assetDetailObj.MasterAsset.brand != null ? assetDetailObj.MasterAsset.brand.Name : "";
                item.BrandNameAr = assetDetailObj.MasterAsset.brand != null ? assetDetailObj.MasterAsset.brand.NameAr : "";
                //var lstAssetStatus = _context.AssetStatusTransactions.Include(a => a.AssetStatus).Where(a => a.AssetDetailId == id).ToList().OrderByDescending(a => a.StatusDate).ToList();
                //if (lstAssetStatus.Count > 0)
                //{
                //    item.AssetStatus = lstAssetStatus[0].AssetStatus.Name;
                //    item.AssetStatusAr = lstAssetStatus[0].AssetStatus.NameAr;
                //}
                return item;

            }
            return null;
        }
        private static Byte[] BitmapToBytes(Bitmap img, int assetId)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                img.Save(Directory.GetCurrentDirectory() + "/UploadedAttachments/qrFiles/equipment-" + assetId + ".png", System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
        public int Update(EditAssetDetailVM model)
        {
            try
            {

                var assetDetailObj = _context.AssetDetails.Find(model.Id);
                assetDetailObj.Id = model.Id;
                assetDetailObj.Code = model.Code;
                assetDetailObj.PurchaseDate = model.PurchaseDate != null ? DateTime.Parse(model.PurchaseDate) : null;
                assetDetailObj.Price = model.Price;
                assetDetailObj.SerialNumber = model.SerialNumber;
                assetDetailObj.Remarks = model.Remarks;
                assetDetailObj.Barcode = model.Barcode;
                assetDetailObj.CreatedBy = model.CreatedBy;
                assetDetailObj.FixCost = model.FixCost;
                assetDetailObj.InstallationDate = model.InstallationDate != null ? DateTime.Parse(model.InstallationDate) : null;

                var lstAssetMovements = _context.AssetMovements.Where(a => a.AssetDetailId == model.Id).ToList();
                if (lstAssetMovements.Count == 0)
                {
                    if (model.BuildingId != 0 && model.FloorId != 0 && model.RoomId != 0)
                    {
                        AssetMovement movementObj = new AssetMovement();
                        movementObj.RoomId = model.RoomId;
                        movementObj.FloorId = model.FloorId;
                        movementObj.BuildingId = model.BuildingId;
                        movementObj.AssetDetailId = model.Id;
                        movementObj.MovementDate = DateTime.Now;
                        _context.AssetMovements.Add(movementObj);
                        _context.SaveChanges();
                    }
                }

                assetDetailObj.RoomId = model.RoomId;
                assetDetailObj.FloorId = model.FloorId;
                assetDetailObj.BuildingId = model.BuildingId;
                assetDetailObj.ReceivingDate = model.ReceivingDate != null ? DateTime.Parse(model.ReceivingDate) : null;
                assetDetailObj.OperationDate = model.OperationDate != null ? DateTime.Parse(model.OperationDate) : null;
                assetDetailObj.PONumber = model.PONumber;
                assetDetailObj.DepartmentId = model.DepartmentId;
                assetDetailObj.SupplierId = model.SupplierId;
                assetDetailObj.HospitalId = model.HospitalId;
                assetDetailObj.MasterAssetId = model.MasterAssetId;
                assetDetailObj.WarrantyStart = model.WarrantyStart != null ? DateTime.Parse(model.WarrantyStart) : null;
                assetDetailObj.WarrantyEnd = model.WarrantyEnd != null ? DateTime.Parse(model.WarrantyEnd) : null;
                assetDetailObj.WarrantyExpires = model.WarrantyExpires;
                assetDetailObj.DepreciationRate = model.DepreciationRate;
                assetDetailObj.CostCenter = model.CostCenter;

                if (assetDetailObj.QrFilePath == null)
                {
                    string url = "http://http://10.10.0.119/#/AssetDetail/" + model.Id;
                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);

                    QRCode qrCode = new QRCode(qrCodeData);
                    Bitmap qrCodeImage = qrCode.GetGraphic(15);
                    var bitmapFiles = BitmapToBytes(qrCodeImage, model.Id);
                    assetDetailObj.QrFilePath = url;
                }
                else
                {
                    assetDetailObj.QrFilePath = model.QrFilePath;
                }
                _context.Entry(assetDetailObj).State = EntityState.Modified;
                _context.SaveChanges();


                List<int> oldIds = new List<int>();
                List<int> newIds = new List<int>();

                var lstSavedIds = _context.AssetOwners.Where(a => a.AssetDetailId == assetDetailObj.Id).Select(q => q.EmployeeId).ToList();
                foreach (var saved in lstSavedIds)
                {
                    oldIds.Add(int.Parse(saved.ToString()));
                }


                var lstNewIds = model.ListOwners;
                foreach (var news in lstNewIds)
                {
                    newIds.Add(int.Parse(news.ToString()));
                }


                var savedIds = oldIds.Except(newIds);
                foreach (var item in savedIds)
                {
                    var row = _context.AssetOwners.Where(a => a.AssetDetailId == assetDetailObj.Id && a.EmployeeId == item).ToList();
                    if (row.Count > 0)
                    {
                        var ownerObj = row[0];
                        _context.AssetOwners.Remove(ownerObj);
                        _context.SaveChanges();
                    }
                }


                var neewIds = newIds.Except(oldIds);
                foreach (var itm in neewIds)
                {
                    AssetOwner ownerObj = new AssetOwner();
                    ownerObj.AssetDetailId = assetDetailObj.Id;
                    ownerObj.EmployeeId = int.Parse(itm.ToString());
                    ownerObj.HospitalId = int.Parse(model.HospitalId.ToString());
                    _context.AssetOwners.Add(ownerObj);
                    _context.SaveChanges();
                }




                if (model.MasterAssetId > 0 && model.InstallationDate != null)
                {
                    var dates = new List<DateTime>();
                    var masterObj = _context.MasterAssets.Find(model.MasterAssetId);


                    var pmtimeId = masterObj.PMTimeId;
                    if (pmtimeId == 1)
                    {
                        var pmdate = DateTime.Parse(model.InstallationDate).AddYears(1);
                        if (pmdate.DayOfWeek == DayOfWeek.Friday)
                        {
                            pmdate = pmdate.AddDays(-1);
                        }
                        if (pmdate.DayOfWeek == DayOfWeek.Saturday)
                        {
                            pmdate = pmdate.AddDays(1);
                        }

                    }
                    if (pmtimeId == 2)
                    {

                        for (var dt = DateTime.Parse(model.InstallationDate).AddDays(1); dt <= DateTime.Parse(model.InstallationDate).AddYears(1); dt = dt.AddMonths(6))
                        {
                            if (dt.DayOfWeek == DayOfWeek.Friday)
                            {
                                dt = dt.AddDays(-1);
                            }
                            if (dt.DayOfWeek == DayOfWeek.Saturday)
                            {
                                dt = dt.AddDays(1);
                            }

                            dates.Add(dt);
                        }

                    }
                    if (pmtimeId == 3)
                    {
                        for (var dt = DateTime.Parse(model.InstallationDate).AddDays(1); dt <= DateTime.Parse(model.InstallationDate).AddYears(1); dt = dt.AddMonths(3))
                        {
                            if (dt.DayOfWeek == DayOfWeek.Friday)
                            {
                                dt = dt.AddDays(-1);
                            }
                            if (dt.DayOfWeek == DayOfWeek.Saturday)
                            {
                                dt = dt.AddDays(1);
                            }

                            dates.Add(dt);
                        }
                    }

                    if (dates.Count > 0)
                    {
                        //   var lstSavedDates = _context.PMAssetTimes.ToList().Where(a=>a.Id == model.Id)

                        foreach (var item in dates)
                        {
                            PMAssetTime assetTimeObj = new PMAssetTime();
                            assetTimeObj.PMDate = item;
                            assetTimeObj.AssetDetailId = model.Id;
                            assetTimeObj.HospitalId = model.HospitalId;
                            _context.PMAssetTimes.Add(assetTimeObj);
                            _context.SaveChanges();
                        }
                    }

                }


                return assetDetailObj.Id;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return 0;
        }
        public int CreateAssetDetailDocuments(CreateAssetDetailAttachmentVM attachObj)
        {
            AssetDetailAttachment assetAttachmentObj = new AssetDetailAttachment();
            assetAttachmentObj.AssetDetailId = attachObj.AssetDetailId;
            assetAttachmentObj.Title = attachObj.Title;
            assetAttachmentObj.FileName = attachObj.FileName;
            assetAttachmentObj.HospitalId = attachObj.HospitalId;
            _context.AssetDetailAttachments.Add(assetAttachmentObj);
            _context.SaveChanges();
            int Id = assetAttachmentObj.Id;
            return Id;
        }
        public int DeleteAssetDetailAttachment(int id)
        {
            try
            {
                var attachObj = _context.AssetDetailAttachments.Find(id);
                _context.AssetDetailAttachments.Remove(attachObj);
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return 0;
        }
        public IEnumerable<AssetDetailAttachment> GetAttachmentByAssetDetailId(int assetId)
        {
            return _context.AssetDetailAttachments.Where(a => a.AssetDetailId == assetId).ToList();
        }
        public ViewAssetDetailVM ViewAssetDetailByMasterId(int id)
        {
            ViewAssetDetailVM model = new ViewAssetDetailVM();
            var lstHospitalAssets = _context.AssetDetails.Include(a => a.Supplier)
                                                    .Include(a => a.MasterAsset).Include(a => a.Hospital)
                                                    .Include(a => a.Hospital.Governorate)
                                                    .Include(a => a.Hospital.City)
                                                    .Include(a => a.Hospital.Organization).Include(a => a.Hospital.SubOrganization)
                                                    .Include(a => a.MasterAsset.brand)
                                                    .Include(a => a.MasterAsset.Category)
                                                    .Include(a => a.MasterAsset.SubCategory)
                                                    .Include(a => a.MasterAsset.ECRIS)
                                                    .Include(a => a.Department)
                                                    .Include(a => a.Building).Include(a => a.Floor).Include(a => a.Room)
                                                    .Include(a => a.MasterAsset.Origin).ToList().Where(a => a.Id == id).ToList();
            if (lstHospitalAssets.Count > 0)
            {
                var detailObj = lstHospitalAssets[0];
                model.Id = detailObj.Id;
                model.Code = detailObj.Code;
                model.PurchaseDate = detailObj.PurchaseDate != null ? detailObj.PurchaseDate.ToString() : "";
                model.Price = detailObj.Price.ToString();
                model.SerialNumber = detailObj.SerialNumber;
                model.Remarks = detailObj.Remarks;
                model.Barcode = detailObj.Barcode;
                model.InstallationDate = detailObj.InstallationDate != null ? detailObj.InstallationDate.Value.ToShortDateString() : "";
                model.WarrantyExpires = detailObj.WarrantyExpires;
                model.WarrantyStart = detailObj.WarrantyStart != null ? detailObj.WarrantyStart.Value.ToShortDateString() : "";
                model.WarrantyEnd = detailObj.WarrantyEnd != null ? detailObj.WarrantyEnd.Value.ToShortDateString() : "";
                model.ReceivingDate = detailObj.ReceivingDate != null ? detailObj.ReceivingDate.Value.ToShortDateString() : "";
                model.OperationDate = detailObj.OperationDate != null ? detailObj.OperationDate.Value.ToShortDateString() : "";
                model.CostCenter = detailObj.CostCenter;
                model.DepreciationRate = detailObj.DepreciationRate;
                model.PONumber = detailObj.PONumber;
                model.QrFilePath = detailObj.QrFilePath;

                model.MasterAssetId = detailObj.MasterAsset.Id;
                model.AssetName = detailObj.MasterAsset.Name;
                model.AssetNameAr = detailObj.MasterAsset.NameAr;
                model.MasterCode = detailObj.MasterAsset.Code;
                model.VersionNumber = detailObj.MasterAsset.VersionNumber;
                model.ModelNumber = detailObj.MasterAsset.ModelNumber;
                model.ExpectedLifeTime = detailObj.MasterAsset.ExpectedLifeTime != null ? (int)detailObj.MasterAsset.ExpectedLifeTime : 0;
                model.Description = detailObj.MasterAsset.Description;
                model.DescriptionAr = detailObj.MasterAsset.DescriptionAr;
                model.Length = detailObj.MasterAsset.Length.ToString();
                model.Width = detailObj.MasterAsset.Width.ToString();
                model.Weight = detailObj.MasterAsset.Weight.ToString();
                model.Height = detailObj.MasterAsset.Height.ToString();
                model.AssetImg = detailObj.MasterAsset.AssetImg;


                var lstAssetStatus = _context.AssetStatusTransactions.Include(a => a.AssetStatus).Where(a => a.AssetDetailId == detailObj.Id).ToList().OrderByDescending(a => a.StatusDate).ToList();

                if (lstAssetStatus.Count > 0)
                {
                    model.AssetStatus = lstAssetStatus[0].AssetStatus.Name;
                    model.AssetStatusAr = lstAssetStatus[0].AssetStatus.NameAr;
                }
                if (detailObj.BuildingId != null)
                {
                    model.BuildId = detailObj.Building.Id;
                    model.BuildName = detailObj.Building.Name;
                    model.BuildNameAr = detailObj.Building.NameAr;
                }
                if (detailObj.Floor != null)
                {
                    model.FloorId = detailObj.Floor.Id;
                    model.FloorName = detailObj.Floor.Name;
                    model.FloorNameAr = detailObj.Floor.NameAr;
                }
                if (detailObj.Room != null)
                {
                    model.RoomId = detailObj.Room.Id;
                    model.RoomName = detailObj.Room.Name;
                    model.RoomNameAr = detailObj.Room.NameAr;
                }
                if (detailObj.Department != null)
                {
                    model.DepartmentName = detailObj.Department.Name;
                    model.DepartmentNameAr = detailObj.Department.NameAr;
                }
                if (detailObj.MasterAsset.Category != null)
                {
                    model.CategoryName = detailObj.MasterAsset.Category.Name;
                    model.CategoryNameAr = detailObj.MasterAsset.Category.NameAr;
                }
                if (detailObj.Hospital != null)
                {
                    model.HospitalId = detailObj.Hospital.Id;
                    model.HospitalName = detailObj.Hospital.Name;
                    model.HospitalNameAr = detailObj.Hospital.NameAr;
                }
                if (detailObj.Hospital.Governorate != null)
                {
                    model.GovernorateName = detailObj.Hospital.Governorate.Name;
                    model.GovernorateNameAr = detailObj.Hospital.Governorate.NameAr;
                }
                if (detailObj.Hospital.City != null)
                {
                    model.CityName = detailObj.Hospital.City.Name;
                    model.CityNameAr = detailObj.Hospital.City.NameAr;
                }
                if (detailObj.Hospital.Organization != null)
                {
                    model.OrgName = detailObj.Hospital.Organization.Name;
                    model.OrgNameAr = detailObj.Hospital.Organization.NameAr;
                }

                if (detailObj.Hospital.SubOrganization != null)
                {
                    model.SubOrgName = detailObj.Hospital.SubOrganization.Name;
                    model.SubOrgNameAr = detailObj.Hospital.SubOrganization.NameAr;
                }
                if (detailObj.Supplier != null)
                {
                    model.SupplierName = detailObj.Supplier.Name;
                    model.SupplierNameAr = detailObj.Supplier.NameAr;
                }
                if (detailObj.MasterAsset.Category != null)
                {
                    model.CategoryName = detailObj.MasterAsset.Category.Name;
                    model.CategoryNameAr = detailObj.MasterAsset.Category.NameAr;
                }
                if (detailObj.MasterAsset.SubCategory != null)
                {
                    model.SubCategoryName = detailObj.MasterAsset.SubCategory.Name;
                    model.SubCategoryNameAr = detailObj.MasterAsset.SubCategory.NameAr;
                }
                if (detailObj.MasterAsset.Origin != null)
                {
                    model.OriginName = detailObj.MasterAsset.Origin.Name;
                    model.OriginNameAr = detailObj.MasterAsset.Origin.NameAr;
                }
                if (detailObj.MasterAsset.brand != null)
                {
                    model.BrandName = detailObj.MasterAsset.brand.Name;
                    model.BrandNameAr = detailObj.MasterAsset.brand.NameAr;
                }
            }
            return model;
        }
        public IndexAssetDetailVM SearchAssetInHospital(int pagenumber, int pagesize, SearchMasterAssetVM searchObj)
        {
            IndexAssetDetailVM mainClass = new IndexAssetDetailVM();

            List<IndexAssetDetailVM.GetData> list = new List<IndexAssetDetailVM.GetData>();


            Employee empObj = new Employee();
            ApplicationUser userObj = new ApplicationUser();
            ApplicationRole roleObj = new ApplicationRole();
            List<string> userRoleNames = new List<string>();
            var lstUsers = _context.ApplicationUser.Where(a => a.Id == searchObj.UserId).ToList();
            if (lstUsers.Count > 0)
            {
                userObj = lstUsers[0];
            }
            var roles = (from userRole in _context.UserRoles
                         join role in _context.ApplicationRole on userRole.RoleId equals role.Id
                         where userRole.UserId == searchObj.UserId
                         select role);
            foreach (var role in roles)
            {
                userRoleNames.Add(role.Name);
            }


            var lstEmployees = _context.Employees.Where(a => a.Email == userObj.Email).ToList();
            if (lstEmployees.Count > 0)
            {
                empObj = lstEmployees[0];
            }

            if (userRoleNames.Contains("AssetOwner"))
            {
                list = _context.AssetOwners.Include(a => a.AssetDetail)
                                           .Include(a => a.AssetDetail.MasterAsset).Include(a => a.AssetDetail.Hospital).Include(a => a.AssetDetail.Supplier)
                                           .Include(a => a.AssetDetail.MasterAsset.brand).Include(a => a.AssetDetail.Hospital.Governorate).Include(a => a.AssetDetail.Hospital.City)
                                           .Include(a => a.AssetDetail.Hospital.Organization).Include(a => a.AssetDetail.Hospital.SubOrganization).Include(a => a.AssetDetail.Department)
                                           .OrderBy(a => a.AssetDetail.Barcode)
                                             .Select(detail => new IndexAssetDetailVM.GetData
                                             {
                                                 Id = detail.AssetDetail.Id,
                                                 Code = detail.AssetDetail.Code,
                                                 CreatedBy = detail.AssetDetail.CreatedBy,
                                                 BarCode = detail.AssetDetail.Barcode,
                                                 Model = detail.AssetDetail.MasterAsset.ModelNumber,
                                                 Serial = detail.AssetDetail.SerialNumber,
                                                 SerialNumber = detail.AssetDetail.SerialNumber,
                                                 EmployeeId = detail.EmployeeId,
                                                 DepartmentId = detail.AssetDetail.DepartmentId,
                                                 HospitalId = detail.AssetDetail.HospitalId,
                                                 SupplierId = detail.AssetDetail.SupplierId,
                                                 QrFilePath = detail.AssetDetail.QrFilePath,
                                                 BrandName = detail.AssetDetail.MasterAsset.brand.Name,
                                                 BrandNameAr = detail.AssetDetail.MasterAsset.brand.NameAr,
                                                 SupplierName = detail.AssetDetail.Supplier.Name,
                                                 SupplierNameAr = detail.AssetDetail.Supplier.NameAr,
                                                 AssetId = detail.AssetDetail.Id,
                                                 MasterAssetId = detail.AssetDetail.MasterAsset.Id,
                                                 AssetName = detail.AssetDetail.MasterAsset.Name,
                                                 AssetNameAr = detail.AssetDetail.MasterAsset.NameAr,
                                                 BrandId = detail.AssetDetail.MasterAsset.BrandId,
                                                 OriginId = detail.AssetDetail.MasterAsset.OriginId,
                                                 HospitalName = detail.AssetDetail.Hospital.Name,
                                                 HospitalNameAr = detail.AssetDetail.Hospital.NameAr,
                                                 GovernorateId = detail.AssetDetail.Hospital.GovernorateId,
                                                 GovernorateName = detail.AssetDetail.Hospital.Governorate.Name,
                                                 GovernorateNameAr = detail.AssetDetail.Hospital.Governorate.NameAr,
                                                 CityName = detail.AssetDetail.Hospital.City.Name,
                                                 CityNameAr = detail.AssetDetail.Hospital.City.NameAr,
                                                 OrgName = detail.AssetDetail.Hospital.Organization.Name,
                                                 OrgNameAr = detail.AssetDetail.Hospital.Organization.NameAr,
                                                 SubOrgName = detail.AssetDetail.Hospital.SubOrganization.Name,
                                                 SubOrgNameAr = detail.AssetDetail.Hospital.SubOrganization.NameAr,
                                                 CityId = detail.AssetDetail.Hospital.CityId,
                                                 OrganizationId = detail.AssetDetail.Hospital.OrganizationId,
                                                 SubOrganizationId = detail.AssetDetail.Hospital.SubOrganizationId,
                                             }).ToList();
            }
            else
            {
                var assetDetailList = _context.AssetDetails.Include(a => a.MasterAsset).Include(a => a.Hospital).Include(a => a.Supplier)
                                            .Include(a => a.MasterAsset.brand).Include(a => a.Hospital.Governorate).Include(a => a.Hospital.City)
                                            .Include(a => a.Hospital.Organization).Include(a => a.Hospital.SubOrganization).Include(a => a.Department)
                                            .OrderBy(a => a.Barcode).ToList();
                foreach (var detail in assetDetailList)
                {
                    IndexAssetDetailVM.GetData item = new IndexAssetDetailVM.GetData();
                    item.Id = detail.Id;
                    item.Code = detail.Code;
                    item.BarCode = detail.Barcode;
                    item.Barcode = detail.Barcode;
                    item.CreatedBy = detail.CreatedBy;
                    item.Model = detail.MasterAsset.ModelNumber;
                    item.Serial = detail.SerialNumber;
                    item.SerialNumber = detail.SerialNumber;

                    if (_context.AssetOwners.Where(a => a.AssetDetailId == detail.Id).ToList().Count > 0)
                    {
                        var lstAssetEmployees = _context.AssetOwners.Where(a => a.AssetDetailId == detail.Id).ToList();
                        if (lstAssetEmployees.Count > 0)
                        {
                            var employeeObj = lstAssetEmployees[0];
                            item.EmployeeId = employeeObj.Id;
                        }

                    }
                    item.DepartmentId = detail.DepartmentId;
                    item.HospitalId = detail.HospitalId;
                    item.SupplierId = detail.SupplierId;
                    item.QrFilePath = detail.QrFilePath;
                    item.BrandName = detail.MasterAsset.brand.Name;
                    item.BrandNameAr = detail.MasterAsset.brand.NameAr;
                    item.SupplierName = detail.Supplier.Name;
                    item.SupplierNameAr = detail.Supplier.NameAr;
                    item.AssetId = detail.Id;
                    item.MasterAssetId = detail.MasterAsset.Id;
                    item.AssetName = detail.MasterAsset.Name;
                    item.AssetNameAr = detail.MasterAsset.NameAr;
                    item.BrandId = detail.MasterAsset.BrandId;
                    item.OriginId = detail.MasterAsset.OriginId;
                    item.HospitalName = detail.Hospital.Name;
                    item.HospitalNameAr = detail.Hospital.NameAr;
                    item.GovernorateId = detail.Hospital.GovernorateId;
                    item.GovernorateName = detail.Hospital.Governorate.Name;
                    item.GovernorateNameAr = detail.Hospital.Governorate.NameAr;
                    item.CityName = detail.Hospital.City.Name;
                    item.CityNameAr = detail.Hospital.City.NameAr;
                    item.OrgName = detail.Hospital.Organization.Name;
                    item.OrgNameAr = detail.Hospital.Organization.NameAr;
                    item.SubOrgName = detail.Hospital.SubOrganization.Name;
                    item.SubOrgNameAr = detail.Hospital.SubOrganization.NameAr;
                    item.CityId = detail.Hospital.CityId;
                    item.OrganizationId = detail.Hospital.OrganizationId;
                    item.SubOrganizationId = detail.Hospital.SubOrganizationId;
                    list.Add(item);
                }
            }



            if (userRoleNames.Contains("AssetOwner") && userObj.HospitalId > 0)
            {
                list = list.Where(a => a.HospitalId == userObj.HospitalId && a.EmployeeId == empObj.Id).ToList();
            }
            if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
            {
                list = list.ToList();
            }
            if (userObj.GovernorateId > 0 && userObj.CityId == 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
            {
                list = list.Where(a => a.GovernorateId == userObj.GovernorateId).ToList();
            }
            if (userObj.GovernorateId > 0 && userObj.CityId > 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
            {
                list = list.Where(a => a.GovernorateId == userObj.GovernorateId && a.CityId == userObj.CityId).ToList();
            }
            if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId > 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
            {
                list = list.Where(a => a.OrganizationId == userObj.OrganizationId).ToList();
            }
            if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId > 0 && userObj.SubOrganizationId > 0 && userObj.HospitalId == 0)
            {
                list = list.Where(a => a.OrganizationId == userObj.OrganizationId && a.SubOrganizationId == userObj.SubOrganizationId).ToList();
            }
            if (!userRoleNames.Contains("AssetOwner") && userObj.HospitalId > 0)
            {
                list = list.Where(a => a.HospitalId == userObj.HospitalId).ToList();
            }

            if (userObj.HospitalId == 0 && searchObj.HospitalId > 0)
            {
                list = list.Where(a => a.HospitalId == searchObj.HospitalId).ToList();
            }
            if (searchObj.GovernorateId != 0)
            {
                list = list.Where(a => a.GovernorateId == searchObj.GovernorateId).ToList();
            }
            else
                list = list.ToList();
            if (searchObj.CityId != 0)
            {
                list = list.Where(a => a.CityId == searchObj.CityId).ToList();
            }
            else
                list = list.ToList();
            if (searchObj.OrganizationId != 0)
            {
                list = list.Where(a => a.OrganizationId == searchObj.OrganizationId).ToList();
            }
            else
                list = list.ToList();
            if (searchObj.SubOrganizationId != 0)
            {
                list = list.Where(a => a.SubOrganizationId == searchObj.SubOrganizationId).ToList();
            }
            else
                list = list.ToList();
            if (searchObj.SupplierId != 0)
            {
                list = list.Where(a => a.SupplierId == searchObj.SupplierId).ToList();
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
            if (searchObj.HospitalId != 0)
            {
                list = list.Where(a => a.HospitalId == searchObj.HospitalId).ToList();
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


            if (searchObj.Serial != "")
            {
                list = list.Where(b => b.SerialNumber == searchObj.Serial).ToList();
            }
            else
                list = list.ToList();

            if (searchObj.BarCode != "")
            {
                list = list.Where(b => b.BarCode == searchObj.BarCode).ToList();

            }
            else
                list = list.ToList();
            if (searchObj.Code != "")
            {
                list = list.Where(b => b.Code.Contains(searchObj.Code)).ToList();
            }
            else
                list = list.ToList();
            if (searchObj.Model != "")
            {
                list = list.Where(b => b.Model == searchObj.Model).ToList();
            }
            else
                list = list.ToList();

            //mainClass.Results = list;


            var requestsPerPage = list.Skip((pagenumber - 1) * pagesize).Take(pagesize).ToList();
            mainClass.Results = requestsPerPage;
            mainClass.Count = list.Count();
            return mainClass;
            //  return list;
        }
        public IEnumerable<IndexAssetDetailVM.GetData> SearchAssetInHospitalByHospitalId(SearchMasterAssetVM searchObj)
        {
            List<IndexAssetDetailVM.GetData> lstData = new List<IndexAssetDetailVM.GetData>();

            var list = _context.AssetDetails.Include(a => a.MasterAsset).Include(a => a.Hospital)
                //  .Where(a => a.HospitalId == searchObj.HospitalId && a.Hospital.Id == searchObj.HospitalId)
                .Select(item => new IndexAssetDetailVM.GetData
                {
                    Id = item.Id,
                    Code = item.Code,
                    SerialNumber = item.SerialNumber,
                    CreatedBy = item.CreatedBy,
                    HospitalId = item.HospitalId,
                    SupplierId = item.SupplierId,
                    MasterAssetId = item.MasterAsset.Id,
                    AssetName = item.MasterAsset.Name,
                    OriginId = item.MasterAsset.OriginId,
                    BrandId = item.MasterAsset.BrandId,

                    HospitalName = item.Hospital.Name,
                    HospitalNameAr = item.Hospital.NameAr,
                    GovernorateId = item.Hospital.GovernorateId,
                    CityId = item.Hospital.CityId,
                    OrganizationId = item.Hospital.OrganizationId,
                    SubOrganizationId = item.Hospital.SubOrganizationId,
                    Serial = item.SerialNumber,

                    //UserId = userObj.Id,
                    Price = item.Price,
                    BarCode = item.Barcode,
                    MasterImg = item.MasterAsset.AssetImg,
                    BrandName = item.MasterAsset.brand.Name,
                    BrandNameAr = item.MasterAsset.brand.NameAr,
                    Model = item.MasterAsset.ModelNumber,
                    PurchaseDate = item.PurchaseDate,
                    AssetNameAr = item.MasterAsset.NameAr,
                    GovernorateName = item.Hospital.Governorate.Name,
                    GovernorateNameAr = item.Hospital.Governorate.NameAr,
                    CityName = item.Hospital.City.Name,
                    CityNameAr = item.Hospital.City.NameAr,
                    OrgName = item.Hospital.Organization.Name,
                    OrgNameAr = item.Hospital.Organization.NameAr,
                    SubOrgName = item.Hospital.SubOrganization.Name,
                    SubOrgNameAr = item.Hospital.SubOrganization.NameAr,
                    SupplierName = item.Supplier.Name,
                    SupplierNameAr = item.Supplier.NameAr,
                    QrFilePath = item.QrFilePath

                }).ToList();

            if (searchObj.SupplierId != 0)
            {
                list = list.Where(a => a.SupplierId == searchObj.SupplierId).ToList();
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


            if (searchObj.AssetName != "")
            {
                list = list.Where(b => b.AssetName.Contains(searchObj.AssetName)).ToList();
            }

            if (searchObj.DepartmentId != 0)
            {
                list = list.Where(a => a.DepartmentId == searchObj.DepartmentId).ToList();
            }
            else
                list = list.ToList();



            if (searchObj.Serial != "")
            {
                list = list.Where(b => b.SerialNumber.Contains(searchObj.Serial)).ToList();
            }
            if (searchObj.Code != "")
            {
                list = list.Where(b => b.Code.Contains(searchObj.Code)).ToList();
            }
            if (searchObj.BarCode != "")
            {
                list = list.Where(b => b.BarCode.Contains(searchObj.BarCode)).ToList();
            }
            if (searchObj.HospitalId > 0)
            {
                list = list.Where(b => b.HospitalId == searchObj.HospitalId).ToList();
            }
            else
            {
                list = list.ToList();
            }

            return list;
        }
        public IEnumerable<IndexPMAssetTaskScheduleVM.GetData> GetAllPMAssetTaskSchedules(int? hospitalId)
        {
            List<IndexPMAssetTaskScheduleVM.GetData> list = new List<IndexPMAssetTaskScheduleVM.GetData>();


            var lstSchedule = (from detail in _context.AssetDetails
                               join tsktime in _context.PMAssetTimes on detail.Id equals tsktime.AssetDetailId
                               join host in _context.Hospitals on detail.HospitalId equals host.Id
                               join schdl in _context.PMAssetTaskSchedules on tsktime.Id equals schdl.PMAssetTimeId

                               //    _context.PMAssetTimes.Include(a=>a.AssetDetail).Include(a=>a.AssetDetail.Hospital).Include(a=>a.)

                               // where tsktime.PMDate.Value.Month == DateTime.Today.Date.Month
                               where tsktime.PMDate.Value.Year == DateTime.Today.Date.Year
                               select new
                               {
                                   Id = schdl.Id,
                                   TimeId = tsktime.Id,
                                   MasterAssetId = detail.MasterAssetId,
                                   AssetDetailId = detail.Id,
                                   HospitalId = host.Id,
                                   Serial = detail.SerialNumber,
                                   StartDate = tsktime.PMDate,
                                   EndDate = tsktime.PMDate,
                                   start = tsktime.PMDate.Value.ToString(),
                                   end = tsktime.PMDate.Value.ToString(),
                                   allDay = true
                               }).ToList()
                               .GroupBy(a => new
                               {
                                   assetId = a.AssetDetailId,
                                   Day = a.StartDate.Value.Day,
                                   Month = a.StartDate.Value.Month,
                                   Year = a.StartDate.Value.Year
                               });



            if (hospitalId == 0)
            {

                lstSchedule = lstSchedule.ToList();
            }

            else
            {
                lstSchedule = lstSchedule.Where(a => a.FirstOrDefault().HospitalId == hospitalId).ToList();
            }

            foreach (var items in lstSchedule)
            {
                string month = "";
                string day = "";
                string endmonth = "";
                string endday = "";


                if (items.FirstOrDefault().StartDate.Value.Month < 10)
                    month = items.FirstOrDefault().StartDate.Value.Month.ToString().PadLeft(2, '0');
                else
                    month = items.FirstOrDefault().StartDate.Value.Month.ToString();

                if (items.FirstOrDefault().EndDate.Value.Month < 10)
                    endmonth = items.FirstOrDefault().EndDate.Value.Month.ToString().PadLeft(2, '0');
                else
                    endmonth = items.FirstOrDefault().EndDate.Value.Month.ToString();

                if (items.FirstOrDefault().StartDate.Value.Day < 10)
                    day = items.FirstOrDefault().StartDate.Value.Day.ToString().PadLeft(2, '0');
                else
                    day = items.FirstOrDefault().StartDate.Value.Day.ToString();

                if (items.FirstOrDefault().EndDate.Value.Day < 10)
                    endday = (items.FirstOrDefault().EndDate.Value.Day).ToString().PadLeft(2, '0');
                else
                    endday = items.FirstOrDefault().EndDate.Value.Day.ToString();




                IndexPMAssetTaskScheduleVM.GetData getDataObj = new IndexPMAssetTaskScheduleVM.GetData();
                var AssetName = _context.MasterAssets.Where(a => a.Id == items.FirstOrDefault().MasterAssetId).ToList().FirstOrDefault().Name;
                var AssetNameAr = _context.MasterAssets.Where(a => a.Id == items.FirstOrDefault().MasterAssetId).ToList().FirstOrDefault().NameAr;
                var color = _context.MasterAssets.Where(a => a.Id == items.FirstOrDefault().MasterAssetId).ToList().FirstOrDefault().PMBGColor;
                var textColor = _context.MasterAssets.Where(a => a.Id == items.FirstOrDefault().MasterAssetId).ToList().FirstOrDefault().PMColor;
                var Serial = items.FirstOrDefault().Serial;
                getDataObj.start = items.FirstOrDefault().StartDate.Value.Year + "-" + month + "-" + day;
                getDataObj.end = items.FirstOrDefault().EndDate.Value.Year + "-" + endmonth + "-" + endday;
                getDataObj.title = AssetName + " - " + Serial;
                getDataObj.titleAr = AssetNameAr + "  -  " + Serial;




                getDataObj.Id = items.FirstOrDefault().Id;
                getDataObj.color = color;
                getDataObj.textColor = textColor;
                // getDataObj.Serial = Serial;
                getDataObj.start = items.FirstOrDefault().StartDate.Value.Year + "-" + month + "-" + day;
                getDataObj.end = items.FirstOrDefault().EndDate.Value.Year + "-" + endmonth + "-" + endday;
                getDataObj.allDay = true;
                getDataObj.HospitalId = (int)hospitalId;
                getDataObj.ListTasks = _context.PMAssetTasks.Where(a => a.MasterAssetId == items.FirstOrDefault().MasterAssetId).ToList();

                list.Add(getDataObj);

            }
            return list;
        }
        public IEnumerable<AssetDetail> ViewAllAssetDetailByMasterId(int MasterAssetId)
        {
            return _context.AssetDetails.Where(a => a.MasterAssetId == MasterAssetId).ToList();
        }
        public IEnumerable<AssetDetail> GetAllSerialsByMasterAssetIdAndHospitalId(int masterAssetId, int hospitalId)
        {

            var lstAssetDetails = _context.AssetDetails.Include(a => a.MasterAsset).Where(a => a.HospitalId == hospitalId && a.MasterAssetId == masterAssetId).ToList();
            return lstAssetDetails;
        }
        public IEnumerable<AssetDetail> GetAllAssetDetailsByHospitalId(int hospitalId)
        {
            var lstAssetDetails = _context.AssetDetails.Where(a => a.HospitalId == hospitalId).ToList();
            return lstAssetDetails;
        }
        public IEnumerable<ViewAssetDetailVM> GetListOfAssetDetailsByHospitalId(int hospitalId)
        {

            List<ViewAssetDetailVM> lstAssetDetails = new List<ViewAssetDetailVM>();

            if (hospitalId > 0)
            {
                lstAssetDetails = _context.AssetDetails.Include(a => a.MasterAsset).Include(a => a.Supplier)
                                         .Include(a => a.MasterAsset.brand)
                                        .Where(a => a.HospitalId == hospitalId)
                                        .Select(item => new ViewAssetDetailVM
                                        {
                                            Id = item.Id,
                                            //CreatedBy = item.CreatedBy,
                                            HospitalId = int.Parse(item.HospitalId.ToString()),
                                            MasterAssetId = item.MasterAsset.Id,
                                            AssetName = item.MasterAsset.Name,
                                            AssetNameAr = item.MasterAsset.NameAr,
                                            ModelNumber = item.MasterAsset.ModelNumber,
                                            SerialNumber = item.SerialNumber,
                                            SupplierName = item.Supplier.Name,
                                            SupplierNameAr = item.Supplier.NameAr,
                                            BrandName = item.MasterAsset.brand.Name,
                                            BrandNameAr = item.MasterAsset.brand.NameAr,
                                            HospitalName = item.Hospital.Name,
                                            HospitalNameAr = item.Hospital.NameAr,
                                            Barcode = item.Barcode,
                                        }).ToList();
            }
            else
            {
                lstAssetDetails = _context.AssetDetails.Include(a => a.MasterAsset).Include(a => a.Supplier)
                                        .Include(a => a.MasterAsset.brand)
                                       .Select(item => new ViewAssetDetailVM
                                       {
                                           Id = item.Id,
                                           //CreatedBy = item.CreatedBy,
                                           HospitalId = int.Parse(item.HospitalId.ToString()),
                                           MasterAssetId = item.MasterAsset.Id,
                                           AssetName = item.MasterAsset.Name,
                                           AssetNameAr = item.MasterAsset.NameAr,
                                           ModelNumber = item.MasterAsset.ModelNumber,
                                           SerialNumber = item.SerialNumber,
                                           SupplierName = item.Supplier.Name,
                                           SupplierNameAr = item.Supplier.NameAr,
                                           BrandName = item.MasterAsset.brand.Name,
                                           BrandNameAr = item.MasterAsset.brand.NameAr,
                                           HospitalName = item.Hospital.Name,
                                           HospitalNameAr = item.Hospital.NameAr,
                                           Barcode = item.Barcode,
                                       }).ToList();
            }
            return lstAssetDetails;
        }
        public List<CountAssetVM> CountAssetsByHospital()
        {
            List<CountAssetVM> list = new List<CountAssetVM>();
            var lstAssetDetails = _context.AssetDetails.Include(a => a.MasterAsset).Include(a => a.Hospital).Take(5).ToList().GroupBy(a => a.MasterAssetId);
            foreach (var asset in lstAssetDetails)
            {

                CountAssetVM countAssetObj = new CountAssetVM();
                countAssetObj.AssetName = asset.FirstOrDefault().MasterAsset.Name;
                countAssetObj.AssetNameAr = asset.FirstOrDefault().MasterAsset.NameAr;
                countAssetObj.AssetPrice = _context.AssetDetails.Where(a => a.HospitalId == asset.FirstOrDefault().HospitalId).Sum(a => Convert.ToDecimal(asset.FirstOrDefault().Price.ToString()));
                countAssetObj.CountAssetsByHospital = _context.AssetDetails.Where(a => a.HospitalId == asset.FirstOrDefault().HospitalId && a.MasterAssetId == asset.FirstOrDefault().MasterAssetId).Count();
                list.Add(countAssetObj);

            }
            return list;
        }
        public List<PmDateGroupVM> GetAllwithgrouping(int assetId)
        {

            List<PmDateGroupVM> snanns = new List<PmDateGroupVM>();

            var assetObj = _context.AssetDetails.Find(assetId);
            var lstTasks = _context.PMAssetTasks.Where(a => a.MasterAssetId == assetObj.MasterAssetId).ToList();

            var assetTasks = _context.PMAssetTimes.Where(a => a.AssetDetailId == assetId).ToList().GroupBy(
                          e => new
                          {
                              day = e.PMDate.Value.Day,
                              month = e.PMDate.Value.Month,
                              year = e.PMDate.Value.Year
                          }
                      ).ToList();


            if (assetTasks.Count > 0)
            {
                foreach (var item in assetTasks)
                {
                    PmDateGroupVM dates = new PmDateGroupVM();
                    dates.PMDate = item.FirstOrDefault().PMDate;
                    dates.PMAssetTimeId = item.FirstOrDefault().Id;
                    dates.Id = item.FirstOrDefault().Id;
                    dates.AssetDetailId = (int)item.FirstOrDefault().AssetDetailId;

                    List<ListPMAssetTaskScheduleVM.GetData> list = new List<ListPMAssetTaskScheduleVM.GetData>();
                    if (lstTasks.Count > 0)
                    {
                        foreach (var tsk in lstTasks)
                        {
                            ListPMAssetTaskScheduleVM.GetData getDataObj = new ListPMAssetTaskScheduleVM.GetData();
                            getDataObj.MasterAssetId = int.Parse(tsk.MasterAssetId.ToString());
                            getDataObj.TaskNameAr = tsk.NameAr;
                            getDataObj.TaskName = tsk.Name;

                            var lstSchedules = _context.PMAssetTaskSchedules.Where(a => a.PMAssetTaskId == tsk.Id).ToList();
                            foreach (var schedule in lstSchedules)
                            {
                                getDataObj.Checked = schedule.PMAssetTaskId == tsk.Id ? true : false;

                            }
                            list.Add(getDataObj);
                        }
                    }

                    dates.AssetSchduleList = list;
                    snanns.Add(dates);

                    //if (dates.AssetSchduleList.Count > 0)
                    //{
                    //    snanns.Add(dates);
                    //}
                    //else
                    //{
                    //    continue;
                    //}
                }
            }

            return snanns;
        }
        public List<IndexAssetDetailVM.GetData> FilterAsset(filterDto data)
        {
            List<IndexAssetDetailVM.GetData> lstAssetDetails = new List<IndexAssetDetailVM.GetData>();
            var Asset = new List<IndexAssetDetailVM.GetData>();
            lstAssetDetails = _context.AssetDetails.Include(a => a.MasterAsset)
                        .Include(a => a.Hospital).ThenInclude(h => h.Organization)
                        .Include(a => a.Hospital).ThenInclude(h => h.Governorate)
                        .Include(a => a.Hospital).ThenInclude(h => h.City)
                        .Include(a => a.Hospital).ThenInclude(h => h.SubOrganization)
                        .OrderBy(a => a.Barcode)
                                             .Select(a => new IndexAssetDetailVM.GetData
                                             {
                                                 Id = a.Id,
                                                 Code = a.Code,
                                                 Price = a.Price,
                                                 CreatedBy = a.CreatedBy,
                                                 Serial = a.SerialNumber,
                                                 SerialNumber = a.SerialNumber,
                                                 MasterAssetId = a.MasterAssetId,
                                                 PurchaseDate = a.PurchaseDate,
                                                 HospitalId = a.HospitalId,
                                                 HospitalName = a.Hospital.Name,
                                                 HospitalNameAr = a.Hospital.NameAr,
                                                 AssetName = a.MasterAsset.Name,
                                                 AssetNameAr = a.MasterAsset.NameAr,
                                                 GovernorateId = a.Hospital.Governorate.Id,
                                                 GovernorateName = a.Hospital.Governorate.Name,
                                                 GovernorateNameAr = a.Hospital.Governorate.NameAr,
                                                 CityId = a.Hospital.City.Id,
                                                 CityName = a.Hospital.City.Name,
                                                 CityNameAr = a.Hospital.City.NameAr,
                                                 OrganizationId = a.Hospital.Organization.Id,
                                                 OrgName = a.Hospital.Organization.Name,
                                                 OrgNameAr = a.Hospital.Organization.NameAr,
                                                 SubOrgName = a.Hospital.SubOrganization.Name,
                                                 SubOrgNameAr = a.Hospital.SubOrganization.NameAr,
                                                 BrandId = a.MasterAsset.brand.Id,
                                                 BrandName = a.MasterAsset.brand.Name,
                                                 BrandNameAr = a.MasterAsset.brand.NameAr,
                                                 SupplierId = a.Supplier.Id,
                                                 SupplierName = a.Supplier.Name,
                                                 SupplierNameAr = a.Supplier.NameAr
                                             }).ToList();

            if (data.name != null && data.name != "")
            {
                lstAssetDetails = lstAssetDetails.Where(e => e.AssetName == data.name || e.AssetNameAr == data.name).ToList();
            }
            else
            {
                lstAssetDetails = lstAssetDetails.ToList();
            }
            if (data.brandName != null && data.brandName != "")
            {
                lstAssetDetails = lstAssetDetails.Where(e => e.BrandName == data.brandName || e.BrandNameAr == data.brandName).ToList();
            }
            else
            {
                lstAssetDetails = lstAssetDetails.ToList();
            }
            if (data.govName != null && data.govName != "")
            {
                lstAssetDetails = lstAssetDetails.Where(e => e.GovernorateName == data.govName || e.GovernorateNameAr == data.govName).ToList();
            }
            else
            {
                lstAssetDetails = lstAssetDetails.ToList();
            }
            if (data.cityName != null && data.cityName != "")
            {
                lstAssetDetails = lstAssetDetails.Where(e => e.CityName == data.cityName || e.CityNameAr == data.cityName).ToList();
            }
            if (data.hosName != null && data.hosName != "")
            {
                lstAssetDetails = lstAssetDetails.Where(e => e.HospitalName == data.hosName || e.HospitalNameAr == data.hosName).ToList();
            }
            else
            {
                lstAssetDetails = lstAssetDetails.ToList();
            }
            if (data.SupplierName != null && data.SupplierName != "")
            {
                lstAssetDetails = lstAssetDetails.Where(e => e.SupplierName == data.SupplierName || e.SupplierNameAr == data.SupplierName).ToList();
            }
            else
            {
                lstAssetDetails = lstAssetDetails.ToList();
            }
            if (data.purchaseDate != null)
            {
                lstAssetDetails = lstAssetDetails.Where(e => e.PurchaseDate == data.purchaseDate || e.PurchaseDate == data.purchaseDate).ToList();
            }
            else
            {
                lstAssetDetails = lstAssetDetails.ToList();
            }

            if (lstAssetDetails.Count > 0)
            {
                foreach (var item in lstAssetDetails)
                {
                    var As = new IndexAssetDetailVM.GetData
                    {
                        Id = item.Id,
                        Code = item.Code,
                        Price = item.Price,
                        Serial = item.SerialNumber,
                        SerialNumber = item.SerialNumber,
                        MasterAssetId = item.MasterAssetId,
                        PurchaseDate = item.PurchaseDate,
                        HospitalId = item.HospitalId,
                        HospitalName = item.HospitalName,
                        HospitalNameAr = item.HospitalNameAr,
                        AssetName = item.AssetName,
                        AssetNameAr = item.AssetNameAr,
                        GovernorateId = item.GovernorateId,
                        GovernorateName = item.GovernorateName,
                        GovernorateNameAr = item.GovernorateNameAr,
                        CityId = item.CityId,
                        CityName = item.CityName,
                        CityNameAr = item.CityNameAr,
                        OrganizationId = item.OrganizationId,
                        OrgName = item.OrgName,
                        OrgNameAr = item.OrgNameAr,
                        SubOrgName = item.SubOrgName,
                        SubOrgNameAr = item.SubOrgNameAr,
                        BrandId = item.BrandId,
                        BrandName = item.BrandName,
                        BrandNameAr = item.BrandNameAr,
                        SupplierId = item.SupplierId,
                        SupplierName = item.SupplierName,
                        SupplierNameAr = item.SupplierNameAr
                    };
                    Asset.Add(As);
                }
                return Asset;
            }
            return null;
        }
        public List<IndexAssetDetailVM.GetData> FilterDataByDepartmentBrandSupplierId(FilterHospitalAsset data)
        {
            //List<IndexAssetDetailVM.GetData> lstAssetDetails = new List<IndexAssetDetailVM.GetData>();
            List<IndexAssetDetailVM.GetData> list = new List<IndexAssetDetailVM.GetData>();
            // var Asset = new List<IndexAssetDetailVM.GetData>();
            var lstAssetDetails = _context.AssetDetails.Include(a => a.MasterAsset).Include(a => a.MasterAsset.brand)
                         .Include(a => a.Supplier).Include(a => a.Department)
                         .Include(a => a.Hospital).ThenInclude(h => h.Organization)
                         .Include(a => a.Hospital).ThenInclude(h => h.Governorate)
                         .Include(a => a.Hospital).ThenInclude(h => h.City)
                         .Include(a => a.Hospital).ThenInclude(h => h.SubOrganization)
                         .OrderBy(a => a.Barcode).ToList();

            foreach (var item in lstAssetDetails)
            {
                IndexAssetDetailVM.GetData getDataobj = new IndexAssetDetailVM.GetData();
                getDataobj.Id = item.Id;
                getDataobj.CreatedBy = item.CreatedBy;
                getDataobj.Code = item.Code;
                getDataobj.Price = item.Price;
                getDataobj.Barcode = item.Barcode;
                getDataobj.Serial = item.SerialNumber;
                getDataobj.SerialNumber = item.SerialNumber;
                getDataobj.BarCode = item.Barcode;
                getDataobj.Model = item.MasterAsset.ModelNumber;
                getDataobj.MasterAssetId = item.MasterAssetId;
                getDataobj.PurchaseDate = item.PurchaseDate;
                getDataobj.HospitalId = item.HospitalId;
                getDataobj.DepartmentId = item.DepartmentId;
                getDataobj.HospitalName = item.Hospital.Name;
                getDataobj.HospitalNameAr = item.Hospital.NameAr;
                getDataobj.AssetName = item.MasterAsset.Name;
                getDataobj.AssetNameAr = item.MasterAsset.NameAr;
                getDataobj.GovernorateId = item.Hospital.Governorate.Id;
                getDataobj.GovernorateName = item.Hospital.Governorate.Name;
                getDataobj.GovernorateNameAr = item.Hospital.Governorate.NameAr;
                getDataobj.CityId = item.Hospital.City.Id;
                getDataobj.CityName = item.Hospital.City.Name;
                getDataobj.CityNameAr = item.Hospital.City.NameAr;
                getDataobj.OrganizationId = item.Hospital.Organization.Id;
                getDataobj.OrgName = item.Hospital.Organization.Name;
                getDataobj.OrgNameAr = item.Hospital.Organization.NameAr;
                getDataobj.SubOrgName = item.Hospital.SubOrganization.Name;
                getDataobj.SubOrgNameAr = item.Hospital.SubOrganization.NameAr;
                getDataobj.SubOrganizationId = item.Hospital.SubOrganization.Id;
                getDataobj.BrandId = item.MasterAsset.brand != null ? item.MasterAsset.brand.Id : 0;
                getDataobj.BrandName = item.MasterAsset.brand != null ? item.MasterAsset.brand.Name : "";
                getDataobj.BrandNameAr = item.MasterAsset.brand != null ? item.MasterAsset.brand.NameAr : "";
                getDataobj.SupplierId = item.Supplier != null ? item.Supplier.Id : 0;
                getDataobj.SupplierName = item.Supplier != null ? item.Supplier.Name : "";
                getDataobj.SupplierNameAr = item.Supplier != null ? item.Supplier.NameAr : "";
                getDataobj.DepartmentName = item.Department != null ? item.Department.Name : "";
                getDataobj.DepartmentNameAr = item.Department != null ? item.Department.NameAr : "";

                var lstTransactions = _context.AssetStatusTransactions.Where(a => a.AssetDetailId == item.Id).OrderByDescending(a => a.StatusDate.Value.Date).ToList();
                if (lstTransactions.Count > 0)
                {
                    getDataobj.AssetStatusId = lstTransactions[0].AssetStatusId;
                }

                list.Add(getDataobj);
            }


            if (data.DepartmentId != 0)
            {
                list = list.Where(e => e.DepartmentId == data.DepartmentId).ToList();
            }
            else
            {
                list = list.ToList();
            }
            if (data.BrandId != 0)
            {
                list = list.Where(e => e.BrandId == data.BrandId).ToList();
            }
            else
            {
                list = list.ToList();
            }


            if (data.SupplierId != 0)
            {
                list = list.Where(e => e.SupplierId == data.SupplierId).ToList();
            }
            else
            {
                list = list.ToList();
            }
            if (data.MasterAssetId != 0)
            {
                list = list.Where(e => e.MasterAssetId == data.MasterAssetId).ToList();
            }
            else
            {
                list = list.ToList();
            }


            if (data.StatusId != 0)
            {
                list = list.Where(e => e.AssetStatusId == data.StatusId).ToList();
            }
            else
            {
                list = list.ToList();
            }
            string setstartday, setstartmonth, setendday, setendmonth = "";
            DateTime? startingFrom = new DateTime();
            DateTime? endingTo = new DateTime();
            if (data.Start == "")
            {
                data.purchaseDateFrom = DateTime.Parse("01/01/1900");
                startingFrom = DateTime.Parse("01/01/1900");
            }
            else
            {
                data.purchaseDateFrom = DateTime.Parse(data.Start.ToString());
                var startyear = data.purchaseDateFrom.Value.Year;
                var startmonth = data.purchaseDateFrom.Value.Month;
                var startday = data.purchaseDateFrom.Value.Day;
                if (startday < 10)
                    setstartday = data.purchaseDateFrom.Value.Day.ToString().PadLeft(2, '0');
                else
                    setstartday = data.purchaseDateFrom.Value.Day.ToString();

                if (startmonth < 10)
                    setstartmonth = data.purchaseDateFrom.Value.Month.ToString().PadLeft(2, '0');
                else
                    setstartmonth = data.purchaseDateFrom.Value.Month.ToString();

                var sDate = startyear + "-" + setstartmonth + "-" + setstartday;
                startingFrom = DateTime.Parse(sDate);//.AddDays(1);
            }

            if (data.End == "")
            {
                data.purchaseDateTo = DateTime.Today.Date;
                endingTo = DateTime.Today.Date;
            }
            else
            {
                data.purchaseDateTo = DateTime.Parse(data.End.ToString());
                var endyear = data.purchaseDateTo.Value.Year;
                var endmonth = data.purchaseDateTo.Value.Month;
                var endday = data.purchaseDateTo.Value.Day;
                if (endday < 10)
                    setendday = data.purchaseDateTo.Value.Day.ToString().PadLeft(2, '0');
                else
                    setendday = data.purchaseDateTo.Value.Day.ToString();
                if (endmonth < 10)
                    setendmonth = data.purchaseDateTo.Value.Month.ToString().PadLeft(2, '0');
                else
                    setendmonth = data.purchaseDateTo.Value.Month.ToString();
                var eDate = endyear + "-" + setendmonth + "-" + setendday;
                endingTo = DateTime.Parse(eDate);
            }
            if (startingFrom != null || endingTo != null)
            {
                list = list.Where(a => a.PurchaseDate.Value.Date >= startingFrom.Value.Date && a.PurchaseDate.Value.Date <= endingTo.Value.Date).ToList();
            }
            else
            {
                list = list.ToList();
            }


            //if (lstAssetDetails.Count > 0)
            //{
            //    foreach (var item in lstAssetDetails)
            //    {
            //        var As = new IndexAssetDetailVM.GetData
            //        {
            //            Id = item.Id,
            //            Code = item.Code,
            //            Barcode = item.Barcode,
            //            Price = item.Price,
            //            Serial = item.SerialNumber,
            //            SerialNumber = item.SerialNumber,
            //            MasterAssetId = item.MasterAssetId,
            //            PurchaseDate = item.PurchaseDate,
            //            HospitalId = item.HospitalId,
            //            AssetName = item.AssetName,
            //            AssetNameAr = item.AssetNameAr,
            //            GovernorateId = item.GovernorateId,
            //            CityId = item.CityId,
            //            OrganizationId = item.OrganizationId,
            //            BrandId = item.BrandId,
            //            BrandName = item.BrandName,
            //            BrandNameAr = item.BrandNameAr,
            //            SupplierId = item.SupplierId,
            //            SupplierName = item.SupplierName,
            //            SupplierNameAr = item.SupplierNameAr,
            //            DepartmentName = item.DepartmentName,
            //            DepartmentNameAr = item.DepartmentNameAr
            //        };
            //        Asset.Add(As);
            //    }
            return list;
            //}



        }
        public List<DepartmentGroupVM> GetAssetByDepartment(List<IndexAssetDetailVM.GetData> AssetModel)
        {
            List<DepartmentGroupVM> lstAssetDepartments = new List<DepartmentGroupVM>();
            var lsDepartments = (from depart in _context.Departments
                                 select depart).ToList()
                            .GroupBy(a => a.Id).ToList();

            if (lsDepartments.Count > 0)
            {
                foreach (var item in lsDepartments)
                {
                    DepartmentGroupVM departmentGroupObj = new DepartmentGroupVM();
                    departmentGroupObj.Id = item.FirstOrDefault().Id;
                    departmentGroupObj.Name = item.FirstOrDefault().Name;
                    departmentGroupObj.NameAr = item.FirstOrDefault().NameAr;

                    var x = AssetModel.ToList().Where(e => e.DepartmentId == departmentGroupObj.Id);

                    departmentGroupObj.AssetList = AssetModel.ToList().Where(e => e.DepartmentId == departmentGroupObj.Id)
                    .Select(Ass => new IndexAssetDetailVM.GetData
                    {
                        Id = Ass.Id,
                        AssetName = Ass.AssetName,
                        Barcode = Ass.Barcode,
                        SerialNumber = Ass.SerialNumber,
                        Model = Ass.Model,
                        DepartmentName = Ass.DepartmentName,
                        DepartmentNameAr = Ass.DepartmentNameAr,
                        //  CreatedBy = item.CreatedBy,
                        AssetNameAr = Ass.AssetNameAr,
                        BrandName = Ass.BrandName,
                        BrandNameAr = Ass.BrandNameAr,
                        GovernorateName = Ass.GovernorateName,
                        GovernorateNameAr = Ass.GovernorateNameAr,
                        CityName = Ass.CityName,
                        CityNameAr = Ass.CityNameAr,
                        HospitalId = Ass.HospitalId,
                        HospitalName = Ass.HospitalName,
                        HospitalNameAr = Ass.HospitalNameAr,
                        SupplierName = Ass.SupplierName,
                        SupplierNameAr = Ass.SupplierNameAr,
                        OrgName = Ass.OrgName,
                        OrgNameAr = Ass.OrgNameAr,
                        PurchaseDate = Ass.PurchaseDate
                    }).ToList();
                    if (departmentGroupObj.AssetList.Count > 0)
                    {
                        lstAssetDepartments.Add(departmentGroupObj);
                    }
                }
            }
            return lstAssetDepartments;
        }
        public List<BrandGroupVM> GetAssetByBrands(List<IndexAssetDetailVM.GetData> AssetModel)
        {
            List<BrandGroupVM> lstAssetBrand = new List<BrandGroupVM>();
            var lstBrands = (from gov in _context.Brands
                             select gov).ToList()
                            .GroupBy(a => a.Id).ToList();

            if (lstBrands.Count > 0)
            {
                foreach (var item in lstBrands)
                {
                    BrandGroupVM AssetBrandObj = new BrandGroupVM();
                    AssetBrandObj.Id = item.FirstOrDefault().Id;
                    AssetBrandObj.Name = item.FirstOrDefault().Name;
                    AssetBrandObj.NameAr = item.FirstOrDefault().NameAr;

                    AssetBrandObj.AssetList = AssetModel.ToList().Where(e => e.BrandId == AssetBrandObj.Id)
                    .Select(Ass => new IndexAssetDetailVM.GetData
                    {
                        Id = Ass.Id,
                        AssetName = Ass.AssetName,
                        Barcode = Ass.Barcode,
                        SerialNumber = Ass.SerialNumber,
                        Model = Ass.Model,
                        DepartmentName = Ass.DepartmentName,
                        DepartmentNameAr = Ass.DepartmentNameAr,
                        // CreatedBy = item.CreatedBy,
                        AssetNameAr = Ass.AssetNameAr,
                        BrandName = Ass.BrandName,
                        BrandNameAr = Ass.BrandNameAr,
                        GovernorateName = Ass.GovernorateName,
                        GovernorateNameAr = Ass.GovernorateNameAr,
                        CityName = Ass.CityName,
                        CityNameAr = Ass.CityNameAr,
                        HospitalId = Ass.HospitalId,
                        HospitalName = Ass.HospitalName,
                        HospitalNameAr = Ass.HospitalNameAr,
                        SupplierName = Ass.SupplierName,
                        SupplierNameAr = Ass.SupplierNameAr,
                        OrgName = Ass.OrgName,
                        OrgNameAr = Ass.OrgNameAr,
                        PurchaseDate = Ass.PurchaseDate
                    }).ToList();
                    if (AssetBrandObj.AssetList.Count > 0)
                    {
                        lstAssetBrand.Add(AssetBrandObj);
                    }
                }
            }
            return lstAssetBrand;
        }
        public List<GroupHospitalVM> GetAssetByHospital(List<IndexAssetDetailVM.GetData> AssetModel)
        {
            List<GroupHospitalVM> lstAssetHospital = new List<GroupHospitalVM>();
            var lstHosps = (from hos in _context.Hospitals
                            select hos).ToList()
                            .GroupBy(a => a.Id).ToList();

            if (lstHosps.Count > 0)
            {
                foreach (var item in lstHosps)
                {
                    GroupHospitalVM AssetHospitalObj = new GroupHospitalVM();
                    AssetHospitalObj.Id = item.FirstOrDefault().Id;
                    AssetHospitalObj.Name = item.FirstOrDefault().Name;
                    AssetHospitalObj.NameAr = item.FirstOrDefault().NameAr;

                    AssetHospitalObj.AssetList = AssetModel.ToList().Where(e => e.HospitalId == AssetHospitalObj.Id)
                    .Select(Ass => new IndexAssetDetailVM.GetData
                    {
                        Id = Ass.Id,
                        CreatedBy = Ass.CreatedBy,
                        AssetName = Ass.AssetName,
                        AssetNameAr = Ass.AssetNameAr,
                        BrandName = Ass.BrandName,
                        BrandNameAr = Ass.BrandNameAr,
                        GovernorateName = Ass.GovernorateName,
                        GovernorateNameAr = Ass.GovernorateNameAr,
                        CityName = Ass.CityName,
                        CityNameAr = Ass.CityNameAr,
                        HospitalId = Ass.HospitalId,
                        HospitalName = Ass.HospitalName,
                        HospitalNameAr = Ass.HospitalNameAr,
                        SupplierName = Ass.SupplierName,
                        SupplierNameAr = Ass.SupplierNameAr,
                        OrgName = Ass.OrgName,
                        OrgNameAr = Ass.OrgNameAr,
                        PurchaseDate = Ass.PurchaseDate
                    }).ToList();
                    if (AssetHospitalObj.AssetList.Count > 0)
                    {
                        lstAssetHospital.Add(AssetHospitalObj);
                    }
                }
            }
            return lstAssetHospital;
        }
        public List<GroupGovernorateVM> GetAssetByGovernorate(List<IndexAssetDetailVM.GetData> AssetModel)
        {
            List<GroupGovernorateVM> lstAssetGovernorate = new List<GroupGovernorateVM>();
            var lstGovs = (from gov in _context.Governorates
                           select gov).ToList()
                            .GroupBy(a => a.Id).ToList();

            if (lstGovs.Count > 0)
            {
                foreach (var item in lstGovs)
                {
                    GroupGovernorateVM AssetGovernorateObj = new GroupGovernorateVM();
                    AssetGovernorateObj.Id = item.FirstOrDefault().Id;
                    AssetGovernorateObj.Name = item.FirstOrDefault().Name;
                    AssetGovernorateObj.NameAr = item.FirstOrDefault().NameAr;

                    AssetGovernorateObj.AssetList = AssetModel.ToList().Where(e => e.GovernorateId == AssetGovernorateObj.Id)
                    .Select(Ass => new IndexAssetDetailVM.GetData
                    {
                        Id = Ass.Id,
                        CreatedBy = Ass.CreatedBy,
                        AssetName = Ass.AssetName,
                        AssetNameAr = Ass.AssetNameAr,
                        BrandName = Ass.BrandName,
                        BrandNameAr = Ass.BrandNameAr,
                        GovernorateName = Ass.GovernorateName,
                        GovernorateNameAr = Ass.GovernorateNameAr,
                        CityName = Ass.CityName,
                        CityNameAr = Ass.CityNameAr,
                        HospitalId = Ass.HospitalId,
                        HospitalName = Ass.HospitalName,
                        HospitalNameAr = Ass.HospitalNameAr,
                        SupplierName = Ass.SupplierName,
                        SupplierNameAr = Ass.SupplierNameAr,
                        OrgName = Ass.OrgName,
                        OrgNameAr = Ass.OrgNameAr,
                        PurchaseDate = Ass.PurchaseDate
                    }).ToList();
                    if (AssetGovernorateObj.AssetList.Count > 0)
                    {
                        lstAssetGovernorate.Add(AssetGovernorateObj);
                    }
                }
            }
            return lstAssetGovernorate;
        }
        public List<GroupCityVM> GetAssetByCity(List<IndexAssetDetailVM.GetData> AssetModel)
        {
            List<GroupCityVM> lstAssetCity = new List<GroupCityVM>();
            var lstCities = (from city in _context.Cities
                             select city).ToList()
                            .GroupBy(a => a.Id).ToList();

            if (lstCities.Count > 0)
            {
                foreach (var item in lstCities)
                {
                    GroupCityVM AssetCityObj = new GroupCityVM();
                    AssetCityObj.Id = item.FirstOrDefault().Id;
                    AssetCityObj.Name = item.FirstOrDefault().Name;
                    AssetCityObj.NameAr = item.FirstOrDefault().NameAr;

                    AssetCityObj.AssetList = AssetModel.ToList().Where(e => e.CityId == AssetCityObj.Id)
                    .Select(Ass => new IndexAssetDetailVM.GetData
                    {
                        Id = Ass.Id,
                        CreatedBy = Ass.CreatedBy,
                        AssetName = Ass.AssetName,
                        AssetNameAr = Ass.AssetNameAr,
                        BrandName = Ass.BrandName,
                        BrandNameAr = Ass.BrandNameAr,
                        GovernorateName = Ass.GovernorateName,
                        GovernorateNameAr = Ass.GovernorateNameAr,
                        CityName = Ass.CityName,
                        CityNameAr = Ass.CityNameAr,
                        HospitalId = Ass.HospitalId,
                        HospitalName = Ass.HospitalName,
                        HospitalNameAr = Ass.HospitalNameAr,
                        SupplierName = Ass.SupplierName,
                        SupplierNameAr = Ass.SupplierNameAr,
                        OrgName = Ass.OrgName,
                        OrgNameAr = Ass.OrgNameAr,
                        PurchaseDate = Ass.PurchaseDate
                    }).ToList();
                    if (AssetCityObj.AssetList.Count > 0)
                    {
                        lstAssetCity.Add(AssetCityObj);
                    }
                }
            }
            return lstAssetCity;
        }
        public List<GroupSupplierVM> GetAssetBySupplier(List<IndexAssetDetailVM.GetData> AssetModel)
        {
            List<GroupSupplierVM> lstAssetSupplier = new List<GroupSupplierVM>();
            var lstsuppliers = (from sup in _context.Suppliers
                                select sup).ToList()
                            .GroupBy(a => a.Id).ToList();

            if (lstsuppliers.Count > 0)
            {
                foreach (var item in lstsuppliers)
                {
                    GroupSupplierVM AssetSupplierObj = new GroupSupplierVM();
                    AssetSupplierObj.Id = item.FirstOrDefault().Id;
                    AssetSupplierObj.Name = item.FirstOrDefault().Name;
                    AssetSupplierObj.NameAr = item.FirstOrDefault().NameAr;

                    AssetSupplierObj.AssetList = AssetModel.ToList().Where(e => e.SupplierId == AssetSupplierObj.Id)
                    .Select(Ass => new IndexAssetDetailVM.GetData
                    {
                        Id = Ass.Id,
                        CreatedBy = Ass.CreatedBy,
                        Barcode = Ass.Barcode,
                        SerialNumber = Ass.SerialNumber,
                        Model = Ass.Model,
                        AssetName = Ass.AssetName,
                        AssetNameAr = Ass.AssetNameAr,
                        BrandName = Ass.BrandName,
                        BrandNameAr = Ass.BrandNameAr,
                        SupplierName = Ass.SupplierName,
                        SupplierNameAr = Ass.SupplierNameAr,
                        GovernorateName = Ass.GovernorateName,
                        GovernorateNameAr = Ass.GovernorateNameAr,
                        CityName = Ass.CityName,
                        CityNameAr = Ass.CityNameAr,
                        HospitalId = Ass.HospitalId,
                        HospitalName = Ass.HospitalName,
                        HospitalNameAr = Ass.HospitalNameAr,
                        DepartmentName = Ass.DepartmentName,
                        DepartmentNameAr = Ass.DepartmentNameAr,
                        OrgName = Ass.OrgName,
                        OrgNameAr = Ass.OrgNameAr,
                        PurchaseDate = Ass.PurchaseDate
                    }).ToList();
                    if (AssetSupplierObj.AssetList.Count > 0)
                    {
                        lstAssetSupplier.Add(AssetSupplierObj);
                    }
                }
            }
            return lstAssetSupplier;
        }
        public List<GroupOrganizationVM> GetAssetByOrganization(List<IndexAssetDetailVM.GetData> AssetModel)
        {
            List<GroupOrganizationVM> lstAssetOrganization = new List<GroupOrganizationVM>();
            var lstorganizations = (from org in _context.Organizations
                                    select org).ToList()
                            .GroupBy(a => a.Id).ToList();

            if (lstorganizations.Count > 0)
            {
                foreach (var item in lstorganizations)
                {
                    GroupOrganizationVM AssetOrganizationObj = new GroupOrganizationVM();
                    AssetOrganizationObj.Id = item.FirstOrDefault().Id;
                    AssetOrganizationObj.Name = item.FirstOrDefault().Name;
                    AssetOrganizationObj.NameAr = item.FirstOrDefault().NameAr;

                    AssetOrganizationObj.AssetList = AssetModel.ToList().Where(e => e.OrganizationId == AssetOrganizationObj.Id)
                    .Select(Ass => new IndexAssetDetailVM.GetData
                    {
                        Id = Ass.Id,
                        CreatedBy = Ass.CreatedBy,
                        AssetName = Ass.AssetName,
                        AssetNameAr = Ass.AssetNameAr,
                        BrandName = Ass.BrandName,
                        BrandNameAr = Ass.BrandNameAr,
                        GovernorateName = Ass.GovernorateName,
                        GovernorateNameAr = Ass.GovernorateNameAr,
                        CityName = Ass.CityName,
                        CityNameAr = Ass.CityNameAr,
                        HospitalId = Ass.HospitalId,
                        HospitalName = Ass.HospitalName,
                        HospitalNameAr = Ass.HospitalNameAr,
                        SupplierName = Ass.SupplierName,
                        SupplierNameAr = Ass.SupplierNameAr,
                        OrgName = Ass.OrgName,
                        OrgNameAr = Ass.OrgNameAr,
                        PurchaseDate = Ass.PurchaseDate
                    }).ToList();
                    if (AssetOrganizationObj.AssetList.Count > 0)
                    {
                        lstAssetOrganization.Add(AssetOrganizationObj);
                    }
                }
            }
            return lstAssetOrganization;
        }
        public IEnumerable<IndexAssetDetailVM.GetData> SortAssets(Sort sortObj)
        {
            List<IndexAssetDetailVM.GetData> lstAssetData = new List<IndexAssetDetailVM.GetData>();
            ApplicationRole roleObj = new ApplicationRole();
            Employee empObj = new Employee();
            List<string> userRoleNames = new List<string>();
            ApplicationUser userObj = new ApplicationUser();
            if (sortObj.UserId != null)
            {

                var obj = _context.ApplicationUser.Where(a => a.Id == sortObj.UserId).ToList();
                userObj = obj[0];

                var roles = (from userRole in _context.UserRoles
                             join role in _context.ApplicationRole on userRole.RoleId equals role.Id
                             where userRole.UserId == userObj.Id
                             select role);
                foreach (var role in roles)
                {
                    userRoleNames.Add(role.Name);
                }
                var lstEmployees = _context.Employees.Where(a => a.Email == userObj.Email).ToList();
                if (lstEmployees.Count > 0)
                {
                    empObj = lstEmployees[0];
                }
            }

            if (userRoleNames.Contains("AssetOwner"))
            {
                var lstAssetOwners = _context.AssetOwners
                                .Include(a => a.AssetDetail).Include(a => a.AssetDetail.MasterAsset)
                                .Include(a => a.AssetDetail.Hospital)
                                .Include(a => a.AssetDetail.Supplier).Include(a => a.AssetDetail.MasterAsset.brand)
                                .Include(a => a.AssetDetail.Hospital.Governorate).Include(a => a.AssetDetail.Hospital.City)
                                .Include(a => a.AssetDetail.Hospital.Organization).Include(a => a.AssetDetail.Hospital.SubOrganization)
                                .OrderBy(a => a.AssetDetail.Barcode).ToList();


                if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                {
                    lstAssetOwners = lstAssetOwners.ToList();
                }
                if (userObj.GovernorateId > 0 && userObj.CityId == 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                {
                    lstAssetOwners = lstAssetOwners.Where(a => a.AssetDetail.Hospital.GovernorateId == userObj.GovernorateId).ToList();
                }
                if (userObj.GovernorateId > 0 && userObj.CityId > 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                {
                    lstAssetOwners = lstAssetOwners.Where(a => a.AssetDetail.Hospital.GovernorateId == userObj.GovernorateId && a.AssetDetail.Hospital.CityId == userObj.CityId).ToList();
                }
                if (userObj.GovernorateId > 0 && userObj.CityId > 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId > 0)
                {
                    if (userRoleNames.Contains("AssetOwner"))
                    {
                        lstAssetOwners = lstAssetOwners.Where(a => a.AssetDetail.Hospital.GovernorateId == userObj.GovernorateId && a.AssetDetail.Hospital.CityId == userObj.CityId && a.HospitalId == userObj.HospitalId && a.EmployeeId == empObj.Id).ToList();
                    }
                    else
                    {
                        lstAssetOwners = lstAssetOwners.Where(a => a.AssetDetail.Hospital.GovernorateId == userObj.GovernorateId && a.AssetDetail.Hospital.CityId == userObj.CityId && a.HospitalId == userObj.HospitalId).ToList();
                    }
                }
                if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId > 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                {
                    lstAssetOwners = lstAssetOwners.Where(a => a.AssetDetail.Hospital.OrganizationId == userObj.OrganizationId).ToList();
                }
                if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId > 0 && userObj.SubOrganizationId > 0 && userObj.HospitalId == 0)
                {
                    lstAssetOwners = lstAssetOwners.Where(a => a.AssetDetail.Hospital.OrganizationId == userObj.OrganizationId && a.AssetDetail.Hospital.SubOrganizationId == userObj.SubOrganizationId).ToList();
                }
                if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId > 0 && userObj.SubOrganizationId > 0 && userObj.HospitalId > 0)
                {
                    lstAssetOwners = lstAssetOwners.Where(a => a.AssetDetail.HospitalId == userObj.HospitalId).ToList();
                }


                if (sortObj.GovernorateId != 0)
                {
                    lstAssetOwners = lstAssetOwners.Where(a => a.AssetDetail.Hospital.GovernorateId == sortObj.GovernorateId).ToList();
                }
                else
                {
                    lstAssetOwners = lstAssetOwners.ToList();
                }
                if (sortObj.CityId != 0)
                {
                    lstAssetOwners = lstAssetOwners.Where(a => a.AssetDetail.Hospital.CityId == sortObj.CityId).ToList();
                }
                else
                {
                    lstAssetOwners = lstAssetOwners.ToList();
                }
                if (sortObj.OrganizationId != 0)
                {
                    lstAssetOwners = lstAssetOwners.Where(a => a.AssetDetail.Hospital.OrganizationId == sortObj.OrganizationId).ToList();
                }
                else
                {
                    lstAssetOwners = lstAssetOwners.ToList();
                }
                if (sortObj.SubOrganizationId != 0)
                {
                    lstAssetOwners = lstAssetOwners.Where(a => a.AssetDetail.Hospital.SubOrganizationId == sortObj.SubOrganizationId).ToList();
                }
                else
                {
                    lstAssetOwners = lstAssetOwners.ToList();
                }
                if (sortObj.HospitalId != 0)
                {
                    lstAssetOwners = lstAssetOwners.Where(a => a.AssetDetail.HospitalId == sortObj.HospitalId).ToList();
                }
                else
                {
                    lstAssetOwners = lstAssetOwners.ToList();
                }
                if (sortObj.MasterAssetId != 0)
                {
                    lstAssetOwners = lstAssetOwners.Where(a => a.AssetDetail.MasterAssetId == sortObj.MasterAssetId).ToList();
                }
                else
                {
                    lstAssetOwners = lstAssetOwners.ToList();
                }
                if (sortObj.BarCodeValue != "")
                {
                    lstAssetOwners = lstAssetOwners.Where(a => a.AssetDetail.Barcode == sortObj.BarCodeValue).ToList();
                }
                else
                {
                    lstAssetOwners = lstAssetOwners.ToList();
                }
                if (sortObj.SerialValue != "")
                {
                    lstAssetOwners = lstAssetOwners.Where(a => a.AssetDetail.SerialNumber == sortObj.SerialValue).ToList();
                }
                else
                {
                    lstAssetOwners = lstAssetOwners.ToList();
                }

                foreach (var item in lstAssetOwners)
                {
                    IndexAssetDetailVM.GetData Assetobj = new IndexAssetDetailVM.GetData();
                    Assetobj.Id = item.AssetDetail.Id;
                    Assetobj.Code = item.AssetDetail.Code;
                    Assetobj.BarCode = item.AssetDetail.Barcode;
                    Assetobj.Model = item.AssetDetail.MasterAsset.ModelNumber;
                    Assetobj.Serial = item.AssetDetail.SerialNumber;
                    Assetobj.CreatedBy = item.AssetDetail.CreatedBy;
                    Assetobj.BrandName = item.AssetDetail.MasterAsset.BrandId > 0 ? item.AssetDetail.MasterAsset.brand.Name : "";
                    Assetobj.BrandNameAr = item.AssetDetail.MasterAsset.BrandId > 0 ? item.AssetDetail.MasterAsset.brand.NameAr : "";
                    Assetobj.SupplierName = item.AssetDetail.SupplierId > 0 ? item.AssetDetail.Supplier.Name : "";
                    Assetobj.SupplierNameAr = item.AssetDetail.SupplierId > 0 ? item.AssetDetail.Supplier.NameAr : "";
                    Assetobj.HospitalId = item.HospitalId;
                    Assetobj.HospitalName = item.HospitalId > 0 ? item.Hospital.Name : "";
                    Assetobj.HospitalNameAr = item.HospitalId > 0 ? item.Hospital.NameAr : "";
                    Assetobj.AssetName = item.AssetDetail.MasterAssetId > 0 ? item.AssetDetail.MasterAsset.Name : "";
                    Assetobj.AssetNameAr = item.AssetDetail.MasterAssetId > 0 ? item.AssetDetail.MasterAsset.NameAr : "";
                    Assetobj.GovernorateName = item.HospitalId > 0 ? item.Hospital.Governorate.Name : "";
                    Assetobj.GovernorateNameAr = item.HospitalId > 0 ? item.Hospital.Governorate.NameAr : "";

                    Assetobj.GovernorateId = item.Hospital.GovernorateId;
                    Assetobj.CityId = item.Hospital.CityId;
                    Assetobj.OrganizationId = item.Hospital.OrganizationId;
                    Assetobj.SubOrganizationId = item.Hospital.SubOrganizationId;

                    Assetobj.MasterAssetId = item.AssetDetail.MasterAssetId;
                    Assetobj.GovernorateName = item.Hospital.Governorate.Name;
                    Assetobj.GovernorateNameAr = item.Hospital.Governorate.NameAr;
                    Assetobj.CityName = item.Hospital.City.Name;
                    Assetobj.CityNameAr = item.Hospital.City.NameAr;
                    Assetobj.OrgName = item.Hospital.Organization.Name;
                    Assetobj.OrgNameAr = item.Hospital.Organization.NameAr;
                    Assetobj.SubOrgName = item.Hospital.SubOrganization.Name;
                    Assetobj.SubOrgNameAr = item.Hospital.SubOrganization.NameAr;
                    Assetobj.QrFilePath = item.AssetDetail.QrFilePath;

                    var lstStatus = _context.AssetStatusTransactions.Where(a => a.AssetDetailId == item.Id).OrderByDescending(a => a.StatusDate.Value.Date).ToList();
                    if (lstStatus.Count > 0)
                    {
                        Assetobj.AssetStatusId = lstStatus.FirstOrDefault().AssetStatusId;
                    }
                    lstAssetData.Add(Assetobj);
                }


                if (sortObj.AssetName != "")
                {
                    if (sortObj.BarCodeValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                        }
                    }
                    if (sortObj.SerialValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.GovernorateId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.CityId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OriginId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SubOrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstAssetData = lstAssetData.OrderByDescending(d => d.AssetName).ToList();
                        else
                            lstAssetData = lstAssetData.OrderBy(d => d.AssetName).ToList();
                    }
                }
                else if (sortObj.AssetNameAr != "")
                {
                    if (sortObj.BarCodeValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                        }
                    }
                    if (sortObj.SerialValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.GovernorateId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.CityId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OriginId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SubOrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstAssetData = lstAssetData.OrderByDescending(d => d.AssetNameAr).ToList();
                        else
                            lstAssetData = lstAssetData.OrderBy(d => d.AssetNameAr).ToList();
                    }
                }
                else if (sortObj.GovernorateName != "")
                {
                    if (sortObj.BarCodeValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                        }
                    }
                    if (sortObj.SerialValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.GovernorateId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.CityId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OriginId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SubOrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstAssetData = lstAssetData.OrderByDescending(d => d.GovernorateName).ToList();
                        else
                            lstAssetData = lstAssetData.OrderBy(d => d.GovernorateName).ToList();
                    }
                }
                else if (sortObj.GovernorateNameAr != "")
                {
                    if (sortObj.BarCodeValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                        }
                    }
                    if (sortObj.SerialValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.GovernorateId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.CityId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OriginId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SubOrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstAssetData = lstAssetData.OrderByDescending(d => d.GovernorateNameAr).ToList();
                        else
                            lstAssetData = lstAssetData.OrderBy(d => d.GovernorateNameAr).ToList();
                    }
                }
                else if (sortObj.HospitalName != "")
                {
                    if (sortObj.BarCodeValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                        }
                    }
                    if (sortObj.SerialValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.GovernorateId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.CityId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OriginId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SubOrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstAssetData = lstAssetData.OrderByDescending(d => d.HospitalName).ToList();
                        else
                            lstAssetData = lstAssetData.OrderBy(d => d.HospitalName).ToList();
                    }
                }
                else if (sortObj.HospitalNameAr != "")
                {
                    if (sortObj.BarCodeValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                        }
                    }
                    if (sortObj.SerialValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.GovernorateId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.CityId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OriginId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SubOrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstAssetData = lstAssetData.OrderByDescending(d => d.HospitalNameAr).ToList();
                        else
                            lstAssetData = lstAssetData.OrderBy(d => d.HospitalNameAr).ToList();
                    }
                }
                else if (sortObj.GovernorateName != "")
                {
                    if (sortObj.BarCodeValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                        }
                    }
                    if (sortObj.SerialValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.GovernorateId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.CityId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OriginId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SubOrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstAssetData = lstAssetData.OrderByDescending(d => d.GovernorateName).ToList();
                        else
                            lstAssetData = lstAssetData.OrderBy(d => d.GovernorateName).ToList();
                    }
                }
                else if (sortObj.GovernorateNameAr != "")
                {
                    if (sortObj.BarCodeValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                        }
                    }
                    if (sortObj.SerialValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.GovernorateId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.CityId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OriginId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SubOrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstAssetData = lstAssetData.OrderByDescending(d => d.GovernorateNameAr).ToList();
                        else
                            lstAssetData = lstAssetData.OrderBy(d => d.GovernorateNameAr).ToList();
                    }
                }
                else if (sortObj.OrgName != "")
                {
                    if (sortObj.BarCodeValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                        }
                    }
                    if (sortObj.SerialValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.GovernorateId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.CityId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OriginId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SubOrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstAssetData = lstAssetData.OrderByDescending(d => d.OrgName).ToList();
                        else
                            lstAssetData = lstAssetData.OrderBy(d => d.OrgName).ToList();
                    }
                }
                else if (sortObj.OrgNameAr != "")
                {
                    if (sortObj.BarCodeValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                        }
                    }
                    if (sortObj.SerialValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.GovernorateId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.CityId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OriginId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SubOrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstAssetData = lstAssetData.OrderByDescending(d => d.OrgNameAr).ToList();
                        else
                            lstAssetData = lstAssetData.OrderBy(d => d.OrgNameAr).ToList();
                    }
                }
                else if (sortObj.SubOrgName != "")
                {
                    if (sortObj.BarCodeValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                        }
                    }
                    if (sortObj.SerialValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.GovernorateId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.CityId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OriginId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SubOrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstAssetData = lstAssetData.OrderByDescending(d => d.SubOrgName).ToList();
                        else
                            lstAssetData = lstAssetData.OrderBy(d => d.SubOrgName).ToList();
                    }
                }
                else if (sortObj.SubOrgNameAr != "")
                {

                    if (sortObj.BarCodeValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                        }
                    }
                    if (sortObj.SerialValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.GovernorateId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.CityId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OriginId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SubOrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstAssetData = lstAssetData.OrderByDescending(d => d.SubOrgNameAr).ToList();
                        else
                            lstAssetData = lstAssetData.OrderBy(d => d.SubOrgNameAr).ToList();
                    }
                }
                else if (sortObj.BarCode != "")
                {
                    if (sortObj.BarCodeValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                        }
                    }
                    if (sortObj.SerialValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.GovernorateId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.CityId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OriginId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SubOrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstAssetData = lstAssetData.OrderByDescending(d => d.BarCode).ToList();
                        else
                            lstAssetData = lstAssetData.OrderBy(d => d.BarCode).ToList();
                    }
                }
                else if (sortObj.Serial != "")
                {
                    if (sortObj.BarCodeValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                        }
                    }
                    if (sortObj.SerialValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.GovernorateId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.CityId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OriginId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SubOrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                        }
                    }

                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstAssetData = lstAssetData.OrderByDescending(d => d.Serial).ToList();
                        else
                            lstAssetData = lstAssetData.OrderBy(d => d.Serial).ToList();
                    }
                }
                else if (sortObj.Model != "" && sortObj.Model != null)
                {
                    if (sortObj.BarCodeValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                        }
                    }
                    if (sortObj.SerialValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.GovernorateId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.CityId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OriginId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SubOrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstAssetData = lstAssetData.OrderByDescending(d => d.Model).ToList();
                        else
                            lstAssetData = lstAssetData.OrderBy(d => d.Model).ToList();
                    }
                }
                else if (sortObj.BrandName != "" && sortObj.BrandName != null)
                {
                    if (sortObj.BarCodeValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                        }
                    }
                    if (sortObj.SerialValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.GovernorateId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.CityId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OriginId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SubOrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstAssetData = lstAssetData.OrderByDescending(d => d.BrandName).ToList();
                        else
                            lstAssetData = lstAssetData.OrderBy(d => d.BrandName).ToList();
                    }
                }
                else if (sortObj.BrandNameAr != "" && sortObj.BrandNameAr != null)
                {
                    if (sortObj.BarCodeValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                        }
                    }
                    if (sortObj.SerialValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.GovernorateId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.CityId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OriginId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SubOrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstAssetData = lstAssetData.OrderByDescending(d => d.BrandNameAr).ToList();
                        else
                            lstAssetData = lstAssetData.OrderBy(d => d.BrandNameAr).ToList();
                    }
                }
                else if (sortObj.SupplierName != "" && sortObj.SupplierName != null)
                {
                    if (sortObj.BarCodeValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                        }
                    }
                    if (sortObj.SerialValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.GovernorateId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.CityId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OriginId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SubOrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstAssetData = lstAssetData.OrderByDescending(d => d.SupplierName).ToList();
                        else
                            lstAssetData = lstAssetData.OrderBy(d => d.SupplierName).ToList();
                    }
                }
                else if (sortObj.SupplierNameAr != "" && sortObj.SupplierNameAr != null)
                {
                    if (sortObj.BarCodeValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                        }
                    }
                    if (sortObj.SerialValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.GovernorateId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.CityId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OriginId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SubOrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstAssetData = lstAssetData.OrderByDescending(d => d.SupplierNameAr).ToList();
                        else
                            lstAssetData = lstAssetData.OrderBy(d => d.SupplierNameAr).ToList();
                    }
                }

                return lstAssetData;
            }
            else
            {
                var lstAssetOwners = _context.AssetDetails
                                        .Include(a => a.MasterAsset)
                                        .Include(a => a.Hospital)
                                        .Include(a => a.Supplier).Include(a => a.MasterAsset.brand)
                                        .Include(a => a.Hospital.Governorate).Include(a => a.Hospital.City)
                                        .Include(a => a.Hospital.Organization).Include(a => a.Hospital.SubOrganization)
                                        .OrderBy(a => a.Barcode).ToList();


                if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                {
                    lstAssetOwners = lstAssetOwners.ToList();
                }
                if (userObj.GovernorateId > 0 && userObj.CityId == 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                {
                    lstAssetOwners = lstAssetOwners.Where(a => a.Hospital.GovernorateId == userObj.GovernorateId).ToList();
                }
                if (userObj.GovernorateId > 0 && userObj.CityId > 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                {
                    lstAssetOwners = lstAssetOwners.Where(a => a.Hospital.GovernorateId == userObj.GovernorateId && a.Hospital.CityId == userObj.CityId).ToList();
                }
                if (userObj.GovernorateId > 0 && userObj.CityId > 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId > 0)
                {

                    lstAssetOwners = lstAssetOwners.Where(a => a.Hospital.GovernorateId == userObj.GovernorateId && a.Hospital.CityId == userObj.CityId && a.HospitalId == userObj.HospitalId).ToList();

                }
                if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId > 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                {
                    lstAssetOwners = lstAssetOwners.Where(a => a.Hospital.OrganizationId == userObj.OrganizationId).ToList();
                }
                if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId > 0 && userObj.SubOrganizationId > 0 && userObj.HospitalId == 0)
                {
                    lstAssetOwners = lstAssetOwners.Where(a => a.Hospital.OrganizationId == userObj.OrganizationId && a.Hospital.SubOrganizationId == userObj.SubOrganizationId).ToList();
                }
                if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId > 0 && userObj.SubOrganizationId > 0 && userObj.HospitalId > 0)
                {
                    lstAssetOwners = lstAssetOwners.Where(a => a.HospitalId == userObj.HospitalId).ToList();
                }


                if (sortObj.GovernorateId != 0)
                {
                    lstAssetOwners = lstAssetOwners.Where(a => a.Hospital.GovernorateId == sortObj.GovernorateId).ToList();
                }
                else
                {
                    lstAssetOwners = lstAssetOwners.ToList();
                }
                if (sortObj.CityId != 0)
                {
                    lstAssetOwners = lstAssetOwners.Where(a => a.Hospital.CityId == sortObj.CityId).ToList();
                }
                else
                {
                    lstAssetOwners = lstAssetOwners.ToList();
                }
                if (sortObj.OrganizationId != 0)
                {
                    lstAssetOwners = lstAssetOwners.Where(a => a.Hospital.OrganizationId == sortObj.OrganizationId).ToList();
                }
                else
                {
                    lstAssetOwners = lstAssetOwners.ToList();
                }
                if (sortObj.SubOrganizationId != 0)
                {
                    lstAssetOwners = lstAssetOwners.Where(a => a.Hospital.SubOrganizationId == sortObj.SubOrganizationId).ToList();
                }
                else
                {
                    lstAssetOwners = lstAssetOwners.ToList();
                }
                if (sortObj.HospitalId != 0)
                {
                    lstAssetOwners = lstAssetOwners.Where(a => a.HospitalId == sortObj.HospitalId).ToList();
                }
                else
                {
                    lstAssetOwners = lstAssetOwners.ToList();
                }
                if (sortObj.MasterAssetId != 0)
                {
                    lstAssetOwners = lstAssetOwners.Where(a => a.MasterAssetId == sortObj.MasterAssetId).ToList();
                }
                else
                {
                    lstAssetOwners = lstAssetOwners.ToList();
                }
                if (sortObj.BarCodeValue != "")
                {
                    lstAssetOwners = lstAssetOwners.Where(a => a.Barcode == sortObj.BarCodeValue).ToList();
                }
                else
                {
                    lstAssetOwners = lstAssetOwners.ToList();
                }
                if (sortObj.SerialValue != "")
                {
                    lstAssetOwners = lstAssetOwners.Where(a => a.SerialNumber == sortObj.SerialValue).ToList();
                }
                else
                {
                    lstAssetOwners = lstAssetOwners.ToList();
                }

                foreach (var item in lstAssetOwners)
                {
                    IndexAssetDetailVM.GetData Assetobj = new IndexAssetDetailVM.GetData();
                    Assetobj.Id = item.Id;
                    Assetobj.Code = item.Code;
                    Assetobj.BarCode = item.Barcode;
                    Assetobj.Model = item.MasterAsset.ModelNumber;
                    Assetobj.Serial = item.SerialNumber;

                    Assetobj.BrandName = item.MasterAsset.BrandId > 0 ? item.MasterAsset.brand.Name : "";
                    Assetobj.BrandNameAr = item.MasterAsset.BrandId > 0 ? item.MasterAsset.brand.NameAr : "";
                    Assetobj.SupplierName = item.SupplierId > 0 ? item.Supplier.Name : "";
                    Assetobj.SupplierNameAr = item.SupplierId > 0 ? item.Supplier.NameAr : "";
                    Assetobj.HospitalId = item.HospitalId;
                    Assetobj.HospitalName = item.HospitalId > 0 ? item.Hospital.Name : "";
                    Assetobj.HospitalNameAr = item.HospitalId > 0 ? item.Hospital.NameAr : "";
                    Assetobj.AssetName = item.MasterAssetId > 0 ? item.MasterAsset.Name : "";
                    Assetobj.AssetNameAr = item.MasterAssetId > 0 ? item.MasterAsset.NameAr : "";
                    Assetobj.GovernorateName = item.HospitalId > 0 ? item.Hospital.Governorate.Name : "";
                    Assetobj.GovernorateNameAr = item.HospitalId > 0 ? item.Hospital.Governorate.NameAr : "";

                    Assetobj.GovernorateId = item.Hospital.GovernorateId;
                    Assetobj.CityId = item.Hospital.CityId;
                    Assetobj.OrganizationId = item.Hospital.OrganizationId;
                    Assetobj.SubOrganizationId = item.Hospital.SubOrganizationId;

                    Assetobj.MasterAssetId = item.MasterAssetId;
                    Assetobj.GovernorateName = item.Hospital.Governorate.Name;
                    Assetobj.GovernorateNameAr = item.Hospital.Governorate.NameAr;
                    Assetobj.CityName = item.Hospital.City.Name;
                    Assetobj.CityNameAr = item.Hospital.City.NameAr;
                    Assetobj.OrgName = item.Hospital.Organization.Name;
                    Assetobj.OrgNameAr = item.Hospital.Organization.NameAr;
                    Assetobj.SubOrgName = item.Hospital.SubOrganization.Name;
                    Assetobj.SubOrgNameAr = item.Hospital.SubOrganization.NameAr;
                    Assetobj.QrFilePath = item.QrFilePath;

                    var lstStatus = _context.AssetStatusTransactions.Where(a => a.AssetDetailId == item.Id).OrderByDescending(a => a.StatusDate.Value.Date).ToList();
                    if (lstStatus.Count > 0)
                    {
                        Assetobj.AssetStatusId = lstStatus.FirstOrDefault().AssetStatusId;
                    }
                    lstAssetData.Add(Assetobj);
                }


                if (sortObj.AssetName != "")
                {
                    if (sortObj.BarCodeValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                        }
                    }
                    if (sortObj.SerialValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.GovernorateId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.CityId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OriginId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SubOrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstAssetData = lstAssetData.OrderByDescending(d => d.AssetName).ToList();
                        else
                            lstAssetData = lstAssetData.OrderBy(d => d.AssetName).ToList();
                    }
                }
                else if (sortObj.AssetNameAr != "")
                {
                    if (sortObj.BarCodeValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                        }
                    }
                    if (sortObj.SerialValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.GovernorateId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.CityId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OriginId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SubOrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstAssetData = lstAssetData.OrderByDescending(d => d.AssetNameAr).ToList();
                        else
                            lstAssetData = lstAssetData.OrderBy(d => d.AssetNameAr).ToList();
                    }
                }
                else if (sortObj.GovernorateName != "")
                {
                    if (sortObj.BarCodeValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                        }
                    }
                    if (sortObj.SerialValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.GovernorateId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.CityId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OriginId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SubOrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstAssetData = lstAssetData.OrderByDescending(d => d.GovernorateName).ToList();
                        else
                            lstAssetData = lstAssetData.OrderBy(d => d.GovernorateName).ToList();
                    }
                }
                else if (sortObj.GovernorateNameAr != "")
                {
                    if (sortObj.BarCodeValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                        }
                    }
                    if (sortObj.SerialValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.GovernorateId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.CityId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OriginId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SubOrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstAssetData = lstAssetData.OrderByDescending(d => d.GovernorateNameAr).ToList();
                        else
                            lstAssetData = lstAssetData.OrderBy(d => d.GovernorateNameAr).ToList();
                    }
                }
                else if (sortObj.HospitalName != "")
                {
                    if (sortObj.BarCodeValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                        }
                    }
                    if (sortObj.SerialValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.GovernorateId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.CityId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OriginId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SubOrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstAssetData = lstAssetData.OrderByDescending(d => d.HospitalName).ToList();
                        else
                            lstAssetData = lstAssetData.OrderBy(d => d.HospitalName).ToList();
                    }
                }
                else if (sortObj.HospitalNameAr != "")
                {
                    if (sortObj.BarCodeValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                        }
                    }
                    if (sortObj.SerialValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.GovernorateId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.CityId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OriginId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SubOrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstAssetData = lstAssetData.OrderByDescending(d => d.HospitalNameAr).ToList();
                        else
                            lstAssetData = lstAssetData.OrderBy(d => d.HospitalNameAr).ToList();
                    }
                }
                else if (sortObj.GovernorateName != "")
                {
                    if (sortObj.BarCodeValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                        }
                    }
                    if (sortObj.SerialValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.GovernorateId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.CityId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OriginId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SubOrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstAssetData = lstAssetData.OrderByDescending(d => d.GovernorateName).ToList();
                        else
                            lstAssetData = lstAssetData.OrderBy(d => d.GovernorateName).ToList();
                    }
                }
                else if (sortObj.GovernorateNameAr != "")
                {
                    if (sortObj.BarCodeValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                        }
                    }
                    if (sortObj.SerialValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.GovernorateId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.CityId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OriginId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SubOrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstAssetData = lstAssetData.OrderByDescending(d => d.GovernorateNameAr).ToList();
                        else
                            lstAssetData = lstAssetData.OrderBy(d => d.GovernorateNameAr).ToList();
                    }
                }
                else if (sortObj.OrgName != "")
                {
                    if (sortObj.BarCodeValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                        }
                    }
                    if (sortObj.SerialValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.GovernorateId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.CityId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OriginId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SubOrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstAssetData = lstAssetData.OrderByDescending(d => d.OrgName).ToList();
                        else
                            lstAssetData = lstAssetData.OrderBy(d => d.OrgName).ToList();
                    }
                }
                else if (sortObj.OrgNameAr != "")
                {
                    if (sortObj.BarCodeValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                        }
                    }
                    if (sortObj.SerialValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.GovernorateId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.CityId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OriginId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SubOrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstAssetData = lstAssetData.OrderByDescending(d => d.OrgNameAr).ToList();
                        else
                            lstAssetData = lstAssetData.OrderBy(d => d.OrgNameAr).ToList();
                    }
                }
                else if (sortObj.SubOrgName != "")
                {
                    if (sortObj.BarCodeValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                        }
                    }
                    if (sortObj.SerialValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.GovernorateId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.CityId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OriginId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SubOrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstAssetData = lstAssetData.OrderByDescending(d => d.SubOrgName).ToList();
                        else
                            lstAssetData = lstAssetData.OrderBy(d => d.SubOrgName).ToList();
                    }
                }
                else if (sortObj.SubOrgNameAr != "")
                {

                    if (sortObj.BarCodeValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                        }
                    }
                    if (sortObj.SerialValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.GovernorateId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.CityId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OriginId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SubOrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstAssetData = lstAssetData.OrderByDescending(d => d.SubOrgNameAr).ToList();
                        else
                            lstAssetData = lstAssetData.OrderBy(d => d.SubOrgNameAr).ToList();
                    }
                }
                else if (sortObj.BarCode != "")
                {
                    if (sortObj.BarCodeValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                        }
                    }
                    if (sortObj.SerialValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.GovernorateId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.CityId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OriginId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SubOrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstAssetData = lstAssetData.OrderByDescending(d => d.BarCode).ToList();
                        else
                            lstAssetData = lstAssetData.OrderBy(d => d.BarCode).ToList();
                    }
                }
                else if (sortObj.Serial != "")
                {
                    if (sortObj.BarCodeValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                        }
                    }
                    if (sortObj.SerialValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.GovernorateId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.CityId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OriginId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SubOrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                        }
                    }

                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstAssetData = lstAssetData.OrderByDescending(d => d.Serial).ToList();
                        else
                            lstAssetData = lstAssetData.OrderBy(d => d.Serial).ToList();
                    }
                }
                else if (sortObj.Model != "" && sortObj.Model != null)
                {
                    if (sortObj.BarCodeValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                        }
                    }
                    if (sortObj.SerialValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.GovernorateId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.CityId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OriginId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SubOrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstAssetData = lstAssetData.OrderByDescending(d => d.Model).ToList();
                        else
                            lstAssetData = lstAssetData.OrderBy(d => d.Model).ToList();
                    }
                }
                else if (sortObj.BrandName != "" && sortObj.BrandName != null)
                {
                    if (sortObj.BarCodeValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                        }
                    }
                    if (sortObj.SerialValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.GovernorateId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.CityId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OriginId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SubOrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstAssetData = lstAssetData.OrderByDescending(d => d.BrandName).ToList();
                        else
                            lstAssetData = lstAssetData.OrderBy(d => d.BrandName).ToList();
                    }
                }
                else if (sortObj.BrandNameAr != "" && sortObj.BrandNameAr != null)
                {
                    if (sortObj.BarCodeValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                        }
                    }
                    if (sortObj.SerialValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.GovernorateId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.CityId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OriginId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SubOrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstAssetData = lstAssetData.OrderByDescending(d => d.BrandNameAr).ToList();
                        else
                            lstAssetData = lstAssetData.OrderBy(d => d.BrandNameAr).ToList();
                    }
                }
                else if (sortObj.SupplierName != "" && sortObj.SupplierName != null)
                {
                    if (sortObj.BarCodeValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                        }
                    }
                    if (sortObj.SerialValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.GovernorateId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.CityId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OriginId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SubOrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstAssetData = lstAssetData.OrderByDescending(d => d.SupplierName).ToList();
                        else
                            lstAssetData = lstAssetData.OrderBy(d => d.SupplierName).ToList();
                    }
                }
                else if (sortObj.SupplierNameAr != "" && sortObj.SupplierNameAr != null)
                {
                    if (sortObj.BarCodeValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                        }
                    }
                    if (sortObj.SerialValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.GovernorateId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.CityId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.OriginId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.SubOrganizationId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                        }
                        else
                        {
                            lstAssetData = lstAssetData.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstAssetData = lstAssetData.OrderByDescending(d => d.SupplierNameAr).ToList();
                        else
                            lstAssetData = lstAssetData.OrderBy(d => d.SupplierNameAr).ToList();
                    }
                }

                return lstAssetData;
            }
        }
        public IndexAssetDetailVM SortAssets(Sort sortObj, int statusId, string userId)
        {
            IndexAssetDetailVM mainClass = new IndexAssetDetailVM();
            List<IndexAssetDetailVM.GetData> list = new List<IndexAssetDetailVM.GetData>();

            ApplicationRole roleObj = new ApplicationRole();
            Employee empObj = new Employee();
            List<string> userRoleNames = new List<string>();
            ApplicationUser userObj = new ApplicationUser();
            if (userId != null)
            {
                var obj = _context.ApplicationUser.Where(a => a.Id == userId).ToList();
                userObj = obj[0];

                var roles = (from userRole in _context.UserRoles
                             join role in _context.ApplicationRole on userRole.RoleId equals role.Id
                             where userRole.UserId == userObj.Id
                             select role);
                foreach (var role in roles)
                {
                    userRoleNames.Add(role.Name);
                }
                var lstEmployees = _context.Employees.Where(a => a.Email == userObj.Email).ToList();
                if (lstEmployees.Count > 0)
                {
                    empObj = lstEmployees[0];
                }
            }

            if (userRoleNames.Contains("AssetOwner"))
            {
                var lstAllAssets = _context.AssetOwners.Include(a => a.AssetDetail).Include(a => a.Employee).Include(a => a.AssetDetail.MasterAsset).Include(a => a.AssetDetail.Hospital)
                                                        .Include(a => a.AssetDetail.Supplier).Include(a => a.AssetDetail.MasterAsset.brand)
                                                        .Include(a => a.AssetDetail.Hospital.Governorate).Include(a => a.AssetDetail.Hospital.City).Include(a => a.AssetDetail.Hospital.Organization).Include(a => a.AssetDetail.Hospital.SubOrganization)
                                                        .OrderBy(a => a.AssetDetail.Barcode).ToList();


                if (lstAllAssets.Count > 0)
                {
                    foreach (var asset in lstAllAssets)
                    {
                        IndexAssetDetailVM.GetData detail = new IndexAssetDetailVM.GetData();

                        var lstStatus = _context.AssetStatusTransactions.Where(a => a.AssetDetailId == asset.AssetDetailId).OrderByDescending(a => a.StatusDate).ToList();
                        if (lstStatus.Count > 0)
                        {
                            detail.AssetStatusId = lstStatus.FirstOrDefault().AssetStatusId;
                        }
                        detail.Id = (int)asset.AssetDetailId;
                        detail.Code = asset.AssetDetail.Code;
                        detail.CreatedBy = asset.AssetDetail.CreatedBy;
                        detail.UserId = userObj.Id;
                        detail.EmployeeId = asset.EmployeeId;
                        detail.Price = asset.AssetDetail.Price;
                        detail.BarCode = asset.AssetDetail.Barcode;
                        detail.MasterImg = asset.AssetDetail.MasterAsset.AssetImg;
                        detail.Serial = asset.AssetDetail.SerialNumber;
                        detail.BrandName = asset.AssetDetail.MasterAsset.brand != null ? asset.AssetDetail.MasterAsset.brand.Name : "";
                        detail.BrandNameAr = asset.AssetDetail.MasterAsset.brand != null ? asset.AssetDetail.MasterAsset.brand.NameAr : "";
                        detail.Model = asset.AssetDetail.MasterAsset.ModelNumber;
                        detail.SerialNumber = asset.AssetDetail.SerialNumber;
                        detail.MasterAssetId = asset.AssetDetail.MasterAssetId;
                        detail.PurchaseDate = asset.AssetDetail.PurchaseDate;
                        detail.HospitalId = asset.AssetDetail.Hospital.Id;
                        detail.HospitalName = asset.AssetDetail.Hospital.Name;
                        detail.HospitalNameAr = asset.AssetDetail.Hospital.NameAr;
                        detail.AssetName = asset.AssetDetail.MasterAsset.Name;
                        detail.AssetNameAr = asset.AssetDetail.MasterAsset.NameAr;
                        detail.GovernorateId = asset.AssetDetail.Hospital.GovernorateId;
                        detail.GovernorateName = asset.AssetDetail.Hospital.Governorate.Name;
                        detail.GovernorateNameAr = asset.AssetDetail.Hospital.Governorate.NameAr;
                        detail.CityId = asset.AssetDetail.Hospital.CityId;
                        detail.CityName = asset.AssetDetail.Hospital.City.Name;
                        detail.CityNameAr = asset.AssetDetail.Hospital.City.NameAr;
                        detail.OrganizationId = asset.AssetDetail.Hospital.OrganizationId;
                        detail.OrgName = asset.AssetDetail.Hospital.Organization.Name;
                        detail.OrgNameAr = asset.AssetDetail.Hospital.Organization.NameAr;
                        detail.SubOrganizationId = asset.AssetDetail.Hospital.SubOrganizationId;
                        detail.SubOrgName = asset.AssetDetail.Hospital.SubOrganization.Name;
                        detail.SubOrgNameAr = asset.AssetDetail.Hospital.SubOrganization.NameAr;
                        detail.SupplierName = asset.AssetDetail.Supplier != null ? asset.AssetDetail.Supplier.Name : "";
                        detail.SupplierNameAr = asset.AssetDetail.Supplier != null ? asset.AssetDetail.Supplier.NameAr : "";
                        detail.QrFilePath = asset.AssetDetail.QrFilePath;

                        list.Add(detail);
                    }
                }
            }
            else
            {
                var lstAllAssets = _context.AssetDetails.Include(a => a.MasterAsset).Include(a => a.Hospital)
                                                   .Include(a => a.Supplier).Include(a => a.MasterAsset.brand)
                                                   .Include(a => a.Hospital.Governorate).Include(a => a.Hospital.City).Include(a => a.Hospital.Organization).Include(a => a.Hospital.SubOrganization)
                                                   .OrderBy(a => a.Barcode).ToList();
                if (lstAllAssets.Count > 0)
                {
                    foreach (var asset in lstAllAssets)
                    {
                        IndexAssetDetailVM.GetData detail = new IndexAssetDetailVM.GetData();

                        var lstStatus = _context.AssetStatusTransactions.Where(a => a.AssetDetailId == asset.Id).OrderByDescending(a => a.StatusDate).ToList();
                        if (lstStatus.Count > 0)
                        {
                            detail.AssetStatusId = lstStatus.FirstOrDefault().AssetStatusId;
                        }
                        detail.Id = (int)asset.Id;
                        detail.Code = asset.Code;
                        detail.CreatedBy = asset.CreatedBy;
                        detail.UserId = userObj.Id;
                        // detail.EmployeeId = asset.EmployeeId;
                        detail.EmployeeId = _context.AssetOwners.Where(a => a.AssetDetailId == detail.Id).ToList().FirstOrDefault().EmployeeId;
                        detail.Price = asset.Price;
                        detail.BarCode = asset.Barcode;
                        detail.MasterImg = asset.MasterAsset.AssetImg;
                        detail.Serial = asset.SerialNumber;
                        detail.BrandName = asset.MasterAsset.brand != null ? asset.MasterAsset.brand.Name : "";
                        detail.BrandNameAr = asset.MasterAsset.brand != null ? asset.MasterAsset.brand.NameAr : "";
                        detail.Model = asset.MasterAsset.ModelNumber;
                        detail.SerialNumber = asset.SerialNumber;
                        detail.MasterAssetId = asset.MasterAssetId;
                        detail.PurchaseDate = asset.PurchaseDate;
                        detail.HospitalId = asset.Hospital.Id;
                        detail.HospitalName = asset.Hospital.Name;
                        detail.HospitalNameAr = asset.Hospital.NameAr;
                        detail.AssetName = asset.MasterAsset.Name;
                        detail.AssetNameAr = asset.MasterAsset.NameAr;
                        detail.GovernorateId = asset.Hospital.GovernorateId;
                        detail.GovernorateName = asset.Hospital.Governorate.Name;
                        detail.GovernorateNameAr = asset.Hospital.Governorate.NameAr;
                        detail.CityId = asset.Hospital.CityId;
                        detail.CityName = asset.Hospital.City.Name;
                        detail.CityNameAr = asset.Hospital.City.NameAr;
                        detail.OrganizationId = asset.Hospital.OrganizationId;
                        detail.OrgName = asset.Hospital.Organization.Name;
                        detail.OrgNameAr = asset.Hospital.Organization.NameAr;
                        detail.SubOrganizationId = asset.Hospital.SubOrganizationId;
                        detail.SubOrgName = asset.Hospital.SubOrganization.Name;
                        detail.SubOrgNameAr = asset.Hospital.SubOrganization.NameAr;
                        detail.SupplierName = asset.Supplier != null ? asset.Supplier.Name : "";
                        detail.SupplierNameAr = asset.Supplier != null ? asset.Supplier.NameAr : "";
                        detail.QrFilePath = asset.QrFilePath;

                        list.Add(detail);
                    }
                }
            }
            if (userRoleNames.Contains("AssetOwner"))
            {
                list = list.Where(a => a.EmployeeId == empObj.Id && a.HospitalId == userObj.HospitalId).ToList();
            }
            if (userRoleNames.Contains("EngDepManager"))
            {
                list = list.Where(a => a.HospitalId == userObj.HospitalId).ToList();
            }
            if (userRoleNames.Contains("TLHospitalManager"))
            {
                list = list.Where(a => a.HospitalId == userObj.HospitalId).ToList();
            }
            if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
            {
                list = list.ToList();
            }
            if (userObj.GovernorateId > 0 && userObj.CityId == 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
            {
                list = list.Where(a => a.GovernorateId == userObj.GovernorateId).ToList();
            }
            if (userObj.GovernorateId > 0 && userObj.CityId > 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
            {
                list = list.Where(a => a.GovernorateId == userObj.GovernorateId && a.CityId == userObj.CityId).ToList();
            }
            if (userObj.GovernorateId > 0 && userObj.CityId > 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId > 0)
            {
                list = list.Where(a => a.GovernorateId == userObj.GovernorateId && a.CityId == userObj.CityId && a.HospitalId == userObj.HospitalId).ToList();
            }
            if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId > 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
            {
                list = list.Where(a => a.OrganizationId == userObj.OrganizationId).ToList();
            }
            if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId > 0 && userObj.SubOrganizationId > 0 && userObj.HospitalId == 0)
            {
                list = list.Where(a => a.OrganizationId == userObj.OrganizationId && a.SubOrganizationId == userObj.SubOrganizationId).ToList();
            }
            if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId > 0 && userObj.SubOrganizationId > 0 && userObj.HospitalId > 0)
            {
                list = list.Where(a => a.OrganizationId == userObj.OrganizationId && a.SubOrganizationId == userObj.SubOrganizationId && a.HospitalId == userObj.HospitalId).ToList();
            }

            if (statusId != 0)
            {
                list = list.Where(a => a.AssetStatusId == statusId).ToList();
            }
            else
            {
                list = list.ToList();
            }



            if (sortObj.AssetName != "")
            {
                if (sortObj.BarCodeValue != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.SerialValue != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                if (sortObj.MasterAssetId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.GovernorateId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.CityId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.OrganizationId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.OriginId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.BrandId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.SupplierId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.SubOrganizationId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.Model != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                    }
                }
                else
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.AssetName).ToList();
                    else
                        list = list.OrderBy(d => d.AssetName).ToList();
                }
            }
            else if (sortObj.AssetNameAr != "")
            {
                if (sortObj.BarCodeValue != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.SerialValue != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                if (sortObj.MasterAssetId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.GovernorateId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.CityId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.OrganizationId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.OriginId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.BrandId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.SupplierId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.SubOrganizationId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.Model != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                    }
                }
                else
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.AssetNameAr).ToList();
                    else
                        list = list.OrderBy(d => d.AssetNameAr).ToList();
                }
            }
            else if (sortObj.GovernorateName != "")
            {
                if (sortObj.BarCodeValue != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.SerialValue != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                if (sortObj.MasterAssetId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.GovernorateId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.CityId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.OrganizationId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.OriginId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.BrandId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.SupplierId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.SubOrganizationId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.Model != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                    }
                }
                else
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.GovernorateName).ToList();
                    else
                        list = list.OrderBy(d => d.GovernorateName).ToList();
                }
            }
            else if (sortObj.GovernorateNameAr != "")
            {
                if (sortObj.BarCodeValue != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.SerialValue != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                if (sortObj.MasterAssetId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.GovernorateId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.CityId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.OrganizationId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.OriginId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.BrandId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.SupplierId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.SubOrganizationId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.Model != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                    }
                }
                else
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.GovernorateNameAr).ToList();
                    else
                        list = list.OrderBy(d => d.GovernorateNameAr).ToList();
                }
            }
            else if (sortObj.HospitalName != "")
            {
                if (sortObj.BarCodeValue != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.SerialValue != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                if (sortObj.MasterAssetId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.GovernorateId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.CityId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.OrganizationId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.OriginId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.BrandId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.SupplierId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.SubOrganizationId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.Model != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                    }
                }
                else
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.HospitalName).ToList();
                    else
                        list = list.OrderBy(d => d.HospitalName).ToList();
                }
            }
            else if (sortObj.HospitalNameAr != "")
            {
                if (sortObj.BarCodeValue != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.SerialValue != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                if (sortObj.MasterAssetId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.GovernorateId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.CityId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.OrganizationId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.OriginId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.BrandId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.SupplierId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.SubOrganizationId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.Model != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                    }
                }
                else
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.HospitalNameAr).ToList();
                    else
                        list = list.OrderBy(d => d.HospitalNameAr).ToList();
                }
            }
            else if (sortObj.GovernorateName != "")
            {
                if (sortObj.BarCodeValue != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.SerialValue != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                if (sortObj.MasterAssetId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.GovernorateId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.CityId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.OrganizationId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.OriginId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.BrandId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.SupplierId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.SubOrganizationId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.Model != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                    }
                }
                else
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.GovernorateName).ToList();
                    else
                        list = list.OrderBy(d => d.GovernorateName).ToList();
                }
            }
            else if (sortObj.GovernorateNameAr != "")
            {
                if (sortObj.BarCodeValue != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.SerialValue != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                if (sortObj.MasterAssetId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.GovernorateId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.CityId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.OrganizationId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.OriginId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.BrandId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.SupplierId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.SubOrganizationId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.Model != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                    }
                }
                else
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.GovernorateNameAr).ToList();
                    else
                        list = list.OrderBy(d => d.GovernorateNameAr).ToList();
                }
            }
            else if (sortObj.OrgName != "")
            {
                if (sortObj.BarCodeValue != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.SerialValue != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                if (sortObj.MasterAssetId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.GovernorateId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.CityId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.OrganizationId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.OriginId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.BrandId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.SupplierId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.SubOrganizationId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.Model != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                    }
                }
                else
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.OrgName).ToList();
                    else
                        list = list.OrderBy(d => d.OrgName).ToList();
                }
            }
            else if (sortObj.OrgNameAr != "")
            {
                if (sortObj.BarCodeValue != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.SerialValue != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                if (sortObj.MasterAssetId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.GovernorateId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.CityId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.OrganizationId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.OriginId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.BrandId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.SupplierId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.SubOrganizationId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.Model != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                    }
                }
                else
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.OrgNameAr).ToList();
                    else
                        list = list.OrderBy(d => d.OrgNameAr).ToList();
                }
            }
            else if (sortObj.SubOrgName != "")
            {
                if (sortObj.BarCodeValue != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.SerialValue != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                if (sortObj.MasterAssetId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.GovernorateId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.CityId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.OrganizationId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.OriginId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.BrandId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.SupplierId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.SubOrganizationId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.Model != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                    }
                }
                else
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.SubOrgName).ToList();
                    else
                        list = list.OrderBy(d => d.SubOrgName).ToList();
                }
            }
            else if (sortObj.SubOrgNameAr != "")
            {

                if (sortObj.BarCodeValue != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.SerialValue != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                if (sortObj.MasterAssetId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.GovernorateId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.CityId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.OrganizationId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.OriginId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.BrandId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.SupplierId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.SubOrganizationId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.Model != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                    }
                }
                else
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.SubOrgNameAr).ToList();
                    else
                        list = list.OrderBy(d => d.SubOrgNameAr).ToList();
                }
            }
            else if (sortObj.BarCode != "")
            {
                if (sortObj.BarCodeValue != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.SerialValue != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                if (sortObj.MasterAssetId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.GovernorateId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.CityId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.OrganizationId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.OriginId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.BrandId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.SupplierId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.SubOrganizationId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.Model != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                    }
                }
                else
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.BarCode).ToList();
                    else
                        list = list.OrderBy(d => d.BarCode).ToList();
                }
            }
            else if (sortObj.Serial != "")
            {
                if (sortObj.BarCodeValue != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.SerialValue != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                if (sortObj.MasterAssetId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.GovernorateId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.CityId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.OrganizationId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.OriginId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.BrandId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.SupplierId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.SubOrganizationId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.Model != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                    }
                }

                else
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.Serial).ToList();
                    else
                        list = list.OrderBy(d => d.Serial).ToList();
                }
            }
            else if (sortObj.Model != "" && sortObj.Model != null)
            {
                if (sortObj.BarCodeValue != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.SerialValue != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                if (sortObj.MasterAssetId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.GovernorateId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.CityId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.OrganizationId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.OriginId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.BrandId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.SupplierId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.SubOrganizationId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.Model != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                    }
                }
                else
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.Model).ToList();
                    else
                        list = list.OrderBy(d => d.Model).ToList();
                }
            }
            else if (sortObj.BrandName != "" && sortObj.BrandName != null)
            {
                if (sortObj.BarCodeValue != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.SerialValue != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                if (sortObj.MasterAssetId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.GovernorateId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.CityId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.OrganizationId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.OriginId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.BrandId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.SupplierId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.SubOrganizationId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.Model != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                    }
                }
                else
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.BrandName).ToList();
                    else
                        list = list.OrderBy(d => d.BrandName).ToList();
                }
            }
            else if (sortObj.BrandNameAr != "" && sortObj.BrandNameAr != null)
            {
                if (sortObj.BarCodeValue != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.SerialValue != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                if (sortObj.MasterAssetId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.GovernorateId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.CityId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.OrganizationId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.OriginId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.BrandId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.SupplierId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.SubOrganizationId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.Model != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                    }
                }
                else
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.BrandNameAr).ToList();
                    else
                        list = list.OrderBy(d => d.BrandNameAr).ToList();
                }
            }
            else if (sortObj.SupplierName != "" && sortObj.SupplierName != null)
            {
                if (sortObj.BarCodeValue != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.SerialValue != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                if (sortObj.MasterAssetId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.GovernorateId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.CityId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.OrganizationId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.OriginId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.BrandId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.SupplierId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.SubOrganizationId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.Model != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                    }
                }
                else
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.SupplierName).ToList();
                    else
                        list = list.OrderBy(d => d.SupplierName).ToList();
                }
            }
            else if (sortObj.SupplierNameAr != "" && sortObj.SupplierNameAr != null)
            {
                if (sortObj.BarCodeValue != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.BarCodeValue)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.SerialValue != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialValue)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                if (sortObj.MasterAssetId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.GovernorateId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.GovernorateId == sortObj.GovernorateId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.CityId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.CityId == sortObj.CityId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.CityId == sortObj.CityId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.OrganizationId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.OrganizationId == sortObj.OrganizationId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.OriginId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.OriginId == sortObj.OriginId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.OriginId == sortObj.OriginId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.BrandId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.SupplierId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.SubOrganizationId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SubOrganizationId == sortObj.SubOrganizationId).OrderBy(d => d.Serial).ToList();
                    }
                }
                if (sortObj.Model != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.Model.Contains(sortObj.Model)).OrderByDescending(d => d.Serial).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.Model.Contains(sortObj.Model)).OrderBy(d => d.Serial).ToList();
                    }
                }
                else
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.SupplierNameAr).ToList();
                    else
                        list = list.OrderBy(d => d.SupplierNameAr).ToList();
                }
            }



            // var requestsPerPage = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            mainClass.Results = list;
            mainClass.Count = list.Count();
            return mainClass;
        }

        public List<HospitalAssetAge> GetAssetsByAgeGroup(int hospitalId)
        {

            List<HospitalAssetAge> lstHospitalAssets = new List<HospitalAssetAge>();
            if (hospitalId != 0)
            {
                lstHospitalAssets = _context.AssetDetails
                            .Where(a => a.HospitalId == hospitalId && a.InstallationDate != null).ToList()
                            .GroupBy(p => (DateTime.Today.Year - p.InstallationDate.Value.Year) / 5)
                           .OrderBy(p => p.Key)
                            .Select(gr => new HospitalAssetAge
                            {
                                AgeGroup = String.Format("{0}-{1}", gr.Key * 5, (gr.Key + 1) * 5),
                                Count = gr.Count()
                            }).ToList();
            }
            else
            {
                lstHospitalAssets = _context.AssetDetails.Where(a => a.InstallationDate != null).ToList()
                    .GroupBy(p => (DateTime.Today.Year - p.InstallationDate.Value.Year) / 5)
                    .OrderBy(p => p.Key)
                            .Select(gr => new HospitalAssetAge
                            {
                                AgeGroup = String.Format("{0}-{1}", gr.Key * 5, (gr.Key + 1) * 5),
                                Count = gr.Count()
                            }).ToList();
            }
            return lstHospitalAssets;
        }

        public List<HospitalAssetAge> GetGeneralAssetsByAgeGroup(FilterHospitalAssetAge searchObj)
        {
            List<HospitalAssetAge> lstHospitalAssets = new List<HospitalAssetAge>();
            var lstHostAssets = _context.AssetDetails.Include(a => a.MasterAsset).Include(a => a.Supplier).Include(a => a.MasterAsset.brand)
                        .Include(a => a.Hospital).Include(a => a.Hospital.Governorate).Include(a => a.Hospital.City)
                        .Include(a => a.Hospital.Organization).Include(a => a.Hospital.SubOrganization).ToList()
                        .Select(item => new IndexAssetDetailVM.GetData
                        {

                            Id = item.Id,
                            Code = item.Code,
                            Model = item.MasterAsset.ModelNumber,
                            BrandName = item.MasterAsset.brand.Name,
                            BrandNameAr = item.MasterAsset.brand.NameAr,
                            SerialNumber = item.SerialNumber,
                            HospitalId = item.HospitalId,
                            SupplierId = item.SupplierId,
                            MasterAssetId = item.MasterAsset.Id,
                            AssetName = item.MasterAsset.Name,
                            BrandId = item.MasterAsset.BrandId,
                            OriginId = item.MasterAsset.OriginId,
                            GovernorateId = item.Hospital.GovernorateId,
                            CityId = item.Hospital.CityId,
                            OrganizationId = item.Hospital.OrganizationId,
                            SubOrganizationId = item.Hospital.SubOrganizationId,
                            InstallationDate = item.InstallationDate
                        }).ToList();


            if (searchObj.GovernorateId != 0)
            {
                lstHostAssets = lstHostAssets.Where(a => a.GovernorateId == searchObj.GovernorateId).ToList();
            }
            else
                lstHostAssets = lstHostAssets.ToList();


            if (searchObj.CityId != 0)
            {
                lstHostAssets = lstHostAssets.Where(a => a.CityId == searchObj.CityId).ToList();
            }
            else
                lstHostAssets = lstHostAssets.ToList();

            if (searchObj.OrganizationId != 0)
            {
                lstHostAssets = lstHostAssets.Where(a => a.OrganizationId == searchObj.OrganizationId).ToList();
            }
            else
                lstHostAssets = lstHostAssets.ToList();

            if (searchObj.SubOrganizationId != 0)
            {
                lstHostAssets = lstHostAssets.Where(a => a.SubOrganizationId == searchObj.SubOrganizationId).ToList();
            }
            else
                lstHostAssets = lstHostAssets.ToList();


            if (searchObj.SupplierId != 0)
            {
                lstHostAssets = lstHostAssets.Where(a => a.SupplierId == searchObj.SupplierId).ToList();
            }
            else
                lstHostAssets = lstHostAssets.ToList();


            if (searchObj.OriginId != 0)
            {
                lstHostAssets = lstHostAssets.Where(a => a.OriginId == searchObj.OriginId).ToList();
            }
            else
                lstHostAssets = lstHostAssets.ToList();



            if (searchObj.BrandId != 0)
            {
                lstHostAssets = lstHostAssets.Where(a => a.BrandId == searchObj.BrandId).ToList();
            }
            else
                lstHostAssets = lstHostAssets.ToList();


            if (searchObj.HospitalId != 0)
            {
                lstHostAssets = lstHostAssets.Where(a => a.HospitalId == searchObj.HospitalId).ToList();
            }
            else
                lstHostAssets = lstHostAssets.ToList();

            if (searchObj.Model != "")
            {
                lstHostAssets = lstHostAssets.Where(b => b.Model == searchObj.Model).ToList();
            }


            lstHospitalAssets = lstHostAssets.Where(a => a.InstallationDate != null).GroupBy(p => (DateTime.Today.Year - p.InstallationDate.Value.Year) / 5)
                                          .OrderBy(p => p.Key).Select(gr => new HospitalAssetAge
                                          {
                                              AgeGroup = String.Format("{0}-{1}", gr.Key * 5, (gr.Key + 1) * 5),
                                              Count = gr.Count()
                                          }).ToList();


            return lstHospitalAssets;
        }

        public IEnumerable<IndexAssetDetailVM.GetData> AutoCompleteAssetBarCode(string barcode, int hospitalId)
        {
            //var lst = _context.AssetDetails.Include(a => a.MasterAsset).Include(a => a.MasterAsset.brand).Include(a => a.Hospital)
            //    .Where(a => a.Barcode.Contains(barcode) && a.HospitalId == hospitalId).OrderBy(a => a.Barcode).ToList().Select(a => new IndexAssetDetailVM.GetData
            //                   {
            //        Id = a.Id,
            //        Code = a.Code,
            //        Price = a.Price,
            //        MasterAssetName = a.MasterAsset != null ? a.MasterAsset.Name : "",
            //        MasterAssetNameAr = a.MasterAsset != null ? a.MasterAsset.NameAr : "",
            //        BrandName = a.MasterAsset.brand != null ? a.MasterAsset.brand.Name : "",
            //        BrandNameAr = a.MasterAsset.brand != null ? a.MasterAsset.brand.NameAr : "",
            //        Model = a.MasterAsset != null ? a.MasterAsset.ModelNumber : "",
            //        AssetBarCode = a.Barcode,
            //        BarCode = a.Barcode,
            //        Serial = a.SerialNumber,
            //        SerialNumber = a.SerialNumber,
            //        MasterAssetId = a.MasterAssetId,
            //        PurchaseDate = a.PurchaseDate,
            //        HospitalId = a.HospitalId,
            //        HospitalName = a.Hospital.Name,
            //        HospitalNameAr = a.Hospital.NameAr,
            //        AssetName = a.MasterAsset.Name,
            //        AssetNameAr = a.MasterAsset.NameAr
            //    }).ToList();
            //return lst;

            List<IndexAssetDetailVM.GetData> list = new List<IndexAssetDetailVM.GetData>();
            var lst = _context.AssetDetails.Include(a => a.MasterAsset).Include(a => a.MasterAsset.brand)
                .Include(a => a.Hospital).Where(a => a.Barcode.Contains(barcode)).OrderBy(a => a.Barcode).ToList();
            if (hospitalId == 0)
            {
                lst = lst.ToList();
            }
            else
            {
                lst = lst.Where(a => a.HospitalId == hospitalId).ToList();
            }
            if (lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    IndexAssetDetailVM.GetData getDataObj = new IndexAssetDetailVM.GetData();
                    getDataObj.Id = item.Id;
                    getDataObj.Code = item.Code;
                    getDataObj.BarCode = item.Barcode;
                    getDataObj.Price = item.Price;
                    getDataObj.MasterAssetName = item.MasterAsset != null ? item.MasterAsset.Name : "";
                    getDataObj.MasterAssetNameAr = item.MasterAsset != null ? item.MasterAsset.NameAr : "";
                    getDataObj.BrandName = item.MasterAsset.brand != null ? item.MasterAsset.brand.Name : "";
                    getDataObj.BrandNameAr = item.MasterAsset.brand != null ? item.MasterAsset.brand.NameAr : "";
                    getDataObj.Model = item.MasterAsset != null ? item.MasterAsset.ModelNumber : "";
                    getDataObj.AssetBarCode = item.Barcode;
                    getDataObj.BarCode = item.Barcode;
                    getDataObj.Serial = item.SerialNumber;
                    getDataObj.SerialNumber = item.SerialNumber;
                    getDataObj.MasterAssetId = item.MasterAssetId;
                    getDataObj.PurchaseDate = item.PurchaseDate;
                    getDataObj.HospitalId = item.HospitalId;
                    getDataObj.HospitalName = item.Hospital.Name;
                    getDataObj.HospitalNameAr = item.Hospital.NameAr;
                    getDataObj.AssetName = item.MasterAsset.Name;
                    getDataObj.AssetNameAr = item.MasterAsset.NameAr;
                    var lstAssetTransactions = _context.AssetStatusTransactions.Include(a => a.AssetStatus).Where(a => a.AssetDetailId == item.Id).ToList().OrderByDescending(a => a.StatusDate).ToList();
                    if (lstAssetTransactions.Count > 0)
                    {
                        getDataObj.AssetStatusId = lstAssetTransactions[0].AssetStatus.Id;
                        getDataObj.AssetStatus = lstAssetTransactions[0].AssetStatus.Name;
                        getDataObj.AssetStatusAr = lstAssetTransactions[0].AssetStatus.NameAr;
                    }
                    list.Add(getDataObj);
                }
            }
            return list;
        }

        public IEnumerable<IndexAssetDetailVM.GetData> GetAllAssetsByStatusId(int statusId, string userId)
        {
            ApplicationUser UserObj = new ApplicationUser();
            List<string> userRoleNames = new List<string>();


            var obj = _context.ApplicationUser.Where(a => a.Id == userId).ToList();
            if (obj.Count > 0)
            {
                UserObj = obj[0];
                var roleNames = (from userRole in _context.UserRoles
                                 join role in _context.Roles on userRole.RoleId equals role.Id
                                 where userRole.UserId == userId
                                 select role);
                foreach (var name in roleNames)
                {
                    userRoleNames.Add(name.Name);
                }
            }
            var lstAssets = _context.AssetDetails
                             .Include(t => t.Hospital)
                             .Include(t => t.Hospital.Governorate)
                             .Include(t => t.Hospital.City)
                             .Include(t => t.Hospital.Organization)
                             .Include(t => t.Hospital.SubOrganization)
                             .Include(t => t.Supplier)
                             .Include(t => t.MasterAsset)
                             .Include(t => t.MasterAsset.brand).OrderBy(a => a.Barcode).ToList();



            List<IndexAssetDetailVM.GetData> list = new List<IndexAssetDetailVM.GetData>();
            if (lstAssets.Count > 0)
            {
                foreach (var asset in lstAssets)
                {
                    IndexAssetDetailVM.GetData detail = new IndexAssetDetailVM.GetData();
                    detail.Id = asset.Id;
                    var lstStatus = _context.AssetStatusTransactions.Where(a => a.AssetDetailId == asset.Id).OrderByDescending(a => a.StatusDate).ToList();
                    if (lstStatus.Count > 0)
                    {
                        detail.AssetStatusId = lstStatus.FirstOrDefault().AssetStatusId;
                    }
                    detail.Id = asset.Id;
                    detail.Code = asset.Code;
                    detail.CreatedBy = asset.CreatedBy;
                    detail.UserId = UserObj.Id;
                    detail.Price = asset.Price;
                    detail.BarCode = asset.Barcode;
                    detail.MasterImg = asset.MasterAsset.AssetImg;
                    detail.Serial = asset.SerialNumber;
                    detail.BrandName = asset.MasterAsset.brand != null ? asset.MasterAsset.brand.Name : "";
                    detail.BrandNameAr = asset.MasterAsset.brand != null ? asset.MasterAsset.brand.NameAr : "";
                    detail.Model = asset.MasterAsset.ModelNumber;
                    detail.SerialNumber = asset.SerialNumber;
                    detail.MasterAssetId = asset.MasterAssetId;
                    detail.PurchaseDate = asset.PurchaseDate;
                    detail.HospitalId = asset.Hospital.Id;
                    detail.HospitalName = asset.Hospital.Name;
                    detail.HospitalNameAr = asset.Hospital.NameAr;
                    detail.AssetName = asset.MasterAsset.Name;
                    detail.AssetNameAr = asset.MasterAsset.NameAr;
                    detail.GovernorateId = asset.Hospital.GovernorateId;
                    detail.GovernorateName = asset.Hospital.Governorate.Name;
                    detail.GovernorateNameAr = asset.Hospital.Governorate.NameAr;
                    detail.CityId = asset.Hospital.CityId;
                    detail.CityName = asset.Hospital.City.Name;
                    detail.CityNameAr = asset.Hospital.City.NameAr;
                    detail.OrganizationId = asset.Hospital.OrganizationId;
                    detail.OrgName = asset.Hospital.Organization.Name;
                    detail.OrgNameAr = asset.Hospital.Organization.NameAr;
                    detail.SubOrganizationId = asset.Hospital.SubOrganizationId;
                    detail.SubOrgName = asset.Hospital.SubOrganization.Name;
                    detail.SubOrgNameAr = asset.Hospital.SubOrganization.NameAr;
                    detail.SupplierName = asset.Supplier != null ? asset.Supplier.Name : "";
                    detail.SupplierNameAr = asset.Supplier != null ? asset.Supplier.NameAr : "";
                    detail.QrFilePath = asset.QrFilePath;



                    list.Add(detail);
                }
            }
            if (UserObj.GovernorateId == 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0 && UserObj.OrganizationId == 0 && UserObj.SubOrganizationId == 0)
            {
                list = list.ToList();
            }
            else if (UserObj.GovernorateId > 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
            {
                list = list.Where(t => t.GovernorateId == UserObj.GovernorateId).ToList();
            }
            else if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId == 0)
            {
                list = list.Where(t => t.CityId == UserObj.CityId && t.AssetStatusId == statusId).ToList();
            }
            else if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId == 0 && UserObj.HospitalId == 0)
            {
                list = list.Where(t => t.OrganizationId == UserObj.OrganizationId).ToList();
            }
            else if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId == 0)
            {
                list = list.Where(t => t.SubOrganizationId == UserObj.SubOrganizationId).ToList();
            }

            else if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId > 0)
            {
                list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
            }
            else if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId > 0)
            {
                list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
            }


            if (statusId != 0)
            {
                list = list.Where(a => a.AssetStatusId == statusId).ToList();
            }
            else
            {
                list = list.ToList();
            }

            return list;
        }

        public IndexAssetDetailVM GetAllAssetsByStatusId(int pageNumber, int pageSize, int statusId, string userId)
        {
            IndexAssetDetailVM mainClass = new IndexAssetDetailVM();
            List<IndexAssetDetailVM.GetData> list = new List<IndexAssetDetailVM.GetData>();

            if (userId != null)
            {
                Employee empObj = new Employee();
                List<string> userRoleNames = new List<string>();
                var obj = _context.ApplicationUser.Where(a => a.Id == userId).ToList();
                var userObj = obj[0];


                var roles = (from userRole in _context.UserRoles
                             join role in _context.ApplicationRole on userRole.RoleId equals role.Id
                             where userRole.UserId == userObj.Id
                             select role);
                foreach (var role in roles)
                {
                    userRoleNames.Add(role.Name);
                }

                var lstEmployees = _context.Employees.Where(a => a.Email == userObj.Email).ToList();
                if (lstEmployees.Count > 0)
                {
                    empObj = lstEmployees[0];
                }

                if (userRoleNames.Contains("AssetOwner"))
                {
                    var lstAllAssets = _context.AssetOwners.Include(a => a.AssetDetail).Include(a => a.Employee).Include(a => a.AssetDetail.MasterAsset).Include(a => a.AssetDetail.Hospital)
                                                            .Include(a => a.AssetDetail.Supplier).Include(a => a.AssetDetail.MasterAsset.brand)
                                                            .Include(a => a.AssetDetail.Hospital.Governorate).Include(a => a.AssetDetail.Hospital.City).Include(a => a.AssetDetail.Hospital.Organization).Include(a => a.AssetDetail.Hospital.SubOrganization)
                                                            .OrderBy(a => a.AssetDetail.Barcode).ToList();


                    if (lstAllAssets.Count > 0)
                    {
                        foreach (var asset in lstAllAssets)
                        {
                            IndexAssetDetailVM.GetData detail = new IndexAssetDetailVM.GetData();

                            var lstStatus = _context.AssetStatusTransactions.Where(a => a.AssetDetailId == asset.AssetDetailId).OrderByDescending(a => a.StatusDate).ToList();
                            if (lstStatus.Count > 0)
                            {
                                detail.AssetStatusId = lstStatus.FirstOrDefault().AssetStatusId;
                            }
                            detail.Id = (int)asset.AssetDetailId;
                            detail.Code = asset.AssetDetail.Code;
                            detail.UserId = userObj.Id;
                            detail.EmployeeId = asset.EmployeeId;
                            detail.Price = asset.AssetDetail.Price;
                            detail.CreatedBy = asset.AssetDetail.CreatedBy;
                            detail.BarCode = asset.AssetDetail.Barcode;
                            detail.MasterImg = asset.AssetDetail.MasterAsset.AssetImg;
                            detail.Serial = asset.AssetDetail.SerialNumber;
                            detail.BrandName = asset.AssetDetail.MasterAsset.brand != null ? asset.AssetDetail.MasterAsset.brand.Name : "";
                            detail.BrandNameAr = asset.AssetDetail.MasterAsset.brand != null ? asset.AssetDetail.MasterAsset.brand.NameAr : "";
                            detail.Model = asset.AssetDetail.MasterAsset.ModelNumber;
                            detail.SerialNumber = asset.AssetDetail.SerialNumber;
                            detail.MasterAssetId = asset.AssetDetail.MasterAssetId;
                            detail.PurchaseDate = asset.AssetDetail.PurchaseDate;
                            detail.HospitalId = asset.AssetDetail.Hospital.Id;
                            detail.HospitalName = asset.AssetDetail.Hospital.Name;
                            detail.HospitalNameAr = asset.AssetDetail.Hospital.NameAr;
                            detail.AssetName = asset.AssetDetail.MasterAsset.Name;
                            detail.AssetNameAr = asset.AssetDetail.MasterAsset.NameAr;
                            detail.GovernorateId = asset.AssetDetail.Hospital.GovernorateId;
                            detail.GovernorateName = asset.AssetDetail.Hospital.Governorate.Name;
                            detail.GovernorateNameAr = asset.AssetDetail.Hospital.Governorate.NameAr;
                            detail.CityId = asset.AssetDetail.Hospital.CityId;
                            detail.CityName = asset.AssetDetail.Hospital.City.Name;
                            detail.CityNameAr = asset.AssetDetail.Hospital.City.NameAr;
                            detail.OrganizationId = asset.AssetDetail.Hospital.OrganizationId;
                            detail.OrgName = asset.AssetDetail.Hospital.Organization.Name;
                            detail.OrgNameAr = asset.AssetDetail.Hospital.Organization.NameAr;
                            detail.SubOrganizationId = asset.AssetDetail.Hospital.SubOrganizationId;
                            detail.SubOrgName = asset.AssetDetail.Hospital.SubOrganization.Name;
                            detail.SubOrgNameAr = asset.AssetDetail.Hospital.SubOrganization.NameAr;
                            detail.SupplierName = asset.AssetDetail.Supplier != null ? asset.AssetDetail.Supplier.Name : "";
                            detail.SupplierNameAr = asset.AssetDetail.Supplier != null ? asset.AssetDetail.Supplier.NameAr : "";
                            detail.QrFilePath = asset.AssetDetail.QrFilePath;

                            list.Add(detail);
                        }
                    }
                }
                else
                {


                    var lstAllAssets = _context.AssetDetails.Include(a => a.MasterAsset).Include(a => a.Hospital)
                                                       .Include(a => a.Supplier).Include(a => a.MasterAsset.brand)
                                                       .Include(a => a.Hospital.Governorate).Include(a => a.Hospital.City).Include(a => a.Hospital.Organization).Include(a => a.Hospital.SubOrganization)
                                                       .OrderBy(a => a.Barcode).ToList();


                    if (lstAllAssets.Count > 0)
                    {
                        foreach (var asset in lstAllAssets)
                        {
                            IndexAssetDetailVM.GetData detail = new IndexAssetDetailVM.GetData();

                            var lstStatus = _context.AssetStatusTransactions.Where(a => a.AssetDetailId == asset.Id).OrderByDescending(a => a.StatusDate).ToList();
                            if (lstStatus.Count > 0)
                            {
                                detail.AssetStatusId = lstStatus.FirstOrDefault().AssetStatusId;
                            }
                            detail.Id = (int)asset.Id;
                            detail.Code = asset.Code;
                            detail.UserId = userObj.Id;
                            var lstOwners = _context.AssetOwners.Where(a => a.AssetDetailId == detail.Id).ToList();
                            if (lstOwners.Count > 0)
                            {
                                var ownerObj = lstOwners[0];
                                detail.EmployeeId = ownerObj.EmployeeId;
                            }
                            detail.Price = asset.Price;
                            detail.BarCode = asset.Barcode;
                            detail.MasterImg = asset.MasterAsset.AssetImg;
                            detail.Serial = asset.SerialNumber;
                            detail.BrandName = asset.MasterAsset.brand != null ? asset.MasterAsset.brand.Name : "";
                            detail.BrandNameAr = asset.MasterAsset.brand != null ? asset.MasterAsset.brand.NameAr : "";
                            detail.Model = asset.MasterAsset.ModelNumber;
                            detail.SerialNumber = asset.SerialNumber;
                            detail.MasterAssetId = asset.MasterAssetId;
                            detail.PurchaseDate = asset.PurchaseDate;
                            detail.HospitalId = asset.Hospital.Id;
                            detail.HospitalName = asset.Hospital.Name;
                            detail.HospitalNameAr = asset.Hospital.NameAr;
                            detail.AssetName = asset.MasterAsset.Name;
                            detail.AssetNameAr = asset.MasterAsset.NameAr;
                            detail.GovernorateId = asset.Hospital.GovernorateId;
                            detail.GovernorateName = asset.Hospital.Governorate.Name;
                            detail.GovernorateNameAr = asset.Hospital.Governorate.NameAr;
                            detail.CityId = asset.Hospital.CityId;
                            detail.CityName = asset.Hospital.City.Name;
                            detail.CityNameAr = asset.Hospital.City.NameAr;
                            detail.OrganizationId = asset.Hospital.OrganizationId;
                            detail.OrgName = asset.Hospital.Organization.Name;
                            detail.OrgNameAr = asset.Hospital.Organization.NameAr;
                            detail.SubOrganizationId = asset.Hospital.SubOrganizationId;
                            detail.SubOrgName = asset.Hospital.SubOrganization.Name;
                            detail.SubOrgNameAr = asset.Hospital.SubOrganization.NameAr;
                            detail.SupplierName = asset.Supplier != null ? asset.Supplier.Name : "";
                            detail.SupplierNameAr = asset.Supplier != null ? asset.Supplier.NameAr : "";
                            detail.QrFilePath = asset.QrFilePath;

                            list.Add(detail);
                        }
                    }

                }


                if (userRoleNames.Contains("AssetOwner"))
                {
                    list = list.Where(a => a.EmployeeId == empObj.Id && a.HospitalId == userObj.HospitalId).ToList();
                }
                if (userRoleNames.Contains("EngDepManager"))
                {
                    list = list.Where(a => a.HospitalId == userObj.HospitalId).ToList();
                }
                if (userRoleNames.Contains("TLHospitalManager"))
                {
                    list = list.Where(a => a.HospitalId == userObj.HospitalId).ToList();
                }
                if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                {
                    list = list.ToList();
                }
                if (userObj.GovernorateId > 0 && userObj.CityId == 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                {
                    list = list.Where(a => a.GovernorateId == userObj.GovernorateId).ToList();
                }
                if (userObj.GovernorateId > 0 && userObj.CityId > 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                {
                    list = list.Where(a => a.GovernorateId == userObj.GovernorateId && a.CityId == userObj.CityId).ToList();
                }
                if (userObj.GovernorateId > 0 && userObj.CityId > 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId > 0)
                {

                    list = list.Where(a => a.GovernorateId == userObj.GovernorateId && a.CityId == userObj.CityId && a.HospitalId == userObj.HospitalId).ToList();

                }
                if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId > 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                {
                    list = list.Where(a => a.OrganizationId == userObj.OrganizationId).ToList();
                }
                if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId > 0 && userObj.SubOrganizationId > 0 && userObj.HospitalId == 0)
                {
                    list = list.Where(a => a.OrganizationId == userObj.OrganizationId && a.SubOrganizationId == userObj.SubOrganizationId).ToList();
                }

                if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId > 0 && userObj.SubOrganizationId > 0 && userObj.HospitalId > 0)
                {
                    list = list.Where(a => a.OrganizationId == userObj.OrganizationId && a.SubOrganizationId == userObj.SubOrganizationId && a.HospitalId == userObj.HospitalId).ToList();
                }

                if (statusId != 0)
                {
                    list = list.Where(a => a.AssetStatusId == statusId).ToList();
                }
                else
                {
                    list = list.ToList();
                }

                var requestsPerPage = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                mainClass.Results = requestsPerPage;
                mainClass.Count = list.Count();
                return mainClass;
            }
            return null;

        }

        public IEnumerable<IndexAssetDetailVM.GetData> AutoCompleteAssetSerial(string serial, int hospitalId)
        {
            List<IndexAssetDetailVM.GetData> list = new List<IndexAssetDetailVM.GetData>();
            var lst = _context.AssetDetails.Include(a => a.MasterAsset).Include(a => a.MasterAsset.brand)
                .Include(a => a.Hospital).Where(a => a.SerialNumber.Contains(serial)).OrderBy(a => a.SerialNumber).ToList();
            if (hospitalId == 0)
            {
                lst = lst.ToList();
            }
            else
            {
                lst = lst.Where(a => a.HospitalId == hospitalId).ToList();
            }
            if (lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    IndexAssetDetailVM.GetData getDataObj = new IndexAssetDetailVM.GetData();
                    getDataObj.Id = item.Id;
                    getDataObj.Code = item.Code;
                    getDataObj.BarCode = item.Barcode;
                    getDataObj.Price = item.Price;
                    getDataObj.MasterAssetName = item.MasterAsset != null ? item.MasterAsset.Name : "";
                    getDataObj.MasterAssetNameAr = item.MasterAsset != null ? item.MasterAsset.NameAr : "";
                    getDataObj.BrandName = item.MasterAsset.brand != null ? item.MasterAsset.brand.Name : "";
                    getDataObj.BrandNameAr = item.MasterAsset.brand != null ? item.MasterAsset.brand.NameAr : "";
                    getDataObj.Model = item.MasterAsset != null ? item.MasterAsset.ModelNumber : "";
                    getDataObj.AssetBarCode = item.Barcode;
                    getDataObj.BarCode = item.Barcode;
                    getDataObj.Serial = item.SerialNumber;
                    getDataObj.SerialNumber = item.SerialNumber;
                    getDataObj.MasterAssetId = item.MasterAssetId;
                    getDataObj.PurchaseDate = item.PurchaseDate;
                    getDataObj.HospitalId = item.HospitalId;
                    getDataObj.HospitalName = item.Hospital.Name;
                    getDataObj.HospitalNameAr = item.Hospital.NameAr;
                    getDataObj.AssetName = item.MasterAsset.Name;
                    getDataObj.AssetNameAr = item.MasterAsset.NameAr;



                    var lstAssetTransactions = _context.AssetStatusTransactions.Include(a => a.AssetStatus).Where(a => a.AssetDetailId == item.Id).ToList().OrderByDescending(a => a.StatusDate).ToList();

                    if (lstAssetTransactions.Count > 0)
                    {
                        getDataObj.AssetStatusId = lstAssetTransactions[0].AssetStatus.Id;
                        getDataObj.AssetStatus = lstAssetTransactions[0].AssetStatus.Name;
                        getDataObj.AssetStatusAr = lstAssetTransactions[0].AssetStatus.NameAr;
                    }


                    list.Add(getDataObj);
                }
            }
            return list;
        }

        public IEnumerable<ViewAssetDetailVM> GetListOfAssetDetailsByHospitalNotInContract(int hospitalId)
        {

            List<ViewAssetDetailVM> lstAssetDetails = new List<ViewAssetDetailVM>();
            lstAssetDetails = _context.AssetDetails.Include(a => a.MasterAsset).Include(a => a.Supplier)
                               .Where(a => a.HospitalId == hospitalId)
                               .Select(item => new ViewAssetDetailVM
                               {

                                   Id = item.Id,
                                   AssetName = item.MasterAsset.Name,
                                   AssetNameAr = item.MasterAsset.NameAr,
                                   SerialNumber = item.SerialNumber,
                                   SupplierName = item.Supplier.Name,
                                   SupplierNameAr = item.Supplier.NameAr,
                                   HospitalId = int.Parse(item.HospitalId.ToString()),
                                   HospitalName = item.Hospital.Name,
                                   HospitalNameAr = item.Hospital.NameAr,
                                   Barcode = item.Barcode

                               }).ToList();

            var contractAssetDetailIds = _context.ContractDetails.Include(a => a.AssetDetail).Include(a => a.AssetDetail.Hospital).Where(a => a.AssetDetail.HospitalId == hospitalId).Select(a => a.AssetDetailId).ToList();
            var assetDetailIds = _context.AssetDetails.ToList().Where(a => a.HospitalId == hospitalId).Select(a => a.Id).ToList();
            List<int> lstContractAssetDetailIds = new List<int>();
            if (contractAssetDetailIds.Count > 0)
            {
                foreach (var item in contractAssetDetailIds)
                {
                    lstContractAssetDetailIds.Add(int.Parse(item.ToString()));
                }

                var remainIds = assetDetailIds.Except(lstContractAssetDetailIds);


                lstAssetDetails = lstAssetDetails.Where(a => remainIds.Contains(a.Id)).ToList();
            }
            else
            {
                var remainIds = assetDetailIds;


                lstAssetDetails = lstAssetDetails.Where(a => remainIds.Contains(a.Id)).ToList();
            }



            return lstAssetDetails;
        }

        public IEnumerable<ViewAssetDetailVM> GetNoneExcludedAssetsByHospitalId(int hospitalId)
        {
            List<int> idList = new List<int>();
            List<int> idExcludedList = new List<int>();
            List<ViewAssetDetailVM> viewAssetDetailList = new List<ViewAssetDetailVM>();

            var lstAssetDetailIds = _context.AssetDetails.Include(a => a.MasterAsset).Include(a => a.Supplier)
                             .Where(a => a.HospitalId == hospitalId).ToList();
            foreach (var item in lstAssetDetailIds)
            {
                idList.Add(item.Id);
            }
            var excludedIds = _context.HospitalApplications.Include(a => a.AssetDetail).Where(a => a.AssetDetail.HospitalId == hospitalId).ToList();
            foreach (var itm in excludedIds)
            {
                idExcludedList.Add(int.Parse(itm.AssetId.ToString()));
            }
            var lstRemainIds = idList.Except(idExcludedList);

            foreach (var asset in lstRemainIds)
            {
                var lstAssetTransactions = _context.AssetStatusTransactions.Where(a => a.AssetDetailId == asset).ToList();
                if (lstAssetTransactions.Count > 0)
                {
                    lstAssetTransactions = lstAssetTransactions.OrderByDescending(a => a.StatusDate.Value.ToString()).ToList();

                    var transObj = lstAssetTransactions.FirstOrDefault();
                    if (transObj.AssetStatusId == 3)
                    {
                        viewAssetDetailList.Add(_context.AssetDetails.Include(a => a.MasterAsset).Include(a => a.Supplier)
                                                  .Where(a => a.HospitalId == hospitalId && a.Id == asset)
                                                  .Select(item => new ViewAssetDetailVM
                                                  {
                                                      Id = item.Id,
                                                      AssetName = item.MasterAsset.Name,
                                                      AssetNameAr = item.MasterAsset.NameAr,
                                                      SerialNumber = item.SerialNumber,
                                                      SupplierName = item.Supplier.Name,
                                                      SupplierNameAr = item.Supplier.NameAr,
                                                      HospitalId = item.Hospital.Id,
                                                      HospitalName = item.Hospital.Name,
                                                      HospitalNameAr = item.Hospital.NameAr,
                                                      Barcode = item.Barcode,
                                                  }).FirstOrDefault());
                    }
                }
            }


            return viewAssetDetailList;
        }

        public IEnumerable<ViewAssetDetailVM> GetSupplierNoneExcludedAssetsByHospitalId(int hospitalId)
        {
            List<int> idList = new List<int>();
            List<int> idExcludedList = new List<int>();
            List<ViewAssetDetailVM> viewAssetDetailList = new List<ViewAssetDetailVM>();

            var lstAssetDetailIds = _context.AssetDetails.Include(a => a.MasterAsset).Include(a => a.Supplier)
                             .Where(a => a.HospitalId == hospitalId).ToList();
            foreach (var item in lstAssetDetailIds)
            {
                idList.Add(item.Id);
            }
            var excludedIds = _context.SupplierExecludeAssets.Include(a => a.AssetDetail).Where(a => a.AssetDetail.HospitalId == hospitalId).ToList();
            foreach (var itm in excludedIds)
            {
                idExcludedList.Add(int.Parse(itm.AssetId.ToString()));
            }
            var lstRemainIds = idList.Except(idExcludedList);

            foreach (var asset in lstRemainIds)
            {

                // var lstAssetTransactions = _context.AssetStatusTransactions.Where(a => a.AssetDetailId == asset).ToList();
                var lstAssetTransactions = _context.AssetStatusTransactions.Where(a => a.AssetDetailId == asset).OrderByDescending(a => a.StatusDate.Value.ToString()).ToList();
                if (lstAssetTransactions.Count > 0)
                {
                    // lstAssetTransactions = lstAssetTransactions.OrderByDescending(a => a.StatusDate.Value.ToString()).ToList();

                    var transObj = lstAssetTransactions.FirstOrDefault();
                    if (transObj.AssetStatusId == 3)
                    {
                        //viewAssetDetailList.Add(_context.AssetDetails.Include(a => a.MasterAsset).Include(a => a.Supplier)
                        //                           .Where(a => a.HospitalId == hospitalId && a.Id == asset)
                        //                           .Select(item => new ViewAssetDetailVM
                        //                           {
                        //                               //  Id = item.Id,

                        //                               Id = item.MasterAsset.Id,
                        //                               AssetName = item.MasterAsset.Name,
                        //                               AssetNameAr = item.MasterAsset.NameAr,
                        //                               SerialNumber = item.SerialNumber,
                        //                               SupplierName = item.Supplier.Name,
                        //                               SupplierNameAr = item.Supplier.NameAr,
                        //                               HospitalId = item.Hospital.Id,
                        //                               HospitalName = item.Hospital.Name,
                        //                               HospitalNameAr = item.Hospital.NameAr,
                        //                               Barcode = item.Barcode,
                        //                           }).FirstOrDefault());


                        var assetItem = _context.AssetDetails.Include(a => a.MasterAsset).Include(a => a.Supplier).Include(a => a.Hospital)
                                                  .Where(a => a.HospitalId == hospitalId && a.Id == asset).ToList();

                        foreach (var item in assetItem)
                        {
                            ViewAssetDetailVM assetBarCode = new ViewAssetDetailVM();

                            assetBarCode.Id = item.Id;
                            assetBarCode.AssetName = item.MasterAsset.Name;
                            assetBarCode.AssetNameAr = item.MasterAsset.NameAr;
                            assetBarCode.SerialNumber = item.SerialNumber;
                            assetBarCode.SupplierName = item.Supplier.Name;
                            assetBarCode.SupplierNameAr = item.Supplier.NameAr;
                            assetBarCode.HospitalId = item.Hospital.Id;
                            assetBarCode.HospitalName = item.Hospital.Name;
                            assetBarCode.HospitalNameAr = item.Hospital.NameAr;
                            assetBarCode.Barcode = item.Barcode;
                            assetBarCode.BarCode = item.Barcode;
                            assetBarCode.MasterAssetId = item.MasterAsset.Id;
                            assetBarCode.MasterAssetName = item.MasterAsset.Name;
                            assetBarCode.MasterAssetNameAr = item.MasterAsset.NameAr;
                            viewAssetDetailList.Add(assetBarCode);
                        }
                    }
                }
            }
            return viewAssetDetailList;
        }

        public IEnumerable<IndexAssetDetailVM.GetData> GetHospitalAssets(int hospitalId, int statusId, string userId, int page, int pageSize, Sort sortObj)
        {
            ApplicationUser UserObj = new ApplicationUser();
            Employee empObj = new Employee();
            List<string> userRoleNames = new List<string>();


            var obj = _context.ApplicationUser.Where(a => a.Id == userId).ToList();
            if (obj.Count > 0)
            {
                UserObj = obj[0];
                var roleNames = (from userRole in _context.UserRoles
                                 join role in _context.Roles on userRole.RoleId equals role.Id
                                 where userRole.UserId == userId
                                 select role);
                foreach (var name in roleNames)
                {
                    userRoleNames.Add(name.Name);
                }
            }
            var lstEmployees = _context.Employees.Where(a => a.Email == UserObj.Email).ToList();
            if (lstEmployees.Count > 0)
            {
                empObj = lstEmployees[0];
            }

            var lstAssets = _context.AssetDetails
                             .Include(t => t.Hospital)
                             .Include(t => t.Hospital.Governorate)
                             .Include(t => t.Hospital.City)
                             .Include(t => t.Hospital.Organization)
                             .Include(t => t.Hospital.SubOrganization)
                             .Include(t => t.Supplier)
                             .Include(t => t.MasterAsset)
                             .Include(t => t.MasterAsset.brand).OrderBy(a => a.Barcode).ToList();

            List<IndexAssetDetailVM.GetData> list = new List<IndexAssetDetailVM.GetData>();
            if (lstAssets.Count > 0)
            {
                foreach (var asset in lstAssets)
                {
                    IndexAssetDetailVM.GetData detail = new IndexAssetDetailVM.GetData();
                    detail.Id = asset.Id;
                    var lstStatus = _context.AssetStatusTransactions.Where(a => a.AssetDetailId == asset.Id).OrderByDescending(a => a.StatusDate.Value.Date).ToList();
                    if (lstStatus.Count > 0)
                    {
                        detail.AssetStatusId = lstStatus.FirstOrDefault().AssetStatusId;
                    }

                    detail.Code = asset.Code;
                    detail.UserId = UserObj.Id;
                    detail.Price = asset.Price;
                    detail.BarCode = asset.Barcode;
                    detail.MasterImg = asset.MasterAsset.AssetImg;
                    detail.Serial = asset.SerialNumber;
                    detail.BrandName = asset.MasterAsset.brand != null ? asset.MasterAsset.brand.Name : "";
                    detail.BrandNameAr = asset.MasterAsset.brand != null ? asset.MasterAsset.brand.NameAr : "";
                    detail.Model = asset.MasterAsset.ModelNumber;
                    detail.SerialNumber = asset.SerialNumber;
                    detail.MasterAssetId = asset.MasterAssetId;
                    detail.PurchaseDate = asset.PurchaseDate;
                    detail.HospitalId = asset.Hospital.Id;
                    detail.HospitalName = asset.Hospital.Name;
                    detail.HospitalNameAr = asset.Hospital.NameAr;
                    detail.AssetName = asset.MasterAsset.Name;
                    detail.AssetNameAr = asset.MasterAsset.NameAr;
                    detail.GovernorateId = asset.Hospital.GovernorateId;
                    detail.GovernorateName = asset.Hospital.Governorate.Name;
                    detail.GovernorateNameAr = asset.Hospital.Governorate.NameAr;
                    detail.CityId = asset.Hospital.CityId;
                    detail.CityName = asset.Hospital.City.Name;
                    detail.CityNameAr = asset.Hospital.City.NameAr;
                    detail.OrganizationId = asset.Hospital.OrganizationId;
                    detail.OrgName = asset.Hospital.Organization.Name;
                    detail.OrgNameAr = asset.Hospital.Organization.NameAr;
                    detail.SubOrganizationId = asset.Hospital.SubOrganizationId;
                    detail.SubOrgName = asset.Hospital.SubOrganization.Name;
                    detail.SubOrgNameAr = asset.Hospital.SubOrganization.NameAr;
                    detail.SupplierName = asset.Supplier != null ? asset.Supplier.Name : "";
                    detail.SupplierNameAr = asset.Supplier != null ? asset.Supplier.NameAr : "";
                    detail.QrFilePath = asset.QrFilePath;



                    list.Add(detail);
                }
            }
            if (UserObj.GovernorateId == 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0 && UserObj.OrganizationId == 0 && UserObj.SubOrganizationId == 0)
            {
                list = list.ToList();
            }
            else if (UserObj.GovernorateId > 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
            {
                list = list.Where(t => t.GovernorateId == UserObj.GovernorateId).ToList();
            }
            else if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId == 0)
            {
                list = list.Where(t => t.CityId == UserObj.CityId && t.AssetStatusId == statusId).ToList();
            }
            else if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId == 0 && UserObj.HospitalId == 0)
            {
                list = list.Where(t => t.OrganizationId == UserObj.OrganizationId).ToList();
            }
            else if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId == 0)
            {
                list = list.Where(t => t.SubOrganizationId == UserObj.SubOrganizationId).ToList();
            }
            else if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId > 0)
            {
                if (userRoleNames.Contains("AssetOwner"))
                {
                    list = _context.AssetOwners.Include(a => a.AssetDetail)
                        .Include(a => a.AssetDetail.MasterAsset).Include(a => a.AssetDetail.Hospital)
                                      .Include(a => a.AssetDetail.Supplier).Include(a => a.AssetDetail.MasterAsset.brand)
                                    .Include(a => a.AssetDetail.Hospital.Governorate)
                                    .Include(a => a.AssetDetail.Hospital.City).Include(a => a.AssetDetail.Hospital.Organization).Include(a => a.AssetDetail.Hospital.SubOrganization)
                                    .OrderBy(a => a.AssetDetail.Barcode)
                                    .Where(a => a.EmployeeId == empObj.Id && a.AssetDetail.HospitalId == UserObj.HospitalId).Select(detail => new IndexAssetDetailVM.GetData
                                    {
                                        Id = detail.AssetDetail.Id,
                                        Code = detail.AssetDetail.Code,
                                        UserId = UserObj.Id,
                                        Price = detail.AssetDetail.Price,
                                        BarCode = detail.AssetDetail.Barcode,
                                        MasterImg = detail.AssetDetail.MasterAsset.AssetImg,
                                        Serial = detail.AssetDetail.SerialNumber,
                                        BrandName = detail.AssetDetail.MasterAsset.brand.Name,
                                        BrandNameAr = detail.AssetDetail.MasterAsset.brand.NameAr,
                                        Model = detail.AssetDetail.MasterAsset.ModelNumber,
                                        SerialNumber = detail.AssetDetail.SerialNumber,
                                        MasterAssetId = detail.AssetDetail.MasterAssetId,
                                        PurchaseDate = detail.AssetDetail.PurchaseDate,
                                        HospitalId = detail.AssetDetail.Hospital.Id,
                                        HospitalName = detail.AssetDetail.Hospital.Name,
                                        HospitalNameAr = detail.AssetDetail.Hospital.NameAr,
                                        AssetName = detail.AssetDetail.MasterAsset.Name,
                                        AssetNameAr = detail.AssetDetail.MasterAsset.NameAr,
                                        GovernorateId = detail.AssetDetail.Hospital.GovernorateId,
                                        GovernorateName = detail.AssetDetail.Hospital.Governorate.Name,
                                        GovernorateNameAr = detail.AssetDetail.Hospital.Governorate.NameAr,
                                        CityId = detail.AssetDetail.Hospital.CityId,
                                        CityName = detail.AssetDetail.Hospital.City.Name,
                                        CityNameAr = detail.AssetDetail.Hospital.City.NameAr,
                                        OrganizationId = detail.AssetDetail.Hospital.OrganizationId,
                                        OrgName = detail.AssetDetail.Hospital.Organization.Name,
                                        OrgNameAr = detail.AssetDetail.Hospital.Organization.NameAr,
                                        SubOrganizationId = detail.AssetDetail.Hospital.SubOrganizationId,
                                        SubOrgName = detail.AssetDetail.Hospital.SubOrganization.Name,
                                        SubOrgNameAr = detail.AssetDetail.Hospital.SubOrganization.NameAr,
                                        SupplierName = detail.AssetDetail.Supplier.Name,
                                        SupplierNameAr = detail.AssetDetail.Supplier.NameAr,
                                        QrFilePath = detail.AssetDetail.QrFilePath
                                    }).ToList();

                }
                else
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
            }
            else if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId > 0)
            {
                list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
            }


            if (statusId != 0)
            {
                list = list.Where(a => a.AssetStatusId == statusId).ToList();
            }
            else
            {
                list = list.ToList();
            }







            if (sortObj.AssetName != "")
            {
                if (sortObj.SortStatus == "descending")
                    list = list.OrderByDescending(d => d.AssetName).ToList();
                else
                    list = list.OrderBy(d => d.AssetName).ToList();
            }
            else if (sortObj.AssetName == "")
            {
                list = list.OrderBy(d => d.AssetName).ToList();
            }


            //if (sortObj.AssetNameAr != "")
            //{
            //    if (sortObj.SortStatus == "descending")
            //        list = list.OrderByDescending(d => d.AssetNameAr).ToList();
            //    else
            //        list = list.OrderBy(d => d.AssetNameAr).ToList();
            //}
            //if (sortObj.AssetNameAr == "")
            //{
            //    list = list.OrderBy(d => d.AssetName).ToList();
            //}


            // if (sortObj.GovernorateName != "")
            //{
            //    if (sortObj.SortStatus == "descending")
            //        list = list.OrderByDescending(d => d.GovernorateName).ToList();
            //    else
            //        list = list.OrderBy(d => d.GovernorateName).ToList();
            //}
            //else if (sortObj.GovernorateName == "")
            //{
            //    list = list.OrderBy(d => d.GovernorateName).ToList();
            //}



            //else if (sortObj.GovernorateNameAr != "")
            //{
            //    if (sortObj.SortStatus == "descending")
            //        list = list.OrderByDescending(d => d.GovernorateNameAr).ToList();
            //    else
            //        list = list.OrderBy(d => d.GovernorateNameAr).ToList();
            //}
            //else if (sortObj.HospitalName != "")
            //{
            //    if (sortObj.SortStatus == "descending")
            //        list = list.OrderByDescending(d => d.HospitalName).ToList();
            //    else
            //        list = list.OrderBy(d => d.HospitalName).ToList();
            //}
            //else if (sortObj.HospitalNameAr != "")
            //{
            //    if (sortObj.SortStatus == "descending")
            //        list = list.OrderByDescending(d => d.HospitalNameAr).ToList();
            //    else
            //        list = list.OrderBy(d => d.HospitalNameAr).ToList();
            //}
            //else if (sortObj.GovernorateName != "")
            //{
            //    if (sortObj.SortStatus == "descending")
            //        list = list.OrderByDescending(d => d.GovernorateName).ToList();
            //    else
            //        list = list.OrderBy(d => d.GovernorateName).ToList();
            //}
            //else if (sortObj.GovernorateNameAr != "")
            //{
            //    if (sortObj.SortStatus == "descending")
            //        list = list.OrderByDescending(d => d.GovernorateNameAr).ToList();
            //    else
            //        list = list.OrderBy(d => d.GovernorateNameAr).ToList();
            //}
            //else if (sortObj.OrgName != "")
            //{
            //    if (sortObj.SortStatus == "descending")
            //        list = list.OrderByDescending(d => d.OrgName).ToList();
            //    else
            //        list = list.OrderBy(d => d.OrgName).ToList();
            //}
            //else if (sortObj.OrgNameAr != "")
            //{
            //    if (sortObj.SortStatus == "descending")
            //        list = list.OrderByDescending(d => d.OrgNameAr).ToList();
            //    else
            //        list = list.OrderBy(d => d.OrgNameAr).ToList();
            //}
            //else if (sortObj.SubOrgName != "")
            //{
            //    if (sortObj.SortStatus == "descending")
            //        list = list.OrderByDescending(d => d.SubOrgName).ToList();
            //    else
            //        list = list.OrderBy(d => d.SubOrgName).ToList();
            //}
            //else if (sortObj.SubOrgNameAr != "")
            //{
            //    if (sortObj.SortStatus == "descending")
            //        list = list.OrderByDescending(d => d.SubOrgNameAr).ToList();
            //    else
            //        list = list.OrderBy(d => d.SubOrgNameAr).ToList();
            //}
            //else if (sortObj.BarCode != "")
            //{
            //    if (sortObj.SortStatus == "descending")
            //        list = list.OrderByDescending(d => d.BarCode).ToList();
            //    else
            //        list = list.OrderBy(d => d.BarCode).ToList();
            //}


            //else if (sortObj.Model != "" && sortObj.Model != null)
            //{
            //    if (sortObj.SortStatus == "descending")
            //        list = list.OrderByDescending(d => d.Model).ToList();
            //    else
            //        list = list.OrderBy(d => d.Model).ToList();
            //}

            //else if (sortObj.Serial != "")
            //{
            //    if (sortObj.SortStatus == "descending")
            //        list = list.OrderByDescending(d => d.Serial).ToList();
            //    else
            //        list = list.OrderBy(d => d.Serial).ToList();
            //}

            //else if (sortObj.BrandName != "" && sortObj.BrandName != null)
            //{
            //    if (sortObj.SortStatus == "descending")
            //        list = list.OrderByDescending(d => d.BrandName).ToList();
            //    else
            //        list = list.OrderBy(d => d.BrandName).ToList();
            //}


            //else if (sortObj.BrandNameAr != "" && sortObj.BrandNameAr != null)
            //{
            //    if (sortObj.SortStatus == "descending")
            //        list = list.OrderByDescending(d => d.BrandNameAr).ToList();
            //    else
            //        list = list.OrderBy(d => d.BrandNameAr).ToList();
            //}

            //else if (sortObj.SupplierName != "" && sortObj.SupplierName != null)
            //{
            //    if (sortObj.SortStatus == "descending")
            //        list = list.OrderByDescending(d => d.SupplierName).ToList();
            //    else
            //        list = list.OrderBy(d => d.SupplierName).ToList();
            //}

            //else if (sortObj.SupplierNameAr != "" && sortObj.SupplierNameAr != null)
            //{
            //    if (sortObj.SortStatus == "descending")
            //        list = list.OrderByDescending(d => d.SupplierNameAr).ToList();
            //    else
            //        list = list.OrderBy(d => d.SupplierNameAr).ToList();
            //}


            list = list.ToList();



            return list;
        }

        public AssetDetailAttachment GetLastDocumentForAssetDetailId(int assetDetailId)
        {
            AssetDetailAttachment documentObj = new AssetDetailAttachment();
            var lstDocuments = _context.AssetDetailAttachments.Where(a => a.AssetDetailId == assetDetailId).ToList();
            if (lstDocuments.Count > 0)
            {
                documentObj = lstDocuments.Last();
            }
            return documentObj;
        }

        public IEnumerable<ViewAssetDetailVM> GetAutoCompleteSupplierNoneExcludedAssetsByHospitalId(string barcode, int hospitalId)
        {

            List<int> idList = new List<int>();
            List<int> idExcludedList = new List<int>();
            List<ViewAssetDetailVM> viewAssetDetailList = new List<ViewAssetDetailVM>();

            var lstAssetDetailIds = _context.AssetDetails.Include(a => a.MasterAsset).Include(a => a.Supplier)
                             .Where(a => a.HospitalId == hospitalId).ToList();
            foreach (var item in lstAssetDetailIds)
            {
                idList.Add(item.Id);
            }
            var excludedIds = _context.SupplierExecludeAssets.Include(a => a.AssetDetail).Where(a => a.AssetDetail.HospitalId == hospitalId).ToList();
            foreach (var itm in excludedIds)
            {
                idExcludedList.Add(int.Parse(itm.AssetId.ToString()));
            }
            var lstRemainIds = idList.Except(idExcludedList);

            foreach (var asset in lstRemainIds)
            {
                var lstAssetTransactions = _context.AssetStatusTransactions.Where(a => a.AssetDetailId == asset).OrderByDescending(a => a.StatusDate.Value.ToString()).ToList();
                if (lstAssetTransactions.Count > 0)
                {
                    var transObj = lstAssetTransactions.FirstOrDefault();
                    if (transObj.AssetStatusId == 3)
                    {

                        var assetItem = _context.AssetDetails.Include(a => a.MasterAsset).Include(a => a.Supplier).Include(a => a.Hospital)
                                                   .Where(a => a.Barcode.Contains(barcode) && a.Id == asset).ToList();

                        foreach (var item in assetItem)
                        {
                            ViewAssetDetailVM assetBarCode = new ViewAssetDetailVM();

                            assetBarCode.Id = item.Id;
                            assetBarCode.AssetName = item.MasterAsset.Name;
                            assetBarCode.AssetNameAr = item.MasterAsset.NameAr;
                            assetBarCode.SerialNumber = item.SerialNumber;
                            assetBarCode.SupplierName = item.Supplier.Name;
                            assetBarCode.SupplierNameAr = item.Supplier.NameAr;
                            assetBarCode.HospitalId = item.Hospital.Id;
                            assetBarCode.HospitalName = item.Hospital.Name;
                            assetBarCode.HospitalNameAr = item.Hospital.NameAr;
                            assetBarCode.Barcode = item.Barcode;
                            assetBarCode.BarCode = item.Barcode;
                            assetBarCode.MasterAssetId = item.MasterAsset.Id;
                            assetBarCode.MasterAssetName = item.MasterAsset.Name;
                            assetBarCode.MasterAssetNameAr = item.MasterAsset.NameAr;
                            viewAssetDetailList.Add(assetBarCode);
                        }
                    }
                }
            }
            return viewAssetDetailList;
        }

        public int CountAssetsByHospitalId(int hospitalId)
        {

            if (hospitalId != 0)
                return _context.AssetDetails.Where(a => a.HospitalId == hospitalId).Count();
            else
                return _context.AssetDetails.Count();
        }

        public List<CountAssetVM> ListTopAssetsByHospitalId(int hospitalId)
        {
            List<CountAssetVM> list = new List<CountAssetVM>();
            if (hospitalId == 0)
            {

                //var lstMasters = _context.MasterAssets.ToList().Take(10).ToList();
                //if (lstMasters.Count > 0)
                //{
                //    foreach (var item in lstMasters)
                //    {
                //        var lstAssetDetails = _context.AssetDetails.Include(a => a.Hospital).Where(a => a.MasterAssetId == item.Id).ToList();
                //        foreach (var asset in lstAssetDetails)
                //        {
                //            CountAssetVM countAssetObj = new CountAssetVM();
                //            countAssetObj.AssetName = item.Name;
                //            countAssetObj.AssetNameAr = item.NameAr;
                //            countAssetObj.AssetPrice = lstAssetDetails.Sum(a => Convert.ToDecimal(asset.Price.ToString()));
                //            countAssetObj.CountAssetsByHospital = lstAssetDetails.Count();
                //            list.Add(countAssetObj);
                //        }
                //    }
                //}
                var lstAssetDetails = _context.AssetDetails.Include(a => a.MasterAsset).Include(a => a.Hospital).ToList();
                if (lstAssetDetails.Count > 0)
                {
                    var lstGroupMaster = lstAssetDetails.GroupBy(a => a.MasterAssetId).Select(group => new
                    {
                        masterId = group.Key,
                        CountMasterId = group.Count(),
                        Name = group.FirstOrDefault().MasterAsset.Name,
                        NameAr = group.FirstOrDefault().MasterAsset.NameAr,
                        AssetPrice = group.FirstOrDefault().Price != null ? group.Sum(a => Convert.ToDecimal(group.FirstOrDefault().Price.ToString())) : 0
                    }).OrderByDescending(x => x.CountMasterId).ToList().Take(10).ToList();

                    if (lstGroupMaster.Count > 0)
                    {
                        foreach (var item in lstGroupMaster)
                        {
                            CountAssetVM countAssetObj = new CountAssetVM();
                            countAssetObj.AssetName = item.Name;
                            countAssetObj.AssetNameAr = item.NameAr;
                            countAssetObj.AssetPrice = item.AssetPrice;
                            countAssetObj.CountAssetsByHospital = item.CountMasterId;
                            list.Add(countAssetObj);
                        }
                    }
                }

            }
            else
            {

                var lstAssetDetails = _context.AssetDetails.Include(a => a.MasterAsset).Include(a => a.Hospital).Where(a => a.HospitalId == hospitalId).ToList();
                if (lstAssetDetails.Count > 0)
                {
                    var lstGroupMaster = lstAssetDetails.GroupBy(a => a.MasterAssetId).Select(group => new
                    {
                        masterId = group.Key,
                        CountMasterId = group.Count(),
                        Name = group.FirstOrDefault().MasterAsset.Name,
                        NameAr = group.FirstOrDefault().MasterAsset.NameAr,
                        AssetPrice = group.FirstOrDefault().Price != null ? group.Sum(a => Convert.ToDecimal(group.FirstOrDefault().Price.ToString())) : 0
                    }).OrderByDescending(x => x.CountMasterId).ToList().Take(10).ToList();

                    if (lstGroupMaster.Count > 0)
                    {
                        foreach (var item in lstGroupMaster)
                        {
                            CountAssetVM countAssetObj = new CountAssetVM();
                            countAssetObj.AssetName = item.Name;
                            countAssetObj.AssetNameAr = item.NameAr;
                            countAssetObj.AssetPrice = item.AssetPrice;
                            countAssetObj.CountAssetsByHospital = item.CountMasterId;
                            list.Add(countAssetObj);
                        }
                    }
                }
            }
            return list;
        }

        public List<CountAssetVM> ListAssetsByGovernorateIds()
        {
            List<CountAssetVM> list = new List<CountAssetVM>();
            var lstGovernorates = _context.Governorates.ToList();
            if (lstGovernorates.Count > 0)
            {
                foreach (var gov in lstGovernorates)
                {

                    CountAssetVM countAssetObj = new CountAssetVM();
                    countAssetObj.GovernorateName = gov.Name;
                    countAssetObj.GovernorateNameAr = gov.NameAr;

                    var lstAssetDetails = _context.AssetDetails
                                            .Include(a => a.MasterAsset)
                                            .Include(a => a.Hospital)
                                            .Include(a => a.Hospital.Governorate)
                                            .Include(a => a.Hospital.City)
                                            .Where(a => a.Hospital.GovernorateId == gov.Id).ToList();

                    countAssetObj.CountAssetsByGovernorate = lstAssetDetails.Count();
                    list.Add(countAssetObj);
                }
            }

            return list;
        }

        public List<CountAssetVM> ListAssetsByCityIds()
        {
            List<CountAssetVM> list = new List<CountAssetVM>();

            var lstCities = _context.Cities.ToList();
            if (lstCities.Count > 0)
            {
                foreach (var city in lstCities)
                {

                    CountAssetVM countAssetObj = new CountAssetVM();
                    countAssetObj.CityName = city.Name;
                    countAssetObj.CityNameAr = city.NameAr;

                    var lstAssetDetails = _context.AssetDetails
                                            .Include(a => a.MasterAsset)
                                            .Include(a => a.Hospital)
                                            .Include(a => a.Hospital.Governorate)
                                            .Include(a => a.Hospital.City)
                                            .Where(a => a.Hospital.CityId == city.Id).ToList();

                    countAssetObj.CountAssetsByCity = lstAssetDetails.Count();
                    list.Add(countAssetObj);
                }
            }
            return list;
        }

        public List<CountAssetVM> CountAssetsInHospitalByHospitalId(int hospitalId)
        {
            List<CountAssetVM> list = new List<CountAssetVM>();
            var lstAssetDetails = _context.AssetDetails.Include(a => a.MasterAsset).Include(a => a.Hospital).Where(a => a.HospitalId == hospitalId).ToList();
            if (lstAssetDetails.Count > 0)
            {
                var lstGroupMaster = lstAssetDetails.GroupBy(a => a.MasterAssetId).Select(group => new
                {
                    masterId = group.Key,
                    CountMasterId = group.Count(),
                    Name = group.FirstOrDefault().MasterAsset.Name,
                    NameAr = group.FirstOrDefault().MasterAsset.NameAr,
                    AssetPrice = group.FirstOrDefault().Price != null ? group.Sum(a => Convert.ToDecimal(group.FirstOrDefault().Price.ToString())) : 0
                }).OrderByDescending(x => x.CountMasterId).ToList();

                if (lstGroupMaster.Count > 0)
                {
                    foreach (var item in lstGroupMaster)
                    {
                        CountAssetVM countAssetObj = new CountAssetVM();
                        countAssetObj.AssetName = item.Name;
                        countAssetObj.AssetNameAr = item.NameAr;
                        countAssetObj.AssetPrice = item.AssetPrice;
                        countAssetObj.CountAssetsByHospital = item.CountMasterId;
                        list.Add(countAssetObj);
                    }
                }
            }

            return list;
        }

        public IEnumerable<IndexAssetDetailVM.GetData> AlertAssetsBefore3Monthes()
        {
            List<IndexAssetDetailVM.GetData> lstAssetDetails = new List<IndexAssetDetailVM.GetData>();
            var allAssetDetails = _context.AssetDetails.Include(a => a.MasterAsset)
                .Include(a => a.Hospital)
                .Include(a => a.Hospital.Governorate)
                .Include(a => a.Hospital.City).OrderBy(a => a.Id).ToList();

            if (allAssetDetails.Count > 0)
            {
                foreach (var itm in allAssetDetails)
                {

                    if (itm.WarrantyEnd.HasValue && itm.WarrantyEnd != null)
                    {
                        IndexAssetDetailVM.GetData item = new IndexAssetDetailVM.GetData();
                        item.Id = itm.Id;
                        item.Code = itm.Code;
                        item.Model = itm.MasterAsset.ModelNumber;
                        item.Price = itm.Price;
                        item.Serial = itm.SerialNumber;
                        item.BarCode = itm.Barcode;
                        item.SerialNumber = itm.SerialNumber;
                        item.PurchaseDate = itm.PurchaseDate;
                        item.SupplierId = itm.SupplierId;
                        item.DepartmentId = itm.DepartmentId;
                        item.AssetName = itm.MasterAssetId > 0 ? itm.MasterAsset.Name : "";
                        item.AssetNameAr = itm.MasterAssetId > 0 ? itm.MasterAsset.NameAr : "";
                        var ValidToDate = itm.WarrantyEnd.Value;
                        var expiriesInDays = (int)(ValidToDate - DateTime.Now).TotalDays;
                        if (expiriesInDays <= 90)
                        {
                            item.EndWarrantyDate = expiriesInDays.ToString();
                        }
                        lstAssetDetails.Add(item);
                    }
                }
            }
            lstAssetDetails.RemoveAll(s => s.EndWarrantyDate == null);
            return lstAssetDetails;
        }

        public IEnumerable<IndexAssetDetailVM.GetData> AlertAssetsBefore3Monthes(int duration)
        {
            if (duration == 1)
                duration = 30;
            if (duration == 2)
                duration = 60;
            if (duration == 3)
                duration = 90;


            List<IndexAssetDetailVM.GetData> lstAssetDetails = new List<IndexAssetDetailVM.GetData>();
            var allAssetDetails = _context.AssetDetails.Include(a => a.MasterAsset)
                .Include(a => a.Hospital)
                 .Include(a => a.Department)
                .Include(a => a.Hospital.Governorate)
                .Include(a => a.Hospital.City).OrderBy(a => a.Id).ToList();

            if (allAssetDetails.Count > 0)
            {
                foreach (var itm in allAssetDetails)
                {

                    if (itm.WarrantyEnd.HasValue && itm.WarrantyEnd != null)
                    {
                        IndexAssetDetailVM.GetData item = new IndexAssetDetailVM.GetData();
                        item.Id = itm.Id;
                        item.Code = itm.Code;
                        item.Model = itm.MasterAsset.ModelNumber;
                        item.Price = itm.Price;
                        item.Serial = itm.SerialNumber;
                        item.BarCode = itm.Barcode;
                        item.SerialNumber = itm.SerialNumber;
                        item.PurchaseDate = itm.PurchaseDate;
                        item.SupplierId = itm.SupplierId;
                        item.DepartmentId = itm.DepartmentId;
                        item.AssetName = itm.MasterAssetId > 0 ? itm.MasterAsset.Name : "";
                        item.AssetNameAr = itm.MasterAssetId > 0 ? itm.MasterAsset.NameAr : "";

                        item.DepartmentName = itm.DepartmentId > 0 ? itm.Department.Name : "";
                        item.DepartmentNameAr = itm.DepartmentId > 0 ? itm.Department.NameAr : "";


                        var ValidToDate = itm.WarrantyEnd.Value;
                        var expiriesInDays = (int)(ValidToDate - DateTime.Now).TotalDays;
                        if (expiriesInDays <= duration)
                        {
                            item.EndWarrantyDate = expiriesInDays.ToString();
                        }
                        lstAssetDetails.Add(item);
                    }
                }
            }
            lstAssetDetails.RemoveAll(s => s.EndWarrantyDate == null);
            return lstAssetDetails;
        }

        public IndexAssetDetailVM SearchHospitalAssetsByDepartmentId(int departmentId, string userId, int pageNumber, int pageSize)
        {
            IndexAssetDetailVM mainClass = new IndexAssetDetailVM();
            List<IndexAssetDetailVM.GetData> list = new List<IndexAssetDetailVM.GetData>();
            if (userId != null)
            {
                Employee empObj = new Employee();
                List<string> userRoleNames = new List<string>();
                var obj = _context.ApplicationUser.Where(a => a.Id == userId).ToList();
                var userObj = obj[0];


                var roles = (from userRole in _context.UserRoles
                             join role in _context.ApplicationRole on userRole.RoleId equals role.Id
                             where userRole.UserId == userObj.Id
                             select role);
                foreach (var role in roles)
                {
                    userRoleNames.Add(role.Name);
                }

                var lstEmployees = _context.Employees.Where(a => a.Email == userObj.Email).ToList();
                if (lstEmployees.Count > 0)
                {
                    empObj = lstEmployees[0];
                }

                var lstAllAssets = _context.AssetOwners.Include(a => a.AssetDetail).Include(a => a.Employee).Include(a => a.AssetDetail.MasterAsset).Include(a => a.AssetDetail.Hospital).Include(a => a.AssetDetail.Department)
                                                        .Include(a => a.AssetDetail.Supplier).Include(a => a.AssetDetail.MasterAsset.brand)
                                                        .Include(a => a.AssetDetail.Hospital.Governorate).Include(a => a.AssetDetail.Hospital.City).Include(a => a.AssetDetail.Hospital.Organization).Include(a => a.AssetDetail.Hospital.SubOrganization)
                                                        .OrderBy(a => a.AssetDetail.Barcode).ToList();
                if (lstAllAssets.Count > 0)
                {
                    foreach (var asset in lstAllAssets)
                    {
                        IndexAssetDetailVM.GetData detail = new IndexAssetDetailVM.GetData();

                        var lstStatus = _context.AssetStatusTransactions.Where(a => a.AssetDetailId == asset.AssetDetail.Id).OrderByDescending(a => a.StatusDate).ToList();
                        if (lstStatus.Count > 0)
                        {
                            detail.AssetStatusId = lstStatus.FirstOrDefault().AssetStatusId;
                        }
                        detail.Id = asset.AssetDetail.Id;
                        detail.DepartmentId = asset.AssetDetail.DepartmentId != null ? asset.AssetDetail.DepartmentId : 0;
                        detail.Code = asset.AssetDetail.Code;
                        detail.UserId = userObj.Id;
                        detail.EmployeeId = asset.EmployeeId;
                        detail.Price = asset.AssetDetail.Price;
                        detail.BarCode = asset.AssetDetail.Barcode;
                        detail.MasterImg = asset.AssetDetail.MasterAsset.AssetImg;
                        detail.Serial = asset.AssetDetail.SerialNumber;
                        detail.BrandName = asset.AssetDetail.MasterAsset.brand != null ? asset.AssetDetail.MasterAsset.brand.Name : "";
                        detail.BrandNameAr = asset.AssetDetail.MasterAsset.brand != null ? asset.AssetDetail.MasterAsset.brand.NameAr : "";
                        detail.Model = asset.AssetDetail.MasterAsset.ModelNumber;
                        detail.SerialNumber = asset.AssetDetail.SerialNumber;
                        detail.MasterAssetId = asset.AssetDetail.MasterAssetId;
                        detail.PurchaseDate = asset.AssetDetail.PurchaseDate;
                        detail.HospitalId = asset.AssetDetail.Hospital.Id;
                        detail.HospitalName = asset.AssetDetail.Hospital.Name;
                        detail.HospitalNameAr = asset.AssetDetail.Hospital.NameAr;
                        detail.AssetName = asset.AssetDetail.MasterAsset.Name;
                        detail.AssetNameAr = asset.AssetDetail.MasterAsset.NameAr;


                        // detail.DepartmentId = asset.AssetDetail.Department != null ? asset.AssetDetail.DepartmentId: 0;
                        detail.DepartmentName = asset.AssetDetail.Department != null ? asset.AssetDetail.Department.Name : "";
                        detail.DepartmentNameAr = asset.AssetDetail.Department != null ? asset.AssetDetail.Department.NameAr : "";



                        detail.GovernorateId = asset.AssetDetail.Hospital.GovernorateId;
                        detail.GovernorateName = asset.AssetDetail.Hospital.Governorate.Name;
                        detail.GovernorateNameAr = asset.AssetDetail.Hospital.Governorate.NameAr;
                        detail.CityId = asset.AssetDetail.Hospital.CityId;
                        detail.CityName = asset.AssetDetail.Hospital.City.Name;
                        detail.CityNameAr = asset.AssetDetail.Hospital.City.NameAr;
                        detail.OrganizationId = asset.AssetDetail.Hospital.OrganizationId;
                        detail.OrgName = asset.AssetDetail.Hospital.Organization.Name;
                        detail.OrgNameAr = asset.AssetDetail.Hospital.Organization.NameAr;
                        detail.SubOrganizationId = asset.AssetDetail.Hospital.SubOrganizationId;
                        detail.SubOrgName = asset.AssetDetail.Hospital.SubOrganization.Name;
                        detail.SubOrgNameAr = asset.AssetDetail.Hospital.SubOrganization.NameAr;
                        detail.SupplierName = asset.AssetDetail.Supplier != null ? asset.AssetDetail.Supplier.Name : "";
                        detail.SupplierNameAr = asset.AssetDetail.Supplier != null ? asset.AssetDetail.Supplier.NameAr : "";
                        detail.QrFilePath = asset.AssetDetail.QrFilePath;

                        list.Add(detail);
                    }
                }
                if (userRoleNames.Contains("AssetOwner"))
                {
                    list = list.Where(a => a.HospitalId == userObj.HospitalId).ToList();
                }
                if (userRoleNames.Contains("EngDepManager"))
                {
                    list = list.Where(a => a.HospitalId == userObj.HospitalId).ToList();
                }
                if (userRoleNames.Contains("HospitalManager"))
                {
                    list = list.Where(a => a.HospitalId == userObj.HospitalId).ToList();
                }
                if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                {
                    list = list.ToList();
                }
                if (userObj.GovernorateId > 0 && userObj.CityId == 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                {
                    list = list.Where(a => a.GovernorateId == userObj.GovernorateId).ToList();
                }
                if (userObj.GovernorateId > 0 && userObj.CityId > 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                {
                    list = list.Where(a => a.GovernorateId == userObj.GovernorateId && a.CityId == userObj.CityId).ToList();
                }
                if (userObj.GovernorateId > 0 && userObj.CityId > 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId > 0)
                {

                    list = list.Where(a => a.GovernorateId == userObj.GovernorateId && a.CityId == userObj.CityId && a.HospitalId == userObj.HospitalId).ToList();

                }
                if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId > 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                {
                    list = list.Where(a => a.OrganizationId == userObj.OrganizationId).ToList();
                }
                if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId > 0 && userObj.SubOrganizationId > 0 && userObj.HospitalId == 0)
                {
                    list = list.Where(a => a.OrganizationId == userObj.OrganizationId && a.SubOrganizationId == userObj.SubOrganizationId).ToList();
                }
                if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId > 0 && userObj.SubOrganizationId > 0 && userObj.HospitalId > 0)
                {
                    list = list.Where(a => a.OrganizationId == userObj.OrganizationId && a.SubOrganizationId == userObj.SubOrganizationId && a.HospitalId == userObj.HospitalId).ToList();
                }

                //if (statusId != 0)
                //{
                //    list = list.Where(a => a.AssetStatusId == statusId).ToList();
                //}
                //else
                //{
                //    list = list.ToList();
                //}


                if (departmentId != 0)
                {
                    list = list.Where(a => a.DepartmentId == departmentId).ToList();
                }
                else
                {
                    list = list.ToList();
                }


                var requestsPerPage = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                mainClass.Results = requestsPerPage;
                mainClass.Count = list.Count();
                return mainClass;
            }
            return null;
        }

        public ViewAssetDetailVM GetAssetHistoryById(int assetId)
        {
            ViewAssetDetailVM model = new ViewAssetDetailVM();
            var lstHospitalAssets = _context.AssetDetails.Include(a => a.Supplier)
                                                    .Include(a => a.MasterAsset).Include(a => a.Hospital)
                                                    .Include(a => a.Hospital.Governorate)
                                                    .Include(a => a.Hospital.City)
                                                    .Include(a => a.Hospital.Organization).Include(a => a.Hospital.SubOrganization)
                                                    .Include(a => a.MasterAsset.brand)
                                                    .Include(a => a.MasterAsset.Category)
                                                    .Include(a => a.MasterAsset.SubCategory)
                                                    .Include(a => a.MasterAsset.ECRIS)
                                                    .Include(a => a.Department)
                                                    .Include(a => a.Building).Include(a => a.Floor).Include(a => a.Room)
                                                    .Include(a => a.MasterAsset.Origin).ToList().Where(a => a.Id == assetId).ToList();
            if (lstHospitalAssets.Count > 0)
            {
                var detailObj = lstHospitalAssets[0];
                model.Id = detailObj.Id;
                model.Code = detailObj.Code;
                model.PurchaseDate = detailObj.PurchaseDate != null ? detailObj.PurchaseDate.ToString() : "";
                model.Price = detailObj.Price.ToString();
                model.SerialNumber = detailObj.SerialNumber;
                model.Serial = detailObj.SerialNumber;
                model.Remarks = detailObj.Remarks;
                model.Barcode = detailObj.Barcode;
                model.InstallationDate = detailObj.InstallationDate != null ? detailObj.InstallationDate.Value.ToShortDateString() : "";
                model.WarrantyExpires = detailObj.WarrantyExpires;
                model.WarrantyStart = detailObj.WarrantyStart != null ? detailObj.WarrantyStart.Value.ToShortDateString() : "";
                model.WarrantyEnd = detailObj.WarrantyEnd != null ? detailObj.WarrantyEnd.Value.ToShortDateString() : "";
                model.ReceivingDate = detailObj.ReceivingDate != null ? detailObj.ReceivingDate.Value.ToShortDateString() : "";
                model.OperationDate = detailObj.OperationDate != null ? detailObj.OperationDate.Value.ToShortDateString() : "";
                model.CostCenter = detailObj.CostCenter;
                model.DepreciationRate = detailObj.DepreciationRate;
                model.PONumber = detailObj.PONumber;
                model.QrFilePath = detailObj.QrFilePath;

                model.MasterAssetId = detailObj.MasterAsset.Id;
                model.AssetName = detailObj.MasterAsset.Name;
                model.AssetNameAr = detailObj.MasterAsset.NameAr;
                model.MasterCode = detailObj.MasterAsset.Code;
                model.VersionNumber = detailObj.MasterAsset.VersionNumber;
                model.ModelNumber = detailObj.MasterAsset.ModelNumber;
                //model.Mo = detailObj.MasterAsset.ModelNumber;
                model.ExpectedLifeTime = detailObj.MasterAsset.ExpectedLifeTime != null ? (int)detailObj.MasterAsset.ExpectedLifeTime : 0;
                model.Description = detailObj.MasterAsset.Description;
                model.DescriptionAr = detailObj.MasterAsset.DescriptionAr;
                model.Length = detailObj.MasterAsset.Length.ToString();
                model.Width = detailObj.MasterAsset.Width.ToString();
                model.Weight = detailObj.MasterAsset.Weight.ToString();
                model.Height = detailObj.MasterAsset.Height.ToString();
                model.AssetImg = detailObj.MasterAsset.AssetImg;


                var lstAssetStatus = _context.AssetStatusTransactions.Include(a => a.AssetStatus).Where(a => a.AssetDetailId == detailObj.Id).ToList().OrderByDescending(a => a.StatusDate).ToList();

                if (lstAssetStatus.Count > 0)
                {
                    model.AssetStatus = lstAssetStatus[0].AssetStatus.Name;
                    model.AssetStatusAr = lstAssetStatus[0].AssetStatus.NameAr;
                }
                if (detailObj.BuildingId != null)
                {
                    model.BuildId = detailObj.Building.Id;
                    model.BuildName = detailObj.Building.Name;
                    model.BuildNameAr = detailObj.Building.NameAr;
                }
                if (detailObj.Floor != null)
                {
                    model.FloorId = detailObj.Floor.Id;
                    model.FloorName = detailObj.Floor.Name;
                    model.FloorNameAr = detailObj.Floor.NameAr;
                }
                if (detailObj.Room != null)
                {
                    model.RoomId = detailObj.Room.Id;
                    model.RoomName = detailObj.Room.Name;
                    model.RoomNameAr = detailObj.Room.NameAr;
                }
                if (detailObj.Department != null)
                {
                    model.DepartmentName = detailObj.Department.Name;
                    model.DepartmentNameAr = detailObj.Department.NameAr;
                }
                if (detailObj.MasterAsset.Category != null)
                {
                    model.CategoryName = detailObj.MasterAsset.Category.Name;
                    model.CategoryNameAr = detailObj.MasterAsset.Category.NameAr;
                }
                if (detailObj.Hospital != null)
                {
                    model.HospitalId = detailObj.Hospital.Id;
                    model.HospitalName = detailObj.Hospital.Name;
                    model.HospitalNameAr = detailObj.Hospital.NameAr;
                }
                if (detailObj.Hospital.Governorate != null)
                {
                    model.GovernorateName = detailObj.Hospital.Governorate.Name;
                    model.GovernorateNameAr = detailObj.Hospital.Governorate.NameAr;
                }
                if (detailObj.Hospital.City != null)
                {
                    model.CityName = detailObj.Hospital.City.Name;
                    model.CityNameAr = detailObj.Hospital.City.NameAr;
                }
                if (detailObj.Hospital.Organization != null)
                {
                    model.OrgName = detailObj.Hospital.Organization.Name;
                    model.OrgNameAr = detailObj.Hospital.Organization.NameAr;
                }

                if (detailObj.Hospital.SubOrganization != null)
                {
                    model.SubOrgName = detailObj.Hospital.SubOrganization.Name;
                    model.SubOrgNameAr = detailObj.Hospital.SubOrganization.NameAr;
                }
                if (detailObj.Supplier != null)
                {
                    model.SupplierName = detailObj.Supplier.Name;
                    model.SupplierNameAr = detailObj.Supplier.NameAr;
                }
                if (detailObj.MasterAsset.Category != null)
                {
                    model.CategoryName = detailObj.MasterAsset.Category.Name;
                    model.CategoryNameAr = detailObj.MasterAsset.Category.NameAr;
                }
                if (detailObj.MasterAsset.SubCategory != null)
                {
                    model.SubCategoryName = detailObj.MasterAsset.SubCategory.Name;
                    model.SubCategoryNameAr = detailObj.MasterAsset.SubCategory.NameAr;
                }
                if (detailObj.MasterAsset.Origin != null)
                {
                    model.OriginName = detailObj.MasterAsset.Origin.Name;
                    model.OriginNameAr = detailObj.MasterAsset.Origin.NameAr;
                }
                if (detailObj.MasterAsset.brand != null)
                {
                    model.BrandName = detailObj.MasterAsset.brand.Name;
                    model.BrandNameAr = detailObj.MasterAsset.brand.NameAr;
                }


                List<IndexRequestVM.GetData> lstAssetRequests = new List<IndexRequestVM.GetData>();
                List<ListWorkOrderVM.GetData> allWorkOrders = new List<ListWorkOrderVM.GetData>();
                var lstRequests = _context.Request.Where(a => a.AssetDetailId == assetId).ToList();
                if (lstRequests.Count > 0)
                {
                    foreach (var req in lstRequests)
                    {

                        IndexRequestVM.GetData requestVMObj = new IndexRequestVM.GetData();
                        requestVMObj.RequestCode = req.RequestCode;
                        requestVMObj.RequestDate = req.RequestDate;
                        requestVMObj.Subject = req.Subject;
                        var lstRequestTracking = _context.RequestTracking.Include(a => a.RequestStatus).Where(a => a.RequestId == req.Id).ToList().OrderByDescending(a => a.DescriptionDate.Value.Date).ToList().GroupBy(a => a.RequestId).ToList();

                        foreach (var reqtrack in lstRequestTracking)
                        {
                            requestVMObj.StatusId = (int)reqtrack.FirstOrDefault().RequestStatusId;
                            requestVMObj.StatusName = reqtrack.FirstOrDefault().RequestStatus.Name;
                            requestVMObj.StatusNameAr = reqtrack.FirstOrDefault().RequestStatus.NameAr;
                        }

                        var lstWorkOrders = _context.WorkOrders.Where(a => a.RequestId == req.Id).ToList();



                        if (lstWorkOrders.Count > 0)
                        {

                            foreach (var wo in lstWorkOrders)
                            {
                                ListWorkOrderVM.GetData workOrderVMObj = new ListWorkOrderVM.GetData();
                                workOrderVMObj.WorkOrderNumber = wo.WorkOrderNumber;
                                workOrderVMObj.ActualStartDate = wo.ActualStartDate;
                                workOrderVMObj.Subject = wo.Subject;
                                var lstWOTracking = _context.WorkOrderTrackings.Include(a => a.WorkOrderStatus).Where(a => a.WorkOrderId == wo.Id).ToList().OrderByDescending(a => a.ActualStartDate.Value.Date).ToList().GroupBy(a => a.WorkOrderId).ToList();

                                foreach (var wotrack in lstWOTracking)
                                {
                                    workOrderVMObj.StatusId = (int)wotrack.FirstOrDefault().WorkOrderStatusId;
                                    workOrderVMObj.StatusName = wotrack.FirstOrDefault().WorkOrderStatus.Name;
                                    workOrderVMObj.StatusNameAr = wotrack.FirstOrDefault().WorkOrderStatus.NameAr;
                                }
                                allWorkOrders.Add(workOrderVMObj);
                            }


                            requestVMObj.ListWorkOrders = allWorkOrders;

                        }

                        lstAssetRequests.Add(requestVMObj);
                    }
                }


                model.ListRequests = lstAssetRequests;

                // model.ListWorkOrders = allWorkOrders;
            }
            return model;
        }

        public IndexAssetDetailVM SearchHospitalAssetsByDepartmentId(int departmentId, string userId)
        {
            throw new NotImplementedException();
        }

        public IndexAssetDetailVM GetAssetsByUserId(string userId, int pageNumber, int pageSize)
        {
            IndexAssetDetailVM mainClass = new IndexAssetDetailVM();
            List<IndexAssetDetailVM.GetData> list = new List<IndexAssetDetailVM.GetData>();
            if (userId != null)
            {
                Employee empObj = new Employee();
                List<string> userRoleNames = new List<string>();
                var obj = _context.ApplicationUser.Where(a => a.Id == userId).ToList();
                var userObj = obj[0];


                var roles = (from userRole in _context.UserRoles
                             join role in _context.ApplicationRole on userRole.RoleId equals role.Id
                             where userRole.UserId == userObj.Id
                             select role);
                foreach (var role in roles)
                {
                    userRoleNames.Add(role.Name);
                }

                var lstEmployees = _context.Employees.Where(a => a.Email == userObj.Email).ToList();
                if (lstEmployees.Count > 0)
                {
                    empObj = lstEmployees[0];
                }

                var lstAllAssets = _context.AssetDetails.Include(a => a.MasterAsset).Include(a => a.Hospital)
                                    .Include(a => a.Department).Include(a => a.Supplier).Include(a => a.MasterAsset.brand)
                                    .Include(a => a.Hospital.Governorate).Include(a => a.Hospital.City)
                                    .Include(a => a.Hospital.Organization).Include(a => a.Hospital.SubOrganization)
                                    .OrderBy(a => a.Barcode).ToList();


                if (lstAllAssets.Count > 0)
                {
                    foreach (var asset in lstAllAssets)
                    {
                        IndexAssetDetailVM.GetData detail = new IndexAssetDetailVM.GetData();

                        var lstStatus = _context.AssetStatusTransactions.Where(a => a.AssetDetailId == asset.Id).OrderByDescending(a => a.StatusDate).ToList();
                        if (lstStatus.Count > 0)
                        {
                            detail.AssetStatusId = lstStatus.FirstOrDefault().AssetStatusId;
                        }
                        detail.Id = asset.Id;
                        detail.DepartmentId = asset.DepartmentId != null ? asset.DepartmentId : 0;
                        detail.Code = asset.Code;
                        detail.UserId = userObj.Id;
                        detail.Price = asset.Price;
                        detail.BarCode = asset.Barcode;
                        detail.MasterImg = asset.MasterAsset.AssetImg;
                        detail.Serial = asset.SerialNumber;
                        detail.BrandName = asset.MasterAsset.brand != null ? asset.MasterAsset.brand.Name : "";
                        detail.BrandNameAr = asset.MasterAsset.brand != null ? asset.MasterAsset.brand.NameAr : "";
                        detail.Model = asset.MasterAsset.ModelNumber;
                        detail.SerialNumber = asset.SerialNumber;
                        detail.MasterAssetId = asset.MasterAssetId;
                        detail.PurchaseDate = asset.PurchaseDate;
                        detail.HospitalId = asset.Hospital.Id;
                        detail.HospitalName = asset.Hospital.Name;
                        detail.HospitalNameAr = asset.Hospital.NameAr;
                        detail.AssetName = asset.MasterAsset.Name;
                        detail.AssetNameAr = asset.MasterAsset.NameAr;
                        detail.DepartmentId = asset.Department != null ? asset.DepartmentId : 0;
                        detail.DepartmentName = asset.Department != null ? asset.Department.Name : "";
                        detail.DepartmentNameAr = asset.Department != null ? asset.Department.NameAr : "";
                        detail.GovernorateId = asset.Hospital.GovernorateId;
                        detail.GovernorateName = asset.Hospital.Governorate.Name;
                        detail.GovernorateNameAr = asset.Hospital.Governorate.NameAr;
                        detail.CityId = asset.Hospital.CityId;
                        detail.CityName = asset.Hospital.City.Name;
                        detail.CityNameAr = asset.Hospital.City.NameAr;
                        detail.OrganizationId = asset.Hospital.OrganizationId;
                        detail.OrgName = asset.Hospital.Organization.Name;
                        detail.OrgNameAr = asset.Hospital.Organization.NameAr;
                        detail.SubOrganizationId = asset.Hospital.SubOrganizationId;
                        detail.SubOrgName = asset.Hospital.SubOrganization.Name;
                        detail.SubOrgNameAr = asset.Hospital.SubOrganization.NameAr;
                        detail.SupplierName = asset.Supplier != null ? asset.Supplier.Name : "";
                        detail.SupplierNameAr = asset.Supplier != null ? asset.Supplier.NameAr : "";
                        detail.QrFilePath = asset.QrFilePath;
                        list.Add(detail);
                    }
                }
                if (userRoleNames.Contains("AssetOwner"))
                {
                    list = list.Where(a => a.HospitalId == userObj.HospitalId).ToList();
                }
                if (userRoleNames.Contains("EngDepManager"))
                {
                    list = list.Where(a => a.HospitalId == userObj.HospitalId).ToList();
                }
                if (userRoleNames.Contains("HospitalManager"))
                {
                    list = list.Where(a => a.HospitalId == userObj.HospitalId).ToList();
                }
                if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                {
                    list = list.ToList();
                }
                if (userObj.GovernorateId > 0 && userObj.CityId == 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                {
                    list = list.Where(a => a.GovernorateId == userObj.GovernorateId).ToList();
                }
                if (userObj.GovernorateId > 0 && userObj.CityId > 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                {
                    list = list.Where(a => a.GovernorateId == userObj.GovernorateId && a.CityId == userObj.CityId).ToList();
                }
                if (userObj.GovernorateId > 0 && userObj.CityId > 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId > 0)
                {

                    list = list.Where(a => a.GovernorateId == userObj.GovernorateId && a.CityId == userObj.CityId && a.HospitalId == userObj.HospitalId).ToList();

                }
                if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId > 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                {
                    list = list.Where(a => a.OrganizationId == userObj.OrganizationId).ToList();
                }
                if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId > 0 && userObj.SubOrganizationId > 0 && userObj.HospitalId == 0)
                {
                    list = list.Where(a => a.OrganizationId == userObj.OrganizationId && a.SubOrganizationId == userObj.SubOrganizationId).ToList();
                }
                if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId > 0 && userObj.SubOrganizationId > 0 && userObj.HospitalId > 0)
                {
                    list = list.Where(a => a.OrganizationId == userObj.OrganizationId && a.SubOrganizationId == userObj.SubOrganizationId && a.HospitalId == userObj.HospitalId).ToList();
                }




                var requestsPerPage = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                mainClass.Results = requestsPerPage;
                mainClass.Count = list.Count();
                return mainClass;
            }
            return null;
        }

        public IEnumerable<ViewAssetDetailVM> GetListOfAssetDetailsByHospitalNotInContract(string barcode, int hospitalId)
        {
            List<ViewAssetDetailVM> lstAssetDetails = new List<ViewAssetDetailVM>();
            lstAssetDetails = _context.AssetDetails.Include(a => a.MasterAsset).Include(a => a.Supplier)
                               .Where(a => a.Barcode.Contains(barcode) && a.HospitalId == hospitalId)
                               .Select(item => new ViewAssetDetailVM
                               {
                                   Id = item.Id,
                                   AssetName = item.MasterAsset.Name,
                                   AssetNameAr = item.MasterAsset.NameAr,
                                   SerialNumber = item.SerialNumber,
                                   SupplierName = item.Supplier.Name,
                                   SupplierNameAr = item.Supplier.NameAr,
                                   HospitalId = int.Parse(item.HospitalId.ToString()),
                                   HospitalName = item.Hospital.Name,
                                   HospitalNameAr = item.Hospital.NameAr,
                                   Barcode = item.Barcode
                               }).ToList();

            var contractAssetDetailIds = _context.ContractDetails.Include(a => a.AssetDetail).Include(a => a.AssetDetail.Hospital).Where(a => a.AssetDetail.HospitalId == hospitalId).Select(a => a.AssetDetailId).ToList();
            var assetDetailIds = _context.AssetDetails.ToList().Where(a => a.HospitalId == hospitalId).Select(a => a.Id).ToList();
            List<int> lstContractAssetDetailIds = new List<int>();
            if (contractAssetDetailIds.Count > 0)
            {
                foreach (var item in contractAssetDetailIds)
                {
                    lstContractAssetDetailIds.Add(int.Parse(item.ToString()));
                }
                var remainIds = assetDetailIds.Except(lstContractAssetDetailIds);
                lstAssetDetails = lstAssetDetails.Where(a => remainIds.Contains(a.Id)).ToList();
            }
            else
            {
                var remainIds = assetDetailIds;
                lstAssetDetails = lstAssetDetails.Where(a => remainIds.Contains(a.Id)).ToList();
            }
            return lstAssetDetails;
        }

        public IEnumerable<ViewAssetDetailVM> GetListOfAssetDetailsByHospitalNotInContractBySerialNumber(string serialNumber, int hospitalId)
        {
            List<ViewAssetDetailVM> lstAssetDetails = new List<ViewAssetDetailVM>();
            lstAssetDetails = _context.AssetDetails.Include(a => a.MasterAsset).Include(a => a.Supplier)
                               .Where(a => a.SerialNumber.Contains(serialNumber) && a.HospitalId == hospitalId)
                               .Select(item => new ViewAssetDetailVM
                               {
                                   Id = item.Id,
                                   AssetName = item.MasterAsset.Name,
                                   AssetNameAr = item.MasterAsset.NameAr,
                                   SerialNumber = item.SerialNumber,
                                   SupplierName = item.Supplier.Name,
                                   SupplierNameAr = item.Supplier.NameAr,
                                   HospitalId = int.Parse(item.HospitalId.ToString()),
                                   HospitalName = item.Hospital.Name,
                                   HospitalNameAr = item.Hospital.NameAr,
                                   Barcode = item.Barcode
                               }).ToList();

            var contractAssetDetailIds = _context.ContractDetails.Include(a => a.AssetDetail).Include(a => a.AssetDetail.Hospital).Where(a => a.AssetDetail.HospitalId == hospitalId).Select(a => a.AssetDetailId).ToList();
            var assetDetailIds = _context.AssetDetails.ToList().Where(a => a.HospitalId == hospitalId).Select(a => a.Id).ToList();
            List<int> lstContractAssetDetailIds = new List<int>();
            if (contractAssetDetailIds.Count > 0)
            {
                foreach (var item in contractAssetDetailIds)
                {
                    lstContractAssetDetailIds.Add(int.Parse(item.ToString()));
                }
                var remainIds = assetDetailIds.Except(lstContractAssetDetailIds);
                lstAssetDetails = lstAssetDetails.Where(a => remainIds.Contains(a.Id)).ToList();
            }
            else
            {
                var remainIds = assetDetailIds;
                lstAssetDetails = lstAssetDetails.Where(a => remainIds.Contains(a.Id)).ToList();
            }
            return lstAssetDetails;
        }

        public GeneratedAssetDetailBCVM GenerateAssetDetailBarcode()
        {
            GeneratedAssetDetailBCVM numberObj = new GeneratedAssetDetailBCVM();
            int barCode = 0;

            var lastId = _context.AssetDetails.ToList();
            if (lastId.Count > 0)
            {
                //var code = int.Parse(lastId.LastOrDefault().Barcode);
                var code = lastId.Max(a => a.Barcode);
                if (code.Contains('-'))
                {
                    string[] barcodenumber = code.Split("-");
                    var barcode = (int.Parse(barcodenumber[0]) + 1).ToString();
                    var lastcode = barcode.ToString().PadLeft(9, '0');
                    numberObj.BarCode = lastcode;
                }
                else
                {
                    var barcode = (int.Parse(code) + 1).ToString();
                    var lastcode = code.ToString().PadLeft(9, '0');
                    numberObj.BarCode = lastcode;
                }
            }
            else
            {
                numberObj.BarCode = (barCode + 1).ToString();
            }

            return numberObj;
        }

        public IEnumerable<IndexPMAssetTaskScheduleVM.GetData> GetAllPMAssetTaskScheduleByAssetId(int? assetId)
        {
            List<IndexPMAssetTaskScheduleVM.GetData> list = new List<IndexPMAssetTaskScheduleVM.GetData>();


            var lstSchedule = (from detail in _context.AssetDetails
                               join tsktime in _context.WNPMAssetTimes on detail.Id equals tsktime.AssetDetailId
                               join depart in _context.Departments on detail.DepartmentId equals depart.Id
                               where tsktime.PMDate.Value.Year == DateTime.Today.Date.Year
                                && detail.Id == assetId
                               select new
                               {
                                   Id = tsktime.Id,
                                   TimeId = tsktime.Id,
                                   MasterAssetId = detail.MasterAssetId,
                                   AssetDetailId = detail.Id,
                                   HospitalId = detail.HospitalId,
                                   Barcode = detail.Barcode,
                                   Serial = detail.SerialNumber,
                                   StartDate = tsktime.PMDate,
                                   EndDate = tsktime.PMDate,
                                   start = tsktime.PMDate.Value.ToString(),
                                   end = tsktime.PMDate.Value.ToString(),
                                   DepartmentName = depart.Name,
                                   DepartmentNameAr = depart.NameAr,
                                   IsDone = tsktime.IsDone,
                                   PMDate = tsktime.PMDate,
                                   allDay = true
                               }).ToList()
                               .GroupBy(a => new
                               {
                                   assetId = a.AssetDetailId,
                                   Day = a.StartDate.Value.Day,
                                   Month = a.StartDate.Value.Month,
                                   Year = a.StartDate.Value.Year
                               });


            foreach (var items in lstSchedule)
            {
                string month = "";
                string day = "";
                string endmonth = "";
                string endday = "";


                if (items.FirstOrDefault().StartDate.Value.Month < 10)
                    month = items.FirstOrDefault().StartDate.Value.Month.ToString().PadLeft(2, '0');
                else
                    month = items.FirstOrDefault().StartDate.Value.Month.ToString();

                if (items.FirstOrDefault().EndDate.Value.Month < 10)
                    endmonth = items.FirstOrDefault().EndDate.Value.Month.ToString().PadLeft(2, '0');
                else
                    endmonth = items.FirstOrDefault().EndDate.Value.Month.ToString();

                if (items.FirstOrDefault().StartDate.Value.Day < 10)
                    day = items.FirstOrDefault().StartDate.Value.Day.ToString().PadLeft(2, '0');
                else
                    day = items.FirstOrDefault().StartDate.Value.Day.ToString();

                if (items.FirstOrDefault().EndDate.Value.Day < 10)
                    endday = (items.FirstOrDefault().EndDate.Value.Day).ToString().PadLeft(2, '0');
                else
                    endday = items.FirstOrDefault().EndDate.Value.Day.ToString();




                IndexPMAssetTaskScheduleVM.GetData getDataObj = new IndexPMAssetTaskScheduleVM.GetData();
                var AssetName = _context.MasterAssets.Where(a => a.Id == items.FirstOrDefault().MasterAssetId).ToList().FirstOrDefault().Name;
                var AssetNameAr = _context.MasterAssets.Where(a => a.Id == items.FirstOrDefault().MasterAssetId).ToList().FirstOrDefault().NameAr;
                //  var color = _context.MasterAssets.Where(a => a.Id == items.FirstOrDefault().MasterAssetId).ToList().FirstOrDefault().PMBGColor;
                //var textColor = _context.MasterAssets.Where(a => a.Id == items.FirstOrDefault().MasterAssetId).ToList().FirstOrDefault().PMColor;
                var Serial = items.FirstOrDefault().Serial;
                var DepartmentName = items.FirstOrDefault().DepartmentName;
                var DepartmentNameAr = items.FirstOrDefault().DepartmentNameAr;
                var Barcode = items.FirstOrDefault().Barcode;

                getDataObj.start = items.FirstOrDefault().StartDate.Value.Year + "-" + month + "-" + day;
                getDataObj.end = items.FirstOrDefault().EndDate.Value.Year + "-" + endmonth + "-" + endday;
                getDataObj.title = AssetName + " - " + Serial + " - " + DepartmentName + " - " + Barcode;
                getDataObj.titleAr = AssetNameAr + "  -  " + Serial + " - " + DepartmentNameAr + " - " + Barcode;


                if (items.FirstOrDefault().IsDone == true)
                {
                    getDataObj.color = "#79be47";
                    getDataObj.textColor = "#fff";
                }
                else if (items.FirstOrDefault().PMDate < DateTime.Today.Date && items.FirstOrDefault().IsDone == false)
                {
                    getDataObj.color = "#ff7578";
                    getDataObj.textColor = "#fff";
                }


                getDataObj.Id = items.FirstOrDefault().Id;
                //getDataObj.color = color;
                //getDataObj.textColor = textColor;
                getDataObj.start = items.FirstOrDefault().StartDate.Value.Year + "-" + month + "-" + day;
                getDataObj.end = items.FirstOrDefault().EndDate.Value.Year + "-" + endmonth + "-" + endday;
                getDataObj.allDay = true;
                getDataObj.ListTasks = _context.PMAssetTasks.Where(a => a.MasterAssetId == items.FirstOrDefault().MasterAssetId).ToList();

                list.Add(getDataObj);

            }
            return list;
        }
    }
}

