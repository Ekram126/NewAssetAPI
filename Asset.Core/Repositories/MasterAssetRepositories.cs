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

        public List<CountMasterAssetBrands> CountMasterAssetsByBrand()
        {
            List<CountMasterAssetBrands> list = new List<CountMasterAssetBrands>();
            var lstBrands = _context.Brands.ToList().Take(5);
            foreach (var item in lstBrands)
            {
                CountMasterAssetBrands countHospitalObj = new CountMasterAssetBrands();
                countHospitalObj.BrandName = item.Name;
                countHospitalObj.BrandNameAr = item.NameAr;
                countHospitalObj.CountOfMasterAssets = _context.MasterAssets.Where(a => a.BrandId == item.Id).ToList().Count();
                list.Add(countHospitalObj);
            }
            return list;
        }

        public List<CountMasterAssetSuppliers> CountMasterAssetsBySupplier()
        {
            List<CountMasterAssetSuppliers> list = new List<CountMasterAssetSuppliers>();
            var lstBrands = _context.Suppliers.ToList().Take(5);
            foreach (var item in lstBrands)
            {
                CountMasterAssetSuppliers countHospitalObj = new CountMasterAssetSuppliers();
                countHospitalObj.SupplierName = item.Name;
                countHospitalObj.SupplierNameAr = item.NameAr;
                countHospitalObj.CountOfMasterAssets = _context.AssetDetails.Where(a => a.SupplierId == item.Id).ToList().GroupBy(a => a.SupplierId).Count();
                list.Add(countHospitalObj);
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
                getDataObj.BrandName = item.BrandId != null ? item.brand.Name : "";
                getDataObj.BrandNameAr = item.BrandId != null ? item.brand.NameAr : "";
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
                                   .Where(a => a.HospitalId == hospitalId).GroupBy(a => a.MasterAsset.Id).ToList();


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
            return _context.MasterAssets.Where(a => a.Id == id).Select(item => new EditMasterAssetVM
            {
                Id = item.Id,
                Name = item.Name,
                NameAr = item.NameAr,
                Code = item.Code,
                ECRIId = item.ECRIId != null ? (int)item.ECRIId : null,
                BrandId = item.BrandId != null ? item.BrandId : null,
                CategoryId = item.CategoryId != null ? item.CategoryId : null,
                SubCategoryId = item.SubCategoryId != null ? item.SubCategoryId : null,
                Description = item.Description,
                DescriptionAr = item.DescriptionAr,
                ExpectedLifeTime = item.ExpectedLifeTime != null ? (int)item.ExpectedLifeTime : 0,
                Height = item.Height,
                Length = item.Length,
                ModelNumber = item.ModelNumber,
                VersionNumber = item.VersionNumber,
                Weight = item.Weight,
                Width = item.Width,
                PeriorityId = item.PeriorityId,
                OriginId = item.OriginId != null ? item.OriginId : null,
                Power = item.Power,
                Voltage = item.Voltage,
                Ampair = item.Ampair,
                Frequency = item.Frequency,
                ElectricRequirement = item.ElectricRequirement,
                PMTimeId = item.PMTimeId,
                AssetImg = item.AssetImg
            }).First();
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

            if (searchObj.OriginName != "" || searchObj.OriginNameAr !="")
            {
                lstData = lstData.OrderBy(d => d.OriginName).ThenBy(d=>d.OriginNameAr).ToList();
            }
            else
                lstData = lstData.ToList();

            if (searchObj.ECRIName != "" || searchObj.ECRINameAr != "")
            {
                lstData = lstData.OrderBy(d => d.ECRIName).ThenBy(d => d.ECRINameAr).ToList();
            }
            else
                list = list.ToList();

            if (searchObj.BrandName != "" || searchObj.BrandNameAr != "")
            {
                lstData = lstData.OrderBy(d => d.BrandName).ThenBy(d => d.BrandNameAr).ToList();
            }
            else
                lstData = lstData.ToList();

            if (searchObj.CategoryName != "" || searchObj.CategoryNameAr != "")
            {
                lstData = lstData.OrderBy(d => d.CategoryName).ThenBy(d => d.CategoryNameAr).ToList();
            }
            else
                lstData = lstData.ToList();

            if (searchObj.SubCategoryName != "" || searchObj.SubCategoryNameAr != "")
            {
                lstData = lstData.OrderBy(d => d.SubCategoryName).ThenBy(d => d.SubCategoryNameAr).ToList();
            }
            else
                lstData = lstData.ToList();

            if (searchObj.AssetName != "" || searchObj.AssetNameAr != "")
            {
                lstData = lstData.OrderBy(d => d.AssetName).ThenBy(d => d.AssetNameAr).ToList();
            }
            else
                lstData = lstData.ToList();

            if (searchObj.Code != "")
            {
                lstData = lstData.OrderBy(d => d.Code).ToList();
            }
            else
                lstData = lstData.ToList();

            if (searchObj.ModelNumber != "")
            {
                lstData = lstData.OrderBy(d => d.Model).ToList();
            }
            else
                lstData = lstData.ToList();

            return lstData;
        }
    
    }
}
