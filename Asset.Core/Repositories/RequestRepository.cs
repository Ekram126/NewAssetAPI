using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.RequestTrackingVM;
using Asset.ViewModels.RequestVM;
using Asset.ViewModels.WorkOrderTrackingVM;
using Asset.ViewModels.WorkOrderVM;
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
                    request.RequestDate = DateTime.Parse(createRequestVM.StrRequestDate);// DateTime.Now; 
                    request.RequestModeId = createRequestVM.RequestModeId;
                    request.RequestPeriorityId = createRequestVM.RequestPeriorityId;
                    request.AssetDetailId = createRequestVM.AssetDetailId;
                    request.CreatedById = createRequestVM.CreatedById;
                    request.HospitalId = createRequestVM.HospitalId;
                    request.IsOpened = false;
                    if (createRequestVM.SubProblemId > 0)
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
                    var lstTransactions = _context.AssetStatusTransactions.Where(a => a.AssetDetailId == request.AssetDetailId
                    && a.StatusDate.Value.Date.Year == request.RequestDate.Date.Year
                         && a.StatusDate.Value.Date.Month == request.RequestDate.Date.Month
                              && a.StatusDate.Value.Date.Day == request.RequestDate.Date.Day

                    ).OrderBy(a => a.StatusDate).ToList();
                    if (lstTransactions.Count > 0)
                    {
                        var transObj = lstTransactions.Last();
                        _context.AssetStatusTransactions.Remove(transObj);
                        _context.SaveChanges();
                    }
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
                                               Barcode = req.AssetDetail.Barcode,
                                               Description = req.Description,
                                               RequestDate = req.RequestDate,
                                               RequestModeId = req.RequestModeId != null ? (int)req.RequestModeId : 0,
                                               ModeName = req.RequestMode.Name,
                                               SubProblemId = req.SubProblemId != null ? (int)req.SubProblemId : 0,
                                               SubProblemName = req.SubProblem.Name,
                                               RequestTypeId = req.RequestTypeId != null ? (int)req.RequestTypeId : 0,
                                               RequestTypeName = req.RequestType.Name,
                                               RequestPeriorityId = req.RequestPeriorityId != null ? (int)req.RequestPeriorityId : 0,
                                               PeriorityName = req.RequestPeriority.Name,
                                               CreatedById = req.CreatedById,
                                               ClosedDate =


                                               _context.RequestTracking.Include(t => t.Request).Include(t => t.RequestStatus).Where(a => a.RequestId == req.Id && a.DescriptionDate.HasValue && a.DescriptionDate != null).Select(a => a.DescriptionDate).ToString(),



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
            IndexRequestsVM requestDTO = new IndexRequestsVM();
            var lstRequests = _context.Request
               .Include(p => p.RequestMode).Include(r => r.User)
               .Include(r => r.SubProblem)
               .Include(r => r.SubProblem.Problem)
               .Include(p => p.RequestPeriority)
               .Include(r => r.RequestType)
               .Include(r => r.AssetDetail)
                .Include(r => r.AssetDetail.Department)
               .Include(r => r.AssetDetail.MasterAsset)
                .Include(r => r.AssetDetail.MasterAsset.brand)
                  .Include(r => r.AssetDetail.Supplier)
                   .Include(r => r.AssetDetail.Building)
                   .Include(r => r.AssetDetail.Floor)
                   .Include(r => r.AssetDetail.Room)
           .Where(e => e.Id == id).ToList();


            if (lstRequests.Count > 0)
            {
                Request req = lstRequests[0];

                requestDTO.Id = req.Id;
                requestDTO.HospitalId = int.Parse(req.HospitalId.ToString());
                requestDTO.Subject = req.Subject;
                requestDTO.RequestCode = req.RequestCode;
                requestDTO.AssetCode = req.AssetDetail.Code;
                requestDTO.AssetDetailId = int.Parse(req.AssetDetailId.ToString());
                requestDTO.Barcode = req.AssetDetail.Barcode;
                requestDTO.ModelNumber = req.AssetDetail.MasterAsset.ModelNumber;
                requestDTO.Description = req.Description;
                requestDTO.RequestDate = req.RequestDate;
                requestDTO.RequestModeId = req.RequestModeId != null ? (int)req.RequestModeId : 0;
                requestDTO.ModeName = req.RequestMode.Name;
                requestDTO.ModeNameAr = req.RequestMode.NameAr;
                requestDTO.RequestPeriorityId = req.RequestPeriorityId != null ? (int)req.RequestPeriorityId : 0;
                requestDTO.PeriorityName = req.RequestPeriority.Name;
                requestDTO.PeriorityNameAr = req.RequestPeriority.NameAr;
                requestDTO.MasterAssetId = (int)req.AssetDetail.MasterAssetId;
                requestDTO.AssetName = req.AssetDetail.MasterAsset.Name;
                requestDTO.AssetNameAr = req.AssetDetail.MasterAsset.NameAr;
                requestDTO.AssetDetailId = req.AssetDetailId != null ? (int)req.AssetDetailId : 0;
                requestDTO.SerialNumber = req.AssetDetail.SerialNumber;
                requestDTO.CreatedById = req.CreatedById;
                requestDTO.CreatedBy = req.User.UserName;
                requestDTO.ProblemId = req.SubProblem != null ? (int)req.SubProblem.ProblemId : 0;
                requestDTO.SubProblemId = req.SubProblem != null ? (int)req.SubProblemId : 0;
                requestDTO.SubProblemName = req.SubProblem != null ? req.SubProblem.Name : "";
                requestDTO.SubProblemNameAr = req.SubProblem != null ? req.SubProblem.NameAr : "";
                requestDTO.RequestTypeId = req.RequestTypeId != null ? (int)req.RequestTypeId : 0;
                requestDTO.RequestTypeName = req.RequestType.Name;
                requestDTO.RequestTypeNameAr = req.RequestType.NameAr;

                if (req.AssetDetail.Supplier != null)
                {
                    requestDTO.SupplierName = req.AssetDetail.Supplier.Name;
                    requestDTO.SupplierNameAr = req.AssetDetail.Supplier.NameAr;
                }
                if (req.AssetDetail.MasterAsset.brand != null)
                {
                    requestDTO.BrandName = req.AssetDetail.MasterAsset.brand.Name;
                    requestDTO.BrandNameAr = req.AssetDetail.MasterAsset.brand.NameAr;
                }

                requestDTO.DepartmentName = req.AssetDetail.Department != null ? req.AssetDetail.Department.Name : "";
                requestDTO.DepartmentNameAr = req.AssetDetail.Department != null ? req.AssetDetail.Department.NameAr : "";
                requestDTO.RequestTrackingId = _context.RequestTracking.Where(t => t.RequestId == id).FirstOrDefault().Id;
                requestDTO.RequestStatusId = _context.RequestTracking.Where(t => t.RequestId == id).FirstOrDefault().RequestStatusId != null ? (int)_context.RequestTracking.Where(t => t.RequestId == id).FirstOrDefault().RequestStatusId : 0;

                requestDTO.PurchaseDate = req.AssetDetail.PurchaseDate != null ? req.AssetDetail.PurchaseDate.Value.ToShortDateString() : "";
                requestDTO.InstallationDate = req.AssetDetail.InstallationDate != null ? req.AssetDetail.InstallationDate.Value.ToShortDateString() : "";
                requestDTO.OperationDate = req.AssetDetail.OperationDate != null ? req.AssetDetail.OperationDate.Value.ToShortDateString() : "";
                requestDTO.ReceivingDate = req.AssetDetail.ReceivingDate != null ? req.AssetDetail.ReceivingDate.Value.ToShortDateString() : "";
                requestDTO.PONumber = req.AssetDetail.PONumber;
                requestDTO.WarrantyExpires = req.AssetDetail.WarrantyExpires;
                requestDTO.Remarks = req.AssetDetail.Remarks;
                requestDTO.WarrantyStart = req.AssetDetail.WarrantyStart != null ? req.AssetDetail.WarrantyStart.Value.ToShortDateString() : "";
                requestDTO.WarrantyEnd = req.AssetDetail.WarrantyEnd != null ? req.AssetDetail.WarrantyEnd.Value.ToShortDateString() : "";
                requestDTO.CostCenter = req.AssetDetail.CostCenter;
                requestDTO.DepreciationRate = req.AssetDetail.DepreciationRate;
                if (req.AssetDetail.BuildingId != null)
                {
                    requestDTO.BuildingId = req.AssetDetail.BuildingId != 0 ? req.AssetDetail.BuildingId : 0;
                    requestDTO.BuildName = req.AssetDetail.Building != null ? req.AssetDetail.Building.Name : "";
                    requestDTO.BuildNameAr = req.AssetDetail.Building != null ? req.AssetDetail.Building.NameAr : "";
                }

                if (req.AssetDetail.RoomId != null)
                {
                    requestDTO.RoomId = req.AssetDetail.RoomId;
                    requestDTO.RoomName = req.AssetDetail.Room.Name;
                    requestDTO.RoomNameAr = req.AssetDetail.Room.NameAr;
                }

                if (req.AssetDetail.FloorId != null)
                {
                    requestDTO.FloorId = req.AssetDetail.FloorId;
                    requestDTO.FloorName = req.AssetDetail.Floor.Name;
                    requestDTO.FloorNameAr = req.AssetDetail.Floor.NameAr;
                }

                var lstStatus = _context.RequestTracking
                               .Include(t => t.Request).Include(t => t.RequestStatus)
                               .Where(a => a.RequestId == req.Id).ToList().OrderByDescending(a => DateTime.Parse(a.DescriptionDate.ToString())).ToList();
                if (lstStatus.Count > 0)
                {
                    requestDTO.RequestTrackDescription = lstStatus[0].Description;

                    requestDTO.StatusId = lstStatus[0].RequestStatus.Id;
                    requestDTO.StatusName = lstStatus[0].RequestStatus.Name;
                    requestDTO.StatusNameAr = lstStatus[0].RequestStatus.NameAr;
                    requestDTO.StatusColor = lstStatus[0].RequestStatus.Color;
                    requestDTO.StatusIcon = lstStatus[0].RequestStatus.Icon;
                    if (requestDTO.StatusId == 2)
                    {
                        requestDTO.ClosedDate = lstStatus[0].DescriptionDate.ToString();
                    }
                    else
                    {
                        requestDTO.ClosedDate = "";
                    }
                }


            }
            return requestDTO;
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
                request.HospitalId = editRequestVM.HospitalId;
                if (editRequestVM.SubProblemId > 0)
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
                getDataObj.RequestCode = req.RequestCode;
                getDataObj.Barcode = req.AssetDetail.Barcode;
                getDataObj.CreatedById = req.CreatedById;
                getDataObj.UserName = req.User != null ? req.User.UserName : "";
                getDataObj.Subject = req.Subject;
                getDataObj.RequestDate = req.RequestDate;
                getDataObj.AssetDetailId = req.AssetDetailId != null ? (int)req.AssetDetailId : 0;
                getDataObj.HospitalId = req.AssetDetail.HospitalId;
                getDataObj.AssetHospitalId = req.HospitalId;
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


                getDataObj.PeriorityId = req.RequestPeriority != null ? req.RequestPeriority.Id : 0;
                getDataObj.PeriorityName = req.RequestPeriority != null ? req.RequestPeriority.Name : "";
                getDataObj.PeriorityNameAr = req.RequestPeriority != null ? req.RequestPeriority.NameAr : "";
                getDataObj.PeriorityColor = req.RequestPeriority != null ? req.RequestPeriority.Color : "";
                getDataObj.PeriorityIcon = req.RequestPeriority != null ? req.RequestPeriority.Icon : "";


                getDataObj.AssetName = req.AssetDetail.MasterAsset.Name;
                getDataObj.AssetNameAr = req.AssetDetail.MasterAsset.NameAr;
                getDataObj.ListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id)
                        .ToList().Select(item => new IndexRequestTrackingVM.GetData
                        {
                            Id = item.Id,
                            StatusName = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().Name,
                            StatusNameAr = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().NameAr,
                            StatusColor = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().Color,
                            StatusIcon = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().Icon,

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
                    getDataObj.StatusName = lstWOStatus[0].WorkOrderStatus != null ? lstWOStatus[0].WorkOrderStatus.Name : "";
                    getDataObj.StatusNameAr = lstWOStatus[0].WorkOrderStatus != null ? lstWOStatus[0].WorkOrderStatus.NameAr : "";
                    getDataObj.StatusColor = lstWOStatus[0].WorkOrderStatus != null ? lstWOStatus[0].WorkOrderStatus.Color : "";
                }
                getDataObj.CountListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id).ToList().Count;
                getDataObj.CountWorkOrder = _context.WorkOrders.Where(a => a.RequestId == req.Id).ToList().Count > 0 ? _context.WorkOrders.Where(a => a.RequestId == req.Id).ToList().Count : 0;
                getDataObj.GovernorateId = req.User != null ? req.User.GovernorateId : 0;
                getDataObj.CityId = req.User != null ? req.User.CityId : 0;
                getDataObj.OrganizationId = req.User != null ? req.User.OrganizationId : 0;
                getDataObj.SubOrganizationId = req.User != null ? req.User.SubOrganizationId : 0;
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

            //if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId > 0)
            //{
            //    if (userRoleNames.Contains("Admin"))
            //    {
            //        list = list.ToList();
            //    }
            //    if (userRoleNames.Contains("TLHospitalManager"))
            //    {
            //        list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
            //    }
            //    if (userRoleNames.Contains("EngDepManager") && !userRoleNames.Contains("Eng"))
            //    {
            //        list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
            //    }
            //    if (!userRoleNames.Contains("EngManager") && userRoleNames.Contains("Eng"))
            //    {
            //        list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
            //    }
            //    if (userRoleNames.Contains("Eng") && userRoleNames.Contains("EngDepManager"))
            //    {
            //        list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
            //    }
            //    if (userRoleNames.Contains("AssetOwner"))
            //    {
            //        list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
            //    }
            //    if (userRoleNames.Contains("DE"))
            //    {
            //        list = list = new List<IndexRequestVM.GetData>();
            //    }
            //    if (userRoleNames.Contains("HR"))
            //    {
            //        list = list = new List<IndexRequestVM.GetData>();
            //    }
            //}
            if (UserObj.HospitalId > 0)
            {
                if (userRoleNames.Contains("Admin"))
                {
                    list = list.ToList();
                }
                if (userRoleNames.Contains("TLHospitalManager"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                if (userRoleNames.Contains("EngDepManager") && userRoleNames.Contains("Eng"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                if (userRoleNames.Contains("EngDepManager") && !userRoleNames.Contains("Eng"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                if (userRoleNames.Contains("Eng") && !userRoleNames.Contains("EngDepManager"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                }
                if (userRoleNames.Contains("EngManager"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                if (userRoleNames.Contains("AssetOwner"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                }

                if (userRoleNames.Contains("SRCreator") && !userRoleNames.Contains("SRReviewer"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                }
                if (userRoleNames.Contains("SRCreator") && userRoleNames.Contains("SRReviewer"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                if (userRoleNames.Contains("SRReviewer") && !userRoleNames.Contains("SRCreator"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
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
            //var lstRequests = _context.Request
            //                   .Include(t => t.AssetDetail)
            //                   .Include(t => t.AssetDetail.MasterAsset)
            //                   .Include(t => t.User)
            //                   .Include(t => t.RequestMode)
            //                   .Include(t => t.RequestPeriority).OrderByDescending(a => a.RequestDate).ToList();


            var lstRequests = _context.Request
                            .Include(t => t.AssetDetail)
                            .Include(t => t.AssetDetail.MasterAsset)
                            .Include(t => t.User)
                            .Include(t => t.RequestMode)
                            .Include(t => t.RequestPeriority).OrderByDescending(a => a.RequestDate.Date).ToList();

            if (lstRequests.Count > 0)
            {
                foreach (var req in lstRequests)
                {
                    IndexRequestVM.GetData getDataObj = new IndexRequestVM.GetData();
                    getDataObj.RequestId = req.Id;
                    getDataObj.Id = req.Id;
                    getDataObj.RequestCode = req.RequestCode;
                    getDataObj.Barcode = req.AssetDetail.Barcode;
                    getDataObj.CreatedById = req.CreatedById;
                    getDataObj.CreatedBy = req.User != null ? req.User.UserName : "";
                    getDataObj.Subject = req.Subject;
                    getDataObj.RequestDate = req.RequestDate;



                    getDataObj.AssetDetailId = req.AssetDetailId != null ? (int)req.AssetDetailId : 0;
                    getDataObj.HospitalId = req.AssetDetail.HospitalId;
                    var lstStatus = _context.RequestTracking.Include(t => t.Request).Include(t => t.RequestStatus)
                                     .Where(a => a.RequestId == req.Id).ToList().OrderByDescending(a => a.DescriptionDate).ToList();
                    if (lstStatus.Count > 0)
                    {
                        getDataObj.StatusId = lstStatus[0].RequestStatus.Id;
                        getDataObj.StatusName = lstStatus[0].RequestStatus.Name;
                        getDataObj.StatusNameAr = lstStatus[0].RequestStatus.NameAr;
                        getDataObj.StatusColor = lstStatus[0].RequestStatus.Color;
                        getDataObj.StatusIcon = lstStatus[0].RequestStatus.Icon;
                        getDataObj.Description = lstStatus[0].Description;
                        if (getDataObj.StatusId == 2)
                        {
                            getDataObj.ClosedDate = lstStatus[0].DescriptionDate.ToString();

                        }
                        else
                        {
                            getDataObj.ClosedDate = "";
                        }
                    }
                    getDataObj.Barcode = req.AssetDetail.Barcode;
                    getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                    getDataObj.ModeId = req.RequestModeId != null ? (int)req.RequestModeId : 0;
                    getDataObj.ModeName = req.RequestMode != null ? req.RequestMode.Name : "";
                    getDataObj.ModeNameAr = req.RequestMode != null ? req.RequestMode.NameAr : "";
                    getDataObj.PeriorityId = req.RequestPeriorityId != null ? (int)req.RequestPeriorityId : 0;
                    getDataObj.PeriorityName = req.RequestPeriority != null ? req.RequestPeriority.Name : "";
                    getDataObj.PeriorityNameAr = req.RequestPeriority != null ? req.RequestPeriority.NameAr : "";
                    getDataObj.PeriorityColor = req.RequestPeriority != null ? req.RequestPeriority.Color : "";
                    getDataObj.PeriorityIcon = req.RequestPeriority != null ? req.RequestPeriority.Icon : "";

                    getDataObj.AssetHospitalId = req.HospitalId;

                    getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                    getDataObj.Barcode = req.AssetDetail.Barcode;

                    getDataObj.AssetName = req.AssetDetail.MasterAsset.Name;// _context.MasterAssets.Where(a => a.Id == req.AssetDetail.MasterAssetId).ToList().FirstOrDefault().Name;
                    getDataObj.AssetNameAr = req.AssetDetail.MasterAsset.NameAr;
                    getDataObj.ListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id)
                            .ToList().Select(item => new IndexRequestTrackingVM.GetData
                            {
                                Id = item.Id,
                                StatusName = item.RequestStatusId != null ? _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().Name : "",
                                StatusNameAr = item.RequestStatusId != null ? _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().NameAr : "",
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
                        getDataObj.WOLastTrackDescription = lstWOStatus[0].Notes;
                        //  getDataObj.workorderS = lstWOStatus[0].WorkOrderStatusId;
                    }


                    getDataObj.CountListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id).ToList().Count;
                    getDataObj.CountWorkOrder = _context.WorkOrders.Where(a => a.RequestId == req.Id).ToList().Count > 0 ? _context.WorkOrders.Where(a => a.RequestId == req.Id).ToList().Count : 0;
                    getDataObj.GovernorateId = req.User != null ? req.User.GovernorateId : 0;
                    getDataObj.CityId = req.User != null ? req.User.CityId : 0;
                    getDataObj.OrganizationId = req.User != null ? req.User.OrganizationId : 0;
                    getDataObj.SubOrganizationId = req.User != null ? req.User.SubOrganizationId : 0;
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
                if (UserObj.HospitalId > 0)
                {

                    if (userRoleNames.Contains("TLHospitalManager"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                    }
                    if (userRoleNames.Contains("EngDepManager") && !userRoleNames.Contains("Eng"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                    }
                    if (userRoleNames.Contains("Eng") && userRoleNames.Contains("EngDepManager"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                    }
                    if (userRoleNames.Contains("Eng") && !userRoleNames.Contains("EngDepManager"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                    }
                    if (userRoleNames.Contains("AssetOwner"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                    }
                    if (userRoleNames.Contains("Eng"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                    }

                    //if (userRoleNames.Contains("SRCreator"))
                    //{
                    //    list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                    //}
                    //if (userRoleNames.Contains("SRReviewer"))
                    //{
                    //    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                    //}



                    if (userRoleNames.Contains("SRCreator") && !userRoleNames.Contains("SRReviewer"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                    }
                    if (userRoleNames.Contains("SRCreator") && userRoleNames.Contains("SRReviewer"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                    }
                    if (userRoleNames.Contains("SRReviewer") && !userRoleNames.Contains("SRCreator"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                    }


                }
            }

            if (statusId == 0)
            {
                list = list.ToList();
            }
            else
            {
                list = list.Where(t => t.StatusId == statusId).ToList();
            }
            return list;
        }
        public IEnumerable<IndexRequestVM.GetData> ExportRequestsByStatusId(string userId, int statusId)
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
                             .Include(t => t.AssetDetail.Department)
                            .Include(t => t.AssetDetail.MasterAsset)
                            .Include(t => t.AssetDetail.MasterAsset.brand)
                            .Include(t => t.User)
                            .Include(t => t.RequestMode)
                            .Include(t => t.RequestPeriority).OrderByDescending(a => a.RequestDate.Date).ToList();

            if (lstRequests.Count > 0)
            {
                foreach (var req in lstRequests)
                {
                    IndexRequestVM.GetData getDataObj = new IndexRequestVM.GetData();
                    getDataObj.RequestId = req.Id;
                    getDataObj.Id = req.Id;
                    getDataObj.RequestCode = req.RequestCode;
                    getDataObj.Barcode = req.AssetDetail.Barcode;
                    getDataObj.CreatedById = req.CreatedById;
                    getDataObj.CreatedBy = req.User != null ? req.User.UserName : "";
                    getDataObj.Subject = req.Subject;
                    getDataObj.RequestDate = req.RequestDate;
                    getDataObj.BrandName = req.AssetDetail.MasterAsset.brand != null ? req.AssetDetail.MasterAsset.brand.Name : "";
                    getDataObj.BrandNameAr = req.AssetDetail.MasterAsset.brand != null ? req.AssetDetail.MasterAsset.brand.NameAr : "";

                    getDataObj.DepartmentName = req.AssetDetail.Department != null ? req.AssetDetail.Department.Name : "";
                    getDataObj.DepartmentNameAr = req.AssetDetail.Department != null ? req.AssetDetail.Department.NameAr : "";


                    getDataObj.AssetDetailId = req.AssetDetailId != null ? (int)req.AssetDetailId : 0;
                    getDataObj.HospitalId = req.AssetDetail.HospitalId;
                    var lstStatus = _context.RequestTracking.Include(t => t.Request).Include(t => t.RequestStatus)
                                     .Where(a => a.RequestId == req.Id).ToList().OrderByDescending(a => a.DescriptionDate).ToList();
                    if (lstStatus.Count > 0)
                    {
                        getDataObj.StatusId = lstStatus[0].RequestStatus.Id;
                        getDataObj.StatusName = lstStatus[0].RequestStatus.Name;
                        getDataObj.StatusNameAr = lstStatus[0].RequestStatus.NameAr;
                        getDataObj.StatusColor = lstStatus[0].RequestStatus.Color;
                        getDataObj.StatusIcon = lstStatus[0].RequestStatus.Icon;
                        getDataObj.Description = lstStatus[0].Description;
                        if (getDataObj.StatusId == 2)
                        {
                            getDataObj.ClosedDate = lstStatus[0].DescriptionDate.ToString();
                        }
                        else
                        {
                            getDataObj.ClosedDate = "";
                        }
                    }
                    getDataObj.Barcode = req.AssetDetail.Barcode;
                    getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                    getDataObj.ModeId = req.RequestModeId != null ? (int)req.RequestModeId : 0;
                    getDataObj.ModeName = req.RequestMode != null ? req.RequestMode.Name : "";
                    getDataObj.ModeNameAr = req.RequestMode != null ? req.RequestMode.NameAr : "";
                    getDataObj.PeriorityId = req.RequestPeriorityId != null ? (int)req.RequestPeriorityId : 0;
                    getDataObj.PeriorityName = req.RequestPeriority != null ? req.RequestPeriority.Name : "";
                    getDataObj.PeriorityNameAr = req.RequestPeriority != null ? req.RequestPeriority.NameAr : "";
                    getDataObj.PeriorityColor = req.RequestPeriority != null ? req.RequestPeriority.Color : "";
                    getDataObj.PeriorityIcon = req.RequestPeriority != null ? req.RequestPeriority.Icon : "";

                    getDataObj.AssetHospitalId = req.HospitalId;

                    getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                    getDataObj.Barcode = req.AssetDetail.Barcode;

                    getDataObj.AssetName = req.AssetDetail.MasterAsset.Name;// _context.MasterAssets.Where(a => a.Id == req.AssetDetail.MasterAssetId).ToList().FirstOrDefault().Name;
                    getDataObj.AssetNameAr = req.AssetDetail.MasterAsset.NameAr;
                    getDataObj.ListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id)
                            .ToList().Select(item => new IndexRequestTrackingVM.GetData
                            {
                                Id = item.Id,
                                StatusName = item.RequestStatusId != null ? _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().Name : "",
                                StatusNameAr = item.RequestStatusId != null ? _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().NameAr : "",
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
                        getDataObj.WOLastTrackDescription = lstWOStatus[0].Notes;
                        //  getDataObj.workorderS = lstWOStatus[0].WorkOrderStatusId;
                    }


                    getDataObj.CountListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id).ToList().Count;
                    getDataObj.CountWorkOrder = _context.WorkOrders.Where(a => a.RequestId == req.Id).ToList().Count > 0 ? _context.WorkOrders.Where(a => a.RequestId == req.Id).ToList().Count : 0;
                    getDataObj.GovernorateId = req.User != null ? req.User.GovernorateId : 0;
                    getDataObj.CityId = req.User != null ? req.User.CityId : 0;
                    getDataObj.OrganizationId = req.User != null ? req.User.OrganizationId : 0;
                    getDataObj.SubOrganizationId = req.User != null ? req.User.SubOrganizationId : 0;
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
                if (UserObj.HospitalId > 0)
                {

                    if (userRoleNames.Contains("TLHospitalManager"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                    }
                    if (userRoleNames.Contains("EngDepManager") && !userRoleNames.Contains("Eng"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                    }
                    if (userRoleNames.Contains("Eng") && userRoleNames.Contains("EngDepManager"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                    }
                    if (userRoleNames.Contains("Eng") && !userRoleNames.Contains("EngDepManager"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                    }
                    if (userRoleNames.Contains("AssetOwner"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                    }
                    if (userRoleNames.Contains("SRCreator") && !userRoleNames.Contains("SRReviewer"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                    }
                    if (userRoleNames.Contains("SRReviewer") && !userRoleNames.Contains("SRCreator"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                    }
                    if (userRoleNames.Contains("SRReviewer") && userRoleNames.Contains("SRCreator"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                    }
                    if (userRoleNames.Contains("Eng"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                    }
                }
            }

            if (statusId == 0)
            {
                list = list.ToList();
            }
            else
            {
                list = list.Where(t => t.StatusId == statusId).ToList();
            }
            return list;
        }
        public IEnumerable<IndexRequestVM.GetData> GetAllRequestsByStatusId(string userId, int statusId, int page, int pageSize)
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
                    getDataObj.RequestCode = req.RequestCode;
                    getDataObj.Barcode = req.AssetDetail.Barcode;
                    getDataObj.CreatedById = req.CreatedById;
                    getDataObj.CreatedBy = req.User != null ? req.User.UserName : "";
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

                        getDataObj.Description = lstStatus[0].Description;
                        if (getDataObj.StatusId == 2)
                        {
                            getDataObj.ClosedDate = lstStatus[0].DescriptionDate.ToString();
                        }
                        else
                        {
                            getDataObj.ClosedDate = "";
                        }
                    }
                    getDataObj.Barcode = req.AssetDetail.Barcode;
                    getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                    getDataObj.ModeId = req.RequestModeId != null ? (int)req.RequestModeId : 0;
                    getDataObj.ModeName = req.RequestMode != null ? req.RequestMode.Name : "";
                    getDataObj.ModeNameAr = req.RequestMode != null ? req.RequestMode.NameAr : "";
                    getDataObj.PeriorityId = req.RequestPeriorityId != null ? (int)req.RequestPeriorityId : 0;
                    getDataObj.PeriorityName = req.RequestPeriority != null ? req.RequestPeriority.Name : "";
                    getDataObj.PeriorityNameAr = req.RequestPeriority != null ? req.RequestPeriority.NameAr : "";
                    getDataObj.PeriorityColor = req.RequestPeriority != null ? req.RequestPeriority.Color : "";
                    getDataObj.PeriorityIcon = req.RequestPeriority != null ? req.RequestPeriority.Icon : "";

                    getDataObj.AssetHospitalId = req.HospitalId;

                    getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                    getDataObj.Barcode = req.AssetDetail.Barcode;

                    getDataObj.AssetName = req.AssetDetail.MasterAsset.Name;// _context.MasterAssets.Where(a => a.Id == req.AssetDetail.MasterAssetId).ToList().FirstOrDefault().Name;
                    getDataObj.AssetNameAr = req.AssetDetail.MasterAsset.NameAr;
                    getDataObj.ListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id)
                            .ToList().Select(item => new IndexRequestTrackingVM.GetData
                            {
                                Id = item.Id,
                                StatusName = item.RequestStatusId != null ? _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().Name : "",
                                StatusNameAr = item.RequestStatusId != null ? _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().NameAr : "",
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
                        //  getDataObj.workorderS = lstWOStatus[0].WorkOrderStatusId;
                    }


                    getDataObj.CountListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id).ToList().Count;
                    getDataObj.CountWorkOrder = _context.WorkOrders.Where(a => a.RequestId == req.Id).ToList().Count > 0 ? _context.WorkOrders.Where(a => a.RequestId == req.Id).ToList().Count : 0;
                    getDataObj.GovernorateId = req.User != null ? req.User.GovernorateId : 0;
                    getDataObj.CityId = req.User != null ? req.User.CityId : 0;
                    getDataObj.OrganizationId = req.User != null ? req.User.OrganizationId : 0;
                    getDataObj.SubOrganizationId = req.User != null ? req.User.SubOrganizationId : 0;
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

                //if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId > 0)
                //{

                //    if (userRoleNames.Contains("TLHospitalManager"))
                //    {
                //        list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.StatusId == statusId).ToList();
                //    }

                //    if (userRoleNames.Contains("EngDepManager") && !userRoleNames.Contains("Eng"))
                //    {
                //        list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.StatusId == statusId).ToList();
                //    }
                //    if (userRoleNames.Contains("Eng") && !userRoleNames.Contains("EngDepManager"))
                //    {
                //        list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                //    }

                //    if (userRoleNames.Contains("Eng") && userRoleNames.Contains("EngDepManager"))
                //    {
                //        list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                //    }
                //    if (userRoleNames.Contains("AssetOwner"))
                //    {
                //        list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                //    }
                //    if (userRoleNames.Contains("DE"))
                //    {
                //        list = list = new List<IndexRequestVM.GetData>();
                //    }
                //    if (userRoleNames.Contains("HR"))
                //    {
                //        list = list = new List<IndexRequestVM.GetData>();
                //    }
                //}
                if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId > 0)
                {

                    if (userRoleNames.Contains("TLHospitalManager"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                    }
                    if (userRoleNames.Contains("EngDepManager") && !userRoleNames.Contains("Eng"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                    }
                    if (userRoleNames.Contains("Eng") && userRoleNames.Contains("EngDepManager"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                    }
                    if (userRoleNames.Contains("Eng") && !userRoleNames.Contains("EngDepManager"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                    }
                    if (userRoleNames.Contains("AssetOwner"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                    }
                    if (userRoleNames.Contains("Eng"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                    }
                    //if (userRoleNames.Contains("SRCreator"))
                    //{
                    //    list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                    //}
                    //if (userRoleNames.Contains("SRReviewer"))
                    //{
                    //    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                    //}


                    if (userRoleNames.Contains("SRCreator") && !userRoleNames.Contains("SRReviewer"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                    }
                    if (userRoleNames.Contains("SRReviewer") && !userRoleNames.Contains("SRCreator"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                    }
                    if (userRoleNames.Contains("SRReviewer") && userRoleNames.Contains("SRCreator"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
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

            if (statusId == 0)
            {
                list = list.ToList();
            }
            else
            {
                list = list.Where(t => t.StatusId == statusId).ToList();
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
                getDataObj.RequestCode = req.RequestCode;
                getDataObj.Barcode = req.AssetDetail.Barcode;
                getDataObj.CreatedById = req.CreatedById;
                getDataObj.UserName = req.User.UserName;
                getDataObj.Subject = req.Subject;
                getDataObj.RequestDate = req.RequestDate;
                getDataObj.AssetDetailId = req.AssetDetailId != null ? (int)req.AssetDetailId : 0;
                getDataObj.HospitalId = req.AssetDetail.HospitalId;
                getDataObj.AssetHospitalId = req.HospitalId;
                var lstStatus = _context.RequestTracking
                                 .Include(t => t.Request).Include(t => t.RequestStatus)
                                 .Where(a => a.RequestId == req.Id).ToList().OrderByDescending(a => a.DescriptionDate).ToList();
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
                            StatusColor = item.RequestStatus.Color,
                            StatusIcon = item.RequestStatus.Icon,


                            Description = item.Description,
                            Date = item.DescriptionDate,
                            StatusId = item.RequestStatus != null ? (int)item.RequestStatusId : 0,
                            isExpanded = (_context.RequestDocument.Where(a => a.RequestTrackingId == item.Id).Count()) > 0 ? true : false,
                            ListDocuments = _context.RequestDocument.Where(a => a.RequestTrackingId == item.Id).ToList(),
                        }).ToList();
                getDataObj.CountListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id).ToList().Count;
                getDataObj.CountWorkOrder = _context.WorkOrders.Where(a => a.RequestId == req.Id).ToList().Count;

                //var lstWOStatus = _context.WorkOrderTrackings
                //        .Include(o => o.WorkOrder).Include(o => o.WorkOrderStatus).Where(a => a.WorkOrder.RequestId == req.Id)
                //        .OrderByDescending(a => a.CreationDate).ToList();

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

                if (userRoleNames.Contains("EngDepManager") && !userRoleNames.Contains("Eng"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                //if (userRoleNames.Contains("EngManager"))
                //{
                //    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                //}

                if (userRoleNames.Contains("Eng") && userRoleNames.Contains("EngDepManager"))
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
                        getDataObj.RequestCode = assigned.Request.RequestCode;
                        getDataObj.Barcode = assigned.Request.AssetDetail.Barcode;
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

                                    StatusColor = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().Color,
                                    StatusIcon = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().Icon,

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


                if (userRoleNames.Contains("Eng") && !userRoleNames.Contains("EngDepManager"))
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
                        getDataObj.RequestCode = assigned.Request.RequestCode;
                        getDataObj.Barcode = assigned.Request.AssetDetail.Barcode;
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
                                    StatusColor = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().Color,
                                    StatusIcon = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().Icon,
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
                //if (userRoleNames.Contains("SRCreator"))
                //{
                //    list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                //}
                //if (userRoleNames.Contains("SRReviewer"))
                //{
                //    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                //}

                if (userRoleNames.Contains("SRCreator") && !userRoleNames.Contains("SRReviewer"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                }
                if (userRoleNames.Contains("SRReviewer") && !userRoleNames.Contains("SRCreator"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                if (userRoleNames.Contains("SRReviewer") && userRoleNames.Contains("SRCreator"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
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
                if (userRoleNames.Contains("SRCreator"))
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
                        getDataObj.RequestCode = assigned.Request.RequestCode;
                        getDataObj.Barcode = assigned.Request.AssetDetail.Barcode;
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
                                    StatusColor = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().Color,
                                    StatusIcon = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().Icon,
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

                //if (userRoleNames.Contains("SRCreator"))
                //{
                //    list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                //}
                //if (userRoleNames.Contains("SRReviewer"))
                //{
                //    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                //}

                if (userRoleNames.Contains("SRCreator") && !userRoleNames.Contains("SRReviewer"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                }
                if (userRoleNames.Contains("SRReviewer") && !userRoleNames.Contains("SRCreator"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                if (userRoleNames.Contains("SRReviewer") && userRoleNames.Contains("SRCreator"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
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
                getDataObj.RequestCode = req.RequestCode;
                getDataObj.Barcode = req.AssetDetail.Barcode;
                getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
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
                getDataObj.PeriorityName = req.RequestPeriority != null ? req.RequestPeriority.Name : "";
                getDataObj.PeriorityNameAr = req.RequestPeriority != null ? req.RequestPeriority.NameAr : "";
                getDataObj.AssetName = req.AssetDetail.MasterAsset.Name;
                getDataObj.AssetNameAr = req.AssetDetail.MasterAsset.NameAr;
                getDataObj.CountListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id).ToList().Count;
                getDataObj.GovernorateId = req.User.GovernorateId;
                getDataObj.CityId = req.User.CityId;
                getDataObj.OrganizationId = req.User.OrganizationId;
                getDataObj.SubOrganizationId = req.User.SubOrganizationId;
                getDataObj.AssetHospitalId = req.HospitalId;
                list.Add(getDataObj);
            }
            return list;
        }
        public IndexRequestsVM GetRequestByWorkOrderId(int workOrderId)
        {
            List<IndexRequestsVM> list = new List<IndexRequestsVM>();
            IndexRequestsVM reqObj = new IndexRequestsVM();

            var lstRequests = _context.WorkOrders
                                    .Include(a => a.Request)
                                    .Include(a => a.Request.RequestType)
                                     .Include(a => a.Request.SubProblem)
                                     .Include(a => a.Request.SubProblem.Problem)
                                    .Include(t => t.Request.AssetDetail)
                                     .Include(t => t.Request.AssetDetail.MasterAsset)
                                    .Include(t => t.User)
                                      .Include(t => t.Request.User)
                                    .Include(t => t.Request.RequestMode)
                                    .Include(t => t.Request.RequestPeriority).Where(a => a.Id == workOrderId).ToList();
            if (lstRequests.Count > 0)
            {
                var item = lstRequests[0];
                reqObj.Id = item.Id;
                reqObj.RequestCode = item.Request.RequestCode;
                reqObj.Barcode = item.Request.AssetDetail.Barcode;
                reqObj.CreatedById = item.CreatedById;
                reqObj.Subject = item.Request.Subject;
                reqObj.AssetCode = item.Request.AssetDetail.Code;
                reqObj.RequestDate = item.Request.RequestDate;
                reqObj.AssetHospitalId = int.Parse(item.HospitalId.ToString());
                reqObj.AssetDetailId = item.Request.AssetDetailId != null ? (int)item.Request.AssetDetailId : 0;
                reqObj.HospitalId = (int)item.Request.AssetDetail.HospitalId;
                reqObj.SerialNumber = item.Request.AssetDetail.SerialNumber;
                reqObj.ModeName = item.Request.RequestMode.Name;
                reqObj.ModeNameAr = item.Request.RequestMode.NameAr;

                reqObj.RequestTypeName = item.Request.RequestTypeId != null ? item.Request.RequestType.Name : "";
                reqObj.RequestTypeNameAr = item.Request.RequestTypeId != null ? item.Request.RequestType.NameAr : "";

                reqObj.SubProblemName = item.Request.SubProblemId != null ? item.Request.SubProblem.Name : "";
                reqObj.SubProblemNameAr = item.Request.SubProblemId != null ? item.Request.SubProblem.NameAr : "";
                reqObj.Description = item.Request.Description;

                reqObj.CreatedBy = item.Request.User.UserName;
                reqObj.PeriorityName = item.Request.RequestPeriority != null ? item.Request.RequestPeriority.Name : "";
                reqObj.PeriorityNameAr = item.Request.RequestPeriority != null ? item.Request.RequestPeriority.NameAr : "";


                reqObj.ListTracks = _context.RequestTracking.Include(a => a.User).Where(a => a.RequestId == item.RequestId).OrderByDescending(a => a.DescriptionDate)
                                      .ToList().Select(item => new IndexRequestTrackingVM.GetData
                                      {
                                          Id = item.Id,
                                          StatusName = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().Name,
                                          StatusNameAr = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().NameAr,
                                          StatusColor = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().Color,
                                          StatusIcon = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().Icon,
                                          Description = item.Description,
                                          Date = item.DescriptionDate,
                                          StatusId = item.RequestStatus.Id,
                                          UserName = item.User.UserName,
                                          ListDocuments = _context.RequestDocument.Where(a => a.RequestTrackingId == item.Id).ToList(),
                                      }).ToList();



                reqObj.AssetName = item.Request.AssetDetail.MasterAsset != null ? item.Request.AssetDetail.MasterAsset.Name : "";
                reqObj.AssetNameAr = item.Request.AssetDetail.MasterAsset != null ? item.Request.AssetDetail.MasterAsset.NameAr : "";
                reqObj.GovernorateId = (int)item.User.GovernorateId;
                reqObj.CityId = (int)item.User.CityId;
                reqObj.OrganizationId = (int)item.User.OrganizationId;
                reqObj.SubOrganizationId = (int)item.User.SubOrganizationId;
            }
            return reqObj;
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
                            .Include(a => a.AssetDetail.Department)
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
                    list = list.Where(t => t.AssetDetail.Hospital.CityId == UserObj.CityId).ToList();
                }
                if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId == 0 && UserObj.HospitalId == 0)
                {
                    list = list.Where(t => t.AssetDetail.Hospital.OrganizationId == UserObj.OrganizationId).ToList();
                }
                if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId == 0)
                {
                    list = list.Where(t => t.AssetDetail.Hospital.SubOrganizationId == UserObj.SubOrganizationId).ToList();
                }
                if (UserObj.HospitalId > 0 && searchObj.HospitalId > 0)
                {
                    list = list.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
                }
                else if (searchObj.HospitalId > 0)
                {
                    list = list.Where(t => t.AssetDetail.HospitalId == searchObj.HospitalId).ToList();
                }



                if (searchObj.PeriorityId != 0)
                {
                    list = list.Where(a => a.AssetDetail.MasterAsset.PeriorityId == searchObj.PeriorityId).ToList();
                }
                else
                    list = list.ToList();


                if (searchObj.MasterAssetId != 0)
                {
                    list = list.Where(a => a.AssetDetail.MasterAssetId == searchObj.MasterAssetId).ToList();
                }
                else
                    list = list.ToList();



                if (searchObj.Barcode != "")
                {
                    list = list.Where(a => a.AssetDetail.Barcode == searchObj.Barcode).ToList();
                }
                else
                    list = list.ToList();



                if (searchObj.SerialNumber != "")
                {
                    list = list.Where(a => a.AssetDetail.SerialNumber.Contains(searchObj.SerialNumber)).ToList();
                }
                else
                    list = list.ToList();

            }

            foreach (var item in list)
            {
                IndexRequestVM.GetData getDataObj = new IndexRequestVM.GetData();
                getDataObj.Id = item.Id;
                getDataObj.RequestCode = item.RequestCode;
                getDataObj.Barcode = item.AssetDetail.Barcode;
                getDataObj.Subject = item.Subject;
                getDataObj.AssetHospitalId = item.HospitalId;
                getDataObj.RequestDate = item.RequestDate;
                getDataObj.HospitalId = item.AssetDetail.HospitalId;
                getDataObj.CreatedById = item.CreatedById;
                getDataObj.CreatedBy = item.User.UserName;
                getDataObj.AssetDetailId = item.AssetDetailId;
                getDataObj.MasterAssetId = item.AssetDetail.MasterAssetId;
                getDataObj.SerialNumber = item.AssetDetail.SerialNumber;
                getDataObj.Barcode = item.AssetDetail.Barcode;
                getDataObj.ModelNumber = item.AssetDetail.MasterAsset.ModelNumber;

                var lstStatus = _context.RequestTracking
                            .Include(t => t.Request).Include(t => t.RequestStatus)
                            .Where(a => a.RequestId == item.Id).ToList().OrderByDescending(a => a.DescriptionDate).ToList();
                if (lstStatus.Count > 0)
                {

                    getDataObj.StatusId = (int)lstStatus[0].RequestStatus.Id;
                    getDataObj.StatusName = lstStatus[0].RequestStatus.Name;
                    getDataObj.StatusNameAr = lstStatus[0].RequestStatus.NameAr;
                    getDataObj.StatusColor = lstStatus[0].RequestStatus.Color;
                    getDataObj.StatusIcon = lstStatus[0].RequestStatus.Icon;
                    getDataObj.Description = lstStatus[0].Description;
                    getDataObj.DescriptionDate = lstStatus[0].DescriptionDate;
                    if (getDataObj.StatusId == 2)
                    {
                        getDataObj.ClosedDate = lstStatus[0].DescriptionDate.ToString();
                    }
                    else
                    {
                        getDataObj.ClosedDate = "";
                    }
                }

                if (item.AssetDetailId != null)
                {
                    getDataObj.AssetDetailId = item.AssetDetailId != null ? (int)item.AssetDetailId : 0;
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

                if (item.AssetDetail.DepartmentId != null)
                {
                    getDataObj.DepartmentId = item.AssetDetail.Department.Id;
                    getDataObj.ModeName = item.AssetDetail.Department.Name;
                    getDataObj.ModeNameAr = item.AssetDetail.Department.NameAr;
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
                    StatusColor = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().Color,
                    StatusIcon = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().Icon,
                    Description = item.Description,
                    Date = item.DescriptionDate,
                    StatusId = item.RequestStatus.Id,
                    isExpanded = (_context.RequestDocument.Where(a => a.RequestTrackingId == item.Id).Count()) > 0 ? true : false,
                    ListDocuments = _context.RequestDocument.Where(a => a.RequestTrackingId == item.Id).ToList(),
                }).ToList();
                getDataObj.CountListTracks = _context.RequestTracking.Where(a => a.RequestId == item.Id).ToList().Count;
                getDataObj.CountWorkOrder = _context.WorkOrders.Where(a => a.RequestId == item.Id).ToList().Count;

                var lstWOStatus = _context.WorkOrderTrackings
                  .Include(o => o.WorkOrder).Include(o => o.WorkOrderStatus).Where(a => a.WorkOrder.RequestId == item.Id)
                  .OrderByDescending(a => a.CreationDate).ToList();

                if (lstWOStatus.Count > 0)
                {
                    getDataObj.LatestWorkOrderStatusId = lstWOStatus[0].WorkOrderStatusId;
                    getDataObj.WOLastTrackDescription = lstWOStatus[0].Notes;
                }
                lstData.Add(getDataObj);
            }

            if (lstData.Count > 0)
            {

                if (searchObj.StatusId != 0)
                {
                    lstData = lstData.Where(a => a.StatusId == searchObj.StatusId).ToList();
                }
                else
                    lstData = lstData.ToList();


                if (searchObj.ModeId != 0)
                {
                    lstData = lstData.Where(a => a.ModeId == searchObj.ModeId).ToList();
                }
                else
                    lstData = lstData.ToList();






                if (searchObj.DepartmentId != 0)
                {
                    lstData = lstData.Where(a => a.DepartmentId == searchObj.DepartmentId).ToList();
                }
                else
                    lstData = lstData.ToList();





                if (searchObj.ModelNumber != "")
                {
                    lstData = lstData.Where(a => a.ModelNumber.Contains(searchObj.ModelNumber)).ToList();
                }
                else
                    lstData = lstData.ToList();


                if (searchObj.Subject != "")
                {
                    lstData = lstData.Where(a => a.Subject.Contains(searchObj.Subject)).ToList();
                }
                else
                    lstData = lstData.ToList();

                if (searchObj.Code != "")
                {
                    lstData = lstData.Where(b => b.RequestCode == searchObj.Code).ToList();
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
                    startingFrom = DateTime.Parse(sDate);//.AddDays(1);
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
                    endingTo = DateTime.Parse(eDate);//.AddDays(1);
                }
                if (searchObj.Start != "" && searchObj.End != "")
                {
                    lstData = lstData.Where(a => a.RequestDate.Date >= startingFrom.Date && a.RequestDate.Date <= endingTo.Date).ToList();
                }

                return lstData;
            }

            return null;
        }
        public async Task<IEnumerable<IndexRequestsVM>> SortRequests(SortRequestVM sortObj, int statusId)
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
                    getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                    getDataObj.Barcode = req.AssetDetail.Barcode;
                    getDataObj.RequestCode = req.RequestCode;
                    getDataObj.AssetHospitalId = int.Parse(req.HospitalId.ToString());
                    getDataObj.ModelNumber = req.AssetDetail.MasterAsset.ModelNumber;
                    getDataObj.MasterAssetId = req.AssetDetail.MasterAsset.Id;
                    getDataObj.Description = req.Description;
                    getDataObj.RequestDate = req.RequestDate;
                    getDataObj.RequestModeId = req.RequestModeId != null ? (int)req.RequestModeId : 0;
                    getDataObj.ModeName = req.RequestMode.Name;
                    getDataObj.ModeNameAr = req.RequestMode.NameAr;
                    getDataObj.SubProblemId = req.SubProblem != null ? (int)req.SubProblemId : 0;
                    getDataObj.SubProblemName = req.SubProblem != null ? req.SubProblem.Name : "";
                    getDataObj.RequestTypeId = req.RequestTypeId != null ? (int)req.RequestTypeId : 0;
                    getDataObj.RequestTypeName = req.RequestType != null ? req.RequestType.Name : "";
                    getDataObj.RequestPeriorityId = req.RequestPeriorityId != null ? (int)req.RequestPeriorityId : 0;
                    getDataObj.PeriorityName = req.RequestPeriority != null ? req.RequestPeriority.Name : "";
                    getDataObj.PeriorityNameAr = req.RequestPeriority != null ? req.RequestPeriority.NameAr : "";
                    getDataObj.PeriorityColor = req.RequestPeriority != null ? req.RequestPeriority.Color : "";
                    getDataObj.PeriorityIcon = req.RequestPeriority != null ? req.RequestPeriority.Icon : "";
                    getDataObj.CreatedById = req.CreatedById;
                    getDataObj.CreatedBy = req.User.UserName;
                    getDataObj.AssetDetailId = req.AssetDetailId != null ? (int)req.AssetDetailId : 0;
                    getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                    getDataObj.Barcode = req.AssetDetail.Barcode;
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
                        getDataObj.Description = lstStatus[0].Description;
                        if (getDataObj.StatusId == 2)
                        {
                            getDataObj.ClosedDate = lstStatus[0].DescriptionDate.ToString();
                        }
                        else
                        {
                            getDataObj.ClosedDate = "";
                        }
                    }
                    getDataObj.ListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id)
                                            .ToList().Select(item => new IndexRequestTrackingVM.GetData
                                            {
                                                Id = item.Id,
                                                StatusName = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().Name,
                                                StatusNameAr = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().NameAr,
                                                StatusColor = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().Color,
                                                StatusIcon = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().Icon,
                                                Description = item.Description,
                                                Date = item.DescriptionDate,
                                                StatusId = item.RequestStatus.Id,
                                                isExpanded = (_context.RequestDocument.Where(a => a.RequestTrackingId == item.Id).Count()) > 0 ? true : false,
                                                ListDocuments = _context.RequestDocument.Where(a => a.RequestTrackingId == item.Id).ToList(),
                                            }).ToList();
                    getDataObj.CountListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id).ToList().Count;
                    getDataObj.CountWorkOrder = _context.WorkOrders.Where(a => a.RequestId == req.Id).ToList().Count;

                    var lstWOStatus = _context.WorkOrderTrackings.Include(o => o.WorkOrder).Include(o => o.WorkOrderStatus).Where(a => a.WorkOrder.RequestId == req.Id).OrderByDescending(a => a.CreationDate).ToList();

                    if (lstWOStatus.Count > 0)
                    {
                        getDataObj.LatestWorkOrderStatusId = lstWOStatus[0].WorkOrderStatusId;
                        getDataObj.WOLastTrackDescription = lstWOStatus[0].Notes;
                    }




                    request.Add(getDataObj);
                }


                if (statusId == 0)
                    request = request.ToList();
                else
                    request = request.Where(a => a.StatusId == statusId).ToList();

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
                        request = request.Where(a => a.HospitalId == userObj.HospitalId && a.CreatedById == userObj.Id).ToList();
                    }
                    //if (userRoleNames.Contains("SRCreator"))
                    //{
                    //    request = request.Where(t => t.HospitalId == userObj.HospitalId && t.CreatedById == userObj.Id).ToList();
                    //}
                    //if (userRoleNames.Contains("SRReviewer"))
                    //{
                    //    request = request.Where(t => t.HospitalId == userObj.HospitalId).ToList();
                    //}

                    if (userRoleNames.Contains("SRCreator") && !userRoleNames.Contains("SRReviewer"))
                    {
                        request = request.Where(t => t.HospitalId == userObj.HospitalId && t.CreatedById == userObj.Id).ToList();
                    }
                    if (userRoleNames.Contains("SRReviewer") && !userRoleNames.Contains("SRCreator"))
                    {
                        request = request.Where(t => t.HospitalId == userObj.HospitalId).ToList();
                    }
                    if (userRoleNames.Contains("SRReviewer") && userRoleNames.Contains("SRCreator"))
                    {
                        request = request.Where(t => t.HospitalId == userObj.HospitalId).ToList();
                    }
                    else
                    {
                        request = request.Where(a => a.HospitalId == userObj.HospitalId).ToList();
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
                        request = request.Where(a => a.HospitalId == userObj.HospitalId && a.CreatedById == userObj.Id).ToList();
                    }
                    if (userRoleNames.Contains("SRCreator"))
                    {
                        request = request.Where(a => a.HospitalId == userObj.HospitalId && a.CreatedById == userObj.Id).ToList();
                    }
                    else
                    {
                        request = request.Where(a => a.HospitalId == userObj.HospitalId).ToList();
                    }
                }



                if (sortObj.Serial != "")
                {

                    if (sortObj.StrBarCode != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.Barcode).ToList();
                        }
                        else
                        {
                            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderBy(d => d.Barcode).ToList();
                        }
                    }
                    if (sortObj.StrSerial != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    //if (sortObj.StrModel != "")
                    //{
                    //    if (sortObj.SortStatus == "descending")
                    //        request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                    //    else
                    //        request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                    //}
                    if (sortObj.StrRequestCode != "")
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderByDescending(d => d.RequestCode).ToList();
                        else
                            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderBy(d => d.RequestCode).ToList();
                    }
                    if (sortObj.StrSubject != "")
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                        else
                            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.OrderByDescending(d => d.SerialNumber).ToList();
                        else
                            request = request.OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                if (sortObj.RequestCode != "")
                {
                    if (sortObj.StrBarCode != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.Barcode).ToList();
                        }
                        else
                        {
                            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderBy(d => d.Barcode).ToList();
                        }
                    }
                    if (sortObj.StrSerial != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    //if (sortObj.StrModel != "")
                    //{
                    //    if (sortObj.SortStatus == "descending")
                    //        request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                    //    else
                    //        request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                    //}
                    if (sortObj.StrRequestCode != "")
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderByDescending(d => d.RequestCode).ToList();
                        else
                            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderBy(d => d.RequestCode).ToList();
                    }
                    if (sortObj.StrSubject != "")
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                        else
                            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.OrderByDescending(d => d.RequestCode).ToList();
                        else
                            request = request.OrderBy(d => d.RequestCode).ToList();
                    }
                }
                if (sortObj.BarCode != "")
                {
                    if (sortObj.StrBarCode != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.Barcode).ToList();
                        }
                        else
                        {
                            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderBy(d => d.Barcode).ToList();
                        }
                    }
                    if (sortObj.StrSerial != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    //if (sortObj.StrModel != "")
                    //{
                    //    if (sortObj.SortStatus == "descending")
                    //        request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                    //    else
                    //        request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                    //}
                    if (sortObj.StrRequestCode != "")
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderByDescending(d => d.RequestCode).ToList();
                        else
                            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderBy(d => d.RequestCode).ToList();
                    }
                    if (sortObj.StrSubject != "")
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                        else
                            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.OrderByDescending(d => d.Barcode).ToList();
                        else
                            request = request.OrderBy(d => d.Barcode).ToList();
                    }
                }
                if (sortObj.AssetName != "")
                {
                    if (sortObj.StrBarCode != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.Barcode).ToList();
                        }
                        else
                        {
                            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderBy(d => d.Barcode).ToList();
                        }
                    }
                    if (sortObj.StrSerial != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    //if (sortObj.StrModel != "")
                    //{
                    //    if (sortObj.SortStatus == "descending")
                    //        request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                    //    else
                    //        request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                    //}
                    if (sortObj.StrRequestCode != "")
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderByDescending(d => d.RequestCode).ToList();
                        else
                            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderBy(d => d.RequestCode).ToList();
                    }
                    if (sortObj.StrSubject != "")
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                        else
                            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.OrderByDescending(d => d.AssetName).ToList();
                        else
                            request = request.OrderBy(d => d.AssetName).ToList();
                    }
                }
                if (sortObj.AssetNameAr != "")
                {
                    if (sortObj.StrBarCode != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.Barcode).ToList();
                        }
                        else
                        {
                            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderBy(d => d.Barcode).ToList();
                        }
                    }
                    if (sortObj.StrSerial != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    //if (sortObj.StrModel != "")
                    //{
                    //    if (sortObj.SortStatus == "descending")
                    //        request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                    //    else
                    //        request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                    //}
                    if (sortObj.StrRequestCode != "")
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderByDescending(d => d.RequestCode).ToList();
                        else
                            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderBy(d => d.RequestCode).ToList();
                    }
                    if (sortObj.StrSubject != "")
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                        else
                            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.OrderByDescending(d => d.AssetNameAr).ToList();
                        else
                            request = request.OrderBy(d => d.AssetNameAr).ToList();
                    }
                }
                if (sortObj.Subject != "")
                {
                    if (sortObj.StrBarCode != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.Barcode).ToList();
                        }
                        else
                        {
                            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderBy(d => d.Barcode).ToList();
                        }
                    }
                    if (sortObj.StrSerial != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    //if (sortObj.StrModel != "")
                    //{
                    //    if (sortObj.SortStatus == "descending")
                    //        request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                    //    else
                    //        request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                    //}
                    if (sortObj.StrRequestCode != "")
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderByDescending(d => d.RequestCode).ToList();
                        else
                            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderBy(d => d.RequestCode).ToList();
                    }
                    if (sortObj.StrSubject != "")
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                        else
                            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.OrderByDescending(d => d.Subject).ToList();
                        else
                            request = request.OrderBy(d => d.Subject).ToList();
                    }
                }
                if (sortObj.PeriorityName != "")
                {
                    if (sortObj.StrBarCode != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.Barcode).ToList();
                        }
                        else
                        {
                            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderBy(d => d.Barcode).ToList();
                        }
                    }
                    if (sortObj.StrSerial != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    //if (sortObj.StrModel != "")
                    //{
                    //    if (sortObj.SortStatus == "descending")
                    //        request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                    //    else
                    //        request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                    //}
                    if (sortObj.StrRequestCode != "")
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderByDescending(d => d.RequestCode).ToList();
                        else
                            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderBy(d => d.RequestCode).ToList();
                    }
                    if (sortObj.StrSubject != "")
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                        else
                            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.OrderByDescending(d => d.PeriorityName).ToList();
                        else
                            request = request.OrderBy(d => d.PeriorityName).ToList();
                    }
                }
                if (sortObj.PeriorityNameAr != "")
                {
                    if (sortObj.StrBarCode != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.Barcode).ToList();
                        }
                        else
                        {
                            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderBy(d => d.Barcode).ToList();
                        }
                    }
                    if (sortObj.StrSerial != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    //if (sortObj.StrModel != "")
                    //{
                    //    if (sortObj.SortStatus == "descending")
                    //        request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                    //    else
                    //        request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                    //}



                    if (sortObj.StrRequestCode != "")
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderByDescending(d => d.RequestCode).ToList();
                        else
                            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderBy(d => d.RequestCode).ToList();
                    }
                    if (sortObj.StrSubject != "")
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                        else
                            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.OrderByDescending(d => d.PeriorityNameAr).ToList();
                        else
                            request = request.OrderBy(d => d.PeriorityNameAr).ToList();
                    }
                }
                if (sortObj.StatusName != "")
                {
                    if (sortObj.StrBarCode != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.Barcode).ToList();
                        }
                        else
                        {
                            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderBy(d => d.Barcode).ToList();
                        }
                    }
                    if (sortObj.StrSerial != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    //if (sortObj.StrModel != "")
                    //{
                    //    if (sortObj.SortStatus == "descending")
                    //        request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                    //    else
                    //        request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                    //}
                    if (sortObj.StrRequestCode != "")
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderByDescending(d => d.RequestCode).ToList();
                        else
                            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderBy(d => d.RequestCode).ToList();
                    }
                    if (sortObj.StrSubject != "")
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                        else
                            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.OrderByDescending(d => d.StatusName).ToList();
                        else
                            request = request.OrderBy(d => d.StatusName).ToList();
                    }
                }
                if (sortObj.StatusNameAr != "")
                {
                    if (sortObj.StrBarCode != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.Barcode).ToList();
                        }
                        else
                        {
                            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderBy(d => d.Barcode).ToList();
                        }
                    }
                    if (sortObj.StrSerial != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    //if (sortObj.StrModel != "")
                    //{
                    //    if (sortObj.SortStatus == "descending")
                    //        request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                    //    else
                    //        request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                    //}
                    if (sortObj.StrRequestCode != "")
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderByDescending(d => d.RequestCode).ToList();
                        else
                            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderBy(d => d.RequestCode).ToList();
                    }
                    if (sortObj.StrSubject != "")
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                        else
                            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.OrderByDescending(d => d.StatusNameAr).ToList();
                        else
                            request = request.OrderBy(d => d.StatusNameAr).ToList();
                    }
                }
                if (sortObj.ModeName != "")
                {
                    if (sortObj.StrBarCode != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.Barcode).ToList();
                        }
                        else
                        {
                            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderBy(d => d.Barcode).ToList();
                        }
                    }
                    if (sortObj.StrSerial != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    //if (sortObj.StrModel != "")
                    //{
                    //    if (sortObj.SortStatus == "descending")
                    //        request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                    //    else
                    //        request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                    //}
                    if (sortObj.StrRequestCode != "")
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderByDescending(d => d.RequestCode).ToList();
                        else
                            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderBy(d => d.RequestCode).ToList();
                    }
                    if (sortObj.StrSubject != "")
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                        else
                            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                    }

                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.OrderByDescending(d => d.ModeName).ToList();
                        else
                            request = request.OrderBy(d => d.ModeName).ToList();
                    }
                }
                if (sortObj.ModeNameAr != "")
                {
                    if (sortObj.StrBarCode != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.Barcode).ToList();
                        }
                        else
                        {
                            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderBy(d => d.Barcode).ToList();
                        }
                    }
                    if (sortObj.StrSerial != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    //if (sortObj.StrModel != "")
                    //{
                    //    if (sortObj.SortStatus == "descending")
                    //        request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                    //    else
                    //        request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                    //}
                    if (sortObj.StrRequestCode != "")
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderByDescending(d => d.RequestCode).ToList();
                        else
                            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderBy(d => d.RequestCode).ToList();
                    }
                    if (sortObj.StrSubject != "")
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                        else
                            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.OrderByDescending(d => d.ModeNameAr).ToList();
                        else
                            request = request.OrderBy(d => d.ModeNameAr).ToList();
                    }
                }
                if (sortObj.RequestDate != "")
                {
                    if (sortObj.StrBarCode != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.Barcode).ToList();
                        }
                        else
                        {
                            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderBy(d => d.Barcode).ToList();
                        }
                    }
                    if (sortObj.StrSerial != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    //if (sortObj.StrModel != "")
                    //{
                    //    if (sortObj.SortStatus == "descending")
                    //        request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                    //    else
                    //        request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                    //}
                    if (sortObj.StrRequestCode != "")
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderByDescending(d => d.RequestCode).ToList();
                        else
                            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderBy(d => d.RequestCode).ToList();
                    }
                    if (sortObj.StrSubject != "")
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                        else
                            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.OrderByDescending(d => d.RequestDate).ToList();
                        else
                            request = request.OrderBy(d => d.RequestDate).ToList();
                    }
                }
                if (sortObj.ClosedDate != "")
                {
                    if (sortObj.SortStatus == "descending")
                        request = request.OrderByDescending(d => d.ClosedDate).ToList();
                    else
                        request = request.OrderBy(d => d.ClosedDate).ToList();
                }
                if (sortObj.CreatedBy != "")
                {
                    if (sortObj.StrBarCode != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.Barcode).ToList();
                        }
                        else
                        {
                            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderBy(d => d.Barcode).ToList();
                        }
                    }
                    if (sortObj.StrSerial != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    //if (sortObj.StrModel != "")
                    //{
                    //    if (sortObj.SortStatus == "descending")
                    //        request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                    //    else
                    //        request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                    //}
                    if (sortObj.StrRequestCode != "")
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderByDescending(d => d.RequestCode).ToList();
                        else
                            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderBy(d => d.RequestCode).ToList();
                    }
                    if (sortObj.StrSubject != "")
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                        else
                            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.OrderByDescending(d => d.CreatedBy).ToList();
                        else
                            request = request.OrderBy(d => d.CreatedBy).ToList();
                    }
                }
                if (sortObj.Description != "")
                {
                    if (sortObj.StrBarCode != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.Barcode).ToList();
                        }
                        else
                        {
                            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderBy(d => d.Barcode).ToList();
                        }
                    }
                    if (sortObj.StrSerial != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    //if (sortObj.StrModel != "")
                    //{
                    //    if (sortObj.SortStatus == "descending")
                    //        request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                    //    else
                    //        request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                    //}
                    if (sortObj.StrRequestCode != "")
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderByDescending(d => d.RequestCode).ToList();
                        else
                            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderBy(d => d.RequestCode).ToList();
                    }
                    if (sortObj.StrSubject != "")
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                        else
                            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.OrderByDescending(d => d.Description).ToList();
                        else
                            request = request.OrderBy(d => d.Description).ToList();
                    }
                }
                if (sortObj.WOLastTrackDescription != "")
                {
                    if (sortObj.StrBarCode != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.Barcode).ToList();
                        }
                        else
                        {
                            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderBy(d => d.Barcode).ToList();
                        }
                    }
                    if (sortObj.StrSerial != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    //if (sortObj.StrModel != "")
                    //{
                    //    if (sortObj.SortStatus == "descending")
                    //        request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                    //    else
                    //        request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                    //}
                    if (sortObj.StrRequestCode != "")
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderByDescending(d => d.RequestCode).ToList();
                        else
                            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderBy(d => d.RequestCode).ToList();
                    }
                    if (sortObj.StrSubject != "")
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                        else
                            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                    }
                    if (sortObj.Description != "")
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.Where(b => b.Description.Contains(sortObj.Description)).OrderByDescending(d => d.Subject).ToList();

                        else
                            request = request.Where(b => b.Description.Contains(sortObj.Description)).OrderBy(d => d.Subject).ToList();
                    }

                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            request = request.OrderByDescending(d => d.WOLastTrackDescription).ToList();
                        else
                            request = request.OrderBy(d => d.WOLastTrackDescription).ToList();
                    }
                }
            }



            return request;
        }
        public async Task<List<IndexRequestsVM>> SortRequestsByPaging(SortRequestVM sortObj, int statusId, int pageNumber, int pageSize)
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
                var lstRequests = _context.RequestTracking
                          .Include(a => a.Request)
                           .Include(a => a.RequestStatus)
                         .Include(r => r.Request.RequestPeriority)
                         .Include(r => r.Request.AssetDetail).Include(r => r.Request.AssetDetail.MasterAsset)
                         .Include(r => r.Request.AssetDetail.Hospital)
                         .Include(r => r.Request.AssetDetail.Hospital.Governorate)
                         .Include(r => r.Request.AssetDetail.Hospital.City)
                         .Include(r => r.Request.AssetDetail.Hospital.Organization)
                         .Include(r => r.Request.AssetDetail.Hospital.SubOrganization)
                         .Include(r => r.Request.RequestType)
                         .Include(r => r.Request.SubProblem)
                         .Include(r => r.Request.RequestMode)
                         .Include(r => r.Request.User)
                         .OrderByDescending(p => p.Request.RequestDate).AsQueryable();



                //  var allrequests = requests.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();


                if (statusId == 0)
                    lstRequests = lstRequests.AsQueryable();
                else
                    lstRequests = lstRequests.Where(a => a.RequestStatusId == statusId).AsQueryable();



                if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                {
                    lstRequests = lstRequests.AsQueryable();
                }
                if (userObj.GovernorateId > 0 && userObj.CityId == 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                {
                    lstRequests = lstRequests.Where(a => a.Request.User.GovernorateId == userObj.GovernorateId).AsQueryable();
                }
                if (userObj.GovernorateId > 0 && userObj.CityId > 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                {
                    lstRequests = lstRequests.Where(a => a.Request.User.GovernorateId == userObj.GovernorateId && a.Request.User.CityId == userObj.CityId).AsQueryable();
                }
                if (userObj.GovernorateId > 0 && userObj.CityId > 0 && userObj.HospitalId > 0)
                {
                    if (userRoleNames.Contains("AssetOwner"))
                    {
                        lstRequests = lstRequests.Where(a => a.Request.User.HospitalId == userObj.HospitalId && a.CreatedById == userObj.Id).AsQueryable();
                    }
                    //if (userRoleNames.Contains("SRCreator"))
                    //{
                    //    lstRequests = lstRequests.Where(t => t.Request.User.HospitalId == userObj.HospitalId && t.CreatedById == userObj.Id).AsQueryable();
                    //}
                    //if (userRoleNames.Contains("SRReviewer"))
                    //{
                    //    lstRequests = lstRequests.Where(t => t.Request.User.HospitalId == userObj.HospitalId).AsQueryable();
                    //}
                    if (userRoleNames.Contains("SRCreator") && !userRoleNames.Contains("SRReviewer"))
                    {
                        lstRequests = lstRequests.Where(t => t.Request.User.HospitalId == userObj.HospitalId && t.CreatedById == userObj.Id).AsQueryable();
                    }
                    if (userRoleNames.Contains("SRReviewer") && !userRoleNames.Contains("SRCreator"))
                    {
                        lstRequests = lstRequests.Where(t => t.Request.User.HospitalId == userObj.HospitalId).AsQueryable();
                    }
                    if (userRoleNames.Contains("SRReviewer") && userRoleNames.Contains("SRCreator"))
                    {
                        lstRequests = lstRequests.Where(t => t.Request.User.HospitalId == userObj.HospitalId).AsQueryable();
                    }
                    else
                    {
                        lstRequests = lstRequests.Where(a => a.Request.User.HospitalId == userObj.HospitalId).AsQueryable();
                    }
                }
                if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId > 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                {
                    lstRequests = lstRequests.Where(a => a.Request.User.OrganizationId == userObj.OrganizationId).AsQueryable();
                }
                if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId > 0 && userObj.SubOrganizationId > 0 && userObj.HospitalId == 0)
                {
                    lstRequests = lstRequests.Where(a => a.Request.User.OrganizationId == userObj.OrganizationId && a.Request.User.SubOrganizationId == userObj.SubOrganizationId).AsQueryable();
                }
                if (userObj.OrganizationId > 0 && userObj.SubOrganizationId > 0 && userObj.HospitalId > 0)
                {
                    if (userRoleNames.Contains("AssetOwner"))
                    {
                        lstRequests = lstRequests.Where(a => a.Request.HospitalId == userObj.HospitalId && a.Request.CreatedById == userObj.Id).AsQueryable();
                    }
                    //if (userRoleNames.Contains("SRCreator"))
                    //{
                    //    lstRequests = lstRequests.Where(t => t.Request.User.HospitalId == userObj.HospitalId && t.CreatedById == userObj.Id).AsQueryable();
                    //}
                    //if (userRoleNames.Contains("SRReviewer"))
                    //{
                    //    lstRequests = lstRequests.Where(t => t.Request.User.HospitalId == userObj.HospitalId).AsQueryable();
                    //}
                    if (userRoleNames.Contains("SRCreator") && !userRoleNames.Contains("SRReviewer"))
                    {
                        lstRequests = lstRequests.Where(t => t.Request.User.HospitalId == userObj.HospitalId && t.CreatedById == userObj.Id).AsQueryable();
                    }
                    if (userRoleNames.Contains("SRReviewer") && !userRoleNames.Contains("SRCreator"))
                    {
                        lstRequests = lstRequests.Where(t => t.Request.User.HospitalId == userObj.HospitalId).AsQueryable();
                    }
                    if (userRoleNames.Contains("SRReviewer") && userRoleNames.Contains("SRCreator"))
                    {
                        lstRequests = lstRequests.Where(t => t.Request.User.HospitalId == userObj.HospitalId).AsQueryable();
                    }

                    else
                    {
                        lstRequests = lstRequests.Where(a => a.Request.HospitalId == userObj.HospitalId).AsQueryable();
                    }
                }




                if (sortObj.Serial != "")
                {
                    //if (sortObj.StrBarCode != "")
                    //{
                    //    if (sortObj.SortStatus == "descending")
                    //        lstRequests = lstRequests.Where(b => b.Request.AssetDetail.Barcode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.Request.AssetDetail.Barcode).AsQueryable();
                    //    else
                    //        allrequests = allrequests.Where(b => b.Request.AssetDetail.Barcode.Contains(sortObj.StrBarCode)).OrderBy(d => d.Request.AssetDetail.Barcode).ToList();
                    //}
                    //if (sortObj.StrSerial != "")
                    //{
                    //    if (sortObj.SortStatus == "descending")
                    //        allrequests = allrequests.Where(b => b.Request.AssetDetail.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.Request.AssetDetail.SerialNumber).ToList();
                    //    else
                    //        allrequests = allrequests.Where(b => b.Request.AssetDetail.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.Request.AssetDetail.SerialNumber).ToList();
                    //}
                    //if (sortObj.StrModel != "")
                    //{
                    //    if (sortObj.SortStatus == "descending")
                    //        allrequests = allrequests.Where(b => b.Request.AssetDetail.MasterAsset.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.Request.AssetDetail.MasterAsset.ModelNumber).ToList();
                    //    else
                    //        allrequests = allrequests.Where(b => b.Request.AssetDetail.MasterAsset.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.Request.AssetDetail.MasterAsset.ModelNumber).ToList();
                    //}
                    //if (sortObj.StrRequestCode != "")
                    //{
                    //    if (sortObj.SortStatus == "descending")
                    //        allrequests = allrequests.Where(b => b.Request.RequestCode.Contains(sortObj.StrRequestCode)).OrderByDescending(d => d.Request.RequestCode).ToList();
                    //    else
                    //        allrequests = allrequests.Where(b => b.Request.RequestCode.Contains(sortObj.StrRequestCode)).OrderBy(d => d.Request.RequestCode).ToList();
                    //}
                    //if (sortObj.StrSubject != "")
                    //{
                    //    if (sortObj.SortStatus == "descending")
                    //        allrequests = allrequests.Where(b => b.Request.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Request.Subject).ToList();
                    //    else
                    //        allrequests = allrequests.Where(b => b.Request.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Request.Subject).ToList();
                    //}
                    //else
                    //{
                    if (sortObj.SortStatus == "descending")
                        lstRequests = lstRequests.OrderByDescending(d => d.Request.AssetDetail.SerialNumber).AsQueryable();
                    else
                        lstRequests = lstRequests.OrderBy(d => d.Request.AssetDetail.SerialNumber).AsQueryable();
                    //}
                }
                //if (sortObj.RequestCode != "")
                //{
                //    if (sortObj.StrBarCode != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //        {
                //            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.Barcode).ToList();
                //        }
                //        else
                //        {
                //            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderBy(d => d.Barcode).ToList();
                //        }
                //    }
                //    if (sortObj.StrSerial != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //        {
                //            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                //        }
                //        else
                //        {
                //            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                //        }
                //    }
                //    if (sortObj.StrModel != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                //        else
                //            request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                //    }
                //    if (sortObj.StrRequestCode != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderByDescending(d => d.RequestCode).ToList();
                //        else
                //            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderBy(d => d.RequestCode).ToList();
                //    }
                //    if (sortObj.StrSubject != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                //        else
                //            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                //    }
                //    else
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.OrderByDescending(d => d.RequestCode).ToList();
                //        else
                //            request = request.OrderBy(d => d.RequestCode).ToList();
                //    }
                //}
                //if (sortObj.BarCode != "")
                //{
                //    if (sortObj.StrBarCode != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //        {
                //            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.Barcode).ToList();
                //        }
                //        else
                //        {
                //            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderBy(d => d.Barcode).ToList();
                //        }
                //    }
                //    if (sortObj.StrSerial != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //        {
                //            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                //        }
                //        else
                //        {
                //            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                //        }
                //    }
                //    if (sortObj.StrModel != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                //        else
                //            request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                //    }
                //    if (sortObj.StrRequestCode != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderByDescending(d => d.RequestCode).ToList();
                //        else
                //            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderBy(d => d.RequestCode).ToList();
                //    }
                //    if (sortObj.StrSubject != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                //        else
                //            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                //    }
                //    else
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.OrderByDescending(d => d.Barcode).ToList();
                //        else
                //            request = request.OrderBy(d => d.Barcode).ToList();
                //    }
                //}
                //if (sortObj.AssetName != "")
                //{
                //    if (sortObj.StrBarCode != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //        {
                //            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.Barcode).ToList();
                //        }
                //        else
                //        {
                //            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderBy(d => d.Barcode).ToList();
                //        }
                //    }
                //    if (sortObj.StrSerial != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //        {
                //            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                //        }
                //        else
                //        {
                //            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                //        }
                //    }
                //    if (sortObj.StrModel != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                //        else
                //            request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                //    }
                //    if (sortObj.StrRequestCode != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderByDescending(d => d.RequestCode).ToList();
                //        else
                //            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderBy(d => d.RequestCode).ToList();
                //    }
                //    if (sortObj.StrSubject != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                //        else
                //            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                //    }
                //    else
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.OrderByDescending(d => d.AssetName).ToList();
                //        else
                //            request = request.OrderBy(d => d.AssetName).ToList();
                //    }
                //}
                //if (sortObj.AssetNameAr != "")
                //{
                //    if (sortObj.StrBarCode != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //        {
                //            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.Barcode).ToList();
                //        }
                //        else
                //        {
                //            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderBy(d => d.Barcode).ToList();
                //        }
                //    }
                //    if (sortObj.StrSerial != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //        {
                //            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                //        }
                //        else
                //        {
                //            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                //        }
                //    }
                //    if (sortObj.StrModel != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                //        else
                //            request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                //    }
                //    if (sortObj.StrRequestCode != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderByDescending(d => d.RequestCode).ToList();
                //        else
                //            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderBy(d => d.RequestCode).ToList();
                //    }
                //    if (sortObj.StrSubject != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                //        else
                //            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                //    }
                //    else
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.OrderByDescending(d => d.AssetNameAr).ToList();
                //        else
                //            request = request.OrderBy(d => d.AssetNameAr).ToList();
                //    }
                //}
                //if (sortObj.Subject != "")
                //{
                //    if (sortObj.StrBarCode != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //        {
                //            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.Barcode).ToList();
                //        }
                //        else
                //        {
                //            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderBy(d => d.Barcode).ToList();
                //        }
                //    }
                //    if (sortObj.StrSerial != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //        {
                //            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                //        }
                //        else
                //        {
                //            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                //        }
                //    }
                //    if (sortObj.StrModel != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                //        else
                //            request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                //    }
                //    if (sortObj.StrRequestCode != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderByDescending(d => d.RequestCode).ToList();
                //        else
                //            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderBy(d => d.RequestCode).ToList();
                //    }
                //    if (sortObj.StrSubject != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                //        else
                //            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                //    }
                //    else
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.OrderByDescending(d => d.Subject).ToList();
                //        else
                //            request = request.OrderBy(d => d.Subject).ToList();
                //    }
                //}
                //if (sortObj.PeriorityName != "")
                //{
                //    if (sortObj.StrBarCode != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //        {
                //            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.Barcode).ToList();
                //        }
                //        else
                //        {
                //            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderBy(d => d.Barcode).ToList();
                //        }
                //    }
                //    if (sortObj.StrSerial != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //        {
                //            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                //        }
                //        else
                //        {
                //            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                //        }
                //    }
                //    if (sortObj.StrModel != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                //        else
                //            request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                //    }
                //    if (sortObj.StrRequestCode != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderByDescending(d => d.RequestCode).ToList();
                //        else
                //            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderBy(d => d.RequestCode).ToList();
                //    }
                //    if (sortObj.StrSubject != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                //        else
                //            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                //    }
                //    else
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.OrderByDescending(d => d.PeriorityName).ToList();
                //        else
                //            request = request.OrderBy(d => d.PeriorityName).ToList();
                //    }
                //}
                //if (sortObj.PeriorityNameAr != "")
                //{
                //    if (sortObj.StrBarCode != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //        {
                //            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.Barcode).ToList();
                //        }
                //        else
                //        {
                //            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderBy(d => d.Barcode).ToList();
                //        }
                //    }
                //    if (sortObj.StrSerial != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //        {
                //            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                //        }
                //        else
                //        {
                //            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                //        }
                //    }
                //    if (sortObj.StrModel != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                //        else
                //            request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                //    }
                //    if (sortObj.StrRequestCode != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderByDescending(d => d.RequestCode).ToList();
                //        else
                //            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderBy(d => d.RequestCode).ToList();
                //    }
                //    if (sortObj.StrSubject != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                //        else
                //            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                //    }
                //    else
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.OrderByDescending(d => d.PeriorityNameAr).ToList();
                //        else
                //            request = request.OrderBy(d => d.PeriorityNameAr).ToList();
                //    }
                //}
                //if (sortObj.StatusName != "")
                //{
                //    if (sortObj.StrBarCode != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //        {
                //            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.Barcode).ToList();
                //        }
                //        else
                //        {
                //            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderBy(d => d.Barcode).ToList();
                //        }
                //    }
                //    if (sortObj.StrSerial != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //        {
                //            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                //        }
                //        else
                //        {
                //            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                //        }
                //    }
                //    if (sortObj.StrModel != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                //        else
                //            request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                //    }
                //    if (sortObj.StrRequestCode != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderByDescending(d => d.RequestCode).ToList();
                //        else
                //            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderBy(d => d.RequestCode).ToList();
                //    }
                //    if (sortObj.StrSubject != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                //        else
                //            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                //    }
                //    else
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.OrderByDescending(d => d.StatusName).ToList();
                //        else
                //            request = request.OrderBy(d => d.StatusName).ToList();
                //    }
                //}
                //if (sortObj.StatusNameAr != "")
                //{
                //    if (sortObj.StrBarCode != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //        {
                //            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.Barcode).ToList();
                //        }
                //        else
                //        {
                //            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderBy(d => d.Barcode).ToList();
                //        }
                //    }
                //    if (sortObj.StrSerial != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //        {
                //            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                //        }
                //        else
                //        {
                //            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                //        }
                //    }
                //    if (sortObj.StrModel != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                //        else
                //            request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                //    }
                //    if (sortObj.StrRequestCode != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderByDescending(d => d.RequestCode).ToList();
                //        else
                //            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderBy(d => d.RequestCode).ToList();
                //    }
                //    if (sortObj.StrSubject != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                //        else
                //            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                //    }
                //    else
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.OrderByDescending(d => d.StatusNameAr).ToList();
                //        else
                //            request = request.OrderBy(d => d.StatusNameAr).ToList();
                //    }
                //}
                //if (sortObj.ModeName != "")
                //{
                //    if (sortObj.StrBarCode != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //        {
                //            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.Barcode).ToList();
                //        }
                //        else
                //        {
                //            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderBy(d => d.Barcode).ToList();
                //        }
                //    }
                //    if (sortObj.StrSerial != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //        {
                //            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                //        }
                //        else
                //        {
                //            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                //        }
                //    }
                //    if (sortObj.StrModel != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                //        else
                //            request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                //    }
                //    if (sortObj.StrRequestCode != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderByDescending(d => d.RequestCode).ToList();
                //        else
                //            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderBy(d => d.RequestCode).ToList();
                //    }
                //    if (sortObj.StrSubject != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                //        else
                //            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                //    }

                //    else
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.OrderByDescending(d => d.ModeName).ToList();
                //        else
                //            request = request.OrderBy(d => d.ModeName).ToList();
                //    }
                //}
                //if (sortObj.ModeNameAr != "")
                //{
                //    if (sortObj.StrBarCode != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //        {
                //            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.Barcode).ToList();
                //        }
                //        else
                //        {
                //            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderBy(d => d.Barcode).ToList();
                //        }
                //    }
                //    if (sortObj.StrSerial != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //        {
                //            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                //        }
                //        else
                //        {
                //            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                //        }
                //    }
                //    if (sortObj.StrModel != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                //        else
                //            request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                //    }
                //    if (sortObj.StrRequestCode != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderByDescending(d => d.RequestCode).ToList();
                //        else
                //            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderBy(d => d.RequestCode).ToList();
                //    }
                //    if (sortObj.StrSubject != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                //        else
                //            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                //    }
                //    else
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.OrderByDescending(d => d.ModeNameAr).ToList();
                //        else
                //            request = request.OrderBy(d => d.ModeNameAr).ToList();
                //    }
                //}
                //if (sortObj.RequestDate != "")
                //{
                //    if (sortObj.StrBarCode != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //        {
                //            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.Barcode).ToList();
                //        }
                //        else
                //        {
                //            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderBy(d => d.Barcode).ToList();
                //        }
                //    }
                //    if (sortObj.StrSerial != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //        {
                //            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                //        }
                //        else
                //        {
                //            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                //        }
                //    }
                //    if (sortObj.StrModel != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                //        else
                //            request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                //    }
                //    if (sortObj.StrRequestCode != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderByDescending(d => d.RequestCode).ToList();
                //        else
                //            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderBy(d => d.RequestCode).ToList();
                //    }
                //    if (sortObj.StrSubject != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                //        else
                //            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                //    }
                //    else
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.OrderByDescending(d => d.RequestDate).ToList();
                //        else
                //            request = request.OrderBy(d => d.RequestDate).ToList();
                //    }
                //}
                //if (sortObj.ClosedDate != "")
                //{
                //    if (sortObj.SortStatus == "descending")
                //        request = request.OrderByDescending(d => d.ClosedDate).ToList();
                //    else
                //        request = request.OrderBy(d => d.ClosedDate).ToList();
                //}
                //if (sortObj.CreatedBy != "")
                //{
                //    if (sortObj.StrBarCode != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //        {
                //            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.Barcode).ToList();
                //        }
                //        else
                //        {
                //            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderBy(d => d.Barcode).ToList();
                //        }
                //    }
                //    if (sortObj.StrSerial != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //        {
                //            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                //        }
                //        else
                //        {
                //            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                //        }
                //    }
                //    if (sortObj.StrModel != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                //        else
                //            request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                //    }
                //    if (sortObj.StrRequestCode != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderByDescending(d => d.RequestCode).ToList();
                //        else
                //            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderBy(d => d.RequestCode).ToList();
                //    }
                //    if (sortObj.StrSubject != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                //        else
                //            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                //    }
                //    else
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.OrderByDescending(d => d.CreatedBy).ToList();
                //        else
                //            request = request.OrderBy(d => d.CreatedBy).ToList();
                //    }
                //}
                //if (sortObj.Description != "")
                //{
                //    if (sortObj.StrBarCode != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //        {
                //            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.Barcode).ToList();
                //        }
                //        else
                //        {
                //            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderBy(d => d.Barcode).ToList();
                //        }
                //    }
                //    if (sortObj.StrSerial != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //        {
                //            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                //        }
                //        else
                //        {
                //            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                //        }
                //    }
                //    if (sortObj.StrModel != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                //        else
                //            request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                //    }
                //    if (sortObj.StrRequestCode != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderByDescending(d => d.RequestCode).ToList();
                //        else
                //            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderBy(d => d.RequestCode).ToList();
                //    }
                //    if (sortObj.StrSubject != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                //        else
                //            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                //    }
                //    else
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.OrderByDescending(d => d.Description).ToList();
                //        else
                //            request = request.OrderBy(d => d.Description).ToList();
                //    }
                //}
                //if (sortObj.WOLastTrackDescription != "")
                //{
                //    if (sortObj.StrBarCode != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //        {
                //            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.Barcode).ToList();
                //        }
                //        else
                //        {
                //            request = request.Where(b => b.Barcode.Contains(sortObj.StrBarCode)).OrderBy(d => d.Barcode).ToList();
                //        }
                //    }
                //    if (sortObj.StrSerial != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //        {
                //            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                //        }
                //        else
                //        {
                //            request = request.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                //        }
                //    }
                //    if (sortObj.StrModel != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                //        else
                //            request = request.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                //    }
                //    if (sortObj.StrRequestCode != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderByDescending(d => d.RequestCode).ToList();
                //        else
                //            request = request.Where(b => b.RequestCode.Contains(sortObj.StrRequestCode)).OrderBy(d => d.RequestCode).ToList();
                //    }
                //    if (sortObj.StrSubject != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                //        else
                //            request = request.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                //    }
                //    if (sortObj.Description != "")
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.Where(b => b.Description.Contains(sortObj.Description)).OrderByDescending(d => d.Subject).ToList();

                //        else
                //            request = request.Where(b => b.Description.Contains(sortObj.Description)).OrderBy(d => d.Subject).ToList();
                //    }

                //    else
                //    {
                //        if (sortObj.SortStatus == "descending")
                //            request = request.OrderByDescending(d => d.WOLastTrackDescription).ToList();
                //        else
                //            request = request.OrderBy(d => d.WOLastTrackDescription).ToList();
                //    }
                //}


                var allrequests = lstRequests.Select(a => a.Request).ToList();
                foreach (var req in allrequests)
                {
                    IndexRequestsVM getDataObj = new IndexRequestsVM();
                    getDataObj.Id = req.Id;
                    getDataObj.Code = req.RequestCode;
                    getDataObj.Subject = req.Subject;
                    getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                    getDataObj.Barcode = req.AssetDetail.Barcode;
                    getDataObj.RequestCode = req.RequestCode;
                    getDataObj.AssetHospitalId = int.Parse(req.HospitalId.ToString());
                    getDataObj.ModelNumber = req.AssetDetail.MasterAsset.ModelNumber;
                    getDataObj.Description = req.Description;
                    getDataObj.RequestDate = req.RequestDate;
                    getDataObj.RequestModeId = req.RequestModeId != null ? (int)req.RequestModeId : 0;
                    getDataObj.ModeName = req.RequestMode.Name;
                    getDataObj.ModeNameAr = req.RequestMode.NameAr;
                    getDataObj.SubProblemId = req.SubProblem != null ? (int)req.SubProblemId : 0;
                    getDataObj.SubProblemName = req.SubProblem != null ? req.SubProblem.Name : "";
                    getDataObj.RequestTypeId = req.RequestTypeId != null ? (int)req.RequestTypeId : 0;
                    getDataObj.RequestTypeName = req.RequestType != null ? req.RequestType.Name : "";
                    getDataObj.RequestPeriorityId = req.RequestPeriorityId != null ? (int)req.RequestPeriorityId : 0;
                    getDataObj.PeriorityName = req.RequestPeriority != null ? req.RequestPeriority.Name : "";
                    getDataObj.PeriorityNameAr = req.RequestPeriority != null ? req.RequestPeriority.NameAr : "";
                    getDataObj.PeriorityColor = req.RequestPeriority != null ? req.RequestPeriority.Color : "";
                    getDataObj.PeriorityIcon = req.RequestPeriority != null ? req.RequestPeriority.Icon : "";
                    getDataObj.CreatedById = req.CreatedById;
                    getDataObj.CreatedBy = req.User.UserName;
                    getDataObj.AssetDetailId = req.AssetDetailId != null ? (int)req.AssetDetailId : 0;
                    getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                    getDataObj.Barcode = req.AssetDetail.Barcode;
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
                                     .Where(a => a.RequestId == req.Id).ToList().OrderByDescending(a => a.DescriptionDate).ToList();
                    if (lstStatus.Count > 0)
                    {
                        getDataObj.StatusId = lstStatus[0].RequestStatus.Id;
                        getDataObj.StatusName = lstStatus[0].RequestStatus.Name;
                        getDataObj.StatusNameAr = lstStatus[0].RequestStatus.NameAr;
                        getDataObj.StatusColor = lstStatus[0].RequestStatus.Color;
                        getDataObj.StatusIcon = lstStatus[0].RequestStatus.Icon;
                        getDataObj.Description = lstStatus[0].Description;
                        if (getDataObj.StatusId == 2)
                        {
                            getDataObj.ClosedDate = lstStatus[0].DescriptionDate.ToString();
                        }
                        else
                        {
                            getDataObj.ClosedDate = "";
                        }
                    }
                    getDataObj.ListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id)
                                            .ToList().Select(item => new IndexRequestTrackingVM.GetData
                                            {
                                                Id = item.Id,
                                                StatusName = item.RequestStatus.Name,
                                                StatusNameAr = item.RequestStatus.NameAr,
                                                StatusColor = item.RequestStatus.Color,
                                                StatusIcon = item.RequestStatus.Icon,
                                                Description = item.Description,
                                                Date = item.DescriptionDate,
                                                StatusId = item.RequestStatus.Id,
                                                isExpanded = (_context.RequestDocument.Where(a => a.RequestTrackingId == item.Id).Count()) > 0 ? true : false,
                                                ListDocuments = _context.RequestDocument.Where(a => a.RequestTrackingId == item.Id).ToList(),
                                            }).ToList();



                    getDataObj.CountListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id).ToList().Count;
                    getDataObj.CountWorkOrder = _context.WorkOrders.Where(a => a.RequestId == req.Id).ToList().Count;

                    var lstWOStatus = _context.WorkOrderTrackings.Include(o => o.WorkOrder).Include(o => o.WorkOrderStatus).Where(a => a.WorkOrder.RequestId == req.Id).OrderByDescending(a => a.CreationDate).ToList();

                    if (lstWOStatus.Count > 0)
                    {
                        getDataObj.LatestWorkOrderStatusId = lstWOStatus[0].WorkOrderStatusId;
                        getDataObj.WOLastTrackDescription = lstWOStatus[0].Notes;
                    }
                    request.Add(getDataObj);
                }
            }
            return request.ToList();
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
                getDataObj.Barcode = req.AssetDetail.Barcode;
                getDataObj.Description = req.Description;
                getDataObj.AssetHospitalId = int.Parse(req.HospitalId.ToString());
                getDataObj.RequestDate = req.RequestDate;
                getDataObj.RequestModeId = req.RequestModeId != null ? (int)req.RequestModeId : 0;
                getDataObj.ModeName = req.RequestMode.Name;
                getDataObj.ModeNameAr = req.RequestMode.NameAr;
                getDataObj.SubProblemId = req.SubProblemId != null ? (int)req.SubProblemId : 0;
                getDataObj.SubProblemName = req.SubProblem.Name;
                getDataObj.RequestTypeId = req.RequestTypeId != null ? (int)req.RequestTypeId : 0;
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


            if (sortObj.RequestCode != "")
            {
                if (sortObj.SortStatus == "descending")
                    request = request.OrderByDescending(d => d.RequestCode).ToList();
                else
                    request = request.OrderBy(d => d.RequestCode).ToList();
            }

            if (sortObj.BarCode != "")
            {
                if (sortObj.SortStatus == "descending")
                    request = request.OrderByDescending(d => d.Barcode).ToList();
                else
                    request = request.OrderBy(d => d.Barcode).ToList();
            }

            if (sortObj.Serial != "")
            {
                if (sortObj.SortStatus == "descending")
                    request = request.OrderByDescending(d => d.SerialNumber).ToList();
                else
                    request = request.OrderBy(d => d.SerialNumber).ToList();
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
            int totalRequests = 0;
            ApplicationUser UserObj = new ApplicationUser();
            List<Request> listCountRequests = new List<Request>();
            // string userRoleName = "";
            var obj = _context.ApplicationUser.Where(a => a.Id == userId).ToList();
            List<string> userRoleNames = new List<string>();
            if (obj.Count > 0)
            {
                UserObj = obj[0];
                //var roleNames = (from userRole in _context.UserRoles
                //                 join role in _context.Roles on userRole.RoleId equals role.Id
                //                 where userRole.UserId == userId
                //                 select role.Name);


                //foreach (var role in roles)
                //{
                //    userRoleNames.Add(role.Name);
                //}


                var roles = (from userRole in _context.UserRoles
                             join role in _context.ApplicationRole on userRole.RoleId equals role.Id
                             where userRole.UserId == userId
                             select role);
                foreach (var role in roles)
                {
                    userRoleNames.Add(role.Name);
                }
            }

            var list = _context.Request
                              .Include(t => t.AssetDetail)
                              .Include(t => t.AssetDetail.Hospital)
                              .Include(t => t.AssetDetail.Hospital.Governorate)
                              .Include(t => t.AssetDetail.Hospital.City)
                              .Include(t => t.AssetDetail.Hospital.Organization)
                              .Include(t => t.AssetDetail.Hospital.SubOrganization)
                              .Include(t => t.User)
                              .Where(a => a.IsOpened == false).ToList();

            if (list.Count > 0)
            {
                if (UserObj.GovernorateId == 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
                {
                    if (userRoleNames.Contains("Supplier"))
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
                    list = list.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId && t.IsOpened == false).ToList();
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
                    if (userRoleNames.Contains("Admin"))
                    {
                        list = list.ToList();
                    }
                    if (userRoleNames.Contains("TLHospitalManager"))
                    {
                        list = list.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId && t.IsOpened == false).ToList();
                    }
                    if (userRoleNames.Contains("EngDepManager"))
                    {
                        list = list.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId && t.IsOpened == false).ToList();
                    }
                    if (userRoleNames.Contains("EngManager"))
                    {
                        list = list.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId && t.IsOpened == false).ToList();
                    }
                    if (userRoleNames.Contains("AssetOwner"))
                    {
                        list = list.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId && t.CreatedById == userId && t.IsOpened == false).ToList();
                    }
                    //if (userRoleNames.Contains("SRCreator"))
                    //{
                    //    list = list.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId && t.CreatedById == userId && t.IsOpened == false).ToList();
                    //}
                    //if (userRoleNames.Contains("SRReviewer"))
                    //{
                    //    list = list.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId && t.IsOpened == false).ToList();
                    //}

                    if (userRoleNames.Contains("SRCreator") && !userRoleNames.Contains("SRReviewer"))
                    {
                        list = list.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId && t.CreatedById == userId && t.IsOpened == false).ToList();
                    }
                    if (userRoleNames.Contains("SRReviewer") && !userRoleNames.Contains("SRCreator"))
                    {
                        list = list.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId && t.IsOpened == false).ToList();
                    }
                    if (userRoleNames.Contains("SRReviewer") && userRoleNames.Contains("SRCreator"))
                    {
                        list = list.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId && t.IsOpened == false).ToList();
                    }
                    if (userRoleNames.Contains("Eng"))
                    {
                        //var lstAssigned = (from order in _context.WorkOrders
                        //                   join track in _context.WorkOrderTrackings on order.Id equals track.WorkOrderId
                        //                   join usr in _context.ApplicationUser on track.AssignedTo equals usr.Id
                        //                   join req in _context.Request on order.RequestId equals req.Id
                        //                   where usr.HospitalId == UserObj.HospitalId
                        //                   && track.AssignedTo == userId
                        //                   select order).ToList();
                    }
                }
            }



            foreach (var item in list)
            {
                var listWO = _context.WorkOrders.Where(a => a.RequestId == item.Id);
                if (listWO.ToList().Count == 0)
                {
                    listCountRequests.Add(item);
                }
            }

            var lstTracks = _context.RequestTracking
                        .Include(a => a.Request).Include(a => a.Request.AssetDetail)
                        .Include(a => a.Request.AssetDetail.Hospital)
                        .Include(a => a.Request.AssetDetail.MasterAsset)
                        .Where(a => a.IsOpened == false && a.Request.AssetDetail.HospitalId == UserObj.HospitalId && a.RequestStatusId == 2).ToList()
                        .OrderByDescending(a => a.Id).ToList().GroupBy(a => a.RequestId).ToList();

            totalRequests = (lstTracks.Count) + (listCountRequests.Count);

            return totalRequests;
        }
        public PrintServiceRequestVM PrintServiceRequestById(int id)
        {
            List<LstWorkOrderFromTracking> lstTracking = new List<LstWorkOrderFromTracking>();

            List<IndexRequestTracking> lstServiceTracking = new List<IndexRequestTracking>();
            PrintServiceRequestVM printSRObj = new PrintServiceRequestVM();

            var lstRequests = _context.Request.Where(a => a.Id == id).Include(a => a.AssetDetail)
                .Include(a => a.AssetDetail.Department)
                .Include(a => a.AssetDetail.MasterAsset).Include(a => a.AssetDetail.MasterAsset.brand)
                 .Include(a => a.RequestType).Include(a => a.SubProblem).Include(a => a.SubProblem.Problem)
                 .Include(a => a.RequestMode)
                 .Include(a => a.AssetDetail.Hospital)
                 .Include(a => a.AssetDetail.Room)
                 .Include(a => a.AssetDetail.Floor)
                 .Include(a => a.AssetDetail.Building)
                .Include(a => a.AssetDetail.MasterAsset)
                .Include(w => w.RequestPeriority).Include(w => w.User).ToList();

            if (lstRequests.Count > 0)
            {

                var requestObj = lstRequests[0];
                printSRObj.RequestId = requestObj.Id;
                printSRObj.AssetCode = requestObj.AssetDetail.Code;
                printSRObj.AssetBarCode = requestObj.AssetDetail.Barcode;
                printSRObj.AssetHospitalId = requestObj.HospitalId;
                printSRObj.RequestSubject = requestObj.Subject;
                printSRObj.RequestDate = requestObj.RequestDate.ToShortDateString();
                printSRObj.RequestCode = requestObj.RequestCode;
                printSRObj.RequestTypeName = requestObj.RequestType.Name;
                printSRObj.RequestTypeNameAr = requestObj.RequestType.NameAr;
                printSRObj.ModeName = requestObj.RequestMode.Name;
                printSRObj.ModeNameAr = requestObj.RequestMode.NameAr;
                printSRObj.SubProblemName = requestObj.SubProblem != null ? requestObj.SubProblem.Name : "";
                printSRObj.SubProblemNameAr = requestObj.SubProblem != null ? requestObj.SubProblem.NameAr : "";
                printSRObj.ProblemName = requestObj.SubProblem != null ? requestObj.SubProblem.Problem.Name : "";
                printSRObj.ProblemNameAr = requestObj.SubProblem != null ? requestObj.SubProblem.Problem.NameAr : "";

                printSRObj.HospitalId = requestObj.AssetDetail.HospitalId;
                printSRObj.HospitalName = requestObj.AssetDetail.Hospital != null ? requestObj.AssetDetail.Hospital.Name : "";
                printSRObj.HospitalNameAr = requestObj.AssetDetail.Hospital != null ? requestObj.AssetDetail.Hospital.NameAr : "";
                printSRObj.AssetName = requestObj.AssetDetail.MasterAsset != null ? requestObj.AssetDetail.MasterAsset.Name : "";
                printSRObj.AssetNameAr = requestObj.AssetDetail.MasterAsset != null ? requestObj.AssetDetail.MasterAsset.NameAr : "";
                printSRObj.SerialNumber = requestObj.AssetDetail.SerialNumber;


                printSRObj.BuildingName = requestObj.AssetDetail.Building != null ? requestObj.AssetDetail.Building.Name : "";
                printSRObj.BuildingNameAr = requestObj.AssetDetail.Building != null ? requestObj.AssetDetail.Building.NameAr : "";
                printSRObj.FloorName = requestObj.AssetDetail.Floor != null ? requestObj.AssetDetail.Floor.Name : "";
                printSRObj.FloorNameAr = requestObj.AssetDetail.Floor != null ? requestObj.AssetDetail.Floor.NameAr : "";
                printSRObj.RoomName = requestObj.AssetDetail.Room != null ? requestObj.AssetDetail.Room.Name : "";
                printSRObj.RoomNameAr = requestObj.AssetDetail.Room != null ? requestObj.AssetDetail.Room.NameAr : "";
                printSRObj.ModelNumber = requestObj.AssetDetail.MasterAsset != null ? requestObj.AssetDetail.MasterAsset.ModelNumber : "";


                printSRObj.BrandName = requestObj.AssetDetail.MasterAsset.brand != null ? requestObj.AssetDetail.MasterAsset.brand.Name : "";
                printSRObj.BrandNameAr = requestObj.AssetDetail.MasterAsset.brand != null ? requestObj.AssetDetail.MasterAsset.brand.NameAr : "";


                printSRObj.DepartmentName = requestObj.AssetDetail.Department != null ? requestObj.AssetDetail.Department.Name : "";
                printSRObj.DepartmentNameAr = requestObj.AssetDetail.Department != null ? requestObj.AssetDetail.Department.NameAr : "";




                var lstRequestNote = _context.RequestTracking.Where(a => a.RequestId == requestObj.Id && a.RequestStatusId == 1).Include(a => a.Request.AssetDetail).ToList();
                if (lstRequestNote.Count > 0)
                {
                    var note = lstRequestNote[0].Description;
                    printSRObj.RequestNote = note;
                    printSRObj.DescriptionDate = lstRequestNote[0].DescriptionDate.ToString();
                }


                var lstRequestLastDate = _context.RequestTracking.Include(a => a.Request.AssetDetail).Where(a => a.RequestId == requestObj.Id).OrderByDescending(a => a.DescriptionDate).ToList();
                if (lstRequestLastDate.Count > 0)
                {
                    printSRObj.LastRequestDate = lstRequestLastDate[0].DescriptionDate.ToString();
                }

                var lstMainWorkOrders = _context.WorkOrders.Where(a => a.RequestId == requestObj.Id).Include(a => a.Request.AssetDetail)
            .Include(a => a.Request.RequestType).Include(a => a.Request.SubProblem).Include(a => a.Request.SubProblem.Problem)
            .Include(a => a.Request.RequestMode)
            .Include(a => a.Request.AssetDetail.Hospital)
            .Include(a => a.Request.AssetDetail.MasterAsset)
            .Include(w => w.WorkOrderType).Include(w => w.WorkOrderPeriority).Include(w => w.User).ToList();
                if (lstMainWorkOrders.Count > 0)
                {
                    var woObj = lstMainWorkOrders[0];


                    printSRObj.Subject = woObj.Subject != "" ? woObj.Subject : "";
                    printSRObj.WorkOrderNumber = woObj.WorkOrderNumber != "" ? woObj.WorkOrderNumber : "";
                    printSRObj.CreationDate = woObj.CreationDate;
                    printSRObj.PlannedStartDate = woObj.PlannedStartDate;
                    printSRObj.PlannedEndDate = woObj.PlannedEndDate;
                    printSRObj.ActualStartDate = woObj.ActualStartDate;
                    printSRObj.ActualEndDate = woObj.ActualEndDate;
                    printSRObj.Note = woObj.Note;
                    printSRObj.CreatedById = woObj.CreatedById;
                    printSRObj.CreatedBy = woObj.User.UserName;
                    printSRObj.PeriorityName = woObj.WorkOrderPeriority != null ? woObj.WorkOrderPeriority.Name : "";
                    printSRObj.PeriorityNameAr = woObj.WorkOrderPeriority != null ? woObj.WorkOrderPeriority.NameAr : "";
                    printSRObj.TypeName = woObj.WorkOrderType != null ? woObj.WorkOrderType.Name : "";
                    printSRObj.TypeNameAr = woObj.WorkOrderType != null ? woObj.WorkOrderType.NameAr : "";




                    var lstTracks = _context.WorkOrderTrackings
                                                          .Include(A => A.WorkOrderStatus)
                                                          .Include(A => A.User)
                                                         .Where(t => t.WorkOrderId == woObj.Id).ToList();

                    if (lstTracks.Count > 0)
                    {
                        printSRObj.WorkOrderSubject = lstTracks[0].WorkOrder.Subject;

                        foreach (var item in lstTracks)
                        {
                            LstWorkOrderFromTracking trackObj = new LstWorkOrderFromTracking();
                            if (item.ActualStartDate != null)
                                trackObj.ActualStartDate = DateTime.Parse(item.ActualStartDate.ToString());
                            if (item.ActualEndDate != null)
                                trackObj.ActualEndDate = DateTime.Parse(item.ActualEndDate.ToString());
                            trackObj.Notes = item.Notes;
                            trackObj.CreatedBy = _context.ApplicationUser.Where(a => a.Id == item.CreatedById).ToList().FirstOrDefault().UserName;
                            trackObj.StatusName = item.WorkOrderStatus.Name;
                            trackObj.StatusNameAr = item.WorkOrderStatus.NameAr;
                            if (item.AssignedTo != "")
                                trackObj.AssignedToName = _context.ApplicationUser.Where(a => a.Id == item.AssignedTo).ToList().FirstOrDefault().UserName;
                            lstTracking.Add(trackObj);

                        }
                        printSRObj.LstWorkOrderTracking = lstTracking;
                    }
                }

                var lstServiceTracks = _context.RequestTracking
                                                 .Include(A => A.RequestStatus)
                                                 .Include(A => A.User)
                                                .Where(t => t.RequestId == id).ToList();

                if (lstServiceTracks.Count > 0)
                {
                    foreach (var item in lstServiceTracks)
                    {
                        IndexRequestTracking trackObj = new IndexRequestTracking();
                        trackObj.Description = item.Description;
                        trackObj.DescriptionDate = item.DescriptionDate;
                        trackObj.CreatedById = item.User.UserName;
                        trackObj.StatusName = item.RequestStatus.Name;
                        trackObj.StatusNameAr = item.RequestStatus.NameAr;
                        lstServiceTracking.Add(trackObj);

                    }
                    printSRObj.RequestTrackingList = lstServiceTracking;
                }
            }
            return printSRObj;
        }
        public IEnumerable<IndexRequestVM.GetData> GetRequestsByDate(SearchRequestDateVM requestDateObj)
        {
            List<IndexRequestVM.GetData> list = new List<IndexRequestVM.GetData>();
            ApplicationUser UserObj = new ApplicationUser();
            ApplicationRole roleObj = new ApplicationRole();

            List<string> userRoleNames = new List<string>();
            var obj = _context.ApplicationUser.Where(a => a.Id == requestDateObj.UserId).ToList();
            if (obj.Count > 0)
            {
                UserObj = obj[0];
                var roles = (from userRole in _context.UserRoles
                             join role in _context.ApplicationRole on userRole.RoleId equals role.Id
                             where userRole.UserId == requestDateObj.UserId
                             select role);
                foreach (var role in roles)
                {
                    userRoleNames.Add(role.Name);
                }
            }
            if (requestDateObj.StrStartDate != "")
                requestDateObj.StartDate = DateTime.Parse(requestDateObj.StrStartDate);
            else
                requestDateObj.StartDate = DateTime.Today.Date;


            if (requestDateObj.StrEndDate != "")
                requestDateObj.EndDate = DateTime.Parse(requestDateObj.StrEndDate);
            else
            {
                requestDateObj.EndDate = DateTime.Today.Date;
            }



            var lstRequests = _context.RequestTracking
                 .Include(t => t.Request)
                             .Include(t => t.Request.AssetDetail)
                             .Include(t => t.Request.AssetDetail.MasterAsset)
                             .Include(t => t.User)
                             .Include(t => t.Request.RequestMode).Include(t => t.RequestStatus)
                             .Include(t => t.Request.RequestPeriority).OrderByDescending(a => a.DescriptionDate)
                             .ToList().GroupBy(a => a.Request.Id).ToList();



            foreach (var req in lstRequests)
            {
                IndexRequestVM.GetData getDataObj = new IndexRequestVM.GetData();
                getDataObj.RequestId = req.FirstOrDefault().Request.Id;
                getDataObj.StatusId = (int)req.FirstOrDefault().RequestStatusId;
                getDataObj.Id = req.FirstOrDefault().Request.Id;
                getDataObj.RequestCode = req.FirstOrDefault().Request.RequestCode;
                getDataObj.Barcode = req.FirstOrDefault().Request.AssetDetail.Barcode;
                getDataObj.CreatedById = req.FirstOrDefault().CreatedById;
                getDataObj.UserName = req.FirstOrDefault().User.UserName;
                getDataObj.Subject = req.FirstOrDefault().Request.Subject;
                getDataObj.AssetHospitalId = req.FirstOrDefault().Request.HospitalId;
                getDataObj.RequestDate = req.FirstOrDefault().Request.RequestDate;
                getDataObj.AssetDetailId = req.FirstOrDefault().Request.AssetDetailId != null ? (int)req.FirstOrDefault().Request.AssetDetailId : 0;
                getDataObj.HospitalId = req.FirstOrDefault().Request.AssetDetail.HospitalId;
                getDataObj.DescriptionDate = req.FirstOrDefault().DescriptionDate;
                getDataObj.Description = req.FirstOrDefault().Description;


                getDataObj.StatusId = int.Parse(req.FirstOrDefault().RequestStatusId.ToString());
                getDataObj.StatusName = req.FirstOrDefault().RequestStatus.Name;
                getDataObj.StatusNameAr = req.FirstOrDefault().RequestStatus.NameAr;
                getDataObj.StatusColor = req.FirstOrDefault().RequestStatus.Color;
                getDataObj.StatusIcon = req.FirstOrDefault().RequestStatus.Icon;

                if (getDataObj.StatusId == 2)
                    getDataObj.ClosedDate = req.FirstOrDefault().DescriptionDate.ToString();
                else
                    getDataObj.ClosedDate = "";



                getDataObj.SerialNumber = req.FirstOrDefault().Request.AssetDetail.SerialNumber;
                getDataObj.ModeId = req.FirstOrDefault().Request.RequestMode.Id;
                getDataObj.ModeName = req.FirstOrDefault().Request.RequestMode.Name;
                getDataObj.ModeNameAr = req.FirstOrDefault().Request.RequestMode.NameAr;


                getDataObj.PeriorityId = req.FirstOrDefault().Request.RequestPeriority.Id;
                getDataObj.PeriorityName = req.FirstOrDefault().Request.RequestPeriority.Name.Trim();
                getDataObj.PeriorityNameAr = req.FirstOrDefault().Request.RequestPeriority.NameAr.Trim();
                getDataObj.PeriorityColor = req.FirstOrDefault().Request.RequestPeriority != null ? req.FirstOrDefault().Request.RequestPeriority.Color.Trim() : "";
                getDataObj.PeriorityIcon = req.FirstOrDefault().Request.RequestPeriority != null ? req.FirstOrDefault().Request.RequestPeriority.Icon : "";


                getDataObj.AssetName = req.FirstOrDefault().Request.AssetDetail.MasterAsset.Name.Trim();
                getDataObj.AssetNameAr = req.FirstOrDefault().Request.AssetDetail.MasterAsset.NameAr;
                //getDataObj.ListTracks = _context.RequestTracking.Where(a => a.RequestId == req.FirstOrDefault().RequestId)
                //        .ToList().Select(item => new IndexRequestTrackingVM.GetData
                //        {
                //            Id = item.Id,
                //            StatusName = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().Name,
                //            StatusNameAr = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().NameAr,
                //            StatusColor = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().Color,
                //            StatusIcon = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().Icon,
                //            Description = item.Description,
                //            Date = item.DescriptionDate,
                //            StatusId = item.RequestStatus.Id,
                //            isExpanded = (_context.RequestDocument.Where(a => a.RequestTrackingId == item.Id).Count()) > 0 ? true : false,
                //            ListDocuments = _context.RequestDocument.Where(a => a.RequestTrackingId == item.Id).ToList(),
                //        }).ToList();

                var lstWOStatus = _context.WorkOrderTrackings
                        .Include(o => o.WorkOrder).Include(o => o.WorkOrderStatus).Where(a => a.WorkOrder.RequestId == req.FirstOrDefault().Id)
                        .OrderByDescending(a => a.CreationDate).ToList();

                if (lstWOStatus.Count > 0)
                {
                    getDataObj.LatestWorkOrderStatusId = lstWOStatus[0].WorkOrderStatusId;

                    getDataObj.WorkOrderStatusName = lstWOStatus[0].WorkOrderStatus.Name;
                    getDataObj.WorkOrderStatusNameAr = lstWOStatus[0].WorkOrderStatus.NameAr;
                    getDataObj.WorkOrderStatusColor = lstWOStatus[0].WorkOrderStatus.Color;
                    getDataObj.WorkOrderStatusIcon = lstWOStatus[0].WorkOrderStatus.Icon;
                }
                getDataObj.CountListTracks = _context.RequestTracking.Where(a => a.RequestId == req.FirstOrDefault().Id).ToList().Count;
                getDataObj.CountWorkOrder = _context.WorkOrders.Where(a => a.RequestId == req.FirstOrDefault().Id).ToList().Count;
                getDataObj.GovernorateId = req.FirstOrDefault().User.GovernorateId;
                getDataObj.CityId = req.FirstOrDefault().User.CityId;
                getDataObj.OrganizationId = req.FirstOrDefault().User.OrganizationId;
                getDataObj.SubOrganizationId = req.FirstOrDefault().User.SubOrganizationId;


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
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();

                    //  list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == requestDateObj.UserId).ToList();
                }
                if (userRoleNames.Contains("AssetOwner"))
                {
                    //  list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == requestDateObj.UserId).ToList();
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == UserObj.Id).ToList();
                }
                //if (userRoleNames.Contains("SRCreator"))
                //{
                //    list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == UserObj.Id).ToList();
                //}
                //if (userRoleNames.Contains("SRReviewer"))
                //{
                //    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                //}
                if (userRoleNames.Contains("SRCreator") && !userRoleNames.Contains("SRReviewer"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == UserObj.Id).ToList();
                }
                if (userRoleNames.Contains("SRReviewer") && !userRoleNames.Contains("SRCreator"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                if (userRoleNames.Contains("SRReviewer") && userRoleNames.Contains("SRCreator"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
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
                    // list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == requestDateObj.UserId).ToList();

                    list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == UserObj.Id).ToList();
                }
                if (userRoleNames.Contains("Eng"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                //if (userRoleNames.Contains("SRCreator"))
                //{
                //    list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == UserObj.Id).ToList();
                //}
                //if (userRoleNames.Contains("SRReviewer"))
                //{
                //    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                //  }

                if (userRoleNames.Contains("SRCreator") && !userRoleNames.Contains("SRReviewer"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == UserObj.Id).ToList();
                }
                if (userRoleNames.Contains("SRReviewer") && !userRoleNames.Contains("SRCreator"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                if (userRoleNames.Contains("SRReviewer") && userRoleNames.Contains("SRCreator"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }

            }



            if (requestDateObj.StatusId > 0)
            {
                list = list.Where(a => a.StatusId == requestDateObj.StatusId).ToList();
            }
            else
            {
                list = list.ToList();
            }

            DateTime? start = new DateTime();
            DateTime? end = new DateTime();
            if (requestDateObj.StrStartDate != "")
                start = DateTime.Parse(requestDateObj.StrStartDate);
            if (requestDateObj.StrEndDate != "")
                end = DateTime.Parse(requestDateObj.StrEndDate);
            if (start != null || end != null)
            {
                list = list.Where(a => a.RequestDate.Date >= start.Value.Date && a.RequestDate.Date <= end.Value.Date).ToList();
            }



            return list;
        }


        public IndexRequestsVM GetByRequestCode(string code)
        {
            var request = _context.Request.Where(a => a.RequestCode == code)
                                        .Select(req => new IndexRequestsVM
                                        {
                                            Id = req.Id,
                                            Subject = req.Subject,
                                            RequestCode = req.RequestCode,
                                            AssetCode = req.AssetDetail.Code
                                        }).FirstOrDefault();

            return request;
        }
        public IEnumerable<IndexRequestVM.GetData> GetAllRequestsByHospitalAssetId(int assetId)
        {
            List<IndexRequestVM.GetData> list = new List<IndexRequestVM.GetData>();
            var lstRequests = _context.Request
                      .Include(t => t.AssetDetail)
                      .Include(t => t.AssetDetail.MasterAsset)
                      .Include(t => t.User)
                      .Include(t => t.RequestMode)
                      .Include(t => t.RequestPeriority)
                      .OrderByDescending(a => a.RequestDate)
                      .Where(a => a.AssetDetailId == assetId).ToList();
            foreach (var req in lstRequests)
            {
                IndexRequestVM.GetData getDataObj = new IndexRequestVM.GetData();
                getDataObj.RequestId = req.Id;
                getDataObj.Id = req.Id;
                getDataObj.RequestCode = req.RequestCode;
                getDataObj.Barcode = req.AssetDetail.Barcode;
                getDataObj.AssetHospitalId = req.HospitalId;
                getDataObj.CreatedById = req.CreatedById;
                getDataObj.UserName = req.User.UserName;
                getDataObj.Subject = req.Subject;
                getDataObj.RequestDate = req.RequestDate;
                getDataObj.AssetDetailId = req.AssetDetailId != null ? (int)req.AssetDetailId : 0;
                getDataObj.HospitalId = req.AssetDetail.HospitalId;
                getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                getDataObj.ModeId = req.RequestModeId != null ? (int)req.RequestModeId : 0;
                getDataObj.ModeName = req.RequestMode.Name;
                getDataObj.ModeNameAr = req.RequestMode.NameAr;
                getDataObj.PeriorityId = req.RequestPeriorityId != null ? (int)req.RequestPeriorityId : 0;
                getDataObj.PeriorityName = req.RequestPeriority.Name;
                getDataObj.PeriorityNameAr = req.RequestPeriority.NameAr;
                getDataObj.PeriorityColor = req.RequestPeriority.Color;
                getDataObj.PeriorityIcon = req.RequestPeriority.Icon;

                var lstSRStatus = _context.RequestTracking
                                .Include(t => t.Request).Include(t => t.RequestStatus)
                                .Where(a => a.RequestId == req.Id).ToList().OrderByDescending(a => a.DescriptionDate).ToList();
                if (lstSRStatus.Count > 0)
                {
                    getDataObj.StatusId = lstSRStatus[0].RequestStatus.Id;
                    getDataObj.StatusName = lstSRStatus[0].RequestStatus.Name;
                    getDataObj.StatusNameAr = lstSRStatus[0].RequestStatus.NameAr;
                    getDataObj.StatusColor = lstSRStatus[0].RequestStatus.Color;
                    getDataObj.StatusIcon = lstSRStatus[0].RequestStatus.Icon;
                }

                var woObj = _context.WorkOrders
                .Include(wo => wo.User)
                 .Include(wo => wo.WorkOrderPeriority)
                .Include(wo => wo.Request.AssetDetail)
                .Where(a => a.Request.Id == req.Id && a.Request.AssetDetailId == assetId).OrderByDescending(a => a.CreationDate).ToList();
                if (woObj.Count > 0)
                {
                    var workOrderObj = woObj[0];
                    getDataObj.CreationDate = workOrderObj.CreationDate;
                    getDataObj.WorkOrderSubject = workOrderObj.Subject;
                    getDataObj.WorkOrderNumber = workOrderObj.WorkOrderNumber;
                    getDataObj.CreatedById = workOrderObj.CreatedById;
                    getDataObj.CreatedBy = workOrderObj.User.UserName;
                    getDataObj.UserName = workOrderObj.User.UserName;
                    getDataObj.PeriorityName = workOrderObj.WorkOrderPeriority.Name;
                    getDataObj.PeriorityNameAr = workOrderObj.WorkOrderPeriority.NameAr;
                    var lstStatus = _context.WorkOrderTrackings.Include(t => t.WorkOrder).Include(t => t.WorkOrderStatus).Where(a => a.WorkOrderId == workOrderObj.Id).ToList().OrderByDescending(a => a.WorkOrderDate).ToList();
                    if (lstStatus.Count > 0)
                    {
                        getDataObj.WorkOrderStatusName = lstStatus[0].WorkOrderStatus.Name;
                        getDataObj.WorkOrderStatusNameAr = lstStatus[0].WorkOrderStatus.NameAr;
                        getDataObj.WorkOrderStatusColor = lstStatus[0].WorkOrderStatus.Color;
                        getDataObj.WorkOrderStatusIcon = lstStatus[0].WorkOrderStatus.Icon;

                    }
                }







                list.Add(getDataObj);
            }
            return list;
        }
        public int CountRequestsByHospitalId(int hospitalId, string userId)
        {


            ApplicationUser UserObj = new ApplicationUser();
            ApplicationRole roleObj = new ApplicationRole();

            List<string> userRoleNames = new List<string>();
            var user = _context.ApplicationUser.Where(a => a.Id == userId).ToList();
            if (user.Count > 0)
            {
                UserObj = user[0];
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
                             .Include(t => t.AssetDetail.Hospital)
                             .Include(t => t.User)

                     .OrderByDescending(a => a.RequestDate).ToList();
            if (lstRequests.Count > 0)
            {
                if (UserObj.GovernorateId == 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
                {
                    lstRequests = lstRequests.ToList();
                }
                if (UserObj.GovernorateId > 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
                {
                    lstRequests = lstRequests.Where(t => t.AssetDetail.Hospital.GovernorateId == UserObj.GovernorateId).ToList();
                }
                if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId == 0)
                {
                    lstRequests = lstRequests.Where(t => t.AssetDetail.Hospital.GovernorateId == UserObj.GovernorateId && t.AssetDetail.Hospital.CityId == UserObj.CityId).ToList();
                }
                if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId == 0 && UserObj.HospitalId == 0)
                {
                    lstRequests = lstRequests.Where(t => t.AssetDetail.Hospital.OrganizationId == UserObj.OrganizationId).ToList();
                }
                if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId == 0)
                {
                    lstRequests = lstRequests.Where(t => t.AssetDetail.Hospital.OrganizationId == UserObj.OrganizationId && t.AssetDetail.Hospital.SubOrganizationId == UserObj.SubOrganizationId).ToList();
                }
                if (UserObj.HospitalId > 0)
                {
                    if (userRoleNames.Contains("Admin"))
                    {
                        lstRequests = lstRequests.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
                    }
                    else if ((userRoleNames.Contains("AssetOwner") || userRoleNames.Contains("SRCreator")) && !userRoleNames.Contains("TLHospitalManager") && !userRoleNames.Contains("EngDepManager") && !userRoleNames.Contains("SRReviewer"))
                    {
                        lstRequests = lstRequests.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                    }
                    else if ((userRoleNames.Contains("AssetOwner") && userRoleNames.Contains("SRCreator")) && !userRoleNames.Contains("TLHospitalManager") && !userRoleNames.Contains("EngDepManager") && !userRoleNames.Contains("SRReviewer"))
                    {
                        lstRequests = lstRequests.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                    }
                    else if (userRoleNames.Contains("TLHospitalManager") && userRoleNames.Contains("EngDepManager") && userRoleNames.Contains("SRReviewer") && userRoleNames.Contains("SRCreator") && userRoleNames.Contains("AssetOwner") && userRoleNames.Contains("SRCreator"))
                    {
                        lstRequests = lstRequests.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
                    }
                    else if (userRoleNames.Contains("TLHospitalManager") && !userRoleNames.Contains("EngDepManager") && !userRoleNames.Contains("SRReviewer") && !userRoleNames.Contains("SRCreator") && !userRoleNames.Contains("AssetOwner"))
                    {
                        lstRequests = lstRequests.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
                    }
                    else if (userRoleNames.Contains("TLHospitalManager") && userRoleNames.Contains("EngDepManager") && !userRoleNames.Contains("SRReviewer") && !userRoleNames.Contains("SRCreator") && !userRoleNames.Contains("AssetOwner"))
                    {
                        lstRequests = lstRequests.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
                    }
                    else if (userRoleNames.Contains("TLHospitalManager") && userRoleNames.Contains("EngDepManager") && userRoleNames.Contains("SRReviewer") && !userRoleNames.Contains("SRCreator") && !userRoleNames.Contains("AssetOwner"))
                    {
                        lstRequests = lstRequests.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
                    }
                    else if (userRoleNames.Contains("TLHospitalManager") && userRoleNames.Contains("EngDepManager") && userRoleNames.Contains("SRReviewer") && userRoleNames.Contains("SRCreator") && !userRoleNames.Contains("AssetOwner"))
                    {
                        lstRequests = lstRequests.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
                    }
                    else if (userRoleNames.Contains("TLHospitalManager") && userRoleNames.Contains("EngDepManager") && userRoleNames.Contains("SRReviewer") && userRoleNames.Contains("SRCreator") && userRoleNames.Contains("AssetOwner"))
                    {
                        lstRequests = lstRequests.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
                    }
                    else if (userRoleNames.Contains("EngDepManager"))
                    {
                        lstRequests = lstRequests.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
                    }
                }
            }
            return lstRequests.Count();

        }
        public int CreateRequestAttachments(RequestDocument attachObj)
        {
            RequestDocument documentObj = new RequestDocument();
            documentObj.DocumentName = attachObj.DocumentName;
            documentObj.FileName = attachObj.FileName;
            documentObj.RequestTrackingId = attachObj.RequestTrackingId;
            documentObj.HospitalId = attachObj.HospitalId;
            _context.RequestDocument.Add(documentObj);
            _context.SaveChanges();
            return attachObj.Id;
        }
        public int UpdateOpenedRequest(int requestId)
        {
            Request request = _context.Request.Find(requestId);
            request.IsOpened = true;
            _context.Entry(request).State = EntityState.Modified;
            _context.SaveChanges();
            return requestId;
        }
        public List<Request> ListOpenRequests(int hospitalId)
        {
            List<Request> listCountRequests = new List<Request>();
            var list = _context.Request
                               .Include(t => t.AssetDetail)
                               .Include(t => t.AssetDetail.Hospital)
                               .Include(t => t.AssetDetail.Hospital.Governorate)
                               .Include(t => t.AssetDetail.Hospital.City)
                               .Include(t => t.AssetDetail.Hospital.Organization)
                               .Include(t => t.AssetDetail.Hospital.SubOrganization)
                               .Include(t => t.User)
                               .Where(a => a.IsOpened == false && a.AssetDetail.HospitalId == hospitalId).ToList();

            foreach (var item in list)
            {
                var listWO = _context.WorkOrders.Where(a => a.RequestId == item.Id).ToList();
                if (listWO.Count == 0)
                {
                    listCountRequests.Add(item);
                }
            }
            return listCountRequests;
        }
        public List<IndexRequestVM.GetData> ListNewRequests(int hospitalId)
        {
            List<IndexRequestVM.GetData> listReuests = new List<IndexRequestVM.GetData>();
            var list = _context.Request
                               .Include(t => t.AssetDetail)
                               .Include(t => t.AssetDetail.MasterAsset)
                               .Include(t => t.AssetDetail.Hospital)
                               .Include(t => t.AssetDetail.Hospital.Governorate)
                               .Include(t => t.AssetDetail.Hospital.City)
                               .Include(t => t.AssetDetail.Hospital.Organization)
                               .Include(t => t.AssetDetail.Hospital.SubOrganization)
                               .Include(t => t.User)
                               .Where(a => a.IsOpened == false && a.AssetDetail.HospitalId == hospitalId).ToList();

            foreach (var item in list)
            {
                var listWO = _context.WorkOrders.Where(a => a.RequestId == item.Id).ToList();
                if (listWO.Count == 0)
                {
                    IndexRequestVM.GetData getDataObj = new IndexRequestVM.GetData();
                    getDataObj.Id = item.Id;
                    getDataObj.Subject = item.Subject;
                    getDataObj.RequestCode = item.RequestCode;
                    getDataObj.Barcode = item.AssetDetail.Barcode;
                    getDataObj.AssetHospitalId = item.HospitalId;
                    getDataObj.SerialNumber = item.AssetDetail.SerialNumber;
                    getDataObj.AssetName = item.AssetDetail.MasterAsset.Name;
                    getDataObj.AssetNameAr = item.AssetDetail.MasterAsset.NameAr;
                    listReuests.Add(getDataObj);
                }
            }
            return listReuests;
        }
        public List<IndexRequestTracking> ListOpenRequestTracks(int hospitalId)
        {
            List<IndexRequestTracking> list = new List<IndexRequestTracking>();
            var lstRequests = _context.Request
                             .Include(t => t.AssetDetail)
                             .Include(t => t.AssetDetail.Hospital)
                             .Include(t => t.AssetDetail.Hospital.Governorate)
                             .Include(t => t.AssetDetail.Hospital.City)
                             .Include(t => t.AssetDetail.Hospital.Organization)
                             .Include(t => t.AssetDetail.Hospital.SubOrganization)
                             .Include(t => t.User)
                             .Where(a => a.IsOpened == false && a.AssetDetail.HospitalId == hospitalId).ToList();

            foreach (var item in lstRequests)
            {

                var lstTracks = _context.RequestTracking
                              .Include(a => a.Request).Include(a => a.Request.AssetDetail)
                              .Include(a => a.Request.AssetDetail.Hospital)
                              .Include(a => a.Request.AssetDetail.MasterAsset)
                              .Where(a => a.IsOpened == false && a.Request.AssetDetail.HospitalId == hospitalId && a.RequestId == item.Id && a.RequestStatusId == 2).ToList()
                              .OrderByDescending(a => a.Id).ToList().GroupBy(a => a.RequestId).ToList();




                foreach (var track in lstTracks)
                {
                    IndexRequestTracking itm = new IndexRequestTracking();
                    itm.Id = track.FirstOrDefault().Id;
                    itm.Subject = track.FirstOrDefault().Description;
                    itm.AssetName = track.FirstOrDefault().Request.AssetDetail.MasterAsset.Name;
                    itm.AssetNameAr = track.FirstOrDefault().Request.AssetDetail.MasterAsset.NameAr;
                    itm.SerialNumber = track.FirstOrDefault().Request.AssetDetail.SerialNumber;
                    itm.BarCode = track.FirstOrDefault().Request.AssetDetail.Barcode;
                    list.Add(itm);
                }
            }
            //var lstTracks = _context.RequestTracking
            //                   .Include(a => a.Request).Include(a => a.Request.AssetDetail)
            //                   .Include(a => a.Request.AssetDetail.Hospital)
            //                   .Include(a => a.Request.AssetDetail.MasterAsset)
            //                   .Where(a => a.IsOpened == false && a.Request.AssetDetail.HospitalId == hospitalId).ToList()
            //                   .OrderByDescending(a => a.Id).ToList().GroupBy(a => a.RequestId).ToList();

            //foreach (var track in lstTracks)
            //{
            //    IndexRequestTracking item = new IndexRequestTracking();
            //    item.Id = track.FirstOrDefault().Id;
            //    item.Subject = track.FirstOrDefault().Description;
            //    item.AssetName = track.FirstOrDefault().Request.AssetDetail.MasterAsset.Name;
            //    item.AssetNameAr = track.FirstOrDefault().Request.AssetDetail.MasterAsset.NameAr;
            //    list.Add(item);
            //}
            return list;
        }
        public int UpdateOpenedRequestTrack(int trackId)
        {
            RequestTracking requestTrackingObj = _context.RequestTracking.Find(trackId);
            requestTrackingObj.IsOpened = true;
            _context.Entry(requestTrackingObj).State = EntityState.Modified;
            _context.SaveChanges();
            return trackId;
        }
        public List<ReportRequestVM> GetRequestEstimationById(int id)
        {
            List<ReportRequestVM> list = new List<ReportRequestVM>();
            var lstTracks = _context.RequestTracking.Where(a => a.RequestId == id && a.RequestStatusId == 1).ToList();
            if (lstTracks.Count > 0)
            {
                var trackObj = lstTracks[0];
                var srWorkOrder = _context.WorkOrderTrackings.Include(a => a.WorkOrder).Include(a => a.WorkOrder.Request).Where(a => a.WorkOrder.RequestId == id && a.WorkOrderStatusId == 1).ToList().FirstOrDefault();

                var lstWorkorders = _context.WorkOrderTrackings.Include(a => a.WorkOrder).Include(a => a.WorkOrder.Request).Where(a => a.WorkOrder.RequestId == id).ToList();

                if (lstWorkorders.Count > 0)
                {

                    WorkOrderTracking woObj = lstWorkorders[0];
                    //foreach (var woObj in lstWorkorders)
                    //{ 
                    ReportRequestVM reportObj = new ReportRequestVM();
                    reportObj.Id = trackObj.Request.Id;
                    reportObj.InitialWorkOrderDate = srWorkOrder.CreationDate.ToString();
                    reportObj.RequestNumber = trackObj.Request.RequestCode;

                    reportObj.StartRequestDate = trackObj.Request.RequestDate.ToString();
                    lstWorkorders = lstWorkorders.Where(a => a.WorkOrderStatusId == 2 && a.WorkOrder.RequestId == id).ToList();
                    if (lstWorkorders.Count > 0)
                    {
                        var woLastTrack = lstWorkorders.ToList().First();
                        reportObj.StartWorkOrderDate = woLastTrack.CreationDate.ToString();

                        TimeSpan difference = DateTime.Parse(woObj.CreationDate.ToString()) - DateTime.Parse(trackObj.Request.RequestDate.ToString());
                        int days = difference.Days;
                        int hours = difference.Hours;
                        int minutes = difference.Minutes;
                        int seconds = difference.Seconds;
                        var elapsedTime = days + " days " + hours + " hours " + minutes + " minutes " + seconds + " seconds";
                        reportObj.DurationBetweenStartRequestWorkOrder = elapsedTime;
                    }


                    var firstInProgressWork = _context.WorkOrderTrackings.Include(a => a.WorkOrder).Include(a => a.WorkOrder.Request).Where(a => a.WorkOrderStatusId == 2 && a.WorkOrder.RequestId == id).ToList();
                    var lastInProgressWork = _context.WorkOrderTrackings.Include(a => a.WorkOrder).Include(a => a.WorkOrder.Request).Where(a => a.WorkOrderStatusId == 12 && a.WorkOrder.RequestId == id).ToList();
                    if (firstInProgressWork.Count > 0 && lastInProgressWork.Count > 0)
                    {
                        var workOrderStartInProgress = firstInProgressWork.ToList().First();
                        reportObj.FirstStepInTrackWorkOrderInProgress = workOrderStartInProgress.CreationDate.ToString();

                        var workOrderEndInProgress = lastInProgressWork.ToList().Last();
                        reportObj.LastStepInTrackWorkOrderInProgress = workOrderEndInProgress.CreationDate.ToString();

                        TimeSpan difference = DateTime.Parse(workOrderEndInProgress.CreationDate.ToString()) - DateTime.Parse(workOrderStartInProgress.CreationDate.ToString());
                        int days = difference.Days;
                        int hours = difference.Hours;
                        int minutes = difference.Minutes;
                        int seconds = difference.Seconds;
                        var elapsedTime = days + " days " + hours + " hours " + minutes + " minutes " + seconds + " seconds";
                        reportObj.DurationBetweenWorkOrders = elapsedTime;
                    }


                    var closedWorkOrder = _context.WorkOrderTrackings.Where(a => a.WorkOrderStatusId == 12 && a.WorkOrder.Request.Id == id && a.WorkOrderId == woObj.Id).ToList();
                    var closeRequest = _context.RequestTracking.Where(a => a.RequestId == id && a.RequestStatusId == 2).ToList();



                    if (closedWorkOrder.Count > 0)
                    {

                        var workOrderCloseDate = closedWorkOrder.ToList().Last();
                        reportObj.ClosedWorkOrderDate = workOrderCloseDate.CreationDate.ToString();



                        var requestCloseDate = closeRequest.ToList().Last();
                        reportObj.CloseRequestDate = requestCloseDate.DescriptionDate.ToString();

                        TimeSpan difference = DateTime.Parse(requestCloseDate.DescriptionDate.ToString()) - DateTime.Parse(workOrderCloseDate.CreationDate.ToString());
                        int days = difference.Days;
                        int hours = difference.Hours;
                        int minutes = difference.Minutes;
                        int seconds = difference.Seconds;
                        var elapsedTime = days + " days " + hours + " hours " + minutes + " minutes " + seconds + " seconds";
                        reportObj.DurationTillCloseDate = elapsedTime;

                    }


                    list.Add(reportObj);
                    //  }
                }
            }

            return list;
        }
        public List<ReportRequestVM> GetRequestEstimations(SearchRequestDateVM searchRequestDateObj)
        {
            List<ReportRequestVM> list = new List<ReportRequestVM>();
            DateTime start = new DateTime();
            DateTime end = new DateTime();
            if (searchRequestDateObj.StrStartDate != "")
                start = DateTime.Parse(searchRequestDateObj.StrStartDate);
            if (searchRequestDateObj.StrEndDate != "")
                end = DateTime.Parse(searchRequestDateObj.StrEndDate);




            var lstTracks = _context.RequestTracking.Include(a => a.Request).Where(a => a.RequestStatusId == 1
            && (a.Request.RequestDate.Date >= start.Date && a.Request.RequestDate.Date <= end.Date)).ToList().GroupBy(a => a.RequestId).ToList();
            if (lstTracks.Count > 0)
            {
                foreach (var trackObj in lstTracks)
                {
                    var srWorkOrder = _context.WorkOrderTrackings.Include(a => a.WorkOrder)
                                        .Include(a => a.WorkOrder.Request).Where(a => a.WorkOrder.RequestId == trackObj.FirstOrDefault().RequestId && a.WorkOrderStatusId == 1).ToList()
                                        .FirstOrDefault();


                    var lstWorkorders = _context.WorkOrderTrackings.Include(a => a.WorkOrder)
                                        .Include(a => a.WorkOrder.Request).Where(a => a.WorkOrder.RequestId == trackObj.FirstOrDefault().RequestId).ToList().GroupBy(a => a.WorkOrder.RequestId).ToList();

                    if (lstWorkorders.Count > 0)
                    {
                        foreach (var woObj in lstWorkorders)
                        {
                            ReportRequestVM reportObj = new ReportRequestVM();
                            reportObj.Id = trackObj.FirstOrDefault().Request.Id;

                            reportObj.WorkOrderNumber = srWorkOrder.WorkOrder.WorkOrderNumber;
                            reportObj.RequestNumber = trackObj.FirstOrDefault().Request.RequestCode;
                            reportObj.StartRequestDate = trackObj.FirstOrDefault().Request.RequestDate.ToString();
                            reportObj.InitialWorkOrderDate = srWorkOrder.CreationDate.ToString();

                            var lstWorkorders2 = _context.WorkOrderTrackings.Include(a => a.WorkOrder)
                                                  .Include(a => a.WorkOrder.Request)
                                                  .Where(a => a.WorkOrderStatusId == 2 && a.WorkOrder.RequestId == trackObj.FirstOrDefault().RequestId)
                                                  .ToList().GroupBy(a => a.WorkOrder.RequestId).ToList();



                            if (lstWorkorders2.Count > 0)
                            {
                                var woLastTrack = lstWorkorders2.ToList().First();
                                reportObj.StartWorkOrderDate = woLastTrack.FirstOrDefault().CreationDate.ToString();
                                string elapsedTime = "";
                                TimeSpan difference = DateTime.Parse(woObj.FirstOrDefault().CreationDate.ToString()) - DateTime.Parse(trackObj.FirstOrDefault().Request.RequestDate.ToString());
                                int days = difference.Days;
                                int hours = difference.Hours;
                                int minutes = difference.Minutes;
                                int seconds = difference.Seconds;
                                if (searchRequestDateObj.Lang == "en")
                                    elapsedTime = days + " days " + hours + " hours ";// + minutes + " minutes " + seconds + " seconds";
                                else
                                    elapsedTime = days + " يوم " + hours + " ساعة ";// + minutes + " دقيقة " + seconds + " ثانية";

                                reportObj.DurationBetweenStartRequestWorkOrder = elapsedTime;
                            }

                            var firstInProgressWork = _context.WorkOrderTrackings.Include(a => a.WorkOrder).Include(a => a.WorkOrder.Request).Where(a => a.WorkOrderStatusId == 2 && a.WorkOrder.RequestId == trackObj.FirstOrDefault().RequestId).ToList();
                            var lastInProgressWork = _context.WorkOrderTrackings.Include(a => a.WorkOrder).Include(a => a.WorkOrder.Request).Where(a => a.WorkOrderStatusId == 12 && a.WorkOrder.RequestId == trackObj.FirstOrDefault().RequestId).ToList();

                            WorkOrderTracking workOrderStartInProgress = new WorkOrderTracking();
                            WorkOrderTracking workOrderEndInProgress = new WorkOrderTracking();
                            if (firstInProgressWork.Count > 0)
                            {
                                workOrderStartInProgress = firstInProgressWork.ToList().First();
                                reportObj.FirstStepInTrackWorkOrderInProgress = workOrderStartInProgress.CreationDate.ToString();
                            }
                            if (firstInProgressWork.Count > 0 && lastInProgressWork.Count > 0)
                            {
                                workOrderEndInProgress = lastInProgressWork.ToList().Last();
                                reportObj.LastStepInTrackWorkOrderInProgress = workOrderEndInProgress.CreationDate.ToString();
                            }
                            if (firstInProgressWork.Count > 0 && lastInProgressWork.Count > 0)
                            {
                                TimeSpan difference = DateTime.Parse(workOrderEndInProgress.CreationDate.ToString()) - DateTime.Parse(workOrderStartInProgress.CreationDate.ToString());
                                int days = difference.Days;
                                int hours = difference.Hours;
                                int minutes = difference.Minutes;
                                int seconds = difference.Seconds;
                                // var elapsedTime = days + " days " + hours + " hours " + minutes + " minutes " + seconds + " seconds";
                                string elapsedTime = "";
                                if (searchRequestDateObj.Lang == "en")
                                    elapsedTime = days + " days " + hours + " hours ";// + minutes + " minutes " + seconds + " seconds";
                                else
                                    elapsedTime = days + " يوم " + hours + " ساعة ";// + minutes + " دقيقة " + seconds + " ثانية";

                                reportObj.DurationBetweenWorkOrders = elapsedTime;
                            }



                            var closedWorkOrder = _context.WorkOrderTrackings.Where(a => a.WorkOrderStatusId == 12 && a.WorkOrder.Request.Id == trackObj.FirstOrDefault().RequestId && a.WorkOrderId == woObj.FirstOrDefault().WorkOrder.Id).ToList();
                            var closeRequest = _context.RequestTracking.Where(a => a.RequestId == trackObj.FirstOrDefault().RequestId && a.RequestStatusId == 2).ToList();

                            WorkOrderTracking workOrderCloseDate = new WorkOrderTracking();
                            RequestTracking requestCloseDate = new RequestTracking();

                            if (closedWorkOrder.Count > 0)
                            {
                                workOrderCloseDate = closedWorkOrder.ToList().Last();
                                reportObj.ClosedWorkOrderDate = workOrderCloseDate.CreationDate.ToString();
                            }
                            if (closeRequest.Count > 0)
                            {
                                requestCloseDate = closeRequest.ToList().Last();
                                reportObj.CloseRequestDate = requestCloseDate.DescriptionDate.ToString();
                            }

                            if (closedWorkOrder.Count > 0 && closeRequest.Count > 0)
                            {
                                TimeSpan difference = DateTime.Parse(workOrderCloseDate.CreationDate.ToString()) - DateTime.Parse(trackObj.FirstOrDefault().Request.RequestDate.ToString());
                                int days = difference.Days;
                                int hours = difference.Hours;
                                int minutes = difference.Minutes;
                                int seconds = difference.Seconds;
                                //  var elapsedTime = days + " days " + hours + " hours " + minutes + " minutes " + seconds + " seconds";

                                string elapsedTime = "";
                                if (searchRequestDateObj.Lang == "en")
                                    elapsedTime = days + " days " + hours + " hours " + minutes + " minutes " + seconds + " seconds";
                                else
                                    elapsedTime = days + " يوم " + hours + " ساعة " + minutes + " دقيقة " + seconds + " ثانية";

                                reportObj.DurationTillCloseDate = elapsedTime;
                            }

                            list.Add(reportObj);
                        }
                    }

                    else
                    {
                        ReportRequestVM reportObj = new ReportRequestVM();
                        reportObj.Id = trackObj.FirstOrDefault().Request.Id;
                        reportObj.RequestNumber = trackObj.FirstOrDefault().Request.RequestCode;
                        reportObj.StartRequestDate = trackObj.FirstOrDefault().Request.RequestDate.ToString();
                        list.Add(reportObj);
                    }

                }
            }
            return list;
        }
        public int GetRequestsCountByStatusIdAndPaging(string userId, int statusId)
        {
            IQueryable<Request> lstRequests = _context.Request
                                        .Include(a => a.AssetDetail)
                                        .Include(a => a.AssetDetail.Hospital)
                                        .Include(a => a.AssetDetail.Hospital.Governorate)
                                        .Include(a => a.AssetDetail.Hospital.City)
                                        .Include(a => a.AssetDetail.Hospital.Organization)
                                        .Include(a => a.AssetDetail.Hospital.SubOrganization)
                                        .Include(a => a.User)
                                        .Include(a => a.RequestMode)
                                        .Include(a => a.RequestPeriority)
                                        .Include(a => a.RequestType)
                                        .Include(a => a.AssetDetail)
                                        .Include(a => a.AssetDetail.MasterAsset)
                                        .OrderByDescending(a => a.RequestDate.Date)
                                        .AsQueryable();


            if (statusId == 0)
            {
                List<RequestTracking> listTracks = new List<RequestTracking>();
                lstRequests = lstRequests.AsQueryable();

                var allrequests = lstRequests.ToList<Request>();
                var requestsPerPage = allrequests.ToList<Request>();
                if (requestsPerPage.Count() > 0)
                {
                    foreach (var req in requestsPerPage)
                    {
                        var lstTracks = _context.RequestTracking.Include(a => a.Request).Include(a => a.RequestStatus).Where(a => a.RequestId == req.Id).OrderByDescending(a => a.DescriptionDate).ToList();
                        if (lstTracks.Count > 0)
                        {
                            var requestStatusId = lstTracks[0].RequestStatusId;
                            listTracks.Add(lstTracks[0]);
                        }
                    }
                    var count = listTracks.Select(a => a.Request).Count();
                }
            }
            else
            {
                List<RequestTracking> listTracks = new List<RequestTracking>();
                var allrequests = lstRequests.ToList<Request>();
                var requestsPerPage = allrequests.ToList<Request>();
                if (requestsPerPage.Count() > 0)
                {
                    foreach (var req in requestsPerPage)
                    {
                        var lstTracks = _context.RequestTracking.Include(a => a.Request).Include(a => a.RequestStatus).Where(a => a.RequestId == req.Id).OrderByDescending(a => a.DescriptionDate).ToList();
                        if (lstTracks.Count > 0)
                        {
                            var requestStatusId = lstTracks[0].RequestStatusId;
                            if (requestStatusId == statusId)
                                listTracks.Add(lstTracks[0]);
                        }
                    }
                    var count = listTracks.Select(a => a.Request).Count();

                    return count;
                }
            }

            return lstRequests.Count();
        }
        public List<IndexRequestVM.GetData> GetRequestsByStatusIdAndPaging(string userId, int statusId, int pageNumber, int pageSize)
        {
            List<RequestTracking> lstTracks2 = new List<RequestTracking>();
            List<IndexRequestVM.GetData> lstModel = new List<IndexRequestVM.GetData>();
            ApplicationUser UserObj = new ApplicationUser();
            List<string> lstRoleNames = new List<string>();

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
                    lstRoleNames.Add(name.Name);
                }
            }

            IQueryable<Request> requests = _context.Request
                                        .Include(a => a.AssetDetail)
                                        .Include(a => a.AssetDetail.Hospital)
                                        .Include(a => a.AssetDetail.Hospital.Governorate)
                                        .Include(a => a.AssetDetail.Hospital.City)
                                        .Include(a => a.AssetDetail.Hospital.Organization)
                                        .Include(a => a.AssetDetail.Hospital.SubOrganization)
                                        .Include(a => a.User)
                                        .Include(a => a.RequestMode)
                                        .Include(a => a.RequestPeriority)
                                        .Include(a => a.RequestType)
                                        .Include(a => a.AssetDetail.MasterAsset)
                                        .OrderByDescending(a => a.RequestDate)
                                        .AsQueryable();

            var allrequests = requests.ToList<Request>();



            if (UserObj.GovernorateId == 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
            {
                allrequests = allrequests.ToList();
            }
            if (UserObj.GovernorateId > 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
            {
                allrequests = allrequests.Where(t => t.AssetDetail.Hospital.GovernorateId == UserObj.GovernorateId).ToList();
            }
            if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId == 0)
            {
                allrequests = allrequests.Where(t => t.AssetDetail.Hospital.CityId == UserObj.CityId).ToList();
            }
            if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId == 0 && UserObj.HospitalId == 0)
            {
                allrequests = allrequests.Where(t => t.AssetDetail.Hospital.OrganizationId == UserObj.OrganizationId).ToList();
            }
            if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId == 0)
            {
                allrequests = allrequests.Where(t => t.AssetDetail.Hospital.SubOrganizationId == UserObj.SubOrganizationId).ToList();
            }
            if (UserObj.HospitalId > 0)
            {
                if (lstRoleNames.Contains("Admin"))
                {
                    allrequests = allrequests.ToList();
                }
                if (lstRoleNames.Contains("TLHospitalManager"))
                {
                    allrequests = allrequests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                }
                if (lstRoleNames.Contains("AssetOwner"))
                {
                    allrequests = allrequests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId && t.CreatedById == userId).ToList();
                }
                if (lstRoleNames.Contains("SRCreator"))
                {
                    allrequests = allrequests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId && t.CreatedById == userId).ToList();
                }
                if (lstRoleNames.Contains("EngDepManager"))
                {
                    allrequests = allrequests.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
                }
                if (lstRoleNames.Contains("SRReviewer"))
                {
                    allrequests = allrequests.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
                }
            }


            if (allrequests.Count() > 0)
            {
                foreach (var req in allrequests)
                {
                    if (statusId > 0)
                    {
                        var lstTracks = _context.RequestTracking.Include(a => a.RequestStatus).Where(a => a.RequestId == req.Id).OrderByDescending(a => a.DescriptionDate).ToList();
                        if (lstTracks.Count > 0)
                        {
                            var requestStatusId = lstTracks[0].RequestStatusId;
                            if (requestStatusId == statusId)
                            {
                                IndexRequestVM.GetData getDataObj = new IndexRequestVM.GetData();
                                getDataObj.RequestId = req.Id;
                                getDataObj.Id = req.Id;
                                getDataObj.RequestCode = req.RequestCode;
                                getDataObj.Barcode = req.AssetDetail.Barcode;
                                getDataObj.CreatedById = req.CreatedById;
                                getDataObj.CreatedBy = lstTracks[0].User != null ? lstTracks[0].User.UserName : "";
                                getDataObj.Subject = req.Subject;
                                getDataObj.RequestDate = req.RequestDate;
                                getDataObj.AssetDetailId = req.AssetDetailId != null ? (int)req.AssetDetailId : 0;
                                getDataObj.HospitalId = req.AssetDetail.HospitalId;
                                getDataObj.StatusId = (int)lstTracks[0].RequestStatusId;
                                getDataObj.StatusName = lstTracks[0].RequestStatus.Name;
                                getDataObj.StatusNameAr = lstTracks[0].RequestStatus.NameAr;
                                getDataObj.StatusColor = lstTracks[0].RequestStatus.Color;
                                getDataObj.StatusIcon = lstTracks[0].RequestStatus.Icon;
                                getDataObj.Description = lstTracks[0].Description;
                                if (int.Parse(lstTracks[0].RequestStatusId.ToString()) == 2)
                                {
                                    getDataObj.ClosedDate = lstTracks[0].DescriptionDate.ToString();
                                }
                                else
                                {
                                    getDataObj.ClosedDate = "";
                                }
                                getDataObj.Barcode = req.AssetDetail.Barcode;
                                getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                                getDataObj.ModelNumber = req.AssetDetail.MasterAsset.ModelNumber;
                                getDataObj.ModeId = req.RequestModeId != null ? (int)req.RequestModeId : 0;
                                getDataObj.ModeName = req.RequestMode != null ? req.RequestMode.Name : "";
                                getDataObj.ModeNameAr = req.RequestMode != null ? req.RequestMode.NameAr : "";
                                getDataObj.PeriorityId = req.RequestPeriorityId != null ? (int)req.RequestPeriorityId : 0;
                                getDataObj.PeriorityName = req.RequestPeriority != null ? req.RequestPeriority.Name : "";
                                getDataObj.PeriorityNameAr = req.RequestPeriority != null ? req.RequestPeriority.NameAr : "";
                                getDataObj.PeriorityColor = req.RequestPeriority != null ? req.RequestPeriority.Color : "";
                                getDataObj.PeriorityIcon = req.RequestPeriority != null ? req.RequestPeriority.Icon : "";
                                getDataObj.AssetHospitalId = req.HospitalId;
                                getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                                getDataObj.Barcode = req.AssetDetail.Barcode;
                                getDataObj.AssetName = req.AssetDetail.MasterAsset.Name;
                                getDataObj.AssetNameAr = req.AssetDetail.MasterAsset.NameAr;
                                getDataObj.ListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id)
                                   .ToList().Select(item => new IndexRequestTrackingVM.GetData
                                   {
                                       Id = item.Id,
                                       StatusName = item.RequestStatusId != null ? lstTracks[0].RequestStatus.Name : "",
                                       StatusNameAr = item.RequestStatusId != null ? lstTracks[0].RequestStatus.NameAr : "",
                                       StatusColor = item.RequestStatusId != null ? lstTracks[0].RequestStatus.Color : "",
                                       StatusIcon = item.RequestStatusId != null ? lstTracks[0].RequestStatus.Icon : "",
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
                                    getDataObj.WOLastTrackDescription = lstWOStatus[0].Notes;
                                }
                                getDataObj.CountListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id).ToList().Count;
                                getDataObj.CountWorkOrder = _context.WorkOrders.Where(a => a.RequestId == req.Id).ToList().Count > 0 ? _context.WorkOrders.Where(a => a.RequestId == req.Id).ToList().Count : 0;
                                getDataObj.GovernorateId = req.User != null ? req.User.GovernorateId : 0;
                                getDataObj.CityId = req.User != null ? req.User.CityId : 0;
                                getDataObj.OrganizationId = req.User != null ? req.User.OrganizationId : 0;
                                getDataObj.SubOrganizationId = req.User != null ? req.User.SubOrganizationId : 0;
                                lstModel.Add(getDataObj);
                            }
                        }
                    }
                    else
                    {
                        var lstTracks = _context.RequestTracking.Include(a => a.RequestStatus).Where(a => a.RequestId == req.Id).OrderByDescending(a => a.DescriptionDate).ToList();
                        if (lstTracks.Count > 0)
                        {
                            var requestStatusId = lstTracks[0].RequestStatusId;
                            IndexRequestVM.GetData getDataObj = new IndexRequestVM.GetData();
                            getDataObj.RequestId = req.Id;
                            getDataObj.Id = req.Id;
                            getDataObj.RequestCode = req.RequestCode;
                            getDataObj.Barcode = req.AssetDetail.Barcode;
                            getDataObj.CreatedById = req.CreatedById;
                            getDataObj.CreatedBy = lstTracks[0].User != null ? lstTracks[0].User.UserName : "";
                            getDataObj.Subject = req.Subject;
                            getDataObj.RequestDate = req.RequestDate;
                            getDataObj.AssetDetailId = req.AssetDetailId != null ? (int)req.AssetDetailId : 0;
                            getDataObj.HospitalId = req.AssetDetail.HospitalId;
                            getDataObj.StatusId = (int)lstTracks[0].RequestStatusId;
                            getDataObj.StatusName = lstTracks[0].RequestStatus.Name;
                            getDataObj.StatusNameAr = lstTracks[0].RequestStatus.NameAr;
                            getDataObj.StatusColor = lstTracks[0].RequestStatus.Color;
                            getDataObj.StatusIcon = lstTracks[0].RequestStatus.Icon;
                            getDataObj.Description = lstTracks[0].Description;
                            if (int.Parse(lstTracks[0].RequestStatusId.ToString()) == 2)
                            {
                                getDataObj.ClosedDate = lstTracks[0].DescriptionDate.ToString();
                            }
                            else
                            {
                                getDataObj.ClosedDate = "";
                            }
                            getDataObj.Barcode = req.AssetDetail.Barcode;
                            getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                            getDataObj.ModelNumber = req.AssetDetail.MasterAsset.ModelNumber;
                            getDataObj.ModeId = req.RequestModeId != null ? (int)req.RequestModeId : 0;
                            getDataObj.ModeName = req.RequestMode != null ? req.RequestMode.Name : "";
                            getDataObj.ModeNameAr = req.RequestMode != null ? req.RequestMode.NameAr : "";
                            getDataObj.PeriorityId = req.RequestPeriorityId != null ? (int)req.RequestPeriorityId : 0;
                            getDataObj.PeriorityName = req.RequestPeriority != null ? req.RequestPeriority.Name : "";
                            getDataObj.PeriorityNameAr = req.RequestPeriority != null ? req.RequestPeriority.NameAr : "";
                            getDataObj.PeriorityColor = req.RequestPeriority != null ? req.RequestPeriority.Color : "";
                            getDataObj.PeriorityIcon = req.RequestPeriority != null ? req.RequestPeriority.Icon : "";
                            getDataObj.AssetHospitalId = req.HospitalId;
                            getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                            getDataObj.Barcode = req.AssetDetail.Barcode;
                            getDataObj.AssetName = req.AssetDetail.MasterAsset.Name;
                            getDataObj.AssetNameAr = req.AssetDetail.MasterAsset.NameAr;
                            getDataObj.ListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id)
                                    .ToList().Select(item => new IndexRequestTrackingVM.GetData
                                    {
                                        Id = item.Id,
                                        StatusName = item.RequestStatusId != null ? lstTracks[0].RequestStatus.Name : "",
                                        StatusNameAr = item.RequestStatusId != null ? lstTracks[0].RequestStatus.NameAr : "",
                                        StatusColor = item.RequestStatusId != null ? lstTracks[0].RequestStatus.Color : "",
                                        StatusIcon = item.RequestStatusId != null ? lstTracks[0].RequestStatus.Icon : "",
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
                                getDataObj.WOLastTrackDescription = lstWOStatus[0].Notes;
                            }
                            getDataObj.CountListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id).ToList().Count;
                            getDataObj.CountWorkOrder = _context.WorkOrders.Where(a => a.RequestId == req.Id).ToList().Count > 0 ? _context.WorkOrders.Where(a => a.RequestId == req.Id).ToList().Count : 0;
                            getDataObj.GovernorateId = req.User != null ? req.User.GovernorateId : 0;
                            getDataObj.CityId = req.User != null ? req.User.CityId : 0;
                            getDataObj.OrganizationId = req.User != null ? req.User.OrganizationId : 0;
                            getDataObj.SubOrganizationId = req.User != null ? req.User.SubOrganizationId : 0;
                            lstModel.Add(getDataObj);
                        }
                    }
                }
            }

            // lstModel = lstModel.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            var requestsPerPage = lstModel.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return requestsPerPage.ToList();
        }

        public IndexRequestVM GetAllRequestsByStatusIdAndPaging(string userId, int statusId, int pageNumber, int pageSize)
        {

            IndexRequestVM mainClass = new IndexRequestVM();
            List<IndexRequestVM.GetData> lstModel = new List<IndexRequestVM.GetData>();
            ApplicationUser UserObj = new ApplicationUser();
            List<string> lstRoleNames = new List<string>();

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
                    lstRoleNames.Add(name.Name);
                }
            }

            IQueryable<Request> requests = _context.Request
                                        .Include(a => a.AssetDetail)
                                        .Include(a => a.AssetDetail.Hospital)
                                        .Include(a => a.AssetDetail.Hospital.Governorate)
                                        .Include(a => a.AssetDetail.Hospital.City)
                                        .Include(a => a.AssetDetail.Hospital.Organization)
                                        .Include(a => a.AssetDetail.Hospital.SubOrganization)
                                        .Include(a => a.User)
                                        .Include(a => a.RequestMode)
                                        .Include(a => a.RequestPeriority)
                                        .Include(a => a.RequestType)
                                        .Include(a => a.AssetDetail.MasterAsset)
                                        .OrderByDescending(a => a.RequestDate)

                                        .AsQueryable();

            var allrequests = requests.ToList<Request>();
            if (UserObj.GovernorateId == 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
            {
                allrequests = allrequests.ToList();
            }
            if (UserObj.GovernorateId > 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
            {
                allrequests = allrequests.Where(t => t.AssetDetail.Hospital.GovernorateId == UserObj.GovernorateId).ToList();
            }
            if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId == 0)
            {
                allrequests = allrequests.Where(t => t.AssetDetail.Hospital.CityId == UserObj.CityId).ToList();
            }
            if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId == 0 && UserObj.HospitalId == 0)
            {
                allrequests = allrequests.Where(t => t.AssetDetail.Hospital.OrganizationId == UserObj.OrganizationId).ToList();
            }
            if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId == 0)
            {
                allrequests = allrequests.Where(t => t.AssetDetail.Hospital.SubOrganizationId == UserObj.SubOrganizationId).ToList();
            }
            if (UserObj.HospitalId > 0)
            {
                if (lstRoleNames.Contains("Admin"))
                {
                    allrequests = allrequests.ToList();
                }
                else if ((lstRoleNames.Contains("AssetOwner") || lstRoleNames.Contains("SRCreator")) && !lstRoleNames.Contains("TLHospitalManager") && !lstRoleNames.Contains("EngDepManager") && !lstRoleNames.Contains("SRReviewer"))
                {
                    allrequests = allrequests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId && t.CreatedById == userId).ToList();
                }
                else if ((lstRoleNames.Contains("AssetOwner") && lstRoleNames.Contains("SRCreator")) && !lstRoleNames.Contains("TLHospitalManager") && !lstRoleNames.Contains("EngDepManager") && !lstRoleNames.Contains("SRReviewer"))
                {
                    allrequests = allrequests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId && t.CreatedById == userId).ToList();
                }
                else if (lstRoleNames.Contains("TLHospitalManager") && lstRoleNames.Contains("EngDepManager") && lstRoleNames.Contains("SRReviewer") && lstRoleNames.Contains("SRCreator") && lstRoleNames.Contains("AssetOwner") && lstRoleNames.Contains("SRCreator"))
                {
                    allrequests = allrequests.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
                }
                else if (lstRoleNames.Contains("TLHospitalManager") && !lstRoleNames.Contains("EngDepManager") && !lstRoleNames.Contains("SRReviewer") && !lstRoleNames.Contains("SRCreator") && !lstRoleNames.Contains("AssetOwner") && !lstRoleNames.Contains("SRCreator"))
                {
                    allrequests = allrequests.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
                }
                else if (lstRoleNames.Contains("TLHospitalManager") && lstRoleNames.Contains("EngDepManager") && !lstRoleNames.Contains("SRReviewer") && !lstRoleNames.Contains("SRCreator") && !lstRoleNames.Contains("AssetOwner") && !lstRoleNames.Contains("SRCreator"))
                {
                    allrequests = allrequests.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
                }
                else if (lstRoleNames.Contains("TLHospitalManager") && lstRoleNames.Contains("EngDepManager") && lstRoleNames.Contains("SRReviewer") && !lstRoleNames.Contains("SRCreator") && !lstRoleNames.Contains("AssetOwner") && !lstRoleNames.Contains("SRCreator"))
                {
                    allrequests = allrequests.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
                }
                else if (lstRoleNames.Contains("TLHospitalManager") && lstRoleNames.Contains("EngDepManager") && lstRoleNames.Contains("SRReviewer") && lstRoleNames.Contains("SRCreator") && !lstRoleNames.Contains("AssetOwner") && !lstRoleNames.Contains("SRCreator"))
                {
                    allrequests = allrequests.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
                }
                else if (lstRoleNames.Contains("TLHospitalManager") && lstRoleNames.Contains("EngDepManager") && lstRoleNames.Contains("SRReviewer") && lstRoleNames.Contains("SRCreator") && lstRoleNames.Contains("AssetOwner") && !lstRoleNames.Contains("SRCreator"))
                {
                    allrequests = allrequests.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
                }
            }


            if (allrequests.Count() > 0)
            {
                foreach (var req in allrequests)
                {
                    if (statusId > 0)
                    {
                        var lstTracks = _context.RequestTracking.Include(a => a.Request).Include(a => a.User).Include(a => a.RequestStatus).Where(a => a.RequestId == req.Id).OrderByDescending(a => a.DescriptionDate).ToList();
                        if (lstTracks.Count > 0)
                        {
                            var requestStatusId = lstTracks[0].RequestStatusId;
                            if (requestStatusId == statusId)
                            {
                                IndexRequestVM.GetData getDataObj = new IndexRequestVM.GetData();
                                getDataObj.RequestId = req.Id;
                                getDataObj.Id = req.Id;
                                getDataObj.RequestCode = req.RequestCode;
                                getDataObj.Barcode = req.AssetDetail.Barcode;
                                getDataObj.CreatedById = req.CreatedById;
                                getDataObj.CreatedBy = lstTracks[0].User != null ? lstTracks[0].User.UserName : "";
                                getDataObj.Subject = req.Subject;
                                getDataObj.RequestDate = req.RequestDate;
                                getDataObj.AssetDetailId = req.AssetDetailId != null ? (int)req.AssetDetailId : 0;
                                getDataObj.HospitalId = req.AssetDetail.HospitalId;
                                getDataObj.StatusId = (int)lstTracks[0].RequestStatusId;
                                getDataObj.StatusName = lstTracks[0].RequestStatus.Name;
                                getDataObj.StatusNameAr = lstTracks[0].RequestStatus.NameAr;
                                getDataObj.StatusColor = lstTracks[0].RequestStatus.Color;
                                getDataObj.StatusIcon = lstTracks[0].RequestStatus.Icon;
                                getDataObj.Description = lstTracks[0].Description;
                                if (int.Parse(lstTracks[0].RequestStatusId.ToString()) == 2)
                                {
                                    getDataObj.ClosedDate = lstTracks[0].DescriptionDate.ToString();
                                }
                                else
                                {
                                    getDataObj.ClosedDate = "";
                                }
                                getDataObj.Barcode = req.AssetDetail.Barcode;
                                getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                                getDataObj.ModelNumber = req.AssetDetail.MasterAsset.ModelNumber;
                                getDataObj.ModeId = req.RequestModeId != null ? (int)req.RequestModeId : 0;
                                getDataObj.ModeName = req.RequestMode != null ? req.RequestMode.Name : "";
                                getDataObj.ModeNameAr = req.RequestMode != null ? req.RequestMode.NameAr : "";
                                getDataObj.PeriorityId = req.RequestPeriorityId != null ? (int)req.RequestPeriorityId : 0;
                                getDataObj.PeriorityName = req.RequestPeriority != null ? req.RequestPeriority.Name : "";
                                getDataObj.PeriorityNameAr = req.RequestPeriority != null ? req.RequestPeriority.NameAr : "";
                                getDataObj.PeriorityColor = req.RequestPeriority != null ? req.RequestPeriority.Color : "";
                                getDataObj.PeriorityIcon = req.RequestPeriority != null ? req.RequestPeriority.Icon : "";
                                getDataObj.AssetHospitalId = req.HospitalId;
                                getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                                getDataObj.Barcode = req.AssetDetail.Barcode;
                                getDataObj.AssetName = req.AssetDetail.MasterAsset.Name;
                                getDataObj.AssetNameAr = req.AssetDetail.MasterAsset.NameAr;
                                getDataObj.ListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id)
                                   .ToList().Select(item => new IndexRequestTrackingVM.GetData
                                   {
                                       Id = item.Id,
                                       StatusName = item.RequestStatusId != null ? lstTracks[0].RequestStatus.Name : "",
                                       StatusNameAr = item.RequestStatusId != null ? lstTracks[0].RequestStatus.NameAr : "",
                                       StatusColor = item.RequestStatusId != null ? lstTracks[0].RequestStatus.Color : "",
                                       StatusIcon = item.RequestStatusId != null ? lstTracks[0].RequestStatus.Icon : "",
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
                                    getDataObj.WOLastTrackDescription = lstWOStatus[0].Notes;
                                }
                                getDataObj.CountListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id).ToList().Count;
                                getDataObj.CountWorkOrder = _context.WorkOrders.Where(a => a.RequestId == req.Id).ToList().Count > 0 ? _context.WorkOrders.Where(a => a.RequestId == req.Id).ToList().Count : 0;
                                getDataObj.GovernorateId = req.User != null ? req.User.GovernorateId : 0;
                                getDataObj.CityId = req.User != null ? req.User.CityId : 0;
                                getDataObj.OrganizationId = req.User != null ? req.User.OrganizationId : 0;
                                getDataObj.SubOrganizationId = req.User != null ? req.User.SubOrganizationId : 0;
                                lstModel.Add(getDataObj);
                            }
                        }
                    }
                    else
                    {
                        var lstTracks = _context.RequestTracking.Include(a => a.User).Include(a => a.RequestStatus).Where(a => a.RequestId == req.Id).OrderByDescending(a => a.DescriptionDate).ToList();
                        if (lstTracks.Count > 0)
                        {
                            var requestStatusId = lstTracks[0].RequestStatusId;
                            IndexRequestVM.GetData getDataObj = new IndexRequestVM.GetData();
                            getDataObj.RequestId = req.Id;
                            getDataObj.Id = req.Id;
                            getDataObj.RequestCode = req.RequestCode;
                            getDataObj.Barcode = req.AssetDetail.Barcode;
                            getDataObj.CreatedById = req.CreatedById;
                            getDataObj.CreatedBy = lstTracks[0].User != null ? lstTracks[0].User.UserName : "";
                            getDataObj.Subject = req.Subject;
                            getDataObj.RequestDate = req.RequestDate;
                            getDataObj.AssetDetailId = req.AssetDetailId != null ? (int)req.AssetDetailId : 0;
                            getDataObj.HospitalId = req.AssetDetail.HospitalId;
                            getDataObj.StatusId = (int)lstTracks[0].RequestStatusId;
                            getDataObj.StatusName = lstTracks[0].RequestStatus.Name;
                            getDataObj.StatusNameAr = lstTracks[0].RequestStatus.NameAr;
                            getDataObj.StatusColor = lstTracks[0].RequestStatus.Color;
                            getDataObj.StatusIcon = lstTracks[0].RequestStatus.Icon;
                            getDataObj.Description = lstTracks[0].Description;
                            if (int.Parse(lstTracks[0].RequestStatusId.ToString()) == 2)
                            {
                                getDataObj.ClosedDate = lstTracks[0].DescriptionDate.ToString();
                            }
                            else
                            {
                                getDataObj.ClosedDate = "";
                            }
                            getDataObj.Barcode = req.AssetDetail.Barcode;
                            getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                            getDataObj.ModelNumber = req.AssetDetail.MasterAsset.ModelNumber;
                            getDataObj.ModeId = req.RequestModeId != null ? (int)req.RequestModeId : 0;
                            getDataObj.ModeName = req.RequestMode != null ? req.RequestMode.Name : "";
                            getDataObj.ModeNameAr = req.RequestMode != null ? req.RequestMode.NameAr : "";
                            getDataObj.PeriorityId = req.RequestPeriorityId != null ? (int)req.RequestPeriorityId : 0;
                            getDataObj.PeriorityName = req.RequestPeriority != null ? req.RequestPeriority.Name : "";
                            getDataObj.PeriorityNameAr = req.RequestPeriority != null ? req.RequestPeriority.NameAr : "";
                            getDataObj.PeriorityColor = req.RequestPeriority != null ? req.RequestPeriority.Color : "";
                            getDataObj.PeriorityIcon = req.RequestPeriority != null ? req.RequestPeriority.Icon : "";
                            getDataObj.AssetHospitalId = req.HospitalId;
                            getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                            getDataObj.Barcode = req.AssetDetail.Barcode;
                            getDataObj.AssetName = req.AssetDetail.MasterAsset.Name;
                            getDataObj.AssetNameAr = req.AssetDetail.MasterAsset.NameAr;
                            getDataObj.ListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id)
                                    .ToList().Select(item => new IndexRequestTrackingVM.GetData
                                    {
                                        Id = item.Id,
                                        StatusName = item.RequestStatusId != null ? lstTracks[0].RequestStatus.Name : "",
                                        StatusNameAr = item.RequestStatusId != null ? lstTracks[0].RequestStatus.NameAr : "",
                                        StatusColor = item.RequestStatusId != null ? lstTracks[0].RequestStatus.Color : "",
                                        StatusIcon = item.RequestStatusId != null ? lstTracks[0].RequestStatus.Icon : "",
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
                                getDataObj.WOLastTrackDescription = lstWOStatus[0].Notes;
                            }
                            getDataObj.CountListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id).ToList().Count;
                            getDataObj.CountWorkOrder = _context.WorkOrders.Where(a => a.RequestId == req.Id).ToList().Count > 0 ? _context.WorkOrders.Where(a => a.RequestId == req.Id).ToList().Count : 0;
                            getDataObj.GovernorateId = req.User != null ? req.User.GovernorateId : 0;
                            getDataObj.CityId = req.User != null ? req.User.CityId : 0;
                            getDataObj.OrganizationId = req.User != null ? req.User.OrganizationId : 0;
                            getDataObj.SubOrganizationId = req.User != null ? req.User.SubOrganizationId : 0;
                            lstModel.Add(getDataObj);
                        }
                    }
                }
            }

            var requestsPerPage = lstModel.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            mainClass.Results = requestsPerPage;
            mainClass.Count = lstModel.Count;
            return mainClass;
        }
        public IEnumerable<IndexRequestVM.GetData> ExportRequestByStatusId(int? hospitalId, string userId, int statusId)
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
                             .Include(t => t.AssetDetail.MasterAsset.brand)
                               .Include(t => t.AssetDetail.Department)
                            .Include(t => t.AssetDetail.MasterAsset)
                            .Include(t => t.User)
                            .Include(t => t.RequestMode)
                            .Include(t => t.RequestPeriority).Where(a => a.HospitalId == hospitalId).OrderByDescending(a => a.RequestDate.Date).ToList();

            if (lstRequests.Count > 0)
            {
                foreach (var req in lstRequests)
                {
                    IndexRequestVM.GetData getDataObj = new IndexRequestVM.GetData();
                    getDataObj.RequestId = req.Id;
                    getDataObj.Id = req.Id;
                    getDataObj.RequestCode = req.RequestCode;
                    getDataObj.Barcode = req.AssetDetail.Barcode;
                    getDataObj.CreatedById = req.CreatedById;
                    getDataObj.CreatedBy = req.User != null ? req.User.UserName : "";
                    getDataObj.Subject = req.Subject;
                    getDataObj.RequestDate = req.RequestDate;



                    getDataObj.AssetDetailId = req.AssetDetailId != null ? (int)req.AssetDetailId : 0;
                    getDataObj.HospitalId = req.AssetDetail.HospitalId;
                    var lstStatus = _context.RequestTracking.Include(t => t.Request).Include(t => t.RequestStatus)
                                     .Where(a => a.RequestId == req.Id).ToList().OrderByDescending(a => a.DescriptionDate).ToList();
                    if (lstStatus.Count > 0)
                    {
                        getDataObj.StatusId = lstStatus[0].RequestStatus.Id;
                        getDataObj.StatusName = lstStatus[0].RequestStatus.Name;
                        getDataObj.StatusNameAr = lstStatus[0].RequestStatus.NameAr;
                        getDataObj.StatusColor = lstStatus[0].RequestStatus.Color;
                        getDataObj.StatusIcon = lstStatus[0].RequestStatus.Icon;
                        getDataObj.Description = lstStatus[0].Description;
                        if (getDataObj.StatusId == 2)
                        {
                            getDataObj.ClosedDate = lstStatus[0].DescriptionDate.ToString();

                        }
                        else
                        {
                            getDataObj.ClosedDate = "";
                        }
                    }
                    if (req.AssetDetail.MasterAsset.brand != null)
                    {
                        getDataObj.BrandName = req.AssetDetail.MasterAsset.brand.Name;
                        getDataObj.BrandNameAr = req.AssetDetail.MasterAsset.brand.NameAr;
                    }

                    if (req.AssetDetail.Department != null)
                    {
                        getDataObj.DepartmentName = req.AssetDetail.Department.Name;
                        getDataObj.DepartmentNameAr = req.AssetDetail.Department.NameAr;
                    }

                    getDataObj.Barcode = req.AssetDetail.Barcode;
                    getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                    getDataObj.ModeId = req.RequestModeId != null ? (int)req.RequestModeId : 0;
                    getDataObj.ModeName = req.RequestMode != null ? req.RequestMode.Name : "";
                    getDataObj.ModeNameAr = req.RequestMode != null ? req.RequestMode.NameAr : "";
                    getDataObj.PeriorityId = req.RequestPeriorityId != null ? (int)req.RequestPeriorityId : 0;
                    getDataObj.PeriorityName = req.RequestPeriority != null ? req.RequestPeriority.Name : "";
                    getDataObj.PeriorityNameAr = req.RequestPeriority != null ? req.RequestPeriority.NameAr : "";
                    getDataObj.PeriorityColor = req.RequestPeriority != null ? req.RequestPeriority.Color : "";
                    getDataObj.PeriorityIcon = req.RequestPeriority != null ? req.RequestPeriority.Icon : "";
                    getDataObj.AssetHospitalId = req.HospitalId;
                    getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                    getDataObj.Barcode = req.AssetDetail.Barcode;
                    getDataObj.AssetName = req.AssetDetail.MasterAsset.Name;// _context.MasterAssets.Where(a => a.Id == req.AssetDetail.MasterAssetId).ToList().FirstOrDefault().Name;
                    getDataObj.AssetNameAr = req.AssetDetail.MasterAsset.NameAr;
                    list.Add(getDataObj);
                }

            }
            if (list.Count > 0)
            {

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
                if (UserObj.HospitalId > 0)
                {
                    if (userRoleNames.Contains("Admin"))
                    {
                        list = list.ToList();
                    }
                    else if ((userRoleNames.Contains("AssetOwner") || userRoleNames.Contains("SRCreator")) && !userRoleNames.Contains("TLHospitalManager") && !userRoleNames.Contains("EngDepManager") && !userRoleNames.Contains("SRReviewer"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                    }
                    else if ((userRoleNames.Contains("AssetOwner") && userRoleNames.Contains("SRCreator")) && !userRoleNames.Contains("TLHospitalManager") && !userRoleNames.Contains("EngDepManager") && !userRoleNames.Contains("SRReviewer"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                    }
                    else if (userRoleNames.Contains("TLHospitalManager") && userRoleNames.Contains("EngDepManager") && userRoleNames.Contains("SRReviewer") && userRoleNames.Contains("SRCreator") && userRoleNames.Contains("AssetOwner") && userRoleNames.Contains("SRCreator"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                    }
                    else if (userRoleNames.Contains("TLHospitalManager") && !userRoleNames.Contains("EngDepManager") && !userRoleNames.Contains("SRReviewer") && !userRoleNames.Contains("SRCreator") && !userRoleNames.Contains("AssetOwner") && !userRoleNames.Contains("SRCreator"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                    }
                    else if (userRoleNames.Contains("TLHospitalManager") && userRoleNames.Contains("EngDepManager") && !userRoleNames.Contains("SRReviewer") && !userRoleNames.Contains("SRCreator") && !userRoleNames.Contains("AssetOwner") && !userRoleNames.Contains("SRCreator"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                    }
                    else if (userRoleNames.Contains("TLHospitalManager") && userRoleNames.Contains("EngDepManager") && userRoleNames.Contains("SRReviewer") && !userRoleNames.Contains("SRCreator") && !userRoleNames.Contains("AssetOwner") && !userRoleNames.Contains("SRCreator"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                    }
                    else if (userRoleNames.Contains("TLHospitalManager") && userRoleNames.Contains("EngDepManager") && userRoleNames.Contains("SRReviewer") && userRoleNames.Contains("SRCreator") && !userRoleNames.Contains("AssetOwner") && !userRoleNames.Contains("SRCreator"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                    }
                    else if (userRoleNames.Contains("TLHospitalManager") && userRoleNames.Contains("EngDepManager") && userRoleNames.Contains("SRReviewer") && userRoleNames.Contains("SRCreator") && userRoleNames.Contains("AssetOwner") && !userRoleNames.Contains("SRCreator"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                    }
                }
            }
            if (statusId == 0)
            {
                list = list.ToList();
            }
            else
            {
                list = list.Where(t => t.StatusId == statusId).ToList();
            }
            return list;
        }
        public IndexRequestVM GetRequestsByDateAndStatus(SearchRequestDateVM requestDateObj, int pageNumber, int pageSize)
        {
            IndexRequestVM mainClass = new IndexRequestVM();
            List<IndexRequestVM.GetData> list = new List<IndexRequestVM.GetData>();
            ApplicationUser UserObj = new ApplicationUser();
            List<string> userRoleNames = new List<string>();
            var obj = _context.ApplicationUser.Where(a => a.Id == requestDateObj.UserId).ToList();
            if (obj.Count > 0)
            {
                UserObj = obj[0];
                var roles = (from userRole in _context.UserRoles
                             join role in _context.ApplicationRole on userRole.RoleId equals role.Id
                             where userRole.UserId == requestDateObj.UserId
                             select role);
                foreach (var role in roles)
                {
                    userRoleNames.Add(role.Name);
                }
            }

            DateTime? start = new DateTime();
            DateTime? end = new DateTime();

            if (requestDateObj.StrStartDate != "")
            {
                requestDateObj.StartDate = DateTime.Parse(requestDateObj.StrStartDate);
                start = DateTime.Parse(requestDateObj.StrStartDate);
                requestDateObj.StartDate = start;
            }
            else
            {
                requestDateObj.StartDate = DateTime.Parse("01/01/1900");
                start = DateTime.Parse(requestDateObj.StartDate.ToString());
                requestDateObj.StartDate = start;
            }


            if (requestDateObj.StrEndDate != "")
            {
                requestDateObj.EndDate = DateTime.Parse(requestDateObj.StrEndDate);
                end = DateTime.Parse(requestDateObj.StrEndDate);
                requestDateObj.EndDate = end;
            }
            else
            {
                requestDateObj.EndDate = DateTime.Today.Date;
                end = DateTime.Parse(requestDateObj.EndDate.ToString());
                requestDateObj.EndDate = end;
            }

            var lstRequests = _context.RequestTracking
                              .Include(t => t.Request)
                             .Include(t => t.Request.AssetDetail)
                              .Include(t => t.Request.AssetDetail.Hospital)
                             .Include(t => t.Request.AssetDetail.MasterAsset)
                             .Include(t => t.User)
                             .Include(t => t.Request.RequestMode).Include(t => t.RequestStatus)
                             .Include(t => t.Request.RequestPeriority)
                             .OrderByDescending(a => a.DescriptionDate)
                             .ToList().GroupBy(a => a.Request.Id).ToList();

            if (requestDateObj.HospitalId > 0)
            {
                lstRequests = lstRequests.Where(a => a.FirstOrDefault().Request.HospitalId == requestDateObj.HospitalId).ToList();
            }
            else
            {
                lstRequests = lstRequests.ToList();
            }



            if (start != null || end != null)
            {
                lstRequests = lstRequests.Where(a => a.FirstOrDefault().Request.RequestDate.Date >= start.Value.Date && a.FirstOrDefault().Request.RequestDate.Date <= end.Value.Date).ToList();
            }
            else
            {
                lstRequests = lstRequests.ToList();
            }
            if (requestDateObj.StatusId > 0)
            {
                lstRequests = lstRequests.Where(a => a.FirstOrDefault().RequestStatusId == requestDateObj.StatusId).ToList();
            }
            else
            {
                lstRequests = lstRequests.ToList();
            }



            foreach (var req in lstRequests)
            {
                IndexRequestVM.GetData getDataObj = new IndexRequestVM.GetData();
                getDataObj.StatusId = (int)req.FirstOrDefault().RequestStatusId;
                getDataObj.RequestId = req.FirstOrDefault().Request.Id;
                getDataObj.Id = req.FirstOrDefault().Request.Id;
                getDataObj.RequestCode = req.FirstOrDefault().Request.RequestCode;
                getDataObj.Barcode = req.FirstOrDefault().Request.AssetDetail.Barcode;
                getDataObj.CreatedById = req.FirstOrDefault().CreatedById;
                getDataObj.UserName = req.FirstOrDefault().User.UserName;
                getDataObj.Subject = req.FirstOrDefault().Request.Subject;
                getDataObj.AssetHospitalId = req.FirstOrDefault().Request.HospitalId;
                getDataObj.RequestDate = req.FirstOrDefault().Request.RequestDate;
                getDataObj.AssetDetailId = req.FirstOrDefault().Request.AssetDetailId != null ? (int)req.FirstOrDefault().Request.AssetDetailId : 0;
                getDataObj.HospitalId = req.FirstOrDefault().Request.AssetDetail.HospitalId;
                if (req.FirstOrDefault().RequestStatusId != 0)
                {
                    getDataObj.StatusId = int.Parse(req.FirstOrDefault().RequestStatusId.ToString());
                    getDataObj.StatusName = req.FirstOrDefault().RequestStatus.Name;
                    getDataObj.StatusNameAr = req.FirstOrDefault().RequestStatus.NameAr;
                    getDataObj.StatusColor = req.FirstOrDefault().RequestStatus.Color;
                    getDataObj.StatusIcon = req.FirstOrDefault().RequestStatus.Icon;
                    getDataObj.DescriptionDate = req.FirstOrDefault().DescriptionDate;
                    getDataObj.Description = req.FirstOrDefault().Description;
                    if (getDataObj.StatusId == 2)
                    {
                        getDataObj.ClosedDate = req.FirstOrDefault().DescriptionDate.ToString();
                    }
                    else
                    {
                        getDataObj.ClosedDate = "";
                    }
                }


                getDataObj.SerialNumber = req.FirstOrDefault().Request.AssetDetail.SerialNumber;
                getDataObj.ModeId = req.FirstOrDefault().Request.RequestMode.Id;
                getDataObj.ModeName = req.FirstOrDefault().Request.RequestMode.Name;
                getDataObj.ModeNameAr = req.FirstOrDefault().Request.RequestMode.NameAr;
                getDataObj.PeriorityId = req.FirstOrDefault().Request.RequestPeriority.Id;
                getDataObj.PeriorityName = req.FirstOrDefault().Request.RequestPeriority.Name.Trim();
                getDataObj.PeriorityNameAr = req.FirstOrDefault().Request.RequestPeriority.NameAr.Trim();
                getDataObj.PeriorityColor = req.FirstOrDefault().Request.RequestPeriority != null ? req.FirstOrDefault().Request.RequestPeriority.Color.Trim() : "";
                getDataObj.PeriorityIcon = req.FirstOrDefault().Request.RequestPeriority != null ? req.FirstOrDefault().Request.RequestPeriority.Icon : "";
                getDataObj.AssetName = req.FirstOrDefault().Request.AssetDetail.MasterAsset.Name.Trim();
                getDataObj.AssetNameAr = req.FirstOrDefault().Request.AssetDetail.MasterAsset.NameAr;
                getDataObj.CountListTracks = _context.RequestTracking.Where(a => a.RequestId == req.FirstOrDefault().Id).ToList().Count;
                getDataObj.CountWorkOrder = _context.WorkOrders.Where(a => a.RequestId == req.FirstOrDefault().Id).ToList().Count;
                getDataObj.GovernorateId = req.FirstOrDefault().User.GovernorateId;
                getDataObj.CityId = req.FirstOrDefault().User.CityId;
                getDataObj.OrganizationId = req.FirstOrDefault().User.OrganizationId;
                getDataObj.SubOrganizationId = req.FirstOrDefault().User.SubOrganizationId;

                var lstWOStatus = _context.WorkOrderTrackings
                                             .Include(o => o.WorkOrder)
                                             .Include(o => o.WorkOrderStatus)
                                             .Where(a => a.WorkOrder.RequestId == req.FirstOrDefault().RequestId)
                                             .OrderByDescending(a => a.CreationDate.Value).ToList();

                if (lstWOStatus.Count > 0)
                {
                    getDataObj.LatestWorkOrderStatusId = lstWOStatus[0].WorkOrderStatusId;
                    getDataObj.WorkOrderStatusName = lstWOStatus[0].WorkOrderStatus.Name;
                    getDataObj.WorkOrderStatusNameAr = lstWOStatus[0].WorkOrderStatus.NameAr;
                    getDataObj.WorkOrderStatusColor = lstWOStatus[0].WorkOrderStatus.Color;
                    getDataObj.WorkOrderStatusIcon = lstWOStatus[0].WorkOrderStatus.Icon;

                    getDataObj.WorkOrderNote = lstWOStatus[0].Notes;
                    getDataObj.WorkOrderDate = DateTime.Parse(lstWOStatus[0].WorkOrderDate.ToString());

                    if (getDataObj.StatusId == 3 && (getDataObj.LatestWorkOrderStatusId == 2 && (lstWOStatus.Count > 2)))
                    {
                        getDataObj.WorkOrderNote = lstWOStatus[0].Notes;
                        getDataObj.WorkOrderDate = DateTime.Parse(lstWOStatus[0].WorkOrderDate.ToString());
                    }
                    else if (getDataObj.StatusId == 3 && (getDataObj.LatestWorkOrderStatusId == 2 && (lstWOStatus.Count == 2)))
                    {
                        foreach (var item in lstWOStatus.Where(a => a.WorkOrderStatusId == 1))
                        {
                            getDataObj.WorkOrderNote = item.Notes;
                            getDataObj.WorkOrderDate = DateTime.Parse(item.WorkOrderDate.ToString());
                        }
                    }


                }
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
            if (UserObj.HospitalId > 0)
            {
                if (userRoleNames.Contains("Admin"))
                {
                    list = list.ToList();
                }
                else if ((userRoleNames.Contains("AssetOwner") || userRoleNames.Contains("SRCreator")) && !userRoleNames.Contains("TLHospitalManager") && !userRoleNames.Contains("EngDepManager") && !userRoleNames.Contains("SRReviewer"))
                {
                    // list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == requestDateObj.UserId).ToList();
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                else if ((userRoleNames.Contains("AssetOwner") && userRoleNames.Contains("SRCreator")) && !userRoleNames.Contains("TLHospitalManager") && !userRoleNames.Contains("EngDepManager") && !userRoleNames.Contains("SRReviewer"))
                {
                    //list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == requestDateObj.UserId).ToList();
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                else if (userRoleNames.Contains("TLHospitalManager") && userRoleNames.Contains("EngDepManager") && userRoleNames.Contains("SRReviewer") && userRoleNames.Contains("SRCreator") && userRoleNames.Contains("AssetOwner") && userRoleNames.Contains("SRCreator"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                else if (userRoleNames.Contains("TLHospitalManager") && !userRoleNames.Contains("EngDepManager") && !userRoleNames.Contains("SRReviewer") && !userRoleNames.Contains("SRCreator") && !userRoleNames.Contains("AssetOwner") && !userRoleNames.Contains("SRCreator"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                else if (userRoleNames.Contains("TLHospitalManager") && userRoleNames.Contains("EngDepManager") && !userRoleNames.Contains("SRReviewer") && !userRoleNames.Contains("SRCreator") && !userRoleNames.Contains("AssetOwner") && !userRoleNames.Contains("SRCreator"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                else if (userRoleNames.Contains("TLHospitalManager") && userRoleNames.Contains("EngDepManager") && userRoleNames.Contains("SRReviewer") && !userRoleNames.Contains("SRCreator") && !userRoleNames.Contains("AssetOwner") && !userRoleNames.Contains("SRCreator"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                else if (userRoleNames.Contains("TLHospitalManager") && userRoleNames.Contains("EngDepManager") && userRoleNames.Contains("SRReviewer") && userRoleNames.Contains("SRCreator") && !userRoleNames.Contains("AssetOwner") && !userRoleNames.Contains("SRCreator"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                else if (userRoleNames.Contains("TLHospitalManager") && userRoleNames.Contains("EngDepManager") && userRoleNames.Contains("SRReviewer") && userRoleNames.Contains("SRCreator") && userRoleNames.Contains("AssetOwner") && !userRoleNames.Contains("SRCreator"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
            }



            var requestsPerPage = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            mainClass.Results = requestsPerPage;
            mainClass.Count = list.Count();
            return mainClass;



        }
        public List<IndexRequestVM.GetData> GetRequestsByDateAndStatus(SearchRequestDateVM requestDateObj)
        {

            List<IndexRequestVM.GetData> list = new List<IndexRequestVM.GetData>();
            ApplicationUser UserObj = new ApplicationUser();
            List<string> userRoleNames = new List<string>();
            List<string> listNotes = new List<string>();
            List<string> listRequestDesc = new List<string>();
            var obj = _context.ApplicationUser.Where(a => a.Id == requestDateObj.UserId).ToList();
            if (obj.Count > 0)
            {
                UserObj = obj[0];
                var roles = (from userRole in _context.UserRoles
                             join role in _context.ApplicationRole on userRole.RoleId equals role.Id
                             where userRole.UserId == requestDateObj.UserId
                             select role);
                foreach (var role in roles)
                {
                    userRoleNames.Add(role.Name);
                }
            }

            DateTime? start = new DateTime();
            DateTime? end = new DateTime();



            if (requestDateObj.StrStartDate != "")
            {
                requestDateObj.StartDate = DateTime.Parse(requestDateObj.StrStartDate);
                start = DateTime.Parse(requestDateObj.StrStartDate);
            }
            else
            {
                requestDateObj.StartDate = DateTime.Parse("01/01/1900");
                start = DateTime.Parse(requestDateObj.StartDate.Value.Date.ToString());
            }


            if (requestDateObj.StrEndDate != "")
            {
                requestDateObj.EndDate = DateTime.Parse(requestDateObj.StrEndDate);
                end = DateTime.Parse(requestDateObj.StrEndDate);
            }
            else
            {

                var utcDate = DateTime.UtcNow;
                DateTime localDate = utcDate.ToLocalTime();
                requestDateObj.EndDate = localDate.Date;
                end = DateTime.Parse(requestDateObj.EndDate.Value.Date.ToString());
            }


            var lstRequests = _context.RequestTracking
                              .Include(t => t.Request)
                             .Include(t => t.Request.AssetDetail)
                             .Include(t => t.Request.AssetDetail.MasterAsset)
                             .Include(t => t.User)
                             .Include(t => t.Request.RequestMode).Include(t => t.RequestStatus)
                             .Include(t => t.Request.RequestPeriority)
                             .OrderByDescending(a => a.DescriptionDate)
                             .ToList().GroupBy(a => a.Request.Id).ToList();

            if (start != null || end != null)
            {
                lstRequests = lstRequests.Where(a => a.FirstOrDefault().Request.RequestDate.Date >= start.Value.Date && a.FirstOrDefault().Request.RequestDate.Date <= end.Value.Date).ToList();
            }
            else
            {
                lstRequests = lstRequests.ToList();
            }


            if (requestDateObj.StatusId > 0)
            {
                lstRequests = lstRequests.Where(a => a.FirstOrDefault().RequestStatusId == requestDateObj.StatusId).ToList();

            }
            else
            {
                lstRequests = lstRequests.ToList();
            }


            foreach (var req in lstRequests)
            {
                IndexRequestVM.GetData getDataObj = new IndexRequestVM.GetData();
                getDataObj.StatusId = (int)req.FirstOrDefault().RequestStatusId;
                getDataObj.RequestId = req.FirstOrDefault().Request.Id;
                getDataObj.Id = req.FirstOrDefault().Request.Id;
                getDataObj.RequestCode = req.FirstOrDefault().Request.RequestCode;
                getDataObj.Barcode = req.FirstOrDefault().Request.AssetDetail.Barcode;
                getDataObj.CreatedById = req.FirstOrDefault().CreatedById;
                getDataObj.UserName = req.FirstOrDefault().User.UserName;
                getDataObj.Subject = req.FirstOrDefault().Request.Subject;
                getDataObj.AssetHospitalId = req.FirstOrDefault().Request.HospitalId;
                getDataObj.RequestDate = req.FirstOrDefault().Request.RequestDate;
                getDataObj.AssetDetailId = req.FirstOrDefault().Request.AssetDetailId != null ? (int)req.FirstOrDefault().Request.AssetDetailId : 0;
                getDataObj.HospitalId = req.FirstOrDefault().Request.AssetDetail.HospitalId;
                if (req.FirstOrDefault().RequestStatusId != 0)
                {
                    getDataObj.StatusId = int.Parse(req.FirstOrDefault().RequestStatusId.ToString());
                    getDataObj.StatusName = req.FirstOrDefault().RequestStatus.Name;
                    getDataObj.StatusNameAr = req.FirstOrDefault().RequestStatus.NameAr;
                    getDataObj.StatusColor = req.FirstOrDefault().RequestStatus.Color;
                    getDataObj.StatusIcon = req.FirstOrDefault().RequestStatus.Icon;
                    getDataObj.DescriptionDate = req.FirstOrDefault().DescriptionDate;
                    getDataObj.Description = req.FirstOrDefault().Description;


                    var lstDescription = _context.RequestTracking.Include(t => t.Request).Where(a => a.RequestId == req.FirstOrDefault().RequestId).ToList();

                    foreach (var itm in lstDescription)
                    {
                        listRequestDesc.Add("-" + itm.Description);
                    }
                    getDataObj.ListDescriptions = string.Join(Environment.NewLine, listRequestDesc);









                    if (getDataObj.StatusId == 2)
                    {
                        getDataObj.ClosedDate = req.FirstOrDefault().DescriptionDate.ToString();
                    }
                    else
                    {
                        getDataObj.ClosedDate = "";
                    }
                }


                getDataObj.SerialNumber = req.FirstOrDefault().Request.AssetDetail.SerialNumber;
                getDataObj.ModeId = req.FirstOrDefault().Request.RequestMode.Id;
                getDataObj.ModeName = req.FirstOrDefault().Request.RequestMode.Name;
                getDataObj.ModeNameAr = req.FirstOrDefault().Request.RequestMode.NameAr;
                getDataObj.PeriorityId = req.FirstOrDefault().Request.RequestPeriority.Id;
                getDataObj.PeriorityName = req.FirstOrDefault().Request.RequestPeriority.Name.Trim();
                getDataObj.PeriorityNameAr = req.FirstOrDefault().Request.RequestPeriority.NameAr.Trim();
                getDataObj.PeriorityColor = req.FirstOrDefault().Request.RequestPeriority != null ? req.FirstOrDefault().Request.RequestPeriority.Color.Trim() : "";
                getDataObj.PeriorityIcon = req.FirstOrDefault().Request.RequestPeriority != null ? req.FirstOrDefault().Request.RequestPeriority.Icon : "";
                getDataObj.AssetName = req.FirstOrDefault().Request.AssetDetail.MasterAsset.Name.Trim();
                getDataObj.AssetNameAr = req.FirstOrDefault().Request.AssetDetail.MasterAsset.NameAr;
                getDataObj.CountListTracks = _context.RequestTracking.Where(a => a.RequestId == req.FirstOrDefault().Id).ToList().Count;
                getDataObj.CountWorkOrder = _context.WorkOrders.Where(a => a.RequestId == req.FirstOrDefault().Id).ToList().Count;
                getDataObj.GovernorateId = req.FirstOrDefault().User.GovernorateId;
                getDataObj.CityId = req.FirstOrDefault().User.CityId;
                getDataObj.OrganizationId = req.FirstOrDefault().User.OrganizationId;
                getDataObj.SubOrganizationId = req.FirstOrDefault().User.SubOrganizationId;



                var lstWOStatus = _context.WorkOrderTrackings
                       .Include(o => o.WorkOrder).Include(o => o.WorkOrderStatus).Where(a => a.WorkOrder.RequestId == req.FirstOrDefault().RequestId)
                       .OrderByDescending(a => a.CreationDate.Value).ToList();

                if (lstWOStatus.Count > 0)
                {
                    getDataObj.LatestWorkOrderStatusId = lstWOStatus[0].WorkOrderStatusId;
                    getDataObj.WorkOrderStatusName = lstWOStatus[0].WorkOrderStatus.Name;
                    getDataObj.WorkOrderStatusNameAr = lstWOStatus[0].WorkOrderStatus.NameAr;
                    getDataObj.WorkOrderStatusColor = lstWOStatus[0].WorkOrderStatus.Color;
                    getDataObj.WorkOrderStatusIcon = lstWOStatus[0].WorkOrderStatus.Icon;

                    getDataObj.WorkOrderNote = lstWOStatus[0].Notes;
                    getDataObj.WorkOrderDate = DateTime.Parse(lstWOStatus[0].WorkOrderDate.ToString());




                    foreach (var note in lstWOStatus)
                    {
                        listNotes.Add(note.Notes);
                    }
                    getDataObj.ListWorkOrderNotes = string.Join(",", listNotes);




                    if (getDataObj.StatusId == 3 && (getDataObj.LatestWorkOrderStatusId == 2 && (lstWOStatus.Count > 2)))
                    {
                        getDataObj.WorkOrderNote = lstWOStatus[0].Notes;
                        getDataObj.WorkOrderDate = DateTime.Parse(lstWOStatus[0].WorkOrderDate.ToString());
                    }
                    else if (getDataObj.StatusId == 3 && (getDataObj.LatestWorkOrderStatusId == 2 && (lstWOStatus.Count == 2)))
                    {
                        foreach (var item in lstWOStatus.Where(a => a.WorkOrderStatusId == 1))
                        {
                            getDataObj.WorkOrderNote = item.Notes;
                            getDataObj.WorkOrderDate = DateTime.Parse(item.WorkOrderDate.ToString());
                        }
                    }
                }
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
            if (UserObj.HospitalId > 0)
            {
                if (userRoleNames.Contains("Admin"))
                {
                    list = list.ToList();
                }
                else if ((userRoleNames.Contains("AssetOwner") || userRoleNames.Contains("SRCreator")) && !userRoleNames.Contains("TLHospitalManager") && !userRoleNames.Contains("EngDepManager") && !userRoleNames.Contains("SRReviewer"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == UserObj.Id).ToList();
                }
                else if ((userRoleNames.Contains("AssetOwner") && userRoleNames.Contains("SRCreator")) && !userRoleNames.Contains("TLHospitalManager") && !userRoleNames.Contains("EngDepManager") && !userRoleNames.Contains("SRReviewer"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == UserObj.Id).ToList();
                }
                else if (userRoleNames.Contains("TLHospitalManager") && userRoleNames.Contains("EngDepManager") && userRoleNames.Contains("SRReviewer") && userRoleNames.Contains("SRCreator") && userRoleNames.Contains("AssetOwner") && userRoleNames.Contains("SRCreator"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                else if (userRoleNames.Contains("TLHospitalManager") && !userRoleNames.Contains("EngDepManager") && !userRoleNames.Contains("SRReviewer") && !userRoleNames.Contains("SRCreator") && !userRoleNames.Contains("AssetOwner") && !userRoleNames.Contains("SRCreator"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                else if (userRoleNames.Contains("TLHospitalManager") && userRoleNames.Contains("EngDepManager") && !userRoleNames.Contains("SRReviewer") && !userRoleNames.Contains("SRCreator") && !userRoleNames.Contains("AssetOwner") && !userRoleNames.Contains("SRCreator"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                else if (userRoleNames.Contains("TLHospitalManager") && userRoleNames.Contains("EngDepManager") && userRoleNames.Contains("SRReviewer") && !userRoleNames.Contains("SRCreator") && !userRoleNames.Contains("AssetOwner") && !userRoleNames.Contains("SRCreator"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                else if (userRoleNames.Contains("TLHospitalManager") && userRoleNames.Contains("EngDepManager") && userRoleNames.Contains("SRReviewer") && userRoleNames.Contains("SRCreator") && !userRoleNames.Contains("AssetOwner") && !userRoleNames.Contains("SRCreator"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                else if (userRoleNames.Contains("TLHospitalManager") && userRoleNames.Contains("EngDepManager") && userRoleNames.Contains("SRReviewer") && userRoleNames.Contains("SRCreator") && userRoleNames.Contains("AssetOwner") && !userRoleNames.Contains("SRCreator"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
            }



            return list;



        }
        public IndexRequestVM AlertOpenedRequestAssetsAndHighPeriority(int periorityId, int hospitalId, int pageNumber, int pageSize)
        {

            IndexRequestVM mainClass = new IndexRequestVM();
            List<IndexRequestVM.GetData> list = new List<IndexRequestVM.GetData>();
            var lstRequests = _context.Request
                        .Include(t => t.AssetDetail).Include(t => t.AssetDetail.Department)
                        .Include(t => t.AssetDetail.MasterAsset)
                         .OrderByDescending(a => a.RequestDate)
                         .Where(a => a.AssetDetail.HospitalId == hospitalId)

                         .ToList();

            foreach (var req in lstRequests)
            {
                var lstStatus = _context.RequestTracking.Include(t => t.Request).Where(a => a.RequestId == req.Id).ToList();
                if (lstStatus.Count == 1)
                {
                    foreach (var item in lstStatus)
                    {
                        IndexRequestVM.GetData getDataObj = new IndexRequestVM.GetData();
                        getDataObj.Id = req.Id;
                        getDataObj.RequestCode = req.RequestCode;
                        getDataObj.Barcode = req.AssetDetail.Barcode;
                        getDataObj.Subject = req.Subject;
                        getDataObj.RequestDate = req.RequestDate;
                        getDataObj.HospitalId = req.AssetDetail.HospitalId;
                        getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                        getDataObj.AssetName = req.AssetDetail.MasterAsset != null ? req.AssetDetail.MasterAsset.Name : "";
                        getDataObj.AssetNameAr = req.AssetDetail.MasterAsset != null ? req.AssetDetail.MasterAsset.NameAr : "";
                        getDataObj.DepartmentName = req.AssetDetail.Department != null ? req.AssetDetail.Department.Name : "";
                        getDataObj.DepartmentNameAr = req.AssetDetail.Department != null ? req.AssetDetail.Department.NameAr : "";
                        getDataObj.PeriorityId = (int)req.AssetDetail.MasterAsset.PeriorityId;

                        if (req.AssetDetail.MasterAsset.PeriorityId == 1)
                        {
                            getDataObj.BGColor = "#f95a5a";
                        }
                        if (req.AssetDetail.MasterAsset.PeriorityId == 2)
                        {
                            getDataObj.BGColor = "#f7ddae";
                        }
                        if (req.AssetDetail.MasterAsset.PeriorityId == 3)
                        {
                            getDataObj.BGColor = "#e8f2e2";
                        }
                        if (req.AssetDetail.MasterAsset.PeriorityId > 3)
                        {
                            getDataObj.BGColor = "#f7f7ae";
                        }
                        list.Add(getDataObj);
                    }
                }
            }


            if (periorityId != 0)
            {
                list = list.Where(a => a.PeriorityId == periorityId).ToList();
            }
            else
            {
                list = list.ToList();
            }
            var requestsPerPage = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            mainClass.Results = requestsPerPage;
            mainClass.Count = list.Count();
            return mainClass;
        }
        public List<IndexRequestVM.GetData> PrintListOfRequests(List<ExportRequestVM> requests)
        {
            List<IndexRequestVM.GetData> lstData = new List<IndexRequestVM.GetData>();

            foreach (var reqId in requests)
            {
                var list = _context.Request
                                .Include(a => a.RequestPeriority)
                                .Include(a => a.RequestMode)
                                .Include(a => a.User)
                                .Include(a => a.AssetDetail)
                                .Include(a => a.AssetDetail.Department)
                                .Include(a => a.AssetDetail.MasterAsset)
                                .Include(a => a.AssetDetail.Hospital)
                                .Where(a => a.Id == reqId.Id)
                                .ToList();

                foreach (var item in list)
                {
                    IndexRequestVM.GetData getDataObj = new IndexRequestVM.GetData();
                    getDataObj.Id = item.Id;
                    getDataObj.RequestCode = item.RequestCode;
                    getDataObj.Barcode = item.AssetDetail.Barcode;
                    getDataObj.Subject = item.Subject;
                    getDataObj.AssetHospitalId = item.HospitalId;
                    getDataObj.RequestDate = item.RequestDate;
                    getDataObj.HospitalId = item.AssetDetail.HospitalId;
                    getDataObj.CreatedById = item.CreatedById;
                    getDataObj.CreatedBy = item.User.UserName;
                    getDataObj.AssetDetailId = item.AssetDetailId;
                    getDataObj.MasterAssetId = item.AssetDetail.MasterAssetId;
                    getDataObj.SerialNumber = item.AssetDetail.SerialNumber;
                    getDataObj.Barcode = item.AssetDetail.Barcode;
                    getDataObj.ModelNumber = item.AssetDetail.MasterAsset.ModelNumber;

                    var lstStatus = _context.RequestTracking
                                .Include(t => t.Request).Include(t => t.RequestStatus)
                                .Where(a => a.RequestId == item.Id).ToList().OrderByDescending(a => a.DescriptionDate).ToList();
                    if (lstStatus.Count > 0)
                    {

                        getDataObj.StatusId = (int)lstStatus[0].RequestStatus.Id;
                        getDataObj.StatusName = lstStatus[0].RequestStatus.Name;
                        getDataObj.StatusNameAr = lstStatus[0].RequestStatus.NameAr;
                        getDataObj.StatusColor = lstStatus[0].RequestStatus.Color;
                        getDataObj.StatusIcon = lstStatus[0].RequestStatus.Icon;
                        getDataObj.Description = lstStatus[0].Description;
                        if (getDataObj.StatusId == 2)
                        {
                            getDataObj.ClosedDate = lstStatus[0].DescriptionDate.ToString();
                        }
                        else
                        {
                            getDataObj.ClosedDate = "";
                        }
                    }

                    if (item.AssetDetailId != null)
                    {
                        getDataObj.AssetDetailId = item.AssetDetailId != null ? (int)item.AssetDetailId : 0;
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

                    if (item.AssetDetail.DepartmentId != null)
                    {
                        getDataObj.DepartmentId = item.AssetDetail.Department.Id;
                        getDataObj.ModeName = item.AssetDetail.Department.Name;
                        getDataObj.ModeNameAr = item.AssetDetail.Department.NameAr;
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
                        StatusColor = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().Color,
                        StatusIcon = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().Icon,
                        Description = item.Description,
                        Date = item.DescriptionDate,
                        StatusId = item.RequestStatus.Id,
                        isExpanded = (_context.RequestDocument.Where(a => a.RequestTrackingId == item.Id).Count()) > 0 ? true : false,
                        ListDocuments = _context.RequestDocument.Where(a => a.RequestTrackingId == item.Id).ToList(),
                    }).ToList();
                    getDataObj.CountListTracks = _context.RequestTracking.Where(a => a.RequestId == item.Id).ToList().Count;
                    getDataObj.CountWorkOrder = _context.WorkOrders.Where(a => a.RequestId == item.Id).ToList().Count;

                    var lstWOStatus = _context.WorkOrderTrackings
                      .Include(o => o.WorkOrder).Include(o => o.WorkOrderStatus).Where(a => a.WorkOrder.RequestId == item.Id)
                      .OrderByDescending(a => a.CreationDate).ToList();

                    if (lstWOStatus.Count > 0)
                    {
                        getDataObj.LatestWorkOrderStatusId = lstWOStatus[0].WorkOrderStatusId;
                        getDataObj.WOLastTrackDescription = lstWOStatus[0].Notes;
                    }


                    lstData.Add(getDataObj);
                }
            }


            return lstData;
        }
        public IndexRequestVM SearchInRequests(SearchRequestVM searchObj, int pageNumber, int pageSize)
        {

            IndexRequestVM mainClass = new IndexRequestVM();
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
                            .Include(a => a.AssetDetail.Department)
                            .Include(a => a.AssetDetail.MasterAsset)
                             .Include(a => a.AssetDetail.MasterAsset.brand)
                            .Include(a => a.AssetDetail.Hospital)
                            .ToList();

            if (list.Count > 0)
            {
                if (searchObj.GovernorateId > 0 && searchObj.CityId == 0 && searchObj.HospitalId == 0 && searchObj.OrganizationId == 0 && searchObj.SubOrganizationId == 0)
                {
                    list = list.Where(t => t.AssetDetail.Hospital.GovernorateId == searchObj.GovernorateId).ToList();
                }
                else
                    list = list.ToList();

                if (searchObj.GovernorateId > 0 && searchObj.CityId > 0 && searchObj.HospitalId == 0 && searchObj.OrganizationId == 0 && searchObj.SubOrganizationId == 00)
                {
                    list = list.Where(t => t.AssetDetail.Hospital.CityId == searchObj.CityId).ToList();
                }
                else
                    list = list.ToList();

                if (searchObj.GovernorateId == 0 && searchObj.CityId == 0 && searchObj.HospitalId == 0 && searchObj.OrganizationId > 0 && searchObj.SubOrganizationId == 0)
                {
                    list = list.Where(t => t.AssetDetail.Hospital.OrganizationId == searchObj.OrganizationId).ToList();
                }
                else
                    list = list.ToList();


                if (searchObj.GovernorateId == 0 && searchObj.CityId == 0 && searchObj.HospitalId == 0 && searchObj.OrganizationId > 0 && searchObj.SubOrganizationId > 0)
                {
                    list = list.Where(t => t.AssetDetail.Hospital.SubOrganizationId == searchObj.SubOrganizationId).ToList();
                }
                else
                    list = list.ToList();


                if (searchObj.HospitalId > 0)
                {
                    if (userRoleNames.Contains("Admin"))
                    {
                        if (searchObj.HospitalId != 0)
                            list = list.Where(a => a.HospitalId == searchObj.HospitalId).ToList();
                        else
                            list = list.ToList();
                    }
                    else if ((userRoleNames.Contains("AssetOwner") || userRoleNames.Contains("SRCreator")) && !userRoleNames.Contains("TLHospitalManager") && !userRoleNames.Contains("EngDepManager") && !userRoleNames.Contains("SRReviewer"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == UserObj.Id).ToList();
                    }
                    else if ((userRoleNames.Contains("AssetOwner") && userRoleNames.Contains("SRCreator")) && !userRoleNames.Contains("TLHospitalManager") && !userRoleNames.Contains("EngDepManager") && !userRoleNames.Contains("SRReviewer"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == UserObj.Id).ToList();
                    }
                    else if (userRoleNames.Contains("TLHospitalManager") && userRoleNames.Contains("EngDepManager") && userRoleNames.Contains("SRReviewer") && userRoleNames.Contains("SRCreator") && userRoleNames.Contains("AssetOwner") && userRoleNames.Contains("SRCreator"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                    }
                    else if (userRoleNames.Contains("TLHospitalManager") && !userRoleNames.Contains("EngDepManager") && !userRoleNames.Contains("SRReviewer") && !userRoleNames.Contains("SRCreator") && !userRoleNames.Contains("AssetOwner") && !userRoleNames.Contains("SRCreator"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                    }
                    else if (userRoleNames.Contains("TLHospitalManager") && userRoleNames.Contains("EngDepManager") && !userRoleNames.Contains("SRReviewer") && !userRoleNames.Contains("SRCreator") && !userRoleNames.Contains("AssetOwner") && !userRoleNames.Contains("SRCreator"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                    }
                    else if (userRoleNames.Contains("TLHospitalManager") && userRoleNames.Contains("EngDepManager") && userRoleNames.Contains("SRReviewer") && !userRoleNames.Contains("SRCreator") && !userRoleNames.Contains("AssetOwner") && !userRoleNames.Contains("SRCreator"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                    }
                    else if (userRoleNames.Contains("TLHospitalManager") && userRoleNames.Contains("EngDepManager") && userRoleNames.Contains("SRReviewer") && userRoleNames.Contains("SRCreator") && !userRoleNames.Contains("AssetOwner") && !userRoleNames.Contains("SRCreator"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                    }
                    else if (userRoleNames.Contains("TLHospitalManager") && userRoleNames.Contains("EngDepManager") && userRoleNames.Contains("SRReviewer") && userRoleNames.Contains("SRCreator") && userRoleNames.Contains("AssetOwner") && !userRoleNames.Contains("SRCreator"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                    }
                }



                if (searchObj.PeriorityId != 0)
                {
                    list = list.Where(a => a.AssetDetail.MasterAsset.PeriorityId == searchObj.PeriorityId).ToList();
                }
                else
                    list = list.ToList();


                if (searchObj.ModeId != 0)
                {
                    list = list.Where(a => a.RequestModeId == searchObj.ModeId).ToList();
                }
                else
                    list = list.ToList();


                if (searchObj.MasterAssetId != 0)
                {
                    list = list.Where(a => a.AssetDetail.MasterAssetId == searchObj.MasterAssetId).ToList();
                }
                else
                    list = list.ToList();


                if (searchObj.DepartmentId != 0)
                {
                    list = list.Where(a => a.AssetDetail.DepartmentId == searchObj.DepartmentId).ToList();
                }
                else
                    list = list.ToList();



                if (searchObj.Barcode != "")
                {
                    list = list.Where(a => a.AssetDetail.Barcode == searchObj.Barcode).ToList();
                }
                else
                    list = list.ToList();

                if (searchObj.SerialNumber != "")
                {
                    list = list.Where(a => a.AssetDetail.SerialNumber.Contains(searchObj.SerialNumber)).ToList();
                }
                else
                    list = list.ToList();


                if (searchObj.ModelNumber != "")
                {
                    list = list.Where(a => a.AssetDetail.MasterAsset.ModelNumber.Contains(searchObj.ModelNumber)).ToList();
                }
                else
                    list = list.ToList();


                if (searchObj.Subject != "")
                {
                    list = list.Where(a => a.Subject.Contains(searchObj.Subject)).ToList();
                }
                else
                    list = list.ToList();

                if (searchObj.Code != "")
                {
                    list = list.Where(b => b.RequestCode == searchObj.Code).ToList();
                }
                else
                    list = list.ToList();


            }

            foreach (var item in list)
            {
                IndexRequestVM.GetData getDataObj = new IndexRequestVM.GetData();
                getDataObj.Id = item.Id;
                getDataObj.RequestCode = item.RequestCode;
                getDataObj.Barcode = item.AssetDetail.Barcode;
                getDataObj.Subject = item.Subject;
                getDataObj.AssetHospitalId = item.HospitalId;
                getDataObj.RequestDate = item.RequestDate;
                getDataObj.HospitalId = item.AssetDetail.HospitalId;
                getDataObj.CreatedById = item.CreatedById;
                getDataObj.CreatedBy = item.User.UserName;
                getDataObj.AssetDetailId = item.AssetDetailId;
                getDataObj.MasterAssetId = item.AssetDetail.MasterAssetId;
                getDataObj.SerialNumber = item.AssetDetail.SerialNumber;
                getDataObj.Barcode = item.AssetDetail.Barcode;
                getDataObj.ModelNumber = item.AssetDetail.MasterAsset.ModelNumber;

                var lstStatus = _context.RequestTracking
                            .Include(t => t.Request).Include(t => t.RequestStatus)
                            .Where(a => a.RequestId == item.Id).ToList().OrderByDescending(a => a.DescriptionDate).ToList();
                if (lstStatus.Count > 0)
                {

                    getDataObj.StatusId = (int)lstStatus[0].RequestStatus.Id;
                    getDataObj.StatusName = lstStatus[0].RequestStatus.Name;
                    getDataObj.StatusNameAr = lstStatus[0].RequestStatus.NameAr;
                    getDataObj.StatusColor = lstStatus[0].RequestStatus.Color;
                    getDataObj.StatusIcon = lstStatus[0].RequestStatus.Icon;
                    getDataObj.Description = lstStatus[0].Description;
                    if (getDataObj.StatusId == 2)
                    {
                        getDataObj.ClosedDate = lstStatus[0].DescriptionDate.ToString();
                    }
                    else
                    {
                        getDataObj.ClosedDate = "";
                    }
                }

                if (item.AssetDetailId != null)
                {
                    getDataObj.AssetDetailId = item.AssetDetailId != null ? (int)item.AssetDetailId : 0;
                }
                if (item.AssetDetail.MasterAsset != null)
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

                if (item.AssetDetail.DepartmentId != null)
                {
                    getDataObj.DepartmentId = item.AssetDetail.Department.Id;
                    getDataObj.ModeName = item.AssetDetail.Department.Name;
                    getDataObj.ModeNameAr = item.AssetDetail.Department.NameAr;
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
                    StatusColor = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().Color,
                    StatusIcon = _context.RequestStatus.Where(a => a.Id == item.RequestStatusId).First().Icon,
                    Description = item.Description,
                    Date = item.DescriptionDate,
                    StatusId = item.RequestStatus.Id,
                    isExpanded = (_context.RequestDocument.Where(a => a.RequestTrackingId == item.Id).Count()) > 0 ? true : false,
                    ListDocuments = _context.RequestDocument.Where(a => a.RequestTrackingId == item.Id).ToList(),
                }).ToList();
                getDataObj.CountListTracks = _context.RequestTracking.Where(a => a.RequestId == item.Id).ToList().Count;
                getDataObj.CountWorkOrder = _context.WorkOrders.Where(a => a.RequestId == item.Id).ToList().Count;

                var lstWOStatus = _context.WorkOrderTrackings
                  .Include(o => o.WorkOrder).Include(o => o.WorkOrderStatus).Where(a => a.WorkOrder.RequestId == item.Id)
                  .OrderByDescending(a => a.CreationDate).ToList();

                if (lstWOStatus.Count > 0)
                {
                    getDataObj.LatestWorkOrderStatusId = lstWOStatus[0].WorkOrderStatusId;
                    getDataObj.WOLastTrackDescription = lstWOStatus[0].Notes;
                }
                lstData.Add(getDataObj);
            }

            if (lstData.Count > 0)
            {

                if (searchObj.StatusId != 0)
                {
                    lstData = lstData.Where(a => a.StatusId == searchObj.StatusId).ToList();
                }
                else
                    lstData = lstData.ToList();



                string setstartday, setstartmonth, setendday, setendmonth = "";
                DateTime startingFrom = new DateTime();
                DateTime endingTo = new DateTime();
                if (searchObj.Start == "")
                {
                    searchObj.StartDate = DateTime.Parse("01/01/1900");
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
                    startingFrom = DateTime.Parse(sDate);//.AddDays(1);
                }

                if (searchObj.End == "")
                {
                    var utcDate = DateTime.UtcNow;
                    DateTime localDate = utcDate.ToLocalTime();
                    searchObj.EndDate = localDate.Date;
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
                    endingTo = DateTime.Parse(eDate);//.AddDays(1);
                }
                if (searchObj.Start != "" && searchObj.End != "")
                {
                    lstData = lstData.Where(a => a.RequestDate.Date >= startingFrom.Date && a.RequestDate.Date <= endingTo.Date).ToList();
                }
            }
            var requestsPerPage = lstData.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            mainClass.Results = requestsPerPage;
            mainClass.Count = lstData.Count();
            return mainClass;

        }
        public OpenRequestVM ListOpenRequests(SearchOpenRequestVM searchOpenRequestObj, int pageNumber, int pageSize)
        {
            OpenRequestVM mainClass = new OpenRequestVM();
            List<OpenRequestVM.GetData> list = new List<OpenRequestVM.GetData>();
            ApplicationUser UserObj = new ApplicationUser();
            ApplicationRole roleObj = new ApplicationRole();

            List<string> userRoleNames = new List<string>();
            var obj = _context.ApplicationUser.Where(a => a.Id == searchOpenRequestObj.UserId).ToList();
            if (obj.Count > 0)
            {
                UserObj = obj[0];

                var roles = (from userRole in _context.UserRoles
                             join role in _context.ApplicationRole on userRole.RoleId equals role.Id
                             where userRole.UserId == searchOpenRequestObj.UserId
                             select role);
                foreach (var role in roles)
                {
                    userRoleNames.Add(role.Name);
                }
            }

            var lstRequests = _context.RequestTracking
                                .Include(t => t.Request)
                                .Include(t => t.RequestStatus)
                                .Include(t => t.Request.AssetDetail.Hospital)
                                .Include(t => t.Request.AssetDetail.MasterAsset)
                               .Include(t => t.Request.AssetDetail.MasterAsset.brand)
                               .Include(t => t.User)
                               .Include(t => t.Request.RequestMode)
                               .Include(t => t.Request.RequestPeriority)
                               .OrderByDescending(a => a.DescriptionDate).ToList().GroupBy(a => a.RequestId).ToList();




            if (searchOpenRequestObj.HospitalId != 0)
                lstRequests = lstRequests.Where(a => a.FirstOrDefault().Request.HospitalId == searchOpenRequestObj.HospitalId).ToList();
            else
                lstRequests = lstRequests.ToList();



            if (searchOpenRequestObj.StrStartDate != "")
                searchOpenRequestObj.StartDate = DateTime.Parse(searchOpenRequestObj.StrStartDate);


            if (searchOpenRequestObj.StrEndDate != "")
                searchOpenRequestObj.EndDate = DateTime.Parse(searchOpenRequestObj.StrEndDate);




            if (searchOpenRequestObj.StartDate != null || searchOpenRequestObj.EndDate != null)
            {
                lstRequests = lstRequests.Where(a => a.FirstOrDefault().Request.RequestDate.Date >= searchOpenRequestObj.StartDate.Value.Date && a.FirstOrDefault().Request.RequestDate.Date <= searchOpenRequestObj.EndDate.Value.Date).ToList();
            }
            else
            {
                lstRequests = lstRequests.ToList();
            }



            foreach (var req in lstRequests)
            {

                if (req.FirstOrDefault().RequestStatusId != 2)
                {
                    OpenRequestVM.GetData getDataObj = new OpenRequestVM.GetData();
                    getDataObj.Id = req.FirstOrDefault().Request.Id;
                    getDataObj.CreatedById = req.FirstOrDefault().Request.User.Id;
                    getDataObj.CreatedBy = req.FirstOrDefault().Request.User.UserName;
                    getDataObj.FixCost = req.FirstOrDefault().Request.AssetDetail.FixCost;
                    getDataObj.CostPerDay = req.FirstOrDefault().Request.AssetDetail.FixCost != null ? Math.Round((((decimal)req.FirstOrDefault().Request.AssetDetail.FixCost) / 365)) : 0;
                    getDataObj.RequestCode = req.FirstOrDefault().Request.RequestCode;
                    getDataObj.Barcode = req.FirstOrDefault().Request.AssetDetail.Barcode;
                    getDataObj.ModelNumber = req.FirstOrDefault().Request.AssetDetail.MasterAsset.ModelNumber;
                    getDataObj.RequestDate = req.FirstOrDefault().Request.RequestDate;
                    getDataObj.SerialNumber = req.FirstOrDefault().Request.AssetDetail.SerialNumber;
                    getDataObj.AssetName = req.FirstOrDefault().Request.AssetDetail.MasterAsset.Name;
                    getDataObj.AssetNameAr = req.FirstOrDefault().Request.AssetDetail.MasterAsset.NameAr;

                    getDataObj.BrandName = req.FirstOrDefault().Request.AssetDetail.MasterAsset.brand != null ? req.FirstOrDefault().Request.AssetDetail.MasterAsset.brand.Name : "";
                    getDataObj.BrandNameAr = req.FirstOrDefault().Request.AssetDetail.MasterAsset.brand != null ? req.FirstOrDefault().Request.AssetDetail.MasterAsset.brand.NameAr : "";


                    getDataObj.AssetDetailId = req.FirstOrDefault().Request.AssetDetailId != null ? (int)req.FirstOrDefault().Request.AssetDetailId : 0;
                    getDataObj.HospitalId = req.FirstOrDefault().Request.AssetDetail.HospitalId;

                    var lstWOStatus = _context.WorkOrderTrackings
                   .Include(o => o.WorkOrder).Include(o => o.WorkOrderStatus).Where(a => a.WorkOrder.RequestId == req.FirstOrDefault().RequestId)
                   .OrderByDescending(a => a.CreationDate).ToList();

                    if (lstWOStatus.Count > 0)
                    {
                        if (lstWOStatus[0].WorkOrderStatusId < 11)
                        {
                            getDataObj.WorkOrderStatusId = lstWOStatus[0].WorkOrderStatusId;
                            getDataObj.WorkOrderStatusName = lstWOStatus[0].WorkOrderStatus != null ? lstWOStatus[0].WorkOrderStatus.Name : "";
                            getDataObj.WorkOrderStatusNameAr = lstWOStatus[0].WorkOrderStatus != null ? lstWOStatus[0].WorkOrderStatus.NameAr : "";
                            getDataObj.WorkOrderStatusColor = lstWOStatus[0].WorkOrderStatus != null ? lstWOStatus[0].WorkOrderStatus.Color : "";
                            if (searchOpenRequestObj.EndDate != null)
                            {
                                getDataObj.AllDays = Math.Round((DateTime.Parse(searchOpenRequestObj.EndDate.ToString()) - req.FirstOrDefault().Request.RequestDate.Date).TotalDays);
                            }
                            else
                            {
                                getDataObj.AllDays = 0;
                            }
                        }
                    }
                    else
                    {
                        if (searchOpenRequestObj.EndDate != null)
                        {

                            getDataObj.AllDays = Math.Round((DateTime.Parse(searchOpenRequestObj.EndDate.ToString()) - req.FirstOrDefault().Request.RequestDate.Date).TotalDays);

                        }
                        else
                        {
                            getDataObj.AllDays = 0;
                        }
                    }

                    getDataObj.BGColor = "";
                    if (getDataObj.CostPerDay != 0 && getDataObj.AllDays != null)
                        getDataObj.TotalCost = Math.Round((decimal)getDataObj.CostPerDay * (decimal)getDataObj.AllDays);
                    else
                        getDataObj.TotalCost = 0;

                    getDataObj.GovernorateId = req.FirstOrDefault().Request.User != null ? req.FirstOrDefault().Request.User.GovernorateId : 0;
                    getDataObj.CityId = req.FirstOrDefault().Request.User != null ? req.FirstOrDefault().Request.User.CityId : 0;
                    getDataObj.OrganizationId = req.FirstOrDefault().Request.User != null ? req.FirstOrDefault().Request.User.OrganizationId : 0;
                    getDataObj.SubOrganizationId = req.FirstOrDefault().Request.User != null ? req.FirstOrDefault().Request.User.SubOrganizationId : 0;

                    //var lstTransactions = _context.AssetStatusTransactions.Include(a => a.AssetDetail)
                    //                           .Include(a => a.AssetDetail.Hospital)
                    //                           .Where(a => (a.AssetStatusId == 8 || a.AssetStatusId == 9) && (a.AssetDetailId == req.FirstOrDefault().Request.AssetDetailId))
                    //                           .OrderByDescending(a => a.StatusDate).ToList().GroupBy(a => a.AssetDetailId).ToList();
                    //if (lstTransactions.Count > 0)
                    //{
                    //    getDataObj.BGColor = "#f7c2c2";
                    //}
                    //else
                    //{
                    //    getDataObj.BGColor = "";
                    //}

                    list.Add(getDataObj);
                }


            }

            if (searchOpenRequestObj.HospitalId != 0)
                list = list.Where(a => a.HospitalId == searchOpenRequestObj.HospitalId).ToList();
            else
                list = list.ToList();


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
            if (UserObj.HospitalId > 0)
            {
                if (userRoleNames.Contains("Admin"))
                {
                    list = list.ToList();
                }
                else if ((userRoleNames.Contains("AssetOwner") || userRoleNames.Contains("SRCreator")) && !userRoleNames.Contains("TLHospitalManager") && !userRoleNames.Contains("EngDepManager") && !userRoleNames.Contains("SRReviewer"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == UserObj.Id).ToList();
                }
                else if ((userRoleNames.Contains("AssetOwner") && userRoleNames.Contains("SRCreator")) && !userRoleNames.Contains("TLHospitalManager") && !userRoleNames.Contains("EngDepManager") && !userRoleNames.Contains("SRReviewer"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == UserObj.Id).ToList();
                }
                else if (userRoleNames.Contains("TLHospitalManager") && userRoleNames.Contains("EngDepManager") && userRoleNames.Contains("SRReviewer") && userRoleNames.Contains("SRCreator") && userRoleNames.Contains("AssetOwner") && userRoleNames.Contains("SRCreator"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                else if (userRoleNames.Contains("TLHospitalManager") && !userRoleNames.Contains("EngDepManager") && !userRoleNames.Contains("SRReviewer") && !userRoleNames.Contains("SRCreator") && !userRoleNames.Contains("AssetOwner") && !userRoleNames.Contains("SRCreator"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                else if (userRoleNames.Contains("TLHospitalManager") && userRoleNames.Contains("EngDepManager") && !userRoleNames.Contains("SRReviewer") && !userRoleNames.Contains("SRCreator") && !userRoleNames.Contains("AssetOwner") && !userRoleNames.Contains("SRCreator"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                else if (userRoleNames.Contains("TLHospitalManager") && userRoleNames.Contains("EngDepManager") && userRoleNames.Contains("SRReviewer") && !userRoleNames.Contains("SRCreator") && !userRoleNames.Contains("AssetOwner") && !userRoleNames.Contains("SRCreator"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                else if (userRoleNames.Contains("TLHospitalManager") && userRoleNames.Contains("EngDepManager") && userRoleNames.Contains("SRReviewer") && userRoleNames.Contains("SRCreator") && !userRoleNames.Contains("AssetOwner") && !userRoleNames.Contains("SRCreator"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                else if (userRoleNames.Contains("TLHospitalManager") && userRoleNames.Contains("EngDepManager") && userRoleNames.Contains("SRReviewer") && userRoleNames.Contains("SRCreator") && userRoleNames.Contains("AssetOwner") && !userRoleNames.Contains("SRCreator"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                else if (userRoleNames.Contains("AssetOwner") && userRoleNames.Contains("SRReviewer"))
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
            }
            //if (searchOpenRequestObj.StrStartDate != "")
            //    searchOpenRequestObj.StartDate = DateTime.Parse(searchOpenRequestObj.StrStartDate);
            //if (searchOpenRequestObj.StrEndDate != "")
            //    searchOpenRequestObj.EndDate = DateTime.Parse(searchOpenRequestObj.StrEndDate);

            //if (searchOpenRequestObj.StartDate != null || searchOpenRequestObj.EndDate != null)
            //{
            //    list = list.Where(a => a.RequestDate.Date >= searchOpenRequestObj.StartDate.Value.Date && a.RequestDate.Date <= searchOpenRequestObj.EndDate.Value.Date).ToList();
            //}
            //else
            //{
            //    list = list.ToList();
            //}

            var requestsPerPage = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            mainClass.Results = requestsPerPage;
            mainClass.Count = list.Count();
            return mainClass;
        }
        public List<OpenRequestVM.GetData> ListOpenRequestsPDF(SearchOpenRequestVM searchOpenRequestObj)
        {
            List<OpenRequestVM.GetData> list = new List<OpenRequestVM.GetData>();


            var lstRequests = _context.RequestTracking
                                .Include(t => t.Request)
                                .Include(t => t.RequestStatus)
                                .Include(t => t.Request.AssetDetail.Hospital)
                                .Include(t => t.Request.AssetDetail.MasterAsset)
                               .Include(t => t.Request.AssetDetail.MasterAsset.brand)
                               .Include(t => t.User)
                               .Include(t => t.Request.RequestMode)
                               .Include(t => t.Request.RequestPeriority)
                               .OrderByDescending(a => a.DescriptionDate).ToList().GroupBy(a => a.RequestId).ToList();


            if (searchOpenRequestObj.HospitalId != 0)
                lstRequests = lstRequests.Where(a => a.FirstOrDefault().Request.HospitalId == searchOpenRequestObj.HospitalId).ToList();
            else
                lstRequests = lstRequests.ToList();




            foreach (var req in lstRequests)
            {

                if (req.FirstOrDefault().RequestStatusId != 2)
                {
                    OpenRequestVM.GetData getDataObj = new OpenRequestVM.GetData();
                    getDataObj.Id = req.FirstOrDefault().Request.Id;
                    getDataObj.CreatedById = req.FirstOrDefault().Request.User.Id;
                    getDataObj.CreatedBy = req.FirstOrDefault().Request.User.UserName;
                    getDataObj.FixCost = req.FirstOrDefault().Request.AssetDetail.FixCost;
                    getDataObj.CostPerDay = req.FirstOrDefault().Request.AssetDetail.FixCost != null ? Math.Round((((decimal)req.FirstOrDefault().Request.AssetDetail.FixCost) / 365)) : 0;
                    getDataObj.RequestCode = req.FirstOrDefault().Request.RequestCode;
                    getDataObj.Barcode = req.FirstOrDefault().Request.AssetDetail.Barcode;
                    getDataObj.ModelNumber = req.FirstOrDefault().Request.AssetDetail.MasterAsset.ModelNumber;
                    getDataObj.RequestDate = req.FirstOrDefault().Request.RequestDate;
                    getDataObj.SerialNumber = req.FirstOrDefault().Request.AssetDetail.SerialNumber;
                    getDataObj.AssetName = req.FirstOrDefault().Request.AssetDetail.MasterAsset.Name;
                    getDataObj.AssetNameAr = req.FirstOrDefault().Request.AssetDetail.MasterAsset.NameAr;

                    getDataObj.BrandName = req.FirstOrDefault().Request.AssetDetail.MasterAsset.brand != null ? req.FirstOrDefault().Request.AssetDetail.MasterAsset.brand.Name : "";
                    getDataObj.BrandNameAr = req.FirstOrDefault().Request.AssetDetail.MasterAsset.brand != null ? req.FirstOrDefault().Request.AssetDetail.MasterAsset.brand.NameAr : "";


                    getDataObj.AssetDetailId = req.FirstOrDefault().Request.AssetDetailId != null ? (int)req.FirstOrDefault().Request.AssetDetailId : 0;
                    getDataObj.HospitalId = req.FirstOrDefault().Request.AssetDetail.HospitalId;

                    var lstWOStatus = _context.WorkOrderTrackings
                   .Include(o => o.WorkOrder).Include(o => o.WorkOrderStatus).Where(a => a.WorkOrder.RequestId == req.FirstOrDefault().RequestId)
                   .OrderByDescending(a => a.CreationDate).ToList();

                    if (lstWOStatus.Count > 0)
                    {

                        if (lstWOStatus[0].WorkOrderStatusId < 11)
                        {
                            getDataObj.WorkOrderStatusId = lstWOStatus[0].WorkOrderStatusId;
                            getDataObj.WorkOrderStatusName = lstWOStatus[0].WorkOrderStatus != null ? lstWOStatus[0].WorkOrderStatus.Name : "";
                            getDataObj.WorkOrderStatusNameAr = lstWOStatus[0].WorkOrderStatus != null ? lstWOStatus[0].WorkOrderStatus.NameAr : "";
                            getDataObj.WorkOrderStatusColor = lstWOStatus[0].WorkOrderStatus != null ? lstWOStatus[0].WorkOrderStatus.Color : "";
                            getDataObj.LastWOFixedDate = lstWOStatus[0].CreationDate;

                            if (searchOpenRequestObj.EndDate != null)
                            {
                                getDataObj.AllDays = Math.Round((DateTime.Parse(searchOpenRequestObj.StrEndDate.ToString()) - req.FirstOrDefault().Request.RequestDate.Date).TotalDays);
                            }
                            else
                            {
                                getDataObj.AllDays = 0;
                            }
                        }
                        else
                        {
                            if (searchOpenRequestObj.EndDate != null)
                            {
                                getDataObj.AllDays = Math.Round((DateTime.Parse(searchOpenRequestObj.StrEndDate.ToString()) - req.FirstOrDefault().Request.RequestDate.Date).TotalDays);
                            }
                            else
                            {
                                getDataObj.AllDays = 0;
                            }
                        }
                    }
                    else
                    {
                        if (searchOpenRequestObj.EndDate != null)
                        {
                            getDataObj.AllDays = Math.Round((DateTime.Parse(searchOpenRequestObj.StrEndDate.ToString()) - req.FirstOrDefault().Request.RequestDate.Date).TotalDays);
                        }
                        else
                        {
                            getDataObj.AllDays = 0;
                        }
                    }
                    if (getDataObj.CostPerDay != 0 && getDataObj.AllDays != null)
                        getDataObj.TotalCost = Math.Round((decimal)getDataObj.CostPerDay * (decimal)getDataObj.AllDays);
                    else
                        getDataObj.TotalCost = 0;

                    getDataObj.GovernorateId = req.FirstOrDefault().Request.User != null ? req.FirstOrDefault().Request.User.GovernorateId : 0;
                    getDataObj.CityId = req.FirstOrDefault().Request.User != null ? req.FirstOrDefault().Request.User.CityId : 0;
                    getDataObj.OrganizationId = req.FirstOrDefault().Request.User != null ? req.FirstOrDefault().Request.User.OrganizationId : 0;
                    getDataObj.SubOrganizationId = req.FirstOrDefault().Request.User != null ? req.FirstOrDefault().Request.User.SubOrganizationId : 0;



                    var lstTransactions = _context.AssetStatusTransactions.Include(a => a.AssetDetail)
                                                 .Include(a => a.AssetDetail.Hospital)
                                                .Where(a => (a.AssetStatusId == 8 || a.AssetStatusId == 9) && (a.AssetDetailId == req.FirstOrDefault().Request.AssetDetailId))
                                                 .OrderByDescending(a => a.StatusDate).ToList().GroupBy(a => a.AssetDetailId).ToList();
                    if (lstTransactions.Count > 0)
                    {
                        getDataObj.BGColor = "#f7c2c2";
                    }
                    list.Add(getDataObj);
                }
            }



            if (searchOpenRequestObj.StrStartDate != "")
                searchOpenRequestObj.StartDate = DateTime.Parse(searchOpenRequestObj.StrStartDate);


            if (searchOpenRequestObj.StrEndDate != "")
                searchOpenRequestObj.EndDate = DateTime.Parse(searchOpenRequestObj.StrEndDate);




            if (searchOpenRequestObj.StartDate != null || searchOpenRequestObj.EndDate != null)
            {
                list = list.Where(a => a.RequestDate.Date >= searchOpenRequestObj.StartDate.Value.Date && a.RequestDate.Date <= searchOpenRequestObj.EndDate.Value.Date).ToList();
            }
            else
            {
                list = list.ToList();
            }

            return list;
        }






        public IndexRequestVM GetAllRequestsByStatusIdAndPaging2(string userId, int statusId, int pageNumber, int pageSize)
        {
            IndexRequestVM mainClass = new IndexRequestVM();
            List<IndexRequestVM.GetData> lstModel = new List<IndexRequestVM.GetData>();
            ApplicationUser UserObj = new ApplicationUser();
            List<string> lstRoleNames = new List<string>();

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
                    lstRoleNames.Add(name.Name);
                }
            }

            IQueryable<Request> requests = _context.Request
                                        .Include(a => a.AssetDetail)
                                        .Include(a => a.AssetDetail.Hospital)
                                        .Include(a => a.AssetDetail.Hospital.Governorate)
                                        .Include(a => a.AssetDetail.Hospital.City)
                                        .Include(a => a.AssetDetail.Hospital.Organization)
                                        .Include(a => a.AssetDetail.Hospital.SubOrganization)
                                        .Include(a => a.User)
                                        .Include(a => a.RequestMode)
                                        .Include(a => a.RequestPeriority)
                                        .Include(a => a.RequestType)
                                        .Include(a => a.AssetDetail.MasterAsset)
                                        .OrderByDescending(a => a.RequestDate)
                                        .AsQueryable();



            if (UserObj.GovernorateId == 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
            {
                requests = requests.AsQueryable();
            }
            if (UserObj.GovernorateId > 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
            {
                requests = requests.Where(t => t.AssetDetail.Hospital.GovernorateId == UserObj.GovernorateId).AsQueryable();
            }
            if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId == 0)
            {
                requests = requests.Where(t => t.AssetDetail.Hospital.CityId == UserObj.CityId).AsQueryable();
            }
            if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId == 0 && UserObj.HospitalId == 0)
            {
                requests = requests.Where(t => t.AssetDetail.Hospital.OrganizationId == UserObj.OrganizationId).AsQueryable();
            }
            if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId == 0)
            {
                requests = requests.Where(t => t.AssetDetail.Hospital.SubOrganizationId == UserObj.SubOrganizationId).AsQueryable();
            }
            if (UserObj.HospitalId > 0)
            {
                if (lstRoleNames.Contains("Admin"))
                {
                    requests = requests.AsQueryable();
                }
                else if ((lstRoleNames.Contains("AssetOwner") || lstRoleNames.Contains("SRCreator")) && !lstRoleNames.Contains("TLHospitalManager") && !lstRoleNames.Contains("EngDepManager") && !lstRoleNames.Contains("SRReviewer"))
                {
                    requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId && t.CreatedById == userId).AsQueryable();
                }
                else if (lstRoleNames.Contains("AssetOwner") && lstRoleNames.Contains("SRReviewer"))
                {
                    requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).AsQueryable();
                }
                else if ((lstRoleNames.Contains("AssetOwner") && lstRoleNames.Contains("SRCreator")) && !lstRoleNames.Contains("TLHospitalManager") && !lstRoleNames.Contains("EngDepManager") && !lstRoleNames.Contains("SRReviewer"))
                {
                    requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId && t.CreatedById == userId).AsQueryable();
                }
                else if (lstRoleNames.Contains("TLHospitalManager") && lstRoleNames.Contains("EngDepManager") && lstRoleNames.Contains("SRReviewer") && lstRoleNames.Contains("SRCreator") && lstRoleNames.Contains("AssetOwner") && lstRoleNames.Contains("SRCreator"))
                {
                    requests = requests.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId).AsQueryable();
                }
                else if (lstRoleNames.Contains("TLHospitalManager") && !lstRoleNames.Contains("EngDepManager") && !lstRoleNames.Contains("SRReviewer") && !lstRoleNames.Contains("SRCreator") && !lstRoleNames.Contains("AssetOwner") && !lstRoleNames.Contains("SRCreator"))
                {
                    requests = requests.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId).AsQueryable();
                }
                else if (lstRoleNames.Contains("TLHospitalManager") && lstRoleNames.Contains("EngDepManager") && !lstRoleNames.Contains("SRReviewer") && !lstRoleNames.Contains("SRCreator") && !lstRoleNames.Contains("AssetOwner") && !lstRoleNames.Contains("SRCreator"))
                {
                    requests = requests.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId).AsQueryable();
                }
                else if (lstRoleNames.Contains("TLHospitalManager") && lstRoleNames.Contains("EngDepManager") && lstRoleNames.Contains("SRReviewer") && !lstRoleNames.Contains("SRCreator") && !lstRoleNames.Contains("AssetOwner") && !lstRoleNames.Contains("SRCreator"))
                {
                    requests = requests.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId).AsQueryable();
                }
                else if (lstRoleNames.Contains("TLHospitalManager") && lstRoleNames.Contains("EngDepManager") && lstRoleNames.Contains("SRReviewer") && lstRoleNames.Contains("SRCreator") && !lstRoleNames.Contains("AssetOwner") && !lstRoleNames.Contains("SRCreator"))
                {
                    requests = requests.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId).AsQueryable();
                }
                else if (lstRoleNames.Contains("TLHospitalManager") && lstRoleNames.Contains("EngDepManager") && lstRoleNames.Contains("SRReviewer") && lstRoleNames.Contains("SRCreator") && lstRoleNames.Contains("AssetOwner") && !lstRoleNames.Contains("SRCreator"))
                {
                    requests = requests.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId).AsQueryable();
                }

            }


            var allrequests = requests.ToList();

            if (allrequests.Count() > 0)
            {
                IQueryable<RequestTracking> lstQueryTracks = null;
                List<RequestTracking> lstTracks = new List<RequestTracking>();
                foreach (var req in allrequests)
                {

                    lstQueryTracks = _context.RequestTracking.Include(a => a.Request).Include(a => a.User).Include(a => a.RequestStatus)
                                                        .Where(a => a.RequestId == req.Id).OrderByDescending(a => a.DescriptionDate).AsQueryable();

                    lstTracks = lstQueryTracks.ToList();



                    if (lstTracks.Count > 0)
                    {
                        IndexRequestVM.GetData getDataObj = new IndexRequestVM.GetData();
                        var requestStatusId = lstTracks[0].RequestStatusId;
                        if (statusId > 0)
                        {
                            if (requestStatusId == statusId)
                            {
                                getDataObj.RequestId = req.Id;
                                getDataObj.Id = req.Id;
                                getDataObj.RequestCode = req.RequestCode;
                                getDataObj.Barcode = req.AssetDetail.Barcode;
                                getDataObj.CreatedById = req.CreatedById;
                                getDataObj.CreatedBy = lstTracks[0].User != null ? lstTracks[0].User.UserName : "";
                                getDataObj.Subject = req.Subject;
                                getDataObj.RequestDate = req.RequestDate;
                                getDataObj.AssetDetailId = req.AssetDetailId != null ? (int)req.AssetDetailId : 0;
                                getDataObj.HospitalId = req.AssetDetail.HospitalId;
                                getDataObj.StatusId = (int)lstTracks[0].RequestStatusId;
                                getDataObj.StatusName = lstTracks[0].RequestStatus.Name;
                                getDataObj.StatusNameAr = lstTracks[0].RequestStatus.NameAr;
                                getDataObj.StatusColor = lstTracks[0].RequestStatus.Color;
                                getDataObj.StatusIcon = lstTracks[0].RequestStatus.Icon;
                                getDataObj.Description = lstTracks[0].Description;
                                if (int.Parse(lstTracks[0].RequestStatusId.ToString()) == 2)
                                {
                                    getDataObj.ClosedDate = lstTracks[0].DescriptionDate.ToString();
                                }
                                else
                                {
                                    getDataObj.ClosedDate = "";
                                }
                                getDataObj.Barcode = req.AssetDetail.Barcode;
                                getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                                getDataObj.ModelNumber = req.AssetDetail.MasterAsset.ModelNumber;
                                getDataObj.ModeId = req.RequestModeId != null ? (int)req.RequestModeId : 0;
                                getDataObj.ModeName = req.RequestMode != null ? req.RequestMode.Name : "";
                                getDataObj.ModeNameAr = req.RequestMode != null ? req.RequestMode.NameAr : "";
                                getDataObj.PeriorityId = req.RequestPeriorityId != null ? (int)req.RequestPeriorityId : 0;
                                getDataObj.PeriorityName = req.RequestPeriority != null ? req.RequestPeriority.Name : "";
                                getDataObj.PeriorityNameAr = req.RequestPeriority != null ? req.RequestPeriority.NameAr : "";
                                getDataObj.PeriorityColor = req.RequestPeriority != null ? req.RequestPeriority.Color : "";
                                getDataObj.PeriorityIcon = req.RequestPeriority != null ? req.RequestPeriority.Icon : "";
                                getDataObj.AssetHospitalId = req.HospitalId;
                                getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                                getDataObj.Barcode = req.AssetDetail.Barcode;
                                getDataObj.AssetName = req.AssetDetail.MasterAsset.Name;
                                getDataObj.AssetNameAr = req.AssetDetail.MasterAsset.NameAr;
                                getDataObj.ListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id)
                                   .ToList().Select(item => new IndexRequestTrackingVM.GetData
                                   {
                                       Id = item.Id,
                                       StatusName = item.RequestStatusId != null ? lstTracks[0].RequestStatus.Name : "",
                                       StatusNameAr = item.RequestStatusId != null ? lstTracks[0].RequestStatus.NameAr : "",
                                       StatusColor = item.RequestStatusId != null ? lstTracks[0].RequestStatus.Color : "",
                                       StatusIcon = item.RequestStatusId != null ? lstTracks[0].RequestStatus.Icon : "",
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
                                    getDataObj.WOLastTrackDescription = lstWOStatus[0].Notes;
                                }
                                getDataObj.CountListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id).ToList().Count;
                                getDataObj.CountWorkOrder = _context.WorkOrders.Where(a => a.RequestId == req.Id).ToList().Count > 0 ? _context.WorkOrders.Where(a => a.RequestId == req.Id).ToList().Count : 0;
                                getDataObj.GovernorateId = req.User != null ? req.User.GovernorateId : 0;
                                getDataObj.CityId = req.User != null ? req.User.CityId : 0;
                                getDataObj.OrganizationId = req.User != null ? req.User.OrganizationId : 0;
                                getDataObj.SubOrganizationId = req.User != null ? req.User.SubOrganizationId : 0;
                                lstModel.Add(getDataObj);
                            }
                        }
                        else
                        {

                            getDataObj.RequestId = req.Id;
                            getDataObj.Id = req.Id;
                            getDataObj.RequestCode = req.RequestCode;
                            getDataObj.Barcode = req.AssetDetail.Barcode;
                            getDataObj.CreatedById = req.CreatedById;
                            getDataObj.CreatedBy = lstTracks[0].User != null ? lstTracks[0].User.UserName : "";
                            getDataObj.Subject = req.Subject;
                            getDataObj.RequestDate = req.RequestDate;
                            getDataObj.AssetDetailId = req.AssetDetailId != null ? (int)req.AssetDetailId : 0;
                            getDataObj.HospitalId = req.AssetDetail.HospitalId;
                            getDataObj.StatusId = (int)lstTracks[0].RequestStatusId;
                            getDataObj.StatusName = lstTracks[0].RequestStatus.Name;
                            getDataObj.StatusNameAr = lstTracks[0].RequestStatus.NameAr;
                            getDataObj.StatusColor = lstTracks[0].RequestStatus.Color;
                            getDataObj.StatusIcon = lstTracks[0].RequestStatus.Icon;
                            getDataObj.Description = lstTracks[0].Description;
                            if (int.Parse(lstTracks[0].RequestStatusId.ToString()) == 2)
                            {
                                getDataObj.ClosedDate = lstTracks[0].DescriptionDate.ToString();
                            }
                            else
                            {
                                getDataObj.ClosedDate = "";
                            }
                            getDataObj.Barcode = req.AssetDetail.Barcode;
                            getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                            getDataObj.ModelNumber = req.AssetDetail.MasterAsset.ModelNumber;
                            getDataObj.ModeId = req.RequestModeId != null ? (int)req.RequestModeId : 0;
                            getDataObj.ModeName = req.RequestMode != null ? req.RequestMode.Name : "";
                            getDataObj.ModeNameAr = req.RequestMode != null ? req.RequestMode.NameAr : "";
                            getDataObj.PeriorityId = req.RequestPeriorityId != null ? (int)req.RequestPeriorityId : 0;
                            getDataObj.PeriorityName = req.RequestPeriority != null ? req.RequestPeriority.Name : "";
                            getDataObj.PeriorityNameAr = req.RequestPeriority != null ? req.RequestPeriority.NameAr : "";
                            getDataObj.PeriorityColor = req.RequestPeriority != null ? req.RequestPeriority.Color : "";
                            getDataObj.PeriorityIcon = req.RequestPeriority != null ? req.RequestPeriority.Icon : "";
                            getDataObj.AssetHospitalId = req.HospitalId;
                            getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                            getDataObj.Barcode = req.AssetDetail.Barcode;
                            getDataObj.AssetName = req.AssetDetail.MasterAsset.Name;
                            getDataObj.AssetNameAr = req.AssetDetail.MasterAsset.NameAr;
                            getDataObj.ListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id)
                               .ToList().Select(item => new IndexRequestTrackingVM.GetData
                               {
                                   Id = item.Id,
                                   StatusName = item.RequestStatusId != null ? lstTracks[0].RequestStatus.Name : "",
                                   StatusNameAr = item.RequestStatusId != null ? lstTracks[0].RequestStatus.NameAr : "",
                                   StatusColor = item.RequestStatusId != null ? lstTracks[0].RequestStatus.Color : "",
                                   StatusIcon = item.RequestStatusId != null ? lstTracks[0].RequestStatus.Icon : "",
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
                                getDataObj.WOLastTrackDescription = lstWOStatus[0].Notes;
                            }
                            getDataObj.CountListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id).ToList().Count;
                            getDataObj.CountWorkOrder = _context.WorkOrders.Where(a => a.RequestId == req.Id).ToList().Count > 0 ? _context.WorkOrders.Where(a => a.RequestId == req.Id).ToList().Count : 0;
                            getDataObj.GovernorateId = req.User != null ? req.User.GovernorateId : 0;
                            getDataObj.CityId = req.User != null ? req.User.CityId : 0;
                            getDataObj.OrganizationId = req.User != null ? req.User.OrganizationId : 0;
                            getDataObj.SubOrganizationId = req.User != null ? req.User.SubOrganizationId : 0;
                            lstModel.Add(getDataObj);
                        }

                    }
                    var requestsPerPage = lstModel.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                    mainClass.Results = requestsPerPage;
                    mainClass.Count = lstModel.Count;

                }
            }

            return mainClass;
        }


        public IndexRequestVM GetRequestsByStatusIdAndPagingV2(string userId, int statusId, int pageNumber, int pageSize)
        {


            var LastTrackingForeachRequest =
                from rt1 in _context.RequestTracking
                join Subquery in
                (from rt2 in _context.RequestTracking
                 group rt2 by rt2.RequestId into g
                 select new { RequestId = g.Key, DescriptionDate = g.Max(x => x.DescriptionDate) }
                )
                on new { rt1.RequestId, rt1.DescriptionDate }
                equals new { Subquery.RequestId, Subquery.DescriptionDate }
                select rt1;


            IndexRequestVM mainClass = new();
            List<IndexRequestVM.GetData> lstModel = new List<IndexRequestVM.GetData>();
            // identity part
            ApplicationUser UserObj = new ApplicationUser();
            List<string> lstRoleNames = new List<string>();
            Func<Request, bool> IsEligble = null; // func delegate to pass function as a parameter
            var obj = _context.ApplicationUser.Where(a => a.Id == userId).ToList();
            // fetch UserRoles to Filter Data base on it
            if (obj.Count > 0)
            {
                UserObj = obj[0];
                var roleNames = (from userRole in _context.UserRoles
                                 join role in _context.Roles on userRole.RoleId equals role.Id
                                 where userRole.UserId == userId
                                 select role);
                foreach (var name in roleNames)
                {
                    lstRoleNames.Add(name.Name);
                }

            }
            // Fill The Func Delegate With The Filteration Condition 

            if (UserObj.GovernorateId == 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
            {
                IsEligble = null;
            }

            if (UserObj.GovernorateId > 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
            {
                IsEligble = t => t.AssetDetail.Hospital.GovernorateId == UserObj.GovernorateId;
            }

            if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId == 0)
            {
                IsEligble = t => t.AssetDetail.Hospital.CityId == UserObj.CityId;
            }

            if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId == 0 && UserObj.HospitalId == 0)
            {
                IsEligble = t => t.AssetDetail.Hospital.OrganizationId == UserObj.OrganizationId;
            }
            if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId == 0)
            {
                IsEligble = t => t.AssetDetail.Hospital.SubOrganizationId == UserObj.SubOrganizationId;
            }


            if (UserObj.HospitalId > 0)
            {
                //if (lstRoleNames.Contains("Admin"))
                //{
                //    IsEligble = null;
                //}
                //if (lstRoleNames.Contains("TLHospitalManager"))
                //{
                //    IsEligble = t => t.HospitalId == UserObj.HospitalId;
                //}
                //if (lstRoleNames.Contains("EngDepManager"))
                //{
                //    IsEligble = t => t.HospitalId == UserObj.HospitalId;
                //}
                //if (lstRoleNames.Contains("EngManager"))
                //{
                //    IsEligble = t => t.HospitalId == UserObj.HospitalId;
                //}
                //if (lstRoleNames.Contains("AssetOwner"))
                //{
                //    IsEligble = t => t.AssetDetail.Hospital.Id == UserObj.HospitalId && t.CreatedById == userId;
                //}
                //if (lstRoleNames.Contains("AssetOwner") && lstRoleNames.Contains("SRReviewer"))
                //{
                //    IsEligble = t => t.AssetDetail.Hospital.Id == UserObj.HospitalId;
                //}
                //if (lstRoleNames.Contains("Eng"))
                //{
                //    IsEligble = t => t.HospitalId == UserObj.HospitalId;
                //}


                if (lstRoleNames.Contains("Admin"))
                {
                    IsEligble = null;
                }
                else if ((lstRoleNames.Contains("AssetOwner") || lstRoleNames.Contains("SRCreator")) && !lstRoleNames.Contains("TLHospitalManager") && !lstRoleNames.Contains("EngDepManager") && !lstRoleNames.Contains("SRReviewer"))
                {
                    IsEligble = t => t.AssetDetail.Hospital.Id == UserObj.HospitalId && t.CreatedById == userId;
                }
                else if ((lstRoleNames.Contains("AssetOwner") && lstRoleNames.Contains("SRCreator")) && !lstRoleNames.Contains("TLHospitalManager") && !lstRoleNames.Contains("EngDepManager") && !lstRoleNames.Contains("SRReviewer"))
                {
                    IsEligble = t => t.AssetDetail.Hospital.Id == UserObj.HospitalId && t.CreatedById == userId;
                }
                else if (lstRoleNames.Contains("TLHospitalManager") && lstRoleNames.Contains("EngDepManager") && lstRoleNames.Contains("SRReviewer") && lstRoleNames.Contains("SRCreator") && lstRoleNames.Contains("AssetOwner") && lstRoleNames.Contains("SRCreator"))
                {
                    IsEligble = t => t.AssetDetail.HospitalId == UserObj.HospitalId;
                }
                else if (lstRoleNames.Contains("TLHospitalManager") && !lstRoleNames.Contains("EngDepManager") && !lstRoleNames.Contains("SRReviewer") && !lstRoleNames.Contains("SRCreator") && !lstRoleNames.Contains("AssetOwner") && !lstRoleNames.Contains("SRCreator"))
                {
                    IsEligble = t => t.AssetDetail.HospitalId == UserObj.HospitalId;
                }
                else if (lstRoleNames.Contains("TLHospitalManager") && lstRoleNames.Contains("EngDepManager") && !lstRoleNames.Contains("SRReviewer") && !lstRoleNames.Contains("SRCreator") && !lstRoleNames.Contains("AssetOwner") && !lstRoleNames.Contains("SRCreator"))
                {
                    IsEligble = t => t.AssetDetail.HospitalId == UserObj.HospitalId;
                }
                else if (lstRoleNames.Contains("TLHospitalManager") && lstRoleNames.Contains("EngDepManager") && lstRoleNames.Contains("SRReviewer") && !lstRoleNames.Contains("SRCreator") && !lstRoleNames.Contains("AssetOwner") && !lstRoleNames.Contains("SRCreator"))
                {
                    IsEligble = t => t.AssetDetail.HospitalId == UserObj.HospitalId;
                }
                else if (lstRoleNames.Contains("TLHospitalManager") && lstRoleNames.Contains("EngDepManager") && lstRoleNames.Contains("SRReviewer") && lstRoleNames.Contains("SRCreator") && !lstRoleNames.Contains("AssetOwner") && !lstRoleNames.Contains("SRCreator"))
                {
                    IsEligble = t => t.AssetDetail.HospitalId == UserObj.HospitalId;
                }
                else if (lstRoleNames.Contains("TLHospitalManager") && lstRoleNames.Contains("EngDepManager") && lstRoleNames.Contains("SRReviewer") && lstRoleNames.Contains("SRCreator") && lstRoleNames.Contains("AssetOwner") && !lstRoleNames.Contains("SRCreator"))
                {
                    IsEligble = t => t.AssetDetail.HospitalId == UserObj.HospitalId;
                }
            }

            if (statusId > 0)
            {
                if (IsEligble is not null)
                {
                    //mainClass.Count = LastTrackingForeachRequest.Where(a => a.RequestStatusId == statusId).Count();
                    var lastTrackOfRequestByStatus = LastTrackingForeachRequest.Where(a => a.RequestStatusId == statusId).ToList();
                    var ids = lastTrackOfRequestByStatus.Select(a => a.RequestId).ToList();

                    var requests = _context.Request
                                       .Include(a => a.AssetDetail)
                                       .Include(a => a.AssetDetail.Hospital)
                                       .Include(a => a.AssetDetail.Hospital.Governorate)
                                       .Include(a => a.AssetDetail.Hospital.City)
                                       .Include(a => a.AssetDetail.Hospital.Organization)
                                       .Include(a => a.AssetDetail.Hospital.SubOrganization)
                                       .Include(a => a.AssetDetail.Hospital.SubOrganization)
                                       .Include(a => a.User)
                                       .Include(a => a.RequestMode)
                                       .Include(a => a.RequestPeriority)
                                       .Include(a => a.RequestType)
                                       .Include(a => a.AssetDetail.MasterAsset)
                                       .OrderByDescending(a => a.RequestDate)
                                       .AsNoTracking()
                                       .Where(IsEligble).ToList();
                    requests = requests.Where(a => ids.Contains(a.Id)).ToList();
                    mainClass.Count = requests.Count;
                    requests = requests.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();



                    if (requests.Count > 0)
                    {
                        foreach (var req in requests)
                        {
                            var lstTracks = _context.RequestTracking.Include(a => a.RequestStatus).Where(a => a.RequestId == req.Id).OrderByDescending(a => a.DescriptionDate).ToList();
                            if (lstTracks.Count > 0)
                            {
                                var requestStatusId = lstTracks[0].RequestStatusId;
                                if (requestStatusId == statusId)
                                {
                                    IndexRequestVM.GetData getDataObj = new IndexRequestVM.GetData();
                                    getDataObj.RequestId = req.Id;
                                    getDataObj.Id = req.Id;
                                    getDataObj.RequestCode = req.RequestCode;
                                    getDataObj.Barcode = req.AssetDetail.Barcode;
                                    getDataObj.CreatedById = req.CreatedById;
                                    //  getDataObj.CreatedBy = lstTracks[0].User != null ? lstTracks[0].User.UserName : "";
                                    getDataObj.CreatedBy = req.User != null ? req.User.UserName : "";
                                    getDataObj.Subject = req.Subject;
                                    getDataObj.RequestDate = req.RequestDate;
                                    getDataObj.AssetDetailId = req.AssetDetailId != null ? (int)req.AssetDetailId : 0;
                                    getDataObj.HospitalId = req.AssetDetail.HospitalId;
                                    getDataObj.StatusId = (int)lstTracks[0].RequestStatusId;
                                    getDataObj.StatusName = lstTracks[0].RequestStatus.Name;
                                    getDataObj.StatusNameAr = lstTracks[0].RequestStatus.NameAr;
                                    getDataObj.StatusColor = lstTracks[0].RequestStatus.Color;
                                    getDataObj.StatusIcon = lstTracks[0].RequestStatus.Icon;
                                    getDataObj.Description = lstTracks[0].Description;
                                    if (int.Parse(lstTracks[0].RequestStatusId.ToString()) == 2)
                                    {
                                        getDataObj.ClosedDate = lstTracks[0].DescriptionDate.ToString();
                                    }
                                    else
                                    {
                                        getDataObj.ClosedDate = "";
                                    }
                                    getDataObj.Barcode = req.AssetDetail.Barcode;
                                    getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                                    getDataObj.ModelNumber = req.AssetDetail.MasterAsset.ModelNumber;
                                    getDataObj.ModeId = req.RequestModeId != null ? (int)req.RequestModeId : 0;
                                    getDataObj.ModeName = req.RequestMode != null ? req.RequestMode.Name : "";
                                    getDataObj.ModeNameAr = req.RequestMode != null ? req.RequestMode.NameAr : "";
                                    getDataObj.PeriorityId = req.RequestPeriorityId != null ? (int)req.RequestPeriorityId : 0;
                                    getDataObj.PeriorityName = req.RequestPeriority != null ? req.RequestPeriority.Name : "";
                                    getDataObj.PeriorityNameAr = req.RequestPeriority != null ? req.RequestPeriority.NameAr : "";
                                    getDataObj.PeriorityColor = req.RequestPeriority != null ? req.RequestPeriority.Color : "";
                                    getDataObj.PeriorityIcon = req.RequestPeriority != null ? req.RequestPeriority.Icon : "";
                                    getDataObj.AssetHospitalId = req.HospitalId;
                                    getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                                    getDataObj.Barcode = req.AssetDetail.Barcode;
                                    getDataObj.AssetName = req.AssetDetail.MasterAsset.Name;
                                    getDataObj.AssetNameAr = req.AssetDetail.MasterAsset.NameAr;
                                    getDataObj.ListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id)
                                       .ToList().Select(item => new IndexRequestTrackingVM.GetData
                                       {
                                           Id = item.Id,
                                           StatusName = item.RequestStatusId != null ? lstTracks[0].RequestStatus.Name : "",
                                           StatusNameAr = item.RequestStatusId != null ? lstTracks[0].RequestStatus.NameAr : "",
                                           Description = item.Description,
                                           Date = item.DescriptionDate,
                                           StatusId = item.RequestStatusId != null ? (int)item.RequestStatusId : 0,
                                           //   isExpanded = (_context.ExternalFixFile.Where(a => a.RequestTrackingId == item.Id).Count()) > 0 ? true : false,
                                           //   ListDocuments = _context.ExternalFixFile.Where(a => a.RequestTrackingId == item.Id).ToList(),
                                       }).ToList();
                                    var lstWOStatus = _context.WorkOrderTrackings
                                            .Include(o => o.WorkOrder).Include(o => o.WorkOrderStatus).Where(a => a.WorkOrder.RequestId == req.Id)
                                            .OrderByDescending(a => a.CreationDate).ToList();

                                    if (lstWOStatus.Count > 0)
                                    {
                                        getDataObj.LatestWorkOrderStatusId = lstWOStatus[0].WorkOrderStatusId;
                                        getDataObj.WOLastTrackDescription = lstWOStatus[0].Notes;
                                    }
                                    getDataObj.CountListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id).ToList().Count;
                                    getDataObj.CountWorkOrder = _context.WorkOrders.Where(a => a.RequestId == req.Id).ToList().Count > 0 ? _context.WorkOrders.Where(a => a.RequestId == req.Id).ToList().Count : 0;
                                    getDataObj.GovernorateId = req.User != null ? req.User.GovernorateId : 0;
                                    getDataObj.CityId = req.User != null ? req.User.CityId : 0;
                                    getDataObj.OrganizationId = req.User != null ? req.User.OrganizationId : 0;
                                    getDataObj.SubOrganizationId = req.User != null ? req.User.SubOrganizationId : 0;
                                    lstModel.Add(getDataObj);
                                }
                            }
                        }

                    }

                }

                else
                {
                    mainClass.Count = LastTrackingForeachRequest.Where(a => a.RequestStatusId == statusId).Count();

                    var lastTrackOfRequestByStatus = LastTrackingForeachRequest.Where(a => a.RequestStatusId == statusId).ToList();

                    var requests = _context.Request
                                       .Include(a => a.AssetDetail)
                                       .Include(a => a.AssetDetail.Hospital)
                                       .Include(a => a.AssetDetail.Hospital.Governorate)
                                       .Include(a => a.AssetDetail.Hospital.City)
                                       .Include(a => a.AssetDetail.Hospital.Organization)
                                       .Include(a => a.AssetDetail.Hospital.SubOrganization)
                                       .Include(a => a.AssetDetail.Hospital.SubOrganization)
                                       .Include(a => a.User)
                                       .Include(a => a.RequestMode)
                                       .Include(a => a.RequestPeriority)
                                       .Include(a => a.RequestType)
                                       .Include(a => a.AssetDetail.MasterAsset)
                                       .OrderByDescending(a => a.RequestDate)
                                       .AsNoTracking().ToList();


                    //  List<Person> selectedPeople = people.Where(p => selectedIds.Contains(p.Id)).ToList();
                    var ids = lastTrackOfRequestByStatus.Select(a => a.RequestId).ToList();
                    requests = requests.Where(a => ids.Contains(a.Id)).ToList();
                    mainClass.Count = requests.Count;
                    requests = requests.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                    if (requests.Count > 0)
                    {
                        foreach (var req in requests)
                        {
                            var lstTracks = _context.RequestTracking.Include(a => a.RequestStatus).Where(a => a.RequestId == req.Id).OrderByDescending(a => a.DescriptionDate).ToList();
                            if (lstTracks.Count > 0)
                            {
                                var requestStatusId = lstTracks[0].RequestStatusId;
                                if (requestStatusId == statusId)
                                {
                                    IndexRequestVM.GetData getDataObj = new IndexRequestVM.GetData();
                                    getDataObj.RequestId = req.Id;
                                    getDataObj.Id = req.Id;
                                    getDataObj.RequestCode = req.RequestCode;
                                    getDataObj.Barcode = req.AssetDetail.Barcode;
                                    getDataObj.CreatedById = req.CreatedById;
                                    //getDataObj.CreatedBy = lstTracks[0].User != null ? lstTracks[0].User.UserName : "";
                                    getDataObj.CreatedBy = req.User != null ? req.User.UserName : "";
                                    getDataObj.Subject = req.Subject;
                                    getDataObj.RequestDate = req.RequestDate;
                                    getDataObj.AssetDetailId = req.AssetDetailId != null ? (int)req.AssetDetailId : 0;
                                    getDataObj.HospitalId = req.AssetDetail.HospitalId;
                                    getDataObj.StatusId = (int)lstTracks[0].RequestStatusId;
                                    getDataObj.StatusName = lstTracks[0].RequestStatus.Name;
                                    getDataObj.StatusNameAr = lstTracks[0].RequestStatus.NameAr;
                                    getDataObj.StatusColor = lstTracks[0].RequestStatus.Color;
                                    getDataObj.StatusIcon = lstTracks[0].RequestStatus.Icon;
                                    getDataObj.Description = lstTracks[0].Description;
                                    if (int.Parse(lstTracks[0].RequestStatusId.ToString()) == 2)
                                    {
                                        getDataObj.ClosedDate = lstTracks[0].DescriptionDate.ToString();
                                    }
                                    else
                                    {
                                        getDataObj.ClosedDate = "";
                                    }
                                    getDataObj.Barcode = req.AssetDetail.Barcode;
                                    getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                                    getDataObj.ModelNumber = req.AssetDetail.MasterAsset.ModelNumber;
                                    getDataObj.ModeId = req.RequestModeId != null ? (int)req.RequestModeId : 0;
                                    getDataObj.ModeName = req.RequestMode != null ? req.RequestMode.Name : "";
                                    getDataObj.ModeNameAr = req.RequestMode != null ? req.RequestMode.NameAr : "";
                                    getDataObj.PeriorityId = req.RequestPeriorityId != null ? (int)req.RequestPeriorityId : 0;
                                    getDataObj.PeriorityName = req.RequestPeriority != null ? req.RequestPeriority.Name : "";
                                    getDataObj.PeriorityNameAr = req.RequestPeriority != null ? req.RequestPeriority.NameAr : "";
                                    getDataObj.PeriorityColor = req.RequestPeriority != null ? req.RequestPeriority.Color : "";
                                    getDataObj.PeriorityIcon = req.RequestPeriority != null ? req.RequestPeriority.Icon : "";
                                    getDataObj.AssetHospitalId = req.HospitalId;
                                    getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                                    getDataObj.Barcode = req.AssetDetail.Barcode;
                                    getDataObj.AssetName = req.AssetDetail.MasterAsset.Name;
                                    getDataObj.AssetNameAr = req.AssetDetail.MasterAsset.NameAr;
                                    getDataObj.ListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id)
                                       .ToList().Select(item => new IndexRequestTrackingVM.GetData
                                       {
                                           Id = item.Id,
                                           StatusName = item.RequestStatusId != null ? lstTracks[0].RequestStatus.Name : "",
                                           StatusNameAr = item.RequestStatusId != null ? lstTracks[0].RequestStatus.NameAr : "",
                                           Description = item.Description,
                                           Date = item.DescriptionDate,
                                           StatusId = item.RequestStatusId != null ? (int)item.RequestStatusId : 0,
                                           //   isExpanded = (_context.ExternalFixFile.Where(a => a.RequestTrackingId == item.Id).Count()) > 0 ? true : false,
                                           //   ListDocuments = _context.ExternalFixFile.Where(a => a.RequestTrackingId == item.Id).ToList(),
                                       }).ToList();
                                    var lstWOStatus = _context.WorkOrderTrackings
                                            .Include(o => o.WorkOrder).Include(o => o.WorkOrderStatus).Where(a => a.WorkOrder.RequestId == req.Id)
                                            .OrderByDescending(a => a.CreationDate).ToList();

                                    if (lstWOStatus.Count > 0)
                                    {
                                        getDataObj.LatestWorkOrderStatusId = lstWOStatus[0].WorkOrderStatusId;
                                        getDataObj.WOLastTrackDescription = lstWOStatus[0].Notes;
                                    }
                                    getDataObj.CountListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id).ToList().Count;
                                    getDataObj.CountWorkOrder = _context.WorkOrders.Where(a => a.RequestId == req.Id).ToList().Count > 0 ? _context.WorkOrders.Where(a => a.RequestId == req.Id).ToList().Count : 0;
                                    getDataObj.GovernorateId = req.User != null ? req.User.GovernorateId : 0;
                                    getDataObj.CityId = req.User != null ? req.User.CityId : 0;
                                    getDataObj.OrganizationId = req.User != null ? req.User.OrganizationId : 0;
                                    getDataObj.SubOrganizationId = req.User != null ? req.User.SubOrganizationId : 0;
                                    lstModel.Add(getDataObj);
                                }
                            }
                        }

                    }

                }

            }

            else
            {
                if (IsEligble is not null)
                {
                    // count of request based on sepecific condition
                    mainClass.Count = _context.Request
                        .Include(a => a.AssetDetail)
                         .Include(a => a.AssetDetail.Hospital)
                        .Include(a => a.AssetDetail.Hospital.Governorate)
                        .Include(a => a.AssetDetail.Hospital.City)
                        .Include(a => a.AssetDetail.Hospital.Organization)
                        .Include(a => a.AssetDetail.Hospital.SubOrganization)
                       .Include(a => a.AssetDetail.Hospital.SubOrganization)
                        .Include(a => a.User)
                        .Include(a => a.RequestMode)
                        .Include(a => a.RequestPeriority)
                       .Include(a => a.RequestType)
                      .Include(a => a.AssetDetail.MasterAsset).AsNoTracking().Where(IsEligble).Count();

                    // requests based on specific condition
                    var requests = _context.Request
                      .Include(a => a.AssetDetail)
                      .Include(a => a.AssetDetail.Hospital)
                      .Include(a => a.AssetDetail.Hospital.Governorate)
                      .Include(a => a.AssetDetail.Hospital.City)
                      .Include(a => a.AssetDetail.Hospital.Organization)
                      .Include(a => a.AssetDetail.Hospital.SubOrganization)
                      .Include(a => a.AssetDetail.Hospital.SubOrganization)
                      .Include(a => a.User)
                      .Include(a => a.RequestMode)
                      .Include(a => a.RequestPeriority)
                      .Include(a => a.RequestType)
                      .Include(a => a.AssetDetail.MasterAsset)
                      .OrderByDescending(a => a.RequestDate)
                      .Where(IsEligble).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                    if (requests.Count > 0)
                    {

                        foreach (var req in requests)
                        {


                            var lstTracks = _context.RequestTracking.Include(a => a.RequestStatus).Where(a => a.RequestId == req.Id).OrderByDescending(a => a.DescriptionDate).ToList();
                            if (lstTracks.Count > 0)
                            {
                                var requestStatusId = lstTracks[0].RequestStatusId;
                                IndexRequestVM.GetData getDataObj = new IndexRequestVM.GetData();
                                getDataObj.RequestId = req.Id;
                                getDataObj.Id = req.Id;
                                getDataObj.RequestCode = req.RequestCode;
                                getDataObj.Barcode = req.AssetDetail.Barcode;
                                getDataObj.CreatedById = req.CreatedById;
                                //getDataObj.CreatedBy = lstTracks[0].User != null ? lstTracks[0].User.UserName : "";
                                getDataObj.CreatedBy = req.User != null ? req.User.UserName : "";
                                getDataObj.Subject = req.Subject;
                                getDataObj.RequestDate = req.RequestDate;
                                getDataObj.AssetDetailId = req.AssetDetailId != null ? (int)req.AssetDetailId : 0;
                                getDataObj.HospitalId = req.AssetDetail.HospitalId;
                                getDataObj.StatusId = (int)lstTracks[0].RequestStatusId;
                                getDataObj.StatusName = lstTracks[0].RequestStatus.Name;
                                getDataObj.StatusNameAr = lstTracks[0].RequestStatus.NameAr;
                                getDataObj.StatusColor = lstTracks[0].RequestStatus.Color;
                                getDataObj.StatusIcon = lstTracks[0].RequestStatus.Icon;
                                getDataObj.Description = lstTracks[0].Description;
                                if (int.Parse(lstTracks[0].RequestStatusId.ToString()) == 2)
                                {
                                    getDataObj.ClosedDate = lstTracks[0].DescriptionDate.ToString();
                                }
                                else
                                {
                                    getDataObj.ClosedDate = "";
                                }
                                getDataObj.Barcode = req.AssetDetail.Barcode;
                                getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                                getDataObj.ModelNumber = req.AssetDetail.MasterAsset.ModelNumber;
                                getDataObj.ModeId = req.RequestModeId != null ? (int)req.RequestModeId : 0;
                                getDataObj.ModeName = req.RequestMode != null ? req.RequestMode.Name : "";
                                getDataObj.ModeNameAr = req.RequestMode != null ? req.RequestMode.NameAr : "";
                                getDataObj.PeriorityId = req.RequestPeriorityId != null ? (int)req.RequestPeriorityId : 0;
                                getDataObj.PeriorityName = req.RequestPeriority != null ? req.RequestPeriority.Name : "";
                                getDataObj.PeriorityNameAr = req.RequestPeriority != null ? req.RequestPeriority.NameAr : "";
                                getDataObj.PeriorityColor = req.RequestPeriority != null ? req.RequestPeriority.Color : "";
                                getDataObj.PeriorityIcon = req.RequestPeriority != null ? req.RequestPeriority.Icon : "";
                                getDataObj.AssetHospitalId = req.HospitalId;
                                getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                                getDataObj.Barcode = req.AssetDetail.Barcode;
                                getDataObj.AssetName = req.AssetDetail.MasterAsset.Name;
                                getDataObj.AssetNameAr = req.AssetDetail.MasterAsset.NameAr;
                                getDataObj.ListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id)
                                        .ToList().Select(item => new IndexRequestTrackingVM.GetData
                                        {
                                            Id = item.Id,
                                            StatusName = item.RequestStatusId != null ? lstTracks[0].RequestStatus.Name : "",
                                            StatusNameAr = item.RequestStatusId != null ? lstTracks[0].RequestStatus.NameAr : "",
                                            Description = item.Description,
                                            Date = item.DescriptionDate,
                                            StatusId = item.RequestStatusId != null ? (int)item.RequestStatusId : 0,
                                            // isExpanded = (_context.ExternalFixFile.Where(a => a.RequestTrackingId == item.Id).Count()) > 0 ? true : false,
                                            // ListDocuments = _context.ExternalFixFile.Where(a => a.RequestTrackingId == item.Id).ToList(),
                                        }).ToList();

                                var lstWOStatus = _context.WorkOrderTrackings
                                        .Include(o => o.WorkOrder).Include(o => o.WorkOrderStatus).Where(a => a.WorkOrder.RequestId == req.Id)
                                        .OrderByDescending(a => a.CreationDate).ToList();

                                if (lstWOStatus.Count > 0)
                                {
                                    getDataObj.LatestWorkOrderStatusId = lstWOStatus[0].WorkOrderStatusId;
                                    getDataObj.WOLastTrackDescription = lstWOStatus[0].Notes;
                                }
                                getDataObj.CountListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id).ToList().Count;
                                getDataObj.CountWorkOrder = _context.WorkOrders.Where(a => a.RequestId == req.Id).ToList().Count > 0 ? _context.WorkOrders.Where(a => a.RequestId == req.Id).ToList().Count : 0;
                                getDataObj.GovernorateId = req.User != null ? req.User.GovernorateId : 0;
                                getDataObj.CityId = req.User != null ? req.User.CityId : 0;
                                getDataObj.OrganizationId = req.User != null ? req.User.OrganizationId : 0;
                                getDataObj.SubOrganizationId = req.User != null ? req.User.SubOrganizationId : 0;
                                lstModel.Add(getDataObj);
                            }

                        }
                    }




                }

                else
                {
                    mainClass.Count = _context.Request.AsNoTracking().Count();

                    var requests = _context.Request
                                        .Include(a => a.AssetDetail)
                                        .Include(a => a.AssetDetail.Hospital)
                                        .Include(a => a.AssetDetail.Hospital.Governorate)
                                        .Include(a => a.AssetDetail.Hospital.City)
                                        .Include(a => a.AssetDetail.Hospital.Organization)
                                        .Include(a => a.AssetDetail.Hospital.SubOrganization)
                                        .Include(a => a.AssetDetail.Hospital.SubOrganization)
                                        .Include(a => a.User)
                                        .Include(a => a.RequestMode)
                                        .Include(a => a.RequestPeriority)
                                        .Include(a => a.RequestType)
                                        .Include(a => a.AssetDetail.MasterAsset)
                                        .OrderByDescending(a => a.RequestDate)
                                        .AsNoTracking()
                                        .Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();



                    if (requests.Count > 0)
                    {

                        foreach (var req in requests)
                        {


                            var lstTracks = _context.RequestTracking.Include(a => a.RequestStatus).Where(a => a.RequestId == req.Id).OrderByDescending(a => a.DescriptionDate).ToList();
                            if (lstTracks.Count > 0)
                            {
                                var requestStatusId = lstTracks[0].RequestStatusId;
                                IndexRequestVM.GetData getDataObj = new IndexRequestVM.GetData();
                                getDataObj.RequestId = req.Id;
                                getDataObj.Id = req.Id;
                                getDataObj.RequestCode = req.RequestCode;
                                getDataObj.Barcode = req.AssetDetail.Barcode;
                                getDataObj.CreatedById = req.CreatedById;
                                //       getDataObj.CreatedBy = lstTracks[0].User != null ? lstTracks[0].User.UserName : "";
                                getDataObj.CreatedBy = req.User != null ? req.User.UserName : "";
                                getDataObj.Subject = req.Subject;
                                getDataObj.RequestDate = req.RequestDate;
                                getDataObj.AssetDetailId = req.AssetDetailId != null ? (int)req.AssetDetailId : 0;
                                getDataObj.HospitalId = req.AssetDetail.HospitalId;
                                getDataObj.StatusId = (int)lstTracks[0].RequestStatusId;
                                getDataObj.StatusName = lstTracks[0].RequestStatus.Name;
                                getDataObj.StatusNameAr = lstTracks[0].RequestStatus.NameAr;
                                getDataObj.StatusColor = lstTracks[0].RequestStatus.Color;
                                getDataObj.StatusIcon = lstTracks[0].RequestStatus.Icon;
                                getDataObj.Description = lstTracks[0].Description;
                                if (int.Parse(lstTracks[0].RequestStatusId.ToString()) == 2)
                                {
                                    getDataObj.ClosedDate = lstTracks[0].DescriptionDate.ToString();
                                }
                                else
                                {
                                    getDataObj.ClosedDate = "";
                                }
                                getDataObj.Barcode = req.AssetDetail.Barcode;
                                getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                                getDataObj.ModelNumber = req.AssetDetail.MasterAsset.ModelNumber;
                                getDataObj.ModeId = req.RequestModeId != null ? (int)req.RequestModeId : 0;
                                getDataObj.ModeName = req.RequestMode != null ? req.RequestMode.Name : "";
                                getDataObj.ModeNameAr = req.RequestMode != null ? req.RequestMode.NameAr : "";
                                getDataObj.PeriorityId = req.RequestPeriorityId != null ? (int)req.RequestPeriorityId : 0;
                                getDataObj.PeriorityName = req.RequestPeriority != null ? req.RequestPeriority.Name : "";
                                getDataObj.PeriorityNameAr = req.RequestPeriority != null ? req.RequestPeriority.NameAr : "";
                                getDataObj.PeriorityColor = req.RequestPeriority != null ? req.RequestPeriority.Color : "";
                                getDataObj.PeriorityIcon = req.RequestPeriority != null ? req.RequestPeriority.Icon : "";
                                getDataObj.AssetHospitalId = req.HospitalId;
                                getDataObj.SerialNumber = req.AssetDetail.SerialNumber;
                                getDataObj.Barcode = req.AssetDetail.Barcode;
                                getDataObj.AssetName = req.AssetDetail.MasterAsset.Name;
                                getDataObj.AssetNameAr = req.AssetDetail.MasterAsset.NameAr;
                                getDataObj.ListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id)
                                        .ToList().Select(item => new IndexRequestTrackingVM.GetData
                                        {
                                            Id = item.Id,
                                            StatusName = item.RequestStatusId != null ? lstTracks[0].RequestStatus.Name : "",
                                            StatusNameAr = item.RequestStatusId != null ? lstTracks[0].RequestStatus.NameAr : "",
                                            Description = item.Description,
                                            Date = item.DescriptionDate,
                                            StatusId = item.RequestStatusId != null ? (int)item.RequestStatusId : 0,
                                            // isExpanded = (_context.ExternalFixFile.Where(a => a.RequestTrackingId == item.Id).Count()) > 0 ? true : false,
                                            // ListDocuments = _context.ExternalFixFile.Where(a => a.RequestTrackingId == item.Id).ToList(),
                                        }).ToList();

                                var lstWOStatus = _context.WorkOrderTrackings
                                        .Include(o => o.WorkOrder).Include(o => o.WorkOrderStatus).Where(a => a.WorkOrder.RequestId == req.Id)
                                        .OrderByDescending(a => a.CreationDate).ToList();

                                if (lstWOStatus.Count > 0)
                                {
                                    getDataObj.LatestWorkOrderStatusId = lstWOStatus[0].WorkOrderStatusId;
                                    getDataObj.WOLastTrackDescription = lstWOStatus[0].Notes;
                                }
                                getDataObj.CountListTracks = _context.RequestTracking.Where(a => a.RequestId == req.Id).ToList().Count;
                                getDataObj.CountWorkOrder = _context.WorkOrders.Where(a => a.RequestId == req.Id).ToList().Count > 0 ? _context.WorkOrders.Where(a => a.RequestId == req.Id).ToList().Count : 0;
                                getDataObj.GovernorateId = req.User != null ? req.User.GovernorateId : 0;
                                getDataObj.CityId = req.User != null ? req.User.CityId : 0;
                                getDataObj.OrganizationId = req.User != null ? req.User.OrganizationId : 0;
                                getDataObj.SubOrganizationId = req.User != null ? req.User.SubOrganizationId : 0;
                                lstModel.Add(getDataObj);
                            }

                        }
                    }


                }


            }


            mainClass.Results = lstModel;
            return mainClass;
        }




        private IQueryable<RequestTracking> GetAllRequests()
        {

            var lastStatusPerRequest = _context.RequestTracking.Include(a => a.Request)
                .Include(a => a.Request.RequestMode).Include(a => a.Request.RequestType).Include(a => a.RequestStatus)
                 .Include(a => a.Request.Hospital)

                                     .Include(a => a.Request.Hospital.Governorate)
                                     .Include(a => a.Request.Hospital.City)
                                         .Include(a => a.Request.Hospital.Organization)
                                         .Include(a => a.Request.Hospital.SubOrganization)

                         .Include(a => a.Request.AssetDetail).Include(a => a.User).Include(a => a.Request.AssetDetail.MasterAsset)
                         .Include(a => a.Request.AssetDetail.MasterAsset.brand).Include(a => a.Request.AssetDetail.Supplier)
                         .Include(a => a.Request.AssetDetail.Department).Include(a => a.Request.AssetDetail.MasterAsset.Origin)
                         .Include(a => a.Request.AssetDetail.Building).Include(a => a.Request.AssetDetail.Floor).Include(a => a.Request.AssetDetail.Room)
                         .Include(a => a.Request.RequestPeriority).Include(a => a.Request.RequestMode)
                         .Include(a => a.Request.AssetDetail.Hospital)
                         //.Include(a => a.Request.AssetDetail.Hospital).ThenInclude(h => h.Organization)
                         //.Include(a => a.Request.AssetDetail.Hospital).ThenInclude(h => h.Governorate)
                         //.Include(a => a.Request.AssetDetail.Hospital).ThenInclude(h => h.City)
                         //.Include(a => a.Request.AssetDetail.Hospital).ThenInclude(h => h.SubOrganization)
                         .ToList()
                         .GroupBy(track => track.RequestId)
                         .Select(g => g.OrderByDescending(track => track.DescriptionDate).FirstOrDefault());
                       //  .OrderByDescending(a => a.DescriptionDate);
            return lastStatusPerRequest.AsQueryable();
        }



        public IndexRequestVM ListRequests(SortAndFilterRequestVM data, int pageNumber, int pageSize)
        {
            #region Initial Variables
            IQueryable<RequestTracking> query = GetAllRequests();

            IndexRequestVM mainClass = new IndexRequestVM();
            List<IndexRequestVM.GetData> list = new List<IndexRequestVM.GetData>();
            ApplicationUser userObj = new ApplicationUser();
            List<string> lstRoleNames = new List<string>();
            Employee empObj = new Employee();
            #endregion

            #region User Role

            if (data.SearchObj.UserId != null)
            {
                var getUserById = _context.ApplicationUser.Where(a => a.Id == data.SearchObj.UserId).ToList();
                userObj = getUserById[0];

                var roles = (from userRole in _context.UserRoles
                             join role in _context.ApplicationRole on userRole.RoleId equals role.Id
                             where userRole.UserId == userObj.Id
                             select role);
                foreach (var role in roles)
                {
                    lstRoleNames.Add(role.Name);
                }
                var lstEmployees = _context.Employees.Where(a => a.Email == userObj.Email).ToList();
                if (lstEmployees.Count > 0)
                {
                    empObj = lstEmployees[0];
                }
            }
            #endregion

            #region Load Data Depend on User
            if (userObj.HospitalId > 0)
            {

                if (lstRoleNames.Any() && ((lstRoleNames.Contains("AssetOwner") || lstRoleNames.Contains("SRCreator") || (lstRoleNames.Contains("AssetOwner") && lstRoleNames.Contains("SRCreator"))) && !lstRoleNames.Any(itm => itm == "TLHospitalManager" || itm == "EngDepManager" || itm == "SRReviewer")))
                {
                    query = query.Where(a => a.CreatedById == userObj.Id && a.HospitalId == userObj.HospitalId);
                }
                if (lstRoleNames.Contains("TLHospitalManager") && lstRoleNames.Contains("EngDepManager") && lstRoleNames.Contains("SRReviewer") && lstRoleNames.Contains("SRCreator") && lstRoleNames.Contains("AssetOwner") && lstRoleNames.Contains("SRCreator"))
                {
                    query = query.Where(a => a.HospitalId == userObj.HospitalId);
                }
                if (lstRoleNames.Contains("TLHospitalManager") && !lstRoleNames.Contains("EngDepManager") && !lstRoleNames.Contains("SRReviewer") && !lstRoleNames.Contains("SRCreator") && !lstRoleNames.Contains("AssetOwner") && !lstRoleNames.Contains("SRCreator"))
                {
                    query = query.Where(a => a.HospitalId == userObj.HospitalId);
                }
                if (lstRoleNames.Contains("TLHospitalManager") && lstRoleNames.Contains("EngDepManager") && !lstRoleNames.Contains("SRReviewer") && !lstRoleNames.Contains("SRCreator") && !lstRoleNames.Contains("AssetOwner") && !lstRoleNames.Contains("SRCreator"))
                {
                    query = query.Where(a => a.HospitalId == userObj.HospitalId);
                }
                if (lstRoleNames.Contains("TLHospitalManager") && lstRoleNames.Contains("EngDepManager") && lstRoleNames.Contains("SRReviewer") && !lstRoleNames.Contains("SRCreator") && !lstRoleNames.Contains("AssetOwner") && !lstRoleNames.Contains("SRCreator"))
                {
                    query = query.Where(a => a.HospitalId == userObj.HospitalId);
                }
                if (lstRoleNames.Contains("TLHospitalManager") && lstRoleNames.Contains("EngDepManager") && lstRoleNames.Contains("SRReviewer") && lstRoleNames.Contains("SRCreator") && !lstRoleNames.Contains("AssetOwner") && !lstRoleNames.Contains("SRCreator"))
                {
                    query = query.Where(a => a.HospitalId == userObj.HospitalId);
                }
                if (lstRoleNames.Contains("TLHospitalManager") && lstRoleNames.Contains("EngDepManager") && lstRoleNames.Contains("SRReviewer") && lstRoleNames.Contains("SRCreator") && lstRoleNames.Contains("AssetOwner") && !lstRoleNames.Contains("SRCreator"))
                {
                    query = query.Where(a => a.HospitalId == userObj.HospitalId);
                }
            }
            else
            {
                if (userObj.GovernorateId > 0 && userObj.CityId == 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                {
                    query = query.Where(a => a.Hospital.GovernorateId == userObj.GovernorateId);
                }
                if (userObj.GovernorateId > 0 && userObj.CityId > 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                {
                    query = query.Where(a => a.Hospital.GovernorateId == userObj.GovernorateId && a.Hospital.CityId == userObj.CityId);
                }
                if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId > 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                {
                    query = query.Where(a => a.Hospital.OrganizationId == userObj.OrganizationId);
                }
                if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId > 0 && userObj.SubOrganizationId > 0 && userObj.HospitalId == 0)
                {
                    query = query.Where(a => a.Hospital.OrganizationId == userObj.OrganizationId && a.Hospital.SubOrganizationId == userObj.SubOrganizationId);
                }
                //if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                //{
                //    query = query;
                //}

            }
            #endregion

            #region Search Criteria
            //if (data.SearchObj.GovernorateId != 0)
            //{
            //    query = query.Where(x => x.Request.Hospital.GovernorateId == data.SearchObj.GovernorateId);
            //}
            if (data.SearchObj.CityId != 0)
            {
                query = query.Where(x => x.Request.Hospital.CityId == data.SearchObj.CityId);
            }
            //if (data.SearchObj.OrganizationId != 0)
            //{
            //    query = query.Where(x => x.Request.Hospital.OrganizationId == data.SearchObj.OrganizationId);
            //}
            //if (data.SearchObj.SubOrganizationId != 0)
            //{
            //    query = query.Where(x => x.Request.Hospital.SubOrganizationId == data.SearchObj.SubOrganizationId);
            //}
            if (data.SearchObj.HospitalId != 0)
            {
                query = query.Where(x => x.Request.HospitalId == data.SearchObj.HospitalId);
            }
            if (data.SearchObj.MasterAssetId != 0)
            {
                query = query.Where(x => x.Request.AssetDetail.MasterAssetId == data.SearchObj.MasterAssetId);
            }
            if (!string.IsNullOrEmpty(data.SearchObj.Barcode))
            {
                query = query.Where(x => x.Request.AssetDetail.Barcode.Contains(data.SearchObj.Barcode));
            }
            if (!string.IsNullOrEmpty(data.SearchObj.SerialNumber))
            {
                query = query.Where(x => x.Request.AssetDetail.SerialNumber.Contains(data.SearchObj.SerialNumber));
            }
            if (data.SearchObj.ModelNumber != "")
            {
                query = query.Where(x => x.Request.AssetDetail.MasterAsset.ModelNumber == data.SearchObj.ModelNumber);
            }
            if (data.SearchObj.DepartmentId != 0)
            {
                query = query.Where(x => x.Request.AssetDetail.DepartmentId == data.SearchObj.DepartmentId);
            }
            if (data.SearchObj.PeriorityId != 0)
            {
                query = query.Where(x => x.Request.AssetDetail.MasterAsset.PeriorityId == data.SearchObj.PeriorityId);
            }
            if (data.SearchObj.ModeId != 0)
            {
                query = query.Where(x => x.Request.RequestModeId == data.SearchObj.ModeId);
            }
            if (data.SearchObj.Code != "")
            {
                query = query.Where(x => x.Request.RequestCode == data.SearchObj.Code);
            }
            if (data.SearchObj.Subject != "")
            {
                query = query.Where(x => x.Request.Subject == data.SearchObj.Subject);
            }
            if (data.SearchObj.StatusId != 0)
            {
                query = query.Where(x => x.RequestStatusId == data.SearchObj.StatusId);
            }

            if (data.SearchObj.AssetDetailId != 0)
            {
                query = query.Where(x => x.Request.AssetDetailId == data.SearchObj.AssetDetailId);
            }

            string setstartday, setstartmonth, setendday, setendmonth = "";
            DateTime startingFrom = new DateTime();
            DateTime endingTo = new DateTime();
            if (data.SearchObj.Start == "")
            {
                startingFrom = DateTime.Parse("1900-01-01").Date;
            }
            else
            {
                data.SearchObj.StartDate = DateTime.Parse(data.SearchObj.Start.ToString());
                var startyear = data.SearchObj.StartDate.Value.Year;
                var startmonth = data.SearchObj.StartDate.Value.Month;
                var startday = data.SearchObj.StartDate.Value.Day;
                if (startday < 10)
                    setstartday = data.SearchObj.StartDate.Value.Day.ToString().PadLeft(2, '0');
                else
                    setstartday = data.SearchObj.StartDate.Value.Day.ToString();

                if (startmonth < 10)
                    setstartmonth = data.SearchObj.StartDate.Value.Month.ToString().PadLeft(2, '0');
                else
                    setstartmonth = data.SearchObj.StartDate.Value.Month.ToString();

                var sDate = startyear + "/" + setstartmonth + "/" + setstartday;
                startingFrom = DateTime.Parse(sDate);
            }

            if (data.SearchObj.End == "")
            {
                endingTo = DateTime.Today.Date;
            }
            else
            {
                data.SearchObj.EndDate = DateTime.Parse(data.SearchObj.End.ToString());
                var endyear = data.SearchObj.EndDate.Value.Year;
                var endmonth = data.SearchObj.EndDate.Value.Month;
                var endday = data.SearchObj.EndDate.Value.Day;
                if (endday < 10)
                    setendday = data.SearchObj.EndDate.Value.Day.ToString().PadLeft(2, '0');
                else
                    setendday = data.SearchObj.EndDate.Value.Day.ToString();
                if (endmonth < 10)
                    setendmonth = data.SearchObj.EndDate.Value.Month.ToString().PadLeft(2, '0');
                else
                    setendmonth = data.SearchObj.EndDate.Value.Month.ToString();
                var eDate = endyear + "/" + setendmonth + "/" + setendday;
                endingTo = DateTime.Parse(eDate);
            }
            //if (data.SearchObj.Start != "" && data.SearchObj.End != "")
            //{
            //    query = query.Where(a => a.Request.RequestDate.Date >= startingFrom.Date && a.Request.RequestDate.Date <= endingTo.Date);
            //}


            #endregion

            #region Sort Criteria

            switch (data.SortObj.SortBy)
            {
                case "Barcode":
                case "الباركود":
                    if (data.SortObj.SortStatus == "ascending")
                    {
                        query = query.OrderBy(x => x.Request.AssetDetail.Barcode);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.Request.AssetDetail.Barcode);
                    }
                    break;
                case "Code":
                case "الكود":
                    if (data.SortObj.SortStatus == "ascending")
                    {
                        query = query.OrderBy(x => x.Request.RequestCode);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.Request.RequestCode);
                    }
                    break;

                case "Serial":
                case "السيريال":
                    if (data.SortObj.SortStatus == "ascending")
                    {
                        query = query.OrderBy(x => x.Request.AssetDetail.SerialNumber);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.Request.AssetDetail.SerialNumber);
                    }
                    break;
                case "Model":
                case "الموديل":
                    if (data.SortObj.SortStatus == "ascending")
                    {
                        query = query.OrderBy(x => x.Request.AssetDetail.MasterAsset.ModelNumber);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.Request.AssetDetail.MasterAsset.ModelNumber);
                    }
                    break;
                case "Subject":
                case "الموضوع":
                    if (data.SortObj.SortStatus == "ascending")
                    {
                        query = query.OrderBy(x => x.Request.Subject);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.Request.Subject);
                    }
                    break;

                case "Date":
                case "التاريخ":
                    if (data.SortObj.SortStatus == "ascending")
                    {
                        query = query.OrderBy(x => x.Request.RequestDate);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.Request.RequestDate);
                    }
                    break;

                case "Request Code":
                case "رقم الطلب":
                case "رقم طلب بلاغ العطل":
                    if (data.SortObj.SortStatus == "ascending")
                    {
                        query = query.OrderBy(x => x.Request.RequestCode);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.Request.RequestCode);
                    }
                    break;
                case "CreatedBy":
                case "تم بواسطة":
                    if (data.SortObj.SortStatus == "ascending")
                    {
                        query = query.OrderBy(x => x.User.UserName);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.User.UserName);
                    }
                    break;
                case "Periority":
                    if (data.SortObj.SortStatus == "ascending")
                    {
                        query.OrderBy(x => x.Request.RequestPeriority.Name);
                    }
                    else
                    {
                        query.OrderByDescending(x => x.Request.RequestPeriority.Name);
                    }
                    break;

                case "الأولوية":
                    if (data.SortObj.SortStatus == "ascending")
                    {
                        query = query.OrderBy(x => x.Request.RequestPeriority.NameAr);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.Request.RequestPeriority.NameAr);
                    }
                    break;
                case "Mode":
                    if (data.SortObj.SortStatus == "ascending")
                    {
                        query.OrderBy(x => x.Request.RequestMode.Name);
                    }
                    else
                    {
                        query.OrderByDescending(x => x.Request.RequestMode.Name);
                    }
                    break;
                case "طريقة الإبلاغ":
                    if (data.SortObj.SortStatus == "ascending")
                    {
                        query = query.OrderBy(x => x.Request.RequestMode.NameAr);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.Request.RequestMode.NameAr);
                    }
                    break;
                case "Asset Name":
                    if (data.SortObj.SortStatus == "ascending")
                    {
                        query.OrderBy(x => x.Request.AssetDetail.MasterAsset.Name);
                    }
                    else
                    {
                        query.OrderByDescending(x => x.Request.AssetDetail.MasterAsset.Name);
                    }
                    break;
                case "اسم الأصل":
                    if (data.SortObj.SortStatus == "ascending")
                    {
                        query = query.OrderBy(x => x.Request.AssetDetail.MasterAsset.NameAr);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.Request.AssetDetail.MasterAsset.NameAr);
                    }
                    break;


                case "Hospital":
                    if (data.SortObj.SortStatus == "ascending")
                    {
                        query = query.OrderBy(x => x.Hospital.Name);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.Hospital.Name);
                    }
                    break;
                case "المستشفى":
                    if (data.SortObj.SortStatus == "ascending")
                    {
                        query = query.OrderBy(x => x.Hospital.NameAr);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.Hospital.NameAr);
                    }
                    break;
                case "Brands":
                    if (data.SortObj.SortStatus == "ascending")
                    {
                        query = query.OrderBy(x => x.Request.AssetDetail.MasterAsset.brand.Name);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.Request.AssetDetail.MasterAsset.brand.Name);
                    }
                    break;
                case "الماركات":
                    if (data.SortObj.SortStatus == "ascending")
                    {
                        query = query.OrderBy(x => x.Request.AssetDetail.MasterAsset.brand.NameAr);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.Request.AssetDetail.MasterAsset.brand.NameAr);
                    }
                    break;
                case "Department":
                    if (data.SortObj.SortStatus == "ascending")
                    {
                        query = query.OrderBy(x => x.Request.AssetDetail.Department.Name);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.Request.AssetDetail.Department.Name);
                    }
                    break;
                case "القسم":
                    if (data.SortObj.SortStatus == "ascending")
                    {
                        query = query.OrderBy(x => x.Request.AssetDetail.Department.NameAr);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.Request.AssetDetail.Department.NameAr);
                    }
                    break;
                case "Governorate":
                    if (data.SortObj.SortStatus == "ascending")
                    {
                        query = query.OrderBy(x => x.Hospital.Governorate.Name);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.Hospital.Governorate.Name);
                    }
                    break;
                case "المحافظة":
                    if (data.SortObj.SortStatus == "ascending")
                    {
                        query = query.OrderBy(x => x.Hospital.Governorate.NameAr);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.Hospital.Governorate.NameAr);
                    }
                    break;

                case "City":
                    if (data.SortObj.SortStatus == "ascending")
                    {
                        query = query.OrderBy(x => x.Hospital.City.Name);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.Hospital.City.Name);
                    }
                    break;
                case "المدينة":
                    if (data.SortObj.SortStatus == "ascending")
                    {
                        query = query.OrderBy(x => x.Hospital.City.NameAr);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.Hospital.City.NameAr);
                    }
                    break;
                case "Organization":
                    if (data.SortObj.SortStatus == "ascending")
                    {
                        query = query.OrderBy(x => x.Hospital.Organization.Name);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.Hospital.Organization.Name);
                    }
                    break;

                case "الهيئة":
                    if (data.SortObj.SortStatus == "ascending")
                    {
                        query = query.OrderBy(x => x.Hospital.Organization.NameAr);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.Hospital.Organization.NameAr);
                    }
                    break;
                case "SubOrganization":
                    if (data.SortObj.SortStatus == "ascending")
                    {
                        query = query.OrderBy(x => x.Hospital.SubOrganization.Name);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.Hospital.SubOrganization.Name);
                    }
                    break;
                case "هيئة فرعية":
                    if (data.SortObj.SortStatus == "ascending")
                    {
                        query = query.OrderBy(x => x.Hospital.SubOrganization.NameAr);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.Hospital.SubOrganization.NameAr);
                    }
                    break;
            }

            #endregion

            #region Count data and fiter By Paging
            IQueryable<RequestTracking> lstResults;
            var countItems = query.ToList();
            mainClass.Count = countItems.Count();
            if (pageNumber == 0 && pageSize == 0)
                lstResults = query;
            else
                lstResults = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            #endregion

            #region Loop to get Items after serach and sort

            foreach (var req in lstResults.ToList())
            {
                IndexRequestVM.GetData getDataObj = new IndexRequestVM.GetData();
                getDataObj.Id = req.Request.Id;
                getDataObj.RequestCode = req.Request.RequestCode;
                getDataObj.Barcode = req.Request.AssetDetail.Barcode;
                getDataObj.CreatedById = req.CreatedById;
                getDataObj.CreatedBy = req.User != null ? req.User.UserName : "";
                getDataObj.Subject = req.Request.Subject;
                getDataObj.RequestDate = req.Request.RequestDate;
                getDataObj.AssetDetailId = req.Request.AssetDetailId != null ? (int)req.Request.AssetDetailId : 0;
                getDataObj.HospitalId = req.Request.AssetDetail.HospitalId;
                getDataObj.Barcode = req.Request.AssetDetail.Barcode;
                getDataObj.SerialNumber = req.Request.AssetDetail.SerialNumber;
                getDataObj.ModelNumber = req.Request.AssetDetail.MasterAsset.ModelNumber;
                getDataObj.AssetName = req.Request.AssetDetail.MasterAsset.Name;
                getDataObj.AssetNameAr = req.Request.AssetDetail.MasterAsset.NameAr;
                getDataObj.ModeId = req.Request.RequestModeId != null ? (int)req.Request.RequestModeId : 0;
                getDataObj.ModeName = req.Request.RequestMode != null ? req.Request.RequestMode.Name : "";
                getDataObj.ModeNameAr = req.Request.RequestMode != null ? req.Request.RequestMode.NameAr : "";
                getDataObj.PeriorityId = req.Request.RequestPeriorityId != null ? (int)req.Request.RequestPeriorityId : 0;
                getDataObj.PeriorityName = req.Request.RequestPeriority != null ? req.Request.RequestPeriority.Name : "";
                getDataObj.PeriorityNameAr = req.Request.RequestPeriority != null ? req.Request.RequestPeriority.NameAr : "";
                getDataObj.PeriorityColor = req.Request.RequestPeriority != null ? req.Request.RequestPeriority.Color : "";
                getDataObj.PeriorityIcon = req.Request.RequestPeriority != null ? req.Request.RequestPeriority.Icon : "";
                getDataObj.GovernorateId = req.User != null ? req.User.GovernorateId : 0;
                getDataObj.CityId = req.User != null ? req.User.CityId : 0;
                getDataObj.OrganizationId = req.User != null ? req.User.OrganizationId : 0;
                getDataObj.SubOrganizationId = req.User != null ? req.User.SubOrganizationId : 0;

                if (req.Request.RequestModeId != null)
                {
                    getDataObj.ModeId = req.Request.RequestMode.Id;
                    getDataObj.ModeName = req.Request.RequestMode.Name;
                    getDataObj.ModeNameAr = req.Request.RequestMode.NameAr;
                }
                if (req.Request.RequestPeriorityId != null)
                {
                    getDataObj.PeriorityId = (int)req.Request.RequestPeriorityId;
                    getDataObj.PeriorityName = req.Request.RequestPeriority.Name;
                    getDataObj.PeriorityNameAr = req.Request.RequestPeriority.NameAr;
                    getDataObj.PeriorityColor = req.Request.RequestPeriority.Color;
                    getDataObj.PeriorityIcon = req.Request.RequestPeriority.Icon;
                }
                var lstWOStatusDB = _context.WorkOrderTrackings.Include(o => o.WorkOrder).Include(o => o.WorkOrderStatus)
                    .Where(a => a.WorkOrder.RequestId == req.RequestId).OrderByDescending(a => a.CreationDate);
                var lstWOStatus = lstWOStatusDB.ToList();
                if (lstWOStatus.Count > 0)
                {
                    getDataObj.LatestWorkOrderStatusId = lstWOStatus[0].WorkOrderStatusId;
                    getDataObj.WOLastTrackDescription = lstWOStatus[0].Notes;
                }
                getDataObj.StatusId = (int)req.RequestStatus.Id;
                getDataObj.StatusName = req.RequestStatus.Name;
                getDataObj.StatusNameAr = req.RequestStatus.NameAr;
                getDataObj.StatusColor = req.RequestStatus.Color;
                getDataObj.StatusIcon = req.RequestStatus.Icon;
                getDataObj.Description = req.Description;
                if (getDataObj.StatusId == 2)
                {
                    getDataObj.ClosedDate = req.DescriptionDate.ToString();
                }
                else
                {
                    getDataObj.ClosedDate = "";
                }

                getDataObj.CountWorkOrder = _context.WorkOrders.Where(a => a.RequestId == req.RequestId).ToList().Count > 0 ? _context.WorkOrders.Where(a => a.RequestId == req.RequestId).ToList().Count : 0;
                getDataObj.CountListTracks = _context.RequestTracking.Where(a => a.RequestId == req.RequestId).ToList().Count;
                list.Add(getDataObj);
            }
            #endregion

            #region Represent data after Paging and count
            mainClass.Results = list;
            #endregion

            return mainClass;
        }
    }
}
