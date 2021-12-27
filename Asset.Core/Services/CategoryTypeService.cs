using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.CategoryTypeVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
    public class CategoryTypeService : ICategoryTypeService
    {
        private IUnitOfWork _unitOfWork;

        public CategoryTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Add(CreateCategoryTypeVM categoryObj)
        {
            return _unitOfWork.CategoryTypeRepository.Add(categoryObj);
          //  return _unitOfWork.CommitAsync();
        }

        public int Delete(int id)
        {
            var categoryObj = _unitOfWork.CategoryRepository.GetById(id);
            _unitOfWork.CategoryRepository.Delete(categoryObj.Id);
            _unitOfWork.CommitAsync();
            return categoryObj.Id;
        }

        public IEnumerable<IndexCategoryTypeVM.GetData> GetAll()
        {
            return _unitOfWork.CategoryTypeRepository.GetAll();
        }

    

        public EditCategoryTypeVM GetById(int id)
        {
            return _unitOfWork.CategoryTypeRepository.GetById(id);
        }

  

        public int Update(EditCategoryTypeVM categoryObj)
        {
            _unitOfWork.CategoryTypeRepository.Update(categoryObj);
            _unitOfWork.CommitAsync();
            return categoryObj.Id;
        }
    }
}
