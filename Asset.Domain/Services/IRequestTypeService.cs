using Asset.ViewModels.RequestTypeVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
    public interface IRequestTypeService
    {
        IEnumerable<IndexRequestTypeVM> GetAllRequestTypes();
        IndexRequestTypeVM GetRequestTypeById(int id);
        void AddRequestType(CreateRequestTypeVM createRequestTypeVM);
        void UpdateRequestType(EditRequestTypeVM editRequestTypeVM);
        void DeleteRequestType(int id);

        IEnumerable<IndexRequestTypeVM> SortRequestTypes(SortRequestTypeVM sortObj);
    }
}
