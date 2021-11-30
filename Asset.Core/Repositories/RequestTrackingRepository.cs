using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.RequestTrackingVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class RequestTrackingRepository : IRequestTrackingRepository
    {
        private readonly ApplicationDbContext _context;
        private string msg;

        public RequestTrackingRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public int Add(CreateRequestTracking createRequestTracking)
        {
            try
            {
                if (createRequestTracking != null)
                {
                    RequestTracking requestTracking = new RequestTracking();
                    requestTracking.Description = createRequestTracking.Description;
                    requestTracking.DescriptionDate = DateTime.Now; //requestDescriptionDTO.DescriptionDate;
                    requestTracking.RequestId = createRequestTracking.RequestId;
                    requestTracking.RequestStatusId = createRequestTracking.RequestStatusId;
                    requestTracking.CreatedById = createRequestTracking.CreatedById;
                    _context.RequestTracking.Add(requestTracking);
                    _context.SaveChanges();
                    createRequestTracking.Id = requestTracking.Id;
                    return createRequestTracking.Id;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return createRequestTracking.Id;

        }

        public void Delete(int id)
        {
            RequestTracking requestTracking = _context.RequestTracking.Find(id);
            try
            {
                if (requestTracking != null)
                {
                    _context.RequestTracking.Remove(requestTracking);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }

        public IEnumerable<IndexRequestTracking> GetAll(string userId, int? assetDetailId)
        {
            ApplicationUser UserObj = new ApplicationUser();
            ApplicationRole roleObj = new ApplicationRole();
            string userRoleName = "";
            List<IndexRequestTracking> lstRequestTrackings = new List<IndexRequestTracking>();
            var obj = _context.ApplicationUser.Where(a => a.Id == userId).ToList();
            if (obj.Count > 0)
            {
                UserObj = obj[0];

                var lstRoles = _context.ApplicationRole.Where(a => a.Id == UserObj.RoleId).ToList();
                if (lstRoles.Count > 0)
                {
                    roleObj = lstRoles[0];
                    userRoleName = roleObj.Name;
                }
            }

            var ListRequestTracking = _context.RequestTracking
                .Include(req => req.User)
                .Include(req => req.Request.AssetDetail).Select(req => new IndexRequestTracking
                {
                    Id = req.Id,
                    Description = req.Description,
                    CreatedById = req.CreatedById,
                    UserName = req.User.UserName,
                    RequestId = req.RequestId != null ? (int)req.RequestId : 0,
                    RequestStatusId = req.RequestStatusId != null ? (int)req.RequestStatusId : 0,
                    StatusName = req.RequestStatus.Name,
                    StatusNameAr = req.RequestStatus.NameAr,
                    Subject = req.Request.Subject,
                    RequestCode = req.Request.RequestCode,
                    RequestDate = req.Request.RequestDate,
                    AssetDetailId = assetDetailId > 0 ? assetDetailId : (int)req.Request.AssetDetailId,
                    SerialNumber = req.Request.AssetDetail.SerialNumber,
                    HospitalId = (int)req.User.HospitalId,
                    GovernorateId = (int)req.User.GovernorateId,
                    CityId = (int)req.User.CityId,
                    OrganizationId = (int)req.User.OrganizationId,
                    SubOrganizationId = (int)req.User.SubOrganizationId,
                    RoleId = req.User.RoleId,
                    AssetName = assetDetailId > 0 ? _context.MasterAssets.Where(a => a.Id == req.Request.AssetDetail.MasterAssetId).FirstOrDefault().Name : _context.MasterAssets.Where(a => a.Id == req.Request.AssetDetail.MasterAssetId).FirstOrDefault().Name,

                    AssetNameAr = assetDetailId > 0 ? _context.MasterAssets.Where(a => a.Id == req.Request.AssetDetail.MasterAssetId).FirstOrDefault().NameAr : _context.MasterAssets.Where(a => a.Id == req.Request.AssetDetail.MasterAssetId).FirstOrDefault().NameAr,

                    // RoleName = _context.Roles.Where(a => a.Id == req.User.RoleId).FirstOrDefault().Name

                    //AssetName=req.Request.AssetDetail.MasterAssetId,

                }).ToList().OrderByDescending(a => a.DescriptionDate).GroupBy(r => r.RequestId);
            foreach (var item in ListRequestTracking)
            {
                lstRequestTrackings.Add(item.LastOrDefault());
            }


            if (UserObj.GovernorateId > 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
            {
                lstRequestTrackings = lstRequestTrackings.Where(t => t.GovernorateId == UserObj.GovernorateId && t.AssetDetailId == assetDetailId).ToList();
            }
            if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId == 0)
            {
                lstRequestTrackings = lstRequestTrackings.Where(t => t.CityId == UserObj.CityId && t.AssetDetailId == assetDetailId).ToList();
            }
            if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId == 0 && UserObj.HospitalId == 0)
            {
                lstRequestTrackings = lstRequestTrackings.Where(t => t.OrganizationId == UserObj.OrganizationId && t.AssetDetailId == assetDetailId).ToList();
            }
            if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId == 0)
            {
                lstRequestTrackings = lstRequestTrackings.Where(t => t.SubOrganizationId == UserObj.SubOrganizationId && t.AssetDetailId == assetDetailId).ToList();
            }



            if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId > 0)
            {
                if (userRoleName == "Admin")
                {
                    lstRequestTrackings = lstRequestTrackings.ToList();
                }

                if (userRoleName == "TLHospitalManager")
                {
                    lstRequestTrackings = lstRequestTrackings.Where(t => t.HospitalId == UserObj.HospitalId && t.AssetDetailId == assetDetailId).ToList();
                }

                if (userRoleName == "EngDepManager")
                {
                    lstRequestTrackings = lstRequestTrackings.Where(t => t.HospitalId == UserObj.HospitalId && t.AssetDetailId == assetDetailId).ToList();
                }
                if (userRoleName == "AssetOwner")
                {
                    lstRequestTrackings = lstRequestTrackings.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId && t.AssetDetailId == assetDetailId).ToList();
                }
                if (userRoleName == "DE")
                {
                    lstRequestTrackings = lstRequestTrackings.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId && t.AssetDetailId == assetDetailId).ToList();
                }

            }
            if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId > 0)
            {
                // lstRequestTrackings = lstRequestTrackings.Where(t => t.HospitalId == UserObj.HospitalId).ToList();


                if (userRoleName == "TLHospitalManager")
                {
                    lstRequestTrackings = lstRequestTrackings.Where(t => t.HospitalId == UserObj.HospitalId && t.AssetDetailId == assetDetailId).ToList();
                }
                if (userRoleName == "AssetOwner")
                {
                    lstRequestTrackings = lstRequestTrackings.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId && t.AssetDetailId == assetDetailId).ToList();
                }
                //if (userRoleName == "DE")
                //{
                //    lstRequestTrackings = lstRequestTrackings.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId && t.AssetDetailId == assetDetailId).ToList();
                //}

                if (userRoleName == "EngDepManager")
                {
                    lstRequestTrackings = lstRequestTrackings.Where(t => t.HospitalId == UserObj.HospitalId && t.AssetDetailId == assetDetailId).ToList();
                }

            }






            return lstRequestTrackings;

        }



        public List<RequestTrackingView> GetRequestTracksByRequestId(int requestId)
        {
            var trackings = _context.RequestTracking.Where(r => r.RequestId == requestId).OrderByDescending(a => a.DescriptionDate).Select(req => new RequestTrackingView
            {
                Id = req.Id,

                RequestId = req.RequestId != null ? (int)req.RequestId : 0,
                RequestStatusId = req.RequestStatusId != null ? (int)req.RequestStatusId : 0,
                Description = req.Description,
                DescriptionDate = req.DescriptionDate,
                CreatedById = req.CreatedById,
                UserName = req.User.UserName,

                StatusName = req.RequestStatus.Name,
                StatusNameAr = req.RequestStatus.NameAr
            }).OrderByDescending(t => t.DescriptionDate).ToList();

            return trackings;
        }






        public RequestDetails GetAllTrackingsByRequestId(int RequestId)
        {
            var trackings = _context.RequestTracking.Where(r => r.RequestId == RequestId).Select(req => new RequestTrackingView
            {
                Id = req.Id,

                Description = req.Description,
                DescriptionDate = req.DescriptionDate,
                CreatedById = req.CreatedById,
                UserName = req.User.UserName,

                RequestStatusId = req.RequestStatusId != null ? (int)req.RequestStatusId : 0,

                StatusName = req.RequestStatus.Name,
                StatusNameAr = req.RequestStatus.NameAr
            }).OrderByDescending(t => t.DescriptionDate).ToList();



            var lstRequestTracking = _context.RequestTracking.Include(t => t.Request.AssetDetail)
                .Include(t => t.Request.RequestMode).Include(t => t.Request.RequestPeriority)
                .Include(t => t.Request.SubProblem).Include(t => t.Request.RequestType).Include(r => r.RequestStatus)
                .Where(r => r.RequestId == RequestId).Select(req => new RequestDetails
                {
                    Id = req.Id,
                    Description = req.Description,
                    CreatedById = req.CreatedById,
                    UserName = req.User.UserName,
                    Subject = req.Request.Subject,
                    RequestCode = req.Request.RequestCode,
                    RequestDate = req.Request.RequestDate,
                    AssetDetailId = req.Request.AssetDetailId!=null ? (int)req.Request.AssetDetailId:0,
                    MasterAssetId = (int)req.Request.AssetDetail.MasterAssetId,
                    SerialNumber = req.Request.AssetDetail.SerialNumber,
                    RequestModeId = req.Request.RequestModeId != null ? (int)req.Request.RequestModeId : 0,
                    RequestPeriorityId = req.Request.RequestPeriorityId != null ? (int)req.Request.RequestPeriorityId : 0,
                    RequestStatusId = req.RequestStatusId != null ? (int)req.RequestStatusId : 0,
             
                    ModeName = req.Request.RequestMode.Name,
                    ModeNameAr = req.Request.RequestMode.NameAr,

                    PeriorityName = req.Request.RequestPeriority.Name,
                    PeriorityNameAr = req.Request.RequestPeriority.NameAr,
                    SubProblemId = req.Request.SubProblemId,
                    SubProblemName = req.Request.SubProblem.Name,
                    SubProblemNameAr = req.Request.SubProblem.NameAr,
                    RequestTypeId = req.Request.RequestTypeId,
                    RequestTypeName = req.Request.RequestType.Name,
                    RequestTypeNameAr = req.Request.RequestType.NameAr,
                 
                    StatusName = req.RequestStatus.Name,
                    StatusNameAr = req.RequestStatus.NameAr,
                    AssetName = _context.MasterAssets.Where(a => a.Id == req.Request.AssetDetail.MasterAssetId).FirstOrDefault().Name,
                    AssetNameAr = _context.MasterAssets.Where(a => a.Id == req.Request.AssetDetail.MasterAssetId).FirstOrDefault().NameAr,
                    lstTracking = trackings
                }).FirstOrDefault();
            return lstRequestTracking;
        }

        public IndexRequestTracking GetById(int id)
        {
            var RequestTrackingObj = _context.RequestTracking.Select(req => new IndexRequestTracking
            {
                Id = req.Id,
                RequestStatusId = req.RequestStatusId != null ? (int)req.RequestStatusId : 0,
                Description = req.Description,
                DescriptionDate = req.DescriptionDate,
                CreatedById = req.CreatedById,
                UserName = req.User.UserName
            }).FirstOrDefault();
            return RequestTrackingObj;
        }

        public void Update(int id, EditRequestTracking editRequestTracking)
        {
            try
            {
                RequestTracking requestTracking = new RequestTracking();
                requestTracking.Description = editRequestTracking.Description;
                requestTracking.DescriptionDate = editRequestTracking.DescriptionDate; //requestDescriptionDTO.DescriptionDate;
                requestTracking.RequestId = editRequestTracking.RequestId;
                requestTracking.CreatedById = editRequestTracking.CreatedById;
                requestTracking.RequestStatusId = editRequestTracking.RequestStatusId;
                _context.Entry(requestTracking).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }

        public int CountRequestTracksByRequestId(int requestId)
        {
            var counting = _context.RequestTracking.Where(a => a.RequestId == requestId).OrderByDescending(a => a.DescriptionDate).Count();
            return counting;
        }
    }
}
