using Asset.Domain.Services;
using Asset.ViewModels.AssetWorkOrderTaskVM;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetWorkOrderTaskController : ControllerBase
    {
        private IAssetWorkOrderTaskService _assetWorkOrderTaskService;

        public AssetWorkOrderTaskController(IAssetWorkOrderTaskService assetWorkOrderTaskService)
        {
            _assetWorkOrderTaskService = assetWorkOrderTaskService;
        }
        // GET: api/<AssetWorkOrderTaskController>
        [HttpGet]
        public IEnumerable<IndexAssetWorkOrderTaskVM> Get()
        {
            return _assetWorkOrderTaskService.GetAllAssetWorkOrderTasks();
        }
        [Route("GetAllAssetWorkOrderTasksByMasterAssetId/{MasterAssetId}")]
        public IEnumerable<IndexAssetWorkOrderTaskVM> GetAllAssetWorkOrderTasksByMasterAssetId(int MasterAssetId)
        {
            return _assetWorkOrderTaskService.GetAllAssetWorkOrderTasksByMasterAssetId(MasterAssetId);
        }
        // GET api/<AssetWorkOrderTaskController>/5
        [HttpGet("{id}")]
        public ActionResult<IndexAssetWorkOrderTaskVM> Get(int id)
        {
            return _assetWorkOrderTaskService.GetAssetWorkOrderTaskById(id);
        }

        // POST api/<AssetWorkOrderTaskController>
        [HttpPost]
        public void Post(CreateAssetWorkOrderTaskVM createAssetWorkOrderTaskVM)
        {
            _assetWorkOrderTaskService.AddAssetWorkOrderTask(createAssetWorkOrderTaskVM);
        }

        // PUT api/<AssetWorkOrderTaskController>/5
        [HttpPut("{id}")]
        public void Put(int id, EditAssetWorkOrderTaskVM editAssetWorkOrderTaskVM)
        {
            _assetWorkOrderTaskService.UpdateAssetWorkOrderTask(id, editAssetWorkOrderTaskVM);
        }

        // DELETE api/<AssetWorkOrderTaskController>/5
        [HttpDelete("DeleteAssetWorkOrderTask/{id}")]
        public void Delete(int id)
        {
            _assetWorkOrderTaskService.DeleteAssetWorkOrderTask(id);
        }
    }
}
