using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.AssetDetailVM;
using Asset.ViewModels.ContractVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
    public class ContractDetailService : IContractDetailService
    {

        private IUnitOfWork _unitOfWork;

        public ContractDetailService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public int Add(ContractDetail contractDetailObj)
        {
         return   _unitOfWork.ContractDetailRepository.Add(contractDetailObj);
        }

        public int Delete(int id)
        {
            return _unitOfWork.ContractDetailRepository.Delete(id);
        }

        public IEnumerable<ContractDetail> GetAll()
        {
            return _unitOfWork.ContractDetailRepository.GetAll();
        }

        public ContractDetail GetById(int id)
        {
            return _unitOfWork.ContractDetailRepository.GetById(id);
        }

        public IEnumerable<IndexContractVM.GetData> GetContractAssetsByHospitalId(int hospitalId, int masterContractId)
        {
            return _unitOfWork.ContractDetailRepository.GetContractAssetsByHospitalId(hospitalId,masterContractId);
        }

        public IEnumerable<IndexContractVM.GetData> GetContractByHospitalId(int hospitalId)
        {
            return _unitOfWork.ContractDetailRepository.GetContractByHospitalId(hospitalId);
        }

        public IEnumerable<IndexContractVM.GetData> GetContractsByMasterContractId(int masterContractId)
        {
            return _unitOfWork.ContractDetailRepository.GetContractsByMasterContractId(masterContractId);
        }

        public IEnumerable<Hospital> GetListofHospitalsFromAssetContractDetailByMasterContractId(int masterContractId)
        {
            return _unitOfWork.ContractDetailRepository.GetListofHospitalsFromAssetContractDetailByMasterContractId(masterContractId);
        }

        public int Update(ContractDetail contractDetailObj)
        {
            return _unitOfWork.ContractDetailRepository.Update(contractDetailObj);
        }
    }
}
