using Asset.Models;
using Asset.ViewModels.ContractVM;
using System;
using System.Collections.Generic;


namespace Asset.Domain.Repositories
{
    public interface IMasterContractRepository
    {
        IEnumerable<MasterContract> GetAll();
       // IEnumerable<IndexMasterContractVM.GetData> GetMasterContractsByHospitalId(int hospitalId);

        IndexMasterContractVM GetMasterContractsByHospitalId(int hospitalId, int pageNumber, int pageSize);
        IndexMasterContractVM GetMasterContractsByHospitalId(int hospitalId);
        IEnumerable<IndexMasterContractVM.GetData> Search(SearchContractVM model);
        MasterContract GetById(int id);
        int Add(CreateMasterContractVM masterContractObj);
        int Update(MasterContract masterContractObj);
        int Delete(int id);
        IEnumerable<IndexMasterContractVM.GetData> SortContracts(int hospitalId, SortContractsVM sortObj);
        int CreateContractAttachments(ContractAttachment attachObj);
        GeneratedMasterContractNumberVM GenerateMasterContractSerial();
        IEnumerable<ContractAttachment> GetContractAttachmentByMasterContractId(int masterContractId);



        IEnumerable<IndexMasterContractVM.GetData> AlertContractsBefore3Months(int hospitalId,int duration);



    }
}
