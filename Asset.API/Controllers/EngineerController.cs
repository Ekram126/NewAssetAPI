using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.EngineerVM;
using Asset.ViewModels.PagingParameter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EngineerController : ControllerBase
    {

        private IEngineerService _EngineerService;
        private IPagingService _pagingService;
        private readonly UserManager<ApplicationUser> _userManager;

        public EngineerController(IEngineerService EngineerService, UserManager<ApplicationUser> userManager,
            IPagingService pagingService)
        { 
            _EngineerService = EngineerService;
            _userManager = userManager;
            _pagingService = pagingService;
        }


        [HttpGet]
        [Route("ListEngineers")]
        public IEnumerable<IndexEngineerVM.GetData> GetAll()
        {
            return _EngineerService.GetAll();
        }
        [HttpPut]
        [Route("ListEngineersWithPaging")]
        public IEnumerable<IndexEngineerVM.GetData> ListEngineersWithPaging(PagingParameter pageInfo)
        {
            var engs = _EngineerService.GetAll().ToList();
            return _pagingService.GetAll<IndexEngineerVM.GetData>(pageInfo, engs);
        }
        [HttpGet]
        [Route("getcount")]
        public int count()
        {  
            return _pagingService.Count<Engineer>();
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public Engineer GetById(int id)
        {
            return _EngineerService.GetById(id);
        }

        [HttpGet]
        [Route("GetByEmail/{email}")]
        public Engineer GetByEmail(string email)
        {
            return _EngineerService.GetByEmail(email);
        }

        [HttpPut]
        [Route("UpdateEngineer")]
        public IActionResult Update(EditEngineerVM EngineerVM)
        {
            try
            {
                var lstEmails = _EngineerService.GetAll().ToList().Where(a => a.Email == EngineerVM.Email && a.Id != EngineerVM.Id).ToList();
                if (lstEmails.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "email", Message = "Email already exist", MessageAr = "هذا البريد الإلكتروني مسجل سابقاً" });
                }

                else
                {
                    int updatedRow = _EngineerService.Update(EngineerVM);
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
        [Route("AddEngineer")]
        public ActionResult<Engineer> Add(CreateEngineerVM EngineerVM)
        {
            var lstEmails = _EngineerService.GetAll().ToList().Where(a => a.Email == EngineerVM.Email).ToList();
            if (lstEmails.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "email", Message = "Email already exist", MessageAr = "هذا البريد الإلكتروني مسجل سابقاً" });
            }

            else
            {
                var savedId = _EngineerService.Add(EngineerVM);
                return CreatedAtAction("GetById", new { id = savedId }, EngineerVM);
            }

        }

        [HttpDelete]
        [Route("DeleteEngineer/{id}")]
        public ActionResult<Engineer> Delete(int id)
        {
            try
            {

                int deletedRow = _EngineerService.Delete(id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in delete");
            }

            return Ok();
        }


        [HttpPost]
        [Route("SortEngineers/{pagenumber}/{pagesize}")]
        public IEnumerable<IndexEngineerVM.GetData> SortEngineers(int pagenumber, int pagesize, SortEngineerVM sortObj)
        {
            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var list = _EngineerService.SortEngineer(sortObj).ToList();
            return _pagingService.GetAll<IndexEngineerVM.GetData>(pageInfo, list);
        }



        [HttpGet]
        [Route("GetUnregisteredEngineerUsers")]
        public IEnumerable<IndexEngineerVM.GetData> GetUnregisteredEngineerUsers()
        {
            List<IndexEngineerVM.GetData> list = new List<IndexEngineerVM.GetData>();
            List<string> lstEngEmails = new List<string>();
            List<string> lstUserEmails = new List<string>();
            List<Engineer> lstUnregisteredEngineers = new List<Engineer>();
            var lstEngineers = _EngineerService.GetAll().ToList();
            var lstUsers = _userManager.Users.ToList();

            foreach (var item in lstEngineers)
            {
                lstEngEmails.Add(item.Email);
            }

            foreach (var usr in lstUsers)
            {
                lstUserEmails.Add(usr.Email);
            }

            var lstEMails = lstEngEmails.Except(lstUserEmails).ToList();

            foreach (var item in lstEMails)
            {
                lstUnregisteredEngineers.Add(_EngineerService.GetAllEngineers().Where(a => a.Email == item).ToList().FirstOrDefault());

            }

            if (lstUnregisteredEngineers.Count > 0)
            {
                list = lstUnregisteredEngineers.Select(item => new IndexEngineerVM.GetData
                {
                    Id = item.Id,
                    Name = item.Name,
                    NameAr = item.NameAr,
                    Email = item.Email,
                    Code = item.Code,
                    GenderId = item.GenderId,
                    WhatsApp = item.WhatsApp,
                    Phone = item.Phone,
                    Address = item.Address,
                    AddressAr = item.AddressAr,
                    CardId = item.CardId
                }).ToList();
            }
            return list;
        }


    }
}
