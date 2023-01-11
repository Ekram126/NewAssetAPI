using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.StockTakingHospitalVM;
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
            _context= context;
        }

        public IEnumerable<IndexStockTakingHospitalVM.GetData> GetAll()
        {
            return _context.StockTakingHospitals.ToList().Select(item => new IndexStockTakingHospitalVM.GetData
            {
                HospitalId= item.HospitalId,
                Id= item.Id,
                STSchedulesId=item.STSchedulesId
            });
        }

        public StockTakingHospitalVM GetById(int id)
        {
            return _context.StockTakingHospitals.Where(ww => ww.HospitalId == id).Select(item =>
            new StockTakingHospitalVM
            {
                HospitalId= item.HospitalId,
                Id= item.Id,
                STSchedulesId=item.STSchedulesId
                

            }).First();
        }

     


    }
}
