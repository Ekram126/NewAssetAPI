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

        public int Delete(int id)
        {
            return _unitOfWork.MasterContractRepository.Delete(id);
        }

        public IEnumerable<MasterContract> GetAll()
        {
            return _unitOfWork.MasterContractRepository.GetAll();
        }

        public MasterContract GetById(int id)
        {
            return _unitOfWork.MasterContractRepository.GetById(id);
        }

        public IEnumerable<IndexMasterContractVM.GetData> GetMasterContractsByHospitalId(int hospitalId)
        {
            return _unitOfWork.MasterContractRepository.GetMasterContractsByHospitalId(hospitalId);
        }

        public IEnumerable<IndexMasterContractVM.GetData> Search(SearchContractVM model)
        {
            return _unitOfWork.MasterContractRepository.Search(model);
        }

        public int Update(MasterContract masterContractObj)
        {
            return _unitOfWork.MasterContractRepository.Update(masterContractObj);
        }
    }
}
