using Asset.ViewModels.WorkOrderStatusVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
    public interface IWorkOrderStatusService
    {
        IEnumerable<IndexWorkOrderStatusVM> GetAllWorkOrderStatuses();
        IndexWorkOrderStatusVM GetWorkOrderStatusById(int id);
        void AddWorkOrderStatus(CreateWorkOrderStatusVM createWorkOrderStatusVM);
        void UpdateWorkOrderStatus(int id, EditWorkOrderStatusVM editWorkOrderStatusVM);
        void DeleteWorkOrderStatus(int id);
    }
}
