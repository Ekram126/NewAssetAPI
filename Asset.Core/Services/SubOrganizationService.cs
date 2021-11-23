using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.SubOrganizationVM;
using Asset.ViewModels.RoleCategoryVM;
using System.Collections.Generic;


namespace Asset.Core.Services
{
    public class SubOrganizationService : ISubOrganizationService
    {

        private IUnitOfWork _unitOfWork;

        public SubOrganizationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public int Add(CreateSubOrganizationVM subOrganizationVM)
        {
            _unitOfWork.SubOrganizationRepository.Add(subOrganizationVM);
            return _unitOfWork.CommitAsync();
        }

        public int Delete(int id)
        {
            var SubOrganizationObj = _unitOfWork.SubOrganizationRepository.GetById(id);
            _unitOfWork.SubOrganizationRepository.Delete(SubOrganizationObj.Id);
            _unitOfWork.CommitAsync();

            return SubOrganizationObj.Id;
        }

        public IEnumerable<IndexSubOrganizationVM.GetData> GetAll()
        {
            return _unitOfWork.SubOrganizationRepository.GetAll();
        }

        public IEnumerable<SubOrganization> GetAllSubOrganizations()
        {
            return _unitOfWork.SubOrganizationRepository.GetAllSubOrganizations();
        }

        public SubOrganization GetById(int id)
        {
            return _unitOfWork.SubOrganizationRepository.GetById(id);
        }

        public Organization GetOrganizationBySubId(int subId)
        {
            return _unitOfWork.SubOrganizationRepository.GetOrganizationBySubId(subId);
        }

        public IEnumerable<IndexSubOrganizationVM.GetData> GetSubOrganizationByOrgId(int orgId)
        {
            return _unitOfWork.SubOrganizationRepository.GetSubOrganizationByOrgId(orgId);
        }

        public IEnumerable<IndexSubOrganizationVM.GetData> GetSubOrganizationByOrgName(string orgName)
        {
            return _unitOfWork.SubOrganizationRepository.GetSubOrganizationByOrgName(orgName);
        }

        public int Update(EditSubOrganizationVM SubOrganizationVM)
        {
            _unitOfWork.SubOrganizationRepository.Update(SubOrganizationVM);
            _unitOfWork.CommitAsync();
            return SubOrganizationVM.Id;
        }
    }
}
