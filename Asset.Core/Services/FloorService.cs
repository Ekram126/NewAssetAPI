using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using System.Collections.Generic;


namespace Asset.Core.Services
{
    public class FloorService : IFloorService
    {

        private IUnitOfWork _unitOfWork;

        public FloorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public int Add(Floor FloorVM)
        {
            _unitOfWork.FloorRepository.Add(FloorVM);
            return _unitOfWork.CommitAsync();
        }

        public int Delete(int id)
        {
            var FloorObj = _unitOfWork.FloorRepository.GetById(id);
            _unitOfWork.FloorRepository.Delete(FloorObj.Id);
            _unitOfWork.CommitAsync();

            return FloorObj.Id;
        }

        public IEnumerable<Floor> GetAll()
        {
            return _unitOfWork.FloorRepository.GetAll();
        }


        public Floor GetById(int id)
        {
            return _unitOfWork.FloorRepository.GetById(id);
        }

        public int Update(Floor FloorVM)
        {
            _unitOfWork.FloorRepository.Update(FloorVM);
            _unitOfWork.CommitAsync();
            return FloorVM.Id;
        }


        public IEnumerable<Floor> GetAllFloorsBybuildingId(int buildId)
        {
            return _unitOfWork.FloorRepository.GetAllFloorsBybuildingId(buildId);
        }

       

      
    }
}
