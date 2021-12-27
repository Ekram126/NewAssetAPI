using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.CategoryVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Add(CreateCategoryVM categoryObj)
        {
            return _unitOfWork.CategoryRepository.Add(categoryObj);
          //  return _unitOfWork.CommitAsync();
        }

        public int Delete(int id)
        {
            var categoryObj = _unitOfWork.CategoryRepository.GetById(id);
            _unitOfWork.CategoryRepository.Delete(categoryObj.Id);
            _unitOfWork.CommitAsync();
            return categoryObj.Id;
        }

        public IEnumerable<IndexCategoryVM.GetData> GetAll()
        {
            return _unitOfWork.CategoryRepository.GetAll();
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _unitOfWork.CategoryRepository.GetAllCategories();
        }

        public EditCategoryVM GetById(int id)
        {
            return _unitOfWork.CategoryRepository.GetById(id);
        }

        public IEnumerable<IndexCategoryVM.GetData> GetCategoryByCategoryTypeId(int categoryTypeId)
        {
            return _unitOfWork.CategoryRepository.GetCategoryByCategoryTypeId(categoryTypeId);
        }

        public IEnumerable<IndexCategoryVM.GetData> GetCategoryByName(string categoryName)
        {
            return _unitOfWork.CategoryRepository.GetCategoryByName(categoryName);
        }

        public int Update(EditCategoryVM categoryObj)
        {
            _unitOfWork.CategoryRepository.Update(categoryObj);
            //_unitOfWork.CommitAsync();
            return categoryObj.Id;
        }
    }
}
