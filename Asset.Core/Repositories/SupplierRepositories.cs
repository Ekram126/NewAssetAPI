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
                Code=item.Code,
                Name = item.Name,
                NameAr = item.NameAr
            });
        }

        public IEnumerable<Supplier> GetAllSuppliers()
        {
            return _context.Suppliers.ToList();
        }

        public EditSupplierVM GetById(int id)
        {
            return _context.Suppliers.Where(a => a.Id == id).Select(item => new EditSupplierVM
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name.Trim(),
                NameAr = item.NameAr.Trim()
            }).First();
        }

        public IEnumerable<IndexSupplierVM.GetData> GetSupplierByName(string supplierName)
        {
            return _context.Suppliers.Where(a => a.Name == supplierName || a.NameAr == supplierName).ToList().Select(item => new IndexSupplierVM.GetData
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name.Trim(),
                NameAr = item.NameAr.Trim()
            });
        }

        public IEnumerable<IndexSupplierVM.GetData> GetTop10Suppliers(int hospitalId)
        {
            if (hospitalId != 0)
            {
                return _context.AssetDetails.Include(a => a.Supplier).ToList().Take(10).ToList().Where(a => a.HospitalId == hospitalId).ToList().GroupBy(a=>a.SupplierId).Select(item => new IndexSupplierVM.GetData
                {
                    Id = item.FirstOrDefault().Supplier.Id,
                    Code = item.FirstOrDefault().Supplier.Code,
                    Name = item.FirstOrDefault().Supplier.Name.Trim(),
                    NameAr = item.FirstOrDefault().Supplier.NameAr.Trim()
                });
            }
            else
            {
                return _context.AssetDetails.Include(a => a.Supplier).ToList().Take(10).ToList().Select(item => new IndexSupplierVM.GetData
                {
                    Id = item.Supplier.Id,
                    Code = item.Supplier.Code,
                    Name = item.Supplier.Name.Trim(),
                    NameAr = item.Supplier.NameAr.Trim()
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
