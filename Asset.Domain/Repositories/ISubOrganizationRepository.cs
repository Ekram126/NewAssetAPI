using System.Collections.Generic;
using Asset.Models;
using Asset.ViewModels.SubOrganizationVM;

namespace Asset.Domain.Repositories
{
  public  interface ISubOrganizationRepository
    {

        IEnumerable<SubOrganization> GetAllSubOrganizations();
        IEnumerable<IndexSubOrganizationVM.GetData> GetAll();
        IEnumerable<IndexSubOrganizationVM.GetData> GetSubOrganizationByOrgId(int orgId);
        IEnumerable<IndexSubOrganizationVM.GetData> GetSubOrganizationByOrgName(string orgName);

        SubOrganization GetById(int id);
        Organization GetOrganizationBySubId(int subId);
        int Add(CreateSubOrganizationVM subOrganization);
        int Update(EditSubOrganizationVM subOrganization);
        int Delete(int id);
    }
}
