using Asset.Models;
using Asset.ViewModels.RequestModeVM;
using System.Collections.Generic;

namespace Asset.Domain.Services
{
    public interface IRequestModeService
    {
        IEnumerable<IndexRequestMode> GetAllRequestMode();
        IndexRequestMode GetRequestModeById(int id);
        void AddRequestMode(CreateRequestMode createRequestMode);
        void UpdateRequestMode(int id, EditRequestMode editRequestMode);
        void DeleteRequestMode(int id);
    }
}
