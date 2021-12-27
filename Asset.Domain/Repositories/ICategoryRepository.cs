using Asset.Models;
using Asset.ViewModels.CategoryVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Repositories
{
   public interface ICategoryRepository
    {

        IEnumerable<IndexCategoryVM.GetData> GetAll();

        IEnumerable<Category> GetAllCategories();
        IEnumerable<IndexCategoryVM.GetData> GetCategoryByName(string categoryName);

        IEnumerable<IndexCategoryVM.GetData> GetCategoryByCategoryTypeId(int categoryTypeId);

        EditCategoryVM GetById(int id);
        int Add(CreateCategoryVM categoryVM);
        int Update(EditCategoryVM categoryVM);
        int Delete(int id);
    }
}
