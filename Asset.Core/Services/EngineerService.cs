using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.EngineerVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
  public  class EngineerService : IEngineerService
    {
        private IUnitOfWork _unitOfWork;

        public EngineerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Add(CreateEngineerVM engineerObj)
        {
          return  _unitOfWork.EngineerRepository.Add(engineerObj);
          //  return _unitOfWork.CommitAsync();
        }

        public int Delete(int id)
        {
            var engineerObj = _unitOfWork.EngineerRepository.GetById(id);
            _unitOfWork.EngineerRepository.Delete(engineerObj.Id);
            _unitOfWork.CommitAsync();
            return engineerObj.Id;
        }

        public IEnumerable<IndexEngineerVM.GetData> GetAll()
        {
            return _unitOfWork.EngineerRepository.GetAll();
        }

        public IEnumerable<Engineer> GetAllEngineers()
        {
            return _unitOfWork.EngineerRepository.GetAllEngineers();
        }

        public Engineer GetByEmail(string email)
        {
            return _unitOfWork.EngineerRepository.GetByEmail(email);
        }

        public Engineer GetById(int id)
        {
            return _unitOfWork.EngineerRepository.GetById(id);
        }

     

        public IEnumerable<IndexEngineerVM.GetData> SortEngineer(SortEngineerVM sortObj)
        {
            return _unitOfWork.EngineerRepository.SortEngineer(sortObj);
        }

        public int Update(EditEngineerVM engineerObj)
        {
            _unitOfWork.EngineerRepository.Update(engineerObj);
          //  _unitOfWork.CommitAsync();
            return engineerObj.Id;
        }
    }
}
