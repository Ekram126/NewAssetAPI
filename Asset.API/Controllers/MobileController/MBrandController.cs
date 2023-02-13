using Asset.Domain.Services;
using Asset.ViewModels.BrandVM;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Asset.API.Controllers.MobileController
{
    [Route("mobile/api/[controller]")]
    [ApiController]
    public class MBrandController : ControllerBase
    {
        private IBrandService _brandService;
        private IMasterAssetService _masterAssetService;
        public MBrandController(IBrandService brandService, IMasterAssetService masterAssetService)
        {
            _brandService = brandService;
            _masterAssetService = masterAssetService;
        }



        [HttpGet]
        [Route("ListBrands")]
        public ActionResult GetAll()
        {
            IEnumerable<IndexBrandVM.GetData> list = new List<IndexBrandVM.GetData>();

            list = _brandService.GetAll();
            if (list.Count() == 0)
            {
                return Ok(new { data = list, msg = "No Data", status = '0' });
            }
            else
                return Ok(new { data = list, msg = "Success", status = '1' });
        }


        [HttpGet]
        [Route("CountMasterAssetsByBrand/{hospitalId}")]
        public ActionResult CountMasterAssetsByBrand(int hospitalId)
        {
            var lstMasterAssetBrands = _masterAssetService.CountMasterAssetsByBrand(hospitalId);
            if (lstMasterAssetBrands.Count() == 0)
            {
                return Ok(new { data = lstMasterAssetBrands, msg = "No Data", status = '0' });
            }
            else
                return Ok(new { data = lstMasterAssetBrands, msg = "Success", status = '1' });
        }

        [HttpGet]
        [Route("GetTop10BrandsCount/{hospitalId}")]
        public ActionResult GetTop10BrandsCount(int hospitalId)
        {
            var total = _brandService.GetTop10Brands(hospitalId).ToList().Count;

            return Ok(new { data = total.ToString(), msg = "Success", status = '1' });
        }

    }
}
