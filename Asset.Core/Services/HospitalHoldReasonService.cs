using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.HospitalHoldReasonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
   public class HospitalHoldReasonService : IHospitalHoldReasonService
    {
        private IUnitOfWork _unitOfWork;

        public HospitalHoldReasonService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Add(CreateHospitalHoldReasonVM HospitalHoldReasonObj)
        {
            _unitOfWork.HospitalHoldReasonRepository.Add(HospitalHoldReasonObj);
            return _unitOfWork.CommitAsync();
        }

        public int Delete(int id)
        {
            var HospitalHoldReasonObj = _unitOfWork.HospitalHoldReasonRepository.GetById(id);
            _unitOfWork.HospitalHoldReasonRepository.Delete(HospitalHoldReasonObj.Id);
            _unitOfWork.CommitAsync();
            return HospitalHoldReasonObj.Id;
        }

        public IEnumerable<IndexHospitalHoldReasonVM.GetData> GetAll()
        {
            return _unitOfWork.HospitalHoldReasonRepository.GetAll();
        }

      

        public EditHospitalHoldReasonVM GetById(int id)
        {
            return _unitOfWork.HospitalHoldReasonRepository.GetById(id);
        }

        

        public int Update(EditHospitalHoldReasonVM HospitalHoldReasonObj)
        {
            _unitOfWork.HospitalHoldReasonRepository.Update(HospitalHoldReasonObj);
            _unitOfWork.CommitAsync();
            return HospitalHoldReasonObj.Id;
        }
    }
}
