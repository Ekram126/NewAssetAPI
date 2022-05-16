using Asset.Models;
using Asset.ViewModels.RequestTrackingVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
    public interface IRequestTrackingService
    {
        IEnumerable<RequestTracking> GetAll();
        IEnumerable<IndexRequestTracking> GetAllRequestTracking(string UserId, int assetDetailId);
        RequestTracking GetFirstTrackForRequestByRequestId(int requestId);
        RequestTracking GetLastTrackForRequestByRequestId(int requestId);
        IndexRequestTracking GetRequestTrackingById(int id);
        RequestDetails GetAllTrackingsByRequestId(int RequestId);
        List<RequestTrackingView> GetRequestTracksByRequestId(int requestId);
        int CountRequestTracksByRequestId(int requestId);
        int AddRequestTracking(CreateRequestTracking createRequestTracking);
        void UpdateRequestTracking(EditRequestTracking editRequestTracking);
        void DeleteRequestTracking(int id);
    }
}
