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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
    public interface IAssetDetailService
    {
        IEnumerable<IndexAssetDetailVM.GetData> GetAll();
        EditAssetDetailVM GetById(int id);
        int Add(CreateAssetDetailVM assetDetailObj);
        int Update(EditAssetDetailVM assetDetailObj);
        int Delete(int id);
        IEnumerable<IndexAssetDetailVM.GetData> GetAssetDetailsByAssetId(int assetId);
        Task<IEnumerable<IndexAssetDetailVM.GetData>> GetAssetDetailsByUserId(string userId);
        Task<IndexAssetDetailVM> GetAssetDetailsByUserId2(int pageNumber, int pageSize, string userId);
        Task<IEnumerable<IndexAssetDetailVM.GetData>> GetAssetsByUserId(string userId);
        IndexAssetDetailVM GetAssetsByUserId(string userId, int pageNumber, int pageSize);
        IEnumerable<IndexAssetDetailVM.GetData> GetAllAssetsByStatusId(int statusId, string userId);
        IndexAssetDetailVM GetAllAssetsByStatusId(int pageNumber, int pageSize, int statusId, string userId);
        IndexAssetDetailVM SearchHospitalAssetsByDepartmentId(int departmentId, string userId, int pageNumber, int pageSize);
        IEnumerable<AssetDetail> GetAllSerialsByMasterAssetIdAndHospitalId(int masterAssetId, int hospitalId);
        IEnumerable<AssetDetail> GetAllAssetDetailsByHospitalId(int hospitalId);
        IEnumerable<ViewAssetDetailVM> GetListOfAssetDetailsByHospitalId(int hospitalId);
        IEnumerable<ViewAssetDetailVM> GetNoneExcludedAssetsByHospitalId(int hospitalId);
        IEnumerable<ViewAssetDetailVM> GetSupplierNoneExcludedAssetsByHospitalId(int hospitalId);
        IEnumerable<ViewAssetDetailVM> GetAutoCompleteSupplierNoneExcludedAssetsByHospitalId(string barcode, int hospitalId);
        IEnumerable<ViewAssetDetailVM> GetListOfAssetDetailsByHospitalNotInContract(int hospitalId);
        IEnumerable<ViewAssetDetailVM> GetListOfAssetDetailsByHospitalNotInContract(string barcode, int hospitalId);
        IEnumerable<ViewAssetDetailVM> GetListOfAssetDetailsByHospitalNotInContractBySerialNumber(string serialNumber, int hospitalId);
        IEnumerable<IndexAssetDetailVM.GetData> AlertAssetsBefore3Monthes();
        IEnumerable<IndexAssetDetailVM.GetData> AlertAssetsBefore3Monthes(int duration);
        ViewAssetDetailVM ViewAssetDetailByMasterId(int masterId);
        ViewAssetDetailVM GetAssetHistoryById(int assetId);
        IEnumerable<AssetDetail> ViewAllAssetDetailByMasterId(int MasterAssetId);
        int CreateAssetDetailDocuments(CreateAssetDetailAttachmentVM attachObj);
        IEnumerable<AssetDetailAttachment> GetAttachmentByAssetDetailId(int assetId);
        int DeleteAssetDetailAttachment(int id);
        IndexAssetDetailVM SearchAssetInHospital(int pagenumber, int pagesize, SearchMasterAssetVM searchObj);
        IEnumerable<IndexAssetDetailVM.GetData> SearchAssetInHospitalByHospitalId(SearchMasterAssetVM searchObj);
        IEnumerable<IndexPMAssetTaskScheduleVM.GetData> GetAllPMAssetTaskSchedules(int? hospitalId);
        IEnumerable<IndexPMAssetTaskScheduleVM.GetData> GetAllPMAssetTaskScheduleByAssetId(int? assetId);
        List<CountAssetVM> CountAssetsByHospital();
        List<CountAssetVM> ListTopAssetsByHospitalId(int hospitalId);
        List<CountAssetVM> ListAssetsByGovernorateIds();
        List<CountAssetVM> ListAssetsByCityIds();
        List<CountAssetVM> CountAssetsInHospitalByHospitalId(int hospitalId);
        int CountAssetsByHospitalId(int hospitalId);
        List<PmDateGroupVM> GetAllwithgrouping(int assetId);
        List<IndexAssetDetailVM.GetData> FilterAsset(filterDto data);
        List<IndexAssetDetailVM.GetData> FilterDataByDepartmentBrandSupplierId(FilterHospitalAsset data);
        List<DepartmentGroupVM> GetAssetByDepartment(List<IndexAssetDetailVM.GetData> AssetModel);
        List<BrandGroupVM> GetAssetByBrands(List<IndexAssetDetailVM.GetData> AssetModel);
        List<GroupHospitalVM> GetAssetByHospital(List<IndexAssetDetailVM.GetData> AssetModel);
        List<GroupGovernorateVM> GetAssetByGovernorate(List<IndexAssetDetailVM.GetData> AssetModel);
        List<GroupCityVM> GetAssetByCity(List<IndexAssetDetailVM.GetData> AssetModel);
        List<GroupSupplierVM> GetAssetBySupplier(List<IndexAssetDetailVM.GetData> AssetModel);
        List<GroupOrganizationVM> GetAssetByOrganization(List<IndexAssetDetailVM.GetData> AssetModel);
        IEnumerable<IndexAssetDetailVM.GetData> SortAssets(Sort sortObj);
        //IndexAssetDetailVM SortAssets(Sort sortObj, int hospitalId, int statusId, string userId, int pageNumber, int pageSize);
        IndexAssetDetailVM SortAssets(Sort sortObj,  int statusId, string userId);



        List<HospitalAssetAge> GetAssetsByAgeGroup(int hospitalId);
        List<HospitalAssetAge> GetGeneralAssetsByAgeGroup(FilterHospitalAssetAge model);
        IEnumerable<IndexAssetDetailVM.GetData> AutoCompleteAssetBarCode(string barcode, int hospitalId);
        IEnumerable<IndexAssetDetailVM.GetData> AutoCompleteAssetSerial(string serial, int hospitalId);
        IEnumerable<IndexAssetDetailVM.GetData> GetHospitalAssets(int hospitalId, int statusId, string userId, int page, int pageSize, Sort sortObj);
        AssetDetailAttachment GetLastDocumentForAssetDetailId(int assetDetailId);

        GeneratedAssetDetailBCVM GenerateAssetDetailBarcode();
    }
}
