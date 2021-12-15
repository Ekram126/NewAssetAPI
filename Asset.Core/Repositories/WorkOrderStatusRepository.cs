using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.WorkOrderStatusVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class WorkOrderStatusRepository : IWorkOrderStatusRepository
    {
        private ApplicationDbContext _context;
        string msg;
        public WorkOrderStatusRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(CreateWorkOrderStatusVM createWorkOrderStatusVM)
        {
            try
            {
                if (createWorkOrderStatusVM != null)
                {
                    WorkOrderStatus workOrderStatus = new WorkOrderStatus();
                    workOrderStatus.Name = createWorkOrderStatusVM.Name;
                    workOrderStatus.NameAr = createWorkOrderStatusVM.NameAr;
                    workOrderStatus.Code = createWorkOrderStatusVM.Code;
                    workOrderStatus.Color = createWorkOrderStatusVM.Color;
                    _context.WorkOrderStatuses.Add(workOrderStatus);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }

        public void Delete(int id)
        {
            var WorkOrderStatus = _context.WorkOrderStatuses.Find(id);
            try
            {
                if (WorkOrderStatus != null)
                {
                    _context.WorkOrderStatuses.Remove(WorkOrderStatus);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }

        public IEnumerable<IndexWorkOrderStatusVM> GetAll()
        {
            return _context.WorkOrderStatuses.Select(item => new IndexWorkOrderStatusVM
            {
                Id = item.Id,
                Color = item.Color,
                Name = item.Name,
                NameAr = item.NameAr,
                Code = item.Code
                
             
            }).ToList();
        }

        public IndexWorkOrderStatusVM GetById(int id)
        {
            return _context.WorkOrderStatuses.Where(a => a.Id == id).Select(item => new IndexWorkOrderStatusVM
            {
                Id = item.Id,
                Color = item.Color,
                Name = item.Name,
                NameAr = item.NameAr,
                Code = item.Code
            }).FirstOrDefault();
        }

        public void Update(int id, EditWorkOrderStatusVM editWorkOrderStatusVM)
        {
            try
            {
                WorkOrderStatus workOrderStatus = new WorkOrderStatus();
                workOrderStatus.Id = editWorkOrderStatusVM.Id;
                workOrderStatus.Name = editWorkOrderStatusVM.Name;
                workOrderStatus.NameAr = editWorkOrderStatusVM.NameAr;
                workOrderStatus.Code = editWorkOrderStatusVM.Code;

                workOrderStatus.Color = editWorkOrderStatusVM.Color;
                _context.Entry(workOrderStatus).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }
    }
}
