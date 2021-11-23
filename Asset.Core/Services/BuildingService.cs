using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using System.Collections.Generic;


namespace Asset.Core.Services
{
    public class BuildingService : IBuildingService
    {

        private IUnitOfWork _unitOfWork;

        public BuildingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public int Add(Building buildingVM)
        {
            _unitOfWork.BuildingRepository.Add(buildingVM);
            return _unitOfWork.CommitAsync();
        }

        public int Delete(int id)
        {
            var BuildingObj = _unitOfWork.BuildingRepository.GetById(id);
            _unitOfWork.BuildingRepository.Delete(BuildingObj.Id);
            _unitOfWork.CommitAsync();

            return BuildingObj.Id;
        }

        public IEnumerable<Building> GetAll()
        {
            return _unitOfWork.BuildingRepository.GetAll();
        }


        public Building GetById(int id)
        {
            return _unitOfWork.BuildingRepository.GetById(id);
        }

        public int Update(Building buildingVM)
        {
            _unitOfWork.BuildingRepository.Update(buildingVM);
            _unitOfWork.CommitAsync();
            return buildingVM.Id;
        }


        public IEnumerable<Building> GetAllBuildingsByHospitalId(int hospitalId)
        {
            return _unitOfWork.BuildingRepository.GetAllBuildingsByHospitalId(hospitalId);
        }

       

      
    }
}
