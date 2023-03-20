using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.PagingParameter;
using Asset.ViewModels.UserVM;
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
    public class UserController : ControllerBase
    {

        UserManager<ApplicationUser> _applicationUser;
        RoleManager<ApplicationRole> _roleManager;
        private IRoleCategoryService _roleCategoryService;
        private IPagingService _pagingService;
        private readonly ApplicationDbContext _context;
        public UserController(UserManager<ApplicationUser> applicationUser, RoleManager<ApplicationRole> roleManager,
            ApplicationDbContext context, IRoleCategoryService roleCategoryService, IPagingService pagingService)
        {
            _applicationUser = applicationUser;
            _roleManager = roleManager;
            _roleCategoryService = roleCategoryService;
            _context = context;
            _pagingService = pagingService;
        }


        [HttpGet]
        [Route("ListUsers")]
        public List<IndexUserVM.GetData> Index()
        {
            List<IndexUserVM.GetData> lstUsers = new List<IndexUserVM.GetData>();
            var users = _applicationUser.Users.ToList();
            foreach (var item in users)
            {
                IndexUserVM.GetData userObj = new IndexUserVM.GetData();
                userObj.Id = item.Id;
                userObj.UserName = item.UserName;
                userObj.DisplayName = _context.ApplicationRole.Where(a => a.Id == item.RoleId).First().DisplayName;
                userObj.CategoryRoleName = _roleCategoryService.GetById((int)item.RoleCategoryId).Name;
                userObj.PhoneNumber = item.PhoneNumber;
                userObj.Email = item.Email;
                lstUsers.Add(userObj);
            }
            return lstUsers;
        }

        [HttpPost]
        [Route("ListUsersWithPaging")]
        public IEnumerable<IndexUserVM.GetData> GetUsers(PagingParameter paging)
        {
            List<IndexUserVM.GetData> lstUsers = new List<IndexUserVM.GetData>();
            var userlist = _applicationUser.Users.ToList();
            //var users = _pagingService.GetAll<ApplicationUser>(paging, userlist);
            foreach (var item in userlist)
            {
                IndexUserVM.GetData userObj = new IndexUserVM.GetData();
                userObj.Id = item.Id;
                userObj.UserName = item.UserName;
                var roleNames = (from userRole in _context.UserRoles
                                 join role in _context.ApplicationRole on userRole.RoleId equals role.Id
                                 where userRole.UserId == item.Id
                                 select role.Name).ToList();
                string strRoles = "";
                var list = new List<string>();
                foreach (var role in roleNames)
                {
                    list.Add(role);
                }

                strRoles = string.Join<string>(",", list);
                userObj.DisplayName = strRoles;
                userObj.CategoryRoleName = _roleCategoryService.GetById((int)item.RoleCategoryId).Name;
                userObj.PhoneNumber = item.PhoneNumber;
                userObj.Email = item.Email;
                lstUsers.Add(userObj);
            }


            return _pagingService.GetAll<IndexUserVM.GetData>(paging, lstUsers);
            //  return lstUsers;
        }




        //[HttpPost]
        //[Route("SortHospitals/{pagenumber}/{pagesize}")]
        //public IEnumerable<IndexHospitalVM.GetData> SortHospitals(int pagenumber, int pagesize, SortVM sortObj)
        //{
        //    PagingParameter pageInfo = new PagingParameter();
        //    pageInfo.PageNumber = pagenumber;
        //    pageInfo.PageSize = pagesize;
        //    var list = _HospitalService.SortHospitals(sortObj).ToList();
        //    return _pagingService.GetAll<IndexHospitalVM.GetData>(pageInfo, list);
        //}

        [HttpPost]
        [Route("SortUsers/{pagenumber}/{pagesize}")]
        public IEnumerable<IndexUserVM.GetData> SortUsers(int pagenumber, int pagesize, UserSortVM sortObj)
        {

            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            List<IndexUserVM.GetData> lstUsers = new List<IndexUserVM.GetData>();
            var userlist = _applicationUser.Users.ToList();
            foreach (var item in userlist)
            {
                IndexUserVM.GetData userObj = new IndexUserVM.GetData();
                userObj.Id = item.Id;
                userObj.UserName = item.UserName;
                var roleNames = (from userRole in _context.UserRoles
                                 join role in _context.ApplicationRole on userRole.RoleId equals role.Id
                                 where userRole.UserId == item.Id
                                 select role.Name).ToList();
                string strRoles = "";
                var list = new List<string>();
                foreach (var role in roleNames)
                {
                    list.Add(role);
                }

                strRoles = string.Join<string>(",", list);
                userObj.DisplayName = strRoles;
                userObj.CategoryRoleName = _roleCategoryService.GetById((int)item.RoleCategoryId).Name;
                userObj.PhoneNumber = item.PhoneNumber;
                userObj.Email = item.Email;
                lstUsers.Add(userObj);
            }

            if (sortObj.UserName != "")
            {
                if (sortObj.SortStatus == "descending")
                    lstUsers = lstUsers.OrderByDescending(d => d.UserName).ToList();
                else
                    lstUsers = lstUsers.OrderBy(d => d.UserName).ToList();
            }
            if (sortObj.PhoneNumber != "")
            {
                if (sortObj.SortStatus == "descending")
                    lstUsers = lstUsers.OrderByDescending(d => d.PhoneNumber).ToList();
                else
                    lstUsers = lstUsers.OrderBy(d => d.PhoneNumber).ToList();
            }
            if (sortObj.DisplayName != "")
            {
                if (sortObj.SortStatus == "descending")
                    lstUsers = lstUsers.OrderByDescending(d => d.DisplayName).ToList();
                else
                    lstUsers = lstUsers.OrderBy(d => d.DisplayName).ToList();
            }

            if (sortObj.DisplayNameAr != "")
            {
                if (sortObj.SortStatus == "descending")
                    lstUsers = lstUsers.OrderByDescending(d => d.DisplayName).ToList();
                else
                    lstUsers = lstUsers.OrderBy(d => d.DisplayName).ToList();
            }




            if (sortObj.Email != "")
            {
                if (sortObj.SortStatus == "descending")
                    lstUsers = lstUsers.OrderByDescending(d => d.Email).ToList();
                else
                    lstUsers = lstUsers.OrderBy(d => d.Email).ToList();
            }

            if (sortObj.CategoryRoleName != "")
            {
                if (sortObj.SortStatus == "descending")
                    lstUsers = lstUsers.OrderByDescending(d => d.CategoryRoleName).ToList();
                else
                    lstUsers = lstUsers.OrderBy(d => d.CategoryRoleName).ToList();
            }



            return _pagingService.GetAll<IndexUserVM.GetData>(pageInfo, lstUsers);
        }



        [HttpGet]
        [Route("getcount")]
        public int count()
        {
            return _applicationUser.Users.ToList().Count;
        }

        [HttpGet]
        [Route("SortCount")]
        public int SortCount()
        {
            return _applicationUser.Users.ToList().Count;
        }




        [HttpGet]
        [Route("ListUsersByHospitalId/{HospitalId}")]
        public List<IndexUserVM.GetData> ListUsersByHospitalId(int HospitalId)
        {
            List<IndexUserVM.GetData> lstUsers = new List<IndexUserVM.GetData>();
            var users = _applicationUser.Users.Where(a => a.HospitalId == HospitalId).ToList();
            foreach (var item in users)
            {
                IndexUserVM.GetData userObj = new IndexUserVM.GetData();
                userObj.Id = item.Id;
                userObj.UserName = item.UserName;
                lstUsers.Add(userObj);
            }
            return lstUsers;
        }








        [HttpGet]
        [Route("ListUsersInHospitalByEngRoleName/{hospitalId}")]
        public async Task<List<IndexUserVM.GetData>> ListUsersInHospitalByRoleName(int hospitalId)
        {

            var roleObj = await _roleManager.FindByNameAsync("Eng");
            List<IndexUserVM.GetData> lstUsers = new List<IndexUserVM.GetData>();
            var users = _applicationUser.Users.Where(a => a.HospitalId == hospitalId && a.RoleId == roleObj.Id).ToList();

            foreach (var item in users)
            {
                IndexUserVM.GetData userObj = new IndexUserVM.GetData();
                userObj.Id = item.Id;
                userObj.UserName = item.UserName;
                lstUsers.Add(userObj);
            }
            return lstUsers;
        }



        [HttpGet]
        [Route("ListUsersInHospitalByEngManagerRoleName/{hospitalId}")]
        public async Task<List<IndexUserVM.GetData>> ListUsersInHospitalByEngManageRoleName(int hospitalId)
        {

            var roleEngManagerObj = await _roleManager.FindByNameAsync("EngManager");
            var roleEngDepManagerObj = await _roleManager.FindByNameAsync("EngManager");
            List<IndexUserVM.GetData> lstUsers = new List<IndexUserVM.GetData>();
            var users = _applicationUser.Users.Where(a => a.HospitalId == hospitalId && (a.RoleId == roleEngManagerObj.Id || a.RoleId == roleEngDepManagerObj.Id)).ToList();

            foreach (var item in users)
            {
                IndexUserVM.GetData userObj = new IndexUserVM.GetData();
                userObj.Id = item.Id;
                userObj.UserName = item.UserName;
                lstUsers.Add(userObj);
            }
            return lstUsers;
        }




        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<ApplicationUser> GetById(string id)
        {
            var userObj = await _applicationUser.FindByIdAsync(id);

            var RoleIds = (from userRole in _context.UserRoles
                           join role in _roleManager.Roles on userRole.RoleId equals role.Id
                           where userObj.Id == userRole.UserId
                           select role.Id).ToList();

            userObj.RoleIds = RoleIds;
          //  userObj.HospitalId
            return userObj;
        }




        [HttpPost]
        [Route("AddUser")]
        public async Task<IActionResult> Create(ApplicationUser userObj)
        {
            var userExists = await _applicationUser.FindByNameAsync(userObj.UserName);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });


            ApplicationUser user = new ApplicationUser();
            user.UserName = userObj.UserName;
            user.PasswordHash = userObj.PasswordHash;
            user.Email = userObj.Email;
            user.PhoneNumber = userObj.PhoneNumber;
            user.GovernorateId = userObj.GovernorateId;
            user.CityId = userObj.CityId;
            user.OrganizationId = userObj.OrganizationId;
            user.SubOrganizationId = userObj.SubOrganizationId;
            user.HospitalId = userObj.HospitalId;
            user.RoleCategoryId = userObj.RoleCategoryId;
            user.RoleId = userObj.RoleId;
            user.SupplierId = userObj.SupplierId;
            user.CommetieeMemberId = userObj.CommetieeMemberId;
            var userResult = await _applicationUser.CreateAsync(user, user.PasswordHash);


            if (!userResult.Succeeded)
            {
                foreach (var error in userResult.Errors)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = error.Description, MessageAr = error.Description });
                }
            }
            else
            {
                foreach (var role in userObj.RoleIds)
                {
                    var roleName = _context.ApplicationRole.Where(a => a.Id == role).FirstOrDefault().Name;
                    await _applicationUser.AddToRoleAsync(user, roleName);
                }

                //foreach (var role in userObj.userRoleIds)
                //{
                //    var roleName = _context.ApplicationRole.Where(a => a.Id == role.Id).FirstOrDefault().Name;
                //    await _applicationUser.AddToRoleAsync(user, roleName);
                //}
            }
            return Ok();
        }


        [HttpPut]
        [Route("UpdateUser")]
        public async Task<IActionResult> Update(ApplicationUser userObj)
        {
            var updateObj = await _context.ApplicationUser.FindAsync(userObj.Id);
            if (userObj.RoleIds.Count > 0)
            {
                var lstUserRoles = _context.UserRoles.Where(a => a.UserId == userObj.Id).ToList();
                foreach (var roleUserObj in lstUserRoles)
                {
                    _context.UserRoles.Remove(roleUserObj);
                    _context.SaveChanges();
                }


                foreach (var itm in userObj.RoleIds)
                {
                    var roleName = _context.ApplicationRole.Where(a => a.Id == itm).FirstOrDefault().Name;
                    if (roleName == "VisitEngineer")
                    {
                        var lstEngineers = _context.Engineers.Where(a => a.Email == updateObj.Email).ToList();
                        if (lstEngineers.Count > 0)
                        {
                            updateObj.Email = userObj.Email;

                            var engObj = lstEngineers[0];
                            engObj.Email = userObj.Email;
                            engObj.Phone = userObj.PhoneNumber;
                            engObj.WhatsApp = userObj.PhoneNumber;
                            _context.Entry(engObj).State = EntityState.Modified;
                            _context.SaveChanges();
                        }
                    }
                    if (roleName == "VisitEngineerManager")
                    {
                        var lstEngineers = _context.Engineers.Where(a => a.Email == updateObj.Email).ToList();
                        if (lstEngineers.Count > 0)
                        {
                            updateObj.Email = userObj.Email;

                            var engObj = lstEngineers[0];
                            engObj.Email = userObj.Email;
                            engObj.Phone = userObj.PhoneNumber;
                            engObj.WhatsApp = userObj.PhoneNumber;
                            _context.Entry(engObj).State = EntityState.Modified;
                            _context.SaveChanges();
                        }
                    }
                    if (roleName == "AssetOwner")
                    {
                        var lstEmployees = _context.Employees.Where(a => a.Email == updateObj.Email).ToList();
                        if (lstEmployees.Count > 0)
                        {
                            updateObj.Email = userObj.Email;

                            var empObj = lstEmployees[0];
                            empObj.Email = userObj.Email;
                            empObj.Phone = userObj.PhoneNumber;
                            empObj.WhatsApp = userObj.PhoneNumber;
                            _context.Entry(empObj).State = EntityState.Modified;
                            _context.SaveChanges();
                        }
                    }

                    if (roleName == "EngDepManager")
                    {
                        var lstEmployees = _context.Employees.Where(a => a.Email == updateObj.Email).ToList();
                        if (lstEmployees.Count > 0)
                        {
                            updateObj.Email = userObj.Email;

                            var empObj = lstEmployees[0];
                            empObj.Email = userObj.Email;
                            empObj.Phone = userObj.PhoneNumber;
                            empObj.WhatsApp = userObj.PhoneNumber;
                            _context.Entry(empObj).State = EntityState.Modified;
                            _context.SaveChanges();
                        }
                    }
                    if (roleName == "TLHospitalManager")
                    {
                        var lstEmployees = _context.Employees.Where(a => a.Email == updateObj.Email).ToList();
                        if (lstEmployees.Count > 0)
                        {
                            updateObj.Email = userObj.Email;

                            var empObj = lstEmployees[0];
                            empObj.Email = userObj.Email;
                            empObj.Phone = userObj.PhoneNumber;
                            empObj.WhatsApp = userObj.PhoneNumber;
                            _context.Entry(empObj).State = EntityState.Modified;
                            _context.SaveChanges();
                        }
                    }
                   else
                    {

                        updateObj.Email = userObj.Email;
                        updateObj.PhoneNumber = userObj.PhoneNumber;
                    }
                    await _applicationUser.AddToRoleAsync(updateObj, roleName);
                }

            }
            updateObj.UserName = userObj.UserName;
            updateObj.GovernorateId = userObj.GovernorateId;
            updateObj.CityId = userObj.CityId;
            updateObj.OrganizationId = userObj.OrganizationId;
            updateObj.SubOrganizationId = userObj.SubOrganizationId;
            updateObj.HospitalId = userObj.HospitalId;
            updateObj.RoleCategoryId = userObj.RoleCategoryId;
            updateObj.RoleId = userObj.RoleId;

            var result = await _applicationUser.UpdateAsync(updateObj);
            return Ok(new Response { Status = "Success", Message = "User updated successfully!" });

            //await _applicationUser.UpdateAsync(updateObj);
            //return Ok();
        }

        [HttpDelete]
        [Route("DeleteUser/{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var deleteRoleObj = await _applicationUser.FindByIdAsync(id);
                await _applicationUser.DeleteAsync(deleteRoleObj);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in delete");
            }

            return Ok();
        }

    }




}
