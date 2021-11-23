using Asset.ViewModels.WorkOrderTaskVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Repositories
{
    public interface IWorkOrderTaskRepository
    {
        IEnumerable<IndexWorkOrderTaskVM> GetAll();
        IEnumerable<IndexWorkOrderTaskVM> GetAllWorkOrderTaskByWorkOrderId(int WorkOrderId);
        IndexWorkOrderTaskVM GetById(int id);
        void Add(CreateWorkOrderTaskVM createWorkOrderTaskVM);
        void Update(int id, EditWorkOrderTaskVM editWorkOrderTaskVM);
        void Delete(int id);
    }
}
