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
        IEnumerable<IndexRequestTracking> GetAllRequestTracking(string UserId, int assetDetailId);
        IndexRequestTracking GetRequestTrackingById(int id);
        RequestDetails GetAllTrackingsByRequestId(int RequestId);
        List<RequestTrackingView> GetRequestTracksByRequestId(int requestId);
        int CountRequestTracksByRequestId(int requestId);
        int AddRequestTracking(CreateRequestTracking createRequestTracking);
        void UpdateRequestTracking(int id, EditRequestTracking editRequestTracking);
        void DeleteRequestTracking(int id);
    }
}
