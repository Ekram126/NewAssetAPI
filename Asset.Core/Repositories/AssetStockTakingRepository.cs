using Asset.Domain.Repositories;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.AssetStockTakingVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class AssetStockTakingRepository : IAssetStockTakingRepository
    {

        private ApplicationDbContext _context;

        public AssetStockTakingRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        

        public int Add(CreateAssetStockTakingVM createAssetStockTakingVM)
        {
            AssetStockTaking assetStockTakingObj = new AssetStockTaking();
            try
            {

                if (createAssetStockTakingVM != null)
                {


                    //  assetStockTakingObj.Id = createAssetStockTakingVM.Id;
                   assetStockTakingObj.HospitalId = createAssetStockTakingVM.HospitalId;
                    assetStockTakingObj.STSchedulesId = createAssetStockTakingVM.STSchedulesId;
                    assetStockTakingObj.UserId = createAssetStockTakingVM.UserId;
                    assetStockTakingObj.AssetDetailId = createAssetStockTakingVM.AssetDetailId;
                    assetStockTakingObj.CaptureDate = createAssetStockTakingVM.CaptureDate;
                    assetStockTakingObj.Longtitude = createAssetStockTakingVM.Longtitude;
                    assetStockTakingObj.Latitude = createAssetStockTakingVM.Latitude;
                    _context.Add(assetStockTakingObj);
                    _context.SaveChanges();
                    return assetStockTakingObj.Id;

                }


            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return assetStockTakingObj.Id;
        }

        public IEnumerable<IndexAssetStockTakingVM.GetData> GetAll()
        {
            return _context.AssetStockTakings.ToList().Select
                 (item => new IndexAssetStockTakingVM.GetData
                 {
                     AssetDetailId = item.AssetDetailId,
                     CaptureDate = item.CaptureDate,
                     HospitalId = item.HospitalId,
                     Id = item.Id,
                     Latitude = item.Latitude,
                     Longtitude = item.Longtitude,
                     STSchedulesId = item.STSchedulesId,
                     UserId = item.UserId,

                 });
        }




        #region OldCodeArea
        //public IndexExternalFixVM GetAllWithPaging(int hospitalId, int pageNumber, int pageSize)
        //{
        //    IndexExternalFixVM mainClass = new IndexExternalFixVM();
        //    var lstExternalFixes = _context.ExternalFixes.Include(a => a.AssetDetail)
        //          .Include(a => a.AssetDetail.MasterAsset)
        //           .Include(a => a.AssetDetail.MasterAsset.brand)
        //            .Include(a => a.AssetDetail.Supplier)
        //             .Include(a => a.AssetDetail.Department)
        //          .ToList().Select(item => new IndexExternalFixVM.GetData
        //          {
        //              Id = item.Id,
        //              AssetName = item.AssetDetail.MasterAsset.Name,
        //              HospitalId = (int)item.AssetDetail.HospitalId,
        //              AssetNameAr = item.AssetDetail.MasterAsset.NameAr,
        //              DepartmentName = item.AssetDetail.Department.Name,
        //              DepartmentNameAr = item.AssetDetail.Department.NameAr,
        //              SupplierName = item.AssetDetail.Supplier.Name,
        //              SupplierNameAr = item.AssetDetail.Supplier.NameAr,
        //              BrandName = item.AssetDetail.MasterAsset.brand.Name,
        //              BrandNameAr = item.AssetDetail.MasterAsset.brand.NameAr,
        //              ModelNumber = item.AssetDetail.MasterAsset.ModelNumber,
        //              SerialNumber = item.AssetDetail.SerialNumber,
        //              Barcode = item.AssetDetail.Barcode,
        //              AssetDetailId = item.AssetDetailId,
        //              ComingDate = item.ComingDate
        //          }).ToList();

        //    if (hospitalId == 0)
        //        lstExternalFixes = lstExternalFixes.ToList();
        //    else
        //        lstExternalFixes = lstExternalFixes.Where(a => a.HospitalId == hospitalId).ToList();


        //    mainClass.Results = lstExternalFixes.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        //    mainClass.Count = lstExternalFixes.Count;
        //    return mainClass;
        //}

        #endregion






        public IndexAssetStockTakingVM GetAllWithPaging(int pageNumber, int pageSize)
        {
            IndexAssetStockTakingVM mainClass = new IndexAssetStockTakingVM();
            var lstAssetStockTaking = _context.AssetStockTakings.Include(ww => ww.ApplicationUser)
                 .Include(ww => ww.Hospital).Include(ww => ww.AssetDetail.MasterAsset).Include(ww => ww.AssetDetail)
                 .ToList().Select(item => new IndexAssetStockTakingVM.GetData()
                 {
                  UserName=item.ApplicationUser.UserName,
                  HospitalName=item.Hospital.Name,
                  Latitude=item.Latitude,
                  Longtitude=item.Longtitude,
                  AssetName=item.AssetDetail.MasterAsset.Name,
                  BarCode=item.AssetDetail.Barcode,
                 });
            mainClass.Count = lstAssetStockTaking.Count();
            mainClass.Results= lstAssetStockTaking.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return mainClass;
        }
    }
}

