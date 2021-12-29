using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.GovernorateVM;
using Asset.ViewModels.RoleCategoryVM;
using System.Collections.Generic;


namespace Asset.Core.Services
{
    public class GovernorateService : IGovernorateService
    {

        private IUnitOfWork _unitOfWork;

        public GovernorateService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public int Add(CreateGovernorateVM GovernorateVM)
        {
            _unitOfWork.GovernorateRepository.Add(GovernorateVM);
            return _unitOfWork.CommitAsync();
        }

        public int Delete(int id)
        {
            var GovernorateObj = _unitOfWork.GovernorateRepository.GetById(id);
            _unitOfWork.GovernorateRepository.Delete(GovernorateObj.Id);
            _unitOfWork.CommitAsync();

            return GovernorateObj.Id;
        }

        public IEnumerable<IndexGovernorateVM.GetData> GetAll()
        {
            return _unitOfWork.GovernorateRepository.GetAll();
        }

        public IEnumerable<Governorate> GetAllGovernorates()
        {
            return _unitOfWork.GovernorateRepository.GetAllGovernorates();
        }

        public EditGovernorateVM GetById(int id)
        {
            return _unitOfWork.GovernorateRepository.GetById(id);
        }

        public EditGovernorateVM GetGovernorateByName(string govName)
        {
            return _unitOfWork.GovernorateRepository.GetGovernorateByName(govName);
        }

        public int Update(EditGovernorateVM GovernorateVM)
        {
            _unitOfWork.GovernorateRepository.Update(GovernorateVM);
            _unitOfWork.CommitAsync();
            return GovernorateVM.Id;
        }
        public IEnumerable<GovernorateWithHospitalsVM> GetGovernorateWithHospitals()
        {
            return _unitOfWork.GovernorateRepository.GetGovernorateWithHospitals();
        }
    }
}
