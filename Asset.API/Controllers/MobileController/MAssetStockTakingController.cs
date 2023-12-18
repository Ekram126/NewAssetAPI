using Asset.API.Helpers;
using Asset.Core.Services;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.AssetStockTakingVM;
using Asset.ViewModels.StockTakingHospitalVM;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.API.Controllers
{
    [Route("mobile/api/[controller]")]
    [ApiController]
    public class MAssetStockTakingController : ControllerBase
    {
        private IAssetStockTakingService _assetStockTakingService;
        private IAssetDetailService _assetDetailService;
        private IStockTakingScheduleService _stockTakingScheduleService;
        private IStockTakingHospitalService _stockTakingHospitalService;
        private readonly UserManager<ApplicationUser> _userManager;
        private IEmployeeService _employeeService;
        private IAssetOwnerService _assetOwnerService;
        public MAssetStockTakingController(IAssetStockTakingService assetStockTakingService, IAssetDetailService assetDetailService, IStockTakingScheduleService stockTakingScheduleService, IStockTakingHospitalService stockTakingHospitalService, UserManager<ApplicationUser> userManager, IEmployeeService employeeService, IAssetOwnerService assetOwnerService)
        {
            _assetStockTakingService = assetStockTakingService;

            _assetDetailService = assetDetailService;
            _stockTakingScheduleService = stockTakingScheduleService;
            _stockTakingHospitalService = stockTakingHospitalService;
            _userManager = userManager;
            _employeeService = employeeService;
            _assetOwnerService = assetOwnerService;
        }


        [HttpPost]
        [Route("CaptureQRCode")]
        public async Task<IActionResult> CaptureQRCode(CreateAssetStockTakingVM createAssetStockTakingVM)
        {



            int assetId = int.Parse(createAssetStockTakingVM.URL.Split('/').Last());
            if (assetId == 0)
            {

                return Ok(new { data = assetId, msg = "error this asset id does not exist", status = "400" });
            }

            var userManagerObject = await _userManager.FindByIdAsync(createAssetStockTakingVM.UserId);

            if (userManagerObject != null)
            {
                if (userManagerObject.Email != null)
                {
                    var userEmail = userManagerObject.Email;
                    var lstEmployees = _employeeService.GetAll().Where(ww => ww.Email == userEmail).ToList();
                    if (lstEmployees.Count() > 0)
                    {
                        var employee = lstEmployees[0];
                        var employeeId = employee.Id;

                        if (_assetOwnerService.GetAll().Where(ww => ww.EmployeeId == employeeId && ww.AssetDetailId == assetId).Count() > 0)
                        {
                            var assetDetaiList = _assetDetailService.GetAll().Where(ww => ww.Id == assetId).ToList();
                            if (assetDetaiList.Count() > 0)
                            {
                                var assetDetailObj = assetDetaiList[0];
                                int HospitalId = (int)assetDetailObj.HospitalId;
                                var stockTakingHospitaList = _stockTakingHospitalService.GetAll().Where(ww => ww.HospitalId == HospitalId).OrderByDescending(a=>a.Id).ToList();

                                if (stockTakingHospitaList.Count() > 0)
                                {
                                    var stockTakingHospitalObj = stockTakingHospitaList[0];
                                    var stockTakingScheduleId = stockTakingHospitalObj.STSchedulesId;
                                    var stockTakingScheduleList = _stockTakingScheduleService.GetAll().Where(ww => ww.Id == stockTakingScheduleId).OrderByDescending(a=>a.CreationDate.Value.Date).ToList();
                                    if (stockTakingScheduleList.Count() > 0)
                                    {
                                        var stockTakingScheduleObj = stockTakingScheduleList[0];
                                        
                                        if (createAssetStockTakingVM.CaptureDate.Value.Date >= stockTakingScheduleObj.StartDate.Value.Date && createAssetStockTakingVM.CaptureDate.Value.Date <= stockTakingScheduleObj.EndDate.Value.Date)
                                        {
                                            if (_assetStockTakingService.GetAll().Where(ww => ww.AssetDetailId == assetId).Count() > 0)
                                            {
                                                return Ok(new { data = "", msg = "Exist", status = "1" });

                                            }
                                            else
                                            {
                                                createAssetStockTakingVM.UserId = createAssetStockTakingVM.UserId;
                                                createAssetStockTakingVM.AssetDetailId = assetId;
                                                createAssetStockTakingVM.HospitalId = HospitalId;
                                                createAssetStockTakingVM.CaptureDate = DateTime.Now;
                                                createAssetStockTakingVM.Latitude = createAssetStockTakingVM.Latitude;
                                                createAssetStockTakingVM.Longtitude = createAssetStockTakingVM.Longtitude;
                                                createAssetStockTakingVM.STSchedulesId = stockTakingScheduleObj.Id;
                                                _assetStockTakingService.Add(createAssetStockTakingVM);
                                                return Ok(new { data = createAssetStockTakingVM, msg = "success", status = "200" });
                                            }

                                        }
                                        if (createAssetStockTakingVM.CaptureDate.Value.Date <= stockTakingScheduleObj.StartDate.Value.Date || createAssetStockTakingVM.CaptureDate.Value.Date >= stockTakingScheduleObj.EndDate.Value.Date)
                                        {
                                            return Ok(new { data = "", msg = "Capture Date is not in range", status = "9" });
                                        }
                                    }
                                    else
                                    {
                                        return Ok(new { data = "", msg = "No Schedule Found", status = "8" });
                                    }

                                }
                                else
                                {
                                    return Ok(new { data = "", msg = "No Hospital Found", status = "7" });
                                }

                            }
                            else
                            {
                                return Ok(new { data = "", msg = "No Data Found", status = "2" });
                            }

                        }
                        else
                        {
                            return Ok(new { data = "", msg = "You are not asset owner", status = "10" });
                        }

                    }
                    else
                    {
                        return Ok(new { data = "", msg = "No Asset Owner Found", status = "3" });
                    }
                }
                else
                {
                    return Ok(new { data = "", msg = "No Email Found", status = "4" });
                }
            }
            else
            {
                return Ok(new { data = "", msg = "unauthorized User", status = "5" });
            }

            return Ok();


        }

        [HttpPost]
        [Route("GetAssetDetailIdByQrCode")]
        public ActionResult GetAssetDetailIdByQrCode(string url)
        {
            int assetId = int.Parse(url.Split('/').Last());
            if (assetId == 0)
            {

                return Ok(new { data = "", msg = "error this is asset id not exist", status = "400" });
            }
            else
            {
                return Ok(new { data = assetId, msg = "", status = "200" });
            }
        }







    }
}


