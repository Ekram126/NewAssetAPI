using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.DepartmentVM;
using Asset.ViewModels.RoleCategoryVM;
using System.Collections.Generic;


namespace Asset.Core.Services
{
    public class DepartmentService : IDepartmentService
    {

        private IUnitOfWork _unitOfWork;

        public DepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public int Add(CreateDepartmentVM DepartmentVM)
        {
            return _unitOfWork.DepartmentRepository.Add(DepartmentVM);
           // return _unitOfWork.CommitAsync();
        }

        public int AddDepartmentToHospital(CreateDepartmentVM departmentObj)
        {
            return _unitOfWork.DepartmentRepository.AddDepartmentToHospital(departmentObj);
        }

        public int Delete(int id)
        {
            var DepartmentObj = _unitOfWork.DepartmentRepository.GetById(id);
            _unitOfWork.DepartmentRepository.Delete(DepartmentObj.Id);
            _unitOfWork.CommitAsync();

            return DepartmentObj.Id;
        }

        public GenerateDepartmentCodeVM GenerateDepartmentCode()
        {
            return _unitOfWork.DepartmentRepository.GenerateDepartmentCode();
        }

        public IEnumerable<IndexDepartmentVM.GetData> GetAll()
        {
            return _unitOfWork.DepartmentRepository.GetAll();
        }

        public IEnumerable<Department> GetAllDepartments()
        {
            return _unitOfWork.DepartmentRepository.GetAllDepartments();
        }

        public EditDepartmentVM GetById(int id)
        {
            return _unitOfWork.DepartmentRepository.GetById(id);
        }

        public IEnumerable<Department> GetDepartmentsByHospitalId(int hospitalId)
        {
            return _unitOfWork.DepartmentRepository.GetDepartmentsByHospitalId(hospitalId);
        }

        public IEnumerable<IndexDepartmentVM.GetData> SortDepartments(SortDepartmentVM sortObj)
        {
            return _unitOfWork.DepartmentRepository.SortDepartments(sortObj);
        }

        public int Update(EditDepartmentVM DepartmentVM)
        {
            _unitOfWork.DepartmentRepository.Update(DepartmentVM);
            _unitOfWork.CommitAsync();
            return DepartmentVM.Id;
        }
    }
}
