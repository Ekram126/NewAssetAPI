using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.RequestDocumentVM;
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
    public class RequestDocumentController : ControllerBase
    {
        private IRequestDocumentService _requestDocumentService;

        public RequestDocumentController(IRequestDocumentService requestDocumentService)
        {
            _requestDocumentService = requestDocumentService;
        }
        // GET: api/<RequestDocumentController>
        [HttpGet]
        public IEnumerable<IndexRequestDocument> Get()
        {
            return _requestDocumentService.GetAllRequestDocument();
        }

        // GET api/<RequestDocumentController>/5
        [HttpGet("{id}")]
        public ActionResult<IndexRequestDocument> Get(int id)
        {
            return _requestDocumentService.GetRequestDocumentById(id);
        }
        [Route("GetRequestDocumentsByRequestTrackingId/{RequestTrackingId}")]
        public IEnumerable<IndexRequestDocument> GetRequestDocumentsByRequestTrackingId(int RequestTrackingId)
        {
            return _requestDocumentService.GetRequestDocumentsByRequestTrackingId(RequestTrackingId);
        }

        [Route("GetLastDocumentForRequestTrackingId/{RequestTrackingId}")]
        public RequestDocument GetLastDocumentForRequestTrackingId(int RequestTrackingId)
        {
            return _requestDocumentService.GetLastDocumentForRequestTrackingId(RequestTrackingId);
        }



        // POST api/<RequestDocumentController>
        [HttpPost]
        //  [Route("AddRequestDocuments")]
        public void Post(List<CreateRequestDocument> requestDocuments)
        {
            _requestDocumentService.AddRequestDocument(requestDocuments);
        }

        // PUT api/<RequestDocumentController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<RequestDocumentController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _requestDocumentService.DeleteRequestDocument(id);
            return Ok();
        }


        [HttpDelete]
        [Route("DeleteRequestDocument/{id}")]
        public ActionResult DeleteRequestDocument(int id)
        {
            _requestDocumentService.DeleteRequestDocument(id);
            return Ok();
        }




        [HttpPost, DisableRequestSizeLimit]
        [Route("uploadimage")]
        public IActionResult Upload()
        {
            try
            {
                var files = Request.Form.Files;
                var folderName = Path.Combine("UploadedAttachments", "RequestDocuments");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var fullPath = "";
                if (files.Count > 0)
                {
                    foreach (var file in files)
                    {

                        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        fullPath = Path.Combine(pathToSave, fileName);
                        var dbPath = Path.Combine(folderName, fileName);

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                    }

                    return Ok(new { fullPath });
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
    }
}
