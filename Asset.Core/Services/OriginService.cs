using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.OriginVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
   public class OriginService : IOriginService
    {
        private IUnitOfWork _unitOfWork;

        public OriginService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Add(CreateOriginVM originObj)
        {
          return  _unitOfWork.OriginRepository.Add(originObj);
           // return _unitOfWork.CommitAsync();
        }

        public int Delete(int id)
        {
            var originObj = _unitOfWork.OriginRepository.GetById(id);
            _unitOfWork.OriginRepository.Delete(originObj.Id);
            _unitOfWork.CommitAsync();
            return originObj.Id;
        }

        public IEnumerable<IndexOriginVM.GetData> GetAll()
        {
            return _unitOfWork.OriginRepository.GetAll();
        }

        public IEnumerable<Origin> GetAllOrigins()
        {
            return _unitOfWork.OriginRepository.GetAllOrigins();
        }

        public EditOriginVM GetById(int id)
        {
            return _unitOfWork.OriginRepository.GetById(id);
        }

        public IEnumerable<IndexOriginVM.GetData> GetOriginByName(string originName)
        {
            return _unitOfWork.OriginRepository.GetOriginByName(originName);
        }

        public IEnumerable<IndexOriginVM.GetData> SortOrigins(SortOriginVM sortObj)
        {
            return _unitOfWork.OriginRepository.SortOrigins(sortObj);
        }

        public int Update(EditOriginVM originObj)
        {
            _unitOfWork.OriginRepository.Update(originObj);
            _unitOfWork.CommitAsync();
            return originObj.Id;
        }
    }
}
