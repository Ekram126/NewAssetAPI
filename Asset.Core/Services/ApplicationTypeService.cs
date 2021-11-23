using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.ApplicationTypeVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
    public class ApplicationTypeService : IApplicationTypeService
    {
        private IUnitOfWork _unitOfWork;

        public ApplicationTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //public int Add(CreateApplicationTypeVM ApplicationTypeObj)
        //{
        //    _unitOfWork.ApplicationTypeRepository.Add(ApplicationTypeObj);
        //    return _unitOfWork.CommitAsync();
        //}

        //public int Delete(int id)
        //{
        //    var ApplicationTypeObj = _unitOfWork.ApplicationTypeRepository.GetById(id);
        //    _unitOfWork.ApplicationTypeRepository.Delete(ApplicationTypeObj.Id);
        //    _unitOfWork.CommitAsync();
        //    return ApplicationTypeObj.Id;
        //}

        public IEnumerable<IndexApplicationTypeVM.GetData> GetAll()
        {
            return _unitOfWork.ApplicationTypeRepository.GetAll();
        }

        //public IEnumerable<ApplicationType> GetAllApplicationTypes()
        //{
        //    return _unitOfWork.ApplicationTypeRepository.GetAllApplicationTypes();
        //}

        //public EditApplicationTypeVM GetById(int id)
        //{
        //    return _unitOfWork.ApplicationTypeRepository.GetById(id);
        //}

     
        //public int Update(EditApplicationTypeVM ApplicationTypeObj)
        //{
        //    _unitOfWork.ApplicationTypeRepository.Update(ApplicationTypeObj);
        //    _unitOfWork.CommitAsync();
        //    return ApplicationTypeObj.Id;
        //}
    }
}
