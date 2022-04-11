using Asset.Models;
using Asset.ViewModels.WorkOrderAttachmentVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Repositories
{
    public interface IWorkOrderAttachmentRepository
    {
        IEnumerable<IndexWorkOrderAttachmentVM> GetAll();
        IndexWorkOrderAttachmentVM GetById(int id);
        IEnumerable<IndexWorkOrderAttachmentVM> GetWorkOrderAttachmentsByWorkOrderTrackingId(int WorkOrderTrackingId);
        void Add(List<CreateWorkOrderAttachmentVM> WorkOrderAttachments);
        void DeleteWorkOrderAttachment(int id);

        WorkOrderAttachment GetLastDocumentForWorkOrderTrackingId(int workOrderTrackingId);
    }
}
