using Asset.Models;
using Asset.ViewModels.ContractVM;
using System;
using System.Collections.Generic;


namespace Asset.Domain.Repositories
{
    public interface IMasterContractRepository
    {
        IEnumerable<MasterContract> GetAll();
        IEnumerable<IndexMasterContractVM.GetData> GetMasterContractsByHospitalId(int hospitalId);
        IEnumerable<IndexMasterContractVM.GetData> Search(SearchContractVM model);


        MasterContract GetById(int id);
        int Add(CreateMasterContractVM masterContractObj);
        int Update(MasterContract masterContractObj);
        int Delete(int id);
        IEnumerable<IndexMasterContractVM.GetData> SortContracts(int hospitalId, SortContractsVM sortObj);

    }
}
