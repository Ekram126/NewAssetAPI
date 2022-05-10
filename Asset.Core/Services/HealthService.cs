using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.DateVM;
using Asset.ViewModels.MultiIDVM;
using Asset.ViewModels.OrganizationVM;
using System.Collections.Generic;

namespace Asset.Core.Services
{
    public class HealthService : IHealthService
    {
        private IUnitOfWork _unitOfWork;

        public HealthService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<Hospital> GetDateRange(dateVM dates)
        {
            return _unitOfWork.healthRepository.GetDateRange(dates);
        }

        public IEnumerable<Department> GetDepartmants(int[] orgIds)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Hospital> GetHospitalInCity(string[] model)
        {
            return _unitOfWork.healthRepository.GetHospitalInCity(model);
        }

        public IEnumerable<Hospital> GetHospitalInDepartment(int[] DeptIds)
        {
            return _unitOfWork.healthRepository.GetHospitalInDepartment(DeptIds);
        }

        public IEnumerable<Hospital> GetHospitalInSubOrganization(int[] subOrgIds)
        {
            return _unitOfWork.healthRepository.GetHospitalInSubOrganization(subOrgIds);
        }

        public IEnumerable<Hospital> GetHospitalsBySupplier(int[] supplierIds)
        {
            return _unitOfWork.healthRepository.GetHospitalsBySupplier(supplierIds);
        }

        public IEnumerable<HealthOrganizationVM> GetOrganizationDetails(getMultiIDVM model)
        {
            return _unitOfWork.healthRepository.GetOrganizationDetails(model);
        }

        public IEnumerable<Hospital> GetPriceRange(decimal FPrice, decimal ToPrice)
        {
            return _unitOfWork.healthRepository.GetPriceRange(FPrice, ToPrice);
        }
    }
}
