using System.Collections.Generic;
using System.Threading.Tasks;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.AssetDetailVM;
using Asset.ViewModels.BrandVM;
using Asset.ViewModels.DateVM;
using Asset.ViewModels.DepartmentVM;
using Asset.ViewModels.HospitalVM;
using Asset.ViewModels.MultiIDVM;
using Asset.ViewModels.OrganizationVM;
using Asset.ViewModels.SubOrganizationVM;
using Asset.ViewModels.SupplierVM;
using Microsoft.AspNetCore.Mvc;

namespace BiomedicalSystemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthInfoController : ControllerBase
    {
        //private IGeneratePdf _generatePdf;
        private readonly IHealthService _healthService;
        public HealthInfoController(IHealthService healthService)//, IGeneratePdf generatePdf)
        {
            _healthService = healthService;
         //   _generatePdf = generatePdf;
        }

        [HttpGet]
        [Route("GetHealthData")]
        public IEnumerable<HealthAssetVM> GetHealthData(int hospitalId, int departmantId)
        {
            return _healthService.GetHealthCareData(hospitalId, departmantId);
        }


        [HttpGet]
        [Route("GetDepartmantData")]
        public IEnumerable<HealthDepartmentVM> GetDepartmantData(string code)
        {
            return _healthService.GetDepartmant(code);
        }

        [HttpPost]
        [Route("GetDepartmantsData")]
        public IEnumerable<HealthDepartmentVM> GetDepartmantsData(int[] orgIds)
        {
            return _healthService.GetDepartmants(orgIds);
        }


        //[HttpGet]
        //[Route("GetDevice")]
        //public IEnumerable<HealthCareDevicesViewModels> GetDevice(int id)
        //{
        //    return _dbAccessLayer.GetDeviceData(id);
        //}

        //[HttpGet]
        //[Route("GetHospitalDetails")]
        //public IEnumerable<HealthCareUnit> GetHospitalDetails(int id)
        //{
        //    return _dbAccessLayer.GetHospitalData(id);
        //}

        [HttpPost("GetOrginisations")]
        public IEnumerable<HealthOrganizationVM> GetOrginisations(getMultiIDVM model)
        {
            return _healthService.GetOrganizationDetails(model);
        }

        [HttpPost("GetSubOrginisations")]
        public IEnumerable<HealthSubOrganizationVM> GetSubOrginisations(int[] orgId)
        {
            return _healthService.GetSubOrganizationDetails(orgId);
        }

        [HttpPost("GetBrands")]
        public IEnumerable<HealthBrandVM> GetBrands(int[] model)
        {
            return _healthService.GetBrandsetails(model);
        }

        // [HttpPost("GetHospitalsBySubOrginizations")]
        // public IEnumerable<HealthCareUnit> GetHospitalsBySubOrginizations(getMultiIDViewModel model)
        // {
        //     return _dbAccessLayer.GetHospitalsBySubOrginizationsDetails(model);
        // }

        // [HttpPost("GetHospitalsByOrginizations")]
        // public IEnumerable<HealthCareUnit> GetHospitalsByOrginizations(getMultiIDViewModel model)
        // {
        //     return _dbAccessLayer.GetHospitalsByOrginizationsDetails(model);
        // }

        [HttpPost("GetSuppliers")]
        public IEnumerable<HealthSupplierVM> GetSuppliers(int[] brandId)
        {
            return _healthService.GetSuppliersDetails(brandId);
        }

        //[HttpGet("GetInstallDate")]
        //public IEnumerable<InstallDateViewModel> GetInstallDate(int id)
        //{
        //    return _dbAccessLayer.GetInstallDateetails(id);
        //}

        //[HttpGet("GetPrice")]
        //public IEnumerable<PriceViewModel> GetPrice(int id)
        //{
        //    return _dbAccessLayer.GetPricetails(id);
        //}

        //[HttpPost]
        //[Route("GetReport")]
        //public async Task<IActionResult> GetReport(GetReportViewModel model)
        //{
        //    return await _generatePdf.GetPdf("views/Reports/GetReport.cshtml", model);
        //}

        //[HttpPost]
        //[Route("GetReportEng")]
        //public async Task<IActionResult> GetReportEng(GetReportViewModel model)
        //{
        //    var options = new ConvertOptions
        //    {
        //        //HeaderHtml = "http://localhost/header.html",
        //        HeaderSpacing = 0,
        //        FooterSpacing = 0,
        //        // IsGrayScale = true,
        //        PageSize = Wkhtmltopdf.NetCore.Options.Size.A4,
        //        // PageMargins = new Wkhtmltopdf.NetCore.Options.Margins() { Bottom = 0, Left = 0, Right = 0, Top = 0 },
        //        PageOrientation = Wkhtmltopdf.NetCore.Options.Orientation.Portrait
        //    };
        //    _generatePdf.SetConvertOptions(options);

        //    return await _generatePdf.GetPdf("views/Reports/GetReportEng.cshtml", model);
        //}

        [HttpPost]
        [Route("GetHospitalsInCity")]
        public IEnumerable<Hospital> GetHospitalsInCity(string[] cityCode)
        {
            return _healthService.GetHospitalInCity(cityCode);
        }
        [HttpPost]
        [Route("GetHospitalsInOrganization")]
        public IEnumerable<Hospital> GetHospitalsInOrganization(int[] OrgIds)
        {
            return _healthService.GetHospitalsInOrganization(OrgIds);
        }
        [HttpPost]
        [Route("GetHospitalsInSubOrganization")]
        public IEnumerable<Hospital> GetHospitalsInSubOrganization(int[] subOrgIds)
        {
            return _healthService.GetHospitalInSubOrganization(subOrgIds);
        }

        [HttpPost]
        [Route("GetHospitalsInDepartment")]
        public IEnumerable<Hospital> GetHospitalsInDepartment(int[] DeptIds)
        {
            return _healthService.GetHospitalInDepartment(DeptIds);
        }

        [HttpPost]
        [Route("GetHospitalsBySupplier")]
        public IEnumerable<Hospital> GetHospitalsBySupplier(int[] supplierIds)
        {
            return _healthService.GetHospitalsBySupplier(supplierIds);
        }

        [HttpGet]
        [Route("GetPriceRange")]
        public IEnumerable<Hospital> GetPriceRange(decimal FPrice, decimal ToPrice)
        {
            return _healthService.GetPriceRange(FPrice, ToPrice);
        }
        [HttpPost]
        [Route("GetDateRange")]
        public IEnumerable<Hospital> GetDateRange(dateVM dates)
        {
            return _healthService.GetDateRange(dates);
        }
    }
}

