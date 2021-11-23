﻿using Asset.Domain;
using Asset.Domain.Services;
using Asset.ViewModels.WorkOrderStatusVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
    public class WorkOrderStatusService: IWorkOrderStatusService
    {
        private IUnitOfWork _unitOfWork;

        public WorkOrderStatusService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void AddWorkOrderStatus(CreateWorkOrderStatusVM createWorkOrderStatusVM)
        {
            _unitOfWork.WorkOrderStatus.Add(createWorkOrderStatusVM);
        }

        public void DeleteWorkOrderStatus(int id)
        {
            _unitOfWork.WorkOrderStatus.Delete(id);
        }

        public IEnumerable<IndexWorkOrderStatusVM> GetAllWorkOrderStatuses()
        {
            return _unitOfWork.WorkOrderStatus.GetAll();
        }

        public IndexWorkOrderStatusVM GetWorkOrderStatusById(int id)
        {
            return _unitOfWork.WorkOrderStatus.GetById(id);
        }

        public void UpdateWorkOrderStatus(int id, EditWorkOrderStatusVM editWorkOrderStatusVM)
        {
            _unitOfWork.WorkOrderStatus.Update(id, editWorkOrderStatusVM);
        }
    }
}

