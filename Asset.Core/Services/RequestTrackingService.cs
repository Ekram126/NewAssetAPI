using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.RequestTrackingVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
     public class RequestTrackingService : IRequestTrackingService
    {
        private IUnitOfWork _unitOfWork;

        public RequestTrackingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public int AddRequestTracking(CreateRequestTracking createRequestTracking)
        {
            return _unitOfWork.RequestTracking.Add(createRequestTracking);
        }

        public int CountRequestTracksByRequestId(int requestId)
        {
            return _unitOfWork.RequestTracking.CountRequestTracksByRequestId(requestId);
        }

        public void DeleteRequestTracking(int id)
        {
            _unitOfWork.RequestTracking.Delete(id);
        }

        public IEnumerable<RequestTracking> GetAll()
        {
            return _unitOfWork.RequestTracking.GetAll();
        }

        public IEnumerable<IndexRequestTracking> GetAllRequestTracking(string UserId, int assetDetailId)
        {
            return _unitOfWork.RequestTracking.GetAll(UserId,assetDetailId);
        }

        public RequestDetails GetAllTrackingsByRequestId(int RequestId)
        {
            return _unitOfWork.RequestTracking.GetAllTrackingsByRequestId(RequestId);
        }

        public RequestTracking GetFirstTrackForRequestByRequestId(int requestId)
        {
            return _unitOfWork.RequestTracking.GetFirstTrackForRequestByRequestId(requestId);
        }

        public RequestTracking GetLastTrackForRequestByRequestId(int requestId)
        {
            return _unitOfWork.RequestTracking.GetLastTrackForRequestByRequestId(requestId);
        }

        public IndexRequestTracking GetRequestTrackingById(int id)
        {
            return _unitOfWork.RequestTracking.GetById(id);
        }

        public List<RequestTrackingView> GetRequestTracksByRequestId(int requestId)
        {
                 
            return _unitOfWork.RequestTracking.GetRequestTracksByRequestId(requestId);
        }

        public void UpdateRequestTracking(EditRequestTracking editRequestTracking)
        {
            _unitOfWork.RequestTracking.Update(editRequestTracking);
        }
    }
}
