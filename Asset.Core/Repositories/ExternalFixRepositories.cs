using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.AssetStatusTransactionVM;
using Asset.ViewModels.ExternalFixFileVM;
using Asset.ViewModels.ExternalFixVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class ExternalFixRepositories : IExternalFixRepository
    {
        private ApplicationDbContext _context;

        public ExternalFixRepositories(ApplicationDbContext context)
        {
            _context = context;
        }



        public int Delete(int id)
        {
            var externalFixObj = _context.ExternalFixes.Find(id);
            try
            {
                if (externalFixObj != null)
                {

                    var lstAssetTransactions = _context.AssetStatusTransactions.Where(a => a.AssetDetailId == externalFixObj.AssetDetailId
                    && a.StatusDate.Value.Year == externalFixObj.OutDate.Value.Year
                      && a.StatusDate.Value.Month == externalFixObj.OutDate.Value.Month
                        && a.StatusDate.Value.Day == externalFixObj.OutDate.Value.Day
                    ).ToList();
         
                    if(lstAssetTransactions.Count > 0)
                    {
                        _context.AssetStatusTransactions.Remove(lstAssetTransactions[0]);
                        _context.SaveChanges();
                    }



                    _context.ExternalFixes.Remove(externalFixObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }

        public IEnumerable<IndexExternalFixVM.GetData> GetAll()
        {
            return _context.ExternalFixes.Include(a => a.AssetDetail)
                .Include(a => a.AssetDetail.MasterAsset)
                 .Include(a => a.AssetDetail.MasterAsset.brand)
                  .Include(a => a.AssetDetail.Supplier)
                   .Include(a => a.AssetDetail.Department)
                .ToList().Select(item => new IndexExternalFixVM.GetData
                {
                    Id = item.Id,
                    AssetName = item.AssetDetail.MasterAsset.Name,
                    AssetNameAr = item.AssetDetail.MasterAsset.NameAr,
                    DepartmentName = item.AssetDetail.Department.Name,
                    DepartmentNameAr = item.AssetDetail.Department.NameAr,
                    SupplierName = item.AssetDetail.Supplier.Name,
                    SupplierNameAr = item.AssetDetail.Supplier.NameAr,
                    BrandName = item.AssetDetail.MasterAsset.brand.Name,
                    BrandNameAr = item.AssetDetail.MasterAsset.brand.NameAr,
                    ModelNumber = item.AssetDetail.MasterAsset.ModelNumber,
                    SerialNumber = item.AssetDetail.SerialNumber,
                    Barcode = item.AssetDetail.Barcode,
                    ComingDate = item.ComingDate

                });
        }

        public IndexExternalFixVM GetAllWithPaging(int hospitalId, int pageNumber, int pageSize)
        {
            IndexExternalFixVM mainClass = new IndexExternalFixVM();
            var lstExternalFixes = _context.ExternalFixes.Include(a => a.AssetDetail)
                  .Include(a => a.AssetDetail.MasterAsset)
                   .Include(a => a.AssetDetail.MasterAsset.brand)
                    .Include(a => a.AssetDetail.Supplier)
                     .Include(a => a.AssetDetail.Department)
                  .ToList().Select(item => new IndexExternalFixVM.GetData
                  {
                      Id = item.Id,
                      AssetName = item.AssetDetail.MasterAsset.Name,
                      HospitalId = (int)item.AssetDetail.HospitalId,
                      AssetNameAr = item.AssetDetail.MasterAsset.NameAr,
                      DepartmentName = item.AssetDetail.Department.Name,
                      DepartmentNameAr = item.AssetDetail.Department.NameAr,
                      SupplierName = item.AssetDetail.Supplier.Name,
                      SupplierNameAr = item.AssetDetail.Supplier.NameAr,
                      BrandName = item.AssetDetail.MasterAsset.brand.Name,
                      BrandNameAr = item.AssetDetail.MasterAsset.brand.NameAr,
                      ModelNumber = item.AssetDetail.MasterAsset.ModelNumber,
                      SerialNumber = item.AssetDetail.SerialNumber,
                      Barcode = item.AssetDetail.Barcode,
                      AssetDetailId = item.AssetDetailId,
                      ComingDate = item.ComingDate
                  }).ToList();

            if (hospitalId == 0)
                lstExternalFixes = lstExternalFixes.ToList();
            else
                lstExternalFixes = lstExternalFixes.Where(a => a.HospitalId == hospitalId).ToList();


            mainClass.Results = lstExternalFixes.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            mainClass.Count = lstExternalFixes.Count;
            return mainClass;
        }

        public object GetById(int id)
        {
            throw new NotImplementedException();
        }
        public int Add(CreateExternalFixVM model)
        {

            try
            {
                if (model != null)
                {
                    ExternalFix externalFixObj = new ExternalFix();
                    externalFixObj.OutDate = model.OutDate;
                    externalFixObj.HospitalId = model.HospitalId;
                    externalFixObj.ExpectedDate = model.ExpectedDate;
                    externalFixObj.ComingDate = model.ComingDate;
                    externalFixObj.SupplierId = model.SupplierId;
                    externalFixObj.AssetDetailId = model.AssetDetailId;
                    externalFixObj.Notes = model.Notes;
                    externalFixObj.OutNumber = model.OutNumber;

                    _context.ExternalFixes.Add(externalFixObj);
                    _context.SaveChanges();
                    return externalFixObj.Id;
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return 0;
        }



        public int AddExternalFixFile(CreateExternalFixFileVM externalFixFileObj)
        {
            try
            {
                if (externalFixFileObj != null)
                {

                    ExternalFixFile externalFixFile = new ExternalFixFile();
                    externalFixFile.FileName = externalFixFileObj.FileName;
                    externalFixFile.Title = externalFixFileObj.Title;
                    externalFixFile.ExternalFixId = externalFixFileObj.ExternalFixId;
                    externalFixFile.HospitalId = externalFixFileObj.HospitalId;
                    _context.ExternalFixFiles.Add(externalFixFile);
                    _context.SaveChanges();

                    return 1;

                }
            }
            catch (Exception ex)
            {
                // msg = ex.Message;
            }

            return 0;
        }

        public IEnumerable<ExternalFixFile> GetFilesByExternalFixFileId(int externalFixId)
        {
            return _context.ExternalFixFiles.Where(a => a.ExternalFixId == externalFixId).ToList();
        }

        public GenerateExternalFixNumberVM GenerateExternalFixNumber()
        {
            GenerateExternalFixNumberVM numberObj = new GenerateExternalFixNumberVM();
            string str = "ExtrnlFix";

            var lstIds = _context.ExternalFixes.ToList();
            if (lstIds.Count > 0)
            {
                var code = lstIds.LastOrDefault().Id;
                numberObj.OutNumber = str + (code + 1);
            }
            else
            {
                numberObj.OutNumber = str + 1;
            }

            return numberObj;
        }



        public ViewExternalFixVM ViewExternalFixById(int externalFixId)
        {
            ViewExternalFixVM viewExternalFixObj = new ViewExternalFixVM();
            var lstExternalFix = _context.ExternalFixes
                  .Include(a => a.AssetDetail)
                  .Include(a => a.AssetDetail.Department)
                  .Include(a => a.AssetDetail.MasterAsset)
                  .Include(a => a.AssetDetail.MasterAsset.brand)
                  .Include(a => a.AssetDetail.Supplier)
                  .Where(a => a.Id == externalFixId).ToList();

            if (lstExternalFix.Count > 0)
            {
                ExternalFix a = lstExternalFix[0];
                viewExternalFixObj.Id = a.Id;
                viewExternalFixObj.ComingDate = a.ComingDate;
                viewExternalFixObj.ComingNotes = a.ComingNotes;
                viewExternalFixObj.Notes = a.Notes;
                viewExternalFixObj.ExpectedDate = a.ExpectedDate;
                viewExternalFixObj.OutDate = DateTime.Parse(a.OutDate.ToString());
                viewExternalFixObj.OutNumber = a.OutNumber;
                viewExternalFixObj.AssetName = a.AssetDetail.MasterAsset.Name;
                viewExternalFixObj.AssetNameAr = a.AssetDetail.MasterAsset.NameAr;
                viewExternalFixObj.BrandName = a.AssetDetail.MasterAsset.brand.Name;
                viewExternalFixObj.BrandNameAr = a.AssetDetail.MasterAsset.brand.NameAr;
                viewExternalFixObj.SupplierName = a.AssetDetail.Supplier.Name;
                viewExternalFixObj.SupplierNameAr = a.AssetDetail.Supplier.NameAr;
                viewExternalFixObj.DepartmentName = a.AssetDetail.Department.Name;
                viewExternalFixObj.DepartmentNameAr = a.AssetDetail.Department.NameAr;
                viewExternalFixObj.SerialNumber = a.AssetDetail.SerialNumber;
                viewExternalFixObj.ModelNumber = a.AssetDetail.MasterAsset.ModelNumber;
                viewExternalFixObj.Barcode = a.AssetDetail.Barcode;
                viewExternalFixObj.AssetDetailId = a.AssetDetail.Id;
                viewExternalFixObj.HospitalId = (int)a.HospitalId;

                var listFiles = _context.ExternalFixFiles.Where(a => a.ExternalFixId == externalFixId).ToList().Select(file => new IndexExternalFixFileVM.GetData
                {

                    Id = file.Id,
                    FileName = file.FileName,
                    Title = file.Title,
                    ExternalFixId = file.ExternalFixId,
                    HospitalId = file.HospitalId,
                }).ToList();
                viewExternalFixObj.ListExternalFixFiles = listFiles;
            }



            return viewExternalFixObj;
        }

        public void Update(EditExternalFixVM editExternalFixVMObj)
        {


            try
            {
                var externalFixObj = _context.ExternalFixes.Find(editExternalFixVMObj.Id);
                externalFixObj.ComingDate = editExternalFixVMObj.ComingDate;
                externalFixObj.ComingNotes = editExternalFixVMObj.ComingNotes;
                _context.Entry(externalFixObj).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }
    }
}


