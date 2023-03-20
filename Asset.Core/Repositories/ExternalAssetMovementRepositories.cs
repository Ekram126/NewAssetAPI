using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.AssetDetailAttachmentVM;
using Asset.ViewModels.AssetDetailVM;
using Asset.ViewModels.AssetMovementVM;
using Asset.ViewModels.ExternalAssetMovementVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class ExternalAssetMovementRepositories : IExternalAssetMovementRepository
    {
        private ApplicationDbContext _context;


        public ExternalAssetMovementRepositories(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Add(ExternalAssetMovement movementObj)
        {
            ExternalAssetMovement assetMovementObj = new ExternalAssetMovement();
            try
            {
                if (movementObj != null)
                {
                    assetMovementObj.MovementDate = movementObj.MovementDate;
                    assetMovementObj.AssetDetailId = movementObj.AssetDetailId;
                    assetMovementObj.Notes = movementObj.Notes;
                    assetMovementObj.HospitalName = movementObj.HospitalName;
                    assetMovementObj.MovementDate = movementObj.MovementDate;
                    _context.ExternalAssetMovements.Add(assetMovementObj);
                    _context.SaveChanges();
                    return assetMovementObj.Id;
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return assetMovementObj.Id;
        }

        public int CreateExternalAssetMovementAttachments(ExternalAssetMovementAttachment attachObj)
        {
            ExternalAssetMovementAttachment documentObj = new ExternalAssetMovementAttachment();
            documentObj.Title = attachObj.Title;
            documentObj.FileName = attachObj.FileName;
            documentObj.ExternalAssetMovementId = attachObj.ExternalAssetMovementId;
            _context.ExternalAssetMovementAttachments.Add(documentObj);
            _context.SaveChanges();
            return attachObj.Id;
        }

        public int Delete(int id)
        {
            var externalAssetMovementObj = _context.ExternalAssetMovements.Find(id);
            try
            {
                if (externalAssetMovementObj != null)
                {
                    _context.ExternalAssetMovements.Remove(externalAssetMovementObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }

        public ExternalAssetMovement GetById(int id)
        {
            var externalAssetMovementObj = _context.ExternalAssetMovements.Find(id);
            return externalAssetMovementObj;
        }

        public IndexExternalAssetMovementVM GetExternalAssetMovements(int pageNumber, int pageSize)
        {
            IndexExternalAssetMovementVM mainClass = new IndexExternalAssetMovementVM();

            List<IndexExternalAssetMovementVM.GetData> list = new List<IndexExternalAssetMovementVM.GetData>();

            var lstMovements = _context.ExternalAssetMovements.Include(a=>a.AssetDetail)
                .Include(a => a.AssetDetail.MasterAsset).OrderByDescending(a => a.MovementDate)
                .Select(item => new IndexExternalAssetMovementVM.GetData
                {
                    Id = item.Id,
                    AssetDetailId = item.AssetDetailId,
                    AssetName= item.AssetDetail.MasterAsset != null? item.AssetDetail.MasterAsset.Name:"",
                    AssetNameAr = item.AssetDetail.MasterAsset != null ? item.AssetDetail.MasterAsset.NameAr : "",
                    BarCode = item.AssetDetail.Barcode,
                    SerialNumber = item.AssetDetail.SerialNumber,
                    ModelNumber = item.AssetDetail.MasterAsset != null ? item.AssetDetail.MasterAsset.ModelNumber : "",
                    MovementDate = item.MovementDate,
                    HospitalName = item.HospitalName,
                    Notes = item.Notes
                }).ToList();
            var movementPerPage = lstMovements.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            mainClass.Results = movementPerPage;
            mainClass.Count = lstMovements.Count();
            return mainClass;
        }

        public IEnumerable<ExternalAssetMovementAttachment> GetExternalMovementAttachmentByExternalAssetMovementId(int externalAssetMovementId)
        {
            return _context.ExternalAssetMovementAttachments.ToList().Where(a => a.ExternalAssetMovementId == externalAssetMovementId).ToList();
        }

        public IEnumerable<ExternalAssetMovement> GetExternalMovementsByAssetDetailId(int assetId)
        {
            List<ExternalAssetMovement> list = new List<ExternalAssetMovement>();

            var lstMovements = _context.ExternalAssetMovements.ToList().Where(a => a.AssetDetailId == assetId).OrderByDescending(a => a.MovementDate).ToList();
            if (lstMovements.Count > 0)
            {
                foreach (var item in lstMovements)
                {
                    ExternalAssetMovement getDataObj = new ExternalAssetMovement();
                    getDataObj.Id = item.Id;
                    getDataObj.AssetDetailId = item.AssetDetailId;
                    getDataObj.MovementDate = item.MovementDate;
                    getDataObj.HospitalName = item.HospitalName;
                    getDataObj.Notes = item.Notes;
                    list.Add(getDataObj);
                }
            }
            return list;
        }

        public int Update(ExternalAssetMovement movementObj)
        {
            try
            {

                var assetDetailObj = _context.ExternalAssetMovements.Find(movementObj.Id);
                assetDetailObj.Id = movementObj.Id;
                assetDetailObj.AssetDetailId = movementObj.AssetDetailId;
                assetDetailObj.MovementDate = movementObj.MovementDate;
                assetDetailObj.Notes = movementObj.Notes;
                assetDetailObj.HospitalName = movementObj.HospitalName;
                _context.Entry(assetDetailObj).State = EntityState.Modified;
                _context.SaveChanges();
                return assetDetailObj.Id;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return 0;
        }
    }


}
