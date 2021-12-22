﻿using Asset.Models;
using Asset.ViewModels.MasterAssetAttachmentVM;
using Asset.ViewModels.MasterAssetVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Repositories
{
    public interface IMasterAssetRepository
    {
        IEnumerable<IndexMasterAssetVM.GetData> GetAll();
        IEnumerable<IndexMasterAssetVM.GetData> GetTop10MasterAsset();
        IEnumerable<IndexMasterAssetVM.GetData> SearchInMasterAssets(SearchMasterAssetVM searchObj);
        IEnumerable<MasterAsset> GetAllMasterAssets();
        //IEnumerable<MasterAsset> GetAssetOwnerByHospitalId(int hospitalId, string userId);

        IEnumerable<MasterAsset> AutoCompleteMasterAssetName(string name);


        IEnumerable<MasterAsset> GetAllMasterAssetsByHospitalId(int hospitalId, string userId);
        IEnumerable<MasterAsset> GetAllMasterAssetsByHospitalId(int hospitalId);
        EditMasterAssetVM GetById(int id);
        ViewMasterAssetVM ViewMasterAsset(int id);
        int Add(CreateMasterAssetVM masterAssetObj);
        int Update(EditMasterAssetVM masterAssetObj);
        int Delete(int id);

        int CreateMasterAssetDocuments(CreateMasterAssetAttachmentVM attachObj);

        IEnumerable<MasterAssetAttachment> GetAttachmentByMasterAssetId(int assetId);

        int DeleteMasterAssetAttachment(int id);
        int CountMasterAssets();

        List<CountMasterAssetBrands> CountMasterAssetsByBrand();

        List<CountMasterAssetSuppliers> CountMasterAssetsBySupplier();
        IEnumerable<IndexMasterAssetVM.GetData> sortMasterAssets(SortMasterAssetVM searchObj);
    }
}
