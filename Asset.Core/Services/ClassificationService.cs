using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.ClassificationVM;
using Asset.ViewModels.OriginVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
   public class ClassificationService : IClassificationService
    {
        private IUnitOfWork _unitOfWork;

        public ClassificationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Add(Classification classObj)
        {
            _unitOfWork.ClassificationRepository.Add(classObj);
            return _unitOfWork.CommitAsync();
        }

        public int Delete(int id)
        {
            var classObj = _unitOfWork.ClassificationRepository.GetById(id);
            _unitOfWork.ClassificationRepository.Delete(classObj.Id);
            _unitOfWork.CommitAsync();
            return classObj.Id;
        }

        public IEnumerable<Classification> GetAll()
        {
            return _unitOfWork.ClassificationRepository.GetAll();
        }

     

        public Classification GetById(int id)
        {
            return _unitOfWork.ClassificationRepository.GetById(id);
        }

        public IEnumerable<Classification> SortClassification(SortClassificationVM sortObj)
        {
            return _unitOfWork.ClassificationRepository.SortClassification(sortObj);
        }

        public int Update(Classification classObj)
        {
            _unitOfWork.ClassificationRepository.Update(classObj);
            _unitOfWork.CommitAsync();
            return classObj.Id;
        }
    }
}
