using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.WorkOrderVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
    public class WorkOrderService : IWorkOrderService
    {
        private IUnitOfWork _unitOfWork;

        public WorkOrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public int AddWorkOrder(CreateWorkOrderVM createWorkOrderVM)
        {
            return _unitOfWork.WorkOrder.Add(createWorkOrderVM);
        }

        public void DeleteWorkOrder(int id)
        {
            _unitOfWork.WorkOrder.Delete(id);
        }

        public IEnumerable<IndexWorkOrderVM> GetAllWorkOrders()
        {
            return _unitOfWork.WorkOrder.GetAll();
        }

        public IndexWorkOrderVM GetWorkOrderById(int id)
        {
            return _unitOfWork.WorkOrder.GetById(id);
        }

        public IEnumerable<IndexWorkOrderVM> GetworkOrderByUserId(int requestId, string userId)
        {
            return _unitOfWork.WorkOrder.GetworkOrderByUserId(requestId, userId);
        }

        public IEnumerable<IndexWorkOrderVM> GetworkOrder(string userId)
        {
            return _unitOfWork.WorkOrder.GetworkOrder(userId);
        }


        public void UpdateWorkOrder(int id, EditWorkOrderVM editWorkOrderVM)
        {
            _unitOfWork.WorkOrder.Update(id, editWorkOrderVM);
        }

        public IndexWorkOrderVM GetWorkOrderByRequestId(int requestId)
        {

            return _unitOfWork.WorkOrder.GetWorkOrderByRequestId(requestId);

        }

        public IEnumerable<IndexWorkOrderVM> GetAllWorkOrdersByHospitalId(int? hospitalId, string userId)
        {
            return _unitOfWork.WorkOrder.GetAllWorkOrdersByHospitalId(hospitalId, userId);
        }

        public GeneratedWorkOrderNumberVM GenerateWorOrderNumber()
        {
            return _unitOfWork.WorkOrder.GenerateWorOrderNumber();
        }

        public int GetTotalWorkOrdersForAssetInHospital(int assetDetailId)
        {
            return _unitOfWork.WorkOrder.GetTotalWorkOrdersForAssetInHospital(assetDetailId);
        }

        public PrintWorkOrderVM PrintWorkOrderById(int id)
        {
            return _unitOfWork.WorkOrder.PrintWorkOrderById(id);
        }

        public IEnumerable<IndexWorkOrderVM> GetAllWorkOrdersByHospitalId(int? hospitalId, string userId, int statusId)
        {
            return _unitOfWork.WorkOrder.GetAllWorkOrdersByHospitalId(hospitalId, userId, statusId);
        }

        public IEnumerable<IndexWorkOrderVM> SearchWorkOrders(SearchWorkOrderVM searchObj)
        {
            return _unitOfWork.WorkOrder.SearchWorkOrders(searchObj);
        }
        public IEnumerable<IndexWorkOrderVM> SortWorkOrders(int hosId, string userId, SortWorkOrderVM sortObj, int statusId)
        {
            return _unitOfWork.WorkOrder.SortWorkOrders(hosId, userId, sortObj, statusId);
        }

        public List<IndexWorkOrderVM> GetLastRequestAndWorkOrderByAssetId(int assetId)
        {
            return _unitOfWork.WorkOrder.GetLastRequestAndWorkOrderByAssetId(assetId);
        }

        public IEnumerable<IndexWorkOrderVM> GetWorkOrdersByDate(SearchWorkOrderByDateVM woDateObj)
        {
            return _unitOfWork.WorkOrder.GetWorkOrdersByDate(woDateObj);
        }

        public int CountWorkOrdersByHospitalId(int hospitalId, string userId)
        {
            return _unitOfWork.WorkOrder.CountWorkOrdersByHospitalId(hospitalId, userId);
        }

        public int CreateWorkOrderAttachments(WorkOrderAttachment attachObj)
        {
            return _unitOfWork.WorkOrder.CreateWorkOrderAttachments(attachObj);

        }

        public List<IndexWorkOrderVM> GetLastRequestAndWorkOrderByAssetId(int assetId, int requestId)
        {
            return _unitOfWork.WorkOrder.GetLastRequestAndWorkOrderByAssetId(assetId, requestId);
        }

        public List<IndexWorkOrderVM> GetAllWorkOrdersByHospitalIdAndPaging(int? hospitalId, string userId, int statusId, int pageNumber, int pageSize)
        {
            return _unitOfWork.WorkOrder.GetAllWorkOrdersByHospitalIdAndPaging(hospitalId, userId, statusId, pageNumber, pageSize);
        }

        public int GetWorkOrdersCountByStatusIdAndPaging(int? hospitalId, string userId, int statusId)
        {
            return _unitOfWork.WorkOrder.GetWorkOrdersCountByStatusIdAndPaging(hospitalId, userId, statusId);
        }

        public IEnumerable<IndexWorkOrderVM> ExportWorkOrdersByStatusId(int? hospitalId, string userId, int statusId)
        {
            return _unitOfWork.WorkOrder.ExportWorkOrdersByStatusId(hospitalId, userId, statusId);
        }

        public IndexWorkOrderVM2 GetWorkOrdersByDateAndStatus(SearchWorkOrderByDateVM woDateObj, int pageNumber, int pageSize)
        {
            return _unitOfWork.WorkOrder.GetWorkOrdersByDateAndStatus(woDateObj, pageNumber, pageSize);

        }

        public IndexWorkOrderVM2 GetWorkOrdersByDateAndStatus(SearchWorkOrderByDateVM woDateObj)
        {
            return _unitOfWork.WorkOrder.GetWorkOrdersByDateAndStatus(woDateObj);

        }



        public List<IndexWorkOrderVM2.GetData> PrintListOfWorkOrders(List<ExportWorkOrderVM> workOrders)
        {
            return _unitOfWork.WorkOrder.PrintListOfWorkOrders(workOrders);
        }

        public IEnumerable<IndexWorkOrderVM> GetworkOrderByUserAssetId(int assetId, string userId)
        {
            return _unitOfWork.WorkOrder.GetworkOrderByUserAssetId(assetId, userId);
        }

        public IndexWorkOrderVM2 SearchWorkOrders(SearchWorkOrderVM searchObj, int pageNumber, int pageSize)
        {
            return _unitOfWork.WorkOrder.SearchWorkOrders(searchObj, pageNumber, pageSize);
        }

        public IndexWorkOrderVM2 GetAllWorkOrdersByHospitalIdAndPaging2(int? hospitalId, string userId, int statusId, int pageNumber, int pageSize)
        {
            return _unitOfWork.WorkOrder.GetAllWorkOrdersByHospitalIdAndPaging2(hospitalId, userId, statusId, pageNumber, pageSize);
        }

        public IndexWorkOrderVM GetMobileWorkOrderByRequestUserId(int requestId, string userId)
        {
            return _unitOfWork.WorkOrder.GetMobileWorkOrderByRequestUserId(requestId, userId);
        }

        public List<IndexWorkOrderVM2.GetData> PrintListOfWorkOrders(PrintWorkOrderVM printWorkOrderObj)
        {
            return _unitOfWork.WorkOrder.PrintListOfWorkOrders(printWorkOrderObj);
        }
    }
}
