using Asset.Core.Helpers;
using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.OriginVM;
using Asset.ViewModels.PMAssetTaskVM;
using Asset.ViewModels.WNPMAssetTimes;
using Itenso.TimePeriod;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class WNPMAssetTimeRepositories : IWNPMAssetTimeRepository
    {

        private ApplicationDbContext _context;


        public WNPMAssetTimeRepositories(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Add(WNPMAssetTime model)
        {
            WNPMAssetTime timeObj = new WNPMAssetTime();
            try
            {
                if (model != null)
                {
                    timeObj.AssetDetailId = model.AssetDetailId;
                    timeObj.DueDate = model.DueDate;
                    timeObj.IsDone = model.IsDone;
                    timeObj.DoneDate = model.DoneDate;
                    timeObj.SysDoneDate = DateTime.Today.Date;
                    timeObj.Comment = model.Comment;
                    timeObj.HospitalId = model.HospitalId;
                    timeObj.PMDate = model.PMDate;
                    _context.WNPMAssetTimes.Add(timeObj);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return timeObj.Id;
        }

        public int Delete(int id)
        {
            var timeObj = _context.WNPMAssetTimes.Find(id);
            try
            {
                if (timeObj != null)
                {
                    _context.WNPMAssetTimes.Remove(timeObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return 0;
        }

        //public IndexWNPMAssetTimesVM GetAll(FilterAssetTimeVM filterObj, int pageNumber, int pageSize, string userId)
        //{
        //    IndexWNPMAssetTimesVM mainClass = new IndexWNPMAssetTimesVM();
        //    List<IndexWNPMAssetTimesVM.GetData> list = new List<IndexWNPMAssetTimesVM.GetData>();
        //    var allAssetDetails = _context.WNPMAssetTimes
        //        .Include(a => a.AssetDetail).Include(a => a.AssetDetail.MasterAsset)
        //         .Include(a => a.AssetDetail.Department)
        //         .Include(a => a.Supplier)
        //        .Include(a => a.AssetDetail.Hospital).ToList();
        //    var allAssetDetailsByQuarter = allAssetDetails.GroupBy(item => (Math.Ceiling(decimal.Parse(item.PMDate.Value.Month.ToString()) / 3)));
        //    if (allAssetDetailsByQuarter.ToList().Count > 0)
        //    {
        //        foreach (var itm2 in allAssetDetailsByQuarter)
        //        {
        //            if (filterObj.yearQuarter == itm2.Key)
        //            {
        //                mainClass.YearQuarter = int.Parse(itm2.Key.ToString());
        //                foreach (var itm in itm2)
        //                {
        //                    IndexWNPMAssetTimesVM.GetData item = new IndexWNPMAssetTimesVM.GetData();
        //                    item.Id = itm.Id;
        //                    item.BarCode = itm.AssetDetail.Barcode;
        //                    item.ModelNumber = itm.AssetDetail.MasterAsset.ModelNumber;
        //                    item.SerialNumber = itm.AssetDetail.SerialNumber;
        //                    item.DepartmentId = itm.AssetDetail.Department != null ? itm.AssetDetail.DepartmentId : 0;
        //                    item.DepartmentName = itm.AssetDetail.Department != null ? itm.AssetDetail.Department.Name : "";
        //                    item.DepartmentNameAr = itm.AssetDetail.Department != null ? itm.AssetDetail.Department.NameAr : "";
        //                    if (itm.Supplier != null)
        //                    {
        //                        item.SupplierId = itm.AgencyId;
        //                        item.SupplierName = itm.Supplier.Name;
        //                        item.SupplierNameAr = itm.Supplier.NameAr;
        //                    }
        //                    item.PMDate = itm.PMDate;
        //                    item.IsDone = itm.IsDone != null ? (bool)itm.IsDone : false;
        //                    item.DoneDate = itm.DoneDate;
        //                    item.DueDate = itm.DueDate;
        //                    item.AssetName = itm.AssetDetail.MasterAssetId > 0 ? itm.AssetDetail.MasterAsset.Name : "";
        //                    item.AssetNameAr = itm.AssetDetail.MasterAssetId > 0 ? itm.AssetDetail.MasterAsset.NameAr : "";
        //                    list.Add(item);
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        list = list.ToList();
        //    }
        //    mainClass.Count = list.Count();
        //    mainClass.CountDone = list.Where(a => a.IsDone == true).ToList().Count;
        //    mainClass.CountNotDone = list.Where(a => a.IsDone == false).ToList().Count;

        //    if (filterObj.IsDone == true)
        //    {
        //        list = list.Where(a => a.IsDone == true).ToList();
        //    }
        //    else if (filterObj.IsDone == false)
        //    {
        //        list = list.Where(a => a.IsDone == false).ToList();
        //    }
        //    else
        //        list = list.ToList();
        //    //mainClass.CountDone = list.Where(a => a.IsDone == true).ToList().Count;
        //    //mainClass.CountNotDone = list.Where(a => a.IsDone == false).ToList().Count;

        //    var assetTimeObjuestsPerPage = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        //    mainClass.Results = assetTimeObjuestsPerPage;
        //    return mainClass;
        //}

        public IndexWNPMAssetTimesVM GetAll(FilterAssetTimeVM filterObj, int pageNumber, int pageSize, string userId)
        {

            IndexWNPMAssetTimesVM mainClass = new IndexWNPMAssetTimesVM();
            List<IndexWNPMAssetTimesVM.GetData> list = new List<IndexWNPMAssetTimesVM.GetData>();
            var allAssetDetails = _context.WNPMAssetTimes
                .Include(a => a.AssetDetail).Include(a => a.AssetDetail.MasterAsset)
                 .Include(a => a.AssetDetail.Department)
                 .Include(a => a.Supplier)
                .Include(a => a.AssetDetail.Hospital).ToList();

            //.GroupBy(item => (Math.Ceiling(decimal.Parse(item.PMDate.Value.AddMonths(-3).Month.ToString())/3+2)%4+1 ));
            var allAssetDetailsByQuarter = allAssetDetails.GroupBy(item => (Math.Ceiling(decimal.Parse(item.PMDate.Value.AddMonths(6).Month.ToString()) / 3)));
            if (allAssetDetailsByQuarter.ToList().Count > 0)
            {
                foreach (var itm2 in allAssetDetailsByQuarter)
                {
                    if (filterObj.yearQuarter == itm2.Key)
                    {
                        mainClass.YearQuarter = int.Parse(itm2.Key.ToString());
                        foreach (var itm in itm2)
                        {

                            IndexWNPMAssetTimesVM.GetData item = new IndexWNPMAssetTimesVM.GetData();
                            item.Id = itm.Id;
                            if (itm.AssetDetail != null)
                            {

                                item.BarCode = itm.AssetDetail.Barcode;

                                if (itm.AssetDetail.MasterAsset != null)
                                {
                                    item.ModelNumber = itm.AssetDetail.MasterAsset.ModelNumber;

                                }
                                item.SerialNumber = itm.AssetDetail.SerialNumber;
                                item.DepartmentId = itm.AssetDetail.Department != null ? itm.AssetDetail.DepartmentId : 0;
                                item.DepartmentName = itm.AssetDetail.Department != null ? itm.AssetDetail.Department.Name : "";
                                item.DepartmentNameAr = itm.AssetDetail.Department != null ? itm.AssetDetail.Department.NameAr : "";

                            }


                            if (itm.Supplier != null)
                            {
                                item.SupplierId = itm.AgencyId;
                                item.SupplierName = itm.Supplier.Name;
                                item.SupplierNameAr = itm.Supplier.NameAr;
                            }
                            item.PMDate = itm.PMDate;
                            item.IsDone = itm.IsDone != null ? (bool)itm.IsDone : false;
                            item.DoneDate = itm.DoneDate;
                            item.DueDate = itm.DueDate;
                            if (itm.AssetDetail != null)
                            {
                                if (itm.AssetDetail.MasterAsset != null)
                                {
                                    item.AssetName = itm.AssetDetail.MasterAssetId > 0 ? itm.AssetDetail.MasterAsset.Name : "";
                                    item.AssetNameAr = itm.AssetDetail.MasterAssetId > 0 ? itm.AssetDetail.MasterAsset.NameAr : "";
                                }

                            }

                            list.Add(item);
                        }
                    }
                }
            }
            else
            {
                list = list.ToList();
            }
            mainClass.Count = list.Count();
            mainClass.CountDone = list.Where(a => a.IsDone == true).ToList().Count;
            mainClass.CountNotDone = list.Where(a => a.IsDone == false).ToList().Count;

            if (filterObj.IsDone == true)
            {
                list = list.Where(a => a.IsDone == true).ToList();
            }
            else if (filterObj.IsDone == false)
            {
                list = list.Where(a => a.IsDone == false).ToList();
            }
            else
                list = list.ToList();

            var assetTimeObjuestsPerPage = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            mainClass.Results = assetTimeObjuestsPerPage;
            return mainClass;
        }




















        public List<CalendarWNPMAssetTimeVM> GetAll(int hospitalId, string userId)
        {
            List<CalendarWNPMAssetTimeVM> list = new List<CalendarWNPMAssetTimeVM>();
            var lstAssetTimes = _context.WNPMAssetTimes
               .Include(p => p.AssetDetail)
               .Include(p => p.Supplier)
               .Include(r => r.AssetDetail.Hospital)
               .Include(r => r.AssetDetail.Department)
               .Include(r => r.AssetDetail.MasterAsset)
               .Include(r => r.AssetDetail.MasterAsset.brand).Where(a => a.HospitalId == hospitalId).ToList();

            if (lstAssetTimes.Count > 0)
            {
                foreach (var item in lstAssetTimes)
                {

                    string month = "";
                    string day = "";
                    string endmonth = "";
                    string endday = "";
                    //string duemonth = "";
                    //string dueday = "";
                    //string endduemonth = "";
                    //string enddueday = "";

                    CalendarWNPMAssetTimeVM viewWNPMAssetTimeObj = new CalendarWNPMAssetTimeVM();
                    //if (item.AssetDetail.Barcode == "200700889")
                    //{
                    //    viewWNPMAssetTimeObj.titleAr = item.AssetDetail.MasterAsset.NameAr.Trim();
                    //}

                    viewWNPMAssetTimeObj.Id = item.Id;
                    viewWNPMAssetTimeObj.PMDate = item.PMDate;

                    if (item.PMDate.Value.Month < 10)
                        month = item.PMDate.Value.Month.ToString().PadLeft(2, '0');
                    else
                        month = item.PMDate.Value.Month.ToString();

                    if (item.PMDate.Value.Month < 10)
                        endmonth = item.PMDate.Value.Month.ToString().PadLeft(2, '0');
                    else
                        endmonth = item.PMDate.Value.Month.ToString();

                    if (item.PMDate.Value.Day < 10)
                        day = item.PMDate.Value.Day.ToString().PadLeft(2, '0');
                    else
                        day = item.PMDate.Value.Day.ToString();

                    if (item.PMDate.Value.Day < 10)
                        endday = (item.PMDate.Value.Day).ToString().PadLeft(2, '0');
                    else
                        endday = item.PMDate.Value.Day.ToString();


                    //viewWNPMAssetTimeObj.title = item.AssetDetail.MasterAsset.Name.Trim();
                    //viewWNPMAssetTimeObj.titleAr = item.AssetDetail.MasterAsset.NameAr.Trim();
                    viewWNPMAssetTimeObj.start = (item.PMDate.Value.Year + "-" + month + "-" + day).Trim();
                    viewWNPMAssetTimeObj.end = (item.PMDate.Value.Year + "-" + endmonth + "-" + endday).Trim();

                    if (item.IsDone == true)
                    {
                        viewWNPMAssetTimeObj.color = "#79be47";
                        viewWNPMAssetTimeObj.textColor = "#fff";
                    }
                    else if (item.PMDate < DateTime.Today.Date && item.IsDone == false)
                    {
                        viewWNPMAssetTimeObj.color = "#ff7578";
                        viewWNPMAssetTimeObj.textColor = "#fff";
                    }

                    viewWNPMAssetTimeObj.allDay = true;
                    viewWNPMAssetTimeObj.DoneDate = item.DoneDate;
                    viewWNPMAssetTimeObj.IsDone = item.IsDone;
                    viewWNPMAssetTimeObj.Comment = item.Comment;
                    viewWNPMAssetTimeObj.HospitalId = int.Parse(item.HospitalId.ToString());
                    viewWNPMAssetTimeObj.AssetDetailId = int.Parse(item.AssetDetailId.ToString());
                    viewWNPMAssetTimeObj.BarCode = item.AssetDetail.Barcode;
                    viewWNPMAssetTimeObj.ModelNumber = item.AssetDetail.MasterAsset.ModelNumber;
                    viewWNPMAssetTimeObj.SerialNumber = item.AssetDetail.SerialNumber;
                    viewWNPMAssetTimeObj.MasterAssetId = (int)item.AssetDetail.MasterAssetId;

                    if (item.AssetDetail.MasterAsset != null)
                    {
                        if (item.AssetDetail.MasterAsset.Name.Contains('/'))
                        {
                            item.AssetDetail.MasterAsset.Name.Replace("/", "\\/");
                            viewWNPMAssetTimeObj.AssetName = item.AssetDetail.MasterAsset.Name.Trim().Replace("/", "\\/");// item.AssetDetail.MasterAsset.Name;
                            viewWNPMAssetTimeObj.AssetNameAr = item.AssetDetail.MasterAsset.NameAr.Trim().Replace("/", "\\/");// item.AssetDetail.MasterAsset.NameAr;


                            if (item.AssetDetail.Department != null)
                            {
                                viewWNPMAssetTimeObj.title = viewWNPMAssetTimeObj.AssetName + "-" + item.AssetDetail.Department.Name;
                                viewWNPMAssetTimeObj.titleAr = viewWNPMAssetTimeObj.AssetNameAr + "-" + item.AssetDetail.Department.NameAr;
                            }
                            else
                            {
                                viewWNPMAssetTimeObj.title = viewWNPMAssetTimeObj.AssetName;
                                viewWNPMAssetTimeObj.titleAr = viewWNPMAssetTimeObj.AssetNameAr;
                            }
                        }
                        else
                        {
                            viewWNPMAssetTimeObj.AssetName = item.AssetDetail.MasterAsset.Name;
                            viewWNPMAssetTimeObj.AssetNameAr = item.AssetDetail.MasterAsset.NameAr;

                            if (item.AssetDetail.Department != null)
                            {
                                viewWNPMAssetTimeObj.title = viewWNPMAssetTimeObj.AssetName + "-" + item.AssetDetail.Department.Name;
                                viewWNPMAssetTimeObj.titleAr = viewWNPMAssetTimeObj.AssetNameAr + "-" + item.AssetDetail.Department.NameAr;
                            }
                            else
                            {
                                viewWNPMAssetTimeObj.title = viewWNPMAssetTimeObj.AssetName;
                                viewWNPMAssetTimeObj.titleAr = viewWNPMAssetTimeObj.AssetNameAr;
                            }

                        }


                        if (item.AssetDetail.MasterAsset.Name.Contains('\n'))
                        {
                            viewWNPMAssetTimeObj.AssetName = item.AssetDetail.MasterAsset.Name.Trim().Replace("\n", "");// item.AssetDetail.MasterAsset.Name;
                            viewWNPMAssetTimeObj.AssetNameAr = item.AssetDetail.MasterAsset.NameAr.Trim().Replace("\n", "");// item.AssetDetail.MasterAsset.NameAr;


                            if (item.AssetDetail.Department != null)
                            {
                                viewWNPMAssetTimeObj.title = viewWNPMAssetTimeObj.AssetName + "-" + item.AssetDetail.Department.Name;
                                viewWNPMAssetTimeObj.titleAr = viewWNPMAssetTimeObj.AssetNameAr + "-" + item.AssetDetail.Department.NameAr;
                            }
                            else
                            {

                                viewWNPMAssetTimeObj.title = viewWNPMAssetTimeObj.AssetName;
                                viewWNPMAssetTimeObj.titleAr = viewWNPMAssetTimeObj.AssetNameAr;
                            }


                        }

                    }
                    viewWNPMAssetTimeObj.AssetDetailId = item.AssetDetailId != null ? (int)item.AssetDetailId : 0;
                    viewWNPMAssetTimeObj.DepartmentName = item.AssetDetail.Department != null ? item.AssetDetail.Department.Name : "";
                    viewWNPMAssetTimeObj.DepartmentNameAr = item.AssetDetail.Department != null ? item.AssetDetail.Department.NameAr : "";







                    List<IndexPMAssetTaskVM.GetData> lstTasks = new List<IndexPMAssetTaskVM.GetData>();
                    var lstAssetTasks = _context.PMAssetTasks.Where(a => a.MasterAssetId == item.AssetDetail.MasterAssetId).ToList();
                    if (lstAssetTasks.Count > 0)
                    {
                        foreach (var tsk in lstAssetTasks)
                        {
                            IndexPMAssetTaskVM.GetData getDataObj = new IndexPMAssetTaskVM.GetData();
                            getDataObj.TaskName = tsk.Name;
                            getDataObj.TaskNameAr = tsk.NameAr;
                            getDataObj.MasterAssetId = (int)tsk.MasterAssetId;
                            lstTasks.Add(getDataObj);
                        }
                        viewWNPMAssetTimeObj.ListMasterAssetTasks = lstTasks;
                    }
                    list.Add(viewWNPMAssetTimeObj);
                }

            }
            return list;
        }

        public IndexWNPMAssetTimesVM GetAllAssetTimesIsDone(bool? isDone, int pageNumber, int pageSize, string userId)
        {

            IndexWNPMAssetTimesVM mainClass = new IndexWNPMAssetTimesVM();
            List<IndexWNPMAssetTimesVM.GetData> list = new List<IndexWNPMAssetTimesVM.GetData>();
            var allAssetDetails = _context.WNPMAssetTimes
                .Include(a => a.AssetDetail).Include(a => a.AssetDetail.MasterAsset)
                 .Include(a => a.AssetDetail.Department)
                .Include(a => a.AssetDetail.Hospital).ToList();

            if (allAssetDetails.Count > 0)
            {
                foreach (var itm in allAssetDetails)
                {
                    IndexWNPMAssetTimesVM.GetData item = new IndexWNPMAssetTimesVM.GetData();
                    item.Id = itm.Id;
                    item.PMDate = itm.PMDate;
                    item.DoneDate = itm.DoneDate;
                    item.DueDate = itm.DueDate;
                    item.IsDone = (bool)itm.IsDone;
                    item.BarCode = itm.AssetDetail.Barcode;
                    item.ModelNumber = itm.AssetDetail.MasterAsset.ModelNumber;
                    item.SerialNumber = itm.AssetDetail.SerialNumber;
                    item.DepartmentId = itm.AssetDetail.DepartmentId;
                    item.DepartmentName = itm.AssetDetail.Department.Name;
                    item.DepartmentNameAr = itm.AssetDetail.Department.NameAr;
                    item.PMDate = itm.PMDate;
                    item.IsDone = itm.IsDone != null ? (bool)itm.IsDone : false;
                    item.DoneDate = itm.DoneDate;
                    item.DueDate = itm.DueDate;
                    item.AssetName = itm.AssetDetail.MasterAssetId > 0 ? itm.AssetDetail.MasterAsset.Name : "";
                    item.AssetNameAr = itm.AssetDetail.MasterAssetId > 0 ? itm.AssetDetail.MasterAsset.NameAr : "";

                    list.Add(item);
                }
            }

            if (isDone == true)
                list = list.Where(a => a.IsDone == true).ToList();

            else if (isDone == false)
                list = list.Where(a => a.IsDone == false).ToList();
            else
                list = list.ToList();

            var assetTimeObjuestsPerPage = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            mainClass.Results = assetTimeObjuestsPerPage;
            mainClass.Count = list.Count();

            mainClass.CountDone = list.Where(a => a.IsDone == true).ToList().Count;
            mainClass.CountNotDone = list.Where(a => a.IsDone == false).ToList().Count;
            return mainClass;
        }

        public ViewWNPMAssetTimeVM GetAssetTimeById(int id)
        {
            ViewWNPMAssetTimeVM viewWNPMAssetTimeObj = new ViewWNPMAssetTimeVM();
            var lstAssetTimes = _context.WNPMAssetTimes
               .Include(p => p.AssetDetail)
               .Include(p => p.Supplier)
               .Include(r => r.AssetDetail.Hospital)
                .Include(r => r.AssetDetail.Department)
               .Include(r => r.AssetDetail.MasterAsset)
                  .Include(r => r.AssetDetail.MasterAsset.brand)
           .Where(e => e.Id == id).ToList();


            if (lstAssetTimes.Count > 0)
            {
                WNPMAssetTime assetTimeObj = lstAssetTimes[0];
                viewWNPMAssetTimeObj.Id = assetTimeObj.Id;
                viewWNPMAssetTimeObj.PMDate = assetTimeObj.PMDate;
                viewWNPMAssetTimeObj.DoneDate = assetTimeObj.DoneDate;
                viewWNPMAssetTimeObj.IsDone = assetTimeObj.IsDone;
                viewWNPMAssetTimeObj.Comment = assetTimeObj.Comment;
                viewWNPMAssetTimeObj.HospitalId = int.Parse(assetTimeObj.HospitalId.ToString());
                viewWNPMAssetTimeObj.AssetDetailId = int.Parse(assetTimeObj.AssetDetailId.ToString());
                viewWNPMAssetTimeObj.BarCode = assetTimeObj.AssetDetail.Barcode;
                viewWNPMAssetTimeObj.ModelNumber = assetTimeObj.AssetDetail.MasterAsset.ModelNumber;
                viewWNPMAssetTimeObj.SerialNumber = assetTimeObj.AssetDetail.SerialNumber;
                viewWNPMAssetTimeObj.MasterAssetId = (int)assetTimeObj.AssetDetail.MasterAssetId;
                viewWNPMAssetTimeObj.AssetName = assetTimeObj.AssetDetail.MasterAsset.Name;
                viewWNPMAssetTimeObj.AssetNameAr = assetTimeObj.AssetDetail.MasterAsset.NameAr;
                viewWNPMAssetTimeObj.AssetDetailId = assetTimeObj.AssetDetailId != null ? (int)assetTimeObj.AssetDetailId : 0;
                viewWNPMAssetTimeObj.DepartmentName = assetTimeObj.AssetDetail.Department != null ? assetTimeObj.AssetDetail.Department.Name : "";
                viewWNPMAssetTimeObj.DepartmentNameAr = assetTimeObj.AssetDetail.Department != null ? assetTimeObj.AssetDetail.Department.NameAr : "";
                viewWNPMAssetTimeObj.SupplierName = assetTimeObj.Supplier != null ? assetTimeObj.Supplier.Name : "";
                viewWNPMAssetTimeObj.SupplierNameAr = assetTimeObj.Supplier != null ? assetTimeObj.Supplier.NameAr : "";
                viewWNPMAssetTimeObj.HospitalName = assetTimeObj.AssetDetail.Hospital != null ? assetTimeObj.AssetDetail.Hospital.Name : "";
                viewWNPMAssetTimeObj.HospitalNameAr = assetTimeObj.AssetDetail.Hospital != null ? assetTimeObj.AssetDetail.Hospital.NameAr : "";
                viewWNPMAssetTimeObj.BrandName = assetTimeObj.AssetDetail.MasterAsset.brand != null ? assetTimeObj.AssetDetail.MasterAsset.brand.Name : "";
                viewWNPMAssetTimeObj.BrandNameAr = assetTimeObj.AssetDetail.MasterAsset.brand != null ? assetTimeObj.AssetDetail.MasterAsset.brand.NameAr : "";


                List<IndexPMAssetTaskVM.GetData> lstTasks = new List<IndexPMAssetTaskVM.GetData>();
                var lstAssetTasks = _context.PMAssetTasks.Where(a => a.MasterAssetId == assetTimeObj.AssetDetail.MasterAssetId).ToList();
                if (lstAssetTasks.Count > 0)
                {
                    foreach (var item in lstAssetTasks)
                    {
                        IndexPMAssetTaskVM.GetData getDataObj = new IndexPMAssetTaskVM.GetData();
                        getDataObj.TaskName = item.Name;
                        getDataObj.TaskNameAr = item.NameAr;
                        getDataObj.MasterAssetId = (int)item.MasterAssetId;
                        lstTasks.Add(getDataObj);
                    }
                    viewWNPMAssetTimeObj.ListMasterAssetTasks = lstTasks;
                }
            }
            return viewWNPMAssetTimeObj;
        }

        public WNPMAssetTime GetById(int id)
        {
            return _context.WNPMAssetTimes.Find(id);
        }

        public IEnumerable<WNPMAssetTime> GetDateByAssetDetailId(int assetDetailId)
        {
            return _context.WNPMAssetTimes.Where(a => a.AssetDetailId == assetDetailId).ToList();
        }

        public IndexWNPMAssetTimesVM SearchAssetTimes(SearchAssetTimeVM searchObj, int pageNumber, int pageSize, string userId)
        {
            IndexWNPMAssetTimesVM mainClass = new IndexWNPMAssetTimesVM();
            List<IndexWNPMAssetTimesVM.GetData> list = new List<IndexWNPMAssetTimesVM.GetData>();

            ApplicationUser UserObj = new ApplicationUser();
            List<string> userRoleNames = new List<string>();
            var obj = _context.ApplicationUser.Where(a => a.Id == searchObj.UserId).ToList();
            if (obj.Count > 0)
            {
                UserObj = obj[0];
                var roleNames = (from userRole in _context.UserRoles
                                 join role in _context.Roles on userRole.RoleId equals role.Id
                                 where userRole.UserId == searchObj.UserId
                                 select role);
                foreach (var name in roleNames)
                {
                    userRoleNames.Add(name.Name);
                }
            }
            var allAssetDetails = _context.WNPMAssetTimes
                .Include(a => a.AssetDetail).Include(a => a.AssetDetail.MasterAsset)
                .Include(a => a.AssetDetail.MasterAsset.brand)
                 .Include(a => a.AssetDetail.Department)
                .Include(a => a.AssetDetail.Hospital)
                .Include(a => a.AssetDetail.Hospital.Governorate)
                   .Include(a => a.AssetDetail.Hospital.City)
                      .Include(a => a.AssetDetail.Hospital.Organization)
                         .Include(a => a.AssetDetail.Hospital.SubOrganization)
                .ToList();
            if (list.Count > 0)
            {
                if (UserObj.GovernorateId == 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
                {
                    allAssetDetails = allAssetDetails.ToList();
                }
                if (UserObj.GovernorateId > 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
                {
                    allAssetDetails = allAssetDetails.Where(t => t.AssetDetail.Hospital.GovernorateId == UserObj.GovernorateId).ToList();
                }
                if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId == 0)
                {
                    allAssetDetails = allAssetDetails.Where(t => t.AssetDetail.Hospital.GovernorateId == UserObj.GovernorateId && t.AssetDetail.Hospital.CityId == UserObj.CityId).ToList();
                }
                if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId > 0)
                {
                    allAssetDetails = allAssetDetails.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                }
                if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId == 0 && UserObj.HospitalId == 0)
                {
                    allAssetDetails = allAssetDetails.Where(t => t.AssetDetail.Hospital.OrganizationId == UserObj.OrganizationId).ToList();
                }
                if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId == 0)
                {
                    allAssetDetails = allAssetDetails.Where(t => t.AssetDetail.Hospital.OrganizationId == UserObj.OrganizationId && t.AssetDetail.Hospital.SubOrganizationId == UserObj.SubOrganizationId).ToList();
                }
                if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId > 0)
                {
                    allAssetDetails = allAssetDetails.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                }
            }


            if (allAssetDetails.Count > 0)
            {
                foreach (var itm in allAssetDetails)
                {
                    IndexWNPMAssetTimesVM.GetData item = new IndexWNPMAssetTimesVM.GetData();
                    item.Id = itm.Id;
                    item.PMDate = itm.PMDate;
                    item.DoneDate = itm.DoneDate;
                    item.DueDate = itm.DueDate;
                    item.IsDone = (bool)itm.IsDone;
                    item.BarCode = itm.AssetDetail.Barcode;
                    item.ModelNumber = itm.AssetDetail.MasterAsset.ModelNumber;
                    item.SerialNumber = itm.AssetDetail.SerialNumber;
                    item.DepartmentId = itm.AssetDetail.Department != null ? itm.AssetDetail.DepartmentId : 0;
                    item.DepartmentName = itm.AssetDetail.Department != null ? itm.AssetDetail.Department.Name : "";
                    item.DepartmentNameAr = itm.AssetDetail.Department != null ? itm.AssetDetail.Department.NameAr : "";

                    item.BrandId = itm.AssetDetail.MasterAsset.BrandId;
                    item.BrandName = itm.AssetDetail.MasterAsset.brand != null ? itm.AssetDetail.MasterAsset.brand.Name : "";
                    item.BrandNameAr = itm.AssetDetail.MasterAsset.brand != null ? itm.AssetDetail.MasterAsset.brand.NameAr : "";

                    item.PMDate = itm.PMDate;
                    item.IsDone = itm.IsDone != null ? (bool)itm.IsDone : false;
                    item.DoneDate = itm.DoneDate;
                    item.DueDate = itm.DueDate;
                    item.AssetName = itm.AssetDetail.MasterAssetId > 0 ? itm.AssetDetail.MasterAsset.Name : "";
                    item.AssetNameAr = itm.AssetDetail.MasterAssetId > 0 ? itm.AssetDetail.MasterAsset.NameAr : "";
                    list.Add(item);
                }
            }
            if (searchObj.BarCode != "")
                list = list.Where(a => a.BarCode == searchObj.BarCode).ToList();
            if (searchObj.SerialNumber != "")
                list = list.Where(a => a.SerialNumber == searchObj.SerialNumber).ToList();
            if (searchObj.ModelNumber != "")
                list = list.Where(a => a.ModelNumber == searchObj.ModelNumber).ToList();

            if (searchObj.BrandId != 0)
                list = list.Where(a => a.BrandId == searchObj.BrandId).ToList();
            else
                list = list.ToList();

            if (searchObj.DepartmentId != 0)
                list = list.Where(a => a.DepartmentId == searchObj.DepartmentId).ToList();
            else
                list = list.ToList();


            var assetTimeObjuestsPerPage = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            mainClass.Results = assetTimeObjuestsPerPage;
            mainClass.Count = list.Count();
            mainClass.CountDone = list.Where(a => a.IsDone == true).ToList().Count;
            mainClass.CountNotDone = list.Where(a => a.IsDone == false).ToList().Count;
            return mainClass;
        }

        public IndexWNPMAssetTimesVM SortAssetTimes(SortWNPMAssetTimeVM sortObj, int pageNumber, int pageSize, string userId)
        {
            IndexWNPMAssetTimesVM mainClass = new IndexWNPMAssetTimesVM();
            List<IndexWNPMAssetTimesVM.GetData> list = new List<IndexWNPMAssetTimesVM.GetData>();

            ApplicationUser UserObj = new ApplicationUser();
            List<string> userRoleNames = new List<string>();
            var obj = _context.ApplicationUser.Where(a => a.Id == sortObj.UserId).ToList();
            if (obj.Count > 0)
            {
                UserObj = obj[0];
                var roleNames = (from userRole in _context.UserRoles
                                 join role in _context.Roles on userRole.RoleId equals role.Id
                                 where userRole.UserId == sortObj.UserId
                                 select role);
                foreach (var name in roleNames)
                {
                    userRoleNames.Add(name.Name);
                }
            }
            var allAssetDetails = _context.WNPMAssetTimes
                .Include(a => a.AssetDetail).Include(a => a.AssetDetail.MasterAsset)
                .Include(a => a.AssetDetail.MasterAsset.brand)
                 .Include(a => a.AssetDetail.Department)
                .Include(a => a.AssetDetail.Hospital)
                .Include(a => a.AssetDetail.Hospital.Governorate)
                   .Include(a => a.AssetDetail.Hospital.City)
                      .Include(a => a.AssetDetail.Hospital.Organization)
                         .Include(a => a.AssetDetail.Hospital.SubOrganization)
                .ToList();
            if (list.Count > 0)
            {
                if (UserObj.GovernorateId == 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
                {
                    allAssetDetails = allAssetDetails.ToList();
                }
                if (UserObj.GovernorateId > 0 && UserObj.CityId == 0 && UserObj.HospitalId == 0)
                {
                    allAssetDetails = allAssetDetails.Where(t => t.AssetDetail.Hospital.GovernorateId == UserObj.GovernorateId).ToList();
                }
                if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId == 0)
                {
                    allAssetDetails = allAssetDetails.Where(t => t.AssetDetail.Hospital.GovernorateId == UserObj.GovernorateId && t.AssetDetail.Hospital.CityId == UserObj.CityId).ToList();
                }
                if (UserObj.GovernorateId > 0 && UserObj.CityId > 0 && UserObj.HospitalId > 0)
                {
                    allAssetDetails = allAssetDetails.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                }
                if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId == 0 && UserObj.HospitalId == 0)
                {
                    allAssetDetails = allAssetDetails.Where(t => t.AssetDetail.Hospital.OrganizationId == UserObj.OrganizationId).ToList();
                }
                if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId == 0)
                {
                    allAssetDetails = allAssetDetails.Where(t => t.AssetDetail.Hospital.OrganizationId == UserObj.OrganizationId && t.AssetDetail.Hospital.SubOrganizationId == UserObj.SubOrganizationId).ToList();
                }
                if (UserObj.OrganizationId > 0 && UserObj.SubOrganizationId > 0 && UserObj.HospitalId > 0)
                {
                    allAssetDetails = allAssetDetails.Where(t => t.AssetDetail.Hospital.Id == UserObj.HospitalId).ToList();
                }
            }


            if (allAssetDetails.Count > 0)
            {
                foreach (var itm in allAssetDetails)
                {
                    IndexWNPMAssetTimesVM.GetData item = new IndexWNPMAssetTimesVM.GetData();
                    item.Id = itm.Id;
                    item.PMDate = itm.PMDate;
                    item.DoneDate = itm.DoneDate;
                    item.DueDate = itm.DueDate;
                    item.IsDone = (bool)itm.IsDone;
                    item.BarCode = itm.AssetDetail.Barcode;
                    item.ModelNumber = itm.AssetDetail.MasterAsset.ModelNumber;
                    item.SerialNumber = itm.AssetDetail.SerialNumber;
                    item.DepartmentId = itm.AssetDetail.DepartmentId;
                    item.DepartmentName = itm.AssetDetail.Department.Name;
                    item.DepartmentNameAr = itm.AssetDetail.Department.NameAr;

                    item.BrandId = itm.AssetDetail.MasterAsset.BrandId;
                    item.BrandName = itm.AssetDetail.MasterAsset.brand.Name;
                    item.BrandNameAr = itm.AssetDetail.MasterAsset.brand.NameAr;

                    item.PMDate = itm.PMDate;
                    item.IsDone = itm.IsDone != null ? (bool)itm.IsDone : false;
                    item.DoneDate = itm.DoneDate;
                    item.DueDate = itm.DueDate;
                    item.AssetName = itm.AssetDetail.MasterAssetId > 0 ? itm.AssetDetail.MasterAsset.Name : "";
                    item.AssetNameAr = itm.AssetDetail.MasterAssetId > 0 ? itm.AssetDetail.MasterAsset.NameAr : "";
                    list.Add(item);
                }
            }

            if (sortObj.SerialNumber != "")
            {

                if (sortObj.StrBarCode != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.StrSerialNumber != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.StrSerialNumber)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.StrSerialNumber)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                if (sortObj.StrModelNumber != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.StrModelNumber)).OrderByDescending(d => d.ModelNumber).ToList();
                    else
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.StrModelNumber)).OrderBy(d => d.ModelNumber).ToList();
                }
                if (sortObj.PMDate != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.PMDate).ToList();
                    else
                        list = list.OrderBy(d => d.PMDate).ToList();
                }

                else
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.SerialNumber).ToList();
                    else
                        list = list.OrderBy(d => d.SerialNumber).ToList();
                }
            }
            if (sortObj.BarCode != "")
            {
                if (sortObj.StrBarCode != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.BarCode != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.StrSerialNumber != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.StrSerialNumber)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.StrSerialNumber)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                if (sortObj.StrModelNumber != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.StrModelNumber)).OrderByDescending(d => d.ModelNumber).ToList();
                    else
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.StrModelNumber)).OrderBy(d => d.ModelNumber).ToList();
                }
                else
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.BarCode).ToList();
                    else
                        list = list.OrderBy(d => d.BarCode).ToList();
                }
            }
            if (sortObj.ModelNumber != "")
            {
                if (sortObj.StrBarCode != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.StrSerialNumber != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.StrSerialNumber)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.StrSerialNumber)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }

                if (sortObj.BarCode != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.BarCode)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.BarCode)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.SerialNumber != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialNumber)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialNumber)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                else
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.ModelNumber).ToList();
                    else
                        list = list.OrderBy(d => d.ModelNumber).ToList();
                }
            }
            if (sortObj.AssetName != "")
            {
                if (sortObj.StrBarCode != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.SerialNumber != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialNumber)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialNumber)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                if (sortObj.ModelNumber != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.ModelNumber)).OrderByDescending(d => d.ModelNumber).ToList();
                    else
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.ModelNumber)).OrderBy(d => d.ModelNumber).ToList();
                }

                else
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.AssetName).ToList();
                    else
                        list = list.OrderBy(d => d.AssetName).ToList();
                }
            }
            if (sortObj.AssetNameAr != "")
            {
                if (sortObj.BarCode != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.SerialNumber != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialNumber)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialNumber)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                if (sortObj.ModelNumber != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.ModelNumber)).OrderByDescending(d => d.ModelNumber).ToList();
                    else
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.ModelNumber)).OrderBy(d => d.ModelNumber).ToList();
                }
                else
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.AssetNameAr).ToList();
                    else
                        list = list.OrderBy(d => d.AssetNameAr).ToList();
                }
            }
            if (sortObj.PMDate != "")
            {
                if (sortObj.BarCode != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.SerialNumber != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialNumber)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialNumber)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                if (sortObj.ModelNumber != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.ModelNumber)).OrderByDescending(d => d.ModelNumber).ToList();
                    else
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.ModelNumber)).OrderBy(d => d.ModelNumber).ToList();
                }
                else
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.PMDate).ToList();
                    else
                        list = list.OrderBy(d => d.PMDate).ToList();
                }
            }
            if (sortObj.DoneDate != "")
            {
                if (sortObj.BarCode != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.SerialNumber != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialNumber)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialNumber)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                if (sortObj.ModelNumber != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.ModelNumber)).OrderByDescending(d => d.ModelNumber).ToList();
                    else
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.ModelNumber)).OrderBy(d => d.ModelNumber).ToList();
                }
                else
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.DoneDate).ToList();
                    else
                        list = list.OrderBy(d => d.DoneDate).ToList();
                }
            }
            if (sortObj.DueDate != "")
            {
                if (sortObj.BarCode != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.SerialNumber != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialNumber)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialNumber)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                if (sortObj.ModelNumber != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.ModelNumber)).OrderByDescending(d => d.ModelNumber).ToList();
                    else
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.ModelNumber)).OrderBy(d => d.ModelNumber).ToList();
                }
                else
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.DueDate).ToList();
                    else
                        list = list.OrderBy(d => d.DueDate).ToList();
                }
            }
            if (sortObj.IsDone != "")
            {
                if (sortObj.BarCode != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderByDescending(d => d.BarCode).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.BarCode.Contains(sortObj.StrBarCode)).OrderBy(d => d.BarCode).ToList();
                    }
                }
                if (sortObj.SerialNumber != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialNumber)).OrderByDescending(d => d.SerialNumber).ToList();
                    }
                    else
                    {
                        list = list.Where(b => b.SerialNumber.Contains(sortObj.SerialNumber)).OrderBy(d => d.SerialNumber).ToList();
                    }
                }
                if (sortObj.ModelNumber != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.ModelNumber)).OrderByDescending(d => d.ModelNumber).ToList();
                    else
                        list = list.Where(b => b.ModelNumber.Contains(sortObj.ModelNumber)).OrderBy(d => d.ModelNumber).ToList();
                }
                else
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.IsDone).ToList();
                    else
                        list = list.OrderBy(d => d.IsDone).ToList();
                }
            }

            var assetTimeObjuestsPerPage = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            mainClass.Results = assetTimeObjuestsPerPage;
            mainClass.Count = list.Count();
            mainClass.CountDone = list.Where(a => a.IsDone == true).ToList().Count;
            mainClass.CountNotDone = list.Where(a => a.IsDone == false).ToList().Count;
            return mainClass;
        }

        public int Update(WNPMAssetTime model)
        {
            try
            {
                var timeObj = _context.WNPMAssetTimes.Find(model.Id);
                timeObj.PMDate = model.PMDate;
                timeObj.HospitalId = model.HospitalId;
                timeObj.AssetDetailId = model.AssetDetailId;

                if (model.strDueDate != null)
                    timeObj.DueDate = DateTime.Parse(model.strDueDate);//.AddDays(1);
                timeObj.IsDone = model.IsDone;


                if (model.strDoneDate != null)
                    timeObj.DoneDate = DateTime.Parse(model.strDoneDate);


                timeObj.SysDoneDate = DateTime.Today.Date;


                timeObj.Comment = model.Comment;
                if (model.AgencyId > 0)
                    timeObj.AgencyId = model.AgencyId;


                _context.Entry(timeObj).State = EntityState.Modified;
                _context.SaveChanges();
                return timeObj.Id;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return 0;
        }

        public int CreateAssetTimes(int year, int hospitalId)
        {
            int counter = 0;
            int m = 0;
            var totalAssets = _context.AssetDetails.OrderBy(a => a.DepartmentId).ToList();
            //  Year todayYear = new Year(DateTime.Today.Year);
            Year todayYear = new Year(year);
            ITimePeriodCollection quarters = todayYear.GetQuarters();

            var first = totalAssets.Skip(0).Take((int)Math.Round(totalAssets.Count * (0.4))).ToList();
            var sec = totalAssets.Skip(first.Count).Take((int)Math.Round(totalAssets.Count * (0.4))).ToList();
            var third = totalAssets.Skip(first.Count + sec.Count).Take((totalAssets.Count - (first.Count + sec.Count))).ToList();
            foreach (Quarter quarter in quarters)
            {

                counter = 0;
                m = 0;
                var s = quarter.FirstDayStart;
                var e = quarter.LastDayStart;
                var s1 = quarter.FirstMonthStart;
                var e1 = quarter.LastMonthStart;

                for (int i = 0; i < quarter.GetMonths().Count; i++)
                {
                    counter++;
                    var c = quarter.FirstMonthStart.AddMonths(m);
                    DateTime startDate = c;
                    int remain = 0;
                    //int noOfDaysInMonth = DateTime.DaysInMonth(DateTime.Today.Year, startDate.Month);
                    int noOfDaysInMonth = DateTime.DaysInMonth(year, startDate.Month);
                    if (counter == 1)
                    {

                        var list = new List<int>();
                        for (int day = 0; day < noOfDaysInMonth; day++)
                        {
                            int itemsInDay = (((day + 1) * first.Count()) + noOfDaysInMonth / 2) / noOfDaysInMonth - (day * first.Count() + noOfDaysInMonth / 2) / noOfDaysInMonth;
                            int x = first.Count() - remain;
                            List<AssetDetail> assetperday = new List<AssetDetail>();
                            if (day == 0)
                            {
                                assetperday = first.Skip(0).Take(itemsInDay).ToList();
                                foreach (var f in assetperday)
                                {
                                    var assetDate = DateTime.Parse(year + "-" + startDate.Month + "-" + (day + 1));
                                    WNPMAssetTime timeObj = new WNPMAssetTime();
                                    timeObj.AssetDetailId = f.Id;
                                    timeObj.HospitalId = f.HospitalId;
                                    timeObj.PMDate = assetDate;
                                    timeObj.IsDone = false;
                                    _context.WNPMAssetTimes.Add(timeObj);
                                    _context.SaveChanges();
                                }
                            }
                            else
                            {
                                assetperday = first.Skip(remain).Take(itemsInDay).ToList();
                                foreach (var f in assetperday)
                                {
                                    var assetDate = DateTime.Parse(year + "-" + startDate.Month + "-" + (day + 1));
                                    WNPMAssetTime timeObj = new WNPMAssetTime();
                                    timeObj.AssetDetailId = f.Id;
                                    timeObj.HospitalId = f.HospitalId;
                                    timeObj.PMDate = assetDate;
                                    timeObj.IsDone = false;
                                    _context.WNPMAssetTimes.Add(timeObj);
                                    _context.SaveChanges();
                                }
                            }
                            remain += itemsInDay;
                        }
                    }
                    if (counter == 2)
                    {
                        remain = 0;
                        var list = new List<int>();
                        for (int day = 0; day < noOfDaysInMonth; day++)
                        {
                            int itemsInDay = (((day + 1) * sec.Count()) + noOfDaysInMonth / 2) / noOfDaysInMonth - (day * sec.Count() + noOfDaysInMonth / 2) / noOfDaysInMonth;
                            int x = sec.Count() - remain;
                            List<AssetDetail> assetperday = new List<AssetDetail>();
                            if (day == 0)
                            {
                                assetperday = sec.Skip(0).Take(itemsInDay).ToList();
                                foreach (var f in assetperday)
                                {
                                    var assetDate = DateTime.Parse(year + "-" + startDate.Month + "-" + (day + 1));
                                    WNPMAssetTime timeObj = new WNPMAssetTime();
                                    timeObj.AssetDetailId = f.Id;
                                    timeObj.HospitalId = f.HospitalId;
                                    timeObj.PMDate = assetDate;
                                    timeObj.IsDone = false;
                                    _context.WNPMAssetTimes.Add(timeObj);
                                    _context.SaveChanges();
                                }
                            }
                            else
                            {
                                assetperday = sec.Skip(remain).Take(itemsInDay).ToList();
                                foreach (var f in assetperday)
                                {
                                    var assetDate = DateTime.Parse(year + "-" + startDate.Month + "-" + (day + 1));
                                    WNPMAssetTime timeObj = new WNPMAssetTime();
                                    timeObj.AssetDetailId = f.Id;
                                    timeObj.HospitalId = f.HospitalId;
                                    timeObj.PMDate = assetDate;
                                    timeObj.IsDone = false;
                                    _context.WNPMAssetTimes.Add(timeObj);
                                    _context.SaveChanges();

                                }
                            }
                            remain += itemsInDay;
                        }
                    }
                    if (counter == 3)
                    {
                        if (third.Count < noOfDaysInMonth)
                        {
                            remain = 0;
                            int ass = 0;
                            var list = new List<int>();
                            for (int day = 0; day < noOfDaysInMonth; day++)
                            {
                                //  int itemsInDay = (((day + 1) * third.Count()) + noOfDaysInMonth / 2) / noOfDaysInMonth - (day * third.Count() + noOfDaysInMonth / 2) / noOfDaysInMonth;
                                int x = third.Count() - remain;
                                List<AssetDetail> assetperday = new List<AssetDetail>();
                                if (day == 0)
                                {
                                    assetperday = third.Skip(0).Take(1).ToList();
                                    foreach (var f in assetperday)
                                    {
                                        var assetDate = DateTime.Parse(year + "-" + startDate.Month + "-" + (day + 1));
                                        WNPMAssetTime timeObj = new WNPMAssetTime();
                                        timeObj.AssetDetailId = f.Id;
                                        timeObj.HospitalId = f.HospitalId;
                                        timeObj.PMDate = assetDate;
                                        timeObj.IsDone = false;
                                        _context.WNPMAssetTimes.Add(timeObj);
                                        _context.SaveChanges();
                                    }
                                }
                                else
                                {
                                    assetperday = third.Skip(ass).Take(1).ToList();
                                    foreach (var f in assetperday)
                                    {
                                        var assetDate = DateTime.Parse(year + "-" + startDate.Month + "-" + (day + 1));
                                        WNPMAssetTime timeObj = new WNPMAssetTime();
                                        timeObj.AssetDetailId = f.Id;
                                        timeObj.HospitalId = f.HospitalId;
                                        timeObj.PMDate = assetDate;
                                        timeObj.IsDone = false;
                                        _context.WNPMAssetTimes.Add(timeObj);
                                        _context.SaveChanges();
                                    }
                                }
                                //  remain += itemsInDay;
                                ass++;
                            }
                        }

                        else
                        {
                            remain = 0;
                            // int noOfDaysInMonth = DateTime.DaysInMonth(DateTime.Today.Year, startDate.Month);
                            var list = new List<int>();
                            for (int day = 0; day < noOfDaysInMonth; day++)
                            {
                                int itemsInDay = (((day + 1) * third.Count()) + noOfDaysInMonth / 2) / noOfDaysInMonth - (day * third.Count() + noOfDaysInMonth / 2) / noOfDaysInMonth;


                                int x = third.Count() - remain;
                                List<AssetDetail> assetperday = new List<AssetDetail>();
                                if (day == 0)
                                {
                                    assetperday = third.Skip(0).Take(itemsInDay).ToList();
                                    foreach (var f in assetperday)
                                    {
                                        var assetDate = DateTime.Parse(year + "-" + startDate.Month + "-" + (day + 1));
                                        WNPMAssetTime timeObj = new WNPMAssetTime();
                                        timeObj.AssetDetailId = f.Id;
                                        timeObj.HospitalId = f.HospitalId;
                                        timeObj.PMDate = assetDate;
                                        timeObj.IsDone = false;
                                        _context.WNPMAssetTimes.Add(timeObj);
                                        _context.SaveChanges();
                                    }
                                }
                                else
                                {
                                    assetperday = third.Skip(remain).Take(itemsInDay).ToList();
                                    foreach (var f in assetperday)
                                    {
                                        var assetDate = DateTime.Parse(year + "-" + startDate.Month + "-" + (day + 1));
                                        WNPMAssetTime timeObj = new WNPMAssetTime();
                                        timeObj.AssetDetailId = f.Id;
                                        timeObj.HospitalId = f.HospitalId;
                                        timeObj.PMDate = assetDate;
                                        timeObj.IsDone = false;
                                        _context.WNPMAssetTimes.Add(timeObj);
                                        _context.SaveChanges();
                                    }
                                }
                                remain += itemsInDay;
                            }
                        }
                    }
                    m++;
                }
            }
            return 1;
        }



        //public int CreateAssetTimes(int year, int hospitalId)
        //{
        //    int counter = 0;
        //    int m = 0;
        //    var totalAssets = _context.AssetDetails.OrderBy(a => a.DepartmentId).ToList();
        //    Year todayYear = new Year(DateTime.Today.Year);
        //    ITimePeriodCollection quarters = todayYear.GetQuarters();
        //    var first = totalAssets.Skip(0).Take((int)Math.Round(totalAssets.Count * (0.4))).ToList();
        //    var sec = totalAssets.Skip(first.Count).Take((int)Math.Round(totalAssets.Count * (0.4))).ToList();
        //    var third = totalAssets.Skip(first.Count + sec.Count).Take((totalAssets.Count - (first.Count + sec.Count))).ToList();
        //    foreach (Quarter quarter in quarters)
        //    {
        //        counter = 0;
        //        m = 0;
        //        var s = quarter.FirstDayStart;
        //        var e = quarter.LastDayStart;
        //        var s1 = quarter.FirstMonthStart;
        //        var e1 = quarter.LastMonthStart;

        //        for (int i = 0; i < quarter.GetMonths().Count; i++)
        //        {
        //            counter++;
        //            var c = quarter.FirstMonthStart.AddMonths(m);
        //            DateTime startDate = c;
        //            int remain = 0;
        //            int noOfDaysInMonth = DateTime.DaysInMonth(DateTime.Today.Year, startDate.Month);
        //            int CountOfMonthWithoutWeekends = 0;
        //            for (int j = 0; j < noOfDaysInMonth; j++)
        //            {
        //                var nameOfday = DateTime.Parse(DateTime.Today.Year + "-" + startDate.Month + "-" + (j + 1));
        //                string dayname = nameOfday.ToString("dddd");
        //                if (!dayname.Equals("Friday"))
        //                {
        //                    CountOfMonthWithoutWeekends++;
        //                }
        //            }


        //            if (counter == 1)
        //            {
        //                if (first.Count < noOfDaysInMonth)
        //                {
        //                    remain = 0;
        //                    int ass = 0;
        //                    var list = new List<int>();
        //                    for (int day = 0; day < noOfDaysInMonth; day++)
        //                    {
        //                        var assetDate = DateTime.Parse(DateTime.Today.Year + "-" + startDate.Month + "-" + (day + 1));
        //                        string stry = assetDate.ToString("dddd");
        //                        if (stry.Equals("Friday") && day < noOfDaysInMonth - 1)
        //                        {
        //                            day++;
        //                            assetDate = DateTime.Parse(DateTime.Today.Year + "-" + startDate.Month + "-" + (day + 1));

        //                        }
        //                        List<AssetDetail> assetperday = new List<AssetDetail>();
        //                        if (day < noOfDaysInMonth - 1)
        //                        {
        //                            if (day == 0)
        //                            {
        //                                assetperday = first.Skip(0).Take(1).ToList();
        //                                foreach (var f in assetperday)
        //                                {
        //                                    assetDate = DateTime.Parse(DateTime.Today.Year + "-" + startDate.Month + "-" + (day + 1));
        //                                    WNPMAssetTime timeObj = new WNPMAssetTime();
        //                                    timeObj.AssetDetailId = f.Id;
        //                                    timeObj.HospitalId = f.HospitalId;
        //                                    timeObj.PMDate = assetDate;
        //                                    timeObj.IsDone = false;
        //                                    _context.WNPMAssetTimes.Add(timeObj);
        //                                    _context.SaveChanges();
        //                                }
        //                            }
        //                            else
        //                            {
        //                                assetperday = first.Skip(ass).Take(1).ToList();
        //                                foreach (var f in assetperday)
        //                                {
        //                                    assetDate = DateTime.Parse(DateTime.Today.Year + "-" + startDate.Month + "-" + (day + 1));
        //                                    WNPMAssetTime timeObj = new WNPMAssetTime();
        //                                    timeObj.AssetDetailId = f.Id;
        //                                    timeObj.HospitalId = f.HospitalId;
        //                                    timeObj.PMDate = assetDate;
        //                                    timeObj.IsDone = false;
        //                                    _context.WNPMAssetTimes.Add(timeObj);
        //                                    _context.SaveChanges();
        //                                }
        //                            }
        //                        }
        //                        ass++;
        //                    }
        //                }
        //                else
        //                {

        //                    var listCountAssetPerDay = new List<int>();
        //                    for (int d = 0; d < CountOfMonthWithoutWeekends; d++)
        //                    {
        //                        int itemsInDay = (((d + 1) * first.Count()) + CountOfMonthWithoutWeekends / 2) / CountOfMonthWithoutWeekends - (d * first.Count() + CountOfMonthWithoutWeekends / 2) / CountOfMonthWithoutWeekends;
        //                        listCountAssetPerDay.Add(itemsInDay);
        //                    }
        //                    int xx = 0;
        //                    for (int day = 0; day < noOfDaysInMonth; day++)
        //                    {

        //                        List<AssetDetail> assetperday = new List<AssetDetail>();
        //                        var assetDate = DateTime.Parse(DateTime.Today.Year + "-" + startDate.Month + "-" + (day + 1));
        //                        string stry = assetDate.ToString("dddd");
        //                        if (stry.Equals("Friday") && day < noOfDaysInMonth - 1)
        //                        {
        //                            day++;
        //                            assetDate = DateTime.Parse(DateTime.Today.Year + "-" + startDate.Month + "-" + (day + 1));

        //                        }

        //                        if (day <= noOfDaysInMonth - 1)
        //                        {
        //                            if (day == 0)
        //                            {
        //                                assetperday = first.Skip(0).Take(listCountAssetPerDay[xx]).ToList();
        //                                foreach (var f in assetperday)
        //                                {
        //                                    WNPMAssetTime timeObj = new WNPMAssetTime();
        //                                    timeObj.AssetDetailId = f.Id;
        //                                    timeObj.HospitalId = f.HospitalId;
        //                                    timeObj.PMDate = assetDate;
        //                                    timeObj.IsDone = false;
        //                                    _context.WNPMAssetTimes.Add(timeObj);
        //                                    _context.SaveChanges();
        //                                }
        //                                xx++;
        //                            }
        //                            else
        //                            {
        //                                assetperday = first.Skip(remain).Take(listCountAssetPerDay[xx]).ToList();
        //                                foreach (var f in assetperday)
        //                                {
        //                                    //   var assetDate = DateTime.Parse(DateTime.Today.Year + "-" + startDate.Month + "-" + (day + 1));
        //                                    WNPMAssetTime timeObj = new WNPMAssetTime();
        //                                    timeObj.AssetDetailId = f.Id;
        //                                    timeObj.HospitalId = f.HospitalId;
        //                                    timeObj.PMDate = assetDate;
        //                                    timeObj.IsDone = false;
        //                                    _context.WNPMAssetTimes.Add(timeObj);
        //                                    _context.SaveChanges();

        //                                }
        //                                xx++;
        //                            }
        //                            if (xx < listCountAssetPerDay.Count)
        //                            {
        //                                remain += listCountAssetPerDay[xx];
        //                            }

        //                        }
        //                    }
        //                }
        //            }

        //            if (counter == 2)
        //            {
        //                if (sec.Count < noOfDaysInMonth)
        //                {
        //                    remain = 0;
        //                    int ass = 0;
        //                    var list = new List<int>();
        //                    for (int day = 0; day < noOfDaysInMonth; day++)
        //                    {
        //                        var assetDate = DateTime.Parse(DateTime.Today.Year + "-" + startDate.Month + "-" + (day + 1));
        //                        string stry = assetDate.ToString("dddd");
        //                        if (stry.Equals("Friday") && day < noOfDaysInMonth - 1)
        //                        {
        //                            day++;
        //                            assetDate = DateTime.Parse(DateTime.Today.Year + "-" + startDate.Month + "-" + (day + 1));

        //                        }
        //                        List<AssetDetail> assetperday = new List<AssetDetail>();
        //                        if (day < noOfDaysInMonth - 1)
        //                        {
        //                            if (day == 0)
        //                            {
        //                                assetperday = sec.Skip(0).Take(1).ToList();
        //                                foreach (var f in assetperday)
        //                                {
        //                                    assetDate = DateTime.Parse(DateTime.Today.Year + "-" + startDate.Month + "-" + (day + 1));
        //                                    WNPMAssetTime timeObj = new WNPMAssetTime();
        //                                    timeObj.AssetDetailId = f.Id;
        //                                    timeObj.HospitalId = f.HospitalId;
        //                                    timeObj.PMDate = assetDate;
        //                                    timeObj.IsDone = false;
        //                                    _context.WNPMAssetTimes.Add(timeObj);
        //                                    _context.SaveChanges();
        //                                }
        //                            }
        //                            else
        //                            {
        //                                assetperday = sec.Skip(ass).Take(1).ToList();
        //                                foreach (var f in assetperday)
        //                                {
        //                                    assetDate = DateTime.Parse(DateTime.Today.Year + "-" + startDate.Month + "-" + (day + 1));
        //                                    WNPMAssetTime timeObj = new WNPMAssetTime();
        //                                    timeObj.AssetDetailId = f.Id;
        //                                    timeObj.HospitalId = f.HospitalId;
        //                                    timeObj.PMDate = assetDate;
        //                                    timeObj.IsDone = false;
        //                                    _context.WNPMAssetTimes.Add(timeObj);
        //                                    _context.SaveChanges();
        //                                }
        //                            }
        //                        }
        //                        ass++;
        //                    }
        //                }
        //                else
        //                {

        //                    var listCountAssetPerDay = new List<int>();
        //                    for (int d = 0; d < CountOfMonthWithoutWeekends; d++)
        //                    {
        //                        int itemsInDay = (((d + 1) * sec.Count()) + CountOfMonthWithoutWeekends / 2) / CountOfMonthWithoutWeekends - (d * sec.Count() + CountOfMonthWithoutWeekends / 2) / CountOfMonthWithoutWeekends;
        //                        listCountAssetPerDay.Add(itemsInDay);
        //                    }
        //                    int xx = 0;
        //                    for (int day = 0; day < noOfDaysInMonth; day++)
        //                    {

        //                        List<AssetDetail> assetperday = new List<AssetDetail>();
        //                        var assetDate = DateTime.Parse(DateTime.Today.Year + "-" + startDate.Month + "-" + (day + 1));
        //                        string stry = assetDate.ToString("dddd");
        //                        if (stry.Equals("Friday") && day < noOfDaysInMonth - 1)
        //                        {
        //                            day++;
        //                            assetDate = DateTime.Parse(DateTime.Today.Year + "-" + startDate.Month + "-" + (day + 1));

        //                        }

        //                        if (day <= noOfDaysInMonth - 1)
        //                        {
        //                            if (day == 0)
        //                            {
        //                                assetperday = sec.Skip(0).Take(listCountAssetPerDay[xx]).ToList();
        //                                foreach (var f in assetperday)
        //                                {
        //                                    WNPMAssetTime timeObj = new WNPMAssetTime();
        //                                    timeObj.AssetDetailId = f.Id;
        //                                    timeObj.HospitalId = f.HospitalId;
        //                                    timeObj.PMDate = assetDate;
        //                                    timeObj.IsDone = false;
        //                                    _context.WNPMAssetTimes.Add(timeObj);
        //                                    _context.SaveChanges();
        //                                }
        //                                xx++;
        //                            }
        //                            else
        //                            {
        //                                assetperday = sec.Skip(remain).Take(listCountAssetPerDay[xx]).ToList();
        //                                foreach (var f in assetperday)
        //                                {
        //                                    //   var assetDate = DateTime.Parse(DateTime.Today.Year + "-" + startDate.Month + "-" + (day + 1));
        //                                    WNPMAssetTime timeObj = new WNPMAssetTime();
        //                                    timeObj.AssetDetailId = f.Id;
        //                                    timeObj.HospitalId = f.HospitalId;
        //                                    timeObj.PMDate = assetDate;
        //                                    timeObj.IsDone = false;
        //                                    _context.WNPMAssetTimes.Add(timeObj);
        //                                    _context.SaveChanges();

        //                                }
        //                                xx++;
        //                            }
        //                            if (xx < listCountAssetPerDay.Count)
        //                            {
        //                                remain += listCountAssetPerDay[xx];
        //                            }

        //                        }
        //                    }
        //                }
        //            }

        //            if (counter == 3)
        //            {
        //                if (third.Count < noOfDaysInMonth)
        //                {
        //                    remain = 0;
        //                    int ass = 0;
        //                    var list = new List<int>();
        //                    for (int day = 0; day < noOfDaysInMonth; day++)
        //                    {
        //                        var assetDate = DateTime.Parse(DateTime.Today.Year + "-" + startDate.Month + "-" + (day + 1));
        //                        string stry = assetDate.ToString("dddd");
        //                        if (stry.Equals("Friday") && day < noOfDaysInMonth - 1)
        //                        {
        //                            day++;
        //                            assetDate = DateTime.Parse(DateTime.Today.Year + "-" + startDate.Month + "-" + (day + 1));

        //                        }
        //                        List<AssetDetail> assetperday = new List<AssetDetail>();
        //                        if (day <= noOfDaysInMonth - 1)
        //                        {
        //                            if (day == 0)
        //                            {
        //                                assetperday = third.Skip(0).Take(1).ToList();
        //                                foreach (var f in assetperday)
        //                                {
        //                                    assetDate = DateTime.Parse(DateTime.Today.Year + "-" + startDate.Month + "-" + (day + 1));
        //                                    WNPMAssetTime timeObj = new WNPMAssetTime();
        //                                    timeObj.AssetDetailId = f.Id;
        //                                    timeObj.HospitalId = f.HospitalId;
        //                                    timeObj.PMDate = assetDate;
        //                                    timeObj.IsDone = false;
        //                                    _context.WNPMAssetTimes.Add(timeObj);
        //                                    _context.SaveChanges();
        //                                }
        //                            }
        //                            else
        //                            {
        //                                assetperday = third.Skip(ass).Take(1).ToList();
        //                                foreach (var f in assetperday)
        //                                {
        //                                    assetDate = DateTime.Parse(DateTime.Today.Year + "-" + startDate.Month + "-" + (day + 1));
        //                                    WNPMAssetTime timeObj = new WNPMAssetTime();
        //                                    timeObj.AssetDetailId = f.Id;
        //                                    timeObj.HospitalId = f.HospitalId;
        //                                    timeObj.PMDate = assetDate;
        //                                    timeObj.IsDone = false;
        //                                    _context.WNPMAssetTimes.Add(timeObj);
        //                                    _context.SaveChanges();
        //                                }
        //                            }
        //                        }
        //                        ass++;
        //                    }
        //                }
        //                else
        //                {

        //                    var listCountAssetPerDay = new List<int>();
        //                    for (int d = 0; d < CountOfMonthWithoutWeekends; d++)
        //                    {
        //                        int itemsInDay = (((d + 1) * third.Count()) + CountOfMonthWithoutWeekends / 2) / CountOfMonthWithoutWeekends - (d * third.Count() + CountOfMonthWithoutWeekends / 2) / CountOfMonthWithoutWeekends;
        //                        listCountAssetPerDay.Add(itemsInDay);
        //                    }
        //                    int xx = 0;
        //                    for (int day = 0; day < noOfDaysInMonth; day++)
        //                    {

        //                        List<AssetDetail> assetperday = new List<AssetDetail>();
        //                        var assetDate = DateTime.Parse(DateTime.Today.Year + "-" + startDate.Month + "-" + (day + 1));
        //                        string stry = assetDate.ToString("dddd");
        //                        if (stry.Equals("Friday") && day < noOfDaysInMonth - 1)
        //                        {
        //                            day++;
        //                            assetDate = DateTime.Parse(DateTime.Today.Year + "-" + startDate.Month + "-" + (day + 1));

        //                        }

        //                        if (day <= noOfDaysInMonth - 1)
        //                        {
        //                            if (day == 0)
        //                            {
        //                                assetperday = third.Skip(0).Take(listCountAssetPerDay[xx]).ToList();
        //                                foreach (var f in assetperday)
        //                                {
        //                                    WNPMAssetTime timeObj = new WNPMAssetTime();
        //                                    timeObj.AssetDetailId = f.Id;
        //                                    timeObj.HospitalId = f.HospitalId;
        //                                    timeObj.PMDate = assetDate;
        //                                    timeObj.IsDone = false;
        //                                    _context.WNPMAssetTimes.Add(timeObj);
        //                                    _context.SaveChanges();
        //                                }
        //                                xx++;
        //                            }
        //                            else
        //                            {
        //                                assetperday = third.Skip(remain).Take(listCountAssetPerDay[xx]).ToList();
        //                                foreach (var f in assetperday)
        //                                {
        //                                    //   var assetDate = DateTime.Parse(DateTime.Today.Year + "-" + startDate.Month + "-" + (day + 1));
        //                                    WNPMAssetTime timeObj = new WNPMAssetTime();
        //                                    timeObj.AssetDetailId = f.Id;
        //                                    timeObj.HospitalId = f.HospitalId;
        //                                    timeObj.PMDate = assetDate;
        //                                    timeObj.IsDone = false;
        //                                    _context.WNPMAssetTimes.Add(timeObj);
        //                                    _context.SaveChanges();

        //                                }
        //                                xx++;
        //                            }
        //                            if (xx < listCountAssetPerDay.Count)
        //                            {
        //                                remain += listCountAssetPerDay[xx];
        //                            }

        //                        }
        //                    }
        //                }
        //            }

        //            m++;
        //        }
        //    }
        //    return 1;
        //}


        public List<WNPMAssetTime> GetAllWNPMAssetTime()
        {
            return _context.WNPMAssetTimes.ToList();
        }


        public int CreateWNPMAssetTimeAttachment(WNPMAssetTimeAttachment attachObj)
        {
            WNPMAssetTimeAttachment documentObj = new WNPMAssetTimeAttachment();
            documentObj.Title = attachObj.Title;
            documentObj.FileName = attachObj.FileName;
            documentObj.WNPMAssetTimeId = attachObj.WNPMAssetTimeId;
            documentObj.HospitalId = attachObj.HospitalId;
            _context.WNPMAssetTimeAttachments.Add(documentObj);
            _context.SaveChanges();
            return attachObj.Id;
        }

        public List<WNPMAssetTimeAttachment> GetWNPMAssetTimeAttachmentByWNPMAssetTimeId(int wnpmAssetTimeId)
        {
            return _context.WNPMAssetTimeAttachments.Where(a => a.WNPMAssetTimeId == wnpmAssetTimeId).ToList();
        }

        public int CreateAssetFiscalTimes(int year, int hospitalId)
        {
            int counter = 0;
            int m = 0;
            var totalAssets = _context.AssetDetails.OrderBy(a => a.DepartmentId).ToList();
            //  Year todayYear = new Year(DateTime.Today.Year);
            Year todayYear = new Year(year, new FiscalTimeCalendar());
            ITimePeriodCollection quarters = todayYear.GetQuarters();

            var first = totalAssets.Skip(0).Take((int)Math.Round(totalAssets.Count * (0.4))).ToList();
            var sec = totalAssets.Skip(first.Count).Take((int)Math.Round(totalAssets.Count * (0.4))).ToList();
            var third = totalAssets.Skip(first.Count + sec.Count).Take((totalAssets.Count - (first.Count + sec.Count))).ToList();
            foreach (Quarter quarter in quarters)
            {

                counter = 0;
                m = 0;
                var s = quarter.FirstDayStart;
                var e = quarter.LastDayStart;
                var s1 = quarter.FirstMonthStart;
                var e1 = quarter.LastMonthStart;

                for (int i = 0; i < quarter.GetMonths().Count; i++)
                {
                    counter++;
                    var c = quarter.FirstMonthStart.AddMonths(m);
                    DateTime startDate = c;
                    int remain = 0;
                    //int noOfDaysInMonth = DateTime.DaysInMonth(DateTime.Today.Year, startDate.Month);

                    int noOfDaysInMonth = DateTime.DaysInMonth(quarter.FirstDayStart.Year, startDate.Month);
                    if (counter == 1)
                    {

                        var list = new List<int>();
                        for (int day = 0; day < noOfDaysInMonth; day++)
                        {
                            int itemsInDay = (((day + 1) * first.Count()) + noOfDaysInMonth / 2) / noOfDaysInMonth - (day * first.Count() + noOfDaysInMonth / 2) / noOfDaysInMonth;
                            int x = first.Count() - remain;
                            List<AssetDetail> assetperday = new List<AssetDetail>();
                            if (day == 0)
                            {
                                assetperday = first.Skip(0).Take(itemsInDay).ToList();
                                foreach (var f in assetperday)
                                {
                                    var assetDate = DateTime.Parse(quarter.FirstDayStart.Year + "-" + startDate.Month + "-" + (day + 1));
                                    WNPMAssetTime timeObj = new WNPMAssetTime();
                                    timeObj.AssetDetailId = f.Id;
                                    timeObj.HospitalId = f.HospitalId;
                                    timeObj.PMDate = assetDate;
                                    timeObj.IsDone = false;
                                    _context.WNPMAssetTimes.Add(timeObj);
                                    _context.SaveChanges();
                                }
                            }
                            else
                            {
                                assetperday = first.Skip(remain).Take(itemsInDay).ToList();
                                foreach (var f in assetperday)
                                {
                                    var assetDate = DateTime.Parse(quarter.FirstDayStart.Year + "-" + startDate.Month + "-" + (day + 1));
                                    WNPMAssetTime timeObj = new WNPMAssetTime();
                                    timeObj.AssetDetailId = f.Id;
                                    timeObj.HospitalId = f.HospitalId;
                                    timeObj.PMDate = assetDate;
                                    timeObj.IsDone = false;
                                    _context.WNPMAssetTimes.Add(timeObj);
                                    _context.SaveChanges();
                                }
                            }
                            remain += itemsInDay;
                        }
                    }
                    if (counter == 2)
                    {
                        remain = 0;
                        var list = new List<int>();
                        for (int day = 0; day < noOfDaysInMonth; day++)
                        {
                            int itemsInDay = (((day + 1) * sec.Count()) + noOfDaysInMonth / 2) / noOfDaysInMonth - (day * sec.Count() + noOfDaysInMonth / 2) / noOfDaysInMonth;
                            int x = sec.Count() - remain;
                            List<AssetDetail> assetperday = new List<AssetDetail>();
                            if (day == 0)
                            {
                                assetperday = sec.Skip(0).Take(itemsInDay).ToList();
                                foreach (var f in assetperday)
                                {
                                    var assetDate = DateTime.Parse(quarter.FirstDayStart.Year + "-" + startDate.Month + "-" + (day + 1));
                                    WNPMAssetTime timeObj = new WNPMAssetTime();
                                    timeObj.AssetDetailId = f.Id;
                                    timeObj.HospitalId = f.HospitalId;
                                    timeObj.PMDate = assetDate;
                                    timeObj.IsDone = false;
                                    _context.WNPMAssetTimes.Add(timeObj);
                                    _context.SaveChanges();
                                }
                            }
                            else
                            {
                                assetperday = sec.Skip(remain).Take(itemsInDay).ToList();
                                foreach (var f in assetperday)
                                {
                                    var assetDate = DateTime.Parse(quarter.FirstDayStart.Year + "-" + startDate.Month + "-" + (day + 1));
                                    WNPMAssetTime timeObj = new WNPMAssetTime();
                                    timeObj.AssetDetailId = f.Id;
                                    timeObj.HospitalId = f.HospitalId;
                                    timeObj.PMDate = assetDate;
                                    timeObj.IsDone = false;
                                    _context.WNPMAssetTimes.Add(timeObj);
                                    _context.SaveChanges();

                                }
                            }
                            remain += itemsInDay;
                        }
                    }
                    if (counter == 3)
                    {
                        if (third.Count < noOfDaysInMonth)
                        {
                            remain = 0;
                            int ass = 0;
                            var list = new List<int>();
                            for (int day = 0; day < noOfDaysInMonth; day++)
                            {
                                //  int itemsInDay = (((day + 1) * third.Count()) + noOfDaysInMonth / 2) / noOfDaysInMonth - (day * third.Count() + noOfDaysInMonth / 2) / noOfDaysInMonth;
                                int x = third.Count() - remain;
                                List<AssetDetail> assetperday = new List<AssetDetail>();
                                if (day == 0)
                                {
                                    assetperday = third.Skip(0).Take(1).ToList();
                                    foreach (var f in assetperday)
                                    {
                                        var assetDate = DateTime.Parse(quarter.FirstDayStart.Year + "-" + startDate.Month + "-" + (day + 1));
                                        WNPMAssetTime timeObj = new WNPMAssetTime();
                                        timeObj.AssetDetailId = f.Id;
                                        timeObj.HospitalId = f.HospitalId;
                                        timeObj.PMDate = assetDate;
                                        timeObj.IsDone = false;
                                        _context.WNPMAssetTimes.Add(timeObj);
                                        _context.SaveChanges();
                                    }
                                }
                                else
                                {
                                    assetperday = third.Skip(ass).Take(1).ToList();
                                    foreach (var f in assetperday)
                                    {
                                        var assetDate = DateTime.Parse(quarter.FirstDayStart.Year + "-" + startDate.Month + "-" + (day + 1));
                                        WNPMAssetTime timeObj = new WNPMAssetTime();
                                        timeObj.AssetDetailId = f.Id;
                                        timeObj.HospitalId = f.HospitalId;
                                        timeObj.PMDate = assetDate;
                                        timeObj.IsDone = false;
                                        _context.WNPMAssetTimes.Add(timeObj);
                                        _context.SaveChanges();
                                    }
                                }
                                //  remain += itemsInDay;
                                ass++;
                            }
                        }

                        else
                        {
                            remain = 0;
                            // int noOfDaysInMonth = DateTime.DaysInMonth(DateTime.Today.Year, startDate.Month);
                            var list = new List<int>();
                            for (int day = 0; day < noOfDaysInMonth; day++)
                            {
                                int itemsInDay = (((day + 1) * third.Count()) + noOfDaysInMonth / 2) / noOfDaysInMonth - (day * third.Count() + noOfDaysInMonth / 2) / noOfDaysInMonth;


                                int x = third.Count() - remain;
                                List<AssetDetail> assetperday = new List<AssetDetail>();
                                if (day == 0)
                                {
                                    assetperday = third.Skip(0).Take(itemsInDay).ToList();
                                    foreach (var f in assetperday)
                                    {
                                        var assetDate = DateTime.Parse(quarter.FirstDayStart.Year + "-" + startDate.Month + "-" + (day + 1));
                                        WNPMAssetTime timeObj = new WNPMAssetTime();
                                        timeObj.AssetDetailId = f.Id;
                                        timeObj.HospitalId = f.HospitalId;
                                        timeObj.PMDate = assetDate;
                                        timeObj.IsDone = false;
                                        _context.WNPMAssetTimes.Add(timeObj);
                                        _context.SaveChanges();
                                    }
                                }
                                else
                                {
                                    assetperday = third.Skip(remain).Take(itemsInDay).ToList();
                                    foreach (var f in assetperday)
                                    {
                                        var assetDate = DateTime.Parse(quarter.FirstDayStart.Year + "-" + startDate.Month + "-" + (day + 1));
                                        WNPMAssetTime timeObj = new WNPMAssetTime();
                                        timeObj.AssetDetailId = f.Id;
                                        timeObj.HospitalId = f.HospitalId;
                                        timeObj.PMDate = assetDate;
                                        timeObj.IsDone = false;
                                        _context.WNPMAssetTimes.Add(timeObj);
                                        _context.SaveChanges();
                                    }
                                }
                                remain += itemsInDay;
                            }
                        }
                    }
                    m++;
                }
            }
            return 1;
        }

        public IndexWNPMAssetTimesVM GetAllWithDate(WNPDateVM filterObj, int pageNumber, int pageSize, string userId)
        {

            IndexWNPMAssetTimesVM mainClass = new IndexWNPMAssetTimesVM();
            List<IndexWNPMAssetTimesVM.GetData> list = new List<IndexWNPMAssetTimesVM.GetData>();
            var allWNAssetDetails = _context.WNPMAssetTimes
                .Include(a => a.AssetDetail).Include(a => a.AssetDetail.MasterAsset)
                 .Include(a => a.AssetDetail.Department)
                 .Include(a => a.Supplier)
                .Include(a => a.AssetDetail.Hospital)
                .OrderBy(a=>a.PMDate.Value.Date).ToList();


            foreach (var itm in allWNAssetDetails)
            {

                IndexWNPMAssetTimesVM.GetData item = new IndexWNPMAssetTimesVM.GetData();
                item.Id = itm.Id;
                if (itm.AssetDetail != null)
                {

                    item.BarCode = itm.AssetDetail.Barcode;

                    if (itm.AssetDetail.MasterAsset != null)
                    {
                        item.ModelNumber = itm.AssetDetail.MasterAsset.ModelNumber;

                    }
                    item.SerialNumber = itm.AssetDetail.SerialNumber;
                    item.DepartmentId = itm.AssetDetail.Department != null ? itm.AssetDetail.DepartmentId : 0;
                    item.DepartmentName = itm.AssetDetail.Department != null ? itm.AssetDetail.Department.Name : "";
                    item.DepartmentNameAr = itm.AssetDetail.Department != null ? itm.AssetDetail.Department.NameAr : "";

                }


                if (itm.Supplier != null)
                {
                    item.SupplierId = itm.AgencyId;
                    item.SupplierName = itm.Supplier.Name;
                    item.SupplierNameAr = itm.Supplier.NameAr;
                }
                item.PMDate = itm.PMDate;
                item.IsDone = itm.IsDone != null ? (bool)itm.IsDone : false;
                item.DoneDate = itm.DoneDate;
                item.DueDate = itm.DueDate;
                if (itm.AssetDetail != null)
                {
                    if (itm.AssetDetail.MasterAsset != null)
                    {
                        item.AssetName = itm.AssetDetail.MasterAssetId > 0 ? itm.AssetDetail.MasterAsset.Name : "";
                        item.AssetNameAr = itm.AssetDetail.MasterAssetId > 0 ? itm.AssetDetail.MasterAsset.NameAr : "";
                    }

                }

                list.Add(item);
            }


            string setstartday, setstartmonth, setendday, setendmonth = "";
            DateTime startingFrom = new DateTime();
            DateTime endingTo = new DateTime();
            if (filterObj.Start == "")
            {
                filterObj.StartDate = DateTime.Parse("01/01/1900");
            }
            else
            {
                filterObj.StartDate = DateTime.Parse(filterObj.Start.ToString());
                var startyear = filterObj.StartDate.Value.Year;
                var startmonth = filterObj.StartDate.Value.Month;
                var startday = filterObj.StartDate.Value.Day;
                if (startday < 10)
                    setstartday = filterObj.StartDate.Value.Day.ToString().PadLeft(2, '0');
                else
                    setstartday = filterObj.StartDate.Value.Day.ToString();

                if (startmonth < 10)
                    setstartmonth = filterObj.StartDate.Value.Month.ToString().PadLeft(2, '0');
                else
                    setstartmonth = filterObj.StartDate.Value.Month.ToString();

                var sDate = startyear + "/" + setstartmonth + "/" + setstartday;
                startingFrom = DateTime.Parse(sDate);//.AddDays(1);
            }

            if (filterObj.End == "")
            {
                filterObj.EndDate = DateTime.Today.Date;
            }
            else
            {
                filterObj.EndDate = DateTime.Parse(filterObj.End.ToString());
                var endyear = filterObj.EndDate.Value.Year;
                var endmonth = filterObj.EndDate.Value.Month;
                var endday = filterObj.EndDate.Value.Day;
                if (endday < 10)
                    setendday = filterObj.EndDate.Value.Day.ToString().PadLeft(2, '0');
                else
                    setendday = filterObj.EndDate.Value.Day.ToString();
                if (endmonth < 10)
                    setendmonth = filterObj.EndDate.Value.Month.ToString().PadLeft(2, '0');
                else
                    setendmonth = filterObj.EndDate.Value.Month.ToString();
                var eDate = endyear + "/" + setendmonth + "/" + setendday;
                endingTo = DateTime.Parse(eDate);
            }



            if (filterObj.Start != "" && filterObj.End != "")
            {
                list = list.Where(a => a.PMDate.Value.Date >= startingFrom.Date && a.PMDate.Value.Date <= endingTo.Date).OrderBy(d => d.PMDate.Value.Date).ToList();
            }

            var assetTimeObjuestsPerPage = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            mainClass.Results = assetTimeObjuestsPerPage;
            mainClass.Count = list.Count();
            return mainClass;
        }



    }
}
