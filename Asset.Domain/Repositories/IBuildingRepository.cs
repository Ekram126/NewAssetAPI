using System.Collections.Generic;
using Asset.Models;
using Asset.ViewModels.GovernorateVM;


namespace Asset.Domain.Repositories
{
  public  interface IBuildingRepository
    {
        IEnumerable<Building> GetAll();
        IEnumerable<Building> GetAllBuildingsByHospitalId(int hospitalId);
        Building GetById(int id);
        int Add(Building buildingObj); 
        int Update(Building buildingObj);
        int Delete(int id);
    }
}
