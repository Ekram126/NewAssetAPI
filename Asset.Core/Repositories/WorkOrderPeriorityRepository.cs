using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.WorkOrderPeriorityVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class WorkOrderPeriorityRepository : IWorkOrderPeriorityRepository
    {
         private ApplicationDbContext _context;
         string msg;

        public WorkOrderPeriorityRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(CreateWorkOrderPeriorityVM createWorkOrderPeriorityVM)
        {
            try
            {
                if (createWorkOrderPeriorityVM != null)
                {
                    WorkOrderPeriority workOrderPeriority = new WorkOrderPeriority();
                    workOrderPeriority.Name = createWorkOrderPeriorityVM.Name;
                    workOrderPeriority.NameAr = createWorkOrderPeriorityVM.NameAr;
                    workOrderPeriority.Code = createWorkOrderPeriorityVM.Code;
                    _context.WorkOrderPeriorities.Add(workOrderPeriority);
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
            var WorkOrderPeriority = _context.WorkOrderPeriorities.Find(id);
            try
            {
                if (WorkOrderPeriority != null)
                {
                    _context.WorkOrderPeriorities.Remove(WorkOrderPeriority);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }

        public IEnumerable<IndexWorkOrderPeriorityVM> GetAll()
        {
            return _context.WorkOrderPeriorities.Select(Type => new IndexWorkOrderPeriorityVM
            {
                Id = Type.Id,
                Name = Type.Name,
                NameAr = Type.NameAr,
                Code = Type.Code
            }).ToList();
        }

        public IndexWorkOrderPeriorityVM GetById(int id)
        {
            return _context.WorkOrderPeriorities.Select(Type => new IndexWorkOrderPeriorityVM
            {
                Id = Type.Id,
                Name = Type.Name,
                NameAr = Type.NameAr,
                Code = Type.Code
            }).FirstOrDefault();
        }

        public void Update(int id, EditWorkOrderPeriorityVM editWorkOrderPeriorityVM)
        {
            try
            {
                WorkOrderPeriority workOrderPeriority = new WorkOrderPeriority();
                workOrderPeriority.Id = editWorkOrderPeriorityVM.Id;
                workOrderPeriority.Name = editWorkOrderPeriorityVM.Name;
                workOrderPeriority.NameAr = editWorkOrderPeriorityVM.NameAr;
                workOrderPeriority.Code = editWorkOrderPeriorityVM.Code;
                _context.Entry(workOrderPeriority).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }
    }
}