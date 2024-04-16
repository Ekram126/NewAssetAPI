using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.RequestStatusVM;
using Asset.ViewModels.RequestVM;
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
                    reqStatusObj.Icon = createRequestVM.Icon;
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

        public IndexRequestStatusVM.GetData GetAll(string userId)
        {
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

            }

            IndexRequestStatusVM.GetData ItemObj = new IndexRequestStatusVM.GetData();
            List<RequestTracking> lstOpenTracks = new List<RequestTracking>();
            List<RequestTracking> lstCloseTracks = new List<RequestTracking>();
            List<RequestTracking> lstInProgressTracks = new List<RequestTracking>();
            List<RequestTracking> lstSolvedTracks = new List<RequestTracking>();
            List<RequestTracking> lstApprovedTracks = new List<RequestTracking>();


            var lstStatus = _context.RequestStatus.Where(a => a.Id != 5).ToList();
            ItemObj.ListStatus = lstStatus;


            var requests = _context.Request.Include(a => a.AssetDetail).Include(a => a.AssetDetail.Hospital)
                .Include(a => a.AssetDetail.Hospital.Governorate)
                  .Include(a => a.AssetDetail.Hospital.City)
                    .Include(a => a.AssetDetail.Hospital.Organization)
                      .Include(a => a.AssetDetail.Hospital.SubOrganization)
                .Include(a => a.User).OrderByDescending(a => a.RequestDate.Date).ToList();

            if (UserObj.GovernorateId == 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
            {
                requests = requests.ToList();
            }
            if (UserObj.GovernorateId > 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
            {
                requests = requests.Where(t => t.AssetDetail.Hospital.GovernorateId == UserObj.GovernorateId).ToList();
            }
            if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId == 0)
            {
                requests = requests.Where(t => t.AssetDetail.Hospital.CityId == UserObj.CityId).ToList();
            }
            if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId == 0 && UserObj.HospitalId == 0)
            {
                requests = requests.Where(t => t.AssetDetail.Hospital.OrganizationId == UserObj.OrganizationId).ToList();
            }
            if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId == 0)
            {
                requests = requests.Where(t => t.AssetDetail.Hospital.SubOrganizationId == UserObj.SubOrganizationId).ToList();
            }
            if (UserObj.HospitalId > 0)
            {
                if (lstRoleNames.Contains("Admin"))
                {
                    requests = requests.ToList();
                }
                if (lstRoleNames.Contains("TLHospitalManager"))
                {
                    requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                }
                if (lstRoleNames.Contains("EngDepManager") && lstRoleNames.Contains("Eng"))
                {
                    requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                }
                if (!lstRoleNames.Contains("EngDepManager") && lstRoleNames.Contains("Eng"))
                {
                    requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId && t.CreatedById == userId).ToList();
                }

                if (!lstRoleNames.Contains("EngDepManager") && !lstRoleNames.Contains("Eng"))
                {
                    requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                }
                if (lstRoleNames.Contains("AssetOwner"))
                {
                    requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId && t.CreatedById == userId).ToList();
                }
                if (lstRoleNames.Contains("SRCreator") && !lstRoleNames.Contains("SRReviewer"))
                {
                    requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId && t.CreatedById == userId).ToList();
                }

                if (lstRoleNames.Contains("SRCreator") && lstRoleNames.Contains("SRReviewer"))
                {
                    requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                }

                if (lstRoleNames.Contains("AssetOwner") && !lstRoleNames.Contains("SRReviewer"))
                {
                    requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                }



                if (lstRoleNames.Contains("SRReviewer") && !lstRoleNames.Contains("SRCreator"))
                {

                    List<Request> lstRequests = new List<Request>();
                    var lstTracks = _context.RequestTracking.Where(a => a.Request.HospitalId == UserObj.HospitalId).OrderByDescending(a => a.DescriptionDate).ToList().GroupBy(a => a.RequestId).ToList();
                    foreach (var item in lstTracks)
                    {
                        if (item.FirstOrDefault().RequestStatusId == 4)
                        {
                            lstRequests.Add(_context.Request.Where(a => a.Id == item.FirstOrDefault().RequestId).FirstOrDefault());
                        }
                    }
                    requests = lstRequests;
                }
                if (lstRoleNames.Contains("EngDepManager"))
                {
                    requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                }
            }

            if (requests.Count > 0)
            {
                foreach (var req in requests)
                {
                    // var trackObj = _context.RequestTracking.OrderByDescending(a => a.DescriptionDate.Value.Date).FirstOrDefault(a => a.RequestId == req.Id);
                    var trackObj = _context.RequestTracking.OrderByDescending(a => a.DescriptionDate).Where(a => a.RequestId == req.Id).FirstOrDefault();


                    if (trackObj != null)
                    {
                        RequestTracking trk = trackObj;

                        if (trk.RequestStatusId == 1)
                        {
                            lstOpenTracks.Add(trk);
                        }
                        if (trk.RequestStatusId == 2)
                        {
                            lstCloseTracks.Add(trk);
                        }
                        if (trk.RequestStatusId == 3)
                        {
                            lstInProgressTracks.Add(trk);
                        }
                        if (trk.RequestStatusId == 4)
                        {
                            lstSolvedTracks.Add(trk);
                        }
                        if (trk.RequestStatusId == 5)
                        {
                            lstApprovedTracks.Add(trk);
                        }
                    }

                }
            }

            ItemObj.CountOpen = lstOpenTracks.Count;
            ItemObj.CountClosed = lstCloseTracks.Count;
            ItemObj.CountInProgress = lstInProgressTracks.Count;
            ItemObj.CountSolved = lstSolvedTracks.Count;
            ItemObj.CountApproved = lstApprovedTracks.Count;
            ItemObj.CountAll = requests.Count;

            return ItemObj;
        }


        public IndexRequestStatusVM.GetData GetAllByHospitalId(string userId, int hospitalId)
        {
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

            }

            IndexRequestStatusVM.GetData ItemObj = new IndexRequestStatusVM.GetData();
            List<RequestTracking> lstOpenTracks = new List<RequestTracking>();
            List<RequestTracking> lstCloseTracks = new List<RequestTracking>();
            List<RequestTracking> lstInProgressTracks = new List<RequestTracking>();
            List<RequestTracking> lstSolvedTracks = new List<RequestTracking>();
            List<RequestTracking> lstApprovedTracks = new List<RequestTracking>();


            var lstStatus = _context.RequestStatus.Where(a => a.Id != 5).ToList();
            ItemObj.ListStatus = lstStatus;


            var requests = _context.Request.Include(a => a.AssetDetail).Include(a => a.AssetDetail.Hospital)
                .Include(a => a.AssetDetail.Hospital.Governorate)
                  .Include(a => a.AssetDetail.Hospital.City)
                    .Include(a => a.AssetDetail.Hospital.Organization)
                      .Include(a => a.AssetDetail.Hospital.SubOrganization)
                .Include(a => a.User).Where(a => a.HospitalId == hospitalId).OrderByDescending(a => a.RequestDate.Date).ToList();

            if (UserObj.GovernorateId == 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
            {
                requests = requests.ToList();
            }
            if (UserObj.GovernorateId > 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
            {
                requests = requests.Where(t => t.AssetDetail.Hospital.GovernorateId == UserObj.GovernorateId).ToList();
            }
            if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId == 0)
            {
                requests = requests.Where(t => t.AssetDetail.Hospital.CityId == UserObj.CityId).ToList();
            }
            if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId == 0 && UserObj.HospitalId == 0)
            {
                requests = requests.Where(t => t.AssetDetail.Hospital.OrganizationId == UserObj.OrganizationId).ToList();
            }
            if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId == 0)
            {
                requests = requests.Where(t => t.AssetDetail.Hospital.SubOrganizationId == UserObj.SubOrganizationId).ToList();
            }


            if (UserObj.HospitalId > 0)
            {
                if (lstRoleNames.Contains("Admin"))
                {
                    requests = requests.ToList();
                }
                if (lstRoleNames.Contains("TLHospitalManager"))
                {
                    requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                }
                if (lstRoleNames.Contains("EngDepManager") && lstRoleNames.Contains("Eng"))
                {
                    requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                }
                if (!lstRoleNames.Contains("EngDepManager") && lstRoleNames.Contains("Eng"))
                {
                    requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId && t.CreatedById == userId).ToList();
                }
                if (!lstRoleNames.Contains("EngDepManager") && !lstRoleNames.Contains("Eng"))
                {
                    requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                }
                if (lstRoleNames.Contains("AssetOwner") && !lstRoleNames.Contains("Eng") && !lstRoleNames.Contains("EngDepManager") && !lstRoleNames.Contains("SRReviewer"))
                {
                    requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId && t.CreatedById == userId).ToList();
                }
                if (lstRoleNames.Contains("SRCreator") && !lstRoleNames.Contains("SRReviewer"))
                {
                    requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId && t.CreatedById == userId).ToList();
                }
                if (lstRoleNames.Contains("AssetOwner") && lstRoleNames.Contains("SRReviewer"))
                {
                    requests = requests.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
                }
                if (lstRoleNames.Contains("SRReviewer") && lstRoleNames.Contains("SRCreator") && lstRoleNames.Contains("EngDepManager"))
                {
                    requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                }
                if (lstRoleNames.Contains("SRReviewer") && lstRoleNames.Contains("SRCreator") && !lstRoleNames.Contains("EngDepManager"))
                {
                    requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                }
                if (lstRoleNames.Contains("SRReviewer") && !lstRoleNames.Contains("SRCreator") && !lstRoleNames.Contains("EngDepManager"))
                {
                    requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                }


                if (lstRoleNames.Contains("EngDepManager") && !lstRoleNames.Contains("SRReviewer") && !lstRoleNames.Contains("SRCreator"))
                {
                    requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                }
                if (lstRoleNames.Contains("EngDepManager") && lstRoleNames.Contains("SRReviewer") && !lstRoleNames.Contains("SRCreator"))
                {
                    requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                }
                if (lstRoleNames.Contains("EngDepManager") && lstRoleNames.Contains("SRReviewer") && lstRoleNames.Contains("SRCreator"))
                {
                    requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                }
            }

            if (requests.Count > 0)
            {
                foreach (var req in requests)
                {
                    // var trackObj = _context.RequestTracking.OrderByDescending(a => a.DescriptionDate.Value.Date).FirstOrDefault(a => a.RequestId == req.Id);
                    var trackObj = _context.RequestTracking.OrderByDescending(a => a.DescriptionDate).Where(a => a.RequestId == req.Id).FirstOrDefault();


                    if (trackObj != null)
                    {
                        RequestTracking trk = trackObj;

                        if (trk.RequestStatusId == 1)
                        {
                            lstOpenTracks.Add(trk);
                        }
                        if (trk.RequestStatusId == 2)
                        {
                            lstCloseTracks.Add(trk);
                        }
                        if (trk.RequestStatusId == 3)
                        {
                            lstInProgressTracks.Add(trk);
                        }
                        if (trk.RequestStatusId == 4)
                        {
                            lstSolvedTracks.Add(trk);
                        }
                        if (trk.RequestStatusId == 5)
                        {
                            lstApprovedTracks.Add(trk);
                        }
                    }

                }
            }

            ItemObj.CountOpen = lstOpenTracks.Count;
            ItemObj.CountClosed = lstCloseTracks.Count;
            ItemObj.CountInProgress = lstInProgressTracks.Count;
            ItemObj.CountSolved = lstSolvedTracks.Count;
            ItemObj.CountApproved = lstApprovedTracks.Count;
            ItemObj.CountAll = requests.Count;

            return ItemObj;
        }


        public IndexRequestStatusVM.GetData GetAllForReport()
        {
            ApplicationUser UserObj = new ApplicationUser();
            ApplicationRole roleObj = new ApplicationRole();
            List<string> lstRoleNames = new List<string>();

            IndexRequestStatusVM.GetData ItemObj = new IndexRequestStatusVM.GetData();
            List<RequestTracking> lstOpenTracks = new List<RequestTracking>();
            List<RequestTracking> lstCloseTracks = new List<RequestTracking>();
            List<RequestTracking> lstInProgressTracks = new List<RequestTracking>();
            List<RequestTracking> lstSolvedTracks = new List<RequestTracking>();
            List<RequestTracking> lstApprovedTracks = new List<RequestTracking>();



            var lstStatus = _context.RequestStatus.Where(a => a.Id != 5).ToList();
            ItemObj.ListStatus = lstStatus;


            var requests = _context.Request.Include(a => a.AssetDetail).Include(a => a.AssetDetail.Hospital)
                .Include(a => a.AssetDetail.Hospital.Governorate)
                  .Include(a => a.AssetDetail.Hospital.City)
                    .Include(a => a.AssetDetail.Hospital.Organization)
                      .Include(a => a.AssetDetail.Hospital.SubOrganization)
                .Include(a => a.User).OrderByDescending(a => a.RequestDate.Date).ToList();


            if (UserObj.GovernorateId == 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
            {
                requests = requests.ToList();
            }
            if (UserObj.GovernorateId > 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
            {
                requests = requests.Where(t => t.AssetDetail.Hospital.GovernorateId == UserObj.GovernorateId).ToList();
            }
            if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId == 0)
            {
                requests = requests.Where(t => t.AssetDetail.Hospital.CityId == UserObj.CityId).ToList();
            }
            if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId == 0 && UserObj.HospitalId == 0)
            {
                requests = requests.Where(t => t.AssetDetail.Hospital.OrganizationId == UserObj.OrganizationId).ToList();
            }
            if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId == 0)
            {
                requests = requests.Where(t => t.AssetDetail.Hospital.SubOrganizationId == UserObj.SubOrganizationId).ToList();
            }
            //if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId > 0)
            //{
            //    if (lstRoleNames.Contains("Admin"))
            //    {
            //        requests = requests.ToList();
            //    }
            //    if (lstRoleNames.Contains("TLHospitalManager"))
            //    {
            //        requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
            //    }
            //    if (lstRoleNames.Contains("EngDepManager") && lstRoleNames.Contains("Eng"))
            //    {
            //        requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
            //    }
            //    if (!lstRoleNames.Contains("EngDepManager") && lstRoleNames.Contains("Eng"))
            //    {
            //        requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
            //    }

            //    if (!lstRoleNames.Contains("EngDepManager") && !lstRoleNames.Contains("Eng"))
            //    {
            //        requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
            //    }
            //    if (lstRoleNames.Contains("AssetOwner"))
            //    {
            //        requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
            //    }
            //    if (lstRoleNames.Contains("EngDepManager"))
            //    {
            //        requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
            //    }
            //    if (lstRoleNames.Contains("SRCreator"))
            //    {
            //        requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
            //    }
            //    if (lstRoleNames.Contains("SRReviewer"))
            //    {

            //        List<Request> lstRequests = new List<Request>();
            //        var lstTracks = _context.RequestTracking.Where(a => a.Request.HospitalId == UserObj.HospitalId).OrderByDescending(a => a.DescriptionDate).ToList().GroupBy(a => a.RequestId).ToList();
            //        foreach (var item2 in lstTracks)
            //        {
            //            if (item2.FirstOrDefault().RequestStatusId == 4)
            //            {
            //                lstRequests.Add(_context.Request.Where(a => a.Id == item2.FirstOrDefault().RequestId).FirstOrDefault());
            //            }
            //        }
            //        requests = lstRequests;
            //    }
            //}
            //if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId > 0)
            //{
            //    if (lstRoleNames.Contains("Admin"))
            //    {
            //        requests = requests.ToList();
            //    }
            //    if (lstRoleNames.Contains("TLHospitalManager"))
            //    {
            //        requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
            //    }
            //    if (lstRoleNames.Contains("EngDepManager") && lstRoleNames.Contains("Eng"))
            //    {
            //        requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
            //    }
            //    if (!lstRoleNames.Contains("EngDepManager") && lstRoleNames.Contains("Eng"))
            //    {
            //        requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
            //    }

            //    if (!lstRoleNames.Contains("EngDepManager") && !lstRoleNames.Contains("Eng"))
            //    {
            //        requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
            //    }
            //    if (lstRoleNames.Contains("AssetOwner"))
            //    {
            //        requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
            //    }

            //    if (lstRoleNames.Contains("EngDepManager"))
            //    {
            //        requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
            //    }
            //    if (lstRoleNames.Contains("SRCreator"))
            //    {
            //        requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
            //    }
            //    if (lstRoleNames.Contains("SRReviewer"))
            //    {

            //        List<Request> lstRequests = new List<Request>();
            //        var lstTracks = _context.RequestTracking.Where(a => a.Request.HospitalId == UserObj.HospitalId).OrderByDescending(a => a.DescriptionDate).ToList().GroupBy(a => a.RequestId).ToList();
            //        foreach (var item2 in lstTracks)
            //        {
            //            if (item2.FirstOrDefault().RequestStatusId == 4)
            //            {
            //                lstRequests.Add(_context.Request.Where(a => a.Id == item2.FirstOrDefault().RequestId).FirstOrDefault());
            //            }
            //        }
            //        requests = lstRequests;
            //    }
            //}


            if (UserObj.HospitalId > 0)
            {
                if (lstRoleNames.Contains("Admin"))
                {
                    requests = requests.ToList();
                }
                else if ((lstRoleNames.Contains("AssetOwner") || lstRoleNames.Contains("SRCreator")) && !lstRoleNames.Contains("TLHospitalManager") && !lstRoleNames.Contains("EngDepManager") && !lstRoleNames.Contains("SRReviewer"))
                {
                    requests = requests.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == UserObj.Id).ToList();
                }
                else if ((lstRoleNames.Contains("AssetOwner") && lstRoleNames.Contains("SRCreator")) && !lstRoleNames.Contains("TLHospitalManager") && !lstRoleNames.Contains("EngDepManager") && !lstRoleNames.Contains("SRReviewer"))
                {
                    requests = requests.Where(t => t.HospitalId == UserObj.HospitalId && t.CreatedById == UserObj.Id).ToList();
                }
                else if (lstRoleNames.Contains("TLHospitalManager") && lstRoleNames.Contains("EngDepManager") && lstRoleNames.Contains("SRReviewer") && lstRoleNames.Contains("SRCreator") && lstRoleNames.Contains("AssetOwner") && lstRoleNames.Contains("SRCreator"))
                {
                    requests = requests.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                else if (lstRoleNames.Contains("TLHospitalManager") && !lstRoleNames.Contains("EngDepManager") && !lstRoleNames.Contains("SRReviewer") && !lstRoleNames.Contains("SRCreator") && !lstRoleNames.Contains("AssetOwner") && !lstRoleNames.Contains("SRCreator"))
                {
                    requests = requests.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                else if (lstRoleNames.Contains("TLHospitalManager") && lstRoleNames.Contains("EngDepManager") && !lstRoleNames.Contains("SRReviewer") && !lstRoleNames.Contains("SRCreator") && !lstRoleNames.Contains("AssetOwner") && !lstRoleNames.Contains("SRCreator"))
                {
                    requests = requests.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                else if (lstRoleNames.Contains("TLHospitalManager") && lstRoleNames.Contains("EngDepManager") && lstRoleNames.Contains("SRReviewer") && !lstRoleNames.Contains("SRCreator") && !lstRoleNames.Contains("AssetOwner") && !lstRoleNames.Contains("SRCreator"))
                {
                    requests = requests.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                else if (lstRoleNames.Contains("TLHospitalManager") && lstRoleNames.Contains("EngDepManager") && lstRoleNames.Contains("SRReviewer") && lstRoleNames.Contains("SRCreator") && !lstRoleNames.Contains("AssetOwner") && !lstRoleNames.Contains("SRCreator"))
                {
                    requests = requests.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
                else if (lstRoleNames.Contains("TLHospitalManager") && lstRoleNames.Contains("EngDepManager") && lstRoleNames.Contains("SRReviewer") && lstRoleNames.Contains("SRCreator") && lstRoleNames.Contains("AssetOwner") && !lstRoleNames.Contains("SRCreator"))
                {
                    requests = requests.Where(t => t.HospitalId == UserObj.HospitalId).ToList();
                }
            }





            if (requests.Count > 0)
            {
                foreach (var req in requests)
                {
                    var trackObj = _context.RequestTracking.OrderByDescending(a => a.DescriptionDate).Where(a => a.RequestId == req.Id).FirstOrDefault();


                    if (trackObj != null)
                    {
                        RequestTracking trk = trackObj;

                        if (trk.RequestStatusId == 1)
                        {
                            lstOpenTracks.Add(trk);
                        }
                        if (trk.RequestStatusId == 2)
                        {
                            lstCloseTracks.Add(trk);
                        }
                        if (trk.RequestStatusId == 3)
                        {
                            lstInProgressTracks.Add(trk);
                        }
                        if (trk.RequestStatusId == 4)
                        {
                            lstSolvedTracks.Add(trk);
                        }
                        if (trk.RequestStatusId == 5)
                        {
                            lstApprovedTracks.Add(trk);
                        }
                    }

                }
            }

            ItemObj.CountOpen = lstOpenTracks.Count;
            ItemObj.CountClosed = lstCloseTracks.Count;
            ItemObj.CountInProgress = lstInProgressTracks.Count;
            ItemObj.CountSolved = lstSolvedTracks.Count;
            ItemObj.CountApproved = lstApprovedTracks.Count;
            ItemObj.CountAll = requests.Count;
            return ItemObj;
        }





        public IndexRequestStatusVM.GetData GetAllForReport(SearchRequestDateVM requestDateObj)
        {
            ApplicationUser UserObj = new ApplicationUser();
            ApplicationRole roleObj = new ApplicationRole();
            List<string> lstRoleNames = new List<string>();

            IndexRequestStatusVM.GetData ItemObj = new IndexRequestStatusVM.GetData();
            List<RequestTracking> lstOpenTracks = new List<RequestTracking>();
            List<RequestTracking> lstCloseTracks = new List<RequestTracking>();
            List<RequestTracking> lstInProgressTracks = new List<RequestTracking>();
            List<RequestTracking> lstSolvedTracks = new List<RequestTracking>();
            List<RequestTracking> lstApprovedTracks = new List<RequestTracking>();


            //var obj = _context.ApplicationUser.Where(a => a.Id == requestDateObj.UserId).ToList();
            //if (obj.Count > 0)
            //{
            //    UserObj = obj[0];
            //    //var roleNames = (from userRole in _context.UserRoles
            //    //                 join role in _context.Roles on userRole.RoleId equals role.Id
            //    //                 where userRole.UserId == requestDateObj.UserId
            //    //                 select role);
            //    //foreach (var name in roleNames)
            //    //{
            //    //    lstRoleNames.Add(name.Name);
            //    //}
            //}

            var lstStatus = _context.RequestStatus.Where(a => a.Id != 5).ToList();
            ItemObj.ListStatus = lstStatus;


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



            var requests = _context.Request.Include(a => a.AssetDetail).Include(a => a.AssetDetail.Hospital)
                .Include(a => a.AssetDetail.Hospital.Governorate)
                  .Include(a => a.AssetDetail.Hospital.City)
                    .Include(a => a.AssetDetail.Hospital.Organization)
                      .Include(a => a.AssetDetail.Hospital.SubOrganization)
                .Include(a => a.User).OrderByDescending(a => a.RequestDate.Date).ToList();


            if (UserObj.GovernorateId == 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
            {
                requests = requests.ToList();
            }
            if (UserObj.GovernorateId > 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
            {
                requests = requests.Where(t => t.AssetDetail.Hospital.GovernorateId == UserObj.GovernorateId).ToList();
            }
            if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId == 0)
            {
                requests = requests.Where(t => t.AssetDetail.Hospital.CityId == UserObj.CityId).ToList();
            }
            if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId == 0 && UserObj.HospitalId == 0)
            {
                requests = requests.Where(t => t.AssetDetail.Hospital.OrganizationId == UserObj.OrganizationId).ToList();
            }
            if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId == 0)
            {
                requests = requests.Where(t => t.AssetDetail.Hospital.SubOrganizationId == UserObj.SubOrganizationId).ToList();
            }
            if (UserObj.HospitalId > 0)
            {
                if (lstRoleNames.Contains("Admin"))
                {
                    requests = requests.ToList();
                }
                else
                {
                    requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                }

            }


            if (start != null || end != null)
            {
                requests = requests.Where(a => a.RequestDate.Date >= start.Value.Date && a.RequestDate.Date <= end.Value.Date).ToList();
            }
            else
            {
                requests = requests.ToList();
            }

            if (requests.Count > 0)
            {
                foreach (var req in requests)
                {
                    var trackObj = _context.RequestTracking.OrderByDescending(a => a.DescriptionDate).Where(a => a.RequestId == req.Id).FirstOrDefault();

                    if (trackObj != null)
                    {
                        RequestTracking trk = trackObj;

                        if (trk.RequestStatusId == 1)
                        {
                            lstOpenTracks.Add(trk);
                        }
                        if (trk.RequestStatusId == 2)
                        {
                            lstCloseTracks.Add(trk);
                        }
                        if (trk.RequestStatusId == 3)
                        {
                            lstInProgressTracks.Add(trk);
                        }
                        if (trk.RequestStatusId == 4)
                        {
                            lstSolvedTracks.Add(trk);
                        }
                        if (trk.RequestStatusId == 5)
                        {
                            lstApprovedTracks.Add(trk);
                        }
                    }
                }
            }

            ItemObj.CountOpen = lstOpenTracks.Count;
            ItemObj.CountClosed = lstCloseTracks.Count;
            ItemObj.CountInProgress = lstInProgressTracks.Count;
            ItemObj.CountSolved = lstSolvedTracks.Count;
            ItemObj.CountApproved = lstApprovedTracks.Count;
            ItemObj.CountAll = requests.Count;
            return ItemObj;
        }


        public IEnumerable<IndexRequestStatusVM.GetData> GetAll()
        {
            return _context.RequestStatus.Select(sts => new IndexRequestStatusVM.GetData
            {
                Id = sts.Id,
                Name = sts.Name,
                NameAr = sts.NameAr,
                Color = sts.Color,
                Icon = sts.Icon
            }).ToList();
        }

        public RequestStatus GetById(int id)
        {
            return _context.RequestStatus.Find(id);
        }

        public IEnumerable<IndexRequestStatusVM.GetData> SortRequestStatuses(SortRequestStatusVM sortObj)
        {
            var lstBrands = GetAll().ToList();

            if (sortObj.Name != "")
            {
                if (sortObj.SortStatus == "descending")
                    lstBrands = lstBrands.OrderByDescending(d => d.Name).ToList();
                else
                    lstBrands = lstBrands.OrderBy(d => d.Name).ToList();
            }

            else if (sortObj.NameAr != "")
            {
                if (sortObj.SortStatus == "descending")
                    lstBrands = lstBrands.OrderByDescending(d => d.NameAr).ToList();
                else
                    lstBrands = lstBrands.OrderBy(d => d.NameAr).ToList();
            }

            return lstBrands;
        }

        public int Update(RequestStatus editRequestVM)
        {
            try
            {
                var reqStatusObj = _context.RequestStatus.Find(editRequestVM.Id);
                reqStatusObj.Color = editRequestVM.Color;
                reqStatusObj.Icon = editRequestVM.Icon;
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




        public IndexRequestStatusVM GetRequestStatusByUserId(string userId, int hospitalId)
        {
            IndexRequestStatusVM mainClass = new IndexRequestStatusVM();
            List<IndexRequestStatusVM.GetData> list = new List<IndexRequestStatusVM.GetData>();
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
            }

            List<RequestTracking> lstOpenTracks = new List<RequestTracking>();
            List<RequestTracking> lstCloseTracks = new List<RequestTracking>();
            List<RequestTracking> lstInProgressTracks = new List<RequestTracking>();
            List<RequestTracking> lstSolvedTracks = new List<RequestTracking>();
            List<RequestTracking> lstApprovedTracks = new List<RequestTracking>();


            var lstStatus = _context.RequestStatus.Where(a => a.Id != 5).ToList();
            mainClass.ListStatus = lstStatus;

            var requests = _context.RequestTracking.Include(a => a.Request).Include(a => a.Request.AssetDetail).Include(a => a.Request.AssetDetail.Hospital)
                             //    .Include(a => a.Request.AssetDetail.Hospital.Governorate).Include(a => a.Request.AssetDetail.Hospital.City)
                              //   .Include(a => a.Request.AssetDetail.Hospital.Organization).Include(a => a.Request.AssetDetail.Hospital.SubOrganization)
                                 .Include(a => a.Request.User)
                                 .OrderByDescending(a => a.DescriptionDate)
                                 .ToList().GroupBy(rt => rt.RequestId).ToList();



            if (UserObj.GovernorateId > 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
            {
                requests = requests.Where(t => t.FirstOrDefault().Request.AssetDetail.Hospital.GovernorateId == UserObj.GovernorateId).ToList();
            }
            if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId == 0)
            {
                requests = requests.Where(t => t.FirstOrDefault().Request.AssetDetail.Hospital.CityId == UserObj.CityId).ToList();
            }
            if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId == 0 && UserObj.HospitalId == 0)
            {
                requests = requests.Where(t => t.FirstOrDefault().Request.AssetDetail.Hospital.OrganizationId == UserObj.OrganizationId).ToList();
            }
            if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId == 0)
            {
                requests = requests.Where(t => t.FirstOrDefault().Request.AssetDetail.Hospital.SubOrganizationId == UserObj.SubOrganizationId).ToList();
            }
            if (UserObj.HospitalId > 0)
            {
                //if (lstRoleNames.Contains("TLHospitalManager"))
                //{
                //    requests = requests.Where(t => t.FirstOrDefault().Request.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
                //}
                //if (lstRoleNames.Contains("EngDepManager") && lstRoleNames.Contains("Eng"))
                //{
                //    requests = requests.Where(t => t.FirstOrDefault().Request.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
                //}
                //if (!lstRoleNames.Contains("EngDepManager") && lstRoleNames.Contains("Eng"))
                //{
                //    requests = requests.Where(t => t.FirstOrDefault().Request.AssetDetail.HospitalId == UserObj.HospitalId && t.FirstOrDefault().CreatedById == userId).ToList();
                //}
                //if (!lstRoleNames.Contains("EngDepManager") && !lstRoleNames.Contains("Eng"))
                //{
                //    requests = requests.Where(t => t.FirstOrDefault().Request.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
                //}
                //if (lstRoleNames.Contains("AssetOwner"))
                //{
                //    requests = requests.Where(t => t.FirstOrDefault().Request.AssetDetail.HospitalId == UserObj.HospitalId && t.FirstOrDefault().CreatedById == userId).ToList();
                //}
                //if (lstRoleNames.Contains("SRCreator") && !lstRoleNames.Contains("SRReviewer"))
                //{
                //    requests = requests.Where(t => t.FirstOrDefault().Request.AssetDetail.HospitalId == UserObj.HospitalId && t.FirstOrDefault().CreatedById == userId).ToList();
                //}

                //if (lstRoleNames.Contains("SRCreator") && lstRoleNames.Contains("SRReviewer"))
                //{
                //    requests = requests.Where(t => t.FirstOrDefault().Request.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                //}

                //if (lstRoleNames.Contains("AssetOwner") && !lstRoleNames.Contains("SRReviewer"))
                //{
                //    requests = requests.Where(t => t.FirstOrDefault().Request.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
                //}
                //if (lstRoleNames.Contains("EngDepManager"))
                //{
                //    requests = requests.Where(t => t.FirstOrDefault().Request.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
                //}

                //if (lstRoleNames.Contains("SRReviewer") && !lstRoleNames.Contains("SRCreator"))
                //{
                //    requests = requests.Where(t => t.FirstOrDefault().Request.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
                //}

                if (lstRoleNames.Any() && ((lstRoleNames.Contains("AssetOwner") || lstRoleNames.Contains("SRCreator") || (lstRoleNames.Contains("AssetOwner") && lstRoleNames.Contains("SRCreator"))) && !lstRoleNames.Any(itm => itm == "TLHospitalManager" || itm == "EngDepManager" || itm == "SRReviewer")))
                {
                    requests = requests.Where(a => a.FirstOrDefault().CreatedById == UserObj.Id && a.FirstOrDefault().HospitalId == UserObj.HospitalId).ToList();
                }
                if (lstRoleNames.Contains("TLHospitalManager") && lstRoleNames.Contains("EngDepManager") && lstRoleNames.Contains("SRReviewer") && lstRoleNames.Contains("SRCreator") && lstRoleNames.Contains("AssetOwner") && lstRoleNames.Contains("SRCreator"))
                {
                    requests = requests.Where(a => a.FirstOrDefault().HospitalId == UserObj.HospitalId).ToList();
                }
                if (lstRoleNames.Contains("TLHospitalManager") && !lstRoleNames.Contains("EngDepManager") && !lstRoleNames.Contains("SRReviewer") && !lstRoleNames.Contains("SRCreator") && !lstRoleNames.Contains("AssetOwner") && !lstRoleNames.Contains("SRCreator"))
                {
                    requests = requests.Where(a => a.FirstOrDefault().HospitalId == UserObj.HospitalId).ToList();
                }
                if (lstRoleNames.Contains("TLHospitalManager") && lstRoleNames.Contains("EngDepManager") && !lstRoleNames.Contains("SRReviewer") && !lstRoleNames.Contains("SRCreator") && !lstRoleNames.Contains("AssetOwner") && !lstRoleNames.Contains("SRCreator"))
                {
                    requests = requests.Where(a => a.FirstOrDefault().HospitalId == UserObj.HospitalId).ToList();
                }
                if (lstRoleNames.Contains("TLHospitalManager") && lstRoleNames.Contains("EngDepManager") && lstRoleNames.Contains("SRReviewer") && !lstRoleNames.Contains("SRCreator") && !lstRoleNames.Contains("AssetOwner") && !lstRoleNames.Contains("SRCreator"))
                {
                    requests = requests.Where(a => a.FirstOrDefault().HospitalId == UserObj.HospitalId).ToList();
                }
                if (lstRoleNames.Contains("TLHospitalManager") && lstRoleNames.Contains("EngDepManager") && lstRoleNames.Contains("SRReviewer") && lstRoleNames.Contains("SRCreator") && !lstRoleNames.Contains("AssetOwner") && !lstRoleNames.Contains("SRCreator"))
                {
                    requests = requests.Where(a => a.FirstOrDefault().HospitalId == UserObj.HospitalId).ToList();
                }
                if (lstRoleNames.Contains("TLHospitalManager") && lstRoleNames.Contains("EngDepManager") && lstRoleNames.Contains("SRReviewer") && lstRoleNames.Contains("SRCreator") && lstRoleNames.Contains("AssetOwner") && !lstRoleNames.Contains("SRCreator"))
                {
                    requests = requests.Where(a => a.FirstOrDefault().HospitalId == UserObj.HospitalId).ToList();
                }
            }
            if (hospitalId != 0)
            {
                requests = requests.Where(t => t.FirstOrDefault().Request.AssetDetail.HospitalId == hospitalId).ToList();
            }


            if (requests.ToList().Count > 0)
            {
                foreach (var req in requests)
                {
                    switch (req.FirstOrDefault().RequestStatusId)
                    {
                        case 1:
                            lstOpenTracks.Add(req.FirstOrDefault());
                            break;
                        case 2:
                            lstCloseTracks.Add(req.FirstOrDefault());
                            break;
                        case 3:
                            lstInProgressTracks.Add(req.FirstOrDefault());
                            break;
                        case 4:
                            lstSolvedTracks.Add(req.FirstOrDefault());
                            break;
                        case 5:
                            lstApprovedTracks.Add(req.FirstOrDefault());
                            break;
                    }

                }
            }

            mainClass.CountOpen = lstOpenTracks.Count;
            mainClass.CountClosed = lstCloseTracks.Count;
            mainClass.CountInProgress = lstInProgressTracks.Count;
            mainClass.CountSolved = lstSolvedTracks.Count;
            mainClass.CountApproved = lstApprovedTracks.Count;
            mainClass.CountAll = requests.Count();

            return mainClass;
        }

    }
}
