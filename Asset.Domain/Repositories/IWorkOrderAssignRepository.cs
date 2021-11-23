using System.Collections.Generic;
using Asset.Models;
using Asset.ViewModels.CityVM;

namespace Asset.Domain.Services
{
  public  interface IWorkOrderAssignService
    {
        IEnumerable<WorkOrderAssign> GetAllWorkOrderAssignsByWorkOrederTrackId(int wotrackId);
        WorkOrderAssign GetById(int id);
        int Add(WorkOrderAssign model);
        int Update(WorkOrderAssign model);
        int Delete(int id);
    }
}
