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
    public class BuildingController : ControllerBase
    {

        private IBuildingService _buildingService;
        private IFloorService _floorService;



        public BuildingController(IBuildingService buildingService, IFloorService floorService)
        {
            _buildingService = buildingService;
            _floorService = floorService;
        }


        [HttpGet]
        [Route("ListBuildings")]
        public IEnumerable<Building> GetAll()
       {
            return _buildingService.GetAll();
        }


        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<Building> GetById(int id)
        {
            return _buildingService.GetById(id);
        }


        [HttpGet]
        [Route("GetAllBuildingsByHospitalId/{hospitalId}")]
        public IEnumerable<Building> GetAllBuildingsByHospitalId(int hospitalId)
        {
            return _buildingService.GetAllBuildingsByHospitalId(hospitalId);
        }








        [HttpPut]
        [Route("UpdateBuilding")]
        public IActionResult Update(Building buildingVM)
        {
            try
            {
                int id = buildingVM.Id;
                var lstOrgCode = _buildingService.GetAll().ToList().Where(a => a.Code == buildingVM.Code && a.HospitalId == buildingVM.HospitalId && a.Id != id).ToList();
                if (lstOrgCode.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "Building code already exist in same hospital", MessageAr = "هذا الكود مسجل سابقاً في نفس المستشفى" });
                }
                var lstOrgNames = _buildingService.GetAll().ToList().Where(a => a.Name == buildingVM.Name && a.HospitalId == buildingVM.HospitalId  && a.Id != id).ToList();
                if (lstOrgNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Building name already exist in same hospital", MessageAr = "هذا الاسم مسجل سابقاً في نفس المستشفى" });
                }

                var lstOrgArNames = _buildingService.GetAll().ToList().Where(a => a.NameAr == buildingVM.NameAr && a.HospitalId == buildingVM.HospitalId  && a.Id != id).ToList();
                if (lstOrgArNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "nameAr", Message = "Building arabic name already exist in same hospital", MessageAr = "هذا الاسم مسجل سابقاً في نفس المستشفى" });
                }

                else
                {
                    int updatedRow = _buildingService.Update(buildingVM);
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
        [Route("AddBuilding")]
        public ActionResult<Building> Add(Building buildingVM)
        {
            var lstCode = _buildingService.GetAll().ToList().Where(a => a.Code == buildingVM.Code && a.HospitalId == buildingVM.HospitalId).ToList();
            if (lstCode.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "Building code already exist in same hospital", MessageAr = "هذا الكود مسجل سابقاً في نفس المستشفى" });
            }
            var lstNames = _buildingService.GetAll().ToList().Where(a => a.Name == buildingVM.Name && a.HospitalId == buildingVM.HospitalId).ToList();
            if (lstNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Building name already exist in same hospital", MessageAr = "هذا الاسم مسجل سابقاً في نفس المستشفى" });
            }
            var lstArNames = _buildingService.GetAll().ToList().Where(a => a.NameAr == buildingVM.NameAr && a.HospitalId == buildingVM.HospitalId).ToList();
            if (lstArNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Building arabic name already exist in same hospital", MessageAr = "هذا الاسم مسجل سابقاً في نفس المستشفى" });
            }

            else
            {
                var savedId = _buildingService.Add(buildingVM);
                return CreatedAtAction("GetById", new { id = savedId }, buildingVM);
            }
        }

        [HttpDelete]
        [Route("DeleteBuilding/{id}")]
        public ActionResult<Building> Delete(int id)
        {
            try
            {
                //var buildObj = _buildingService.GetById(id);
                //var lstFloors = _floorService.GetAll().ToList().Where(a => a.BuildingId ==id).ToList();
                //if (lstFloors.Count > 0)
                //{
                //    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "floor", Message = "Please delete floors first", MessageAr = "لابد من مسح جميع الأدوار" });

                //}
                //else
                //{
                    int deletedRow = _buildingService.Delete(id);
               // }
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
