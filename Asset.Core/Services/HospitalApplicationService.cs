using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.HospitalApplicationVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
    public class HospitalApplicationService : IHospitalApplicationService
    {
        private IUnitOfWork _unitOfWork;

        public HospitalApplicationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Add(CreateHospitalApplicationVM HospitalApplicationObj)
        {
            return _unitOfWork.HospitalApplicationRepository.Add(HospitalApplicationObj);
            //return _unitOfWork.CommitAsync();
        }

        public int CreateHospitalApplicationAttachments(HospitalApplicationAttachment attachObj)
        {
            return _unitOfWork.HospitalApplicationRepository.CreateHospitalApplicationAttachments(attachObj);
        }

        public int Delete(int id)
        {
            var HospitalApplicationObj = _unitOfWork.HospitalApplicationRepository.GetById(id);
            if (HospitalApplicationObj != null)
            {
                _unitOfWork.HospitalApplicationRepository.Delete(HospitalApplicationObj.Id);
                _unitOfWork.CommitAsync();
                return HospitalApplicationObj.Id;
            }
            return 0;
        }

        public int DeleteHospitalApplicationAttachment(int id)
        {
            return _unitOfWork.HospitalApplicationRepository.DeleteHospitalApplicationAttachment(id);

        }

        public GeneratedHospitalApplicationNumberVM GenerateHospitalApplicationNumber()
        {
            return _unitOfWork.HospitalApplicationRepository.GenerateHospitalApplicationNumber();
        }

        public IEnumerable<IndexHospitalApplicationVM.GetData> GetAll()
        {
            return _unitOfWork.HospitalApplicationRepository.GetAll();
        }

        public IEnumerable<IndexHospitalApplicationVM.GetData> GetAllByAppTypeId(int appTypeId)
        {
            return _unitOfWork.HospitalApplicationRepository.GetAllByAppTypeId(appTypeId);
        }

        public IEnumerable<IndexHospitalApplicationVM.GetData> GetAllByAppTypeIdAndStatusId(int statusId, int appTypeId, int hospitalId)
        {
            return _unitOfWork.HospitalApplicationRepository.GetAllByAppTypeIdAndStatusId(statusId, appTypeId, hospitalId);
        }


        public IndexHospitalApplicationVM GetAllHospitalExecludes(SearchHospitalApplicationVM searchObj,int statusId, int appTypeId, int hospitalId, int pageNumber, int pageSize)
        {
            return _unitOfWork.HospitalApplicationRepository.GetAllHospitalExecludes(searchObj,statusId, appTypeId, hospitalId, pageNumber, pageSize);
        }
        public IndexHospitalApplicationVM GetAllHospitalHolds(SearchHospitalApplicationVM searchObj,int statusId, int appTypeId, int hospitalId, int pageNumber, int pageSize)
        {
            return _unitOfWork.HospitalApplicationRepository.GetAllHospitalHolds(searchObj,statusId, appTypeId, hospitalId, pageNumber, pageSize);
        }


        public IndexHospitalApplicationVM GetAllHospitalExecludes(SearchHospitalApplicationVM searchObj)
        {
            return _unitOfWork.HospitalApplicationRepository.GetAllHospitalExecludes(searchObj);
        }
        public IndexHospitalApplicationVM GetAllHospitalHolds(SearchHospitalApplicationVM searchObj)
        {
            return _unitOfWork.HospitalApplicationRepository.GetAllHospitalHolds(searchObj);
        }




        public IEnumerable<IndexHospitalApplicationVM.GetData> GetAllByHospitalId(int hospitalId)
        {
            return _unitOfWork.HospitalApplicationRepository.GetAllByHospitalId(hospitalId);
        }

        public IEnumerable<IndexHospitalApplicationVM.GetData> GetAllByStatusId(int statusId, int hospitalId)
        {
            return _unitOfWork.HospitalApplicationRepository.GetAllByStatusId(statusId, hospitalId);
        }

        public int GetAssetHospitalId(int assetId)
        {
            return _unitOfWork.HospitalApplicationRepository.GetAssetHospitalId(assetId);
        }

        public IEnumerable<HospitalApplicationAttachment> GetAttachmentByHospitalApplicationId(int hospitalApplicationId)
        {

            return _unitOfWork.HospitalApplicationRepository.GetAttachmentByHospitalApplicationId(hospitalApplicationId);
        }

        public EditHospitalApplicationVM GetById(int id)
        {
            return _unitOfWork.HospitalApplicationRepository.GetById(id);
        }

        public IEnumerable<IndexHospitalApplicationVM.GetData> GetHospitalApplicationByDate(SearchHospitalApplicationVM searchObj)
        {
            return _unitOfWork.HospitalApplicationRepository.GetHospitalApplicationByDate(searchObj);
        }

        public ViewHospitalApplicationVM GetHospitalApplicationById(int id)
        {
            return _unitOfWork.HospitalApplicationRepository.GetHospitalApplicationById(id);
        }

        public IEnumerable<IndexHospitalApplicationVM.GetData> SortHospitalApp(SortHospitalApplication sortObj)
        {
            return _unitOfWork.HospitalApplicationRepository.SortHospitalApp(sortObj);
        }

        public int Update(EditHospitalApplicationVM HospitalApplicationObj)
        {
            return _unitOfWork.HospitalApplicationRepository.Update(HospitalApplicationObj);
            //_unitOfWork.CommitAsync();
            // HospitalApplicationObj.Id;
        }

        public int UpdateExcludedDate(EditHospitalApplicationVM hospitalApplicationObj)
        {
            return _unitOfWork.HospitalApplicationRepository.UpdateExcludedDate(hospitalApplicationObj);
        }
    }
}
