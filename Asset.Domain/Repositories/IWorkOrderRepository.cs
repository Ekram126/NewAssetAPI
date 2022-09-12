﻿using Asset.Models;
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



        List<IndexWorkOrderVM> GetAllWorkOrdersByHospitalIdAndPaging(int? hospitalId, string userId, int statusId, int pageNumber, int pageSize);
        int GetWorkOrdersCountByStatusIdAndPaging(int? hospitalId,string userId, int statusId);



        IEnumerable<IndexWorkOrderVM> ExportWorkOrdersByStatusId(int? hospitalId, string userId, int statusId);
        IEnumerable<IndexWorkOrderVM> GetAllWorkOrdersByHospitalId(int? hospitalId, string userId,int statusId);
        IEnumerable<IndexWorkOrderVM> GetworkOrderByUserId(int requestId, string userId);
        IEnumerable<IndexWorkOrderVM> GetworkOrder(string userId);

        List<IndexWorkOrderVM> GetLastRequestAndWorkOrderByAssetId(int assetId);
        List<IndexWorkOrderVM> GetLastRequestAndWorkOrderByAssetId(int assetId,int requestId);

        IEnumerable<IndexWorkOrderVM> SearchWorkOrders(SearchWorkOrderVM searchObj);

        IndexWorkOrderVM GetById(int id);
        IndexWorkOrderVM GetWorkOrderByRequestId(int requestId);
        int Add(CreateWorkOrderVM createWorkOrderVM);
        void Update(int id, EditWorkOrderVM editWorkOrderVM);
        void Delete(int id);

        GeneratedWorkOrderNumberVM GenerateWorOrderNumber();
        int GetTotalWorkOrdersForAssetInHospital(int assetDetailId);

        PrintWorkOrderVM PrintWorkOrderById(int id);
        IEnumerable<IndexWorkOrderVM> SortWorkOrders(int hosId,string userId,SortWorkOrderVM sortObj, int statusId);


        IEnumerable<IndexWorkOrderVM> GetWorkOrdersByDate(SearchWorkOrderByDateVM woDateObj);

        int CountWorkOrdersByHospitalId(int hospitalId, string userId);
    
        int CreateWorkOrderAttachments(WorkOrderAttachment attachObj);
    }
}
