using Asset.Models;
using Asset.ViewModels.CategoryTypeVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Repositories
{
    public interface ICategoryTypeRepository
    {
        IEnumerable<IndexCategoryTypeVM.GetData> GetAll();
        EditCategoryTypeVM GetById(int id);
        int Add(CreateCategoryTypeVM categoryTypeVM);
        int Update(EditCategoryTypeVM categoryTypeVM);
        int Delete(int id);
    }
}
