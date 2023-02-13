using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.PagingParameter;
using Asset.ViewModels.SupplierVM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.API.Controllers
{
    [Route("mobile/api/[controller]")]
    [ApiController]
    public class MSupplierController : ControllerBase
    {
        private IAssetDetailService _assetDetailService;
        private IMasterContractService _masterContractService;
        private ISupplierService _SupplierService;
        private IPagingService _pagingService;


        public MSupplierController(ISupplierService SupplierService, IAssetDetailService assetDetailService, IMasterContractService masterContractService, IPagingService pagingService)
        {
            _masterContractService = masterContractService;
            _assetDetailService = assetDetailService;
            _SupplierService = SupplierService;
            _pagingService = pagingService;

        }


        [HttpGet]
        [Route("ListSuppliers")]

        public ActionResult<IEnumerable<IndexSupplierVM.GetData>> GetAll()
        {
            var ListSuppliers = _SupplierService.GetAll();
            if (ListSuppliers.Count() == 0)
            {
                return Ok(new { data = ListSuppliers, msg = "No Data Found", status = '0' });
            }
            else
                return Ok(new { data = ListSuppliers, msg = "Success", status = '1' });
        }


        [HttpGet]
        [Route("GetTop10SuppliersCount/{hospitalId}")]
        public ActionResult GetTop10SuppliersCount(int hospitalId)
        {
            var total= _SupplierService.GetTop10Suppliers(hospitalId).ToList().Count;
            return Ok(new { data = total.ToString(), msg = "Success", status = '1' });
        }

    }


}

