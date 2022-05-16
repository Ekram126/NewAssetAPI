using Asset.Models;
using Asset.ViewModels.RequestTrackingVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Repositories
{
    public interface IRequestTrackingRepository
    {
        IEnumerable<RequestTracking> GetAll();
        IEnumerable<IndexRequestTracking> GetAll(string UserId, int? assetDetailId);
        RequestTracking GetFirstTrackForRequestByRequestId(int requestId);
        RequestTracking GetLastTrackForRequestByRequestId(int requestId);
        IndexRequestTracking GetById(int id);
        RequestDetails GetAllTrackingsByRequestId(int RequestId);
        List<RequestTrackingView> GetRequestTracksByRequestId(int requestId);
        int CountRequestTracksByRequestId(int requestId);
        int Add(CreateRequestTracking createRequestTracking);
        void Update(EditRequestTracking editRequestTracking);
        void Delete(int id);
    }
}
