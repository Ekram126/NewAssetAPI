using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.WorkOrderAttachmentVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
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
                        workOrderAttachment.HospitalId = item.HospitalId;
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
                    var folderName = Path.Combine("UploadedAttachments", "WorkOrderFiles");
                    var folderPathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    var filePath = folderPathToSave + "/" + workOrderAttachment.FileName;
                    FileInfo file = new FileInfo(filePath);
                    if (file.Exists)
                    {
                        file.Delete();
                    }

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
                HospitalId = req.HospitalId,
                WorkOrderTrackingId = int.Parse(req.WorkOrderTrackingId.ToString())
            }).ToList();
        }

        public IndexWorkOrderAttachmentVM GetById(int id)
        {
            return _context.WorkOrderAttachments.Where(w => w.Id == id).Include(r => r.WorkOrderTracking.WorkOrder).Select(req => new IndexWorkOrderAttachmentVM
            {
                Id = req.Id,
                FileName = req.FileName,
                DocumentName = req.DocumentName,
                HospitalId = req.HospitalId,
                WorkOrderTrackingId = int.Parse(req.WorkOrderTrackingId.ToString())
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
                HospitalId= doc.HospitalId,
                WorkOrderTrackingId = int.Parse(doc.WorkOrderTrackingId.ToString())
            }).ToList();
        }
    }
}
