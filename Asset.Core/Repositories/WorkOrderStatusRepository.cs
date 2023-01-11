using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.WorkOrderStatusVM;
using Asset.ViewModels.WorkOrderVM;
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
        public IndexWorkOrderStatusVM GetAll(string userId)
        {

            //List<IndexWorkOrderStatusVM> listCountWorkOrders = new List<IndexWorkOrderStatusVM>();
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
            IndexWorkOrderStatusVM itemObj = new IndexWorkOrderStatusVM();



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


                var statusIds = new List<int>(new int[] { 6, 7, 8, 10 });
                var lstStatus = _context.WorkOrderStatuses.Where(a => !statusIds.Any(x => x == a.Id)).ToList();
                itemObj.ListStatus = lstStatus;
                var workorders = _context.WorkOrders
                                    .Include(a => a.Request)
                                    .Include(a => a.Request.AssetDetail)
                                    .Include(a => a.Request.AssetDetail.Hospital)
                                    .Include(a => a.Request.AssetDetail.Hospital.Governorate)
                                    .Include(a => a.Request.AssetDetail.Hospital.City)
                                    .Include(a => a.Request.AssetDetail.Hospital.Organization)
                                    .Include(a => a.Request.AssetDetail.Hospital.SubOrganization)
                                    .Include(a => a.User).ToList();



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
                        workorders = workorders.Where(t => t.Request.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
                    }
                    if (lstRoleNames.Contains("EngDepManager") && !lstRoleNames.Contains("Eng"))
                    {
                        workorders = workorders.Where(t => t.Request.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                    }
                    if (lstRoleNames.Contains("Eng") && !lstRoleNames.Contains("EngDepManager"))
                    {
                        List<WorkOrder> list = new List<WorkOrder>();
                        var lstEngWorkorders = _context.WorkOrderTrackings.Include(a => a.WorkOrder)
                            .Where(t => t.AssignedTo == userId && t.WorkOrder.Request.AssetDetail.Hospital.Id == UserObj.HospitalId)
                            .Select(a => a.WorkOrder).ToList().GroupBy(a => a.Id).ToList();
                        foreach (var item in lstEngWorkorders)
                        {
                            WorkOrder workOrderObj = new WorkOrder();
                            workOrderObj.Id = item.Key;

                            list.Add(workOrderObj);
                        }
                        workorders = list.ToList();
                    }
                    if (lstRoleNames.Contains("EngDepManager") && lstRoleNames.Contains("Eng"))
                    {
                        workorders = workorders.Where(t => t.Request.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
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
                        workorders = workorders.Where(t => t.Request.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
                    }
                    //if (lstRoleNames.Contains("EngDepManager") && !lstRoleNames.Contains("Eng"))
                    //{
                    //    workorders = workorders.Where(t => t.Request.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                    //}
                    //if (lstRoleNames.Contains("AssetOwner"))
                    //{
                    //    workorders = workorders.Where(t => t.Request.AssetDetail.Hospital.Id == UserObj.HospitalId && t.CreatedById == userId).ToList();
                    //}

                    if (lstRoleNames.Contains("AssetOwner"))
                    {
                        workorders = workorders.Where(t => t.Request.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                    }
                    //if (lstRoleNames.Contains("EngDepManager"))
                    //{
                    //    workorders = workorders.Where(t => t.Request.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                    //}
                    if (lstRoleNames.Contains("Eng") && !lstRoleNames.Contains("EngDepManager"))
                    {
                        workorders = workorders.Where(t => t.Request.AssetDetail.Hospital.Id == UserObj.HospitalId && t.CreatedById == userId).ToList();
                    }
                    if (lstRoleNames.Contains("Eng") && lstRoleNames.Contains("EngDepManager"))
                    {
                        //  workorders = workorders.Where(t => t.Request.AssetDetail.Hospital.Id == UserObj.HospitalId && t.CreatedById == userId).ToList();
                        workorders = workorders.Where(t => t.Request.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                    }
                }


                if (workorders.Count > 0)
                {

                    foreach (var wo in workorders)
                    {
                        var trackObj = _context.WorkOrderTrackings.OrderByDescending(a => a.CreationDate).Where(a => a.WorkOrderId == wo.Id).FirstOrDefault();
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

                itemObj.CountAssigned = lstAssignedTracks.Count;
                itemObj.CountClosed = lstCloseTracks.Count;
                itemObj.CountInProgress = lstInProgressTracks.Count;
                itemObj.CountDone = lstDoneTracks.Count;
                itemObj.CountEscalate = lstEscalateTracks.Count;
                itemObj.CountExternalSupport = lstExternalSupportTracks.Count;
                itemObj.CountPending = lstPendingTracks.Count;
                itemObj.CountReAssigned = lstReAssignedTracks.Count;
                itemObj.CountReview = lstReviewTracks.Count;
                itemObj.CountSparePart = lstSparePartTracks.Count;
                itemObj.CountTechApprove = lstTechApproveTracks.Count;
                itemObj.CountUserApprove = lstUserApproveTracks.Count;




                itemObj.CountAll = (lstAssignedTracks.Count + lstCloseTracks.Count + lstInProgressTracks.Count + lstDoneTracks.Count +
                    lstEscalateTracks.Count + lstExternalSupportTracks.Count + lstPendingTracks.Count + lstReAssignedTracks.Count + lstReviewTracks.Count + lstSparePartTracks.Count + lstTechApproveTracks.Count + lstUserApproveTracks.Count);
            }

            return itemObj;
        }
        public IEnumerable<IndexWorkOrderStatusVM> GetAll()
        {


            var statusIds = new List<int>(new int[] { 6, 7, 8, 10 });


            return _context.WorkOrderStatuses.Where(a => !statusIds.Any(x => x == a.Id)).Select(item => new IndexWorkOrderStatusVM
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
        public IEnumerable<IndexWorkOrderStatusVM> SortWOStatuses(SortWorkOrderStatusVM sortObj)
        {
            var lstWOStatuses = GetAll().ToList();
            if (sortObj.Code != "")
            {
                if (sortObj.SortStatus == "descending")
                    lstWOStatuses = lstWOStatuses.OrderByDescending(d => d.Code).ToList();
                else
                    lstWOStatuses = lstWOStatuses.OrderBy(d => d.Code).ToList();
            }
            else if (sortObj.Name != "")
            {
                if (sortObj.SortStatus == "descending")
                    lstWOStatuses = lstWOStatuses.OrderByDescending(d => d.Name).ToList();
                else
                    lstWOStatuses = lstWOStatuses.OrderBy(d => d.Name).ToList();
            }

            else if (sortObj.NameAr != "")
            {
                if (sortObj.SortStatus == "descending")
                    lstWOStatuses = lstWOStatuses.OrderByDescending(d => d.NameAr).ToList();
                else
                    lstWOStatuses = lstWOStatuses.OrderBy(d => d.NameAr).ToList();
            }

            return lstWOStatuses;
        }






        public IndexWorkOrderStatusVM GetAllForReport(SearchWorkOrderByDateVM woDateObj)
        {

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
            IndexWorkOrderStatusVM itemObj = new IndexWorkOrderStatusVM();



            ApplicationUser UserObj = new ApplicationUser();
            ApplicationRole roleObj = new ApplicationRole();
            List<string> lstRoleNames = new List<string>();

            var obj = _context.ApplicationUser.Where(a => a.Id == woDateObj.UserId).ToList();
            if (obj.Count > 0)
            {
                UserObj = obj[0];

                var roleNames = (from userRole in _context.UserRoles
                                 join role in _context.Roles on userRole.RoleId equals role.Id
                                 where userRole.UserId == woDateObj.UserId
                                 select role);
                foreach (var item in roleNames)
                {
                    lstRoleNames.Add(item.Name);
                }
            }

            var statusIds = new List<int>(new int[] { 6, 7, 8, 10 });
            var lstStatus = _context.WorkOrderStatuses.Where(a => !statusIds.Any(x => x == a.Id)).ToList();
            itemObj.ListStatus = lstStatus;




            DateTime? start = new DateTime();
            DateTime? end = new DateTime();
            if (woDateObj.StrStartDate != "")
            {
                woDateObj.StartDate = DateTime.Parse(woDateObj.StrStartDate);
                start = DateTime.Parse(woDateObj.StrStartDate);
                woDateObj.StartDate = start;
            }
            else
            {
                woDateObj.StartDate = DateTime.Parse("01/01/1900");
                start = DateTime.Parse(woDateObj.StartDate.ToString());
            }


            if (woDateObj.StrEndDate != "")
            {
                woDateObj.EndDate = DateTime.Parse(woDateObj.StrEndDate);
                end = DateTime.Parse(woDateObj.StrEndDate);
                woDateObj.EndDate = end;
            }
            else
            {
                woDateObj.EndDate = DateTime.Today.Date;
                end = DateTime.Parse(woDateObj.EndDate.ToString());
            }





            var workorders = _context.WorkOrders
                                .Include(a => a.Request)
                                .Include(a => a.Request.AssetDetail)
                                .Include(a => a.Request.AssetDetail.Hospital)
                                .Include(a => a.Request.AssetDetail.Hospital.Governorate)
                                .Include(a => a.Request.AssetDetail.Hospital.City)
                                .Include(a => a.Request.AssetDetail.Hospital.Organization)
                                .Include(a => a.Request.AssetDetail.Hospital.SubOrganization)
                                .Include(a => a.User).OrderByDescending(a => a.CreationDate.Value.Date).ToList();



            if (UserObj.GovernorateId == 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0 && UserObj.OrganizationId == 0 && UserObj.SubOrganizationId == 0)
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

            if (UserObj.HospitalId > 0)
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
                    workorders = workorders.Where(t => t.Request.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
                }
                if (lstRoleNames.Contains("AssetOwner"))
                {
                    workorders = workorders.Where(t => t.Request.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                }
            }


            if (start != null && end != null)
            {
                workorders = workorders.Where(a => a.CreationDate >= start.Value && a.CreationDate <= end.Value).ToList();
            }
            else
            {
                workorders = workorders.ToList();
            }


            if (workorders.Count > 0)
            {

                foreach (var wo in workorders)
                {
                    var trackObj = _context.WorkOrderTrackings.OrderByDescending(a => a.CreationDate).Where(a => a.WorkOrderId == wo.Id).FirstOrDefault();
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

            itemObj.CountAssigned = lstAssignedTracks.Count;
            itemObj.CountClosed = lstCloseTracks.Count;
            itemObj.CountInProgress = lstInProgressTracks.Count;
            itemObj.CountDone = lstDoneTracks.Count;
            itemObj.CountEscalate = lstEscalateTracks.Count;
            itemObj.CountExternalSupport = lstExternalSupportTracks.Count;
            itemObj.CountPending = lstPendingTracks.Count;
            itemObj.CountReAssigned = lstReAssignedTracks.Count;
            itemObj.CountReview = lstReviewTracks.Count;
            itemObj.CountSparePart = lstSparePartTracks.Count;
            itemObj.CountTechApprove = lstTechApproveTracks.Count;
            itemObj.CountUserApprove = lstUserApproveTracks.Count;




            itemObj.CountAll = (lstAssignedTracks.Count + lstCloseTracks.Count + lstInProgressTracks.Count + lstDoneTracks.Count +
                lstEscalateTracks.Count + lstExternalSupportTracks.Count + lstPendingTracks.Count + lstReAssignedTracks.Count + lstReviewTracks.Count + lstSparePartTracks.Count + lstTechApproveTracks.Count + lstUserApproveTracks.Count);


            return itemObj;
        }
    }
}
