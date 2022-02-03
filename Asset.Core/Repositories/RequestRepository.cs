using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.RequestTrackingVM;
using Asset.ViewModels.RequestVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class RequestRepository : IRequestRepository
    {
        private readonly ApplicationDbContext _context;


        public RequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public int Add(CreateRequestVM createRequestVM)
        {
            try
            {
                if (createRequestVM != null)
                {
                    Request request = new Request();
                    request.Subject = createRequestVM.Subject;
                    request.RequestCode = createRequestVM.RequestCode;
                    request.Description = createRequestVM.Description;
                    request.RequestDate = DateTime.Now; //requestDTO.RequestDate;
                    request.RequestModeId = createRequestVM.RequestModeId;
                    request.RequestPeriorityId = createRequestVM.RequestPeriorityId;
                    request.AssetDetailId = createRequestVM.AssetDetailId;
                    request.CreatedById = createRequestVM.CreatedById;
                    request.SubProblemId = createRequestVM.SubProblemId;
                    request.RequestTypeId = createRequestVM.RequestTypeId;
                    _context.Request.Add(request);
                    _context.SaveChanges();
                    createRequestVM.Id = request.Id;
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return createRequestVM.Id;
        }
        public void Delete(int id)
        {
            Request request = _context.Request.Find(id);
            try
            {
                if (request != null)
                {
                    _context.Request.Remove(request);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }
        public IEnumerable<IndexRequestsVM> GetAll()
        {

            var request = _context.Request
                                           .Include(r => r.RequestPeriority)
                                           .Include(r => r.AssetDetail)
                                           .Include(r => r.AssetDetail.MasterAsset)
                                           .Include(r => r.RequestType)
                                           .Include(r => r.SubProblem)
                                           .Include(r => r.RequestMode)
                                           .Include(r => r.User)
                                           .Select(req => new IndexRequestsVM
                                           {
                                               Id = req.Id,
                                               Subject = req.Subject,
                                               RequestCode = req.RequestCode,
                                               Description = req.Description,
                                               RequestDate = req.RequestDate,
                                               RequestModeId = req.RequestModeId != null ? (int)req.RequestModeId : 0,
                                               ModeName = req.RequestMode.Name,
                                               SubProblemId = req.SubProblemId,
                                               SubProblemName = req.SubProblem.Name,
                                               RequestTypeId = req.RequestTypeId,
                                               RequestTypeName = req.RequestType.Name,
                                               RequestPeriorityId = req.RequestPeriorityId != null ? (int)req.RequestPeriorityId : 0,
                                               PeriorityName = req.RequestPeriority.Name,
                                               CreatedById = req.CreatedById,
                                               CreatedBy = req.User.UserName,
                                               AssetDetailId = req.AssetDetailId != null ? (int)req.AssetDetailId : 0,
                                               SerialNumber = req.AssetDetail.SerialNumber,
                                               AssetName = req.AssetDetail.MasterAsset.Name,
                                               AssetNameAr = req.AssetDetail.MasterAsset.NameAr,
                                               UserId = req.User.Id,
                                           }).OrderByDescending(p => p.RequestDate).ToList();

            return request;
        }
        public IndexRequestsVM GetById(int id)
        {
            var req = _context.Request.
               Include(p => p.RequestMode).Include(r => r.User).Include(r => r.SubProblem).
               Include(p => p.RequestPeriority).Include(r => r.RequestType).Include(r => r.AssetDetail)
           .FirstOrDefault(e => e.Id == id);

            if (req == null)
            {
                return null;
            }
            else
            {
                var requestDTO = new IndexRequestsVM
                {
                    Id = req.Id,
                    Subject = req.Subject,
                    RequestCode = req.RequestCode,
                    AssetCode = req.AssetDetail.Code,
                    Description = req.Description,
                    RequestDate = req.RequestDate,
                    RequestModeId = req.RequestModeId != null ? (int)req.RequestModeId : 0,
                    ModeName = req.RequestMode.Name,
                    RequestPeriorityId = req.RequestPeriorityId != null ? (int)req.RequestPeriorityId : 0,
                    PeriorityName = req.RequestPeriority.Name,
                    MasterAssetId = (int)req.AssetDetail.MasterAssetId,
                    AssetName = _context.MasterAssets.Where(t => t.Id == req.AssetDetail.MasterAssetId).FirstOrDefault().Name,
                    AssetNameAr = _context.MasterAssets.Where(t => t.Id == req.AssetDetail.MasterAssetId).FirstOrDefault().NameAr,
                    AssetDetailId = req.AssetDetailId != null ? (int)req.AssetDetailId : 0,
                    SerialNumber = req.AssetDetail.SerialNumber,
                    CreatedById = req.CreatedById,
                    CreatedBy = req.User.UserName,
                    ProblemId = req.SubProblem.ProblemId,
                    SubProblemId = req.SubProblemId,
                    SubProblemName = req.SubProblem.Name,
                    RequestTypeId = req.RequestTypeId,
                    RequestTypeName = req.RequestType.Name,
                    RequestTrackingId = _context.RequestTracking.Where(t => t.RequestId == id).FirstOrDefault().Id,
                    RequestStatusId = _context.RequestTracking.Where(t => t.RequestId == id).FirstOrDefault().RequestStatusId != null ? (int)_context.RequestTracking.Where(t => t.RequestId == id).FirstOrDefault().RequestStatusId : 0,
                    //  StatusName = _context.RequestTracking.Where(t => t.RequestId == id).FirstOrDefault().RequestStatus.StatusName

                };
                return requestDTO;
            }
        }
        public void Update(EditRequestVM editRequestVM)
        {

            try
            {
                Request request = _context.Request.Find(editRequestVM.Id);
                request.Id = editRequestVM.Id;
                request.Subject = editRequestVM.Subject;
                request.RequestCode = editRequestVM.RequestCode;
                request.Description = editRequestVM.Description;
                request.RequestDate = editRequestVM.RequestDate;
                request.RequestModeId = editRequestVM.RequestModeId;
                request.RequestPeriorityId = editRequestVM.RequestPeriorityId;
                request.AssetDetailId = editRequestVM.AssetDetailId;
                request.CreatedById = editRequestVM.CreatedById;
                request.SubProblemId = editRequestVM.SubProblemId;
                request.RequestTypeId = editRequestVM.RequestTypeId;
                _context.Entry(request).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

        }
        public IEnumerable<IndexRequestVM.GetData> GetAllRequestsWithTrackingByUserId(string userId)
        {
            List<IndexRequestVM.GetData> list = new List<IndexRequestVM.GetData>();
            List<IndexRequestVM.GetData> listWO = new List<IndexRequestVM.GetData>();
            ApplicationUser UserObj = new ApplicationUser();
            ApplicationRole roleObj = new ApplicationRole();

            List<string> userRoleNames = new List<string>();
            var obj = _context.ApplicationUser.Where(a => a.Id == userId).ToList();
            if (obj.Count > 0)
            {
                UserObj = obj[0];

                var roles = (from userRole in _context.UserRoles
                             join role in _context.ApplicationRole on userRole.RoleId equals role.Id
                             where userRole.UserId == userId
                             select role);
                foreach (var role in roles)
                {
                    userRoleNames.Add(role.Name);
                }
            }

            var lstRequests = _context.Request
                               .Include(t => t.AssetDetail)
                               .Include(t => t.AssetDetail.MasterAsset)
                               .Include(t => t.User)
                               .Include(t => t.RequestMode)
                               .Include(t => t.RequestPeriority).OrderByDescending(a => a.RequestDate).ToList();

            foreach (var req in lstRequests)
            {
                IndexRequestVM.GetData getDataObj = new IndexRequestVM.GetData();
                getDataObj.RequestId = req.Id;
                getDataObj.Id = req.Id;
                getDataObj.Code = req.RequestCode;
                getDataObj.CreatedById = req.CreatedById;
                getDataObj.UserName = req.User.UserName;
                getDataObj.Subject = req.Subject;
                getDataObj.RequestDate = req.RequestDate;
                getDataObj.AssetDetailId = req.AssetDetailId != null ? (int)req.AssetDetailId : 0;
                getDataObj.HospitalId = req.AssetDetail.HospitalId;
                var lstStatus = _context.RequestTracking
                                 .Include(t => t.Request).Include(t => t.RequestStatus)
                                 .Where(a => a.RequestId == req.Id).ToList().OrderByDescending(a => a.Id).ToList();
                if (lstStatus.Count > 0)
                {
                    getDataObj.StatusId = lstStatus[0].RequestStatus.Id;
                    getDataObj.StatusName = lstStatus[0].RequestStatus.Name;
                    getDataObj.StatusNameAr = lstStatus[0].RequestStatus.NameAr;
                    getDataObj.StatusColor = lstStatus[0].RequestStatus.Color;
                    getDataObj.StatusIcon = lstStatus[0].RequestStatus.Icon;
                }
                getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                getDataObj.ModeId = req.RequestMode.Id;
                getDataObj.ModeName = req.RequestMode.Name;
                getDataObj.ModeNameAr = req.RequestMode.NameAr;


                getDataObj.PeriorityId = req.RequestPeriority.Id;
                getDataObj.PeriorityName = req.RequestPeriority.Name;
                getDataObj.PeriorityNameAr = req.RequestPeriority.NameAr;
                getDataObj.PeriorityColor = req.RequestPeriority.Color;
                getDataObj.PeriorityIcon = req.RequestPeriority.Icon;


                getDataObj.AssetName = req.AssetDetail.MasterAsset.Name;
                getDataObj.AssetNameAr = req.AssetDetail.MasterAsset.NameAr;
                getDataObj.ListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id)
                        .ToList().Select(item => new IndexRequestTrackingVM.GetData
                        {
                            Id = item.Id,
                            StatusName = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().Name,
                            StatusNameAr = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().NameAr,
                            Description = item.Description,
                            Date = item.DescriptionDate,
                            StatusId = item.RequestStatus.Id,
                            isExpanded = (_context.RequestDocument.Where(a => a.RequestTrackingId == item.Id).Count()) > 0 ? true : false,
                            ListDocuments = _context.RequestDocument.Where(a => a.RequestTrackingId == item.Id).ToList(),
                        }).ToList();

                var lstWOStatus = _context.WorkOrderTrackings
                        .Include(o => o.WorkOrder).Include(o => o.WorkOrderStatus).Where(a => a.WorkOrder.RequestId == req.Id)
                        .OrderByDescending(a => a.CreationDate).ToList();

                if (lstWOStatus.Count > 0)
                {
                    getDataObj.LatestWorkOrderStatusId = lstWOStatus[0].WorkOrderStatusId;
                    getDataObj.StatusName = lstStatus[0].RequestStatus.Name;
                    getDataObj.StatusNameAr = lstStatus[0].RequestStatus.NameAr;
                    getDataObj.StatusColor = lstStatus[0].RequestStatus.Color;
                }
                getDataObj.CountListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id).ToList().Count;
                getDataObj.CountWorkOrder = _context.WorkOrders.Where(a => a.RequestId == req.Id).ToList().Count;
                getDataObj.GovernorateId = req.User.GovernorateId;
                getDataObj.CityId = req.User.CityId;
                getDataObj.OrganizationId = req.User.OrganizationId;
                getDataObj.SubOrganizationId = req.User.SubOrganizationId;
                list.Add(getDataObj);
            }



            if (UserObj.GovernorateId == 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
            {
                list = list.ToList();
            }
            if (UserObj.GovernorateId > 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
            {
                list = list.Where(t => t.GovernorateId == UserObj.GovernorateId).ToList();
            }
            if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId == 0)
            {
                list = list.Where(t => t.CityId == UserObj.CityId).ToList();
            }
            if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId == 0 && UserObj.HospitalId == 0)
            {
                list = list.Where(t => t.OrganizationId == UserObj.OrganizationId).ToList();
            }
            if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId == 0)
            {
                list = list.Where(t => t.SubOrganizationId == UserObj.SubOrganizationId).ToList();
            }

            if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId > 0)
            {
                if (userRoleNames.Contains("Admin"))
                {
                    list = list.ToList();
                }
                if (userRoleNames.Contains("TLHospitalManager"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }

                if (userRoleNames.Contains("EngDepManager"))
                {

                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                if (userRoleNames.Contains("EngManager"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }

                if (userRoleNames.Contains("Eng"))
                {

                    list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();

                }


                if (userRoleNames.Contains("AssetOwner"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                }
                if (userRoleNames.Contains("DE"))
                {
                    list = list = new List<IndexRequestVM.GetData>();
                }
                if (userRoleNames.Contains("HR"))
                {
                    list = list = new List<IndexRequestVM.GetData>();
                }


            }
            if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId > 0)
            {
                if (userRoleNames.Contains("Admin"))
                {
                    list = list.ToList();
                }
                if (userRoleNames.Contains("TLHospitalManager"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                if (userRoleNames.Contains("EngDepManager"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                if (userRoleNames.Contains("EngManager"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                if (userRoleNames.Contains("AssetOwner"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                }
                if (userRoleNames.Contains("Eng"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                }

                if (userRoleNames.Contains("DE"))
                {
                    list = list = new List<IndexRequestVM.GetData>();
                }
                if (userRoleNames.Contains("HR"))
                {
                    list = list = new List<IndexRequestVM.GetData>();
                }


            }

            return list;
        }
        public IEnumerable<IndexRequestVM.GetData> GetAllRequestsByStatusId(string userId, int statusId)
        {
            List<IndexRequestVM.GetData> list = new List<IndexRequestVM.GetData>();
            ApplicationUser UserObj = new ApplicationUser();
            List<string> userRoleNames = new List<string>();


            var obj = _context.ApplicationUser.Where(a => a.Id == userId).ToList();
            if (obj.Count > 0)
            {
                UserObj = obj[0];
                var roleNames = (from userRole in _context.UserRoles
                                 join role in _context.Roles on userRole.RoleId equals role.Id
                                 where userRole.UserId == userId
                                 select role);
                foreach (var name in roleNames)
                {
                    userRoleNames.Add(name.Name);
                }
            }
            var lstRequests = _context.Request
                               .Include(t => t.AssetDetail)
                               .Include(t => t.AssetDetail.MasterAsset)
                               .Include(t => t.User)
                               .Include(t => t.RequestMode)
                               .Include(t => t.RequestPeriority).OrderByDescending(a => a.RequestDate).ToList();


            if (lstRequests.Count > 0)
            {
                foreach (var req in lstRequests)
                {
                    IndexRequestVM.GetData getDataObj = new IndexRequestVM.GetData();
                    getDataObj.RequestId = req.Id;
                    getDataObj.Id = req.Id;
                    getDataObj.Code = req.RequestCode;
                    getDataObj.CreatedById = req.CreatedById;
                    getDataObj.UserName = req.User.UserName;
                    getDataObj.Subject = req.Subject;
                    getDataObj.RequestDate = req.RequestDate;



                    getDataObj.AssetDetailId = req.AssetDetailId != null ? (int)req.AssetDetailId : 0;
                    getDataObj.HospitalId = req.AssetDetail.HospitalId;
                    var lstStatus = _context.RequestTracking
                                     .Include(t => t.Request).Include(t => t.RequestStatus)
                                     .Where(a => a.RequestId == req.Id).ToList().OrderByDescending(a => a.Id).ToList();
                    if (lstStatus.Count > 0)
                    {
                        getDataObj.StatusId = lstStatus[0].RequestStatus.Id;
                        getDataObj.StatusName = lstStatus[0].RequestStatus.Name;
                        getDataObj.StatusNameAr = lstStatus[0].RequestStatus.NameAr;
                        getDataObj.StatusColor = lstStatus[0].RequestStatus.Color;
                        getDataObj.StatusIcon = lstStatus[0].RequestStatus.Icon;
                    }
                    getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                    getDataObj.ModeId = req.RequestModeId != null ? (int)req.RequestModeId : 0;
                    getDataObj.ModeName = req.RequestMode.Name;
                    getDataObj.ModeNameAr = req.RequestMode.NameAr;
                    getDataObj.PeriorityId = req.RequestPeriorityId != null ? (int)req.RequestPeriorityId : 0;
                    getDataObj.PeriorityName = req.RequestPeriority.Name;
                    getDataObj.PeriorityNameAr = req.RequestPeriority.NameAr;
                    getDataObj.PeriorityColor = req.RequestPeriority.Color;
                    getDataObj.PeriorityIcon = req.RequestPeriority.Icon;




                    getDataObj.AssetName = req.AssetDetail.MasterAsset.Name;// _context.MasterAssets.Where(a => a.Id == req.AssetDetail.MasterAssetId).ToList().FirstOrDefault().Name;
                    getDataObj.AssetNameAr = req.AssetDetail.MasterAsset.NameAr;
                    getDataObj.ListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id)
                            .ToList().Select(item => new IndexRequestTrackingVM.GetData
                            {
                                Id = item.Id,
                                StatusName = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().Name,
                                StatusNameAr = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().NameAr,
                                Description = item.Description,
                                Date = item.DescriptionDate,
                                StatusId = item.RequestStatusId != null ? (int)item.RequestStatusId : 0,
                                isExpanded = (_context.RequestDocument.Where(a => a.RequestTrackingId == item.Id).Count()) > 0 ? true : false,
                                ListDocuments = _context.RequestDocument.Where(a => a.RequestTrackingId == item.Id).ToList(),
                            }).ToList();

                    var lstWOStatus = _context.WorkOrderTrackings
                            .Include(o => o.WorkOrder).Include(o => o.WorkOrderStatus).Where(a => a.WorkOrder.RequestId == req.Id)
                            .OrderByDescending(a => a.CreationDate).ToList();

                    if (lstWOStatus.Count > 0)
                    {
                        getDataObj.LatestWorkOrderStatusId = lstWOStatus[0].WorkOrderStatusId;
                    }


                    getDataObj.CountListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id).ToList().Count;
                    getDataObj.CountWorkOrder = _context.WorkOrders.Where(a => a.RequestId == req.Id).ToList().Count;
                    getDataObj.GovernorateId = req.User.GovernorateId;
                    getDataObj.CityId = req.User.CityId;
                    getDataObj.OrganizationId = req.User.OrganizationId;
                    getDataObj.SubOrganizationId = req.User.SubOrganizationId;
                    list.Add(getDataObj);
                }

                if (UserObj.GovernorateId == 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
                {
                    list = list.Where(a => a.StatusId == statusId).ToList();
                }
                if (UserObj.GovernorateId > 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
                {
                    list = list.Where(t => t.GovernorateId == UserObj.GovernorateId && t.StatusId == statusId).ToList();
                }
                if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId == 0)
                {
                    list = list.Where(t => t.CityId == UserObj.CityId && t.StatusId == statusId).ToList();
                }
                if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId == 0 && UserObj.HospitalId == 0)
                {
                    list = list.Where(t => t.OrganizationId == UserObj.OrganizationId && t.StatusId == statusId).ToList();
                }
                if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId == 0)
                {
                    list = list.Where(t => t.SubOrganizationId == UserObj.SubOrganizationId && t.StatusId == statusId).ToList();
                }

                if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId > 0)
                {

                    if (userRoleNames.Contains("TLHospitalManager"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.StatusId == statusId).ToList();
                    }

                    if (userRoleNames.Contains("EngDepManager"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.StatusId == statusId).ToList();
                    }
                    if (userRoleNames.Contains("EngManager"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.StatusId == statusId).ToList();
                    }

                    if (userRoleNames.Contains("Eng"))
                    {

                        list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId && t.StatusId == statusId).ToList();

                    }

                    if (userRoleNames.Contains("AssetOwner"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId && t.StatusId == statusId).ToList();
                    }
                    if (userRoleNames.Contains("DE"))
                    {
                        list = list = new List<IndexRequestVM.GetData>();
                    }
                    if (userRoleNames.Contains("HR"))
                    {
                        list = list = new List<IndexRequestVM.GetData>();
                    }
                }
                if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId > 0)
                {

                    if (userRoleNames.Contains("TLHospitalManager"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.StatusId == statusId).ToList();
                    }
                    if (userRoleNames.Contains("EngDepManager"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.StatusId == statusId).ToList();
                    }
                    if (userRoleNames.Contains("EngManager"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.StatusId == statusId).ToList();
                    }
                    if (userRoleNames.Contains("AssetOwner"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId && t.StatusId == statusId).ToList();
                    }
                    if (userRoleNames.Contains("Eng"))
                    {
                        //      list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                    }

                    if (userRoleNames.Contains("DE"))
                    {
                        list = new List<IndexRequestVM.GetData>();
                    }
                    if (userRoleNames.Contains("HR"))
                    {
                        list = new List<IndexRequestVM.GetData>();
                    }

                }
            }


            return list;
        }
        public IEnumerable<IndexRequestVM.GetData> GetRequestsByUserIdAssetId(string userId, int assetId)
        {
            List<IndexRequestVM.GetData> list = new List<IndexRequestVM.GetData>();
            List<IndexRequestVM.GetData> listWO = new List<IndexRequestVM.GetData>();
            ApplicationUser UserObj = new ApplicationUser();

            List<string> userRoleNames = new List<string>();

            var obj = _context.ApplicationUser.Where(a => a.Id == userId).ToList();
            if (obj.Count > 0)
            {
                UserObj = obj[0];
                var roleNames = (from userRole in _context.UserRoles
                                 join role in _context.Roles on userRole.RoleId equals role.Id
                                 where userRole.UserId == userId
                                 select role);
                foreach (var name in roleNames)
                {
                    userRoleNames.Add(name.Name);
                }
            }




            var lstRequests = _context.Request
                               .Include(t => t.AssetDetail)
                               .Include(t => t.User)
                               .Include(t => t.RequestMode)
                               .Include(t => t.RequestPeriority).Where(a => a.AssetDetailId == assetId).ToList();



            foreach (var req in lstRequests)
            {
                IndexRequestVM.GetData getDataObj = new IndexRequestVM.GetData();
                getDataObj.RequestId = req.Id;
                getDataObj.Id = req.Id;
                getDataObj.Code = req.RequestCode;
                getDataObj.CreatedById = req.CreatedById;
                getDataObj.UserName = req.User.UserName;
                getDataObj.Subject = req.Subject;
                getDataObj.RequestDate = req.RequestDate;
                getDataObj.AssetDetailId = req.AssetDetailId != null ? (int)req.AssetDetailId : 0;
                getDataObj.HospitalId = req.AssetDetail.HospitalId;
                var lstStatus = _context.RequestTracking
                                 .Include(t => t.Request).Include(t => t.RequestStatus)
                                 .Where(a => a.RequestId == req.Id).ToList().OrderByDescending(a => a.Id).ToList();
                if (lstStatus.Count > 0)
                {
                    getDataObj.StatusId = lstStatus[0].RequestStatus.Id;
                    getDataObj.StatusName = lstStatus[0].RequestStatus.Name;
                    getDataObj.StatusNameAr = lstStatus[0].RequestStatus.NameAr;
                    getDataObj.StatusColor = lstStatus[0].RequestStatus.Color;
                    getDataObj.StatusIcon = lstStatus[0].RequestStatus.Icon;
                }
                getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                getDataObj.ModeId = req.RequestModeId != null ? (int)req.RequestModeId : 0;
                getDataObj.ModeName = req.RequestMode.Name;
                getDataObj.ModeNameAr = req.RequestMode.NameAr;
                getDataObj.PeriorityId = req.RequestPeriorityId != null ? (int)req.RequestPeriorityId : 0;
                getDataObj.PeriorityName = req.RequestPeriority.Name;
                getDataObj.PeriorityNameAr = req.RequestPeriority.NameAr;
                getDataObj.AssetName = _context.MasterAssets.Where(a => a.Id == req.AssetDetail.MasterAssetId).ToList().FirstOrDefault().Name;
                getDataObj.AssetNameAr = _context.MasterAssets.Where(a => a.Id == req.AssetDetail.MasterAssetId).ToList().FirstOrDefault().NameAr;
                getDataObj.ListTracks = _context.RequestTracking.Include(a => a.RequestStatus).Where(a => a.RequestId == req.Id)
                        .ToList().Select(item => new IndexRequestTrackingVM.GetData
                        {
                            Id = item.Id,

                            StatusName = item.RequestStatus.Name,
                            StatusNameAr = item.RequestStatus.NameAr,
                            Description = item.Description,
                            Date = item.DescriptionDate,
                            StatusId = item.RequestStatus != null ? (int)item.RequestStatusId : 0,
                            isExpanded = (_context.RequestDocument.Where(a => a.RequestTrackingId == item.Id).Count()) > 0 ? true : false,
                            ListDocuments = _context.RequestDocument.Where(a => a.RequestTrackingId == item.Id).ToList(),
                        }).ToList();
                getDataObj.CountListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id).ToList().Count;
                getDataObj.CountWorkOrder = _context.WorkOrders.Where(a => a.RequestId == req.Id).ToList().Count;

                var lstWOStatus = _context.WorkOrderTrackings
                        .Include(o => o.WorkOrder).Include(o => o.WorkOrderStatus).Where(a => a.WorkOrder.RequestId == req.Id)
                        .OrderByDescending(a => a.CreationDate).ToList();

                if (lstWOStatus.Count > 0)
                {
                    getDataObj.LatestWorkOrderStatusId = lstWOStatus[0].WorkOrderStatusId;
                }

                getDataObj.GovernorateId = req.User.GovernorateId;
                getDataObj.CityId = req.User.CityId;
                getDataObj.OrganizationId = req.User.OrganizationId;
                getDataObj.SubOrganizationId = req.User.SubOrganizationId;
                list.Add(getDataObj);
            }



            if (UserObj.GovernorateId == 0 && UserObj.CityId == 0 && UserObj.OrganizationId > 0 && UserObj.SubOrganizationId == 0 && UserObj.HospitalId == 0)
            {
                list = list.ToList();
            }
            if (UserObj.GovernorateId > 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
            {
                list = list.Where(t => t.GovernorateId == UserObj.GovernorateId).ToList();
            }
            if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId == 0)
            {
                list = list.Where(t => t.CityId == UserObj.CityId).ToList();
            }
            if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId == 0 && UserObj.HospitalId == 0)
            {
                list = list.Where(t => t.OrganizationId == UserObj.OrganizationId).ToList();
            }
            if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId == 0)
            {
                list = list.Where(t => t.SubOrganizationId == UserObj.SubOrganizationId).ToList();
            }

            if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId > 0)
            {

                if (userRoleNames.Contains("TLHospitalManager"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }

                if (userRoleNames.Contains("EngDepManager"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                if (userRoleNames.Contains("EngManager"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }

                if (userRoleNames.Contains("Eng"))
                {

                    var lstAssigned = (from order in _context.WorkOrders
                                       join track in _context.WorkOrderTrackings on order.Id equals track.WorkOrderId
                                       join usr in _context.ApplicationUser on track.AssignedTo equals usr.Id
                                       join req in _context.Request on order.RequestId equals req.Id
                                       where usr.HospitalId == UserObj.HospitalId
                                       && track.AssignedTo == userId
                                       select order).ToList();


                    foreach (var assigned in lstAssigned)
                    {
                        IndexRequestVM.GetData getDataObj = new IndexRequestVM.GetData();
                        getDataObj.Id = assigned.Request.Id;
                        getDataObj.Code = assigned.Request.RequestCode;
                        getDataObj.CreatedById = assigned.Request.CreatedById;
                        getDataObj.UserName = assigned.Request.User.UserName;
                        getDataObj.Subject = assigned.Request.Subject;
                        getDataObj.RequestDate = assigned.Request.RequestDate;
                        //  getDataObj.AssetDetailId = assigned.Request.AssetDetailId;
                        getDataObj.AssetDetailId = assigned.Request.AssetDetailId != null ? (int)assigned.Request.AssetDetailId : 0;
                        getDataObj.HospitalId = assigned.Request.AssetDetail.HospitalId;
                        var lstStatus = _context.RequestTracking
                               .Include(t => t.Request).Include(t => t.RequestStatus)
                               .Where(a => a.RequestId == assigned.Request.Id).ToList().OrderByDescending(a => a.DescriptionDate).ToList();
                        if (lstStatus.Count > 0)
                        {
                            getDataObj.StatusId = lstStatus[0].RequestStatus.Id;
                            getDataObj.StatusName = lstStatus[0].RequestStatus.Name;
                            getDataObj.StatusNameAr = lstStatus[0].RequestStatus.NameAr;
                            getDataObj.StatusColor = lstStatus[0].RequestStatus.Color;
                            getDataObj.StatusIcon = lstStatus[0].RequestStatus.Icon;
                        }
                        getDataObj.SerialNumber = assigned.Request.AssetDetail.SerialNumber;
                        getDataObj.ModeId = assigned.Request.RequestModeId != null ? (int)assigned.Request.RequestModeId : 0;
                        getDataObj.ModeName = assigned.Request.RequestMode.Name;
                        getDataObj.ModeNameAr = assigned.Request.RequestMode.NameAr;
                        getDataObj.PeriorityId = assigned.Request.RequestPeriorityId != null ? (int)assigned.Request.RequestPeriorityId : 0;
                        getDataObj.PeriorityName = assigned.Request.RequestPeriority.Name;
                        getDataObj.PeriorityNameAr = assigned.Request.RequestPeriority.NameAr;
                        getDataObj.AssetName = _context.MasterAssets.Where(a => a.Id == assigned.Request.AssetDetail.MasterAssetId).ToList().FirstOrDefault().Name;
                        getDataObj.AssetNameAr = _context.MasterAssets.Where(a => a.Id == assigned.Request.AssetDetail.MasterAssetId).ToList().FirstOrDefault().NameAr;
                        getDataObj.ListTracks = _context.RequestTracking.Where(a => a.RequestId == assigned.Request.Id)
                                .ToList().Select(item => new IndexRequestTrackingVM.GetData
                                {
                                    Id = item.Id,
                                    StatusName = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().Name,
                                    StatusNameAr = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().NameAr,
                                    Description = item.Description,
                                    Date = item.DescriptionDate,
                                    StatusId = item.RequestStatusId != null ? (int)item.RequestStatusId : 0,
                                    isExpanded = (_context.RequestDocument.Where(a => a.RequestTrackingId == item.Id).Count()) > 0 ? true : false,
                                    ListDocuments = _context.RequestDocument.Where(a => a.RequestTrackingId == item.Id).ToList(),
                                }).ToList();
                        getDataObj.CountListTracks = _context.RequestTracking.Where(a => a.RequestId == assigned.Request.Id).ToList().Count;
                        getDataObj.CountWorkOrder = _context.WorkOrders.Where(a => a.RequestId == assigned.Request.Id).ToList().Count;
                        getDataObj.GovernorateId = assigned.Request.User.GovernorateId;
                        getDataObj.CityId = assigned.Request.User.CityId;
                        getDataObj.OrganizationId = assigned.Request.User.OrganizationId;
                        getDataObj.SubOrganizationId = assigned.Request.User.SubOrganizationId;
                        listWO.Add(getDataObj);
                    }




                    var lstCreatedItems = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                    list = listWO.Concat(lstCreatedItems).ToList();

                }


                if (userRoleNames.Contains("AssetOwner"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                }
                if (userRoleNames.Contains("DE"))
                {
                    list = list = new List<IndexRequestVM.GetData>();
                }
                if (userRoleNames.Contains("HR"))
                {
                    list = list = new List<IndexRequestVM.GetData>();
                }


            }
            if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId > 0)
            {

                if (userRoleNames.Contains("TLHospitalManager"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                if (userRoleNames.Contains("EngDepManager"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                if (userRoleNames.Contains("EngManager"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                if (userRoleNames.Contains("AssetOwner"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                }
                if (userRoleNames.Contains("Eng"))
                {

                    var lstAssigned = (from order in _context.WorkOrders
                                       join track in _context.WorkOrderTrackings on order.Id equals track.WorkOrderId
                                       join usr in _context.ApplicationUser on track.AssignedTo equals usr.Id
                                       join req in _context.Request on order.RequestId equals req.Id
                                       where usr.HospitalId == UserObj.HospitalId
                                       && track.AssignedTo == userId
                                       select order).ToList();


                    foreach (var assigned in lstAssigned)
                    {
                        IndexRequestVM.GetData getDataObj = new IndexRequestVM.GetData();
                        getDataObj.Id = assigned.Request.Id;
                        getDataObj.Code = assigned.Request.RequestCode;
                        getDataObj.CreatedById = assigned.Request.CreatedById;
                        getDataObj.UserName = assigned.Request.User.UserName;
                        getDataObj.Subject = assigned.Request.Subject;
                        getDataObj.RequestDate = assigned.Request.RequestDate;
                        getDataObj.AssetDetailId = assigned.Request.AssetDetailId != null ? (int)assigned.Request.AssetDetailId : 0;


                        getDataObj.HospitalId = assigned.Request.AssetDetail.HospitalId;
                        var lstStatus = _context.RequestTracking
                               .Include(t => t.Request).Include(t => t.RequestStatus)
                               .Where(a => a.RequestId == assigned.Request.Id).ToList().OrderByDescending(a => a.DescriptionDate).ToList();
                        if (lstStatus.Count > 0)
                        {
                            getDataObj.StatusId = lstStatus[0].RequestStatus.Id;
                            getDataObj.StatusName = lstStatus[0].RequestStatus.Name;
                            getDataObj.StatusNameAr = lstStatus[0].RequestStatus.NameAr;
                            getDataObj.StatusColor = lstStatus[0].RequestStatus.Color;
                            getDataObj.StatusIcon = lstStatus[0].RequestStatus.Icon;
                        }
                        getDataObj.SerialNumber = assigned.Request.AssetDetail.SerialNumber;
                        getDataObj.ModeId = assigned.Request.RequestModeId != null ? (int)assigned.Request.RequestModeId : 0;
                        getDataObj.ModeName = assigned.Request.RequestMode.Name;
                        getDataObj.ModeNameAr = assigned.Request.RequestMode.NameAr;
                        getDataObj.PeriorityId = assigned.Request.RequestPeriorityId != null ? (int)assigned.Request.RequestPeriorityId : 0;
                        getDataObj.PeriorityName = assigned.Request.RequestPeriority.Name;
                        getDataObj.PeriorityNameAr = assigned.Request.RequestPeriority.NameAr;
                        getDataObj.AssetName = _context.MasterAssets.Where(a => a.Id == assigned.Request.AssetDetail.MasterAssetId).ToList().FirstOrDefault().Name;
                        getDataObj.AssetNameAr = _context.MasterAssets.Where(a => a.Id == assigned.Request.AssetDetail.MasterAssetId).ToList().FirstOrDefault().NameAr;
                        getDataObj.ListTracks = _context.RequestTracking.Where(a => a.RequestId == assigned.Request.Id)
                                .ToList().Select(item => new IndexRequestTrackingVM.GetData
                                {
                                    Id = item.Id,
                                    StatusName = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().Name,
                                    StatusNameAr = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().NameAr,
                                    Description = item.Description,
                                    Date = item.DescriptionDate,
                                    StatusId = item.RequestStatusId != null ? (int)item.RequestStatusId : 0,
                                    isExpanded = (_context.RequestDocument.Where(a => a.RequestTrackingId == item.Id).Count()) > 0 ? true : false,
                                    ListDocuments = _context.RequestDocument.Where(a => a.RequestTrackingId == item.Id).ToList(),
                                }).ToList();
                        getDataObj.CountListTracks = _context.RequestTracking.Where(a => a.RequestId == assigned.Request.Id).ToList().Count;
                        getDataObj.CountWorkOrder = _context.WorkOrders.Where(a => a.RequestId == assigned.Request.Id).ToList().Count;
                        getDataObj.GovernorateId = assigned.Request.User.GovernorateId;
                        getDataObj.CityId = assigned.Request.User.CityId;
                        getDataObj.OrganizationId = assigned.Request.User.OrganizationId;
                        getDataObj.SubOrganizationId = assigned.Request.User.SubOrganizationId;
                        listWO.Add(getDataObj);
                    }

                    var lstCreatedItems = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                    list = listWO.Concat(lstCreatedItems).ToList();

                }

                if (userRoleNames.Contains("DE"))
                {
                    list = new List<IndexRequestVM.GetData>();
                }
                if (userRoleNames.Contains("HR"))
                {
                    list = new List<IndexRequestVM.GetData>();
                }

            }


            return list;
        }
        public IEnumerable<IndexRequestVM.GetData> GetAllRequestsByHospitalId(int hospitalId)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<IndexRequestVM.GetData> GetAllRequestsByHospitalUserId(int hospitalId, string userId)
        {
            throw new NotImplementedException();
        }



        public IEnumerable<IndexRequestVM.GetData> GetAllRequestsByAssetId(int assetId, int hospitalId)
        {
            List<IndexRequestVM.GetData> list = new List<IndexRequestVM.GetData>();
            var lstRequests = _context.Request
                      .Include(t => t.AssetDetail)
                      .Include(t => t.AssetDetail.MasterAsset)
                      .Include(t => t.User)
                      .Include(t => t.RequestMode)
                      .Include(t => t.RequestPeriority)
                      .Where(a => a.AssetDetailId == assetId && a.AssetDetail.HospitalId == hospitalId).ToList();
            foreach (var req in lstRequests)
            {
                IndexRequestVM.GetData getDataObj = new IndexRequestVM.GetData();
                getDataObj.RequestId = req.Id;
                getDataObj.Id = req.Id;
                getDataObj.Code = req.RequestCode;
                getDataObj.CreatedById = req.CreatedById;
                getDataObj.UserName = req.User.UserName;
                getDataObj.Subject = req.Subject;
                getDataObj.RequestDate = req.RequestDate;
                getDataObj.AssetDetailId = req.AssetDetailId != null ? (int)req.AssetDetailId : 0;
                getDataObj.HospitalId = req.AssetDetail.HospitalId;
                var lstStatus = _context.RequestTracking
                                 .Include(t => t.Request).Include(t => t.RequestStatus)
                                 .Where(a => a.RequestId == req.Id).ToList().OrderByDescending(a => a.Id).ToList();
                if (lstStatus.Count > 0)
                {
                    getDataObj.StatusId = lstStatus[0].RequestStatus.Id;
                    getDataObj.StatusName = lstStatus[0].RequestStatus.Name;
                    getDataObj.StatusNameAr = lstStatus[0].RequestStatus.NameAr;
                    getDataObj.StatusColor = lstStatus[0].RequestStatus.Color;
                    getDataObj.StatusIcon = lstStatus[0].RequestStatus.Icon;
                }
                getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                getDataObj.ModeId = req.RequestModeId != null ? (int)req.RequestModeId : 0;
                getDataObj.ModeName = req.RequestMode.Name;
                getDataObj.ModeNameAr = req.RequestMode.NameAr;
                getDataObj.PeriorityId = req.RequestPeriorityId != null ? (int)req.RequestPeriorityId : 0;
                getDataObj.PeriorityName = req.RequestPeriority.Name;
                getDataObj.PeriorityNameAr = req.RequestPeriority.NameAr;
                getDataObj.AssetName = req.AssetDetail.MasterAsset.Name;
                getDataObj.AssetNameAr = req.AssetDetail.MasterAsset.NameAr;
                getDataObj.CountListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id).ToList().Count;
                getDataObj.GovernorateId = req.User.GovernorateId;
                getDataObj.CityId = req.User.CityId;
                getDataObj.OrganizationId = req.User.OrganizationId;
                getDataObj.SubOrganizationId = req.User.SubOrganizationId;
                list.Add(getDataObj);
            }
            return list;
        }


        public IndexRequestsVM GetRequestByWorkOrderId(int workOrderId)
        {


            var requestObj = _context.WorkOrders
                                    .Include(a => a.Request)
                                    .Include(t => t.Request.AssetDetail)
                                    .Include(t => t.User)
                                    .Include(t => t.Request.RequestMode)

                                    .Include(t => t.Request.RequestPeriority).ToList().Select(item => new IndexRequestsVM
                                    {
                                        Id = item.Id,

                                        RequestCode = item.Request.RequestCode,
                                        CreatedById = item.CreatedById,
                                        Subject = item.Request.Subject,
                                        AssetCode = item.Request.AssetDetail.Code,
                                        RequestDate = item.Request.RequestDate,
                                        //  AssetDetailId = item.Request.AssetDetailId,
                                        AssetDetailId = item.Request.AssetDetailId != null ? (int)item.Request.AssetDetailId : 0,

                                        HospitalId = (int)item.Request.AssetDetail.HospitalId,
                                        SerialNumber = item.Request.AssetDetail.SerialNumber,
                                        ModeName = item.Request.RequestMode.Name,
                                        ModeNameAr = item.Request.RequestMode.NameAr,
                                        PeriorityName = item.Request.RequestPeriority.Name,
                                        RequestTypeName = _context.RequestTypes.Where(a => a.Id == item.Request.RequestTypeId).ToList().FirstOrDefault().Name,
                                        RequestTypeNameAr = _context.RequestTypes.Where(a => a.Id == item.Request.RequestTypeId).ToList().FirstOrDefault().NameAr,

                                        SubProblemName = _context.SubProblems.Where(a => a.Id == item.Request.SubProblemId).ToList().FirstOrDefault().Name,
                                        SubProblemNameAr = _context.SubProblems.Where(a => a.Id == item.Request.SubProblemId).ToList().FirstOrDefault().NameAr,
                                        Description = item.Request.Description,

                                        PeriorityNameAr = item.Request.RequestPeriority.NameAr,
                                        AssetName = _context.MasterAssets.Where(a => a.Id == item.Request.AssetDetail.MasterAssetId).ToList().FirstOrDefault().Name,
                                        AssetNameAr = _context.MasterAssets.Where(a => a.Id == item.Request.AssetDetail.MasterAssetId).ToList().FirstOrDefault().NameAr,
                                        GovernorateId = (int)item.User.GovernorateId,
                                        CityId = (int)item.User.CityId,
                                        OrganizationId = (int)item.User.OrganizationId,
                                        SubOrganizationId = (int)item.User.SubOrganizationId,
                                    }).Where(a => a.Id == workOrderId).FirstOrDefault();


            return requestObj;
        }
        public int GetTotalRequestForAssetInHospital(int assetDetailId)
        {
            var lstRequestsByAsset = _context.Request
                                    .Include(t => t.AssetDetail).Where(a => a.AssetDetailId == assetDetailId).ToList();
            if (lstRequestsByAsset.Count > 0)
            {
                return lstRequestsByAsset.Count;
            }

            return 0;
        }
        public GeneratedRequestNumberVM GenerateRequestNumber()
        {
            GeneratedRequestNumberVM numberObj = new GeneratedRequestNumberVM();
            string WO = "Req";

            var lstIds = _context.Request.ToList();
            if (lstIds.Count > 0)
            {
                var code = lstIds.LastOrDefault().Id;
                numberObj.RequestCode = WO + (code + 1);
            }
            else
            {
                numberObj.RequestCode = WO + 1;
            }

            return numberObj;
        }
        public IEnumerable<IndexRequestVM.GetData> SearchRequests(SearchRequestVM searchObj)
        {
            List<IndexRequestVM.GetData> lstData = new List<IndexRequestVM.GetData>();
            ApplicationUser UserObj = new ApplicationUser();
            List<string> userRoleNames = new List<string>();
            var obj = _context.ApplicationUser.Where(a => a.Id == searchObj.UserId).ToList();
            if (obj.Count > 0)
            {
                UserObj = obj[0];
                var roleNames = (from userRole in _context.UserRoles
                                 join role in _context.Roles on userRole.RoleId equals role.Id
                                 where userRole.UserId == searchObj.UserId
                                 select role);
                foreach (var name in roleNames)
                {
                    userRoleNames.Add(name.Name);
                }
            }



            var list = _context.Request
                            .Include(a => a.RequestPeriority)
                            .Include(a => a.RequestMode)
                            .Include(a => a.User)
                            .Include(a => a.AssetDetail)
                            .Include(a => a.AssetDetail.MasterAsset)
                            .Include(a => a.AssetDetail.Hospital)
                            .ToList();


            if (list.Count > 0)
            {
                if (UserObj.GovernorateId == 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
                {
                    list = list.ToList();
                }

                if (UserObj.GovernorateId > 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
                {
                    list = list.Where(t => t.AssetDetail.Hospital.GovernorateId == UserObj.GovernorateId).ToList();
                }
                if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId == 0)
                {
                    list = list.Where(t => t.AssetDetail.Hospital.GovernorateId == UserObj.GovernorateId && t.AssetDetail.Hospital.CityId == UserObj.CityId).ToList();
                }
                if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId > 0)
                {
                    list = list.Where(t => t.AssetDetail.Hospital.GovernorateId == UserObj.GovernorateId && t.AssetDetail.Hospital.CityId == UserObj.CityId && t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                }
                if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId == 0 && UserObj.HospitalId == 0)
                {
                    list = list.Where(t => t.AssetDetail.Hospital.OrganizationId == UserObj.OrganizationId).ToList();
                }
                if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId == 0)
                {
                    list = list.Where(t => t.AssetDetail.Hospital.OrganizationId == UserObj.OrganizationId && t.AssetDetail.Hospital.SubOrganizationId == UserObj.SubOrganizationId).ToList();
                }

                if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId > 0)
                {
                    list = list.Where(t => t.AssetDetail.Hospital.SubOrganizationId == UserObj.SubOrganizationId && t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                }
            }

            foreach (var item in list)
            {
                IndexRequestVM.GetData getDataObj = new IndexRequestVM.GetData();
                getDataObj.Id = item.Id;
                getDataObj.Code = item.RequestCode;
                getDataObj.Subject = item.Subject;
                getDataObj.RequestDate = item.RequestDate;
                getDataObj.HospitalId = item.AssetDetail.HospitalId;
                getDataObj.CreatedById = item.CreatedById;
                getDataObj.AssetDetailId = item.AssetDetailId;
                getDataObj.MasterAssetId = item.AssetDetail.MasterAssetId;
                getDataObj.SerialNumber = item.AssetDetail.SerialNumber;
                var lstStatus = _context.RequestTracking
                            .Include(t => t.Request).Include(t => t.RequestStatus)
                            .Where(a => a.RequestId == item.Id).ToList().OrderByDescending(a => a.Id).ToList();
                if (lstStatus.Count > 0)
                {

                    getDataObj.StatusId = (int)lstStatus[0].RequestStatus.Id;
                    getDataObj.StatusName = lstStatus[0].RequestStatus.Name;
                    getDataObj.StatusNameAr = lstStatus[0].RequestStatus.NameAr;
                    getDataObj.StatusColor = lstStatus[0].RequestStatus.Color;
                    getDataObj.StatusIcon = lstStatus[0].RequestStatus.Icon;
                }

                if (item.AssetDetailId != null)
                {
                    getDataObj.AssetDetailId = item.AssetDetailId != null ? (int)item.AssetDetailId : 0;
                    //getDataObj.AssetName = item.AssetDetail.MasterAsset.Name;
                    //getDataObj.AssetNameAr = item.AssetDetail.MasterAsset.NameAr;
                }
                if (item.AssetDetail.MasterAssetId != null)
                {
                    getDataObj.MasterAssetId = item.AssetDetail.MasterAsset != null ? (int)item.AssetDetail.MasterAsset.Id : 0;
                    getDataObj.AssetName = item.AssetDetail.MasterAsset.Name;
                    getDataObj.AssetNameAr = item.AssetDetail.MasterAsset.NameAr;
                }


                if (item.RequestModeId != null)
                {
                    getDataObj.ModeId = item.RequestMode.Id;
                    getDataObj.ModeName = item.RequestMode.Name;
                    getDataObj.ModeNameAr = item.RequestMode.NameAr;
                }


                if (item.RequestPeriorityId != null)
                {
                    getDataObj.PeriorityId = (int)item.RequestPeriorityId;
                    getDataObj.PeriorityName = item.RequestPeriority.Name;
                    getDataObj.PeriorityNameAr = item.RequestPeriority.NameAr;
                    getDataObj.PeriorityColor = item.RequestPeriority.Color;
                    getDataObj.PeriorityIcon = item.RequestPeriority.Icon;
                }

                getDataObj.ListTracks = _context.RequestTracking.Where(a => a.RequestId == item.Id)
                .ToList().Select(item => new IndexRequestTrackingVM.GetData
                {
                    Id = item.Id,
                    StatusName = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().Name,
                    StatusNameAr = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().NameAr,
                    Description = item.Description,
                    Date = item.DescriptionDate,
                    StatusId = item.RequestStatus.Id,
                    isExpanded = (_context.RequestDocument.Where(a => a.RequestTrackingId == item.Id).Count()) > 0 ? true : false,
                    ListDocuments = _context.RequestDocument.Where(a => a.RequestTrackingId == item.Id).ToList(),
                }).ToList();
                getDataObj.CountListTracks = _context.RequestTracking.Where(a => a.RequestId == item.Id).ToList().Count;
                getDataObj.CountWorkOrder = _context.WorkOrders.Where(a => a.RequestId == item.Id).ToList().Count;




                lstData.Add(getDataObj);
            }

            if (searchObj.StatusId != 0)
            {
                lstData = lstData.Where(a => a.StatusId == searchObj.StatusId).ToList();
            }
            else
                lstData = lstData.ToList();

            if (searchObj.PeriorityId != 0)
            {
                lstData = lstData.Where(a => a.PeriorityId == searchObj.PeriorityId).ToList();
            }
            else
                lstData = lstData.ToList();


            if (searchObj.ModeId != 0)
            {
                lstData = lstData.Where(a => a.ModeId == searchObj.ModeId).ToList();
            }
            else
                lstData = lstData.ToList();



            if (searchObj.HospitalId != 0)
            {
                lstData = lstData.Where(a => a.HospitalId == searchObj.HospitalId).ToList();
            }
            else
                lstData = lstData.ToList();


            if (searchObj.AssetDetailId != 0)
            {
                lstData = lstData.Where(a => a.AssetDetailId == searchObj.AssetDetailId).ToList();
            }
            else
                lstData = lstData.ToList();

            if (searchObj.MasterAssetId != 0)
            {
                lstData = lstData.Where(a => a.MasterAssetId == searchObj.MasterAssetId).ToList();
            }
            else
                lstData = lstData.ToList();





            if (searchObj.AssetOwnerId != 0)
            {

                //MyList.Any(c => c.Id == MyObject.Id)

                var lstEmpEmails = _context.Employees.Where(e => e.Id == searchObj.AssetOwnerId).ToList();
                if (lstEmpEmails.Count > 0)
                {
                    var emailObj = _context.ApplicationUser.Where(a => a.Email == lstEmpEmails[0].Email).ToList();
                    if (emailObj.Count > 0)
                    {
                        lstData = lstData.Where(a => a.CreatedById == emailObj[0].Id).ToList();
                    }
                }
            }
            else
                lstData = lstData.ToList();


            if (searchObj.Subject != "")
            {
                lstData = lstData.Where(a => a.Subject == searchObj.Subject).ToList();
            }
            else
                lstData = lstData.ToList();

            if (searchObj.Code != "")
            {
                lstData = lstData.Where(b => b.Code == searchObj.Code).ToList();
            }
            else
                lstData = lstData.ToList();



            string setstartday, setstartmonth, setendday, setendmonth = "";
            DateTime startingFrom = new DateTime();
            DateTime endingTo = new DateTime();
            if (searchObj.Start == "")
            {
                //  searchObj.StartDate = DateTime.Parse("01/01/1900");
            }
            else
            {
                searchObj.StartDate = DateTime.Parse(searchObj.Start.ToString());


                var startyear = searchObj.StartDate.Value.Year;
                var startmonth = searchObj.StartDate.Value.Month;
                var startday = searchObj.StartDate.Value.Day;
                if (startday < 10)
                    setstartday = searchObj.StartDate.Value.Day.ToString().PadLeft(2, '0');
                else
                    setstartday = searchObj.StartDate.Value.Day.ToString();

                if (startmonth < 10)
                    setstartmonth = searchObj.StartDate.Value.Month.ToString().PadLeft(2, '0');
                else
                    setstartmonth = searchObj.StartDate.Value.Month.ToString();

                var sDate = startyear + "/" + setstartmonth + "/" + setstartday;
                startingFrom = DateTime.Parse(sDate).AddDays(1);
            }

            if (searchObj.End == "")
            {
                //  searchObj.EndDate = DateTime.Today.Date;
            }
            else
            {
                searchObj.EndDate = DateTime.Parse(searchObj.End.ToString());

                var endyear = searchObj.EndDate.Value.Year;
                var endmonth = searchObj.EndDate.Value.Month;
                var endday = searchObj.EndDate.Value.Day;
                if (endday < 10)
                    setendday = searchObj.EndDate.Value.Day.ToString().PadLeft(2, '0');
                else
                    setendday = searchObj.EndDate.Value.Day.ToString();

                if (endmonth < 10)
                    setendmonth = searchObj.EndDate.Value.Month.ToString().PadLeft(2, '0');
                else
                    setendmonth = searchObj.EndDate.Value.Month.ToString();

                var eDate = endyear + "/" + setendmonth + "/" + setendday;
                endingTo = DateTime.Parse(eDate).AddDays(1);
            }



            if (searchObj.Start != "" && searchObj.End != "")
            {
                lstData = lstData.Where(a => a.RequestDate >= startingFrom && a.RequestDate <= endingTo).ToList();
            }






            return lstData;
        }
        public async Task<IEnumerable<IndexRequestsVM>> SortRequests(SortRequestVM sortObj)
        {
            List<IndexRequestsVM> request = new List<IndexRequestsVM>();
            if (sortObj.UserId != null)
            {
                var userObj = await _context.Users.FindAsync(sortObj.UserId);
                Employee empObj = new Employee();
                List<string> userRoleNames = new List<string>();
                var roles = (from userRole in _context.UserRoles
                             join role in _context.ApplicationRole on userRole.RoleId equals role.Id
                             where userRole.UserId == sortObj.UserId
                             select role);
                foreach (var role in roles)
                {
                    userRoleNames.Add(role.Name);
                }


                var lstEmployees = _context.Employees.Where(a => a.Email == userObj.Email).ToList();
                if (lstEmployees.Count > 0)
                {
                    empObj = lstEmployees[0];
                }

                var lstRequests = _context.Request.Include(r => r.RequestPeriority)
                                         .Include(r => r.AssetDetail).Include(r => r.AssetDetail.MasterAsset)
                                         .Include(r => r.AssetDetail.Hospital).Include(r => r.AssetDetail.Hospital.Governorate)
                                         .Include(r => r.AssetDetail.Hospital.City).Include(r => r.AssetDetail.Hospital.Organization).Include(r => r.AssetDetail.Hospital.SubOrganization)
                                         .Include(r => r.RequestType)
                                         .Include(r => r.SubProblem)
                                         .Include(r => r.RequestMode)
                                         .Include(r => r.User).OrderByDescending(p => p.RequestDate).ToList();

                foreach (var req in lstRequests)
                {
                    IndexRequestsVM getDataObj = new IndexRequestsVM();
                    getDataObj.Id = req.Id;
                    getDataObj.Code = req.RequestCode;
                    getDataObj.Subject = req.Subject;
                    getDataObj.RequestCode = req.RequestCode;
                    getDataObj.Description = req.Description;
                    getDataObj.RequestDate = req.RequestDate;
                    getDataObj.RequestModeId = req.RequestModeId != null ? (int)req.RequestModeId : 0;
                    getDataObj.ModeName = req.RequestMode.Name;
                    getDataObj.SubProblemId = req.SubProblemId;
                    getDataObj.SubProblemName = req.SubProblem.Name;
                    getDataObj.RequestTypeId = req.RequestTypeId;
                    getDataObj.RequestTypeName = req.RequestType.Name;
                    getDataObj.RequestPeriorityId = req.RequestPeriorityId != null ? (int)req.RequestPeriorityId : 0;
                    getDataObj.PeriorityName = req.RequestPeriority.Name;
                    getDataObj.PeriorityNameAr = req.RequestPeriority.NameAr;
                    getDataObj.PeriorityColor = req.RequestPeriority.Color;
                    getDataObj.PeriorityIcon = req.RequestPeriority.Icon;
                    getDataObj.CreatedById = req.CreatedById;
                    getDataObj.CreatedBy = req.User.UserName;
                    getDataObj.AssetDetailId = req.AssetDetailId != null ? (int)req.AssetDetailId : 0;
                    getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                    getDataObj.AssetName = req.AssetDetail.MasterAsset.Name;
                    getDataObj.AssetNameAr = req.AssetDetail.MasterAsset.NameAr;
                    getDataObj.UserId = req.User.Id;
                    getDataObj.HospitalId = (int)req.AssetDetail.HospitalId;
                    getDataObj.GovernorateId = (int)req.AssetDetail.Hospital.GovernorateId;
                    getDataObj.CityId = (int)req.AssetDetail.Hospital.CityId;
                    getDataObj.OrganizationId = (int)req.AssetDetail.Hospital.OrganizationId;
                    getDataObj.SubOrganizationId = (int)req.AssetDetail.Hospital.SubOrganizationId;

                    var lstStatus = _context.RequestTracking
                                     .Include(t => t.Request).Include(t => t.RequestStatus)
                                     .Where(a => a.RequestId == req.Id).ToList().OrderByDescending(a => a.Id).ToList();
                    if (lstStatus.Count > 0)
                    {
                        getDataObj.StatusId = lstStatus[0].RequestStatus.Id;
                        getDataObj.StatusName = lstStatus[0].RequestStatus.Name;
                        getDataObj.StatusNameAr = lstStatus[0].RequestStatus.NameAr;
                        getDataObj.StatusColor = lstStatus[0].RequestStatus.Color;
                        getDataObj.StatusIcon = lstStatus[0].RequestStatus.Icon;
                    }
                    getDataObj.ListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id)
                                            .ToList().Select(item => new IndexRequestTrackingVM.GetData
                                            {
                                                Id = item.Id,
                                                StatusName = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().Name,
                                                StatusNameAr = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().NameAr,
                                                Description = item.Description,
                                                Date = item.DescriptionDate,
                                                StatusId = item.RequestStatus.Id,
                                                isExpanded = (_context.RequestDocument.Where(a => a.RequestTrackingId == item.Id).Count()) > 0 ? true : false,
                                                ListDocuments = _context.RequestDocument.Where(a => a.RequestTrackingId == item.Id).ToList(),
                                            }).ToList();
                    getDataObj.CountListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id).ToList().Count;
                    getDataObj.CountWorkOrder = _context.WorkOrders.Where(a => a.RequestId == req.Id).ToList().Count;
                    request.Add(getDataObj);
                }





                if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                {
                    request = request.ToList();
                }
                if (userObj.GovernorateId > 0 && userObj.CityId == 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                {
                    request = request.Where(a => a.GovernorateId == userObj.GovernorateId).ToList();
                }

                if (userObj.GovernorateId > 0 && userObj.CityId > 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                {
                    request = request.Where(a => a.GovernorateId == userObj.GovernorateId && a.CityId == userObj.CityId).ToList();
                }

                if (userObj.GovernorateId > 0 && userObj.CityId > 0 && userObj.HospitalId > 0)
                {
                    if (userRoleNames.Contains("AssetOwner"))
                    {
                        request = request.Where(a => a.GovernorateId == userObj.GovernorateId && a.CityId == userObj.CityId && a.HospitalId == userObj.HospitalId && a.CreatedById == userObj.Id).ToList();
                    }
                    else
                    {
                        request = request.Where(a => a.GovernorateId == userObj.GovernorateId && a.CityId == userObj.CityId && a.HospitalId == userObj.HospitalId).ToList();
                    }
                }
                if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId > 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                {
                    request = request.Where(a => a.OrganizationId == userObj.OrganizationId).ToList();
                }
                if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId > 0 && userObj.SubOrganizationId > 0 && userObj.HospitalId == 0)
                {
                    request = request.Where(a => a.OrganizationId == userObj.OrganizationId && a.SubOrganizationId == userObj.SubOrganizationId).ToList();
                }

                if (userObj.OrganizationId > 0 && userObj.SubOrganizationId > 0 && userObj.HospitalId > 0)
                {
                    if (userRoleNames.Contains("AssetOwner"))
                    {
                        request = request.Where(a => a.OrganizationId == userObj.OrganizationId && a.SubOrganizationId == userObj.SubOrganizationId && a.HospitalId == userObj.HospitalId && a.CreatedById == userObj.Id).ToList();
                    }
                    else
                    {
                        request = request.Where(a => a.OrganizationId == userObj.OrganizationId && a.SubOrganizationId == userObj.SubOrganizationId && a.HospitalId == userObj.HospitalId).ToList();
                    }
                }

                if (sortObj.Code != "")
                {
                    if (sortObj.SortStatus == "descending")
                        request = request.OrderByDescending(d => d.RequestCode).ToList();
                    else
                        request = request.OrderBy(d => d.RequestCode).ToList();
                }
                else if (sortObj.AssetName != "")
                {
                    if (sortObj.SortStatus == "descending")
                        request = request.OrderByDescending(d => d.AssetName).ToList();
                    else
                        request = request.OrderBy(d => d.AssetName).ToList();
                }
                else if (sortObj.AssetNameAr != "")
                {
                    if (sortObj.SortStatus == "descending")
                        request = request.OrderByDescending(d => d.AssetNameAr).ToList();
                    else
                        request = request.OrderBy(d => d.AssetNameAr).ToList();
                }
                else if (sortObj.Subject != "")
                {
                    if (sortObj.SortStatus == "descending")
                        request = request.OrderByDescending(d => d.Subject).ToList();
                    else
                        request = request.OrderBy(d => d.Subject).ToList();
                }
                else if (sortObj.PeriorityName != "")
                {
                    if (sortObj.SortStatus == "descending")
                        request = request.OrderByDescending(d => d.PeriorityName).ToList();
                    else
                        request = request.OrderBy(d => d.PeriorityName).ToList();
                }
                else if (sortObj.PeriorityNameAr != "")
                {
                    if (sortObj.SortStatus == "descending")
                        request = request.OrderByDescending(d => d.PeriorityNameAr).ToList();
                    else
                        request = request.OrderBy(d => d.PeriorityNameAr).ToList();
                }
                else if (sortObj.StatusName != "")
                {
                    if (sortObj.SortStatus == "descending")
                        request = request.OrderByDescending(d => d.StatusName).ToList();
                    else
                        request = request.OrderBy(d => d.StatusName).ToList();
                }
                else if (sortObj.StatusNameAr != "")
                {
                    if (sortObj.SortStatus == "descending")
                        request = request.OrderByDescending(d => d.StatusNameAr).ToList();
                    else
                        request = request.OrderBy(d => d.StatusNameAr).ToList();
                }
                else if (sortObj.ModeName != "")
                {
                    if (sortObj.SortStatus == "descending")
                        request = request.OrderByDescending(d => d.ModeName).ToList();
                    else
                        request = request.OrderBy(d => d.ModeName).ToList();
                }
                else if (sortObj.ModeNameAr != "")
                {
                    if (sortObj.SortStatus == "descending")
                        request = request.OrderByDescending(d => d.ModeNameAr).ToList();
                    else
                        request = request.OrderBy(d => d.ModeNameAr).ToList();
                }
                else if (sortObj.RequestDate != null)
                {
                    if (sortObj.SortStatus == "descending")
                        request = request.OrderByDescending(d => d.RequestDate).ToList();
                    else
                        request = request.OrderBy(d => d.RequestDate).ToList();
                }
            }
            return request;
        }



        public IEnumerable<IndexRequestsVM> SortRequestsByAssetId(SortRequestVM sortObj)
        {
            List<IndexRequestsVM> request = new List<IndexRequestsVM>();

            var lstRequests = _context.Request.Include(r => r.RequestPeriority)
                                     .Include(r => r.AssetDetail).Include(r => r.AssetDetail.MasterAsset)
                                     .Include(r => r.AssetDetail.Hospital).Include(r => r.AssetDetail.Hospital.Governorate)
                                     .Include(r => r.AssetDetail.Hospital.City).Include(r => r.AssetDetail.Hospital.Organization).Include(r => r.AssetDetail.Hospital.SubOrganization)
                                     .Include(r => r.RequestType)
                                     .Include(r => r.SubProblem)
                                     .Include(r => r.RequestMode)
                                     .Include(r => r.User).OrderByDescending(p => p.RequestDate)
                                     .Where(a => a.AssetDetail.HospitalId == sortObj.HospitalId && a.AssetDetailId == sortObj.AssetId).ToList();

            foreach (var req in lstRequests)
            {
                IndexRequestsVM getDataObj = new IndexRequestsVM();
                getDataObj.Id = req.Id;
                getDataObj.Code = req.RequestCode;
                getDataObj.Subject = req.Subject;
                getDataObj.RequestCode = req.RequestCode;
                getDataObj.Description = req.Description;
                getDataObj.RequestDate = req.RequestDate;
                getDataObj.RequestModeId = req.RequestModeId != null ? (int)req.RequestModeId : 0;
                getDataObj.ModeName = req.RequestMode.Name;
                getDataObj.ModeNameAr = req.RequestMode.NameAr;
                getDataObj.SubProblemId = req.SubProblemId;
                getDataObj.SubProblemName = req.SubProblem.Name;
                getDataObj.RequestTypeId = req.RequestTypeId;
                getDataObj.RequestTypeName = req.RequestType.Name;
                getDataObj.RequestPeriorityId = req.RequestPeriorityId != null ? (int)req.RequestPeriorityId : 0;
                getDataObj.PeriorityName = req.RequestPeriority.Name;
                getDataObj.PeriorityNameAr = req.RequestPeriority.NameAr;
                getDataObj.PeriorityColor = req.RequestPeriority.Color;
                getDataObj.PeriorityIcon = req.RequestPeriority.Icon;
                getDataObj.CreatedById = req.CreatedById;
                getDataObj.CreatedBy = req.User.UserName;
                getDataObj.AssetDetailId = req.AssetDetailId != null ? (int)req.AssetDetailId : 0;
                getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                getDataObj.AssetName = req.AssetDetail.MasterAsset.Name;
                getDataObj.AssetNameAr = req.AssetDetail.MasterAsset.NameAr;
                getDataObj.UserId = req.User.Id;
                getDataObj.HospitalId = (int)req.AssetDetail.HospitalId;
                getDataObj.GovernorateId = (int)req.AssetDetail.Hospital.GovernorateId;
                getDataObj.CityId = (int)req.AssetDetail.Hospital.CityId;
                getDataObj.OrganizationId = (int)req.AssetDetail.Hospital.OrganizationId;
                getDataObj.SubOrganizationId = (int)req.AssetDetail.Hospital.SubOrganizationId;

                var lstStatus = _context.RequestTracking
                                 .Include(t => t.Request).Include(t => t.RequestStatus)
                                 .Where(a => a.RequestId == req.Id).ToList().OrderByDescending(a => a.Id).ToList();
                if (lstStatus.Count > 0)
                {
                    getDataObj.StatusId = lstStatus[0].RequestStatus.Id;
                    getDataObj.StatusName = lstStatus[0].RequestStatus.Name;
                    getDataObj.StatusNameAr = lstStatus[0].RequestStatus.NameAr;
                    getDataObj.StatusColor = lstStatus[0].RequestStatus.Color;
                    getDataObj.StatusIcon = lstStatus[0].RequestStatus.Icon;
                }

                request.Add(getDataObj);
            }


            if (sortObj.Code != "")
            {
                if (sortObj.SortStatus == "descending")
                    request = request.OrderByDescending(d => d.RequestCode).ToList();
                else
                    request = request.OrderBy(d => d.RequestCode).ToList();
            }
            //else if (sortObj.AssetName != "")
            //{
            //    if (sortObj.SortStatus == "descending")
            //        request = request.OrderByDescending(d => d.AssetName).ToList();
            //    else
            //        request = request.OrderBy(d => d.AssetName).ToList();
            //}
            //else if (sortObj.AssetNameAr != "")
            //{
            //    if (sortObj.SortStatus == "descending")
            //        request = request.OrderByDescending(d => d.AssetNameAr).ToList();
            //    else
            //        request = request.OrderBy(d => d.AssetNameAr).ToList();
            //}
            else if (sortObj.Subject != "")
            {
                if (sortObj.SortStatus == "descending")
                    request = request.OrderByDescending(d => d.Subject).ToList();
                else
                    request = request.OrderBy(d => d.Subject).ToList();
            }
            else if (sortObj.PeriorityName != "")
            {
                if (sortObj.SortStatus == "descending")
                    request = request.OrderByDescending(d => d.PeriorityName).ToList();
                else
                    request = request.OrderBy(d => d.PeriorityName).ToList();
            }
            else if (sortObj.PeriorityNameAr != "")
            {
                if (sortObj.SortStatus == "descending")
                    request = request.OrderByDescending(d => d.PeriorityNameAr).ToList();
                else
                    request = request.OrderBy(d => d.PeriorityNameAr).ToList();
            }
            else if (sortObj.StatusName != "")
            {
                if (sortObj.SortStatus == "descending")
                    request = request.OrderByDescending(d => d.StatusName).ToList();
                else
                    request = request.OrderBy(d => d.StatusName).ToList();
            }
            else if (sortObj.StatusNameAr != "")
            {
                if (sortObj.SortStatus == "descending")
                    request = request.OrderByDescending(d => d.StatusNameAr).ToList();
                else
                    request = request.OrderBy(d => d.StatusNameAr).ToList();
            }
            else if (sortObj.ModeName != "")
            {
                if (sortObj.SortStatus == "descending")
                    request = request.OrderByDescending(d => d.ModeName).ToList();
                else
                    request = request.OrderBy(d => d.ModeName).ToList();
            }
            else if (sortObj.ModeNameAr != "")
            {
                if (sortObj.SortStatus == "descending")
                    request = request.OrderByDescending(d => d.ModeNameAr).ToList();
                else
                    request = request.OrderBy(d => d.ModeNameAr).ToList();
            }
            else if (sortObj.RequestDate != null)
            {
                if (sortObj.SortStatus == "descending")
                    request = request.OrderByDescending(d => d.RequestDate).ToList();
                else
                    request = request.OrderBy(d => d.RequestDate).ToList();
            }

            return request;
        }
        public int GetTotalOpenRequest(string userId)
        {

            //DateTime today = DateTime.Today;
            //int currentDayOfWeek = (int)today.DayOfWeek;
            //DateTime sunday = today.AddDays(-currentDayOfWeek);
            //var dates = Enumerable.Range(0, 7).Select(days => sunday.AddDays(days)).ToList();
            //var startDate = dates[0];
            //var endDate = dates.Last();
            // var countOpenRequest = _context.RequestTracking.Include(a=>a.Request).Where(a => a.DescriptionDate >= startDate && a.DescriptionDate <= endDate && a.RequestStatusId == 1).Count();

            ApplicationUser UserObj = new ApplicationUser();
            List<Request> listCountRequests = new List<Request>();
            // string userRoleName = "";
            var obj = _context.ApplicationUser.Where(a => a.Id == userId).ToList();
            if (obj.Count > 0)
            {
                UserObj = obj[0];




                var roleNames = (from userRole in _context.UserRoles
                                 join role in _context.Roles on userRole.RoleId equals role.Id
                                 where userRole.UserId == userId
                                 select role.Name);


                var list = _context.Request
                                  .Include(t => t.AssetDetail)
                                  .Include(t => t.AssetDetail.Hospital)
                                  .Include(t => t.AssetDetail.Hospital.Governorate)
                                  .Include(t => t.AssetDetail.Hospital.City)
                                  .Include(t => t.AssetDetail.Hospital.Organization)
                                  .Include(t => t.AssetDetail.Hospital.SubOrganization)
                                  .Include(t => t.User)
                                  .Include(t => t.RequestMode)
                                  .Include(t => t.RequestPeriority).ToList();

                if (list.Count > 0)
                {
                    if (UserObj.GovernorateId == 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
                    {
                        if (roleNames.Contains("Supplier"))
                        {
                            list = new List<Request>();
                        }
                        else
                        {
                            list = list.ToList();
                        }
                    }

                    if (UserObj.GovernorateId > 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
                    {
                        list = list.Where(t => t.AssetDetail.Hospital.GovernorateId == UserObj.GovernorateId).ToList();
                    }
                    if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId == 0)
                    {
                        list = list.Where(t => t.AssetDetail.Hospital.GovernorateId == UserObj.GovernorateId && t.AssetDetail.Hospital.CityId == UserObj.CityId).ToList();
                    }
                    if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId > 0)
                    {
                        list = list.Where(t => t.AssetDetail.Hospital.GovernorateId == UserObj.GovernorateId && t.AssetDetail.Hospital.CityId == UserObj.CityId && t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                    }
                    if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId == 0 && UserObj.HospitalId == 0)
                    {
                        list = list.Where(t => t.AssetDetail.Hospital.OrganizationId == UserObj.OrganizationId).ToList();
                    }
                    if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId == 0)
                    {
                        list = list.Where(t => t.AssetDetail.Hospital.OrganizationId == UserObj.OrganizationId && t.AssetDetail.Hospital.SubOrganizationId == UserObj.SubOrganizationId).ToList();
                    }

                    if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId > 0)
                    {
                        if (roleNames.Contains("Admin"))
                        {
                            list = list.ToList();
                        }
                        if (roleNames.Contains("TLHospitalManager"))
                        {
                            list = list.Where(t => t.AssetDetail.Hospital.SubOrganizationId == UserObj.SubOrganizationId && t.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
                        }
                        if (roleNames.Contains("EngDepManager"))
                        {
                            list = list.Where(t => t.AssetDetail.Hospital.SubOrganizationId == UserObj.SubOrganizationId && t.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
                        }
                        if (roleNames.Contains("EngManager"))
                        {
                            list = list.Where(t => t.AssetDetail.Hospital.SubOrganizationId == UserObj.SubOrganizationId && t.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
                        }
                        if (roleNames.Contains("AssetOwner"))
                        {
                            list = list.Where(t => t.AssetDetail.Hospital.SubOrganizationId == UserObj.SubOrganizationId && t.AssetDetail.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                        }
                        if (roleNames.Contains("Eng"))
                        {

                            var lstAssigned = (from order in _context.WorkOrders
                                               join track in _context.WorkOrderTrackings on order.Id equals track.WorkOrderId
                                               join usr in _context.ApplicationUser on track.AssignedTo equals usr.Id
                                               join req in _context.Request on order.RequestId equals req.Id
                                               where usr.HospitalId == UserObj.HospitalId
                                               && track.AssignedTo == userId
                                               select order).ToList();

                        }

                        //   list = list.Where(t => t.AssetDetail.Hospital.SubOrganizationId == UserObj.SubOrganizationId && t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                    }
                }



                foreach (var item in list)
                {
                    var listWO = _context.WorkOrders.Where(a => a.RequestId == item.Id).ToList();
                    if (listWO.Count == 0)
                    {
                        listCountRequests.Add(item);
                    }
                }
            }
            return listCountRequests.Count;
        }
    }
}
