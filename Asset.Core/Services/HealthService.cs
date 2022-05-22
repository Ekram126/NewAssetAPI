using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.AssetDetailVM;
using Asset.ViewModels.BrandVM;
using Asset.ViewModels.DateVM;
using Asset.ViewModels.DepartmentVM;
using Asset.ViewModels.MultiIDVM;
using Asset.ViewModels.OrganizationVM;
using Asset.ViewModels.SubOrganizationVM;
using Asset.ViewModels.SupplierVM;
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

        public IEnumerable<HealthBrandVM> GetBrandsetails(int[] model)
        {
            return _unitOfWork.healthRepository.GetBrandsetails(model);
        }

        public IEnumerable<Hospital> GetDateRange(dateVM dates)
        {
            return _unitOfWork.healthRepository.GetDateRange(dates);
        }

        public IEnumerable<HealthDepartmentVM> GetDepartmant(string code)
        {
            return _unitOfWork.healthRepository.GetDepartmant(code);
        }

        public IEnumerable<HealthDepartmentVM> GetDepartmants(int[] orgIds)
        {
            return _unitOfWork.healthRepository.GetDepartmants(orgIds);
        }

        public IEnumerable<HealthAssetVM> GetHealthCareData(int hospitalID, int departmantId)
        {
            return _unitOfWork.healthRepository.GetHealthCareData(hospitalID,departmantId);
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

        public IEnumerable<Hospital> GetHospitalsInOrganization(int[] orgIds)
        {
            return _unitOfWork.healthRepository.GetHospitalsInOrganization(orgIds);
        }

        public IEnumerable<HealthOrganizationVM> GetOrganizationDetails(getMultiIDVM model)
        {
            return _unitOfWork.healthRepository.GetOrganizationDetails(model);
        }

        public IEnumerable<Hospital> GetPriceRange(decimal FPrice, decimal ToPrice)
        {
            return _unitOfWork.healthRepository.GetPriceRange(FPrice, ToPrice);
        }

        public IEnumerable<HealthSubOrganizationVM> GetSubOrganizationDetails(int[] orgId)
        {
            return _unitOfWork.healthRepository.GetSubOrganizationDetails(orgId);
        }

        public IEnumerable<HealthSupplierVM> GetSuppliersDetails(int[] brandId)
        {
            return _unitOfWork.healthRepository.GetSuppliersDetails(brandId);
        }
    }
}
