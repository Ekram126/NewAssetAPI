using Asset.Models;
using Asset.ViewModels.WorkOrderTrackingVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
    public interface IWorkOrderTrackingService
    {
        IEnumerable<LstWorkOrderFromTracking> GetAllWorkOrderFromTrackingByServiceRequestId(int ServiceRequestId,string userId);
        IEnumerable<LstWorkOrderFromTracking> GetAllWorkOrderFromTrackingByServiceRequestUserId(int ServiceRequestId, string userId);
        IEnumerable<LstWorkOrderFromTracking> GetAllWorkOrderFromTrackingByUserId(string userId);
        IndexWorkOrderTrackingVM GetWorkOrderTrackingById(int id);
        WorkOrderDetails GetAllWorkOrderByWorkOrderId(int WorkOrderId);
        List<WorkOrderAttachment> GetAttachmentsByWorkOrderId(int id);
        List<IndexWorkOrderTrackingVM> GetAllWorkOrderTrackingByWorkOrderId(int WorkOrderId);
        List<IndexWorkOrderTrackingVM> GetTrackOfWorkOrderByWorkOrderId(int workOrderId);
        int AddWorkOrderTracking(CreateWorkOrderTrackingVM createWorkOrderTrackingVM);
        void UpdateWorkOrderTracking(int id, EditWorkOrderTrackingVM editWorkOrderTrackingVM);
        void DeleteWorkOrderTracking(int id);



        LstWorkOrderFromTracking GetEngManagerWhoFirstAssignedWO(int woId);
    }
}
