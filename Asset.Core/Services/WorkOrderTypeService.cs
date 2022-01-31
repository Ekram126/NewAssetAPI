using Asset.Domain;
using Asset.Domain.Services;
using Asset.ViewModels.WorkOrderTypeVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
    public class WorkOrderTypeService: IWorkOrderTypeService
    {
        private IUnitOfWork _unitOfWork;

        public WorkOrderTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void AddWorkOrderType(CreateWorkOrderTypeVM createWorkOrderTypeVM)
        {
            _unitOfWork.WorkOrderType.Add(createWorkOrderTypeVM);
        }

        public void DeleteWorkOrderType(int id)
        {
            _unitOfWork.WorkOrderType.Delete(id);
        }

        public IEnumerable<IndexWorkOrderTypeVM> GetAllWorkOrderTypes()
        {
            return _unitOfWork.WorkOrderType.GetAll();
        }

        public IndexWorkOrderTypeVM GetWorkOrderTypeById(int id)
        {
            return _unitOfWork.WorkOrderType.GetById(id);
        }

        public IEnumerable<IndexWorkOrderTypeVM> SortWorkOrderTypes(SortWorkOrderTypeVM sortObj)
        {
            return _unitOfWork.WorkOrderType.SortWorkOrderTypes(sortObj);
        }

        public void UpdateWorkOrderType(int id, EditWorkOrderTypeVM editWorkOrderTypeVM)
        {
            _unitOfWork.WorkOrderType.Update(id, editWorkOrderTypeVM);
        }
    }
}
