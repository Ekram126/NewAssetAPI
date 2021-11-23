using Asset.ViewModels.WorkOrderVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Repositories
{
    public interface IWorkOrderRepository
    {
        IEnumerable<IndexWorkOrderVM> GetAll();
        IEnumerable<IndexWorkOrderVM> GetAllWorkOrdersByHospitalId(int? hospitalId, string userId);
        IEnumerable<IndexWorkOrderVM> GetAllWorkOrdersByHospitalId(int? hospitalId, string userId,int statusId);
        IEnumerable<IndexWorkOrderVM> GetworkOrderByUserId(int requestId, string userId);
        IEnumerable<IndexWorkOrderVM> GetworkOrder(string userId);
        IndexWorkOrderVM GetById(int id);
        IndexWorkOrderVM GetWorkOrderByRequestId(int requestId);
        int Add(CreateWorkOrderVM createWorkOrderVM);
        void Update(int id, EditWorkOrderVM editWorkOrderVM);
        void Delete(int id);

        GeneratedWorkOrderNumberVM GenerateWorOrderNumber();
        int GetTotalWorkOrdersForAssetInHospital(int assetDetailId);

        PrintWorkOrderVM PrintWorkOrderById(int id);

    }
}
