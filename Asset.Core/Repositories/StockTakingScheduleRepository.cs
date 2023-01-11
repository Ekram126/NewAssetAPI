using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.AssetStatusTransactionVM;
using Asset.ViewModels.StockTakingScheduleVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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



        //public int Delete(int id)
        //{
        //    var StockTakingScheduleObj = _context.ExternalFixes.Find(id);


        //            _context.ExternalFixes.Remove(StockTakingScheduleObj);
        //            return _context.SaveChanges();
        //        }
        //    }
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
                    UserName = item.ApplicationUser.UserName,


                });


        }

        //public IEnumerable<IndexStockTakingScheduleVM.GetData> GetAll()
        //{
        //    return _context.StockTakingSchedules.Include(a => a.AssetDetail)
        //        .Include(a => a.AssetDetail.MasterAsset)
        //         .Include(a => a.AssetDetail.MasterAsset.brand)

        //        .ToList().Select(item => new IndexStockTakingScheduleVM.GetData
        //        {
        //            Id = item.Id,
        //            STCode = item.STCode,
        //            StartDate = item.StartDate,
        //            EndDate = item.EndDate,
        //            HospitalName = item.Hospital.Name,
        //            HospitalNameAr = item.Hospital.NameAr,
        //            CreationDate = item.CreationDate,
        //            UserName = item.ApplicationUser.UserName,


        //        });
        //}


        //public object GetById(int id)
        //{
        //    throw new NotImplementedException();
        //}
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
                    foreach (var hospitalId in model.ListHospitalIds)
                    {
                        StockTakingHospital stockTakingHospitalObj = new StockTakingHospital();
                        stockTakingHospitalObj.HospitalId = hospitalId;
                        stockTakingHospitalObj.STSchedulesId = stscheduleId;
                        _context.StockTakingSchedules.Add(stockTakingScheduleObj);
                        _context.SaveChanges();
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





        //public void Update(EditExternalFixVM editExternalFixVMObj)
        //{


        //    try
        //    {
        //        var externalFixObj = _context.StockTakingScheduleRepositories.Find(editExternalFixVMObj.Id);
        //        externalFixObj.ComingDate = editExternalFixVMObj.ComingDate;
        //        externalFixObj.ComingNotes = editExternalFixVMObj.ComingNotes;
        //        _context.Entry(externalFixObj).State = EntityState.Modified;
        //        _context.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        string msg = ex.Message;
        //    }
        //}



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
            var lsStockTakingSchedules = _context.StockTakingSchedules.Include(a=>a.ApplicationUser).ToList();
            


            foreach (var schdule in lsStockTakingSchedules)
            {
                var lsStockTakingHospitals = _context.StockTakingHospitals.Include(a=>a.Hospital)
                    .Where(a => a.STSchedulesId == schdule.Id).ToList();

                IndexStockTakingScheduleVM.GetData item = new IndexStockTakingScheduleVM.GetData();
                item.Id = schdule.Id;
                item.STCode = schdule.STCode;
                item.StartDate = schdule.StartDate;
                item.EndDate = schdule.EndDate;
                item.CreationDate = schdule.CreationDate;
                item.UserName = schdule.ApplicationUser.UserName;
                item.HospitalName = lsStockTakingHospitals[0].Hospital.Name;
                item.HospitalNameAr = lsStockTakingHospitals[0].Hospital.NameAr;
                list.Add(item);
            }

            mainClass.Count = list.Count;
            mainClass.Results = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return mainClass;
        }

        public object GetById(int id)
        {
            throw new NotImplementedException();
        }
    }


}



