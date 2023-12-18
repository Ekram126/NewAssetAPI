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
                    var isEmployeeId = Int32.TryParse(createWorkOrderTrackingVM.AssignedTo, out int empId);
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
                    workOrderTracking.WorkOrderDate = DateTime.Parse(createWorkOrderTrackingVM.StrWorkOrderDate.ToString()); //createWorkOrderTrackingVM.WorkOrderDate;
                    workOrderTracking.CreationDate = createWorkOrderTrackingVM.CreationDate;
                    workOrderTracking.Notes = createWorkOrderTrackingVM.Notes;
                    workOrderTracking.WorkOrderStatusId = createWorkOrderTrackingVM.WorkOrderStatusId;
                    workOrderTracking.CreatedById = createWorkOrderTrackingVM.CreatedById;
                    workOrderTracking.WorkOrderId = createWorkOrderTrackingVM.WorkOrderId;
                    workOrderTracking.HospitalId = createWorkOrderTrackingVM.HospitalId;
                    if (assignedTo != "")
                    {
                        workOrderTracking.AssignedTo = assignedTo;
                        //workOrderTracking.CreatedById = createWorkOrderTrackingVM.CreatedById;
                    }
                    else
                    {
                        //  workOrderTracking.AssignedTo = createWorkOrderTrackingVM.AssignedTo;
                        workOrderTracking.AssignedTo = createWorkOrderTrackingVM.CreatedById;

                    }


                    workOrderTracking.ActualStartDate = createWorkOrderTrackingVM.ActualStartDate != "" ? DateTime.Parse(createWorkOrderTrackingVM.ActualStartDate) : null;
                    workOrderTracking.ActualEndDate = createWorkOrderTrackingVM.ActualEndDate != "" ? DateTime.Parse(createWorkOrderTrackingVM.ActualEndDate) : null;

                    workOrderTracking.PlannedStartDate = createWorkOrderTrackingVM.PlannedStartDate != "" ? DateTime.Parse(createWorkOrderTrackingVM.PlannedStartDate) : null;
                    workOrderTracking.PlannedEndDate = createWorkOrderTrackingVM.PlannedEndDate != "" ? DateTime.Parse(createWorkOrderTrackingVM.PlannedEndDate) : null;

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

        public IEnumerable<WorkOrderTracking> GetAll()
        {
            return _context.WorkOrderTrackings.ToList();
        }

        public WorkOrderDetails GetAllWorkOrderByWorkOrderId(int WorkOrderId)
        {
            var lstTracking = _context.WorkOrderTrackings.Where(r => r.WorkOrderId == WorkOrderId)
                .Include(w => w.WorkOrderStatus)
                .Select(work => new IndexWorkOrderTrackingVM
                {
                    TrackId = work.Id,
                    WorkOrderDate = DateTime.Parse(work.WorkOrderDate.ToString()),
                    CreationDate = DateTime.Parse(work.CreationDate.ToString()),
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
                    CreationDate = DateTime.Parse(work.CreationDate.ToString()),
                    WorkOrderTrackingId = work.WorkOrder.Id, //trackingId
                    WorkOrderNumber = work.WorkOrder.WorkOrderNumber,
                    PlannedStartDate = DateTime.Parse(work.WorkOrder.PlannedStartDate.ToString()),
                    PlannedEndDate = DateTime.Parse(work.WorkOrder.PlannedEndDate.ToString()),
                    ActualStartDate = DateTime.Parse(work.WorkOrder.ActualStartDate.ToString()),
                    ActualEndDate = DateTime.Parse(work.WorkOrder.ActualEndDate.ToString()),
                    Note = work.WorkOrder.Note,
                    WorkOrderPeriorityId = work.WorkOrder.WorkOrderPeriorityId != null ? (int)work.WorkOrder.WorkOrderPeriorityId : 0,
                    WorkOrderPeriorityName = work.WorkOrder.WorkOrderPeriority.Name,
                    WorkOrderPeriorityNameAr = work.WorkOrder.WorkOrderPeriority.NameAr,
                    WorkOrderTypeId = work.WorkOrder.WorkOrderPeriorityId != null ? (int)work.WorkOrder.WorkOrderTypeId : 0,
                    WorkOrderTypeName = work.WorkOrder.WorkOrderType.Name,
                    RequestId = work.WorkOrder.WorkOrderPeriorityId != null ? (int)work.WorkOrder.RequestId : 0,
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
                    WorkOrderDate = DateTime.Parse(work.WorkOrderDate.ToString()),
                    CreationDate = DateTime.Parse(work.CreationDate.ToString()),
                    Notes = work.Notes,
                    CreatedById = work.CreatedById,
                    CreatedBy = _context.ApplicationUser.Where(a => a.Id == work.CreatedById).ToList().FirstOrDefault().UserName,
                    WorkOrderStatusId = work.WorkOrderStatusId,
                    WorkOrderStatusName = work.WorkOrderStatus.Name,
                    WorkOrderStatusNameAr = work.WorkOrderStatus.NameAr,
                    AssignedTo = work.AssignedTo,
                    AssignedToName = _context.ApplicationUser.Where(a => a.Id == work.AssignedTo).ToList().FirstOrDefault().UserName,
                    // from work order
                    Subject = work.WorkOrder.Subject,
                    WorkOrderId = work.WorkOrderId,
                    WorkOrderNumber = work.WorkOrder.WorkOrderNumber,
                    PlannedStartDate = DateTime.Parse(work.WorkOrder.PlannedStartDate.ToString()),
                    PlannedEndDate = DateTime.Parse(work.WorkOrder.PlannedEndDate.ToString()),
                    ActualStartDate = DateTime.Parse(work.WorkOrder.ActualStartDate.ToString()),
                    ActualEndDate = DateTime.Parse(work.WorkOrder.ActualEndDate.ToString()),
                    Note = work.WorkOrder.Note,
                    WorkOrderPeriorityId = work.WorkOrder.WorkOrderPeriorityId != null ? (int)work.WorkOrder.WorkOrderPeriorityId : 0,
                    WorkOrderPeriorityName = work.WorkOrder.WorkOrderPeriorityId != null ? work.WorkOrder.WorkOrderPeriority.Name : "",
                    WorkOrderPeriorityNameAr = work.WorkOrder.WorkOrderPeriorityId != null ? work.WorkOrder.WorkOrderPeriority.NameAr : "",
                    WorkOrderTypeId = work.WorkOrder.WorkOrderTypeId != null ? (int)work.WorkOrder.WorkOrderTypeId : 0,
                    WorkOrderTypeName = work.WorkOrder.WorkOrderType.Name,
                    RequestId = work.WorkOrder.RequestId != null ? (int)work.WorkOrder.RequestId : 0,
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
                    lstWorkOrderTrackingVM = lstWorkOrderTrackingVM.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                    //lstWorkOrderTrackingVM = lstWorkOrderTrackingVM.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
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
                    lstWorkOrderTrackingVM = lstWorkOrderTrackingVM.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                    // lstWorkOrderTrackingVM = lstWorkOrderTrackingVM.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
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
                                                .Include(w => w.WorkOrder.Request.AssetDetail).OrderByDescending(a => a.CreationDate).ToList();

            foreach (var item in ListWorkOrderFromTracking)
            {
                LstWorkOrderFromTracking getDataObj = new LstWorkOrderFromTracking();

                getDataObj.Id = item.Id;  //trackingId
                getDataObj.WorkOrderDate = DateTime.Parse(item.WorkOrderDate.ToString());
                getDataObj.CreationDate = DateTime.Parse(item.CreationDate.ToString());
                getDataObj.Notes = item.Notes;
                getDataObj.CreatedById = item.CreatedById;
                var lstCreatedByUsers = _context.ApplicationUser.Where(a => a.Id == item.CreatedById).ToList();
                if (lstCreatedByUsers.Count > 0)
                {
                    getDataObj.CreatedBy = lstCreatedByUsers[0].UserName;
                }

                getDataObj.WorkOrderStatusId = item.WorkOrderStatusId;
                getDataObj.WorkOrderStatusName = item.WorkOrderStatus.Name;
                getDataObj.WorkOrderStatusNameAr = item.WorkOrderStatus.NameAr;
                getDataObj.WorkOrderStatusIcon = item.WorkOrderStatus.Icon;
                getDataObj.WorkOrderStatusColor = item.WorkOrderStatus.Color;
                getDataObj.AssignedTo = item.AssignedTo;
                var lstAssignedToUsers = _context.ApplicationUser.Where(a => a.Id == item.AssignedTo).ToList();
                if (lstAssignedToUsers.Count > 0)
                {
                    getDataObj.AssignedToName = lstAssignedToUsers[0].UserName;
                }


                getDataObj.Subject = item.WorkOrder.Subject;
                getDataObj.WorkOrderId = item.WorkOrderId;
                getDataObj.WorkOrderNumber = item.WorkOrder.WorkOrderNumber;


                getDataObj.PlannedStartDate = DateTime.Parse(item.WorkOrder.PlannedStartDate.ToString());
                getDataObj.PlannedEndDate = DateTime.Parse(item.WorkOrder.PlannedEndDate.ToString());
                getDataObj.ActualStartDate = DateTime.Parse(item.WorkOrder.ActualStartDate.ToString());
                getDataObj.ActualEndDate = DateTime.Parse(item.WorkOrder.ActualEndDate.ToString());
                getDataObj.Note = item.WorkOrder.Note;
                getDataObj.WorkOrderPeriorityId = item.WorkOrder.WorkOrderPeriorityId != null ? (int)item.WorkOrder.WorkOrderPeriorityId : 0;
                getDataObj.WorkOrderPeriorityName = item.WorkOrder.WorkOrderPeriority.Name;

                getDataObj.WorkOrderPeriorityNameAr = item.WorkOrder.WorkOrderPeriority.NameAr;
                //getDataObj.WorkOrderPeriorityIcon = item.WorkOrder.WorkOrderPeriority.;
                //getDataObj.WorkOrderPeriorityColor = item.WorkOrder.WorkOrderPeriority.Color;



                getDataObj.WorkOrderTypeId = item.WorkOrder.WorkOrderTypeId != null ? (int)item.WorkOrder.WorkOrderTypeId : 0;
                getDataObj.WorkOrderTypeName = item.WorkOrder.WorkOrderType.Name;
                getDataObj.RequestId = item.WorkOrder.RequestId != null ? (int)item.WorkOrder.RequestId : 0;
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
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();

                    // list = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
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
                    WorkOrderDate = DateTime.Parse(work.WorkOrderDate.ToString()),
                    CreationDate = DateTime.Parse(work.CreationDate.ToString()),
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
                    PlannedStartDate = DateTime.Parse(work.WorkOrder.PlannedStartDate.ToString()),
                    PlannedEndDate = DateTime.Parse(work.WorkOrder.PlannedEndDate.ToString()),
                    ActualStartDate = DateTime.Parse(work.WorkOrder.ActualStartDate.ToString()),
                    ActualEndDate = DateTime.Parse(work.WorkOrder.ActualEndDate.ToString()),
                    Note = work.WorkOrder.Note,
                    WorkOrderPeriorityId = work.WorkOrder.WorkOrderPeriorityId != null ? (int)work.WorkOrder.WorkOrderPeriorityId : 0,
                    WorkOrderPeriorityName = work.WorkOrder.WorkOrderPeriority.Name,
                    WorkOrderTypeId = work.WorkOrder.WorkOrderTypeId != null ? (int)work.WorkOrder.WorkOrderTypeId : 0,
                    WorkOrderTypeName = work.WorkOrder.WorkOrderType.Name,
                    RequestId = work.WorkOrder.RequestId != null ? (int)work.WorkOrder.RequestId : 0,
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
            List<IndexWorkOrderTrackingVM> list = new List<IndexWorkOrderTrackingVM>();
            var lstTracks = _context.WorkOrderTrackings.Where(r => r.WorkOrderId == WorkOrderId)
               .Include(w => w.WorkOrder).Include(w => w.WorkOrderStatus).Include(w => w.User).OrderByDescending(a => a.CreationDate).ToList();
            if (lstTracks.Count > 0)
            {
                foreach (var work in lstTracks)
                {
                    IndexWorkOrderTrackingVM workOrderTrackingObj = new IndexWorkOrderTrackingVM();
                    workOrderTrackingObj.TrackId = work.Id;
                    workOrderTrackingObj.WorkOrderDate = DateTime.Parse(work.WorkOrderDate.ToString());
                    workOrderTrackingObj.CreationDate = DateTime.Parse(work.CreationDate.ToString());
                    workOrderTrackingObj.AssignedTo = work.AssignedTo;
                    workOrderTrackingObj.Notes = work.Notes;
                    workOrderTrackingObj.CreatedById = work.CreatedById;
                    workOrderTrackingObj.CreatedBy = work.User.UserName;
                    workOrderTrackingObj.WorkOrderStatusId = work.WorkOrderStatusId;
                    workOrderTrackingObj.WorkOrderStatusName = work.WorkOrderStatus.Name;
                    workOrderTrackingObj.WorkOrderStatusNameAr = work.WorkOrderStatus.NameAr;
                    workOrderTrackingObj.WorkOrderSubject = work.WorkOrder.Subject;
                    var lstAssigndUsers = _context.WorkOrderAssigns.Where(a => a.WOTId == work.Id).ToList();
                    if (lstAssigndUsers.Count > 0)
                    {
                        workOrderTrackingObj.CreatedToId = lstAssigndUsers[0].UserId;
                        workOrderTrackingObj.CreatedTo = lstAssigndUsers[0].User.UserName;
                    }
                    list.Add(workOrderTrackingObj);
                }
            }
            return list;
        }

        public List<WorkOrderAttachment> GetAttachmentsByWorkOrderId(int id)
        {
            return _context.WorkOrderAttachments.Where(a => a.WorkOrderTrackingId == id).ToList();
        }

        public IndexWorkOrderTrackingVM GetById(int id)
        {
            IndexWorkOrderTrackingVM work = new IndexWorkOrderTrackingVM();
            var WorkOrderFromTracking = _context.WorkOrderTrackings.Include(w => w.WorkOrderStatus)
               .Include(w => w.WorkOrder).Include(w => w.WorkOrder.WorkOrderPeriority).Include(w => w.User)
               .Include(w => w.WorkOrder.WorkOrderType).Include(w => w.WorkOrder.Request).Where(a => a.Id == id).ToList();
            if (WorkOrderFromTracking.Count > 0)
            {

                var trackObj = WorkOrderFromTracking[0];
                work.Id = trackObj.Id;
                work.WorkOrderDate = DateTime.Parse(trackObj.WorkOrderDate.ToString());
                work.CreationDate = DateTime.Parse(trackObj.CreationDate.ToString());
                work.Notes = trackObj.Notes;
                work.CreatedById = trackObj.CreatedById;
                work.CreatedBy = trackObj.User.UserName;
                work.WorkOrderStatusId = trackObj.WorkOrderStatusId;
                work.WorkOrderStatusName = trackObj.WorkOrderStatus.Name;
                work.WorkOrderId = trackObj.WorkOrderId;

            }
            return work;
        }

        public LstWorkOrderFromTracking GetEngManagerWhoFirstAssignedWO(int woId)
        {
            var lstWO = _context.WorkOrderTrackings
                            .Where(a => a.WorkOrderId == woId).ToList().OrderBy(a => a.WorkOrderDate).ToList().Select(wo => new LstWorkOrderFromTracking
                            {
                                Id = wo.Id,
                                CreationDate = DateTime.Parse(wo.CreationDate.ToString()),
                                CreatedById = wo.CreatedById,
                                WorkOrderId = wo.WorkOrderId

                            }).ToList();
            if (lstWO.Count > 0)
            {
                return lstWO.First();
            }


            return null;
        }

        public WorkOrderTracking GetFirstTrackForWorkOrderByWorkOrderId(int woId)
        {
            WorkOrderTracking trackingObj = new WorkOrderTracking();
            var lstTracks = _context.WorkOrderTrackings.Where(r => r.WorkOrderId == woId).ToList();
            if (lstTracks.Count > 0)
            {
                trackingObj = lstTracks[0];
            }

            return trackingObj;
        }

        public List<IndexWorkOrderTrackingVM> GetTrackOfWorkOrderByWorkOrderId(int workOrderId)
        {
            List<IndexWorkOrderTrackingVM> lstTracks = new List<IndexWorkOrderTrackingVM>();

            var lstTracks2 = _context.WorkOrderTrackings.Where(r => r.WorkOrderId == workOrderId)
                      .Include(w => w.WorkOrderStatus).Include(w => w.User).OrderByDescending(a => a.CreationDate).ToList();

            foreach (var work in lstTracks2)
            {
                IndexWorkOrderTrackingVM item = new IndexWorkOrderTrackingVM();
                item.Id = work.Id;
                item.TrackId = work.Id;
                item.WorkOrderDate = work.WorkOrderDate != null ? DateTime.Parse(work.WorkOrderDate.ToString()) : null;
                item.CreationDate = work.CreationDate != null ? DateTime.Parse(work.CreationDate.ToString()) : null;
                item.AssignedTo = work.AssignedTo != "" ? _context.ApplicationUser.Where(a => a.Id == work.AssignedTo).FirstOrDefault().UserName : "";
                item.Notes = work.Notes;
                item.CreatedById = work.CreatedById;
                item.CreatedBy = work.User.UserName;
                item.WorkOrderStatusId = work.WorkOrderStatusId;
                item.WorkOrderStatusName = work.WorkOrderStatus.Name;
                item.WorkOrderStatusNameAr = work.WorkOrderStatus.NameAr;


                item.WorkOrderStatusColor = work.WorkOrderStatus.Color;
                item.WorkOrderStatusIcon = work.WorkOrderStatus.Icon;



                item.ActualStartDate = work.ActualStartDate;
                item.ActualEndDate = work.ActualEndDate;
                lstTracks.Add(item);
            }



            //.Select(work => new IndexWorkOrderTrackingVM
            //{
            //    Id = work.Id,
            //    TrackId = work.Id,
            //    WorkOrderDate = DateTime.Parse(work.WorkOrderDate.ToString()),
            //    CreationDate = DateTime.Parse(work.CreationDate.ToString()),
            //    AssignedTo = work.AssignedTo != null ?_context.ApplicationUser.Where(a => a.Id == work.AssignedTo).FirstOrDefault().UserName:"",
            //    Notes = work.Notes,
            //    CreatedById = work.CreatedById,
            //    CreatedBy = work.User.UserName,
            //    WorkOrderStatusId = work.WorkOrderStatusId,
            //    WorkOrderStatusName = work.WorkOrderStatus.Name,
            //    WorkOrderStatusNameAr = work.WorkOrderStatus.NameAr,
            //    ActualStartDate = work.ActualStartDate,
            //    ActualEndDate = work.ActualEndDate,
            //}).ToList().OrderByDescending(a => a.CreationDate).ToList();

            return lstTracks;
        }

        public void Update(int id, EditWorkOrderTrackingVM editWorkOrderTrackingVM)
        {
            try
            {
                WorkOrderTracking workOrderTracking = new WorkOrderTracking();
                workOrderTracking.Id = editWorkOrderTrackingVM.Id;
                if (editWorkOrderTrackingVM.WorkOrderDate != null)
                    workOrderTracking.WorkOrderDate = editWorkOrderTrackingVM.WorkOrderDate;
                if (editWorkOrderTrackingVM.CreationDate != null)
                    workOrderTracking.CreationDate = editWorkOrderTrackingVM.CreationDate;
                workOrderTracking.Notes = editWorkOrderTrackingVM.Notes;
                workOrderTracking.WorkOrderStatusId = editWorkOrderTrackingVM.WorkOrderStatusId;
                workOrderTracking.CreatedById = editWorkOrderTrackingVM.CreatedById;
                if (editWorkOrderTrackingVM.ActualStartDate != null)
                    workOrderTracking.ActualStartDate = editWorkOrderTrackingVM.ActualStartDate;
                if (editWorkOrderTrackingVM.ActualEndDate != null)
                    workOrderTracking.ActualEndDate = editWorkOrderTrackingVM.ActualEndDate;
                _context.Entry(workOrderTracking).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }




        public void Update(EditWorkOrderTrackingVM editWorkOrderTrackingVM)
        {
            try
            {
                var lstTracks = _context.WorkOrderTrackings.Where(a => a.Id == editWorkOrderTrackingVM.Id).ToList();
                if (lstTracks.Count > 0)
                {
                    var trackObj = lstTracks[0];
                  //  trackObj.Id = editWorkOrderTrackingVM.Id;
                    trackObj.Notes = editWorkOrderTrackingVM.Notes;
                    _context.Entry(trackObj).State = EntityState.Modified;
                    _context.SaveChanges();
                }             
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }


    }
}
