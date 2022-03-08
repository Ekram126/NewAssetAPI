using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.EmployeeVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
  public  class EmployeeService:IEmployeeService
    {
        private IUnitOfWork _unitOfWork;

        public EmployeeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Add(CreateEmployeeVM employeeObj)
        {
          return  _unitOfWork.EmployeeRepository.Add(employeeObj);
          //  return _unitOfWork.CommitAsync();
        }

        public int Delete(int id)
        {
            var employeeObj = _unitOfWork.EmployeeRepository.GetById(id);
            _unitOfWork.EmployeeRepository.Delete(employeeObj.Id);
            _unitOfWork.CommitAsync();
            return employeeObj.Id;
        }

        public IEnumerable<IndexEmployeeVM.GetData> GetAll()
        {
            return _unitOfWork.EmployeeRepository.GetAll();
        }

        public EditEmployeeVM GetById(int id)
        {
            return _unitOfWork.EmployeeRepository.GetById(id);
        }

        public List<Employee> GetEmployeesAssetOwnerByHospitalId(int hospitalId)
        {
            return _unitOfWork.EmployeeRepository.GetEmployeesByHospitalId(hospitalId);
        }

        public List<Employee> GetEmployeesAssetOwnerByHospitalId(int hospitalId, int assetDetailId)
        {
            return _unitOfWork.EmployeeRepository.GetEmployeesAssetOwnerByHospitalId(hospitalId,assetDetailId);
        }

        public List<Employee> GetEmployeesByHospitalId(int hospitalId)
        {
            return _unitOfWork.EmployeeRepository.GetEmployeesByHospitalId(hospitalId);
        }

        public List<EmployeeEngVM> GetEmployeesEngineersByHospitalId(int hospitalId)
        {
            return _unitOfWork.EmployeeRepository.GetEmployeesEngineersByHospitalId(hospitalId);
        }

        public List<EmployeeEngVM> GetEmployeesHasEngDepManagerRoleInHospital(int hospitalId)
        {
            return _unitOfWork.EmployeeRepository.GetEmployeesHasEngDepManagerRoleInHospital(hospitalId);
        }

        public List<EmployeeEngVM> GetEmployeesHasEngRoleInHospital(int hospitalId)
        {
            return _unitOfWork.EmployeeRepository.GetEmployeesHasEngRoleInHospital(hospitalId);
        }

        public IEnumerable<IndexEmployeeVM.GetData> SortEmployee(SortEmployeeVM sortObj)
        {
            return _unitOfWork.EmployeeRepository.SortEmployee(sortObj);
        }

        public int Update(EditEmployeeVM employeeObj)
        {
            _unitOfWork.EmployeeRepository.Update(employeeObj);
          //  _unitOfWork.CommitAsync();
            return employeeObj.Id;
        }
    }
}
