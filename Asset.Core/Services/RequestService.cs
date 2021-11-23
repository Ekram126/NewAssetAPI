using Asset.Domain;
using Asset.Domain.Services;
using Asset.ViewModels.RequestVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
    public class RequestService : IRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RequestService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public int AddRequest(CreateRequestVM createRequestVM)
        {
            _unitOfWork.Request.Add(createRequestVM);
            return createRequestVM.Id;
        }

        public void DeleteRequest(int id)
        {
            _unitOfWork.Request.Delete(id);
        }

        public IEnumerable<IndexRequestsVM> GetAllRequests()
        {
            return _unitOfWork.Request.GetAll();
        }

        public IndexRequestsVM GetRequestById(int id)
        {
            return _unitOfWork.Request.GetById(id);
        }
        public void UpdateRequest(int Id, EditRequestVM editRequestVM)
        {
            _unitOfWork.Request.Update(Id, editRequestVM);
        }

        public IEnumerable<IndexRequestVM.GetData> GetAllRequestsWithTrackingByUserId(string userId)
        {
            return _unitOfWork.Request.GetAllRequestsWithTrackingByUserId(userId);
        }

        public IEnumerable<IndexRequestVM.GetData> GetAllRequestsByHospitalId(int hospitalId)
        { 
            throw new NotImplementedException();
        }

        //public IEnumerable<IndexRequestVM.GetData> GetAllRequestsByHospitalUserId(int hospitalId, string userId)
        //{
        //    throw new NotImplementedException();
        //}

        public IndexRequestsVM GetRequestByWorkOrderId(int workOrderId)
        { 
            return _unitOfWork.Request.GetRequestByWorkOrderId(workOrderId);
           
        }

        public int GetTotalRequestForAssetInHospital(int assetDetailId)
        {
            return _unitOfWork.Request.GetTotalRequestForAssetInHospital(assetDetailId);
        }

        public IEnumerable<IndexRequestVM.GetData> GetAllRequestsByHospitalUserId(int hospitalId, string userId)
        {
            throw new NotImplementedException();
        }

        public GeneratedRequestNumberVM GenerateRequestNumber()
        {
            return _unitOfWork.Request.GenerateRequestNumber();
        }

        public IEnumerable<IndexRequestVM.GetData> GetAllRequestsByStatusId(string userId, int statusId)
        {
            return _unitOfWork.Request.GetAllRequestsByStatusId(userId, statusId);
        }

        public IEnumerable<IndexRequestVM.GetData> GetRequestsByUserIdAssetId(string userId, int assetId)
        {
            return _unitOfWork.Request.GetAllRequestsByStatusId(userId, assetId);
        }
    }
}
