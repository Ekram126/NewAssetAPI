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

namespace Asset.Domain.Services
{
    public interface IHealthService
    {
        public IEnumerable<HealthAssetVM> GetHealthCareData(int hospitalID, int departmantId);
         public IEnumerable<HealthDepartmentVM> GetDepartmant(string code);
        //   IEnumerable<HealthCareDevicesViewModels> GetDeviceData(int id);
        //   IEnumerable<HealthCareUnit> GetHospitalData(int id);
           IEnumerable<HealthOrganizationVM> GetOrganizationDetails(getMultiIDVM model);
           IEnumerable<HealthSubOrganizationVM> GetSubOrganizationDetails(int[] orgId);
        //  IEnumerable<HealthCareUnit> GetHospitalsBySubOrginizationsDetails(getMultiIDViewModel model);
        //    IEnumerable<Hospital> GetHospitalsByOrginizationsDetails(getMultiIDViewModel model);
         public IEnumerable<HealthBrandVM> GetBrandsetails(int[] model);
         public IEnumerable<HealthSupplierVM> GetSuppliersDetails(int[] brandId);
        //    IEnumerable<InstallDateViewModel> GetInstallDateetails(int id);
        //   IEnumerable<PriceViewModel> GetPricetails(int id);
        public IEnumerable<HealthDepartmentVM> GetDepartmants(int[] orgIds);
        public IEnumerable<Hospital> GetHospitalInCity(string[] model);
        public IEnumerable<Hospital> GetHospitalsInOrganization(int[] orgIds);
        public IEnumerable<Hospital> GetHospitalInSubOrganization(int[] subOrgIds);
        public IEnumerable<Hospital> GetHospitalInDepartment(int[] DeptIds);
        public IEnumerable<Hospital> GetHospitalsBySupplier(int[] supplierIds);
        public IEnumerable<Hospital> GetPriceRange(decimal FPrice, decimal ToPrice);
        public IEnumerable<Hospital> GetDateRange(dateVM dates);
    }
}
