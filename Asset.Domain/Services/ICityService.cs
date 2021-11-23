using System.Collections.Generic;
using Asset.Models;
using Asset.ViewModels.CityVM;

namespace Asset.Domain.Services
{
  public  interface ICityService
    {
        IEnumerable<City> GetAllCities();
        IEnumerable<IndexCityVM.GetData> GetAll();
        IEnumerable<IndexCityVM.GetData> GetCitiesByGovernorateId(int govId);
        IEnumerable<IndexCityVM.GetData> GetCitiesByGovernorateName(string govName);
        City GetById(int id);
        int Add(CreateCityVM cityObj);
        int Update(EditCityVM cityObj);
        int Delete(int id);
        int GetGovIdByGovernorateName(string govName);
        int GetCityIdByName(string name);
    }
}
