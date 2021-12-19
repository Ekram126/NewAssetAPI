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
                    workOrder.PlannedStartDate = createWorkOrderVM.PlannedStartDate;
                    workOrder.PlannedEndDate = createWorkOrderVM.PlannedEndDate;
                    workOrder.ActualStartDate = DateTime.Parse(createWorkOrderVM.ActualStartDate);
                    workOrder.ActualEndDate = DateTime.Parse(createWorkOrderVM.ActualEndDate);
                    workOrder.Note = createWorkOrderVM.Note;
                    workOrder.CreatedById = createWorkOrderVM.CreatedById;
                    workOrder.WorkOrderPeriorityId = createWorkOrderVM.WorkOrderPeriorityId;
                    workOrder.WorkOrderTypeId = createWorkOrderVM.WorkOrderTypeId;
                    workOrder.RequestId = createWorkOrderVM.RequestId;
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
            string userRoleName = "";
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
                          .Include(w => w.User).OrderByDescending(a => a.CreationDate).ToList().GroupBy(a => a.Id).ToList();
                foreach (var item in lstWorkOrders)
                {
                    IndexWorkOrderVM work = new IndexWorkOrderVM();
                    work.Id = item.FirstOrDefault().Id;
                    work.WorkOrderNumber = item.FirstOrDefault().WorkOrderNumber;
                    work.Subject = item.FirstOrDefault().Subject;
                    work.RequestSubject = item.FirstOrDefault().Request.Subject;
                    work.CreationDate = item.FirstOrDefault().CreationDate;
                    work.Note = item.FirstOrDefault().Note;
                    work.CreatedById = item.FirstOrDefault().CreatedById;
                    work.CreatedBy = item.FirstOrDefault().User.UserName;

                    work.TypeName = item.FirstOrDefault().WorkOrderType.Name;
                    work.TypeNameAr = item.FirstOrDefault().WorkOrderType.NameAr;
                    work.PeriorityName = item.FirstOrDefault().WorkOrderPeriority.Name;
                    work.PeriorityNameAr = item.FirstOrDefault().WorkOrderPeriority.NameAr;
                    var lstAssignTo = _context.WorkOrderTrackings.Where(a => a.WorkOrderId == item.FirstOrDefault().Id).ToList().OrderByDescending(a => a.WorkOrderDate).ToList().GroupBy(a => item.FirstOrDefault().Id).ToList();




                    var lstStatus = _context.WorkOrderTrackings
                           .Include(t => t.WorkOrder).Include(t => t.WorkOrderStatus)
                           .Where(a => a.WorkOrderId == item.FirstOrDefault().Id).ToList().OrderByDescending(a => a.WorkOrderDate).ToList().GroupBy(a => item.FirstOrDefault().Id).ToList();
                    if (lstStatus.Count > 0)
                    {

                        //  work.TrackCreatedById = lstStatus[0].FirstOrDefault().CreatedById;
                        work.AssignedTo = lstStatus[0].FirstOrDefault().AssignedTo;


                        work.WorkOrderStatusId = lstStatus[0].FirstOrDefault().WorkOrderStatus.Id;

                        if (work.WorkOrderStatusId == 3 || work.WorkOrderStatusId == 4 || work.WorkOrderStatusId == 5)
                        {
                            var pendingStatus = _context.WorkOrderStatuses.Where(a => a.Id == 6).ToList().FirstOrDefault();
                            work.StatusName = lstStatus[0].FirstOrDefault().WorkOrderStatus.Name + " - " + pendingStatus.Name;
                            work.StatusNameAr = lstStatus[0].FirstOrDefault().WorkOrderStatus.NameAr + " - " + pendingStatus.NameAr;
                            work.statusColor = lstStatus[0].FirstOrDefault().WorkOrderStatus.Color;
                        }

                        else
                        {
                            work.StatusName = lstStatus[0].FirstOrDefault().WorkOrderStatus.Name;
                            work.StatusNameAr = lstStatus[0].FirstOrDefault().WorkOrderStatus.NameAr;
                            work.statusColor = lstStatus[0].FirstOrDefault().WorkOrderStatus.Color;
                        }

                    }

                    //var lstStatusIds = _context.WorkOrderTrackings
                    //     .Include(t => t.WorkOrder).Include(t => t.WorkOrderStatus)
                    //          .Where(a => a.WorkOrderId == item.FirstOrDefault().Id).ToList().OrderByDescending(a => a.WorkOrderStatusId).Select(a => a.WorkOrderStatusId).ToList();
                    //if (lstStatusIds.Count > 0)
                    //{
                    //    var statusId = lstStatusIds[0];
                    //    work.WorkOrderStatusId = lstStatusIds[0];
                    //    var exist = lstStatusIds.Contains(9);
                    //    work.ExistStatusId = exist;
                    //}
                    work.ActualStartDate = item.FirstOrDefault().ActualStartDate;
                    work.ActualEndDate = item.FirstOrDefault().ActualEndDate;
                    work.RequestId = item.FirstOrDefault().RequestId != null ? (int)item.FirstOrDefault().RequestId : 0;
                    work.HospitalId = item.FirstOrDefault().Request.AssetDetail.HospitalId;
                    if (userId != null)
                    {
                        var lstUsers = _context.WorkOrderTrackings.Where(a => a.AssignedTo == userId).ToList();
                        if (lstUsers.Count > 0)
                        {
                            work.AssignedTo = _context.WorkOrderTrackings.Where(a => a.AssignedTo == userId).FirstOrDefault().AssignedTo;
                        }
                    }
                    //if (userId != null)
                    //{
                    //    var lstUsers = _context.WorkOrderTrackings.Where(a => a.CreatedById == userId).ToList();
                    //    if (lstUsers.Count > 0)
                    //    {
                    //        work.TrackCreatedById = _context.WorkOrderTrackings.Where(a => a.CreatedById == userId).FirstOrDefault().CreatedById;
                    //    }
                    //}



                    list.Add(work);
                }

                if (list.Count > 0)
                {

                    if (userRoleNames.Contains("Admin"))
                    {
                        list = list.ToList();
                    }
                    if (userRoleNames.Contains("EngDepManager"))
                    {
                        list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                    }
                    if (userRoleNames.Contains("Eng"))
                    {
                        list = list.Where(a => a.HospitalId == hospitalId && a.AssignedTo == userId).ToList();
                    }
                }
            }
            return list;
        }

        public IEnumerable<IndexWorkOrderVM> GetAllWorkOrdersByHospitalId(int? hospitalId, string userId, int statusId)
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


            var lstWorkOrders = _context.WorkOrders
                      .Include(w => w.WorkOrderType)
                      .Include(w => w.WorkOrderPeriority)
                      .Include(w => w.Request)
                      .Include(w => w.Request.AssetDetail)
                      .Include(w => w.User).ToList();
            foreach (var item in lstWorkOrders)
            {
                IndexWorkOrderVM work = new IndexWorkOrderVM();
                work.Id = item.Id;
                work.WorkOrderNumber = item.WorkOrderNumber;
                work.Subject = item.Subject;
                work.RequestSubject = item.Request.Subject;
                work.CreationDate = item.CreationDate;
                work.Note = item.Note;
                work.CreatedById = item.CreatedById;
                work.CreatedBy = item.User.UserName;
                work.TypeName = item.WorkOrderType.Name;
                work.TypeNameAr = item.WorkOrderType.NameAr;
                work.PeriorityName = item.WorkOrderPeriority.Name;
                work.PeriorityNameAr = item.WorkOrderPeriority.NameAr;
                var lstStatus = _context.WorkOrderTrackings
                       .Include(t => t.WorkOrder).Include(t => t.WorkOrderStatus)
                       .Where(a => a.WorkOrderId == item.Id).ToList().OrderByDescending(a => a.WorkOrderDate).ToList();
                if (lstStatus.Count > 0)
                {
                    work.WorkOrderStatusId = lstStatus[0].WorkOrderStatus.Id;

                    if (work.WorkOrderStatusId == 3 || work.WorkOrderStatusId == 4 || work.WorkOrderStatusId == 5)
                    {
                        var pendingStatus = _context.WorkOrderStatuses.Where(a => a.Id == 6).ToList().FirstOrDefault();
                        work.StatusName = lstStatus[0].WorkOrderStatus.Name + " - " + pendingStatus.Name;
                        work.StatusNameAr = lstStatus[0].WorkOrderStatus.NameAr + " - " + pendingStatus.NameAr;
                        work.statusColor = lstStatus[0].WorkOrderStatus.Color;
                    }

                    else
                    {
                        work.StatusName = lstStatus[0].WorkOrderStatus.Name;
                        work.StatusNameAr = lstStatus[0].WorkOrderStatus.NameAr;
                        work.statusColor = lstStatus[0].WorkOrderStatus.Color;
                    }
                }

                //   var lstStatusIds = _context.WorkOrderTrackings
                //        .Include(t => t.WorkOrder).Include(t => t.WorkOrderStatus)
                //             .Where(a => a.WorkOrderId == item.Id).ToList().OrderByDescending(a => a.WorkOrderStatusId).Select(a => a.WorkOrderStatusId).ToList();
                //   if (lstStatusIds.Count > 0)
                //   {
                //var exist = lstStatusIds.Contains(9);
                //   work.ExistStatusId = exist;

                //   }


                //var exist = lstStatusIds.Contains(9);
                //work.ExistStatusId = exist;


                work.ActualStartDate = item.ActualStartDate;
                work.ActualEndDate = item.ActualEndDate;

                work.RequestId = item.RequestId != null ? (int)item.RequestId : 0;
                work.HospitalId = item.Request.AssetDetail.HospitalId;
                list.Add(work);
            }


            if (hospitalId != null)
            {

                if (userRoleName == "EngDepManager")
                {
                    list = list.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                if (userRoleName == "Eng")
                {
                    List<IndexWorkOrderVM> listAssignedUsers = new List<IndexWorkOrderVM>();
                    var lstAssigned = (from wo in _context.WorkOrders
                                       join trk in _context.WorkOrderTrackings on wo.Id equals trk.WorkOrderId
                                       where trk.AssignedTo == userId
                                       select wo).ToList().GroupBy(a => a.Id).ToList();

                    foreach (var item in lstAssigned)
                    {
                        IndexWorkOrderVM work = new IndexWorkOrderVM();
                        work.Id = item.FirstOrDefault().Id;
                        work.WorkOrderNumber = item.FirstOrDefault().WorkOrderNumber;
                        work.Subject = item.FirstOrDefault().Subject;
                        work.RequestSubject = _context.Request.Where(a => a.Id == item.FirstOrDefault().RequestId).ToList().FirstOrDefault().Subject;
                        work.CreationDate = item.FirstOrDefault().CreationDate;
                        work.Note = item.FirstOrDefault().Note;
                        work.CreatedById = item.FirstOrDefault().CreatedById;
                        work.CreatedBy = _context.WorkOrderTrackings.Where(a => a.WorkOrderId == item.FirstOrDefault().Id).ToList().FirstOrDefault().User.UserName;
                        work.TypeName = _context.WorkOrderTypes.Where(a => a.Id == item.FirstOrDefault().WorkOrderTypeId).ToList().FirstOrDefault().Name;
                        work.TypeNameAr = _context.WorkOrderTypes.Where(a => a.Id == item.FirstOrDefault().WorkOrderTypeId).ToList().FirstOrDefault().NameAr;
                        work.PeriorityName = _context.WorkOrderPeriorities.Where(a => a.Id == item.FirstOrDefault().WorkOrderPeriorityId).ToList().FirstOrDefault().Name;
                        work.PeriorityNameAr = _context.WorkOrderPeriorities.Where(a => a.Id == item.FirstOrDefault().WorkOrderPeriorityId).ToList().FirstOrDefault().NameAr;
                        var lstStatus = _context.WorkOrderTrackings
                               .Include(t => t.WorkOrder).Include(t => t.WorkOrderStatus)
                               .Where(a => a.WorkOrderId == item.FirstOrDefault().Id).ToList().OrderByDescending(a => a.WorkOrderDate).ToList();
                        if (lstStatus.Count > 0)
                        {
                            work.WorkOrderStatusId = lstStatus[0].WorkOrderStatus.Id;
                            work.StatusName = lstStatus[0].WorkOrderStatus.Name;
                            work.StatusNameAr = lstStatus[0].WorkOrderStatus.NameAr;
                            work.statusColor = lstStatus[0].WorkOrderStatus.Color;
                        }

                        var lstStatusIds = _context.WorkOrderTrackings
                              .Where(a => a.WorkOrderId == item.FirstOrDefault().Id).ToList().Select(a => a.WorkOrderStatusId).ToList();



                        var exist = lstStatusIds.Contains(9);
                        work.ExistStatusId = exist;

                        work.ActualStartDate = item.FirstOrDefault().ActualStartDate;
                        work.ActualEndDate = item.FirstOrDefault().ActualEndDate;
                        //work.RequestId = item.FirstOrDefault().RequestId;
                        work.RequestId = item.FirstOrDefault().RequestId != null ? (int)item.FirstOrDefault().RequestId : 0;
                        work.HospitalId = item.FirstOrDefault().Request.AssetDetail.HospitalId;
                        work.AssignedTo = _context.WorkOrderTrackings.Where(a => a.WorkOrderId == item.FirstOrDefault().Id).ToList().FirstOrDefault().AssignedTo;
                        listAssignedUsers.Add(work);
                    }



                    List<IndexWorkOrderVM> listCreatedByUser = new List<IndexWorkOrderVM>();
                    var lstcreated = (from wo in _context.WorkOrders
                                      join trk in _context.WorkOrderTrackings on wo.Id equals trk.WorkOrderId
                                      where trk.CreatedById == userId
                                      select wo).ToList().GroupBy(a => a.Id).ToList();

                    foreach (var item in lstcreated)
                    {
                        IndexWorkOrderVM work = new IndexWorkOrderVM();
                        work.Id = item.FirstOrDefault().Id;
                        work.WorkOrderNumber = item.FirstOrDefault().WorkOrderNumber;
                        work.Subject = item.FirstOrDefault().Subject;
                        work.RequestSubject = _context.Request.Where(a => a.Id == item.FirstOrDefault().RequestId).ToList().FirstOrDefault().Subject;
                        work.CreationDate = item.FirstOrDefault().CreationDate;
                        work.Note = item.FirstOrDefault().Note;
                        work.CreatedById = item.FirstOrDefault().CreatedById;
                        work.CreatedBy = _context.WorkOrderTrackings.Where(a => a.WorkOrderId == item.FirstOrDefault().Id).ToList().FirstOrDefault().User.UserName;
                        work.TypeName = _context.WorkOrderTypes.Where(a => a.Id == item.FirstOrDefault().WorkOrderTypeId).ToList().FirstOrDefault().Name;
                        work.TypeNameAr = _context.WorkOrderTypes.Where(a => a.Id == item.FirstOrDefault().WorkOrderTypeId).ToList().FirstOrDefault().NameAr;
                        work.PeriorityName = _context.WorkOrderPeriorities.Where(a => a.Id == item.FirstOrDefault().WorkOrderPeriorityId).ToList().FirstOrDefault().Name;
                        work.PeriorityNameAr = _context.WorkOrderPeriorities.Where(a => a.Id == item.FirstOrDefault().WorkOrderPeriorityId).ToList().FirstOrDefault().NameAr;
                        var lstStatus = _context.WorkOrderTrackings
                               .Include(t => t.WorkOrder).Include(t => t.WorkOrderStatus)
                               .Where(a => a.WorkOrderId == item.FirstOrDefault().Id).ToList().OrderByDescending(a => a.WorkOrderDate).ToList();
                        if (lstStatus.Count > 0)
                        {
                            work.StatusName = lstStatus[0].WorkOrderStatus.Name;
                            work.StatusNameAr = lstStatus[0].WorkOrderStatus.NameAr;
                            work.WorkOrderStatusId = lstStatus[0].WorkOrderStatus.Id;
                            work.statusColor = lstStatus[0].WorkOrderStatus.Color;

                        }

                        work.ActualStartDate = item.FirstOrDefault().ActualStartDate;
                        work.ActualEndDate = item.FirstOrDefault().ActualEndDate;
                        //   work.RequestId = item.FirstOrDefault().RequestId;
                        work.RequestId = item.FirstOrDefault().RequestId != null ? (int)item.FirstOrDefault().RequestId : 0;
                        work.HospitalId = item.FirstOrDefault().Request.AssetDetail.HospitalId;
                        work.AssignedTo = _context.WorkOrderTrackings.Where(a => a.WorkOrderId == item.FirstOrDefault().Id).ToList().FirstOrDefault().AssignedTo;
                        listAssignedUsers.Add(work);
                    }

                    list = listAssignedUsers.Where(a => a.HospitalId == UserObj.HospitalId).Concat(listCreatedByUser.Where(a => a.HospitalId == UserObj.HospitalId)).ToList();
                }
            }
            else
            {
                list = list.ToList();
            }

            if (statusId != 0)
            {
                list = list.Where(a => a.WorkOrderStatusId == statusId).ToList();
            }
            return list;
        }


        public IndexWorkOrderVM GetById(int id)
        {
            var requestObj = _context.WorkOrders.Where(a => a.Id == id).Include(w => w.Request).Include(w => w.WorkOrderType).Include(w => w.WorkOrderPeriority).Include(w => w.User)
                   .Select(work => new IndexWorkOrderVM
                   {
                       Id = work.Id,
                       Subject = work.Subject,
                       WorkOrderNumber = work.WorkOrderNumber,
                       CreationDate = work.CreationDate,
                       PlannedStartDate = work.PlannedStartDate,
                       PlannedEndDate = work.PlannedEndDate,
                       ActualStartDate = work.ActualStartDate,
                       ActualEndDate = work.ActualEndDate,
                       Note = work.Note,
                       MasterAssetId = work.Request.AssetDetail.MasterAssetId,
                       CreatedById = work.CreatedById,
                       CreatedBy = work.User.UserName,
                       WorkOrderPeriorityId = work.WorkOrderPeriorityId != null ? (int)work.WorkOrderPeriorityId : 0,
                       WorkOrderPeriorityName = work.WorkOrderPeriority.Name,
                       PeriorityName = work.WorkOrderPeriority.Name,
                       PeriorityNameAr = work.WorkOrderPeriority.NameAr,
                       //   WorkOrderTypeId = work.WorkOrderTypeId,
                       WorkOrderTypeId = work.WorkOrderTypeId != null ? (int)work.WorkOrderTypeId : 0,
                       WorkOrderTypeName = work.WorkOrderType.Name,
                       TypeName = work.WorkOrderType.Name,
                       TypeNameAr = work.WorkOrderType.NameAr,
                       //  RequestId = work.RequestId,
                       RequestId = work.RequestId != null ? (int)work.RequestId : 0,
                       RequestSubject = work.Request.Subject,
                       HospitalId = work.Request.AssetDetail.HospitalId,
                       WorkOrderTrackingId = _context.WorkOrderTrackings.Where(t => t.WorkOrderId == id).FirstOrDefault().Id,
                       WorkOrderStatusId = _context.WorkOrderTrackings.Where(t => t.WorkOrderId == id).FirstOrDefault().WorkOrderStatusId
                   }).FirstOrDefault();


            return requestObj;
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
                     SerialNumber = wo.WorkOrder.Request.AssetDetail.SerialNumber,
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
                printWorkObj.WorkOrderNumber = work.WorkOrderNumber;
                printWorkObj.CreationDate = work.CreationDate;
                printWorkObj.PlannedStartDate = work.PlannedStartDate;
                printWorkObj.PlannedEndDate = work.PlannedEndDate;
                printWorkObj.ActualStartDate = work.ActualStartDate;
                printWorkObj.ActualEndDate = work.ActualEndDate;
                printWorkObj.Note = work.Note;
                printWorkObj.CreatedById = work.CreatedById;
                printWorkObj.CreatedBy = work.User.UserName;
                printWorkObj.PeriorityName = work.WorkOrderPeriority.Name;
                printWorkObj.PeriorityNameAr = work.WorkOrderPeriority.NameAr;
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
                printWorkObj.SubProblemName = work.Request.SubProblem.Name;
                printWorkObj.SubProblemNameAr = work.Request.SubProblem.NameAr;
                printWorkObj.ProblemName = work.Request.SubProblem.Problem.Name;
                printWorkObj.ProblemNameAr = work.Request.SubProblem.Problem.NameAr;



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
                getDataObj.WorkOrderNumber = item.WorkOrderNumber;
                getDataObj.WorkOrderTypeName = item.WorkOrderType.Name;
                getDataObj.RequestSubject = item.Request.Subject;
                getDataObj.AssetId = item.Request.AssetDetailId;
                getDataObj.CreatedById = item.CreatedById;
                getDataObj.CreatedBy = item.User.UserName;
                //getDataObj.UserName = item.User.UserName;
                getDataObj.ActualStartDate = item.ActualStartDate;
                getDataObj.ActualEndDate = item.ActualEndDate;
                getDataObj.PlannedStartDate = item.PlannedStartDate;
                getDataObj.PlannedEndDate = item.PlannedEndDate;
                getDataObj.WorkOrderPeriorityId = item.WorkOrderPeriorityId != null ? (int)item.WorkOrderPeriorityId : 0;
                getDataObj.WorkOrderPeriorityName = item.WorkOrderPeriority.Name;
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

                var lstStatus = _context.WorkOrderTrackings
                            .Include(t => t.WorkOrder).Include(t => t.WorkOrderStatus)
                            .Where(a => a.WorkOrderId == item.Id).ToList().OrderByDescending(a => a.Id).ToList();
                if (lstStatus.Count > 0)
                {

                    getDataObj.WorkOrderStatusId = (int)lstStatus[0].WorkOrder.Id;
                    getDataObj.StatusName = lstStatus[0].WorkOrderStatus.Name;
                    getDataObj.StatusNameAr = lstStatus[0].WorkOrderStatus.NameAr;
                    getDataObj.statusColor = lstStatus[0].WorkOrderStatus.Color;

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

            if (searchObj.StatusId != 0)
            {
                lstData = lstData.Where(a => a.WorkOrderStatusId == searchObj.StatusId).ToList();
            }
            else
                lstData = lstData.ToList();

            if (searchObj.PeriorityId != 0)
            {
                lstData = lstData.Where(a => a.WorkOrderPeriorityId == searchObj.PeriorityId).ToList();
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
                lstData = lstData.Where(a => a.AssetId == searchObj.AssetDetailId).ToList();
            }
            else
                lstData = lstData.ToList();

            if (searchObj.MasterAssetId != 0)
            {
                lstData = lstData.Where(a => a.MasterAssetId == searchObj.MasterAssetId).ToList();
            }
            else
                lstData = lstData.ToList();

            if (searchObj.Subject != "")
            {
                lstData = lstData.Where(a => a.Subject == searchObj.Subject).ToList();
            }
            else
                lstData = lstData.ToList();




            if (searchObj.WONumber != "")
            {
                lstData = lstData.Where(b => b.WorkOrderNumber == searchObj.WONumber).ToList();
            }
            else
                lstData = lstData.ToList();



            string setstartday, setstartmonth, setendday, setendmonth = "";


            if (searchObj.Start == "")
            {
                searchObj.PlannedStartDate = DateTime.Parse("01/01/1900");
            }
            else
            {
                searchObj.PlannedStartDate = DateTime.Parse(searchObj.Start.ToString());
            }

            if (searchObj.End == "")
            {
                searchObj.PlannedEndDate = DateTime.Today.Date;
            }
            else
            {
                searchObj.PlannedEndDate = DateTime.Parse(searchObj.End.ToString());
            }



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
            var startingFrom = DateTime.Parse(sDate);





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
            var endingTo = DateTime.Parse(eDate);

            lstData = lstData.Where(a => a.PlannedStartDate >= startingFrom && a.PlannedStartDate <= endingTo).ToList();


            //lstData = lstData.Where(a => a.PlannedEndDate >= startingFrom && a.PlannedEndDate <= endingTo).ToList();


            return lstData;
        }

        public IEnumerable<IndexWorkOrderVM> SortWorkOrders(int hosId, string userId, SortWorkOrderVM sortObj)
        {
            var list =   GetAllWorkOrdersByHospitalId(hosId, userId);
            if (sortObj.WorkOrderNumber != "")
            {
                if (sortObj.SortStatus == "descending")
                    list = list.OrderByDescending(d => d.WorkOrderNumber).ToList();
                else
                    list = list.OrderBy(d => d.WorkOrderNumber).ToList();
            }
            else if (sortObj.StatusName != "")
            {
                if (sortObj.SortStatus == "descending")
                    list = list.OrderByDescending(d => d.StatusName).ToList();
                else
                    list = list.OrderBy(d => d.StatusName).ToList();
            }
            else if (sortObj.StatusNameAr != "")
            {
                if (sortObj.SortStatus == "descending")
                    list = list.OrderByDescending(d => d.StatusNameAr).ToList();
                else
                    list = list.OrderBy(d => d.StatusNameAr).ToList();
            }
            else if (sortObj.Subject != "")
            {
                if (sortObj.SortStatus == "descending")
                    list = list.OrderByDescending(d => d.Subject).ToList();
                else
                    list = list.OrderBy(d => d.Subject).ToList();
            }
            else if (sortObj.RequestSubject != "")
            {
                if (sortObj.SortStatus == "descending")
                    list = list.OrderByDescending(d => d.RequestSubject).ToList();
                else
                    list = list.OrderBy(d => d.RequestSubject).ToList();
            }
            else if (sortObj.CreatedBy != "")
            {
                if (sortObj.SortStatus == "descending")
                    list = list.OrderByDescending(d => d.CreatedBy).ToList();
                else
                    list = list.OrderBy(d => d.CreatedBy).ToList();
            }
            else if (sortObj.CreationDate != "")
            {
                if (sortObj.SortStatus == "descending")
                    list = list.OrderByDescending(d => d.CreationDate).ToList();
                else
                    list = list.OrderBy(d => d.CreationDate).ToList();
            }
            else if (sortObj.Note != "")
            {
                if (sortObj.SortStatus == "descending")
                    list = list.OrderByDescending(d => d.Note).ToList();
                else
                    list = list.OrderBy(d => d.Note).ToList();
            }
            return list;
        }

        public List<IndexWorkOrderVM> GetLastRequestAndWorkOrderByAssetId(int assetId)
        {
            //throw new NotImplementedException();

            return _context.WorkOrders.Include(a => a.Request)
                                    .Include(a => a.Request.AssetDetail)
                                    .Include(a => a.Request.AssetDetail.Hospital)
                                .Where(a => a.Request.AssetDetailId == assetId)
                                .OrderByDescending(a => a.CreationDate)
                                .ToList()
                                .Select(item => new IndexWorkOrderVM
                                {
                                    Id = item.Id,
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
