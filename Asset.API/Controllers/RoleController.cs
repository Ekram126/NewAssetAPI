using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.PagingParameter;
using Asset.ViewModels.RoleVM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
    public class RoleController : ControllerBase
    {

        RoleManager<ApplicationRole> _applicationRole;
        private readonly ApplicationDbContext _context;
        private IPagingService _pagingService;
        private IRoleCategoryService _roleCategoryService;

        public RoleController(ApplicationDbContext context, RoleManager<ApplicationRole> applicationRole,
            IRoleCategoryService roleCategoryService, IPagingService pagingService)
        {
            _context = context;
            _applicationRole = applicationRole;
            _roleCategoryService = roleCategoryService;
            _pagingService = pagingService;
        }


        [HttpPut]
        [Route("ListRolesWithPages")]
        public List<IndexRoleVM.GetData> Index(PagingParameter pageInfo)
        {
            List<IndexRoleVM.GetData> lstRoles = new List<IndexRoleVM.GetData>();
            var rlst = _context.ApplicationRole.ToList();
            var roles = _pagingService.GetAll<ApplicationRole>(pageInfo, rlst);
            foreach (var item in roles)
            {
                IndexRoleVM.GetData roleObj = new IndexRoleVM.GetData();
                roleObj.Id = item.Id;
                roleObj.Name = item.Name;
                roleObj.DisplayName = item.DisplayName;
                roleObj.CategoryName = _roleCategoryService.GetById(item.RoleCategoryId).Name;
                lstRoles.Add(roleObj);
            }
            return lstRoles;
        }



        [HttpGet]
        [Route("ListRoles")]
        public List<IndexRoleVM.GetData> ListRoles()
        {
            List<IndexRoleVM.GetData> lstRoles = new List<IndexRoleVM.GetData>();
            var roles = _context.ApplicationRole.ToList();
            //var roles = _pagingService.GetAll<ApplicationRole>(pageInfo, rlst);
            foreach (var item in roles)
            {
                IndexRoleVM.GetData roleObj = new IndexRoleVM.GetData();
                roleObj.Id = item.Id;
                roleObj.Name = item.Name;
                roleObj.DisplayName = item.DisplayName;
                roleObj.CategoryName = _roleCategoryService.GetById(item.RoleCategoryId).Name;
                lstRoles.Add(roleObj);
            }
            return lstRoles;
        }

        [HttpGet]
        [Route("getcount")]
        public int count()
        {
            return _pagingService.Count<RoleCategory>();
        }
        [HttpGet]
        [Route("GetById/{roleId}")]
        public async Task<ActionResult<ApplicationRole>> GetById(string roleId)
        {
            return await _applicationRole.FindByIdAsync(roleId);

        }


        [HttpGet]
        [Route("GetRolesByRoleCategoryId/{catId}")]
        public async Task<List<ApplicationRole>> GetById(int catId)
        {
            var lstRoles = await _applicationRole.Roles.Where(a => a.RoleCategoryId == catId).ToListAsync();
            return lstRoles;
        }

        [HttpGet]
        [Route("AddRoleToListById/{id}")]
        public async Task<IActionResult> AddRoleToListById(string id)
        {
           
            var updateObj = await _applicationRole.FindByIdAsync(id);
            return Ok(updateObj);
        }




        [HttpPost]
        [Route("AddRole")]
        public async Task<IActionResult> Create(ApplicationRole role)
        {
            ApplicationRole roleObj = new ApplicationRole();
            roleObj.DisplayName = role.DisplayName;
            roleObj.Name = role.Name;
            roleObj.RoleCategoryId = role.RoleCategoryId;
            var roleresult = await _applicationRole.CreateAsync(roleObj);
            return Ok();
        }


        [HttpPut]
        [Route("UpdateRole")]
        public async Task<IActionResult> Update(ApplicationRole roleObj)
        {
            var updateObj = await _applicationRole.FindByIdAsync(roleObj.Id);
            updateObj.RoleCategoryId = roleObj.RoleCategoryId;
            updateObj.DisplayName = roleObj.DisplayName;
            updateObj.Name = roleObj.Name;
            await _applicationRole.UpdateAsync(updateObj);
            return Ok();
        }

        [HttpDelete]
        [Route("DeleteRole/{id}")]
        public async Task<ActionResult<ApplicationRole>> Delete(string id)
        {
            try
            {
                var deleteRoleObj = await _applicationRole.FindByIdAsync(id);
                await _applicationRole.DeleteAsync(deleteRoleObj);
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
