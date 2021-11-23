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

        public int Add(AssetStatusTransaction model)
        {
            AssetStatusTransaction AssetStatusTransactionsTransactionObj = new AssetStatusTransaction();
            try
            {
                if (model != null)
                {
                    AssetStatusTransactionsTransactionObj.AssetDetailId = model.AssetDetailId;
                    AssetStatusTransactionsTransactionObj.AssetStatusId = model.AssetStatusId;
                    AssetStatusTransactionsTransactionObj.StatusDate = model.StatusDate;
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
                    StatusName = _context.AssetStatus.Where(a => a.Id == item.AssetStatusId).FirstOrDefault().Name,
                    StatusNameAr = _context.AssetStatus.Where(a => a.Id == item.AssetStatusId).FirstOrDefault().NameAr,
                    StatusDate = item.StatusDate.Value.ToShortDateString(),
                    AssetDetailId = item.AssetDetailId
                }).ToList();
            return assetMovementObj;
        }

        public AssetStatusTransaction GetById(int id)
        {
            return _context.AssetStatusTransactions.Find(id);
        }


        public int Update(AssetStatusTransaction model)
        {
            try
            {
                var AssetStatusTransactionsTransactionObj = _context.AssetStatusTransactions.Find(model.Id);
                AssetStatusTransactionsTransactionObj.Id = model.Id;
                AssetStatusTransactionsTransactionObj.AssetDetailId = model.AssetDetailId;
                AssetStatusTransactionsTransactionObj.AssetStatusId = model.AssetStatusId;
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
