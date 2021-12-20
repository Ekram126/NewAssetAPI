using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.WorkOrderStatusVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class WorkOrderStatusRepository : IWorkOrderStatusRepository
    {
        private ApplicationDbContext _context;
        string msg;
        public WorkOrderStatusRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(CreateWorkOrderStatusVM createWorkOrderStatusVM)
        {
            try
            {
                if (createWorkOrderStatusVM != null)
                {
                    WorkOrderStatus workOrderStatus = new WorkOrderStatus();
                    workOrderStatus.Name = createWorkOrderStatusVM.Name;
                    workOrderStatus.NameAr = createWorkOrderStatusVM.NameAr;
                    workOrderStatus.Code = createWorkOrderStatusVM.Code;
                    workOrderStatus.Color = createWorkOrderStatusVM.Color;
                    workOrderStatus.Icon = createWorkOrderStatusVM.Icon;
                    _context.WorkOrderStatuses.Add(workOrderStatus);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }

        public void Delete(int id)
        {
            var WorkOrderStatus = _context.WorkOrderStatuses.Find(id);
            try
            {
                if (WorkOrderStatus != null)
                {
                    _context.WorkOrderStatuses.Remove(WorkOrderStatus);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }


        public IEnumerable<IndexWorkOrderStatusVM> GetAll(string userId)
        {

            List<IndexWorkOrderStatusVM> listCountWorkOrders = new List<IndexWorkOrderStatusVM>();
            List<WorkOrderTracking> lstAssignedTracks = new List<WorkOrderTracking>();
            List<WorkOrderTracking> lstInProgressTracks = new List<WorkOrderTracking>();
            List<WorkOrderTracking> lstExternalSupportTracks = new List<WorkOrderTracking>();
            List<WorkOrderTracking> lstSparePartTracks = new List<WorkOrderTracking>();
            List<WorkOrderTracking> lstEscalateTracks = new List<WorkOrderTracking>();
            List<WorkOrderTracking> lstPendingTracks = new List<WorkOrderTracking>();
            List<WorkOrderTracking> lstDoneTracks = new List<WorkOrderTracking>();
            List<WorkOrderTracking> lstReviewTracks = new List<WorkOrderTracking>();
            List<WorkOrderTracking> lstReAssignedTracks = new List<WorkOrderTracking>();
            List<WorkOrderTracking> lstTechApproveTracks = new List<WorkOrderTracking>();
            List<WorkOrderTracking> lstUserApproveTracks = new List<WorkOrderTracking>();
            List<WorkOrderTracking> lstCloseTracks = new List<WorkOrderTracking>();
            IndexWorkOrderStatusVM getDataObj = new IndexWorkOrderStatusVM();



            ApplicationUser UserObj = new ApplicationUser();
            ApplicationRole roleObj = new ApplicationRole();
            List<string> lstRoleNames = new List<string>();
            var obj = _context.ApplicationUser.Where(a => a.Id == userId).ToList();
            if (obj.Count > 0)
            {
                UserObj = obj[0];

                var roleNames = (from userRole in _context.UserRoles
                                 join role in _context.Roles on userRole.RoleId equals role.Id
                                 where userRole.UserId == userId
                                 select role);
                foreach (var item in roleNames)
                {
                    lstRoleNames.Add(item.Name);
                }



                var lstStatus = _context.WorkOrderStatuses.ToList();
                getDataObj.ListStatus = lstStatus;
                var workorders = _context.WorkOrders
                                    .Include(a => a.Request)
                                    .Include(a => a.Request.AssetDetail)
                                    .Include(a => a.Request.AssetDetail.Hospital)
                                    .Include(a => a.User).ToList();

                getDataObj.GovernorateId = workorders[0].Request.AssetDetail.Hospital.GovernorateId;
                getDataObj.CityId = workorders[0].Request.AssetDetail.Hospital.CityId;
                getDataObj.OrganizationId = workorders[0].Request.AssetDetail.Hospital.OrganizationId;
                getDataObj.SubOrganizationId = workorders[0].Request.AssetDetail.Hospital.SubOrganizationId;
                getDataObj.HospitalId = workorders[0].Request.AssetDetail.HospitalId;


                if (UserObj.GovernorateId == 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
                {
                    workorders = workorders.ToList();
                }
                if (UserObj.GovernorateId > 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
                {
                    workorders = workorders.Where(t => t.Request.AssetDetail.Hospital.GovernorateId == UserObj.GovernorateId).ToList();
                }
                if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId == 0)
                {
                    workorders = workorders.Where(t => t.Request.AssetDetail.Hospital.CityId == UserObj.CityId).ToList();
                }
                if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId == 0 && UserObj.HospitalId == 0)
                {
                    workorders = workorders.Where(t => t.Request.AssetDetail.Hospital.OrganizationId == UserObj.OrganizationId).ToList();
                }
                if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId == 0)
                {
                    workorders = workorders.Where(t => t.Request.AssetDetail.Hospital.SubOrganizationId == UserObj.SubOrganizationId).ToList();
                }

                if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId > 0)
                {
                    if (lstRoleNames.Contains("Admin"))
                    {
                        workorders = workorders.ToList();
                    }
                    if (lstRoleNames.Contains("TLHospitalManager"))
                    {
                        workorders = workorders.Where(t => t.Request.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                    }
                    if (lstRoleNames.Contains("EngDepManager"))
                    {
                        workorders = workorders.Where(t => t.Request.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                    }
                    if (lstRoleNames.Contains("EngManager"))
                    {
                        workorders = workorders.Where(t => t.Request.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                    }
                    if (lstRoleNames.Contains("Eng"))
                    {
                        List<WorkOrder> list = new List<WorkOrder>();
                        var lstEngWorkorders = _context.WorkOrderTrackings.Include(a => a.WorkOrder)
                            .Where(t => t.AssignedTo == userId && t.WorkOrder.Request.AssetDetail.Hospital.Id == UserObj.HospitalId)
                            .Select(a=>a.WorkOrder).ToList().GroupBy(a=>a.Id).ToList();
                        foreach (var item in lstEngWorkorders)
                        {
                            WorkOrder workOrderObj = new WorkOrder();
                            workOrderObj.Id = item.Key;

                            list.Add(workOrderObj);
                        }
                        workorders = list.ToList();
                    }
                    if (lstRoleNames.Contains("AssetOwner"))
                    {
                        workorders = workorders.Where(t => t.Request.AssetDetail.Hospital.Id == UserObj.HospitalId && t.CreatedById == userId).ToList();
                    }
                }
                if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId > 0)
                {
                    if (lstRoleNames.Contains("Admin"))
                    {
                        workorders = workorders.ToList();
                    }
                    if (lstRoleNames.Contains("TLHospitalManager"))
                    {
                        workorders = workorders.Where(t => t.Request.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                    }
                    if (lstRoleNames.Contains("EngDepManager"))
                    {
                        workorders = workorders.Where(t => t.Request.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                    }
                    if (lstRoleNames.Contains("EngManager"))
                    {
                        workorders = workorders.Where(t => t.Request.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                    }
                    if (lstRoleNames.Contains("AssetOwner"))
                    {
                        workorders = workorders.Where(t => t.Request.AssetDetail.Hospital.Id == UserObj.HospitalId && t.CreatedById == userId).ToList();
                    }
                    if (lstRoleNames.Contains("Eng"))
                    {
                        workorders = workorders.Where(t => t.Request.AssetDetail.Hospital.Id == UserObj.HospitalId && t.CreatedById == userId).ToList();
                    }

                }


                if (workorders.Count > 0)
                {

                    foreach (var wo in workorders)
                    {
                        var trackObj = _context.WorkOrderTrackings.OrderByDescending(a => a.Id).FirstOrDefault(a => a.WorkOrderId == wo.Id);
                        if (trackObj != null)
                        {
                            WorkOrderTracking trk = trackObj;

                            if (trk.WorkOrderStatusId == 1)
                            {
                                lstAssignedTracks.Add(trk);
                            }
                            if (trk.WorkOrderStatusId == 2)
                            {
                                lstInProgressTracks.Add(trk);
                            }
                            if (trk.WorkOrderStatusId == 3)
                            {
                                lstExternalSupportTracks.Add(trk);
                            }
                            if (trk.WorkOrderStatusId == 4)
                            {
                                lstSparePartTracks.Add(trk);
                            }
                            if (trk.WorkOrderStatusId == 5)
                            {
                                lstEscalateTracks.Add(trk);
                            }
                            if (trk.WorkOrderStatusId == 6)
                            {
                                lstPendingTracks.Add(trk);
                            }
                            if (trk.WorkOrderStatusId == 7)
                            {
                                lstDoneTracks.Add(trk);
                            }
                            if (trk.WorkOrderStatusId == 8)
                            {
                                lstReviewTracks.Add(trk);
                            }
                            if (trk.WorkOrderStatusId == 9)
                            {
                                lstReAssignedTracks.Add(trk);
                            }
                            if (trk.WorkOrderStatusId == 10)
                            {
                                lstTechApproveTracks.Add(trk);
                            }
                            if (trk.WorkOrderStatusId == 11)
                            {
                                lstUserApproveTracks.Add(trk);
                            }
                            if (trk.WorkOrderStatusId == 12)
                            {
                                lstCloseTracks.Add(trk);
                            }
                        }

                    }
                }

                getDataObj.CountAssigned = lstAssignedTracks.Count;
                getDataObj.CountClosed = lstCloseTracks.Count;
                getDataObj.CountInProgress = lstInProgressTracks.Count;
                getDataObj.CountDone = lstDoneTracks.Count;
                getDataObj.CountEscalate = lstEscalateTracks.Count;
                getDataObj.CountExternalSupport = lstExternalSupportTracks.Count;
                getDataObj.CountPending = lstPendingTracks.Count;
                getDataObj.CountReAssigned = lstReAssignedTracks.Count;
                getDataObj.CountReview = lstReviewTracks.Count;
                getDataObj.CountSparePart = lstSparePartTracks.Count;
                getDataObj.CountTechApprove = lstTechApproveTracks.Count;
                getDataObj.CountUserApprove = lstUserApproveTracks.Count;
                listCountWorkOrders.Add(getDataObj);

            }

            return listCountWorkOrders;
        }
        public IEnumerable<IndexWorkOrderStatusVM> GetAll()
        {
            return _context.WorkOrderStatuses.Select(item => new IndexWorkOrderStatusVM
            {
                Id = item.Id,
                Color = item.Color,
                Name = item.Name,
                NameAr = item.NameAr,
                Code = item.Code,
                Icon = item.Icon
            }).ToList();
        }

        public IndexWorkOrderStatusVM GetById(int id)
        {
            return _context.WorkOrderStatuses.Where(a => a.Id == id).Select(item => new IndexWorkOrderStatusVM
            {
                Id = item.Id,
                Color = item.Color,
                Name = item.Name,
                NameAr = item.NameAr,
                Code = item.Code,
                Icon = item.Icon
            }).FirstOrDefault();
        }

        public void Update(int id, EditWorkOrderStatusVM editWorkOrderStatusVM)
        {
            try
            {
                WorkOrderStatus workOrderStatus = new WorkOrderStatus();
                workOrderStatus.Id = editWorkOrderStatusVM.Id;
                workOrderStatus.Name = editWorkOrderStatusVM.Name;
                workOrderStatus.NameAr = editWorkOrderStatusVM.NameAr;
                workOrderStatus.Code = editWorkOrderStatusVM.Code;
                workOrderStatus.Icon = editWorkOrderStatusVM.Icon;
                workOrderStatus.Color = editWorkOrderStatusVM.Color;
                _context.Entry(workOrderStatus).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }
    }
}
