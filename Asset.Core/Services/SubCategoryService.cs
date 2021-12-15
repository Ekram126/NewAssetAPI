using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.SubCategoryVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
   public class SubCategoryService: ISubCategoryService
    {

        private IUnitOfWork _unitOfWork;

        public SubCategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Add(CreateSubCategoryVM subCategoryObj)
        {
            return _unitOfWork.SubCategoryRepository.Add(subCategoryObj);
            //return _unitOfWork.CommitAsync();
        }

        public int Delete(int id)
        {
            var subCategoryObj = _unitOfWork.SubCategoryRepository.GetById(id);
            _unitOfWork.SubCategoryRepository.Delete(subCategoryObj.Id);
            _unitOfWork.CommitAsync();
            return subCategoryObj.Id;
        }

        public IEnumerable<IndexSubCategoryVM.GetData> GetAll()
        {
            return _unitOfWork.SubCategoryRepository.GetAll();
        }

        public IEnumerable<SubCategory> GetAllSubCategories()
        {
            return _unitOfWork.SubCategoryRepository.GetAllSubCategories();
        }

        public EditSubCategoryVM GetById(int id)
        {
            return _unitOfWork.SubCategoryRepository.GetById(id);
        }

        public IEnumerable<IndexSubCategoryVM.GetData> GetSubCategoriesByName(string subCategoryName)
        {
            return _unitOfWork.SubCategoryRepository.GetSubCategoriesByName(subCategoryName);
        }

        public IEnumerable<SubCategory> GetSubCategoryByCategoryId(int categoryId)
        {
            return _unitOfWork.SubCategoryRepository.GetSubCategoryByCategoryId(categoryId);
        }

        public int Update(EditSubCategoryVM subCategoryObj)
        {
            _unitOfWork.SubCategoryRepository.Update(subCategoryObj);
            _unitOfWork.CommitAsync();
            return subCategoryObj.Id;
        }
    }
}
