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
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    assetDetailObj.PurchaseDate = model.PurchaseDate != "" ? DateTime.Parse(model.PurchaseDate) : null;
                    assetDetailObj.Price = model.Price;
                    assetDetailObj.SerialNumber = model.SerialNumber;
                    assetDetailObj.Remarks = model.Remarks;
                    assetDetailObj.Barcode = model.Barcode;
                    assetDetailObj.InstallationDate = model.InstallationDate != "" ? DateTime.Parse(model.InstallationDate) : null;
                    assetDetailObj.RoomId = model.RoomId;
                    assetDetailObj.FloorId = model.FloorId;
                    assetDetailObj.BuildingId = model.BuildingId;
                    assetDetailObj.ReceivingDate = model.ReceivingDate != "" ? DateTime.Parse(model.ReceivingDate) : null;
                    assetDetailObj.OperationDate = model.OperationDate != "" ? DateTime.Parse(model.OperationDate) : null;
                    assetDetailObj.PONumber = model.PONumber;
                    assetDetailObj.DepartmentId = model.DepartmentId;
                    assetDetailObj.SupplierId = model.SupplierId;
                    assetDetailObj.HospitalId = model.HospitalId;
                    assetDetailObj.MasterAssetId = model.MasterAssetId;
                    assetDetailObj.WarrantyStart = model.WarrantyStart != "" ? DateTime.Parse(model.WarrantyStart) : null;
                    assetDetailObj.WarrantyEnd = model.WarrantyEnd != "" ? DateTime.Parse(model.WarrantyEnd) : null;
                    assetDetailObj.DepreciationRate = model.DepreciationRate;
                    assetDetailObj.CostCenter = model.CostCenter;
                    assetDetailObj.WarrantyExpires = model.WarrantyExpires;
                    _context.AssetDetails.Add(assetDetailObj);
                    _context.SaveChanges();

                    int assetDetailId = assetDetailObj.Id;


                    foreach (var item in model.ListOwners)
                    {
                        AssetOwner ownerObj = new AssetOwner();
                        ownerObj.AssetDetailId = assetDetailId;
                        ownerObj.EmployeeId = int.Parse(item.ToString());
                        _context.AssetOwners.Add(ownerObj);
                        _context.SaveChanges();
                    }




                    if (model.MasterAssetId > 0 && model.InstallationDate != "")
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
                                _context.PMAssetTimes.Add(assetTimeObj);
                                _context.SaveChanges();
                            }
                        }

                    }

                    return assetDetailId;
                }
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
                    Price = item.Price,
                    Serial = item.SerialNumber,
                    SerialNumber = item.SerialNumber,
                    PurchaseDate = item.PurchaseDate,
                    //HospitalName = item.HospitalId > 0 ? _context.Hospitals.Where(a => a.Id == item.HospitalId).ToList().First().Name : "",
                    HospitalName = item.HospitalId > 0 ? item.Hospital.Name : "",
                    HospitalNameAr = item.HospitalId > 0 ? item.Hospital.NameAr : "",
                    AssetName = item.MasterAssetId > 0 ? item.MasterAsset.Name : "",
                    AssetNameAr = item.MasterAssetId > 0 ? item.MasterAsset.NameAr : "",
                    //GovernorateName = item.HospitalId > 0 ? (from host in _context.Hospitals
                    //                                         join gov in _context.Governorates on host.GovernorateId equals gov.Id
                    //                                         where host.Id == item.HospitalId
                    //                                         select gov).First().Name : "",

                    GovernorateName = item.HospitalId > 0 ? item.Hospital.Governorate.Name : "",
                    GovernorateNameAr = item.HospitalId > 0 ? item.Hospital.Governorate.NameAr : "",
                    CityName = item.HospitalId > 0 ? item.Hospital.City.Name : "",
                    CityNameAr = item.HospitalId > 0 ? item.Hospital.City.NameAr : ""
                });

            return lstAssetDetails;
        }

        public IEnumerable<IndexAssetDetailVM.GetData> GetAssetDetailsByAssetId(int assetId)
        {
            var lstAssetDetails = _context.AssetDetails.ToList().Where(a => a.MasterAssetId == assetId).Select(item => new IndexAssetDetailVM.GetData
            {
                Id = item.Id,
                Code = item.Code,
                Price = item.Price,
                Serial = item.SerialNumber,
                SerialNumber = item.SerialNumber,
                PurchaseDate = item.PurchaseDate,
                HospitalName = _context.Hospitals.Where(a => a.Id == item.HospitalId).ToList().First().Name,
                HospitalNameAr = _context.Hospitals.Where(a => a.Id == item.HospitalId).ToList().First().NameAr,
                AssetName = _context.MasterAssets.Where(a => a.Id == item.MasterAssetId).ToList().First().Name,
                AssetNameAr = _context.MasterAssets.Where(a => a.Id == item.MasterAssetId).ToList().First().NameAr

            });
            return lstAssetDetails;
        }

        public async Task<IEnumerable<IndexAssetDetailVM.GetData>> GetAssetDetailsByUserId(string userId)
        {
            List<IndexAssetDetailVM.GetData> lstAssetDetails = new List<IndexAssetDetailVM.GetData>();
            if (userId != null)
            {
                var userObj = await _context.Users.FindAsync(userId);
                lstAssetDetails = await _context.AssetDetails
                                        .Include(a => a.Hospital)
                                        .Include(a => a.Hospital.Governorate).Include(a => a.Hospital.City)
                                        .Include(a => a.Hospital.Organization).Include(a => a.Hospital.SubOrganization)
                                        .Include(a => a.MasterAsset)
                                        .Include(a => a.MasterAsset).Select(detail => new IndexAssetDetailVM.GetData
                                        {
                                            Id = detail.Id,
                                            Code = detail.Code,
                                            Price = detail.Price,
                                            Serial = detail.SerialNumber,
                                            SerialNumber = detail.SerialNumber,
                                            MasterAssetId = detail.MasterAssetId,
                                            PurchaseDate = detail.PurchaseDate,
                                            HospitalName = detail.Hospital.Name,
                                            HospitalNameAr = detail.Hospital.NameAr,
                                            AssetName = detail.MasterAsset.Name,
                                            AssetNameAr = detail.MasterAsset.NameAr,
                                            GovernorateName = detail.Hospital.Governorate.Name,
                                            GovernorateNameAr = detail.Hospital.Governorate.NameAr,
                                            CityName = detail.Hospital.City.Name,
                                            CityNameAr = detail.Hospital.City.NameAr,
                                            OrgName = detail.Hospital.Organization.Name,
                                            OrgNameAr = detail.Hospital.Organization.NameAr,
                                            SubOrgName = detail.Hospital.SubOrganization.Name,
                                            SubOrgNameAr = detail.Hospital.SubOrganization.NameAr
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
                    lstAssetDetails = lstAssetDetails.Where(a => a.GovernorateId == userObj.GovernorateId && a.CityId == userObj.CityId && a.HospitalId == userObj.HospitalId).ToList();
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


            }
            return lstAssetDetails;

        }
        public async Task<IEnumerable<IndexAssetDetailVM.GetData>> GetAssetsByUserId(string userId)
        {
            List<IndexAssetDetailVM.GetData> lstAssetDetails = new List<IndexAssetDetailVM.GetData>();
            if (userId != null)
            {
                var userObj = await _context.Users.FindAsync(userId);


                if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                {
                    await _context.AssetDetails.Include(a => a.MasterAsset)
                          .Include(a => a.Hospital).ThenInclude(h => h.Organization)
                          .Include(a => a.Hospital).ThenInclude(h => h.Governorate)
                          .Include(a => a.Hospital).ThenInclude(h => h.City)
                          .Include(a => a.Hospital).ThenInclude(h => h.SubOrganization)
                          .Include(a => a.Supplier)
                          .Select(a => new IndexAssetDetailVM.GetData
                          {
                              Id = a.Id,
                              Code = a.Code,
                              Price = a.Price,
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
                          }).ToListAsync();


                }

                lstAssetDetails = await _context.AssetDetails.Include(a => a.MasterAsset)
                          .Include(a => a.Hospital).ThenInclude(h => h.Organization)
                          .Include(a => a.Hospital).ThenInclude(h => h.Governorate)
                          .Include(a => a.Hospital).ThenInclude(h => h.City)
                          .Include(a => a.Hospital).ThenInclude(h => h.SubOrganization)
                          .Select(a => new IndexAssetDetailVM.GetData
                          {
                              Id = a.Id,
                              Code = a.Code,
                              Price = a.Price,
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
                          }).ToListAsync();


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
                    lstAssetDetails = lstAssetDetails.Where(a => a.GovernorateId == userObj.GovernorateId && a.CityId == userObj.CityId && a.HospitalId == userObj.HospitalId).ToList();
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
            }
            return lstAssetDetails;

        }


        public EditAssetDetailVM GetById(int id)
        {
            var assetDetailObj = _context.AssetDetails.Find(id);
            if (assetDetailObj != null)
            {
                EditAssetDetailVM item = new EditAssetDetailVM();

                item.Id = assetDetailObj.Id;
                item.AssetName = _context.MasterAssets.Where(a => a.Id == assetDetailObj.MasterAssetId).FirstOrDefault().Name;
                item.AssetNameAr = _context.MasterAssets.Where(a => a.Id == assetDetailObj.MasterAssetId).FirstOrDefault().NameAr;
                item.Code = assetDetailObj.Code;
                // item.PurchaseDateString = assetDetailObj.PurchaseDate != null ? assetDetailObj.PurchaseDate.Value.ToShortDateString() : "";

                item.PurchaseDate = assetDetailObj.PurchaseDate != null ? assetDetailObj.PurchaseDate.Value.ToShortDateString() : "";
                item.Price = assetDetailObj.Price;
                item.SerialNumber = assetDetailObj.SerialNumber;
                item.Remarks = assetDetailObj.Remarks;
                item.Barcode = assetDetailObj.Barcode;
                item.InstallationDate = assetDetailObj.InstallationDate != null ? assetDetailObj.InstallationDate.Value.ToShortDateString() : "";
                item.OperationDate = assetDetailObj.OperationDate != null ? assetDetailObj.OperationDate.Value.ToShortDateString() : "";
                item.ReceivingDate = assetDetailObj.ReceivingDate != null ? assetDetailObj.ReceivingDate.Value.ToShortDateString() : "";
                item.PONumber = assetDetailObj.PONumber;
                item.WarrantyExpires = assetDetailObj.WarrantyExpires;
                item.BuildingId = assetDetailObj.BuildingId;
                item.RoomId = assetDetailObj.RoomId;
                item.FloorId = assetDetailObj.FloorId;
                item.DepartmentId = assetDetailObj.DepartmentId;
                item.SupplierId = assetDetailObj.SupplierId;
                item.HospitalId = assetDetailObj.HospitalId;
                item.MasterAssetId = assetDetailObj.MasterAssetId;
                item.WarrantyStart = assetDetailObj.WarrantyStart != null ? assetDetailObj.WarrantyStart.Value.ToShortDateString() : "";
                item.WarrantyEnd = assetDetailObj.WarrantyEnd != null ? assetDetailObj.WarrantyEnd.Value.ToShortDateString() : "";
                item.CostCenter = assetDetailObj.CostCenter;
                item.DepreciationRate = assetDetailObj.DepreciationRate;

                return item;

            }

            return null;
        }

        public int Update(EditAssetDetailVM model)
        {
            try
            {

                var assetDetailObj = _context.AssetDetails.Find(model.Id);
                assetDetailObj.Id = model.Id;
                assetDetailObj.Code = model.Code;
                assetDetailObj.PurchaseDate = model.PurchaseDate != null ? DateTime.Parse(model.PurchaseDate).AddDays(1) : null;
                assetDetailObj.Price = model.Price;
                assetDetailObj.SerialNumber = model.SerialNumber;
                assetDetailObj.Remarks = model.Remarks;
                assetDetailObj.Barcode = model.Barcode;
                assetDetailObj.InstallationDate = model.InstallationDate != null ? DateTime.Parse(model.InstallationDate).AddDays(1) : null;
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
                    _context.AssetOwners.Add(ownerObj);
                    _context.SaveChanges();
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
            var lstHospitalAssets = _context.AssetDetails.ToList().Where(a => a.Id == id).ToList();
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

                var lstAssetStatus = _context.AssetStatusTransactions.Include(a => a.AssetStatus).Where(a => a.AssetDetailId == detailObj.Id).ToList().OrderByDescending(a => a.StatusDate).ToList();

                if (lstAssetStatus.Count > 0)
                {
                    model.AssetStatus = lstAssetStatus[0].AssetStatus.Name;
                    model.AssetStatusAr = lstAssetStatus[0].AssetStatus.NameAr;
                }



                if (detailObj.BuildingId > 0)
                {
                    model.BuildName = _context.Buildings.Where(a => a.Id == detailObj.BuildingId).FirstOrDefault().Name;
                    model.BuildNameAr = _context.Buildings.Where(a => a.Id == detailObj.BuildingId).FirstOrDefault().NameAr;
                }
                if (detailObj.FloorId > 0)
                {
                    model.FloorName = _context.Floors.Where(a => a.Id == detailObj.FloorId).FirstOrDefault().Name;
                    model.FloorNameAr = _context.Floors.Where(a => a.Id == detailObj.FloorId).FirstOrDefault().NameAr;
                }
                if (detailObj.RoomId > 0)
                {
                    model.RoomName = _context.Rooms.Where(a => a.Id == detailObj.RoomId).FirstOrDefault().Name;
                    model.RoomNameAr = _context.Rooms.Where(a => a.Id == detailObj.RoomId).FirstOrDefault().NameAr;
                }

                if (detailObj.DepartmentId > 0)
                {
                    model.DepartmentName = _context.Departments.Where(a => a.Id == detailObj.DepartmentId).FirstOrDefault().Name;
                    model.DepartmentNameAr = _context.Departments.Where(a => a.Id == detailObj.DepartmentId).FirstOrDefault().NameAr;
                }



                var lstMasterAssets = _context.MasterAssets.Where(a => a.Id == detailObj.MasterAssetId).ToList();
                if (lstMasterAssets.Count > 0)
                {
                    MasterAsset masterObj = lstMasterAssets[0];


                    model.MasterAssetId = masterObj.Id;
                    model.AssetName = masterObj.Name + " - " + detailObj.SerialNumber;
                    model.AssetNameAr = masterObj.NameAr + " - " + detailObj.SerialNumber;
                    model.MasterCode = masterObj.Code;
                    model.VersionNumber = masterObj.VersionNumber;
                    model.ModelNumber = masterObj.ModelNumber;
                    model.ExpectedLifeTime = masterObj.ExpectedLifeTime != null ? (int)masterObj.ExpectedLifeTime : 0;
                    model.Description = masterObj.Description;
                    model.DescriptionAr = masterObj.DescriptionAr;
                    model.Length = masterObj.Length.ToString();
                    model.Width = masterObj.Width.ToString();
                    model.Weight = masterObj.Weight.ToString();
                    model.Height = masterObj.Height.ToString();
                    model.AssetImg = masterObj.AssetImg;

                    if (masterObj.CategoryId > 0)
                    {
                        var lstCategories = _context.Categories.Where(a => a.Id == masterObj.CategoryId).ToList();
                        model.CategoryName = lstCategories[0].Name;
                        model.CategoryNameAr = lstCategories[0].NameAr;
                    }

                    if (masterObj.SubCategoryId > 0)
                    {
                        var lstSubCategories = _context.SubCategories.Where(a => a.Id == masterObj.SubCategoryId).ToList();
                        model.SubCategoryName = lstSubCategories[0].Name;
                        model.SubCategoryNameAr = lstSubCategories[0].NameAr;
                    }
                    if (masterObj.OriginId > 0)
                    {
                        var lstOrigins = _context.Origins.Where(a => a.Id == masterObj.OriginId).ToList();
                        model.OriginName = lstOrigins[0].Name;
                        model.OriginNameAr = lstOrigins[0].NameAr;
                    }
                    if (masterObj.BrandId > 0)
                    {
                        var lstBrands = _context.Brands.Where(a => a.Id == masterObj.BrandId).ToList();
                        model.BrandName = lstBrands[0].Name;
                        model.BrandNameAr = lstBrands[0].NameAr;
                    }


                    var lstSuppliers = (from supply in _context.Suppliers
                                        join detail in _context.AssetDetails on supply.Id equals detail.SupplierId
                                        where detail.MasterAssetId == masterObj.Id
                                        select supply).ToList();

                    if (lstSuppliers.Count > 0)
                    {
                        model.SupplierName = lstSuppliers[0].Name;
                        model.SupplierNameAr = lstSuppliers[0].NameAr;
                    }


                    var lstHospitals = (from host in _context.Hospitals
                                        join detail in _context.AssetDetails on host.Id equals detail.HospitalId
                                        where detail.MasterAssetId == masterObj.Id
                                        select host).ToList();

                    if (lstHospitals.Count > 0)
                    {
                        model.HospitalName = lstHospitals[0].Name;
                        model.HospitalNameAr = lstHospitals[0].NameAr;
                    }





                    var lstOrganizations = (from org in _context.Organizations
                                            join host in _context.Hospitals on org.Id equals host.OrganizationId
                                            join detail in _context.AssetDetails on host.Id equals detail.HospitalId
                                            where detail.MasterAssetId == masterObj.Id

                                            select org).ToList();

                    if (lstOrganizations.Count > 0)
                    {
                        model.OrgName = lstOrganizations[0].Name;
                        model.OrgNameAr = lstOrganizations[0].NameAr;
                    }



                    var lstSubOrganizations = (from org in _context.Organizations
                                               join host in _context.Hospitals on org.Id equals host.OrganizationId
                                               join sub in _context.SubOrganizations on org.Id equals sub.OrganizationId
                                               join detail in _context.AssetDetails on host.Id equals detail.HospitalId
                                               where detail.MasterAssetId == masterObj.Id
                                               select sub).ToList();

                    if (lstSubOrganizations.Count > 0)
                    {
                        model.SubOrgName = lstSubOrganizations[0].Name;
                        model.SubOrgNameAr = lstSubOrganizations[0].NameAr;
                    }





                    var lstGovernortes = (from gov in _context.Governorates
                                          join host in _context.Hospitals on gov.Id equals host.GovernorateId
                                          join detail in _context.AssetDetails on host.Id equals detail.HospitalId
                                          where detail.MasterAssetId == masterObj.Id
                                          select gov).ToList();
                    if (lstGovernortes.Count > 0)
                    {
                        model.GovernorateName = lstGovernortes[0].Name;
                        model.GovernorateNameAr = lstGovernortes[0].NameAr;
                    }



                    var lstCities = (from gov in _context.Governorates
                                     join city in _context.Cities on gov.Id equals city.GovernorateId
                                     join host in _context.Hospitals on gov.Id equals host.GovernorateId
                                     join detail in _context.AssetDetails on host.Id equals detail.HospitalId
                                     where detail.MasterAssetId == masterObj.Id
                                     select city).ToList();
                    if (lstCities.Count > 0)
                    {
                        model.CityName = lstGovernortes[0].Name;
                        model.CityNameAr = lstGovernortes[0].NameAr;
                    }
                }


            }
            return model;
        }

        public IEnumerable<IndexAssetDetailVM.GetData> SearchAssetInHospital(SearchMasterAssetVM searchObj)
        {
            List<IndexAssetDetailVM.GetData> lstData = new List<IndexAssetDetailVM.GetData>();

            var list = (from master in _context.MasterAssets
                        join detail in _context.AssetDetails on master.Id equals detail.MasterAssetId
                        join host in _context.Hospitals on detail.HospitalId equals host.Id
                        select new
                        {
                            MasterAssetId = master.Id,
                            Id = detail.Id,
                            AssetName = master.Name,
                            HospitalName = host.Name,
                            HospitalNameAr = host.NameAr,
                            GovernorateId = host.GovernorateId,
                            CityId = host.CityId,
                            OrganizationId = host.OrganizationId,
                            SubOrganization = host.SubOrganizationId,
                            SupplierId = detail.SupplierId,
                            OriginId = master.OriginId,
                            Code = detail.Code,
                            SerialNumber = detail.SerialNumber,
                            BrandId = master.BrandId,
                            HospitalId = detail.HospitalId
                        }).ToList();

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
                list = list.Where(a => a.SubOrganization == searchObj.SubOrganizationId).ToList();
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



            if (searchObj.AssetId != 0)
            {
                list = list.Where(a => a.Id == searchObj.AssetId).ToList();
            }
            else
                list = list.ToList();


            //if (searchObj.AssetName != "")
            //{
            //    list = list.Where(b => b.AssetName.Contains(searchObj.AssetName) || b.SerialNumber.Contains(searchObj.AssetName)).ToList();
            //}




            if (searchObj.Serial != "")
            {
                list = list.Where(b => b.SerialNumber.Contains(searchObj.Serial)).ToList();
            }


            foreach (var item in list)
            {
                IndexAssetDetailVM.GetData getDataObj = new IndexAssetDetailVM.GetData();
                getDataObj.Id = item.Id;
                getDataObj.MasterAssetId = item.MasterAssetId;
                getDataObj.Code = item.Code;
                getDataObj.AssetName = item.AssetName;
                getDataObj.GovernorateName = _context.Governorates.Where(a => a.Id == item.GovernorateId).First().Name;
                getDataObj.CityName = _context.Cities.Where(a => a.Id == item.CityId).First().Name;
                getDataObj.OrgName = _context.Organizations.Where(a => a.Id == item.OrganizationId).First().Name;
                getDataObj.SubOrgName = _context.SubOrganizations.Where(a => a.Id == item.SubOrganization).First().Name;
                getDataObj.HospitalName = item.HospitalName;

                getDataObj.HospitalNameAr = item.HospitalNameAr;



                lstData.Add(getDataObj);
            }
            return lstData;
        }


        public IEnumerable<IndexAssetDetailVM.GetData> SearchAssetInHospitalByHospitalId(SearchMasterAssetVM searchObj)
        {
            List<IndexAssetDetailVM.GetData> lstData = new List<IndexAssetDetailVM.GetData>();

            var list = (from master in _context.MasterAssets
                        join detail in _context.AssetDetails on master.Id equals detail.MasterAssetId
                        join host in _context.Hospitals on detail.HospitalId equals host.Id
                        where detail.HospitalId == searchObj.HospitalId && host.Id == searchObj.HospitalId
                        select new
                        {
                            MasterAssetId = master.Id,
                            Id = detail.Id,
                            AssetName = master.Name,
                            HospitalName = host.Name,
                            HospitalNameAr = host.NameAr,
                            GovernorateId = host.GovernorateId,
                            CityId = host.CityId,
                            OrganizationId = host.OrganizationId,
                            SubOrganization = host.SubOrganizationId,
                            SupplierId = detail.SupplierId,
                            OriginId = master.OriginId,
                            Code = detail.Code,
                            SerialNumber = detail.SerialNumber,
                            BrandId = master.BrandId,
                            HospitalId = detail.HospitalId
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


            if (searchObj.Serial != "")
            {
                list = list.Where(b => b.SerialNumber.Contains(searchObj.Serial)).ToList();
            }


            foreach (var item in list)
            {
                IndexAssetDetailVM.GetData getDataObj = new IndexAssetDetailVM.GetData();
                getDataObj.Id = item.Id;
                getDataObj.Serial = item.SerialNumber;
                getDataObj.MasterAssetId = item.MasterAssetId;
                getDataObj.Code = item.Code;
                getDataObj.AssetName = item.AssetName;
                getDataObj.GovernorateName = _context.Governorates.Where(a => a.Id == item.GovernorateId).First().Name;
                getDataObj.CityName = _context.Cities.Where(a => a.Id == item.CityId).First().Name;
                getDataObj.OrgName = _context.Organizations.Where(a => a.Id == item.OrganizationId).First().Name;
                getDataObj.SubOrgName = _context.SubOrganizations.Where(a => a.Id == item.SubOrganization).First().Name;
                getDataObj.HospitalName = item.HospitalName;
                getDataObj.HospitalNameAr = item.HospitalNameAr;
                lstData.Add(getDataObj);
            }
            return lstData;
        }

        public IEnumerable<IndexPMAssetTaskScheduleVM.GetData> GetAllPMAssetTaskSchedules(int? hospitalId)
        {
            List<IndexPMAssetTaskScheduleVM.GetData> list = new List<IndexPMAssetTaskScheduleVM.GetData>();


            var lstSchedule = (from detail in _context.AssetDetails
                               join tsktime in _context.PMAssetTimes on detail.Id equals tsktime.AssetDetailId
                               join host in _context.Hospitals on detail.HospitalId equals host.Id
                               join schdl in _context.PMAssetTaskSchedules on tsktime.Id equals schdl.PMAssetTimeId
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
            var lstAssetDetails = _context.AssetDetails.Where(a => a.HospitalId == hospitalId && a.MasterAssetId == masterAssetId).ToList();
            return lstAssetDetails;
        }

        public IEnumerable<AssetDetail> GetAllAssetDetailsByHospitalId(int hospitalId)
        {
            var lstAssetDetails = _context.AssetDetails.Where(a => a.HospitalId == hospitalId).ToList();
            return lstAssetDetails;
        }


        public IEnumerable<ViewAssetDetailVM> GetListOfAssetDetailsByHospitalId(int hospitalId)
        {
            var lstAssetDetails = _context.AssetDetails.Include(a => a.MasterAsset)
                                     .Where(a => a.HospitalId == hospitalId)
                                     .Select(item => new ViewAssetDetailVM
                                     {

                                         Id = item.Id,
                                         AssetName = item.MasterAsset.Name + " - " + item.SerialNumber,
                                         AssetNameAr = item.MasterAsset.NameAr + " - " + item.SerialNumber,

                                     }).ToList();
            return lstAssetDetails;
        }

        public List<CountAssetVM> CountAssetsByHospital()
        {
            List<CountAssetVM> list = new List<CountAssetVM>();
            var lstAssetDetails = _context.AssetDetails.Take(5).Include(a => a.MasterAsset).Include(a => a.Hospital).ToList().GroupBy(a => a.MasterAssetId);
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

        public List<PmDateGroupVM> GetAllwithgrouping(int? assetId)
        {

            List<PmDateGroupVM> snanns = new List<PmDateGroupVM>();

            var assetTasks = _context.PMAssetTimes.Where(a => a.AssetDetailId == assetId).ToList().GroupBy(
                e => new
                {
                    day = e.PMDate.Value.Day,
                    month = e.PMDate.Value.Month,
                    year = e.PMDate.Value.Year
                }
            ).Distinct().ToList();
            foreach (var item in assetTasks)
            {

                PmDateGroupVM dates = new PmDateGroupVM();
                dates.PMDate = item.FirstOrDefault().PMDate;
                dates.Id = item.FirstOrDefault().Id;
                dates.AssetTasksList = (from assetTime in _context.PMAssetTimes
                                        .Where(e => e.AssetDetailId == assetId)
                                        join assetTask in _context.PMAssetTasks
                                        on assetTime.AssetDetail.MasterAssetId equals assetTask.MasterAssetId
                                        where assetTime.PMDate.Value.Day == item.First().PMDate.Value.Day
                                       && assetTime.PMDate.Value.Month == item.First().PMDate.Value.Month
                                       && assetTime.PMDate.Value.Year == item.First().PMDate.Value.Year

                                        select assetTask)
                            .ToList().Select(e => new CreatePMAssetTaskVM
                            {
                                Id = e.Id,
                                MasterAssetId = int.Parse(e.MasterAssetId.ToString()),
                                TaskNameAr = e.NameAr,
                                TaskName = e.Name
                            }).ToList();

                if (dates.AssetTasksList.Count > 0)
                {

                    snanns.Add(dates);
                }
                else
                {
                    continue;
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
                                             .Select(a => new IndexAssetDetailVM.GetData
                                             {
                                                 Id = a.Id,
                                                 Code = a.Code,
                                                 Price = a.Price,
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
                        AssetNameAr = Ass.AssetNameAr,
                        BrandName = Ass.BrandName,
                        BrandNameAr = Ass.BrandNameAr,
                        GovernorateName = Ass.GovernorateName,
                        GovernorateNameAr = Ass.GovernorateNameAr,
                        CityName = Ass.CityName,
                        CityNameAr = Ass.CityNameAr,
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
                        AssetName = Ass.AssetName,
                        AssetNameAr = Ass.AssetNameAr,
                        BrandName = Ass.BrandName,
                        BrandNameAr = Ass.BrandNameAr,
                        GovernorateName = Ass.GovernorateName,
                        GovernorateNameAr = Ass.GovernorateNameAr,
                        CityName = Ass.CityName,
                        CityNameAr = Ass.CityNameAr,
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
                        AssetName = Ass.AssetName,
                        AssetNameAr = Ass.AssetNameAr,
                        BrandName = Ass.BrandName,
                        BrandNameAr = Ass.BrandNameAr,
                        GovernorateName = Ass.GovernorateName,
                        GovernorateNameAr = Ass.GovernorateNameAr,
                        CityName = Ass.CityName,
                        CityNameAr = Ass.CityNameAr,
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
                        AssetName = Ass.AssetName,
                        AssetNameAr = Ass.AssetNameAr,
                        BrandName = Ass.BrandName,
                        BrandNameAr = Ass.BrandNameAr,
                        GovernorateName = Ass.GovernorateName,
                        GovernorateNameAr = Ass.GovernorateNameAr,
                        CityName = Ass.CityName,
                        CityNameAr = Ass.CityNameAr,
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
                        AssetName = Ass.AssetName,
                        AssetNameAr = Ass.AssetNameAr,
                        BrandName = Ass.BrandName,
                        BrandNameAr = Ass.BrandNameAr,
                        GovernorateName = Ass.GovernorateName,
                        GovernorateNameAr = Ass.GovernorateNameAr,
                        CityName = Ass.CityName,
                        CityNameAr = Ass.CityNameAr,
                        HospitalName = Ass.HospitalName,
                        HospitalNameAr = Ass.HospitalNameAr,
                        SupplierName = Ass.SupplierName,
                        SupplierNameAr = Ass.SupplierNameAr,
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
                        AssetName = Ass.AssetName,
                        AssetNameAr = Ass.AssetNameAr,
                        BrandName = Ass.BrandName,
                        BrandNameAr = Ass.BrandNameAr,
                        GovernorateName = Ass.GovernorateName,
                        GovernorateNameAr = Ass.GovernorateNameAr,
                        CityName = Ass.CityName,
                        CityNameAr = Ass.CityNameAr,
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

            var lstAssetDetails = _context.AssetDetails.Include(a => a.MasterAsset)
                 .Include(a => a.Hospital)
                 .Include(a => a.Hospital.Governorate)
                 .Include(a => a.Hospital.City)
                 .ToList();
            foreach (var item in lstAssetDetails)
            {
                IndexAssetDetailVM.GetData Assetobj = new IndexAssetDetailVM.GetData();
                Assetobj.Id = item.Id;
                Assetobj.Code = item.Code;
                Assetobj.HospitalName = item.HospitalId > 0 ? item.Hospital.Name : "";
                Assetobj.HospitalNameAr = item.HospitalId > 0 ? item.Hospital.NameAr : "";
                Assetobj.AssetName = item.MasterAssetId > 0 ? item.MasterAsset.Name : "";
                Assetobj.AssetNameAr = item.MasterAssetId > 0 ? item.MasterAsset.NameAr : "";
                Assetobj.GovernorateName = item.HospitalId > 0 ? item.Hospital.Governorate.Name : "";
                Assetobj.GovernorateNameAr = item.HospitalId > 0 ? item.Hospital.Governorate.NameAr : "";
                lstAssetData.Add(Assetobj);
            }
            if (sortObj.AssetName != "" || sortObj.AssetNameAr != "")
            {
                lstAssetData = lstAssetData.OrderBy(d => d.AssetName).ThenBy(d => d.AssetNameAr).ToList();
            }

            if (sortObj.GovernorateName != "" || sortObj.GovernorateNameAr != "")
            {
                lstAssetData = lstAssetData.OrderBy(d => d.GovernorateName).ThenBy(d => d.GovernorateNameAr).ToList();
            }

            if (sortObj.HospitalName != "" || sortObj.HospitalNameAr != "")
            {
                lstAssetData = lstAssetData.OrderBy(d => d.HospitalName).ThenBy(d => d.HospitalNameAr).ToList();
            }

            if (sortObj.GovernorateName != "" || sortObj.GovernorateNameAr != "")
            {
                lstAssetData = lstAssetData.OrderBy(d => d.HospitalName).ThenBy(d => d.HospitalNameAr).ToList();
            }

            if (sortObj.OrgName != "" || sortObj.OrgNameAr != "")
            {
                lstAssetData = lstAssetData.OrderBy(d => d.HospitalName).ThenBy(d => d.HospitalNameAr).ToList();
            }

            if (sortObj.SubOrgName != "" || sortObj.SubOrgNameAr != "")
            {
                lstAssetData = lstAssetData.OrderBy(d => d.HospitalName).ThenBy(d => d.HospitalNameAr).ToList();
            }

            if (sortObj.Code != "")
            {
                lstAssetData = lstAssetData.OrderBy(d => d.Code).ToList();
            }

            return lstAssetData;
        }
    }
}

