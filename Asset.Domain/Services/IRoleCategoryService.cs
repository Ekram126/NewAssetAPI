using System.Collections.Generic;
using Asset.Models;
using Asset.ViewModels.RoleCategoryVM;


namespace Asset.Domain.Services
{
  public  interface IRoleCategoryService
    {
        IEnumerable<IndexCategoryVM.GetData> GetAll();
        RoleCategory GetById(int id);
        int Add(CreateRoleCategory roleCategory);
        int Delete(int id);
        int Update(EditRoleCategory roleCategory);
    }
}
