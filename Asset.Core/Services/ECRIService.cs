using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.ECRIVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
    public class ECRIService : IECRIService
    {
        private IUnitOfWork _unitOfWork;

        public ECRIService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Add(CreateECRIVM ECRIObj)
        {
            _unitOfWork.ECRIRepository.Add(ECRIObj);
            return _unitOfWork.CommitAsync();
        }

        public int Delete(int id)
        {
            var ECRIObj = _unitOfWork.ECRIRepository.GetById(id);
            _unitOfWork.ECRIRepository.Delete(ECRIObj.Id);
            _unitOfWork.CommitAsync();
            return ECRIObj.Id;
        }

        public IEnumerable<IndexECRIVM.GetData> GetAll()
        {
            return _unitOfWork.ECRIRepository.GetAll();
        }

        public IEnumerable<ECRI> GetAllECRIs()
        {
            return _unitOfWork.ECRIRepository.GetAllECRIs();
        }

        public EditECRIVM GetById(int id)
        {
            return _unitOfWork.ECRIRepository.GetById(id);
        }

        public IEnumerable<IndexECRIVM.GetData> sortECRI(SortECRIVM searchObj)
        {
            return _unitOfWork.ECRIRepository.sortECRI(searchObj);
        }

        public int Update(EditECRIVM ECRIObj)
        {
            _unitOfWork.ECRIRepository.Update(ECRIObj);
            //_unitOfWork.CommitAsync();
            return ECRIObj.Id;
        }
    }
}
