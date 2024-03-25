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

namespace Asset.Domain.Repositories
{
    public interface IAssetDetailRepository
    {
        IEnumerable<IndexAssetDetailVM.GetData> GetAll();

        Task<IndexAssetDetailVM> LoadAssetDetailsByUserId(int pageNumber, int pageSize, string userId);

        IEnumerable<IndexAssetDetailVM.GetData> GetAssetDetailsByAssetId(int assetId);
        Task<IEnumerable<IndexAssetDetailVM.GetData>> GetAssetDetailsByUserId(string userId);
        Task<IndexAssetDetailVM> GetAssetDetailsByUserId2(int pageNumber, int pageSize, string userId);
        IndexAssetDetailVM SearchHospitalAssetsByDepartmentId(int departmentId, string userId, int pageNumber, int pageSize);
        Task<IEnumerable<IndexAssetDetailVM.GetData>> GetAssetsByUserId(string userId);
        IndexAssetDetailVM GetAssetsByUserId(string userId, int pageNumber, int pageSize);
        IndexAssetDetailVM SearchAssetInHospital(int pagenumber, int pagesize, SearchMasterAssetVM searchObj);
        IndexAssetDetailVM SearchAssetWarrantyInHospital(int pagenumber, int pagesize, SearchMasterAssetVM searchObj);
        IEnumerable<IndexAssetDetailVM.GetData> GetAllAssetsByStatusId(int statusId, string userId);
        IndexAssetDetailVM GetAllAssetsByStatusId(int pageNumber, int pageSize, int statusId, string userId);
        IEnumerable<IndexAssetDetailVM.GetData> SearchAssetInHospitalByHospitalId(SearchMasterAssetVM searchObj);
        IndexAssetDetailVM SearchHospitalAssetsByHospitalId(SearchMasterAssetVM searchObj);
        IEnumerable<IndexPMAssetTaskScheduleVM.GetData> GetAllPMAssetTaskSchedules(int? hospitalId);
        IEnumerable<IndexPMAssetTaskScheduleVM.GetData> GetAllPMAssetTaskScheduleByAssetId(int? assetId);
        IEnumerable<AssetDetail> GetAllSerialsByMasterAssetIdAndHospitalId(int masterAssetId, int hospitalId);
        IEnumerable<AssetDetail> GetAllAssetDetailsByHospitalId(int hospitalId);
        IEnumerable<ViewAssetDetailVM> GetListOfAssetDetailsByHospitalId(int hospitalId);
        IEnumerable<ViewAssetDetailVM> GetNoneExcludedAssetsByHospitalId(int hospitalId);
        IEnumerable<IndexAssetDetailVM.GetData> AlertAssetsBefore3Monthes();
        IEnumerable<IndexAssetDetailVM.GetData> AlertAssetsBefore3Monthes(int duration);
        IndexAssetDetailVM AlertAssetsBefore3Monthes(int duration, int pageNumber, int pageSize);
        IEnumerable<ViewAssetDetailVM> GetSupplierNoneExcludedAssetsByHospitalId(int hospitalId);
        IEnumerable<ViewAssetDetailVM> GetAutoCompleteSupplierNoneExcludedAssetsByHospitalId(string barcode, int hospitalId);
        IEnumerable<ViewAssetDetailVM> GetAutoCompleteSupplierExcludedAssetsByHospitalId(string barcode, int hospitalId);



        IEnumerable<ViewAssetDetailVM> GetListOfAssetDetailsByHospitalNotInContract(int hospitalId);
        IEnumerable<ViewAssetDetailVM> GetListOfAssetDetailsByHospitalNotInContract(string barcode, int hospitalId);
        IEnumerable<ViewAssetDetailVM> GetListOfAssetDetailsByHospitalNotInContractBySerialNumber(string serialNumber, int hospitalId);
        EditAssetDetailVM GetById(int id);
        ViewAssetDetailVM ViewAssetDetailByMasterId(int masterId);
        AssetDetail QueryAssetDetailById(int assetId);

