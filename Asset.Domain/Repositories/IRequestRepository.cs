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
        IEnumerable<IndexRequestVM.GetData> GetRequestsByUserIdAssetId(string userId, int assetId);
        IEnumerable<IndexRequestVM.GetData> GetAllRequestsByHospitalId(int hospitalId);
        IEnumerable<IndexRequestVM.GetData> GetAllRequestsByHospitalUserId(int hospitalId, string userId);
        IEnumerable<IndexRequestVM.GetData> GetAllRequestsByAssetId(int assetId, int hospitalId);
        IEnumerable<IndexRequestVM.GetData> GetAllRequestsByHospitalAssetId(int assetId);
        IndexRequestsVM GetRequestByWorkOrderId(int workOrderId);
        int GetTotalRequestForAssetInHospital(int assetDetailId);
        int GetTotalOpenRequest(string userId);
        List<Request> ListOpenRequests(int hospitalId);
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
        Task<IEnumerable<IndexRequestsVM>> SortRequests(SortRequestVM sortObj, int statusId);
        IEnumerable<IndexRequestsVM> SortRequestsByAssetId(SortRequestVM sortObj);
        IEnumerable<IndexRequestVM.GetData> GetRequestsByDate(SearchRequestDateVM requestDateObj);
        int CountRequestsByHospitalId(int hospitalId, string userId);
       int CreateRequestAttachments(RequestDocument attachObj);
    }
}
