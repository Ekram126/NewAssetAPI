using Asset.Models;
using Asset.ViewModels.AssetDetailVM;
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
        IEnumerable<Hospital> GetListofHospitalsFromAssetContractDetailByMasterContractId(int masterContractId);
        IEnumerable<IndexContractVM.GetData> GetContractAssetsByHospitalId(int hospitalId, int masterContractId);
        IEnumerable<IndexContractVM.GetData> GetContractByHospitalId(int hospitalId);
        int Add(ContractDetail masterContractObj);
        int Update(ContractDetail masterContractObj);
        int Delete(int id);
    }
}
