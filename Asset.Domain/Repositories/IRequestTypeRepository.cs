using Asset.ViewModels.RequestTypeVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Repositories
{
    public interface IRequestTypeRepository
    {
        IEnumerable<IndexRequestTypeVM> GetAll();
        IndexRequestTypeVM GetById(int id);
        void Add(CreateRequestTypeVM createRequestTypeVM);
        void Update(EditRequestTypeVM editRequestTypeVM);
        void Delete(int id);

        IEnumerable<IndexRequestTypeVM> SortRequestTypes(SortRequestTypeVM sortObj);
    }
}
