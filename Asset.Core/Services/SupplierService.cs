using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.SupplierVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
   public class SupplierService: ISupplierService
    {
        private IUnitOfWork _unitOfWork;

        public SupplierService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Add(CreateSupplierVM supplierObj)
        {
            _unitOfWork.SupplierRepository.Add(supplierObj);
            return _unitOfWork.CommitAsync();
        }

        public int CountSuppliers()
        {
            return _unitOfWork.SupplierRepository.CountSuppliers();
        }

        public int Delete(int id)
        {
            var supplierObj = _unitOfWork.SupplierRepository.GetById(id);
            _unitOfWork.SupplierRepository.Delete(supplierObj.Id);
            _unitOfWork.CommitAsync();
            return supplierObj.Id;
        }

        public IEnumerable<IndexSupplierVM.GetData> GetAll()
        {
            return _unitOfWork.SupplierRepository.GetAll();
        }

        public IEnumerable<Supplier> GetAllSuppliers()
        {
            return _unitOfWork.SupplierRepository.GetAllSuppliers();
        }

        public EditSupplierVM GetById(int id)
        {
            return _unitOfWork.SupplierRepository.GetById(id);
        }

        public IEnumerable<IndexSupplierVM.GetData> GetSupplierByName(string supplierName)
        {
            return _unitOfWork.SupplierRepository.GetSupplierByName(supplierName);
        }

        public IEnumerable<IndexSupplierVM.GetData> GetTop10Suppliers()
        {
            return _unitOfWork.SupplierRepository.GetTop10Suppliers();
        }

        public int Update(EditSupplierVM supplierObj)
        {
            _unitOfWork.SupplierRepository.Update(supplierObj);
            _unitOfWork.CommitAsync();
            return supplierObj.Id;
        }

        public IEnumerable<IndexSupplierVM.GetData> SortSuppliers(SortSupplierVM sortObj)
        {
            return _unitOfWork.SupplierRepository.SortSuppliers(sortObj);
        }
    }
}
