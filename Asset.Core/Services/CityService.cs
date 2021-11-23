using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.CityVM;
using Asset.ViewModels.RoleCategoryVM;
using System.Collections.Generic;


namespace Asset.Core.Services
{
    public class CityService : ICityService
    {

        private IUnitOfWork _unitOfWork;

        public CityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public int Add(CreateCityVM cityVM)
        {
            _unitOfWork.CityRepository.Add(cityVM);
            return _unitOfWork.CommitAsync();
        }

        public int Delete(int id)
        {
            var CityObj = _unitOfWork.CityRepository.GetById(id);
            _unitOfWork.CityRepository.Delete(CityObj.Id);
            _unitOfWork.CommitAsync();

            return CityObj.Id;
        }

        public IEnumerable<IndexCityVM.GetData> GetAll()
        {
            return _unitOfWork.CityRepository.GetAll();
        }

        public IEnumerable<City> GetAllCities()
        {
            return _unitOfWork.CityRepository.GetAllCities();
        }

        public City GetById(int id)
        {
            return _unitOfWork.CityRepository.GetById(id);
        }

        public IEnumerable<IndexCityVM.GetData> GetCitiesByGovernorateId(int govId)
        {
            return _unitOfWork.CityRepository.GetCitiesByGovernorateId(govId);
        }

        public IEnumerable<IndexCityVM.GetData> GetCitiesByGovernorateName(string govName)
        {
            return _unitOfWork.CityRepository.GetCitiesByGovernorateName(govName);
        }

        public int Update(EditCityVM cityVM)
        {
            _unitOfWork.CityRepository.Update(cityVM);
            _unitOfWork.CommitAsync();
            return cityVM.Id;
        }
        public int GetGovIdByGovernorateName(string govName)
        {
            return _unitOfWork.CityRepository.GetGovIdByGovernorateName(govName);
        }
        public int GetCityIdByName(string name)
        {
            return _unitOfWork.CityRepository.GetCityIdByName(name);
        }
    }
}
