using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.ManufacturerPMAssetVM;
using Asset.ViewModels.WNPMAssetTimes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
    public class ManufacturerPMAssetService : IManufacturerPMAssetService
    {
        private IUnitOfWork _unitOfWork;
        public ManufacturerPMAssetService(IUnitOfWork unitOfWork)
        {
            _unitOfWork= unitOfWork;
        }

        public IndexUnScheduledManfacturerPMAssetVM CreateManfacturerAssetTimes(int pageNumber, int pageSize)
        {
            return _unitOfWork.ManufacturerPMAssetRepository.CreateManfacturerAssetTimes(pageNumber, pageSize);
        }

        //IndexUnScheduledManfacturerPMAssetVM CreateManfacturerAssetTimes(int pageNumber, int pageSize)
        //{
        //    return _unitOfWork.ManufacturerPMAssetRepository.CreateManfacturerAssetTimes(pageNumber,pageSize);
        //}

        public IndexManfacturerPMAssetVM GetAll(int pageNumber, int pageSize, string userId)
        {
            return _unitOfWork.ManufacturerPMAssetRepository.GetAll( pageNumber, pageSize,  userId);

        }

        public List<CalendarManfacturerPMAssetTimeVM> GetAll(int hospitalId, string userId)
        {
            return _unitOfWork.ManufacturerPMAssetRepository.GetAll(hospitalId, userId);
        }

        public IndexManfacturerPMAssetVM GetAll(FilterManfacturerTimeVM filterObj, int pageNumber, int pageSize, string userId)
        {
            return _unitOfWork.ManufacturerPMAssetRepository.GetAll(filterObj, pageNumber, pageSize, userId);
        }

        public List<ForCheckManfacturerPMAssetsVM> GetAllForCheck()
        {
            return _unitOfWork.ManufacturerPMAssetRepository.GetAllForCheck();
        }

        public ViewManfacturerPMAssetTimeVM GetAssetTimeById(int id)
        {
            return _unitOfWork.ManufacturerPMAssetRepository.GetAssetTimeById(id);
        }

        public ManufacturerPMAsset GetById(int id)
        {
           return _unitOfWork.ManufacturerPMAssetRepository.GetById(id);
        }

        public IndexManfacturerPMAssetVM SearchAssetTimes(SearchManfacturerAssetTimeVM searchObj, int pageNumber, int pageSize, string userId)
        {
            return _unitOfWork.ManufacturerPMAssetRepository.SearchAssetTimes(searchObj,pageNumber,pageSize,userId);
        }

        public IndexManfacturerPMAssetVM SortManfacturerAssetTimes(SortManfacturerPMAssetTimeVM sortObj, int pageNumber, int pageSize, string userId)
        {
           return _unitOfWork.ManufacturerPMAssetRepository.SortManfacturerAssetTimes(sortObj,pageNumber,pageSize,userId);
        }

        public int Update(ManufacturerPMAsset model)
        {
            return _unitOfWork.ManufacturerPMAssetRepository.Update(model); 
        }
    }
}
