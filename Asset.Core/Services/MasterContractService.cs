using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.ContractVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
    public class MasterContractService : IMasterContractService
    {
        private IUnitOfWork _unitOfWork;

        public MasterContractService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public int Add(CreateMasterContractVM masterContractObj)
        {
            return _unitOfWork.MasterContractRepository.Add(masterContractObj);
        }

        public IEnumerable<IndexMasterContractVM.GetData> AlertContractsBefore3Months(int hospitalId, int duration)
        {
            return _unitOfWork.MasterContractRepository.AlertContractsBefore3Months(hospitalId, duration);
        }

        public int CreateContractAttachments(ContractAttachment attachObj)
        {
            return _unitOfWork.MasterContractRepository.CreateContractAttachments(attachObj);
        }

        public int Delete(int id)
        {
            return _unitOfWork.MasterContractRepository.Delete(id);
        }

        public int DeleteContractAttachment(int attachId)
        {
            return _unitOfWork.MasterContractRepository.DeleteContractAttachment(attachId);
        }

        public GeneratedMasterContractNumberVM GenerateMasterContractSerial()
        {
            return _unitOfWork.MasterContractRepository.GenerateMasterContractSerial();
        }

        public IEnumerable<MasterContract> GetAll()
        {
            return _unitOfWork.MasterContractRepository.GetAll();
        }

        public MasterContract GetById(int id)
        {
            return _unitOfWork.MasterContractRepository.GetById(id);
        }

        public IEnumerable<ContractAttachment> GetContractAttachmentByMasterContractId(int masterContractId)
        {
            return _unitOfWork.MasterContractRepository.GetContractAttachmentByMasterContractId(masterContractId).ToList();
        }

        public ContractAttachment GetLastDocumentForMasterContractId(int masterContractId)
        {
            return _unitOfWork.MasterContractRepository.GetLastDocumentForMasterContractId(masterContractId);
        }

        public IndexMasterContractVM GetMasterContractsByHospitalId(int hospitalId, int pageNumber, int pageSize)
        {
            return _unitOfWork.MasterContractRepository.GetMasterContractsByHospitalId(hospitalId, pageNumber, pageSize);
        }

        public IndexMasterContractVM GetMasterContractsByHospitalId(int hospitalId)
        {
            return _unitOfWork.MasterContractRepository.GetMasterContractsByHospitalId(hospitalId);
        }

        public IEnumerable<IndexMasterContractVM.GetData> Search(SearchContractVM model)
        {
            return _unitOfWork.MasterContractRepository.Search(model);
        }

        public IEnumerable<IndexMasterContractVM.GetData> SortContracts(int hospitalId, SortContractsVM sortObj)
        {
            return _unitOfWork.MasterContractRepository.SortContracts(hospitalId, sortObj);
        }

        public int Update(MasterContract masterContractObj)
        {
            return _unitOfWork.MasterContractRepository.Update(masterContractObj);
        }
    }
}
