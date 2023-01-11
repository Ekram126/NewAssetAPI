using Asset.Domain;
using Asset.Domain.Services;
using Asset.ViewModels.WorkOrderStatusVM;
using Asset.ViewModels.WorkOrderVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
    public class WorkOrderStatusService: IWorkOrderStatusService
    {
        private IUnitOfWork _unitOfWork;

        public WorkOrderStatusService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void AddWorkOrderStatus(CreateWorkOrderStatusVM createWorkOrderStatusVM)
        {
            _unitOfWork.WorkOrderStatus.Add(createWorkOrderStatusVM);
        }

        public void DeleteWorkOrderStatus(int id)
        {
            _unitOfWork.WorkOrderStatus.Delete(id);
        }

        public IndexWorkOrderStatusVM GetAll(string userId)
        {
            return _unitOfWork.WorkOrderStatus.GetAll(userId);
        }

        public IEnumerable<IndexWorkOrderStatusVM> GetAllWorkOrderStatuses()
        {
            return _unitOfWork.WorkOrderStatus.GetAll();
        }

        public IndexWorkOrderStatusVM GetWorkOrderStatusById(int id)
        {
            return _unitOfWork.WorkOrderStatus.GetById(id);
        }

        public void UpdateWorkOrderStatus(int id, EditWorkOrderStatusVM editWorkOrderStatusVM)
        {
            _unitOfWork.WorkOrderStatus.Update(id, editWorkOrderStatusVM);
        }

        public IEnumerable<IndexWorkOrderStatusVM> SortWOStatuses(SortWorkOrderStatusVM sortObj)
        {
            return _unitOfWork.WorkOrderStatus.SortWOStatuses(sortObj);
        }

        public IndexWorkOrderStatusVM GetAllForReport(SearchWorkOrderByDateVM woDateObj)
        {
              return _unitOfWork.WorkOrderStatus.GetAllForReport(woDateObj);
        }
    }
}

