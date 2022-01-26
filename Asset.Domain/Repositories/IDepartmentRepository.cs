using System.Collections.Generic;
using Asset.Models;
using Asset.ViewModels.DepartmentVM;


namespace Asset.Domain.Repositories
{
  public  interface IDepartmentRepository
    {
        IEnumerable<Department> GetAllDepartments();
        IEnumerable<IndexDepartmentVM.GetData> GetAll();
        EditDepartmentVM GetById(int id);


        int Add(CreateDepartmentVM Department); 
        int Update(EditDepartmentVM Department);
        int Delete(int id);


        IEnumerable<IndexDepartmentVM.GetData> SortDepartments(SortDepartmentVM sortObj);
    }
}
