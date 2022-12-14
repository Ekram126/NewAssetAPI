using Asset.Models;
using Asset.ViewModels.RequestTrackingVM;
using Asset.ViewModels.RequestVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Repositories
{
    public interface IRequestRepository
    {
        IEnumerable<IndexRequestsVM> GetAll();
        IEnumerable<IndexRequestVM.GetData> GetAllRequestsWithTrackingByUserId(string userId);
        IEnumerable<IndexRequestVM.GetData> GetAllRequestsByStatusId(string userId, int statusId);
        IEnumerable<IndexRequestVM.GetData> ExportRequestByStatusId(int? hospitalId, string userId, int statusId);
        IEnumerable<IndexRequestVM.GetData> GetAllRequestsByStatusId(string userId, int statusId, int page, int pageSize);
        IEnumerable<IndexRequestVM.GetData> GetRequestsByUserIdAssetId(string userId, int assetId);
        IEnumerable<IndexRequestVM.GetData> ExportRequestsByStatusId(string userId, int statusId);
        IEnumerable<IndexRequestVM.GetData> GetAllRequestsByHospitalId(int hospitalId);
        IEnumerable<IndexRequestVM.GetData> GetAllRequestsByHospitalUserId(int hospitalId, string userId);
        IEnumerable<IndexRequestVM.GetData> GetAllRequestsByAssetId(int assetId, int hospitalId);
        IEnumerable<IndexRequestVM.GetData> GetAllRequestsByHospitalAssetId(int assetId);
        List<ReportRequestVM> GetRequestEstimationById(int id);
        List<ReportRequestVM> GetRequestEstimations(SearchRequestDateVM searchRequestDateObj);
        IndexRequestsVM GetRequestByWorkOrderId(int workOrderId);
        int GetTotalRequestForAssetInHospital(int assetDetailId);
        int GetTotalOpenRequest(string userId);
        List<Request> ListOpenRequests(int hospitalId);
        List<IndexRequestVM.GetData> ListNewRequests(int hospitalId);
        List<IndexRequestTracking> ListOpenRequestTracks(int hospitalId);
        int UpdateOpenedRequest(int requestId);
        int UpdateOpenedRequestTrack(int trackId);
        IndexRequestsVM GetById(int id);
        IndexRequestsVM GetByRequestCode(string code);
        int Add(CreateRequestVM createRequestVM);
        void Update(EditRequestVM editRequestVM);
        void Delete(int id);
        PrintServiceRequestVM PrintServiceRequestById(int id);
        GeneratedRequestNumberVM GenerateRequestNumber();
        IEnumerable<IndexRequestVM.GetData> SearchRequests(SearchRequestVM searchObj);


        IndexRequestVM SearchInRequests(SearchRequestVM searchObj, int pageNumber, int pageSize);



        Task<IEnumerable<IndexRequestsVM>> SortRequests(SortRequestVM sortObj, int statusId);
        IEnumerable<IndexRequestsVM> SortRequestsByAssetId(SortRequestVM sortObj);
        IEnumerable<IndexRequestVM.GetData> GetRequestsByDate(SearchRequestDateVM requestDateObj);
        IndexRequestVM GetRequestsByDateAndStatus(SearchRequestDateVM requestDateObj, int pageNumber, int pageSize);


        List<IndexRequestVM.GetData> GetRequestsByDateAndStatus(SearchRequestDateVM requestDateObj);



        int CountRequestsByHospitalId(int hospitalId, string userId);
        int CreateRequestAttachments(RequestDocument attachObj);
        Task<List<IndexRequestsVM>> SortRequestsByPaging(SortRequestVM sortObj, int statusId, int pageNumber, int pageSize);
        List<IndexRequestVM.GetData> GetRequestsByStatusIdAndPaging(string userId, int statusId, int pageNumber, int pageSize);
        int GetRequestsCountByStatusIdAndPaging(string userId, int statusId);

        List<IndexRequestVM.GetData> AlertOpenedRequestAssetsAndHighPeriority(int periorityId,int hospitalId);


        List<IndexRequestVM.GetData> PrintListOfRequests(List<ExportRequestVM> requests);






        OpenRequestVM ListOpenRequests(SearchOpenRequestVM searchOpenRequestObj, int pageNumber, int pageSize);


        List<OpenRequestVM.GetData> ListOpenRequestsPDF(SearchOpenRequestVM searchOpenRequestObj);


    }
}
