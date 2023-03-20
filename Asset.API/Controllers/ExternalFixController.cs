using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.ExternalFixFileVM;
using Asset.ViewModels.ExternalFixVM;
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

namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExternalFixController : ControllerBase
    {

        private IExternalFixService _ExternalFixService;
        IWebHostEnvironment _webHostingEnvironment;

        private IAssetStatusTransactionService _assetStatusTransactionService;
        public ExternalFixController(IExternalFixService ExternalFixService,
            IAssetStatusTransactionService assetStatusTransactionService, IWebHostEnvironment webHostingEnvironment)
        {
            _ExternalFixService = ExternalFixService;
            _assetStatusTransactionService = assetStatusTransactionService;
            _webHostingEnvironment = webHostingEnvironment;
        }


        [HttpGet]
        [Route("ListExternalFixes")]
        public IEnumerable<IndexExternalFixVM.GetData> GetAll()
        {
            return _ExternalFixService.GetAll();
        }



        [HttpGet]
        [Route("GetAllWithPaging/{hospitalId}/{pageNumber}/{pageSize}")]
        public IndexExternalFixVM GetAllWithPaging(int hospitalId, int pageNumber, int pageSize)
        {
            return _ExternalFixService.GetAllWithPaging(hospitalId, pageNumber, pageSize);
        }







        [HttpPost]
        [Route("SearchInExternalFix/{pageNumber}/{pageSize}")]
        public IndexExternalFixVM SearchInExternalFix(SearchExternalFixVM searchObj, int pageNumber, int pageSize)
        {
            return _ExternalFixService.SearchInExternalFix(searchObj, pageNumber, pageSize);
        }



        [HttpPost]
        [Route("SortExternalFix/{pageNumber}/{pageSize}")]
        public IndexExternalFixVM SortExternalFix(SortExternalFixVM sortObj, int pageNumber, int pageSize)
        {
            return _ExternalFixService.SortExternalFix(sortObj, pageNumber, pageSize);
        }






        [HttpGet]
        [Route("GetAssetsExceed72HoursInExternalFix/{hospitalId}/{pageNumber}/{pageSize}")]
        public IndexExternalFixVM GetAssetsExceed72HoursInExternalFix(int hospitalId, int pageNumber, int pageSize)
        {
            return _ExternalFixService.GetAssetsExceed72HoursInExternalFix(hospitalId, pageNumber, pageSize);
        }



        [HttpGet]
        [Route("GenerateExternalFixNumber")]
        public GenerateExternalFixNumberVM GenerateExternalFixNumber()
        {
            return _ExternalFixService.GenerateExternalFixNumber();
        }




        [HttpDelete]
        [Route("DeleteExternalFix/{id}")]
        public ActionResult<ExternalFix> Delete(int id)
        {
            try
            {

                int deletedRow = _ExternalFixService.Delete(id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in delete");
            }

            return Ok();

        }
        [HttpPost]
        [Route("AddExternalFix")]
        public int Add(CreateExternalFixVM externalFixVM)
        {

            var savedId = _ExternalFixService.Add(externalFixVM);
            return savedId;

        }



        [HttpPost]
        [Route("CreateExternalFixFile")]
        public int CreateExternalFixFiles(CreateExternalFixFileVM attachObj)
        {
            return _ExternalFixService.AddExternalFixFile(attachObj);
        }

        [HttpPost]
        [Route("UploadExternalFixFiles")]
        public ActionResult UploadExternalFixFiles(IFormFile file)
        {
            var folderPath = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/ExternalFixFiles";
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
        [Route("ViewExternalFixById/{externalFixId}")]
        public ActionResult<ViewExternalFixVM> ViewExternalFixById(int externalFixId)
        {
            return _ExternalFixService.ViewExternalFixById(externalFixId);
        }
        [HttpPut]
        [Route("UpdateExternalFixFile")]
        public void Put(EditExternalFixVM editExternalFixVMObj)
        {
            _ExternalFixService.Update(editExternalFixVMObj);
        }



    }
}
