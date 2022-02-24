using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.SupplierExecludeAssetVM;
using Asset.ViewModels.RoleCategoryVM;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Asset.Core.Repositories
{
    public class SupplierExecludeAssetRepositories : ISupplierExecludeAssetRepository
    {
        private ApplicationDbContext _context;

        public SupplierExecludeAssetRepositories(ApplicationDbContext context)
        {
            _context = context;
        }


        public EditSupplierExecludeAssetVM GetById(int id)
        {



            var reasonIds = (from execlude in _context.SupplierExecludeReasons
                             join trans in _context.SupplierExecludes on execlude.Id equals trans.ReasonId
                             where trans.SupplierExecludeAssetId == id
                              && trans.SupplierExecludeAsset.AppTypeId == 1
                             select execlude.Id).ToList();

            var holdIds = (from execlude in _context.SupplierHoldReasons
                           join trans in _context.SupplierExecludes on execlude.Id equals trans.ReasonId
                           where trans.SupplierExecludeAssetId == id
                           && trans.SupplierExecludeAsset.AppTypeId == 2
                           select execlude.Id).ToList();

            return _context.SupplierExecludeAssets.Include(a => a.User).Include(a => a.AssetDetail).Include(a => a.AssetDetail.MasterAsset).Where(a => a.Id == id).Select(item => new EditSupplierExecludeAssetVM
            {
                Id = item.Id,
                AssetId = item.AssetId,
                AppTypeId = item.AppTypeId,
                StatusId = item.StatusId,
                Date = item.Date,
                ExecludeDate = item.ExecludeDate,
                ExNumber = item.ExNumber,
                UserId = item.User.UserName,
                ReasonIds = reasonIds,
                HoldReasonIds = holdIds,
                Comment = item.Comment,
                assetName = item.AssetDetail.MasterAsset.Name + " - " + item.AssetDetail.SerialNumber,
                assetNameAr = item.AssetDetail.MasterAsset.NameAr + " - " + item.AssetDetail.SerialNumber

            }).FirstOrDefault();

        }

        public IEnumerable<IndexSupplierExecludeAssetVM.GetData> GetAll()
        {
            List<IndexSupplierExecludeAssetVM.GetData> list = new List<IndexSupplierExecludeAssetVM.GetData>();
            var lstSupplierExecludeAssets = _context.SupplierExecludeAssets.Include(a => a.User)
                .Include(a => a.AssetDetail).Include(a => a.ApplicationType)
                .Include(a => a.HospitalSupplierStatus)
                .Include(a => a.AssetDetail.MasterAsset).OrderByDescending(a => a.Date.Value.Date).ToList();
            foreach (var item in lstSupplierExecludeAssets)
            {

                IndexSupplierExecludeAssetVM.GetData getDataObj = new IndexSupplierExecludeAssetVM.GetData();
                getDataObj.Id = item.Id;
                getDataObj.ExNumber = item.ExNumber;
                getDataObj.AppTypeId = item.AppTypeId;
                getDataObj.Date = item.Date != null ? item.Date.Value.ToShortDateString() : "";
                getDataObj.ExecludeDate = item.ExecludeDate != null ? item.ExecludeDate.Value.ToShortDateString() : "";
                getDataObj.UserName = item.User.UserName;
                getDataObj.AssetName = item.AssetDetail.MasterAsset.Name + " - " + item.AssetDetail.SerialNumber;
                getDataObj.AssetNameAr = item.AssetDetail.MasterAsset.NameAr + " - " + item.AssetDetail.SerialNumber;
                getDataObj.DiffMonths = ((item.Date.Value.Year - DateTime.Today.Date.Year) * 12) + item.Date.Value.Month - DateTime.Today.Date.Month;
                getDataObj.IsMoreThan3Months = getDataObj.DiffMonths <= -3 ? true : false;
                getDataObj.StatusId = item.StatusId;

                var lstStatuses = _context.HospitalSupplierStatuses.Where(a => a.Id == item.StatusId).ToList();
                if (lstStatuses.Count > 0)
                {
                    getDataObj.StatusName = lstStatuses[0].Name;
                    getDataObj.StatusNameAr = lstStatuses[0].NameAr;
                }

                if (item.AppTypeId == 1)
                {
                    var lstExTitles = (from execlude in _context.SupplierExecludeReasons
                                       join trans in _context.SupplierExecludes on execlude.Id equals trans.ReasonId
                                       where trans.SupplierExecludeAssetId == item.Id
                                         && trans.SupplierExecludeAsset.AppTypeId == 1
                                       select execlude).ToList();
                    if (lstExTitles.Count > 0)
                    {
                        List<string> execludeNames = new List<string>();
                        List<string> execludeNamesAr = new List<string>();
                        foreach (var reason in lstExTitles)
                        {
                            execludeNames.Add(reason.Name);
                            execludeNamesAr.Add(reason.NameAr);
                        }
                        getDataObj.ReasonExTitles = string.Join(",", execludeNames);
                        getDataObj.ReasonExTitlesAr = string.Join(",", execludeNamesAr);

                    }
                }
                if (item.AppTypeId == 2)
                {
                    var lstHoldTitles = (from execlude in _context.SupplierHoldReasons
                                         join trans in _context.SupplierExecludes on execlude.Id equals trans.ReasonId
                                         where trans.SupplierExecludeAssetId == item.Id
                                           && trans.SupplierExecludeAsset.AppTypeId == 2
                                         select execlude).ToList();
                    if (lstHoldTitles.Count > 0)
                    {
                        List<string> holdNames = new List<string>();
                        List<string> holdNamesAr = new List<string>();
                        foreach (var reason in lstHoldTitles)
                        {
                            holdNames.Add(reason.Name);
                            holdNamesAr.Add(reason.NameAr);
                        }
                        getDataObj.ReasonHoldTitles = string.Join(",", holdNames);
                        getDataObj.ReasonHoldTitlesAr = string.Join(",", holdNamesAr);
                    }
                }

                list.Add(getDataObj);
            }

            return list;
        }

        public int Add(CreateSupplierExecludeAssetVM model)
        {
            SupplierExecludeAsset supplierExecludeAssetObj = new SupplierExecludeAsset();
            try
            {
                if (supplierExecludeAssetObj != null)
                {
                    supplierExecludeAssetObj.Id = model.Id;
                    supplierExecludeAssetObj.AssetId = model.AssetId;
                    supplierExecludeAssetObj.StatusId = 1;
                    supplierExecludeAssetObj.AppTypeId = model.AppTypeId;
                    supplierExecludeAssetObj.Comment = model.Comment;

                    supplierExecludeAssetObj.Date = DateTime.Today.Date;
                    if (model.ExecludeDate != "")
                        supplierExecludeAssetObj.ExecludeDate = DateTime.Parse(model.ExecludeDate.ToString());
                    supplierExecludeAssetObj.ExNumber = model.ExNumber;
                    supplierExecludeAssetObj.UserId = model.UserId;
                    _context.SupplierExecludeAssets.Add(supplierExecludeAssetObj);
                    _context.SaveChanges();
                    int id = supplierExecludeAssetObj.Id;
                    if (model.ReasonIds.Count > 0)
                    {
                        foreach (var reasonId in model.ReasonIds)
                        {
                            SupplierExeclude supplierExecludeObj = new SupplierExeclude();
                            supplierExecludeObj.SupplierExecludeAssetId = id;
                            supplierExecludeObj.ReasonId = reasonId;
                            _context.SupplierExecludes.Add(supplierExecludeObj);
                            _context.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return supplierExecludeAssetObj.Id;
        }

        public int Delete(int id)
        {
            var SupplierExecludeAssetObj = _context.SupplierExecludeAssets.Find(id);
            try
            {
                if (SupplierExecludeAssetObj != null)
                {
                    _context.SupplierExecludeAssets.Remove(SupplierExecludeAssetObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }

            return 0;
        }

        public int Update(EditSupplierExecludeAssetVM model)
        {
            try
            {
                var supplierExecludeAssetObj = _context.SupplierExecludeAssets.Find(model.Id);
                supplierExecludeAssetObj.Id = model.Id;
                supplierExecludeAssetObj.AssetId = model.AssetId;
                supplierExecludeAssetObj.StatusId = model.StatusId;
                supplierExecludeAssetObj.Date = model.Date;
                supplierExecludeAssetObj.ExecludeDate = model.ExecludeDate;
                supplierExecludeAssetObj.ExNumber = model.ExNumber;
                supplierExecludeAssetObj.UserId = model.UserId;
                supplierExecludeAssetObj.Comment = model.Comment;
                _context.Entry(supplierExecludeAssetObj).State = EntityState.Modified;
                _context.SaveChanges();

                if (model.ReasonIds.Count > 0)
                {


                    List<int> reasonIds = new List<int>();
                    var savedReasonIds = (from execlude in _context.SupplierExecludeReasons
                                          join trans in _context.SupplierExecludes on execlude.Id equals trans.ReasonId
                                          where trans.SupplierExecludeAssetId == model.Id
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
                            var row = _context.SupplierExecludes.Where(a => a.SupplierExecludeAssetId == model.Id && a.ReasonId == item).ToList();
                            if (row.Count > 0)
                            {
                                var reasonObj = row[0];
                                _context.SupplierExecludes.Remove(reasonObj);
                                _context.SaveChanges();
                            }
                        }
                    }
                    var neewIds = model.ReasonIds.Except(reasonIds);
                    if (neewIds.Count() > 0)
                    {
                        foreach (var itm in neewIds)
                        {
                            SupplierExeclude supplierExecludeObj = new SupplierExeclude();
                            supplierExecludeObj.SupplierExecludeAssetId = model.Id;
                            supplierExecludeObj.ReasonId = itm;
                            _context.SupplierExecludes.Add(supplierExecludeObj);
                            _context.SaveChanges();
                        }
                    }



                    return supplierExecludeAssetObj.Id;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return 0;
        }

        public int CreateSupplierExecludAttachments(SupplierExecludeAttachment attachObj)
        {
            SupplierExecludeAttachment assetAttachmentObj = new SupplierExecludeAttachment();
            assetAttachmentObj.SupplierExecludeAssetId = attachObj.SupplierExecludeAssetId;
            assetAttachmentObj.Title = attachObj.Title;
            assetAttachmentObj.FileName = attachObj.FileName;
            _context.SupplierExecludeAttachments.Add(assetAttachmentObj);
            _context.SaveChanges();
            int Id = assetAttachmentObj.Id;
            return Id;
        }

        public IEnumerable<SupplierExecludeAttachment> GetAttachmentBySupplierExecludeAssetId(int assetId)
        {
            return _context.SupplierExecludeAttachments.Where(a => a.SupplierExecludeAssetId == assetId).ToList();
        }

        public int DeleteSupplierExecludeAttachment(int id)
        {
            try
            {
                var attachObj = _context.SupplierExecludeAttachments.Find(id);
                _context.SupplierExecludeAttachments.Remove(attachObj);
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return 0;

        }

        public int UpdateExcludedDate(EditSupplierExecludeAssetVM model)
        {
            try
            {
                var supplierExecludeAssetObj = _context.SupplierExecludeAssets.Find(model.Id);
                supplierExecludeAssetObj.Id = model.Id;
                supplierExecludeAssetObj.StatusId = model.StatusId;
                if (model.StatusId == 2)
                    supplierExecludeAssetObj.ExecludeDate = model.ExecludeDate;
                if (model.StatusId == 3)
                    supplierExecludeAssetObj.ExecludeDate = DateTime.Today.Date;

                supplierExecludeAssetObj.UserId = model.UserId;
                supplierExecludeAssetObj.Comment = model.Comment;

                _context.Entry(supplierExecludeAssetObj).State = EntityState.Modified;
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
                return supplierExecludeAssetObj.Id;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return 0;
        }

        public ViewSupplierExecludeAssetVM GetSupplierExecludeAssetDetailById(int id)
        {


            var reasonNames = (from execlude in _context.SupplierExecludeReasons
                               join trans in _context.SupplierExecludes on execlude.Id equals trans.ReasonId
                               where trans.SupplierExecludeAssetId == id
                               && trans.SupplierExecludeAsset.AppTypeId == 1
                               select execlude).ToList();


            var holdReasonNames = (from execlude in _context.SupplierHoldReasons
                                   join trans in _context.SupplierExecludes on execlude.Id equals trans.ReasonId
                                   where trans.SupplierExecludeAssetId == id
                                        && trans.SupplierExecludeAsset.AppTypeId == 2
                                   select execlude).ToList();




            return _context.SupplierExecludeAssets.Include(a => a.User).Include(a => a.AssetDetail)
                                        .Include(a => a.AssetDetail.Hospital)
                                        .Include(a => a.AssetDetail.Supplier)
                                        .Include(a => a.AssetDetail.MasterAsset).Where(a => a.Id == id).Select(item => new ViewSupplierExecludeAssetVM
                                        {
                                            Id = item.Id,
                                            AssetId = item.AssetId,
                                            StatusId = item.StatusId,
                                            Date = item.Date,
                                            ExecludeDate = item.ExecludeDate,
                                            Comment = item.Comment,
                                            ExNumber = item.ExNumber,
                                            UserId = item.User.UserName,
                                            ReasonNames = reasonNames,
                                            HoldReasonNames = holdReasonNames,
                                            assetName = item.AssetDetail.MasterAsset.Name + " - " + item.AssetDetail.SerialNumber,
                                            assetNameAr = item.AssetDetail.MasterAsset.NameAr + " - " + item.AssetDetail.SerialNumber,


                                            HospitalName = item.AssetDetail.Hospital.Name,
                                            HospitalNameAr = item.AssetDetail.Hospital.NameAr,

                                            GovName = item.AssetDetail.Hospital.Governorate.Name,
                                            GovNameAr = item.AssetDetail.Hospital.Governorate.NameAr,


                                            CityName = item.AssetDetail.Hospital.City.Name,
                                            CityNameAr = item.AssetDetail.Hospital.City.NameAr,

                                            OrgName = item.AssetDetail.Hospital.Organization.Name,
                                            OrgNameAr = item.AssetDetail.Hospital.Organization.NameAr,

                                            SubOrgName = item.AssetDetail.Hospital.SubOrganization.Name,
                                            SubOrgNameAr = item.AssetDetail.Hospital.SubOrganization.NameAr,


                                            appTypeName = item.ApplicationType.Name,
                                            appTypeNameAr = item.ApplicationType.NameAr,

                                        }).FirstOrDefault();

        }

        public IEnumerable<IndexSupplierExecludeAssetVM.GetData> GetAllByStatusId(int statusId)
        {
            List<IndexSupplierExecludeAssetVM.GetData> list = new List<IndexSupplierExecludeAssetVM.GetData>();
            var lstSupplierExecludeAssets = _context.SupplierExecludeAssets.Include(a => a.User)
                     .Include(a => a.ApplicationType)
                .Include(a => a.HospitalSupplierStatus)
                .Include(a => a.AssetDetail).Include(a => a.AssetDetail.MasterAsset).ToList().OrderByDescending(a => a.Date.Value.Date).ToList();
            if (statusId != 0)
            {
                lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.StatusId == statusId).ToList();
            }
            foreach (var item in lstSupplierExecludeAssets)
            {

                IndexSupplierExecludeAssetVM.GetData getDataObj = new IndexSupplierExecludeAssetVM.GetData();
                getDataObj.Id = item.Id;
                getDataObj.ExNumber = item.ExNumber;
                getDataObj.AppTypeId = item.AppTypeId;
                getDataObj.Comment = item.Comment;
                getDataObj.Date = item.Date != null ? item.Date.Value.ToShortDateString() : "";
                getDataObj.ExecludeDate = item.ExecludeDate != null ? item.ExecludeDate.Value.ToShortDateString() : "";
                getDataObj.UserName = item.User.UserName;
                getDataObj.AssetName = item.AssetDetail.MasterAsset.Name + " - " + item.AssetDetail.SerialNumber;
                getDataObj.AssetNameAr = item.AssetDetail.MasterAsset.NameAr + " - " + item.AssetDetail.SerialNumber;
                getDataObj.DiffMonths = ((item.Date.Value.Year - DateTime.Today.Date.Year) * 12) + item.Date.Value.Month - DateTime.Today.Date.Month;
                getDataObj.IsMoreThan3Months = getDataObj.DiffMonths <= -3 ? true : false;
                getDataObj.StatusId = item.StatusId;

                var lstStatuses = _context.HospitalSupplierStatuses.Where(a => a.Id == item.StatusId).ToList();
                if (lstStatuses.Count > 0)
                {
                    getDataObj.StatusName = lstStatuses[0].Name;
                    getDataObj.StatusNameAr = lstStatuses[0].NameAr;
                }
                if (item.AppTypeId == 1)
                {
                    var lstExTitles = (from execlude in _context.SupplierExecludeReasons
                                       join trans in _context.SupplierExecludes on execlude.Id equals trans.ReasonId
                                       where trans.SupplierExecludeAssetId == item.Id
                                         && trans.SupplierExecludeAsset.AppTypeId == 1
                                       select execlude).ToList();
                    if (lstExTitles.Count > 0)
                    {
                        List<string> execludeNames = new List<string>();
                        List<string> execludeNamesAr = new List<string>();
                        foreach (var reason in lstExTitles)
                        {
                            execludeNames.Add(reason.Name);
                            execludeNamesAr.Add(reason.NameAr);
                        }
                        getDataObj.ReasonExTitles = string.Join(",", execludeNames);
                        getDataObj.ReasonExTitlesAr = string.Join(",", execludeNamesAr);

                    }
                }
                if (item.AppTypeId == 2)
                {
                    var lstHoldTitles = (from execlude in _context.SupplierHoldReasons
                                         join trans in _context.SupplierExecludes on execlude.Id equals trans.ReasonId
                                         where trans.SupplierExecludeAssetId == item.Id
                                           && trans.SupplierExecludeAsset.AppTypeId == 2
                                         select execlude).ToList();
                    if (lstHoldTitles.Count > 0)
                    {
                        List<string> holdNames = new List<string>();
                        List<string> holdNamesAr = new List<string>();
                        foreach (var reason in lstHoldTitles)
                        {
                            holdNames.Add(reason.Name);
                            holdNamesAr.Add(reason.NameAr);
                        }
                        getDataObj.ReasonHoldTitles = string.Join(",", holdNames);
                        getDataObj.ReasonHoldTitlesAr = string.Join(",", holdNamesAr);
                    }
                }

                list.Add(getDataObj);
            }

            return list;
        }

        public IEnumerable<IndexSupplierExecludeAssetVM.GetData> SortSuplierApp(SortSupplierExecludeAssetVM sortObj)
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


            else if (sortObj.ReasonExTitles != "" || sortObj.ReasonHoldTitles != "")
            {
                if (sortObj.SortStatus == "descending")
                    list = list.OrderByDescending(d => d.ReasonExTitles).ThenByDescending(d => d.ReasonExTitles).ToList();
                else
                    list = list.OrderBy(d => d.ReasonExTitles).ThenBy(d => d.ReasonExTitles).ToList();
            }
            else if (sortObj.ReasonExTitlesAr != "" || sortObj.ReasonHoldTitlesAr != "")
            {
                if (sortObj.SortStatus == "descending")
                    list = list.OrderByDescending(d => d.ReasonExTitlesAr).ThenByDescending(d => d.ReasonExTitlesAr).ToList();
                else
                    list = list.OrderBy(d => d.ReasonExTitlesAr).ThenBy(d => d.ReasonExTitlesAr).ToList();
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
                    list = list.OrderByDescending(d => d.ExecludeDate).ToList();
                else
                    list = list.OrderBy(d => d.ExecludeDate).ToList();
            }
            else if (sortObj.AppNumber != "")
            {
                if (sortObj.SortStatus == "descending")
                    list = list.OrderByDescending(d => d.ExNumber).ToList();
                else
                    list = list.OrderBy(d => d.ExNumber).ToList();
            }
            return list;
        }

        public IEnumerable<IndexSupplierExecludeAssetVM.GetData> GetAllByAppTypeId(int appTypeId)
        {
            List<IndexSupplierExecludeAssetVM.GetData> list = new List<IndexSupplierExecludeAssetVM.GetData>();
            var lstSupplierExecludeAssets = _context.SupplierExecludeAssets
                .Include(a => a.User)
                .Include(a => a.ApplicationType)
                .Include(a => a.HospitalSupplierStatus)
                .Include(a => a.AssetDetail)
                .Include(a => a.AssetDetail.MasterAsset).ToList().OrderByDescending(a => a.Date.Value.Date).ToList();
            if (appTypeId != 0)
            {
                lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.AppTypeId == appTypeId).ToList();
            }
            foreach (var item in lstSupplierExecludeAssets)
            {

                IndexSupplierExecludeAssetVM.GetData getDataObj = new IndexSupplierExecludeAssetVM.GetData();
                getDataObj.Id = item.Id;
                getDataObj.ExNumber = item.ExNumber;
                getDataObj.Comment = item.Comment;
                getDataObj.AppTypeId = item.AppTypeId;
                getDataObj.Date = item.Date != null ? item.Date.Value.ToShortDateString() : "";
                getDataObj.ExecludeDate = item.ExecludeDate != null ? item.ExecludeDate.Value.ToShortDateString() : "";
                getDataObj.UserName = item.User.UserName;
                getDataObj.AssetName = item.AssetDetail.MasterAsset != null ? item.AssetDetail.MasterAsset.Name + " - " + item.AssetDetail.SerialNumber : "";
                getDataObj.AssetNameAr = item.AssetDetail.MasterAsset != null ? item.AssetDetail.MasterAsset.NameAr + " - " + item.AssetDetail.SerialNumber : "";

                getDataObj.DiffMonths = ((item.Date.Value.Year - DateTime.Today.Date.Year) * 12) + item.Date.Value.Month - DateTime.Today.Date.Month;


                getDataObj.IsMoreThan3Months = getDataObj.DiffMonths <= -3 ? true : false;

                getDataObj.StatusId = item.StatusId;

                var lstStatuses = _context.HospitalSupplierStatuses.Where(a => a.Id == item.StatusId).ToList();
                if (lstStatuses.Count > 0)
                {
                    getDataObj.StatusName = lstStatuses[0].Name;
                    getDataObj.StatusNameAr = lstStatuses[0].NameAr;
                }
                if (appTypeId == 1)
                {
                    var lstExTitles = (from execlude in _context.SupplierExecludeReasons
                                       join trans in _context.SupplierExecludes on execlude.Id equals trans.ReasonId
                                       where trans.SupplierExecludeAssetId == item.Id
                                       && item.AppTypeId == 1
                                       select execlude).ToList();
                    if (lstExTitles.Count > 0)
                    {
                        List<string> execludeNames = new List<string>();
                        List<string> execludeNamesAr = new List<string>();
                        foreach (var reason in lstExTitles)
                        {
                            execludeNames.Add(reason.Name);
                            execludeNamesAr.Add(reason.NameAr);
                        }
                        getDataObj.ReasonExTitles = string.Join(",", execludeNames);
                        getDataObj.ReasonExTitlesAr = string.Join(",", execludeNamesAr);
                    }
                }
                if (appTypeId == 2)
                {
                    var lstHoldTitles = (from execlude in _context.SupplierHoldReasons
                                         join trans in _context.SupplierExecludes on execlude.Id equals trans.ReasonId
                                         where trans.SupplierExecludeAssetId == item.Id
                                            && item.AppTypeId == 2
                                         select execlude).ToList();
                    if (lstHoldTitles.Count > 0)
                    {
                        List<string> holdNames = new List<string>();
                        List<string> holdNamesAr = new List<string>();
                        foreach (var reason in lstHoldTitles)
                        {
                            holdNames.Add(reason.Name);
                            holdNamesAr.Add(reason.NameAr);
                        }
                        getDataObj.ReasonHoldTitles = string.Join(",", holdNames);
                        getDataObj.ReasonHoldTitlesAr = string.Join(",", holdNamesAr);
                    }
                }
                list.Add(getDataObj);
            }

            return list;
        }

        public IEnumerable<IndexSupplierExecludeAssetVM.GetData> GetAllByStatusIdAndAppTypeId(int statusId, int appTypeId)
        {
            List<IndexSupplierExecludeAssetVM.GetData> list = new List<IndexSupplierExecludeAssetVM.GetData>();
            var lstSupplierExecludeAssets = _context.SupplierExecludeAssets
                   .Include(a => a.HospitalSupplierStatus)
                       .Include(a => a.ApplicationType)
                .Include(a => a.User)
                .Include(a => a.AssetDetail)
                .Include(a => a.AssetDetail.MasterAsset).OrderByDescending(a => a.Date.Value.Date).ToList();

            if (statusId != 0)
                lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.StatusId == statusId).ToList();


            if (appTypeId != 0)
                lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.AppTypeId == appTypeId).ToList();



            foreach (var item in lstSupplierExecludeAssets)
            {

                IndexSupplierExecludeAssetVM.GetData getDataObj = new IndexSupplierExecludeAssetVM.GetData();
                getDataObj.Id = item.Id;
                getDataObj.AppTypeId = item.AppTypeId;
                getDataObj.ExNumber = item.ExNumber;
                getDataObj.Date = item.Date != null ? item.Date.Value.ToShortDateString() : "";
                getDataObj.ExecludeDate = item.ExecludeDate != null ? item.ExecludeDate.Value.ToShortDateString() : "";
                getDataObj.UserName = item.User.UserName;
                getDataObj.AssetName = item.AssetDetail.MasterAsset.Name + " - " + item.AssetDetail.SerialNumber;
                getDataObj.AssetNameAr = item.AssetDetail.MasterAsset.NameAr + " - " + item.AssetDetail.SerialNumber;
                getDataObj.DiffMonths = ((item.Date.Value.Year - DateTime.Today.Date.Year) * 12) + item.Date.Value.Month - DateTime.Today.Date.Month;
                getDataObj.IsMoreThan3Months = getDataObj.DiffMonths <= -3 ? true : false;
                getDataObj.StatusId = item.StatusId;
                getDataObj.StatusName = item.HospitalSupplierStatus.Name;
                getDataObj.StatusNameAr = item.HospitalSupplierStatus.NameAr;


                if (item.AppTypeId == 1)
                {
                    var lstExTitles = (from execlude in _context.SupplierExecludeReasons
                                       join trans in _context.SupplierExecludes on execlude.Id equals trans.ReasonId
                                       where trans.SupplierExecludeAssetId == item.Id
                                         && trans.SupplierExecludeAsset.AppTypeId == 1
                                       select execlude).ToList();
                    if (lstExTitles.Count > 0)
                    {
                        List<string> execludeNames = new List<string>();
                        List<string> execludeNamesAr = new List<string>();
                        foreach (var reason in lstExTitles)
                        {
                            execludeNames.Add(reason.Name);
                            execludeNamesAr.Add(reason.NameAr);
                        }
                        getDataObj.ReasonExTitles = string.Join(",", execludeNames);
                        getDataObj.ReasonExTitlesAr = string.Join(",", execludeNamesAr);

                    }
                }
                if (item.AppTypeId == 2)
                {
                    var lstHoldTitles = (from execlude in _context.SupplierHoldReasons
                                         join trans in _context.SupplierExecludes on execlude.Id equals trans.ReasonId
                                         where trans.SupplierExecludeAssetId == item.Id
                                           && trans.SupplierExecludeAsset.AppTypeId == 2
                                         select execlude).ToList();
                    if (lstHoldTitles.Count > 0)
                    {
                        List<string> holdNames = new List<string>();
                        List<string> holdNamesAr = new List<string>();
                        foreach (var reason in lstHoldTitles)
                        {
                            holdNames.Add(reason.Name);
                            holdNamesAr.Add(reason.NameAr);
                        }
                        getDataObj.ReasonHoldTitles = string.Join(",", holdNames);
                        getDataObj.ReasonHoldTitlesAr = string.Join(",", holdNamesAr);
                    }
                }

                list.Add(getDataObj);
            }

            return list;
        }
    }
}