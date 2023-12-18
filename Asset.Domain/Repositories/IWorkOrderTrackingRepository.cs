using Asset.Models;
using Asset.ViewModels.WorkOrderTrackingVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Repositories
{
    public interface IWorkOrderTrackingRepository
    {
        IEnumerable<WorkOrderTracking> GetAll();
        IEnumerable<LstWorkOrderFromTracking> GetAllWorkOrderFromTrackingByServiceRequestId(int ServiceRequestId, string userId);
        IEnumerable<LstWorkOrderFromTracking> GetAllWorkOrderFromTrackingByServiceRequestUserId(int ServiceRequestId, string userId);
        IEnumerable<LstWorkOrderFromTracking> GetAllWorkOrderFromTrackingByUserId(string userId);
        List<IndexWorkOrderTrackingVM> GetAllWorkOrderTrackingByWorkOrderId(int WorkOrderId);

        WorkOrderTracking GetFirstTrackForWorkOrderByWorkOrderId(int woId);

        List<IndexWorkOrderTrackingVM> GetTrackOfWorkOrderByWorkOrderId(int workOrderId);
        List<WorkOrderAttachment> GetAttachmentsByWorkOrderId(int id);
        WorkOrderDetails GetAllWorkOrderByWorkOrderId(int WorkOrderId);


        IndexWorkOrderTrackingVM GetById(int id);
        int Add(CreateWorkOrderTrackingVM createWorkOrderTrackingVM);
        void Update(int id, EditWorkOrderTrackingVM editWorkOrderTrackingVM);
        void Update(EditWorkOrderTrackingVM editWorkOrderTrackingVM);
        void Delete(int id);

        LstWorkOrderFromTracking GetEngManagerWhoFirstAssignedWO(int woId);
    }
}
