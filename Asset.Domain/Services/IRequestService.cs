using Asset.ViewModels.RequestVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
    public interface IRequestService
    {
        IEnumerable<IndexRequestsVM> GetAllRequests();
        IEnumerable<IndexRequestVM.GetData> GetAllRequestsWithTrackingByUserId(string userId);
        IEnumerable<IndexRequestVM.GetData> GetAllRequestsByStatusId(string userId, int statusId);
        IEnumerable<IndexRequestVM.GetData> GetRequestsByUserIdAssetId(string userId,int assetId);
        IEnumerable<IndexRequestVM.GetData> GetAllRequestsByHospitalId(int hospitalId);
        IEnumerable<IndexRequestVM.GetData> GetAllRequestsByHospitalUserId(int hospitalId,string userId);

        int GetTotalOpenRequestInThisWeek(string userId);
        IndexRequestsVM GetRequestByWorkOrderId(int workOrderId);

        int GetTotalRequestForAssetInHospital(int assetDetailId);
        IndexRequestsVM GetRequestById(int id);
        int AddRequest(CreateRequestVM createRequestVM);
        void UpdateRequest(int Id, EditRequestVM editRequestVM);
        void DeleteRequest(int id);

        GeneratedRequestNumberVM GenerateRequestNumber();

        IEnumerable<IndexRequestVM.GetData> SearchRequests(SearchRequestVM searchObj);
        IEnumerable<IndexRequestsVM> SortRequests(SortRequestVM sortObj);
    }
}
