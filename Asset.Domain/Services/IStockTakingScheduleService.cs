using Asset.Models;
using Asset.ViewModels.BrandVM;
using Asset.ViewModels.StockTakingScheduleVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
    public interface IStockTakingScheduleService
    {
        IEnumerable<IndexStockTakingScheduleVM.GetData> GetAll();
        IndexStockTakingScheduleVM GetAllWithPaging(int pageNumber, int pageSize);
        int Add(CreateStockTakingScheduleVM StockTakingScheduleObj);
        int Delete(int id);
        IndexStockTakingScheduleVM.GetData GetById(int id);
        GenerateStockScheduleTakingNumberVM GenerateStockScheduleTakingNumber();

        IndexStockTakingScheduleVM SearchStockTakingSchedule(SearchStockTakingScheduleVM searchObj, int pageNumber, int pageSize);
        IndexStockTakingScheduleVM SortStockTakingSchedule(int page, int pageSize, SortStockTakingScheduleVM sortObj);
    }
}
