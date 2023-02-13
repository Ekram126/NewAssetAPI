using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.StockTakingHospitalVM;
using Asset.ViewModels.StockTakingScheduleVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class StockTakingHospitalRepository : IStockTakingHospitalRepository
    {
        private ApplicationDbContext _context;

        public StockTakingHospitalRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<IndexStockTakingHospitalVM.GetData> GetAll()
        {
            return _context.StockTakingHospitals.ToList().Select(item => new IndexStockTakingHospitalVM.GetData
            {
                HospitalId = item.HospitalId,
                Id = item.Id,
                STSchedulesId = item.STSchedulesId
            });

        }

        public List<RelatedHospital> GetHospitalsByScheduleId(int scheduleId)
        {
            var lstRelatedHospitals = new List<RelatedHospital>();
            if (scheduleId != 0)
            {
                lstRelatedHospitals = _context.StockTakingHospitals.Include(a => a.Hospital)
               .Where(a => a.STSchedulesId == scheduleId).ToList().Select(hospital => new RelatedHospital()
               {
                   Name = hospital.Hospital.Name,
                   NameAr = hospital.Hospital.NameAr,
               }).ToList();

            }

            return lstRelatedHospitals;

        }
    }
}
