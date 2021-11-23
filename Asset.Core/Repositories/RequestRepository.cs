using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.RequestTrackingVM;
using Asset.ViewModels.RequestVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
                    //string Req = "Req";
                    //string requestCode = "";
                    //var lstIds = _context.Request.ToList();
                    //if (lstIds.Count > 0)
                    //{
                    //    var code = lstIds.LastOrDefault().Id;
                    //    requestCode = Req + (code + 1);
                    //}
                    //else
                    //{
                    //    requestCode = Req + 1;
                    //}
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
                                               RequestModeId = req.RequestModeId,
                                               ModeName = req.RequestMode.Name,
                                               SubProblemId = req.SubProblemId,
                                               SubProblemName = req.SubProblem.Name,
                                               RequestTypeId = req.RequestTypeId,
                                               RequestTypeName = req.RequestType.Name,
                                               RequestPeriorityId = req.RequestPeriorityId,
                                               PeriorityName = req.RequestPeriority.Name,
                                               CreatedById = req.CreatedById,
                                               CreatedBy = req.User.UserName,
                                               AssetDetailId = req.AssetDetailId,
                                               SerialNumber = req.AssetDetail.SerialNumber,
                                               AssetName = _context.MasterAssets.Where(t => t.Id == req.AssetDetail.MasterAssetId).FirstOrDefault().Name,
                                               AssetNameAr = _context.MasterAssets.Where(t => t.Id == req.AssetDetail.MasterAssetId).FirstOrDefault().NameAr,
                                               UserId = req.User.Id,
                                           }).OrderByDescending(p => p.RequestDate).ToList();
            //if (userId != null || userId != "")
            //{
            //    request = request.Where(a => a.UserId == userId).ToList();

            //}
            //else
            //{
            //    request = request.ToList();
            //}

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
                    Description = req.Description,
                    RequestDate = req.RequestDate,
                    RequestModeId = req.RequestModeId,
                    ModeName = req.RequestMode.Name,
                    RequestPeriorityId = req.RequestPeriorityId,
                    PeriorityName = req.RequestPeriority.Name,
                    MasterAssetId = (int)req.AssetDetail.MasterAssetId,
                    AssetName = _context.MasterAssets.Where(t => t.Id == req.AssetDetail.MasterAssetId).FirstOrDefault().Name,
                    AssetNameAr = _context.MasterAssets.Where(t => t.Id == req.AssetDetail.MasterAssetId).FirstOrDefault().NameAr,
                    AssetDetailId = req.AssetDetailId,
                    SerialNumber = req.AssetDetail.SerialNumber,
                    CreatedById = req.CreatedById,
                    CreatedBy = req.User.UserName,
                    ProblemId = req.SubProblem.ProblemId,
                    SubProblemId = req.SubProblemId,
                    SubProblemName = req.SubProblem.Name,
                    RequestTypeId = req.RequestTypeId,
                    RequestTypeName = req.RequestType.Name,
                    RequestTrackingId = _context.RequestTracking.Where(t => t.RequestId == id).FirstOrDefault().Id,
                    RequestStatusId = _context.RequestTracking.Where(t => t.RequestId == id).FirstOrDefault().RequestStatusId,
                    //  StatusName = _context.RequestTracking.Where(t => t.RequestId == id).FirstOrDefault().RequestStatus.StatusName

                };
                return requestDTO;
            }
        }
        public void Update(int Id, EditRequestVM editRequestVM)
        {

            try
            {
                Request request = new Request();
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
            string userRoleName = "";
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
            var lstRequests = _context.Request
                               .Include(t => t.AssetDetail)
                               .Include(t => t.User)
                               .Include(t => t.RequestMode)
                               .Include(t => t.RequestPeriority).ToList();



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
                getDataObj.AssetDetailId = req.AssetDetailId;
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
                }
                getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                getDataObj.ModeId = req.RequestModeId;
                getDataObj.ModeName = req.RequestMode.Name;
                getDataObj.ModeNameAr = req.RequestMode.NameAr;
                getDataObj.PeriorityId = req.RequestPeriorityId;
                getDataObj.PeriorityName = req.RequestPeriority.Name;
                getDataObj.PeriorityNameAr = req.RequestPeriority.NameAr;
                getDataObj.AssetName = _context.MasterAssets.Where(a => a.Id == req.AssetDetail.MasterAssetId).ToList().FirstOrDefault().Name;
                getDataObj.AssetNameAr = _context.MasterAssets.Where(a => a.Id == req.AssetDetail.MasterAssetId).ToList().FirstOrDefault().NameAr;
                getDataObj.ListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id)
                        .ToList().Select(item => new IndexRequestTrackingVM.GetData
                        {
                            Id = item.Id,
                            StatusName = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().Name,
                            StatusNameAr = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().NameAr,
                            Description = item.Description,
                            Date = item.DescriptionDate,
                            StatusId = item.RequestStatusId,
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

                if (userRoleName == "TLHospitalManager")
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }

                if (userRoleName == "EngDepManager")
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                if (userRoleName == "EngManager")
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }

                if (userRoleName == "Eng")
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
                        getDataObj.AssetDetailId = assigned.Request.AssetDetailId;
                        getDataObj.HospitalId = assigned.Request.AssetDetail.HospitalId;
                        var lstStatus = _context.RequestTracking
                               .Include(t => t.Request).Include(t => t.RequestStatus)
                               .Where(a => a.RequestId == assigned.Request.Id).ToList().OrderByDescending(a => a.DescriptionDate).ToList();
                        if (lstStatus.Count > 0)
                        {
                            getDataObj.StatusId = lstStatus[0].RequestStatus.Id;
                            getDataObj.StatusName = lstStatus[0].RequestStatus.Name;
                            getDataObj.StatusNameAr = lstStatus[0].RequestStatus.NameAr;
                        }
                        getDataObj.SerialNumber = assigned.Request.AssetDetail.SerialNumber;
                        getDataObj.ModeId = assigned.Request.RequestModeId;
                        getDataObj.ModeName = assigned.Request.RequestMode.Name;
                        getDataObj.ModeNameAr = assigned.Request.RequestMode.NameAr;
                        getDataObj.PeriorityId = assigned.Request.RequestPeriorityId;
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
                                    StatusId = item.RequestStatusId,
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

                if (userRoleName == "Admin")
                {
                    list = list.ToList();
                }
                if (userRoleName == "AssetOwner")
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                }
                if (userRoleName == "DE")
                {
                    list = list = new List<IndexRequestVM.GetData>();
                }
                if (userRoleName == "HR")
                {
                    list = list = new List<IndexRequestVM.GetData>();
                }


            }
            if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId > 0)
            {
                if (userRoleName == "Admin")
                {
                    list = list.ToList();
                }
                if (userRoleName == "TLHospitalManager")
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                if (userRoleName == "EngDepManager")
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                if (userRoleName == "EngManager")
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                if (userRoleName == "AssetOwner")
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                }
                if (userRoleName == "Eng")
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
                        getDataObj.AssetDetailId = assigned.Request.AssetDetailId;
                        getDataObj.HospitalId = assigned.Request.AssetDetail.HospitalId;
                        var lstStatus = _context.RequestTracking
                               .Include(t => t.Request).Include(t => t.RequestStatus)
                               .Where(a => a.RequestId == assigned.Request.Id).ToList().OrderByDescending(a => a.DescriptionDate).ToList();
                        if (lstStatus.Count > 0)
                        {
                            getDataObj.StatusId = lstStatus[0].RequestStatus.Id;
                            getDataObj.StatusName = lstStatus[0].RequestStatus.Name;
                            getDataObj.StatusNameAr = lstStatus[0].RequestStatus.NameAr;
                        }
                        getDataObj.SerialNumber = assigned.Request.AssetDetail.SerialNumber;
                        getDataObj.ModeId = assigned.Request.RequestModeId;
                        getDataObj.ModeName = assigned.Request.RequestMode.Name;
                        getDataObj.ModeNameAr = assigned.Request.RequestMode.NameAr;
                        getDataObj.PeriorityId = assigned.Request.RequestPeriorityId;
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
                                    StatusId = item.RequestStatusId,
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

                if (userRoleName == "DE")
                {
                    list = list = new List<IndexRequestVM.GetData>();
                }
                if (userRoleName == "HR")
                {
                    list = list = new List<IndexRequestVM.GetData>();
                }


            }
            return list;
        }







        public IEnumerable<IndexRequestVM.GetData> GetAllRequestsByStatusId(string userId,int assetId)
        {
            List<IndexRequestVM.GetData> list = new List<IndexRequestVM.GetData>();
            List<IndexRequestVM.GetData> listWO = new List<IndexRequestVM.GetData>();
            ApplicationUser UserObj = new ApplicationUser();
            ApplicationRole roleObj = new ApplicationRole();
            string userRoleName = "";
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

            var roleNames = (from userRole in _context.UserRoles
                             join role in _context.Roles on userRole.RoleId equals role.Id
                             where userRole.UserId == userId
                             select role);


            var lstRequests = _context.Request
                               .Include(t => t.AssetDetail)
                               .Include(t => t.AssetDetail.MasterAsset)
                               .Include(t => t.User)
                               .Include(t => t.RequestMode)
                               .Include(t => t.RequestPeriority).Where(a=>a.AssetDetailId == assetId).ToList();


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
                    getDataObj.AssetDetailId = req.AssetDetailId;
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
                    }
                    getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                    getDataObj.ModeId = req.RequestModeId;
                    getDataObj.ModeName = req.RequestMode.Name;
                    getDataObj.ModeNameAr = req.RequestMode.NameAr;
                    getDataObj.PeriorityId = req.RequestPeriorityId;
                    getDataObj.PeriorityName = req.RequestPeriority.Name;
                    getDataObj.PeriorityNameAr = req.RequestPeriority.NameAr;
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
                                StatusId = item.RequestStatusId,
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

                    if (userRoleName == "TLHospitalManager")
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                    }

                    if (userRoleName == "EngDepManager")
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                    }
                    if (userRoleName == "EngManager")
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                    }

                    if (userRoleName == "Eng")
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
                            getDataObj.AssetDetailId = assigned.Request.AssetDetailId;
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
                            }
                            getDataObj.SerialNumber = assigned.Request.AssetDetail.SerialNumber;
                            getDataObj.ModeId = assigned.Request.RequestModeId;
                            getDataObj.ModeName = assigned.Request.RequestMode.Name;
                            getDataObj.ModeNameAr = assigned.Request.RequestMode.NameAr;
                            getDataObj.PeriorityId = assigned.Request.RequestPeriorityId;
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
                                        StatusId = item.RequestStatusId,
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

                    if (userRoleName == "Admin")
                    {
                        list = list.ToList();
                    }
                    if (userRoleName == "AssetOwner")
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                    }
                    if (userRoleName == "DE")
                    {
                        list = list = new List<IndexRequestVM.GetData>();
                    }
                    if (userRoleName == "HR")
                    {
                        list = list = new List<IndexRequestVM.GetData>();
                    }


                }
                if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId > 0)
                {
                    if (userRoleName == "Admin")
                    {
                        list = list.ToList();
                    }
                    if (userRoleName == "TLHospitalManager")
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                    }
                    if (userRoleName == "EngDepManager")
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                    }
                    if (userRoleName == "EngManager")
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                    }
                    if (userRoleName == "AssetOwner")
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                    }
                    if (userRoleName == "Eng")
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
                            getDataObj.AssetDetailId = assigned.Request.AssetDetailId;
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
                            }
                            getDataObj.SerialNumber = assigned.Request.AssetDetail.SerialNumber;
                            getDataObj.ModeId = assigned.Request.RequestModeId;
                            getDataObj.ModeName = assigned.Request.RequestMode.Name;
                            getDataObj.ModeNameAr = assigned.Request.RequestMode.NameAr;
                            getDataObj.PeriorityId = assigned.Request.RequestPeriorityId;
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
                                        StatusId = item.RequestStatusId,
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

                    if (userRoleName == "DE")
                    {
                        list = new List<IndexRequestVM.GetData>();
                    }
                    if (userRoleName == "HR")
                    {
                        list = new List<IndexRequestVM.GetData>();
                    }

                }
            }


            return list;
        }







        public IEnumerable<IndexRequestVM.GetData> GetRequestsByUserIdAssetId(string userId,int statusId)
        {
            List<IndexRequestVM.GetData> list = new List<IndexRequestVM.GetData>();
            List<IndexRequestVM.GetData> listWO = new List<IndexRequestVM.GetData>();
            ApplicationUser UserObj = new ApplicationUser();
            ApplicationRole roleObj = new ApplicationRole();
            string userRoleName = "";
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

            var roleNames = (from userRole in _context.UserRoles
                             join role in _context.Roles on userRole.RoleId equals role.Id
                             where userRole.UserId == userId
                             select role);


            var lstRequests = _context.Request
                               .Include(t => t.AssetDetail)
                               .Include(t => t.User)
                               .Include(t => t.RequestMode)
                               .Include(t => t.RequestPeriority).ToList();



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
                getDataObj.AssetDetailId = req.AssetDetailId;
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
                }
                getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                getDataObj.ModeId = req.RequestModeId;
                getDataObj.ModeName = req.RequestMode.Name;
                getDataObj.ModeNameAr = req.RequestMode.NameAr;
                getDataObj.PeriorityId = req.RequestPeriorityId;
                getDataObj.PeriorityName = req.RequestPeriority.Name;
                getDataObj.PeriorityNameAr = req.RequestPeriority.NameAr;
                getDataObj.AssetName = _context.MasterAssets.Where(a => a.Id == req.AssetDetail.MasterAssetId).ToList().FirstOrDefault().Name;
                getDataObj.AssetNameAr = _context.MasterAssets.Where(a => a.Id == req.AssetDetail.MasterAssetId).ToList().FirstOrDefault().NameAr;
                getDataObj.ListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id)
                        .ToList().Select(item => new IndexRequestTrackingVM.GetData
                        {
                            Id = item.Id,
                           
                            StatusName = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().Name,
                            StatusNameAr = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().NameAr,
                            Description = item.Description,
                            Date = item.DescriptionDate,
                            StatusId = item.RequestStatusId,
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

            if (statusId != 0)
            {
                list = list.Where(c => c.StatusId == statusId).ToList();
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

                if (userRoleName == "TLHospitalManager")
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }

                if (userRoleName == "EngDepManager")
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                if (userRoleName == "EngManager")
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }

                if (userRoleName == "Eng")
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
                        getDataObj.AssetDetailId = assigned.Request.AssetDetailId;
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
                        }
                        getDataObj.SerialNumber = assigned.Request.AssetDetail.SerialNumber;
                        getDataObj.ModeId = assigned.Request.RequestModeId;
                        getDataObj.ModeName = assigned.Request.RequestMode.Name;
                        getDataObj.ModeNameAr = assigned.Request.RequestMode.NameAr;
                        getDataObj.PeriorityId = assigned.Request.RequestPeriorityId;
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
                                    StatusId = item.RequestStatusId,
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

                if (userRoleName == "Admin")
                {
                    list = list.ToList();
                }
                if (userRoleName == "AssetOwner")
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                }
                if (userRoleName == "DE")
                {
                    list = list = new List<IndexRequestVM.GetData>();
                }
                if (userRoleName == "HR")
                {
                    list = list = new List<IndexRequestVM.GetData>();
                }


            }
            if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId > 0)
            {
                if (userRoleName == "Admin")
                {
                    list = list.ToList();
                }
                if (userRoleName == "TLHospitalManager")
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                if (userRoleName == "EngDepManager")
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                if (userRoleName == "EngManager")
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                if (userRoleName == "AssetOwner")
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                }
                if (userRoleName == "Eng")
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
                        getDataObj.AssetDetailId = assigned.Request.AssetDetailId;
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
                        }
                        getDataObj.SerialNumber = assigned.Request.AssetDetail.SerialNumber;
                        getDataObj.ModeId = assigned.Request.RequestModeId;
                        getDataObj.ModeName = assigned.Request.RequestMode.Name;
                        getDataObj.ModeNameAr = assigned.Request.RequestMode.NameAr;
                        getDataObj.PeriorityId = assigned.Request.RequestPeriorityId;
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
                                    StatusId = item.RequestStatusId,
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

                if (userRoleName == "DE")
                {
                    list = new List<IndexRequestVM.GetData>();
                }
                if (userRoleName == "HR")
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
                                        RequestDate = item.Request.RequestDate,
                                        AssetDetailId = item.Request.AssetDetailId,
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
                                    }).FirstOrDefault();


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
    }
}
