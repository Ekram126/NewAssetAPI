using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.SupplierVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class SupplierRepositories : ISupplierRepository
    {
        private ApplicationDbContext _context;


        public SupplierRepositories(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Add(CreateSupplierVM model)
        {
            Supplier supplierObj = new Supplier();
            try
            {
                if (model != null)
                {
                    supplierObj.Code = model.Code;
                    supplierObj.Name = model.Name.Trim();
                    supplierObj.NameAr = model.NameAr.Trim();
                    supplierObj.Mobile = model.Mobile;
                    supplierObj.Website = model.Website;
                    supplierObj.EMail = model.EMail;
                    supplierObj.Address = model.Address;
                    supplierObj.AddressAr = model.AddressAr;
                    supplierObj.ContactPerson = model.ContactPerson;
                    supplierObj.Notes = model.Notes;
                    supplierObj.Fax = model.Fax;
                    _context.Suppliers.Add(supplierObj);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return supplierObj.Id;
        }

        public int CountSuppliers()
        {
            return _context.Suppliers.Count();
        }

        public int Delete(int id)
        {
            var supplierObj = _context.Suppliers.Find(id);
            try
            {
                _context.Suppliers.Remove(supplierObj);
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }

        public IEnumerable<IndexSupplierVM.GetData> GetAll()
        {
            return _context.Suppliers.ToList().Select(item => new IndexSupplierVM.GetData
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                NameAr = item.NameAr,
                Address = item.Address,
                AddressAr = item.AddressAr,
                EMail = item.EMail,
                Mobile = item.Mobile,
                Website = item.Website,
                ContactPerson = item.ContactPerson,
                //Notes = item.Notes,
                //Fax = item.Fax
            });
        }

        public IEnumerable<Supplier> GetAllSuppliers()
        {
            return _context.Suppliers.ToList();
        }

        public EditSupplierVM GetById(int id)
        {
            EditSupplierVM item = new EditSupplierVM();
            var lstSuppliers = _context.Suppliers.Where(a => a.Id == id).ToList();
            if (lstSuppliers.Count > 0)
            {
                var supplierObj = lstSuppliers[0];
                item.Id = supplierObj.Id;
                item.Code = supplierObj.Code;
                item.Name = supplierObj.Name;
                item.NameAr = supplierObj.NameAr;
                item.Address = supplierObj.Address;
                item.AddressAr = supplierObj.AddressAr;
                item.EMail = supplierObj.EMail;
                item.Mobile = supplierObj.Mobile;
                item.Website = supplierObj.Website;
                item.ContactPerson = supplierObj.ContactPerson;
                item.Fax = supplierObj.Fax;
                item.Notes = supplierObj.Notes;
            }
            return item;
        }

        public IEnumerable<IndexSupplierVM.GetData> GetSupplierByName(string supplierName)
        {
            return _context.Suppliers.Where(a => a.Name == supplierName || a.NameAr == supplierName).ToList().Select(item => new IndexSupplierVM.GetData
            {
                Id = item.Id,
                Code = item.Code != null ? item.Code : "",
                Name = item.Name.Trim(),
                NameAr = item.NameAr.Trim(),
                Address = item.Address != null ? item.Address : "",
                AddressAr = item.AddressAr != null ? item.AddressAr : "",
                EMail = item.EMail != null ? item.EMail : "",
                Mobile = item.Mobile != null ? item.Mobile : "",
                Website = item.Website != null ? item.Website : "",
                ContactPerson = item.ContactPerson,
                Fax = item.Fax,
                Notes = item.Notes
            });
        }

        public IEnumerable<IndexSupplierVM.GetData> GetTop10Suppliers(int hospitalId)
        {
            if (hospitalId != 0)
            {
                return _context.AssetDetails.Include(a => a.Supplier).ToList().Where(a => a.HospitalId == hospitalId).ToList().GroupBy(a => a.SupplierId)
                    .Select(item => new IndexSupplierVM.GetData
                    {
                        Id = item.FirstOrDefault().Supplier != null ? item.FirstOrDefault().Supplier.Id : 0,
                        Code = item.FirstOrDefault().Supplier != null ? item.FirstOrDefault().Supplier.Code : "",
                        Name = item.FirstOrDefault().Supplier != null ? item.FirstOrDefault().Supplier.Name.Trim() : "",
                        NameAr = item.FirstOrDefault().Supplier != null ? item.FirstOrDefault().Supplier.NameAr.Trim() : ""
                    });


            }
            else
            {
                return _context.Suppliers.ToList().Select(item => new IndexSupplierVM.GetData
                {
                    Id = item.Id,
                    Code = item.Code,
                    Name = item.Name,
                    NameAr = item.NameAr,
                    Address = item.Address,
                    AddressAr = item.AddressAr,
                    EMail = item.EMail,
                    Mobile = item.Mobile,
                    Website = item.Website,
                    ContactPerson = item.ContactPerson,
                    Fax = item.Fax,
                    Notes = item.Notes
                });
            }
        }

        public int Update(EditSupplierVM model)
        {
            try
            {
                var supplierObj = _context.Suppliers.Find(model.Id);
                supplierObj.Id = model.Id;
                supplierObj.Code = model.Code;
                supplierObj.Name = model.Name;
                supplierObj.NameAr = model.NameAr;
                supplierObj.Mobile = model.Mobile;
                supplierObj.Website = model.Website;
                supplierObj.EMail = model.EMail;
                supplierObj.Address = model.Address;
                supplierObj.AddressAr = model.AddressAr;
                supplierObj.ContactPerson = model.ContactPerson;
                supplierObj.Notes = model.Notes;
                supplierObj.Fax = model.Fax;
                _context.Entry(supplierObj).State = EntityState.Modified;
                _context.SaveChanges();
                return supplierObj.Id;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }

        public IEnumerable<IndexSupplierVM.GetData> SortSuppliers(SortSupplierVM sortObj)
        {
            var lstSuppliers = GetAll().ToList();
            if (sortObj.Code != "")
            {
                if (sortObj.SortStatus == "descending")
                    lstSuppliers = lstSuppliers.OrderByDescending(d => d.Code).ToList();
                else
                    lstSuppliers = lstSuppliers.OrderBy(d => d.Code).ToList();
            }
            else if (sortObj.Name != "")
            {
                if (sortObj.SortStatus == "descending")
                    lstSuppliers = lstSuppliers.OrderByDescending(d => d.Name).ToList();
                else
                    lstSuppliers = lstSuppliers.OrderBy(d => d.Name).ToList();
            }

            else if (sortObj.NameAr != "")
            {
                if (sortObj.SortStatus == "descending")
                    lstSuppliers = lstSuppliers.OrderByDescending(d => d.NameAr).ToList();
                else
                    lstSuppliers = lstSuppliers.OrderBy(d => d.NameAr).ToList();
            }

            else if (sortObj.Email != "")
            {
                if (sortObj.SortStatus == "descending")
                    lstSuppliers = lstSuppliers.OrderByDescending(d => d.EMail).ToList();
                else
                    lstSuppliers = lstSuppliers.OrderBy(d => d.EMail).ToList();
            }
            else if (sortObj.ContactPerson != "")
            {
                if (sortObj.SortStatus == "descending")
                    lstSuppliers = lstSuppliers.OrderByDescending(d => d.ContactPerson).ToList();
                else
                    lstSuppliers = lstSuppliers.OrderBy(d => d.ContactPerson).ToList();
            }

            else if (sortObj.Address != "")
            {
                if (sortObj.SortStatus == "descending")
                    lstSuppliers = lstSuppliers.OrderByDescending(d => d.Address).ToList();
                else
                    lstSuppliers = lstSuppliers.OrderBy(d => d.Address).ToList();
            }


            else if (sortObj.AddressAr != "")
            {
                if (sortObj.SortStatus == "descending")
                    lstSuppliers = lstSuppliers.OrderByDescending(d => d.AddressAr).ToList();
                else
                    lstSuppliers = lstSuppliers.OrderBy(d => d.AddressAr).ToList();
            }


            return lstSuppliers;
        }

        public IndexSupplierVM FindSupplier(string strText, int pageNumber, int pageSize)
        {
            IndexSupplierVM mainClass = new IndexSupplierVM();
            List<IndexSupplierVM.GetData> list = new List<IndexSupplierVM.GetData>();


            list = _context.Suppliers.Where(a =>
            a.Name.Contains(strText)
            || a.NameAr.Contains(strText)
            || a.Mobile.Contains(strText)
            || a.Address.Contains(strText)
            || a.AddressAr.Contains(strText)
            || a.EMail.Contains(strText)
            || a.ContactPerson.Contains(strText)
            ).ToList().Select(item => new IndexSupplierVM.GetData
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name.Trim(),
                NameAr = item.NameAr.Trim(),
                Mobile = item.Mobile,
                Address = item.Address,
                AddressAr = item.AddressAr,
                EMail = item.EMail,
                Website = item.Website,
                ContactPerson = item.ContactPerson
            }).ToList();

            var supplierPerPage = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            mainClass.Results = supplierPerPage;
            mainClass.Count = list.Count();
            return mainClass;
        }

        public IEnumerable<IndexSupplierVM.GetData> FindSupplierByText(string strText)
        {
            return _context.Suppliers.Where(a =>
            a.Name == strText
            || a.NameAr == strText
            || a.Mobile.Contains(strText)
            || a.Address.Contains(strText)
            || a.AddressAr.Contains(strText)
            || a.EMail.Contains(strText)
            || a.Website.Contains(strText)
 || a.ContactPerson.Contains(strText)
            ).ToList().Select(item => new IndexSupplierVM.GetData
            {
                Code = item.Code != null ? item.Code : "",
                Name = item.Name,
                NameAr = item.NameAr,
                Address = item.Address != null ? item.Address : "",
                AddressAr = item.AddressAr != null ? item.AddressAr : "",
                EMail = item.EMail != null ? item.EMail : "",
                Mobile = item.Mobile != null ? item.Mobile : "",
                Website = item.Website != null ? item.Website : "",
                ContactPerson = item.ContactPerson,
            }).ToList();

        }

        public IndexSupplierVM GetAllSuppliersWithPaging(int pageNumber, int pageSize)
        {
            IndexSupplierVM mainClass = new IndexSupplierVM();
            List<IndexSupplierVM.GetData> list = new List<IndexSupplierVM.GetData>();
            var lstSuppliers = _context.Suppliers.ToList();
            if (lstSuppliers.Count > 0)
            {
                foreach (var item in lstSuppliers)
                {
                    IndexSupplierVM.GetData supplierObj = new IndexSupplierVM.GetData();
                    supplierObj.Id = item.Id;
                    supplierObj.Code = item.Code != null ? item.Code : "";
                    supplierObj.Name = item.Name;
                    supplierObj.NameAr = item.NameAr;
                    supplierObj.Address = item.Address != null ? item.Address : "";
                    supplierObj.AddressAr = item.AddressAr != null ? item.AddressAr : "";
                    supplierObj.EMail = item.EMail != null ? item.EMail : "";
                    supplierObj.Mobile = item.Mobile != null ? item.Mobile : "";
                    supplierObj.Website = item.Website != null ? item.Website : "";
                    supplierObj.ContactPerson = item.ContactPerson;
                    list.Add(supplierObj);
                }
                var suppliersPerPage = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                mainClass.Results = suppliersPerPage;
                mainClass.Count = list.Count();
                return mainClass;
            }
            return null;
        }
    }
}
