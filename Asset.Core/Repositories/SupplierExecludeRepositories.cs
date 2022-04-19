using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.SupplierExecludeAssetVM;
using Asset.ViewModels.RoleCategoryVM;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Asset.ViewModels.SupplierExecludeVM;

namespace Asset.Core.Repositories
{
    public class SupplierExecludeRepositories : ISupplierExecludeRepository
    {
        private ApplicationDbContext _context;

        public SupplierExecludeRepositories(ApplicationDbContext context)
        {
            _context = context;
        }

        public SupplierExeclude GetById(int id)
        {

            return _context.SupplierExecludes.Find(id);
        }

        public IEnumerable<SupplierExeclude> GetAll()
        {

            return _context.SupplierExecludes.ToList();
        }

        public int Add(SupplierExeclude model)
        {
            SupplierExeclude supplierExecludeObj = new SupplierExeclude();
            try
            {
                if (supplierExecludeObj != null)
                {
                    supplierExecludeObj.Id = model.Id;
                    supplierExecludeObj.SupplierExecludeAssetId = model.SupplierExecludeAssetId;
                    supplierExecludeObj.ReasonId = model.ReasonId;
                    supplierExecludeObj.HospitalId = model.HospitalId;
                    _context.SupplierExecludes.Add(supplierExecludeObj);
                    _context.SaveChanges();
                    int id = supplierExecludeObj.Id;
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return supplierExecludeObj.Id;
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

        public int Update(SupplierExeclude model)
        {
            try
            {
                var supplierExecludeAssetObj = _context.SupplierExecludes.Find(model.Id);
                supplierExecludeAssetObj.Id = model.Id;
                supplierExecludeAssetObj.SupplierExecludeAssetId = model.SupplierExecludeAssetId;
                supplierExecludeAssetObj.ReasonId = model.ReasonId;
                supplierExecludeAssetObj.HospitalId = model.HospitalId;
                _context.Entry(supplierExecludeAssetObj).State = EntityState.Modified;
                _context.SaveChanges();

                return supplierExecludeAssetObj.Id;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return 0;
        }

        public IEnumerable<SupplierExeclude> GetSupplierExecludesBySupplierExecludeAssetId(int supplierExecludeAssetId)
        {
            throw new NotImplementedException();
        }

        public List<IndexSupplierExecludeVM.GetData> GetAttachmentBySupplierExecludeAssetId(int supplierExecludeAssetId)
        {
            List<IndexSupplierExecludeVM.GetData> list = new List<IndexSupplierExecludeVM.GetData>();
            var supplierExecludeAssetObj = _context.SupplierExecludeAssets.Find(supplierExecludeAssetId);

            var lstTransactions = _context.SupplierExecludes.Where(a => a.SupplierExecludeAssetId == supplierExecludeAssetObj.Id).OrderBy(a => a.ReasonId).ToList();
            if (lstTransactions.Count > 0)
            {

                foreach (var item in lstTransactions)
                {
                    IndexSupplierExecludeVM.GetData getDataObj = new IndexSupplierExecludeVM.GetData();
                    var lstAttachments = _context.SupplierExecludeAttachments.Where(a => a.SupplierExecludeId == item.Id).OrderBy(a => a.FileName).ToList();

                    if (supplierExecludeAssetObj.AppTypeId == 1)
                    {
                        if (_context.HospitalExecludeReasons.Where(a => a.Id == item.ReasonId).OrderBy(a => a.Id).ToList().Count > 0)
                        {
                            getDataObj.ReasonId = _context.SupplierExecludeReasons.Where(a => a.Id == item.ReasonId).FirstOrDefault().Id;
                            getDataObj.ReasonName = _context.SupplierExecludeReasons.Where(a => a.Id == item.ReasonId).FirstOrDefault().Name;
                            getDataObj.ReasonNameAr = _context.SupplierExecludeReasons.Where(a => a.Id == item.ReasonId).FirstOrDefault().NameAr;
                            getDataObj.Attachments = lstAttachments;
                        }
                    }
                    else
                    {
                        if (_context.HospitalHoldReasons.Where(a => a.Id == item.ReasonId).OrderBy(a => a.Id).ToList().Count > 0)
                        {
                            getDataObj.ReasonId = _context.SupplierHoldReasons.Where(a => a.Id == item.ReasonId).FirstOrDefault().Id;
                            getDataObj.ReasonName = _context.SupplierHoldReasons.Where(a => a.Id == item.ReasonId).FirstOrDefault().Name;
                            getDataObj.ReasonNameAr = _context.SupplierHoldReasons.Where(a => a.Id == item.ReasonId).FirstOrDefault().NameAr;
                            getDataObj.Attachments = lstAttachments;
                        }
                    }
                    list.Add(getDataObj);
                }
            }
            return list;
        }
    }
}