using Asset.ViewModels.StockTakingHospitalVM;
using Asset.ViewModels.StockTakingScheduleVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Repositories
{
    public interface IStockTakingHospitalRepository
    {
        //StockTakingHospitalVM GetById(int id);
        IEnumerable<IndexStockTakingHospitalVM.GetData> GetAll();
        List<RelatedHospital> GetHospitalsByScheduleId(int scheduleId);


    }
}
