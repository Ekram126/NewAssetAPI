using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using System.Collections.Generic;


namespace Asset.Core.Services
{
    public class RoomService : IRoomService
    {

        private IUnitOfWork _unitOfWork;

        public RoomService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public int Add(Room RoomVM)
        {
            _unitOfWork.RoomRepository.Add(RoomVM);
            return _unitOfWork.CommitAsync();
        }

        public int Delete(int id)
        {
            var RoomObj = _unitOfWork.RoomRepository.GetById(id);
            _unitOfWork.RoomRepository.Delete(RoomObj.Id);
            _unitOfWork.CommitAsync();

            return RoomObj.Id;
        }

        public IEnumerable<Room> GetAll()
        {
            return _unitOfWork.RoomRepository.GetAll();
        }


        public Room GetById(int id)
        {
            return _unitOfWork.RoomRepository.GetById(id);
        }

        public int Update(Room RoomVM)
        {
            _unitOfWork.RoomRepository.Update(RoomVM);
            _unitOfWork.CommitAsync();
            return RoomVM.Id;
        }


        public IEnumerable<Room> GetAllRoomsByFloorId(int floorId)
        {
            return _unitOfWork.RoomRepository.GetAllRoomsByFloorId(floorId);
        }

       

      
    }
}
