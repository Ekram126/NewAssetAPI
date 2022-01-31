using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.WorkOrderTypeVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class WorkOrderTypeRepository: IWorkOrderTypeRepository
    {
        private ApplicationDbContext _context;
        string msg;
        public WorkOrderTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(CreateWorkOrderTypeVM createWorkOrderTypeVM)
        {
            try
            {
                if (createWorkOrderTypeVM != null)
                {
                    WorkOrderType workOrderType = new WorkOrderType();
                    workOrderType.Name = createWorkOrderTypeVM.Name;
                    workOrderType.NameAr = createWorkOrderTypeVM.NameAr;
                    workOrderType.Code = createWorkOrderTypeVM.Code;
                    _context.WorkOrderTypes.Add(workOrderType);
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
            var WorkOrderType = _context.WorkOrderTypes.Find(id);
            try
            {
                if (WorkOrderType != null)
                {
                    _context.WorkOrderTypes.Remove(WorkOrderType);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }

        public IEnumerable<IndexWorkOrderTypeVM> GetAll()
        {
            return _context.WorkOrderTypes.Select(Type => new IndexWorkOrderTypeVM
            {
                Id = Type.Id,
                Name = Type.Name,
                NameAr = Type.NameAr,
                Code = Type.Code
            }).ToList();
        }

        public IndexWorkOrderTypeVM GetById(int id)
        {
            return _context.WorkOrderTypes.Select(Type => new IndexWorkOrderTypeVM
            {
                Id = Type.Id,
                Name = Type.Name,
                NameAr = Type.NameAr,
                Code = Type.Code
            }).Where(e=>e.Id==id).FirstOrDefault();
        }

        public void Update(int id, EditWorkOrderTypeVM editWorkOrderTypeVM)
        {
            try
            {
                WorkOrderType workOrderType = new WorkOrderType();
                workOrderType.Id = editWorkOrderTypeVM.Id;
                workOrderType.Name = editWorkOrderTypeVM.Name;
                workOrderType.NameAr = editWorkOrderTypeVM.NameAr;
                workOrderType.Code = editWorkOrderTypeVM.Code;
                _context.Entry(workOrderType).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }




        public IEnumerable<IndexWorkOrderTypeVM> SortWorkOrderTypes(SortWorkOrderTypeVM sortObj)
        {
            var lstBrands = GetAll().ToList();
            if (sortObj.Code != "")
            {
                if (sortObj.SortStatus == "descending")
                    lstBrands = lstBrands.OrderByDescending(d => d.Code).ToList();
                else
                    lstBrands = lstBrands.OrderBy(d => d.Code).ToList();
            }
            else if (sortObj.Name != "")
            {
                if (sortObj.SortStatus == "descending")
                    lstBrands = lstBrands.OrderByDescending(d => d.Name).ToList();
                else
                    lstBrands = lstBrands.OrderBy(d => d.Name).ToList();
            }

            else if (sortObj.NameAr != "")
            {
                if (sortObj.SortStatus == "descending")
                    lstBrands = lstBrands.OrderByDescending(d => d.NameAr).ToList();
                else
                    lstBrands = lstBrands.OrderBy(d => d.NameAr).ToList();
            }

            return lstBrands;
        }

    }
}
