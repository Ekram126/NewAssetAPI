using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.WorkOrderAttachmentVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
    public class WorkOrderAttachmentService : IWorkOrderAttachmentService
    {
        private IUnitOfWork _unitOfWork;

        public WorkOrderAttachmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void AddWorkOrderAttachment(List<CreateWorkOrderAttachmentVM> WorkOrderAttachments)
        {
            _unitOfWork.WorkOrderAttachment.Add(WorkOrderAttachments);
        }

        public void DeleteWorkOrderAttachment(int id)
        {
            _unitOfWork.WorkOrderAttachment.DeleteWorkOrderAttachment(id);
        }

        public IEnumerable<IndexWorkOrderAttachmentVM> GetAllWorkOrderAttachment()
        {
           return _unitOfWork.WorkOrderAttachment.GetAll();
        }

        public WorkOrderAttachment GetLastDocumentForWorkOrderTrackingId(int workOrderTrackingId)
        {
            return _unitOfWork.WorkOrderAttachment.GetLastDocumentForWorkOrderTrackingId(workOrderTrackingId);
        }

        public IndexWorkOrderAttachmentVM GetWorkOrderAttachmentById(int id)
        {
            return _unitOfWork.WorkOrderAttachment.GetById(id);
        }

        public IEnumerable<IndexWorkOrderAttachmentVM> GetWorkOrderAttachmentsByWorkOrderTrackingId(int WorkOrderTrackingId)
        {
            return _unitOfWork.WorkOrderAttachment.GetWorkOrderAttachmentsByWorkOrderTrackingId(WorkOrderTrackingId);
        }
    }
}
