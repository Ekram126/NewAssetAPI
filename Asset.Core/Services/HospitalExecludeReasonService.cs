using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.HospitalExecludeReasonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
   public class HospitalExecludeReasonService : IHospitalExecludeReasonService
    {
        private IUnitOfWork _unitOfWork;

        public HospitalExecludeReasonService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Add(CreateHospitalExecludeReasonVM HospitalExecludeReasonObj)
        {
            _unitOfWork.HospitalExecludeReasonRepository.Add(HospitalExecludeReasonObj);
            return _unitOfWork.CommitAsync();
        }

        public int Delete(int id)
        {
            var HospitalExecludeReasonObj = _unitOfWork.HospitalExecludeReasonRepository.GetById(id);
            _unitOfWork.HospitalExecludeReasonRepository.Delete(HospitalExecludeReasonObj.Id);
            _unitOfWork.CommitAsync();
            return HospitalExecludeReasonObj.Id;
        }

        public IEnumerable<IndexHospitalExecludeReasonVM.GetData> GetAll()
        {
            return _unitOfWork.HospitalExecludeReasonRepository.GetAll();
        }

      

        public EditHospitalExecludeReasonVM GetById(int id)
        {
            return _unitOfWork.HospitalExecludeReasonRepository.GetById(id);
        }

        

        public int Update(EditHospitalExecludeReasonVM HospitalExecludeReasonObj)
        {
            _unitOfWork.HospitalExecludeReasonRepository.Update(HospitalExecludeReasonObj);
            _unitOfWork.CommitAsync();
            return HospitalExecludeReasonObj.Id;
        }
    }
}
