using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.HospitalVM;
using Asset.ViewModels.PagingParameter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class HospitalController : ControllerBase
    {
        private IHospitalService _HospitalService;
        private IBuildingService _buildingService;
        private IAssetDetailService _assetDetailService;
        private IEmployeeService _employeeService;
        private IPagingService _pagingService;



        public HospitalController(IHospitalService HospitalService, IBuildingService buildingService,
            IAssetDetailService assetDetailService, IEmployeeService employeeService, IPagingService pagingService)
        {
            _HospitalService = HospitalService;
            _buildingService = buildingService;
            _assetDetailService = assetDetailService;
            _employeeService = employeeService;
            _pagingService = pagingService;
        }


        [HttpGet]
        [Route("ListHospitals")]
        public IEnumerable<IndexHospitalVM.GetData> GetAll()
        {
            return _HospitalService.GetAll().ToList();
        }


        [HttpGet]
        [Route("GetTop10Hospitals")]
        public IEnumerable<IndexHospitalVM.GetData> GetTop10Hospitals()
        {
            return _HospitalService.GetTop10Hospitals();
        }
        [HttpGet]
        [Route("GetTop10HospitalsCount")]
        public int GetTop10HospitalsCount()
        {
            return _HospitalService.GetTop10Hospitals().ToList().Count;
        }

        [HttpGet]
        [Route("CountDepartments/{hospitalId}")]
        public int CountDepartments(int hospitalId)
        {
            return _HospitalService.CountDepartmentsByHospitalId(hospitalId);
        }




        [HttpPut]
        [Route("GetAllWithPaging")]
        public IEnumerable<IndexHospitalVM.GetData> GetAllWithPaging(PagingParameter pageInfo)
        {
            var hoslist = _HospitalService.GetAll().ToList();
            return _pagingService.GetAll<IndexHospitalVM.GetData>(pageInfo, hoslist);
        }
        [HttpGet]
        [Route("getcount")]
        public int count()
        {
            return _pagingService.Count<Hospital>();
        }
        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<EditHospitalVM> GetById(int id)
        {
            return _HospitalService.GetById(id);
        }

        [HttpGet]
        [Route("GetHospitalDetailById/{id}")]
        public ActionResult<DetailHospitalVM> GetHospitalDetailById(int id)
        {
            return _HospitalService.GetHospitalDetailById(id);
        }

        [HttpPost]
        [Route("GetHospitalsByUserId/{userId}")]
        public IEnumerable<IndexHospitalVM.GetData> GetHospitalsByUserId(string userId)
        {
            return _HospitalService.GetHospitalsByUserId(userId).ToList();
        }


        [HttpPost]
        [Route("GetHospitalsByUserIdAndPaging/{userId}")]
        public IEnumerable<IndexHospitalVM.GetData> GetHospitalsByUserIdAndPaging(string userId, PagingParameter pageInfo)
        {
            var hoslist = _HospitalService.GetHospitalsByUserId(userId).ToList();
            return _pagingService.GetAll<IndexHospitalVM.GetData>(pageInfo, hoslist);
        }

        [HttpGet]
        [Route("GetHospitalsByUserIdAndPagingCount/{userId}")]
        public int GetHospitalsByUserIdAndPagingCount(string userId)
        {
            return _HospitalService.GetHospitalsByUserId(userId).ToList().Count();
        }

        [HttpGet]
        [Route("GetHospitalDepartmentByHospitalId/{hospitalId}")]
        public List<HospitalDepartment> GetHospitalDepartmentByHospitalId(int hospitalId)
        {
            return _HospitalService.GetHospitalDepartmentByHospitalId(hospitalId);
        }


        [HttpGet]
        [Route("GetSubOrganizationsByHospitalId/{hospitalId}")]
        public List<SubOrganization> GetSubOrganizationsByHospitalId(int hospitalId)
        {
            return _HospitalService.GetSubOrganizationsByHospitalId(hospitalId);
        }


        [HttpGet]
        [Route("GetHospitalDepartmentByHospitalId2/{hospitalId}")]
        public List<IndexHospitalDepartmentVM.GetData> GetHospitalDepartmentByHospitalId2(int hospitalId)
        {
            return _HospitalService.GetHospitalDepartmentByHospitalId2(hospitalId);
        }



        [HttpPost]
        [Route("SearchInHospitals/{pagenumber}/{pagesize}")]
        public IEnumerable<IndexHospitalVM.GetData> SearchInHospitals(int pagenumber, int pagesize, SearchHospitalVM searchObj)
        {
            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var list = _HospitalService.SearchHospitals(searchObj).ToList();
            return _pagingService.GetAll<IndexHospitalVM.GetData>(pageInfo, list);
        }

        [HttpPost]
        [Route("SearchInHospitalsCount")]
        public int SearchInRequestsCount(SearchHospitalVM searchObj)
        {
            int count = _HospitalService.SearchHospitals(searchObj).ToList().Count();
            return count;
        }






        [HttpPut]
        [Route("UpdateHospitalDepartment")]
        public int UpdateHospitalDepartment(EditHospitalDepartmentVM hospitalDepartmentVM)
        {
            return _HospitalService.UpdateHospitalDepartment(hospitalDepartmentVM);
        }

        [HttpGet]
        [Route("GetHospitalsByCityId/{cityId}")]
        public IEnumerable<Hospital> GetHospitalsByCityId(int cityId)
        {
            return _HospitalService.GetHospitalsByCityId(cityId);
        }


        [HttpGet]
        [Route("GetHospitalsBySubOrganizationId/{subOrgId}")]
        public IEnumerable<Hospital> GetHospitalsBySubOrganizationId(int subOrgId)
        {
            return _HospitalService.GetHospitalsBySubOrganizationId(subOrgId);
        }



        [HttpPut]
        [Route("UpdateHospital")]
        public IActionResult Update(EditHospitalVM HospitalVM)
        {
            try
            {
                int id = HospitalVM.Id;
                if (HospitalVM.Code.Length > 5)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "codelen", Message = "code must not exceed 5 characters", MessageAr = "الكود لا يتعدى 5 حروف وأرقام" });
                }
                var lstOrgCode = _HospitalService.GetAllHospitals().ToList().Where(a => a.Code == HospitalVM.Code && a.Id != id).ToList();
                if (lstOrgCode.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "Hospital code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
                }
                var lstOrgNames = _HospitalService.GetAllHospitals().ToList().Where(a => a.Name == HospitalVM.Name && a.Id != id).ToList();
                if (lstOrgNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Hospital name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }
                else
                {
                    int updatedRow = _HospitalService.Update(HospitalVM);
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
        [Route("AddHospital")]
        public ActionResult<Hospital> Add(CreateHospitalVM HospitalVM)
        {
            if (HospitalVM.Code.Length > 5)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "codelen", Message = "code must not exceed 5 characters", MessageAr = "الكود لا يتعدى 5 حروف وأرقام" });
            }
            var lstOrgCode = _HospitalService.GetAllHospitals().ToList().Where(a => a.Code == HospitalVM.Code).ToList();
            if (lstOrgCode.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "Hospital code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
            }
            var lstOrgNames = _HospitalService.GetAllHospitals().ToList().Where(a => a.Name == HospitalVM.Name).ToList();
            if (lstOrgNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Hospital name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            else
            {
                var savedId = _HospitalService.Add(HospitalVM);


                return CreatedAtAction("GetById", new { id = savedId }, HospitalVM);
            }
        }

        [HttpDelete]
        [Route("DeleteHospital/{id}")]
        public ActionResult<Hospital> Delete(int id)
        {
            try
            {
                var hospitalObj = _HospitalService.GetById(id);

                var lstBuildings = _buildingService.GetAllBuildingsByHospitalId(id).ToList();
                if (lstBuildings.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "build", Message = "You cannot delete this hospital it has related buildings", MessageAr = "لا يمكنك مسح المستشفى وذلك لارتباط مباني بها" });
                }
                var lstAssets = _assetDetailService.GetAllAssetDetailsByHospitalId(id).ToList();
                if (lstAssets.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "asset", Message = "You cannot delete this hospital it has related assets", MessageAr = "لا يمكنك مسح المستشفى وذلك لارتباط أصول بها" });
                }

                var lstEmployees = _employeeService.GetEmployeesByHospitalId(id).ToList();
                if (lstEmployees.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "emp", Message = "You cannot delete this hospital it has related employees", MessageAr = "لا يمكنك مسح المستشفى وذلك لارتباط موظفين بها" });
                }

                else
                {
                    int deletedRow = _HospitalService.Delete(id);
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in delete");
            }

            return Ok();
        }



        [HttpGet]
        [Route("CountHospitalsByCities")]
        public List<CountHospitalVM> CountHospitalsByCities()
        {
            return _HospitalService.CountHospitalsByCities();
        }

        [HttpGet]
        [Route("CountHospitals")]
        public int CountHospitals()
        {
            return _HospitalService.CountHospitals();
        }
        
        [HttpPost]
        [Route("SortHospitals/{pagenumber}/{pagesize}")]
        public IEnumerable<IndexHospitalVM.GetData> SortHospitals(int pagenumber, int pagesize, SortVM sortObj)
        {
            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var list = _HospitalService.SortHospitals(sortObj).ToList();
            return _pagingService.GetAll<IndexHospitalVM.GetData>(pageInfo, list);
        }

        [HttpGet("GetHospitalsWithAssets")]
        public IEnumerable<HospitalWithAssetVM> GetHospitalsWithAssets()
        {
            return _HospitalService.GetHospitalsWithAssets();
        }
    }
}
