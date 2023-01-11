using Asset.ViewModels.WorkOrderStatusVM;
using Asset.ViewModels.WorkOrderVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
    public interface IWorkOrderStatusService
    {
       // IEnumerable<IndexWorkOrderStatusVM> GetAll(string userId);

        IndexWorkOrderStatusVM GetAll(string userId);
        IEnumerable<IndexWorkOrderStatusVM> GetAllWorkOrderStatuses();
        IndexWorkOrderStatusVM GetWorkOrderStatusById(int id);
        void AddWorkOrderStatus(CreateWorkOrderStatusVM createWorkOrderStatusVM);
        void UpdateWorkOrderStatus(int id, EditWorkOrderStatusVM editWorkOrderStatusVM);
        void DeleteWorkOrderStatus(int id);

        IEnumerable<IndexWorkOrderStatusVM> SortWOStatuses(SortWorkOrderStatusVM sortObj);
        IndexWorkOrderStatusVM GetAllForReport(SearchWorkOrderByDateVM woDateObj);

    }
}