        IEnumerable<AssetDetail> ViewAllAssetDetailByMasterId(int MasterAssetId);
        int Add(CreateAssetDetailVM assetDetailObj);
        int Update(EditAssetDetailVM assetDetailObj);
        int Delete(int id);
        List<CountAssetVM> CountAssetsByHospital();
        List<CountAssetVM> ListTopAssetsByHospitalId(int hospitalId);
        List<CountAssetVM> ListAssetsByGovernorateIds();
        List<CountAssetVM> ListAssetsByCityIds();
        List<CountAssetVM> CountAssetsInHospitalByHospitalId(int hospitalId);
        int CountAssetsByHospitalId(int hospitalId);
        ViewAssetDetailVM GetAssetHistoryById(int assetId);
        int CreateAssetDetailDocuments(CreateAssetDetailAttachmentVM attachObj);
        IEnumerable<AssetDetailAttachment> GetAttachmentByAssetDetailId(int assetId);
        int DeleteAssetDetailAttachment(int id);
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
        IndexAssetDetailVM SortAssets2(Sort sortObj, int pageNumber, int pageSize);
        IndexAssetDetailVM SortAssets(Sort sortObj, int statusId, string userId);
        List<HospitalAssetAge> GetAssetsByAgeGroup(int hospitalId);
        List<HospitalAssetAge> GetGeneralAssetsByAgeGroup(FilterHospitalAssetAge model);
        IEnumerable<IndexAssetDetailVM.GetData> AutoCompleteAssetBarCode(string barcode, int hospitalId);
        IEnumerable<IndexAssetDetailVM.GetData> AutoCompleteAssetSerial(string serial, int hospitalId);
        IEnumerable<IndexAssetDetailVM.GetData> GetHospitalAssets(int hospitalId, int statusId, string userId, int page, int pageSize, Sort sortObj);
        AssetDetailAttachment GetLastDocumentForAssetDetailId(int assetDetailId);
        GeneratedAssetDetailBCVM GenerateAssetDetailBarcode();
        MobileAssetDetailVM GetAssetDetailById(string userId, int assetId);
        MobileAssetDetailVM2 GetAssetDetailByIdOnly(string userId, int assetId);
        bool GenerateQrCodeForAllAssets(string domainName);
        IndexAssetDetailVM MobSearchAssetInHospital(SearchMasterAssetVM searchObj, int pageNumber, int pageSize);


        IndexAssetDetailVM GetHospitalAssetsByGovIdAndDeptIdAndHospitalId(int departmentId, int govId, int hospitalId, string userId, int pageNumber, int pageSize);
        public IndexAssetDetailVM SortAssetsWithoutSearch(Sort sortObj, int pageNumber, int pageSize);
        IndexAssetDetailVM GetHospitalAssetsBySupplierId(int supplierId, int pageNumber, int pageSize);
        IndexAssetDetailVM SearchHospitalAssetsBySupplierId(SearchAssetDetailVM searchObj, int pageNumber, int pageSize);
        IndexAssetDetailVM SortHospitalAssetsBySupplierId(Sort sortObj, int pageNumber, int pageSize);




        List<IndexAssetDetailVM.GetData> GetAssetsBySupplierId(int supplierId);
        IndexAssetDetailVM FilterDataByDepartmentBrandSupplierIdAndPaging(FilterHospitalAsset data, string userId, int pageNumber, int pageSize);
        IndexAssetDetailVM GetAssetsByBrandId(int brandId);
        IndexAssetDetailVM GetAssetsByDepartmentId(int departmentId);

        IndexAssetDetailVM GetAssetsBySupplierIdWithPaging(int supplierId, int pageNumber, int pageSize);
        IndexAssetDetailVM SortAssetDetail(SortAssetDetail sortObject, int pageNumber, int PageSize);
        IndexAssetDetailVM SortAssetDetailAfterSearch(SortAndFilterDataModel data, int pageNumber, int pageSize);

        List<IndexAssetDetailVM.GetData> FindAllFilteredAssetsForGrouping(FilterHospitalAsset data);

        List<BrandGroupVM> GroupAssetDetailsByBrand(FilterHospitalAsset data);
        List<SupplierGroupVM> GroupAssetDetailsBySupplier(FilterHospitalAsset data);
        List<DepartmentGroupVM> GroupAssetDetailsByDepartment(FilterHospitalAsset data);

        IndexAssetDetailVM GetAssetsByUserIdAndPaging(string userId, int pageNumber, int pageSize);


        IndexAssetDetailVM ListHospitalAssets(SortAndFilterVM data, int pageNumber, int pageSize);

        List<DrawChart> DrawingChart();

    }
}
