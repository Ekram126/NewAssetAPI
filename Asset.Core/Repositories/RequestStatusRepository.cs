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

        public IEnumerable<IndexRequestStatusVM.GetData> GetAll(string userId)
        {
            ApplicationUser UserObj = new ApplicationUser();
            ApplicationRole roleObj = new ApplicationRole();
            //  string userRoleName = "";
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
       
            List<IndexRequestStatusVM.GetData> listCountRequests = new List<IndexRequestStatusVM.GetData>();
            List<RequestTracking> lstOpenTracks = new List<RequestTracking>();
            List<RequestTracking> lstCloseTracks = new List<RequestTracking>();
            List<RequestTracking> lstInProgressTracks = new List<RequestTracking>();
            List<RequestTracking> lstSolvedTracks = new List<RequestTracking>();
            List<RequestTracking> lstApprovedTracks = new List<RequestTracking>();
            IndexRequestStatusVM.GetData getDataObj = new IndexRequestStatusVM.GetData();



            var lstStatus = _context.RequestStatus.ToList();
            getDataObj.ListStatus = lstStatus;
            var requests = _context.Request.Include(a => a.AssetDetail).Include(a => a.AssetDetail.Hospital).Include(a => a.User).ToList();

            getDataObj.GovernorateId = requests[0].User.GovernorateId;
            getDataObj.CityId = requests[0].User.CityId;
            getDataObj.OrganizationId = requests[0].User.OrganizationId;
            getDataObj.SubOrganizationId = requests[0].User.SubOrganizationId;
            getDataObj.HospitalId = requests[0].User.HospitalId;


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

            if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId > 0)
            {
                if (lstRoleNames.Contains("Admin"))
                {
                    requests = requests.ToList();
                }
                if (lstRoleNames.Contains("TLHospitalManager"))
                {
                    requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                }
                if (lstRoleNames.Contains("EngDepManager"))
                {
                    requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                }
                if (lstRoleNames.Contains("EngManager"))
                {
                    requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                }
                if (lstRoleNames.Contains("Eng"))
                {
                    requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId && t.CreatedById == userId).ToList();
                }
                if (lstRoleNames.Contains("AssetOwner"))
                {
                    requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId && t.CreatedById == userId).ToList();
                }
            }
            if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId > 0)
            {
                if (lstRoleNames.Contains("Admin"))
                {
                    requests = requests.ToList();
                }
                if (lstRoleNames.Contains("TLHospitalManager"))
                {
                    requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                }
                if (lstRoleNames.Contains("EngDepManager"))
                {
                    requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                }
                if (lstRoleNames.Contains("EngManager"))
                {
                    requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                }
                if (lstRoleNames.Contains("AssetOwner"))
                {
                    requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId && t.CreatedById == userId).ToList();
                }
                if (lstRoleNames.Contains("Eng"))
                {
                    requests = requests.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId && t.CreatedById == userId).ToList();
                }

            }




            if (requests.Count > 0)
            {

                foreach (var req in requests)
                {
                    var trackObj = _context.RequestTracking.OrderByDescending(a => a.Id).FirstOrDefault(a => a.RequestId == req.Id);
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

            getDataObj.CountOpen = lstOpenTracks.Count;
            getDataObj.CountClosed = lstCloseTracks.Count;
            getDataObj.CountInProgress = lstInProgressTracks.Count;
            getDataObj.CountSolved = lstSolvedTracks.Count;
            getDataObj.CountApproved = lstApprovedTracks.Count;
            listCountRequests.Add(getDataObj);



            return listCountRequests;
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
            //if (sortObj.Code != "")
            //{
            //    if (sortObj.SortStatus == "descending")
            //        lstBrands = lstBrands.OrderByDescending(d => d.Code).ToList();
            //    else
            //        lstBrands = lstBrands.OrderBy(d => d.Code).ToList();
            //}
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


    }
}
