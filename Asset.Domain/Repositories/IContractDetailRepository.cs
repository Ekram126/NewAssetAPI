using Asset.Models;
using Asset.ViewModels.ContractVM;
using System;
using System.Collections.Generic;


namespace Asset.Domain.Repositories
{
    public interface IContractDetailRepository
    {
        IEnumerable<ContractDetail> GetAll();
        ContractDetail GetById(int id);
        IEnumerable<IndexContractVM.GetData> GetContractsByMasterContractId(int masterContractId);
        int Add(ContractDetail masterContractObj);
        int Update(ContractDetail masterContractObj);
        int Delete(int id);
    }
}
