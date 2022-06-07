using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.EmployeeVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class EmployeeRepositories : IEmployeeRepository
    {
        private ApplicationDbContext _context;


        public EmployeeRepositories(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Add(CreateEmployeeVM model)
        {

            try
            {
                if (model != null)
                {
                    Employee employeeObj = new Employee();
                    employeeObj.Code = model.Code;
                    employeeObj.Name = model.Name;
                    employeeObj.NameAr = model.NameAr;
                    employeeObj.EmpImg = model.EmpImg;
                    employeeObj.CardId = model.CardId;
                    employeeObj.Email = model.Email;
                    employeeObj.Address = model.Address;
                    employeeObj.AddressAr = model.AddressAr;
                    employeeObj.Phone = model.Phone;
                    employeeObj.Dob = model.Dob != "" ? DateTime.Parse(model.Dob) : null;
                    employeeObj.WhatsApp = model.WhatsApp;
                    employeeObj.GenderId = model.GenderId;
                    employeeObj.HospitalId = model.HospitalId;
                    employeeObj.DepartmentId = model.DepartmentId;
                    employeeObj.ClassificationId = model.ClassificationId;
                    _context.Employees.Add(employeeObj);
                    _context.SaveChanges();
                    return employeeObj.Id;
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }

            return 0;

        }

        public int Delete(int id)
        {
            var employeeObj = _context.Employees.Find(id);
            try
            {
                _context.Employees.Remove(employeeObj);
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }

        public IEnumerable<IndexEmployeeVM.GetData> GetAll()
        {

            List<IndexEmployeeVM.GetData> list = new List<IndexEmployeeVM.GetData>();
            var lstEmployees = _context.Employees.ToList();
            if (lstEmployees.Count > 0)
            {
                foreach (var item in lstEmployees)
                {
                    IndexEmployeeVM.GetData getDataObj = new IndexEmployeeVM.GetData();
                    getDataObj.Id = item.Id;
                    getDataObj.Name = item.Name;
                    getDataObj.NameAr = item.NameAr;
                    getDataObj.Email = item.Email;
                    getDataObj.Code = item.Code;
                    getDataObj.GenderId = item.GenderId;
                    getDataObj.WhatsApp = item.WhatsApp;
                    getDataObj.Phone = item.Phone;
                    getDataObj.Address = item.Address;
                    getDataObj.AddressAr = item.AddressAr;
                    getDataObj.CardId = item.CardId;
                    getDataObj.HospitalId = item.HospitalId;
                    getDataObj.DepartmentId = item.DepartmentId;
                    getDataObj.ClassificationId = item.ClassificationId;
                    getDataObj.EmpImg = item.EmpImg;

                    if (item.HospitalId != 0 || item.HospitalId != null)
                    {
                        var lstHospitals = _context.Hospitals.Where(a => a.Id == item.HospitalId).ToList();
                        if (lstHospitals.Count > 0)
                        {
                            getDataObj.HospitalName = lstHospitals[0].Name;
                            getDataObj.HospitalNameAr = lstHospitals[0].NameAr;
                        }
                    }

                    if (item.DepartmentId != 0 || item.DepartmentId != null)
                    {
                        var lstDepartments = _context.Departments.Where(a => a.Id == item.DepartmentId).ToList();
                        if (lstDepartments.Count > 0)
                        {
                            getDataObj.DepartmentName = lstDepartments[0].Name;
                            getDataObj.DepartmentNameAr = lstDepartments[0].NameAr;
                        }
                    }

                    list.Add(getDataObj);
                }
            }
            return list;
        }

        public EditEmployeeVM GetById(int id)
        {
            return _context.Employees.Where(a => a.Id == id).Select(item => new EditEmployeeVM
            {
                Id = item.Id,

                Name = item.Name,
                NameAr = item.NameAr,
                Dob = item.Dob.Value.ToShortDateString(),
                Email = item.Email,
                Code = item.Code,
                GenderId = item.GenderId,
                WhatsApp = item.WhatsApp,
                Phone = item.Phone,
                Address = item.Address,
                AddressAr = item.AddressAr,
                CardId = item.CardId,
                HospitalId = item.HospitalId,
                DepartmentId = item.DepartmentId,
                ClassificationId = item.ClassificationId,
                EmpImg = item.EmpImg
            }).First();
        }

        public List<Employee> GetEmployeesAssetOwnerByHospitalId(int hospitalId)
        {
            var lstOwners = (from ownr in _context.AssetOwners
                             join detail in _context.AssetDetails on ownr.AssetDetailId equals detail.Id
                             join emp in _context.Employees on ownr.EmployeeId equals emp.Id
                             where detail.HospitalId == hospitalId
                             select emp).ToList();

            return lstOwners;
        }

        public List<Employee> GetEmployeesAssetOwnerByHospitalId(int hospitalId, int assetDetailId)
        {
            var lstOwners = (from ownr in _context.AssetOwners
                             join detail in _context.AssetDetails on ownr.AssetDetailId equals detail.Id
                             join emp in _context.Employees on ownr.EmployeeId equals emp.Id
                             where detail.Id == assetDetailId
                             && detail.HospitalId == hospitalId
                             select emp).ToList();

            return lstOwners;
        }




        public List<EmployeeEngVM> GetEmployeesHasEngRoleInHospital(int hospitalId)
        {
            List<EmployeeEngVM> lstEngineers = new List<EmployeeEngVM>();
            var lstEmployees = _context.Employees.Where(a => a.HospitalId == hospitalId).ToList();
            foreach (var empObj in lstEmployees)
            {
                var lstUsers = _context.ApplicationUser.Where(a => a.Email == empObj.Email).ToList();
                if (lstUsers.Count > 0)
                {
                    foreach (var usrObj in lstUsers)
                    {
                        var lstRoles = (from roles in _context.ApplicationRole
                                        join usrroles in _context.UserRoles on roles.Id equals usrroles.RoleId
                                        where roles.Name == "EngDepManager"
                                        && usrroles.UserId == usrObj.Id
                                        select roles).ToList();

                        if (lstRoles.Count > 0)
                        {
                            EmployeeEngVM employeeEngObj = new EmployeeEngVM();
                            employeeEngObj.Name = usrObj.UserName;
                            employeeEngObj.roleName = lstRoles[0].Name;
                            employeeEngObj.UserId = usrObj.Id;
                            employeeEngObj.Id = empObj.Id;
                            lstEngineers.Add(employeeEngObj);
                        }
                    }
                }
            }

            return lstEngineers;
        }


        public List<EmployeeEngVM> GetEmployeesHasEngDepManagerRoleInHospital(int hospitalId)
        {

            List<EmployeeEngVM> list = new List<EmployeeEngVM>();
            var lstEngineers = (from usr in _context.ApplicationUser
                                join role in _context.UserRoles on usr.Id equals role.UserId
                                where usr.HospitalId == hospitalId
                                select usr).ToList().ToList();



            if (lstEngineers.Count > 0)
            {
                foreach (var usr in lstEngineers)
                {

                    EmployeeEngVM engObj = new EmployeeEngVM();
                    engObj.Name = usr.UserName;
                    var lstRoles = _context.UserRoles.Where(a => a.UserId == usr.Id).ToList();
                    if (lstRoles.Count > 0)
                    {
                        engObj.roleName = _context.ApplicationRole.Where(a => a.Id == lstRoles[0].RoleId).FirstOrDefault().Name;
                    }
                    engObj.UserId = usr.Id;
                    var lstEmployees = _context.Employees.Where(a => a.Email == usr.Email).ToList();
                    if (lstEmployees.Count > 0)
                    {
                        engObj.Id = lstEmployees[0].Id;
                    }

                    list.Add(engObj);
                }
            }

            list = list.Where(a => a.roleName == "Eng").ToList();
            return list;
        }


        public List<Employee> GetEmployeesByHospitalId(int hospitalId)
        {
            return _context.Employees.Where(a => a.HospitalId == hospitalId).ToList();
        }

        public int Update(EditEmployeeVM model)
        {
            try
            {
                var employeeObj = _context.Employees.Find(model.Id);
                employeeObj.Id = model.Id;
                employeeObj.Code = model.Code;
                employeeObj.Name = model.Name;
                employeeObj.NameAr = model.NameAr;
                employeeObj.EmpImg = model.EmpImg;
                employeeObj.CardId = model.CardId;
                employeeObj.Email = model.Email;
                employeeObj.Address = model.Address;
                employeeObj.AddressAr = model.AddressAr;
                employeeObj.Phone = model.Phone;
                if (model.Dob != null)
                    employeeObj.Dob = DateTime.Parse(model.Dob);
                employeeObj.WhatsApp = model.WhatsApp;
                employeeObj.GenderId = model.GenderId;
                employeeObj.HospitalId = model.HospitalId;
                employeeObj.DepartmentId = model.DepartmentId;
                employeeObj.ClassificationId = model.ClassificationId;
                _context.Entry(employeeObj).State = EntityState.Modified;
                _context.SaveChanges();
                return employeeObj.Id;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }

        public IEnumerable<IndexEmployeeVM.GetData> SortEmployee(SortEmployeeVM sortObj)
        {
            List<IndexEmployeeVM.GetData> list = new List<IndexEmployeeVM.GetData>();
            var lstEmployees = _context.Employees.ToList();
            if (lstEmployees.Count > 0)
            {
                foreach (var item in lstEmployees)
                {
                    IndexEmployeeVM.GetData getDataObj = new IndexEmployeeVM.GetData();
                    getDataObj.Id = item.Id;
                    getDataObj.Name = item.Name;
                    getDataObj.NameAr = item.NameAr;
                    getDataObj.Email = item.Email;
                    getDataObj.Code = item.Code;
                    getDataObj.GenderId = item.GenderId;
                    getDataObj.WhatsApp = item.WhatsApp;
                    getDataObj.Phone = item.Phone;
                    getDataObj.Address = item.Address;
                    getDataObj.AddressAr = item.AddressAr;
                    getDataObj.CardId = item.CardId;
                    getDataObj.HospitalId = item.HospitalId;
                    getDataObj.DepartmentId = item.DepartmentId;
                    getDataObj.ClassificationId = item.ClassificationId;
                    getDataObj.EmpImg = item.EmpImg;

                    if (item.HospitalId != 0 || item.HospitalId != null)
                    {
                        var lstHospitals = _context.Hospitals.Where(a => a.Id == item.HospitalId).ToList();
                        if (lstHospitals.Count > 0)
                        {
                            getDataObj.HospitalName = lstHospitals[0].Name;
                            getDataObj.HospitalNameAr = lstHospitals[0].NameAr;
                        }
                    }

                    if (item.DepartmentId != 0 || item.DepartmentId != null)
                    {
                        var lstDepartments = _context.Departments.Where(a => a.Id == item.DepartmentId).ToList();
                        if (lstDepartments.Count > 0)
                        {
                            getDataObj.DepartmentName = lstDepartments[0].Name;
                            getDataObj.DepartmentNameAr = lstDepartments[0].NameAr;
                        }
                    }

                    list.Add(getDataObj);
                }
                if (sortObj.Code != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.Code).ToList();
                    else
                        list = list.OrderBy(d => d.Code).ToList();
                }
                else if (sortObj.Name != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.Name).ToList();
                    else
                        list = list.OrderBy(d => d.Name).ToList();
                }
                else if (sortObj.NameAr != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.NameAr).ToList();
                    else
                        list = list.OrderBy(d => d.NameAr).ToList();
                }
                else if (sortObj.DepartmentName != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.DepartmentName).ToList();
                    else
                        list = list.OrderBy(d => d.DepartmentName).ToList();
                }
                else if (sortObj.DepartmentNameAr != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.DepartmentNameAr).ToList();
                    else
                        list = list.OrderBy(d => d.DepartmentNameAr).ToList();
                }
                else if (sortObj.Email != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.Email).ToList();
                    else
                        list = list.OrderBy(d => d.Email).ToList();
                }
                else if (sortObj.HospitalName != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.HospitalName).ToList();
                    else
                        list = list.OrderBy(d => d.HospitalName).ToList();
                }
                else if (sortObj.HospitalNameAr != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.HospitalNameAr).ToList();
                    else
                        list = list.OrderBy(d => d.HospitalNameAr).ToList();
                }
            }
            return list;
        }

        public List<EmployeeEngVM> GetEmployeesEngineersByHospitalId(int hospitalId)
        {
            List<EmployeeEngVM> lstEngineers = new List<EmployeeEngVM>();
            var listEngEmployees = (from emp in _context.Employees
                                    join usr in _context.ApplicationUser on emp.Email equals usr.Email
                                    // join role in _context.ApplicationRole on usr.RoleId equals role.Id
                                    where usr.HospitalId == hospitalId
                                    select usr).ToList();

            foreach (var item in listEngEmployees)
            {
                var empObj = _context.Employees.Where(a => a.Email == item.Email).ToList().FirstOrDefault();

                var itemObj = (from role in _context.ApplicationRole
                               join usrrole in _context.UserRoles on role.Id equals usrrole.RoleId
                               where role.Name == "EngDepManager" || role.Name == "Eng"
                               select new EmployeeEngVM
                               {
                                   Name = item.UserName,
                                   roleName = role.Name,
                                   UserId = item.Id,
                                   Id = empObj.Id
                               }).FirstOrDefault();

                lstEngineers.Add(itemObj);
            }
            return lstEngineers;
        }
    }
}
