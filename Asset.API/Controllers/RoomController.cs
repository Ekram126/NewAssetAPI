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
    public class RoomController : ControllerBase
    {

        private IRoomService _RoomService;

        public RoomController(IRoomService RoomService)
        {
            _RoomService = RoomService;
        }


        [HttpGet]
        [Route("ListRooms")]
        public IEnumerable<Room> GetAll()
       {
            return _RoomService.GetAll();
        }


        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<Room> GetById(int id)
        {
            return _RoomService.GetById(id);
        }


        [HttpGet]
        [Route("GetAllRoomsByFloorId/{floorId}")]
        public IEnumerable<Room> GetAllRoomsByFloorId(int floorId)
        {
            return _RoomService.GetAllRoomsByFloorId(floorId);
        }








        [HttpPut]
        [Route("UpdateRoom")]
        public IActionResult Update(Room roomVM)
        {
            try
            {
                int id = roomVM.Id;
                var lstOrgCode = _RoomService.GetAll().ToList().Where(a => a.Code == roomVM.Code && a.FloorId == roomVM.FloorId && a.Id != id).ToList();
                if (lstOrgCode.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "Room code already exist in same floor", MessageAr = "هذا الكود مسجل سابقاً  في نفس الدور" });
                }
                var lstOrgNames = _RoomService.GetAll().ToList().Where(a => a.Name == roomVM.Name && a.FloorId == roomVM.FloorId && a.Id != id).ToList();
                if (lstOrgNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Room name already exist in same floor", MessageAr = " هذا الاسم مسجل سابقاً  في نفس الدور" });
                }

                var lstOrgArNames = _RoomService.GetAll().ToList().Where(a => a.NameAr == roomVM.NameAr && a.FloorId == roomVM.FloorId && a.Id != id).ToList();
                if (lstOrgArNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "nameAr", Message = "Room arabic name already exist in same floor", MessageAr = "هذا الاسم مسجل سابقاً  في نفس الدور" });
                }

                else
                {
                    int updatedRow = _RoomService.Update(roomVM);
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
        [Route("AddRoom")]
        public ActionResult<Room> Add(Room roomVM)
        {
            var lstCode = _RoomService.GetAll().ToList().Where(a => a.Code == roomVM.Code && a.FloorId == roomVM.FloorId).ToList();
            if (lstCode.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "Room code already exist in same floor", MessageAr = "هذا الكود مسجل سابقاً  في نفس الدور" });
            }
            var lstNames = _RoomService.GetAll().ToList().Where(a => a.Name == roomVM.Name && a.FloorId == roomVM.FloorId).ToList();
            if (lstNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Room name already exist in same floor", MessageAr = "هذا الاسم مسجل سابقاً  في نفس الدور" });
            }
            var lstArNames = _RoomService.GetAll().ToList().Where(a => a.NameAr == roomVM.NameAr && a.FloorId == roomVM.FloorId).ToList();
            if (lstArNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Room arabic name already exist in same floor", MessageAr = "هذا الاسم مسجل سابقاً في نفس الدور" });
            }

            else
            {
                var savedId = _RoomService.Add(roomVM);
                return CreatedAtAction("GetById", new { id = savedId }, roomVM);
            }
        }

        [HttpDelete]
        [Route("DeleteRoom/{id}")]
        public ActionResult<Room> Delete(int id)
        {
            try
            {

                int deletedRow = _RoomService.Delete(id);
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
