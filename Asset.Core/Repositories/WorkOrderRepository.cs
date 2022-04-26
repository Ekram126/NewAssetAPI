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
    public class WorkOrderRepository : IWorkOrderRepository
    {
        private ApplicationDbContext _context;
        string msg;
        public WorkOrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public int Add(CreateWorkOrderVM createWorkOrderVM)
        {
            try
            {
                if (createWorkOrderVM != null)
                {
                    WorkOrder workOrder = new WorkOrder();
                    workOrder.Subject = createWorkOrderVM.Subject;
                    workOrder.WorkOrderNumber = createWorkOrderVM.WorkOrderNumber;
                    workOrder.CreationDate = DateTime.Now;
                    workOrder.PlannedStartDate = DateTime.Parse(createWorkOrderVM.PlannedStartDate);
                    workOrder.PlannedEndDate = DateTime.Parse(createWorkOrderVM.PlannedEndDate);
                    workOrder.ActualStartDate = DateTime.Parse(createWorkOrderVM.ActualStartDate);
                    workOrder.ActualEndDate = DateTime.Parse(createWorkOrderVM.ActualEndDate);
                    workOrder.Note = createWorkOrderVM.Note;
                    workOrder.CreatedById = createWorkOrderVM.CreatedById;
                    workOrder.WorkOrderPeriorityId = createWorkOrderVM.WorkOrderPeriorityId;
                    workOrder.WorkOrderTypeId = createWorkOrderVM.WorkOrderTypeId;
                    workOrder.RequestId = createWorkOrderVM.RequestId;
                    workOrder.HospitalId = createWorkOrderVM.HospitalId;
                    _context.WorkOrders.Add(workOrder);
                    _context.SaveChanges();
                    createWorkOrderVM.Id = workOrder.Id;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return createWorkOrderVM.Id;
        }

        public void Delete(int id)
        {
            var workOrder = _context.WorkOrders.Find(id);
            try
            {
                if (workOrder != null)
                {

                    _context.WorkOrders.Remove(workOrder);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }

        public GeneratedWorkOrderNumberVM GenerateWorOrderNumber()
        {
            GeneratedWorkOrderNumberVM numberObj = new GeneratedWorkOrderNumberVM();
            string WO = "WO";

            var lstIds = _context.WorkOrders.ToList();
            if (lstIds.Count > 0)
            {
                var code = lstIds.LastOrDefault().Id;
                numberObj.WONumber = WO + (code + 1);
            }
            else
            {
                numberObj.WONumber = WO + 1;
            }

            return numberObj;
        }

        public IEnumerable<IndexWorkOrderVM> GetAll()
        {
            return _context.WorkOrders.Include(w => w.Request).Include(w => w.WorkOrderType).Include(w => w.WorkOrderPeriority).Include(w => w.User)
                .Select(prob => new IndexWorkOrderVM
                {
                    Id = prob.Id,
                    Subject = prob.Subject,
                    BarCode = prob.Request.AssetDetail.Barcode,
                    WorkOrderNumber = prob.WorkOrderNumber,
                    CreationDate = prob.CreationDate,
                    PlannedStartDate = prob.PlannedStartDate,
                    PlannedEndDate = prob.PlannedEndDate,
                    ActualStartDate = prob.ActualStartDate,
                    ActualEndDate = prob.ActualEndDate,
                    Note = prob.Note,
                    CreatedById = prob.CreatedById,
                    CreatedBy = prob.User.UserName,
                    WorkOrderPeriorityId = prob.WorkOrderPeriorityId != null ? (int)prob.WorkOrderPeriorityId : 0,
                    WorkOrderPeriorityName = prob.WorkOrderPeriority.Name,
                    WorkOrderTypeId = prob.WorkOrderTypeId != null ? (int)prob.WorkOrderTypeId : 0,
                    WorkOrderTypeName = prob.WorkOrderType.Name,
                    RequestId = prob.RequestId != null ? (int)prob.RequestId : 0,
                    RequestSubject = prob.Request.Subject,

                }).OrderByDescending(o => o.CreationDate).ToList();
        }

        public IEnumerable<IndexWorkOrderVM> GetAllWorkOrdersByHospitalId(int? hospitalId, string userId)
        {
            List<IndexWorkOrderVM> list = new List<IndexWorkOrderVM>();
            ApplicationUser UserObj = new ApplicationUser();
            ApplicationRole roleObj = new ApplicationRole();
            List<string> userRoleNames = new List<string>();
            // string userRoleName = "";
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
                var lstWorkOrders = _context.WorkOrders
                          .Include(w => w.WorkOrderType)
                          .Include(w => w.WorkOrderPeriority)
                          .Include(w => w.Request)
                          .Include(w => w.Request.AssetDetail)
                             .Include(w => w.Request.AssetDetail.MasterAsset)
                          .Include(w => w.User).OrderByDescending(a => a.CreationDate).ToList().GroupBy(a => a.Id).ToList();
                foreach (var item in lstWorkOrders)
                {
                    IndexWorkOrderVM work = new IndexWorkOrderVM();
                    work.Id = item.FirstOrDefault().Id;
                    work.AssetName = item.FirstOrDefault().Request.AssetDetail.MasterAsset.Name;
                    work.AssetNameAr = item.FirstOrDefault().Request.AssetDetail.MasterAsset.NameAr;
                    work.SerialNumber = item.FirstOrDefault().Request.AssetDetail.SerialNumber;
                    work.BarCode = item.FirstOrDefault().Request.AssetDetail.Barcode;
                    work.ModelNumber = item.FirstOrDefault().Request.AssetDetail.MasterAsset.ModelNumber;


                    work.WorkOrderNumber = item.FirstOrDefault().WorkOrderNumber;

                    work.ModelNumber = item.FirstOrDefault().Request.AssetDetail.MasterAsset.ModelNumber;
                    work.Subject = item.FirstOrDefault().Subject;
                    work.RequestSubject = item.FirstOrDefault().Request.Subject;
                    work.CreationDate = item.FirstOrDefault().CreationDate;
                    work.Note = item.FirstOrDefault().Note;
                    work.CreatedById = item.FirstOrDefault().CreatedById;
                    work.CreatedBy = item.FirstOrDefault().User.UserName;
                    work.BarCode = item.FirstOrDefault().Request.AssetDetail.Barcode;
                    work.TypeName = item.FirstOrDefault().WorkOrderType != null ? item.FirstOrDefault().WorkOrderType.Name : "";
                    work.TypeNameAr = item.FirstOrDefault().WorkOrderType != null ? item.FirstOrDefault().WorkOrderType.NameAr : "";
                    work.PeriorityName = item.FirstOrDefault().WorkOrderPeriority != null ? item.FirstOrDefault().WorkOrderPeriority.Name : "";
                    work.PeriorityNameAr = item.FirstOrDefault().WorkOrderPeriority != null ? item.FirstOrDefault().WorkOrderPeriority.NameAr : "";
                    var lstAssignTo = _context.WorkOrderTrackings.Where(a => a.WorkOrderId == item.FirstOrDefault().Id).ToList().OrderByDescending(a => a.WorkOrderDate).ToList().GroupBy(a => item.FirstOrDefault().Id).ToList();

                    var lstStatus = _context.WorkOrderTrackings
                           .Include(t => t.WorkOrder).Include(t => t.WorkOrderStatus)
                           .Where(a => a.WorkOrderId == item.FirstOrDefault().Id).ToList().OrderByDescending(a => a.WorkOrderDate).ToList().GroupBy(a => item.FirstOrDefault().Id).ToList();
                    if (lstStatus.Count > 0)
                    {

                        work.AssignedTo = lstStatus[0].FirstOrDefault().AssignedTo;
                        work.WorkOrderStatusId = lstStatus[0].FirstOrDefault().WorkOrderStatus.Id;

                        if (work.WorkOrderStatusId == 3 || work.WorkOrderStatusId == 4 || work.WorkOrderStatusId == 5)
                        {
                            var pendingStatus = _context.WorkOrderStatuses.Where(a => a.Id == 6).ToList().FirstOrDefault();
                            work.WorkOrderStatusId = lstStatus[0].FirstOrDefault().WorkOrderStatus.Id;

                            work.StatusName = lstStatus[0].FirstOrDefault().WorkOrderStatus.Name + " - " + pendingStatus.Name;
                            work.StatusNameAr = lstStatus[0].FirstOrDefault().WorkOrderStatus.NameAr + " - " + pendingStatus.NameAr;
                            work.statusColor = lstStatus[0].FirstOrDefault().WorkOrderStatus.Color;
                            work.statusIcon = lstStatus[0].FirstOrDefault().WorkOrderStatus.Icon;
                        }

                        else
                        {
                            work.WorkOrderStatusId = lstStatus[0].FirstOrDefault().WorkOrderStatus.Id;
                            //    work.StatusId = lstStatus[0].FirstOrDefault().WorkOrderStatus.Id;
                            work.StatusName = lstStatus[0].FirstOrDefault().WorkOrderStatus.Name;
                            work.StatusNameAr = lstStatus[0].FirstOrDefault().WorkOrderStatus.NameAr;
                            work.statusColor = lstStatus[0].FirstOrDefault().WorkOrderStatus.Color;
                            work.statusIcon = lstStatus[0].FirstOrDefault().WorkOrderStatus.Icon;
                        }

                    }

                    var lstClosedDate = _context.WorkOrderTrackings
                        .Include(t => t.WorkOrder).Include(t => t.WorkOrderStatus)
                        .Where(a => a.WorkOrderId == item.FirstOrDefault().Id && a.WorkOrderStatusId == 12).ToList().OrderByDescending(a => a.WorkOrderDate).ToList().GroupBy(a => item.FirstOrDefault().Id).ToList();
                    if (lstClosedDate.Count > 0)
                    {
                        work.ClosedDate = lstClosedDate[0].FirstOrDefault().CreationDate;
                    }
                    work.ActualStartDate = item.FirstOrDefault().ActualStartDate;
                    work.ActualEndDate = item.FirstOrDefault().ActualEndDate;
                    work.RequestId = item.FirstOrDefault().RequestId != null ? (int)item.FirstOrDefault().RequestId : 0;
                    work.HospitalId = item.FirstOrDefault().Request.AssetDetail.HospitalId;
                    if (userId != null)
                    {
                        var lstAssigned = _context.WorkOrderTrackings.Where(a => a.AssignedTo == userId && a.WorkOrderId == work.Id).ToList();
                        if (lstAssigned.Count > 0)
                        {
                            work.AssignedTo = lstAssigned[0].AssignedTo;
                        }
                    }
                    work.ListTracks = _context.WorkOrderTrackings.Include(a => a.WorkOrderStatus).Where(a => a.WorkOrderId == work.Id)
                    .ToList().Select(item => new LstWorkOrderFromTracking
                    {
                        Id = item.Id,
                        StatusName = item.WorkOrderStatus.Name,
                        StatusNameAr = item.WorkOrderStatus.NameAr,
                        // CreationDate = DateTime.Parse(item.CreationDate.ToString())
                    }).ToList();

                    list.Add(work);
                }

                if (list.Count > 0)
                {

                    if (userRoleNames.Contains("Admin"))
                    {
                        list = list.ToList();
                    }
                    if (userRoleNames.Contains("EngDepManager") && !userRoleNames.Contains("Eng"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                    }
                    if (userRoleNames.Contains("Eng") && !userRoleNames.Contains("EngDepManager"))
                    {
                        list = list.Where(a => a.HospitalId == hospitalId && a.AssignedTo == userId).ToList();
                    }
                    if (userRoleNames.Contains("Eng") && userRoleNames.Contains("EngDepManager"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                    }
                }
            }
            return list;
        }

        public IEnumerable<IndexWorkOrderVM> GetAllWorkOrdersByHospitalId(int? hospitalId, string userId, int statusId)
        {
            List<IndexWorkOrderVM> list = new List<IndexWorkOrderVM>();
            List<IndexWorkOrderVM> listEngineer = new List<IndexWorkOrderVM>();
            ApplicationUser UserObj = new ApplicationUser();
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


            var lstWorkOrders = _context.WorkOrders
                      .Include(w => w.WorkOrderType)
                      .Include(w => w.WorkOrderPeriority)
                      .Include(w => w.Request)
                      .Include(w => w.Request.AssetDetail)
                      .Include(w => w.Request.AssetDetail.MasterAsset)
                      .Include(w => w.User).ToList();
            foreach (var item in lstWorkOrders)
            {
                IndexWorkOrderVM work = new IndexWorkOrderVM();
                work.Id = item.Id;
                work.WorkOrderNumber = item.WorkOrderNumber;
                work.Subject = item.Subject;
                work.ModelNumber = item.Request.AssetDetail.MasterAsset != null?item.Request.AssetDetail.MasterAsset.ModelNumber:"";
                work.RequestSubject = item.Request.Subject;
                work.CreationDate = item.CreationDate;
                work.Note = item.Note;
                work.BarCode = item.Request.AssetDetail != null? item.Request.AssetDetail.Barcode:"";
                work.CreatedById = item.CreatedById;
                work.CreatedBy = item.User.UserName;
                work.TypeName = item.WorkOrderType.Name;
                work.TypeNameAr = item.WorkOrderType.NameAr;
                work.PeriorityName = item.WorkOrderPeriority !=null? item.WorkOrderPeriority.Name:"";
                work.PeriorityNameAr = item.WorkOrderPeriority != null ? item.WorkOrderPeriority.NameAr:"";
                var lstStatus = _context.WorkOrderTrackings
                       .Include(t => t.WorkOrder).Include(t => t.WorkOrderStatus)
                       .Where(a => a.WorkOrderId == item.Id).ToList().OrderByDescending(a => a.WorkOrderDate).ToList();
                if (lstStatus.Count > 0)
                {
                    work.AssignedTo = lstStatus[0].AssignedTo;
                    work.CreatedById = lstStatus[0].CreatedById;
                    work.WorkOrderStatusId = lstStatus[0].WorkOrderStatus.Id;
                    if (work.WorkOrderStatusId == 3 || work.WorkOrderStatusId == 4 || work.WorkOrderStatusId == 5)
                    {
                        var pendingStatus = _context.WorkOrderStatuses.Where(a => a.Id == 6).ToList().FirstOrDefault();
                        work.StatusName = lstStatus[0].WorkOrderStatus.Name + " - " + pendingStatus.Name;
                        work.StatusNameAr = lstStatus[0].WorkOrderStatus.NameAr + " - " + pendingStatus.NameAr;
                        work.statusColor = lstStatus[0].WorkOrderStatus.Color;
                        work.statusIcon = lstStatus[0].WorkOrderStatus.Icon;
                    }

                    else
                    {
                        work.StatusName = lstStatus[0].WorkOrderStatus.Name;
                        work.StatusNameAr = lstStatus[0].WorkOrderStatus.NameAr;
                        work.statusColor = lstStatus[0].WorkOrderStatus.Color;
                        work.statusIcon = lstStatus[0].WorkOrderStatus.Icon;
                    }
                }

                work.ActualStartDate = item.ActualStartDate;
                work.ActualEndDate = item.ActualEndDate;
                work.RequestId = item.RequestId != null ? (int)item.RequestId : 0;
                work.HospitalId = item.Request.AssetDetail.HospitalId;

                var lstClosedDate = _context.WorkOrderTrackings
                      .Include(t => t.WorkOrder).Include(t => t.WorkOrderStatus)
                      .Where(a => a.WorkOrderId == item.Id && a.WorkOrderStatusId == 12).ToList().OrderByDescending(a => a.WorkOrderDate).ToList().GroupBy(a => item.Id).ToList();
                if (lstClosedDate.Count > 0)
                {
                    work.ClosedDate = lstClosedDate[0].FirstOrDefault().CreationDate;
                }
                work.ListTracks = _context.WorkOrderTrackings.Include(a => a.WorkOrderStatus).Where(a => a.WorkOrderId == work.Id)
                .ToList().Select(item => new LstWorkOrderFromTracking
                {
                    Id = item.Id,
                    StatusName = item.WorkOrderStatus.Name,
                    StatusNameAr = item.WorkOrderStatus.NameAr,
                  //  CreationDate = DateTime.Parse(item.CreationDate.ToString())
                }).ToList();

                list.Add(work);
            }
            //if (userRoleNames.Contains("EngDepManager") && !userRoleNames.Contains("Eng"))
            //{
            //    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
            //}
            //if (userRoleNames.Contains("Eng") && !userRoleNames.Contains("EngDepManager"))
            //{
            //    List<IndexWorkOrderVM> listAssignedUsers = new List<IndexWorkOrderVM>();
            //    var lstAssigned = (from wo in _context.WorkOrders
            //                       join trk in _context.WorkOrderTrackings on wo.Id equals trk.WorkOrderId
            //                       where trk.AssignedTo == userId
            //                       select wo).ToList().GroupBy(a => a.Id).ToList();

            //    foreach (var item in lstAssigned)
            //    {
            //        IndexWorkOrderVM work = new IndexWorkOrderVM();
            //        work.Id = item.FirstOrDefault().Id;
            //        work.WorkOrderNumber = item.FirstOrDefault().WorkOrderNumber;
            //        work.BarCode = item.FirstOrDefault().Request.AssetDetail.Barcode;
            //        work.Subject = item.FirstOrDefault().Subject;
            //        work.RequestSubject = item.FirstOrDefault().Request.Subject;
            //        work.CreationDate = item.FirstOrDefault().CreationDate;
            //        work.Note = item.FirstOrDefault().Note;
            //        work.CreatedById = item.FirstOrDefault().CreatedById;
            //        work.CreatedBy = _context.WorkOrderTrackings.Where(a => a.WorkOrderId == item.FirstOrDefault().Id).ToList().FirstOrDefault().User.UserName;
            //        work.TypeName = _context.WorkOrderTypes.Where(a => a.Id == item.FirstOrDefault().WorkOrderTypeId).ToList().FirstOrDefault().Name;
            //        work.TypeNameAr = _context.WorkOrderTypes.Where(a => a.Id == item.FirstOrDefault().WorkOrderTypeId).ToList().FirstOrDefault().NameAr;
            //        work.WorkOrderPeriorityId = _context.WorkOrderPeriorities.Where(a => a.Id == item.FirstOrDefault().WorkOrderPeriorityId).ToList().FirstOrDefault().Id;
            //        work.PeriorityName = _context.WorkOrderPeriorities.Where(a => a.Id == item.FirstOrDefault().WorkOrderPeriorityId).ToList().FirstOrDefault().Name;
            //        work.PeriorityNameAr = _context.WorkOrderPeriorities.Where(a => a.Id == item.FirstOrDefault().WorkOrderPeriorityId).ToList().FirstOrDefault().NameAr;
            //        var lstStatus = _context.WorkOrderTrackings
            //               .Include(t => t.WorkOrder).Include(t => t.WorkOrderStatus)
            //               .Where(a => a.WorkOrderId == item.FirstOrDefault().Id).ToList().OrderByDescending(a => a.WorkOrderDate).ToList();
            //        if (lstStatus.Count > 0)
            //        {
            //            work.StatusId = lstStatus[0].WorkOrderStatus.Id;
            //            work.WorkOrderStatusId = lstStatus[0].WorkOrderStatus.Id;
            //            work.StatusName = lstStatus[0].WorkOrderStatus.Name;
            //            work.StatusNameAr = lstStatus[0].WorkOrderStatus.NameAr;
            //            work.statusColor = lstStatus[0].WorkOrderStatus.Color;
            //            work.statusIcon = lstStatus[0].WorkOrderStatus.Icon;

            //        }

            //        var lstStatusIds = _context.WorkOrderTrackings
            //              .Where(a => a.WorkOrderId == item.FirstOrDefault().Id).ToList().Select(a => a.WorkOrderStatusId).ToList();



            //        var exist = lstStatusIds.Contains(9);
            //        work.ExistStatusId = exist;

            //        work.ActualStartDate = item.FirstOrDefault().ActualStartDate;
            //        work.ActualEndDate = item.FirstOrDefault().ActualEndDate;
            //        //work.RequestId = item.FirstOrDefault().RequestId;
            //        work.RequestId = item.FirstOrDefault().RequestId != null ? (int)item.FirstOrDefault().RequestId : 0;
            //        work.HospitalId = item.FirstOrDefault().Request.AssetDetail.HospitalId;
            //        work.AssignedTo = _context.WorkOrderTrackings.Where(a => a.WorkOrderId == item.FirstOrDefault().Id).ToList().FirstOrDefault().AssignedTo;
            //        listAssignedUsers.Add(work);
            //    }



            //    List<IndexWorkOrderVM> listCreatedByUser = new List<IndexWorkOrderVM>();
            //    var lstcreated = (from wo in _context.WorkOrders
            //                      join trk in _context.WorkOrderTrackings on wo.Id equals trk.WorkOrderId
            //                      where trk.CreatedById == userId
            //                      select wo).ToList().GroupBy(a => a.Id).ToList();

            //    foreach (var item in lstcreated)
            //    {
            //        IndexWorkOrderVM work = new IndexWorkOrderVM();
            //        work.Id = item.FirstOrDefault().Id;
            //        work.WorkOrderNumber = item.FirstOrDefault().WorkOrderNumber;
            //        work.Subject = item.FirstOrDefault().Subject;
            //        work.BarCode = item.FirstOrDefault().Request.AssetDetail.Barcode;
            //        work.RequestSubject = _context.Request.Where(a => a.Id == item.FirstOrDefault().RequestId).ToList().FirstOrDefault().Subject;
            //        work.CreationDate = item.FirstOrDefault().CreationDate;
            //        work.Note = item.FirstOrDefault().Note;
            //        work.CreatedById = item.FirstOrDefault().CreatedById;
            //        work.CreatedBy = _context.WorkOrderTrackings.Where(a => a.WorkOrderId == item.FirstOrDefault().Id).ToList().FirstOrDefault().User.UserName;
            //        work.TypeName = _context.WorkOrderTypes.Where(a => a.Id == item.FirstOrDefault().WorkOrderTypeId).ToList().FirstOrDefault().Name;
            //        work.TypeNameAr = _context.WorkOrderTypes.Where(a => a.Id == item.FirstOrDefault().WorkOrderTypeId).ToList().FirstOrDefault().NameAr;
            //        work.PeriorityName = _context.WorkOrderPeriorities.Where(a => a.Id == item.FirstOrDefault().WorkOrderPeriorityId).ToList().FirstOrDefault().Name;
            //        work.PeriorityNameAr = _context.WorkOrderPeriorities.Where(a => a.Id == item.FirstOrDefault().WorkOrderPeriorityId).ToList().FirstOrDefault().NameAr;
            //        var lstStatus = _context.WorkOrderTrackings
            //               .Include(t => t.WorkOrder).Include(t => t.WorkOrderStatus)
            //               .Where(a => a.WorkOrderId == item.FirstOrDefault().Id).ToList().OrderByDescending(a => a.WorkOrderDate).ToList();
            //        if (lstStatus.Count > 0)
            //        {
            //            work.StatusName = lstStatus[0].WorkOrderStatus.Name;
            //            work.StatusNameAr = lstStatus[0].WorkOrderStatus.NameAr;
            //            work.WorkOrderStatusId = lstStatus[0].WorkOrderStatus.Id;
            //            work.statusColor = lstStatus[0].WorkOrderStatus.Color;
            //            work.statusIcon = lstStatus[0].WorkOrderStatus.Icon;

            //        }

            //        work.ActualStartDate = item.FirstOrDefault().ActualStartDate;
            //        work.ActualEndDate = item.FirstOrDefault().ActualEndDate;
            //        //   work.RequestId = item.FirstOrDefault().RequestId;
            //        work.RequestId = item.FirstOrDefault().RequestId != null ? (int)item.FirstOrDefault().RequestId : 0;
            //        work.HospitalId = item.FirstOrDefault().Request.AssetDetail.HospitalId;
            //        work.AssignedTo = _context.WorkOrderTrackings.Where(a => a.WorkOrderId == item.FirstOrDefault().Id).ToList().FirstOrDefault().AssignedTo;
            //        listAssignedUsers.Add(work);
            //    }

            //    list = listAssignedUsers.Where(a => a.HospitalId == UserObj.HospitalId).Concat(listCreatedByUser.Where(a => a.HospitalId == UserObj.HospitalId)).ToList();
            //}

            //if (userRoleNames.Contains("Eng") && userRoleNames.Contains("EngDepManager"))
            //{
            //    list = list.Where(a => a.CreatedById == userId && a.AssignedTo == userId).ToList();
            //}

            if ( userRoleNames.Contains("EngDepManager"))
            {
                list = list.Where(a => a.HospitalId == UserObj.HospitalId).ToList();
            }


            if (statusId > 0)
                list = list.Where(a => a.WorkOrderStatusId == statusId).ToList();
            else
                list = list.ToList();

            return list;
        }


        public IndexWorkOrderVM GetById(int id)
        {
            IndexWorkOrderVM work = new IndexWorkOrderVM();
            var woObj = _context.WorkOrders.Where(a => a.Id == id).Include(w => w.Request).Include(w => w.Request.AssetDetail)
                .Include(w => w.Request.AssetDetail.MasterAsset).Include(w => w.WorkOrderType).Include(w => w.WorkOrderPeriority).Include(w => w.User).ToList();
            if (woObj.Count > 0)
            {
                work.Id = woObj[0].Id;
                work.Subject = woObj[0].Subject;
                work.BarCode = woObj[0].Request.AssetDetail.Barcode;
                work.WorkOrderNumber = woObj[0].WorkOrderNumber;
                work.ModelNumber = woObj[0].Request.AssetDetail.MasterAsset.ModelNumber;
                work.CreationDate = woObj[0].CreationDate;
                work.PlannedStartDate = woObj[0].PlannedStartDate;
                work.PlannedEndDate = woObj[0].PlannedEndDate;
                work.ActualStartDate = woObj[0].ActualStartDate;
                work.ActualEndDate = woObj[0].ActualEndDate;
                work.Note = woObj[0].Note;
                work.MasterAssetId = woObj[0].Request.AssetDetail.MasterAssetId;
                work.CreatedById = woObj[0].CreatedById;
                work.CreatedBy = woObj[0].User.UserName;
                work.WorkOrderPeriorityId = woObj[0].WorkOrderPeriorityId != null ? (int)woObj[0].WorkOrderPeriorityId : 0;
                work.WorkOrderPeriorityId = woObj[0].WorkOrderPeriority != null ? woObj[0].WorkOrderPeriority.Id : 0;
                work.PeriorityName = woObj[0].WorkOrderPeriority != null ? woObj[0].WorkOrderPeriority.Name : "";
                work.PeriorityNameAr = woObj[0].WorkOrderPeriority != null ? woObj[0].WorkOrderPeriority.NameAr : "";
                work.AssetName = woObj[0].Request.AssetDetail.MasterAsset.Name;
                work.AssetNameAr = woObj[0].Request.AssetDetail.MasterAsset.NameAr;
                work.WorkOrderTypeId = woObj[0].WorkOrderTypeId != null ? (int)woObj[0].WorkOrderTypeId : 0;
                work.WorkOrderTypeName = woObj[0].WorkOrderTypeId != null ? woObj[0].WorkOrderType.Name : "";
                work.TypeName = woObj[0].WorkOrderTypeId != null ? woObj[0].WorkOrderType.Name : "";
                work.TypeNameAr = woObj[0].WorkOrderTypeId != null ? woObj[0].WorkOrderType.NameAr : "";
                work.RequestId = woObj[0].RequestId != null ? (int)woObj[0].RequestId : 0;
                work.RequestSubject = woObj[0].Request.Subject;
                work.HospitalId = woObj[0].Request.AssetDetail.HospitalId;
                work.WorkOrderTrackingId = _context.WorkOrderTrackings.Where(t => t.WorkOrderId == id).FirstOrDefault().Id;
                work.WorkOrderStatusId = _context.WorkOrderTrackings.Where(t => t.WorkOrderId == id).FirstOrDefault().WorkOrderStatusId;
                work.StatusName = _context.WorkOrderTrackings
                    .Include(a => a.WorkOrderStatus).Where(t => t.WorkOrderId == id).FirstOrDefault().WorkOrderStatus.Name;
                work.StatusNameAr = _context.WorkOrderTrackings
                  .Include(a => a.WorkOrderStatus).Where(t => t.WorkOrderId == id).FirstOrDefault().WorkOrderStatus.NameAr;

            }




            return work;
        }


        public int GetTotalWorkOrdersForAssetInHospital(int assetDetailId)
        {
            var lstRequestsByAsset = _context.WorkOrders
                                   .Include(a => a.Request)
                                   .Include(t => t.Request.AssetDetail).Where(a => a.Request.AssetDetailId == assetDetailId).ToList();
            if (lstRequestsByAsset.Count > 0)
            {
                return lstRequestsByAsset.Count;
            }

            return 0;
        }

        public IEnumerable<IndexWorkOrderVM> GetworkOrder(string userId)
        {
            List<IndexWorkOrderVM> list = new List<IndexWorkOrderVM>();

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

            var lstWorkOrders = _context.WorkOrderTrackings
                                .Include(w => w.WorkOrder)
                                .Include(w => w.WorkOrder.Request)
                                .Include(w => w.WorkOrder.WorkOrderType)
                                .Include(w => w.WorkOrder.WorkOrderPeriority)
                                .Include(w => w.WorkOrder.User)
                                .Include(w => w.AssignedToUser)

                 .Select(wo => new IndexWorkOrderVM
                 {
                     Id = wo.WorkOrder.Id,
                     Subject = wo.WorkOrder.Subject,

                     AssetName = wo.WorkOrder.Request.AssetDetail.MasterAsset.Name,
                     AssetNameAr = wo.WorkOrder.Request.AssetDetail.MasterAsset.NameAr,

                     BarCode = wo.WorkOrder.Request.AssetDetail.Barcode,
                     SerialNumber = wo.WorkOrder.Request.AssetDetail.SerialNumber,
                     ModelNumber = wo.WorkOrder.Request.AssetDetail.MasterAsset.ModelNumber,
                     UserName = wo.User.UserName,
                     AssignedTo = wo.AssignedToUser.UserName,
                     StatusName = wo.WorkOrderStatus.Name,
                     StatusNameAr = wo.WorkOrderStatus.NameAr,
                     WorkOrderNumber = wo.WorkOrder.WorkOrderNumber,
                     CreationDate = wo.WorkOrder.CreationDate,
                     PlannedStartDate = wo.WorkOrder.PlannedStartDate,
                     PlannedEndDate = wo.WorkOrder.PlannedEndDate,
                     ActualStartDate = wo.WorkOrder.ActualStartDate,
                     ActualEndDate = wo.WorkOrder.ActualEndDate,
                     Note = wo.WorkOrder.Note,
                     WorkOrderTrackingId = wo.Id,
                     MasterAssetId = wo.WorkOrder.Request.AssetDetail.MasterAssetId,
                     CreatedById = wo.User.Id,
                     CreatedBy = wo.User.UserName,
                     //  WorkOrderPeriorityId = wo.WorkOrder.WorkOrderPeriorityId,
                     WorkOrderPeriorityId = wo.WorkOrder.WorkOrderPeriorityId != null ? (int)wo.WorkOrder.WorkOrderPeriorityId : 0,
                     WorkOrderPeriorityName = wo.WorkOrder.WorkOrderPeriority.Name,
                     // WorkOrderTypeId = wo.WorkOrder.WorkOrderTypeId,
                     WorkOrderTypeId = wo.WorkOrder.WorkOrderTypeId != null ? (int)wo.WorkOrder.WorkOrderTypeId : 0,
                     WorkOrderTypeName = wo.WorkOrder.WorkOrderType.Name,
                     //RequestId = wo.WorkOrder.RequestId,
                     RequestId = wo.WorkOrder.RequestId != null ? (int)wo.WorkOrder.RequestId : 0,
                     RequestSubject = wo.WorkOrder.Request.Subject,
                     HospitalId = wo.User.HospitalId,
                     GovernorateId = wo.User.GovernorateId,
                     CityId = wo.User.CityId,
                     OrganizationId = wo.User.OrganizationId,
                     SubOrganizationId = wo.User.SubOrganizationId,
                     RoleId = wo.User.RoleId,

                 }).OrderByDescending(o => o.CreationDate).ToList();






            foreach (var order in lstWorkOrders)
            {
                IndexWorkOrderVM getDataObj = new IndexWorkOrderVM();
                getDataObj.Id = order.Id;
                getDataObj.Subject = order.Subject;
                getDataObj.BarCode = order.BarCode;
                getDataObj.ModelNumber = order.ModelNumber;
                getDataObj.AssetName = order.AssetName;
                getDataObj.AssetNameAr = order.AssetNameAr;
                getDataObj.SerialNumber = order.SerialNumber;
                getDataObj.WorkOrderNumber = order.WorkOrderNumber;
                getDataObj.WorkOrderTypeName = order.WorkOrderTypeName;
                getDataObj.StatusName = order.StatusName;
                getDataObj.UserName = order.UserName;
                getDataObj.AssignedTo = order.AssignedTo;
                getDataObj.CreatedById = order.CreatedById;
                getDataObj.CreatedBy = order.CreatedBy;
                getDataObj.UserName = order.UserName;
                getDataObj.ActualStartDate = order.ActualStartDate;
                getDataObj.ActualEndDate = order.ActualEndDate;
                getDataObj.PlannedStartDate = order.PlannedStartDate;
                getDataObj.PlannedEndDate = order.PlannedEndDate;
                getDataObj.WorkOrderPeriorityId = order.WorkOrderPeriorityId;
                getDataObj.WorkOrderPeriorityName = order.WorkOrderPeriorityName;
                getDataObj.HospitalId = order.HospitalId;
                getDataObj.GovernorateId = order.GovernorateId;
                getDataObj.CityId = order.CityId;
                getDataObj.OrganizationId = order.OrganizationId;
                getDataObj.SubOrganizationId = order.SubOrganizationId;
                getDataObj.CreatedById = order.CreatedById;
                getDataObj.CreationDate = order.CreationDate;
                getDataObj.AssetName = _context.MasterAssets.Where(a => a.Id == order.MasterAssetId).ToList().FirstOrDefault().Name;
                getDataObj.AssetNameAr = _context.MasterAssets.Where(a => a.Id == order.MasterAssetId).ToList().FirstOrDefault().NameAr;
                getDataObj.WorkOrderTrackingId = order.WorkOrderTrackingId;
                //  getDataObj.ListTracks = _context.orderuestTracking.Where(a => a.orderuestId == order.Id)
                //          .ToList().Select(item => new IndexorderuestTrackingVM.GetData
                //          {
                //              Id = item.Id,
                //              StatusName = _context.orderuestStatus.Where(a => a.Id == item.orderuestStatusId).First().StatusName,
                //              Description = item.Description,
                //              Date = item.DescriptionDate,
                //              StatusId = item.orderuestStatusId,
                //              isExpanded = (_context.orderuestDocument.Where(a => a.orderuestTrackingId == item.Id).Count()) > 0 ? true : false,
                //              ListDocuments = _context.orderuestDocument.Where(a => a.orderuestTrackingId == item.Id).ToList(),
                //          }).ToList();
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

                if (userRoleName == "TLHospitalManager")
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }

                if (userRoleName == "EngDepManager")
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }

                if (userRoleName == "AssetOwner")
                {
                    list = list = new List<IndexWorkOrderVM>();
                }
                if (userRoleName == "Eng")
                {
                    var lstAssigned = list.Where(t => t.HospitalId == UserObj.HospitalId && t.AssignedTo == userId).ToList();
                    var lstCreatedItems = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                    list = lstAssigned.Concat(lstCreatedItems).ToList();
                }

                if (userRoleName == "DE")
                {
                    list = new List<IndexWorkOrderVM>();
                }
                if (userRoleName == "HR")
                {
                    list = new List<IndexWorkOrderVM>();
                }
            }
            if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId > 0)
            {
                if (userRoleName == "Admin")
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }

                if (userRoleName == "Admin")
                {
                    list = list.ToList();
                }
                if (userRoleName == "TLHospitalManager")
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                if (userRoleName == "EngManager")
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }

                if (userRoleName == "EngDepManager")
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }

                if (userRoleName == "AssetOwner")
                {
                    list = list = new List<IndexWorkOrderVM>();
                }

                if (userRoleName == "Eng")
                {
                    var lstAssigned = list.Where(t => t.HospitalId == UserObj.HospitalId && t.AssignedTo == userId).ToList();
                    var lstCreatedItems = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                    list = lstAssigned.Concat(lstCreatedItems).ToList();
                }

                if (userRoleName == "DE")
                {
                    list = list = new List<IndexWorkOrderVM>();
                }

                if (userRoleName == "HR")
                {
                    list = list = new List<IndexWorkOrderVM>();
                }

            }
            return list;
        }

        public IndexWorkOrderVM GetWorkOrderByRequestId(int requestId)
        {
            return _context.WorkOrders.Where(a => a.RequestId == requestId)
                            .Include(w => w.Request)
                            .Include(w => w.WorkOrderType)

                            .Include(w => w.WorkOrderPeriority)
                            .Include(w => w.User)
                   .Select(wo => new IndexWorkOrderVM
                   {
                       Id = wo.Id,
                       Subject = wo.Subject,
                       BarCode = wo.Request.AssetDetail.Barcode,
                       ModelNumber = wo.Request.AssetDetail.MasterAsset.ModelNumber,
                       AssetName = wo.Request.AssetDetail.MasterAsset.Name,
                       AssetNameAr = wo.Request.AssetDetail.MasterAsset.NameAr,
                       SerialNumber = wo.Request.AssetDetail.SerialNumber,
                       WorkOrderNumber = wo.WorkOrderNumber,
                       CreationDate = wo.CreationDate,
                       PlannedStartDate = wo.PlannedStartDate,
                       PlannedEndDate = wo.PlannedEndDate,
                       ActualStartDate = wo.ActualStartDate,
                       ActualEndDate = wo.ActualEndDate,
                       Note = wo.Note,
                       CreatedById = wo.CreatedById,
                       CreatedBy = wo.User.UserName,
                       // WorkOrderPeriorityId = wo.WorkOrderPeriorityId,
                       WorkOrderPeriorityId = wo.WorkOrderPeriorityId != null ? (int)wo.WorkOrderPeriorityId : 0,
                       WorkOrderPeriorityName = wo.WorkOrderPeriority.Name,
                       PeriorityName = wo.WorkOrderPeriority.Name,
                       PeriorityNameAr = wo.WorkOrderPeriority.NameAr,
                       // WorkOrderTypeId = wo.WorkOrderTypeId,
                       WorkOrderTypeId = wo.WorkOrderTypeId != null ? (int)wo.WorkOrderTypeId : 0,
                       TypeName = wo.WorkOrderType.Name,
                       TypeNameAr = wo.WorkOrderType.NameAr,
                       StatusName = _context.WorkOrderTrackings.Where(t => t.WorkOrderId == wo.Id).FirstOrDefault().WorkOrderStatus.Name,
                       StatusNameAr = _context.WorkOrderTrackings.Where(t => t.WorkOrderId == wo.Id).FirstOrDefault().WorkOrderStatus.NameAr,
                       // RequestId = wo.RequestId,
                       RequestId = wo.RequestId != null ? (int)wo.RequestId : 0,
                       WorkOrderTrackingId = _context.WorkOrderTrackings.Where(t => t.WorkOrderId == wo.Id).FirstOrDefault().Id,
                       WorkOrderStatusId = _context.WorkOrderTrackings.Where(t => t.WorkOrderId == wo.Id).FirstOrDefault().WorkOrderStatusId
                   }).FirstOrDefault();
        }

        public IEnumerable<IndexWorkOrderVM> GetworkOrderByUserId(int requestId, string userId)
        {
            List<IndexWorkOrderVM> list = new List<IndexWorkOrderVM>();

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

            var lstWorkOrders = _context.WorkOrderTrackings
                                .Include(w => w.WorkOrder)
                                .Include(w => w.WorkOrder.Request)
                                .Include(w => w.WorkOrder.WorkOrderType)
                                .Include(w => w.WorkOrder.WorkOrderPeriority)
                                .Include(w => w.WorkOrder.User)
                                .Include(w => w.AssignedToUser)

                 .Select(wo => new IndexWorkOrderVM
                 {
                     Id = wo.WorkOrder.Id,
                     Subject = wo.WorkOrder.Subject,
                     SerialNumber = wo.WorkOrder.Request.AssetDetail.SerialNumber,
                     BarCode = wo.WorkOrder.Request.AssetDetail.Barcode,
                     ModelNumber = wo.WorkOrder.Request.AssetDetail.MasterAsset.ModelNumber,
                     AssetName = wo.WorkOrder.Request.AssetDetail.MasterAsset.Name,
                     AssetNameAr = wo.WorkOrder.Request.AssetDetail.MasterAsset.NameAr,
                     UserName = wo.User.UserName,
                     AssignedTo = wo.AssignedToUser.UserName,
                     StatusName = wo.WorkOrderStatus.Name,
                     StatusNameAr = wo.WorkOrderStatus.NameAr,
                     WorkOrderNumber = wo.WorkOrder.WorkOrderNumber,
                     CreationDate = wo.WorkOrder.CreationDate,
                     PlannedStartDate = wo.WorkOrder.PlannedStartDate,
                     PlannedEndDate = wo.WorkOrder.PlannedEndDate,
                     ActualStartDate = wo.WorkOrder.ActualStartDate,
                     ActualEndDate = wo.WorkOrder.ActualEndDate,
                     Note = wo.WorkOrder.Note,
                     WorkOrderTrackingId = wo.Id,  //_context.WorkOrderTrackings.Where(w=>w.WorkOrderId== wo.WorkOrder.Id).FirstOrDefault().Id,

                     MasterAssetId = wo.WorkOrder.Request.AssetDetail.MasterAssetId,
                     CreatedById = wo.User.Id,
                     CreatedBy = wo.User.UserName,
                     //WorkOrderPeriorityId = wo.WorkOrder.WorkOrderPeriorityId,
                     WorkOrderPeriorityId = wo.WorkOrder.WorkOrderPeriorityId != null ? (int)wo.WorkOrder.WorkOrderPeriorityId : 0,
                     WorkOrderPeriorityName = wo.WorkOrder.WorkOrderPeriority.Name,
                     //    WorkOrderTypeId = wo.WorkOrder.WorkOrderTypeId,
                     WorkOrderTypeId = wo.WorkOrder.WorkOrderTypeId != null ? (int)wo.WorkOrder.WorkOrderTypeId : 0,
                     WorkOrderTypeName = wo.WorkOrder.WorkOrderType.Name,
                     //  RequestId = wo.WorkOrder.RequestId,
                     RequestId = wo.WorkOrder.RequestId != null ? (int)wo.WorkOrder.RequestId : 0,
                     RequestSubject = wo.WorkOrder.Request.Subject,
                     HospitalId = wo.User.HospitalId,
                     GovernorateId = wo.User.GovernorateId,
                     CityId = wo.User.CityId,
                     OrganizationId = wo.User.OrganizationId,
                     SubOrganizationId = wo.User.SubOrganizationId,
                     RoleId = wo.User.RoleId,

                 }).OrderByDescending(o => o.Id).Where(a => a.RequestId == requestId).ToList();






            foreach (var order in lstWorkOrders)
            {
                IndexWorkOrderVM getDataObj = new IndexWorkOrderVM();
                getDataObj.Id = order.Id;
                getDataObj.Subject = order.Subject;
                getDataObj.SerialNumber = order.SerialNumber;
                getDataObj.ModelNumber = order.ModelNumber;
                getDataObj.BarCode = order.BarCode;
                getDataObj.AssetName = order.AssetName;
                getDataObj.AssetNameAr = order.AssetNameAr;
                getDataObj.WorkOrderNumber = order.WorkOrderNumber;
                getDataObj.WorkOrderTypeName = order.WorkOrderTypeName;
                getDataObj.StatusName = order.StatusName;
                getDataObj.UserName = order.UserName;
                getDataObj.AssignedTo = order.AssignedTo;
                getDataObj.CreatedById = order.CreatedById;
                getDataObj.CreatedBy = order.CreatedBy;
                getDataObj.UserName = order.UserName;
                getDataObj.ActualStartDate = order.ActualStartDate;
                getDataObj.ActualEndDate = order.ActualEndDate;
                getDataObj.PlannedStartDate = order.PlannedStartDate;
                getDataObj.PlannedEndDate = order.PlannedEndDate;
                getDataObj.WorkOrderPeriorityId = order.WorkOrderPeriorityId;
                getDataObj.WorkOrderPeriorityName = order.WorkOrderPeriorityName;
                getDataObj.HospitalId = order.HospitalId;
                getDataObj.GovernorateId = order.GovernorateId;
                getDataObj.CityId = order.CityId;
                getDataObj.OrganizationId = order.OrganizationId;
                getDataObj.SubOrganizationId = order.SubOrganizationId;
                getDataObj.CreatedById = order.CreatedById;
                getDataObj.CreationDate = order.CreationDate;
                getDataObj.AssetName = _context.MasterAssets.Where(a => a.Id == order.MasterAssetId).ToList().FirstOrDefault().Name;
                getDataObj.AssetNameAr = _context.MasterAssets.Where(a => a.Id == order.MasterAssetId).ToList().FirstOrDefault().NameAr;
                getDataObj.WorkOrderTrackingId = order.WorkOrderTrackingId;



                //  getDataObj.ListTracks = _context.orderuestTracking.Where(a => a.orderuestId == order.Id)
                //          .ToList().Select(item => new IndexorderuestTrackingVM.GetData
                //          {
                //              Id = item.Id,
                //              StatusName = _context.orderuestStatus.Where(a => a.Id == item.orderuestStatusId).First().StatusName,
                //              Description = item.Description,
                //              Date = item.DescriptionDate,
                //              StatusId = item.orderuestStatusId,
                //              isExpanded = (_context.orderuestDocument.Where(a => a.orderuestTrackingId == item.Id).Count()) > 0 ? true : false,
                //              ListDocuments = _context.orderuestDocument.Where(a => a.orderuestTrackingId == item.Id).ToList(),
                //          }).ToList();
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

                if (userRoleName == "TLHospitalManager")
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }

                if (userRoleName == "EngDepManager")
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }

                if (userRoleName == "Eng")
                {
                    var lstAssigned = list.Where(t => t.HospitalId == UserObj.HospitalId && t.AssignedTo == userId).ToList();
                    var lstCreatedItems = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                    list = lstAssigned.Concat(lstCreatedItems).ToList();
                }

                if (userRoleName == "DE")
                {
                    list = new List<IndexWorkOrderVM>();
                }
                if (userRoleName == "HR")
                {
                    list = new List<IndexWorkOrderVM>();
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
                if (userRoleName == "EngManager")
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }

                if (userRoleName == "EngDepManager")
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }

                if (userRoleName == "Eng")
                {
                    var lstAssigned = list.Where(t => t.HospitalId == UserObj.HospitalId && t.AssignedTo == userId).ToList();
                    var lstCreatedItems = list.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == userId).ToList();
                    list = lstAssigned.Concat(lstCreatedItems).ToList();
                }

                if (userRoleName == "DE")
                {
                    list = list = new List<IndexWorkOrderVM>();
                }

                if (userRoleName == "HR")
                {
                    list = list = new List<IndexWorkOrderVM>();
                }

            }
            return list;
        }

        public PrintWorkOrderVM PrintWorkOrderById(int id)
        {
            List<LstWorkOrderFromTracking> lstTracking = new List<LstWorkOrderFromTracking>();
            PrintWorkOrderVM printWorkObj = new PrintWorkOrderVM();
            var workOrders = _context.WorkOrders.Where(a => a.Id == id).Include(w => w.Request).Include(a => a.Request.AssetDetail)
                .Include(a => a.Request.RequestType).Include(a => a.Request.SubProblem).Include(a => a.Request.SubProblem.Problem)
                .Include(a => a.Request.RequestMode)
                .Include(a => a.Request.AssetDetail.Hospital)
                .Include(a => a.Request.AssetDetail.MasterAsset)
                .Include(w => w.WorkOrderType).Include(w => w.WorkOrderPeriority).Include(w => w.User).ToList();

            if (workOrders.Count > 0)
            {

                var work = workOrders[0];

                printWorkObj.Id = work.Id;
                printWorkObj.Subject = work.Subject;
                printWorkObj.MasterAssetCode = work.Request.AssetDetail.MasterAsset.Code;
                printWorkObj.AssetCode = work.Request.AssetDetail.Code;
                printWorkObj.BarCode = work.Request.AssetDetail.Barcode;
                printWorkObj.WorkOrderNumber = work.WorkOrderNumber;
                printWorkObj.CreationDate = work.CreationDate;
                printWorkObj.PlannedStartDate = work.PlannedStartDate;
                printWorkObj.PlannedEndDate = work.PlannedEndDate;
                printWorkObj.ActualStartDate = work.ActualStartDate;
                printWorkObj.ActualEndDate = work.ActualEndDate;
                printWorkObj.Note = work.Note;
                printWorkObj.CreatedById = work.CreatedById;
                printWorkObj.CreatedBy = work.User.UserName;
                printWorkObj.PeriorityName = work.WorkOrderPeriority != null ? work.WorkOrderPeriority.Name:"";
                printWorkObj.PeriorityNameAr = work.WorkOrderPeriority != null ? work.WorkOrderPeriority.NameAr:"";
                printWorkObj.TypeName = work.WorkOrderType.Name;
                printWorkObj.TypeNameAr = work.WorkOrderType.NameAr;


                printWorkObj.RequestId = work.Request.Id;
                printWorkObj.RequestSubject = work.Request.Subject;
                printWorkObj.RequestDate = work.Request.RequestDate.ToShortDateString();
                printWorkObj.RequestCode = work.Request.RequestCode;
                printWorkObj.RequestTypeName = work.Request.RequestType.Name;
                printWorkObj.RequestTypeNameAr = work.Request.RequestType.NameAr;
                printWorkObj.ModeName = work.Request.RequestMode.Name;
                printWorkObj.ModeNameAr = work.Request.RequestMode.NameAr;
                printWorkObj.SubProblemName = work.Request.SubProblem != null ? work.Request.SubProblem.Name : "";
                printWorkObj.SubProblemNameAr = work.Request.SubProblem != null ? work.Request.SubProblem.NameAr : "";
                printWorkObj.ProblemName = work.Request.SubProblem != null ? work.Request.SubProblem.Problem.Name : "";
                printWorkObj.ProblemNameAr = work.Request.SubProblem != null ? work.Request.SubProblem.Problem.NameAr : "";



                printWorkObj.HospitalId = work.Request.AssetDetail.HospitalId;
                printWorkObj.HospitalName = work.Request.AssetDetail.Hospital.Name;
                printWorkObj.HospitalNameAr = work.Request.AssetDetail.Hospital.NameAr;
                printWorkObj.AssetName = work.Request.AssetDetail.MasterAsset.Name;
                printWorkObj.AssetNameAr = work.Request.AssetDetail.MasterAsset.NameAr;
                printWorkObj.SerialNumber = work.Request.AssetDetail.SerialNumber;

                var lstTracks = _context.WorkOrderTrackings
                                                      .Include(A => A.WorkOrderStatus)
                                                      .Include(A => A.User)
                                                     .Where(t => t.WorkOrderId == id).ToList();

                if (lstTracks.Count > 0)
                {
                    foreach (var item in lstTracks)
                    {
                        LstWorkOrderFromTracking trackObj = new LstWorkOrderFromTracking();
                        if (item.ActualStartDate != null)
                            trackObj.ActualStartDate = DateTime.Parse(item.ActualStartDate.Value.ToShortDateString());


                        if (item.ActualEndDate != null)
                            trackObj.ActualEndDate = DateTime.Parse(item.ActualEndDate.Value.ToShortDateString());
                        trackObj.Notes = item.Notes;
                        trackObj.CreatedBy = _context.ApplicationUser.Where(a => a.Id == item.CreatedById).ToList().FirstOrDefault().UserName;
                        trackObj.StatusName = item.WorkOrderStatus.Name;
                        trackObj.StatusNameAr = item.WorkOrderStatus.NameAr;
                        if (item.AssignedTo != "")
                            trackObj.AssignedToName = _context.ApplicationUser.Where(a => a.Id == item.AssignedTo).ToList().FirstOrDefault().UserName;
                        lstTracking.Add(trackObj);

                    }
                    printWorkObj.LstWorkOrderTracking = lstTracking;
                }
            }
            return printWorkObj;

        }

        public void Update(int id, EditWorkOrderVM editWorkOrderVM)
        {

            try
            {
                WorkOrder workOrder = new WorkOrder();
                workOrder.Id = editWorkOrderVM.Id;
                workOrder.Subject = editWorkOrderVM.Subject;
                workOrder.WorkOrderNumber = editWorkOrderVM.WorkOrderNumber;
                workOrder.CreationDate = editWorkOrderVM.CreationDate;
                workOrder.PlannedStartDate = editWorkOrderVM.PlannedStartDate;
                workOrder.PlannedEndDate = editWorkOrderVM.PlannedEndDate;
                workOrder.ActualStartDate = editWorkOrderVM.ActualStartDate;
                workOrder.ActualEndDate = editWorkOrderVM.ActualEndDate;
                workOrder.Note = editWorkOrderVM.Note;
                workOrder.CreatedById = editWorkOrderVM.CreatedById;
                workOrder.WorkOrderPeriorityId = editWorkOrderVM.WorkOrderPeriorityId;
                workOrder.WorkOrderTypeId = editWorkOrderVM.WorkOrderTypeId;
                workOrder.RequestId = editWorkOrderVM.RequestId;
                _context.Entry(workOrder).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }

        public IEnumerable<IndexWorkOrderVM> SearchWorkOrders(SearchWorkOrderVM searchObj)
        {
            List<IndexWorkOrderVM> lstData = new List<IndexWorkOrderVM>();

            ApplicationUser UserObj = new ApplicationUser();
            var obj = _context.ApplicationUser.Where(a => a.Id == searchObj.UserId).ToList();
            if (obj.Count > 0)
            {
                UserObj = obj[0];
            }


            var list = _context.WorkOrders.Include(w => w.Request).Include(a => a.Request.AssetDetail)
               .Include(a => a.Request.RequestType).Include(a => a.Request.SubProblem).Include(a => a.Request.SubProblem.Problem)
               .Include(a => a.Request.RequestMode)
                 .Include(a => a.Request.AssetDetail)
               .Include(a => a.Request.AssetDetail.Hospital)
               .Include(a => a.Request.AssetDetail.MasterAsset)
               .Include(w => w.WorkOrderType).Include(w => w.WorkOrderPeriority).Include(w => w.User).ToList();



            if (list.Count > 0)
            {
                if (UserObj.GovernorateId == 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
                {
                    list = list.ToList();
                }

                if (UserObj.GovernorateId > 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
                {
                    list = list.Where(t => t.Request.AssetDetail.Hospital.GovernorateId == UserObj.GovernorateId).ToList();
                }
                if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId == 0)
                {
                    list = list.Where(t => t.Request.AssetDetail.Hospital.GovernorateId == UserObj.GovernorateId && t.Request.AssetDetail.Hospital.CityId == UserObj.CityId).ToList();
                }
                if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId > 0)
                {
                    list = list.Where(t => t.Request.AssetDetail.Hospital.GovernorateId == UserObj.GovernorateId && t.Request.AssetDetail.Hospital.CityId == UserObj.CityId && t.Request.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                }
                if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId == 0 && UserObj.HospitalId == 0)
                {
                    list = list.Where(t => t.Request.AssetDetail.Hospital.OrganizationId == UserObj.OrganizationId).ToList();
                }
                if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId == 0)
                {
                    list = list.Where(t => t.Request.AssetDetail.Hospital.OrganizationId == UserObj.OrganizationId && t.Request.AssetDetail.Hospital.SubOrganizationId == UserObj.SubOrganizationId).ToList();
                }

                if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId > 0)
                {
                    list = list.Where(t => t.Request.AssetDetail.Hospital.SubOrganizationId == UserObj.SubOrganizationId && t.Request.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                }
            }

            foreach (var item in list)
            {
                IndexWorkOrderVM getDataObj = new IndexWorkOrderVM();
                getDataObj.Id = item.Id;
                getDataObj.Subject = item.Subject;
                getDataObj.BarCode = item.Request.AssetDetail.Barcode;
                getDataObj.ModelNumber = item.Request.AssetDetail.MasterAsset.ModelNumber;
                getDataObj.AssetName = item.Request.AssetDetail.MasterAsset.Name;
                getDataObj.AssetNameAr = item.Request.AssetDetail.MasterAsset.NameAr;
                getDataObj.SerialNumber = item.Request.AssetDetail.SerialNumber;

                getDataObj.WorkOrderNumber = item.WorkOrderNumber;



                getDataObj.WorkOrderTypeName = item.WorkOrderType.Name;
                getDataObj.RequestSubject = item.Request.Subject;
                getDataObj.AssetId = item.Request.AssetDetailId;
                getDataObj.CreatedById = item.CreatedById;
                getDataObj.CreatedBy = item.User.UserName;
                getDataObj.ActualStartDate = item.ActualStartDate;
                getDataObj.ActualEndDate = item.ActualEndDate;
                getDataObj.PlannedStartDate = item.PlannedStartDate;
                getDataObj.PlannedEndDate = item.PlannedEndDate;
                getDataObj.WorkOrderPeriorityId = item.WorkOrderPeriorityId != null ? (int)item.WorkOrderPeriorityId : 0;
                getDataObj.WorkOrderPeriorityName = item.WorkOrderPeriority != null ? item.WorkOrderPeriority.Name : "";
                getDataObj.HospitalId = item.Request.AssetDetail.HospitalId;
                getDataObj.GovernorateId = item.Request.AssetDetail.Hospital.GovernorateId;
                getDataObj.CityId = item.Request.AssetDetail.Hospital.CityId;
                getDataObj.OrganizationId = item.Request.AssetDetail.Hospital.OrganizationId;
                getDataObj.SubOrganizationId = item.Request.AssetDetail.Hospital.SubOrganizationId;
                getDataObj.CreatedById = item.CreatedById;
                getDataObj.CreationDate = item.CreationDate;
                getDataObj.AssetName = item.Request.AssetDetail.MasterAsset.Name + " - " + item.Request.AssetDetail.SerialNumber;
                getDataObj.AssetNameAr = item.Request.AssetDetail.MasterAsset.NameAr + " - " + item.Request.AssetDetail.SerialNumber;
                getDataObj.MasterAssetId = item.Request.AssetDetail.MasterAssetId;
                getDataObj.Note = item.Note;
                var lstStatus = _context.WorkOrderTrackings
                            .Include(t => t.WorkOrder).Include(t => t.WorkOrderStatus)
                            .Where(a => a.WorkOrderId == item.Id).ToList().OrderByDescending(a => a.Id).ToList();
                if (lstStatus.Count > 0)
                {

                    getDataObj.WorkOrderStatusId = (int)lstStatus[0].WorkOrderStatus.Id;
                    getDataObj.StatusName = lstStatus[0].WorkOrderStatus.Name;
                    getDataObj.StatusNameAr = lstStatus[0].WorkOrderStatus.NameAr;
                    getDataObj.statusColor = lstStatus[0].WorkOrderStatus.Color;
                    getDataObj.statusIcon = lstStatus[0].WorkOrderStatus.Icon;
                }

                if (item.Request.AssetDetailId != null)
                {
                    getDataObj.AssetName = item.Request.AssetDetail.MasterAsset.Name;
                    getDataObj.AssetNameAr = item.Request.AssetDetail.MasterAsset.NameAr;
                }


                if (item.WorkOrderPeriorityId != null)
                {
                    getDataObj.WorkOrderPeriorityId = (int)item.WorkOrderPeriorityId;
                    getDataObj.PeriorityName = item.WorkOrderPeriority.Name;
                    getDataObj.PeriorityNameAr = item.WorkOrderPeriority.NameAr;

                }
                lstData.Add(getDataObj);
            }
            if (lstData.Count > 0)
            {
                if (searchObj.StatusId != 0)
                {
                    lstData = lstData.Where(a => a.WorkOrderStatusId == searchObj.StatusId).ToList();
                }
                else
                    lstData = lstData.ToList();

                if (searchObj.PeriorityId != null)
                {
                    lstData = lstData.Where(a => a.WorkOrderPeriorityId == searchObj.PeriorityId).ToList();
                }
                else
                    lstData = lstData.ToList();

                if (searchObj.Subject != "")
                {
                    lstData = lstData.Where(a => a.Subject.Contains(searchObj.Subject)).ToList();
                }
                else
                    lstData = lstData.ToList();


                if (searchObj.ModelNumber != "")
                {
                    lstData = lstData.Where(a => a.ModelNumber.Contains(searchObj.ModelNumber)).ToList();
                }
                else
                    lstData = lstData.ToList();

                if (searchObj.BarCode != "")
                {
                    lstData = lstData.Where(b => b.BarCode.Contains(searchObj.BarCode)).ToList();
                }
                else
                    lstData = lstData.ToList();



                if (searchObj.WONumber != "")
                {
                    lstData = lstData.Where(b => b.WorkOrderNumber.Contains(searchObj.WONumber)).ToList();
                }
                else
                    lstData = lstData.ToList();


                if (searchObj.SerialNumber != "")
                {
                    lstData = lstData.Where(b => b.SerialNumber.Contains(searchObj.SerialNumber)).ToList();
                }
                else
                    lstData = lstData.ToList();



                if (searchObj.RequestSubject != "")
                {
                    lstData = lstData.Where(b => b.RequestSubject.Contains(searchObj.RequestSubject)).ToList();
                }
                else
                    lstData = lstData.ToList();


                string setstartday, setstartmonth, setendday, setendmonth = "";
                DateTime startingFrom = new DateTime();
                DateTime endingTo = new DateTime();
                if (searchObj.Start == "")
                {
                    //  searchObj.PlannedStartDate = DateTime.Parse("01/01/1900");
                }
                else
                {
                    searchObj.PlannedStartDate = DateTime.Parse(searchObj.Start.ToString());

                    var startyear = searchObj.PlannedStartDate.Value.Year;
                    var startmonth = searchObj.PlannedStartDate.Value.Month;
                    var startday = searchObj.PlannedStartDate.Value.Day;
                    if (startday < 10)
                        setstartday = searchObj.PlannedStartDate.Value.Day.ToString().PadLeft(2, '0');
                    else
                        setstartday = searchObj.PlannedStartDate.Value.Day.ToString();

                    if (startmonth < 10)
                        setstartmonth = searchObj.PlannedStartDate.Value.Month.ToString().PadLeft(2, '0');
                    else
                        setstartmonth = searchObj.PlannedStartDate.Value.Month.ToString();

                    var sDate = startyear + "/" + setstartmonth + "/" + setstartday;
                    startingFrom = DateTime.Parse(sDate);

                }

                if (searchObj.End == "")
                {
                    // searchObj.PlannedEndDate = DateTime.Today.Date;
                }
                else
                {
                    searchObj.PlannedEndDate = DateTime.Parse(searchObj.End.ToString());
                    var endyear = searchObj.PlannedEndDate.Value.Year;
                    var endmonth = searchObj.PlannedEndDate.Value.Month;
                    var endday = searchObj.PlannedEndDate.Value.Day;
                    if (endday < 10)
                        setendday = searchObj.PlannedEndDate.Value.Day.ToString().PadLeft(2, '0');
                    else
                        setendday = searchObj.PlannedEndDate.Value.Day.ToString();

                    if (endmonth < 10)
                        setendmonth = searchObj.PlannedEndDate.Value.Month.ToString().PadLeft(2, '0');
                    else
                        setendmonth = searchObj.PlannedEndDate.Value.Month.ToString();

                    var eDate = endyear + "/" + setendmonth + "/" + setendday;
                    endingTo = DateTime.Parse(eDate);
                }



                if (searchObj.Start != "" && searchObj.End != "")
                {
                    lstData = lstData.Where(a => a.PlannedStartDate >= startingFrom && a.PlannedStartDate <= endingTo).ToList();
                }
            }

            return lstData;
        }

        public IEnumerable<IndexWorkOrderVM> SortWorkOrders(int hosId, string userId, SortWorkOrderVM sortObj, int statusId)
        {
            var list = GetAllWorkOrdersByHospitalId(hosId, userId);
            
         

            if (sortObj.AssetName != "")
            {
                if (sortObj.StatusId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.StatusId == sortObj.StatusId).OrderByDescending(d => d.StatusName).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.StatusId == sortObj.StatusId).OrderBy(d => d.StatusName).ToList();
                    }
                }
                if (sortObj.StrRequestSubject != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.RequestSubject.Contains(sortObj.StrRequestSubject)).OrderByDescending(d => d.RequestSubject).ToList();

                    else
                        list = list.Where(b => b.RequestSubject.Contains(sortObj.StrRequestSubject)).OrderBy(d => d.RequestSubject).ToList();
                }
                if (sortObj.StrBarCode != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.StrSerial != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                if (sortObj.StrModel != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                    else
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                }
                if (sortObj.StrWorkOrderNumber != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.WorkOrderNumber.Contains(sortObj.StrWorkOrderNumber)).OrderByDescending(d => d.WorkOrderNumber).ToList();
                    else
                        list = list.Where(b => b.WorkOrderNumber.Contains(sortObj.StrWorkOrderNumber)).OrderBy(d => d.WorkOrderNumber).ToList();
                }
                if (sortObj.StrSubject != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                    else
                        list = list.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                }
                else
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.AssetName).ToList();
                    else
                        list = list.OrderBy(d => d.AssetName).ToList();
                }
            }

            if (sortObj.AssetNameAr != "")
            {
                if (sortObj.StatusId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.StatusId == sortObj.StatusId).OrderByDescending(d => d.StatusName).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.StatusId == sortObj.StatusId).OrderBy(d => d.StatusName).ToList();
                    }
                }
                if (sortObj.StrRequestSubject != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.RequestSubject.Contains(sortObj.StrRequestSubject)).OrderByDescending(d => d.RequestSubject).ToList();

                    else
                        list = list.Where(b => b.RequestSubject.Contains(sortObj.StrRequestSubject)).OrderBy(d => d.RequestSubject).ToList();
                }
                if (sortObj.StrBarCode != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.StrSerial != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                if (sortObj.StrModel != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                    else
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                }
                if (sortObj.StrWorkOrderNumber != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.WorkOrderNumber.Contains(sortObj.StrWorkOrderNumber)).OrderByDescending(d => d.WorkOrderNumber).ToList();
                    else
                        list = list.Where(b => b.WorkOrderNumber.Contains(sortObj.StrWorkOrderNumber)).OrderBy(d => d.WorkOrderNumber).ToList();
                }
                if (sortObj.StrSubject != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                    else
                        list = list.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                }
                else
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.AssetNameAr).ToList();
                    else
                        list = list.OrderBy(d => d.AssetNameAr).ToList();
                }
            }

            if (sortObj.StatusName != "")
            {
                if (sortObj.StatusId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.StatusId == sortObj.StatusId).OrderByDescending(d => d.StatusName).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.StatusId == sortObj.StatusId).OrderBy(d => d.StatusName).ToList();
                    }
                }
                if (sortObj.StrBarCode != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.StrRequestSubject != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.RequestSubject.Contains(sortObj.StrRequestSubject)).OrderByDescending(d => d.RequestSubject).ToList();

                    else
                        list = list.Where(b => b.RequestSubject.Contains(sortObj.StrRequestSubject)).OrderBy(d => d.RequestSubject).ToList();
                }
                if (sortObj.StrSerial != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                if (sortObj.StrModel != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                    else
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                }
                if (sortObj.StrWorkOrderNumber != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.WorkOrderNumber.Contains(sortObj.StrWorkOrderNumber)).OrderByDescending(d => d.WorkOrderNumber).ToList();
                    else
                        list = list.Where(b => b.WorkOrderNumber.Contains(sortObj.StrWorkOrderNumber)).OrderBy(d => d.WorkOrderNumber).ToList();
                }
                if (sortObj.StrSubject != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                    else
                        list = list.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                }

                else
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.StatusName).ToList();
                    else
                        list = list.OrderBy(d => d.StatusName).ToList();
                }
            }
            if (sortObj.StatusNameAr != "")
            {

                if (sortObj.StatusId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.StatusId == sortObj.StatusId).OrderByDescending(d => d.StatusId).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.StatusId == sortObj.StatusId).OrderBy(d => d.StatusId).ToList();
                    }
                }
                if (sortObj.StrBarCode != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.StrRequestSubject != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.RequestSubject.Contains(sortObj.StrRequestSubject)).OrderByDescending(d => d.RequestSubject).ToList();

                    else
                        list = list.Where(b => b.RequestSubject.Contains(sortObj.StrRequestSubject)).OrderBy(d => d.RequestSubject).ToList();
                }
                if (sortObj.StrSerial != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                if (sortObj.StrModel != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                    else
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                }
                if (sortObj.StrWorkOrderNumber != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.WorkOrderNumber.Contains(sortObj.StrWorkOrderNumber)).OrderByDescending(d => d.WorkOrderNumber).ToList();
                    else
                        list = list.Where(b => b.WorkOrderNumber.Contains(sortObj.StrWorkOrderNumber)).OrderBy(d => d.WorkOrderNumber).ToList();
                }
                if (sortObj.StrSubject != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                    else
                        list = list.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                }
                else
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.StatusNameAr).ToList();
                    else
                        list = list.OrderBy(d => d.StatusNameAr).ToList();
                }
            }
            if (sortObj.Barcode != "")
            {
                if (sortObj.StatusId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.StatusId == sortObj.StatusId).OrderByDescending(d => d.StatusId).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.StatusId == sortObj.StatusId).OrderBy(d => d.StatusId).ToList();
                    }
                }
                if (sortObj.StrBarCode != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.StrRequestSubject != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.RequestSubject.Contains(sortObj.StrRequestSubject)).OrderByDescending(d => d.RequestSubject).ToList();

                    else
                        list = list.Where(b => b.RequestSubject.Contains(sortObj.StrRequestSubject)).OrderBy(d => d.RequestSubject).ToList();
                }
                if (sortObj.StrSerial != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                if (sortObj.StrModel != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                    else
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                }
                if (sortObj.StrWorkOrderNumber != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.WorkOrderNumber.Contains(sortObj.StrWorkOrderNumber)).OrderByDescending(d => d.WorkOrderNumber).ToList();
                    else
                        list = list.Where(b => b.WorkOrderNumber.Contains(sortObj.StrWorkOrderNumber)).OrderBy(d => d.WorkOrderNumber).ToList();
                }
                if (sortObj.StrSubject != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                    else
                        list = list.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                }
                else
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.BarCode).ToList();
                    else
                        list = list.OrderBy(d => d.BarCode).ToList();
                }
            }
            if (sortObj.SerialNumber != "")
            {
                if (sortObj.StatusId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.StatusId == sortObj.StatusId).OrderByDescending(d => d.StatusId).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.StatusId == sortObj.StatusId).OrderBy(d => d.StatusId).ToList();
                    }
                }
                if (sortObj.StrSerial != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                if (sortObj.StrRequestSubject != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.RequestSubject.Contains(sortObj.StrRequestSubject)).OrderByDescending(d => d.RequestSubject).ToList();

                    else
                        list = list.Where(b => b.RequestSubject.Contains(sortObj.StrRequestSubject)).OrderBy(d => d.RequestSubject).ToList();
                }
                if (sortObj.StrBarCode != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderBy(d => d.BarCode).ToList();
                    }
                }

                if (sortObj.StrModel != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                    else
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                }
                if (sortObj.StrWorkOrderNumber != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.WorkOrderNumber.Contains(sortObj.StrWorkOrderNumber)).OrderByDescending(d => d.WorkOrderNumber).ToList();
                    else
                        list = list.Where(b => b.WorkOrderNumber.Contains(sortObj.StrWorkOrderNumber)).OrderBy(d => d.WorkOrderNumber).ToList();
                }
                if (sortObj.StrSubject != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                    else
                        list = list.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                }
                else
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                        list = list.OrderBy(d => d.SerialNumber).ToList();
                }
            }
            if (sortObj.ModelNumber != "")
            {
                if (sortObj.StatusId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.StatusId == sortObj.StatusId).OrderByDescending(d => d.StatusId).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.StatusId == sortObj.StatusId).OrderBy(d => d.StatusId).ToList();
                    }
                }
                if (sortObj.StrModel != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                    else
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                }
                if (sortObj.StrRequestSubject != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.RequestSubject.Contains(sortObj.StrRequestSubject)).OrderByDescending(d => d.RequestSubject).ToList();

                    else
                        list = list.Where(b => b.RequestSubject.Contains(sortObj.StrRequestSubject)).OrderBy(d => d.RequestSubject).ToList();
                }
                if (sortObj.StrBarCode != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.StrSerial != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }

                if (sortObj.StrWorkOrderNumber != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.WorkOrderNumber.Contains(sortObj.StrWorkOrderNumber)).OrderByDescending(d => d.WorkOrderNumber).ToList();
                    else
                        list = list.Where(b => b.WorkOrderNumber.Contains(sortObj.StrWorkOrderNumber)).OrderBy(d => d.WorkOrderNumber).ToList();
                }
                if (sortObj.StrSubject != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                    else
                        list = list.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                }
                else
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.ModelNumber).ToList();
                    else
                        list = list.OrderBy(d => d.ModelNumber).ToList();
                }
            }
            if (sortObj.WorkOrderNumber != "")
            {
                if (sortObj.StatusId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.StatusId == sortObj.StatusId).OrderByDescending(d => d.StatusId).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.StatusId == sortObj.StatusId).OrderBy(d => d.StatusId).ToList();
                    }
                }
                if (sortObj.StrWorkOrderNumber != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.WorkOrderNumber.Contains(sortObj.StrWorkOrderNumber)).OrderByDescending(d => d.WorkOrderNumber).ToList();
                    else
                        list = list.Where(b => b.WorkOrderNumber.Contains(sortObj.StrWorkOrderNumber)).OrderBy(d => d.WorkOrderNumber).ToList();
                }
                if (sortObj.StrRequestSubject != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.RequestSubject.Contains(sortObj.StrRequestSubject)).OrderByDescending(d => d.RequestSubject).ToList();

                    else
                        list = list.Where(b => b.RequestSubject.Contains(sortObj.StrRequestSubject)).OrderBy(d => d.RequestSubject).ToList();
                }
                if (sortObj.StrBarCode != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.StrSerial != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                if (sortObj.StrModel != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                    else
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                }

                if (sortObj.StrSubject != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                    else
                        list = list.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                }
                else
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.WorkOrderNumber).ToList();
                    else
                        list = list.OrderBy(d => d.WorkOrderNumber).ToList();
                }
            }
            if (sortObj.Subject != "")
            {
                if (sortObj.StatusId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.StatusId == sortObj.StatusId).OrderByDescending(d => d.StatusId).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.StatusId == sortObj.StatusId).OrderBy(d => d.StatusId).ToList();
                    }
                }
                if (sortObj.StrSubject != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                    else
                        list = list.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                }

                if (sortObj.StrRequestSubject != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.RequestSubject.Contains(sortObj.StrRequestSubject)).OrderByDescending(d => d.RequestSubject).ToList();

                    else
                        list = list.Where(b => b.RequestSubject.Contains(sortObj.StrRequestSubject)).OrderBy(d => d.RequestSubject).ToList();
                }
                if (sortObj.StrBarCode != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.StrSerial != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                if (sortObj.StrModel != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                    else
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                }
                if (sortObj.StrWorkOrderNumber != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.WorkOrderNumber.Contains(sortObj.StrWorkOrderNumber)).OrderByDescending(d => d.WorkOrderNumber).ToList();
                    else
                        list = list.Where(b => b.WorkOrderNumber.Contains(sortObj.StrWorkOrderNumber)).OrderBy(d => d.WorkOrderNumber).ToList();
                }

                else
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.Subject).ToList();
                    else
                        list = list.OrderBy(d => d.Subject).ToList();
                }
            }
            if (sortObj.RequestSubject != "")
            {
                if (sortObj.StatusId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.WorkOrderStatusId == sortObj.StatusId).OrderByDescending(d => d.RequestSubject).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.WorkOrderStatusId == sortObj.StatusId).OrderBy(d => d.RequestSubject).ToList();
                    }
                }
                if (sortObj.StrRequestSubject != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.RequestSubject.Contains(sortObj.StrRequestSubject)).OrderByDescending(d => d.RequestSubject).ToList();

                    else
                        list = list.Where(b => b.RequestSubject.Contains(sortObj.StrRequestSubject)).OrderBy(d => d.RequestSubject).ToList();
                }
                if (sortObj.StrBarCode != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.StrSerial != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                if (sortObj.StrModel != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                    else
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                }
                if (sortObj.StrWorkOrderNumber != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.WorkOrderNumber.Contains(sortObj.StrWorkOrderNumber)).OrderByDescending(d => d.WorkOrderNumber).ToList();
                    else
                        list = list.Where(b => b.WorkOrderNumber.Contains(sortObj.StrWorkOrderNumber)).OrderBy(d => d.WorkOrderNumber).ToList();
                }
                if (sortObj.StrSubject != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                    else
                        list = list.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                }
                else
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.RequestSubject).ToList();
                    else
                        list = list.OrderBy(d => d.RequestSubject).ToList();
                }
            }
            if (sortObj.CreatedBy != "")
            {

                if (sortObj.StatusId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.StatusId == sortObj.StatusId).OrderByDescending(d => d.StatusId).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.StatusId == sortObj.StatusId).OrderBy(d => d.StatusId).ToList();
                    }
                }
                if (sortObj.StrRequestSubject != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.RequestSubject.Contains(sortObj.StrRequestSubject)).OrderByDescending(d => d.RequestSubject).ToList();

                    else
                        list = list.Where(b => b.RequestSubject.Contains(sortObj.StrRequestSubject)).OrderBy(d => d.RequestSubject).ToList();
                }
                if (sortObj.StrBarCode != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.StrSerial != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                if (sortObj.StrModel != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                    else
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                }
                if (sortObj.StrWorkOrderNumber != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.WorkOrderNumber.Contains(sortObj.StrWorkOrderNumber)).OrderByDescending(d => d.WorkOrderNumber).ToList();
                    else
                        list = list.Where(b => b.WorkOrderNumber.Contains(sortObj.StrWorkOrderNumber)).OrderBy(d => d.WorkOrderNumber).ToList();
                }
                if (sortObj.StrSubject != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                    else
                        list = list.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                }
                else
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.CreatedBy).ToList();
                    else
                        list = list.OrderBy(d => d.CreatedBy).ToList();
                }
            }
            if (sortObj.CreationDate != "")
            {

                if (sortObj.StatusId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.StatusId == sortObj.StatusId).OrderByDescending(d => d.StatusId).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.StatusId == sortObj.StatusId).OrderBy(d => d.StatusId).ToList();
                    }
                }
                if (sortObj.StrRequestSubject != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.RequestSubject.Contains(sortObj.StrRequestSubject)).OrderByDescending(d => d.RequestSubject).ToList();

                    else
                        list = list.Where(b => b.RequestSubject.Contains(sortObj.StrRequestSubject)).OrderBy(d => d.RequestSubject).ToList();
                }
                if (sortObj.StrBarCode != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.StrSerial != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                if (sortObj.StrModel != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                    else
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                }
                if (sortObj.StrWorkOrderNumber != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.WorkOrderNumber.Contains(sortObj.StrWorkOrderNumber)).OrderByDescending(d => d.WorkOrderNumber).ToList();
                    else
                        list = list.Where(b => b.WorkOrderNumber.Contains(sortObj.StrWorkOrderNumber)).OrderBy(d => d.WorkOrderNumber).ToList();
                }
                if (sortObj.StrSubject != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                    else
                        list = list.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                }
                else
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.CreationDate).ToList();
                    else
                        list = list.OrderBy(d => d.CreationDate).ToList();
                }
            }
            if (sortObj.ClosedDate != "")
            {
                if (sortObj.StatusId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.StatusId == sortObj.StatusId).OrderByDescending(d => d.StatusId).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.StatusId == sortObj.StatusId).OrderBy(d => d.StatusId).ToList();
                    }
                }
                if (sortObj.StrRequestSubject != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.RequestSubject.Contains(sortObj.StrRequestSubject)).OrderByDescending(d => d.RequestSubject).ToList();

                    else
                        list = list.Where(b => b.RequestSubject.Contains(sortObj.StrRequestSubject)).OrderBy(d => d.RequestSubject).ToList();
                }
                if (sortObj.StrBarCode != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.StrSerial != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                if (sortObj.StrModel != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                    else
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                }
                if (sortObj.StrWorkOrderNumber != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.WorkOrderNumber.Contains(sortObj.StrWorkOrderNumber)).OrderByDescending(d => d.WorkOrderNumber).ToList();
                    else
                        list = list.Where(b => b.WorkOrderNumber.Contains(sortObj.StrWorkOrderNumber)).OrderBy(d => d.WorkOrderNumber).ToList();
                }
                if (sortObj.StrSubject != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                    else
                        list = list.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                }
                else
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.ClosedDate).ToList();
                    else
                        list = list.OrderBy(d => d.ClosedDate).ToList();
                }
            }
            if (sortObj.Note != "")
            {

                if (sortObj.StatusId != 0)
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.StatusId == sortObj.StatusId).OrderByDescending(d => d.StatusId).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.StatusId == sortObj.StatusId).OrderBy(d => d.StatusId).ToList();
                    }
                }
                if (sortObj.StrRequestSubject != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.RequestSubject.Contains(sortObj.StrRequestSubject)).OrderByDescending(d => d.RequestSubject).ToList();

                    else
                        list = list.Where(b => b.RequestSubject.Contains(sortObj.StrRequestSubject)).OrderBy(d => d.RequestSubject).ToList();
                }
                if (sortObj.StrBarCode != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.StrSerial != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.StrSerial)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                if (sortObj.StrModel != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderByDescending(d => d.ModelNumber).ToList();
                    else
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.StrModel)).OrderBy(d => d.ModelNumber).ToList();
                }
                if (sortObj.StrWorkOrderNumber != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.WorkOrderNumber.Contains(sortObj.StrWorkOrderNumber)).OrderByDescending(d => d.WorkOrderNumber).ToList();
                    else
                        list = list.Where(b => b.WorkOrderNumber.Contains(sortObj.StrWorkOrderNumber)).OrderBy(d => d.WorkOrderNumber).ToList();
                }
                if (sortObj.StrSubject != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderByDescending(d => d.Subject).ToList();

                    else
                        list = list.Where(b => b.Subject.Contains(sortObj.StrSubject)).OrderBy(d => d.Subject).ToList();
                }
                else
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.Note).ToList();
                    else
                        list = list.OrderBy(d => d.Note).ToList();
                }
            }






            return list;
        }

        public List<IndexWorkOrderVM> GetLastRequestAndWorkOrderByAssetId(int assetId)
        {

            return _context.WorkOrders.Include(a => a.Request)
                                    .Include(a => a.Request.AssetDetail)
                                    .Include(a => a.Request.AssetDetail.Hospital)
                                .Where(a => a.Request.AssetDetailId == assetId)
                                .OrderByDescending(a => a.Request.RequestDate)
                                .ToList()
                                .Select(item => new IndexWorkOrderVM
                                {
                                    Id = item.Id,
                                    RequestId = item.Request.Id,
                                    WorkOrderNumber = item.WorkOrderNumber,
                                    Subject = item.Subject,
                                    RequestSubject = item.Request.Subject,
                                    RequestNumber = item.Request.RequestCode,
                                    CreationDate = item.CreationDate,
                                    HospitalId = item.Request.AssetDetail.HospitalId
                                }).ToList();
        }

        public IEnumerable<IndexWorkOrderVM> GetWorkOrdersByDate(SearchWorkOrderByDateVM woDateObj)
        {
            List<IndexWorkOrderVM> list = new List<IndexWorkOrderVM>();
            ApplicationUser UserObj = new ApplicationUser();
            ApplicationRole roleObj = new ApplicationRole();
            List<string> userRoleNames = new List<string>();

            var obj = _context.ApplicationUser.Where(a => a.Id == woDateObj.UserId).ToList();
            if (obj.Count > 0)
            {
                UserObj = obj[0];

                var roles = (from userRole in _context.UserRoles
                             join role in _context.ApplicationRole on userRole.RoleId equals role.Id
                             where userRole.UserId == woDateObj.UserId
                             select role);
                foreach (var role in roles)
                {
                    userRoleNames.Add(role.Name);
                }
                var lstWorkOrders = _context.WorkOrders
                          .Include(w => w.WorkOrderType)
                          .Include(w => w.WorkOrderPeriority)
                          .Include(w => w.Request)
                          .Include(w => w.Request.AssetDetail)
                          .Include(w => w.Request.AssetDetail.MasterAsset)
                          .Include(w => w.User).OrderByDescending(a => a.CreationDate).ToList().GroupBy(a => a.Id).ToList();
                foreach (var item in lstWorkOrders)
                {
                    IndexWorkOrderVM work = new IndexWorkOrderVM();
                    work.Id = item.FirstOrDefault().Id;
                    work.WorkOrderNumber = item.FirstOrDefault().WorkOrderNumber;
                    work.BarCode = item.FirstOrDefault().Request.AssetDetail.Barcode;
                    work.ModelNumber = item.FirstOrDefault().Request.AssetDetail.MasterAsset.ModelNumber;
                    work.AssetName = item.FirstOrDefault().Request.AssetDetail.MasterAsset.Name;
                    work.AssetNameAr = item.FirstOrDefault().Request.AssetDetail.MasterAsset.NameAr;
                    work.SerialNumber = item.FirstOrDefault().Request.AssetDetail.SerialNumber;

                    work.Subject = item.FirstOrDefault().Subject;
                    work.RequestSubject = item.FirstOrDefault().Request.Subject;
                    work.CreationDate = item.FirstOrDefault().CreationDate;
                    work.Note = item.FirstOrDefault().Note;
                    work.CreatedById = item.FirstOrDefault().CreatedById;
                    work.CreatedBy = item.FirstOrDefault().User.UserName;

                    work.TypeName = item.FirstOrDefault().WorkOrderType.Name;
                    work.TypeNameAr = item.FirstOrDefault().WorkOrderType.NameAr;
                    work.PeriorityName = item.FirstOrDefault().WorkOrderPeriority != null ?item.FirstOrDefault().WorkOrderPeriority.Name:"";
                    work.PeriorityNameAr = item.FirstOrDefault().WorkOrderPeriority != null ? item.FirstOrDefault().WorkOrderPeriority.NameAr:"";
                    var lstAssignTo = _context.WorkOrderTrackings.Where(a => a.WorkOrderId == item.FirstOrDefault().Id).ToList().OrderByDescending(a => a.WorkOrderDate).ToList().GroupBy(a => item.FirstOrDefault().Id).ToList();

                    var lstStatus = _context.WorkOrderTrackings
                           .Include(t => t.WorkOrder).Include(t => t.WorkOrderStatus)
                           .Where(a => a.WorkOrderId == item.FirstOrDefault().Id).ToList().OrderByDescending(a => a.WorkOrderDate).ToList().GroupBy(a => item.FirstOrDefault().Id).ToList();
                    if (lstStatus.Count > 0)
                    {

                        work.AssignedTo = lstStatus[0].FirstOrDefault().AssignedTo;
                        work.WorkOrderStatusId = lstStatus[0].FirstOrDefault().WorkOrderStatus.Id;

                        if (work.WorkOrderStatusId == 3 || work.WorkOrderStatusId == 4 || work.WorkOrderStatusId == 5)
                        {
                            var pendingStatus = _context.WorkOrderStatuses.Where(a => a.Id == 6).ToList().FirstOrDefault();
                            work.StatusId = lstStatus[0].FirstOrDefault().WorkOrderStatus.Id;
                            work.StatusName = lstStatus[0].FirstOrDefault().WorkOrderStatus.Name + " - " + pendingStatus.Name;
                            work.StatusNameAr = lstStatus[0].FirstOrDefault().WorkOrderStatus.NameAr + " - " + pendingStatus.NameAr;
                            work.statusColor = lstStatus[0].FirstOrDefault().WorkOrderStatus.Color;
                            work.statusIcon = lstStatus[0].FirstOrDefault().WorkOrderStatus.Icon;
                        }

                        else
                        {
                            work.StatusId = lstStatus[0].FirstOrDefault().WorkOrderStatus.Id;
                            work.StatusName = lstStatus[0].FirstOrDefault().WorkOrderStatus.Name;
                            work.StatusNameAr = lstStatus[0].FirstOrDefault().WorkOrderStatus.NameAr;
                            work.statusColor = lstStatus[0].FirstOrDefault().WorkOrderStatus.Color;
                            work.statusIcon = lstStatus[0].FirstOrDefault().WorkOrderStatus.Icon;
                        }

                    }
                    work.ActualStartDate = item.FirstOrDefault().ActualStartDate;
                    work.ActualEndDate = item.FirstOrDefault().ActualEndDate;
                    work.RequestId = item.FirstOrDefault().RequestId != null ? (int)item.FirstOrDefault().RequestId : 0;
                    work.HospitalId = item.FirstOrDefault().Request.AssetDetail.HospitalId;
                    if (woDateObj.UserId != null)
                    {
                        var lstAssigned = _context.WorkOrderTrackings.Where(a => a.AssignedTo == woDateObj.UserId && a.WorkOrderId == work.Id).ToList();
                        if (lstAssigned.Count > 0)
                        {
                            work.AssignedTo = lstAssigned[0].AssignedTo;
                        }
                    }


                    work.ListTracks = _context.WorkOrderTrackings.Include(a => a.WorkOrderStatus).Where(a => a.WorkOrderId == work.Id)
                    .ToList().Select(item => new LstWorkOrderFromTracking
                    {
                        Id = item.Id,
                        StatusName = item.WorkOrderStatus.Name,
                        StatusNameAr = item.WorkOrderStatus.NameAr,
                        CreationDate = DateTime.Parse(item.CreationDate.ToString())
                    }).ToList();

                    list.Add(work);
                }

                if (list.Count > 0)
                {

                    if (userRoleNames.Contains("Admin"))
                    {
                        list = list.ToList();
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
                        list = list.Where(a => a.HospitalId == woDateObj.HospitalId && a.AssignedTo == woDateObj.UserId).ToList();
                    }
                }
            }
            return list;
        }

        public int CountWorkOrdersByHospitalId(int hospitalId, string userId)
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


            //var lstWorkOrders = _context.WorkOrders
            //                      .Include(w => w.Request)
            //                      .Include(w => w.Request.AssetDetail)
            //                      .Include(w => w.Request.AssetDetail.Hospital)
            //                      .Include(w => w.User).ToList().GroupBy(a => a.RequestId).ToList();

            var lstWorkOrders = _context.WorkOrderTrackings
                 .Include(w => w.WorkOrder)
                                  .Include(w => w.WorkOrder.Request)
                                  .Include(w => w.WorkOrder.Request.AssetDetail)
                                  .Include(w => w.WorkOrder.Request.AssetDetail.Hospital)
                                  .Include(w => w.User).ToList().GroupBy(a => a.WorkOrder.RequestId).ToList();



            if (lstWorkOrders.Count > 0)
            {
                if (UserObj.GovernorateId == 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
                {
                    lstWorkOrders = lstWorkOrders.ToList();
                }
                if (UserObj.GovernorateId > 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
                {
                    lstWorkOrders = lstWorkOrders.Where(t => t.FirstOrDefault().WorkOrder.Request.AssetDetail.Hospital.GovernorateId == UserObj.GovernorateId).ToList();
                }
                if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId == 0)
                {
                    lstWorkOrders = lstWorkOrders.Where(t => t.FirstOrDefault().WorkOrder.Request.AssetDetail.Hospital.GovernorateId == UserObj.GovernorateId && t.FirstOrDefault().WorkOrder.Request.AssetDetail.Hospital.CityId == UserObj.CityId).ToList();
                }
                if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId > 0)
                {
                    if (userRoleNames.Contains("Admin"))
                    {
                        lstWorkOrders = lstWorkOrders.Where(t => t.FirstOrDefault().WorkOrder.Request.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                    }
                    if (userRoleNames.Contains("TLHospitalManager"))
                    {
                        lstWorkOrders = lstWorkOrders.Where(t => t.FirstOrDefault().WorkOrder.Request.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
                    }
                    if (userRoleNames.Contains("EngDepManager") && !userRoleNames.Contains("Eng"))
                    {
                        lstWorkOrders = lstWorkOrders.Where(t => t.FirstOrDefault().WorkOrder.Request.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
                    }
                    if (userRoleNames.Contains("AssetOwner"))
                    {
                        lstWorkOrders = lstWorkOrders.Where(t => t.FirstOrDefault().WorkOrder.Request.AssetDetail.HospitalId == UserObj.HospitalId && t.FirstOrDefault().CreatedById == userId).ToList();
                    }
                }
                if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId == 0 && UserObj.HospitalId == 0)
                {
                    lstWorkOrders = lstWorkOrders.Where(t => t.FirstOrDefault().WorkOrder.Request.AssetDetail.Hospital.OrganizationId == UserObj.OrganizationId).ToList();
                }
                if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId == 0)
                {
                    lstWorkOrders = lstWorkOrders.Where(t => t.FirstOrDefault().WorkOrder.Request.AssetDetail.Hospital.OrganizationId == UserObj.OrganizationId && t.FirstOrDefault().WorkOrder.Request.AssetDetail.Hospital.SubOrganizationId == UserObj.SubOrganizationId).ToList();
                }
                if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId > 0)
                {
                    lstWorkOrders = lstWorkOrders.Where(t => t.FirstOrDefault().WorkOrder.Request.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();


                    if (userRoleNames.Contains("Admin"))
                    {
                        lstWorkOrders = lstWorkOrders.Where(t => t.FirstOrDefault().WorkOrder.Request.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                    }
                    if (userRoleNames.Contains("TLHospitalManager"))
                    {
                        lstWorkOrders = lstWorkOrders.Where(t => t.FirstOrDefault().WorkOrder.Request.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
                    }
                    if (userRoleNames.Contains("EngDepManager") && !userRoleNames.Contains("Eng"))
                    {
                        lstWorkOrders = lstWorkOrders.Where(t => t.FirstOrDefault().WorkOrder.Request.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
                    }
                    if (userRoleNames.Contains("AssetOwner"))
                    {
                        lstWorkOrders = lstWorkOrders.Where(t => t.FirstOrDefault().WorkOrder.Request.AssetDetail.HospitalId == UserObj.HospitalId && t.FirstOrDefault().CreatedById == userId).ToList();
                    }
                }
            }


            return lstWorkOrders.Count();

        }

        public int CreateWorkOrderAttachments(WorkOrderAttachment attachObj)
        {
            WorkOrderAttachment documentObj = new WorkOrderAttachment();
            documentObj.DocumentName = attachObj.DocumentName;
            documentObj.FileName = attachObj.FileName;
            documentObj.WorkOrderTrackingId = attachObj.WorkOrderTrackingId;
            _context.WorkOrderAttachments.Add(documentObj);
            _context.SaveChanges();
            return attachObj.Id;
        }

        public List<IndexWorkOrderVM> GetLastRequestAndWorkOrderByAssetId(int assetId, int requestId)
        {
            return _context.WorkOrders.Include(a => a.Request)
                                     .Include(a => a.Request.AssetDetail)
                                     .Include(a => a.Request.AssetDetail.Hospital)
                                 .Where(a => a.Request.AssetDetailId == assetId && a.Request.Id == requestId)
                                 .OrderByDescending(a => a.Request.RequestDate)
                                 .ToList()
                                 .Select(item => new IndexWorkOrderVM
                                 {
                                     Id = item.Id,
                                     RequestId = item.Request.Id,
                                     WorkOrderNumber = item.WorkOrderNumber,
                                     Subject = item.Subject,
                                     RequestSubject = item.Request.Subject,
                                     RequestNumber = item.Request.RequestCode,
                                     CreationDate = item.CreationDate,
                                     HospitalId = item.Request.AssetDetail.HospitalId
                                 }).ToList();
        }
    }
}
