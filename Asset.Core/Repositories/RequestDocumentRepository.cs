using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.RequestDocumentVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class RequestDocumentRepository : IRequestDocumentRepository
    {
        private readonly ApplicationDbContext _context;
        private string msg;

        public RequestDocumentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(List<CreateRequestDocument> requestDocuments)
        {
            try
            {
                if (requestDocuments != null)
                {
                    foreach (var item in requestDocuments)
                    {
                        RequestDocument requestDocument = new RequestDocument();
                       // requestDocument.Id = item.Id;
                        requestDocument.FileName = item.FileName;
                        requestDocument.DocumentName = item.DocumentName;
                        requestDocument.RequestTrackingId = item.RequestTrackingId;
                        _context.Add(requestDocument);
                        _context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }

        public void DeleteRequestDocument(int id)
        {
            RequestDocument requestDocument=_context.RequestDocument.Find(id);
            try
            {
                if (requestDocument != null)
                {
                    _context.RequestDocument.Remove(requestDocument);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }

        public IEnumerable<IndexRequestDocument> GetAll()
        {
            return _context.RequestDocument.Include(r=>r.RequestTracking.Request).Select(req => new IndexRequestDocument
            {
                Id = req.Id,
                FileName = req.FileName,
                DocumentName = req.DocumentName,
                RequestTrackingId = req.RequestTrackingId,
                Subject = req.RequestTracking.Request.Subject
            }).ToList();
        }

        public IndexRequestDocument GetById(int id)
        {
            return _context.RequestDocument.Where(r=>r.Id==id).Include(r => r.RequestTracking.Request).Select(req => new IndexRequestDocument
            {
                Id = req.Id,
                FileName = req.FileName,
                DocumentName = req.DocumentName,
                RequestTrackingId = req.RequestTrackingId,
                Subject = req.RequestTracking.Request.Subject
            }).FirstOrDefault();
        }

        public RequestDocument GetLastDocumentForRequestTrackingId(int RequestTrackingId)
        {
            RequestDocument documentObj = new RequestDocument();
            var lstDocuments = _context.RequestDocument.Where(a => a.RequestTrackingId == RequestTrackingId).ToList();
            if(lstDocuments.Count >0)
            {
                documentObj = lstDocuments.Last();
            }
            return documentObj;
        }

        public IEnumerable<IndexRequestDocument> GetRequestDocumentsByRequestTrackingId(int RequestTrackingId)
        {
            return _context.RequestDocument.Include(r => r.RequestTracking.Request).Where(req=>req.RequestTrackingId== RequestTrackingId).OrderBy(a=>a.FileName).Select(req => new IndexRequestDocument
            {
                Id = req.Id,
                FileName = req.FileName,
                DocumentName = req.DocumentName,
                RequestTrackingId = req.RequestTrackingId,
                Subject = req.RequestTracking.Request.Subject
            }).ToList();
        }
    }
}
