using Asset.Models;
using Asset.ViewModels.WorkOrderAttachmentVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
    public interface IWorkOrderAttachmentService
    {
        IEnumerable<IndexWorkOrderAttachmentVM> GetAllWorkOrderAttachment();
        IndexWorkOrderAttachmentVM GetWorkOrderAttachmentById(int id);
        IEnumerable<IndexWorkOrderAttachmentVM> GetWorkOrderAttachmentsByWorkOrderTrackingId(int trackId);
        void AddWorkOrderAttachment(List<CreateWorkOrderAttachmentVM> WorkOrderAttachments);
        void DeleteWorkOrderAttachment(int id);
        WorkOrderAttachment GetLastDocumentForWorkOrderTrackingId(int workOrderTrackingId);
    }
}
