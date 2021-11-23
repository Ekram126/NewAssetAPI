using Asset.Domain;
using Asset.Domain.Services;
using Asset.ViewModels.WorkOrderTaskVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
    public class WorkOrderTaskService : IWorkOrderTaskService
    {
        private IUnitOfWork _unitOfWork;

        public WorkOrderTaskService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void AddWorkOrderTask(CreateWorkOrderTaskVM createWorkOrderTask)
        {
            _unitOfWork.WorkOrderTask.Add(createWorkOrderTask);
        }

        public void DeleteWorkOrderTask(int id)
        {
            _unitOfWork.WorkOrderTask.Delete(id);
        }

        public IEnumerable<IndexWorkOrderTaskVM> GetAllWorkOrderTask()
        {
            return _unitOfWork.WorkOrderTask.GetAll();
        }

        public IEnumerable<IndexWorkOrderTaskVM> GetAllWorkOrderTaskByWorkOrderId(int WorkOrderId)
        {
            return _unitOfWork.WorkOrderTask.GetAllWorkOrderTaskByWorkOrderId(WorkOrderId);
        }

        public IndexWorkOrderTaskVM GetWorkOrderTaskById(int id)
        {
            return _unitOfWork.WorkOrderTask.GetById(id);
        }

        public void UpdateWorkOrderTask(int id, EditWorkOrderTaskVM editWorkOrderTask)
        {
            _unitOfWork.WorkOrderTask.Update(id, editWorkOrderTask);
        }
    }
}
