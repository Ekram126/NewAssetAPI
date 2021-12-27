using Asset.Models;
using Asset.ViewModels.CategoryTypeVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
   public interface ICategoryTypeService
    {
        IEnumerable<IndexCategoryTypeVM.GetData> GetAll();
        EditCategoryTypeVM GetById(int id);
        int Add(CreateCategoryTypeVM categoryTypeVM);
        int Update(EditCategoryTypeVM categoryTypeVM);
        int Delete(int id);
    }
}
