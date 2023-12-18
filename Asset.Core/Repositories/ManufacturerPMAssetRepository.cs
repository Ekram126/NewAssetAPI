using Asset.Domain.Repositories;
using Asset.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Asset.ViewModels.ManufacturerPMAssetVM;
using Asset.ViewModels.WNPMAssetTimes;
using Microsoft.AspNetCore.Server.IIS.Core;
using Asset.ViewModels.PMAssetTaskVM;

namespace Asset.Core.Repositories
{
    public class ManufacturerPMAssetRepository : IManufacturerPMAssetRepository
    {
        private ApplicationDbContext _context;

        public ManufacturerPMAssetRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IndexUnScheduledManfacturerPMAssetVM CreateManfacturerAssetTimes(int pageNumber, int pageSize)
        {

            IndexUnScheduledManfacturerPMAssetVM mainClass = new IndexUnScheduledManfacturerPMAssetVM();
            List<ForCheckManfacturerPMAssetsVM> CheckObjects = GetAllForCheck();
            CheckObjects = CheckObjects.Distinct().ToList();
            ForCheckManfacturerPMAssetsVM checkObj = null; // object to Compare
            List<IndexUnScheduledManfacturerPMAssetVM.GetData> unscheduledList = new List<IndexUnScheduledManfacturerPMAssetVM.GetData>();
            var allAsset = _context.AssetDetails.Include(a => a.MasterAsset)
                .Include(a => a.Department).Include(a => a.Supplier)
                .Include(a => a.Hospital).Include(a => a.MasterAsset.brand)
                .OrderBy(ww => ww.Id).Distinct().ToList();
            allAsset = allAsset.GroupBy(x => new { x.Id }).Select(x => x.First()).ToList();

            var contracts = _context.ContractDetails.Include(a => a.MasterContract).ToList();

            foreach (var asset in allAsset)
            {
                if (CheckObjects.Find(a => a.AssetDetailId == asset.Id) != null)
                {
                    // catch Last Schedule date 
                    checkObj = CheckObjects.Where(a => a.AssetDetailId == asset.Id).ToList().LastOrDefault();
                }
                else
                {
                    // if check object not found of previous scheduled data so its new asset and its null 
                    checkObj = null;
                }
                //check Warranty Or Contract
                //warranty
                if ((asset.InstallationDate != null) && (asset.WarrantyStart != null) && (asset.WarrantyEnd != null))
                {
                    if (checkObj == null)
                    {
                        if (asset.InstallationDate >= asset.WarrantyStart && asset.InstallationDate <= asset.WarrantyEnd)
                        {
                            // device installed in warranty period
                            if (DateTime.Now <= asset.WarrantyEnd)
                            {
                                if (asset.MasterAsset != null)
                                {
                                    var periodcMaintananceDuration = asset.MasterAsset.PMTimeId;
                                    if (periodcMaintananceDuration != null)
                                    {
                                        if (periodcMaintananceDuration == 1)
                                        {
                                            // m for month y for year
                                            int m = 0;
                                            int y = 0;

                                            for (var i = asset.InstallationDate.Value.Year; i <= asset.WarrantyEnd.Value.Year; i += y)
                                            {
                                                var assetDate = asset.InstallationDate.Value.AddMonths(m);
                                                if (asset.WarrantyEnd.Value >= assetDate)
                                                {

                                                    ManufacturerPMAsset timeObj = new ManufacturerPMAsset();
                                                    timeObj.AssetDetailId = asset.Id;
                                                    timeObj.HospitalId = asset.HospitalId;
                                                    timeObj.PMDate = assetDate;
                                                    timeObj.IsDone = false;
                                                    _context.ManufacturerPMAssets.Add(timeObj);
                                                    _context.SaveChanges();

                                                    m += 12;
                                                    y = m % 12 == 0 ? 1 : 0;
                                                }
                                            }
                                        }
                                        if (periodcMaintananceDuration == 2)
                                        {

                                            int m = 0;
                                            int y = 0;

                                            for (var i = asset.InstallationDate.Value.Year; i <= asset.WarrantyEnd.Value.Year; i += y)
                                            {
                                                var assetDate = asset.InstallationDate.Value.AddMonths(m);
                                                if (asset.WarrantyEnd.Value >= assetDate)
                                                {

                                                    ManufacturerPMAsset timeObj = new ManufacturerPMAsset();
                                                    timeObj.AssetDetailId = asset.Id;
                                                    timeObj.HospitalId = asset.HospitalId;
                                                    timeObj.PMDate = assetDate;
                                                    timeObj.IsDone = false;
                                                    _context.ManufacturerPMAssets.Add(timeObj);
                                                    _context.SaveChanges();

                                                    m += 6;
                                                    y = m % 12 == 0 ? 1 : 0;
                                                }
                                            }

                                        }
                                        if (periodcMaintananceDuration == 3)
                                        {
                                            int m = 0;
                                            int y = 0;

                                            for (var i = asset.InstallationDate.Value.Year; i <= asset.WarrantyEnd.Value.Year; i += y)
                                            {
                                                var assetDate = asset.InstallationDate.Value.AddMonths(m);
                                                if (asset.WarrantyEnd.Value >= assetDate)
                                                {

                                                    ManufacturerPMAsset timeObj = new ManufacturerPMAsset();
                                                    timeObj.AssetDetailId = asset.Id;
                                                    timeObj.HospitalId = asset.HospitalId;
                                                    timeObj.PMDate = assetDate;
                                                    timeObj.IsDone = false;
                                                    _context.ManufacturerPMAssets.Add(timeObj);
                                                    _context.SaveChanges();

                                                    m += 3;
                                                    y = m % 12 == 0 ? 1 : 0;
                                                }
                                            }
                                        }
                                        if (periodcMaintananceDuration == 4)
                                        {
                                            int m = 0;
                                            int y = 0;

                                            for (var i = asset.InstallationDate.Value.Year; i <= asset.WarrantyEnd.Value.Year; i += y)
                                            {
                                                var assetDate = asset.InstallationDate.Value.AddMonths(m);
                                                if (asset.WarrantyEnd.Value >= assetDate)
                                                {

                                                    ManufacturerPMAsset timeObj = new ManufacturerPMAsset();
                                                    timeObj.AssetDetailId = asset.Id;
                                                    timeObj.HospitalId = asset.HospitalId;
                                                    timeObj.PMDate = assetDate;
                                                    timeObj.IsDone = false;
                                                    _context.ManufacturerPMAssets.Add(timeObj);
                                                    _context.SaveChanges();

                                                    m += 1;
                                                    y = m % 12 == 0 ? 1 : 0;
                                                }
                                            }

                                        }

                                    }

                                    // else
                                    //  // لا توجد مده صيانه دوريه مسجله
                                    // periodic time is null

                                    else
                                    {

                                        var Obj = new IndexUnScheduledManfacturerPMAssetVM.GetData();
                                        Obj.AssetDetailId = asset.Id;
                                        Obj.UnscheduledReason = "Asset Periodic Maintainance Time is null";
                                        Obj.UnscheduledReasonAR = "لا توجد مده صيانه دوريه مسجله لهذا الاصل ";
                                        Obj.Barcode = asset.Barcode;
                                        Obj.SerialNumber = asset.SerialNumber;
                                        Obj.ModelNumber = asset.MasterAsset.ModelNumber;
                                        Obj.BrandName = asset.MasterAsset.brand.Name;
                                        Obj.BrandNameAR = asset.MasterAsset.brand.NameAr;
                                        Obj.BrandCode = asset.MasterAsset.brand.Code;
                                        Obj.AssetName = asset.MasterAsset.Name;
                                        Obj.AssetNameAR = asset.MasterAsset.NameAr;
                                        unscheduledList.Add(Obj);
                                    }


                                }
                            }
                        }
                    }
                    else
                    {
                        if (asset.InstallationDate >= asset.WarrantyStart && asset.InstallationDate <= asset.WarrantyEnd && asset.WarrantyStart > checkObj.PMDate)
                        {
                            // device installed in warranty period
                            if (DateTime.Now <= asset.WarrantyEnd)
                            {
                                if (asset.MasterAsset != null)
                                {
                                    var periodcMaintananceDuration = asset.MasterAsset.PMTimeId;
                                    if (periodcMaintananceDuration != null)
                                    {
                                        if (periodcMaintananceDuration == 1)
                                        {
                                            // m for month y for year
                                            int m = 0;
                                            int y = 0;

                                            for (var i = asset.InstallationDate.Value.Year; i <= asset.WarrantyEnd.Value.Year; i += y)
                                            {
                                                var assetDate = asset.InstallationDate.Value.AddMonths(m);
                                                if (asset.WarrantyEnd.Value >= assetDate)
                                                {

                                                    ManufacturerPMAsset timeObj = new ManufacturerPMAsset();
                                                    timeObj.AssetDetailId = asset.Id;
                                                    timeObj.HospitalId = asset.HospitalId;
                                                    timeObj.PMDate = assetDate;
                                                    timeObj.IsDone = false;
                                                    _context.ManufacturerPMAssets.Add(timeObj);
                                                    _context.SaveChanges();

                                                    m += 12;
                                                    y = m % 12 == 0 ? 1 : 0;
                                                }
                                            }
                                        }
                                        if (periodcMaintananceDuration == 2)
                                        {

                                            int m = 0;
                                            int y = 0;

                                            for (var i = asset.InstallationDate.Value.Year; i <= asset.WarrantyEnd.Value.Year; i += y)
                                            {
                                                var assetDate = asset.InstallationDate.Value.AddMonths(m);
                                                if (asset.WarrantyEnd.Value >= assetDate)
                                                {

                                                    ManufacturerPMAsset timeObj = new ManufacturerPMAsset();
                                                    timeObj.AssetDetailId = asset.Id;
                                                    timeObj.HospitalId = asset.HospitalId;
                                                    timeObj.PMDate = assetDate;
                                                    timeObj.IsDone = false;
                                                    _context.ManufacturerPMAssets.Add(timeObj);
                                                    _context.SaveChanges();

                                                    m += 6;
                                                    y = m % 12 == 0 ? 1 : 0;
                                                }
                                            }

                                        }
                                        if (periodcMaintananceDuration == 3)
                                        {
                                            int m = 0;
                                            int y = 0;

                                            for (var i = asset.InstallationDate.Value.Year; i <= asset.WarrantyEnd.Value.Year; i += y)
                                            {
                                                var assetDate = asset.InstallationDate.Value.AddMonths(m);
                                                if (asset.WarrantyEnd.Value >= assetDate)
                                                {

                                                    ManufacturerPMAsset timeObj = new ManufacturerPMAsset();
                                                    timeObj.AssetDetailId = asset.Id;
                                                    timeObj.HospitalId = asset.HospitalId;
                                                    timeObj.PMDate = assetDate;
                                                    timeObj.IsDone = false;
                                                    _context.ManufacturerPMAssets.Add(timeObj);
                                                    _context.SaveChanges();

                                                    m += 3;
                                                    y = m % 12 == 0 ? 1 : 0;
                                                }
                                            }
                                        }
                                        if (periodcMaintananceDuration == 4)
                                        {
                                            int m = 0;
                                            int y = 0;

                                            for (var i = asset.InstallationDate.Value.Year; i <= asset.WarrantyEnd.Value.Year; i += y)
                                            {
                                                var assetDate = asset.InstallationDate.Value.AddMonths(m);
                                                if (asset.WarrantyEnd.Value >= assetDate)
                                                {

                                                    ManufacturerPMAsset timeObj = new ManufacturerPMAsset();
                                                    timeObj.AssetDetailId = asset.Id;
                                                    timeObj.HospitalId = asset.HospitalId;
                                                    timeObj.PMDate = assetDate;
                                                    timeObj.IsDone = false;
                                                    _context.ManufacturerPMAssets.Add(timeObj);
                                                    _context.SaveChanges();

                                                    m += 1;
                                                    y = m % 12 == 0 ? 1 : 0;
                                                }
                                            }

                                        }

                                    }

                                    // else
                                    //  // لا توجد مده صيانه دوريه مسجله
                                    // periodic time is null

                                    else
                                    {

                                        var Obj = new IndexUnScheduledManfacturerPMAssetVM.GetData();
                                        Obj.AssetDetailId = asset.Id;
                                        Obj.UnscheduledReason = "Asset Periodic Maintainance Time is null";
                                        Obj.UnscheduledReasonAR = "لا توجد مده صيانه دوريه مسجله لهذا الاصل ";
                                        Obj.Barcode = asset.Barcode;
                                        Obj.SerialNumber = asset.SerialNumber;
                                        Obj.ModelNumber = asset.MasterAsset.ModelNumber;
                                        Obj.BrandName = asset.MasterAsset.brand.Name;
                                        Obj.BrandNameAR = asset.MasterAsset.brand.NameAr;
                                        Obj.BrandCode = asset.MasterAsset.brand.Code;
                                        Obj.AssetName = asset.MasterAsset.Name;
                                        Obj.AssetNameAR = asset.MasterAsset.NameAr;
                                        unscheduledList.Add(Obj);
                                    }


                                }
                            }
                        }
                    }


                }



                //contracts

                else if (contracts != null)
                {
                    if (contracts.Find(a => a.AssetDetailId == asset.Id) != null)
                    {
                        // Have a Contract
                        var contractsInfo = contracts.Where(a => a.AssetDetailId == asset.Id).FirstOrDefault();
                        // var contractsInfo = contracts.Find(a => a.AssetDetailId == asset.Id);
                        if (contractsInfo != null)
                        {
                            if (contractsInfo.MasterContractId != null)
                            {

                                if ((contractsInfo.MasterContract.From != null) && (contractsInfo.MasterContract.To != null))

                                {
                                    if (checkObj == null)
                                    {
                                        if (contractsInfo.MasterContract.To > contractsInfo.MasterContract.From)
                                        {

                                            if (contractsInfo.MasterContract.To >= DateTime.Now)
                                            {
                                                var periodcMaintananceDuration = asset.MasterAsset.PMTimeId;
                                                if (periodcMaintananceDuration != null)
                                                {
                                                    if (periodcMaintananceDuration == 1)
                                                    {
                                                        int m = 0;
                                                        int y = 0;

                                                        for (var i = contractsInfo.MasterContract.From.Value.Year; i <= contractsInfo.MasterContract.To.Value.Year; i += y)
                                                        {
                                                            var assetDate = contractsInfo.MasterContract.From.Value.AddMonths(m);
                                                            if (contractsInfo.MasterContract.To.Value >= assetDate)
                                                            {

                                                                ManufacturerPMAsset timeObj = new ManufacturerPMAsset();
                                                                timeObj.AssetDetailId = asset.Id;
                                                                timeObj.HospitalId = asset.HospitalId;
                                                                timeObj.PMDate = assetDate;
                                                                timeObj.IsDone = false;
                                                                _context.ManufacturerPMAssets.Add(timeObj);
                                                                _context.SaveChanges();

                                                            }

                                                            m += 12;
                                                            y = m % 12 == 0 ? 1 : 0;
                                                        }

                                                    }
                                                    if (periodcMaintananceDuration == 2)
                                                    {
                                                        int m = 0;
                                                        int y = 0;

                                                        for (var i = contractsInfo.MasterContract.From.Value.Year; i <= contractsInfo.MasterContract.To.Value.Year; i += y)
                                                        {
                                                            var assetDate = contractsInfo.MasterContract.From.Value.AddMonths(m);



                                                            if (contractsInfo.MasterContract.To.Value >= assetDate)
                                                            {

                                                                ManufacturerPMAsset timeObj = new ManufacturerPMAsset();
                                                                timeObj.AssetDetailId = asset.Id;
                                                                timeObj.HospitalId = asset.HospitalId;
                                                                timeObj.PMDate = assetDate;
                                                                timeObj.IsDone = false;
                                                                _context.ManufacturerPMAssets.Add(timeObj);
                                                                _context.SaveChanges();
                                                            }
                                                            m += 6;
                                                            y = m % 12 == 0 ? 1 : 0;


                                                        }
                                                    }
                                                    if (periodcMaintananceDuration == 3)
                                                    {
                                                        int m = 0;
                                                        int y = 0;

                                                        for (var i = contractsInfo.MasterContract.From.Value.Year; i <= contractsInfo.MasterContract.To.Value.Year; i += y)
                                                        {
                                                            var assetDate = contractsInfo.MasterContract.From.Value.AddMonths(m);
                                                            if (contractsInfo.MasterContract.To.Value >= assetDate)
                                                            {

                                                                ManufacturerPMAsset timeObj = new ManufacturerPMAsset();
                                                                timeObj.AssetDetailId = asset.Id;
                                                                timeObj.HospitalId = asset.HospitalId;
                                                                timeObj.PMDate = assetDate;
                                                                timeObj.IsDone = false;
                                                                _context.ManufacturerPMAssets.Add(timeObj);
                                                                _context.SaveChanges();
                                                            }
                                                            m += 3;
                                                            y = m % 12 == 0 ? 1 : 0;
                                                        }
                                                    }
                                                    if (periodcMaintananceDuration == 4)
                                                    {
                                                        int m = 0;
                                                        int y = 0;

                                                        for (var i = contractsInfo.MasterContract.From.Value.Year; i <= contractsInfo.MasterContract.To.Value.Year; i += y)
                                                        {
                                                            var assetDate = contractsInfo.MasterContract.From.Value.AddMonths(m);
                                                            if (contractsInfo.MasterContract.To.Value >= assetDate)
                                                            {

                                                                ManufacturerPMAsset timeObj = new ManufacturerPMAsset();
                                                                timeObj.AssetDetailId = asset.Id;
                                                                timeObj.HospitalId = asset.HospitalId;
                                                                timeObj.PMDate = assetDate;
                                                                timeObj.IsDone = false;
                                                                _context.ManufacturerPMAssets.Add(timeObj);
                                                                _context.SaveChanges();


                                                            }

                                                            m += 1;
                                                            y = m % 12 == 0 ? 1 : 0;
                                                        }
                                                    }
                                                }
                                                //else
                                                // لا توجد مده صيانه دوريه مسجله
                                                // periodic time is null
                                                else
                                                {
                                                    var Obj = new IndexUnScheduledManfacturerPMAssetVM.GetData();
                                                    Obj.AssetDetailId = asset.Id;
                                                    Obj.UnscheduledReason = "Asset Periodic Maintainance Time is null";
                                                    Obj.UnscheduledReasonAR = "لا توجد مده صيانه دوريه مسجله لهذا الاصل ";
                                                    unscheduledList.Add(Obj);

                                                }


                                            }

                                            //contract has ended 
                                            // the reason is contract has ended
                                            //انتهاء مده عقد الصيانه


                                            else
                                            {
                                                var Obj = new IndexUnScheduledManfacturerPMAssetVM.GetData();
                                                Obj.AssetDetailId = asset.Id;
                                                Obj.UnscheduledReason = "Asset Contract has been ended";
                                                Obj.UnscheduledReasonAR = "انتهاء مده عقد صيانه هذا الاصل";
                                                unscheduledList.Add(Obj);
                                            }

                                        }

                                    }

                                    else
                                    {
                                        if (contractsInfo.MasterContract.To > contractsInfo.MasterContract.From && contractsInfo.MasterContract.From > checkObj.PMDate)
                                        {

                                            if (contractsInfo.MasterContract.To >= DateTime.Now)
                                            {
                                                var periodcMaintananceDuration = asset.MasterAsset.PMTimeId;
                                                if (periodcMaintananceDuration != null)
                                                {
                                                    if (periodcMaintananceDuration == 1)
                                                    {
                                                        int m = 0;
                                                        int y = 0;

                                                        for (var i = contractsInfo.MasterContract.From.Value.Year; i <= contractsInfo.MasterContract.To.Value.Year; i += y)
                                                        {
                                                            var assetDate = contractsInfo.MasterContract.From.Value.AddMonths(m);
                                                            if (contractsInfo.MasterContract.To.Value >= assetDate)
                                                            {

                                                                ManufacturerPMAsset timeObj = new ManufacturerPMAsset();
                                                                timeObj.AssetDetailId = asset.Id;
                                                                timeObj.HospitalId = asset.HospitalId;
                                                                timeObj.PMDate = assetDate;
                                                                timeObj.IsDone = false;
                                                                _context.ManufacturerPMAssets.Add(timeObj);
                                                                _context.SaveChanges();

                                                            }

                                                            m += 12;
                                                            y = m % 12 == 0 ? 1 : 0;
                                                        }

                                                    }
                                                    if (periodcMaintananceDuration == 2)
                                                    {
                                                        int m = 0;
                                                        int y = 0;

                                                        for (var i = contractsInfo.MasterContract.From.Value.Year; i <= contractsInfo.MasterContract.To.Value.Year; i += y)
                                                        {
                                                            var assetDate = contractsInfo.MasterContract.From.Value.AddMonths(m);



                                                            if (contractsInfo.MasterContract.To.Value >= assetDate)
                                                            {

                                                                ManufacturerPMAsset timeObj = new ManufacturerPMAsset();
                                                                timeObj.AssetDetailId = asset.Id;
                                                                timeObj.HospitalId = asset.HospitalId;
                                                                timeObj.PMDate = assetDate;
                                                                timeObj.IsDone = false;
                                                                _context.ManufacturerPMAssets.Add(timeObj);
                                                                _context.SaveChanges();
                                                            }
                                                            m += 6;
                                                            y = m % 12 == 0 ? 1 : 0;


                                                        }
                                                    }
                                                    if (periodcMaintananceDuration == 3)
                                                    {
                                                        int m = 0;
                                                        int y = 0;

                                                        for (var i = contractsInfo.MasterContract.From.Value.Year; i <= contractsInfo.MasterContract.To.Value.Year; i += y)
                                                        {
                                                            var assetDate = contractsInfo.MasterContract.From.Value.AddMonths(m);
                                                            if (contractsInfo.MasterContract.To.Value >= assetDate)
                                                            {

                                                                ManufacturerPMAsset timeObj = new ManufacturerPMAsset();
                                                                timeObj.AssetDetailId = asset.Id;
                                                                timeObj.HospitalId = asset.HospitalId;
                                                                timeObj.PMDate = assetDate;
                                                                timeObj.IsDone = false;
                                                                _context.ManufacturerPMAssets.Add(timeObj);
                                                                _context.SaveChanges();
                                                            }
                                                            m += 3;
                                                            y = m % 12 == 0 ? 1 : 0;
                                                        }
                                                    }
                                                    if (periodcMaintananceDuration == 4)
                                                    {
                                                        int m = 0;
                                                        int y = 0;

                                                        for (var i = contractsInfo.MasterContract.From.Value.Year; i <= contractsInfo.MasterContract.To.Value.Year; i += y)
                                                        {
                                                            var assetDate = contractsInfo.MasterContract.From.Value.AddMonths(m);
                                                            if (contractsInfo.MasterContract.To.Value >= assetDate)
                                                            {

                                                                ManufacturerPMAsset timeObj = new ManufacturerPMAsset();
                                                                timeObj.AssetDetailId = asset.Id;
                                                                timeObj.HospitalId = asset.HospitalId;
                                                                timeObj.PMDate = assetDate;
                                                                timeObj.IsDone = false;
                                                                _context.ManufacturerPMAssets.Add(timeObj);
                                                                _context.SaveChanges();


                                                            }

                                                            m += 1;
                                                            y = m % 12 == 0 ? 1 : 0;
                                                        }
                                                    }
                                                }
                                                //else
                                                // لا توجد مده صيانه دوريه مسجله
                                                // periodic time is null
                                                else
                                                {
                                                    var Obj = new IndexUnScheduledManfacturerPMAssetVM.GetData();
                                                    Obj.AssetDetailId = asset.Id;
                                                    Obj.UnscheduledReason = "Asset Periodic Maintainance Time is null";
                                                    Obj.UnscheduledReasonAR = "لا توجد مده صيانه دوريه مسجله لهذا الاصل ";
                                                    unscheduledList.Add(Obj);

                                                }


                                            }

                                            //contract has ended 
                                            // the reason is contract has ended
                                            //انتهاء مده عقد الصيانه


                                            else
                                            {
                                                var Obj = new IndexUnScheduledManfacturerPMAssetVM.GetData();
                                                Obj.AssetDetailId = asset.Id;
                                                Obj.UnscheduledReason = "Asset Contract has been ended";
                                                Obj.UnscheduledReasonAR = "انتهاء مده عقد صيانه هذا الاصل";
                                                unscheduledList.Add(Obj);
                                            }

                                        }

                                    }




                                }
                            }
                            else
                            {
                                var Obj = new IndexUnScheduledManfacturerPMAssetVM.GetData();
                                Obj.AssetDetailId = asset.Id;
                                Obj.UnscheduledReason = "Asset have not contract";
                                Obj.UnscheduledReasonAR = "لا توجد عثد صيانه مسجل لهذا الاصل ";
                                unscheduledList.Add(Obj);
                            }
                        }


                    }


                    else
                    {
                        var Obj = new IndexUnScheduledManfacturerPMAssetVM.GetData();
                        Obj.AssetDetailId = asset.Id;
                        Obj.UnscheduledReason = "Asset has no Warranty or recorded contract";
                        Obj.UnscheduledReasonAR = "هذا الاصل ليس له مده ضمان او عقد مسجل ";
                        Obj.Barcode = asset.Barcode;
                        Obj.SerialNumber = asset.SerialNumber;
                        Obj.ModelNumber = asset.MasterAsset.ModelNumber;
                        Obj.BrandName = asset.MasterAsset.brand.Name;
                        Obj.BrandNameAR = asset.MasterAsset.brand.NameAr;
                        Obj.BrandCode = asset.MasterAsset.brand.Code;
                        Obj.AssetName = asset.MasterAsset.Name;
                        Obj.AssetNameAR = asset.MasterAsset.NameAr;
                        unscheduledList.Add(Obj);
                    }
                }
            }
            //this step to prevent repeated data to return uniqe data
            unscheduledList = unscheduledList.GroupBy(x => new { x.AssetDetailId, x.UnscheduledReason })
                .Select(x => x.First()).ToList();
            mainClass.Count = unscheduledList.Count();
            var unScheduledObj = unscheduledList.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            mainClass.Results = unScheduledObj;
            return mainClass;

        }


