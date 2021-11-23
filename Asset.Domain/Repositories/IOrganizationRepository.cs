using System.Collections.Generic;
using Asset.Models;
using Asset.ViewModels.OrganizationVM;


namespace Asset.Domain.Repositories
{
  public  interface IOrganizationRepository
    {
        IEnumerable<Organization> GetAllOrganizations();
        IEnumerable<IndexOrganizationVM.GetData> GetAll();
        EditOrganizationVM GetById(int id);


        int Add(CreateOrganizationVM organization); 
        int Update(EditOrganizationVM organization);
        int Delete(int id);
    }
}
