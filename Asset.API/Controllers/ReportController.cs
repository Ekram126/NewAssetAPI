using Asset.Domain.Services;
using Asset.ViewModels.AssetDetailVM;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyPolicy")]
    public class ReportController : ControllerBase
    {

        //AssetReport assetReport = new AssetReport();

        private IAssetDetailService _assetDetailService;




        public ReportController(IAssetDetailService AssetDetailService)
        {
            _assetDetailService = AssetDetailService;
        }


        //[HttpPost]
        //[Route("GetAssetData/{assetId}")]
        //public XtraReport GetAssetData(int assetId)
        //{
          
        //    var assetObj = _assetDetailService.ViewAssetDetailByMasterId(assetId);

        //    //assetReport.DataSourceDemanded += (s, e) =>
        //    //{
        //    //    ((XtraReport)s).DataSource = assetObj;
        //    //};continue i am on another anydesk i am seeing ???


        //    //D:\Ekram\Projects\Asset System\Asset Project\Assets_3-11-2021\AssetAPI\Asset.API\Reports\
        //    assetReport.DataSource = assetObj;
        //    return assetReport;
        //}
    }
}
