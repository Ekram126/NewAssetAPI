using System.Collections.Generic;
using Asset.Models;
using Asset.ViewModels.GovernorateVM;


namespace Asset.Domain.Services
{
    public interface IRoomService
    {
        IEnumerable<Room> GetAll();
        IEnumerable<Room> GetAllRoomsByFloorId(int floorId);
        Room GetById(int id);
        int Add(Room roomObj);
        int Update(Room roomObj);
        int Delete(int id);
    }
}
