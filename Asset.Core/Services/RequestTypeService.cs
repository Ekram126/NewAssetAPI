using Asset.Domain;
using Asset.Domain.Services;
using Asset.ViewModels.RequestTypeVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
    public class RequestTypeService : IRequestTypeService
    {
        private IUnitOfWork _unitOfWork;

        public RequestTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void AddRequestType(CreateRequestTypeVM createRequestTypeVM)
        {
            _unitOfWork.RequestType.Add(createRequestTypeVM);
        }

        public void DeleteRequestType(int id)
        {
            _unitOfWork.RequestType.Delete(id);
        }

        public IEnumerable<IndexRequestTypeVM> GetAllRequestTypes()
        {
            return _unitOfWork.RequestType.GetAll();
        }

        public IndexRequestTypeVM GetRequestTypeById(int id)
        {
            return _unitOfWork.RequestType.GetById(id);
        }

        public IEnumerable<IndexRequestTypeVM> SortRequestTypes(SortRequestTypeVM sortObj)
        {
            return _unitOfWork.RequestType.SortRequestTypes(sortObj);
        }

        public void UpdateRequestType(EditRequestTypeVM editRequestTypeVM)
        {
            _unitOfWork.RequestType.Update(editRequestTypeVM);
        }
    }
}
