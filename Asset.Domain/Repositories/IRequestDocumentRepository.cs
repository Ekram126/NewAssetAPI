using Asset.Models;
using Asset.ViewModels.RequestDocumentVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Repositories
{
    public interface IRequestDocumentRepository
    {
        IEnumerable<IndexRequestDocument> GetAll();
        IndexRequestDocument GetById(int id);
        IEnumerable<IndexRequestDocument> GetRequestDocumentsByRequestTrackingId(int RequestTrackingId);
        void Add(List<CreateRequestDocument> requestDocuments);
        void DeleteRequestDocument(int id);


        RequestDocument GetLastDocumentForRequestTrackingId(int RequestTrackingId);
    }
}
