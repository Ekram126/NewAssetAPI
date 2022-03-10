
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
    public class AssetDetailService: IAssetDetailService
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
            //_unitOfWork.CommitAsync();
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

        public IEnumerable<IndexAssetDetailVM.GetData> SearchAssetInHospital(SearchMasterAssetVM searchObj)
        {
            return  _unitOfWork.AssetDetailRepository.SearchAssetInHospital(searchObj);
           
        }

        public IEnumerable<IndexAssetDetailVM.GetData> SearchAssetInHospitalByHospitalId(SearchMasterAssetVM searchObj)
        {
            return  _unitOfWork.AssetDetailRepository.SearchAssetInHospitalByHospitalId(searchObj);
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

        public List<PmDateGroupVM> GetAllwithgrouping(int? masterId)
        {
            return _unitOfWork.AssetDetailRepository.GetAllwithgrouping(masterId);
        }
        public List<IndexAssetDetailVM.GetData>FilterAsset(filterDto data)
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

        public async Task< IEnumerable<IndexAssetDetailVM.GetData>> SortAssets(Sort sortObj)
        {
            return await _unitOfWork.AssetDetailRepository.SortAssets(sortObj);
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
            return _unitOfWork.AssetDetailRepository.AutoCompleteAssetBarCode(barcode,hospitalId);
        }

        public IEnumerable<IndexAssetDetailVM.GetData> GetAllAssetsByStatusId(int statusId, string userId)
        {
            return _unitOfWork.AssetDetailRepository.GetAllAssetsByStatusId(statusId, userId);
          
        }

        public IEnumerable<IndexAssetDetailVM.GetData> AutoCompleteAssetSerial(string serial, int hospitalId)
        {
            return _unitOfWork.AssetDetailRepository.AutoCompleteAssetSerial(serial, hospitalId);
        }
    }
}