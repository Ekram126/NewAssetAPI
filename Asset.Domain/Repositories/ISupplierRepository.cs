﻿using Asset.Models;
using Asset.ViewModels.SupplierVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Repositories
{
    public interface ISupplierRepository
    {
        IEnumerable<IndexSupplierVM.GetData> GetAll();
        IEnumerable<IndexSupplierVM.GetData> GetTop10Suppliers(int hospitalId);
        EditSupplierVM GetById(int id);
        IEnumerable<Supplier> GetAllSuppliers();
        IEnumerable<IndexSupplierVM.GetData> GetSupplierByName(string supplierName);
        int Add(CreateSupplierVM supplierObj);
        int Update(EditSupplierVM supplierObj);
        int CountSuppliers();
        int Delete(int id);
        IEnumerable<IndexSupplierVM.GetData> SortSuppliers(SortSupplierVM sortObj);
        IndexSupplierVM FindSupplier(string strText, int pageNumber, int pageSize);

        IEnumerable<IndexSupplierVM.GetData> FindSupplierByText(string strText);
    }
}
