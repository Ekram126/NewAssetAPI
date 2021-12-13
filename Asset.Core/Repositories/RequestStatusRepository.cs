using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.RequestStatusVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class RequestStatusRepository : IRequestStatusRepository
    {
        private ApplicationDbContext _context;

        public RequestStatusRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Add(RequestStatus createRequestVM)
        {
            RequestStatus reqStatusObj = new RequestStatus();
            try
            {
                if (createRequestVM != null)
                {

                    reqStatusObj.Color = createRequestVM.Color;
                    reqStatusObj.Name = createRequestVM.Name;
                    reqStatusObj.NameAr = createRequestVM.NameAr;
                    _context.RequestStatus.Add(reqStatusObj);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return reqStatusObj.Id;
        }

        public int Delete(int id)
        {
            var reqStatusObj = _context.RequestStatus.Find(id);
            try
            {
                if (reqStatusObj != null)
                {
                    _context.RequestStatus.Remove(reqStatusObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }

        public IEnumerable<IndexRequestStatusVM.GetData> GetAll(string userId)
        {
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
            List<IndexRequestStatusVM.GetData> listCountRequests = new List<IndexRequestStatusVM.GetData>();


            var lstRequests = _context.Request.OrderByDescending(a => a.RequestDate).ToList();

            List<RequestTracking> lstTrack = new List<RequestTracking>();
            if (lstRequests.Count > 0)
            {
                foreach (var req in lstRequests)
                {
                    IndexRequestStatusVM.GetData countRequestObj = new IndexRequestStatusVM.GetData();
                    var lstRequesrStatus = _context.RequestTracking
                                     .Include(t => t.Request).Include(t => t.RequestStatus)
                                     .Where(a => a.RequestId == req.Id && a.RequestStatusId == 3).ToList().OrderByDescending(a => a.Id).ToList();
                    if (lstRequesrStatus.Count > 0)
                    {
                        var TrackObj = lstRequesrStatus[0];
                        foreach (var trk in lstRequesrStatus)
                        {
                            lstTrack.Add(trk);
                        }
                    }

                    countRequestObj.CountInProgress = lstTrack.Count;
                    listCountRequests.Add(countRequestObj);
                }
            }


            //var lstStatus = _context.RequestStatus.ToList();
            //foreach (var item in lstStatus)
            //{

            //    IndexRequestStatusVM.GetData countRequestObj = new IndexRequestStatusVM.GetData();
            //    countRequestObj.Id = item.Id;
            //    countRequestObj.Name = item.Name;
            //    countRequestObj.NameAr = item.NameAr;
            //    countRequestObj.Color = item.Color;
            //    countRequestObj.Icon = item.Icon;



            //    var list = _context.RequestTracking
            //                      .Include(t => t.Request)
            //                      //.Include(t => t.RequestStatus)
            //                      //.Include(t => t.Request.AssetDetail.Hospital)
            //                      //.Include(t => t.Request.AssetDetail.Hospital.Governorate)
            //                      //.Include(t => t.Request.AssetDetail.Hospital.City)
            //                      //.Include(t => t.Request.AssetDetail.Hospital.Organization)
            //                      //.Include(t => t.Request.AssetDetail.Hospital.SubOrganization)
            //                      // .Include(t => t.User)
            //                      .ToList().Where(a => a.RequestStatusId == item.Id).OrderByDescending(a => a.RequestStatusId).ToList();



            //    var listCountInProgress = list.Where(t => t.RequestStatusId == 3).ToList();
            //    countRequestObj.CountInProgress = listCountInProgress.Count;



            //    //if (list.Count > 0)
            //    //{
            //    //    if (UserObj.GovernorateId == 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
            //    //    {
            //    //        list = list.ToList();
            //    //    }

            //    //    if (UserObj.GovernorateId > 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
            //    //    {
            //    //        list = list.Where(t => t.Request.AssetDetail.Hospital.GovernorateId == UserObj.GovernorateId).ToList();
            //    //    }
            //    //    if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId == 0)
            //    //    {
            //    //        list = list.Where(t => t.Request.AssetDetail.Hospital.GovernorateId == UserObj.GovernorateId && t.Request.AssetDetail.Hospital.CityId == UserObj.CityId).ToList();
            //    //    }
            //    //    if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId > 0)
            //    //    {
            //    //        //  list = list.Where(t => t.Request.AssetDetail.Hospital.GovernorateId == UserObj.GovernorateId && t.Request.AssetDetail.Hospital.CityId == UserObj.CityId && t.Request.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();


            //    //        if (userRoleName == "Admin")
            //    //        {
            //    //            list = list.ToList();
            //    //        }
            //    //        if (userRoleName == "TLHospitalManager")
            //    //        {
            //    //            list = list.Where(t => t.Request.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
            //    //        }
            //    //        if (userRoleName == "EngDepManager")
            //    //        {
            //    //            list = list.Where(t => t.Request.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
            //    //        }
            //    //        if (userRoleName == "EngManager")
            //    //        {
            //    //            list = list.Where(t => t.Request.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
            //    //        }
            //    //        if (userRoleName == "AssetOwner")
            //    //        {


            //    //            if (item.Id == 1)
            //    //            {
            //    //                List<Request> lstRequests = new List<Request>();
            //    //                foreach (var itm in list)
            //    //                {
            //    //                    var listWO = _context.WorkOrders.Include(a => a.Request).Where(a => a.RequestId == itm.RequestId).ToList();
            //    //                    if (listWO.Count == 0)
            //    //                    {
            //    //                        Request requestObj = new Request();
            //    //                        lstRequests.Add(requestObj);
            //    //                    }
            //    //                    countRequestObj.CountOpen = lstRequests.Count;
            //    //                }
            //    //            }



            //    //            if (item.Id == 2)
            //    //            {
            //    //                list = list.Where(t => t.Request.AssetDetail.HospitalId == UserObj.HospitalId && t.Request.CreatedById == userId && t.RequestStatusId == 2).ToList();

            //    //                countRequestObj.CountClosed = list.Count;
            //    //            }


            //    //            if (item.Id == 3)
            //    //            {

            //    //                var listCountInProgress = list.Where(t => t.Request.AssetDetail.HospitalId == UserObj.HospitalId && t.Request.CreatedById == userId && t.RequestStatusId == 3).ToList();
            //    //                countRequestObj.CountInProgress = listCountInProgress.Count;
            //    //            }


            //    //            if (item.Id == 4)
            //    //            {

            //    //                var listCountSolved = list.Where(t => t.Request.AssetDetail.HospitalId == UserObj.HospitalId && t.Request.CreatedById == userId && t.RequestStatusId == 4).ToList();

            //    //                countRequestObj.CountSolved = listCountSolved.Count;
            //    //            }


            //    //            if (item.Id == 5)
            //    //            {
            //    //                var listCountApproved = list.Where(t => t.Request.AssetDetail.HospitalId == UserObj.HospitalId && t.Request.CreatedById == userId && t.RequestStatusId == 5).ToList();
            //    //                countRequestObj.CountApproved = listCountApproved.Count;
            //    //            }

            //    //        }


            //    //    }
            //    //    if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId == 0 && UserObj.HospitalId == 0)
            //    //    {
            //    //        list = list.Where(t => t.Request.AssetDetail.Hospital.OrganizationId == UserObj.OrganizationId).ToList();
            //    //    }
            //    //    if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId == 0)
            //    //    {
            //    //        list = list.Where(t => t.Request.AssetDetail.Hospital.OrganizationId == UserObj.OrganizationId && t.Request.AssetDetail.Hospital.SubOrganizationId == UserObj.SubOrganizationId).ToList();
            //    //    }

            //    //    if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId > 0)
            //    //    {
            //    //        if (userRoleName == "Admin")
            //    //        {
            //    //            list = list.ToList();
            //    //        }
            //    //        if (userRoleName == "TLHospitalManager")
            //    //        {
            //    //            list = list.Where(t => t.Request.AssetDetail.Hospital.SubOrganizationId == UserObj.SubOrganizationId && t.Request.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
            //    //        }
            //    //        if (userRoleName == "EngDepManager")
            //    //        {
            //    //            list = list.Where(t => t.Request.AssetDetail.Hospital.SubOrganizationId == UserObj.SubOrganizationId && t.Request.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
            //    //        }
            //    //        if (userRoleName == "EngManager")
            //    //        {
            //    //            list = list.Where(t => t.Request.AssetDetail.Hospital.SubOrganizationId == UserObj.SubOrganizationId && t.Request.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
            //    //        }
            //    //        if (userRoleName == "AssetOwner")
            //    //        {


            //    //            if (item.Id == 1)
            //    //            {
            //    //                List<Request> lstRequests = new List<Request>();
            //    //                foreach (var itm in list)
            //    //                {
            //    //                    var listWO = _context.WorkOrders.Include(a => a.Request).Where(a => a.RequestId == itm.RequestId).ToList();
            //    //                    if (listWO.Count == 0)
            //    //                    {
            //    //                        Request requestObj = new Request();
            //    //                        lstRequests.Add(requestObj);
            //    //                    }
            //    //                    countRequestObj.CountOpen = lstRequests.Count;
            //    //                }
            //    //            }



            //    //            if (item.Id == 2)
            //    //            {
            //    //                list = list.Where(t => t.Request.AssetDetail.HospitalId == UserObj.HospitalId && t.CreatedById == userId && t.RequestStatusId == 2).ToList();

            //    //                countRequestObj.CountClosed = list.Count;
            //    //            }


            //    //            if (item.Id == 3)
            //    //            {
            //    //                //      list = list.Where(a => a.RequestStatusId == 3 && a.Request.AssetDetail.HospitalId == UserObj.HospitalId && a.User.Id == userId).ToList();
            //    //                var listCountInProgress = list.Where(t => t.Request.AssetDetail.HospitalId == UserObj.HospitalId && t.CreatedById == userId && t.RequestStatusId == 3).ToList();
            //    //                countRequestObj.CountInProgress = listCountInProgress.Count;
            //    //            }


            //    //            if (item.Id == 4)
            //    //            {
            //    //                list = list.Where(t => t.Request.AssetDetail.HospitalId == UserObj.HospitalId && t.CreatedById == userId && t.RequestStatusId == 4).ToList();

            //    //                countRequestObj.CountSolved = list.Count;
            //    //            }


            //    //            if (item.Id == 5)
            //    //            {
            //    //                list = list.Where(t => t.Request.AssetDetail.HospitalId == UserObj.HospitalId && t.CreatedById == userId && t.RequestStatusId == 5).ToList();

            //    //                countRequestObj.CountApproved = list.Count;
            //    //            }

            //    //        }
            //    //    }
            //    //}

            //    listCountRequests.Add(countRequestObj);
            //}
            return listCountRequests;
        }

        public IEnumerable<IndexRequestStatusVM.GetData> GetAll()
        {
            return _context.RequestStatus.ToList();
        }

        public RequestStatus GetById(int id)
        {
            return _context.RequestStatus.Find(id);
        }

        public int Update(RequestStatus editRequestVM)
        {
            try
            {
                var reqStatusObj = _context.RequestStatus.Find(editRequestVM.Id);
                reqStatusObj.Color = editRequestVM.Color;
                reqStatusObj.Name = editRequestVM.Name;
                reqStatusObj.NameAr = editRequestVM.NameAr;
                _context.Entry(reqStatusObj).State = EntityState.Modified;
                _context.SaveChanges();
                return reqStatusObj.Id;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }


    }
}
