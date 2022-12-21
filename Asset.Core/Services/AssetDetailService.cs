﻿
using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.AssetDetailAttachmentVM;
using Asset.ViewModels.AssetDetailVM;
using Asset.ViewModels.BrandVM;
using Asset.ViewModels.CityVM;
using Asset.ViewModels.GovernorateVM;
using Asset.ViewModels.HospitalVM;
using Asset.ViewModels.OrganizationVM;
using Asset.ViewModels.PMAssetTaskScheduleVM;
using Asset.ViewModels.PmAssetTimeVM;
using Asset.ViewModels.SupplierVM;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
    public class AssetDetailService : IAssetDetailService
    {
        private IUnitOfWork _unitOfWork;

        public AssetDetailService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Add(CreateAssetDetailVM assetDetailObj)
        {
            return _unitOfWork.AssetDetailRepository.Add(assetDetailObj);
            //  return _unitOfWork.CommitAsync();

        }

        public int Delete(int id)
        {
            var assetDetailObj = _unitOfWork.AssetDetailRepository.GetById(id);
            _unitOfWork.AssetDetailRepository.Delete(assetDetailObj.Id);
            _unitOfWork.CommitAsync();

            return assetDetailObj.Id;
        }

        public IEnumerable<IndexAssetDetailVM.GetData> GetAll()
        {
            return _unitOfWork.AssetDetailRepository.GetAll();
        }

        public IEnumerable<IndexAssetDetailVM.GetData> GetAssetDetailsByAssetId(int assetId)
        {
            return _unitOfWork.AssetDetailRepository.GetAssetDetailsByAssetId(assetId);
        }

        public async Task<IEnumerable<IndexAssetDetailVM.GetData>> GetAssetDetailsByUserId(string userId)
        {
            return await _unitOfWork.AssetDetailRepository.GetAssetDetailsByUserId(userId);
        }

        public async Task<IndexAssetDetailVM> GetAssetDetailsByUserId2(int pageNumber, int pageSize, string userId)
        {
            return await _unitOfWork.AssetDetailRepository.GetAssetDetailsByUserId2(pageNumber, pageSize, userId);
        }

        public async Task<IEnumerable<IndexAssetDetailVM.GetData>> GetAssetsByUserId(string userId)
        {
            return await _unitOfWork.AssetDetailRepository.GetAssetsByUserId(userId);
        }
        public EditAssetDetailVM GetById(int id)
        {
            return _unitOfWork.AssetDetailRepository.GetById(id);
        }

        public int Update(EditAssetDetailVM assetDetailObj)
        {
            _unitOfWork.AssetDetailRepository.Update(assetDetailObj);
            return assetDetailObj.Id;
        }

        public int DeleteAssetDetailAttachment(int id)
        {
            return _unitOfWork.AssetDetailRepository.DeleteAssetDetailAttachment(id);
        }

        public IEnumerable<AssetDetailAttachment> GetAttachmentByAssetDetailId(int assetId)
        {
            return _unitOfWork.AssetDetailRepository.GetAttachmentByAssetDetailId(assetId);
        }

        public int CreateAssetDetailDocuments(CreateAssetDetailAttachmentVM attachObj)
        {
            return _unitOfWork.AssetDetailRepository.CreateAssetDetailDocuments(attachObj);
        }

        public ViewAssetDetailVM ViewAssetDetailByMasterId(int masterId)
        {
            return _unitOfWork.AssetDetailRepository.ViewAssetDetailByMasterId(masterId);

        }

        public IndexAssetDetailVM SearchAssetInHospital(int pagenumber, int pagesize, SearchMasterAssetVM searchObj)
        {
            return _unitOfWork.AssetDetailRepository.SearchAssetInHospital(pagenumber, pagesize, searchObj);
        }
        public IEnumerable<IndexAssetDetailVM.GetData> SearchAssetInHospitalByHospitalId(SearchMasterAssetVM searchObj)
        {
            return _unitOfWork.AssetDetailRepository.SearchAssetInHospitalByHospitalId(searchObj);
        }

        public IEnumerable<IndexPMAssetTaskScheduleVM.GetData> GetAllPMAssetTaskSchedules(int? hospitalId)
        {
            return _unitOfWork.AssetDetailRepository.GetAllPMAssetTaskSchedules(hospitalId);
        }


        public IEnumerable<AssetDetail> ViewAllAssetDetailByMasterId(int MasterAssetId)
        {
            return _unitOfWork.AssetDetailRepository.ViewAllAssetDetailByMasterId(MasterAssetId);
        }

        public IEnumerable<AssetDetail> GetAllSerialsByMasterAssetIdAndHospitalId(int masterAssetId, int hospitalId)
        {
            return _unitOfWork.AssetDetailRepository.GetAllSerialsByMasterAssetIdAndHospitalId(masterAssetId, hospitalId);
        }

        public IEnumerable<AssetDetail> GetAllAssetDetailsByHospitalId(int hospitalId)
        {
            return _unitOfWork.AssetDetailRepository.GetAllAssetDetailsByHospitalId(hospitalId);

        }

        public IEnumerable<ViewAssetDetailVM> GetListOfAssetDetailsByHospitalId(int hospitalId)
        {
            return _unitOfWork.AssetDetailRepository.GetListOfAssetDetailsByHospitalId(hospitalId);
        }

        public List<CountAssetVM> CountAssetsByHospital()
        {
            return _unitOfWork.AssetDetailRepository.CountAssetsByHospital();
        }

        public List<PmDateGroupVM> GetAllwithgrouping(int assetId)
        {
            return _unitOfWork.AssetDetailRepository.GetAllwithgrouping(assetId);
        }
        public List<IndexAssetDetailVM.GetData> FilterAsset(filterDto data)
        {
            return _unitOfWork.AssetDetailRepository.FilterAsset(data);
        }

        public List<BrandGroupVM> GetAssetByBrands(List<IndexAssetDetailVM.GetData> AssetModel)
        {
            return _unitOfWork.AssetDetailRepository.GetAssetByBrands(AssetModel);
        }

        public List<GroupHospitalVM> GetAssetByHospital(List<IndexAssetDetailVM.GetData> AssetModel)
        {
            return _unitOfWork.AssetDetailRepository.GetAssetByHospital(AssetModel);
        }

        public List<GroupGovernorateVM> GetAssetByGovernorate(List<IndexAssetDetailVM.GetData> AssetModel)
        {
            return _unitOfWork.AssetDetailRepository.GetAssetByGovernorate(AssetModel);
        }
        public List<GroupCityVM> GetAssetByCity(List<IndexAssetDetailVM.GetData> AssetModel)
        {
            return _unitOfWork.AssetDetailRepository.GetAssetByCity(AssetModel);
        }
        public List<GroupSupplierVM> GetAssetBySupplier(List<IndexAssetDetailVM.GetData> AssetModel)
        {
            return _unitOfWork.AssetDetailRepository.GetAssetBySupplier(AssetModel);
        }
        public List<GroupOrganizationVM> GetAssetByOrganization(List<IndexAssetDetailVM.GetData> AssetModel)
        {
            return _unitOfWork.AssetDetailRepository.GetAssetByOrganization(AssetModel);
        }

        public IEnumerable<IndexAssetDetailVM.GetData> SortAssets(Sort sortObj)
        {
            return _unitOfWork.AssetDetailRepository.SortAssets(sortObj);
        }
        //public IndexAssetDetailVM SortAssets(Sort sortObj, int hospitalId, int statusId, string userId, int pageNumber, int pageSize)
        //{
        //    return _unitOfWork.AssetDetailRepository.SortAssets(sortObj, hospitalId, statusId, userId, pageNumber, pageSize);
        //}


        public IndexAssetDetailVM SortAssets(Sort sortObj,int statusId, string userId)
        {
            return _unitOfWork.AssetDetailRepository.SortAssets(sortObj,  statusId, userId);
        }


        public List<HospitalAssetAge> GetAssetsByAgeGroup(int hospitalId)
        {
            return _unitOfWork.AssetDetailRepository.GetAssetsByAgeGroup(hospitalId);
        }

        public List<HospitalAssetAge> GetGeneralAssetsByAgeGroup(FilterHospitalAssetAge model)
        {
            return _unitOfWork.AssetDetailRepository.GetGeneralAssetsByAgeGroup(model);
        }

        public IEnumerable<IndexAssetDetailVM.GetData> AutoCompleteAssetBarCode(string barcode, int hospitalId)
        {
            return _unitOfWork.AssetDetailRepository.AutoCompleteAssetBarCode(barcode, hospitalId);
        }

        public IEnumerable<IndexAssetDetailVM.GetData> GetAllAssetsByStatusId(int statusId, string userId)
        {
            return _unitOfWork.AssetDetailRepository.GetAllAssetsByStatusId(statusId, userId);

        }

        public IEnumerable<IndexAssetDetailVM.GetData> AutoCompleteAssetSerial(string serial, int hospitalId)
        {
            return _unitOfWork.AssetDetailRepository.AutoCompleteAssetSerial(serial, hospitalId);
        }

        public IEnumerable<ViewAssetDetailVM> GetListOfAssetDetailsByHospitalNotInContract(int hospitalId)
        {
            return _unitOfWork.AssetDetailRepository.GetListOfAssetDetailsByHospitalNotInContract(hospitalId);
        }

        public IEnumerable<ViewAssetDetailVM> GetNoneExcludedAssetsByHospitalId(int hospitalId)
        {
            return _unitOfWork.AssetDetailRepository.GetNoneExcludedAssetsByHospitalId(hospitalId);
        }

        public IEnumerable<ViewAssetDetailVM> GetSupplierNoneExcludedAssetsByHospitalId(int hospitalId)
        {
            return _unitOfWork.AssetDetailRepository.GetSupplierNoneExcludedAssetsByHospitalId(hospitalId);
        }

        public IEnumerable<IndexAssetDetailVM.GetData> GetHospitalAssets(int hospitalId, int statusId, string userId, int page, int pageSize, Sort sortObj)
        {
            return _unitOfWork.AssetDetailRepository.GetHospitalAssets(hospitalId, statusId, userId, page, pageSize, sortObj);
        }

        public AssetDetailAttachment GetLastDocumentForAssetDetailId(int assetDetailId)
        {
            return _unitOfWork.AssetDetailRepository.GetLastDocumentForAssetDetailId(assetDetailId);
        }

        public IEnumerable<ViewAssetDetailVM> GetAutoCompleteSupplierNoneExcludedAssetsByHospitalId(string barcode, int hospitalId)
        {
            return _unitOfWork.AssetDetailRepository.GetAutoCompleteSupplierNoneExcludedAssetsByHospitalId(barcode, hospitalId);
        }

        public int CountAssetsByHospitalId(int hospitalId)
        {
            return _unitOfWork.AssetDetailRepository.CountAssetsByHospitalId(hospitalId);
        }

        public List<CountAssetVM> ListTopAssetsByHospitalId(int hospitalId)
        {
            return _unitOfWork.AssetDetailRepository.ListTopAssetsByHospitalId(hospitalId);
        }

        public List<CountAssetVM> ListAssetsByGovernorateIds()
        {
            return _unitOfWork.AssetDetailRepository.ListAssetsByGovernorateIds();
        }

        public List<CountAssetVM> ListAssetsByCityIds()
        {
            return _unitOfWork.AssetDetailRepository.ListAssetsByCityIds();
        }

        public List<CountAssetVM> CountAssetsInHospitalByHospitalId(int hospitalId)
        {
            return _unitOfWork.AssetDetailRepository.CountAssetsInHospitalByHospitalId(hospitalId);
        }

        public IEnumerable<IndexAssetDetailVM.GetData> AlertAssetsBefore3Monthes()
        {
            return _unitOfWork.AssetDetailRepository.AlertAssetsBefore3Monthes();
        }

        public IndexAssetDetailVM GetAllAssetsByStatusId(int pageNumber, int pageSize, int statusId, string userId)
        {
            return _unitOfWork.AssetDetailRepository.GetAllAssetsByStatusId(pageNumber, pageSize, statusId, userId);
        }

        public IEnumerable<IndexAssetDetailVM.GetData> AlertAssetsBefore3Monthes(int duration)
        {
            return _unitOfWork.AssetDetailRepository.AlertAssetsBefore3Monthes(duration);
        }

        public IndexAssetDetailVM SearchHospitalAssetsByDepartmentId(int departmentId, string userId, int pageNumber, int pageSize)
        {
            return _unitOfWork.AssetDetailRepository.SearchHospitalAssetsByDepartmentId(departmentId, userId, pageNumber, pageSize);
        }

        public ViewAssetDetailVM GetAssetHistoryById(int assetId)
        {
            return _unitOfWork.AssetDetailRepository.GetAssetHistoryById(assetId);
        }

        public List<IndexAssetDetailVM.GetData> FilterDataByDepartmentBrandSupplierId(FilterHospitalAsset data)
        {
            return _unitOfWork.AssetDetailRepository.FilterDataByDepartmentBrandSupplierId(data);
        }

        public List<DepartmentGroupVM> GetAssetByDepartment(List<IndexAssetDetailVM.GetData> AssetModel)
        {
            return _unitOfWork.AssetDetailRepository.GetAssetByDepartment(AssetModel);
        }

        public IndexAssetDetailVM GetAssetsByUserId(string userId, int pageNumber, int pageSize)
        {
            return _unitOfWork.AssetDetailRepository.GetAssetsByUserId(userId, pageNumber, pageSize);
        }

        public IEnumerable<ViewAssetDetailVM> GetListOfAssetDetailsByHospitalNotInContract(string barcode, int hospitalId)
        {
            return _unitOfWork.AssetDetailRepository.GetListOfAssetDetailsByHospitalNotInContract(barcode, hospitalId);
        }

        public IEnumerable<ViewAssetDetailVM> GetListOfAssetDetailsByHospitalNotInContractBySerialNumber(string serialNumber, int hospitalId)
        {
            return _unitOfWork.AssetDetailRepository.GetListOfAssetDetailsByHospitalNotInContractBySerialNumber(serialNumber, hospitalId);
        }

        public GeneratedAssetDetailBCVM GenerateAssetDetailBarcode()
        {
            return _unitOfWork.AssetDetailRepository.GenerateAssetDetailBarcode();
        }

        public IEnumerable<IndexPMAssetTaskScheduleVM.GetData> GetAllPMAssetTaskScheduleByAssetId(int? assetId)
        {
            return _unitOfWork.AssetDetailRepository.GetAllPMAssetTaskScheduleByAssetId(assetId);
            
        }

        public MobileAssetDetailVM GetAssetDetailById(string userId, int assetId)
        {
            return _unitOfWork.AssetDetailRepository.GetAssetDetailById(userId, assetId);
        }

        public bool GenerateQrCodeForAllAssets(string domainName)
        {
            return _unitOfWork.AssetDetailRepository.GenerateQrCodeForAllAssets(domainName);
        }
    }
}