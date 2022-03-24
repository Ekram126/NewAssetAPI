﻿using Asset.Domain;
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
        public void UpdateRequest(EditRequestVM editRequestVM)
        {
            _unitOfWork.Request.Update( editRequestVM);
        }

        public IEnumerable<IndexRequestVM.GetData> GetAllRequestsWithTrackingByUserId(string userId)
        {
            return _unitOfWork.Request.GetAllRequestsWithTrackingByUserId(userId);
        }

        public IEnumerable<IndexRequestVM.GetData> GetAllRequestsByHospitalId(int hospitalId)
        { 
            throw new NotImplementedException();
        }



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

        public IEnumerable<IndexRequestVM.GetData> SearchRequests(SearchRequestVM searchObj)
        {
            return _unitOfWork.Request.SearchRequests(searchObj);
        }

        public async Task< IEnumerable<IndexRequestsVM>> SortRequests(SortRequestVM sortObj, int statusId)
        {
            return await _unitOfWork.Request.SortRequests(sortObj,statusId);
        }

        public int GetTotalOpenRequest(string userId)
        {
            return _unitOfWork.Request.GetTotalOpenRequest(userId);
        }

        public IEnumerable<IndexRequestVM.GetData> GetAllRequestsByAssetId(int assetId, int hospitalId)
        {
            return _unitOfWork.Request.GetAllRequestsByAssetId(assetId,hospitalId);
        }

        public IEnumerable<IndexRequestsVM> SortRequestsByAssetId(SortRequestVM sortObj)
        {   return _unitOfWork.Request.SortRequestsByAssetId(sortObj);
            
        }

        public PrintServiceRequestVM PrintServiceRequestById(int id)
        {
            return _unitOfWork.Request.PrintServiceRequestById(id);
           
        }

        public IEnumerable<IndexRequestVM.GetData> GetRequestsByDate(SearchRequestDateVM requestDateObj)
        {

            return _unitOfWork.Request.GetRequestsByDate(requestDateObj);
          
        }

        public IndexRequestsVM GetByRequestCode(string code)
        {
            return _unitOfWork.Request.GetByRequestCode(code);
        }

        public IEnumerable<IndexRequestVM.GetData> GetAllRequestsByHospitalAssetId(int assetId)
        {
            return _unitOfWork.Request.GetAllRequestsByHospitalAssetId(assetId);
        }
    }
}
