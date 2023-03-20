using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.AssetStatusTransactionVM;
using Asset.ViewModels.StockTakingScheduleVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class StockTakingScheduleRepository : IStockTakingScheduleRepository
    {
        private ApplicationDbContext _context;

        public StockTakingScheduleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Add(CreateStockTakingScheduleVM model)
        {

            try
            {
                if (model != null)
                {
                    StockTakingSchedule stockTakingScheduleObj = new StockTakingSchedule();
                    stockTakingScheduleObj.STCode = model.STCode;
                    stockTakingScheduleObj.UserId = model.UserId;
                    stockTakingScheduleObj.StartDate = model.StartDate;
                    stockTakingScheduleObj.EndDate = model.EndDate;
                    stockTakingScheduleObj.CreationDate = model.CreationDate;


                    _context.StockTakingSchedules.Add(stockTakingScheduleObj);
                    _context.SaveChanges();
                    var stscheduleId = stockTakingScheduleObj.Id;
                    if (model.ListHospitalIds.Count > 0)
                    {
                        foreach (var hospitalId in model.ListHospitalIds)
                        {
                            StockTakingHospital stockTakingHospitalObj = new StockTakingHospital();
                            stockTakingHospitalObj.HospitalId = hospitalId;
                            stockTakingHospitalObj.STSchedulesId = stscheduleId;
                            _context.StockTakingHospitals.Add(stockTakingHospitalObj);
                            _context.SaveChanges();
                        }
                    }
                    return stockTakingScheduleObj.Id;
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return 0;
        }

        public int Delete(int id)
        {
            var stockTakingScheduleObj = _context.StockTakingSchedules.Find(id);


            _context.StockTakingSchedules.Remove(stockTakingScheduleObj);
            return _context.SaveChanges();
        }

        public IndexStockTakingScheduleVM GetAllWithPaging(int pageNumber, int pageSize)
        {
            IndexStockTakingScheduleVM mainClass = new IndexStockTakingScheduleVM();
            List<IndexStockTakingScheduleVM.GetData> list = new List<IndexStockTakingScheduleVM.GetData>();
            var lsStockTakingSchedules = _context.StockTakingSchedules.Include(a => a.ApplicationUser).ToList();



            foreach (var schdule in lsStockTakingSchedules)
            {
                var lsStockTakingHospitals = _context.StockTakingHospitals.Include(a => a.Hospital)
                    .Where(a => a.STSchedulesId == schdule.Id).ToList();

                IndexStockTakingScheduleVM.GetData item = new IndexStockTakingScheduleVM.GetData();
                item.Id = schdule.Id;
                item.STCode = schdule.STCode;
                item.StartDate = schdule.StartDate;
                item.EndDate = schdule.EndDate;
                item.CreationDate = schdule.CreationDate;
                item.UserName = schdule.ApplicationUser.UserName;

                item.RelatedHospitals = _context.StockTakingHospitals.Include(a => a.Hospital)
                    .Where(a => a.STSchedulesId == schdule.Id).ToList().Select(hospital => new RelatedHospital()
                    {
                        Name = hospital.Hospital.Name,
                        NameAr = hospital.Hospital.NameAr,
                    }).ToList();
                list.Add(item);
            }

            mainClass.Count = list.Count;
            mainClass.Results = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return mainClass;
        }

        public IndexStockTakingScheduleVM.GetData GetById(int id)
        {
            var result = new IndexStockTakingScheduleVM.GetData();
            List<IndexStockTakingScheduleVM.GetData> list = new List<IndexStockTakingScheduleVM.GetData>();
            var lsStockTakingSchedules = _context.StockTakingSchedules.Include(a => a.ApplicationUser).ToList();



            foreach (var schdule in lsStockTakingSchedules)
            {
                var lsStockTakingHospitals = _context.StockTakingHospitals.Include(a => a.Hospital)
                    .Where(a => a.STSchedulesId == schdule.Id).ToList();

                IndexStockTakingScheduleVM.GetData item = new IndexStockTakingScheduleVM.GetData();
                item.Id = schdule.Id;
                item.STCode = schdule.STCode;
                item.StartDate = schdule.StartDate;
                item.EndDate = schdule.EndDate;
                item.CreationDate = schdule.CreationDate;
                item.UserName = schdule.ApplicationUser.UserName;

                item.RelatedHospitals = _context.StockTakingHospitals.Include(a => a.Hospital)
                    .Where(a => a.STSchedulesId == schdule.Id).ToList().Select(hospital => new RelatedHospital()
                    {
                        Name = hospital.Hospital.Name,
                        NameAr = hospital.Hospital.NameAr,
                    }).ToList();


                //item.HospitalName = lsStockTakingHospitals[0].Hospital.Name;
                //item.HospitalNameAr = lsStockTakingHospitals[0].Hospital.NameAr;
                list.Add(item);
            }
            if (list.Count > 0)
            {
                result = list.Where(a => a.Id == id).ToList().FirstOrDefault();
                result.RelatedHospitals = _context.StockTakingHospitals.Include(a => a.Hospital)
                    .Where(a => a.STSchedulesId == result.Id).ToList().Select(hospital => new RelatedHospital()
                    {
                        Name = hospital.Hospital.Name,
                        NameAr = hospital.Hospital.NameAr,
                    }).ToList();
            }

            return result;


        }

        public GenerateStockScheduleTakingNumberVM GenerateStockScheduleTakingNumber()
        {
            GenerateStockScheduleTakingNumberVM generatedNumber = new GenerateStockScheduleTakingNumberVM();
            string str = "ST";
            var lstIds = _context.StockTakingSchedules.ToList();
            if (lstIds.Count > 0)
            {
                var code = lstIds.LastOrDefault().Id;
                generatedNumber.OutNumber = str + (code + 1);

            }
            else
            {
                generatedNumber.OutNumber = str + 1;
            }
            return generatedNumber;
        }

        public IEnumerable<IndexStockTakingScheduleVM.GetData> GetAll()
        {
            return _context.StockTakingSchedules
                 .Include(a => a.ApplicationUser)
                 .ToList().Select(item => new IndexStockTakingScheduleVM.GetData
                 {
                     Id = item.Id,
                     STCode = item.STCode,
                     StartDate = item.StartDate,
                     EndDate = item.EndDate,
                     CreationDate = item.CreationDate,
                     UserName = item.ApplicationUser.UserName
                 });
        }

        public IndexStockTakingScheduleVM SearchStockTakingSchedule(SearchStockTakingScheduleVM searchObj, int pageNumber, int pageSize)
        {
            IndexStockTakingScheduleVM mainClass = new IndexStockTakingScheduleVM();
            List<IndexStockTakingScheduleVM.GetData> list = new List<IndexStockTakingScheduleVM.GetData>();
            List<StockTakingSchedule> lstAssetStockTakings = new List<StockTakingSchedule>();
            lstAssetStockTakings = _context.StockTakingSchedules
                                 .Include(a => a.ApplicationUser)
                                 .OrderBy(a => a.StartDate).ToList().ToList();

            if (lstAssetStockTakings.Count > 0)
            {

                string setstartday, setstartmonth, setendday, setendmonth = "";
                DateTime startingFrom = new DateTime();
                DateTime endingTo = new DateTime();
                if (searchObj.Start == "")
                {
                    searchObj.StartDate = DateTime.Parse("01/01/1900");
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
                    searchObj.EndDate = DateTime.Today.Date;
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
                    lstAssetStockTakings = lstAssetStockTakings.Where(a => a.StartDate.Value.Date >= startingFrom.Date && a.EndDate.Value.Date <= endingTo.Date).ToList();
                }
            }

            foreach (var detail in lstAssetStockTakings)
            {
                IndexStockTakingScheduleVM.GetData item = new IndexStockTakingScheduleVM.GetData();
                item.Id = detail.Id;
                item.STCode = detail.STCode;
                item.StartDate = detail.StartDate;
                item.EndDate = detail.EndDate;
                item.CreationDate = detail.CreationDate;
                item.UserName = detail.ApplicationUser.UserName;
                list.Add(item);
            }

            mainClass.Count = list.Count();
            var requestsPerPage = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            mainClass.Results = requestsPerPage;
            return mainClass;
        }

        public IndexStockTakingScheduleVM SortStockTakingSchedule(int page, int pageSize, SortStockTakingScheduleVM sortObj)
        {
            IndexStockTakingScheduleVM mainClass = new IndexStockTakingScheduleVM();
            List<IndexStockTakingScheduleVM.GetData> list = new List<IndexStockTakingScheduleVM.GetData>();
            List<StockTakingSchedule> lstAssetStockTakings = new List<StockTakingSchedule>();
            lstAssetStockTakings = _context.StockTakingSchedules
                                 .Include(a => a.ApplicationUser)
                                 .OrderBy(a => a.StartDate).ToList().ToList();

            if (lstAssetStockTakings.Count > 0)
            {
                foreach (var detail in lstAssetStockTakings)
                {
                    IndexStockTakingScheduleVM.GetData item = new IndexStockTakingScheduleVM.GetData();
                    item.Id = detail.Id;
                    item.STCode = detail.STCode;
                    item.StartDate = detail.StartDate;
                    item.EndDate = detail.EndDate;
                    item.CreationDate = detail.CreationDate;
                    item.UserName = detail.ApplicationUser.UserName;
                    list.Add(item);
                }
            }

            if (sortObj.STCode != "")
            {
                if (sortObj.SortStatus == "descending")
                    list = list.OrderByDescending(d => d.STCode).ToList();
                else
                    list = list.OrderBy(d => d.STCode).ToList();
            }
            else if (sortObj.STCode == "")
            {
                list = list.OrderBy(d => d.STCode).ToList();
            }


            mainClass.Count = list.Count();
            var lstSchedule = list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            mainClass.Results = lstSchedule;
            return mainClass;
        }
    }


}