        public IndexManfacturerPMAssetVM GetAll(int pageNumber, int pageSize, string userId)
        {
            IndexManfacturerPMAssetVM mainClass = new IndexManfacturerPMAssetVM();
            List<IndexManfacturerPMAssetVM.GetData> list = new List<IndexManfacturerPMAssetVM.GetData>();
            var allAssetDetails = _context.ManufacturerPMAssets
                .Include(a => a.AssetDetail).Include(a => a.AssetDetail.MasterAsset)
                 .Include(a => a.AssetDetail.Department)
                 .Include(a => a.Supplier)
                .Include(a => a.AssetDetail.Hospital).ToList();


            foreach (var item in allAssetDetails)
            {

                IndexManfacturerPMAssetVM.GetData Obj = new IndexManfacturerPMAssetVM.GetData();
                Obj.Id = item.Id;
                Obj.BarCode = item.AssetDetail.Barcode;
                Obj.ModelNumber = item.AssetDetail.MasterAsset.ModelNumber;
                Obj.SerialNumber = item.AssetDetail.SerialNumber;
                Obj.DepartmentId = item.AssetDetail.Department != null ? item.AssetDetail.DepartmentId : 0;
                Obj.DepartmentName = item.AssetDetail.Department != null ? item.AssetDetail.Department.Name : "";
                Obj.DepartmentNameAr = item.AssetDetail.Department != null ? item.AssetDetail.Department.NameAr : "";

                if (item.Supplier != null)
                {
                    Obj.SupplierId = item.AgencyId;
                    Obj.SupplierName = item.Supplier.Name;
                    Obj.SupplierNameAr = item.Supplier.NameAr;
                }
                Obj.PMDate = item.PMDate;
                Obj.IsDone = item.IsDone != null ? (bool)item.IsDone : false;
                Obj.DoneDate = item.DoneDate;
                Obj.DueDate = item.DueDate;
                Obj.AssetName = item.AssetDetail.MasterAssetId > 0 ? item.AssetDetail.MasterAsset.Name : "";
                Obj.AssetNameAr = item.AssetDetail.MasterAssetId > 0 ? item.AssetDetail.MasterAsset.NameAr : "";
                list.Add(Obj);
            }
            mainClass.Count = list.Count();
            mainClass.CountDone = list.Where(a => a.IsDone == true).ToList().Count;
            mainClass.CountNotDone = list.Where(a => a.IsDone == false).ToList().Count;
            var assetTimeObjuestsPerPage = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            mainClass.Results = assetTimeObjuestsPerPage;
            return mainClass;
        }

