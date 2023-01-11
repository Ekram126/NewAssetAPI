using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.StockTakingScheduleVM;
using System;
using System.Collections.Generic;
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

   
        public IEnumerable<IndexStockTakingScheduleVM.GetData> GetAll()
        {
            return _unitOfWork.StockTakingScheduleRepository.GetAll();
        }

        public int Add(CreateStockTakingScheduleVM stockTakingScheduleObj)
        {
            return _unitOfWork.StockTakingScheduleRepository.Add(stockTakingScheduleObj);

        }



        public object GetById(int id)
        {
            throw new NotImplementedException();
        }
        public IndexStockTakingScheduleVM GetAllWithPaging(int pageNumber, int pageSize)
        {
            return _unitOfWork.StockTakingScheduleRepository.GetAllWithPaging( pageNumber, pageSize);
        }
    }
}
