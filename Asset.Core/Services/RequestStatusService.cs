using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.RequestStatusVM;
using Asset.ViewModels.RequestVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
    public class RequestStatusService : IRequestStatusService
    {
        private IUnitOfWork _unitOfWork;

        public RequestStatusService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<IndexRequestStatusVM.GetData> GetAllRequestStatus()
        {
            return _unitOfWork.RequestStatus.GetAll();
        }



        public IndexRequestStatusVM.GetData GetAllForReport()
        {
            return _unitOfWork.RequestStatus.GetAllForReport();
        }




        public RequestStatus GetById(int id)
        {
            return _unitOfWork.RequestStatus.GetById(id);
        }

        public int Add(RequestStatus createRequestVM)
        {
            return _unitOfWork.RequestStatus.Add(createRequestVM);
        }

        public int Update(RequestStatus editRequestVM)
        {
            return _unitOfWork.RequestStatus.Update(editRequestVM);
        }

        public int Delete(int id)
        {

            return _unitOfWork.RequestStatus.Delete(id);
        }

        public IndexRequestStatusVM.GetData GetAll(string userId)
        {
            return _unitOfWork.RequestStatus.GetAll(userId);
        }

        public IEnumerable<IndexRequestStatusVM.GetData> SortRequestStatuses(SortRequestStatusVM sortObj)
        {
            return _unitOfWork.RequestStatus.SortRequestStatuses(sortObj);
        }

        public IndexRequestStatusVM.GetData GetAllByHospitalId(string userId, int hospitalId)
        {
            return _unitOfWork.RequestStatus.GetAllByHospitalId(userId, hospitalId);
        }

        public IndexRequestStatusVM.GetData GetAllForReport(SearchRequestDateVM requestDateObj)
        {
            return _unitOfWork.RequestStatus.GetAllForReport(requestDateObj);
        }
    }
}
