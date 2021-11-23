using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.AssetDetailVM;
using Asset.ViewModels.ContractVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
            if (hospitalId == 0)
            {
                var lstMasters = (from master in _context.MasterContracts
                                  join detail in _context.ContractDetails on master.Id equals detail.MasterContractId
                                  join assetDetail in _context.AssetDetails on detail.AssetDetailId equals assetDetail.Id
                                  //   where assetDetail.HospitalId == hospitalId
                                  select new IndexMasterContractVM.GetData
                                  {
                                      Id = master.Id,
                                      SupplierId = master.SupplierId,
                                      HospitalId = assetDetail.HospitalId != 0 ? assetDetail.HospitalId : 0,
                                      Subject = master.Subject,
                                      Cost = master.Cost.ToString(),
                                      ContractNumber = master.Serial,
                                      ContractDate = master.ContractDate.Value.ToShortDateString(),
                                      StartDate = master.From.Value.ToShortDateString(),
                                      EndDate = master.To.Value.ToShortDateString()
                                  }).ToList().GroupBy(a => new { a.ContractDate, a.Id }).ToList();

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

                    if (item.FirstOrDefault().SupplierId != null || item.FirstOrDefault().SupplierId != 0)
                    {
                        var lstSuppliers = _context.Suppliers.Where(a => a.Id == item.FirstOrDefault().SupplierId).ToList();
                        if (lstSuppliers.Count > 0)
                        {
                            var supplierObj = lstSuppliers[0];
                            getDataObj.SupplierName = supplierObj.Name;
                            getDataObj.SupplierNameAr = supplierObj.NameAr;
                        }
                    }
                    list.Add(getDataObj);
                }


                return list;
            }
            else
            {


                var lstMasters = (from master in _context.MasterContracts
                                  join detail in _context.ContractDetails on master.Id equals detail.MasterContractId
                                  join assetDetail in _context.AssetDetails on detail.AssetDetailId equals assetDetail.Id
                                  where assetDetail.HospitalId == hospitalId
                                  select new IndexMasterContractVM.GetData
                                  {
                                      Id = master.Id,
                                      SupplierId = master.SupplierId,
                                      HospitalId = assetDetail.HospitalId != 0 ? assetDetail.HospitalId : 0,
                                      Subject = master.Subject,
                                      Cost = master.Cost.ToString(),
                                      ContractNumber = master.Serial,
                                      ContractDate = master.ContractDate.Value.ToShortDateString(),
                                      StartDate = master.From.Value.ToShortDateString(),
                                      EndDate = master.To.Value.ToShortDateString()
                                  }).ToList().GroupBy(a => new { a.ContractDate, a.Id }).ToList();

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
                    if (item.FirstOrDefault().SupplierId != null || item.FirstOrDefault().SupplierId != 0)
                    {
                        var lstSuppliers = _context.Suppliers.Where(a => a.Id == item.FirstOrDefault().SupplierId).ToList();
                        if (lstSuppliers.Count > 0)
                        {
                            var supplierObj = lstSuppliers[0];
                            getDataObj.SupplierName = supplierObj.Name;
                            getDataObj.SupplierNameAr = supplierObj.NameAr;
                        }
                    }
                    list.Add(getDataObj);
                }


                return list;
            }
            // return list;
        }

        public IEnumerable<IndexMasterContractVM.GetData> Search(SearchContractVM searchObj)
        {
            List<IndexMasterContractVM.GetData> lstData = new List<IndexMasterContractVM.GetData>();

            var list = (from master in _context.MasterContracts
                        join detail in _context.ContractDetails on master.Id equals detail.MasterContractId
                        join assetDetail in _context.AssetDetails on detail.AssetDetailId equals assetDetail.Id
                        where assetDetail.HospitalId == searchObj.HospitalId
                        select new
                        {
                            Id = master.Id,
                            Subject = master.Subject,
                            Cost = master.Cost,
                            ContractNumber = master.Serial,
                            ContractDate = master.ContractDate,
                            StartDate = master.From,
                            EndDate = master.To,
                            SupplierId = master.SupplierId
                        }).ToList();


            if (searchObj.Subject != "")
            {
                list = list.Where(b => b.Subject.Contains(searchObj.Subject)).ToList();
            }
            if (searchObj.ContractNumber != "")
            {
                list = list.Where(b => b.ContractNumber == searchObj.ContractNumber).ToList();
            }

            string setcontractday, setcontractmonth = "";

            var year = searchObj.StartDate.Value.Year;
            var month = searchObj.StartDate.Value.Month;
            var day = searchObj.StartDate.Value.Day;
            if (day < 10)
                setcontractday = searchObj.ContractDate.Value.Day.ToString().PadLeft(2, '0');
            else
                setcontractday = searchObj.ContractDate.Value.Day.ToString();

            if (month < 10)
                setcontractmonth = searchObj.ContractDate.Value.Month.ToString().PadLeft(2, '0');
            else
                setcontractmonth = searchObj.ContractDate.Value.Month.ToString();

            var contrctDate = year + "/" + setcontractmonth + "/" + setcontractday;
            var conDate = DateTime.Parse(contrctDate);

            if (searchObj.ContractDate != null)
            {
                list = list.Where(a => a.ContractDate >= conDate && a.ContractDate <= conDate).ToList();

            }
            else
            {
                list = list.ToList();
            }



            string setstartcontractday, setstartcontractmonth, setendcontractday, setendcontractmonth = "";


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
            if (day < 10)
                setstartcontractday = searchObj.StartDate.Value.Day.ToString().PadLeft(2, '0');
            else
                setstartcontractday = searchObj.StartDate.Value.Day.ToString();

            if (month < 10)
                setstartcontractmonth = searchObj.StartDate.Value.Month.ToString().PadLeft(2, '0');
            else
                setstartcontractmonth = searchObj.StartDate.Value.Month.ToString();

            var sDate = startyear + "/" + setstartcontractmonth + "/" + setstartcontractday;
            var startingFrom = DateTime.Parse(sDate);





            var endyear = searchObj.EndDate.Value.Year;
            var endmonth = searchObj.EndDate.Value.Month;
            var endday = searchObj.EndDate.Value.Day;
            if (day < 10)
                setendcontractday = searchObj.EndDate.Value.Day.ToString().PadLeft(2, '0');
            else
                setendcontractday = searchObj.EndDate.Value.Day.ToString();

            if (month < 10)
                setendcontractmonth = searchObj.EndDate.Value.Month.ToString().PadLeft(2, '0');
            else
                setendcontractmonth = searchObj.EndDate.Value.Month.ToString();

            var eDate = endyear + "/" + setendcontractmonth + "/" + setendcontractday;
            var endingTo = DateTime.Parse(eDate);


            //if (startingFrom == null || endingTo == null)
            //{
            //    list = list.ToList();
            //}
            //else
            //{
            list = list.Where(a => a.StartDate >= startingFrom && a.EndDate <= endingTo).ToList();
            //   }


            //list = (from lst in list
            //        join detail in _context.ContractDetails on lst.Id equals detail.MasterContractId
            //        join assetDetail in _context.AssetDetails on detail.AssetDetailId equals assetDetail.Id
            //        where assetDetail.HospitalId == searchObj.HospitalId
            //        select lst).ToList();

            foreach (var item in list)
            {
                IndexMasterContractVM.GetData getDataObj = new IndexMasterContractVM.GetData();
                getDataObj.Id = item.Id;
                getDataObj.Subject = item.Subject;
                getDataObj.Cost = item.Cost.ToString();
                getDataObj.ContractNumber = item.ContractNumber;
                getDataObj.ContractDate = item.ContractDate.Value.ToShortDateString();
                getDataObj.StartDate = item.StartDate.Value.ToShortDateString();
                getDataObj.EndDate = item.EndDate.Value.ToShortDateString();
                getDataObj.SupplierName = _context.Suppliers.Where(a => a.Id == item.SupplierId).ToList().First().Name;
                getDataObj.SupplierNameAr = _context.Suppliers.Where(a => a.Id == item.SupplierId).ToList().First().NameAr;
                lstData.Add(getDataObj);
            }
            return lstData;
        }
    }
}
