using Asset.Models;
using Asset.ViewModels.RequestDocumentVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
    public interface IRequestDocumentService
    {
        IEnumerable<IndexRequestDocument> GetAllRequestDocument();
        IndexRequestDocument GetRequestDocumentById(int id);
        IEnumerable<IndexRequestDocument> GetRequestDocumentsByRequestTrackingId(int RequestTrackingId);
        void AddRequestDocument(List<CreateRequestDocument> requestDocuments);
        public void DeleteRequestDocument(int id);
        RequestDocument GetLastDocumentForRequestTrackingId(int RequestTrackingId);
    }
}
