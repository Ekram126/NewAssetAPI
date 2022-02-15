using Asset.Domain;
using Asset.Domain.Services;
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
            return _unitOfWork.WorkOrder.GetworkOrder( userId);
        }


        public void UpdateWorkOrder(int id, EditWorkOrderVM editWorkOrderVM)
        {
            _unitOfWork.WorkOrder.Update(id, editWorkOrderVM);
        }

        public IndexWorkOrderVM GetWorkOrderByRequestId(int requestId)
        {  
            
            return _unitOfWork.WorkOrder.GetWorkOrderByRequestId(requestId);
          
        }

        public IEnumerable<IndexWorkOrderVM> GetAllWorkOrdersByHospitalId(int? hospitalId,string userId)
        {
            return  _unitOfWork.WorkOrder.GetAllWorkOrdersByHospitalId(hospitalId, userId);
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
            return _unitOfWork.WorkOrder.GetAllWorkOrdersByHospitalId(hospitalId, userId,statusId);
        }

        public IEnumerable<IndexWorkOrderVM> SearchWorkOrders(SearchWorkOrderVM searchObj)
        {
            return _unitOfWork.WorkOrder.SearchWorkOrders(searchObj);
        }
        public IEnumerable<IndexWorkOrderVM> SortWorkOrders(int hosId, string userId, SortWorkOrderVM sortObj)
        {
            return _unitOfWork.WorkOrder.SortWorkOrders(hosId, userId, sortObj);
        }

        public List<IndexWorkOrderVM> GetLastRequestAndWorkOrderByAssetId(int assetId)
        {
            return _unitOfWork.WorkOrder.GetLastRequestAndWorkOrderByAssetId(assetId);
        }

        public IEnumerable<IndexWorkOrderVM> GetWorkOrdersByDate(SearchWorkOrderByDateVM woDateObj)
        {
            return _unitOfWork.WorkOrder.GetWorkOrdersByDate(woDateObj);
        }
    }
}
