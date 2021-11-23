using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.AssetWorkOrderTaskVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class AssetWorkOrderTaskRepository : IAssetWorkOrderTaskRepository
    {
        private ApplicationDbContext _context;
        string msg;
        public AssetWorkOrderTaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(CreateAssetWorkOrderTaskVM createAssetWorkOrderTaskVM)
        {
            try
            {
                if (createAssetWorkOrderTaskVM != null)
                {
                    AssetWorkOrderTask assetWorkOrderTask = new AssetWorkOrderTask();
                    assetWorkOrderTask.Name = createAssetWorkOrderTaskVM.Name;
                    assetWorkOrderTask.NameAr = createAssetWorkOrderTaskVM.NameAr;
                    assetWorkOrderTask.Code = createAssetWorkOrderTaskVM.Code;
                    assetWorkOrderTask.MasterAssetId = createAssetWorkOrderTaskVM.MasterAssetId;
                    _context.AssetWorkOrderTasks.Add(assetWorkOrderTask);
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
            var assetWorkOrderTask = _context.AssetWorkOrderTasks.Find(id);
            try
            {
                if (assetWorkOrderTask != null)
                {
                    _context.AssetWorkOrderTasks.Remove(assetWorkOrderTask);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }

        public IEnumerable<IndexAssetWorkOrderTaskVM> GetAll()
        {
            return _context.AssetWorkOrderTasks.Select(prob => new IndexAssetWorkOrderTaskVM
            {
                Id = prob.Id,
                Name = prob.Name,
                NameAr = prob.NameAr,
                Code = prob.Code,
                MasterAssetId=prob.MasterAssetId,
                AssetName=prob.MasterAsset.Name
            }).ToList();
        }
        public IEnumerable<IndexAssetWorkOrderTaskVM> GetAllAssetWorkOrderTasksByMasterAssetId(int MasterAssetId)
        {
            return _context.AssetWorkOrderTasks.Where(a=>a.MasterAssetId== MasterAssetId).Select(prob => new IndexAssetWorkOrderTaskVM
            {
                Id = prob.Id,
                Name = prob.Name,
                NameAr = prob.NameAr,
                Code = prob.Code,
                MasterAssetId = prob.MasterAssetId,
                AssetName = prob.MasterAsset.Name
            }).ToList();
        }
        public IndexAssetWorkOrderTaskVM GetById(int id)
        {
            return _context.AssetWorkOrderTasks.Select(prob => new IndexAssetWorkOrderTaskVM
            {
                Id = prob.Id,
                Name = prob.Name,
                NameAr = prob.NameAr,
                Code = prob.Code,
                MasterAssetId = prob.MasterAssetId,
                AssetName = prob.MasterAsset.Name
            }).FirstOrDefault();
        }

        public void Update(int id, EditAssetWorkOrderTaskVM editAssetWorkOrderTaskVM)
        {
            try
            {
                AssetWorkOrderTask assetWorkOrderTask = new AssetWorkOrderTask();
                assetWorkOrderTask.Id = editAssetWorkOrderTaskVM.Id;
                assetWorkOrderTask.Name = editAssetWorkOrderTaskVM.Name;
                assetWorkOrderTask.NameAr = editAssetWorkOrderTaskVM.NameAr;
                assetWorkOrderTask.Code = editAssetWorkOrderTaskVM.Code;
                assetWorkOrderTask.MasterAssetId = editAssetWorkOrderTaskVM.MasterAssetId;
                _context.Entry(editAssetWorkOrderTaskVM).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }
    }
}
