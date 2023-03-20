using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.StockTakingScheduleVM;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
    public class StockTakingScheduleService : IStockTakingScheduleService
    {
        private IUnitOfWork _unitOfWork;

        public StockTakingScheduleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        

        public int Delete(int id)
        {
            _unitOfWork.StockTakingScheduleRepository.Delete(id);
    

            return 1;
        }
       
        public int Add(CreateStockTakingScheduleVM stockTakingScheduleObj)
        {
            return _unitOfWork.StockTakingScheduleRepository.Add(stockTakingScheduleObj);
        }
        public IndexStockTakingScheduleVM.GetData GetById(int id)
        {
            return _unitOfWork.StockTakingScheduleRepository.GetById(id);
        }
        public IndexStockTakingScheduleVM GetAllWithPaging(int pageNumber, int pageSize)
        {
            return _unitOfWork.StockTakingScheduleRepository.GetAllWithPaging( pageNumber, pageSize);
        }

        public GenerateStockScheduleTakingNumberVM GenerateStockScheduleTakingNumber()
        {
            return _unitOfWork.StockTakingScheduleRepository.GenerateStockScheduleTakingNumber();
        }

        public IEnumerable<IndexStockTakingScheduleVM.GetData> GetAll()
        {
            return _unitOfWork.StockTakingScheduleRepository.GetAll();
        }

        public IndexStockTakingScheduleVM SearchStockTakingSchedule(SearchStockTakingScheduleVM searchObj, int pageNumber, int pageSize)
        {
            return _unitOfWork.StockTakingScheduleRepository.SearchStockTakingSchedule(searchObj, pageNumber, pageSize);
        }

        public IndexStockTakingScheduleVM SortStockTakingSchedule(int page, int pageSize, SortStockTakingScheduleVM sortObj)
        {
            return _unitOfWork.StockTakingScheduleRepository.SortStockTakingSchedule(page, pageSize, sortObj);
        }
    }
}
