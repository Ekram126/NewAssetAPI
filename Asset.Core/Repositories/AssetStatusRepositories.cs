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

        public IEnumerable<IndexAssetStatusVM.GetData> GetAllAssetsGroupByStatusCount()
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



            var lstStatus = _context.AssetStatus.ToList();
            //getDataObj.ListStatus = lstStatus;


            var lstTransactions = _context.AssetStatusTransactions.Include(a => a.AssetDetail).Include(a => a.AssetDetail.MasterAsset).ToList();
            if (lstTransactions.Count > 0)
            {
                foreach (var trans in lstTransactions)
                {
                    //var trackObj = _context.RequestTracking.OrderByDescending(a => a.Id).FirstOrDefault(a => a.RequestId == req.Id);
                    //if (trackObj != null)
                    //{
                    //    RequestTracking trk = trackObj;

                    //    if (trk.RequestStatusId == 1)
                    //    {
                    //        lstOpenTracks.Add(trk);
                    //    }
                    //    if (trk.RequestStatusId == 2)
                    //    {
                    //        lstCloseTracks.Add(trk);
                    //    }
                    //    if (trk.RequestStatusId == 3)
                    //    {
                    //        lstInProgressTracks.Add(trk);
                    //    }
                    //    if (trk.RequestStatusId == 4)
                    //    {
                    //        lstSolvedTracks.Add(trk);
                    //    }
                    //    if (trk.RequestStatusId == 5)
                    //    {
                    //        lstApprovedTracks.Add(trk);
                    //    }
                    //}
                }
            }

            //getDataObj.CountOpen = lstOpenTracks.Count;
            //getDataObj.CountClosed = lstCloseTracks.Count;
            //getDataObj.CountInProgress = lstInProgressTracks.Count;
            //getDataObj.CountSolved = lstSolvedTracks.Count;
            //getDataObj.CountApproved = lstApprovedTracks.Count;
            list.Add(getDataObj);
            return list;
        }
    }
}
