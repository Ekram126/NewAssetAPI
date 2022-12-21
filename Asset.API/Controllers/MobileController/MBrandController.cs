using Asset.Domain.Services;
using Asset.ViewModels.BrandVM;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Asset.API.Controllers.MobileController
{
    [Route("mobile/api/[controller]")]
    [ApiController]
    public class MBrandController : ControllerBase
    {
        private IBrandService _brandService;

        public MBrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }



        [HttpGet]
        [Route("ListBrands")]
        public IEnumerable<IndexBrandVM.GetData> GetAll()
        {
            return _brandService.GetAll();
        }



    }
}
