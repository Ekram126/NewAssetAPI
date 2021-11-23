using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.RequestStatusVM;
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
        public IEnumerable<IndexRequestStatusVM> GetAllRequestStatus()
        {
           return _unitOfWork.RequestStatus.GetAll();
        }
      
        public int UpdateRequestStatus(IndexRequestStatusVM editRequestStatus)
        {
          return  _unitOfWork.RequestStatus.Update(editRequestStatus);
        }

        public IndexRequestStatusVM GetById(int id)
        {
            return _unitOfWork.RequestStatus.GetById(id);
        }

        public int Add(RequestStatus createRequestVM)
        {
           return _unitOfWork.RequestStatus.Add(createRequestVM);
        }

        public int Update(IndexRequestStatusVM editRequestVM)
        {
            return _unitOfWork.RequestStatus.Update(editRequestVM);
        }

        public int Delete(int id)
        {

           return _unitOfWork.RequestStatus.Delete(id);
        }
    }
}
