using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Microsoft.AspNetCore.Http;
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
    public class FloorController : ControllerBase
    {

        private IFloorService _FloorService;

        public FloorController(IFloorService FloorService)
        {
            _FloorService = FloorService;
        }


        [HttpGet]
        [Route("ListFloors")]
        public IEnumerable<Floor> GetAll()
       {
            return _FloorService.GetAll();
        }


        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<Floor> GetById(int id)
        {
            return _FloorService.GetById(id);
        }


        [HttpGet]
        [Route("GetAllFloorsBybuildingId/{buildingId}")]
        public IEnumerable<Floor> GetAllFloorsBybuildingId(int buildingId)
        {
            return _FloorService.GetAllFloorsBybuildingId(buildingId);
        }








        [HttpPut]
        [Route("UpdateFloor")]
        public IActionResult Update(Floor floorVM)
        {
            try
            {
                int id = floorVM.Id;
                var lstOrgCode = _FloorService.GetAll().ToList().Where(a => a.Code == floorVM.Code && a.BuildingId == floorVM.BuildingId && a.Id != id).ToList();
                if (lstOrgCode.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "Floor code already exist in same building", MessageAr = "هذا الكود مسجل سابقاً  في نفس المبنى" });
                }
                var lstOrgNames = _FloorService.GetAll().ToList().Where(a => a.Name == floorVM.Name && a.BuildingId == floorVM.BuildingId && a.Id != id).ToList();
                if (lstOrgNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Floor name already exist in same building", MessageAr = "هذا الاسم مسجل سابقاً في نفس المبنى" });
                }

                var lstOrgArNames = _FloorService.GetAll().ToList().Where(a => a.NameAr == floorVM.NameAr && a.BuildingId == floorVM.BuildingId  && a.Id != id).ToList();
                if (lstOrgArNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "nameAr", Message = "Floor arabic name already exist in same building", MessageAr = "هذا الاسم مسجل سابقاً في نفس المبنى" });
                }

                else
                {
                    int updatedRow = _FloorService.Update(floorVM);
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
        [Route("AddFloor")]
        public ActionResult<Floor> Add(Floor floorVM)
        {
            var lstCode = _FloorService.GetAll().ToList().Where(a => a.Code == floorVM.Code && a.BuildingId == floorVM.BuildingId).ToList();
            if (lstCode.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "Floor code already exist in same building", MessageAr = "هذا الكود مسجل سابقاً في نفس المبنى" });
            }
            var lstNames = _FloorService.GetAll().ToList().Where(a => a.Name == floorVM.Name && a.BuildingId == floorVM.BuildingId).ToList();
            if (lstNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Floor name already exist  in same building", MessageAr = "هذا الاسم مسجل سابقاً في نفس المبنى" });
            }
            var lstArNames = _FloorService.GetAll().ToList().Where(a => a.NameAr == floorVM.NameAr && a.BuildingId == floorVM.BuildingId).ToList();
            if (lstArNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Floor arabic name already exist  in same building", MessageAr = " هذا الاسم مسجل سابقاً في نفس المبنى" });
            }

            else
            {
                var savedId = _FloorService.Add(floorVM);
                return CreatedAtAction("GetById", new { id = savedId }, floorVM);
            }
        }

        [HttpDelete]
        [Route("DeleteFloor/{id}")]
        public ActionResult<Floor> Delete(int id)
        {
            try
            {

                int deletedRow = _FloorService.Delete(id);
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
