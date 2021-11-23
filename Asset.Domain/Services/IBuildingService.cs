using System.Collections.Generic;
using Asset.Models;

namespace Asset.Domain.Services
{
  public  interface IBuildingService
    {
        IEnumerable<Building> GetAll();
        IEnumerable<Building> GetAllBuildingsByHospitalId(int hospitalId);
        Building GetById(int id);
        int Add(Building buildingObj); 
        int Update(Building buildingObj);
        int Delete(int id);
    }
}
