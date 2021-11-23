using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.RoleCategoryVM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Asset.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class RoleCategoryController : ControllerBase
    {
        private IRoleCategoryService _roleCategoryService;


        private readonly UserManager<ApplicationUser> _userManager;

        public RoleCategoryController(IRoleCategoryService roleCategoryService, UserManager<ApplicationUser>userManager)
        {
            _roleCategoryService = roleCategoryService;
            _userManager = userManager;

        }


        [HttpGet] 
        [Route("ListRoleCategories")]
        public IEnumerable<IndexCategoryVM.GetData> GetAll()
        {
            return _roleCategoryService.GetAll();
        }

       
        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<RoleCategory> GetById(int id)
        {
            return _roleCategoryService.GetById(id);
        }



        [HttpPut]
        [Route("UpdateRoleCategory")]
        public IActionResult Update(EditRoleCategory roleCategory)
        {
            try
            {
             int updatedRow   = _roleCategoryService.Update(roleCategory);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in update");
            }

            return Ok();
        }


        [HttpPost]
        [Route("AddRoleCategory")]
        public ActionResult<RoleCategory> Add(CreateRoleCategory roleCategory)
        {
           var savedId = _roleCategoryService.Add(roleCategory);
            return CreatedAtAction("GetById", new { id = savedId }, roleCategory);
        }

        [HttpDelete]
        [Route("DeleteRoleCategory/{id}")]
        public ActionResult<RoleCategory> Delete(int id)
        {
            try
            {

                //var lstRoles = await _userManager.Users.Include(a => a.RoleCategoryId == id).ToListAsync();
                //if (lstRoles.Count > 0)
                //{
                //    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "role", Message = "You cannot delete this category it has related data", MessageAr = "لا يمكنك مسح هذا العنصر وذلك لارتباط بيانات بها" });
                //}
                //else
                //{
                    int deletedRow = _roleCategoryService.Delete(id);
              //  }
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
