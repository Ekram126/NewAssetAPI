using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.AssetDetailVM;
using Asset.ViewModels.ContractVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Core.Repositories
{
    public class MasterContractRepositories : IMasterContractRepository
    {

        private ApplicationDbContext _context;

        public MasterContractRepositories(ApplicationDbContext context)
        {
            _context = context;
        }



        public IEnumerable<MasterContract> GetAll()
        {
            return _context.MasterContracts.ToList();
        }

        public MasterContract GetById(int id)
        {
            return _context.MasterContracts.Find(id);
        }

        public int Add(CreateMasterContractVM model)
        {
            MasterContract masterContractObj = new MasterContract();
            try
            {
                if (model != null)
                {
                    masterContractObj.ContractDate = model.ContractDate;
                    masterContractObj.From = model.From;
                    masterContractObj.To = model.To;
                    masterContractObj.Serial = model.Serial;
                    masterContractObj.Subject = model.Subject;
                    masterContractObj.Cost = model.Cost;

                    masterContractObj.SupplierId = model.SupplierId;
                    _context.MasterContracts.Add(masterContractObj);
                    _context.SaveChanges();
                    var masterId = masterContractObj.Id;

                    if (model.lstDetails.Count > 0)
                    {
                        foreach (var item in model.lstDetails)
                        {
                            ContractDetail detailObj = new ContractDetail();
                            detailObj.ContractDate = model.ContractDate;
                            detailObj.MasterContractId = masterId;
                            detailObj.AssetDetailId = item.AssetDetailId;
                            detailObj.ResponseTime = item.ResponseTime;
                            detailObj.HasSpareParts = item.HasSpareParts;
                            _context.ContractDetails.Add(detailObj);
                            _context.SaveChanges();
                        }
                    }
                    return masterContractObj.Id;




                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return 0;
        }
        public int Delete(int id)
        {
            var masterContractObj = _context.MasterContracts.Find(id);
            try
            {
                if (masterContractObj != null)
                {
                    var lstDetails = _context.ContractDetails.Where(a => a.MasterContractId == masterContractObj.Id).ToList();
                    foreach (var item in lstDetails)
                    {
                        _context.ContractDetails.Remove(item);
                        _context.SaveChanges();
                    }
                    _context.MasterContracts.Remove(masterContractObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return 0;
        }
        public int Update(MasterContract model)
        {
            try
            {
                var masterContractObj = _context.MasterContracts.Find(model.Id);
                masterContractObj.Id = model.Id;
                masterContractObj.ContractDate = model.ContractDate;
                masterContractObj.From = model.From;
                masterContractObj.To = model.To;
                masterContractObj.Subject = model.Subject;
                masterContractObj.Serial = model.Serial;
                masterContractObj.Cost = model.Cost;
                masterContractObj.SupplierId = model.SupplierId;

                _context.Entry(masterContractObj).State = EntityState.Modified;
                _context.SaveChanges();
                return masterContractObj.Id;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return 0;
        }

        public IEnumerable<IndexMasterContractVM.GetData> GetMasterContractsByHospitalId(int hospitalId)
        {
             List<IndexMasterContractVM.GetData> list = new List<IndexMasterContractVM.GetData>();

               var lstMasters = _context.ContractDetails
                            .Include(a => a.MasterContract)
                            .Include(a => a.AssetDetail)
                            .Include(a => a.MasterContract.Supplier).Select(item => new IndexMasterContractVM.GetData
                            {
                                Id = item.MasterContract.Id,
                                SupplierId = item.MasterContract.SupplierId,
                                HospitalId =item.AssetDetail.HospitalId ,
                                Subject = item.MasterContract.Subject,
                                Cost = item.MasterContract.Cost.ToString(),
                                ContractNumber = item.MasterContract.Serial,
                                ContractDate = item.MasterContract.ContractDate.Value,
                                StartDate = item.MasterContract.From.Value,
                                EndDate = item.MasterContract.To.Value,
                                SupplierName = item.MasterContract.Supplier.Name,
                                SupplierNameAr = item.MasterContract.Supplier.NameAr
                            }).ToList().GroupBy(a=>a.Id);


            foreach (var item in lstMasters)
            {
                IndexMasterContractVM.GetData getDataObj = new IndexMasterContractVM.GetData();
                getDataObj.Id = item.FirstOrDefault().Id;
                getDataObj.Subject = item.FirstOrDefault().Subject;
                getDataObj.Cost = item.FirstOrDefault().Cost;
                getDataObj.ContractNumber = item.FirstOrDefault().ContractNumber;
                getDataObj.ContractDate = item.FirstOrDefault().ContractDate;
                getDataObj.StartDate = item.FirstOrDefault().StartDate;
                getDataObj.EndDate = item.FirstOrDefault().EndDate;
                getDataObj.HospitalId = item.FirstOrDefault().HospitalId;
                getDataObj.SupplierName = item.FirstOrDefault().SupplierId != null ? item.FirstOrDefault().SupplierName:"";
                getDataObj.SupplierNameAr = item.FirstOrDefault().SupplierId != null ? item.FirstOrDefault().SupplierNameAr:"";
                list.Add(getDataObj);
            }

            if (hospitalId > 0)
            {
                list = list.Where(a => a.HospitalId == hospitalId).ToList();
            }
            else
            {
                list = list.ToList();
            }

            return list;

        }

        public IEnumerable<IndexMasterContractVM.GetData> Search(SearchContractVM searchObj)
        {


            string setstartcontractday, setstartcontractmonth, setendcontractday, setendcontractmonth = "";
            List<IndexMasterContractVM.GetData> lstData = new List<IndexMasterContractVM.GetData>();

            var list = _context.ContractDetails
                             .Include(a => a.MasterContract)
                             .Include(a => a.AssetDetail)
                             .Include(a => a.MasterContract.Supplier)
                             .Select(item => new
                             {
                                 Id = item.MasterContract.Id,
                                 Subject = item.MasterContract.Subject,
                                 Cost = item.MasterContract.Cost,
                                 ContractNumber = item.MasterContract.Serial,
                                 ContractDate = item.MasterContract.ContractDate,
                                 StartDate = item.MasterContract.From,
                                 EndDate = item.MasterContract.To,
                                 SupplierId = item.MasterContract.SupplierId,
                                 SupplierName = item.MasterContract.Supplier.Name,
                                 SupplierNameAr = item.MasterContract.Supplier.NameAr,
                                 HospitalId = item.AssetDetail.HospitalId
                             }).ToList().GroupBy(a => a.Id).ToList();



            foreach (var cntrct in list)
            {
                IndexMasterContractVM.GetData getDataObj = new IndexMasterContractVM.GetData();
                getDataObj.Id = cntrct.FirstOrDefault().Id;
                getDataObj.Subject = cntrct.FirstOrDefault().Subject;
                getDataObj.Cost = cntrct.FirstOrDefault().Cost.ToString();
                getDataObj.ContractNumber = cntrct.FirstOrDefault().ContractNumber;
                getDataObj.ContractDate = cntrct.FirstOrDefault().ContractDate.Value;
                getDataObj.StartDate = cntrct.FirstOrDefault().StartDate;
                getDataObj.EndDate = cntrct.FirstOrDefault().EndDate;
                getDataObj.SupplierName = cntrct.FirstOrDefault().SupplierName;
                getDataObj.SupplierNameAr = cntrct.FirstOrDefault().SupplierNameAr;
                lstData.Add(getDataObj);
            }


            if (searchObj.HospitalId != 0)
            {
                lstData = lstData.Where(b => b.HospitalId == searchObj.HospitalId).ToList();
            }
            if (searchObj.Subject != "")
            {
                lstData = lstData.Where(b => b.Subject == searchObj.Subject).ToList();
            }
            if (searchObj.ContractNumber != "")
            {
                lstData = lstData.Where(b => b.ContractNumber == searchObj.ContractNumber).ToList();
            }
            if (searchObj.StartDate.ToString() == "" || searchObj.StartDate == null)
            {
                searchObj.StartDate = DateTime.Parse("01/01/1900");
            }
            else
            {
                searchObj.StartDate = searchObj.StartDate;
            }

            if (searchObj.EndDate.ToString() == "" || searchObj.EndDate == null)
            {
                searchObj.EndDate = DateTime.Today.Date;
            }
            else
            {
                searchObj.EndDate = searchObj.EndDate;
            }

            var startyear = searchObj.StartDate.Value.Year;
            var startmonth = searchObj.StartDate.Value.Month;
            var startday = searchObj.StartDate.Value.Day;
            if (startday < 10)
                setstartcontractday = searchObj.StartDate.Value.Day.ToString().PadLeft(2, '0');
            else
                setstartcontractday = searchObj.StartDate.Value.Day.ToString();

            if (startmonth < 10)
                setstartcontractmonth = searchObj.StartDate.Value.Month.ToString().PadLeft(2, '0');
            else
                setstartcontractmonth = searchObj.StartDate.Value.Month.ToString();

            var sDate = startyear + "-" + setstartcontractmonth + "-" + setstartcontractday;
            var startingFrom = DateTime.Parse(sDate);



            var endyear = searchObj.EndDate.Value.Year;
            var endmonth = searchObj.EndDate.Value.Month;
            var endday = searchObj.EndDate.Value.Day;
            if (endday < 10)
                setendcontractday = searchObj.EndDate.Value.Day.ToString().PadLeft(2, '0');
            else
                setendcontractday = searchObj.EndDate.Value.Day.ToString();

            if (endmonth < 10)
                setendcontractmonth = searchObj.EndDate.Value.Month.ToString().PadLeft(2, '0');
            else
                setendcontractmonth = searchObj.EndDate.Value.Month.ToString();

            var eDate = endyear + "-" + setendcontractmonth + "-" + setendcontractday;
            var endingTo = DateTime.Parse(eDate);


            lstData = lstData.Where(a => a.StartDate >= startingFrom && a.EndDate <= endingTo).ToList();








            string setcontractday, setcontractmonth = "";
            var year = searchObj.ContractDate.Value.Year;
            var month = searchObj.ContractDate.Value.Month;
            var day = searchObj.ContractDate.Value.Day;
            if (day < 10)
                setcontractday = searchObj.ContractDate.Value.Day.ToString().PadLeft(2, '0');
            else
                setcontractday = searchObj.ContractDate.Value.Day.ToString();

            if (month < 10)
                setcontractmonth = searchObj.ContractDate.Value.Month.ToString().PadLeft(2, '0');
            else
                setcontractmonth = searchObj.ContractDate.Value.Month.ToString();

            var contrctDate = year + "-" + setcontractmonth + "-" + setcontractday;
            var conDate = DateTime.Parse(contrctDate);


            lstData = lstData.Where(a => a.ContractDate >= conDate && a.ContractDate <= conDate).ToList();



            return lstData;
        }

        public IEnumerable<IndexMasterContractVM.GetData> SortContracts(int hospitalId, SortContractsVM sortObj)
        {
            var list=GetMasterContractsByHospitalId(hospitalId);
            if (sortObj.ContractNumber != "")
            {
                if (sortObj.SortStatus == "descending")
                    list = list.OrderByDescending(d => d.ContractNumber).ToList();
                else
                    list = list.OrderBy(d => d.ContractNumber).ToList();
            }
            else if (sortObj.Subject != "")
            {
                if (sortObj.SortStatus == "descending")
                    list = list.OrderByDescending(d => d.Subject).ToList();
                else
                    list = list.OrderBy(d => d.Subject).ToList();
            }
            else if (sortObj.ContractDate != "")
            {
                if (sortObj.SortStatus == "descending")
                    list = list.OrderByDescending(d => d.ContractDate).ToList();
                else
                    list = list.OrderBy(d => d.ContractDate).ToList();
            }
            else if (sortObj.StartDate != "")
            {
                if (sortObj.SortStatus == "descending")
                    list = list.OrderByDescending(d => d.StartDate).ToList();
                else
                    list = list.OrderBy(d => d.StartDate).ToList();
            }
            else if (sortObj.EndDate != "")
            {
                if (sortObj.SortStatus == "descending")
                    list = list.OrderByDescending(d => d.EndDate).ToList();
                else
                    list = list.OrderBy(d => d.EndDate).ToList();
            }
            return list;
        }

        public int CreateContractAttachments(ContractAttachment attachObj)
        {
            ContractAttachment documentObj = new ContractAttachment();
            documentObj.DocumentName = attachObj.DocumentName;
            documentObj.FileName = attachObj.FileName;
            documentObj.MasterContractId = attachObj.MasterContractId;
            documentObj.MasterContractId = attachObj.MasterContractId;
            _context.ContractAttachments.Add(documentObj);
            _context.SaveChanges();
            return attachObj.Id;
        }

        public GeneratedMasterContractNumberVM GenerateMasterContractSerial()
        {
            GeneratedMasterContractNumberVM numberObj = new GeneratedMasterContractNumberVM();
            string strContract = "Cntrct";

            var lstContracts = _context.MasterContracts.ToList();
            if (lstContracts.Count > 0)
            {
                var code = lstContracts.LastOrDefault().Id;
                numberObj.ContractSerial = strContract + (code + 1);
            }
            else
            {
                numberObj.ContractSerial = strContract + 1;
            }

            return numberObj;
        }

        public IEnumerable<ContractAttachment> GetContractAttachmentByMasterContractId(int masterContractId)
        {
         return   _context.ContractAttachments.Where(a => a.MasterContractId == masterContractId).ToList();
        }
    }
}
