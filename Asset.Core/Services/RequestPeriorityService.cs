using Asset.Domain;
using Asset.Domain.Services;
using Asset.ViewModels.RequestPeriorityVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
    public class RequestPeriorityService : IRequestPeriorityService
    {
        private IUnitOfWork _unitOfWork;

        public RequestPeriorityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void AddRequestPeriority(CreateRequestPeriority createRequestPeriority)
        {
            _unitOfWork.RequestPeriority.Add(createRequestPeriority);
        }

        public void DeleteRequestPeriority(int id)
        {
            _unitOfWork.RequestPeriority.Delete(id);
        }

        public IEnumerable<IndexRequestPeriority> GetAllRequestPeriority()
        {
            return _unitOfWork.RequestPeriority.GetAll();
        }

        public IEnumerable<IndexRequestPeriority> GetByPeriorityName(string name)
        {
            return _unitOfWork.RequestPeriority.GetByPeriorityName(name);
        }

        public IndexRequestPeriority GetRequestPeriorityById(int id)
        {
            return _unitOfWork.RequestPeriority.GetById(id);
        }

        public void UpdateRequestPeriority(int id, EditRequestPeriority editRequestPeriority)
        {
            _unitOfWork.RequestPeriority.Update(id,editRequestPeriority);
        }
    }
}
