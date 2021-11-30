using Asset.API.Reports;
using Asset.Domain.Services;
using Asset.ViewModels.AssetDetailVM;
using DevExpress.XtraReports.UI;
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

        AssetReport assetReport = new AssetReport();

        private IAssetDetailService _assetDetailService;



    
        public ReportController(IAssetDetailService AssetDetailService)
        {
            _assetDetailService = AssetDetailService;
        }


        [HttpPost]
        [Route("GetAssetData/{assetId}")]
        public ActionResult<ViewAssetDetailVM> GetAssetData(int assetId)
        {
            var assetObj = _assetDetailService.ViewAssetDetailByMasterId(assetId);
            //assetReport.DataSourceDemanded += (s, e) =>
            //{
            //    ((XtraReport)s).DataSource = assetObj;
            //};

      
            assetReport.DataSource = assetObj;
            return assetObj;
        }

    }
}
