using Asset.Domain.Repositories;
using Asset.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
   public class PMAssetTaskScheduleRepository: IPMAssetTaskScheduleRepository
    {
        private ApplicationDbContext _context;


        public PMAssetTaskScheduleRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public int Add(PMAssetTaskSchedule model)
        {
            PMAssetTaskSchedule PMAssetTaskScheduleObj = new PMAssetTaskSchedule();
            try
            {
                if (model != null)
                {
                    //PMAssetTaskScheduleObj.Id = model.Id;
                    PMAssetTaskScheduleObj.PMAssetTaskId = model.PMAssetTaskId;
                    PMAssetTaskScheduleObj.PMAssetTimeId = model.PMAssetTimeId;
                    PMAssetTaskScheduleObj.HospitalId = model.HospitalId;
                    _context.PMAssetTaskSchedules.Add(PMAssetTaskScheduleObj);
                    _context.SaveChanges();


                    return PMAssetTaskScheduleObj.Id;
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return PMAssetTaskScheduleObj.Id;
        }

        public List<PMAssetTaskSchedule> GetAll()
        {
            return _context.PMAssetTaskSchedules.ToList();
        }
    }
}
