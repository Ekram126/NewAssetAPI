using Asset.Domain;
using Asset.Domain.Services;
using Asset.ViewModels.WorkOrderPeriorityVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
    public class WorkOrderPeriorityService : IWorkOrderPeriorityService
    {
        private IUnitOfWork _unitOfWork;

        public WorkOrderPeriorityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void AddWorkOrderPeriority(CreateWorkOrderPeriorityVM createWorkOrderPeriorityVM)
        {
            _unitOfWork.WorkOrderPeriority.Add(createWorkOrderPeriorityVM);
        }

        public void DeleteWorkOrderPeriority(int id)
        {
            _unitOfWork.WorkOrderPeriority.Delete(id);
        }

        public IEnumerable<IndexWorkOrderPeriorityVM> GetAllWorkOrderPeriorities()
        {
            return _unitOfWork.WorkOrderPeriority.GetAll();
        }

        public IndexWorkOrderPeriorityVM GetWorkOrderPeriorityById(int id)
        {
            return _unitOfWork.WorkOrderPeriority.GetById(id);
        }

        public void UpdateWorkOrderPeriority(int id, EditWorkOrderPeriorityVM editWorkOrderPeriorityVM)
        {
            _unitOfWork.WorkOrderPeriority.Update(id, editWorkOrderPeriorityVM);
        }
    }
}
