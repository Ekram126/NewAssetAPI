using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.PagingParameter;
using Asset.ViewModels.SupplierVM;
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
    public class SupplierController : ControllerBase
    {
        private IAssetDetailService _assetDetailService;
        private IMasterContractService _masterContractService;
        private ISupplierService _SupplierService;
        private IPagingService _pagingService;
        IWebHostEnvironment _webHostingEnvironment;

        public SupplierController(ISupplierService SupplierService, IAssetDetailService assetDetailService, IMasterContractService masterContractService, IPagingService pagingService, IWebHostEnvironment webHostingEnvironment)
        {
            _masterContractService = masterContractService;
            _assetDetailService = assetDetailService;
            _SupplierService = SupplierService;
            _pagingService = pagingService;
            _webHostingEnvironment = webHostingEnvironment;
        }


        [HttpGet]
        [Route("ListSuppliers")]
        public IEnumerable<IndexSupplierVM.GetData> GetAll()
        {
            return _SupplierService.GetAll();
        }


        [HttpPost]
        [Route("GetAllSuppliersWithPaging/{pagenumber}/{pagesize}")]
        public IndexSupplierVM GetAllSuppliersWithPaging(int pagenumber, int pagesize)
        {
            return _SupplierService.GetAllSuppliersWithPaging(pagenumber, pagesize);
        }

        [HttpGet]
        [Route("GetTop10Suppliers/{hospitalId}")]
        public IEnumerable<IndexSupplierVM.GetData> GetTop10Suppliers(int hospitalId)
        {
            return _SupplierService.GetTop10Suppliers(hospitalId);
        }
        [HttpGet]
        [Route("GetTop10SuppliersCount/{hospitalId}")]
        public int GetTop10SuppliersCount(int hospitalId)
        {
            return _SupplierService.GetTop10Suppliers(hospitalId).ToList().Count;
        }


        [HttpPost]
        [Route("SortSuppliers/{pagenumber}/{pagesize}")]
        public IEnumerable<IndexSupplierVM.GetData> SortAssets(int pagenumber, int pagesize, SortSupplierVM sortObj)
        {
            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var list = _SupplierService.SortSuppliers(sortObj);
            return _pagingService.GetAll<IndexSupplierVM.GetData>(pageInfo, list.ToList());
        }

        [HttpPut]
        [Route("GetSuppliersWithPaging")]
        public IEnumerable<IndexSupplierVM.GetData> GetAll(PagingParameter pageInfo)
        {
            var lstSuppliers = _SupplierService.GetAll().ToList();
            return _pagingService.GetAll<IndexSupplierVM.GetData>(pageInfo, lstSuppliers);
        }


        [HttpPost]
        [Route("FindSupplier/{pagenumber}/{pagesize}")]
        public IndexSupplierVM FindSupplier(string strText, int pageNumber, int pageSize)
        {
            var lstSuppliers = _SupplierService.FindSupplier(strText, pageNumber, pageSize);
            return lstSuppliers;
        }



        [HttpGet]
        [Route("FindSupplierByText")]
        public IEnumerable<IndexSupplierVM.GetData> FindSupplierByText(string strText)
        {
            var lstSuppliers = _SupplierService.FindSupplierByText(strText);
            return lstSuppliers;
        }



        [HttpGet]
        [Route("getcount")]
        public int count()
        {
            return _SupplierService.GetAll().ToList().Count;
        }



        [HttpGet]
        [Route("CountSuppliers")]
        public int CountSuppliers()
        {
            return _SupplierService.CountSuppliers();
        }




        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<EditSupplierVM> GetById(int id)
        {
            return _SupplierService.GetById(id);
        }



        [HttpPut]
        [Route("UpdateSupplier")]
        public IActionResult Update(EditSupplierVM SupplierVM)
        {
            try
            {
                int id = SupplierVM.Id;
                var lstCitiesCode = _SupplierService.GetAllSuppliers().ToList().Where(a => a.Code == SupplierVM.Code && a.Id != id).ToList();
                if (lstCitiesCode.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "Supplier code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
                }
                var lstCitiesNames = _SupplierService.GetAllSuppliers().ToList().Where(a => a.Name == SupplierVM.Name && a.Id != id).ToList();
                if (lstCitiesNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Supplier name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }
                var lstCitiesArNames = _SupplierService.GetAllSuppliers().ToList().Where(a => a.NameAr == SupplierVM.NameAr && a.Id != id).ToList();
                if (lstCitiesArNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Supplier arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }

                else
                {
                    int updatedRow = _SupplierService.Update(SupplierVM);
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in update");
            }

            return Ok();
        }


        [HttpPost]
        [Route("AddSupplier")]
        public ActionResult Add(CreateSupplierVM SupplierVM)
        {
            if (SupplierVM.Code.Length > 5)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "codelen", Message = "Supplier code must not exceed 5 characters", MessageAr = "هذا الكود لا يتعدي 5 حروف وأرقام" });

            }
            var lstOrgCode = _SupplierService.GetAllSuppliers().ToList().Where(a => a.Code == SupplierVM.Code).ToList();
            if (lstOrgCode.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "Supplier code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
            }
            var lstOrgNames = _SupplierService.GetAllSuppliers().ToList().Where(a => a.Name == SupplierVM.Name).ToList();
            if (lstOrgNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Supplier name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            var lstCitiesArNames = _SupplierService.GetAllSuppliers().ToList().Where(a => a.NameAr == SupplierVM.NameAr).ToList();
            if (lstCitiesArNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "nameAr", Message = "Supplier arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            else
            {
                var savedId = _SupplierService.Add(SupplierVM);
                return Ok(savedId);// CreatedAtAction("GetById", new { id = savedId }, SupplierVM);
            }
        }


        [HttpPost]
        [Route("CreateSupplierAttachment")]
        public int CreateRequestAttachments(SupplierAttachment attachObj)
        {
            return _SupplierService.CreateSupplierAttachment(attachObj);
        }


        [HttpGet]
        [Route("GetSupplierAttachmentsBySupplierId/{supplierId}")]
        public List<SupplierAttachment> GetSupplierAttachmentsBySupplierId(int supplierId)
        {
            return _SupplierService.GetSupplierAttachmentsBySupplierId(supplierId);
        }



        [HttpPost]
        [Route("UploadSupplierFile")]
        public ActionResult UploadSupplierFile(IFormFile file)
        {
            var folderPath = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/SupplierAttachments";
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







        [HttpDelete]
        [Route("DeleteSupplier/{id}")]
        public ActionResult<Supplier> Delete(int id)
        {
            try
            {
                var supplierObj = _SupplierService.GetById(id);
                var lstHospitalAssets = _assetDetailService.GetAll().Where(a => a.SupplierId == supplierObj.Id).ToList();
                if (lstHospitalAssets.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "hostassets", Message = "Hospital Assets has this supplier", MessageAr = "أصول المستشفى بها منتجات من هذا المورد" });
                }
                var lstMasterContracts = _masterContractService.GetAll().Where(a => a.SupplierId == supplierObj.Id).ToList();
                if (lstMasterContracts.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "contract", Message = "Contract has this supplier", MessageAr = "العقد به هذا المورد" });
                }
                else
                {
                    int deletedRow = _SupplierService.Delete(id);
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in delete");
            }

            return Ok();
        }




        [HttpGet("GenerateSupplierCode")]
        public GenerateSupplierCodeVM GenerateSupplierCode()
        {
            return _SupplierService.GenerateSupplierCode();
        }


    }
}