        public List<CalendarManfacturerPMAssetTimeVM> GetAll(int hospitalId, string userId)
        {
            List<CalendarManfacturerPMAssetTimeVM> list = new List<CalendarManfacturerPMAssetTimeVM>();
            var lstAssetTimes = _context.ManufacturerPMAssets.Include(a => a.AssetDetail)
                .Include(a => a.Supplier).Include(r => r.AssetDetail.Hospital)
               .Include(r => r.AssetDetail.Department)
               .Include(r => r.AssetDetail.MasterAsset).Include(r => r.AssetDetail.MasterAsset.brand)
               .Where(a => a.HospitalId == hospitalId).ToList();


            if (lstAssetTimes.Count > 0)
            {
                foreach (var item in lstAssetTimes)
                {

                    string month = "";
                    string day = "";
                    string endmonth = "";
                    string endday = "";
                    // CalendarWNPMAssetTimeVM viewManfacturerPMAssetTimeObj = new CalendarWNPMAssetTimeVM();
                    CalendarManfacturerPMAssetTimeVM viewManfacturerPMAssetTimeObj = new CalendarManfacturerPMAssetTimeVM();
                    if (item.AssetDetail.Barcode == "200700889")
                    {
                        viewManfacturerPMAssetTimeObj.titleAr = item.AssetDetail.MasterAsset.NameAr.Trim();
                    }


                    viewManfacturerPMAssetTimeObj.Id = item.Id;
                    viewManfacturerPMAssetTimeObj.PMDate = item.PMDate;

                    if (item.PMDate.Value.Month < 10)
                        month = item.PMDate.Value.Month.ToString().PadLeft(2, '0');
                    else
                        month = item.PMDate.Value.Month.ToString();

                    if (item.PMDate.Value.Month < 10)
                        endmonth = item.PMDate.Value.Month.ToString().PadLeft(2, '0');
                    else
                        endmonth = item.PMDate.Value.Month.ToString();

                    if (item.PMDate.Value.Day < 10)
                        day = item.PMDate.Value.Day.ToString().PadLeft(2, '0');
                    else
                        day = item.PMDate.Value.Day.ToString();

                    if (item.PMDate.Value.Day < 10)
                        endday = (item.PMDate.Value.Day).ToString().PadLeft(2, '0');
                    else
                        endday = item.PMDate.Value.Day.ToString();


                    viewManfacturerPMAssetTimeObj.start = (item.PMDate.Value.Year + "-" + month + "-" + day).Trim();
                    viewManfacturerPMAssetTimeObj.end = (item.PMDate.Value.Year + "-" + endmonth + "-" + endday).Trim();
                    if (item.IsDone == true)
                    {
                        viewManfacturerPMAssetTimeObj.color = "#79be47";
                        viewManfacturerPMAssetTimeObj.textColor = "#fff";
                    }
                    else if (item.PMDate < DateTime.Today.Date && item.IsDone == false)
                    {
                        viewManfacturerPMAssetTimeObj.color = "#ff7578";
                        viewManfacturerPMAssetTimeObj.textColor = "#fff";
                    }


                    viewManfacturerPMAssetTimeObj.allDay = true;
                    viewManfacturerPMAssetTimeObj.DoneDate = item.DoneDate;
                    viewManfacturerPMAssetTimeObj.IsDone = item.IsDone;
                    viewManfacturerPMAssetTimeObj.Comment = item.Comment;
                    viewManfacturerPMAssetTimeObj.HospitalId = int.Parse(item.HospitalId.ToString());
                    viewManfacturerPMAssetTimeObj.AssetDetailId = int.Parse(item.AssetDetailId.ToString());
                    viewManfacturerPMAssetTimeObj.BarCode = item.AssetDetail.Barcode;
                    viewManfacturerPMAssetTimeObj.ModelNumber = item.AssetDetail.MasterAsset.ModelNumber;
                    viewManfacturerPMAssetTimeObj.SerialNumber = item.AssetDetail.SerialNumber;
                    viewManfacturerPMAssetTimeObj.MasterAssetId = (int)item.AssetDetail.MasterAssetId;
                    if (item.AssetDetail.MasterAsset != null)
                    {
                        if (item.AssetDetail.MasterAsset.Name.Contains('/'))
                        {
                            item.AssetDetail.MasterAsset.Name.Replace("/", "\\/");
                            viewManfacturerPMAssetTimeObj.AssetName = item.AssetDetail.MasterAsset.Name.Trim().Replace("/", "\\/");// item.AssetDetail.MasterAsset.Name;
                            viewManfacturerPMAssetTimeObj.AssetNameAr = item.AssetDetail.MasterAsset.NameAr.Trim().Replace("/", "\\/");// item.AssetDetail.MasterAsset.NameAr;


                            if (item.AssetDetail.Department != null)
                            {
                                viewManfacturerPMAssetTimeObj.title = viewManfacturerPMAssetTimeObj.AssetName + "-" + item.AssetDetail.Department.Name;
                                viewManfacturerPMAssetTimeObj.titleAr = viewManfacturerPMAssetTimeObj.AssetNameAr + "-" + item.AssetDetail.Department.NameAr;
                            }
                            else
                            {
                                viewManfacturerPMAssetTimeObj.title = viewManfacturerPMAssetTimeObj.AssetName;
                                viewManfacturerPMAssetTimeObj.titleAr = viewManfacturerPMAssetTimeObj.AssetNameAr;
                            }
                        }
                        else
                        {
                            viewManfacturerPMAssetTimeObj.AssetName = item.AssetDetail.MasterAsset.Name.Trim();
                            viewManfacturerPMAssetTimeObj.AssetNameAr = item.AssetDetail.MasterAsset.NameAr.Trim();

                            if (item.AssetDetail.Department != null)
                            {
                                viewManfacturerPMAssetTimeObj.title = viewManfacturerPMAssetTimeObj.AssetName + "-" + item.AssetDetail.Department.Name;
                                viewManfacturerPMAssetTimeObj.titleAr = viewManfacturerPMAssetTimeObj.AssetNameAr + "-" + item.AssetDetail.Department.NameAr;
                            }
                            else
                            {
                                viewManfacturerPMAssetTimeObj.title = viewManfacturerPMAssetTimeObj.AssetName;
                                viewManfacturerPMAssetTimeObj.titleAr = viewManfacturerPMAssetTimeObj.AssetNameAr;
                            }

                        }


                        if (item.AssetDetail.MasterAsset.Name.Contains('\n'))
                        {
                            viewManfacturerPMAssetTimeObj.AssetName = item.AssetDetail.MasterAsset.Name.Trim().Replace("\n", "");// item.AssetDetail.MasterAsset.Name;
                            viewManfacturerPMAssetTimeObj.AssetNameAr = item.AssetDetail.MasterAsset.NameAr.Trim().Replace("\n", "");// item.AssetDetail.MasterAsset.NameAr;


                            if (item.AssetDetail.Department != null)
                            {
                                viewManfacturerPMAssetTimeObj.title = viewManfacturerPMAssetTimeObj.AssetName + "-" + item.AssetDetail.Department.Name;
                                viewManfacturerPMAssetTimeObj.titleAr = viewManfacturerPMAssetTimeObj.AssetNameAr + "-" + item.AssetDetail.Department.NameAr;
                            }
                            else
                            {

                                viewManfacturerPMAssetTimeObj.title = viewManfacturerPMAssetTimeObj.AssetName;
                                viewManfacturerPMAssetTimeObj.titleAr = viewManfacturerPMAssetTimeObj.AssetNameAr;
                            }


                        }

                    }

                    List<IndexPMAssetTaskVM.GetData> lstTasks = new List<IndexPMAssetTaskVM.GetData>();
                    var lstAssetTasks = _context.PMAssetTasks.Where(a => a.MasterAssetId == item.AssetDetail.MasterAssetId).ToList();

                    if (lstTasks.Count > 0)
                    {
                        foreach (var tsk in lstAssetTasks)
                        {
                            IndexPMAssetTaskVM.GetData getDataObj = new IndexPMAssetTaskVM.GetData();
                            getDataObj.TaskName = tsk.Name;
                            getDataObj.TaskNameAr = tsk.NameAr;
                            getDataObj.MasterAssetId = (int)tsk.MasterAssetId;
                            lstTasks.Add(getDataObj);
                        }
                        viewManfacturerPMAssetTimeObj.ListMasterAssetTasks = lstTasks;
                    }
                    list.Add(viewManfacturerPMAssetTimeObj);


                }
            }

            return list;

        }

