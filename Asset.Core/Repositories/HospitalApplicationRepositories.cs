using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.HospitalApplicationVM;
using Asset.ViewModels.RoleCategoryVM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class HospitalApplicationRepositories : IHospitalApplicationRepository
    {
        private ApplicationDbContext _context;
        string msg;

        public HospitalApplicationRepositories(ApplicationDbContext context)
        {
            _context = context;
        }


        public EditHospitalApplicationVM GetById(int id)
        {

            var execludIds = (from execlude in _context.HospitalExecludeReasons
                              join trans in _context.HospitalReasonTransactions on execlude.Id equals trans.ReasonId
                              where trans.HospitalApplicationId == id
                              select execlude.Id).ToList();


            var holdIds = (from execlude in _context.HospitalHoldReasons
                           join trans in _context.HospitalReasonTransactions on execlude.Id equals trans.ReasonId
                           where trans.HospitalApplicationId == id
                           select execlude.Id).ToList();





            return _context.HospitalApplications.Include(a => a.User).Include(a => a.ApplicationType)
                .Include(a => a.User).Include(a => a.AssetDetail).Include(a => a.AssetDetail.MasterAsset).Where(a => a.Id == id).Select(item => new EditHospitalApplicationVM
                {
                    Id = item.Id,
                    AssetId = item.AssetId,
                    StatusId = item.StatusId,
                    AppDate = item.AppDate,
                    DueDate = item.DueDate.Value.ToShortDateString(),
                    AppNumber = item.AppNumber,
                    UserId = item.User.UserName,
                    AppTypeId = item.AppTypeId,
                    Comment = item.Comment,
                    ReasonIds = execludIds,
                    HoldReasonIds = holdIds,
                    assetName = item.AssetDetail.MasterAsset.Name + " - " + item.AssetDetail.SerialNumber,
                    assetNameAr = item.AssetDetail.MasterAsset.NameAr + " - " + item.AssetDetail.SerialNumber

                }).FirstOrDefault();
        }





        public IEnumerable<IndexHospitalApplicationVM.GetData> GetAll()
        {
            List<IndexHospitalApplicationVM.GetData> list = new List<IndexHospitalApplicationVM.GetData>();
            var lstHospitalApplications = _context.HospitalApplications.Include(a => a.ApplicationType).Include(a => a.User)
                .Include(a => a.AssetDetail).Include(a => a.AssetDetail.MasterAsset).ToList().OrderByDescending(a => a.AppDate.Value.Date).ToList();
            foreach (var item in lstHospitalApplications)
            {

                IndexHospitalApplicationVM.GetData getDataObj = new IndexHospitalApplicationVM.GetData();
                getDataObj.Id = item.Id;
                getDataObj.AppNumber = item.AppNumber;
                getDataObj.Date = item.AppDate.Value.ToShortDateString();
                getDataObj.DueDate = item.DueDate != null ? item.DueDate.Value.ToShortDateString() : "";
                getDataObj.AppTypeId = item.AppTypeId;
                getDataObj.UserName = item.User.UserName;
                getDataObj.AssetName = item.AssetDetail.MasterAsset.Name + " - " + item.AssetDetail.SerialNumber;
                getDataObj.AssetNameAr = item.AssetDetail.MasterAsset.NameAr + " - " + item.AssetDetail.SerialNumber;
                getDataObj.TypeName = item.ApplicationType.Name;
                getDataObj.TypeNameAr = item.ApplicationType.NameAr;

                getDataObj.DiffMonths = ((item.AppDate.Value.Year - DateTime.Today.Date.Year) * 12) + item.AppDate.Value.Month - DateTime.Today.Date.Month;
                getDataObj.IsMoreThan3Months = getDataObj.DiffMonths <= -3 ? true : false;

                getDataObj.StatusId = item.StatusId;

                var lstStatuses = _context.HospitalSupplierStatuses.Where(a => a.Id == item.StatusId).ToList();
                if (lstStatuses.Count > 0)
                {
                    getDataObj.StatusName = lstStatuses[0].Name;
                    getDataObj.StatusNameAr = lstStatuses[0].NameAr;
                }




                var ReasonExTitles = (from execlude in _context.HospitalExecludeReasons
                                      join trans in _context.HospitalReasonTransactions on execlude.Id equals trans.ReasonId
                                      where trans.HospitalApplicationId == item.Id
                                      && item.AppTypeId == 1
                                      select execlude).ToList();
                if (ReasonExTitles.Count > 0)
                {
                    List<string> execludeNames = new List<string>();// { "John", "Anna", "Monica" };
                    foreach (var reason in ReasonExTitles)
                    {
                        execludeNames.Add(reason.Name);
                    }

                    getDataObj.ReasonExTitles = string.Join(",", execludeNames);


                    List<string> execludeNamesAr = new List<string>();
                    foreach (var reason in ReasonExTitles)
                    {
                        execludeNamesAr.Add(reason.NameAr);
                    }
                    getDataObj.ReasonExTitlesAr = string.Join(",", execludeNamesAr);

                }

                var ReasonHoldTitles = (from execlude in _context.HospitalHoldReasons
                                        join trans in _context.HospitalReasonTransactions on execlude.Id equals trans.ReasonId
                                        where trans.HospitalApplicationId == item.Id
                                        && item.AppTypeId == 2
                                        select execlude).ToList();
                if (ReasonHoldTitles.Count > 0)
                {
                    List<string> holdNames = new List<string>();
                    foreach (var reason in ReasonHoldTitles)
                    {
                        holdNames.Add(reason.Name);
                    }
                    getDataObj.ReasonHoldTitles = string.Join(",", holdNames);

                    List<string> holdNamesAr = new List<string>();
                    foreach (var reason in ReasonHoldTitles)
                    {
                        holdNamesAr.Add(reason.NameAr);
                    }
                    getDataObj.ReasonHoldTitlesAr = string.Join(",", holdNamesAr);
                }


                list.Add(getDataObj);
            }

            return list;
        }


        public int Add(CreateHospitalApplicationVM model)
        {
            HospitalApplication hospitalApplicationObj = new HospitalApplication();
            try
            {
                if (model != null)
                {
                    hospitalApplicationObj.AssetId = model.AssetId;
                    hospitalApplicationObj.AppTypeId = model.AppTypeId;
                    hospitalApplicationObj.StatusId = 1;
                    hospitalApplicationObj.AppDate = DateTime.Today.Date;
                    if (model.DueDate != "")
                        hospitalApplicationObj.DueDate = DateTime.Parse(model.DueDate.ToString());
                    hospitalApplicationObj.AppNumber = model.AppNumber;
                    hospitalApplicationObj.UserId = model.UserId;
                    _context.HospitalApplications.Add(hospitalApplicationObj);
                    _context.SaveChanges();
                    int id = hospitalApplicationObj.Id;
                    //if (model.ReasonIds.Count > 0)
                    //{
                    //    foreach (var reasonId in model.ReasonIds)
                    //    {
                    //        HospitalReasonTransaction hospitalReasonTransactionObj = new HospitalReasonTransaction();
                    //        hospitalReasonTransactionObj.HospitalApplicationId = id;
                    //        hospitalReasonTransactionObj.ReasonId = reasonId;
                    //        _context.HospitalReasonTransactions.Add(hospitalReasonTransactionObj);
                    //        _context.SaveChanges();
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return hospitalApplicationObj.Id;
        }

        public int Delete(int id)
        {
            var hospitalApplicationObj = _context.HospitalApplications.Find(id);
            try
            {
                if (hospitalApplicationObj != null)
                {

                    _context.HospitalApplications.Remove(hospitalApplicationObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return 0;
        }

        public int Update(EditHospitalApplicationVM model)
        {
            try
            {

                var hospitalApplicationObj = _context.HospitalApplications.Find(model.Id);
                hospitalApplicationObj.AssetId = model.AssetId;
                hospitalApplicationObj.AppTypeId = model.AppTypeId;
                hospitalApplicationObj.StatusId = 1;
                hospitalApplicationObj.AppDate = DateTime.Today.Date;
                if (model.DueDate != "")
                    hospitalApplicationObj.DueDate = DateTime.Parse(model.DueDate.ToString());
                hospitalApplicationObj.AppNumber = model.AppNumber;
                hospitalApplicationObj.UserId = model.UserId;
                _context.Entry(hospitalApplicationObj).State = EntityState.Modified;
                _context.SaveChanges();


                if (model.ReasonIds.Count > 0)
                {

                    if (model.AppTypeId == 1)
                    {
                        List<int> reasonIds = new List<int>();
                        var savedReasonIds = (from execlude in _context.HospitalExecludeReasons
                                              join trans in _context.HospitalReasonTransactions on execlude.Id equals trans.ReasonId
                                              where trans.HospitalApplicationId == model.Id
                                                      && model.AppTypeId == 1
                                              select trans).ToList().Select(a => a.ReasonId).ToList();

                        foreach (var sr in savedReasonIds)
                        {
                            reasonIds.Add(sr.Value);
                        }



                        var savedIds = reasonIds.ToList().Except(model.ReasonIds);
                        if (savedIds.Count() > 0)
                        {
                            foreach (var item in savedIds)
                            {
                                var row = _context.HospitalReasonTransactions.Where(a => a.HospitalApplicationId == model.Id && a.ReasonId == item).ToList();
                                if (row.Count > 0)
                                {
                                    var reasonObj = row[0];
                                    _context.HospitalReasonTransactions.Remove(reasonObj);
                                    _context.SaveChanges();
                                }
                            }
                        }
                        var neewIds = model.ReasonIds.Except(reasonIds);
                        if (neewIds.Count() > 0)
                        {
                            foreach (var itm in neewIds)
                            {
                                HospitalReasonTransaction hospitalReasonObj = new HospitalReasonTransaction();
                                hospitalReasonObj.HospitalApplicationId = model.Id;
                                hospitalReasonObj.ReasonId = itm;
                                _context.HospitalReasonTransactions.Add(hospitalReasonObj);
                                _context.SaveChanges();
                            }
                        }

                    }



                    if (model.AppTypeId == 2)
                    {
                        List<int> reasonIds = new List<int>();
                        var savedReasonIds = (from execlude in _context.HospitalHoldReasons
                                              join trans in _context.HospitalReasonTransactions on execlude.Id equals trans.ReasonId
                                              where trans.HospitalApplicationId == model.Id
                                              && model.AppTypeId == 2
                                              select trans).ToList().Select(a => a.ReasonId).ToList();
                        foreach (var sr in savedReasonIds)
                        {
                            reasonIds.Add(sr.Value);
                        }



                        var savedIds = reasonIds.ToList().Except(model.ReasonIds);
                        if (savedIds.Count() > 0)
                        {
                            foreach (var item in savedIds)
                            {
                                var row = _context.HospitalReasonTransactions.Where(a => a.HospitalApplicationId == model.Id && a.ReasonId == item).ToList();
                                if (row.Count > 0)
                                {
                                    var reasonObj = row[0];
                                    _context.HospitalReasonTransactions.Remove(reasonObj);
                                    _context.SaveChanges();
                                }
                            }
                        }
                        var neewIds = model.ReasonIds.Except(reasonIds);
                        if (neewIds.Count() > 0)
                        {
                            foreach (var itm in neewIds)
                            {
                                HospitalReasonTransaction hospitalReasonObj = new HospitalReasonTransaction();
                                hospitalReasonObj.HospitalApplicationId = model.Id;
                                hospitalReasonObj.ReasonId = itm;
                                _context.HospitalReasonTransactions.Add(hospitalReasonObj);
                                _context.SaveChanges();
                            }
                        }

                    }



                    return hospitalApplicationObj.Id;

                }


            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return 0;
        }

        public int CreateHospitalApplicationAttachments(HospitalApplicationAttachment attachObj)
        {
            HospitalApplicationAttachment assetAttachmentObj = new HospitalApplicationAttachment();
            assetAttachmentObj.HospitalReasonTransactionId = attachObj.HospitalReasonTransactionId;
            assetAttachmentObj.Title = attachObj.Title;
            assetAttachmentObj.FileName = attachObj.FileName;
            _context.HospitalApplicationAttachments.Add(assetAttachmentObj);
            _context.SaveChanges();
            int Id = assetAttachmentObj.Id;
            return Id;
        }

        public IEnumerable<HospitalApplicationAttachment> GetAttachmentByHospitalApplicationId(int id)
        {
            return _context.HospitalApplicationAttachments.Where(a => a.HospitalReasonTransactionId == id).ToList();
        }

        public int DeleteHospitalApplicationAttachment(int id)
        {
            try
            {
                var attachObj = _context.HospitalApplicationAttachments.Find(id);
                _context.HospitalApplicationAttachments.Remove(attachObj);
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return 0;

        }

        public int UpdateExcludedDate(EditHospitalApplicationVM model)
        {
            var hospitalApplicationObj = _context.HospitalApplications.Find(model.Id);
            hospitalApplicationObj.StatusId = model.StatusId;

            if (model.StatusId == 2)
                hospitalApplicationObj.DueDate = DateTime.Parse(model.DueDate);
            if (model.StatusId == 3)
                hospitalApplicationObj.DueDate = DateTime.Today.Date;


            hospitalApplicationObj.DueDate = DateTime.Parse(model.DueDate.ToString());
            hospitalApplicationObj.UserId = model.UserId;
            hospitalApplicationObj.Comment = model.Comment;
            _context.Entry(hospitalApplicationObj).State = EntityState.Modified;
            _context.SaveChanges();


            if (model.StatusId == 2)
            {
                AssetStatusTransaction assetStatusTransactionObj = new AssetStatusTransaction();
                assetStatusTransactionObj.AssetDetailId = (int)model.AssetId;
                assetStatusTransactionObj.AssetStatusId = 8;
                assetStatusTransactionObj.StatusDate = DateTime.Today.Date;
                _context.AssetStatusTransactions.Add(assetStatusTransactionObj);
                _context.SaveChanges();
            }
            //if (model.StatusId == 3)
            //{
            //    AssetStatusTransaction assetStatusTransactionObj = new AssetStatusTransaction();
            //    assetStatusTransactionObj.AssetDetailId = (int)model.AssetId;
            //    assetStatusTransactionObj.AssetStatusId = 9;
            //    assetStatusTransactionObj.StatusDate = DateTime.Today.Date;
            //    _context.AssetStatusTransactions.Add(assetStatusTransactionObj);
            //    _context.SaveChanges();
            //}
            return hospitalApplicationObj.Id;
        }

        public ViewHospitalApplicationVM GetHospitalApplicationById(int id)
        {
            var execludNames = (from execlude in _context.HospitalExecludeReasons
                                join trans in _context.HospitalReasonTransactions on execlude.Id equals trans.ReasonId
                                where trans.HospitalApplicationId == id
                                select execlude).ToList();


            var holdNames = (from hold in _context.HospitalHoldReasons
                             join trans in _context.HospitalReasonTransactions on hold.Id equals trans.ReasonId
                             where trans.HospitalApplicationId == id
                             select hold).ToList();



            ViewHospitalApplicationVM hospitalApplicationObj = new ViewHospitalApplicationVM();
            var lstHostApplication = _context.HospitalApplications.Include(a => a.User).Include(a => a.ApplicationType)
                .Include(a => a.User).Include(a => a.AssetDetail).Include(a => a.AssetDetail.MasterAsset)
                .Where(a => a.Id == id).ToList();
            if (lstHostApplication.Count > 0)
            {
                var item = lstHostApplication[0];

                hospitalApplicationObj.Id = item.Id;
                hospitalApplicationObj.StatusId = item.StatusId;
                hospitalApplicationObj.AppTypeId = item.AppTypeId;
                hospitalApplicationObj.AppDate = item.AppDate;
                if (item.DueDate != null)
                    hospitalApplicationObj.DueDate = item.DueDate.Value.ToShortDateString();

                hospitalApplicationObj.AppNumber = item.AppNumber;
                hospitalApplicationObj.AppTypeName = item.ApplicationType.Name;
                hospitalApplicationObj.AppTypeNameAr = item.ApplicationType.NameAr;
                hospitalApplicationObj.HospitalId = (int)item.AssetDetail.HospitalId;
                hospitalApplicationObj.Comment = item.Comment;
                if (item.AppTypeId == 1)
                {
                    hospitalApplicationObj.ReasonNames = execludNames;
                }
                else
                {
                    hospitalApplicationObj.HoldReasonNames = holdNames;
                }
                hospitalApplicationObj.AssetId = item.AssetDetail.Id;
                hospitalApplicationObj.assetName = item.AssetDetail.MasterAsset.Name + " - " + item.AssetDetail.SerialNumber;
                hospitalApplicationObj.assetNameAr = item.AssetDetail.MasterAsset.NameAr + " - " + item.AssetDetail.SerialNumber;
            }


            return hospitalApplicationObj;



        }

        public int GetAssetHospitalId(int assetId)
        {
            int hospitalId = 0;
            var hospitalAppObj = _context.HospitalApplications.Include(a => a.AssetDetail).Where(a => a.AssetId == assetId).FirstOrDefault();
            if (hospitalAppObj != null)
            {
                hospitalId = int.Parse(hospitalAppObj.AssetDetail.HospitalId.ToString());
            }

            return hospitalId;
        }

        public IEnumerable<IndexHospitalApplicationVM.GetData> GetAllByStatusId(int statusId, int hospitalId)
        {
            List<IndexHospitalApplicationVM.GetData> list = new List<IndexHospitalApplicationVM.GetData>();
            var lstHospitalApplications = _context.HospitalApplications.Include(a => a.ApplicationType).Include(a => a.User).Include(a => a.AssetDetail).Include(a => a.AssetDetail.MasterAsset).ToList();

            if (statusId != 0)
            {
                lstHospitalApplications = lstHospitalApplications.Where(a => a.StatusId == statusId).ToList();
            }
            if (hospitalId != 0)
            {
                lstHospitalApplications = lstHospitalApplications.Where(a => a.AssetDetail.HospitalId == hospitalId).ToList();
            }

            foreach (var item in lstHospitalApplications)
            {

                IndexHospitalApplicationVM.GetData getDataObj = new IndexHospitalApplicationVM.GetData();
                getDataObj.Id = item.Id;
                getDataObj.AppNumber = item.AppNumber;
                getDataObj.Date = item.AppDate.Value.ToShortDateString();
                getDataObj.DueDate = item.DueDate != null ? item.DueDate.Value.ToShortDateString() : "";
                getDataObj.AppTypeId = item.AppTypeId;
                getDataObj.UserName = item.User.UserName;
                getDataObj.AssetName = item.AssetDetail.MasterAsset.Name + " - " + item.AssetDetail.SerialNumber;
                getDataObj.AssetNameAr = item.AssetDetail.MasterAsset.NameAr + " - " + item.AssetDetail.SerialNumber;
                getDataObj.TypeName = item.ApplicationType.Name;
                getDataObj.TypeNameAr = item.ApplicationType.NameAr;

                getDataObj.DiffMonths = ((item.AppDate.Value.Year - DateTime.Today.Date.Year) * 12) + item.AppDate.Value.Month - DateTime.Today.Date.Month;
                getDataObj.IsMoreThan3Months = getDataObj.DiffMonths <= -3 ? true : false;




                getDataObj.StatusId = item.StatusId;

                foreach (var itm in lstHospitalApplications)
                {
                    if (item.StatusId == 1)
                    {

                        //  lstHospitalApplications = lstHospitalApplications.Where(a => a.StatusId == itm.StatusId).ToList();
                        getDataObj.OpenStatus = lstHospitalApplications.Count;
                        getDataObj.StatusName = _context.HospitalSupplierStatuses.FirstOrDefault(a => a.Id == itm.StatusId).Name;
                        getDataObj.StatusNameAr = _context.HospitalSupplierStatuses.FirstOrDefault(a => a.Id == itm.StatusId).NameAr;
                    }
                    if (item.StatusId == 2)
                    {
                        getDataObj.StatusName = _context.HospitalSupplierStatuses.FirstOrDefault(a => a.Id == itm.StatusId).Name;
                        getDataObj.StatusNameAr = _context.HospitalSupplierStatuses.FirstOrDefault(a => a.Id == itm.StatusId).NameAr;
                        // lstHospitalApplications = lstHospitalApplications.Where(a => a.StatusId == itm.StatusId).ToList();
                        getDataObj.ApproveStatus = lstHospitalApplications.Count;
                    }
                    if (item.StatusId == 3)
                    {
                        //  lstHospitalApplications = lstHospitalApplications.Where(a => a.StatusId == itm.StatusId).ToList();
                        getDataObj.StatusName = _context.HospitalSupplierStatuses.FirstOrDefault(a => a.Id == itm.StatusId).Name;
                        getDataObj.StatusNameAr = _context.HospitalSupplierStatuses.FirstOrDefault(a => a.Id == itm.StatusId).NameAr;

                        getDataObj.RejectStatus = lstHospitalApplications.Count;
                    }
                    if (item.StatusId == 4)
                    {
                        getDataObj.StatusName = _context.HospitalSupplierStatuses.FirstOrDefault(a => a.Id == itm.StatusId).Name;
                        getDataObj.StatusNameAr = _context.HospitalSupplierStatuses.FirstOrDefault(a => a.Id == itm.StatusId).NameAr;
                        //   lstHospitalApplications = lstHospitalApplications.Where(a => a.StatusId == itm.StatusId).ToList();
                        getDataObj.SystemRejectStatus = lstHospitalApplications.Count;
                    }
                }

                var ReasonExTitles = (from execlude in _context.HospitalExecludeReasons
                                      join trans in _context.HospitalReasonTransactions on execlude.Id equals trans.ReasonId
                                      where trans.HospitalApplicationId == item.Id
                                      && item.AppTypeId == 1
                                      select execlude).ToList();
                if (ReasonExTitles.Count > 0)
                {
                    List<string> execludeNames = new List<string>();// { "John", "Anna", "Monica" };
                    foreach (var reason in ReasonExTitles)
                    {
                        execludeNames.Add(reason.Name);
                    }

                    getDataObj.ReasonExTitles = string.Join(",", execludeNames);


                    List<string> execludeNamesAr = new List<string>();
                    foreach (var reason in ReasonExTitles)
                    {
                        execludeNamesAr.Add(reason.NameAr);
                    }
                    getDataObj.ReasonExTitlesAr = string.Join(",", execludeNamesAr);

                }

                var ReasonHoldTitles = (from execlude in _context.HospitalHoldReasons
                                        join trans in _context.HospitalReasonTransactions on execlude.Id equals trans.ReasonId
                                        where trans.HospitalApplicationId == item.Id
                                        && item.AppTypeId == 2
                                        select execlude).ToList();
                if (ReasonHoldTitles.Count > 0)
                {
                    List<string> holdNames = new List<string>();
                    foreach (var reason in ReasonHoldTitles)
                    {
                        holdNames.Add(reason.Name);
                    }
                    getDataObj.ReasonHoldTitles = string.Join(",", holdNames);

                    List<string> holdNamesAr = new List<string>();
                    foreach (var reason in ReasonHoldTitles)
                    {
                        holdNamesAr.Add(reason.NameAr);
                    }
                    getDataObj.ReasonHoldTitlesAr = string.Join(",", holdNamesAr);
                }


                list.Add(getDataObj);
            }

            return list;
        }

        public IEnumerable<IndexHospitalApplicationVM.GetData> SortHospitalApp(SortHospitalApplication sortObj)
        {
            var list = GetAll();
            if (sortObj.AssetName != "")
            {
                if (sortObj.SortStatus == "descending")
                    list = list.OrderByDescending(d => d.AssetName).ToList();
                else
                    list = list.OrderBy(d => d.AssetName).ToList();
            }
            else if (sortObj.AssetNameAr != "")
            {
                if (sortObj.SortStatus == "descending")
                    list = list.OrderByDescending(d => d.AssetNameAr).ToList();
                else
                    list = list.OrderBy(d => d.AssetNameAr).ToList();
            }
            else if (sortObj.AssetName != "")
            {
                if (sortObj.SortStatus == "descending")
                    list = list.OrderByDescending(d => d.AssetName).ToList();
                else
                    list = list.OrderBy(d => d.AssetName).ToList();
            }
            else if (sortObj.TypeName != "")
            {
                if (sortObj.SortStatus == "descending")
                    list = list.OrderByDescending(d => d.TypeName).ToList();
                else
                    list = list.OrderBy(d => d.TypeName).ToList();
            }
            else if (sortObj.TypeNameAr != "")
            {
                if (sortObj.SortStatus == "descending")
                    list = list.OrderByDescending(d => d.TypeNameAr).ToList();
                else
                    list = list.OrderBy(d => d.TypeNameAr).ToList();
            }
            else if (sortObj.ReasonExTitles != "" || sortObj.ReasonHoldTitles != "")
            {
                if (sortObj.SortStatus == "descending")
                    list = list.OrderByDescending(d => d.ReasonExTitles).ThenByDescending(d => d.ReasonHoldTitles).ToList();
                else
                    list = list.OrderBy(d => d.ReasonExTitles).ThenBy(d => d.ReasonHoldTitles).ToList();
            }
            else if (sortObj.ReasonExTitlesAr != "" || sortObj.ReasonHoldTitlesAr != "")
            {
                if (sortObj.SortStatus == "descending")
                    list = list.OrderByDescending(d => d.ReasonExTitlesAr).ThenByDescending(d => d.ReasonHoldTitlesAr).ToList();
                else
                    list = list.OrderBy(d => d.ReasonExTitlesAr).ThenBy(d => d.ReasonHoldTitlesAr).ToList();
            }
            else if (sortObj.Date != "")
            {
                if (sortObj.SortStatus == "descending")
                    list = list.OrderByDescending(d => d.Date).ToList();
                else
                    list = list.OrderBy(d => d.Date).ToList();
            }
            else if (sortObj.StatusName != "")
            {
                if (sortObj.SortStatus == "descending")
                    list = list.OrderByDescending(d => d.StatusName).ToList();
                else
                    list = list.OrderBy(d => d.StatusName).ToList();
            }
            else if (sortObj.StatusNameAr != "")
            {
                if (sortObj.SortStatus == "descending")
                    list = list.OrderByDescending(d => d.StatusNameAr).ToList();
                else
                    list = list.OrderBy(d => d.StatusNameAr).ToList();
            }
            else if (sortObj.DueDate != "")
            {
                if (sortObj.SortStatus == "descending")
                    list = list.OrderByDescending(d => d.DueDate).ToList();
                else
                    list = list.OrderBy(d => d.DueDate).ToList();
            }
            else if (sortObj.AppNumber != "")
            {
                if (sortObj.SortStatus == "descending")
                    list = list.OrderByDescending(d => d.AppNumber).ToList();
                else
                    list = list.OrderBy(d => d.AppNumber).ToList();
            }
            return list;
        }

        public IEnumerable<IndexHospitalApplicationVM.GetData> GetAllByHospitalId(int hospitalId)
        {
            List<IndexHospitalApplicationVM.GetData> list = new List<IndexHospitalApplicationVM.GetData>();
            var lstHospitalApplications = _context.HospitalApplications.Include(a => a.ApplicationType).Include(a => a.User)
                .Include(a => a.AssetDetail).Include(a => a.AssetDetail.MasterAsset).ToList().OrderByDescending(a => a.AppDate).ToList();
            foreach (var item in lstHospitalApplications)
            {

                IndexHospitalApplicationVM.GetData getDataObj = new IndexHospitalApplicationVM.GetData();
                getDataObj.Id = item.Id;
                getDataObj.AppNumber = item.AppNumber;
                getDataObj.Date = item.AppDate.Value.ToShortDateString();
                getDataObj.DueDate = item.DueDate != null ? item.DueDate.Value.ToShortDateString() : "";
                getDataObj.AppTypeId = item.AppTypeId;
                getDataObj.UserName = item.User.UserName;
                getDataObj.AssetName = item.AssetDetail.MasterAsset.Name + " - " + item.AssetDetail.SerialNumber;
                getDataObj.AssetNameAr = item.AssetDetail.MasterAsset.NameAr + " - " + item.AssetDetail.SerialNumber;
                getDataObj.TypeName = item.ApplicationType.Name;
                getDataObj.TypeNameAr = item.ApplicationType.NameAr;

                getDataObj.DiffMonths = ((item.AppDate.Value.Year - DateTime.Today.Date.Year) * 12) + item.AppDate.Value.Month - DateTime.Today.Date.Month;
                getDataObj.IsMoreThan3Months = getDataObj.DiffMonths <= -3 ? true : false;
                getDataObj.HospitalId = item.AssetDetail.HospitalId;

                var lstStatuses = _context.HospitalSupplierStatuses.Where(a => a.Id == item.StatusId).ToList();
                if (lstStatuses.Count > 0)
                {
                    getDataObj.StatusName = lstStatuses[0].Name;
                    getDataObj.StatusNameAr = lstStatuses[0].NameAr;
                }

                //getDataObj.StatusId = item.StatusId;
                //if (item.StatusId == 1)
                //{
                //    getDataObj.StatusName = "Open";
                //    getDataObj.StatusNameAr = "فتح";
                //}
                //if (item.StatusId == 2)
                //{
                //    getDataObj.StatusName = "Approved";
                //    getDataObj.StatusNameAr = "موافقة";
                //}
                //if (item.StatusId == 3)
                //{
                //    getDataObj.StatusName = "Rejected";
                //    getDataObj.StatusNameAr = "رفض الطلب";
                //}
                //if (item.StatusId == 4)
                //{
                //    getDataObj.StatusName = "System Rejected";
                //    getDataObj.StatusNameAr = "استبعاد من النظام";
                //}

                var ReasonExTitles = (from execlude in _context.HospitalExecludeReasons
                                      join trans in _context.HospitalReasonTransactions on execlude.Id equals trans.ReasonId
                                      where trans.HospitalApplicationId == item.Id
                                      && item.AppTypeId == 1
                                      select execlude).ToList();
                if (ReasonExTitles.Count > 0)
                {
                    List<string> execludeNames = new List<string>();// { "John", "Anna", "Monica" };
                    foreach (var reason in ReasonExTitles)
                    {
                        execludeNames.Add(reason.Name);
                    }

                    getDataObj.ReasonExTitles = string.Join(",", execludeNames);


                    List<string> execludeNamesAr = new List<string>();
                    foreach (var reason in ReasonExTitles)
                    {
                        execludeNamesAr.Add(reason.NameAr);
                    }
                    getDataObj.ReasonExTitlesAr = string.Join(",", execludeNamesAr);

                }

                var ReasonHoldTitles = (from execlude in _context.HospitalHoldReasons
                                        join trans in _context.HospitalReasonTransactions on execlude.Id equals trans.ReasonId
                                        where trans.HospitalApplicationId == item.Id
                                        && item.AppTypeId == 2
                                        select execlude).ToList();
                if (ReasonHoldTitles.Count > 0)
                {
                    List<string> holdNames = new List<string>();
                    foreach (var reason in ReasonHoldTitles)
                    {
                        holdNames.Add(reason.Name);
                    }
                    getDataObj.ReasonHoldTitles = string.Join(",", holdNames);

                    List<string> holdNamesAr = new List<string>();
                    foreach (var reason in ReasonHoldTitles)
                    {
                        holdNamesAr.Add(reason.NameAr);
                    }
                    getDataObj.ReasonHoldTitlesAr = string.Join(",", holdNamesAr);
                }


                list.Add(getDataObj);
            }
            if (hospitalId == 0)
            {
                list = list.ToList();
            }
            else
            {
                list = list.Where(a => a.HospitalId == hospitalId).ToList();
            }

            return list;
        }

        public IEnumerable<IndexHospitalApplicationVM.GetData> GetAllByAppTypeId(int appTypeId)
        {
            List<IndexHospitalApplicationVM.GetData> list = new List<IndexHospitalApplicationVM.GetData>();
            var lstHospitalApplications = _context.HospitalApplications.Include(a => a.ApplicationType).Include(a => a.User)
                .Include(a => a.AssetDetail).Include(a => a.AssetDetail.MasterAsset).ToList().OrderByDescending(a => a.AppDate.Value.Date).ToList();

            if (appTypeId != 0)
                lstHospitalApplications = lstHospitalApplications.Where(a => a.AppTypeId == appTypeId).ToList();


            foreach (var item in lstHospitalApplications)
            {

                IndexHospitalApplicationVM.GetData getDataObj = new IndexHospitalApplicationVM.GetData();
                getDataObj.Id = item.Id;
                getDataObj.AppNumber = item.AppNumber;
                getDataObj.Date = item.AppDate.Value.ToShortDateString();
                getDataObj.DueDate = item.DueDate != null ? item.DueDate.Value.ToShortDateString() : "";
                getDataObj.AppTypeId = item.AppTypeId;
                getDataObj.UserName = item.User.UserName;
                getDataObj.AssetName = item.AssetDetail.MasterAsset.Name + " - " + item.AssetDetail.SerialNumber;
                getDataObj.AssetNameAr = item.AssetDetail.MasterAsset.NameAr + " - " + item.AssetDetail.SerialNumber;
                getDataObj.TypeName = item.ApplicationType.Name;
                getDataObj.TypeNameAr = item.ApplicationType.NameAr;

                getDataObj.DiffMonths = ((item.AppDate.Value.Year - DateTime.Today.Date.Year) * 12) + item.AppDate.Value.Month - DateTime.Today.Date.Month;
                getDataObj.IsMoreThan3Months = getDataObj.DiffMonths <= -3 ? true : false;

                getDataObj.StatusId = item.StatusId;

                var lstStatuses = _context.HospitalSupplierStatuses.Where(a => a.Id == item.StatusId).ToList();
                if (lstStatuses.Count > 0)
                {
                    getDataObj.StatusName = lstStatuses[0].Name;
                    getDataObj.StatusNameAr = lstStatuses[0].NameAr;
                }




                var ReasonExTitles = (from execlude in _context.HospitalExecludeReasons
                                      join trans in _context.HospitalReasonTransactions on execlude.Id equals trans.ReasonId
                                      where trans.HospitalApplicationId == item.Id
                                      && item.AppTypeId == 1
                                      select execlude).ToList();
                if (ReasonExTitles.Count > 0)
                {
                    List<string> execludeNames = new List<string>();// { "John", "Anna", "Monica" };
                    foreach (var reason in ReasonExTitles)
                    {
                        execludeNames.Add(reason.Name);
                    }

                    getDataObj.ReasonExTitles = string.Join(",", execludeNames);


                    List<string> execludeNamesAr = new List<string>();
                    foreach (var reason in ReasonExTitles)
                    {
                        execludeNamesAr.Add(reason.NameAr);
                    }
                    getDataObj.ReasonExTitlesAr = string.Join(",", execludeNamesAr);

                }

                var ReasonHoldTitles = (from execlude in _context.HospitalHoldReasons
                                        join trans in _context.HospitalReasonTransactions on execlude.Id equals trans.ReasonId
                                        where trans.HospitalApplicationId == item.Id
                                        && item.AppTypeId == 2
                                        select execlude).ToList();
                if (ReasonHoldTitles.Count > 0)
                {
                    List<string> holdNames = new List<string>();
                    foreach (var reason in ReasonHoldTitles)
                    {
                        holdNames.Add(reason.Name);
                    }
                    getDataObj.ReasonHoldTitles = string.Join(",", holdNames);

                    List<string> holdNamesAr = new List<string>();
                    foreach (var reason in ReasonHoldTitles)
                    {
                        holdNamesAr.Add(reason.NameAr);
                    }
                    getDataObj.ReasonHoldTitlesAr = string.Join(",", holdNamesAr);
                }


                list.Add(getDataObj);
            }

            return list;
        }

        public IEnumerable<IndexHospitalApplicationVM.GetData> GetAllByAppTypeIdAndStatusId(int statusId, int appTypeId, int hospitalId)
        {
            List<IndexHospitalApplicationVM.GetData> list = new List<IndexHospitalApplicationVM.GetData>();
            var lstHospitalApplications = _context.HospitalApplications.Include(a => a.ApplicationType).Include(a => a.User)
                .Include(a => a.HospitalSupplierStatus).Include(a => a.ApplicationType)
                .Include(a => a.AssetDetail).Include(a => a.AssetDetail.Hospital).Include(a => a.AssetDetail.MasterAsset).ToList().OrderByDescending(a => a.AppDate.Value.Date).ToList();

            if (hospitalId != 0)
                lstHospitalApplications = lstHospitalApplications.Where(a => a.AssetDetail.HospitalId == hospitalId).ToList();



            if (appTypeId != 0)
                lstHospitalApplications = lstHospitalApplications.Where(a => a.AppTypeId == appTypeId).ToList();

            if (statusId != 0)
                lstHospitalApplications = lstHospitalApplications.Where(a => a.StatusId == statusId).ToList();



            foreach (var item in lstHospitalApplications)
            {

                IndexHospitalApplicationVM.GetData getDataObj = new IndexHospitalApplicationVM.GetData();
                getDataObj.Id = item.Id;
                getDataObj.AppNumber = item.AppNumber;
                getDataObj.Date = item.AppDate.Value.ToShortDateString();
                getDataObj.DueDate = item.DueDate != null ? item.DueDate.Value.ToShortDateString() : "";
                getDataObj.AppTypeId = item.AppTypeId;
                getDataObj.UserName = item.User.UserName;
                getDataObj.AssetName = item.AssetDetail.MasterAsset.Name + " - " + item.AssetDetail.SerialNumber;
                getDataObj.AssetNameAr = item.AssetDetail.MasterAsset.NameAr + " - " + item.AssetDetail.SerialNumber;
                getDataObj.TypeName = item.ApplicationType.Name;
                getDataObj.TypeNameAr = item.ApplicationType.NameAr;

                getDataObj.DiffMonths = ((item.AppDate.Value.Year - DateTime.Today.Date.Year) * 12) + item.AppDate.Value.Month - DateTime.Today.Date.Month;
                getDataObj.IsMoreThan3Months = getDataObj.DiffMonths <= -3 ? true : false;

                getDataObj.StatusId = item.StatusId;
                getDataObj.StatusName = item.HospitalSupplierStatus.Name;
                getDataObj.StatusNameAr = item.HospitalSupplierStatus.NameAr;

                if (appTypeId == 1)
                {

                    var ReasonExTitles = (from execlude in _context.HospitalExecludeReasons
                                          join trans in _context.HospitalReasonTransactions on execlude.Id equals trans.ReasonId
                                          where trans.HospitalApplicationId == item.Id
                                          && item.AppTypeId == 1
                                          select execlude).ToList();
                    if (ReasonExTitles.Count > 0)
                    {
                        List<string> execludeNames = new List<string>();// { "John", "Anna", "Monica" };
                        foreach (var reason in ReasonExTitles)
                        {
                            execludeNames.Add(reason.Name);
                        }

                        getDataObj.ReasonExTitles = string.Join(",", execludeNames);


                        List<string> execludeNamesAr = new List<string>();
                        foreach (var reason in ReasonExTitles)
                        {
                            execludeNamesAr.Add(reason.NameAr);
                        }
                        getDataObj.ReasonExTitlesAr = string.Join(",", execludeNamesAr);

                    }
                }
                if (appTypeId == 2)
                {
                    var ReasonHoldTitles = (from execlude in _context.HospitalHoldReasons
                                            join trans in _context.HospitalReasonTransactions on execlude.Id equals trans.ReasonId
                                            where trans.HospitalApplicationId == item.Id
                                            && item.AppTypeId == 2
                                            select execlude).ToList();
                    if (ReasonHoldTitles.Count > 0)
                    {
                        List<string> holdNames = new List<string>();
                        foreach (var reason in ReasonHoldTitles)
                        {
                            holdNames.Add(reason.Name);
                        }
                        getDataObj.ReasonHoldTitles = string.Join(",", holdNames);

                        List<string> holdNamesAr = new List<string>();
                        foreach (var reason in ReasonHoldTitles)
                        {
                            holdNamesAr.Add(reason.NameAr);
                        }
                        getDataObj.ReasonHoldTitlesAr = string.Join(",", holdNamesAr);
                    }
                }

                list.Add(getDataObj);
            }

            return list;
        }
    }
}