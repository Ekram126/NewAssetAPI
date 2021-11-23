using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.OrganizationVM;
using Asset.ViewModels.RoleCategoryVM;
using System.Collections.Generic;


namespace Asset.Core.Services
{
    public class OrganizationService : IOrganizationService
    {

        private IUnitOfWork _unitOfWork;

        public OrganizationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public int Add(CreateOrganizationVM organizationVM)
        {
            _unitOfWork.OrganizationRepository.Add(organizationVM);
            return _unitOfWork.CommitAsync();
        }

        public int Delete(int id)
        {
            var organizationObj = _unitOfWork.OrganizationRepository.GetById(id);
            _unitOfWork.OrganizationRepository.Delete(organizationObj.Id);
            _unitOfWork.CommitAsync();

            return organizationObj.Id;
        }

        public IEnumerable<IndexOrganizationVM.GetData> GetAll()
        {
            return _unitOfWork.OrganizationRepository.GetAll();
        }

        public IEnumerable<Organization> GetAllOrganizations()
        {
            return _unitOfWork.OrganizationRepository.GetAllOrganizations();
        }

        public EditOrganizationVM GetById(int id)
        {
            return _unitOfWork.OrganizationRepository.GetById(id);
        }

        public int Update(EditOrganizationVM organizationVM)
        {
            _unitOfWork.OrganizationRepository.Update(organizationVM);
            _unitOfWork.CommitAsync();
            return organizationVM.Id;
        }
    }
}
