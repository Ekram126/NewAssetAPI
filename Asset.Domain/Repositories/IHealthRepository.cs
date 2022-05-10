﻿using Asset.Models;
using Asset.ViewModels.DateVM;
using Asset.ViewModels.MultiIDVM;
using Asset.ViewModels.OrganizationVM;
using System.Collections.Generic;

namespace Asset.Domain.Repositories
{
    public interface IHealthRepository
    {
       // IEnumerable<HealthCareDevicesViewModels> GetHealthCareData(int hospitalID, int departmantId);
       // IEnumerable<DepartmemtByHospitalCodeViewModels> GetDepartmant(int id);
     //   IEnumerable<HealthCareDevicesViewModels> GetDeviceData(int id);
     //   IEnumerable<HealthCareUnit> GetHospitalData(int id);
        IEnumerable<HealthOrganizationVM> GetOrganizationDetails(getMultiIDVM model);
     //   IEnumerable<SubOrganizationViewModel> GetSubOrganizationDetails(int[] orgId);
        //  IEnumerable<HealthCareUnit> GetHospitalsBySubOrginizationsDetails(getMultiIDViewModel model);
    //    IEnumerable<Hospital> GetHospitalsByOrginizationsDetails(getMultiIDViewModel model);
     //   IEnumerable<ManFactureViewModel> GetBrandsetails(int[] model);
    //    IEnumerable<SupplierViewModel> GetSuppliersDetails(string[] hosCodesInBrand);
    //    IEnumerable<InstallDateViewModel> GetInstallDateetails(int id);
     //   IEnumerable<PriceViewModel> GetPricetails(int id);
       public IEnumerable<Department> GetDepartmants(int[] orgIds);
        public IEnumerable<Hospital> GetHospitalInCity(string[] model);
        //    public IEnumerable<Hospital> GetHospitalsInOrganization(int[] orgIds);
        public IEnumerable<Hospital> GetHospitalInSubOrganization(int[] subOrgIds);
        public IEnumerable<Hospital> GetHospitalInDepartment(int[] DeptIds);
        public IEnumerable<Hospital> GetHospitalsBySupplier(int[] supplierIds);
        public IEnumerable<Hospital> GetPriceRange(decimal FPrice, decimal ToPrice);
        public IEnumerable<Hospital> GetDateRange(dateVM dates);
    }
}
