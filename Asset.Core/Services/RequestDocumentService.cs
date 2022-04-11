using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.RequestDocumentVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
    public class RequestDocumentService : IRequestDocumentService
    {
        private IUnitOfWork _unitOfWork;

        public RequestDocumentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void AddRequestDocument(List<CreateRequestDocument> requestDocuments)
        {
            _unitOfWork.RequestDocument.Add(requestDocuments);
        }

        public void DeleteRequestDocument(int id)
        {
            _unitOfWork.RequestDocument.DeleteRequestDocument(id);
        }

        public IEnumerable<IndexRequestDocument> GetAllRequestDocument()
        {
            return _unitOfWork.RequestDocument.GetAll();
        }

        public RequestDocument GetLastDocumentForRequestTrackingId(int RequestTrackingId)
        {
            return _unitOfWork.RequestDocument.GetLastDocumentForRequestTrackingId(RequestTrackingId);
        }

        public IndexRequestDocument GetRequestDocumentById(int id)
        {
            return _unitOfWork.RequestDocument.GetById(id);
        }

        public IEnumerable<IndexRequestDocument> GetRequestDocumentsByRequestTrackingId(int RequestTrackingId)
        {
            return _unitOfWork.RequestDocument.GetRequestDocumentsByRequestTrackingId(RequestTrackingId);
        }
    }
}
