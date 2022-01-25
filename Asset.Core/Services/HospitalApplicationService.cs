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
            _unitOfWork.HospitalApplicationRepository.Delete(HospitalApplicationObj.Id);
            _unitOfWork.CommitAsync();
            return HospitalApplicationObj.Id;
        }

        public int DeleteHospitalApplicationAttachment(int id)
        {
            return _unitOfWork.HospitalApplicationRepository.DeleteHospitalApplicationAttachment(id);

        }

        public IEnumerable<IndexHospitalApplicationVM.GetData> GetAll()
        {
            return _unitOfWork.HospitalApplicationRepository.GetAll();
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
