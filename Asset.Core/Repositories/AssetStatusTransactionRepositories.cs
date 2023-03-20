using Asset.Domain.Repositories;
using Asset.Models;

using Asset.ViewModels.AssetStatusTransactionVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class AssetStatusTransactionRepositories : IAssetStatusTransactionRepository
    {

        private ApplicationDbContext _context;


        public AssetStatusTransactionRepositories(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Add(CreateAssetStatusTransactionVM model)
        {
            AssetStatusTransaction AssetStatusTransactionsTransactionObj = new AssetStatusTransaction();
            try
            {
                if (model != null)
                {
                    AssetStatusTransactionsTransactionObj.AssetDetailId = model.AssetDetailId;
                    AssetStatusTransactionsTransactionObj.AssetStatusId = model.AssetStatusId;
                    AssetStatusTransactionsTransactionObj.HospitalId = model.HospitalId;
                    if (model.StatusDate != null)
                        AssetStatusTransactionsTransactionObj.StatusDate = DateTime.Parse(model.StatusDate);
                    else
                        AssetStatusTransactionsTransactionObj.StatusDate = DateTime.Now;

                    _context.AssetStatusTransactions.Add(AssetStatusTransactionsTransactionObj);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return AssetStatusTransactionsTransactionObj.Id;
        }

        public int Delete(int id)
        {
            var AssetStatusTransactionsTransactionObj = _context.AssetStatusTransactions.Find(id);
            try
            {
                if (AssetStatusTransactionsTransactionObj != null)
                {
                    _context.AssetStatusTransactions.Remove(AssetStatusTransactionsTransactionObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }

        public IEnumerable<IndexAssetStatusTransactionVM.GetData> GetAll()
        {
            return _context.AssetStatusTransactions.ToList().Select(item => new IndexAssetStatusTransactionVM.GetData
            {
                Id = item.Id,
                HospitalId = int.Parse(item.HospitalId.ToString()),
                //StatusName = _context.AssetStatus.Where(a => a.Id == item.AssetStatusId).ToString(),
                StatusName = _context.AssetStatus.Where(a => a.Id == item.AssetStatusId).FirstOrDefault().Name,
                StatusNameAr = _context.AssetStatus.Where(a => a.Id == item.AssetStatusId).FirstOrDefault().NameAr,
                StatusDate = item.StatusDate.Value.ToShortDateString()
            });
        }

        public IEnumerable<IndexAssetStatusTransactionVM.GetData> GetAssetStatusByAssetDetailId(int assetId)
        {
            var assetMovementObj = _context.AssetStatusTransactions.ToList().Where(a => a.AssetDetailId == assetId).OrderByDescending(a => a.StatusDate)
                .Select(item => new IndexAssetStatusTransactionVM.GetData
                {
                    Id = item.Id,
                    HospitalId = int.Parse(item.HospitalId.ToString()),
                    StatusName = _context.AssetStatus.Where(a => a.Id == item.AssetStatusId).FirstOrDefault().Name,
                    StatusNameAr = _context.AssetStatus.Where(a => a.Id == item.AssetStatusId).FirstOrDefault().NameAr,
                    StatusDate = item.StatusDate.Value.ToString(),
                    AssetDetailId = int.Parse( item.AssetDetailId.ToString())
                }).ToList();
            return assetMovementObj;
        }

        public AssetStatusTransaction GetById(int id)
        {
            return _context.AssetStatusTransactions.Find(id);
        }

        public List<AssetStatusTransaction> GetLastTransactionByAssetId(int assetId)
        {
            var lstLastTransaction = _context.AssetStatusTransactions.ToList()
                                        .Where(a => a.AssetDetailId == assetId)
                                        .OrderByDescending(a => a.StatusDate)
                                         .Select(item => new AssetStatusTransaction
                                         {
                                             Id = item.Id,
                                             HospitalId = int.Parse(item.HospitalId.ToString()),
                                             AssetDetailId = item.AssetDetailId,
                                             AssetStatusId = item.AssetStatusId,
                                             StatusDate = item.StatusDate
                                         }).ToList();



            return lstLastTransaction;
        }

        public int Update(AssetStatusTransaction model)
        {
            try
            {
                var AssetStatusTransactionsTransactionObj = _context.AssetStatusTransactions.Find(model.Id);
                AssetStatusTransactionsTransactionObj.Id = model.Id;
                AssetStatusTransactionsTransactionObj.AssetDetailId = model.AssetDetailId;
                AssetStatusTransactionsTransactionObj.AssetStatusId = model.AssetStatusId;
                AssetStatusTransactionsTransactionObj.HospitalId = model.HospitalId;
                AssetStatusTransactionsTransactionObj.StatusDate = model.StatusDate;
                _context.Entry(AssetStatusTransactionsTransactionObj).State = EntityState.Modified;
                _context.SaveChanges();
                return AssetStatusTransactionsTransactionObj.Id;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }


    }
}
