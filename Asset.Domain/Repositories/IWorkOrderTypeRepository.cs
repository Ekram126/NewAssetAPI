using Asset.ViewModels.WorkOrderTypeVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Repositories
{
    public interface IWorkOrderTypeRepository
    {
        IEnumerable<IndexWorkOrderTypeVM> GetAll();
        IndexWorkOrderTypeVM GetById(int id);
        void Add(CreateWorkOrderTypeVM createWorkOrderTypeVM);
        void Update(int id, EditWorkOrderTypeVM editWorkOrderTypeVM);
        void Delete(int id);

        IEnumerable<IndexWorkOrderTypeVM> SortWorkOrderTypes(SortWorkOrderTypeVM sortObj);
    }
}
