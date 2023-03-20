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

                    if (lstAssetTransactions.Count > 0)
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
                      OutDate = item.OutDate,
                      ExpectedDate = item.ExpectedDate,
                      ComingNotes = item.ComingNotes,
                      Notes = item.Notes,
                      OutNumber = item.OutNumber,
                      MasterAssetId = (int)item.AssetDetail.MasterAssetId,

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
            return _context.ExternalFixes.Find(id);
        }
        public int Add(CreateExternalFixVM model)
        {

            try
            {
                if (model != null)
                {
                    ExternalFix externalFixObj = new ExternalFix();
                    if (model.StrOutDate != "")
                        externalFixObj.OutDate = DateTime.Parse(model.StrOutDate);// model.OutDate;
                    if (model.StrExpectedDate != "")
                        externalFixObj.ExpectedDate = DateTime.Parse(model.StrExpectedDate); //model.ExpectedDate;

                    externalFixObj.HospitalId = model.HospitalId;
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
                //externalFixObj.ComingDate = editExternalFixVMObj.ComingDate;
                if (editExternalFixVMObj.StrComingDate != "")
                    externalFixObj.ComingDate = DateTime.Parse(editExternalFixVMObj.StrComingDate); //model.ExpectedDate;

                externalFixObj.ComingNotes = editExternalFixVMObj.ComingNotes;
                _context.Entry(externalFixObj).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }

        public IndexExternalFixVM GetAssetsExceed72HoursInExternalFix(int hospitalId, int pageNumber, int pageSize)
        {
            List<IndexExternalFixVM.GetData> list = new List<IndexExternalFixVM.GetData>();
            IndexExternalFixVM mainClass = new IndexExternalFixVM();
            var lstExternalFixes = _context.ExternalFixes.Include(a => a.AssetDetail)
                  .Include(a => a.AssetDetail.MasterAsset)
                   .Include(a => a.AssetDetail.MasterAsset.brand)
                    .Include(a => a.AssetDetail.Supplier)
                     .Include(a => a.AssetDetail.Department).ToList();

            foreach (var externalFixObj in lstExternalFixes)
            {
                var pass72hours = Math.Abs(externalFixObj.ExpectedDate.Value.Subtract(DateTime.Now).TotalHours) >= 72;
                if (pass72hours == true)
                {
                    IndexExternalFixVM.GetData item = new IndexExternalFixVM.GetData();
                    item.Id = externalFixObj.Id;
                    item.OutDate = externalFixObj.OutDate;
                    item.ExpectedDate = externalFixObj.ExpectedDate;
                    item.ComingDate = externalFixObj.ComingDate;
                    item.AssetName = externalFixObj.AssetDetail.MasterAsset.Name;
                    item.HospitalId = (int)externalFixObj.AssetDetail.HospitalId;
                    item.AssetNameAr = externalFixObj.AssetDetail.MasterAsset.NameAr;
                    item.DepartmentName = externalFixObj.AssetDetail.Department.Name;
                    item.DepartmentNameAr = externalFixObj.AssetDetail.Department.NameAr;
                    item.SupplierName = externalFixObj.AssetDetail.Supplier.Name;
                    item.SupplierNameAr = externalFixObj.AssetDetail.Supplier.NameAr;
                    item.BrandName = externalFixObj.AssetDetail.MasterAsset.brand.Name;
                    item.BrandNameAr = externalFixObj.AssetDetail.MasterAsset.brand.NameAr;
                    item.ModelNumber = externalFixObj.AssetDetail.MasterAsset.ModelNumber;
                    item.SerialNumber = externalFixObj.AssetDetail.SerialNumber;
                    item.Barcode = externalFixObj.AssetDetail.Barcode;
                    item.AssetDetailId = externalFixObj.AssetDetailId;
                    item.ComingDate = externalFixObj.ComingDate;
                    list.Add(item);
                }
            }
            if (hospitalId == 0)
                list = list.ToList();
            else
                list = list.Where(a => a.HospitalId == hospitalId).ToList();


            mainClass.Results = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            mainClass.Count = list.Count;
            return mainClass;
        }

        public IndexExternalFixVM SearchInExternalFix(SearchExternalFixVM searchObj, int pageNumber, int pageSize)
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
                          AssetDetailId = item.AssetDetailId,
                          ComingDate = item.ComingDate,
                          HospitalId = (int)item.HospitalId,
                          OutDate = item.OutDate,
                          ExpectedDate = item.ExpectedDate,

                          MasterAssetId = item.AssetDetail.MasterAsset != null ? item.AssetDetail.MasterAsset.Id : 0,
                          AssetName = item.AssetDetail.MasterAsset != null ? item.AssetDetail.MasterAsset.Name : "",
                          AssetNameAr = item.AssetDetail.MasterAsset != null ? item.AssetDetail.MasterAsset.NameAr : "",
                          DepartmentId = item.AssetDetail.Department != null ? int.Parse(item.AssetDetail.DepartmentId.ToString()) : 0,
                          DepartmentName = item.AssetDetail.Department != null ? item.AssetDetail.Department.Name : "",
                          DepartmentNameAr = item.AssetDetail.Department != null ? item.AssetDetail.Department.NameAr : "",
                          SupplierId = item.AssetDetail.Supplier != null ? item.AssetDetail.Supplier.Id : 0,
                          SupplierName = item.AssetDetail.Supplier != null ? item.AssetDetail.Supplier.Name : "",
                          SupplierNameAr = item.AssetDetail.Supplier != null ? item.AssetDetail.Supplier.NameAr : "",
                          BrandId = item.AssetDetail.MasterAsset.brand != null ? item.AssetDetail.MasterAsset.brand.Id : 0,
                          BrandName = item.AssetDetail.MasterAsset.brand != null ? item.AssetDetail.MasterAsset.brand.Name : "",
                          BrandNameAr = item.AssetDetail.MasterAsset.brand != null ? item.AssetDetail.MasterAsset.brand.NameAr : "",
                          ModelNumber = item.AssetDetail.MasterAsset != null ? item.AssetDetail.MasterAsset.ModelNumber : "",
                          SerialNumber = item.AssetDetail.SerialNumber,
                          Barcode = item.AssetDetail.Barcode,

                      }).ToList();



            if (searchObj.HospitalId == 0)
                lstExternalFixes = lstExternalFixes.ToList();
            else
                lstExternalFixes = lstExternalFixes.Where(a => a.HospitalId == searchObj.HospitalId).ToList();




            if (lstExternalFixes.Count > 0)
            {

                if (searchObj.ModelNumber != "")
                {
                    lstExternalFixes = lstExternalFixes.Where(b => b.ModelNumber == searchObj.ModelNumber).ToList();
                }
                else
                    lstExternalFixes = lstExternalFixes.ToList();


                if (searchObj.BrandId != 0)
                {
                    lstExternalFixes = lstExternalFixes.Where(a => a.BrandId == searchObj.BrandId).ToList();
                }
                else
                    lstExternalFixes = lstExternalFixes.ToList();


                if (searchObj.MasterAssetId != 0)
                {
                    lstExternalFixes = lstExternalFixes.Where(a => a.MasterAssetId == searchObj.MasterAssetId).ToList();
                }
                else
                    lstExternalFixes = lstExternalFixes.ToList();

                if (searchObj.DepartmentId != 0)
                {
                    lstExternalFixes = lstExternalFixes.Where(a => a.DepartmentId == searchObj.DepartmentId).ToList();
                }
                else
                    lstExternalFixes = lstExternalFixes.ToList();


                if (searchObj.Serial != "")
                {
                    lstExternalFixes = lstExternalFixes.Where(b => b.SerialNumber == searchObj.Serial).ToList();
                }
                else
                    lstExternalFixes = lstExternalFixes.ToList();

                if (searchObj.BarCode != "")
                {
                    lstExternalFixes = lstExternalFixes.Where(b => b.Barcode == searchObj.BarCode).ToList();

                }
                else
                    lstExternalFixes = lstExternalFixes.ToList();



                if (searchObj.SupplierId != 0)
                {
                    lstExternalFixes = lstExternalFixes.Where(a => a.SupplierId == searchObj.SupplierId).ToList();
                }
                else
                    lstExternalFixes = lstExternalFixes.ToList();




                string setstartday, setstartmonth, setendday, setendmonth = "";
                DateTime startingFrom = new DateTime();
                DateTime endingTo = new DateTime();
                if (searchObj.Start == "")
                {

                }
                else
                {
                    searchObj.StartDate = DateTime.Parse(searchObj.Start.ToString());
                    var startyear = searchObj.StartDate.Value.Year;
                    var startmonth = searchObj.StartDate.Value.Month;
                    var startday = searchObj.StartDate.Value.Day;
                    if (startday < 10)
                        setstartday = searchObj.StartDate.Value.Day.ToString().PadLeft(2, '0');
                    else
                        setstartday = searchObj.StartDate.Value.Day.ToString();

                    if (startmonth < 10)
                        setstartmonth = searchObj.StartDate.Value.Month.ToString().PadLeft(2, '0');
                    else
                        setstartmonth = searchObj.StartDate.Value.Month.ToString();

                    var sDate = startyear + "/" + setstartmonth + "/" + setstartday;
                    startingFrom = DateTime.Parse(sDate);
                }

                if (searchObj.End == "")
                {
                }
                else
                {
                    searchObj.EndDate = DateTime.Parse(searchObj.End.ToString());
                    var endyear = searchObj.EndDate.Value.Year;
                    var endmonth = searchObj.EndDate.Value.Month;
                    var endday = searchObj.EndDate.Value.Day;
                    if (endday < 10)
                        setendday = searchObj.EndDate.Value.Day.ToString().PadLeft(2, '0');
                    else
                        setendday = searchObj.EndDate.Value.Day.ToString();
                    if (endmonth < 10)
                        setendmonth = searchObj.EndDate.Value.Month.ToString().PadLeft(2, '0');
                    else
                        setendmonth = searchObj.EndDate.Value.Month.ToString();
                    var eDate = endyear + "/" + setendmonth + "/" + setendday;
                    endingTo = DateTime.Parse(eDate);
                }
                if (searchObj.Start != "" && searchObj.End != "")
                {
                    lstExternalFixes = lstExternalFixes.Where(a => a.OutDate.Value.Date >= startingFrom.Date && a.OutDate.Value.Date <= endingTo.Date).ToList();
                }








                string setcomingstartday, setcomingstartmonth, setcomingendday, setcomingendmonth = "";
                DateTime comingstartingFrom = new DateTime();
                DateTime comingendingTo = new DateTime();
                if (searchObj.CommingStart == "")
                {

                }
                else
                {
                    searchObj.CommingStartDate = DateTime.Parse(searchObj.CommingStart.ToString());
                    var comingStartyear = searchObj.CommingStartDate.Value.Year;
                    var comingStartmonth = searchObj.CommingStartDate.Value.Month;
                    var comingStartday = searchObj.CommingStartDate.Value.Day;
                    if (comingStartday < 10)
                        setcomingstartday = searchObj.CommingStartDate.Value.Day.ToString().PadLeft(2, '0');
                    else
                        setcomingstartday = searchObj.CommingStartDate.Value.Day.ToString();

                    if (comingStartmonth < 10)
                        setcomingstartmonth = searchObj.CommingStartDate.Value.Month.ToString().PadLeft(2, '0');
                    else
                        setcomingstartmonth = searchObj.CommingStartDate.Value.Month.ToString();

                    var sDate = comingStartyear + "/" + setcomingstartmonth + "/" + setcomingstartday;
                    comingstartingFrom = DateTime.Parse(sDate);
                }
                if (searchObj.CommingEnd == "")
                {
                }
                else
                {
                    searchObj.CommingEndDate = DateTime.Parse(searchObj.CommingEnd.ToString());
                    var comingEndyear = searchObj.CommingEndDate.Value.Year;
                    var comingEndmonth = searchObj.CommingEndDate.Value.Month;
                    var comingEndday = searchObj.CommingEndDate.Value.Day;
                    if (comingEndday < 10)
                        setcomingendday = searchObj.CommingEndDate.Value.Day.ToString().PadLeft(2, '0');
                    else
                        setcomingendday = searchObj.CommingEndDate.Value.Day.ToString();
                    if (comingEndmonth < 10)
                        setcomingendmonth = searchObj.CommingEndDate.Value.Month.ToString().PadLeft(2, '0');
                    else
                        setcomingendmonth = searchObj.CommingEndDate.Value.Month.ToString();
                    var eDate = comingEndyear + "/" + setcomingendmonth + "/" + setcomingendday;
                    comingendingTo = DateTime.Parse(eDate);
                }
                if (searchObj.CommingStart != "" && searchObj.CommingEnd != "")
                {
                    lstExternalFixes = lstExternalFixes.Where(a => a.ComingDate != null).ToList();
                    lstExternalFixes = lstExternalFixes.Where(a => a.ComingDate.Value.Date >= comingstartingFrom.Date && a.ComingDate.Value.Date <= comingendingTo.Date).ToList();
                }







                string setexpectedstartday, setexpectedstartmonth, setexpectedendday, setexpectedendmonth = "";
                DateTime expectedstartingFrom = new DateTime();
                DateTime expectedendingTo = new DateTime();
                if (searchObj.ExpectedStart == "")
                {

                }
                else
                {
                    searchObj.ExpectedStartDate = DateTime.Parse(searchObj.ExpectedStart.ToString());
                    var expectedStartyear = searchObj.ExpectedStartDate.Value.Year;
                    var expectedStartmonth = searchObj.ExpectedStartDate.Value.Month;
                    var expectedStartday = searchObj.ExpectedStartDate.Value.Day;
                    if (expectedStartday < 10)
                        setexpectedstartday = searchObj.ExpectedStartDate.Value.Day.ToString().PadLeft(2, '0');
                    else
                        setexpectedstartday = searchObj.ExpectedStartDate.Value.Day.ToString();

                    if (expectedStartmonth < 10)
                        setexpectedstartmonth = searchObj.ExpectedStartDate.Value.Month.ToString().PadLeft(2, '0');
                    else
                        setexpectedstartmonth = searchObj.ExpectedStartDate.Value.Month.ToString();

                    var sDate = expectedStartyear + "/" + setexpectedstartmonth + "/" + setexpectedstartday;
                    expectedstartingFrom = DateTime.Parse(sDate);
                }

                if (searchObj.ExpectedEnd == "")
                {
                }
                else
                {
                    searchObj.ExpectedEndDate = DateTime.Parse(searchObj.ExpectedEnd.ToString());
                    var expectedEndyear = searchObj.ExpectedEndDate.Value.Year;
                    var expectedEndmonth = searchObj.ExpectedEndDate.Value.Month;
                    var expectedEndday = searchObj.ExpectedEndDate.Value.Day;
                    if (expectedEndday < 10)
                        setexpectedendday = searchObj.ExpectedEndDate.Value.Day.ToString().PadLeft(2, '0');
                    else
                        setexpectedendday = searchObj.ExpectedEndDate.Value.Day.ToString();
                    if (expectedEndmonth < 10)
                        setexpectedendmonth = searchObj.ExpectedEndDate.Value.Month.ToString().PadLeft(2, '0');
                    else
                        setexpectedendmonth = searchObj.ExpectedEndDate.Value.Month.ToString();
                    var eDate = expectedEndyear + "/" + setexpectedendmonth + "/" + setexpectedendday;
                    expectedendingTo = DateTime.Parse(eDate);
                }
                if (searchObj.ExpectedStart != "" && searchObj.ExpectedEnd != "")
                {
                    lstExternalFixes = lstExternalFixes.Where(a => a.ExpectedDate != null).ToList();
                    lstExternalFixes = lstExternalFixes.Where(a => a.ExpectedDate.Value.Date >= expectedstartingFrom.Date && a.ExpectedDate.Value.Date <= expectedendingTo.Date).ToList();
                }


            }
            mainClass.Results = lstExternalFixes.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            mainClass.Count = lstExternalFixes.Count;
            return mainClass;
        }

        public IndexExternalFixVM SortExternalFix(SortExternalFixVM sortObj, int pageNumber, int pageSize)
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
                          AssetDetailId = item.AssetDetailId,
                          ComingDate = item.ComingDate,
                          HospitalId = (int)item.HospitalId,
                          OutDate = item.OutDate,
                          ExpectedDate = item.ExpectedDate,

                          MasterAssetId = item.AssetDetail.MasterAsset != null ? item.AssetDetail.MasterAsset.Id : 0,
                          AssetName = item.AssetDetail.MasterAsset != null ? item.AssetDetail.MasterAsset.Name : "",
                          AssetNameAr = item.AssetDetail.MasterAsset != null ? item.AssetDetail.MasterAsset.NameAr : "",
                          DepartmentId = item.AssetDetail.Department != null ? int.Parse(item.AssetDetail.DepartmentId.ToString()) : 0,
                          DepartmentName = item.AssetDetail.Department != null ? item.AssetDetail.Department.Name : "",
                          DepartmentNameAr = item.AssetDetail.Department != null ? item.AssetDetail.Department.NameAr : "",
                          SupplierId = item.AssetDetail.Supplier != null ? item.AssetDetail.Supplier.Id : 0,
                          SupplierName = item.AssetDetail.Supplier != null ? item.AssetDetail.Supplier.Name : "",
                          SupplierNameAr = item.AssetDetail.Supplier != null ? item.AssetDetail.Supplier.NameAr : "",
                          BrandId = item.AssetDetail.MasterAsset.brand != null ? item.AssetDetail.MasterAsset.brand.Id : 0,
                          BrandName = item.AssetDetail.MasterAsset.brand != null ? item.AssetDetail.MasterAsset.brand.Name : "",
                          BrandNameAr = item.AssetDetail.MasterAsset.brand != null ? item.AssetDetail.MasterAsset.brand.NameAr : "",
                          ModelNumber = item.AssetDetail.MasterAsset != null ? item.AssetDetail.MasterAsset.ModelNumber : "",
                          SerialNumber = item.AssetDetail.SerialNumber,
                          Barcode = item.AssetDetail.Barcode,

                      }).ToList();



            if (sortObj.HospitalId == 0)
                lstExternalFixes = lstExternalFixes.ToList();
            else
                lstExternalFixes = lstExternalFixes.Where(a => a.HospitalId == sortObj.HospitalId).ToList();




            if (lstExternalFixes.Count > 0)
            {
                if (sortObj.AssetName != "")
                {
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.AssetName).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.AssetName).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.AssetName).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.AssetName).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.AssetName).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.AssetName).ToList();
                        }
                    }
                    if (sortObj.Serial != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SerialNumber == sortObj.Serial).OrderByDescending(d => d.AssetName).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SerialNumber == sortObj.Serial).OrderBy(d => d.AssetName).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.ModelNumber == sortObj.Model).OrderByDescending(d => d.AssetName).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.ModelNumber == sortObj.Model).OrderBy(d => d.AssetName).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstExternalFixes = lstExternalFixes.OrderByDescending(d => d.AssetName).ToList();
                        else
                            lstExternalFixes = lstExternalFixes.OrderBy(d => d.AssetName).ToList();
                    }
                }
                else if (sortObj.AssetNameAr != "")
                {
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.AssetNameAr).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.AssetNameAr).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.AssetNameAr).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.AssetNameAr).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.AssetNameAr).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.AssetNameAr).ToList();
                        }
                    }
                    if (sortObj.Serial != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SerialNumber == sortObj.Serial).OrderByDescending(d => d.AssetNameAr).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SerialNumber == sortObj.Serial).OrderBy(d => d.AssetNameAr).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.ModelNumber == sortObj.Model).OrderByDescending(d => d.AssetNameAr).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.ModelNumber == sortObj.Model).OrderBy(d => d.AssetNameAr).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstExternalFixes = lstExternalFixes.OrderByDescending(d => d.AssetNameAr).ToList();
                        else
                            lstExternalFixes = lstExternalFixes.OrderBy(d => d.AssetNameAr).ToList();
                    }

                }
                else if (sortObj.BarCode != "")
                {

                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.Barcode).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.Barcode).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.Barcode).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.Barcode).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.Barcode).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.Barcode).ToList();
                        }
                    }
                    if (sortObj.Serial != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SerialNumber == sortObj.Serial).OrderByDescending(d => d.Barcode).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SerialNumber == sortObj.Serial).OrderBy(d => d.Barcode).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.ModelNumber == sortObj.Model).OrderByDescending(d => d.Barcode).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.ModelNumber == sortObj.Model).OrderBy(d => d.Barcode).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstExternalFixes = lstExternalFixes.OrderByDescending(d => d.Barcode).ToList();
                        else
                            lstExternalFixes = lstExternalFixes.OrderBy(d => d.Barcode).ToList();
                    }
                }
                else if (sortObj.Serial != "")
                {
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    if (sortObj.Serial != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SerialNumber == sortObj.Serial).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SerialNumber == sortObj.Serial).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.ModelNumber == sortObj.Model).OrderByDescending(d => d.SerialNumber).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.ModelNumber == sortObj.Model).OrderBy(d => d.SerialNumber).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstExternalFixes = lstExternalFixes.OrderByDescending(d => d.SerialNumber).ToList();
                        else
                            lstExternalFixes = lstExternalFixes.OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                else if (sortObj.OutDate != "")
                {
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.OutDate).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.OutDate).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.OutDate).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.OutDate).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.OutDate).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.OutDate).ToList();
                        }
                    }
                    if (sortObj.Serial != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SerialNumber == sortObj.Serial).OrderByDescending(d => d.OutDate).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SerialNumber == sortObj.Serial).OrderBy(d => d.OutDate).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.ModelNumber == sortObj.Model).OrderByDescending(d => d.OutDate).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.ModelNumber == sortObj.Model).OrderBy(d => d.OutDate).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstExternalFixes = lstExternalFixes.OrderByDescending(d => d.OutDate).ToList();
                        else
                            lstExternalFixes = lstExternalFixes.OrderBy(d => d.OutDate).ToList();
                    }
                }
                else if (sortObj.Model != "" && sortObj.Model != null)
                {

                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.ModelNumber).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.ModelNumber).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.ModelNumber).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.ModelNumber).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.ModelNumber).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.ModelNumber).ToList();
                        }
                    }
                    if (sortObj.Serial != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SerialNumber == sortObj.Serial).OrderByDescending(d => d.ModelNumber).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SerialNumber == sortObj.Serial).OrderBy(d => d.ModelNumber).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.ModelNumber == sortObj.Model).OrderByDescending(d => d.ModelNumber).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.ModelNumber == sortObj.Model).OrderBy(d => d.ModelNumber).ToList();
                        }
                    }
                    else
                    {

                        if (sortObj.SortStatus == "descending")
                            lstExternalFixes = lstExternalFixes.OrderByDescending(d => d.ModelNumber).ToList();
                        else
                            lstExternalFixes = lstExternalFixes.OrderBy(d => d.ModelNumber).ToList();

                    }

                }
                else if (sortObj.BrandName != "" && sortObj.BrandName != null)
                {

                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.BrandName).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.BrandName).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.BrandName).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.BrandName).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.BrandName).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.BrandName).ToList();
                        }
                    }
                    if (sortObj.Serial != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SerialNumber == sortObj.Serial).OrderByDescending(d => d.BrandName).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SerialNumber == sortObj.Serial).OrderBy(d => d.BrandName).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.ModelNumber == sortObj.Model).OrderByDescending(d => d.BrandName).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.ModelNumber == sortObj.Model).OrderBy(d => d.BrandName).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstExternalFixes = lstExternalFixes.OrderByDescending(d => d.BrandName).ToList();
                        else
                            lstExternalFixes = lstExternalFixes.OrderBy(d => d.BrandName).ToList();
                    }

                }
                else if (sortObj.BrandNameAr != "" && sortObj.BrandNameAr != null)
                {

                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.BrandNameAr).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.BrandNameAr).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.BrandNameAr).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.BrandNameAr).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.BrandNameAr).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.BrandNameAr).ToList();
                        }
                    }
                    if (sortObj.Serial != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SerialNumber == sortObj.Serial).OrderByDescending(d => d.BrandNameAr).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SerialNumber == sortObj.Serial).OrderBy(d => d.BrandNameAr).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.ModelNumber == sortObj.Model).OrderByDescending(d => d.BrandNameAr).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.ModelNumber == sortObj.Model).OrderBy(d => d.BrandNameAr).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstExternalFixes = lstExternalFixes.OrderByDescending(d => d.BrandNameAr).ToList();
                        else
                            lstExternalFixes = lstExternalFixes.OrderBy(d => d.BrandNameAr).ToList();
                    }

                }
                else if (sortObj.SupplierName != "" && sortObj.SupplierName != null)
                {



                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.SupplierName).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.SupplierName).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.SupplierName).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.SupplierName).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.SupplierName).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.SupplierName).ToList();
                        }
                    }
                    if (sortObj.Serial != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SerialNumber == sortObj.Serial).OrderByDescending(d => d.SupplierName).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SerialNumber == sortObj.Serial).OrderBy(d => d.SupplierName).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.ModelNumber == sortObj.Model).OrderByDescending(d => d.SupplierName).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.ModelNumber == sortObj.Model).OrderBy(d => d.SupplierName).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstExternalFixes = lstExternalFixes.OrderByDescending(d => d.SupplierName).ToList();
                        else
                            lstExternalFixes = lstExternalFixes.OrderBy(d => d.SupplierName).ToList();
                    }

                }
                else if (sortObj.SupplierNameAr != "" && sortObj.SupplierNameAr != null)
                {
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.SupplierNameAr).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.SupplierNameAr).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.SupplierNameAr).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.SupplierNameAr).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.SupplierNameAr).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.SupplierNameAr).ToList();
                        }
                    }
                    if (sortObj.Serial != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SerialNumber == sortObj.Serial).OrderByDescending(d => d.SupplierNameAr).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SerialNumber == sortObj.Serial).OrderBy(d => d.SupplierNameAr).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.ModelNumber == sortObj.Model).OrderByDescending(d => d.SupplierNameAr).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.ModelNumber == sortObj.Model).OrderBy(d => d.SupplierNameAr).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstExternalFixes = lstExternalFixes.OrderByDescending(d => d.SupplierNameAr).ToList();
                        else
                            lstExternalFixes = lstExternalFixes.OrderBy(d => d.SupplierNameAr).ToList();
                    }
                }












                else if (sortObj.DepartmentName != "" && sortObj.DepartmentName != null)
                {



                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.DepartmentName).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.DepartmentName).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.DepartmentName).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.DepartmentName).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.DepartmentName).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.DepartmentName).ToList();
                        }
                    }
                    if (sortObj.Serial != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SerialNumber == sortObj.Serial).OrderByDescending(d => d.DepartmentName).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SerialNumber == sortObj.Serial).OrderBy(d => d.DepartmentName).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.ModelNumber == sortObj.Model).OrderByDescending(d => d.DepartmentName).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.ModelNumber == sortObj.Model).OrderBy(d => d.DepartmentName).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstExternalFixes = lstExternalFixes.OrderByDescending(d => d.SupplierName).ToList();
                        else
                            lstExternalFixes = lstExternalFixes.OrderBy(d => d.SupplierName).ToList();
                    }

                }
                else if (sortObj.DepartmentNameAr != "" && sortObj.DepartmentNameAr != null)
                {
                    if (sortObj.BrandId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.BrandId == sortObj.BrandId).OrderByDescending(d => d.DepartmentNameAr).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.BrandId == sortObj.BrandId).OrderBy(d => d.DepartmentNameAr).ToList();
                        }
                    }
                    if (sortObj.SupplierId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SupplierId == sortObj.SupplierId).OrderByDescending(d => d.DepartmentNameAr).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SupplierId == sortObj.SupplierId).OrderBy(d => d.DepartmentNameAr).ToList();
                        }
                    }
                    if (sortObj.MasterAssetId != 0)
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderByDescending(d => d.DepartmentNameAr).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.MasterAssetId == sortObj.MasterAssetId).OrderBy(d => d.DepartmentNameAr).ToList();
                        }
                    }
                    if (sortObj.Serial != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SerialNumber == sortObj.Serial).OrderByDescending(d => d.DepartmentNameAr).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.SerialNumber == sortObj.Serial).OrderBy(d => d.DepartmentNameAr).ToList();
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.ModelNumber == sortObj.Model).OrderByDescending(d => d.DepartmentNameAr).ToList();
                        }
                        else
                        {
                            lstExternalFixes = lstExternalFixes.Where(b => b.ModelNumber == sortObj.Model).OrderBy(d => d.DepartmentNameAr).ToList();
                        }
                    }
                    else
                    {
                        if (sortObj.SortStatus == "descending")
                            lstExternalFixes = lstExternalFixes.OrderByDescending(d => d.SupplierNameAr).ToList();
                        else
                            lstExternalFixes = lstExternalFixes.OrderBy(d => d.SupplierNameAr).ToList();
                    }
                }
            }
            mainClass.Results = lstExternalFixes.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            mainClass.Count = lstExternalFixes.Count;
            return mainClass;
        }
    }
}