        public IndexManfacturerPMAssetVM GetAll(FilterManfacturerTimeVM filterObj, int pageNumber, int pageSize, string userId)
        {
            IndexManfacturerPMAssetVM mainClass = new IndexManfacturerPMAssetVM();
            List<IndexManfacturerPMAssetVM.GetData> list = new List<IndexManfacturerPMAssetVM.GetData>();
            List<IndexManfacturerPMAssetVM.GetData> listPerPage = new List<IndexManfacturerPMAssetVM.GetData>();
            var allAssetDetails = _context.ManufacturerPMAssets
                .Include(a => a.AssetDetail).Include(a => a.AssetDetail.MasterAsset)
                 .Include(a => a.AssetDetail.Department)
                 .Include(a => a.Supplier)
                .Include(a => a.AssetDetail.Hospital).ToList();

            var allAssetDetailsByQuarter = allAssetDetails.GroupBy(item => (Math.Ceiling(decimal.Parse(item.PMDate.Value.Date.AddMonths(6).Month.ToString()) / 3)));
            if (allAssetDetailsByQuarter.ToList().Count > 0)
            {
                foreach (var itm2 in allAssetDetailsByQuarter)
                {
                    if (filterObj.YearQuarter == itm2.Key)
                    {
                        mainClass.YearQuarter = int.Parse(itm2.Key.ToString());
                        foreach (var item in itm2)
                        {
                            IndexManfacturerPMAssetVM.GetData Obj = new IndexManfacturerPMAssetVM.GetData();
                            Obj.Id = item.Id;
                            Obj.BarCode = item.AssetDetail.Barcode;
                            Obj.ModelNumber = item.AssetDetail.MasterAsset.ModelNumber;
                            Obj.SerialNumber = item.AssetDetail.SerialNumber;
                            Obj.DepartmentId = item.AssetDetail.Department != null ? item.AssetDetail.DepartmentId : 0;
                            Obj.DepartmentName = item.AssetDetail.Department != null ? item.AssetDetail.Department.Name : "";
                            Obj.DepartmentNameAr = item.AssetDetail.Department != null ? item.AssetDetail.Department.NameAr : "";

                            if (item.Supplier != null)
                            {
                                Obj.SupplierId = item.AgencyId;
                                Obj.SupplierName = item.Supplier.Name;
                                Obj.SupplierNameAr = item.Supplier.NameAr;
                            }
                            Obj.PMDate = item.PMDate;
                            Obj.IsDone = item.IsDone != null ? (bool)item.IsDone : false;
                            Obj.DoneDate = item.DoneDate;
                            Obj.DueDate = item.DueDate;
                            Obj.AssetName = item.AssetDetail.MasterAssetId > 0 ? item.AssetDetail.MasterAsset.Name : "";
                            Obj.AssetNameAr = item.AssetDetail.MasterAssetId > 0 ? item.AssetDetail.MasterAsset.NameAr : "";

                            list.Add(Obj);
                        }
                    }
                }

                mainClass.TotalCount = list.Count;
                mainClass.CountDone = list.Where(a => a.IsDone == true).ToList().Count;
                mainClass.CountNotDone = list.Where(a => a.IsDone == false).ToList().Count;
                if (filterObj.IsDone == true)
                {
                    list = list.Where(a => a.IsDone == true).ToList();
                    listPerPage = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                }
                if (filterObj.IsDone == false)
                {
                    list = list.Where(a => a.IsDone == false).ToList();
                    listPerPage = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                }
                if (filterObj.IsDone == null)
                {
                    list = list.ToList();
                    listPerPage = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                }
               
  
                mainClass.Results = listPerPage;
                if (mainClass.Results.Count > 0)
                {
                    mainClass.Count = list.Count;
                }
                else
                {
                    mainClass.Count = 0;
                }
            }
            return mainClass;

        }

