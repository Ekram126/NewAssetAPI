using System.Collections.Generic;
using Asset.Models;
using Asset.ViewModels.RoleCategoryVM;


namespace Asset.Domain.Repositories
{
  public  interface IRoleCategoryRepository
    {
        IEnumerable<IndexCategoryVM.GetData> GetAll();
        RoleCategory GetById(int id);
        int Add(CreateRoleCategory roleCategory);
        int Delete(RoleCategory roleCategory);
        int Update(EditRoleCategory roleCategory);
    }
}
