using Asset.Domain.Repositories;
using Asset.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class WorkOrderAssignRepositories : IWorkOrderAssignRepository
    {
        private readonly ApplicationDbContext _context;
        private string msg;

        public WorkOrderAssignRepositories(ApplicationDbContext context)
        {
            _context = context;
        }
        public int Add(WorkOrderAssign model)
        {
            try
            {
                if (model != null)
                {
                    WorkOrderAssign WorkOrderAssign = new WorkOrderAssign();
                    WorkOrderAssign.WOTId = model.WOTId;
                    WorkOrderAssign.CreatedDate = DateTime.Now;
                    WorkOrderAssign.CreatedBy = model.CreatedBy;
                    WorkOrderAssign.Notes = model.Notes;
                    WorkOrderAssign.UserId = model.UserId;
                    WorkOrderAssign.SupplierId = model.SupplierId;
                    _context.WorkOrderAssigns.Add(WorkOrderAssign);
                    _context.SaveChanges();
                    model.Id = WorkOrderAssign.Id;
                    return model.Id;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return model.Id;
        }

        public int Delete(int id)
        {
            var WorkOrderAssign = _context.WorkOrderAssigns.Find(id);
            try
            {
                if (WorkOrderAssign != null)
                {
                    _context.WorkOrderAssigns.Remove(WorkOrderAssign);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return id;
        }
        public IEnumerable<WorkOrderAssign> GetAllAssignedWorkOrderByUserId(string userId)
        {
//var lstAssignedFrom = _context.WorkOrderAssigns.Where(t => t.HospitalId == UserObj.HospitalId && t.UserId == userId && t.CreatedBy == userId ).ToList();

            return _context.WorkOrderAssigns.Where(a=>a.UserId== userId).ToList();
        }
        public IEnumerable<WorkOrderAssign> GetAllWorkOrderAssignsByWorkOrederTrackId(int wotrackId)
        {
            return _context.WorkOrderAssigns.Where(a => a.WOTId == wotrackId).ToList();
        }

        public WorkOrderAssign GetById(int id)
        {
            return _context.WorkOrderAssigns.Find(id);
        }

        public int Update(WorkOrderAssign model)
        {
            WorkOrderAssign workOrderAssignObj = _context.WorkOrderAssigns.Find(model.Id);

            try
            {
                workOrderAssignObj.Id = model.Id;
                workOrderAssignObj.WOTId = model.WOTId;
                workOrderAssignObj.CreatedDate = DateTime.Now;
                workOrderAssignObj.CreatedBy = model.CreatedBy;
                workOrderAssignObj.Notes = model.Notes;
                workOrderAssignObj.UserId = model.UserId;
                workOrderAssignObj.SupplierId = model.SupplierId;
                _context.Entry(workOrderAssignObj).State = EntityState.Modified;
                _context.SaveChanges();

               
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return workOrderAssignObj.Id;
        }

      
    }
}
