using Asset.ViewModels.StockTakingHospitalVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
    public interface IStockTakingHospitalService
    {
        StockTakingHospitalVM GetById(int id);
        IEnumerable<IndexStockTakingHospitalVM.GetData> GetAll();
    }
}
