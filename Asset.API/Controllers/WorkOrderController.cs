using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.PagingParameter;
using Asset.ViewModels.WorkOrderVM;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkOrderController : ControllerBase
    {
        private IWorkOrderService _workOrderService;

        private IPagingService _pagingService;
        IWebHostEnvironment _webHostingEnvironment;
        private IWorkOrderTrackingService _workOrderTackingService;

        public WorkOrderController(IWorkOrderService workOrderService, IWorkOrderTrackingService workOrderTackingService, IPagingService pagingService, IWebHostEnvironment webHostingEnvironment)
        {
            _workOrderService = workOrderService;
            _workOrderTackingService = workOrderTackingService;
            _pagingService = pagingService;
            _webHostingEnvironment = webHostingEnvironment;
        }
        // GET: api/<WorkOrderController>
        [HttpGet]
        public IEnumerable<IndexWorkOrderVM> Get()
        {
            return _workOrderService.GetAllWorkOrders();
        }


        [HttpGet]
        [Route("GetworkOrderByUserId/{requestId}/{userId}")]
        public IEnumerable<IndexWorkOrderVM> GetworkOrderByUserId(int requestId, string userId)
        {
            return _workOrderService.GetworkOrderByUserId(requestId, userId);
        }

        [HttpGet]
        [Route("CountWorkOrdersByHospitalId/{hospitalId}/{userId}")]
        public int CountWorkOrdersByHospitalId(int hospitalId, string userId)
        {
            return _workOrderService.CountWorkOrdersByHospitalId(hospitalId, userId);
        }

  
        [HttpGet]
        [Route("GenerateWorOrderNumber")]
        public GeneratedWorkOrderNumberVM GenerateWorOrderNumber()
        {
            return _workOrderService.GenerateWorOrderNumber();
        }

        [HttpGet]
        [Route("GetLastRequestAndWorkOrderByAssetId/{assetId}")]
        public IEnumerable<IndexWorkOrderVM> GetLastRequestAndWorkOrderByAssetId(int assetId)
        {
            return _workOrderService.GetLastRequestAndWorkOrderByAssetId(assetId);
        }

        [HttpGet]
        [Route("GetLastRequestAndWorkOrderByAssetIdAndRequestId/{assetId}/{requestId}")]
        public IEnumerable<IndexWorkOrderVM> GetLastRequestAndWorkOrderByAssetIdAndRequestId(int assetId, int requestId)
        {
            return _workOrderService.GetLastRequestAndWorkOrderByAssetId(assetId, requestId);
        }

        [HttpGet]
        [Route("GetworkOrder/{userId}")]
        public IEnumerable<IndexWorkOrderVM> GetworkOrder(string userId)
        {
            return _workOrderService.GetworkOrder(userId);
        }

        [HttpGet("GetTotalWorkOrdersForAssetInHospital/{assetDetailId}")]
        public int GetTotalWorkOrdersForAssetInHospital(int assetDetailId)
        {
            return _workOrderService.GetTotalWorkOrdersForAssetInHospital(assetDetailId);

        }

        [HttpPut]
        [Route("GetAllWorkOrdersByHospitalId/{hospitalId}/{userId}")]
        public IEnumerable<IndexWorkOrderVM> GetAllWorkOrdersByHospitalId(int? hospitalId, string userId, PagingParameter pageInfo)
        {
            var lstWorkOrders = _workOrderService.GetAllWorkOrdersByHospitalId(hospitalId, userId);
            return _pagingService.GetAll<IndexWorkOrderVM>(pageInfo, lstWorkOrders.ToList());
        }


        //[HttpPut]
        //[Route("GetAllWorkOrdersByHospitalId/{hospitalId}/{userId}/{statusId}")]
        //public IEnumerable<IndexWorkOrderVM> GetAllWorkOrdersByHospitalId(int? hospitalId, string userId, PagingParameter pageInfo)
        //{
        //    var lstWorkOrders = _workOrderService.GetAllWorkOrdersByHospitalId(hospitalId, userId);
        //    return _pagingService.GetAll<IndexWorkOrderVM>(pageInfo, lstWorkOrders.ToList());
        //}



        [HttpGet]
        [Route("GetWorkOrdersByHospitalId/{hospitalId}/{userId}")]
        public IEnumerable<IndexWorkOrderVM> GetAWorkOrdersByHospitalId(int? hospitalId, string userId)
        {
            return _workOrderService.GetAllWorkOrdersByHospitalId(hospitalId, userId);
           
        }


        [HttpPost]
        [Route("GetWorkOrdersByDate/{pagenumber}/{pagesize}")]
        public IEnumerable<IndexWorkOrderVM> GetRequestsByDate(int pagenumber, int pagesize, SearchWorkOrderByDateVM woDateObj)
        {

            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var lstRequests = _workOrderService.GetWorkOrdersByDate(woDateObj).ToList();
            return _pagingService.GetAll<IndexWorkOrderVM>(pageInfo, lstRequests);
        }


        [HttpPost]
        [Route("CountGetWorkOrdersByDate")]
        public int CountGetRequestsByDate(SearchWorkOrderByDateVM woDateObj)
        {
            return _workOrderService.GetWorkOrdersByDate(woDateObj).ToList().Count;

        }


        [HttpGet]
        [Route("getcount/{hospitalId}/{userId}")]
        public int count(int hospitalId, string userId)
        {
            var count = _workOrderService.GetAllWorkOrdersByHospitalId(hospitalId, userId).ToList().Count;
            return count;
        }
        [HttpPut]
        [Route("GetAllWorkOrdersByHospitalStatusId/{hospitalId}/{userId}/{statusId}")]
        public IEnumerable<IndexWorkOrderVM> GetAllWorkOrdersByHospitalId(PagingParameter pageInfo, int? hospitalId, string userId, int statusId)
        {
            var lstWorkOrders = _workOrderService.GetAllWorkOrdersByHospitalId(hospitalId, userId, statusId).ToList();
            return _pagingService.GetAll<IndexWorkOrderVM>(pageInfo, lstWorkOrders);
        }


        [HttpGet]
        [Route("GetCountByStatus/{hospitalId}/{userId}/{statusId}")]
        public int GetCountByStatus(int? hospitalId, string userId, int statusId)
        {
            //  return _workOrderService.GetAllWorkOrdersByHospitalId(hospitalId, userId).Count();

            return _workOrderService.GetAllWorkOrdersByHospitalId(hospitalId, userId, statusId).ToList().Count;
        }



        [HttpPost]
        [Route("SearchInWorkOrders/{pagenumber}/{pagesize}")]
        public IEnumerable<IndexWorkOrderVM> SearchInWorkOrders(int pagenumber, int pagesize, SearchWorkOrderVM searchObj)
        {
            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var list = _workOrderService.SearchWorkOrders(searchObj).ToList();
            return _pagingService.GetAll<IndexWorkOrderVM>(pageInfo, list);
        }

        [HttpPost]
        [Route("SearchInWorkOrdersCount")]
        public int SearchInWorkOrderssCount(SearchWorkOrderVM searchObj)
        {
            int count = _workOrderService.SearchWorkOrders(searchObj).ToList().Count();
            return count;
        }





        // GET api/<WorkOrderController>/5
        [HttpGet("{id}")]
        public ActionResult<IndexWorkOrderVM> Get(int id)
        {
            return _workOrderService.GetWorkOrderById(id);
        }


        [HttpGet("GetWorkOrderByRequestId/{requestId}")]
        public ActionResult<IndexWorkOrderVM> GetWorkOrderByRequestId(int requestId)
        {
            return _workOrderService.GetWorkOrderByRequestId(requestId);
        }



        [HttpGet("PrintWorkOrderById/{id}")]
        public ActionResult<PrintWorkOrderVM> PrintWorkOrderById(int id)
        {
            return _workOrderService.PrintWorkOrderById(id);
        }





        [HttpPost]
        [Route("CreateWorkOrderAttachments")]
        public int CreateWorkOrderAttachments(WorkOrderAttachment attachObj)
        {
            return _workOrderService.CreateWorkOrderAttachments(attachObj);
        }

        [HttpPost]
        [Route("UploadWorkOrderFiles")]
        public ActionResult UploadWorkOrerFiles(IFormFile file)
        {
            var folderPath = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/WorkOrderFiles";
            bool exists = System.IO.Directory.Exists(folderPath);
            if (!exists)
                System.IO.Directory.CreateDirectory(folderPath);

            string filePath = folderPath + "/" + file.FileName;
            if (System.IO.File.Exists(filePath))
            {

            }
            else
            {
                Stream stream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(stream);
                stream.Close();
            }
            return StatusCode(StatusCodes.Status201Created);
        }
       



        // POST api/<WorkOrderController>
        [HttpPost]
        public int Post(CreateWorkOrderVM createWorkOrderVM)
        {
            return _workOrderService.AddWorkOrder(createWorkOrderVM);
        }

        // PUT api/<WorkOrderController>/5
        [HttpPut("{id}")]
        public void Put(int id, EditWorkOrderVM editWorkOrderVM)
        {
            _workOrderService.UpdateWorkOrder(id, editWorkOrderVM);
        }

        // DELETE api/<WorkOrderController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var lstTracks = _workOrderTackingService.GetAllWorkOrderTrackingByWorkOrderId(id).ToList();
            if (lstTracks.Count > 1)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "workorder", Message = "Work Order has many tracks you cannot delete it", MessageAr = "لا يمكنك مسح هذا الطلب لوجود العديد من الأوامر" });
            }
            else
            {
                _workOrderService.DeleteWorkOrder(id);
            }

            return Ok();
        }
        [HttpPost]
        [Route("SortWorkOrders/{hosId}/{userId}/{pagenumber}/{pagesize}/{statusId}")]
        public IEnumerable<IndexWorkOrderVM> SortWorkOrders(int hosId, string userId, int pagenumber, int pagesize, SortWorkOrderVM sortObj,int statusId)
        {
            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var list = _workOrderService.SortWorkOrders(hosId, userId, sortObj, statusId).ToList();
            return _pagingService.GetAll<IndexWorkOrderVM>(pageInfo, list);
        }


        [HttpPost]
        [Route("CountSortWorkOrders/{hosId}/{userId}/{statusId}")]
        public int CountSortWorkOrders(int hosId, string userId,  SortWorkOrderVM sortObj, int statusId)
        {
     
           return _workOrderService.SortWorkOrders(hosId, userId, sortObj, statusId).ToList().Count;
        
        }
    }
}
