using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.WorkOrderTrackingVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
    public class WorkOrderAssignService : IWorkOrderAssignService
    {
        private IUnitOfWork _unitOfWork;

        public WorkOrderAssignService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Add(WorkOrderAssign model)
        {
            return _unitOfWork.WorkOrderAssignRepository.Add(model);
        }

        public int Delete(int id)
        {
            return _unitOfWork.WorkOrderAssignRepository.Delete(id);
        }

        public IEnumerable<WorkOrderAssign> GetAllWorkOrderAssignsByWorkOrederTrackId(int wotrackId)
        {
            return _unitOfWork.WorkOrderAssignRepository.GetAllWorkOrderAssignsByWorkOrederTrackId(wotrackId);
        }

        public WorkOrderAssign GetById(int id)
        {
            return _unitOfWork.WorkOrderAssignRepository.GetById(id);
        }

        public int Update(WorkOrderAssign model)
        {
            return _unitOfWork.WorkOrderAssignRepository.Update(model);
        }
    }
}
