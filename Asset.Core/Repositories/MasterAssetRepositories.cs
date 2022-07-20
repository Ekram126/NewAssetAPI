using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.MasterAssetAttachmentVM;
using Asset.ViewModels.MasterAssetVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class MasterAssetRepositories : IMasterAssetRepository
    {

        private ApplicationDbContext _context;


        public MasterAssetRepositories(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Add(CreateMasterAssetVM model)
        {
            MasterAsset masterAssetObj = new MasterAsset();
            try
            {
                if (model != null)
                {
                    masterAssetObj.Code = model.Code;
                    masterAssetObj.Name = model.Name;
                    masterAssetObj.NameAr = model.NameAr;
                    masterAssetObj.BrandId = model.BrandId;
                    masterAssetObj.CategoryId = model.CategoryId;
                    masterAssetObj.SubCategoryId = model.SubCategoryId;
                    masterAssetObj.Description = model.Description;
                    masterAssetObj.DescriptionAr = model.DescriptionAr;
                    masterAssetObj.ExpectedLifeTime = model.ExpectedLifeTime;
                    masterAssetObj.Height = model.Height;
                    masterAssetObj.Length = model.Length;
                    masterAssetObj.ModelNumber = model.ModelNumber;
                    masterAssetObj.VersionNumber = model.VersionNumber;
                    masterAssetObj.Weight = model.Weight;
                    masterAssetObj.Width = model.Width;
                    masterAssetObj.ECRIId = model.ECRIId;
                    masterAssetObj.PeriorityId = model.PeriorityId;
                    masterAssetObj.OriginId = model.OriginId;
                    masterAssetObj.Power = model.Power;
                    masterAssetObj.Voltage = model.Voltage;
                    masterAssetObj.Ampair = model.Ampair;
                    masterAssetObj.Frequency = model.Frequency;
                    masterAssetObj.ElectricRequirement = model.ElectricRequirement;
                    masterAssetObj.PMTimeId = model.PMTimeId;
                    masterAssetObj.AssetImg = model.AssetImg;
       masterAssetObj.HospitalId = model.HospitalId;

                    _context.MasterAssets.Add(masterAssetObj);
                    _context.SaveChanges();
                    return masterAssetObj.Id;
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return 0;
        }

        public int CountMasterAssets()
        {
            return _context.MasterAssets.Count();
        }

        public List<CountMasterAssetBrands> CountMasterAssetsByBrand(int hospitalId)
        {
            List<CountMasterAssetBrands> list = new List<CountMasterAssetBrands>();
            var lstBrands = _context.Brands.ToList().Take(10);
            if (hospitalId != 0)
            {
                foreach (var item in lstBrands)
                {
                    CountMasterAssetBrands countHospitalObj = new CountMasterAssetBrands();
                    countHospitalObj.BrandName = item.Name;
                    countHospitalObj.BrandNameAr = item.NameAr;
                    countHospitalObj.CountOfMasterAssets = _context.AssetDetails.Include(a => a.MasterAsset)
                        .Where(a => a.MasterAsset.BrandId == item.Id && a.HospitalId == hospitalId).ToList().Count();
                    list.Add(countHospitalObj);
                }
            }
            else
            {
                foreach (var item in lstBrands)
                {
                    CountMasterAssetBrands countHospitalObj = new CountMasterAssetBrands();
                    countHospitalObj.BrandName = item.Name;
                    countHospitalObj.BrandNameAr = item.NameAr;
                    countHospitalObj.CountOfMasterAssets = _context.MasterAssets.Where(a => a.BrandId == item.Id).ToList().Count();
                    list.Add(countHospitalObj);
                }
            }
            return list;
        }

        public List<CountMasterAssetSuppliers> CountMasterAssetsBySupplier(int hospitalId)
        {
            List<CountMasterAssetSuppliers> list = new List<CountMasterAssetSuppliers>();
            var lstSuppliers = _context.Suppliers.ToList().Take(10);
            if (hospitalId != 0)
            {
                foreach (var item in lstSuppliers)
                {
                    CountMasterAssetSuppliers countHospitalObj = new CountMasterAssetSuppliers();
                    countHospitalObj.SupplierName = item.Name;
                    countHospitalObj.SupplierNameAr = item.NameAr;
                    countHospitalObj.CountOfMasterAssets = _context.AssetDetails.Where(a => a.SupplierId == item.Id && a.HospitalId == hospitalId).ToList().GroupBy(a => a.SupplierId).Count();
                    list.Add(countHospitalObj);
                }
            }
            else
            {
                foreach (var item in lstSuppliers)
                {
                    CountMasterAssetSuppliers countHospitalObj = new CountMasterAssetSuppliers();
                    countHospitalObj.SupplierName = item.Name;
                    countHospitalObj.SupplierNameAr = item.NameAr;
                    countHospitalObj.CountOfMasterAssets = _context.AssetDetails.Where(a => a.SupplierId == item.Id).ToList().GroupBy(a => a.SupplierId).Count();
                    list.Add(countHospitalObj);
                }
            }
            return list;

        }

        public int CreateMasterAssetDocuments(CreateMasterAssetAttachmentVM attachObj)
        {
            MasterAssetAttachment MasterAssetAttachmentObj = new MasterAssetAttachment();
            MasterAssetAttachmentObj.MasterAssetId = attachObj.MasterAssetId;
            MasterAssetAttachmentObj.Title = attachObj.Title;
            MasterAssetAttachmentObj.FileName = attachObj.FileName;
            _context.MasterAssetAttachments.Add(MasterAssetAttachmentObj);
            _context.SaveChanges();
            int Id = MasterAssetAttachmentObj.Id;
            return Id;
        }

        public int Delete(int id)
        {
            var masterAssetObj = _context.MasterAssets.Find(id);
            try
            {
                if (masterAssetObj != null)
                {
                    _context.MasterAssets.Remove(masterAssetObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return 0;
        }

        public int DeleteMasterAssetAttachment(int id)
        {
            try
            {
                var attachObj = _context.MasterAssetAttachments.Find(id);
                _context.MasterAssetAttachments.Remove(attachObj);
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return 0;
        }

        public IEnumerable<IndexMasterAssetVM.GetData> GetAll()
        {
            List<IndexMasterAssetVM.GetData> list = new List<IndexMasterAssetVM.GetData>();
            var lstMasters = _context.MasterAssets.Include(a => a.brand).Include(a => a.Category)

                .Include(a => a.SubCategory).Include(a => a.ECRIS).Include(a => a.Origin).OrderBy(a => a.Name).ToList();

            foreach (var item in lstMasters)
            {
                IndexMasterAssetVM.GetData getDataObj = new IndexMasterAssetVM.GetData();
                getDataObj.Id = item.Id;
                getDataObj.Code = item.Code;
                getDataObj.Model = item.ModelNumber;
                getDataObj.CategoryId = item.CategoryId;
                getDataObj.SubCategoryId = item.SubCategoryId;
                getDataObj.PMColor = item.PMColor;
                getDataObj.PMBGColor = item.PMBGColor;
                getDataObj.ECRIName = item.ECRIId != null ? item.ECRIS.Name : "";
                getDataObj.ECRINameAr = item.ECRIId != null ? item.ECRIS.NameAr : "";
                getDataObj.Name = item.Name;
                getDataObj.NameAr = item.NameAr;
                getDataObj.OriginName = item.OriginId != null ? item.Origin.Name : "";
                getDataObj.OriginNameAr = item.OriginId != null ? item.Origin.NameAr : "";
                getDataObj.BrandName = item.brand != null ? item.brand.Name : "";
                getDataObj.BrandNameAr = item.brand != null ? item.brand.NameAr : "";
                list.Add(getDataObj);
            }
            return list;
        }

        public IEnumerable<MasterAsset> GetAllMasterAssets()
        {
            return _context.MasterAssets.ToList();
        }

        public IEnumerable<MasterAsset> GetAllMasterAssetsByHospitalId(int hospitalId, string userId)
        {
            List<MasterAsset> list = new List<MasterAsset>();
            ApplicationUser UserObj = new ApplicationUser();
            int employeeId = 0;

            var obj = _context.ApplicationUser.Where(a => a.Id == userId).ToList();
            if (obj.Count > 0)
            {
                UserObj = obj[0];
                var lstEmployees = _context.Employees.Where(a => a.Email == UserObj.Email).ToList();
                if (lstEmployees.Count > 0)
                {
                    var employeeObj = lstEmployees[0];
                    employeeId = employeeObj.Id;
                }
            }
            var lstMasterAssets = (from asset in _context.MasterAssets
                                   join detail in _context.AssetDetails on asset.Id equals detail.MasterAssetId
                                   join owner in _context.AssetOwners on detail.Id equals owner.AssetDetailId
                                   where detail.HospitalId == hospitalId
                                  where owner.EmployeeId == employeeId
                                   select asset).ToList().GroupBy(a => a.Id).ToList();

            foreach (var item in lstMasterAssets)
            {
                MasterAsset masterAssetObj = new MasterAsset();
                masterAssetObj.Id = item.FirstOrDefault().Id;
                masterAssetObj.Name = item.FirstOrDefault().Name;
                masterAssetObj.NameAr = item.FirstOrDefault().NameAr;
                list.Add(masterAssetObj);
            }
            return list;
        }

        public IEnumerable<MasterAsset> GetAllMasterAssetsByHospitalId(int hospitalId)
        {
            List<MasterAsset> list = new List<MasterAsset>();
            var lstMasterAssets = _context.AssetDetails.Include(a => a.MasterAsset)
                                   .Where(a => a.HospitalId == hospitalId).ToList().GroupBy(a => a.MasterAsset.Id).ToList();


            foreach (var item in lstMasterAssets)
            {
                MasterAsset masterAssetObj = new MasterAsset();
                masterAssetObj.Id = item.FirstOrDefault().MasterAsset.Id;
                masterAssetObj.Name = item.FirstOrDefault().MasterAsset.Name;
                masterAssetObj.NameAr = item.FirstOrDefault().MasterAsset.NameAr;
                list.Add(masterAssetObj);
            }
            return list;
        }


        public IEnumerable<MasterAssetAttachment> GetAttachmentByMasterAssetId(int assetId)
        {
            return _context.MasterAssetAttachments.Where(a => a.MasterAssetId == assetId).ToList();
        }

        public EditMasterAssetVM GetById(int id)
        {
            var lstMasterAssets = _context.MasterAssets.Where(a => a.Id == id).ToList();

            if (lstMasterAssets.Count > 0)
            {
                MasterAsset item = lstMasterAssets[0];
                EditMasterAssetVM masterAssetObj = new EditMasterAssetVM();
                masterAssetObj.Id = item.Id;
                masterAssetObj.Name = item.Name;
                masterAssetObj.NameAr = item.NameAr;
                masterAssetObj.Code = item.Code;
                masterAssetObj.ECRIId = item.ECRIId != null ? (int)item.ECRIId : null;
                masterAssetObj.BrandId = item.brand != null ? item.BrandId : null;
                masterAssetObj.CategoryId = item.CategoryId != null ? item.CategoryId : null;
                masterAssetObj.SubCategoryId = item.SubCategoryId != null ? item.SubCategoryId : null;
                masterAssetObj.Description = item.Description;
                masterAssetObj.DescriptionAr = item.DescriptionAr;
                masterAssetObj.ExpectedLifeTime = item.ExpectedLifeTime != null ? (int)item.ExpectedLifeTime : 0;
                masterAssetObj.Height = item.Height;
                masterAssetObj.Length = item.Length;
                masterAssetObj.ModelNumber = item.ModelNumber;
                masterAssetObj.VersionNumber = item.VersionNumber;
                masterAssetObj.Weight = item.Weight;
                masterAssetObj.Width = item.Width;
                masterAssetObj.PeriorityId = item.PeriorityId;
                masterAssetObj.OriginId = item.OriginId != null ? item.OriginId : null;
                masterAssetObj.Power = item.Power;
                masterAssetObj.Voltage = item.Voltage;
                masterAssetObj.Ampair = item.Ampair;
                masterAssetObj.Frequency = item.Frequency;
                masterAssetObj.ElectricRequirement = item.ElectricRequirement;
                masterAssetObj.PMTimeId = item.PMTimeId;
                masterAssetObj.AssetImg = item.AssetImg;
                masterAssetObj.HospitalId = item.HospitalId;

                return masterAssetObj;
            }

            return null;
        }

        public int Update(EditMasterAssetVM model)
        {
            try
            {

                var masterAssetObj = _context.MasterAssets.Find(model.Id);
                masterAssetObj.Id = model.Id;
                masterAssetObj.Code = model.Code;
                masterAssetObj.Name = model.Name;
                masterAssetObj.NameAr = model.NameAr;
                masterAssetObj.BrandId = model.BrandId;
                masterAssetObj.CategoryId = model.CategoryId;
                masterAssetObj.SubCategoryId = model.SubCategoryId;
                masterAssetObj.Description = model.Description;
                masterAssetObj.DescriptionAr = model.DescriptionAr;
                masterAssetObj.ExpectedLifeTime = model.ExpectedLifeTime;
                masterAssetObj.Height = model.Height;
                masterAssetObj.Length = model.Length;
                masterAssetObj.ModelNumber = model.ModelNumber;
                masterAssetObj.VersionNumber = model.VersionNumber;
                masterAssetObj.Weight = model.Weight;
                masterAssetObj.Width = model.Width;
                masterAssetObj.ECRIId = model.ECRIId;
                masterAssetObj.PeriorityId = model.PeriorityId;
                masterAssetObj.OriginId = model.OriginId;
                masterAssetObj.Power = model.Power;
                masterAssetObj.Voltage = model.Voltage;
                masterAssetObj.Ampair = model.Ampair;
                masterAssetObj.Frequency = model.Frequency;
                masterAssetObj.ElectricRequirement = model.ElectricRequirement;
                masterAssetObj.PMTimeId = model.PMTimeId;
                masterAssetObj.AssetImg = model.AssetImg;
                masterAssetObj.HospitalId = model.HospitalId;

                _context.Entry(masterAssetObj).State = EntityState.Modified;
                _context.SaveChanges();
                return masterAssetObj.Id;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return 0;
        }

        public ViewMasterAssetVM ViewMasterAsset(int id)
        {
            ViewMasterAssetVM model = new ViewMasterAssetVM();
            var masterAssetObj = _context.MasterAssets.Find(id);
            model.Id = masterAssetObj.Id;
            model.Code = masterAssetObj.Code;
            model.Name = masterAssetObj.Name;
            model.NameAr = masterAssetObj.NameAr;
            model.Description = masterAssetObj.Description;
            model.DescriptionAr = masterAssetObj.DescriptionAr;
            model.ExpectedLifeTime = masterAssetObj.ExpectedLifeTime != null ? (int)masterAssetObj.ExpectedLifeTime : 0;
            model.Height = masterAssetObj.Height;
            model.Length = masterAssetObj.Length;
            model.ModelNumber = masterAssetObj.ModelNumber;
            model.VersionNumber = masterAssetObj.VersionNumber;
            model.Weight = masterAssetObj.Weight;
            model.Width = masterAssetObj.Width;

            model.Power = masterAssetObj.Power;
            model.Voltage = masterAssetObj.Voltage;
            model.Ampair = masterAssetObj.Ampair;
            model.Frequency = masterAssetObj.Frequency;
            model.ElectricRequirement = masterAssetObj.ElectricRequirement;
            //  model.PMTimeId = masterAssetObj.PMTimeId;
            model.AssetImg = masterAssetObj.AssetImg;

            model.HospitalId = masterAssetObj.HospitalId;



            var lstECRIs = _context.ECRIS.Where(a => a.Id == masterAssetObj.ECRIId).ToList();
            if (lstECRIs.Count > 0)
            {
                model.ECRIId = lstECRIs[0].Id;
                model.ECRIName = lstECRIs[0].Name;
                model.ECRINameAr = lstECRIs[0].NameAr;
            }
            var lstBrands = _context.Brands.Where(a => a.Id == masterAssetObj.BrandId).ToList();
            if (lstBrands.Count > 0)
            {
                model.BrandId = lstBrands[0].Id;
                model.BrandName = lstBrands[0].Name;
                model.BrandNameAr = lstBrands[0].NameAr;
            }

            var lstPeriorities = _context.AssetPeriorities.Where(a => a.Id == masterAssetObj.PeriorityId).ToList();
            if (lstPeriorities.Count > 0)
            {
                model.PeriorityId = lstPeriorities[0].Id;
                model.PeriorityName = lstPeriorities[0].Name;
                model.PeriorityNameAr = lstPeriorities[0].NameAr;
            }

            var lstOrigins = _context.AssetPeriorities.Where(a => a.Id == masterAssetObj.OriginId).ToList();
            if (lstOrigins.Count > 0)
            {
                model.OriginId = lstOrigins[0].Id;
                model.OriginName = lstOrigins[0].Name;
                model.OriginNameAr = lstOrigins[0].NameAr;
            }

            var lstCategories = _context.Categories.Where(a => a.Id == masterAssetObj.CategoryId).ToList();
            if (lstCategories.Count > 0)
            {
                model.CategoryId = lstCategories[0].Id;
                model.CategoryName = lstCategories[0].Name;
                model.CategoryNameAr = lstCategories[0].NameAr;
            }


            var lstSubCategories = _context.SubCategories.Where(a => a.Id == masterAssetObj.SubCategoryId).ToList();
            if (lstSubCategories.Count > 0)
            {
                model.SubCategoryId = lstSubCategories[0].Id;
                model.SubCategoryName = lstSubCategories[0].Name;
                model.SubCategoryNameAr = lstSubCategories[0].NameAr;
            }






            return model;
        }


        public IEnumerable<IndexMasterAssetVM.GetData> SearchInMasterAssets(SearchMasterAssetVM searchObj)
        {
            List<IndexMasterAssetVM.GetData> lstData = new List<IndexMasterAssetVM.GetData>();

            var list = _context.MasterAssets.Include(a => a.brand)
                .Include(a => a.Category)
                .Include(a => a.SubCategory)
                .Include(a => a.Origin)
                .Include(a => a.ECRIS).ToList();

            foreach (var item in list)
            {
                IndexMasterAssetVM.GetData getDataObj = new IndexMasterAssetVM.GetData();
                getDataObj.Id = item.Id;
                getDataObj.Code = item.Code;
                getDataObj.Name = item.Name;
                getDataObj.NameAr = item.NameAr;
                getDataObj.AssetName = item.Name;
                getDataObj.AssetNameAr = item.NameAr;
                getDataObj.Model = item.ModelNumber;
                getDataObj.PMBGColor = item.PMBGColor;
                getDataObj.PMColor = item.PMColor;

                if (item.OriginId != null)
                {
                    getDataObj.OriginId = item.OriginId;
                    getDataObj.OriginName = item.Origin.Name;
                    getDataObj.OriginNameAr = item.Origin.NameAr;
                }
                if (item.BrandId != null)
                {
                    getDataObj.BrandId = item.BrandId;
                    getDataObj.BrandName = item.brand.Name;
                    getDataObj.BrandNameAr = item.brand.NameAr;
                }

                if (item.ECRIId != null)
                {
                    getDataObj.ECRIId = item.ECRIId;
                    getDataObj.ECRIName = item.ECRIS.Name;
                    getDataObj.ECRINameAr = item.ECRIS.NameAr;
                }

                if (item.CategoryId != null)
                {
                    getDataObj.CategoryId = item.CategoryId;
                    getDataObj.CategoryName = item.Category.Name;
                    getDataObj.CategoryNameAr = item.Category.NameAr;
                }

                if (item.SubCategoryId != null)
                {
                    getDataObj.SubCategoryId = item.SubCategoryId;
                    getDataObj.SubCategoryName = item.SubCategory.Name;
                    getDataObj.SubCategoryNameAr = item.SubCategory.NameAr;
                }
                lstData.Add(getDataObj);
            }



            if (searchObj.OriginId != 0)
            {
                lstData = lstData.Where(a => a.OriginId == searchObj.OriginId).ToList();
            }
            else
                lstData = lstData.ToList();

            if (searchObj.ECRIId != 0)
            {
                lstData = lstData.Where(a => a.ECRIId == searchObj.ECRIId).ToList();
            }
            else
                list = list.ToList();

            if (searchObj.BrandId != 0)
            {
                lstData = lstData.Where(a => a.BrandId == searchObj.BrandId).ToList();
            }
            else
                lstData = lstData.ToList();

            if (searchObj.CategoryId != 0)
            {
                lstData = lstData.Where(a => a.CategoryId == searchObj.CategoryId).ToList();
            }
            else
                lstData = lstData.ToList();

            if (searchObj.SubCategoryId != 0)
            {
                lstData = lstData.Where(a => a.SubCategoryId == searchObj.SubCategoryId).ToList();
            }
            else
                lstData = lstData.ToList();

            if (searchObj.AssetName != "")
            {
                lstData = lstData.Where(b => b.AssetName == searchObj.AssetName).ToList();
            }
            if (searchObj.AssetNameAr != "")
            {
                lstData = lstData.Where(b => b.NameAr == searchObj.AssetNameAr).ToList();
            }
            if (searchObj.Code != "")
            {
                lstData = lstData.Where(b => b.Code == searchObj.Code).ToList();
            }
            if (searchObj.ModelNumber != "")
            {
                lstData = lstData.Where(b => b.Model == searchObj.ModelNumber).ToList();
            }

            return lstData;
        }

        public IEnumerable<IndexMasterAssetVM.GetData> sortMasterAssets(SortMasterAssetVM searchObj)
        {
            List<IndexMasterAssetVM.GetData> lstData = new List<IndexMasterAssetVM.GetData>();

            var list = _context.MasterAssets.Include(a => a.brand)
                .Include(a => a.Category)
                .Include(a => a.SubCategory)
                .Include(a => a.Origin)
                .Include(a => a.ECRIS).ToList().Take(20).ToList();

            foreach (var item in list)
            {
                IndexMasterAssetVM.GetData getDataObj = new IndexMasterAssetVM.GetData();
                getDataObj.Id = item.Id;
                getDataObj.Code = item.Code;
                getDataObj.Name = item.Name;
                getDataObj.NameAr = item.NameAr;
                getDataObj.AssetName = item.Name;
                getDataObj.AssetNameAr = item.NameAr;
                getDataObj.Model = item.ModelNumber;
                getDataObj.PMBGColor = item.PMBGColor;
                getDataObj.PMColor = item.PMColor;

                if (item.OriginId != null)
                {
                    getDataObj.OriginId = item.OriginId;
                    getDataObj.OriginName = item.Origin.Name;
                    getDataObj.OriginNameAr = item.Origin.NameAr;
                }
                if (item.brand != null)
                {
                    getDataObj.BrandId = item.BrandId;
                    getDataObj.BrandName = item.brand.Name;
                    getDataObj.BrandNameAr = item.brand.NameAr;
                }

                if (item.ECRIId != null)
                {
                    getDataObj.ECRIId = item.ECRIId;
                    getDataObj.ECRIName = item.ECRIS.Name;
                    getDataObj.ECRINameAr = item.ECRIS.NameAr;
                }

                if (item.CategoryId != null)
                {
                    getDataObj.CategoryId = item.CategoryId;
                    getDataObj.CategoryName = item.Category.Name;
                    getDataObj.CategoryNameAr = item.Category.NameAr;
                }

                if (item.SubCategoryId != null)
                {
                    getDataObj.SubCategoryId = item.SubCategoryId;
                    getDataObj.SubCategoryName = item.SubCategory.Name;
                    getDataObj.SubCategoryNameAr = item.SubCategory.NameAr;
                }
                lstData.Add(getDataObj);
            }
            if (searchObj.AssetName != "")
            {
                if (searchObj.SortStatus == "descending")
                    lstData = lstData.OrderByDescending(d => d.AssetName).ToList();
                else
                    lstData = lstData.OrderBy(d => d.AssetName).ToList();
            }
            else if (searchObj.AssetNameAr != "")
            {
                if (searchObj.SortStatus == "descending")
                    lstData = lstData.OrderByDescending(d => d.AssetNameAr).ToList();
                else
                    lstData = lstData.OrderBy(d => d.AssetNameAr).ToList();
            }
            else if (searchObj.OriginName != "")
            {
                if (searchObj.SortStatus == "descending")
                    lstData = lstData.OrderByDescending(d => d.OriginName).ToList();
                else
                    lstData = lstData.OrderBy(d => d.OriginName).ToList();
            }
            else if (searchObj.OriginNameAr != "")
            {
                if (searchObj.SortStatus == "descending")
                    lstData = lstData.OrderByDescending(d => d.OriginNameAr).ToList();
                else
                    lstData = lstData.OrderBy(d => d.OriginNameAr).ToList();
            }
            else if (searchObj.ECRIName != "")
            {
                if (searchObj.SortStatus == "descending")
                    lstData = lstData.OrderByDescending(d => d.ECRIName).ToList();
                else
                    lstData = lstData.OrderBy(d => d.ECRIName).ToList();
            }
            else if (searchObj.ECRINameAr != "")
            {
                if (searchObj.SortStatus == "descending")
                    lstData = lstData.OrderByDescending(d => d.ECRINameAr).ToList();
                else
                    lstData = lstData.OrderBy(d => d.ECRINameAr).ToList();
            }
            else if (searchObj.BrandName != "")
            {
                if (searchObj.SortStatus == "descending")
                    lstData = lstData.OrderByDescending(d => d.BrandName).ToList();
                else
                    lstData = lstData.OrderBy(d => d.BrandName).ToList();
            }
            else if (searchObj.BrandNameAr != "")
            {
                if (searchObj.SortStatus == "descending")
                    lstData = lstData.OrderByDescending(d => d.BrandNameAr).ToList();
                else
                    lstData = lstData.OrderBy(d => d.BrandNameAr).ToList();
            }
            else if (searchObj.CategoryName != "")
            {
                if (searchObj.SortStatus == "descending")
                    lstData = lstData.OrderByDescending(d => d.CategoryName).ToList();
                else
                    lstData = lstData.OrderBy(d => d.CategoryName).ToList();
            }
            else if (searchObj.CategoryNameAr != "")
            {
                if (searchObj.SortStatus == "descending")
                    lstData = lstData.OrderByDescending(d => d.CategoryNameAr).ToList();
                else
                    lstData = lstData.OrderBy(d => d.CategoryNameAr).ToList();
            }
            else if (searchObj.SubCategoryName != "")
            {
                if (searchObj.SortStatus == "descending")
                    lstData = lstData.OrderByDescending(d => d.SubCategoryName).ToList();
                else
                    lstData = lstData.OrderBy(d => d.SubCategoryName).ToList();
            }
            else if (searchObj.SubCategoryNameAr != "")
            {
                if (searchObj.SortStatus == "descending")
                    lstData = lstData.OrderByDescending(d => d.SubCategoryNameAr).ToList();
                else
                    lstData = lstData.OrderBy(d => d.SubCategoryNameAr).ToList();
            }
            else if (searchObj.AssetName != "")
            {
                if (searchObj.SortStatus == "descending")
                    lstData = lstData.OrderByDescending(d => d.AssetName).ToList();
                else
                    lstData = lstData.OrderBy(d => d.AssetName).ToList();
            }
            else if (searchObj.AssetNameAr != "")
            {
                if (searchObj.SortStatus == "descending")
                    lstData = lstData.OrderByDescending(d => d.AssetNameAr).ToList();
                else
                    lstData = lstData.OrderBy(d => d.AssetNameAr).ToList();
            }
            else if (searchObj.Code != "")
            {
                if (searchObj.SortStatus == "descending")
                    lstData = lstData.OrderByDescending(d => d.Code).ToList();
                else
                    lstData = lstData.OrderBy(d => d.Code).ToList();
            }

            else if (searchObj.ModelNumber != "")
            {
                if (searchObj.SortStatus == "descending")
                    lstData = lstData.OrderByDescending(d => d.Model).ToList();
                else
                    lstData = lstData.OrderBy(d => d.Model).ToList();
            }

            return lstData;
        }

        public IEnumerable<MasterAsset> AutoCompleteMasterAssetName(string name)
        {
            var lst = _context.MasterAssets.Where(a => a.Name.Contains(name) || a.NameAr.Contains(name)).ToList();
            return lst;
        }

        public IEnumerable<IndexMasterAssetVM.GetData> GetTop10MasterAsset(int hospitalId)
        {
            List<IndexMasterAssetVM.GetData> list = new List<IndexMasterAssetVM.GetData>();

            if (hospitalId != 0)
            {
                var lstAssets = _context.AssetDetails
                    .Include(a => a.MasterAsset)
                    .Include(a => a.MasterAsset.brand)
                    .Include(a => a.MasterAsset.ECRIS)
                    .Include(a => a.MasterAsset.Origin)
                    .Where(a => a.HospitalId == hospitalId).ToList().OrderBy(a => a.MasterAsset.Name).ToList();//.GroupBy(a=>a.MasterAsset.Id);
                foreach (var item in lstAssets)
                {
                    IndexMasterAssetVM.GetData getDataObj = new IndexMasterAssetVM.GetData();
                    getDataObj.Id = item.MasterAsset.Id;
                    getDataObj.Code = item.MasterAsset.Code;
                    getDataObj.Model = item.MasterAsset.ModelNumber;
                    getDataObj.PMColor = item.MasterAsset.PMColor;
                    getDataObj.PMBGColor = item.MasterAsset.PMBGColor;
                    getDataObj.ECRIName = item.MasterAsset.ECRIId != null ? item.MasterAsset.ECRIS.Name : "";
                    getDataObj.ECRINameAr = item.MasterAsset.ECRIId != null ? item.MasterAsset.ECRIS.NameAr : "";
                    getDataObj.Name = item.MasterAsset.Name;
                    getDataObj.NameAr = item.MasterAsset.NameAr;
                    getDataObj.OriginName = item.MasterAsset.OriginId != null ? item.MasterAsset.Origin.Name : "";
                    getDataObj.OriginNameAr = item.MasterAsset.OriginId != null ? item.MasterAsset.Origin.NameAr : "";
                    getDataObj.BrandName = item.MasterAsset.BrandId != null ? item.MasterAsset.brand.Name : "";
                    getDataObj.BrandNameAr = item.MasterAsset.BrandId != null ? item.MasterAsset.brand.NameAr : "";
                    list.Add(getDataObj);
                }
            }
            else
            {
                var lstMasters = _context.MasterAssets.Include(a => a.brand).Include(a => a.ECRIS).Include(a => a.Origin).OrderBy(a => a.Name).ToList();
                foreach (var item in lstMasters)
                {
                    IndexMasterAssetVM.GetData getDataObj = new IndexMasterAssetVM.GetData();
                    getDataObj.Id = item.Id;
                    getDataObj.Code = item.Code;
                    getDataObj.Model = item.ModelNumber;
                    getDataObj.PMColor = item.PMColor;
                    getDataObj.PMBGColor = item.PMBGColor;
                    getDataObj.ECRIName = item.ECRIId != null ? item.ECRIS.Name : "";
                    getDataObj.ECRINameAr = item.ECRIId != null ? item.ECRIS.NameAr : "";
                    getDataObj.Name = item.Name;
                    getDataObj.NameAr = item.NameAr;
                    getDataObj.OriginName = item.OriginId != null ? item.Origin.Name : "";
                    getDataObj.OriginNameAr = item.OriginId != null ? item.Origin.NameAr : "";
                    getDataObj.BrandName = item.brand != null ? item.brand.Name : "";
                    getDataObj.BrandNameAr = item.brand != null ? item.brand.NameAr : "";
                    list.Add(getDataObj);
                }
            }
            return list;
        }

        public int UpdateMasterAssetImageAfterInsert(CreateMasterAssetVM masterAssetObj)
        {
            var masterObj = _context.MasterAssets.Find(masterAssetObj.Id);

            masterObj.AssetImg = masterAssetObj.AssetImg;
            _context.Entry(masterObj).State = EntityState.Modified;
            _context.SaveChanges();
            return masterAssetObj.Id;
        }
    }
}
