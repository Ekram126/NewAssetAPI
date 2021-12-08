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
        IEnumerable<IndexAssetDetailVM.GetData> GetAssetDetailsByAssetId(int assetId);
        Task<IEnumerable<IndexAssetDetailVM.GetData>> GetAssetDetailsByUserId(string userId);
        Task<IEnumerable<IndexAssetDetailVM.GetData>> GetAssetsByUserId(string userId);


        IEnumerable<IndexAssetDetailVM.GetData> SearchAssetInHospital(SearchMasterAssetVM searchObj);


        IEnumerable<IndexAssetDetailVM.GetData> SearchAssetInHospitalByHospitalId(SearchMasterAssetVM searchObj);
        IEnumerable<IndexPMAssetTaskScheduleVM.GetData> GetAllPMAssetTaskSchedules(int? hospitalId);
        IEnumerable<AssetDetail> GetAllSerialsByMasterAssetIdAndHospitalId(int masterAssetId, int hospitalId);
        IEnumerable<AssetDetail> GetAllAssetDetailsByHospitalId(int hospitalId);

        IEnumerable<ViewAssetDetailVM> GetListOfAssetDetailsByHospitalId(int hospitalId);
        EditAssetDetailVM GetById(int id);
        ViewAssetDetailVM ViewAssetDetailByMasterId(int masterId);
        IEnumerable<AssetDetail> ViewAllAssetDetailByMasterId(int MasterAssetId);
        int Add(CreateAssetDetailVM assetDetailObj);
        int Update(EditAssetDetailVM assetDetailObj);
        int Delete(int id);

        List<CountAssetVM> CountAssetsByHospital();
             
        int CreateAssetDetailDocuments(CreateAssetDetailAttachmentVM attachObj);
        IEnumerable<AssetDetailAttachment> GetAttachmentByAssetDetailId(int assetId);
        int DeleteAssetDetailAttachment(int id);
        List<PmDateGroupVM> GetAllwithgrouping(int? masterId);
        List<IndexAssetDetailVM.GetData> FilterAsset(filterDto data);
        List<BrandGroupVM> GetAssetByBrands(List<IndexAssetDetailVM.GetData> AssetModel);
        List<GroupHospitalVM> GetAssetByHospital(List<IndexAssetDetailVM.GetData> AssetModel);
        List<GroupGovernorateVM> GetAssetByGovernorate(List<IndexAssetDetailVM.GetData> AssetModel);
        List<GroupCityVM> GetAssetByCity(List<IndexAssetDetailVM.GetData> AssetModel);
        List<GroupSupplierVM> GetAssetBySupplier(List<IndexAssetDetailVM.GetData> AssetModel);
        List<GroupOrganizationVM> GetAssetByOrganization(List<IndexAssetDetailVM.GetData> AssetModel);
        IEnumerable<IndexAssetDetailVM.GetData> SortAssets(Sort sortObj);

    }
}
