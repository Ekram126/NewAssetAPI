using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.HospitalApplicationVM;
using Asset.ViewModels.HospitalReasonTransactionVM;
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
    public class HospitalReasonTransactionRepositories : IHospitalReasonTransactionRepository
    {
        private ApplicationDbContext _context;

        public HospitalReasonTransactionRepositories(ApplicationDbContext context)
        {
            _context = context;
        }


        public HospitalReasonTransaction GetById(int id)
        {
            return _context.HospitalReasonTransactions.Find(id);
        }
        public IEnumerable<HospitalReasonTransaction> GetAll()
        {
            return _context.HospitalReasonTransactions.ToList();
        }


        public int Add(CreateHospitalReasonTransactionVM model)
        {
            HospitalReasonTransaction transactionObj = new HospitalReasonTransaction();
            try
            {
                if (model != null)
                {
                    transactionObj.HospitalApplicationId = model.HospitalApplicationId;
                    transactionObj.ReasonId = model.ReasonId;
                    transactionObj.HospitalId = model.HospitalId;
                    _context.HospitalReasonTransactions.Add(transactionObj);
                    _context.SaveChanges();
                    int transId = transactionObj.Id;
                }
            }
            catch (Exception ex)
            {
              string str = ex.Message;
            }
            return transactionObj.Id;
        }

        public int Delete(int id)
        {
            var hospitalApplicationObj = _context.HospitalReasonTransactions.Find(id);
            try
            {
                if (hospitalApplicationObj != null)
                {

                    _context.HospitalReasonTransactions.Remove(hospitalApplicationObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return 0;
        }

        public int Update(HospitalReasonTransaction model)
        {
            return 0;
        }

        public List<IndexHospitalReasonTransactionVM.GetData> GetAttachmentByHospitalApplicationId(int appId)
        {
            List<IndexHospitalReasonTransactionVM.GetData> list = new List<IndexHospitalReasonTransactionVM.GetData>();
            var hospitalApplicationObj = _context.HospitalApplications.Find(appId);

            var lstTransactions = _context.HospitalReasonTransactions.Where(a => a.HospitalApplicationId == hospitalApplicationObj.Id).OrderBy(a=>a.ReasonId).ToList();
            if (lstTransactions.Count > 0)
            {
                foreach (var item in lstTransactions)
                {
                    IndexHospitalReasonTransactionVM.GetData getDataObj = new IndexHospitalReasonTransactionVM.GetData();
                    var lstAttachments = _context.HospitalApplicationAttachments.Where(a => a.HospitalReasonTransactionId == item.Id).OrderBy(a => a.FileName).ToList();

                    if (hospitalApplicationObj.AppTypeId == 1)
                    {
                        if (_context.HospitalExecludeReasons.Where(a => a.Id == item.ReasonId).OrderBy(a => a.Id).ToList().Count > 0)
                        {
                            getDataObj.ReasonId = _context.HospitalExecludeReasons.Where(a => a.Id == item.ReasonId).FirstOrDefault().Id;
                            getDataObj.ReasonName = _context.HospitalExecludeReasons.Where(a => a.Id == item.ReasonId).FirstOrDefault().Name;
                            getDataObj.ReasonNameAr = _context.HospitalExecludeReasons.Where(a => a.Id == item.ReasonId).FirstOrDefault().NameAr;
                            getDataObj.Attachments = lstAttachments;
                        }
                    }
                    else
                    {
                        if (_context.HospitalHoldReasons.Where(a => a.Id == item.ReasonId).OrderBy(a => a.Id).ToList().Count > 0)
                        {
                            getDataObj.ReasonId = _context.HospitalHoldReasons.Where(a => a.Id == item.ReasonId).FirstOrDefault().Id;
                            getDataObj.ReasonName = _context.HospitalHoldReasons.Where(a => a.Id == item.ReasonId).FirstOrDefault().Name;
                            getDataObj.ReasonNameAr = _context.HospitalHoldReasons.Where(a => a.Id == item.ReasonId).FirstOrDefault().NameAr;
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