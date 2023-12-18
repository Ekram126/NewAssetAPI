using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.ScrapVM;
using Microsoft.EntityFrameworkCore;

namespace Asset.Core.Repositories
{
    public class ScrapRepository : IScrapRepository
    {
        private ApplicationDbContext _context;
        public ScrapRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public int Add(CreateScrapVM createScrapVM)
        {
            Scrap scrapObj = new Scrap();
            scrapObj.AssetDetailId = createScrapVM.AssetDetailId;
            scrapObj.ScrapDate = createScrapVM.ScrapDate;
            scrapObj.ScrapDate = DateTime.Parse(createScrapVM.StrScrapDate);
            scrapObj.SysDate = createScrapVM.SysDate;
            scrapObj.Comment = createScrapVM.Comment;
            scrapObj.ScrapNo = createScrapVM.ScrapNo;
            // scrapObj.ListAttachments = createScrapVM.ListAttachments;
            _context.Scraps.Add(scrapObj);
            _context.SaveChanges();
            int id = scrapObj.Id;
            if (createScrapVM.ReasonIds.Count() > 0)
            {
                foreach (var reasonId in createScrapVM.ReasonIds)
                {
                    AssetScrap assetScrapObj = new AssetScrap();
                    assetScrapObj.ScrapId = id;
                    assetScrapObj.ScrapReasonId = reasonId;

                    _context.AssetScraps.Add(assetScrapObj);
                    _context.SaveChanges();
                }
            }
            return scrapObj.Id;
        }
        public GeneratedScrapNumberVM GenerateScrapNumber()
        {
            GeneratedScrapNumberVM numberObj = new GeneratedScrapNumberVM();
            string WO = "Scr";

            var lstIds = _context.Scraps.ToList();
            if (lstIds.Count > 0)
            {
                var code = lstIds.LastOrDefault().Id;
                numberObj.ScrapNo = WO + (code + 1);
            }
            else
            {
                numberObj.ScrapNo = WO + 1;
            }

            return numberObj;
        }
        public int CreateScrapAttachments(ScrapAttachment attachObj)
        {
            ScrapAttachment documentObj = new ScrapAttachment();
            documentObj.Title = attachObj.Title;
            documentObj.FileName = attachObj.FileName;
            documentObj.ScrapId = attachObj.ScrapId;
            _context.ScrapAttachments.Add(documentObj);
            _context.SaveChanges();
            return attachObj.Id;
        }
        public int Delete(int id)
        {
            var scrapObj = _context.Scraps.Find(id);
            try
            {

                _context.Scraps.Remove(scrapObj);
                var isDeleted = _context.SaveChanges();
                if (isDeleted == 1)
                {
                    var lstAssetTrans = _context.AssetStatusTransactions.Include(a => a.AssetDetail).Where(a => a.AssetDetailId == scrapObj.AssetDetailId).ToList();
                    if (lstAssetTrans.Count > 0)
                    {
                        AssetStatusTransaction AssetStatusTransactionsTransactionObj = new AssetStatusTransaction();
                        AssetStatusTransactionsTransactionObj.AssetDetailId = (int)scrapObj.AssetDetailId;
                        AssetStatusTransactionsTransactionObj.AssetStatusId = 3;
                        var lstAssets = _context.AssetDetails.Where(a => a.Id == scrapObj.AssetDetailId).ToList();
                        if (lstAssets.Count > 0)
                        {
                            AssetStatusTransactionsTransactionObj.HospitalId = lstAssets[0].HospitalId;
                        }
                        AssetStatusTransactionsTransactionObj.StatusDate = DateTime.Now;
                        _context.AssetStatusTransactions.Add(AssetStatusTransactionsTransactionObj);
                        _context.SaveChanges();
                    }
                }
                return 1;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }
        public List<IndexScrapVM.GetData> GetAll()
        {
            List<IndexScrapVM.GetData> list = new List<IndexScrapVM.GetData>();
            var lstScraps = _context.Scraps
                .Include(a => a.AssetDetail)
                .Include(a => a.AssetDetail.MasterAsset)
                .Include(a => a.AssetDetail.Department)
                .Include(a => a.AssetDetail.MasterAsset.brand)
               .ToList();
            foreach (var item in lstScraps)
            {
                IndexScrapVM.GetData scrapObj = new IndexScrapVM.GetData();
                scrapObj.Id = item.Id;
                scrapObj.AssetId = item.AssetDetailId;
                scrapObj.AssetName = item.AssetDetail.MasterAsset != null ? item.AssetDetail.MasterAsset.Name : "";
                scrapObj.AssetNameAr = item.AssetDetail.MasterAsset != null ? item.AssetDetail.MasterAsset.NameAr : "";
                scrapObj.Barcode = item.AssetDetail.Barcode;
                scrapObj.BarCode = item.AssetDetail.Barcode;
                scrapObj.SerialNumber = item.AssetDetail.SerialNumber;
                scrapObj.ScrapDate = item.ScrapDate.ToString();
                scrapObj.ScrapNo = item.ScrapNo;
                scrapObj.Comment = item.Comment;
                if (item.AssetDetail.MasterAsset != null)
                    scrapObj.Model = item.AssetDetail.MasterAsset.ModelNumber;

                scrapObj.DepartmentName = item.AssetDetail.Department.Name;
                scrapObj.DepartmentNameAr = item.AssetDetail.Department.NameAr;
                if (item.AssetDetail.MasterAsset.brand != null)
                {
                    scrapObj.BrandId = item.AssetDetail.MasterAsset.BrandId;
                    scrapObj.BrandName = item.AssetDetail.MasterAsset.brand.Name;
                    scrapObj.BrandNameAr = item.AssetDetail.MasterAsset.brand.NameAr;
                }
                scrapObj.Model = item.AssetDetail.MasterAsset.ModelNumber;


                list.Add(scrapObj);
            }
            return list;
        }
        public IndexScrapVM GetAllScraps(int pageNumber, int pageSize)
        {
            IndexScrapVM mainClass = new IndexScrapVM();
            List<IndexScrapVM.GetData> list = new List<IndexScrapVM.GetData>();
            var lstScrap = _context.Scraps
                     .Include(a => a.AssetDetail)
                         .Include(a => a.AssetDetail.MasterAsset)
                          .Include(a => a.AssetDetail.MasterAsset.AssetPeriority)
                         .Include(a => a.AssetDetail.MasterAsset.brand)
                         .Include(a => a.AssetDetail.Hospital)
                         .Include(a => a.AssetDetail.Hospital.Governorate)
                         .Include(a => a.AssetDetail.Hospital.City)
                         .Include(a => a.AssetDetail.Hospital.Organization)
                         .Include(a => a.AssetDetail.Hospital.SubOrganization)
                         .Include(a => a.AssetDetail.Supplier)
                         .Include(a => a.AssetDetail.Department)
                         .OrderBy(a => a.AssetDetail.Barcode).ToList();

            foreach (var detail in lstScrap)
            {
                IndexScrapVM.GetData item = new IndexScrapVM.GetData();
                item.Id = detail.Id;
                item.ScrapDate = detail.ScrapDate.ToString();
                item.ScrapNo = detail.ScrapNo;
                item.Comment = detail.Comment;
                item.AssetId = detail.AssetDetailId;
                item.MasterAssetId = detail.AssetDetail.MasterAssetId;
                item.BarCode = detail.AssetDetail.Barcode;
                item.Barcode = detail.AssetDetail.Barcode;
                item.CreatedBy = detail.AssetDetail.CreatedBy;
                item.SerialNumber = detail.AssetDetail.SerialNumber;
                item.DepartmentId = detail.AssetDetail.DepartmentId;
                item.HospitalId = detail.AssetDetail.HospitalId;
                item.QrFilePath = detail.AssetDetail.QrFilePath;
                item.Model = detail.AssetDetail.MasterAsset.ModelNumber;
                item.OriginId = detail.AssetDetail.MasterAsset.OriginId;
                item.HospitalName = detail.AssetDetail.Hospital.Name;
                item.HospitalNameAr = detail.AssetDetail.Hospital.NameAr;
                if (detail.AssetDetail.MasterAsset.brand != null)
                {
                    item.BrandId = detail.AssetDetail.MasterAsset.BrandId;
                    item.BrandName = detail.AssetDetail.MasterAsset.brand.Name;
                    item.BrandNameAr = detail.AssetDetail.MasterAsset.brand.NameAr;
                }
                item.MasterAssetId = detail.AssetDetail.MasterAsset.Id;
                item.AssetName = detail.AssetDetail.MasterAsset.Name;
                item.AssetNameAr = detail.AssetDetail.MasterAsset.NameAr;
                item.AssetImg = detail.AssetDetail.MasterAsset.AssetImg;

                if (detail.AssetDetail.Department != null)
                {
                    item.DepartmentName = detail.AssetDetail.Department.Name;
                    item.DepartmentNameAr = detail.AssetDetail.Department.NameAr;
                }
                var lstAssetStatus = _context.AssetStatusTransactions.Where(a => a.AssetDetailId == detail.Id).ToList().OrderByDescending(a => a.StatusDate).ToList();
                if (lstAssetStatus.Count > 0)
                {
                    item.AssetStatusId = lstAssetStatus[0].AssetStatusId;
                }


                if (detail.AssetDetail.Supplier != null)
                {
                    item.SupplierId = detail.AssetDetail.SupplierId;
                    item.SupplierName = detail.AssetDetail.Supplier.Name;
                    item.SupplierNameAr = detail.AssetDetail.Supplier.NameAr;
                }

                if (detail.AssetDetail.Hospital != null)
                {
                    item.GovernorateId = detail.AssetDetail.Hospital.GovernorateId;
                    item.GovernorateName = detail.AssetDetail.Hospital.Governorate.Name;
                    item.GovernorateNameAr = detail.AssetDetail.Hospital.Governorate.NameAr;
                    item.CityName = detail.AssetDetail.Hospital.City.Name;
                    item.CityNameAr = detail.AssetDetail.Hospital.City.NameAr;
                    item.OrgName = detail.AssetDetail.Hospital.Organization.Name;
                    item.OrgNameAr = detail.AssetDetail.Hospital.Organization.NameAr;
                    item.SubOrgName = detail.AssetDetail.Hospital.SubOrganization.Name;
                    item.SubOrgNameAr = detail.AssetDetail.Hospital.SubOrganization.NameAr;
                    item.CityId = detail.AssetDetail.Hospital.CityId;
                    item.OrganizationId = detail.AssetDetail.Hospital.OrganizationId;
                    item.SubOrganizationId = detail.AssetDetail.Hospital.SubOrganizationId;
                }
                list.Add(item);
            }
            var requestsPerPage = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            mainClass.Results = requestsPerPage;
            mainClass.Count = list.Count();
            return mainClass;
        }
        public ViewScrapVM ViewScrapById(int id)
        {
            var lstScraps = _context.Scraps.Include(a => a.AssetDetail).Include(a => a.AssetDetail.MasterAsset)
                .Include(a => a.AssetDetail.Department)
                .Include(a => a.AssetDetail.MasterAsset.brand).Where(a => a.Id == id).ToList();
            if (lstScraps.Count > 0)
            {
                Scrap scrapObj = lstScraps[0];
                ViewScrapVM editscrapObj = new ViewScrapVM();
                scrapObj.Id = scrapObj.Id;
                editscrapObj.AssetName = scrapObj.AssetDetail.MasterAsset.Name;
                editscrapObj.AssetNameAr = scrapObj.AssetDetail.MasterAsset.NameAr;
                editscrapObj.Barcode = scrapObj.AssetDetail.Barcode;
                editscrapObj.Model = scrapObj.AssetDetail.MasterAsset.ModelNumber;
                editscrapObj.ScrapDate = scrapObj.ScrapDate.ToString();
                editscrapObj.ScrapNo = scrapObj.ScrapNo;
                editscrapObj.Comment = scrapObj.Comment;
                editscrapObj.SerialNumber = scrapObj.AssetDetail.SerialNumber;

                if (scrapObj.AssetDetail.MasterAsset.brand != null)
                {
                    editscrapObj.BrandId = scrapObj.AssetDetail.MasterAsset.BrandId;
                    editscrapObj.BrandName = scrapObj.AssetDetail.MasterAsset.brand.Name;
                    editscrapObj.BrandNameAr = scrapObj.AssetDetail.MasterAsset.brand.NameAr;
                }
                if (scrapObj.AssetDetail.Department != null)
                {
                    editscrapObj.DepartmentName = scrapObj.AssetDetail.Department.Name;
                    editscrapObj.DepartmentNameAr = scrapObj.AssetDetail.Department.NameAr;
                }
                return editscrapObj;
            }
            return null;
        }
        public Scrap GetById(int id)
        {
            return _context.Scraps.Find(id);
        }

        public IndexScrapVM.GetData GetById2(int id)
        {
            IndexScrapVM.GetData scrapObj = new IndexScrapVM.GetData();
            var lstScraps = _context.Scraps.Include(a => a.AssetDetail)
               .Include(a => a.AssetDetail.MasterAsset)
               .Include(a => a.AssetDetail.Department)
               .Include(a => a.AssetDetail.MasterAsset.brand)
               .Where(a => a.Id == id)
              .ToList();
            if (lstScraps.Count > 0)
            {
                var item = lstScraps[0];
                scrapObj.Id = item.Id;
                scrapObj.AssetName = item.AssetDetail.MasterAsset != null ? item.AssetDetail.MasterAsset.Name : "";
                scrapObj.AssetNameAr = item.AssetDetail.MasterAsset != null ? item.AssetDetail.MasterAsset.NameAr : "";
                scrapObj.Barcode = item.AssetDetail.Barcode;
                scrapObj.BarCode = item.AssetDetail.Barcode;
                scrapObj.Model = item.AssetDetail.MasterAsset.ModelNumber;
                scrapObj.SerialNumber = item.AssetDetail.SerialNumber;
                scrapObj.ScrapDate = item.ScrapDate.ToString();
                scrapObj.ScrapNo = item.ScrapNo;
                scrapObj.Comment = item.Comment;
                if (item.AssetDetail.Department != null)
                {
                    scrapObj.DepartmentName = item.AssetDetail.Department.Name;
                    scrapObj.DepartmentNameAr = item.AssetDetail.Department.NameAr;
                }
                if (item.AssetDetail.MasterAsset.brand != null)
                {
                    scrapObj.BrandId = item.AssetDetail.MasterAsset.BrandId;
                    scrapObj.BrandName = item.AssetDetail.MasterAsset.brand.Name;
                    scrapObj.BrandNameAr = item.AssetDetail.MasterAsset.brand.NameAr;
                }
            }

            return scrapObj;
        }
        public IEnumerable<ScrapAttachment> GetScrapAttachmentByScrapId(int scrapId)
        {
            var lstAttachments = _context.ScrapAttachments.Where(a => a.ScrapId == scrapId).ToList();
            return lstAttachments;
        }
        public List<ViewScrapVM> GetScrapReasonByScrapId(int scrapId)
        {
            var lstReasons = _context.AssetScraps.Include(a => a.ScrapReason).Where(a => a.ScrapId == scrapId).ToList()
                .Select(item => new ViewScrapVM
                {
                    ScrapReasonName = item.ScrapReason.Name,
                    ScrapReasonNameAr = item.ScrapReason.NameAr
                }).ToList();
            return lstReasons;
        }
        public IEnumerable<IndexScrapVM.GetData> SearchInScraps(SearchScrapVM searchObj)
        {

            List<IndexScrapVM.GetData> list = new List<IndexScrapVM.GetData>();

            ApplicationUser userObj = new ApplicationUser();
            ApplicationRole roleObj = new ApplicationRole();
            List<string> userRoleNames = new List<string>();
            var lstUsers = _context.ApplicationUser.Where(a => a.Id == searchObj.UserId).ToList();
            if (lstUsers.Count > 0)
            {
                userObj = lstUsers[0];

                var roles = (from userRole in _context.UserRoles
                             join role in _context.ApplicationRole on userRole.RoleId equals role.Id
                             where userRole.UserId == searchObj.UserId
                             select role);
                foreach (var role in roles)
                {
                    userRoleNames.Add(role.Name);
                }
            }
            var lstScrap = _context.Scraps
                         .Include(a => a.AssetDetail)
                             .Include(a => a.AssetDetail.MasterAsset)
                              .Include(a => a.AssetDetail.MasterAsset.AssetPeriority)
                             .Include(a => a.AssetDetail.MasterAsset.brand)
                             .Include(a => a.AssetDetail.Hospital)
                             .Include(a => a.AssetDetail.Hospital.Governorate)
                             .Include(a => a.AssetDetail.Hospital.City)
                             .Include(a => a.AssetDetail.Hospital.Organization)
                             .Include(a => a.AssetDetail.Hospital.SubOrganization)
                             .Include(a => a.AssetDetail.Supplier).Include(a => a.AssetDetail.Department)
                             .OrderBy(a => a.AssetDetail.Barcode).ToList();





            foreach (var detail in lstScrap)
            {
                IndexScrapVM.GetData item = new IndexScrapVM.GetData();
                item.Id = detail.Id;
                item.ScrapDate = detail.ScrapDate.ToString();
                item.SortScrapDate = detail.ScrapDate;
                item.ScrapNo = detail.ScrapNo;
                item.Comment = detail.Comment;
                item.AssetId = detail.AssetDetailId;
                item.MasterAssetId = detail.AssetDetail.MasterAssetId;
                item.BarCode = detail.AssetDetail.Barcode;
                item.Barcode = detail.AssetDetail.Barcode;
                item.CreatedBy = detail.AssetDetail.CreatedBy;
                item.SerialNumber = detail.AssetDetail.SerialNumber;
                item.DepartmentId = detail.AssetDetail.DepartmentId;
                item.HospitalId = detail.AssetDetail.HospitalId;
                item.QrFilePath = detail.AssetDetail.QrFilePath;
                item.Model = detail.AssetDetail.MasterAsset.ModelNumber;
                item.OriginId = detail.AssetDetail.MasterAsset.OriginId;

                if (detail.AssetDetail.MasterAsset.brand != null)
                {
                    item.BrandId = detail.AssetDetail.MasterAsset.BrandId;
                    item.BrandName = detail.AssetDetail.MasterAsset.brand.Name;
                    item.BrandNameAr = detail.AssetDetail.MasterAsset.brand.NameAr;
                }
                item.MasterAssetId = detail.AssetDetail.MasterAsset.Id;
                item.AssetName = detail.AssetDetail.MasterAsset.Name;
                item.AssetNameAr = detail.AssetDetail.MasterAsset.NameAr;
                item.AssetImg = detail.AssetDetail.MasterAsset.AssetImg;
                var lstAssetStatus = _context.AssetStatusTransactions.Where(a => a.AssetDetailId == detail.Id).ToList().OrderByDescending(a => a.StatusDate).ToList();
                if (lstAssetStatus.Count > 0)
                {
                    item.AssetStatusId = lstAssetStatus[0].AssetStatusId;
                }
                if (detail.AssetDetail.Supplier != null)
                {
                    item.SupplierId = detail.AssetDetail.SupplierId;
                    item.SupplierName = detail.AssetDetail.Supplier.Name;
                    item.SupplierNameAr = detail.AssetDetail.Supplier.NameAr;
                }

                if (detail.AssetDetail.Department != null)
                {
                    item.DepartmentName = detail.AssetDetail.Department.Name;
                    item.DepartmentNameAr = detail.AssetDetail.Department.NameAr;
                }

                if (detail.AssetDetail.Hospital != null)
                {
                    item.HospitalName = detail.AssetDetail.Hospital.Name;
                    item.HospitalNameAr = detail.AssetDetail.Hospital.NameAr;
                    item.GovernorateId = detail.AssetDetail.Hospital.GovernorateId;
                    item.GovernorateName = detail.AssetDetail.Hospital.Governorate.Name;
                    item.GovernorateNameAr = detail.AssetDetail.Hospital.Governorate.NameAr;
                    item.CityName = detail.AssetDetail.Hospital.City.Name;
                    item.CityNameAr = detail.AssetDetail.Hospital.City.NameAr;
                    item.OrgName = detail.AssetDetail.Hospital.Organization.Name;
                    item.OrgNameAr = detail.AssetDetail.Hospital.Organization.NameAr;
                    item.SubOrgName = detail.AssetDetail.Hospital.SubOrganization.Name;
                    item.SubOrgNameAr = detail.AssetDetail.Hospital.SubOrganization.NameAr;
                    item.CityId = detail.AssetDetail.Hospital.CityId;
                    item.OrganizationId = detail.AssetDetail.Hospital.OrganizationId;
                    item.SubOrganizationId = detail.AssetDetail.Hospital.SubOrganizationId;
                }
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
                    list = list.Where(b => b.Model == searchObj.ModelNumber).ToList();
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
                if (searchObj.StartDate2 == "")
                {
                    startingFrom = DateTime.Parse("1900-01-01");
                }
                else
                {
                    searchObj.StartingDate = DateTime.Parse(searchObj.StartDate2.ToString());
                    var startyear = searchObj.StartingDate.Value.Year;
                    var startmonth = searchObj.StartingDate.Value.Month;
                    var startday = searchObj.StartingDate.Value.Day;
                    if (startday < 10)
                        setstartday = searchObj.StartingDate.Value.Day.ToString().PadLeft(2, '0');
                    else
                        setstartday = searchObj.StartingDate.Value.Day.ToString();
                    if (startmonth < 10)
                        setstartmonth = searchObj.StartingDate.Value.Month.ToString().PadLeft(2, '0');
                    else
                        setstartmonth = searchObj.StartingDate.Value.Month.ToString();
                    var sDate = startyear + "/" + setstartmonth + "/" + setstartday;
                    startingFrom = DateTime.Parse(sDate);
                }
                if (searchObj.EndDate2 == "")
                {
                    endingTo = DateTime.Today.Date;
                }
                else
                {
                    searchObj.EndingDate = DateTime.Parse(searchObj.EndDate2.ToString());
                    var endyear = searchObj.EndingDate.Value.Year;
                    var endmonth = searchObj.EndingDate.Value.Month;
                    var endday = searchObj.EndingDate.Value.Day;
                    if (endday < 10)
                        setendday = searchObj.EndingDate.Value.Day.ToString().PadLeft(2, '0');
                    else
                        setendday = searchObj.EndingDate.Value.Day.ToString();
                    if (endmonth < 10)
                        setendmonth = searchObj.EndingDate.Value.Month.ToString().PadLeft(2, '0');
                    else
                        setendmonth = searchObj.EndingDate.Value.Month.ToString();
                    var eDate = endyear + "/" + setendmonth + "/" + setendday;
                    endingTo = DateTime.Parse(eDate);
                }
                if (searchObj.StartDate2 != "" && searchObj.EndDate2 != "")
                {
                    list = list.Where(a => a.SortScrapDate.Value.Date >= startingFrom.Date && a.SortScrapDate.Value.Date <= endingTo.Date).ToList();
                }



            }

            return list;
        }
        //public IndexScrapVM SearchInScraps(SearchScrapVM searchObj, int pageNumber, int pageSize)
        //{
        //    IndexScrapVM mainClass = new IndexScrapVM();
        //    List<IndexScrapVM.GetData> list = new List<IndexScrapVM.GetData>();

        //    ApplicationUser userObj = new ApplicationUser();
        //    ApplicationRole roleObj = new ApplicationRole();
        //    List<string> userRoleNames = new List<string>();
        //    var lstUsers = _context.ApplicationUser.Where(a => a.Id == searchObj.UserId).ToList();
        //    if (lstUsers.Count > 0)
        //    {
        //        userObj = lstUsers[0];

        //        var roles = (from userRole in _context.UserRoles
        //                     join role in _context.ApplicationRole on userRole.RoleId equals role.Id
        //                     where userRole.UserId == searchObj.UserId
        //                     select role);
        //        foreach (var role in roles)
        //        {
        //            userRoleNames.Add(role.Name);
        //        }
        //    }
        //    var lstScrap = _context.Scraps
        //                 .Include(a => a.AssetDetail)
        //                     .Include(a => a.AssetDetail.MasterAsset)
        //                      .Include(a => a.AssetDetail.MasterAsset.AssetPeriority)
        //                     .Include(a => a.AssetDetail.MasterAsset.brand)
        //                     .Include(a => a.AssetDetail.Hospital)
        //                     .Include(a => a.AssetDetail.Hospital.Governorate)
        //                     .Include(a => a.AssetDetail.Hospital.City)
        //                     .Include(a => a.AssetDetail.Hospital.Organization)
        //                     .Include(a => a.AssetDetail.Hospital.SubOrganization)
        //                     .Include(a => a.AssetDetail.Supplier).Include(a => a.AssetDetail.Department)
        //                     .OrderBy(a => a.AssetDetail.Barcode).ToList();

        //    foreach (var detail in lstScrap)
        //    {
        //        IndexScrapVM.GetData item = new IndexScrapVM.GetData();
        //        item.Id = detail.Id;
        //        item.ScrapDate = detail.ScrapDate.ToString();
        //        item.SortScrapDate = detail.ScrapDate;
        //        item.ScrapNo = detail.ScrapNo;
        //        item.Comment = detail.Comment;

        //        item.AssetId = detail.AssetDetailId;
        //        item.MasterAssetId = detail.AssetDetail.MasterAssetId;
        //        item.BarCode = detail.AssetDetail.Barcode;
        //        item.Barcode = detail.AssetDetail.Barcode;
        //        item.CreatedBy = detail.AssetDetail.CreatedBy;
        //        item.SerialNumber = detail.AssetDetail.SerialNumber;
        //        item.DepartmentId = detail.AssetDetail.DepartmentId;
        //        item.HospitalId = detail.AssetDetail.HospitalId;
        //        item.QrFilePath = detail.AssetDetail.QrFilePath;
        //        item.Model = detail.AssetDetail.MasterAsset.ModelNumber;
        //        item.OriginId = detail.AssetDetail.MasterAsset.OriginId;
        //        if (detail.AssetDetail.MasterAsset.brand != null)
        //        {
        //            item.BrandId = detail.AssetDetail.MasterAsset.BrandId;
        //            item.BrandName = detail.AssetDetail.MasterAsset.brand.Name;
        //            item.BrandNameAr = detail.AssetDetail.MasterAsset.brand.NameAr;
        //        }
        //        item.MasterAssetId = detail.AssetDetail.MasterAsset.Id;
        //        item.AssetName = detail.AssetDetail.MasterAsset.Name;
        //        item.AssetNameAr = detail.AssetDetail.MasterAsset.NameAr;
        //        item.AssetImg = detail.AssetDetail.MasterAsset.AssetImg;
        //        var lstAssetStatus = _context.AssetStatusTransactions.Where(a => a.AssetDetailId == detail.Id).ToList().OrderByDescending(a => a.StatusDate).ToList();
        //        if (lstAssetStatus.Count > 0)
        //        {
        //            item.AssetStatusId = lstAssetStatus[0].AssetStatusId;
        //        }
        //        if (detail.AssetDetail.Supplier != null)
        //        {
        //            item.SupplierId = detail.AssetDetail.SupplierId;
        //            item.SupplierName = detail.AssetDetail.Supplier.Name;
        //            item.SupplierNameAr = detail.AssetDetail.Supplier.NameAr;
        //        }
        //        if (detail.AssetDetail.Department != null)
        //        {
        //            item.DepartmentId = detail.AssetDetail.DepartmentId;
        //            item.DepartmentName = detail.AssetDetail.Department.Name;
        //            item.DepartmentNameAr = detail.AssetDetail.Department.NameAr;
        //        }
        //        item.HospitalName = detail.AssetDetail.Hospital.Name;
        //        item.HospitalNameAr = detail.AssetDetail.Hospital.NameAr;
        //        item.GovernorateId = detail.AssetDetail.Hospital.GovernorateId;
        //        item.GovernorateName = detail.AssetDetail.Hospital.Governorate.Name;
        //        item.GovernorateNameAr = detail.AssetDetail.Hospital.Governorate.NameAr;
        //        item.CityName = detail.AssetDetail.Hospital.City.Name;
        //        item.CityNameAr = detail.AssetDetail.Hospital.City.NameAr;
        //        item.OrgName = detail.AssetDetail.Hospital.Organization.Name;
        //        item.OrgNameAr = detail.AssetDetail.Hospital.Organization.NameAr;
        //        item.SubOrgName = detail.AssetDetail.Hospital.SubOrganization.Name;
        //        item.SubOrgNameAr = detail.AssetDetail.Hospital.SubOrganization.NameAr;
        //        item.CityId = detail.AssetDetail.Hospital.CityId;
        //        item.OrganizationId = detail.AssetDetail.Hospital.OrganizationId;
        //        item.SubOrganizationId = detail.AssetDetail.Hospital.SubOrganizationId;

        //        list.Add(item);
        //    }



        //    if (searchObj.GovernorateId > 0 && searchObj.CityId > 0 && searchObj.HospitalId > 0)
        //    {
        //        list = list.Where(a => a.HospitalId == searchObj.HospitalId).ToList();
        //    }
        //    else if (searchObj.GovernorateId == 0 && searchObj.CityId == 0 && searchObj.OrganizationId == 0 && searchObj.SubOrganizationId == 0 && searchObj.HospitalId == 0)
        //    {
        //        list = list.ToList();
        //    }
        //    else if (searchObj.GovernorateId > 0 && searchObj.CityId == 0 && searchObj.OrganizationId == 0 && searchObj.SubOrganizationId == 0 && searchObj.HospitalId == 0)
        //    {
        //        list = list.Where(a => a.GovernorateId == searchObj.GovernorateId).ToList();
        //    }
        //    else if (searchObj.GovernorateId > 0 && searchObj.CityId > 0 && searchObj.OrganizationId == 0 && searchObj.SubOrganizationId == 0 && searchObj.HospitalId == 0)
        //    {
        //        list = list.Where(a => a.CityId == searchObj.CityId).ToList();
        //    }
        //    else if (searchObj.GovernorateId == 0 && searchObj.CityId == 0 && searchObj.OrganizationId > 0 && searchObj.SubOrganizationId == 0 && searchObj.HospitalId == 0)
        //    {
        //        list = list.Where(a => a.OrganizationId == searchObj.OrganizationId).ToList();
        //    }
        //    else if (searchObj.GovernorateId == 0 && searchObj.CityId == 0 && searchObj.OrganizationId > 0 && searchObj.SubOrganizationId > 0 && searchObj.HospitalId == 0)
        //    {
        //        list = list.Where(a => a.SubOrganizationId == searchObj.SubOrganizationId).ToList();
        //    }
        //    else if (searchObj.GovernorateId > 0 && searchObj.CityId > 0 && searchObj.OrganizationId > 0 && searchObj.SubOrganizationId > 0 && searchObj.HospitalId == 0)
        //    {
        //        list = list.Where(a => a.SubOrganizationId == searchObj.SubOrganizationId).ToList();
        //    }
        //    else if (searchObj.GovernorateId > 0 && searchObj.CityId > 0 && searchObj.OrganizationId > 0 && searchObj.SubOrganizationId == 0 && searchObj.HospitalId == 0)
        //    {
        //        list = list.Where(a => a.OrganizationId == searchObj.OrganizationId).ToList();
        //    }


        //    if (searchObj.ModelNumber != "")
        //    {
        //        list = list.Where(b => b.Model.Contains( searchObj.ModelNumber)).ToList();
        //    }
        //    else
        //        list = list.ToList();

        //    if (searchObj.OriginId != 0)
        //    {
        //        list = list.Where(a => a.OriginId == searchObj.OriginId).ToList();
        //    }
        //    else
        //        list = list.ToList();

        //    if (searchObj.BrandId != 0)
        //    {
        //        list = list.Where(a => a.BrandId == searchObj.BrandId).ToList();
        //    }
        //    else
        //        list = list.ToList();

        //    if (searchObj.MasterAssetId != 0)
        //    {
        //        list = list.Where(a => a.MasterAssetId == searchObj.MasterAssetId).ToList();
        //    }
        //    else
        //        list = list.ToList();

        //    if (searchObj.DepartmentId != 0)
        //    {
        //        list = list.Where(a => a.DepartmentId == searchObj.DepartmentId).ToList();
        //    }
        //    else
        //        list = list.ToList();

        //    if (searchObj.SerialNumber != "")
        //    {
        //        list = list.Where(b => b.SerialNumber == searchObj.SerialNumber).ToList();
        //    }
        //    else
        //        list = list.ToList();

        //    if (searchObj.BarCode != "")
        //    {
        //        list = list.Where(b => b.BarCode == searchObj.BarCode).ToList();
        //    }
        //    else
        //        list = list.ToList();



        //    if (searchObj.SupplierId != 0)
        //    {
        //        list = list.Where(a => a.SupplierId == searchObj.SupplierId).ToList();
        //    }
        //    else
        //        list = list.ToList();


        //    if (searchObj.PeriorityId != 0)
        //    {
        //        list = list.Where(b => b.PeriorityId == searchObj.PeriorityId).ToList();
        //    }

        //    string setstartday, setstartmonth, setendday, setendmonth = "";
        //    DateTime startingFrom = new DateTime();
        //    DateTime endingTo = new DateTime();
        //    if (searchObj.StartDate2 == "")
        //    {
        //        startingFrom = DateTime.Parse("1900-01-01");
        //    }
        //    else
        //    {
        //        searchObj.StartingDate = DateTime.Parse(searchObj.StartDate2.ToString());
        //        var startyear = searchObj.StartingDate.Value.Year;
        //        var startmonth = searchObj.StartingDate.Value.Month;
        //        var startday = searchObj.StartingDate.Value.Day;
        //        if (startday < 10)
        //            setstartday = searchObj.StartingDate.Value.Day.ToString().PadLeft(2, '0');
        //        else
        //            setstartday = searchObj.StartingDate.Value.Day.ToString();
        //        if (startmonth < 10)
        //            setstartmonth = searchObj.StartingDate.Value.Month.ToString().PadLeft(2, '0');
        //        else
        //            setstartmonth = searchObj.StartingDate.Value.Month.ToString();
        //        var sDate = startyear + "/" + setstartmonth + "/" + setstartday;
        //        startingFrom = DateTime.Parse(sDate);
        //    }
        //    if (searchObj.EndDate2 == "")
        //    {
        //        endingTo = DateTime.Today.Date;
        //    }
        //    else
        //    {
        //        searchObj.EndingDate = DateTime.Parse(searchObj.EndDate2.ToString());
        //        var endyear = searchObj.EndingDate.Value.Year;
        //        var endmonth = searchObj.EndingDate.Value.Month;
        //        var endday = searchObj.EndingDate.Value.Day;
        //        if (endday < 10)
        //            setendday = searchObj.EndingDate.Value.Day.ToString().PadLeft(2, '0');
        //        else
        //            setendday = searchObj.EndingDate.Value.Day.ToString();
        //        if (endmonth < 10)
        //            setendmonth = searchObj.EndingDate.Value.Month.ToString().PadLeft(2, '0');
        //        else
        //            setendmonth = searchObj.EndingDate.Value.Month.ToString();
        //        var eDate = endyear + "/" + setendmonth + "/" + setendday;
        //        endingTo = DateTime.Parse(eDate);
        //    }
        //    if (searchObj.StartDate2 != "" && searchObj.EndDate2 != "")
        //    {
        //        list = list.Where(a => a.SortScrapDate != null && a.SortScrapDate.Value.Date >= startingFrom.Date && a.SortScrapDate.Value.Date <= endingTo.Date).ToList();
        //    }


        //    var requestsPerPage = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        //    mainClass.Results = requestsPerPage;
        //    mainClass.Count = list.Count();
        //    return mainClass;
        //}


        public IndexScrapVM SearchInScraps(SearchScrapVM searchObj, int pageNumber, int pageSize)
        {
            IndexScrapVM mainClass = new IndexScrapVM();
            List<IndexScrapVM.GetData> list = new List<IndexScrapVM.GetData>();

            ApplicationUser userObj = new ApplicationUser();
            ApplicationRole roleObj = new ApplicationRole();
            List<string> userRoleNames = new List<string>();
            var lstUsers = _context.ApplicationUser.Where(a => a.Id == searchObj.UserId).ToList();
            if (lstUsers.Count > 0)
            {
                userObj = lstUsers[0];

                var roles = (from userRole in _context.UserRoles
                             join role in _context.ApplicationRole on userRole.RoleId equals role.Id
                             where userRole.UserId == searchObj.UserId
                             select role);
                foreach (var role in roles)
                {
                    userRoleNames.Add(role.Name);
                }
            }

            IQueryable<IndexScrapVM.GetData> lstScrap = _context.Scraps
                         .Include(a => a.AssetDetail)
                             .Include(a => a.AssetDetail.MasterAsset)
                              .Include(a => a.AssetDetail.MasterAsset.AssetPeriority)
                             .Include(a => a.AssetDetail.MasterAsset.brand)
                             .Include(a => a.AssetDetail.Hospital)
                             .Include(a => a.AssetDetail.Hospital.Governorate)
                             .Include(a => a.AssetDetail.Hospital.City)
                             .Include(a => a.AssetDetail.Hospital.Organization)
                             .Include(a => a.AssetDetail.Hospital.SubOrganization)
                             .Include(a => a.AssetDetail.Supplier)
                             .Include(a => a.AssetDetail.Department)
                             .OrderBy(a => a.AssetDetail.Barcode)
                                    .Select(detail => new IndexScrapVM.GetData
                                    {

                                        Id = detail.Id,
                                        ScrapDate = detail.ScrapDate.ToString(),
                                        SortScrapDate = detail.ScrapDate,
                                        ScrapNo = detail.ScrapNo,
                                        Comment = detail.Comment,
                                        AssetId = detail.AssetDetailId,
                                        BarCode = detail.AssetDetail.Barcode,
                                        Barcode = detail.AssetDetail.Barcode,
                                        CreatedBy = detail.AssetDetail.CreatedBy,
                                        SerialNumber = detail.AssetDetail.SerialNumber,
                                        HospitalId = detail.AssetDetail.HospitalId,
                                        QrFilePath = detail.AssetDetail.QrFilePath,
                                        Model = detail.AssetDetail.MasterAsset.ModelNumber,
                                        OriginId = detail.AssetDetail.MasterAsset.OriginId,
                                        BrandId = detail.AssetDetail.MasterAsset.brand != null ? detail.AssetDetail.MasterAsset.BrandId : 0,
                                        BrandName = detail.AssetDetail.MasterAsset.brand != null ? detail.AssetDetail.MasterAsset.brand.Name : "",
                                        BrandNameAr = detail.AssetDetail.MasterAsset.brand != null ? detail.AssetDetail.MasterAsset.brand.NameAr : "",
                                        MasterAssetId = detail.AssetDetail.MasterAsset != null ? detail.AssetDetail.MasterAssetId : 0,
                                        AssetName = detail.AssetDetail.MasterAsset.Name,
                                        AssetNameAr = detail.AssetDetail.MasterAsset.NameAr,
                                        AssetImg = detail.AssetDetail.MasterAsset.AssetImg,
                                        SupplierId = detail.AssetDetail.Supplier != null ? detail.AssetDetail.SupplierId : 0,
                                        SupplierName = detail.AssetDetail.Supplier != null ? detail.AssetDetail.Supplier.Name : "",
                                        SupplierNameAr = detail.AssetDetail.Supplier != null ? detail.AssetDetail.Supplier.NameAr : "",
                                        DepartmentId = detail.AssetDetail.Department != null ? int.Parse(detail.AssetDetail.DepartmentId.ToString()) : 0,
                                        DepartmentName = detail.AssetDetail.Department != null ? detail.AssetDetail.Department.Name : "",
                                        DepartmentNameAr = detail.AssetDetail.Department != null ? detail.AssetDetail.Department.NameAr : "",
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
                                    })
                                  .AsQueryable();

            if (lstScrap.Count() > 0)
            {
                if (searchObj.HospitalId > 0)
                {
                    lstScrap = lstScrap.Where(a => a.HospitalId == searchObj.HospitalId).AsQueryable();
                }
                else if (searchObj.GovernorateId == 0 && searchObj.CityId == 0 && searchObj.OrganizationId == 0 && searchObj.SubOrganizationId == 0 && searchObj.HospitalId == 0)
                {
                    lstScrap = lstScrap.AsQueryable();
                }
                else if (searchObj.GovernorateId > 0 && searchObj.CityId == 0 && searchObj.OrganizationId == 0 && searchObj.SubOrganizationId == 0 && searchObj.HospitalId == 0)
                {
                    lstScrap = lstScrap.Where(a => a.GovernorateId == searchObj.GovernorateId).AsQueryable();
                }
                else if (searchObj.GovernorateId > 0 && searchObj.CityId > 0 && searchObj.OrganizationId == 0 && searchObj.SubOrganizationId == 0 && searchObj.HospitalId == 0)
                {
                    lstScrap = lstScrap.Where(a => a.CityId == searchObj.CityId).AsQueryable();
                }
                else if (searchObj.GovernorateId == 0 && searchObj.CityId == 0 && searchObj.OrganizationId > 0 && searchObj.SubOrganizationId == 0 && searchObj.HospitalId == 0)
                {
                    lstScrap = lstScrap.Where(a => a.OrganizationId == searchObj.OrganizationId).AsQueryable();
                }
                else if (searchObj.GovernorateId == 0 && searchObj.CityId == 0 && searchObj.OrganizationId > 0 && searchObj.SubOrganizationId > 0 && searchObj.HospitalId == 0)
                {
                    lstScrap = lstScrap.Where(a => a.SubOrganizationId == searchObj.SubOrganizationId).AsQueryable();
                }
                else if (searchObj.GovernorateId > 0 && searchObj.CityId > 0 && searchObj.OrganizationId > 0 && searchObj.SubOrganizationId > 0 && searchObj.HospitalId == 0)
                {
                    lstScrap = lstScrap.Where(a => a.SubOrganizationId == searchObj.SubOrganizationId).AsQueryable();
                }
                else if (searchObj.GovernorateId > 0 && searchObj.CityId > 0 && searchObj.OrganizationId > 0 && searchObj.SubOrganizationId == 0 && searchObj.HospitalId == 0)
                {
                    lstScrap = lstScrap.Where(a => a.OrganizationId == searchObj.OrganizationId).AsQueryable();
                }
            }



            list = lstScrap.ToList();


            if (searchObj.ModelNumber != "")
            {
                list = list.Where(b => b.Model.Contains(searchObj.ModelNumber)).ToList();
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

            string setstartday, setstartmonth, setendday, setendmonth = "";
            DateTime startingFrom = new DateTime();
            DateTime endingTo = new DateTime();
            if (searchObj.StartDate2 == "")
            {
                startingFrom = DateTime.Parse("1900-01-01");
            }
            else
            {
                searchObj.StartingDate = DateTime.Parse(searchObj.StartDate2.ToString());
                var startyear = searchObj.StartingDate.Value.Year;
                var startmonth = searchObj.StartingDate.Value.Month;
                var startday = searchObj.StartingDate.Value.Day;
                if (startday < 10)
                    setstartday = searchObj.StartingDate.Value.Day.ToString().PadLeft(2, '0');
                else
                    setstartday = searchObj.StartingDate.Value.Day.ToString();
                if (startmonth < 10)
                    setstartmonth = searchObj.StartingDate.Value.Month.ToString().PadLeft(2, '0');
                else
                    setstartmonth = searchObj.StartingDate.Value.Month.ToString();
                var sDate = startyear + "/" + setstartmonth + "/" + setstartday;
                startingFrom = DateTime.Parse(sDate);
            }
            if (searchObj.EndDate2 == "")
            {
                endingTo = DateTime.Today.Date;
            }
            else
            {
                searchObj.EndingDate = DateTime.Parse(searchObj.EndDate2.ToString());
                var endyear = searchObj.EndingDate.Value.Year;
                var endmonth = searchObj.EndingDate.Value.Month;
                var endday = searchObj.EndingDate.Value.Day;
                if (endday < 10)
                    setendday = searchObj.EndingDate.Value.Day.ToString().PadLeft(2, '0');
                else
                    setendday = searchObj.EndingDate.Value.Day.ToString();
                if (endmonth < 10)
                    setendmonth = searchObj.EndingDate.Value.Month.ToString().PadLeft(2, '0');
                else
                    setendmonth = searchObj.EndingDate.Value.Month.ToString();
                var eDate = endyear + "/" + setendmonth + "/" + setendday;
                endingTo = DateTime.Parse(eDate);
            }
            if (searchObj.StartDate2 != "" && searchObj.EndDate2 != "")
            {
                list = list.Where(a => a.SortScrapDate != null && a.SortScrapDate.Value.Date >= startingFrom.Date && a.SortScrapDate.Value.Date <= endingTo.Date).ToList();
            }


            var requestsPerPage = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            mainClass.Results = requestsPerPage;
            mainClass.Count = list.Count();
            return mainClass;
        }
















        public IEnumerable<IndexScrapVM.GetData> SortScraps(SortScrapVM sortObj, int statusId)
        {
            throw new NotImplementedException();
        }
        public List<IndexScrapVM.GetData> PrintListOfScraps(List<IndexScrapVM.GetData> scraps)
        {
            List<IndexScrapVM.GetData> list = new List<IndexScrapVM.GetData>();
            var lstScraps = _context.Scraps.Include(a => a.AssetDetail)
                .Include(a => a.AssetDetail.MasterAsset)
                .Include(a => a.AssetDetail.Department)
                .Include(a => a.AssetDetail.MasterAsset.brand)
               .ToList();
            foreach (var item in lstScraps)
            {
                IndexScrapVM.GetData scrapObj = new IndexScrapVM.GetData();
                scrapObj.Id = item.Id;
                scrapObj.AssetName = item.AssetDetail.MasterAsset != null ? item.AssetDetail.MasterAsset.Name : "";
                scrapObj.AssetNameAr = item.AssetDetail.MasterAsset != null ? item.AssetDetail.MasterAsset.NameAr : "";
                scrapObj.Barcode = item.AssetDetail.Barcode;
                scrapObj.BarCode = item.AssetDetail.Barcode;
                scrapObj.Model = item.AssetDetail.MasterAsset.ModelNumber;
                scrapObj.SerialNumber = item.AssetDetail.SerialNumber;
                scrapObj.ScrapDate = item.ScrapDate.ToString();
                scrapObj.ScrapNo = item.ScrapNo;
                scrapObj.DepartmentName = item.AssetDetail.Department.Name;
                scrapObj.DepartmentNameAr = item.AssetDetail.Department.NameAr;
                if (item.AssetDetail.MasterAsset.brand != null)
                {
                    scrapObj.BrandId = item.AssetDetail.MasterAsset.BrandId;
                    scrapObj.BrandName = item.AssetDetail.MasterAsset.brand.Name;
                    scrapObj.BrandNameAr = item.AssetDetail.MasterAsset.brand.NameAr;
                }
                scrapObj.Model = item.AssetDetail.MasterAsset.ModelNumber;
                list.Add(scrapObj);
            }
            return list;
        }
    }
}
