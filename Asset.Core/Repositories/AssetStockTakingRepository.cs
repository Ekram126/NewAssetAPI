using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.AssetStockTakingVM;
using Asset.ViewModels.StockTakingScheduleVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public IndexAssetStockTakingVM GetAllWithPaging(int pageNumber, int pageSize)
        {
            IndexAssetStockTakingVM mainClass = new IndexAssetStockTakingVM();
            var lstAssetStockTaking = _context.AssetStockTakings.Include(ww => ww.ApplicationUser)
                 .Include(ww => ww.Hospital).Include(ww => ww.AssetDetail.MasterAsset).Include(ww => ww.AssetDetail)
                 .ToList().Select(item => new IndexAssetStockTakingVM.GetData()
                 {
                     UserName = item.ApplicationUser.UserName,
                     HospitalName = item.Hospital.Name,
                     Latitude = item.Latitude,
                     Longtitude = item.Longtitude,
                     AssetName = item.AssetDetail.MasterAsset.Name,
                     BarCode = item.AssetDetail.Barcode,
                 });
            mainClass.Count = lstAssetStockTaking.Count();
            mainClass.Results = lstAssetStockTaking.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return mainClass;
        }

        public IndexAssetStockTakingVM SearchInAssetStockTakings(SearchAssetStockTakingVM searchObj, int page, int pageSize)
        {
            IndexAssetStockTakingVM mainClass = new IndexAssetStockTakingVM();
            List<IndexAssetStockTakingVM.GetData> list = new List<IndexAssetStockTakingVM.GetData>();
            List<AssetStockTaking> lstAssetStockTakings = new List<AssetStockTaking>();
            lstAssetStockTakings = _context.AssetStockTakings
                                        .Include(a => a.StockTakingSchedule)
                                        .Include(a => a.AssetDetail)
                                        .Include(a => a.AssetDetail.Department)
                                        .Include(a => a.AssetDetail.MasterAsset)
                                        .Include(a => a.AssetDetail.MasterAsset.brand)
                                        .Include(a => a.AssetDetail.Supplier)
                                        .Include(a => a.AssetDetail.Hospital)
                                        .Include(a => a.ApplicationUser)
                                        .OrderBy(a => a.CaptureDate).ToList().ToList();

            if (lstAssetStockTakings.Count > 0)
            {

                string setstartday, setstartmonth, setendday, setendmonth = "";
                DateTime startingFrom = new DateTime();
                DateTime endingTo = new DateTime();
                if (searchObj.Start == "")
                {
                  //  searchObj.StartDate = DateTime.Parse("01/01/1900");
                }
                else
                {
                    searchObj.StartDate = DateTime.Parse(searchObj.Start.ToString());
                    var startyear = searchObj.StartDate.Value.Year;
                    var startmonth = searchObj.StartDate.Value.Month;
                    var startday = searchObj.StartDate.Value.Day;
                    if (startday < 10)
                        setstartday = searchObj.StartDate.Value.Day.ToString().PadLeft(2, '0');
                    else
                        setstartday = searchObj.StartDate.Value.Day.ToString();

                    if (startmonth < 10)
                        setstartmonth = searchObj.StartDate.Value.Month.ToString().PadLeft(2, '0');
                    else
                        setstartmonth = searchObj.StartDate.Value.Month.ToString();

                    var sDate = startyear + "/" + setstartmonth + "/" + setstartday;
                    startingFrom = DateTime.Parse(sDate);
                }

                if (searchObj.End == "")
                {
                   /// searchObj.EndDate = DateTime.Today.Date;
                }
                else
                {
                    searchObj.EndDate = DateTime.Parse(searchObj.End.ToString());
                    var endyear = searchObj.EndDate.Value.Year;
                    var endmonth = searchObj.EndDate.Value.Month;
                    var endday = searchObj.EndDate.Value.Day;
                    if (endday < 10)
                        setendday = searchObj.EndDate.Value.Day.ToString().PadLeft(2, '0');
                    else
                        setendday = searchObj.EndDate.Value.Day.ToString();
                    if (endmonth < 10)
                        setendmonth = searchObj.EndDate.Value.Month.ToString().PadLeft(2, '0');
                    else
                        setendmonth = searchObj.EndDate.Value.Month.ToString();
                    var eDate = endyear + "/" + setendmonth + "/" + setendday;
                    endingTo = DateTime.Parse(eDate);
                }

                if (searchObj.Start != "" && searchObj.End != "")
                {
                    lstAssetStockTakings = lstAssetStockTakings.Where(a => a.CaptureDate.Value.Date >= startingFrom.Date && a.CaptureDate.Value.Date <= endingTo.Date).ToList();
                }
            }

            foreach (var detail in lstAssetStockTakings)
            {
                IndexAssetStockTakingVM.GetData item = new IndexAssetStockTakingVM.GetData();
                item.Id = detail.Id;
                item.AssetName = detail.AssetDetail.MasterAsset != null ? detail.AssetDetail.MasterAsset.Name : "";
                item.AssetNameAr = detail.AssetDetail.MasterAsset != null ? detail.AssetDetail.MasterAsset.NameAr : "";

                item.BrandId = detail.AssetDetail.MasterAsset.brand != null ? detail.AssetDetail.MasterAsset.brand.Id : 0;
                item.BrandName = detail.AssetDetail.MasterAsset.brand != null ? detail.AssetDetail.MasterAsset.brand.Name : "";
                item.BrandNameAr = detail.AssetDetail.MasterAsset.brand != null ? detail.AssetDetail.MasterAsset.brand.NameAr : "";

                item.HospitalId = detail.AssetDetail.Hospital != null ? detail.AssetDetail.Hospital.Id : 0;
                item.HospitalName = detail.AssetDetail.Hospital != null ? detail.AssetDetail.Hospital.Name : "";
                item.HospitalNameAr = detail.AssetDetail.Hospital != null ? detail.AssetDetail.Hospital.NameAr : "";


                item.DepartmentId = detail.AssetDetail.Department != null ? detail.AssetDetail.Department.Id : 0;
                item.DepartmentName = detail.AssetDetail.Department != null ? detail.AssetDetail.Department.Name : "";
                item.DepartmentNameAr = detail.AssetDetail.Department != null ? detail.AssetDetail.Department.NameAr : "";

                item.BarCode = detail.AssetDetail != null ? detail.AssetDetail.Barcode : "";
                item.SerialNumber = detail.AssetDetail != null ? detail.AssetDetail.SerialNumber : "";
                item.ModelNumber = detail.AssetDetail != null ? detail.AssetDetail.MasterAsset.ModelNumber : "";

                item.CaptureDate = detail.CaptureDate;
                item.UserName = detail.ApplicationUser.UserName;


                item.GovernorateId = detail.AssetDetail.Hospital.Governorate != null ? detail.AssetDetail.Hospital.GovernorateId : 0;
                item.CityId = detail.AssetDetail.Hospital.City != null ? detail.AssetDetail.Hospital.CityId : 0;



                list.Add(item);
            }

            if (list.Count > 0)
            {
                if (searchObj.ModelNumber != "")
                {
                    list = list.Where(b => b.ModelNumber == searchObj.ModelNumber).ToList();
                }
                else
                    list = list.ToList();



                if (searchObj.BrandId != 0)
                {
                    list = list.Where(a => a.BrandId == searchObj.BrandId).ToList();
                }
                else
                    list = list.ToList();


                if (searchObj.MasterAssetId != 0)
                {
                    list = list.Where(a => a.MasterAssetId == searchObj.MasterAssetId).ToList();
                }
                else
                    list = list.ToList();

                if (searchObj.DepartmentId != 0)
                {
                    list = list.Where(a => a.DepartmentId == searchObj.DepartmentId).ToList();
                }
                else
                    list = list.ToList();


                if (searchObj.SerialNumber != "")
                {
                    list = list.Where(b => b.SerialNumber == searchObj.SerialNumber).ToList();
                }
                else
                    list = list.ToList();

                if (searchObj.BarCode != "")
                {
                    list = list.Where(b => b.BarCode == searchObj.BarCode).ToList();
                }
                else
                    list = list.ToList();

                if (searchObj.HospitalId != 0)
                {
                    list = list.Where(b => b.HospitalId == searchObj.HospitalId).ToList();
                }
                else
                    list = list.ToList();


            }





            mainClass.Count = list.Count();
            var requestsPerPage = list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            mainClass.Results = requestsPerPage;
            return mainClass;
        }

        public IndexAssetStockTakingVM ListAssetStockTakings(int pageNumber, int pageSize)
        {
            IndexAssetStockTakingVM mainClass = new IndexAssetStockTakingVM();
            List<IndexAssetStockTakingVM.GetData> list = new List<IndexAssetStockTakingVM.GetData>();
            List<AssetStockTaking> lstAssetStockTakings = new List<AssetStockTaking>();
            lstAssetStockTakings = _context.AssetStockTakings
                                        .Include(a => a.StockTakingSchedule)
                                        .Include(a => a.AssetDetail)
                                        .Include(a => a.AssetDetail.Department)
                                        .Include(a => a.AssetDetail.MasterAsset)
                                        .Include(a => a.AssetDetail.MasterAsset.brand)
                                        .Include(a => a.AssetDetail.Supplier)
                                        .Include(a => a.AssetDetail.Hospital)
                                        .Include(a => a.ApplicationUser)
                                        .OrderBy(a => a.CaptureDate).ToList().ToList();

            if (lstAssetStockTakings.Count > 0)
            {


                foreach (var detail in lstAssetStockTakings)
                {
                    IndexAssetStockTakingVM.GetData item = new IndexAssetStockTakingVM.GetData();
                    item.Id = detail.Id;
                    item.AssetName = detail.AssetDetail.MasterAsset != null ? detail.AssetDetail.MasterAsset.Name : "";
                    item.AssetNameAr = detail.AssetDetail.MasterAsset != null ? detail.AssetDetail.MasterAsset.NameAr : "";

                    item.BrandId = detail.AssetDetail.MasterAsset.brand != null ? detail.AssetDetail.MasterAsset.brand.Id : 0;
                    item.BrandName = detail.AssetDetail.MasterAsset.brand != null ? detail.AssetDetail.MasterAsset.brand.Name : "";
                    item.BrandNameAr = detail.AssetDetail.MasterAsset.brand != null ? detail.AssetDetail.MasterAsset.brand.NameAr : "";


                    item.HospitalName = detail.AssetDetail.Hospital != null ? detail.AssetDetail.Hospital.Name : "";
                    item.HospitalNameAr = detail.AssetDetail.Hospital != null ? detail.AssetDetail.Hospital.NameAr : "";


                    item.DepartmentName = detail.AssetDetail.Department != null ? detail.AssetDetail.Department.Name : "";
                    item.DepartmentNameAr = detail.AssetDetail.Department != null ? detail.AssetDetail.Department.NameAr : "";

                    item.BarCode = detail.AssetDetail != null ? detail.AssetDetail.Barcode : "";
                    item.SerialNumber = detail.AssetDetail != null ? detail.AssetDetail.SerialNumber : "";
                    item.CaptureDate = detail.CaptureDate;
                    item.UserName = detail.ApplicationUser.UserName;
                    list.Add(item);
                }
                mainClass.Count = list.Count();
                var assetSTPerPage = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                mainClass.Results = assetSTPerPage;
            }

            return mainClass;
        }


    }
}

