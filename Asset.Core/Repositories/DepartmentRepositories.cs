using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.DepartmentVM;
using Asset.ViewModels.RoleCategoryVM;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Asset.Core.Repositories
{
    public class DepartmentRepositories : IDepartmentRepository
    {
        private ApplicationDbContext _context;
        string msg;

        public DepartmentRepositories(ApplicationDbContext context)
        {
            _context = context;
        }


        public EditDepartmentVM GetById(int id)
        {
            //var DepartmentObj = _context.Departments.Where(a => a.Id == id).Select(item => new EditDepartmentVM
            //{
            //    Id = item.Id,
            //    Code = item.Code,
            //    Name = item.Name,
            //    NameAr = item.NameAr,

            //}).First();

            EditDepartmentVM departmentObj = new EditDepartmentVM();
            var lstDepartments = _context.Departments.Where(a => a.Id == id).ToList();
            if (lstDepartments.Count > 0)
            {
                var departObj = _context.Departments.Find(id);
                departmentObj.Id = departObj.Id;
                departmentObj.Code = departObj.Code;
                departmentObj.Name = departObj.Name;
                departmentObj.NameAr = departObj.NameAr;
            }
            return departmentObj;
        }




        public IEnumerable<IndexDepartmentVM.GetData> GetAll()
        {
            var lstDepartments = _context.Departments.ToList().Select(item => new IndexDepartmentVM.GetData
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                NameAr = item.NameAr
            });

            return lstDepartments;
        }

        public int Add(CreateDepartmentVM DepartmentVM)
        {
            Department DepartmentObj = new Department();
            try
            {
                if (DepartmentVM != null)
                {

                    DepartmentObj.Code = DepartmentVM.Code;
                    DepartmentObj.Name = DepartmentVM.Name;
                    DepartmentObj.NameAr = DepartmentVM.NameAr;
                    _context.Departments.Add(DepartmentObj);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return DepartmentObj.Id;
        }

        public int Delete(int id)
        {
            var DepartmentObj = _context.Departments.Find(id);
            try
            {
                if (DepartmentObj != null)
                {
                    _context.Departments.Remove(DepartmentObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return 0;
        }

        public int Update(EditDepartmentVM DepartmentVM)
        {
            try
            {

                var DepartmentObj = _context.Departments.Find(DepartmentVM.Id);
                DepartmentObj.Id = DepartmentVM.Id;
                DepartmentObj.Code = DepartmentVM.Code;
                DepartmentObj.Name = DepartmentVM.Name;
                DepartmentObj.NameAr = DepartmentVM.NameAr;
                _context.Entry(DepartmentObj).State = EntityState.Modified;
                _context.SaveChanges();
                return DepartmentObj.Id;



            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return 0;
        }

        public IEnumerable<Department> GetAllDepartments()
        {
            return _context.Departments.ToList();
        }


        public IEnumerable<IndexDepartmentVM.GetData> SortDepartments(SortDepartmentVM sortObj)
        {
            var lstDepartments = GetAll().ToList();
            if (sortObj.Code != "")
            {
                if (sortObj.SortStatus == "descending")
                    lstDepartments = lstDepartments.OrderByDescending(d => d.Code).ToList();
                else
                    lstDepartments = lstDepartments.OrderBy(d => d.Code).ToList();
            }
            else if (sortObj.Name != "")
            {
                if (sortObj.SortStatus == "descending")
                    lstDepartments = lstDepartments.OrderByDescending(d => d.Name).ToList();
                else
                    lstDepartments = lstDepartments.OrderBy(d => d.Name).ToList();
            }

            else if (sortObj.NameAr != "")
            {
                if (sortObj.SortStatus == "descending")
                    lstDepartments = lstDepartments.OrderByDescending(d => d.NameAr).ToList();
                else
                    lstDepartments = lstDepartments.OrderBy(d => d.NameAr).ToList();
            }

            return lstDepartments;
        }

        public IEnumerable<Department> GetDepartmentsByHospitalId(int hospitalId)
        {
            if (hospitalId != 0)
            {
                return _context.HospitalDepartments.Include(a => a.Department).Where(a => a.HospitalId == hospitalId).ToList().Select(item => new Department
                {
                    Id = item.Department.Id,
                    Code = item.Department.Code,
                    Name = item.Department.Name,
                    NameAr = item.Department.NameAr
                });
            }
            else
            {
                return _context.HospitalDepartments.Include(a => a.Department).ToList().Select(item => new Department
                {
                    Id = item.Department.Id,
                    Code = item.Department.Code,
                    Name = item.Department.Name,
                    NameAr = item.Department.NameAr
                });
            }

        }
    }
}