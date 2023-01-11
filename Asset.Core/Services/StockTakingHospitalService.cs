using Asset.Domain;
using Asset.Domain.Services;
using Asset.ViewModels.StockTakingHospitalVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
    public class StockTakingHospitalService : IStockTakingHospitalService
    {

        private IUnitOfWork _unitOfWork;


        public StockTakingHospitalService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<IndexStockTakingHospitalVM.GetData> GetAll()
        {
            //return _unitOfWork.StockTakingScheduleRepository.GetAll();
            return _unitOfWork.StockTakingHospitalRepository.GetAll();
        }


        //public EditAssetPeriorityVM GetById(int id)
        //{
        //    return _unitOfWork.AssetPeriorityRepository.GetById(id);
        //}
        public StockTakingHospitalVM GetById(int id)
        {
            return _unitOfWork.StockTakingHospitalRepository.GetById(id);
            
        }
    }
}
