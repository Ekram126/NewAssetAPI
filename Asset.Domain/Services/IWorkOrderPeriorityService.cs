using Asset.ViewModels.WorkOrderPeriorityVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
    public interface IWorkOrderPeriorityService
    {
        IEnumerable<IndexWorkOrderPeriorityVM> GetAllWorkOrderPeriorities();
        IndexWorkOrderPeriorityVM GetWorkOrderPeriorityById(int id);
        void AddWorkOrderPeriority(CreateWorkOrderPeriorityVM createWorkOrderPeriorityVM);
        void UpdateWorkOrderPeriority(int id, EditWorkOrderPeriorityVM editWorkOrderPeriorityVM);
        void DeleteWorkOrderPeriority(int id);
    }
}
