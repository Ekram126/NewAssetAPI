using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.AssetStockTakingVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
    public class AssetStockTakingService:IAssetStockTakingService
    {
        private IUnitOfWork _unitOfWork;

        public AssetStockTakingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        public int Add(CreateAssetStockTakingVM createAssetStockTakingVM)
        {
            return _unitOfWork.AssetStockTackingRepository.Add(createAssetStockTakingVM);
        }

        public IEnumerable<IndexAssetStockTakingVM.GetData> GetAll()
        {
            return _unitOfWork.AssetStockTackingRepository.GetAll();
        }

        public IndexAssetStockTakingVM GetAllWithPaging(int page, int pageSize)
        {
            return _unitOfWork.AssetStockTackingRepository.GetAllWithPaging(page, pageSize);
        }



    }
}
