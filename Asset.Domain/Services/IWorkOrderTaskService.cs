using Asset.ViewModels.WorkOrderTaskVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
    public interface IWorkOrderTaskService
    {
        IEnumerable<IndexWorkOrderTaskVM> GetAllWorkOrderTask();
        IEnumerable<IndexWorkOrderTaskVM> GetAllWorkOrderTaskByWorkOrderId(int WorkOrderId);
        IndexWorkOrderTaskVM GetWorkOrderTaskById(int id);
        void AddWorkOrderTask(CreateWorkOrderTaskVM createWorkOrderTask);
        void UpdateWorkOrderTask(int id, EditWorkOrderTaskVM editWorkOrderTask);
        void DeleteWorkOrderTask(int id);
    }
}
