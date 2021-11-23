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

        [HttpPut]
        [Route("ListUsersWithPaging")]
        public IEnumerable<IndexUserVM.GetData> GetUsers(PagingParameter paging)
        {
            List<IndexUserVM.GetData> lstUsers = new List<IndexUserVM.GetData>();
            var userlist = _applicationUser.Users.ToList();
            var users = _pagingService.GetAll<ApplicationUser>(paging, userlist);
            foreach (var item in users)
            {
                IndexUserVM.GetData userObj = new IndexUserVM.GetData();
                userObj.Id = item.Id;
                userObj.UserName = item.UserName;
                var roleNames = (from userRole in _context.UserRoles
                                 join role in _context.ApplicationRole on userRole.RoleId equals role.Id
                                 where userRole.UserId == item.Id
                                 select role.Name).ToList();

                foreach (var roleName in roleNames)
                {
                    userObj.DisplayName = string.Join(",", roleName);
                }




                userObj.CategoryRoleName = _roleCategoryService.GetById((int)item.RoleCategoryId).Name;
                userObj.PhoneNumber = item.PhoneNumber;
                userObj.Email = item.Email;
                lstUsers.Add(userObj);
            }
            return lstUsers;
        }
        [HttpGet]
        [Route("getcount")]
        public int count()
        {
            return _pagingService.Count<ApplicationUser>();
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
            }
            return Ok();
        }


        [HttpPut]
        [Route("UpdateUser")]
        public async Task<IActionResult> Update(ApplicationUser userObj)
        {
            var updateObj = await _context.ApplicationUser.FindAsync(userObj.Id);
            updateObj.UserName = userObj.UserName;
            updateObj.Email = userObj.Email;
            updateObj.PhoneNumber = userObj.PhoneNumber;
            updateObj.GovernorateId = userObj.GovernorateId;
            updateObj.CityId = userObj.CityId;
            updateObj.OrganizationId = userObj.OrganizationId;
            updateObj.SubOrganizationId = userObj.SubOrganizationId;
            updateObj.HospitalId = userObj.HospitalId;
            updateObj.RoleCategoryId = userObj.RoleCategoryId;
            updateObj.RoleId = userObj.RoleId;
            //  var token = await _applicationUser.GeneratePasswordResetTokenAsync(updateObj);
            //   var result = await _applicationUser.ResetPasswordAsync(updateObj, token, updateObj.PasswordHash);

            var result = await _applicationUser.UpdateAsync(updateObj);


            if (userObj.RoleIds.Count > 0)
            {
                var savedRoleIds = (from userRole in _context.UserRoles
                                    join role in _roleManager.Roles on userRole.RoleId equals role.Id
                                    where userObj.Id == userRole.UserId
                                    select userRole).ToList().Select(a => a.RoleId).ToList();

                var savedIds = savedRoleIds.Except(userObj.RoleIds);
                if (savedIds.Count() > 0)
                {
                    foreach (var item in savedIds)
                    {
                        var row = _context.UserRoles.Where(a => a.RoleId == item && a.UserId == userObj.Id).ToList();
                        if (row.Count > 0)
                        {
                            var roleUserObj = row[0];
                            _context.UserRoles.Remove(roleUserObj);
                            _context.SaveChanges();
                        }
                    }
                }
                var neewIds = userObj.RoleIds.Except(savedIds);
                if (neewIds.Count() > 0)
                {
                    foreach (var itm in neewIds)
                    {
                        var roleName = _context.ApplicationRole.Where(a => a.Id == itm).FirstOrDefault().Name;
                        await _applicationUser.AddToRoleAsync(updateObj, roleName);
                    }
                }

            }
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
