using Asset.Models;
using Asset.ViewModels.CategoryVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
   public interface ICategoryService
    {

        IEnumerable<IndexCategoryVM.GetData> GetAll();
        EditCategoryVM GetById(int id);

        IEnumerable<Category> GetAllCategories();
        IEnumerable<IndexCategoryVM.GetData> GetCategoryByName(string categoryName);
        IEnumerable<IndexCategoryVM.GetData> GetCategoryByCategoryTypeId(int categoryTypeId);
        int Add(CreateCategoryVM categoryVM);
        int Update(EditCategoryVM categoryVM);
        int Delete(int id);
    }
}
