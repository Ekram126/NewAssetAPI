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
        IEnumerable<IndexRequestTracking> GetAll(string UserId, int? assetDetailId);
        IndexRequestTracking GetById(int id);
        RequestDetails GetAllTrackingsByRequestId(int RequestId);
        List<RequestTrackingView> GetRequestTracksByRequestId(int requestId);
        int CountRequestTracksByRequestId(int requestId);
        int Add(CreateRequestTracking createRequestTracking);
        void Update(int id, EditRequestTracking editRequestTracking);
        void Delete(int id);
    }
}
