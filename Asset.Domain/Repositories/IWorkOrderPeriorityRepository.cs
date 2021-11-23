using Asset.ViewModels.WorkOrderPeriorityVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Repositories
{
    public interface IWorkOrderPeriorityRepository
    {
        IEnumerable<IndexWorkOrderPeriorityVM> GetAll();
        IndexWorkOrderPeriorityVM GetById(int id);
        void Add(CreateWorkOrderPeriorityVM createWorkOrderPeriorityVM);
        void Update(int id, EditWorkOrderPeriorityVM editWorkOrderPeriorityVM);
        void Delete(int id);
    }
}
