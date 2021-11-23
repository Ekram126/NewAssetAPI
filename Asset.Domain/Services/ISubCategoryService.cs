using Asset.Models;
using Asset.ViewModels.SubCategoryVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
   public interface ISubCategoryService
    {
        IEnumerable<IndexSubCategoryVM.GetData> GetAll();
        EditSubCategoryVM GetById(int id);

  IEnumerable<SubCategory> GetSubCategoryByCategoryId(int categoryId);

        IEnumerable<SubCategory> GetAllSubCategories();
        IEnumerable<IndexSubCategoryVM.GetData> GetSubCategoriesByName(string subCategoryName);

        int Add(CreateSubCategoryVM subCategoryObj);
        int Update(EditSubCategoryVM subCategoryObj);
        int Delete(int id);

    }
}
