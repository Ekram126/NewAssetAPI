using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.WorkOrderTaskVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class WorkOrderTaskRepository : IWorkOrderTaskRepository
    {
        private ApplicationDbContext _context;
        string msg;

        public WorkOrderTaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(CreateWorkOrderTaskVM createWorkOrderTaskVM)
        {
            try
            {
                if (createWorkOrderTaskVM != null)
                {
                    foreach (var item in createWorkOrderTaskVM.LstCreateTasks)
                    {
                        WorkOrderTask workOrderTask = new WorkOrderTask();
                        workOrderTask.Comment = item.Comment;
                        workOrderTask.WorkOrderId = createWorkOrderTaskVM.WorkOrderId;
                        workOrderTask.AssetWorkOrderTaskId = item.AssetWorkOrderTaskId;
                        _context.WorkOrderTasks.Add(workOrderTask);
                        _context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }

        public void Delete(int id)
        {
            var workOrderTask = _context.WorkOrderTasks.Find(id);
            try
            {
                if (workOrderTask != null)
                {
                    _context.WorkOrderTasks.Remove(workOrderTask);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }

        public IEnumerable<IndexWorkOrderTaskVM> GetAll()
        {
            return _context.WorkOrderTasks.Include(a=>a.AssetWorkOrderTask).Include(a=>a.WorkOrder).Select(Task => new IndexWorkOrderTaskVM
            {
                Id = Task.Id,
                Comment = Task.Comment,
                WorkOrderId=Task.WorkOrderId,
                WorkOrderSubject=Task.WorkOrder.Subject,
                AssetWorkOrderTaskId=Task.AssetWorkOrderTaskId,
                AssetWorkOrderTaskName=Task.AssetWorkOrderTask.Name
            }).ToList();
        }
        public IEnumerable<IndexWorkOrderTaskVM> GetAllWorkOrderTaskByWorkOrderId(int WorkOrderId)
        {
            return _context.WorkOrderTasks.Where(w=>w.WorkOrderId== WorkOrderId).Include(a => a.AssetWorkOrderTask).Include(a => a.WorkOrder).Select(Task => new IndexWorkOrderTaskVM
            {
                Id = Task.Id,
                Comment = Task.Comment,
                WorkOrderId = Task.WorkOrderId,
                WorkOrderSubject = Task.WorkOrder.Subject,
                AssetWorkOrderTaskId = Task.AssetWorkOrderTaskId,
                AssetWorkOrderTaskName = Task.AssetWorkOrderTask.Name
            }).ToList();
        }
        public IndexWorkOrderTaskVM GetById(int id)
        {
            return _context.WorkOrderTasks.Include(a => a.AssetWorkOrderTask).Include(a => a.WorkOrder).Select(Task => new IndexWorkOrderTaskVM
            {
                Id = Task.Id,
                Comment = Task.Comment,
                WorkOrderId = Task.WorkOrderId,
                WorkOrderSubject = Task.WorkOrder.Subject,
                AssetWorkOrderTaskId = Task.AssetWorkOrderTaskId,
                AssetWorkOrderTaskName = Task.AssetWorkOrderTask.Name
            }).FirstOrDefault();
        }

        public void Update(int id, EditWorkOrderTaskVM editWorkOrderTaskVM)
        {
            try
            {
                WorkOrderTask workOrderTask = new WorkOrderTask();
                workOrderTask.Id = editWorkOrderTaskVM.Id;
                workOrderTask.Comment = editWorkOrderTaskVM.Comment;
                workOrderTask.WorkOrderId = editWorkOrderTaskVM.WorkOrderId;
                workOrderTask.AssetWorkOrderTaskId = editWorkOrderTaskVM.AssetWorkOrderTaskId;
                _context.Entry(workOrderTask).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }
    }
}
