using Asset.ViewModels.WorkOrderTypeVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
    public interface IWorkOrderTypeService
    {
        IEnumerable<IndexWorkOrderTypeVM> GetAllWorkOrderTypes();
        IndexWorkOrderTypeVM GetWorkOrderTypeById(int id);
        void AddWorkOrderType(CreateWorkOrderTypeVM createWorkOrderTypeVM);
        void UpdateWorkOrderType(int id, EditWorkOrderTypeVM editWorkOrderTypeVM);
        void DeleteWorkOrderType(int id);

        IEnumerable<IndexWorkOrderTypeVM> SortWorkOrderTypes(SortWorkOrderTypeVM sortObj);
    }
}
