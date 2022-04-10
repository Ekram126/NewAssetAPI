using Asset.ViewModels.WorkOrderVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
    public interface IWorkOrderService
    {
        IEnumerable<IndexWorkOrderVM> GetAllWorkOrders();
        IEnumerable<IndexWorkOrderVM> GetAllWorkOrdersByHospitalId(int? hospitalId, string userId);
        IEnumerable<IndexWorkOrderVM> GetAllWorkOrdersByHospitalId(int? hospitalId, string userId, int statusId);
        IEnumerable<IndexWorkOrderVM> GetworkOrderByUserId(int requestId, string userId);
        IEnumerable<IndexWorkOrderVM> GetworkOrder(string userId);

        List<IndexWorkOrderVM> GetLastRequestAndWorkOrderByAssetId(int assetId);




        IEnumerable<IndexWorkOrderVM> SearchWorkOrders(SearchWorkOrderVM searchObj);
        IndexWorkOrderVM GetWorkOrderById(int id);
        IndexWorkOrderVM GetWorkOrderByRequestId(int requestId);
        int AddWorkOrder(CreateWorkOrderVM createWorkOrderVM);
        void UpdateWorkOrder(int id, EditWorkOrderVM editWorkOrderVM);
        void DeleteWorkOrder(int id);

        GeneratedWorkOrderNumberVM GenerateWorOrderNumber();
        int GetTotalWorkOrdersForAssetInHospital(int assetDetailId);
        PrintWorkOrderVM PrintWorkOrderById(int id);
        IEnumerable<IndexWorkOrderVM> SortWorkOrders(int hosId, string userId, SortWorkOrderVM sortObj, int statusId);

        IEnumerable<IndexWorkOrderVM> GetWorkOrdersByDate(SearchWorkOrderByDateVM woDateObj);

        int CountWorkOrdersByHospitalId(int hospitalId, string userId);
    }
}
