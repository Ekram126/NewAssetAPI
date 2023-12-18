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
                HospitalId = item.HospitalId,
                AssetId = item.AssetId,
                AppTypeId = item.AppTypeId,
                StatusId = item.StatusId,
                Date = item.Date,
                ExecludeDate = item.ExecludeDate,
                ExNumber = item.ExNumber,
              //  UserId = item.User.UserName,
                UserId = item.UserId,
                ReasonIds = reasonIds,
                HoldReasonIds = holdIds,
                Comment = item.Comment,
                appTypeName = item.ApplicationType.Name,
                appTypeNameAr = item.ApplicationType.NameAr,

                HospitalName = item.AssetDetail.Hospital.Name,
                HospitalNameAr = item.AssetDetail.Hospital.NameAr,

                assetName = item.AssetDetail.MasterAsset.Name,// + " - " + item.AssetDetail.SerialNumber,
                assetNameAr = item.AssetDetail.MasterAsset.NameAr,// + " - " + item.AssetDetail.SerialNumber,

                SerialNumber = item.AssetDetail.SerialNumber,
                BarCode = item.AssetDetail.Barcode

            }).FirstOrDefault();

        }

        public IEnumerable<IndexSupplierExecludeAssetVM.GetData> GetAll()
        {
            List<IndexSupplierExecludeAssetVM.GetData> list = new List<IndexSupplierExecludeAssetVM.GetData>();
            var lstSupplierExecludeAssets = _context.SupplierExecludeAssets.Include(a => a.User)
                .Include(a => a.AssetDetail).Include(a => a.AssetDetail.Hospital).Include(a => a.ApplicationType)
                .Include(a => a.HospitalSupplierStatus)
                .Include(a => a.AssetDetail.MasterAsset).OrderByDescending(a => a.Date.Value.Date).ToList();
            foreach (var item in lstSupplierExecludeAssets)
            {

                IndexSupplierExecludeAssetVM.GetData getDataObj = new IndexSupplierExecludeAssetVM.GetData();
                getDataObj.Id = item.Id;
                getDataObj.HospitalId = item.HospitalId;
                getDataObj.ExNumber = item.ExNumber;
                getDataObj.AppTypeId = item.AppTypeId;
                getDataObj.Date = item.Date != null ? item.Date.Value.ToString() : "";
                getDataObj.ExecludeDate = item.ExecludeDate != null ? item.ExecludeDate.Value.ToString() : "";
                getDataObj.UserName = item.User.UserName;
                getDataObj.AssetId = item.AssetDetail.Id;
                getDataObj.AssetName = item.AssetDetail.MasterAsset.Name;// + " - " + item.AssetDetail.SerialNumber;
                getDataObj.AssetNameAr = item.AssetDetail.MasterAsset.NameAr;// + " - " + item.AssetDetail.SerialNumber;

                getDataObj.SerialNumber = item.AssetDetail.SerialNumber;
                getDataObj.BarCode = item.AssetDetail.Barcode;
                getDataObj.ModelNumber = item.AssetDetail.MasterAsset.ModelNumber;


                getDataObj.DiffMonths = ((item.Date.Value.Year - DateTime.Today.Date.Year) * 12) + item.Date.Value.Month - DateTime.Today.Date.Month;
                getDataObj.IsMoreThan3Months = getDataObj.DiffMonths <= -3 ? true : false;
                getDataObj.StatusId = item.StatusId;

                getDataObj.HospitalName = item.AssetDetail.Hospital != null ? item.AssetDetail.Hospital.Name : "";
                getDataObj.HospitalNameAr = item.AssetDetail.Hospital != null ? item.AssetDetail.Hospital.NameAr : "";

                var lstStatuses = _context.HospitalSupplierStatuses.Where(a => a.Id == item.StatusId).ToList();
                if (lstStatuses.Count > 0)
                {
                    getDataObj.StatusId = lstStatuses[0].Id;
                    getDataObj.StatusName = lstStatuses[0].Name;
                    getDataObj.StatusNameAr = lstStatuses[0].NameAr;
                    getDataObj.StatusColor = lstStatuses[0].Color;
                    getDataObj.StatusIcon = lstStatuses[0].Icon;
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
                    supplierExecludeAssetObj.HospitalId = model.HospitalId;
                    supplierExecludeAssetObj.Date = DateTime.Now;
                    if (model.ExecludeDate != "")
                        supplierExecludeAssetObj.ExecludeDate = DateTime.Parse(model.ExecludeDate.ToString());

                    if (model.ActionDate != null)
                        supplierExecludeAssetObj.ActionDate = DateTime.Parse(model.ActionDate.ToString());
                    else
                        supplierExecludeAssetObj.ActionDate = DateTime.Now;

                    supplierExecludeAssetObj.ExNumber = model.ExNumber;
                    supplierExecludeAssetObj.UserId = model.UserId;
                    _context.SupplierExecludeAssets.Add(supplierExecludeAssetObj);
                    _context.SaveChanges();
                    int id = supplierExecludeAssetObj.Id;
                    return id;
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
                    var lstTransactions = _context.SupplierExecludes.Where(a => a.SupplierExecludeAssetId == SupplierExecludeAssetObj.Id).ToList();
                    if (lstTransactions.Count > 0)
                    {
                        foreach (var trans in lstTransactions)
                        {
                            var lstAttachments = _context.SupplierExecludeAttachments.Where(a => a.SupplierExecludeId == trans.Id).ToList();
                            foreach (var attach in lstAttachments)
                            {
                                _context.SupplierExecludeAttachments.Remove(attach);
                                _context.SaveChanges();
                            }
                            _context.SupplierExecludes.Remove(trans);
                            _context.SaveChanges();
                        }

                    }
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
                supplierExecludeAssetObj.HospitalId = model.HospitalId;
                if (model.ActionDate != "")
                    supplierExecludeAssetObj.ActionDate = DateTime.Parse(model.ActionDate.ToString());

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
                            supplierExecludeObj.HospitalId = model.HospitalId;
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
            assetAttachmentObj.SupplierExecludeId = attachObj.SupplierExecludeId;
            assetAttachmentObj.Title = attachObj.Title;
            assetAttachmentObj.FileName = attachObj.FileName;
            assetAttachmentObj.HospitalId = attachObj.HospitalId;
            _context.SupplierExecludeAttachments.Add(assetAttachmentObj);
            _context.SaveChanges();
            int Id = assetAttachmentObj.Id;
            return Id;
        }

        public IEnumerable<SupplierExecludeAttachment> GetAttachmentBySupplierExecludeAssetId(int assetId)
        {
            return _context.SupplierExecludeAttachments.Where(a => a.SupplierExecludeId == assetId).ToList();
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
                    supplierExecludeAssetObj.ExecludeDate = DateTime.Now;

                supplierExecludeAssetObj.ActionDate = DateTime.Now;


                supplierExecludeAssetObj.MemberId = model.MemberId;
                supplierExecludeAssetObj.Comment = model.Comment;
                _context.Entry(supplierExecludeAssetObj).State = EntityState.Modified;
                _context.SaveChanges();

                if (model.StatusId == 2 && supplierExecludeAssetObj.AppTypeId == 1)
                {
                    AssetStatusTransaction assetStatusTransactionObj = new AssetStatusTransaction();
                    assetStatusTransactionObj.AssetDetailId = (int)model.AssetId;
                    assetStatusTransactionObj.AssetStatusId = 8;
                    assetStatusTransactionObj.StatusDate = DateTime.Today.Date;
                    assetStatusTransactionObj.HospitalId = supplierExecludeAssetObj.HospitalId;
                    _context.AssetStatusTransactions.Add(assetStatusTransactionObj);
                    _context.SaveChanges();
                }
                if (model.StatusId == 2 && supplierExecludeAssetObj.AppTypeId == 2)
                {
                    AssetStatusTransaction assetStatusTransactionObj = new AssetStatusTransaction();
                    assetStatusTransactionObj.AssetDetailId = (int)model.AssetId;
                    assetStatusTransactionObj.AssetStatusId = 9;
                    assetStatusTransactionObj.StatusDate = DateTime.Today.Date;
                    assetStatusTransactionObj.HospitalId = supplierExecludeAssetObj.HospitalId;
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


                                            HospitalId = item.AssetDetail.Hospital.Id,
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
                getDataObj.AssetId = item.AssetId;
                getDataObj.HospitalId = item.HospitalId;
                getDataObj.ExNumber = item.ExNumber;
                getDataObj.AppTypeId = item.AppTypeId;
                getDataObj.Comment = item.Comment;
                getDataObj.Date = item.Date != null ? item.Date.Value.ToString() : "";
                getDataObj.ExecludeDate = item.ExecludeDate != null ? item.ExecludeDate.Value.ToString() : "";
                getDataObj.UserName = item.User.UserName;
                getDataObj.AssetId = item.AssetDetail.Id;
                getDataObj.AssetName = item.AssetDetail.MasterAsset.Name;// + " - " + item.AssetDetail.SerialNumber;
                getDataObj.AssetNameAr = item.AssetDetail.MasterAsset.NameAr;// + " - " + item.AssetDetail.SerialNumber;

                getDataObj.SerialNumber = item.AssetDetail.SerialNumber;
                getDataObj.BarCode = item.AssetDetail.Barcode;
                getDataObj.ModelNumber = item.AssetDetail.MasterAsset.ModelNumber;


                getDataObj.DiffMonths = ((item.Date.Value.Year - DateTime.Today.Date.Year) * 12) + item.Date.Value.Month - DateTime.Today.Date.Month;
                getDataObj.IsMoreThan3Months = getDataObj.DiffMonths <= -3 ? true : false;
                getDataObj.StatusId = item.StatusId;

                var lstStatuses = _context.HospitalSupplierStatuses.Where(a => a.Id == item.StatusId).ToList();
                if (lstStatuses.Count > 0)
                {
                    getDataObj.StatusName = lstStatuses[0].Name;
                    getDataObj.StatusNameAr = lstStatuses[0].NameAr;
                    getDataObj.StatusColor = lstStatuses[0].Color;
                    getDataObj.StatusIcon = lstStatuses[0].Icon;
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





            else if (sortObj.BarCode != "")
            {
                if (sortObj.SortStatus == "descending")
                    list = list.OrderByDescending(d => d.BarCode).ToList();
                else
                    list = list.OrderBy(d => d.BarCode).ToList();
            }

            else if (sortObj.SerialNumber != "")
            {
                if (sortObj.SortStatus == "descending")
                    list = list.OrderByDescending(d => d.SerialNumber).ToList();
                else
                    list = list.OrderBy(d => d.SerialNumber).ToList();
            }

            else if (sortObj.ModelNumber != "")
            {
                if (sortObj.SortStatus == "descending")
                    list = list.OrderByDescending(d => d.ModelNumber).ToList();
                else
                    list = list.OrderBy(d => d.ModelNumber).ToList();
            }

            if (sortObj.StatusId != 0)
            {
                list = list.Where(a => a.StatusId == sortObj.StatusId).ToList();

            }
            else
            {
                list = list.ToList();
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
                getDataObj.AssetId = item.AssetId;
                getDataObj.HospitalId = item.HospitalId;
                getDataObj.ExNumber = item.ExNumber;
                getDataObj.Comment = item.Comment;
                getDataObj.AppTypeId = item.AppTypeId;
                getDataObj.Date = item.Date != null ? item.Date.Value.ToString() : "";
                getDataObj.ExecludeDate = item.ExecludeDate != null ? item.ExecludeDate.Value.ToString() : "";
                getDataObj.UserName = item.User.UserName;
                getDataObj.AssetId = item.AssetDetail.Id;
                getDataObj.AssetName = item.AssetDetail.MasterAsset != null ? item.AssetDetail.MasterAsset.Name : "";// + " - " + item.AssetDetail.SerialNumber : "";
                getDataObj.AssetNameAr = item.AssetDetail.MasterAsset != null ? item.AssetDetail.MasterAsset.NameAr : "";// + " - " + item.AssetDetail.SerialNumber : "";

                getDataObj.SerialNumber = item.AssetDetail.SerialNumber;
                getDataObj.BarCode = item.AssetDetail.Barcode;
                getDataObj.ModelNumber = item.AssetDetail.MasterAsset.ModelNumber;


                getDataObj.DiffMonths = ((item.Date.Value.Year - DateTime.Today.Date.Year) * 12) + item.Date.Value.Month - DateTime.Today.Date.Month;


                getDataObj.IsMoreThan3Months = getDataObj.DiffMonths <= -3 ? true : false;

                getDataObj.StatusId = item.StatusId;

                var lstStatuses = _context.HospitalSupplierStatuses.Where(a => a.Id == item.StatusId).ToList();
                if (lstStatuses.Count > 0)
                {
                    getDataObj.StatusName = lstStatuses[0].Name;
                    getDataObj.StatusNameAr = lstStatuses[0].NameAr;
                    getDataObj.StatusColor = lstStatuses[0].Color;
                    getDataObj.StatusIcon = lstStatuses[0].Icon;
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
                 .Include(a => a.AssetDetail.Hospital)
                .Include(a => a.AssetDetail.MasterAsset).OrderByDescending(a => a.Date.Value.Date).ToList();

            if (statusId != 0)
                lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.StatusId == statusId).ToList();


            if (appTypeId != 0)
                lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.AppTypeId == appTypeId).ToList();



            foreach (var item in lstSupplierExecludeAssets)
            {

                IndexSupplierExecludeAssetVM.GetData getDataObj = new IndexSupplierExecludeAssetVM.GetData();
                getDataObj.Id = item.Id;
                getDataObj.AssetId = item.AssetId;
                getDataObj.HospitalId = item.HospitalId;
                getDataObj.AppTypeId = item.AppTypeId;
                getDataObj.ExNumber = item.ExNumber;
                getDataObj.Date = item.Date != null ? item.Date.Value.ToString() : "";
                getDataObj.ExecludeDate = item.ExecludeDate != null ? item.ExecludeDate.Value.ToString() : "";
                getDataObj.UserName = item.User.UserName;
                getDataObj.AssetId = item.AssetDetail.Id;
                getDataObj.AssetName = item.AssetDetail.MasterAsset.Name;// + " - " + item.AssetDetail.SerialNumber;
                getDataObj.AssetNameAr = item.AssetDetail.MasterAsset.NameAr;// + " - " + item.AssetDetail.SerialNumber;

                getDataObj.HospitalName = item.AssetDetail.Hospital != null ? item.AssetDetail.Hospital.Name : "";
                getDataObj.HospitalNameAr = item.AssetDetail.Hospital != null ? item.AssetDetail.Hospital.NameAr : "";


                getDataObj.SerialNumber = item.AssetDetail.SerialNumber;
                getDataObj.BarCode = item.AssetDetail.Barcode;
                getDataObj.ModelNumber = item.AssetDetail.MasterAsset.ModelNumber;


                getDataObj.DiffMonths = ((item.Date.Value.Year - DateTime.Today.Date.Year) * 12) + item.Date.Value.Month - DateTime.Today.Date.Month;
                getDataObj.IsMoreThan3Months = getDataObj.DiffMonths <= -3 ? true : false;
                getDataObj.StatusId = item.StatusId;
                getDataObj.StatusName = item.HospitalSupplierStatus.Name;
                getDataObj.StatusNameAr = item.HospitalSupplierStatus.NameAr;
                getDataObj.StatusColor = item.HospitalSupplierStatus.Color;
                getDataObj.StatusIcon = item.HospitalSupplierStatus.Icon;

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

        public GenerateSupplierExecludeAssetNumberVM GenerateSupplierExecludeAssetNumber()
        {
            GenerateSupplierExecludeAssetNumberVM numberObj = new GenerateSupplierExecludeAssetNumberVM();
            string pre = "SEXHLD";

            var lstSupplierExecludeAssets = _context.SupplierExecludeAssets.ToList();
            if (lstSupplierExecludeAssets.Count > 0)
            {
                var appNumber = lstSupplierExecludeAssets.LastOrDefault().Id;
                numberObj.ExNumber = pre + (appNumber + 1);
            }
            else
            {
                numberObj.ExNumber = pre + 1;
            }

            return numberObj;
        }

        public IEnumerable<IndexSupplierExecludeAssetVM.GetData> GetSupplierExecludeAssetByDate(SearchSupplierExecludeAssetVM searchObj)
        {

            if (searchObj.strStartDate != "")
                searchObj.StartDate = DateTime.Parse(searchObj.strStartDate);
            else
                searchObj.StartDate = DateTime.Today.Date;


            if (searchObj.strEndDate != "")
                searchObj.EndDate = DateTime.Parse(searchObj.strEndDate);
            else
                searchObj.EndDate = DateTime.Today.Date;


            List<IndexSupplierExecludeAssetVM.GetData> list = new List<IndexSupplierExecludeAssetVM.GetData>();

            var lstSupplierExecludeAssets = _context.SupplierExecludeAssets
              .Include(a => a.HospitalSupplierStatus)
                  .Include(a => a.ApplicationType)
           .Include(a => a.User)
           .Include(a => a.AssetDetail)
           .Include(a => a.AssetDetail.MasterAsset).Where(a => a.Date >= searchObj.StartDate.Value.Date && a.Date <= searchObj.EndDate.Value.Date).OrderByDescending(a => a.Date.Value.Date).ToList();


            foreach (var item in lstSupplierExecludeAssets)
            {

                IndexSupplierExecludeAssetVM.GetData getDataObj = new IndexSupplierExecludeAssetVM.GetData();
                getDataObj.Id = item.Id;
                getDataObj.AssetId = item.AssetId;
                getDataObj.HospitalId = item.HospitalId;
                getDataObj.AppTypeId = item.AppTypeId;
                getDataObj.ExNumber = item.ExNumber;
                getDataObj.Date = item.Date != null ? item.Date.Value.ToString() : "";
                getDataObj.ExecludeDate = item.ExecludeDate != null ? item.ExecludeDate.Value.ToString() : "";
                getDataObj.UserName = item.User.UserName;
                getDataObj.AssetId = item.AssetDetail.Id;
                getDataObj.AssetName = item.AssetDetail.MasterAsset.Name;// + " - " + item.AssetDetail.SerialNumber;
                getDataObj.AssetNameAr = item.AssetDetail.MasterAsset.NameAr;// + " - " + item.AssetDetail.SerialNumber;

                getDataObj.SerialNumber = item.AssetDetail.SerialNumber;
                getDataObj.BarCode = item.AssetDetail.Barcode;
                getDataObj.ModelNumber = item.AssetDetail.MasterAsset.ModelNumber;

                getDataObj.DiffMonths = ((item.Date.Value.Year - DateTime.Today.Date.Year) * 12) + item.Date.Value.Month - DateTime.Today.Date.Month;
                getDataObj.IsMoreThan3Months = getDataObj.DiffMonths <= -3 ? true : false;
                getDataObj.StatusId = item.StatusId;
                getDataObj.StatusName = item.HospitalSupplierStatus.Name;
                getDataObj.StatusNameAr = item.HospitalSupplierStatus.NameAr;
                getDataObj.StatusColor = item.HospitalSupplierStatus.Color;
                getDataObj.StatusIcon = item.HospitalSupplierStatus.Icon;



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

        public IndexSupplierExecludeAssetVM GetAllSupplierExecludes(SearchSupplierExecludeAssetVM searchObj, int statusId, int appTypeId, int hospitalId, int pageNumber, int pageSize)
        {
            IndexSupplierExecludeAssetVM mainClass = new IndexSupplierExecludeAssetVM();
            List<IndexSupplierExecludeAssetVM.GetData> list = new List<IndexSupplierExecludeAssetVM.GetData>();


            var lstSupplierExecludeAssets = _context.SupplierExecludeAssets.Include(a => a.User)
                 .Include(a => a.ApplicationType)
            .Include(a => a.HospitalSupplierStatus)
            .Include(a => a.AssetDetail).Include(a => a.AssetDetail.MasterAsset).Include(a => a.AssetDetail.MasterAsset.brand).ToList()
            .OrderByDescending(a => a.Date.Value.Date).ToList();

            if (lstSupplierExecludeAssets.Count > 0)
            {
                if (hospitalId != 0)
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.AssetDetail.HospitalId == hospitalId).ToList();

                if (appTypeId != 0)
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.AppTypeId == appTypeId).ToList();

                if (statusId != 0)
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.StatusId == statusId || a.StatusId==4).ToList();



                if (searchObj.strStartDate != "")
                    searchObj.StartDate = DateTime.Parse(searchObj.strStartDate);


                if (searchObj.strEndDate != "")
                    searchObj.EndDate = DateTime.Parse(searchObj.strEndDate);




                if (searchObj.StartDate != null && searchObj.EndDate != null)
                {
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.Date.Value.Date >= searchObj.StartDate.Value.Date && a.Date.Value.Date <= searchObj.EndDate.Value.Date).ToList();
                }
                else
                {
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.ToList();
                }



                foreach (var item in lstSupplierExecludeAssets)
                {

                    IndexSupplierExecludeAssetVM.GetData getDataObj = new IndexSupplierExecludeAssetVM.GetData();
                    getDataObj.Id = item.Id;
                    getDataObj.AssetId = item.AssetId;
                    getDataObj.HospitalId = item.HospitalId;
                    getDataObj.ExNumber = item.ExNumber;
                    getDataObj.Date = item.Date.Value.ToString();
                    getDataObj.AppTypeId = item.AppTypeId;
                    getDataObj.UserName = item.User.UserName;
                    getDataObj.AssetId = item.AssetDetail.Id;
                    getDataObj.AssetName = item.AssetDetail.MasterAsset.Name;
                    getDataObj.AssetNameAr = item.AssetDetail.MasterAsset.NameAr;
                    getDataObj.TypeName = item.ApplicationType.Name;
                    getDataObj.TypeNameAr = item.ApplicationType.NameAr;

                    if (item.AssetDetail.MasterAsset.brand != null)
                    {
                        getDataObj.BrandName = item.AssetDetail.MasterAsset.brand.Name;
                        getDataObj.BrandNameAr = item.AssetDetail.MasterAsset.brand.NameAr;
                    }

                    getDataObj.FixCost = item.AssetDetail.FixCost;
                    getDataObj.CostPerDay = item.AssetDetail.FixCost != null ? Math.Round((((decimal)item.AssetDetail.FixCost) / 365)) : 0;
                    if (searchObj.EndDate != null)
                    {
                        getDataObj.AllDays = Math.Round((DateTime.Parse(searchObj.EndDate.ToString()) - DateTime.Parse(item.Date.ToString()).Date).TotalDays);
                    }
                    else
                    {
                        getDataObj.AllDays = 0;
                    }


                    if (getDataObj.CostPerDay != 0 && getDataObj.AllDays != 0)
                        getDataObj.TotalCost = Math.Round((decimal)getDataObj.CostPerDay * (decimal)getDataObj.AllDays);
                    else
                        getDataObj.TotalCost = 0;





                    getDataObj.SerialNumber = item.AssetDetail.SerialNumber;
                    getDataObj.BarCode = item.AssetDetail.Barcode;
                    getDataObj.ModelNumber = item.AssetDetail.MasterAsset.ModelNumber;


                    getDataObj.DiffMonths = ((item.Date.Value.Year - DateTime.Today.Date.Year) * 12) + item.Date.Value.Month - DateTime.Today.Date.Month;
                    getDataObj.IsMoreThan3Months = getDataObj.DiffMonths <= -3 ? true : false;

                    getDataObj.StatusId = item.StatusId;
                    getDataObj.StatusName = item.HospitalSupplierStatus.Name;
                    getDataObj.StatusNameAr = item.HospitalSupplierStatus.NameAr;
                    getDataObj.StatusColor = item.HospitalSupplierStatus.Color;
                    getDataObj.StatusIcon = item.HospitalSupplierStatus.Icon;


                    if (appTypeId == 1)
                    {

                        var ReasonExTitles = (from execlude in _context.SupplierExecludeReasons
                                              join trans in _context.SupplierExecludes on execlude.Id equals trans.ReasonId
                                              where trans.SupplierExecludeAssetId == item.Id
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
                        var ReasonHoldTitles = (from execlude in _context.SupplierExecludeReasons
                                                join trans in _context.SupplierExecludes on execlude.Id equals trans.ReasonId
                                                where trans.SupplierExecludeAssetId == item.Id
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




                var requestsPerPage = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                mainClass.Results = requestsPerPage;
                mainClass.Count = list.Count();

            }
            return mainClass;
        }

        public IndexSupplierExecludeAssetVM GetAllSupplierHoldes(SearchSupplierExecludeAssetVM searchObj, int statusId, int appTypeId, int hospitalId, int pageNumber, int pageSize)
        {
            IndexSupplierExecludeAssetVM mainClass = new IndexSupplierExecludeAssetVM();
            List<IndexSupplierExecludeAssetVM.GetData> list = new List<IndexSupplierExecludeAssetVM.GetData>();

            var lstSupplierExecludeAssets = _context.SupplierExecludeAssets.Include(a => a.User)
                 .Include(a => a.ApplicationType)
            .Include(a => a.HospitalSupplierStatus)
            .Include(a => a.AssetDetail).Include(a => a.AssetDetail.MasterAsset).Include(a => a.AssetDetail.MasterAsset.brand).ToList().OrderByDescending(a => a.Date.Value.Date).ToList();

            if (lstSupplierExecludeAssets.Count > 0)
            {
                if (hospitalId != 0)
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.AssetDetail.HospitalId == hospitalId).ToList();

                if (appTypeId != 0)
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.AppTypeId == appTypeId).ToList();

                if (statusId != 0)
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.StatusId == statusId || a.StatusId == 4).ToList();



                if (searchObj.strStartDate != "")
                    searchObj.StartDate = DateTime.Parse(searchObj.strStartDate);


                if (searchObj.strEndDate != "")
                    searchObj.EndDate = DateTime.Parse(searchObj.strEndDate);




                if (searchObj.StartDate != null && searchObj.EndDate != null)
                {
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.Date.Value.Date >= searchObj.StartDate.Value.Date && a.Date.Value.Date <= searchObj.EndDate.Value.Date).ToList();
                }
                else
                {
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.ToList();
                }



                foreach (var item in lstSupplierExecludeAssets)
                {

                    IndexSupplierExecludeAssetVM.GetData getDataObj = new IndexSupplierExecludeAssetVM.GetData();
                    getDataObj.Id = item.Id;
                    getDataObj.AssetId = item.AssetId;
                    getDataObj.HospitalId = item.HospitalId;
                    getDataObj.ExNumber = item.ExNumber;
                    getDataObj.Date = item.Date.Value.ToString();
                    getDataObj.AppTypeId = item.AppTypeId;
                    getDataObj.UserName = item.User.UserName;
                    getDataObj.AssetId = item.AssetDetail.Id;
                    getDataObj.AssetName = item.AssetDetail.MasterAsset.Name;
                    getDataObj.AssetNameAr = item.AssetDetail.MasterAsset.NameAr;
                    getDataObj.TypeName = item.ApplicationType.Name;
                    getDataObj.TypeNameAr = item.ApplicationType.NameAr;

                    if (item.AssetDetail.MasterAsset.brand != null)
                    {
                        getDataObj.BrandName = item.AssetDetail.MasterAsset.brand.Name;
                        getDataObj.BrandNameAr = item.AssetDetail.MasterAsset.brand.NameAr;
                    }

                    getDataObj.FixCost = item.AssetDetail.FixCost;
                    getDataObj.CostPerDay = item.AssetDetail.FixCost != null ? Math.Round((((decimal)item.AssetDetail.FixCost) / 365)) : 0;
                    if (searchObj.EndDate != null)
                    {
                        getDataObj.AllDays = Math.Round((DateTime.Parse(searchObj.EndDate.ToString()) - DateTime.Parse(item.Date.ToString()).Date).TotalDays);
                    }
                    else
                    {
                        getDataObj.AllDays = 0;
                    }


                    if (getDataObj.CostPerDay != 0 && getDataObj.AllDays != 0)
                        getDataObj.TotalCost = Math.Round((decimal)getDataObj.CostPerDay * (decimal)getDataObj.AllDays);
                    else
                        getDataObj.TotalCost = 0;





                    getDataObj.SerialNumber = item.AssetDetail.SerialNumber;
                    getDataObj.BarCode = item.AssetDetail.Barcode;
                    getDataObj.ModelNumber = item.AssetDetail.MasterAsset.ModelNumber;


                    getDataObj.DiffMonths = ((item.Date.Value.Year - DateTime.Today.Date.Year) * 12) + item.Date.Value.Month - DateTime.Today.Date.Month;
                    getDataObj.IsMoreThan3Months = getDataObj.DiffMonths <= -3 ? true : false;

                    getDataObj.StatusId = item.StatusId;
                    getDataObj.StatusName = item.HospitalSupplierStatus.Name;
                    getDataObj.StatusNameAr = item.HospitalSupplierStatus.NameAr;
                    getDataObj.StatusColor = item.HospitalSupplierStatus.Color;
                    getDataObj.StatusIcon = item.HospitalSupplierStatus.Icon;


                    if (appTypeId == 1)
                    {

                        var ReasonExTitles = (from execlude in _context.SupplierExecludeReasons
                                              join trans in _context.SupplierExecludes on execlude.Id equals trans.ReasonId
                                              where trans.SupplierExecludeAssetId == item.Id
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
                        var ReasonHoldTitles = (from execlude in _context.SupplierExecludeReasons
                                                join trans in _context.SupplierExecludes on execlude.Id equals trans.ReasonId
                                                where trans.SupplierExecludeAssetId == item.Id
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




                var requestsPerPage = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                mainClass.Results = requestsPerPage;
                mainClass.Count = list.Count();

            }
            return mainClass;
        }






        public IndexSupplierExecludeAssetVM GetAllSupplierExecludes(SearchSupplierExecludeAssetVM searchObj)
        {
            IndexSupplierExecludeAssetVM mainClass = new IndexSupplierExecludeAssetVM();
            List<IndexSupplierExecludeAssetVM.GetData> list = new List<IndexSupplierExecludeAssetVM.GetData>();


            var lstSupplierExecludeAssets = _context.SupplierExecludeAssets.Include(a => a.User)
                .Include(a => a.ApplicationType)
           .Include(a => a.HospitalSupplierStatus)
           .Include(a => a.AssetDetail).Include(a => a.AssetDetail.MasterAsset)
           .Include(a => a.AssetDetail.MasterAsset.brand).Where(t => t.AppTypeId == 1).ToList().OrderByDescending(a => a.Date.Value.Date).ToList();

            if (lstSupplierExecludeAssets.Count > 0)
            {
                if (searchObj.HospitalId != 0)
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.AssetDetail.HospitalId == searchObj.HospitalId).ToList();

                if (searchObj.AppTypeId != 0)
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.AppTypeId == searchObj.AppTypeId).ToList();

                if (searchObj.StatusId != 0)
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.StatusId == searchObj.StatusId).ToList();



                if (searchObj.strStartDate != "")
                    searchObj.StartDate = DateTime.Parse(searchObj.strStartDate);


                if (searchObj.strEndDate != "")
                    searchObj.EndDate = DateTime.Parse(searchObj.strEndDate);




                if (searchObj.StartDate != null && searchObj.EndDate != null)
                {
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.Date.Value.Date >= searchObj.StartDate.Value.Date && a.Date.Value.Date <= searchObj.EndDate.Value.Date).ToList();
                }
                else
                {
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.ToList();
                }



                foreach (var item in lstSupplierExecludeAssets)
                {

                    IndexSupplierExecludeAssetVM.GetData getDataObj = new IndexSupplierExecludeAssetVM.GetData();
                    getDataObj.Id = item.Id;
                    getDataObj.AssetId = item.AssetId;
                    getDataObj.HospitalId = item.HospitalId;
                    getDataObj.ExNumber = item.ExNumber;
                    getDataObj.Date = item.Date.Value.ToString();
                    getDataObj.AppTypeId = item.AppTypeId;
                    getDataObj.UserName = item.User.UserName;
                    getDataObj.AssetId = item.AssetDetail.Id;
                    getDataObj.AssetName = item.AssetDetail.MasterAsset.Name;
                    getDataObj.AssetNameAr = item.AssetDetail.MasterAsset.NameAr;
                    getDataObj.TypeName = item.ApplicationType.Name;
                    getDataObj.TypeNameAr = item.ApplicationType.NameAr;

                    if (item.AssetDetail.MasterAsset.brand != null)
                    {
                        getDataObj.BrandName = item.AssetDetail.MasterAsset.brand.Name;
                        getDataObj.BrandNameAr = item.AssetDetail.MasterAsset.brand.NameAr;
                    }

                    getDataObj.FixCost = item.AssetDetail.FixCost;
                    getDataObj.CostPerDay = item.AssetDetail.FixCost != null ? Math.Round((((decimal)item.AssetDetail.FixCost) / 365)) : 0;
                    if (searchObj.EndDate != null)
                    {
                        getDataObj.AllDays = Math.Round((DateTime.Parse(searchObj.EndDate.ToString()) - DateTime.Parse(item.Date.ToString()).Date).TotalDays);
                    }
                    else
                    {
                        getDataObj.AllDays = 0;
                    }


                    if (getDataObj.CostPerDay != 0 && getDataObj.AllDays != 0)
                        getDataObj.TotalCost = Math.Round((decimal)getDataObj.CostPerDay * (decimal)getDataObj.AllDays);
                    else
                        getDataObj.TotalCost = 0;





                    getDataObj.SerialNumber = item.AssetDetail.SerialNumber;
                    getDataObj.BarCode = item.AssetDetail.Barcode;
                    getDataObj.ModelNumber = item.AssetDetail.MasterAsset.ModelNumber;


                    getDataObj.DiffMonths = ((item.Date.Value.Year - DateTime.Today.Date.Year) * 12) + item.Date.Value.Month - DateTime.Today.Date.Month;
                    getDataObj.IsMoreThan3Months = getDataObj.DiffMonths <= -3 ? true : false;

                    getDataObj.StatusId = item.StatusId;
                    getDataObj.StatusName = item.HospitalSupplierStatus.Name;
                    getDataObj.StatusNameAr = item.HospitalSupplierStatus.NameAr;
                    getDataObj.StatusColor = item.HospitalSupplierStatus.Color;
                    getDataObj.StatusIcon = item.HospitalSupplierStatus.Icon;


                    //if (searchObj.AppTypeId == 1)
                    //{

                    //    var ReasonExTitles = (from execlude in _context.SupplierExecludeReasons
                    //                          join trans in _context.SupplierExecludes on execlude.Id equals trans.ReasonId
                    //                          where trans.SupplierExecludeAssetId == item.Id
                    //                          && item.AppTypeId == 1
                    //                          select execlude).ToList();
                    //    if (ReasonExTitles.Count > 0)
                    //    {
                    //        List<string> execludeNames = new List<string>();// { "John", "Anna", "Monica" };
                    //        foreach (var reason in ReasonExTitles)
                    //        {
                    //            execludeNames.Add(reason.Name);
                    //        }

                    //        getDataObj.ReasonExTitles = string.Join(",", execludeNames);


                    //        List<string> execludeNamesAr = new List<string>();
                    //        foreach (var reason in ReasonExTitles)
                    //        {
                    //            execludeNamesAr.Add(reason.NameAr);
                    //        }
                    //        getDataObj.ReasonExTitlesAr = string.Join(",", execludeNamesAr);

                    //    }
                    //}
                    //if (searchObj.AppTypeId == 2)
                    //{
                    //    var ReasonHoldTitles = (from execlude in _context.SupplierExecludeReasons
                    //                            join trans in _context.SupplierExecludes on execlude.Id equals trans.ReasonId
                    //                            where trans.SupplierExecludeAssetId == item.Id
                    //                              && item.AppTypeId == 2
                    //                            select execlude).ToList();
                    //    if (ReasonHoldTitles.Count > 0)
                    //    {
                    //        List<string> holdNames = new List<string>();
                    //        foreach (var reason in ReasonHoldTitles)
                    //        {
                    //            holdNames.Add(reason.Name);
                    //        }
                    //        getDataObj.ReasonHoldTitles = string.Join(",", holdNames);

                    //        List<string> holdNamesAr = new List<string>();
                    //        foreach (var reason in ReasonHoldTitles)
                    //        {
                    //            holdNamesAr.Add(reason.NameAr);
                    //        }
                    //        getDataObj.ReasonHoldTitlesAr = string.Join(",", holdNamesAr);
                    //    }
                    //}

                    list.Add(getDataObj);
                }





                mainClass.Results = list;
                mainClass.Count = list.Count();

            }
            return mainClass;
        }

        public IndexSupplierExecludeAssetVM GetAllSupplierHoldes(SearchSupplierExecludeAssetVM searchObj)
        {
            IndexSupplierExecludeAssetVM mainClass = new IndexSupplierExecludeAssetVM();
            List<IndexSupplierExecludeAssetVM.GetData> list = new List<IndexSupplierExecludeAssetVM.GetData>();

            var lstSupplierExecludeAssets = _context.SupplierExecludeAssets
                                                    .Include(a => a.User)
                                                    .Include(a => a.ApplicationType)
                                                    .Include(a => a.HospitalSupplierStatus)
                                                    .Include(a => a.AssetDetail).Include(a => a.AssetDetail.MasterAsset)
                                                    .Include(a => a.AssetDetail.MasterAsset.brand)
                                                    .Where(t => t.AppTypeId == 2).ToList().OrderByDescending(a => a.Date.Value.Date).ToList();

            if (lstSupplierExecludeAssets.Count > 0)
            {
                if (searchObj.HospitalId != 0)
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.AssetDetail.HospitalId == searchObj.HospitalId).ToList();

                if (searchObj.AppTypeId != 0)
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.AppTypeId == searchObj.AppTypeId).ToList();

                if (searchObj.StatusId != 0)
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.StatusId == searchObj.StatusId).ToList();



                if (searchObj.strStartDate != "")
                    searchObj.StartDate = DateTime.Parse(searchObj.strStartDate);


                if (searchObj.strEndDate != "")
                    searchObj.EndDate = DateTime.Parse(searchObj.strEndDate);




                if (searchObj.StartDate != null && searchObj.EndDate != null)
                {
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.Date.Value.Date >= searchObj.StartDate.Value.Date && a.Date.Value.Date <= searchObj.EndDate.Value.Date).ToList();
                }
                else
                {
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.ToList();
                }



                foreach (var item in lstSupplierExecludeAssets)
                {

                    IndexSupplierExecludeAssetVM.GetData getDataObj = new IndexSupplierExecludeAssetVM.GetData();
                    getDataObj.Id = item.Id;
                    getDataObj.AssetId = item.AssetId;
                    getDataObj.HospitalId = item.HospitalId;
                    getDataObj.ExNumber = item.ExNumber;
                    getDataObj.Date = item.Date.Value.ToString();
                    getDataObj.AppTypeId = item.AppTypeId;
                    getDataObj.UserName = item.User.UserName;
                    getDataObj.AssetId = item.AssetDetail.Id;
                    getDataObj.AssetName = item.AssetDetail.MasterAsset.Name;
                    getDataObj.AssetNameAr = item.AssetDetail.MasterAsset.NameAr;
                    getDataObj.TypeName = item.ApplicationType.Name;
                    getDataObj.TypeNameAr = item.ApplicationType.NameAr;

                    if (item.AssetDetail.MasterAsset.brand != null)
                    {
                        getDataObj.BrandName = item.AssetDetail.MasterAsset.brand.Name;
                        getDataObj.BrandNameAr = item.AssetDetail.MasterAsset.brand.NameAr;
                    }

                    getDataObj.FixCost = item.AssetDetail.FixCost;
                    getDataObj.CostPerDay = item.AssetDetail.FixCost != null ? Math.Round((((decimal)item.AssetDetail.FixCost) / 365)) : 0;
                    if (searchObj.EndDate != null)
                    {
                        getDataObj.AllDays = Math.Round((DateTime.Parse(searchObj.EndDate.ToString()) - DateTime.Parse(item.Date.ToString()).Date).TotalDays);
                    }
                    else
                    {
                        getDataObj.AllDays = 0;
                    }
                    if (getDataObj.CostPerDay != 0 && getDataObj.AllDays != 0)
                        getDataObj.TotalCost = Math.Round((decimal)getDataObj.CostPerDay * (decimal)getDataObj.AllDays);
                    else
                        getDataObj.TotalCost = 0;

                    getDataObj.SerialNumber = item.AssetDetail.SerialNumber;
                    getDataObj.BarCode = item.AssetDetail.Barcode;
                    getDataObj.ModelNumber = item.AssetDetail.MasterAsset.ModelNumber;
                    getDataObj.DiffMonths = ((item.Date.Value.Year - DateTime.Today.Date.Year) * 12) + item.Date.Value.Month - DateTime.Today.Date.Month;
                    getDataObj.IsMoreThan3Months = getDataObj.DiffMonths <= -3 ? true : false;
                    getDataObj.StatusId = item.StatusId;
                    getDataObj.StatusName = item.HospitalSupplierStatus.Name;
                    getDataObj.StatusNameAr = item.HospitalSupplierStatus.NameAr;
                    getDataObj.StatusColor = item.HospitalSupplierStatus.Color;
                    getDataObj.StatusIcon = item.HospitalSupplierStatus.Icon;


                    //if (searchObj.AppTypeId == 1)
                    //{
                    //    var ReasonExTitles = (from execlude in _context.SupplierExecludeReasons
                    //                          join trans in _context.SupplierExecludes on execlude.Id equals trans.ReasonId
                    //                          where trans.SupplierExecludeAssetId == item.Id
                    //                          && item.AppTypeId == 1
                    //                          select execlude).ToList();
                    //    if (ReasonExTitles.Count > 0)
                    //    {
                    //        List<string> execludeNames = new List<string>();// { "John", "Anna", "Monica" };
                    //        foreach (var reason in ReasonExTitles)
                    //        {
                    //            execludeNames.Add(reason.Name);
                    //        }
                    //        getDataObj.ReasonExTitles = string.Join(",", execludeNames);
                    //        List<string> execludeNamesAr = new List<string>();
                    //        foreach (var reason in ReasonExTitles)
                    //        {
                    //            execludeNamesAr.Add(reason.NameAr);
                    //        }
                    //        getDataObj.ReasonExTitlesAr = string.Join(",", execludeNamesAr);
                    //    }
                    //}
                    //if (searchObj.AppTypeId == 2)
                    //{
                    //    var ReasonHoldTitles = (from execlude in _context.SupplierExecludeReasons
                    //                            join trans in _context.SupplierExecludes on execlude.Id equals trans.ReasonId
                    //                            where trans.SupplierExecludeAssetId == item.Id
                    //                              && item.AppTypeId == 2
                    //                            select execlude).ToList();
                    //    if (ReasonHoldTitles.Count > 0)
                    //    {
                    //        List<string> holdNames = new List<string>();
                    //        foreach (var reason in ReasonHoldTitles)
                    //        {
                    //            holdNames.Add(reason.Name);
                    //        }
                    //        getDataObj.ReasonHoldTitles = string.Join(",", holdNames);
                    //        List<string> holdNamesAr = new List<string>();
                    //        foreach (var reason in ReasonHoldTitles)
                    //        {
                    //            holdNamesAr.Add(reason.NameAr);
                    //        }
                    //        getDataObj.ReasonHoldTitlesAr = string.Join(",", holdNamesAr);
                    //    }
                    //}

                    list.Add(getDataObj);
                }
                mainClass.Results = list;
                mainClass.Count = list.Count();
            }
            return mainClass;
        }

        public IEnumerable<IndexSupplierExecludeAssetVM.GetData> SearchSupplierExecludes(SearchSupplierExecludeAssetVM searchObj)
        {
            List<IndexSupplierExecludeAssetVM.GetData> list = new List<IndexSupplierExecludeAssetVM.GetData>();

            var lstSupplierExecludeAssets = _context.SupplierExecludeAssets.Include(a => a.User).Include(a => a.ApplicationType).Include(a => a.HospitalSupplierStatus)
            .Include(a => a.AssetDetail).Include(a => a.AssetDetail.Hospital).Include(a => a.AssetDetail.MasterAsset).Include(a => a.AssetDetail.MasterAsset.brand)
            .ToList();

            if (lstSupplierExecludeAssets.Count > 0)
            {
                if (searchObj.HospitalId != 0)
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.AssetDetail.HospitalId == searchObj.HospitalId).ToList();
                else
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.ToList();

                if (searchObj.Start != "")
                    searchObj.StartDate = DateTime.Parse(searchObj.Start);


                if (searchObj.End != "")
                    searchObj.EndDate = DateTime.Parse(searchObj.End);


                if (searchObj.StartDate != null && searchObj.EndDate != null)
                {
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.Date.Value.Date >= searchObj.StartDate.Value.Date && a.Date.Value.Date <= searchObj.EndDate.Value.Date).ToList();
                }
                else
                {
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.ToList();
                }


                if (searchObj.MasterAssetId != 0)
                {
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.AssetDetail.MasterAssetId == searchObj.MasterAssetId).ToList();
                }
                else
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.ToList();


                if (searchObj.DepartmentId != 0)
                {
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.AssetDetail.DepartmentId == searchObj.DepartmentId).ToList();
                }
                else
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.ToList();



                if (searchObj.StatusId != 0)
                {
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.StatusId == searchObj.StatusId).ToList();
                }
                else
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.ToList();



                if (searchObj.SerialNumber != "")
                {
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.AssetDetail.SerialNumber == searchObj.SerialNumber).ToList();
                }
                else
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.ToList();




                if (searchObj.ModelNumber != "")
                {
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.AssetDetail.MasterAsset.ModelNumber.Contains(searchObj.ModelNumber)).ToList();
                }
                else
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.ToList();


                if (searchObj.BarCode != "")
                {
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.AssetDetail.Barcode == searchObj.BarCode).ToList();
                }
                else
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.ToList();

                foreach (var item in lstSupplierExecludeAssets)
                {

                    IndexSupplierExecludeAssetVM.GetData getDataObj = new IndexSupplierExecludeAssetVM.GetData();
                    getDataObj.Id = item.Id;
                    getDataObj.AssetId = item.AssetId;
                    getDataObj.HospitalId = item.HospitalId;
                    getDataObj.ExNumber = item.ExNumber;
                    getDataObj.Date = item.Date.Value.ToString();
                    getDataObj.DemandDate = item.Date.Value;


                    getDataObj.AppTypeId = item.AppTypeId;
                    getDataObj.UserName = item.User.UserName;
                    getDataObj.AssetId = item.AssetDetail.Id;
                    getDataObj.AssetName = item.AssetDetail.MasterAsset.Name;
                    getDataObj.AssetNameAr = item.AssetDetail.MasterAsset.NameAr;
                    getDataObj.TypeName = item.ApplicationType.Name;
                    getDataObj.TypeNameAr = item.ApplicationType.NameAr;
                    getDataObj.SerialNumber = item.AssetDetail.SerialNumber;
                    getDataObj.BarCode = item.AssetDetail.Barcode;
                    getDataObj.ModelNumber = item.AssetDetail.MasterAsset.ModelNumber;

                    if (item.AssetDetail.MasterAsset.brand != null)
                    {
                        getDataObj.BrandName = item.AssetDetail.MasterAsset.brand.Name;
                        getDataObj.BrandNameAr = item.AssetDetail.MasterAsset.brand.NameAr;
                    }

                    //getDataObj.FixCost = item.AssetDetail.FixCost;
                    //getDataObj.CostPerDay = item.AssetDetail.FixCost != null ? Math.Round((((decimal)item.AssetDetail.FixCost) / 365)) : 0;
                    //if (searchObj.EndDate != null)
                    //{
                    //    getDataObj.AllDays = Math.Round((DateTime.Parse(searchObj.EndDate.ToString()) - DateTime.Parse(item.Date.ToString()).Date).TotalDays);
                    //}
                    //else
                    //{
                    //    getDataObj.AllDays = 0;
                    //}
                    //if (getDataObj.CostPerDay != 0 && getDataObj.AllDays != 0)
                    //    getDataObj.TotalCost = Math.Round((decimal)getDataObj.CostPerDay * (decimal)getDataObj.AllDays);
                    //else
                    //    getDataObj.TotalCost = 0;

                    //getDataObj.DiffMonths = ((item.Date.Value.Year - DateTime.Today.Date.Year) * 12) + item.Date.Value.Month - DateTime.Today.Date.Month;
                    //getDataObj.IsMoreThan3Months = getDataObj.DiffMonths <= -3 ? true : false;

                    getDataObj.StatusId = item.StatusId;
                    getDataObj.StatusName = item.HospitalSupplierStatus.Name;
                    getDataObj.StatusNameAr = item.HospitalSupplierStatus.NameAr;
                    getDataObj.StatusColor = item.HospitalSupplierStatus.Color;
                    getDataObj.StatusIcon = item.HospitalSupplierStatus.Icon;


                    if (item.AppTypeId == 1)
                    {

                        var ReasonExTitles = (from execlude in _context.SupplierExecludeReasons
                                              join trans in _context.SupplierExecludes on execlude.Id equals trans.ReasonId
                                              where trans.SupplierExecludeAssetId == item.Id
                                              && item.AppTypeId == 1
                                              select execlude).ToList();
                        if (ReasonExTitles.Count > 0)
                        {
                            List<string> execludeNames = new List<string>();
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
                    if (item.AppTypeId == 2)
                    {
                        var ReasonHoldTitles = (from execlude in _context.SupplierExecludeReasons
                                                join trans in _context.SupplierExecludes on execlude.Id equals trans.ReasonId
                                                where trans.SupplierExecludeAssetId == item.Id
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

            }
            return list;
        }

        public IndexSupplierExecludeAssetVM SearchSupplierExecludes(SearchSupplierExecludeAssetVM searchObj, int pageNumber, int pageSize)
        {
            IndexSupplierExecludeAssetVM mainClass = new IndexSupplierExecludeAssetVM();
            List<IndexSupplierExecludeAssetVM.GetData> list = new List<IndexSupplierExecludeAssetVM.GetData>();

            var lstSupplierExecludeAssets = _context.SupplierExecludeAssets
                                                    .Include(a => a.User)
                                                    .Include(a => a.ApplicationType)
                                                    .Include(a => a.HospitalSupplierStatus)
                                                    .Include(a => a.AssetDetail)
                                                    .Include(a => a.AssetDetail.Hospital)
                                                    .Include(a => a.AssetDetail.MasterAsset)
                                                    .Include(a => a.AssetDetail.MasterAsset.brand)
            .ToList();

            if (lstSupplierExecludeAssets.Count > 0)
            {
                if (searchObj.HospitalId != 0)
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.AssetDetail.HospitalId == searchObj.HospitalId).ToList();
                else
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.ToList();

   
                if (searchObj.Start != "")
                    searchObj.StartDate = DateTime.Parse(searchObj.Start);


                if (searchObj.End != "")
                    searchObj.EndDate = DateTime.Parse(searchObj.End);




                if (searchObj.StartDate != null && searchObj.EndDate != null)
                {
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.Date.Value.Date >= searchObj.StartDate.Value.Date && a.Date.Value.Date <= searchObj.EndDate.Value.Date).ToList();
                }
                else
                {
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.ToList();
                }


                if (searchObj.MasterAssetId != 0)
                {
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.AssetDetail.MasterAssetId == searchObj.MasterAssetId).ToList();
                }
                else
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.ToList();


                if (searchObj.DepartmentId != 0)
                {
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.AssetDetail.DepartmentId == searchObj.DepartmentId).ToList();
                }
                else
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.ToList();





                if (searchObj.SerialNumber != "")
                {
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.AssetDetail.SerialNumber == searchObj.SerialNumber).ToList();
                }
                else
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.ToList();




                if (searchObj.ModelNumber != "")
                {
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.AssetDetail.MasterAsset.ModelNumber.Contains(searchObj.ModelNumber)).ToList();
                }
                else
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.ToList();


                if (searchObj.BarCode != "")
                {
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.AssetDetail.Barcode == searchObj.BarCode).ToList();
                }
                else
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.ToList();



                if (searchObj.StatusId != 0)
                {
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.StatusId == searchObj.StatusId).ToList();
                }
                else
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.ToList();




                foreach (var item in lstSupplierExecludeAssets)
                {

                    IndexSupplierExecludeAssetVM.GetData getDataObj = new IndexSupplierExecludeAssetVM.GetData();
                    getDataObj.Id = item.Id;
                    getDataObj.AssetId = item.AssetId;
                    getDataObj.HospitalId = item.HospitalId;
                    getDataObj.ExNumber = item.ExNumber;
                    getDataObj.Date = item.Date.Value.ToString();
                    getDataObj.DemandDate = item.Date.Value;
                    getDataObj.AppTypeId = item.AppTypeId;
                    getDataObj.UserName = item.User.UserName;
                    getDataObj.AssetId = item.AssetDetail.Id;
                    getDataObj.AssetName = item.AssetDetail.MasterAsset.Name;
                    getDataObj.AssetNameAr = item.AssetDetail.MasterAsset.NameAr;
                    getDataObj.TypeName = item.ApplicationType.Name;
                    getDataObj.TypeNameAr = item.ApplicationType.NameAr;
                    getDataObj.HospitalName = item.AssetDetail.Hospital != null ? item.AssetDetail.Hospital.Name : "";
                    getDataObj.HospitalNameAr = item.AssetDetail.Hospital != null ? item.AssetDetail.Hospital.NameAr : "";


                    if (item.AssetDetail.MasterAsset.brand != null)
                    {
                        getDataObj.BrandName = item.AssetDetail.MasterAsset.brand.Name;
                        getDataObj.BrandNameAr = item.AssetDetail.MasterAsset.brand.NameAr;
                    }

                    getDataObj.FixCost = item.AssetDetail.FixCost;
                    getDataObj.CostPerDay = item.AssetDetail.FixCost != null ? Math.Round((((decimal)item.AssetDetail.FixCost) / 365)) : 0;
                    if (searchObj.EndDate != null)
                    {
                        getDataObj.AllDays = Math.Round((DateTime.Parse(searchObj.EndDate.ToString()) - DateTime.Parse(item.Date.ToString()).Date).TotalDays);
                    }
                    else
                    {
                        getDataObj.AllDays = 0;
                    }


                    if (getDataObj.CostPerDay != 0 && getDataObj.AllDays != 0)
                        getDataObj.TotalCost = Math.Round((decimal)getDataObj.CostPerDay * (decimal)getDataObj.AllDays);
                    else
                        getDataObj.TotalCost = 0;



                    getDataObj.SerialNumber = item.AssetDetail.SerialNumber;
                    getDataObj.BarCode = item.AssetDetail.Barcode;
                    getDataObj.ModelNumber = item.AssetDetail.MasterAsset.ModelNumber;


                    getDataObj.DiffMonths = ((item.Date.Value.Year - DateTime.Today.Date.Year) * 12) + item.Date.Value.Month - DateTime.Today.Date.Month;
                    getDataObj.IsMoreThan3Months = getDataObj.DiffMonths <= -3 ? true : false;

                    getDataObj.StatusId = item.StatusId;
                    getDataObj.StatusName = item.HospitalSupplierStatus.Name;
                    getDataObj.StatusNameAr = item.HospitalSupplierStatus.NameAr;
                    getDataObj.StatusColor = item.HospitalSupplierStatus.Color;
                    getDataObj.StatusIcon = item.HospitalSupplierStatus.Icon;


                    if (item.AppTypeId == 1)
                    {

                        var ReasonExTitles = (from execlude in _context.SupplierExecludeReasons
                                              join trans in _context.SupplierExecludes on execlude.Id equals trans.ReasonId
                                              where trans.SupplierExecludeAssetId == item.Id
                                              && item.AppTypeId == 1
                                              select execlude).ToList();
                        if (ReasonExTitles.Count > 0)
                        {
                            List<string> execludeNames = new List<string>();
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
                    if (item.AppTypeId == 2)
                    {
                        var ReasonHoldTitles = (from execlude in _context.SupplierExecludeReasons
                                                join trans in _context.SupplierExecludes on execlude.Id equals trans.ReasonId
                                                where trans.SupplierExecludeAssetId == item.Id
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




                var requestsPerPage = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                mainClass.Results = requestsPerPage;
                mainClass.Count = list.Count();

            }
            return mainClass;
        }

        public IEnumerable<IndexSupplierExecludeAssetVM.GetData> PrintSearchSupplierExecludes(SearchSupplierExecludeAssetVM searchObj)
        {
            List<IndexSupplierExecludeAssetVM.GetData> list = new List<IndexSupplierExecludeAssetVM.GetData>();

            var lstSupplierExecludeAssets = _context.SupplierExecludeAssets.Include(a => a.User).Include(a => a.ApplicationType).Include(a => a.HospitalSupplierStatus)
            .Include(a => a.AssetDetail).Include(a => a.AssetDetail.Hospital).Include(a => a.AssetDetail.MasterAsset).Include(a => a.AssetDetail.MasterAsset.brand)
            .ToList();

            if (lstSupplierExecludeAssets.Count > 0)
            {
                if (searchObj.HospitalId != 0)
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.AssetDetail.HospitalId == searchObj.HospitalId).ToList();
                else
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.ToList();

                if (searchObj.Start != "")
                    searchObj.StartDate = DateTime.Parse(searchObj.Start);


                if (searchObj.End != "")
                    searchObj.EndDate = DateTime.Parse(searchObj.End);


                if (searchObj.StartDate != null && searchObj.EndDate != null)
                {
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.Date.Value.Date >= searchObj.StartDate.Value.Date && a.Date.Value.Date <= searchObj.EndDate.Value.Date).ToList();
                }
                else
                {
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.ToList();
                }


                if (searchObj.MasterAssetId != 0)
                {
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.AssetDetail.MasterAssetId == searchObj.MasterAssetId).ToList();
                }
                else
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.ToList();


                if (searchObj.DepartmentId != 0)
                {
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.AssetDetail.DepartmentId == searchObj.DepartmentId).ToList();
                }
                else
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.ToList();



                if (searchObj.StatusId != 0)
                {
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.StatusId == searchObj.StatusId).ToList();
                }
                else
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.ToList();



                if (searchObj.SerialNumber != "")
                {
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.AssetDetail.SerialNumber == searchObj.SerialNumber).ToList();
                }
                else
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.ToList();




                if (searchObj.ModelNumber != "")
                {
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.AssetDetail.MasterAsset.ModelNumber.Contains(searchObj.ModelNumber)).ToList();
                }
                else
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.ToList();


                if (searchObj.BarCode != "")
                {
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.Where(a => a.AssetDetail.Barcode == searchObj.BarCode).ToList();
                }
                else
                    lstSupplierExecludeAssets = lstSupplierExecludeAssets.ToList();

                foreach (var item in lstSupplierExecludeAssets)
                {

                    IndexSupplierExecludeAssetVM.GetData getDataObj = new IndexSupplierExecludeAssetVM.GetData();
                    getDataObj.Id = item.Id;
                    getDataObj.HospitalId = item.HospitalId;
                    getDataObj.ExNumber = item.ExNumber;
                    getDataObj.Date = item.Date.Value.ToString();
                    getDataObj.DemandDate = item.Date.Value;
                    getDataObj.AppTypeId = item.AppTypeId;
                    getDataObj.UserName = item.User.UserName;
                    getDataObj.AssetId = item.AssetDetail.Id;
                    getDataObj.AssetName = item.AssetDetail.MasterAsset.Name;
                    getDataObj.AssetNameAr = item.AssetDetail.MasterAsset.NameAr;
                    getDataObj.TypeName = item.ApplicationType.Name;
                    getDataObj.TypeNameAr = item.ApplicationType.NameAr;
                    getDataObj.SerialNumber = item.AssetDetail.SerialNumber;
                    getDataObj.BarCode = item.AssetDetail.Barcode;
                    getDataObj.ModelNumber = item.AssetDetail.MasterAsset.ModelNumber;
                    getDataObj.HospitalName = item.AssetDetail.Hospital != null ? item.AssetDetail.Hospital.Name : "";
                    getDataObj.HospitalNameAr = item.AssetDetail.Hospital != null ? item.AssetDetail.Hospital.NameAr : "";


                    if (item.AssetDetail.MasterAsset.brand != null)
                    {
                        getDataObj.BrandName = item.AssetDetail.MasterAsset.brand.Name;
                        getDataObj.BrandNameAr = item.AssetDetail.MasterAsset.brand.NameAr;
                    }

                    //getDataObj.FixCost = item.AssetDetail.FixCost;
                    //getDataObj.CostPerDay = item.AssetDetail.FixCost != null ? Math.Round((((decimal)item.AssetDetail.FixCost) / 365)) : 0;
                    //if (searchObj.EndDate != null)
                    //{
                    //    getDataObj.AllDays = Math.Round((DateTime.Parse(searchObj.EndDate.ToString()) - DateTime.Parse(item.Date.ToString()).Date).TotalDays);
                    //}
                    //else
                    //{
                    //    getDataObj.AllDays = 0;
                    //}
                    //if (getDataObj.CostPerDay != 0 && getDataObj.AllDays != 0)
                    //    getDataObj.TotalCost = Math.Round((decimal)getDataObj.CostPerDay * (decimal)getDataObj.AllDays);
                    //else
                    //    getDataObj.TotalCost = 0;

                    //getDataObj.DiffMonths = ((item.Date.Value.Year - DateTime.Today.Date.Year) * 12) + item.Date.Value.Month - DateTime.Today.Date.Month;
                    //getDataObj.IsMoreThan3Months = getDataObj.DiffMonths <= -3 ? true : false;

                    getDataObj.StatusId = item.StatusId;
                    getDataObj.StatusName = item.HospitalSupplierStatus.Name;
                    getDataObj.StatusNameAr = item.HospitalSupplierStatus.NameAr;
                    getDataObj.StatusColor = item.HospitalSupplierStatus.Color;
                    getDataObj.StatusIcon = item.HospitalSupplierStatus.Icon;


                    if (item.AppTypeId == 1)
                    {

                        var ReasonExTitles = (from execlude in _context.SupplierExecludeReasons
                                              join trans in _context.SupplierExecludes on execlude.Id equals trans.ReasonId
                                              where trans.SupplierExecludeAssetId == item.Id
                                              && item.AppTypeId == 1
                                              select execlude).ToList();
                        if (ReasonExTitles.Count > 0)
                        {
                            List<string> execludeNames = new List<string>();
                            foreach (var reason in ReasonExTitles)
                            {
                                execludeNames.Add(reason.Name);
                            }

                            getDataObj.ReasonExTitles = string.Join(",", execludeNames);


                            List<string> execludeNamesAr = new List<string>();
                            foreach (var reason in ReasonExTitles)
                            {
                                execludeNamesAr.Add(" - " + reason.NameAr);
                            }
                            getDataObj.ReasonExTitlesAr = string.Join("\n", execludeNamesAr);

                        }
                    }
                    if (item.AppTypeId == 2)
                    {
                        var ReasonHoldTitles = (from execlude in _context.SupplierExecludeReasons
                                                join trans in _context.SupplierExecludes on execlude.Id equals trans.ReasonId
                                                where trans.SupplierExecludeAssetId == item.Id
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
                                holdNamesAr.Add(" - " + reason.NameAr);
                            }
                            getDataObj.ReasonHoldTitlesAr = string.Join("\n", holdNamesAr);
                        }
                    }

                    list.Add(getDataObj);
                }

            }
            return list;
        }
    }
}