using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.AssetStatusVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class AssetStatusRepositories : IAssetStatusRepository
    {

        private ApplicationDbContext _context;


        public AssetStatusRepositories(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Add(CreateAssetStatusVM model)
        {
            AssetStatu AssetStatusObj = new AssetStatu();
            try
            {
                if (model != null)
                {
                    AssetStatusObj.Code = model.Code;
                    AssetStatusObj.Name = model.Name;
                    AssetStatusObj.NameAr = model.NameAr;
                    _context.AssetStatus.Add(AssetStatusObj);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return AssetStatusObj.Id;
        }

        public int Delete(int id)
        {
            var AssetStatusObj = _context.AssetStatus.Find(id);
            try
            {
                if (AssetStatusObj != null)
                {
                    _context.AssetStatus.Remove(AssetStatusObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }

        public IEnumerable<IndexAssetStatusVM.GetData> GetAll()
        {
            return _context.AssetStatus.ToList().Select(item => new IndexAssetStatusVM.GetData
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                NameAr = item.NameAr
            });
        }

        public IEnumerable<AssetStatu> GetAllAssetStatus()
        {
            return _context.AssetStatus.ToList();
        }

        public EditAssetStatusVM GetById(int id)
        {
            return _context.AssetStatus.Where(a => a.Id == id).Select(item => new EditAssetStatusVM
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                NameAr = item.NameAr
            }).First();
        }

        public IEnumerable<IndexAssetStatusVM.GetData> GetAssetStatusByName(string AssetStatusName)
        {
            return _context.AssetStatus.Where(a => a.Name == AssetStatusName || a.NameAr == AssetStatusName).ToList().Select(item => new IndexAssetStatusVM.GetData
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                NameAr = item.NameAr
            });
        }





        public int Update(EditAssetStatusVM model)
        {
            try
            {
                var AssetStatusObj = _context.AssetStatus.Find(model.Id);
                AssetStatusObj.Code = model.Code;
                AssetStatusObj.Name = model.Name;
                AssetStatusObj.NameAr = model.NameAr;
                _context.Entry(AssetStatusObj).State = EntityState.Modified;
                _context.SaveChanges();
                return AssetStatusObj.Id;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }

        public IEnumerable<IndexAssetStatusVM.GetData> SortAssetStatuses(SortAssetStatusVM sortObj)
        {
            var lstAssetStatuses = GetAll().ToList();
            if (sortObj.Code != "")
            {
                if (sortObj.SortStatus == "descending")
                    lstAssetStatuses = lstAssetStatuses.OrderByDescending(d => d.Code).ToList();
                else
                    lstAssetStatuses = lstAssetStatuses.OrderBy(d => d.Code).ToList();
            }
            else if (sortObj.Name != "")
            {
                if (sortObj.SortStatus == "descending")
                    lstAssetStatuses = lstAssetStatuses.OrderByDescending(d => d.Name).ToList();
                else
                    lstAssetStatuses = lstAssetStatuses.OrderBy(d => d.Name).ToList();
            }

            else if (sortObj.NameAr != "")
            {
                if (sortObj.SortStatus == "descending")
                    lstAssetStatuses = lstAssetStatuses.OrderByDescending(d => d.NameAr).ToList();
                else
                    lstAssetStatuses = lstAssetStatuses.OrderBy(d => d.NameAr).ToList();
            }

            return lstAssetStatuses;
        }

        //public IEnumerable<IndexAssetStatusVM.GetData> GetAllAssetsGroupByStatusId(int statusId, string userId)
        //{
        //    List<IndexAssetStatusVM.GetData> list = new List<IndexAssetStatusVM.GetData>();
        //    List<AssetStatusTransaction> lstNeedRepair = new List<AssetStatusTransaction>();
        //    List<AssetStatusTransaction> lstInActive = new List<AssetStatusTransaction>();
        //    List<AssetStatusTransaction> lstWorking = new List<AssetStatusTransaction>();
        //    List<AssetStatusTransaction> lstUnderMaintenance = new List<AssetStatusTransaction>();
        //    List<AssetStatusTransaction> lstUnderInstallation = new List<AssetStatusTransaction>();
        //    List<AssetStatusTransaction> lstNotWorking = new List<AssetStatusTransaction>();
        //    List<AssetStatusTransaction> lstShutdown = new List<AssetStatusTransaction>();
        //    List<AssetStatusTransaction> lstExecluded = new List<AssetStatusTransaction>();
        //    List<AssetStatusTransaction> lstHold = new List<AssetStatusTransaction>();
        //    IndexAssetStatusVM.GetData getDataObj = new IndexAssetStatusVM.GetData();

        //    ApplicationUser UserObj = new ApplicationUser();
        //    List<string> lstRoleNames = new List<string>();
        //    var obj = _context.ApplicationUser.Where(a => a.Id == userId).ToList();
        //    if (obj.Count > 0)
        //    {
        //        UserObj = obj[0];

        //        var roleNames = (from userRole in _context.UserRoles
        //                         join role in _context.Roles on userRole.RoleId equals role.Id
        //                         where userRole.UserId == userId
        //                         select role);
        //        foreach (var item in roleNames)
        //        {
        //            lstRoleNames.Add(item.Name);
        //        }
        //    }
        //    var lstStatus = _context.AssetStatus.Where(a=>a.Id != 1 && a.Id != 5 && a.Id != 6 && a.Id != 7).ToList();
        //    getDataObj.ListStatus = lstStatus;


        //    var lstTransactions = _context.AssetStatusTransactions
        //                    .Include(a => a.AssetDetail)
        //                    .Include(a => a.AssetDetail.Hospital)
        //                    .Include(a => a.AssetDetail.MasterAsset).OrderByDescending(a => a.StatusDate.Value.Date).ToList();



        //    if (statusId != 0)
        //    {
        //        lstTransactions = lstTransactions.Where(a => a.AssetStatusId == statusId).ToList();
        //    }
        //    if (lstTransactions.Count > 0)
        //    {
        //        var transObj = lstTransactions[0];
        //        getDataObj.HospitalId = transObj.AssetDetail.HospitalId;
        //        getDataObj.GovernorateId = transObj.AssetDetail.Hospital.GovernorateId;
        //        getDataObj.CityId = transObj.AssetDetail.Hospital.CityId;
        //        getDataObj.OrganizationId = transObj.AssetDetail.Hospital.OrganizationId;
        //        getDataObj.SubOrganizationId = transObj.AssetDetail.Hospital.SubOrganizationId;

        //    }


        //    if (UserObj.GovernorateId == 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
        //    {
        //        lstTransactions = lstTransactions.ToList();
        //    }
        //    if (UserObj.GovernorateId > 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
        //    {
        //        lstTransactions = lstTransactions.Where(t => t.AssetDetail.Hospital.GovernorateId == UserObj.GovernorateId).ToList();
        //    }
        //    if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId == 0)
        //    {
        //        lstTransactions = lstTransactions.Where(t => t.AssetDetail.Hospital.CityId == UserObj.CityId).ToList();
        //    }
        //    if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId == 0 && UserObj.HospitalId == 0)
        //    {
        //        lstTransactions = lstTransactions.Where(t => t.AssetDetail.Hospital.OrganizationId == UserObj.OrganizationId).ToList();
        //    }
        //    if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId == 0)
        //    {
        //        lstTransactions = lstTransactions.Where(t => t.AssetDetail.Hospital.SubOrganizationId == UserObj.SubOrganizationId).ToList();
        //    }
        //    if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId > 0)
        //    {
        //        if (lstRoleNames.Contains("Admin"))
        //        {
        //            lstTransactions = lstTransactions.ToList();
        //        }
        //        if (lstRoleNames.Contains("TLHospitalManager"))
        //        {
        //            lstTransactions = lstTransactions.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
        //        }
        //        if (lstRoleNames.Contains("EngDepManager"))
        //        {
        //            lstTransactions = lstTransactions.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
        //        }
        //        if (lstRoleNames.Contains("EngManager"))
        //        {
        //            lstTransactions = lstTransactions.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
        //        }
        //        if (lstRoleNames.Contains("Eng"))
        //        {
        //            lstTransactions = lstTransactions.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
        //        }
        //        if (lstRoleNames.Contains("AssetOwner"))
        //        {
        //            lstTransactions = lstTransactions.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
        //        }
        //    }
        //    if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId > 0)
        //    {
        //        if (lstRoleNames.Contains("Admin"))
        //        {
        //            lstTransactions = lstTransactions.ToList();
        //        }
        //        if (lstRoleNames.Contains("TLHospitalManager"))
        //        {
        //            lstTransactions = lstTransactions.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
        //        }
        //        if (lstRoleNames.Contains("EngDepManager"))
        //        {
        //            lstTransactions = lstTransactions.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
        //        }
        //        if (lstRoleNames.Contains("EngManager"))
        //        {
        //            lstTransactions = lstTransactions.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
        //        }
        //        if (lstRoleNames.Contains("AssetOwner"))
        //        {
        //            lstTransactions = lstTransactions.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
        //        }
        //        if (lstRoleNames.Contains("Eng"))
        //        {
        //            lstTransactions = lstTransactions.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
        //        }

        //    }
        //    if (lstTransactions.Count > 0)
        //    {
        //        var groupassets = lstTransactions.GroupBy(a => a.AssetDetailId).ToList();
        //        foreach (var trans in groupassets)
        //        {
        //            switch (trans.FirstOrDefault().AssetStatusId)
        //            {
        //                case 1:
        //                    lstNeedRepair.Add(trans.FirstOrDefault());
        //                    break;
        //                case 2:
        //                    lstInActive.Add(trans.FirstOrDefault());
        //                    break;
        //                case 3:
        //                    lstWorking.Add(trans.FirstOrDefault());
        //                    break;
        //                case 4:
        //                    lstUnderMaintenance.Add(trans.FirstOrDefault());
        //                    break;
        //                case 5:
        //                    lstUnderInstallation.Add(trans.FirstOrDefault());
        //                    break;
        //                case 6:
        //                    lstNotWorking.Add(trans.FirstOrDefault());
        //                    break;
        //                case 7:
        //                    lstShutdown.Add(trans.FirstOrDefault());
        //                    break;
        //                case 8:
        //                    lstExecluded.Add(trans.FirstOrDefault());
        //                    break;
        //                case 9:
        //                    lstHold.Add(trans.Last());
        //                    break;
        //            }

        //        }
        //    }

        //    getDataObj.CountNeedRepair = lstNeedRepair.Count;
        //    getDataObj.CountInActive = lstInActive.Count;
        //    getDataObj.CountWorking = lstWorking.Count;
        //    getDataObj.CountUnderMaintenance = lstUnderMaintenance.Count;
        //    getDataObj.CountUnderInstallation = lstUnderInstallation.Count;
        //    getDataObj.CountNotWorking = lstNotWorking.Count;
        //    getDataObj.CountShutdown = lstShutdown.Count;
        //    getDataObj.CountExecluded = lstExecluded.Count;
        //    getDataObj.CountHold = lstHold.Count;


        //    list.Add(getDataObj);


        //    return list;
        //}


        public IEnumerable<IndexAssetStatusVM.GetData> GetAllAssetsGroupByStatusId(int statusId, string userId, int hospitalId)
        {
            List<IndexAssetStatusVM.GetData> list = new List<IndexAssetStatusVM.GetData>();
            List<AssetStatusTransaction> lstNeedRepair = new List<AssetStatusTransaction>();
            List<AssetStatusTransaction> lstInActive = new List<AssetStatusTransaction>();
            List<AssetStatusTransaction> lstWorking = new List<AssetStatusTransaction>();
            List<AssetStatusTransaction> lstUnderMaintenance = new List<AssetStatusTransaction>();
            List<AssetStatusTransaction> lstUnderInstallation = new List<AssetStatusTransaction>();
            List<AssetStatusTransaction> lstNotWorking = new List<AssetStatusTransaction>();
            List<AssetStatusTransaction> lstShutdown = new List<AssetStatusTransaction>();
            List<AssetStatusTransaction> lstExecluded = new List<AssetStatusTransaction>();
            List<AssetStatusTransaction> lstHold = new List<AssetStatusTransaction>();
            IndexAssetStatusVM.GetData getDataObj = new IndexAssetStatusVM.GetData();

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
                foreach (var item in roleNames)
                {
                    lstRoleNames.Add(item.Name);
                }
            }
            var lstStatus = _context.AssetStatus.Where(a => a.Id != 1 && a.Id != 5 && a.Id != 6 && a.Id != 7).ToList();
            getDataObj.ListStatus = lstStatus;


            var lstTransactions = _context.AssetStatusTransactions
                            .Include(a => a.AssetDetail)
                            .Include(a => a.AssetDetail.Hospital)
                            .Include(a => a.AssetDetail.MasterAsset)
                            .OrderByDescending(a => a.StatusDate).ToList();



            if (statusId != 0)
            {
                lstTransactions = lstTransactions.Where(a => a.AssetStatusId == statusId).ToList();
            }
            else
            {
                lstTransactions = lstTransactions.ToList();
            }




            if (hospitalId != 0)
            {
                lstTransactions = lstTransactions.Where(a => a.AssetDetail.HospitalId == hospitalId).ToList();
            }
            else
            {
                lstTransactions = lstTransactions.ToList();
            }


            if (lstTransactions.Count > 0)
            {
                var transObj = lstTransactions[0];
                getDataObj.HospitalId = transObj.AssetDetail.HospitalId;
                getDataObj.GovernorateId = transObj.AssetDetail.Hospital.GovernorateId;
                getDataObj.CityId = transObj.AssetDetail.Hospital.CityId;
                getDataObj.OrganizationId = transObj.AssetDetail.Hospital.OrganizationId;
                getDataObj.SubOrganizationId = transObj.AssetDetail.Hospital.SubOrganizationId;

            }


            if (UserObj.GovernorateId == 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
            {
                lstTransactions = lstTransactions.ToList();
            }
            if (UserObj.GovernorateId > 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
            {
                lstTransactions = lstTransactions.Where(t => t.AssetDetail.Hospital.GovernorateId == UserObj.GovernorateId).ToList();
            }
            if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId == 0)
            {
                lstTransactions = lstTransactions.Where(t => t.AssetDetail.Hospital.CityId == UserObj.CityId).ToList();
            }
            if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId == 0 && UserObj.HospitalId == 0)
            {
                lstTransactions = lstTransactions.Where(t => t.AssetDetail.Hospital.OrganizationId == UserObj.OrganizationId).ToList();
            }
            if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId == 0)
            {
                lstTransactions = lstTransactions.Where(t => t.AssetDetail.Hospital.SubOrganizationId == UserObj.SubOrganizationId).ToList();
            }
            if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId > 0)
            {
                if (lstRoleNames.Contains("Admin"))
                {
                    lstTransactions = lstTransactions.ToList();
                }
                if (lstRoleNames.Contains("TLHospitalManager"))
                {
                    lstTransactions = lstTransactions.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                }
                if (lstRoleNames.Contains("EngDepManager"))
                {
                    lstTransactions = lstTransactions.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                }
                if (lstRoleNames.Contains("EngManager"))
                {
                    lstTransactions = lstTransactions.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                }
                if (lstRoleNames.Contains("Eng"))
                {
                    lstTransactions = lstTransactions.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                }
                if (lstRoleNames.Contains("AssetOwner"))
                {
                    lstTransactions = lstTransactions.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                }
            }
            if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId > 0)
            {
                if (lstRoleNames.Contains("Admin"))
                {
                    lstTransactions = lstTransactions.ToList();
                }
                if (lstRoleNames.Contains("TLHospitalManager"))
                {
                    lstTransactions = lstTransactions.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                }
                if (lstRoleNames.Contains("EngDepManager"))
                {
                    lstTransactions = lstTransactions.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
                }
                if (lstRoleNames.Contains("EngManager"))
                {
                    lstTransactions = lstTransactions.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
                }
                if (lstRoleNames.Contains("AssetOwner"))
                {
                    lstTransactions = lstTransactions.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
                }
                if (lstRoleNames.Contains("Eng"))
                {
                    lstTransactions = lstTransactions.Where(t => t.AssetDetail.HospitalId == UserObj.HospitalId).ToList();
                }

            }
            if (lstTransactions.Count > 0)
            {
                var groupassets = lstTransactions.GroupBy(a => a.AssetDetailId).ToList();
                foreach (var trans in groupassets)
                {
                    switch (trans.FirstOrDefault().AssetStatusId)
                    {
                        case 1:
                            lstNeedRepair.Add(trans.FirstOrDefault());
                            break;
                        case 2:
                            lstInActive.Add(trans.FirstOrDefault());
                            break;
                        case 3:
                            lstWorking.Add(trans.FirstOrDefault());
                            break;
                        case 4:
                            lstUnderMaintenance.Add(trans.FirstOrDefault());
                            break;
                        case 5:
                            lstUnderInstallation.Add(trans.FirstOrDefault());
                            break;
                        case 6:
                            lstNotWorking.Add(trans.FirstOrDefault());
                            break;
                        case 7:
                            lstShutdown.Add(trans.FirstOrDefault());
                            break;
                        case 8:
                            lstExecluded.Add(trans.FirstOrDefault());
                            break;
                        case 9:
                            lstHold.Add(trans.Last());
                            break;
                    }

                }
            }

            getDataObj.CountNeedRepair = lstNeedRepair.Count;
            getDataObj.CountInActive = lstInActive.Count;
            getDataObj.CountWorking = lstWorking.Count;
            getDataObj.CountUnderMaintenance = lstUnderMaintenance.Count;
            getDataObj.CountUnderInstallation = lstUnderInstallation.Count;
            getDataObj.CountNotWorking = lstNotWorking.Count;
            getDataObj.CountShutdown = lstShutdown.Count;
            getDataObj.CountExecluded = lstExecluded.Count;
            getDataObj.CountHold = lstHold.Count;

            getDataObj.TotalCount = lstNeedRepair.Count + lstInActive.Count + lstWorking.Count + lstUnderMaintenance.Count + lstUnderInstallation.Count + lstNotWorking.Count + lstShutdown.Count + lstExecluded.Count + lstHold.Count;
            list.Add(getDataObj);


            return list;
        }



    }
}
