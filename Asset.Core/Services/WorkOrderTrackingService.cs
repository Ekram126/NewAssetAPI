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
    public class WorkOrderTrackingService : IWorkOrderTrackingService
    {
        private IUnitOfWork _unitOfWork;

        public WorkOrderTrackingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public int AddWorkOrderTracking(CreateWorkOrderTrackingVM createWorkOrderTrackingVM)
        {
            _unitOfWork.WorkOrderTracking.Add(createWorkOrderTrackingVM);
            return createWorkOrderTrackingVM.Id;
        }

        public void DeleteWorkOrderTracking(int id)
        {
            _unitOfWork.WorkOrderTracking.Delete(id);
        }

        public IEnumerable<WorkOrderTracking> GetAll()
        {
            return _unitOfWork.WorkOrderTracking.GetAll();
        }

        public WorkOrderDetails GetAllWorkOrderByWorkOrderId(int WorkOrderId)
        {
            return _unitOfWork.WorkOrderTracking.GetAllWorkOrderByWorkOrderId(WorkOrderId);
        }

        public IEnumerable<LstWorkOrderFromTracking> GetAllWorkOrderFromTrackingByServiceRequestId(int ServiceRequestId, string userId)
        {
            return _unitOfWork.WorkOrderTracking.GetAllWorkOrderFromTrackingByServiceRequestId(ServiceRequestId, userId);
        }

        public IEnumerable<LstWorkOrderFromTracking> GetAllWorkOrderFromTrackingByServiceRequestUserId(int ServiceRequestId, string userId)
        {
            return _unitOfWork.WorkOrderTracking.GetAllWorkOrderFromTrackingByServiceRequestUserId(ServiceRequestId, userId);
        }

        public IEnumerable<LstWorkOrderFromTracking> GetAllWorkOrderFromTrackingByUserId(string userId)
        {
            return _unitOfWork.WorkOrderTracking.GetAllWorkOrderFromTrackingByUserId(userId);
        }

        public List<IndexWorkOrderTrackingVM> GetAllWorkOrderTrackingByWorkOrderId(int WorkOrderId)
        {
            return _unitOfWork.WorkOrderTracking.GetAllWorkOrderTrackingByWorkOrderId(WorkOrderId);
        }

        public List<WorkOrderAttachment> GetAttachmentsByWorkOrderId(int id)
        {
              return _unitOfWork.WorkOrderTracking.GetAttachmentsByWorkOrderId(id);
        }

        public LstWorkOrderFromTracking GetEngManagerWhoFirstAssignedWO(int woId)
        {
            return _unitOfWork.WorkOrderTracking.GetEngManagerWhoFirstAssignedWO(woId);
        }

        public WorkOrderTracking GetFirstTrackForWorkOrderByWorkOrderId(int woId)
        {
            return _unitOfWork.WorkOrderTracking.GetFirstTrackForWorkOrderByWorkOrderId(woId);
        }

        public List<IndexWorkOrderTrackingVM> GetTrackOfWorkOrderByWorkOrderId(int workOrderId)
        {
            return _unitOfWork.WorkOrderTracking.GetTrackOfWorkOrderByWorkOrderId(workOrderId);
        }

        public IndexWorkOrderTrackingVM GetWorkOrderTrackingById(int id)
        {
            return _unitOfWork.WorkOrderTracking.GetById(id);
        }

        public void UpdateWorkOrderTracking(int id, EditWorkOrderTrackingVM editWorkOrderTrackingVM)
        {
            _unitOfWork.WorkOrderTracking.Update(id, editWorkOrderTrackingVM);
        }


        public void UpdateWorkOrderTracking(EditWorkOrderTrackingVM editWorkOrderTrackingVM)
        {
            _unitOfWork.WorkOrderTracking.Update(editWorkOrderTrackingVM);
        }
    }
}