        public List<ForCheckManfacturerPMAssetsVM> GetAllForCheck()
        {
            List<ForCheckManfacturerPMAssetsVM> lstAssets = new List<ForCheckManfacturerPMAssetsVM>();
            var manfacturerPMAssets = _context.ManufacturerPMAssets.ToList();
            if (manfacturerPMAssets.Count() > 0)
            {
                foreach (var obj in manfacturerPMAssets)

                {
                    ForCheckManfacturerPMAssetsVM Obj = new ForCheckManfacturerPMAssetsVM();
                    Obj.AssetDetailId = obj.AssetDetailId;
                    Obj.PMDate = obj.PMDate;
                    lstAssets.Add(Obj);
                }
            }
            return lstAssets;







        }

        public ViewManfacturerPMAssetTimeVM GetAssetTimeById(int id)
        {
            ViewManfacturerPMAssetTimeVM viewWNPMAssetTimeObj = new ViewManfacturerPMAssetTimeVM();
            var lstAssetTimes = _context.ManufacturerPMAssets
               .Include(p => p.AssetDetail)
               .Include(p => p.Supplier)
               .Include(r => r.AssetDetail.Hospital)
                .Include(r => r.AssetDetail.Department)
               .Include(r => r.AssetDetail.MasterAsset)
                  .Include(r => r.AssetDetail.MasterAsset.brand).ToList();
            var theObj = lstAssetTimes.Where(e => e.Id == id).ToList();

            if (theObj.Count > 0)
            {
                ManufacturerPMAsset assetTimeObj = lstAssetTimes[0];
                viewWNPMAssetTimeObj.Id = assetTimeObj.Id;
                viewWNPMAssetTimeObj.PMDate = assetTimeObj.PMDate;
                viewWNPMAssetTimeObj.DoneDate = assetTimeObj.DoneDate;
                viewWNPMAssetTimeObj.IsDone = assetTimeObj.IsDone;
                viewWNPMAssetTimeObj.Comment = assetTimeObj.Comment;
                viewWNPMAssetTimeObj.HospitalId = int.Parse(assetTimeObj.HospitalId.ToString());
                viewWNPMAssetTimeObj.AssetDetailId = int.Parse(assetTimeObj.AssetDetailId.ToString());
                viewWNPMAssetTimeObj.BarCode = assetTimeObj.AssetDetail.Barcode;
                viewWNPMAssetTimeObj.ModelNumber = assetTimeObj.AssetDetail.MasterAsset.ModelNumber;
                viewWNPMAssetTimeObj.SerialNumber = assetTimeObj.AssetDetail.SerialNumber;
                viewWNPMAssetTimeObj.MasterAssetId = (int)assetTimeObj.AssetDetail.MasterAssetId;
                viewWNPMAssetTimeObj.AssetName = assetTimeObj.AssetDetail.MasterAsset.Name;
                viewWNPMAssetTimeObj.AssetNameAr = assetTimeObj.AssetDetail.MasterAsset.NameAr;
                viewWNPMAssetTimeObj.AssetDetailId = assetTimeObj.AssetDetailId != null ? (int)assetTimeObj.AssetDetailId : 0;
                viewWNPMAssetTimeObj.DepartmentName = assetTimeObj.AssetDetail.Department != null ? assetTimeObj.AssetDetail.Department.Name : "";
                viewWNPMAssetTimeObj.DepartmentNameAr = assetTimeObj.AssetDetail.Department != null ? assetTimeObj.AssetDetail.Department.NameAr : "";
                viewWNPMAssetTimeObj.SupplierName = assetTimeObj.Supplier != null ? assetTimeObj.Supplier.Name : "";
                viewWNPMAssetTimeObj.SupplierNameAr = assetTimeObj.Supplier != null ? assetTimeObj.Supplier.NameAr : "";
                viewWNPMAssetTimeObj.HospitalName = assetTimeObj.AssetDetail.Hospital != null ? assetTimeObj.AssetDetail.Hospital.Name : "";
                viewWNPMAssetTimeObj.HospitalNameAr = assetTimeObj.AssetDetail.Hospital != null ? assetTimeObj.AssetDetail.Hospital.NameAr : "";
                viewWNPMAssetTimeObj.BrandName = assetTimeObj.AssetDetail.MasterAsset.brand != null ? assetTimeObj.AssetDetail.MasterAsset.brand.Name : "";
                viewWNPMAssetTimeObj.BrandNameAr = assetTimeObj.AssetDetail.MasterAsset.brand != null ? assetTimeObj.AssetDetail.MasterAsset.brand.NameAr : "";


                List<IndexPMAssetTaskVM.GetData> lstTasks = new List<IndexPMAssetTaskVM.GetData>();
                var lstAssetTasks = _context.PMAssetTasks.Where(a => a.MasterAssetId == assetTimeObj.AssetDetail.MasterAssetId).ToList();
                if (lstAssetTasks.Count > 0)
                {
                    foreach (var item in lstAssetTasks)
                    {
                        IndexPMAssetTaskVM.GetData getDataObj = new IndexPMAssetTaskVM.GetData();
                        getDataObj.TaskName = item.Name;
                        getDataObj.TaskNameAr = item.NameAr;
                        getDataObj.MasterAssetId = (int)item.MasterAssetId;
                        lstTasks.Add(getDataObj);
                    }
                    viewWNPMAssetTimeObj.ListMasterAssetTasks = lstTasks;
                }
            }
            return viewWNPMAssetTimeObj;
        }

        public ManufacturerPMAsset GetById(int id)
        {
            return _context.ManufacturerPMAssets.Find(id);
        }

