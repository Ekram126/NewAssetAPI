﻿using Asset.Models;
using Asset.ViewModels.EmployeeVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
    public interface IEmployeeService
    {

       IEnumerable<IndexEmployeeVM.GetData> GetAll();

        List<Employee> GetEmployeesByHospitalId(int hospitalId);
        List<Employee> GetEmployeesAssetOwnerByHospitalId(int hospitalId);
        List<Employee> GetEmployeesAssetOwnerByHospitalId(int hospitalId, int assetDetailId);
        List<EmployeeEngVM> GetEmployeesHasEngDepManagerRoleInHospital(int hospitalId);
        List<EmployeeEngVM> GetEmployeesHasEngRoleInHospital(int hospitalId);
        EditEmployeeVM GetById(int id);
        int Add(CreateEmployeeVM employeeObj);
        int Update(EditEmployeeVM employeeObj);
        int Delete(int id);
    }
}
