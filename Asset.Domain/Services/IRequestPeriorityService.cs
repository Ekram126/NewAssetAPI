using Asset.Models;
using Asset.ViewModels.RequestPeriorityVM;
using System.Collections.Generic;

namespace Asset.Domain.Services
{
    public interface IRequestPeriorityService
    {
        IEnumerable<IndexRequestPeriority> GetAllRequestPeriority();
        IndexRequestPeriority GetRequestPeriorityById(int id);

        IEnumerable<IndexRequestPeriority> GetByPeriorityName(string name);

        void AddRequestPeriority(CreateRequestPeriority createRequestPeriority);
        void UpdateRequestPeriority(int id, EditRequestPeriority editRequestPeriority);
        void DeleteRequestPeriority(int id);
    }
}
