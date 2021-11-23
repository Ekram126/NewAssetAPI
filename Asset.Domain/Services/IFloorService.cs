using System.Collections.Generic;
using Asset.Models;
using Asset.ViewModels.GovernorateVM;


namespace Asset.Domain.Services
{
    public interface IFloorService
    {
        IEnumerable<Floor> GetAll();
        IEnumerable<Floor> GetAllFloorsBybuildingId(int buildId);
        Floor GetById(int id);
        int Add(Floor floorObj);
        int Update(Floor floorObj);
        int Delete(int id);
    }
}
