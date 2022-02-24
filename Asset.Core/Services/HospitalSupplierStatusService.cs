using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.HospitalSupplierStatusVM;
using Asset.ViewModels.RequestStatusVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
    public class HospitalSupplierStatusService : IHospitalSupplierStatusService
    {
        private IUnitOfWork _unitOfWork;

        public HospitalSupplierStatusService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IndexHospitalSupplierStatusVM GetAll(int statusId, int appTypeId, int? hospitalId)
        {
            return _unitOfWork.HospitalSupplierStatusRepository.GetAll(statusId, appTypeId, hospitalId);
        }
        public IndexHospitalSupplierStatusVM GetAllByHospitals()
        {
            return _unitOfWork.HospitalSupplierStatusRepository.GetAllByHospitals();
        }

        public IndexHospitalSupplierStatusVM GetAllByHospitals(int statusId, int appTypeId, int? hospitalId)
        {
            return _unitOfWork.HospitalSupplierStatusRepository.GetAllByHospitals(statusId, appTypeId, hospitalId);
        }
        public HospitalSupplierStatus GetById(int id)
        {
            return _unitOfWork.HospitalSupplierStatusRepository.GetById(id);
        }

    }
}
