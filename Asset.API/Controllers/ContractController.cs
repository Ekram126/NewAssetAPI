using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.AssetDetailVM;
using Asset.ViewModels.ContractVM;
using Asset.ViewModels.PagingParameter;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Contract.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        IWebHostEnvironment _webHostingEnvironment;
        private IMasterContractService _masterContractService;
        private IPagingService _pagingService;

        private IContractDetailService _contractDetailService;
        public ContractController(IMasterContractService masterContractService, IContractDetailService contractDetailService,
            IPagingService pagingService, IWebHostEnvironment webHostingEnvironment)
        {
            _masterContractService = masterContractService;
            _contractDetailService = contractDetailService;
            _pagingService = pagingService;
            _webHostingEnvironment = webHostingEnvironment;
        }

        [HttpGet]
        [Route("ListMasterContracts")]
        public IEnumerable<MasterContract> GetAll()
        {
            return _masterContractService.GetAll();
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<MasterContract> GetById(int id)
        {
            return _masterContractService.GetById(id);
        }

        [HttpGet]
        [Route("GetContractsByMasterContractId/{masterId}")]
        public ActionResult<IEnumerable<IndexContractVM.GetData>> GetContractsByMasterContractId(int masterId)
        {
            return _contractDetailService.GetContractsByMasterContractId(masterId).ToList();
        }


        [HttpGet]
        [Route("AlertContractsBefore3Months/{hospitalId}/{duration}")]
        public IEnumerable<IndexMasterContractVM.GetData> AlertContractsBefore3Months(int hospitalId, int duration)
        {
            return _masterContractService.AlertContractsBefore3Months(hospitalId, duration).ToList();
        }


        [HttpGet]
        [Route("GetListofHospitalsFromAssetContractDetailByMasterContractId/{masterId}")]
        public ActionResult<IEnumerable<Hospital>> GetListofHospitalsFromAssetContractDetailByMasterContractId(int masterId)
        {
            return _contractDetailService.GetListofHospitalsFromAssetContractDetailByMasterContractId(masterId).ToList();
        }


        [HttpGet]
        [Route("GetContractAssetsByHospitalId/{hospitalId}/{masterContractId}")]
        public ActionResult<IEnumerable<IndexContractVM.GetData>> GetContractAssetsByHospitalId(int hospitalId, int masterContractId)
        {
            return _contractDetailService.GetContractAssetsByHospitalId(hospitalId, masterContractId).ToList();
        }



        [HttpGet]
        [Route("GetContractByHospitalId/{hospitalId}")]
        public ActionResult<IEnumerable<IndexContractVM.GetData>> GetContractByHospitalId(int hospitalId)
        {
            return _contractDetailService.GetContractByHospitalId(hospitalId).ToList();
        }

        [HttpGet]
        [Route("GetMasterContractsByHospitalId/{hospitalId}")]
        public ActionResult<IndexMasterContractVM> GetMasterContractsByHospitalId(int hospitalId)
        {
            return _masterContractService.GetMasterContractsByHospitalId(hospitalId);
        }


        [HttpGet]
        [Route("GetLastDocumentForMasterContractId/{masterContractId}")]
        public ContractAttachment GetLastDocumentForMasterContractId(int masterContractId)
        {
            return _masterContractService.GetLastDocumentForMasterContractId(masterContractId);
        }



        [HttpPut]
        [Route("GetMasterContractsByHospitalIdWithPaging/{hospitalId}/{pageNumber}/{pageSize}")]
        public IndexMasterContractVM GetMasterContractsByHospitalIdWithPaging(int hospitalId, int pageNumber, int pageSize)
        {
            var lstContracts = _masterContractService.GetMasterContractsByHospitalId(hospitalId, pageNumber, pageSize);
            return lstContracts;
        }




        //[HttpPut]
        //[Route("GetMasterContractsByHospitalIdWithPaging/{hospitalId}")]
        //public IndexMasterContractVM GetMasterContractsByHospitalIdWithPaging(int hospitalId, PagingParameter pageInfo)
        //{
        //    var lstContracts = _masterContractService.GetMasterContractsByHospitalId(hospitalId);
        //    return lstContracts;// _pagingService.GetAll<IndexMasterContractVM.GetData>(pageInfo, Contracts);
        //}

        //[HttpGet]
        //[Route("getcount/{hospitalId}")]
        //public int count(int hospitalId)
        //{
        //    return _masterContractService.GetMasterContractsByHospitalId(hospitalId).Count();
        //}

        [HttpPut]
        [Route("UpdateMasterContract")]
        public IActionResult Update(MasterContract MasterContractVM)
        {
            try
            {

                int updatedRow = _masterContractService.Update(MasterContractVM);

            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in update");
            }

            return Ok();
        }

        [HttpPost]
        [Route("AddMasterContract")]
        public ActionResult<MasterContract> Add(CreateMasterContractVM MasterContractVM)
        {

            var savedId = _masterContractService.Add(MasterContractVM);
            return Ok(new { MasterContractId = savedId });

        }

        //[HttpPost]
        //[Route("SearchInContract")]
        //public IEnumerable<IndexMasterContractVM.GetData> SearchInContract(SearchContractVM model)
        //{
        //    return _masterContractService.Search(model).ToList();
        //}

        [HttpPost]
        [Route("SearchInContract/{pagenumber}/{pagesize}")]
        public IEnumerable<IndexMasterContractVM.GetData> SearchInContract(int pagenumber, int pagesize, SearchContractVM searchObj)
        {
            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var list = _masterContractService.Search(searchObj).ToList();
            return _pagingService.GetAll<IndexMasterContractVM.GetData>(pageInfo, list);
        }

        [HttpPost]
        [Route("SearchInContractCount")]
        public int SearchInContractCount(SearchContractVM searchObj)
        {
            int count = _masterContractService.Search(searchObj).ToList().Count();
            return count;
        }

        [HttpDelete]
        [Route("DeleteMasterContract/{id}")]
        public ActionResult<MasterContract> Delete(int id)
        {
            try
            {

                var lstContractDetails = _contractDetailService.GetContractsByMasterContractId(id).ToList();
                if (lstContractDetails.Count > 0)
                {
                    foreach (var item in lstContractDetails)
                    {
                        _contractDetailService.Delete(item.Id);
                    }
                }
                int deletedRow = _masterContractService.Delete(id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in delete");
            }

            return Ok();
        }

        [HttpPost]
        [Route("AddContract")]
        public ActionResult<ContractDetail> AddContract(ContractDetail model)
        {

            var savedId = _contractDetailService.Add(model);
            return Ok(new { ContractId = savedId });

        }

        [HttpDelete]
        [Route("DeleteContract/{id}")]
        public ActionResult<ContractDetail> DeleteContract(int id)
        {
            try
            {

                int deletedRow = _contractDetailService.Delete(id);

            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in delete");
            }

            return Ok();
        }

        [HttpPost]
        [Route("SortContracts/{hosId}/{pagenumber}/{pagesize}")]
        public IEnumerable<IndexMasterContractVM.GetData> SortContracts(int hosId, int pagenumber, int pagesize, SortContractsVM sortObj)
        {
            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var list = _masterContractService.SortContracts(hosId, sortObj).ToList();
            return _pagingService.GetAll(pageInfo, list);
        }



        [HttpPost]
        [Route("CreateContractAttachments")]
        public int CreateContractAttachments(ContractAttachment attachObj)
        {
            return _masterContractService.CreateContractAttachments(attachObj);
        }

        [HttpDelete]
        [Route("DeleteContractAttachment/{attachId}")]
        public int DeleteContractAttachment(int attachId)
        {
            return _masterContractService.DeleteContractAttachment(attachId);
        }



        [HttpPost]
        [Route("UploadContractFiles")]
        public ActionResult UploadContractFiles(IFormFile file)
        {
            var folderPath = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/MasterContractFiles";
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


        [HttpGet]
        [Route("GenerateMasterContractSerial")]
        public GeneratedMasterContractNumberVM GenerateMasterContractSerial()
        {
            return _masterContractService.GenerateMasterContractSerial();
        }

        [HttpGet]
        [Route("GetContractAttachmentByMasterContractId/{masterContractId}")]
        public IEnumerable<ContractAttachment> GetContractAttachmentByMasterContractId(int masterContractId)
        {
            return _masterContractService.GetContractAttachmentByMasterContractId(masterContractId);
        }

    }
}
