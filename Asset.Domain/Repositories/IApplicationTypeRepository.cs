using Asset.Models;
using Asset.ViewModels.ApplicationTypeVM;
using Asset.ViewModels.CategoryVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Repositories
{
   public interface IApplicationTypeRepository
    {
        IEnumerable<IndexApplicationTypeVM.GetData> GetAll();
        //EditCategoryVM GetById(int id);
        //int Add(CreateCategoryVM categoryVM);
        //int Update(EditCategoryVM categoryVM);
        //int Delete(int id);
    }
}
