using Asset.ViewModels.WorkOrderStatusVM;
using Asset.ViewModels.WorkOrderVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Repositories
{
    public interface IWorkOrderStatusRepository
    {
       // IEnumerable<IndexWorkOrderStatusVM> GetAll(string userId);

        IndexWorkOrderStatusVM GetAll(string userId);

        IEnumerable<IndexWorkOrderStatusVM> GetAll();
        IndexWorkOrderStatusVM GetAllForReport(SearchWorkOrderByDateVM woDateObj);



        IndexWorkOrderStatusVM GetById(int id);
        void Add(CreateWorkOrderStatusVM createWorkOrderStatusVM);
        void Update(int id, EditWorkOrderStatusVM editWorkOrderStatusVM);
        void Delete(int id);

        IEnumerable<IndexWorkOrderStatusVM> SortWOStatuses(SortWorkOrderStatusVM sortObj);
    }
}
