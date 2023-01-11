using Asset.Models;
using Asset.ViewModels.StockTakingScheduleVM;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Repositories
{
    public interface IStockTakingScheduleRepository
    {
        IEnumerable<IndexStockTakingScheduleVM.GetData> GetAll();

        IndexStockTakingScheduleVM GetAllWithPaging(int pageNumber,int pageSize);

        // EditStockTakingScheduleVM GetById(int id);
        int Add(CreateStockTakingScheduleVM createStockTakingScheduleObj);
        int Delete(int id);
        object GetById(int id);



        
    }
}
