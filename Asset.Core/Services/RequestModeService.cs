using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.RequestModeVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
    public class RequestModeService : IRequestModeService
    {
        private IUnitOfWork _unitOfWork;

        public RequestModeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void AddRequestMode(CreateRequestMode createRequestMode)
        {
            _unitOfWork.RequestMode.Add(createRequestMode);
        }

        public void DeleteRequestMode(int id)
        {
            _unitOfWork.RequestMode.Delete(id);
        }

        public IEnumerable<IndexRequestMode> GetAllRequestMode()
        {
            return _unitOfWork.RequestMode.GetAll();
        }

        public IndexRequestMode GetRequestModeById(int id)
        {
            return _unitOfWork.RequestMode.GetById(id);
        }

        public void UpdateRequestMode(int id, EditRequestMode editRequestMode)
        {
            _unitOfWork.RequestMode.Update(id, editRequestMode);
        }
    }
}
