using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.EmployeeVM;
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
    public class EmployeeController : ControllerBase
    {

        private IEmployeeService _EmployeeService;
        private IPagingService _pagingService;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmployeeController(IEmployeeService EmployeeService, UserManager<ApplicationUser> userManager,
            IPagingService pagingService)
        {
            _EmployeeService = EmployeeService;
            _userManager = userManager;
            _pagingService = pagingService;
        }


        [HttpGet]
        [Route("ListEmployees")]
        public IEnumerable<IndexEmployeeVM.GetData> GetAll()
        {
            return _EmployeeService.GetAll();
        }
        [HttpPut]
        [Route("ListEmployeesWithPaging")]
        public IEnumerable<IndexEmployeeVM.GetData> ListEmployeesWithPaging(PagingParameter pageInfo)
        {
            var emps = _EmployeeService.GetAll().ToList();
            return _pagingService.GetAll<IndexEmployeeVM.GetData>(pageInfo, emps);
        }
        [HttpGet]
        [Route("getcount")]
        public int count()
        {
            return _pagingService.Count<Employee>();
        }



        [HttpGet]
        [Route("GetEmployeesByHospitalId/{hospitalId}")]
        public IEnumerable<Employee> GetEmployeesByHospitalId(int hospitalId)
        {
            return _EmployeeService.GetEmployeesByHospitalId(hospitalId);
        }

        [HttpGet]
        [Route("GetEmployeesEngineersByHospitalId/{hospitalId}")]
        public IEnumerable<EmployeeEngVM> GetEmployeesEngineersByHospitalId(int hospitalId)
        {
            return _EmployeeService.GetEmployeesEngineersByHospitalId(hospitalId);
        }

        [HttpGet]
        [Route("GetEmployeesAssetOwnerByHospitalId/{hospitalId}")]
        public IEnumerable<Employee> GetEmployeesAssetOwnerByHospitalId(int hospitalId)
        {
            return _EmployeeService.GetEmployeesAssetOwnerByHospitalId(hospitalId);
        }



        [HttpGet]
        [Route("GetEmployeesHasEngRoleInHospital/{hospitalId}")]
        public IEnumerable<EmployeeEngVM> GetEmployeesHasEngRoleInHospital(int hospitalId)
        {
            return _EmployeeService.GetEmployeesHasEngRoleInHospital(hospitalId);
        }




        [HttpGet]
        [Route("GetEmployeesHasEngDepManagerRoleInHospital/{hospitalId}")]
        public IEnumerable<EmployeeEngVM> GetEmployeesHasEngDepManagerRoleInHospital(int hospitalId)
        {
            return _EmployeeService.GetEmployeesHasEngDepManagerRoleInHospital(hospitalId);
        }


        [HttpGet]
        [Route("GetEmployeesAssetOwnerByHospitalAndAssetDetailId/{hospitalId}/{assetDetailId}")]
        public IEnumerable<Employee> GetEmployeesAssetOwnerByHospitalId(int hospitalId, int assetDetailId)
        {
            return _EmployeeService.GetEmployeesAssetOwnerByHospitalId(hospitalId, assetDetailId);
        }




        [HttpGet]
        [Route("GetUnregisteredUsers/{hospitalId}")]
        public IEnumerable<IndexEmployeeVM.GetData> GetUnregisteredUsers(int? hospitalId)
        {

            List<IndexEmployeeVM.GetData> list = new List<IndexEmployeeVM.GetData>();
            var removeRegistered = _EmployeeService.GetAll().Where(a => a.HospitalId == hospitalId).ToList();
            if (hospitalId > 0)
            {
                var users = _userManager.Users.Where(a => a.HospitalId == hospitalId).ToList();            
                var employees = _EmployeeService.GetAll().Where(a => a.HospitalId == hospitalId).ToList();

                List<string> usersEmails = new List<string>();
                List<string> employeeEmails = new List<string>();

                foreach (var item in users)
                {
                    usersEmails.Add(item.Email);
                }

                foreach (var item in employees)
                {
                    employeeEmails.Add(item.Email);
                }

                List<string> remainEmails  = employeeEmails.Except(usersEmails).ToList();

                foreach (var item in remainEmails)
                {
                    list.Add(_EmployeeService.GetAll().Where(a => a.Email == item).FirstOrDefault());
                }

                //foreach (var employee in employees)
                //{
                //    foreach (var user in users)
                //    {
                //        if (employee.Email == user.Email)
                //            removeRegistered.Remove(employee);
                //    }
                //}
                return list;
            }
            return list;
        }




        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<EditEmployeeVM> GetById(int id)
        {
            return _EmployeeService.GetById(id);
        }



        [HttpPut]
        [Route("UpdateEmployee")]
        public IActionResult Update(EditEmployeeVM EmployeeVM)
        {
            try
            {
                var lstEmails = _EmployeeService.GetAll().ToList().Where(a => a.Email == EmployeeVM.Email && a.Id != EmployeeVM.Id).ToList();
                if (lstEmails.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "email", Message = "Email already exist", MessageAr = "هذا البريد الإلكتروني مسجل سابقاً" });
                }
                //var lstPhones = _EmployeeService.GetAll().ToList().Where(a => a.Phone == EmployeeVM.Phone && a.Id != EmployeeVM.Id).ToList();
                //if (lstPhones.Count > 0)
                //{
                //    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "phone", Message = "Phone already exist", MessageAr = "هذا المحمول مسجل سابقاً" });
                //}
                //var lstWhatsApps = _EmployeeService.GetAll().ToList().Where(a => a.WhatsApp == EmployeeVM.WhatsApp && a.Id != EmployeeVM.Id).ToList();
                //if (lstWhatsApps.Count > 0)
                //{
                //    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "whatsApp", Message = "WhatsApp already exist", MessageAr = "هذا الواتس اب مسجل سابقاً" });
                //}
                else
                {
                    int updatedRow = _EmployeeService.Update(EmployeeVM);
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
        [Route("AddEmployee")]
        public ActionResult<Employee> Add(CreateEmployeeVM EmployeeVM)
        {
            var lstEmails = _EmployeeService.GetAll().ToList().Where(a => a.Email == EmployeeVM.Email).ToList();
            if (lstEmails.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "email", Message = "Email already exist", MessageAr = "هذا البريد الإلكتروني مسجل سابقاً" });
            }
            //var lstPhones= _EmployeeService.GetAll().ToList().Where(a => a.Phone == EmployeeVM.Phone).ToList();
            //if (lstPhones.Count > 0)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "phone", Message = "Phone already exist", MessageAr = "هذا المحمول مسجل سابقاً" });
            //}
            //var lstWhatsApps = _EmployeeService.GetAll().ToList().Where(a => a.WhatsApp == EmployeeVM.WhatsApp).ToList();
            //if (lstWhatsApps.Count > 0)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "whatsApp", Message = "WhatsApp already exist", MessageAr = "هذا الواتس اب مسجل سابقاً" });
            //}
            else
            {
                var savedId = _EmployeeService.Add(EmployeeVM);
                return CreatedAtAction("GetById", new { id = savedId }, EmployeeVM);
            }

        }

        [HttpDelete]
        [Route("DeleteEmployee/{id}")]
        public ActionResult<Employee> Delete(int id)
        {
            try
            {

                int deletedRow = _EmployeeService.Delete(id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in delete");
            }

            return Ok();
        }

        [HttpPost]
        [Route("SortEmployees/{pagenumber}/{pagesize}")]
        public IEnumerable<IndexEmployeeVM.GetData> SortEmployees(int pagenumber, int pagesize, SortEmployeeVM sortObj)
        {
            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var list = _EmployeeService.SortEmployee(sortObj).ToList();
            return _pagingService.GetAll<IndexEmployeeVM.GetData>(pageInfo, list);
        }
    }
}
