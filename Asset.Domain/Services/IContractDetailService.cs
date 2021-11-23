using Asset.Models;
using Asset.ViewModels.ContractVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
  public  interface IContractDetailService
    {

        IEnumerable<ContractDetail> GetAll();
        ContractDetail GetById(int id);
        IEnumerable<IndexContractVM.GetData> GetContractsByMasterContractId(int masterContractId);
        int Add(ContractDetail masterContractObj);
        int Update(ContractDetail masterContractObj);
        int Delete(int id);
    }
}
