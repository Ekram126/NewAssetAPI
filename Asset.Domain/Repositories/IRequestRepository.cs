using Asset.Models;
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
        IEnumerable<IndexRequestVM.GetData> GetAllRequestsByStatusId(string userId,int statusId);
        IEnumerable<IndexRequestVM.GetData> GetRequestsByUserIdAssetId(string userId, int assetId);
        IEnumerable<IndexRequestVM.GetData> GetAllRequestsByHospitalId(int hospitalId);
        IEnumerable<IndexRequestVM.GetData> GetAllRequestsByHospitalUserId(int hospitalId, string userId);
        IndexRequestsVM GetRequestByWorkOrderId(int workOrderId);
        int GetTotalRequestForAssetInHospital(int assetDetailId);

        IndexRequestsVM GetById(int id);
        int Add(CreateRequestVM createRequestVM);
        void Update(int Id, EditRequestVM editRequestVM);
        void Delete(int id);


        GeneratedRequestNumberVM GenerateRequestNumber();
        IEnumerable<IndexRequestVM.GetData> SearchRequests(SearchRequestVM searchObj);
    }
}