        public IndexManfacturerPMAssetVM SearchAssetTimes(SearchManfacturerAssetTimeVM searchObj, int pageNumber, int pageSize, string userId)
        {
            IndexManfacturerPMAssetVM mainClass = new IndexManfacturerPMAssetVM();
            List<IndexManfacturerPMAssetVM.GetData> list = new List<IndexManfacturerPMAssetVM.GetData>();

            ApplicationUser UserObj = new ApplicationUser();
            List<string> userRoleNames = new List<string>();
            var obj = _context.ApplicationUser.Where(a => a.Id == searchObj.UserId).ToList();
            if (obj.Count > 0)
            {
                UserObj = obj[0];
                var roleNames = (from userRole in _context.UserRoles
                                 join role in _context.Roles on userRole.RoleId equals role.Id
                                 where userRole.UserId == searchObj.UserId
                                 select role);
                foreach (var name in roleNames)
                {
                    userRoleNames.Add(name.Name);
                }
            }

            var allAssetDetails = _context.WNPMAssetTimes
                .Include(a => a.AssetDetail).Include(a => a.AssetDetail.MasterAsset)
                .Include(a => a.AssetDetail.MasterAsset.brand)
                 .Include(a => a.AssetDetail.Department)
                .Include(a => a.AssetDetail.Hospital)
                .Include(a => a.AssetDetail.Hospital.Governorate)
                   .Include(a => a.AssetDetail.Hospital.City)
                      .Include(a => a.AssetDetail.Hospital.Organization)
                         .Include(a => a.AssetDetail.Hospital.SubOrganization)
                .ToList();
            if (list.Count > 0)
            {
                if (UserObj.GovernorateId == 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
                {
                    allAssetDetails = allAssetDetails.ToList();
                }
                if (UserObj.GovernorateId > 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
                {
                    allAssetDetails = allAssetDetails.Where(t => t.AssetDetail.Hospital.GovernorateId == UserObj.GovernorateId).ToList();
                }
                if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId == 0)
                {
                    allAssetDetails = allAssetDetails.Where(t => t.AssetDetail.Hospital.GovernorateId == UserObj.GovernorateId && t.AssetDetail.Hospital.CityId == UserObj.CityId).ToList();
                }
                if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId > 0)
                {
                    allAssetDetails = allAssetDetails.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                }
                if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId == 0 && UserObj.HospitalId == 0)
                {
                    allAssetDetails = allAssetDetails.Where(t => t.AssetDetail.Hospital.OrganizationId == UserObj.OrganizationId).ToList();
                }
                if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId == 0)
                {
                    allAssetDetails = allAssetDetails.Where(t => t.AssetDetail.Hospital.OrganizationId == UserObj.OrganizationId && t.AssetDetail.Hospital.SubOrganizationId == UserObj.SubOrganizationId).ToList();
                }
                if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId > 0)
                {
                    allAssetDetails = allAssetDetails.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                }
            }


            if (allAssetDetails.Count > 0)
            {
                foreach (var itm in allAssetDetails)
                {
                    IndexManfacturerPMAssetVM.GetData item = new IndexManfacturerPMAssetVM.GetData();
                    item.Id = itm.Id;
                    item.PMDate = itm.PMDate;
                    item.DoneDate = itm.DoneDate;
                    item.DueDate = itm.DueDate;
                    item.IsDone = (bool)itm.IsDone;




                    if (itm.AssetDetail is not null)
                    {
                        item.BarCode = itm.AssetDetail.Barcode;
                        item.ModelNumber = itm.AssetDetail.MasterAsset.ModelNumber;
                        item.SerialNumber = itm.AssetDetail.SerialNumber;
                        item.DepartmentId = itm.AssetDetail.Department != null ? itm.AssetDetail.DepartmentId : 0;
                        item.DepartmentName = itm.AssetDetail.Department != null ? itm.AssetDetail.Department.Name : "";
                        item.DepartmentNameAr = itm.AssetDetail.Department != null ? itm.AssetDetail.Department.NameAr : "";

                        item.BrandId = itm.AssetDetail.MasterAsset.BrandId;
                        item.BrandName = itm.AssetDetail.MasterAsset.brand != null ? itm.AssetDetail.MasterAsset.brand.Name : "";
                        item.BrandNameAr = itm.AssetDetail.MasterAsset.brand != null ? itm.AssetDetail.MasterAsset.brand.NameAr : "";
                    }


                    item.PMDate = itm.PMDate;
                    item.IsDone = itm.IsDone != null ? (bool)itm.IsDone : false;
                    item.DoneDate = itm.DoneDate;
                    item.DueDate = itm.DueDate;
                    if (itm.AssetDetail is not null)
                    {
                        item.AssetName = itm.AssetDetail.MasterAssetId > 0 ? itm.AssetDetail.MasterAsset.Name : "";
                        item.AssetNameAr = itm.AssetDetail.MasterAssetId > 0 ? itm.AssetDetail.MasterAsset.NameAr : "";
                    }

                    list.Add(item);
                }
            }
            if (searchObj.BarCode != "")
                list = list.Where(a => a.BarCode == searchObj.BarCode).ToList();
            if (searchObj.SerialNumber != "")
                list = list.Where(a => a.SerialNumber == searchObj.SerialNumber).ToList();
            if (searchObj.ModelNumber != "")
                list = list.Where(a => a.ModelNumber == searchObj.ModelNumber).ToList();

            if (searchObj.BrandId != 0)
                list = list.Where(a => a.BrandId == searchObj.BrandId).ToList();
            else
                list = list.ToList();

            if (searchObj.DepartmentId != 0)
                list = list.Where(a => a.DepartmentId == searchObj.DepartmentId).ToList();
            else
                list = list.ToList();


            var assetTimeObjuestsPerPage = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            mainClass.Results = assetTimeObjuestsPerPage;
            mainClass.Count = list.Count();
            mainClass.CountDone = list.Where(a => a.IsDone == true).ToList().Count;
            mainClass.CountNotDone = list.Where(a => a.IsDone == false).ToList().Count;


            return mainClass;
        }

        public IndexManfacturerPMAssetVM SortManfacturerAssetTimes(SortManfacturerPMAssetTimeVM sortObj, int pageNumber, int pageSize, string userId)
        {

            IndexManfacturerPMAssetVM mainClass = new IndexManfacturerPMAssetVM();
            List<IndexManfacturerPMAssetVM.GetData> list = new List<IndexManfacturerPMAssetVM.GetData>();

            ApplicationUser UserObj = new ApplicationUser();
            List<string> userRoleNames = new List<string>();
            var obj = _context.ApplicationUser.Where(a => a.Id == sortObj.UserId).ToList();
            if (obj.Count > 0)
            {
                UserObj = obj[0];
                var roleNames = (from userRole in _context.UserRoles
                                 join role in _context.Roles on userRole.RoleId equals role.Id
                                 where userRole.UserId == sortObj.UserId
                                 select role);
                foreach (var name in roleNames)
                {
                    userRoleNames.Add(name.Name);
                }
            }
            var allAssetDetails = _context.ManufacturerPMAssets
                .Include(a => a.AssetDetail).Include(a => a.AssetDetail.MasterAsset)
                .Include(a => a.AssetDetail.MasterAsset.brand)
                 .Include(a => a.AssetDetail.Department)
                .Include(a => a.AssetDetail.Hospital)
                .Include(a => a.AssetDetail.Hospital.Governorate)
                   .Include(a => a.AssetDetail.Hospital.City)
                      .Include(a => a.AssetDetail.Hospital.Organization)
                         .Include(a => a.AssetDetail.Hospital.SubOrganization)
                .ToList();
            if (list.Count > 0)
            {
                if (UserObj.GovernorateId == 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
                {
                    allAssetDetails = allAssetDetails.ToList();
                }
                if (UserObj.GovernorateId > 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
                {
                    allAssetDetails = allAssetDetails.Where(t => t.AssetDetail.Hospital.GovernorateId == UserObj.GovernorateId).ToList();
                }
                if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId == 0)
                {
                    allAssetDetails = allAssetDetails.Where(t => t.AssetDetail.Hospital.GovernorateId == UserObj.GovernorateId && t.AssetDetail.Hospital.CityId == UserObj.CityId).ToList();
                }
                if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId > 0)
                {
                    allAssetDetails = allAssetDetails.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                }
                if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId == 0 && UserObj.HospitalId == 0)
                {
                    allAssetDetails = allAssetDetails.Where(t => t.AssetDetail.Hospital.OrganizationId == UserObj.OrganizationId).ToList();
                }
                if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId == 0)
                {
                    allAssetDetails = allAssetDetails.Where(t => t.AssetDetail.Hospital.OrganizationId == UserObj.OrganizationId && t.AssetDetail.Hospital.SubOrganizationId == UserObj.SubOrganizationId).ToList();
                }
                if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId > 0)
                {
                    allAssetDetails = allAssetDetails.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                }
            }


            if (allAssetDetails.Count > 0)
            {
                foreach (var itm in allAssetDetails)
                {
                    IndexManfacturerPMAssetVM.GetData item = new IndexManfacturerPMAssetVM.GetData();
                    item.Id = itm.Id;
                    item.PMDate = itm.PMDate;
                    item.DoneDate = itm.DoneDate;
                    item.DueDate = itm.DueDate;
                    item.IsDone = (bool)itm.IsDone;
                    if (itm.AssetDetail != null)
                    {
                        if (itm.AssetDetail.Barcode != null)
                        {
                            item.BarCode = itm.AssetDetail.Barcode;

                        }
                        if (itm.AssetDetail.SerialNumber != null)
                        {
                            item.SerialNumber = itm.AssetDetail.SerialNumber;
                        }
                        if (itm.AssetDetail.DepartmentId != null)
                        {
                            item.DepartmentId = itm.AssetDetail.DepartmentId;
                        }
                        if (itm.AssetDetail.MasterAsset != null)
                        {
                            if (itm.AssetDetail.MasterAsset.ModelNumber != null)
                            {
                                item.ModelNumber = itm.AssetDetail.MasterAsset.ModelNumber;
                            }
                        }
                        if (itm.AssetDetail.Department != null)
                        {
                            item.DepartmentName = itm.AssetDetail.Department.Name;
                            item.DepartmentNameAr = itm.AssetDetail.Department.NameAr;
                        }
                        if (itm.AssetDetail.MasterAsset != null)
                        {
                            item.BrandId = itm.AssetDetail.MasterAsset.BrandId;
                            item.BrandName = itm.AssetDetail.MasterAsset.brand.Name;
                            item.BrandNameAr = itm.AssetDetail.MasterAsset.brand.NameAr;
                        }
                    }



                    item.PMDate = itm.PMDate;
                    item.IsDone = itm.IsDone != null ? (bool)itm.IsDone : false;
                    item.DoneDate = itm.DoneDate;
                    item.DueDate = itm.DueDate;
                    if (itm.AssetDetail != null)
                    {
                        if (itm.AssetDetail.MasterAsset != null)
                        {
                            item.AssetName = itm.AssetDetail.MasterAssetId > 0 ? itm.AssetDetail.MasterAsset.Name : "";
                            item.AssetNameAr = itm.AssetDetail.MasterAssetId > 0 ? itm.AssetDetail.MasterAsset.NameAr : "";
                        }
                    }

                    list.Add(item);
                }
            }







            #region Old Sorting Code By ekram

            #region SerialNumber

            //if (sortObj.SerialNumber != "")
            //{

            //    if (sortObj.StrBarCode != "")
            //    {
            //        if (sortObj.SortStatus == "descending")
            //        {
            //            list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.BarCode).ToList();
            //        }
            //        else
            //        {
            //            list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderBy(d => d.BarCode).ToList();
            //        }
            //    }
            //    if (sortObj.StrSerialNumber != "")
            //    {
            //        if (sortObj.SortStatus == "descending")
            //        {
            //            list = list.Where(b => b.SerialNumber.Contains(sortObj.StrSerialNumber)).OrderByDescending(d => d.SerialNumber).ToList();
            //        }
            //        else
            //        {
            //            list = list.Where(b => b.SerialNumber.Contains(sortObj.StrSerialNumber)).OrderBy(d => d.SerialNumber).ToList();
            //        }
            //    }
            //    if (sortObj.StrModelNumber != "")
            //    {
            //        if (sortObj.SortStatus == "descending")
            //            list = list.Where(b => b.ModelNumber.Contains(sortObj.StrModelNumber)).OrderByDescending(d => d.ModelNumber).ToList();
            //        else
            //            list = list.Where(b => b.ModelNumber.Contains(sortObj.StrModelNumber)).OrderBy(d => d.ModelNumber).ToList();
            //    }
            //    if (sortObj.PMDate != "")
            //    {
            //        if (sortObj.SortStatus == "descending")
            //            list = list.OrderByDescending(d => d.PMDate).ToList();
            //        else
            //            list = list.OrderBy(d => d.PMDate).ToList();
            //    }

            //    else
            //    {
            //        if (sortObj.SortStatus == "descending")
            //            list = list.OrderByDescending(d => d.SerialNumber).ToList();
            //        else
            //            list = list.OrderBy(d => d.SerialNumber).ToList();
            //    }
            //}

            #endregion

            #region BarCode
            //if (sortObj.BarCode != "")
            //{
            //    if (sortObj.StrBarCode != "")
            //    {
            //        if (sortObj.SortStatus == "descending")
            //        {
            //            list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.BarCode).ToList();
            //        }
            //        else
            //        {
            //            list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderBy(d => d.BarCode).ToList();
            //        }
            //    }
            //    if (sortObj.BarCode != "")
            //    {
            //        if (sortObj.SortStatus == "descending")
            //        {
            //            list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.BarCode).ToList();
            //        }
            //        else
            //        {
            //            list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderBy(d => d.BarCode).ToList();
            //        }
            //    }
            //    if (sortObj.StrSerialNumber != "")
            //    {
            //        if (sortObj.SortStatus == "descending")
            //        {
            //            list = list.Where(b => b.SerialNumber.Contains(sortObj.StrSerialNumber)).OrderByDescending(d => d.SerialNumber).ToList();
            //        }
            //        else
            //        {
            //            list = list.Where(b => b.SerialNumber.Contains(sortObj.StrSerialNumber)).OrderBy(d => d.SerialNumber).ToList();
            //        }
            //    }
            //    if (sortObj.StrModelNumber != "")
            //    {
            //        if (sortObj.SortStatus == "descending")
            //            list = list.Where(b => b.ModelNumber.Contains(sortObj.StrModelNumber)).OrderByDescending(d => d.ModelNumber).ToList();
            //        else
            //            list = list.Where(b => b.ModelNumber.Contains(sortObj.StrModelNumber)).OrderBy(d => d.ModelNumber).ToList();
            //    }
            //    else
            //    {
            //        if (sortObj.SortStatus == "descending")
            //            list = list.OrderByDescending(d => d.BarCode).ToList();
            //        else
            //            list = list.OrderBy(d => d.BarCode).ToList();
            //    }
            //} 
            #endregion

            #region ModelNumber
            //if (sortObj.ModelNumber != "")
            //{
            //    if (sortObj.StrBarCode != "")
            //    {
            //        if (sortObj.SortStatus == "descending")
            //        {
            //            list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.BarCode).ToList();
            //        }
            //        else
            //        {
            //            list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderBy(d => d.BarCode).ToList();
            //        }
            //    }
            //    if (sortObj.StrSerialNumber != "")
            //    {
            //        if (sortObj.SortStatus == "descending")
            //        {
            //            list = list.Where(b => b.SerialNumber.Contains(sortObj.StrSerialNumber)).OrderByDescending(d => d.SerialNumber).ToList();
            //        }
            //        else
            //        {
            //            list = list.Where(b => b.SerialNumber.Contains(sortObj.StrSerialNumber)).OrderBy(d => d.SerialNumber).ToList();
            //        }
            //    }

            //    if (sortObj.BarCode != "")
            //    {
            //        if (sortObj.SortStatus == "descending")
            //        {
            //            list = list.Where(b => b.BarCode.Contains(sortObj.BarCode)).OrderByDescending(d => d.BarCode).ToList();
            //        }
            //        else
            //        {
            //            list = list.Where(b => b.BarCode.Contains(sortObj.BarCode)).OrderBy(d => d.BarCode).ToList();
            //        }
            //    }
            //    if (sortObj.SerialNumber != "")
            //    {
            //        if (sortObj.SortStatus == "descending")
            //        {
            //            list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialNumber)).OrderByDescending(d => d.SerialNumber).ToList();
            //        }
            //        else
            //        {
            //            list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialNumber)).OrderBy(d => d.SerialNumber).ToList();
            //        }
            //    }
            //    else
            //    {
            //        if (sortObj.SortStatus == "descending")
            //            list = list.OrderByDescending(d => d.ModelNumber).ToList();
            //        else
            //            list = list.OrderBy(d => d.ModelNumber).ToList();
            //    }
            //}

            #endregion

            #region AssetName

            //if (sortObj.AssetName != "")
            //{
            //    if (sortObj.StrBarCode != "")
            //    {
            //        if (sortObj.SortStatus == "descending")
            //        {
            //            list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.BarCode).ToList();
            //        }
            //        else
            //        {
            //            list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderBy(d => d.BarCode).ToList();
            //        }
            //    }
            //    if (sortObj.SerialNumber != "")
            //    {
            //        if (sortObj.SortStatus == "descending")
            //        {
            //            list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialNumber)).OrderByDescending(d => d.SerialNumber).ToList();
            //        }
            //        else
            //        {
            //            list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialNumber)).OrderBy(d => d.SerialNumber).ToList();
            //        }
            //    }
            //    if (sortObj.ModelNumber != "")
            //    {
            //        if (sortObj.SortStatus == "descending")
            //            list = list.Where(b => b.ModelNumber.Contains(sortObj.ModelNumber)).OrderByDescending(d => d.ModelNumber).ToList();
            //        else
            //            list = list.Where(b => b.ModelNumber.Contains(sortObj.ModelNumber)).OrderBy(d => d.ModelNumber).ToList();
            //    }

            //    else
            //    {
            //        if (sortObj.SortStatus == "descending")
            //            list = list.OrderByDescending(d => d.AssetName).ToList();
            //        else
            //            list = list.OrderBy(d => d.AssetName).ToList();
            //    }
            //}

            #endregion


            #region AssetNameAr

            //if (sortObj.AssetNameAr != "")
            //{
            //    if (sortObj.BarCode != "")
            //    {
            //        if (sortObj.SortStatus == "descending")
            //        {
            //            list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.BarCode).ToList();
            //        }
            //        else
            //        {
            //            list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderBy(d => d.BarCode).ToList();
            //        }
            //    }
            //    if (sortObj.SerialNumber != "")
            //    {
            //        if (sortObj.SortStatus == "descending")
            //        {
            //            list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialNumber)).OrderByDescending(d => d.SerialNumber).ToList();
            //        }
            //        else
            //        {
            //            list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialNumber)).OrderBy(d => d.SerialNumber).ToList();
            //        }
            //    }
            //    if (sortObj.ModelNumber != "")
            //    {
            //        if (sortObj.SortStatus == "descending")
            //            list = list.Where(b => b.ModelNumber.Contains(sortObj.ModelNumber)).OrderByDescending(d => d.ModelNumber).ToList();
            //        else
            //            list = list.Where(b => b.ModelNumber.Contains(sortObj.ModelNumber)).OrderBy(d => d.ModelNumber).ToList();
            //    }
            //    else
            //    {
            //        if (sortObj.SortStatus == "descending")
            //            list = list.OrderByDescending(d => d.AssetNameAr).ToList();
            //        else
            //            list = list.OrderBy(d => d.AssetNameAr).ToList();
            //    }
            //}
            #endregion


            #region PMDate
            //if (sortObj.PMDate != "")
            //{
            //    if (sortObj.BarCode != "")
            //    {
            //        if (sortObj.SortStatus == "descending")
            //        {
            //            list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.BarCode).ToList();
            //        }
            //        else
            //        {
            //            list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderBy(d => d.BarCode).ToList();
            //        }
            //    }
            //    if (sortObj.SerialNumber != "")
            //    {
            //        if (sortObj.SortStatus == "descending")
            //        {
            //            list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialNumber)).OrderByDescending(d => d.SerialNumber).ToList();
            //        }
            //        else
            //        {
            //            list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialNumber)).OrderBy(d => d.SerialNumber).ToList();
            //        }
            //    }
            //    if (sortObj.ModelNumber != "")
            //    {
            //        if (sortObj.SortStatus == "descending")
            //            list = list.Where(b => b.ModelNumber.Contains(sortObj.ModelNumber)).OrderByDescending(d => d.ModelNumber).ToList();
            //        else
            //            list = list.Where(b => b.ModelNumber.Contains(sortObj.ModelNumber)).OrderBy(d => d.ModelNumber).ToList();
            //    }
            //    else
            //    {
            //        if (sortObj.SortStatus == "descending")
            //            list = list.OrderByDescending(d => d.PMDate).ToList();
            //        else
            //            list = list.OrderBy(d => d.PMDate).ToList();
            //    }
            //}

            #endregion


            #region DoneDate
            //if (sortObj.DoneDate != "")
            //{
            //    if (sortObj.BarCode != "")
            //    {
            //        if (sortObj.SortStatus == "descending")
            //        {
            //            list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.BarCode).ToList();
            //        }
            //        else
            //        {
            //            list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderBy(d => d.BarCode).ToList();
            //        }
            //    }
            //    if (sortObj.SerialNumber != "")
            //    {
            //        if (sortObj.SortStatus == "descending")
            //        {
            //            list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialNumber)).OrderByDescending(d => d.SerialNumber).ToList();
            //        }
            //        else
            //        {
            //            list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialNumber)).OrderBy(d => d.SerialNumber).ToList();
            //        }
            //    }
            //    if (sortObj.ModelNumber != "")
            //    {
            //        if (sortObj.SortStatus == "descending")
            //            list = list.Where(b => b.ModelNumber.Contains(sortObj.ModelNumber)).OrderByDescending(d => d.ModelNumber).ToList();
            //        else
            //            list = list.Where(b => b.ModelNumber.Contains(sortObj.ModelNumber)).OrderBy(d => d.ModelNumber).ToList();
            //    }
            //    else
            //    {
            //        if (sortObj.SortStatus == "descending")
            //            list = list.OrderByDescending(d => d.DoneDate).ToList();
            //        else
            //            list = list.OrderBy(d => d.DoneDate).ToList();
            //    }
            //} 
            #endregion


            #region Duedate
            //if (sortObj.DueDate != "")
            //{
            //    if (sortObj.BarCode != "")
            //    {
            //        if (sortObj.SortStatus == "descending")
            //        {
            //            list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.BarCode).ToList();
            //        }
            //        else
            //        {
            //            list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderBy(d => d.BarCode).ToList();
            //        }
            //    }
            //    if (sortObj.SerialNumber != "")
            //    {
            //        if (sortObj.SortStatus == "descending")
            //        {
            //            list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialNumber)).OrderByDescending(d => d.SerialNumber).ToList();
            //        }
            //        else
            //        {
            //            list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialNumber)).OrderBy(d => d.SerialNumber).ToList();
            //        }
            //    }
            //    if (sortObj.ModelNumber != "")
            //    {
            //        if (sortObj.SortStatus == "descending")
            //            list = list.Where(b => b.ModelNumber.Contains(sortObj.ModelNumber)).OrderByDescending(d => d.ModelNumber).ToList();
            //        else
            //            list = list.Where(b => b.ModelNumber.Contains(sortObj.ModelNumber)).OrderBy(d => d.ModelNumber).ToList();
            //    }
            //    else
            //    {
            //        if (sortObj.SortStatus == "descending")
            //            list = list.OrderByDescending(d => d.DueDate).ToList();
            //        else
            //            list = list.OrderBy(d => d.DueDate).ToList();
            //    }
            //} 
            #endregion


            #region IsDone
            //if (sortObj.IsDone != "")
            //{
            //    if (sortObj.BarCode != "")
            //    {
            //        if (sortObj.SortStatus == "descending")
            //        {
            //            list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.BarCode).ToList();
            //        }
            //        else
            //        {
            //            list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderBy(d => d.BarCode).ToList();
            //        }
            //    }
            //    if (sortObj.SerialNumber != "")
            //    {
            //        if (sortObj.SortStatus == "descending")
            //        {
            //            list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialNumber)).OrderByDescending(d => d.SerialNumber).ToList();
            //        }
            //        else
            //        {
            //            list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialNumber)).OrderBy(d => d.SerialNumber).ToList();
            //        }
            //    }
            //    if (sortObj.ModelNumber != "")
            //    {
            //        if (sortObj.SortStatus == "descending")
            //            list = list.Where(b => b.ModelNumber.Contains(sortObj.ModelNumber)).OrderByDescending(d => d.ModelNumber).ToList();
            //        else
            //            list = list.Where(b => b.ModelNumber.Contains(sortObj.ModelNumber)).OrderBy(d => d.ModelNumber).ToList();
            //    }
            //    else
            //    {
            //        if (sortObj.SortStatus == "descending")
            //            list = list.OrderByDescending(d => d.IsDone).ToList();
            //        else
            //            list = list.OrderBy(d => d.IsDone).ToList();
            //    }
            //} 
            #endregion



            #endregion

            // sorting Area
            if (sortObj.AssetName != "")
            {
                if (sortObj.SortStatus == "descending")
                {
                    list = list.Where(b => b.AssetName.Contains(sortObj.AssetName)).OrderByDescending(d => d.BarCode).ToList();
                }
                else
                {
                    list = list.Where(b => b.AssetName.Contains(sortObj.AssetName)).OrderBy(d => d.BarCode).ToList();
                }

            }

            else if (sortObj.AssetNameAr != "")
            {

                if (sortObj.SortStatus == "descending")
                {
                    list = list.OrderByDescending(d => d.AssetNameAr).ToList();
                }
                else
                {
                    list = list.OrderBy(d => d.AssetNameAr).ToList();
                }

            }

            else if (sortObj.StrBarCode != "")
            {

                if (sortObj.SortStatus == "descending")
                {
                    list = list.OrderByDescending(d => d.BarCode).ToList();
                }
                else
                {
                    list = list.OrderBy(d => d.BarCode).ToList();
                }

            }

            else if (sortObj.StrSerialNumber != "")
            {


                if (sortObj.SortStatus == "descending")
                {
                    list = list.OrderByDescending(d => d.SerialNumber).ToList();
                }
                else
                {
                    list = list.OrderBy(d => d.SerialNumber).ToList();
                }
            }

            else if (sortObj.StrModelNumber != "")

            {

                if (sortObj.SortStatus == "descending")
                {
                    list = list.OrderByDescending(d => d.ModelNumber).ToList();
                }
                else
                {
                    list = list.OrderBy(d => d.ModelNumber).ToList();
                }

            }


            else if (sortObj.IsDone != "")
            {

                if (sortObj.SortStatus == "descending")
                {
                    list = list.OrderByDescending(d => d.IsDone).ToList();
                }
                else
                {
                    list = list.OrderBy(d => d.IsDone).ToList();
                }

            }


            else if (sortObj.DoneDate != "")
            {

                if (sortObj.SortStatus == "descending")
                {
                    list = list.OrderByDescending(d => d.DoneDate).ToList();
                }
                else
                {
                    list = list.OrderBy(d => d.DoneDate).ToList();
                }

            }

            else if (sortObj.DueDate != "")
            {

                if (sortObj.SortStatus == "descending")
                {
                    list = list.OrderByDescending(d => d.DueDate).ToList();
                }
                else
                {
                    list = list.OrderBy(d => d.DueDate).ToList();
                }
            }

            else if (sortObj.PMDate != "")
            {
                if (sortObj.SortStatus == "descending")
                {
                    list = list.OrderByDescending(d => d.PMDate).ToList();
                }
                else
                {
                    list = list.OrderBy(d => d.PMDate).ToList();
                }

            }

            var assetTimeObjuestsPerPage = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            mainClass.Results = assetTimeObjuestsPerPage;
            mainClass.Count = list.Count();
            mainClass.CountDone = list.Where(a => a.IsDone == true).ToList().Count;
            mainClass.CountNotDone = list.Where(a => a.IsDone == false).ToList().Count;
            return mainClass;





        }

        public int Update(ManufacturerPMAsset model)
        {
            try
            {
                var timeObj = _context.ManufacturerPMAssets.Find(model.Id);

                timeObj.HospitalId = model.HospitalId;
                timeObj.AssetDetailId = model.AssetDetailId;
                timeObj.PMDate = model.PMDate;

                if (model.strDueDate != null)
                    timeObj.DueDate = DateTime.Parse(model.strDueDate);//.AddDays(1);
                timeObj.IsDone = model.IsDone;


                if (model.strDoneDate != null)
                    timeObj.DoneDate = DateTime.Parse(model.strDoneDate);


                timeObj.SysDoneDate = DateTime.Today.Date;


                timeObj.Comment = model.Comment;
                if (model.AgencyId > 0)
                    timeObj.AgencyId = model.AgencyId;


                _context.Entry(timeObj).State = EntityState.Modified;
                _context.SaveChanges();
                return timeObj.Id;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return 0;
        }
    }
}
