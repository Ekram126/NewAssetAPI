using System.Collections.Generic;
using Asset.Models;
using Asset.ViewModels.DepartmentVM;


namespace Asset.Domain.Services
{
  public  interface IDepartmentService
    {
        IEnumerable<Department> GetAllDepartments();
        IEnumerable<IndexDepartmentVM.GetData> GetAll();
        IEnumerable<Department> GetDepartmentsByHospitalId(int hospitalId);
        EditDepartmentVM GetById(int id);
        int Add(CreateDepartmentVM Department); 
        int Update(EditDepartmentVM Department);
        int Delete(int id);

        IEnumerable<IndexDepartmentVM.GetData> SortDepartments(SortDepartmentVM sortObj);
    }
}
