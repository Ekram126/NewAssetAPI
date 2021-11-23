using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.OriginVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
   public class PMAssetTimeService : IPMAssetTimeService
    {
        private IUnitOfWork _unitOfWork;

        public PMAssetTimeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Add(PMAssetTime timeObj)
        {
            return _unitOfWork.PMAssetTimeRepository.Add(timeObj);
        }

        public int Delete(int id)
        {
            return _unitOfWork.PMAssetTimeRepository.Delete(id);
        }

        public IEnumerable<PMAssetTime> GetAll()
        {
            return _unitOfWork.PMAssetTimeRepository.GetAll();
        }

        public PMAssetTime GetById(int id)
        {
            return _unitOfWork.PMAssetTimeRepository.GetById(id);
        }

        public IEnumerable<PMAssetTime> GetDateByAssetDetailId(int assetDetailId)
        {
            return _unitOfWork.PMAssetTimeRepository.GetDateByAssetDetailId(assetDetailId);
        }

        public int Update(PMAssetTime timeObj)
        {
            return _unitOfWork.PMAssetTimeRepository.Update(timeObj);
        }
    }
}
