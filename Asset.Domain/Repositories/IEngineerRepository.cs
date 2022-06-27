using Asset.Models;
using Asset.ViewModels.EngineerVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Repositories
{
    public interface IEngineerRepository
    {

        IEnumerable<IndexEngineerVM.GetData> GetAll();

        IEnumerable<Engineer> GetAllEngineers();
        Engineer GetById(int id);
        Engineer GetByEmail(string email);
        int Add(CreateEngineerVM EngineerObj);
        int Update(EditEngineerVM EngineerObj);
        int Delete(int id);
        IEnumerable<IndexEngineerVM.GetData>SortEngineer(SortEngineerVM sortObj);
    }
}
