using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.WorkOrderAttachmentVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class WorkOrderAttachmentRepository : IWorkOrderAttachmentRepository
    {
        private readonly ApplicationDbContext _context;
        private string msg;

        public WorkOrderAttachmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(List<CreateWorkOrderAttachmentVM> WorkOrderAttachments)
        {
            try
            {
                if (WorkOrderAttachments != null)
                {
                    foreach (var item in WorkOrderAttachments)
                    {
                        WorkOrderAttachment workOrderAttachment = new WorkOrderAttachment();
                        workOrderAttachment.FileName = item.FileName;
                        workOrderAttachment.DocumentName = item.DocumentName;
                        workOrderAttachment.WorkOrderTrackingId = item.WorkOrderTrackingId;
                        _context.Add(workOrderAttachment);
                        _context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }

        public void DeleteWorkOrderAttachment(int id)
        {
            var workOrderAttachment = _context.WorkOrderAttachments.Find(id);
            try
            {
                if (workOrderAttachment != null)
                {
                    _context.WorkOrderAttachments.Remove(workOrderAttachment);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }

        public IEnumerable<IndexWorkOrderAttachmentVM> GetAll()
        {
            return _context.WorkOrderAttachments.Include(r => r.WorkOrderTracking.WorkOrder).Select(req => new IndexWorkOrderAttachmentVM
            {
                Id = req.Id,
                FileName = req.FileName,
                DocumentName = req.DocumentName,
                WorkOrderTrackingId = req.WorkOrderTrackingId,
            }).ToList();
        }

        public IndexWorkOrderAttachmentVM GetById(int id)
        {
            return _context.WorkOrderAttachments.Where(w=>w.Id==id).Include(r => r.WorkOrderTracking.WorkOrder).Select(req => new IndexWorkOrderAttachmentVM
            {
                Id = req.Id,
                FileName = req.FileName,
                DocumentName = req.DocumentName,
                WorkOrderTrackingId = req.WorkOrderTrackingId,
            }).FirstOrDefault();
        }

        public WorkOrderAttachment GetLastDocumentForWorkOrderTrackingId(int workOrderTrackingId)
        {
            WorkOrderAttachment documentObj = new WorkOrderAttachment();
            var lstDocuments = _context.WorkOrderAttachments.Where(a => a.WorkOrderTrackingId == workOrderTrackingId).ToList();
            if (lstDocuments.Count > 0)
            {
                documentObj = lstDocuments.Last();
            }
            return documentObj;
        }

        public IEnumerable<IndexWorkOrderAttachmentVM> GetWorkOrderAttachmentsByWorkOrderTrackingId(int WorkOrderTrackingId)
        {
            return _context.WorkOrderAttachments.Where(w => w.WorkOrderTrackingId == WorkOrderTrackingId).Include(r => r.WorkOrderTracking.WorkOrder).Select(doc => new IndexWorkOrderAttachmentVM
            {
                Id = doc.Id,
                FileName = doc.FileName,
                DocumentName = doc.DocumentName,
                WorkOrderTrackingId = doc.WorkOrderTrackingId,
            }).ToList();
        }
    }
}
