using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.WorkOrderAttachmentVM;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkOrderAttachmentController : ControllerBase
    {
        private IWorkOrderAttachmentService _workOrderAttachmentService;

        public WorkOrderAttachmentController(IWorkOrderAttachmentService workOrderAttachmentService)
        {
            _workOrderAttachmentService = workOrderAttachmentService;
        }
        // GET: api/<WorkOrderAttachmentController>
        [HttpGet]
        public IEnumerable<IndexWorkOrderAttachmentVM> Get()
        {
            return _workOrderAttachmentService.GetAllWorkOrderAttachment();
        }
        [Route("GetWorkOrderAttachmentsByWorkOrderTrackingId/{trackId}")]
        public IEnumerable<IndexWorkOrderAttachmentVM> GetWorkOrderAttachmentsByWorkOrderTrackingId(int trackId)
        {
            return _workOrderAttachmentService.GetWorkOrderAttachmentsByWorkOrderTrackingId(trackId);
        }
        // GET api/<WorkOrderAttachmentController>/5
        [HttpGet("{id}")]
        public IndexWorkOrderAttachmentVM Get(int id)
        {
            return _workOrderAttachmentService.GetWorkOrderAttachmentById(id);
        }

        // POST api/<WorkOrderAttachmentController>
        [HttpPost]
        public void Post(List<CreateWorkOrderAttachmentVM> WorkOrderAttachments)
        {
            _workOrderAttachmentService.AddWorkOrderAttachment(WorkOrderAttachments);
        }

        // PUT api/<WorkOrderAttachmentController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<WorkOrderAttachmentController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _workOrderAttachmentService.DeleteWorkOrderAttachment(id);
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("uploadWorkOrderDcouments")]
        public IActionResult Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("UploadedAttachments", "WorkOrderFiles");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"the error is {ex.Message}");
            }
        }


        [Route("GetLastDocumentForWorkOrderTrackingId/{RequestTrackingId}")]
        public WorkOrderAttachment GetLastDocumentForWorkOrderTrackingId(int workOrderTrackingId)
        {
            return _workOrderAttachmentService.GetLastDocumentForWorkOrderTrackingId(workOrderTrackingId);
        }
    }
}
