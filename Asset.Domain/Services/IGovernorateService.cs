using System.Collections.Generic;
using Asset.Models;
using Asset.ViewModels.GovernorateVM;


namespace Asset.Domain.Services
{
  public  interface IGovernorateService
    {
        IEnumerable<Governorate> GetAllGovernorates();
        IEnumerable<IndexGovernorateVM.GetData> GetAll();
        EditGovernorateVM GetById(int id);
        EditGovernorateVM GetGovernorateByName(string govName);
        int Add(CreateGovernorateVM governorateObj); 
        int Update(EditGovernorateVM governorateObj);
        int Delete(int id);
        public IEnumerable<GovernorateWithHospitalsVM> GetGovernorateWithHospitals();
    }
}
