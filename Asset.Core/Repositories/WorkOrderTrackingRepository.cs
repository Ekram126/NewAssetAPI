using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.WorkOrderTrackingVM;
using Asset.ViewModels.WorkOrderVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class WorkOrderTrackingRepository : IWorkOrderTrackingRepository
    {
        private readonly ApplicationDbContext _context;
        private string msg;

        public WorkOrderTrackingRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public int Add(CreateWorkOrderTrackingVM createWorkOrderTrackingVM)
        {
            try
            {
                string assignedTo = "";
                if (createWorkOrderTrackingVM != null)
                {
                  var isEmployeeId =  Int32.TryParse(createWorkOrderTrackingVM.AssignedTo, out int empId);
                    if (createWorkOrderTrackingVM.AssignedTo != "" && isEmployeeId == true)
                    {

                        int employeeId = int.Parse(createWorkOrderTrackingVM.AssignedTo);
                        var employeeObj = _context.Employees.Find(employeeId);

                        if (employeeObj != null)
                        {
                            var lstUsers = _context.ApplicationUser.Where(a => a.Email == employeeObj.Email).ToList();
                            if (lstUsers.Count > 0)
                            {
                                assignedTo = lstUsers[0].Id;
                            }
                        }
                    }
                    WorkOrderTracking workOrderTracking = new WorkOrderTracking();
                    workOrderTracking.WorkOrderDate = DateTime.Now; //createWorkOrderTrackingVM.WorkOrderDate;
                    workOrderTracking.CreationDate = DateTime.Now;
                    workOrderTracking.Notes = createWorkOrderTrackingVM.Notes;
                    workOrderTracking.WorkOrderStatusId = createWorkOrderTrackingVM.WorkOrderStatusId;
                    workOrderTracking.CreatedById = createWorkOrderTrackingVM.CreatedById;
                    workOrderTracking.WorkOrderId = createWorkOrderTrackingVM.WorkOrderId;
                    if (assignedTo != "")
                        workOrderTracking.AssignedTo = assignedTo;
                    else
                        workOrderTracking.AssignedTo = createWorkOrderTrackingVM.AssignedTo;


                    workOrderTracking.ActualStartDate = DateTime.Parse(createWorkOrderTrackingVM.ActualStartDate);
                    workOrderTracking.ActualEndDate = DateTime.Parse(createWorkOrderTrackingVM.ActualEndDate);
                    _context.WorkOrderTrackings.Add(workOrderTracking);
                    _context.SaveChanges();
                    createWorkOrderTrackingVM.Id = workOrderTracking.Id;
                    return createWorkOrderTrackingVM.Id;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return createWorkOrderTrackingVM.Id;
        }

        public void Delete(int id)
        {
            var workOrderTracking = _context.WorkOrderTrackings.Find(id);
            try
            {
                if (workOrderTracking != null)
                {
                    _context.WorkOrderTrackings.Remove(workOrderTracking);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }

        public WorkOrderDetails GetAllWorkOrderByWorkOrderId(int WorkOrderId)
        {
            var lstTracking = _context.WorkOrderTrackings.Where(r => r.WorkOrderId == WorkOrderId)
                .Include(w => w.WorkOrderStatus)
                .Select(work => new IndexWorkOrderTrackingVM
                {
                    TrackId = work.Id,
                    WorkOrderDate = work.WorkOrderDate,
                    CreationDate = work.CreationDate,
                    AssignedTo = work.AssignedTo,
                    Notes = work.Notes,
                    CreatedById = work.CreatedById,
                    CreatedBy = work.User.UserName,
                    WorkOrderStatusId = work.WorkOrderStatusId,
                    WorkOrderStatusName = work.WorkOrderStatus.Name,
                    WorkOrderStatusNameAr = work.WorkOrderStatus.NameAr,
                    WorkOrderSubject = work.WorkOrder.Subject,
                    CreatedToId = _context.WorkOrderAssigns.Where(a => a.WOTId == work.Id).FirstOrDefault().UserId,
                    CreatedTo = _context.WorkOrderAssigns.Where(a => a.WOTId == work.Id).FirstOrDefault().User.UserName,

                }).OrderByDescending(a => a.CreationDate).ToList();





            var workOrderDetails = _context.WorkOrderTrackings.Where(w => w.WorkOrderId == WorkOrderId)
                .Include(w => w.WorkOrderStatus)
                .Include(w => w.WorkOrder).Include(w => w.WorkOrder.WorkOrderPeriority)
                .Include(w => w.WorkOrder.WorkOrderType).Include(w => w.WorkOrder.Request)
                .Include(w => w.WorkOrder.Request.AssetDetail)
                .Select(work => new WorkOrderDetails
                {

                    // from work order
                    Subject = work.WorkOrder.Subject,
                    Id = work.WorkOrderId,
                    CreatedById = work.CreatedById,
                    CreatedBy = work.User.UserName,
                    CreationDate = work.CreationDate,
                    WorkOrderTrackingId = work.WorkOrder.Id, //trackingId
                    WorkOrderNumber = work.WorkOrder.WorkOrderNumber,
                    PlannedStartDate = work.WorkOrder.PlannedStartDate,
                    PlannedEndDate = work.WorkOrder.PlannedEndDate,
                    ActualStartDate = work.WorkOrder.ActualStartDate,
                    ActualEndDate = work.WorkOrder.ActualEndDate,
                    Note = work.WorkOrder.Note,
                    WorkOrderPeriorityId = work.WorkOrder.WorkOrderPeriorityId,
                    WorkOrderPeriorityName = work.WorkOrder.WorkOrderPeriority.Name,
                    WorkOrderPeriorityNameAr = work.WorkOrder.WorkOrderPeriority.NameAr,
                    WorkOrderTypeId = work.WorkOrder.WorkOrderTypeId,
                    WorkOrderTypeName = work.WorkOrder.WorkOrderType.Name,
                    RequestId = work.WorkOrder.RequestId,
                    RequestSubject = work.WorkOrder.Request.Subject,
                    AssetSerial = work.WorkOrder.Request.AssetDetail.SerialNumber,
                    MasterAssetId = (int)work.WorkOrder.Request.AssetDetail.MasterAssetId,
                    LstWorkOrderTracking = lstTracking
                }).FirstOrDefault();



            return workOrderDetails;
        }

        public IEnumerable<LstWorkOrderFromTracking> GetAllWorkOrderFromTrackingByServiceRequestId(int ServiceRequestId, string userId)
        {
            ApplicationUser UserObj = new ApplicationUser();
            ApplicationRole roleObj = new ApplicationRole();
            string userRoleName = "";
            var lstUsers = _context.ApplicationUser.Where(a => a.Id == userId).ToList();
            if (lstUsers.Count > 0)
            {
                UserObj = lstUsers[0];

                var lstRoles = _context.ApplicationRole.Where(a => a.Id == UserObj.RoleId).ToList();
                if (lstRoles.Count > 0)
                {
                    roleObj = lstRoles[0];
                    userRoleName = roleObj.Name;
                }
            }
            List<LstWorkOrderFromTracking> lstWorkOrderTrackingVM = new List<LstWorkOrderFromTracking>();
            var ListWorkOrderFromTracking = _context.WorkOrderTrackings.Where(w => w.WorkOrder.RequestId == ServiceRequestId)
                .Include(w => w.WorkOrderStatus)
                .Include(w => w.WorkOrder).Include(w => w.WorkOrder.WorkOrderPeriority)
                .Include(w => w.WorkOrder.WorkOrderType).Include(w => w.WorkOrder.Request)
                .Include(w => w.WorkOrder.Request.AssetDetail).ToList()
                //.Include(w => w.WorkOrder.Request.AssetDetail.MasterAsset)
                .Select(work => new LstWorkOrderFromTracking
                {
                    Id = work.Id,  //trackingId
                    WorkOrderDate = work.WorkOrderDate,
                    CreationDate = work.CreationDate,
                    Notes = work.Notes,
                    CreatedById = work.CreatedById,
                    CreatedBy = _context.ApplicationUser.Where(a => a.Id == work.CreatedById).ToList().FirstOrDefault().UserName,
                    WorkOrderStatusId = work.WorkOrderStatusId,
                    WorkOrderStatusName = work.WorkOrderStatus.Name,
                    AssignedTo = work.AssignedTo,
                    AssignedToName = _context.ApplicationUser.Where(a => a.Id == work.AssignedTo).ToList().FirstOrDefault().UserName,
                    // from work order
                    Subject = work.WorkOrder.Subject,
                    WorkOrderId = work.WorkOrderId,
                    WorkOrderNumber = work.WorkOrder.WorkOrderNumber,
                    PlannedStartDate = work.WorkOrder.PlannedStartDate,
                    PlannedEndDate = work.WorkOrder.PlannedEndDate,
                    ActualStartDate = work.WorkOrder.ActualStartDate,
                    ActualEndDate = work.WorkOrder.ActualEndDate,
                    Note = work.WorkOrder.Note,
                    WorkOrderPeriorityId = work.WorkOrder.WorkOrderPeriorityId,
                    WorkOrderPeriorityName = work.WorkOrder.WorkOrderPeriority.Name,
                    WorkOrderTypeId = work.WorkOrder.WorkOrderTypeId,
                    WorkOrderTypeName = work.WorkOrder.WorkOrderType.Name,
                    RequestId = work.WorkOrder.RequestId,
                    RequestSubject = work.WorkOrder.Request.Subject,
                    SerialNumber = work.WorkOrder.Request.AssetDetail.SerialNumber,
                    WorkOrderSubject = work.WorkOrder.Request.Subject,

                    HospitalId = (int)work.User.HospitalId,
                    GovernorateId = (int)work.User.GovernorateId,
                    CityId = (int)work.User.CityId,
                    OrganizationId = (int)work.User.OrganizationId,
                    SubOrganizationId = (int)work.User.SubOrganizationId,
                    RoleId = work.User.RoleId,
                }).ToList().OrderByDescending(a => a.CreationDate).GroupBy(w => w.WorkOrderId);


            foreach (var item in ListWorkOrderFromTracking)
            {
                lstWorkOrderTrackingVM.Add(item.LastOrDefault());
            }








            if (UserObj.GovernorateId > 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
            {
                lstWorkOrderTrackingVM = lstWorkOrderTrackingVM.Where(t => t.GovernorateId == UserObj.GovernorateId).ToList();
            }
            if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId == 0)
            {
                lstWorkOrderTrackingVM = lstWorkOrderTrackingVM.Where(t => t.CityId == UserObj.CityId).ToList();
            }
            if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId == 0 && UserObj.HospitalId == 0)
            {
                lstWorkOrderTrackingVM = lstWorkOrderTrackingVM.Where(t => t.OrganizationId == UserObj.OrganizationId).ToList();
            }
            if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId == 0)
            {
                lstWorkOrderTrackingVM = lstWorkOrderTrackingVM.Where(t => t.SubOrganizationId == UserObj.SubOrganizationId).ToList();
            }



            if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId > 0)
            {

                if (userRoleName == "Admin")
                {
                    lstWorkOrderTrackingVM = lstWorkOrderTrackingVM.ToList();
                }
                if (userRoleName == "Eng")
                {
                    var lstAssigned = lstWorkOrderTrackingVM.Where(t => t.HospitalId == UserObj.HospitalId && t.AssignedTo == userId).ToList();
                    var lstCreated = lstWorkOrderTrackingVM.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();


                    lstWorkOrderTrackingVM = lstAssigned.Concat(lstCreated).ToList();
                }


                if (userRoleName == "EngDepManager")
                {
                    lstWorkOrderTrackingVM = lstWorkOrderTrackingVM.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }


                if (userRoleName == "TLHospitalManager")
                {
                    lstWorkOrderTrackingVM = lstWorkOrderTrackingVM.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                if (userRoleName == "AssetOwner")
                {
                    lstWorkOrderTrackingVM = lstWorkOrderTrackingVM.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                }
                if (userRoleName == "DE")
                {
                    lstWorkOrderTrackingVM = lstWorkOrderTrackingVM.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                }

            }
            if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId > 0)
            {
                // lstWorkOrderTrackingVM = lstWorkOrderTrackingVM.Where(t => t.HospitalId == UserObj.HospitalId).ToList();

                if (userRoleName == "Admin")
                {
                    lstWorkOrderTrackingVM = lstWorkOrderTrackingVM.ToList();
                }
                if (userRoleName == "TLHospitalManager")
                {
                    lstWorkOrderTrackingVM = lstWorkOrderTrackingVM.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                if (userRoleName == "AssetOwner")
                {
                    lstWorkOrderTrackingVM = lstWorkOrderTrackingVM.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                }
                if (userRoleName == "DE")
                {
                    lstWorkOrderTrackingVM = lstWorkOrderTrackingVM.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                }
            }


            return lstWorkOrderTrackingVM;
        }

        public IEnumerable<LstWorkOrderFromTracking> GetAllWorkOrderFromTrackingByServiceRequestUserId(int ServiceRequestId, string userId)
        {
            ApplicationUser UserObj = new ApplicationUser();
            ApplicationRole roleObj = new ApplicationRole();
            string userRoleName = "";
            var lstUsers = _context.ApplicationUser.Where(a => a.Id == userId).ToList();
            if (lstUsers.Count > 0)
            {
                UserObj = lstUsers[0];

                var lstRoles = _context.ApplicationRole.Where(a => a.Id == UserObj.RoleId).ToList();
                if (lstRoles.Count > 0)
                {
                    roleObj = lstRoles[0];
                    userRoleName = roleObj.Name;
                }
            }
            //  List<LstWorkOrderFromTracking> lstWorkOrderTrackingVM = new List<LstWorkOrderFromTracking>();

            List<LstWorkOrderFromTracking> list = new List<LstWorkOrderFromTracking>();
            var ListWorkOrderFromTracking = _context.WorkOrderTrackings.Where(w => w.WorkOrder.RequestId == ServiceRequestId)
                                                .Include(w => w.WorkOrderStatus)
                                                .Include(w => w.WorkOrder).Include(w => w.WorkOrder.WorkOrderPeriority)
                                                .Include(w => w.WorkOrder.WorkOrderType).Include(w => w.WorkOrder.Request)
                                                .Include(w => w.WorkOrder.Request.AssetDetail).ToList();

            foreach (var item in ListWorkOrderFromTracking)
            {
                LstWorkOrderFromTracking getDataObj = new LstWorkOrderFromTracking();

                getDataObj.Id = item.Id;  //trackingId
                getDataObj.WorkOrderDate = item.WorkOrderDate;
                getDataObj.CreationDate = item.CreationDate;
                getDataObj.Notes = item.Notes;
                getDataObj.CreatedById = item.CreatedById;
                var lstCreatedByUsers = _context.ApplicationUser.Where(a => a.Id == item.CreatedById).ToList();
                if (lstCreatedByUsers.Count > 0)
                {
                    getDataObj.CreatedBy = lstCreatedByUsers[0].UserName;
                }

                getDataObj.WorkOrderStatusId = item.WorkOrderStatusId;
                getDataObj.WorkOrderStatusName = item.WorkOrderStatus.Name;
                getDataObj.AssignedTo = item.AssignedTo;
                var lstAssignedToUsers = _context.ApplicationUser.Where(a => a.Id == item.AssignedTo).ToList();
                if (lstAssignedToUsers.Count > 0)
                {
                    getDataObj.AssignedToName = lstAssignedToUsers[0].UserName;
                }


                getDataObj.Subject = item.WorkOrder.Subject;
                getDataObj.WorkOrderId = item.WorkOrderId;
                getDataObj.WorkOrderNumber = item.WorkOrder.WorkOrderNumber;


                getDataObj.PlannedStartDate = item.WorkOrder.PlannedStartDate;
                getDataObj.PlannedEndDate = item.WorkOrder.PlannedEndDate;
                getDataObj.ActualStartDate = item.WorkOrder.ActualStartDate;
                getDataObj.ActualEndDate = item.WorkOrder.ActualEndDate;
                getDataObj.Note = item.WorkOrder.Note;
                getDataObj.WorkOrderPeriorityId = item.WorkOrder.WorkOrderPeriorityId;
                getDataObj.WorkOrderPeriorityName = item.WorkOrder.WorkOrderPeriority.Name;
                getDataObj.WorkOrderTypeId = item.WorkOrder.WorkOrderTypeId;
                getDataObj.WorkOrderTypeName = item.WorkOrder.WorkOrderType.Name;
                getDataObj.RequestId = item.WorkOrder.RequestId;
                getDataObj.RequestSubject = item.WorkOrder.Request.Subject;
                getDataObj.SerialNumber = item.WorkOrder.Request.AssetDetail.SerialNumber;
                getDataObj.WorkOrderSubject = item.WorkOrder.Request.Subject;
                getDataObj.HospitalId = (int)item.User.HospitalId;
                getDataObj.GovernorateId = (int)item.User.GovernorateId;
                getDataObj.CityId = (int)item.User.CityId;
                getDataObj.OrganizationId = (int)item.User.OrganizationId;
                getDataObj.SubOrganizationId = (int)item.User.SubOrganizationId;
                getDataObj.RoleId = item.User.RoleId;
                list.Add(getDataObj);
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

                if (userRoleName == "Admin")
                {
                    list = list.ToList();
                }

                if (userRoleName == "Eng")
                {
                    var lstAssigned = list.Where(t => t.HospitalId == UserObj.HospitalId && t.AssignedTo == userId).ToList();
                    var lstCreated = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();


                    list = lstAssigned.Concat(lstCreated).ToList();
                }


                if (userRoleName == "EngDepManager")
                {
                    //var lstAssigned = list.Where(t => t.HospitalId == UserObj.HospitalId && t.AssignedTo == userId).ToList();
                    //var lstCreated = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();

                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }


                if (userRoleName == "TLHospitalManager")
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                if (userRoleName == "AssetOwner")
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                }
                if (userRoleName == "DE")
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                }

            }
            if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId > 0)
            {
                // list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();

                if (userRoleName == "Admin")
                {
                    list = list.ToList();
                }
                if (userRoleName == "TLHospitalManager")
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                if (userRoleName == "AssetOwner")
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                }
                if (userRoleName == "DE")
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                }
            }


            return list;
        }

        public IEnumerable<LstWorkOrderFromTracking> GetAllWorkOrderFromTrackingByUserId(string userId)
        {
            ApplicationUser UserObj = new ApplicationUser();
            ApplicationRole roleObj = new ApplicationRole();
            string userRoleName = "";
            var lstUsers = _context.ApplicationUser.Where(a => a.Id == userId).ToList();
            if (lstUsers.Count > 0)
            {
                UserObj = lstUsers[0];

                var lstRoles = _context.ApplicationRole.Where(a => a.Id == UserObj.RoleId).ToList();
                if (lstRoles.Count > 0)
                {
                    roleObj = lstRoles[0];
                    userRoleName = roleObj.Name;
                }
            }
            List<LstWorkOrderFromTracking> lstWorkOrderTrackingVM = new List<LstWorkOrderFromTracking>();
            var ListWorkOrderFromTracking = _context.WorkOrderTrackings
                .Include(w => w.WorkOrderStatus)
                .Include(w => w.WorkOrder).Include(w => w.WorkOrder.WorkOrderPeriority)
                .Include(w => w.WorkOrder.WorkOrderType).Include(w => w.WorkOrder.Request)
                .Include(w => w.WorkOrder.Request.AssetDetail).ToList()
                //.Include(w => w.WorkOrder.Request.AssetDetail.MasterAsset)
                .Select(work => new LstWorkOrderFromTracking
                {
                    Id = work.Id,  //trackingId
                    WorkOrderDate = work.WorkOrderDate,
                    CreationDate = work.CreationDate,
                    Notes = work.Notes,
                    CreatedById = work.CreatedById,
                    CreatedBy = _context.ApplicationUser.Where(a => a.Id == work.CreatedById).ToList().FirstOrDefault().UserName,
                    WorkOrderStatusId = work.WorkOrderStatusId,
                    WorkOrderStatusName = work.WorkOrderStatus.Name,
                    AssignedTo = work.AssignedTo,
                    AssignedToName = _context.ApplicationUser.Where(a => a.Id == work.AssignedTo).ToList().FirstOrDefault().UserName,
                    // from work order
                    Subject = work.WorkOrder.Subject,
                    WorkOrderId = work.WorkOrderId,
                    WorkOrderNumber = work.WorkOrder.WorkOrderNumber,
                    PlannedStartDate = work.WorkOrder.PlannedStartDate,
                    PlannedEndDate = work.WorkOrder.PlannedEndDate,
                    ActualStartDate = work.WorkOrder.ActualStartDate,
                    ActualEndDate = work.WorkOrder.ActualEndDate,
                    Note = work.WorkOrder.Note,
                    WorkOrderPeriorityId = work.WorkOrder.WorkOrderPeriorityId,
                    WorkOrderPeriorityName = work.WorkOrder.WorkOrderPeriority.Name,
                    WorkOrderTypeId = work.WorkOrder.WorkOrderTypeId,
                    WorkOrderTypeName = work.WorkOrder.WorkOrderType.Name,
                    RequestId = work.WorkOrder.RequestId,
                    RequestSubject = work.WorkOrder.Request.Subject,
                    SerialNumber = work.WorkOrder.Request.AssetDetail.SerialNumber,
                    WorkOrderSubject = work.WorkOrder.Request.Subject,

                    HospitalId = (int)work.User.HospitalId,
                    GovernorateId = (int)work.User.GovernorateId,
                    CityId = (int)work.User.CityId,
                    OrganizationId = (int)work.User.OrganizationId,
                    SubOrganizationId = (int)work.User.SubOrganizationId,
                    RoleId = work.User.RoleId,
                }).ToList().OrderByDescending(a => a.CreationDate).GroupBy(w => w.WorkOrderId);


            foreach (var item in ListWorkOrderFromTracking)
            {
                lstWorkOrderTrackingVM.Add(item.LastOrDefault());
            }
            if (UserObj.GovernorateId > 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
            {
                lstWorkOrderTrackingVM = lstWorkOrderTrackingVM.Where(t => t.GovernorateId == UserObj.GovernorateId).ToList();
            }
            if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId == 0)
            {
                lstWorkOrderTrackingVM = lstWorkOrderTrackingVM.Where(t => t.CityId == UserObj.CityId).ToList();
            }
            if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId == 0 && UserObj.HospitalId == 0)
            {
                lstWorkOrderTrackingVM = lstWorkOrderTrackingVM.Where(t => t.OrganizationId == UserObj.OrganizationId).ToList();
            }
            if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId == 0)
            {
                lstWorkOrderTrackingVM = lstWorkOrderTrackingVM.Where(t => t.SubOrganizationId == UserObj.SubOrganizationId).ToList();
            }
            if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId > 0)
            {
                if (userRoleName == "Admin")
                {
                    lstWorkOrderTrackingVM = lstWorkOrderTrackingVM.ToList();
                }
                if (userRoleName == "Eng")
                {
                    var lstAssigned = lstWorkOrderTrackingVM.Where(t => t.HospitalId == UserObj.HospitalId && t.AssignedTo == userId).ToList();
                    var lstCreated = lstWorkOrderTrackingVM.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                    lstWorkOrderTrackingVM = lstAssigned.Concat(lstCreated).ToList();
                }
                if (userRoleName == "EngDepManager")
                {
                    lstWorkOrderTrackingVM = lstWorkOrderTrackingVM.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                if (userRoleName == "TLHospitalManager")
                {
                    lstWorkOrderTrackingVM = lstWorkOrderTrackingVM.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                if (userRoleName == "AssetOwner")
                {
                    lstWorkOrderTrackingVM = lstWorkOrderTrackingVM.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                }
                if (userRoleName == "DE")
                {
                    lstWorkOrderTrackingVM = lstWorkOrderTrackingVM.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                }
            }
            if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId > 0)
            {
                // lstWorkOrderTrackingVM = lstWorkOrderTrackingVM.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                if (userRoleName == "Admin")
                {
                    lstWorkOrderTrackingVM = lstWorkOrderTrackingVM.ToList();
                }

                if (userRoleName == "TLHospitalManager")
                {
                    lstWorkOrderTrackingVM = lstWorkOrderTrackingVM.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                if (userRoleName == "AssetOwner")
                {
                    lstWorkOrderTrackingVM = lstWorkOrderTrackingVM.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                }
                if (userRoleName == "DE")
                {
                    lstWorkOrderTrackingVM = lstWorkOrderTrackingVM.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                }
            }


            return lstWorkOrderTrackingVM;
        }

        public List<IndexWorkOrderTrackingVM> GetAllWorkOrderTrackingByWorkOrderId(int WorkOrderId)
        {
            return _context.WorkOrderTrackings.Where(r => r.WorkOrderId == WorkOrderId)
                .Include(w => w.WorkOrderStatus)

                .Select(work => new IndexWorkOrderTrackingVM
                {
                    TrackId = work.Id,
                    WorkOrderDate = work.WorkOrderDate,
                    CreationDate = work.CreationDate,
                    AssignedTo = work.AssignedTo,
                    Notes = work.Notes,
                    CreatedById = work.CreatedById,
                    CreatedBy = work.User.UserName,
                    WorkOrderStatusId = work.WorkOrderStatusId,
                    WorkOrderStatusName = work.WorkOrderStatus.Name,
                    WorkOrderStatusNameAr = work.WorkOrderStatus.NameAr,
                    WorkOrderSubject = work.WorkOrder.Subject,

                    CreatedToId = _context.WorkOrderAssigns.Where(a => a.WOTId == work.Id).FirstOrDefault().UserId,
                    CreatedTo = _context.WorkOrderAssigns.Where(a => a.WOTId == work.Id).FirstOrDefault().User.UserName,

                }).OrderByDescending(a => a.CreationDate).ToList();
        }

        public List<WorkOrderAttachment> GetAttachmentsByWorkOrderId(int id)
        {
            return _context.WorkOrderAttachments.Where(a => a.WorkOrderTrackingId == id).ToList();
        }

        public IndexWorkOrderTrackingVM GetById(int id)
        {
            var WorkOrderFromTracking = _context.WorkOrderTrackings.Include(w => w.WorkOrderStatus)
               .Include(w => w.WorkOrder).Include(w => w.WorkOrder.WorkOrderPeriority)
               .Include(w => w.WorkOrder.WorkOrderType).Include(w => w.WorkOrder.Request)
               .Select(work => new IndexWorkOrderTrackingVM
               {
                   Id = work.Id,  //trackingId
                   WorkOrderDate = work.WorkOrderDate,
                   CreationDate = work.CreationDate,
                   Notes = work.Notes,
                   CreatedById = work.CreatedById,
                   CreatedBy = work.User.UserName,
                   WorkOrderStatusId = work.WorkOrderStatusId,
                   WorkOrderStatusName = work.WorkOrderStatus.Name,
                   // from work order
                   WorkOrderId = work.WorkOrderId,
               }).FirstOrDefault();
            return WorkOrderFromTracking;
        }

        public LstWorkOrderFromTracking GetEngManagerWhoFirstAssignedWO(int woId)
        {
            var lstWO = _context.WorkOrderTrackings
                            .Where(a => a.WorkOrderId == woId).ToList().OrderBy(a => a.WorkOrderDate).ToList().Select(wo => new LstWorkOrderFromTracking
                            {
                                Id = wo.Id,
                                CreationDate = wo.CreationDate,
                                CreatedById = wo.CreatedById,
                                WorkOrderId = wo.WorkOrderId

                            }).ToList();
            if(lstWO.Count >0)
            {
                return lstWO.First();
            }


            return null;
        }

        public List<IndexWorkOrderTrackingVM> GetTrackOfWorkOrderByWorkOrderId(int workOrderId)
        {
            var lstTracks = _context.WorkOrderTrackings.Where(r => r.WorkOrderId == workOrderId)
                      .Include(w => w.WorkOrderStatus).Include(w => w.User)
                      .Select(work => new IndexWorkOrderTrackingVM
                      {
                          Id=work.Id,
                          TrackId = work.Id,
                          WorkOrderDate = work.WorkOrderDate,
                          CreationDate = work.CreationDate,
                          AssignedTo = _context.ApplicationUser.Where(a => a.Id == work.AssignedTo).FirstOrDefault().UserName,
                          Notes = work.Notes,
                          CreatedById = work.CreatedById,
                          CreatedBy = work.User.UserName,
                          WorkOrderStatusId = work.WorkOrderStatusId,
                          WorkOrderStatusName = work.WorkOrderStatus.Name,
                          WorkOrderStatusNameAr = work.WorkOrderStatus.NameAr,
                          ActualStartDate = work.ActualStartDate,
                          ActualEndDate = work.ActualEndDate,
                          
                      }).ToList().OrderByDescending(a=>a.CreationDate).ToList();

            return lstTracks;
        }

        public void Update(int id, EditWorkOrderTrackingVM editWorkOrderTrackingVM)
        {
            try
            {
                WorkOrderTracking workOrderTracking = new WorkOrderTracking();
                workOrderTracking.Id = editWorkOrderTrackingVM.Id;
                workOrderTracking.WorkOrderDate = editWorkOrderTrackingVM.WorkOrderDate;
                workOrderTracking.CreationDate = editWorkOrderTrackingVM.CreationDate;
                workOrderTracking.Notes = editWorkOrderTrackingVM.Notes;
                workOrderTracking.WorkOrderStatusId = editWorkOrderTrackingVM.WorkOrderStatusId;
                workOrderTracking.CreatedById = editWorkOrderTrackingVM.CreatedById;
                workOrderTracking.ActualStartDate = editWorkOrderTrackingVM.ActualStartDate;
                workOrderTracking.ActualEndDate = editWorkOrderTrackingVM.ActualEndDate;


                _context.Entry(workOrderTracking).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }
    }
}
