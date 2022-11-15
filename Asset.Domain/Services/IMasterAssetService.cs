using Asset.Models;
using Asset.ViewModels.MasterAssetAttachmentVM;
using Asset.ViewModels.MasterAssetVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
    public interface IMasterAssetService
    {
        IEnumerable<IndexMasterAssetVM.GetData> GetAll();
        IEnumerable<IndexMasterAssetVM.GetData> GetTop10MasterAsset(int hospitalId);
        IEnumerable<IndexMasterAssetVM.GetData> GetListMasterAsset();
        IEnumerable<IndexMasterAssetVM.GetData> SearchInMasterAssets(SearchMasterAssetVM searchObj);
        IEnumerable<MasterAsset> GetAllMasterAssets();
        IEnumerable<MasterAsset> AutoCompleteMasterAssetName(string name);
        IEnumerable<IndexMasterAssetVM.GetData> AutoCompleteMasterAssetName2(string name);
        IEnumerable<IndexMasterAssetVM.GetData> AutoCompleteMasterAssetName3(string name, int hospitalId);
        //   IEnumerable<MasterAsset> GetAssetOwnerByHospitalId(int hospitalId, string userId);
        IEnumerable<MasterAsset> GetAllMasterAssetsByHospitalId(int hospitalId, string userId);
        IEnumerable<MasterAsset> GetAllMasterAssetsByHospitalId(int hospitalId);
        EditMasterAssetVM GetById(int id);
        ViewMasterAssetVM ViewMasterAsset(int id);
        int Add(CreateMasterAssetVM masterAssetObj);
        int Update(EditMasterAssetVM masterAssetObj);
        int UpdateMasterAssetImageAfterInsert(CreateMasterAssetVM masterAssetObj);
        int Delete(int id);
        int CreateMasterAssetDocuments(CreateMasterAssetAttachmentVM attachObj);
        IEnumerable<MasterAssetAttachment> GetAttachmentByMasterAssetId(int assetId);
        int DeleteMasterAssetAttachment(int id);
        int CountMasterAssets();
        List<CountMasterAssetBrands> CountMasterAssetsByBrand(int hospitalId);
        List<CountMasterAssetSuppliers> CountMasterAssetsBySupplier(int hospitalId);
        IEnumerable<IndexMasterAssetVM.GetData> sortMasterAssets(SortMasterAssetVM sortObj);
    }
}
